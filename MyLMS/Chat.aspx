﻿<!DOCTYPE html><html class=''>
<html lang="en">
<head> 
	<meta charset="UTF-8">
	<title>Center</title>
	<link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet">
	<script src="https://use.fontawesome.com/1c6f725ec5.js"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
	<link rel="stylesheet" href="chat.css">
	<script>
	var messagesPanel, typedMessage, first = true, ws;
	
	const TESTINGURI = "ws://localhost:4000";
	const PRODURI = "ws://ec2-18-218-225-8.us-east-2.compute.amazonaws.com:4000";
	var statusBar;

	var urlParams = parseURLParams(window.location.href);
	console.log(urlParams);
	var SESSION;
	var NAME;

	if ("SessionID" in urlParams) {
		SESSION = urlParams["SessionID"][0];
	} else {
		alert("Exception: Unable to read Session from URL");
	}
	console.log(SESSION);

	if ("CenterName" in urlParams) {
		NAME = urlParams["CenterName"][0];
	} else {
		NAME = "Center";
		console.log("Exception: Unable to read Center name from URL");
	}
	console.log(NAME);


	const GENERAL = "general";
	const PRIVATE = "private";
	var currentId = GENERAL;
	const UNREAD = "unread";

	var messages = {};
	var studio = {};
	var currentStudentId;

	function main() {
		messagesPanel = document.getElementById("messagePanel");
		typedMessage = document.getElementById("typedMessage");
		statusBar = document.getElementById("statusBar");
		var list = document.getElementById("contacts");



		var contact = document.createElement("div");
				contact.className = "new-message-contact active-contact";
				var text = document.createElement("div");
				contact.id = GENERAL;  // general chat 
				contact.onclick = displayMessages;
				text.className = "contact-text";
				text.textContent = "Studio - General";
				var newMessage = document.createElement("div");
				newMessage.id = "nm";
				contact.appendChild(newMessage);
				contact.appendChild(text);
				list.appendChild(contact);	
				studio[GENERAL] = {"unread": 0};			



		var contact2 = document.createElement("div");
				contact2.className = "new-message-contact";
				var text2 = document.createElement("div");
				contact2.onclick = displayMessages;
				contact2.id = PRIVATE; // private chat
				text2.className = "contact-text";
				text2.textContent = "Studio - Private";
				newMessage = document.createElement("div");
				newMessage.id = "nm";
				contact2.appendChild(newMessage);
				contact2.appendChild(text2);
				list.appendChild(contact2);
				studio[PRIVATE] = {"unread": 0};

		var dividor = document.createElement("div");
	
			dividor.className = "hline";
			dividor.id = "dividor";
			list.appendChild(dividor);

		addStudents();

		messages[GENERAL] = [];
		messages[PRIVATE] = [];
		
		ws = new WebSocket(TESTINGURI);

		ws.onerror = function(e){
			alert("Could not connect to server");
		}

		ws.onopen = function(e) {
			ws.send(JSON.stringify({
				session: SESSION,
				type: "centerAdd",
				name: NAME
			}));
			statusBar.textContent = "Connected!";
		}

		ws.onmessage = function ws_message(input){
			var data = input.data;
			if (isJson(data)){
				console.log("Received message!" + data);
				var obj = JSON.parse(data);
				var messageWrapper = {'type': 'you', 'data': obj.data};
				if (obj.type == "singleMessage"){
					if(currentId == PRIVATE)
						addMessage(obj.data, true);
					else
						addUnread(PRIVATE);
					messages[PRIVATE].push(messageWrapper);
				}
				else{
					if(currentId == GENERAL)
						addMessage(obj.data, true);
					else
						addUnread(GENERAL);

					messages[GENERAL].push(messageWrapper);
				}

			}
			else{
				console.log("Data received is not JSON");
			}
		};

		ws.onclose = function() {
			statusBar.textContent = 'Connection lost';
			typedMessage.disabled = true;
		};
		typedMessage.onkeydown = function(e) {
			if (e.keyCode === 13 && !e.shiftKey) {
				if (typedMessage.value.length) {
					typedMessage.value = typedMessage.value.substring(0, 512);
					var messageWrapper = {'type': 'me', 'data': typedMessage.value};
					if(currentId == GENERAL){
						if(currentStudentId){
							ws.send(JSON.stringify({
								type: "groupMessage",
								group: "center",
								session: SESSION,
								data: typedMessage.value,
								student: currentStudentId
							}));
							addMessage(typedMessage.value, false);
							messages[GENERAL].push(messageWrapper);
						}
						else{
							alert("Please choose a student.");
						}
					}
					else{
						ws.send(JSON.stringify({
							type: "singleMessage",
							group: "center",
							session: SESSION,
							data: typedMessage.value
						}));
						addMessage(typedMessage.value, false);
						messages[PRIVATE].push(messageWrapper);
					}
					
				}
				typedMessage.value = "";
				return false;
			}
			return true;
		};

		typedMessage.onclick = function() {
			console.log("Clicked typed message");
			if (first) {
				typedMessage.value = "";
				first = false;
			}
		}

		if (localStorage.id) {
			id = localStorage.id;
		} else {
			id = Math.floor(Math.random() * 1000000);
			localStorage.id = id;
		}
	}

	function parseURLParams(url) {
		var queryStart = url.indexOf("?") + 1,
				queryEnd   = url.indexOf("#") + 1 || url.length + 1,
				query = url.slice(queryStart, queryEnd - 1),
				pairs = query.replace(/\+/g, " ").split("&"),
				parms = {}, i, n, v, nv;

		if (query === url || query === "") return;

		for (i = 0; i < pairs.length; i++) {
				nv = pairs[i].split("=", 2);
				n = decodeURIComponent(nv[0]);
				v = decodeURIComponent(nv[1]);

				if (!parms.hasOwnProperty(n)) parms[n] = [];
				parms[n].push(nv.length === 2 ? v : null);
		}
		return parms;
	}
	
	function isJson(str) {
	try {
		JSON.parse(str);
	} catch (e) {
		console.log(e);
		return false;
	}
	return true;
	}

	function addStudents(){
		list = document.getElementById("contacts");
		var hardcoded = [
			["Bob", "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_01.jpg"],
			["Ross", "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_02.jpg"],
			["Max", "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_03.jpg"],
			["Claire", "https://s3-us-west-2.amazonaws.com/s.cdpn.io/195612/chat_avatar_04.jpg"]
		];

		for(i=0;i<hardcoded.length;i++){
			var student = document.createElement("div");
			student.className = "contact";
			student.id = hardcoded[i][0];
			var image_placeholder = document.createElement("img");
			image_placeholder.src = hardcoded[i][1];
			image_placeholder.id = "image";

			var text = document.createElement("span");
			text.className = "contact-text2";
			text.id = "text";
			text.textContent = hardcoded[i][0];
			
			student.appendChild(image_placeholder);
			student.appendChild(text);

			student.onclick = setCurrentStudentId;

			list.appendChild(student);					
		}
	}

	function addUnread(id){
			var unread = studio[id][UNREAD];
			unread = unread+1;
			studio[id][UNREAD] = unread;
			var group = document.getElementById(id);
			var m = group.childNodes.item("nm");
			m.className = "new-message";
			m.textContent = unread;
		}

	function setCurrentStudentId(evt){
		var name = evt.target;
		var id = this.id;

		if (currentId != PRIVATE){
			if(!currentStudentId){
				currentStudentId = id;
			}
			else if(currentStudentId != id){
				document.getElementById(currentStudentId).className = "contact";
				currentStudentId = id;
				console.log("getting here");
			}
			document.getElementById(currentStudentId).className = "active-student";
		}
	}
	function displayMessages(evt){
			var name = evt.target;
			console.log("reaching here");
			var id = this.id // get the contact id
			var c = currentId;
			// display on{ly the private chat messages for that id
			if (c != id){
				if(id==PRIVATE){
					if(currentStudentId){
						document.getElementById(currentStudentId).className = "contact";
						currentStudentId = null;
					}
				}
				document.getElementById(id).className ="new-message-contact active-contact";
				document.getElementById(currentId).className = "new-message-contact"; 
				currentId = id;
				console.log("Current id: " + currentId);
				typedMessage.value = "";
				messagesPanel.innerHTML = "";
				var messageList = messages[id];                    
			}
		
			var g = document.getElementById(currentId);
			var s = g.childNodes.item("nm");
			s.className = "";
			s.textContent = "";
			studio[currentId][UNREAD] = 0;

			// display appropriate messages
			messageList.forEach(function(mess){
					if(mess.type == "you"){
						addMessage(mess.data, true);
					}
					else
						addMessage(mess.data, false);
				});
		}



	function addMessage(message, left) {
		var flexBox = document.createElement('div');if (!left)
		flexBox.className = "chat-bubble me";
		else
		flexBox.className = "chat-bubble you";
	//  flexBox.style = 'display: flex; ' + (left ? '' : 'justify-content: flex-end');
		var time = new Date().toLocaleTimeString();
		var messageBox = document.createElement('p');
		messageBox.className = "content";
		var timeShow = document.createElement('p');
		timeShow.className = "time";
		timeShow.textContent = time; 
		messageBox.textContent = message;
		flexBox.appendChild(messageBox);
		flexBox.appendChild(timeShow);
		setTimeout(function() {
			messageBox.className = "show";
		}, 10);

		var wereBottomScrolled = true;//messagesPanel.scrollTop == messagesPanel.scrollHeight;
		messagesPanel.appendChild(flexBox);
		if (wereBottomScrolled) {
			messagesPanel.scrollTop = messagesPanel.scrollHeight;
		}
		typedMessage.value = "";
	}
	</script>
		
</head>
	<body onload="main();">

	<div class="green-background"></div>
	<div class="wrap">
		<section class="left">
			<div class="profile">
				
			</div>
			<div class="wrap-search">
				<div class="search">
					<i class="fa fa-search fa" aria-hidden="true"></i>
					<input type="text" class="input-search" placeholder="Search">
				</div>
			</div>
			<div class="contact-list" id="contacts"></div>
		</section>

		<section class="right">
			<div class="chat-head">
				<div class="chat-name" style="padding-left:20px">
					<h1 class="font-name">Center</h1>
					<p id="statusBar" class="font-online">Connecting</p>
				</div>
				<i class="fa fa-times fa-lg" aria-hidden="true" id="close-contact-information"></i>
			</div>
			<div class="wrap-chat">
				<div class="chat" id="messagePanel"></div>
				<div class="information" ></div>
			</div>
			<div class="wrap-message">
				<i class="fa fa-smile-o fa-lg" aria-hidden="true"></i>
				<div class="message">
					<input type="text" id="typedMessage" class="input-message" placeholder="Type Message">
				</div>
			</div>
		</section>
	</div>		
</body>
</html>
