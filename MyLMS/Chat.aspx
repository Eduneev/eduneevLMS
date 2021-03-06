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
		var Events;
	(function (Events) {
		Events["CONNECTION"] = "Connection";
		Events["GROUPMESSAGE"] = "groupMessage";
		Events["SINGLEMESSAGE"] = "singleMessage";
		Events["DISCONNECTION"] = "Disconnect";
		Events["CENTERREMOVE"] = "centerRemove";
	})(Events || (Events = {}));
	var Profile;
	(function (Profile) {
		Profile["STARCHAT"] = "starchat";
	})(Profile || (Profile = {}));
	var Group;
	(function (Group) {
		Group["TEACHER"] = "teacher";
		Group["CENTER"] = "center";
	})(Group || (Group = {}));
	var Message;
	(function (Message) {
		Message["PROFILE"] = "profile";
		Message["TYPE"] = "type";
		Message["SESSIONID"] = "SessionID";
		Message["GROUP"] = "group";
		Message["WSID"] = "wsID";
		Message["DATA"] = "data";
		Message["STUDENT"] = "student";
		Message["NAME"] = "name";
	})(Message || (Message = {}));

	var messagesPanel, typedMessage, first = true, ws, centerName;
	
	const TESTINGURI = "ws://localhost:4000";
    const PRODURI = "ws://52.15.186.193:4000";
    const SERVERURI = 'http://portal.2waylive.com/api/';
	var statusBar;

	var urlParams = parseURLParams(window.location.href);
	var SESSION;
	var NAME;
	var CenterID

	if ("SessionID" in urlParams) {
        SESSION = urlParams["SessionID"][0];
        console.log(SESSION);
	} else {
		alert("Exception: Unable to read Session from URL");
	}

	if ("CenterID" in urlParams) {
		CenterID = urlParams["CenterID"][0];
	} else {
		alert("Unable to read CenterID from URL");
	}

	if ("CenterName" in urlParams) {
		NAME = urlParams["CenterName"][0];
	} else {
		NAME = "Center";
	}
	
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
		centerName = document.getElementById("centerName");
		var list = document.getElementById("contacts");

		centerName.innerHTML = NAME;

		var contact = document.createElement("div");
				contact.className = "new-message-contact active-contact";
				var text = document.createElement("div");
				contact.id = GENERAL;  // general chat 
				contact.onclick = displayMessages;
				text.className = "contact-text";
				text.textContent = "Session Chat";
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
				text2.textContent = "Support Chat";
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
		
        //ws = new WebSocket(TESTINGURI);
        ws = new WebSocket(PRODURI);

		ws.onerror = function(e){
			alert("Could not connect to server");
		}

		ws.onopen = function(e) {
			ws.send(JSON.stringify({
				profile: Profile.STARCHAT,
				group: Group.CENTER,
				SessionID: SESSION,
				type: Events.CONNECTION,
				name: NAME
			}));
			statusBar.textContent = "Connected!";
		}

		ws.onmessage = function ws_message(input){
			var data = input.data;
			if (isJson(data)){
                var obj = JSON.parse(data);
                var messageWrapper = { 'type': 'you', 'data': obj.data, 'time': new Date().toLocaleTimeString() };
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
		};

		ws.onclose = function() {
			statusBar.textContent = 'Connection lost';
			typedMessage.disabled = true;
		};
		typedMessage.onkeydown = function(e) {
			if (e.keyCode === 13 && !e.shiftKey) {
				if (typedMessage.value.length) {
					typedMessage.value = typedMessage.value.substring(0, 512);
					var messageWrapper = {'type': 'me', 'data': typedMessage.value, 'time': new Date().toLocaleTimeString()};
					if(currentId == GENERAL){
						if(currentStudentId){
							ws.send(JSON.stringify({
								profile: Profile.STARCHAT,
								type: Events.GROUPMESSAGE,
								group: Group.CENTER,
								SessionID: SESSION,
                                data: typedMessage.value,
                                student: document.getElementById(currentStudentId).textContent
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
							profile: Profile.STARCHAT,
							type: Events.SINGLEMESSAGE,
							group: Group.CENTER,
							SessionID: SESSION,
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
			if (first) {
				typedMessage.value = "";
				first = false;
			}
		}
	}

    function filterFunction() {
        var input, filter, ul, li, a, i, txtValue, arr;
        arr = [];
        input = document.getElementById("search");
        filter = input.value.toUpperCase();
        ul = document.getElementById("contacts");
        arr.push(ul.getElementsByClassName("contact"));
        arr.push(ul.getElementsByClassName("disabled-contact"));
        arr.forEach(function (li) {
            for (i = 0; i < li.length; i++) {
                a = li[i].getElementsByTagName("span")[0];
                txtValue = a.textContent || a.innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    li[i].style.display = "";
                } else {
                    li[i].style.display = "none";
                }
            }
        });

        li = ul.getElementsByClassName("contact");
        for (i = 0; i < li.length; i++) {
            a = li[i].getElementsByTagName("span")[0];
            txtValue = a.textContent || a.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                li[i].style.display = "";
            } else {
                li[i].style.display = "none";
            }
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
		return false;
	}
	return true;
	}

	function addStudents() {

        list = document.getElementById("contacts");

        const url = SERVERURI + SESSION + "/" + CenterID + "/getAttendingStudents";
        //const url = "http://localhost:55082/api/" + SESSION + "/" + CenterID + "/getAttendingStudents";
		
        var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
			if (xhr.readyState == XMLHttpRequest.DONE) {
				var students = xhr.response;
				for (let i=0; i < students.length; i++) {
                    var data = students[i];
					var student = document.createElement('div');
					student.className = 'contact';
					student.id = data['StudentID'];
					var image_placeholder = document.createElement("img");
					image_placeholder.src = data['StudentImageURL'];
                    image_placeholder.id = "image";

					var text = document.createElement("span");
					text.className = "contact-text2";
					text.id = "text";
					text.textContent = data['StudentName'];

					student.appendChild(image_placeholder);
                    student.appendChild(text);
                    if (data['Present'] == 0) {
                        student.className = 'disabled-contact'
                        student.disabled = true;
                    }
                    else
    					student.onclick = setCurrentStudentId;

					list.appendChild(student);
				}
            }
		}
		xhr.responseType='json';
		xhr.open('GET', url, true);
		xhr.send(null);
	}

	function addUnread(id) {
		var unread = studio[id][UNREAD];
		unread = unread+1;
		studio[id][UNREAD] = unread;
		var group = document.getElementById(id);
		var m = group.childNodes.item("nm");
		m.className = "new-message";
		m.textContent = unread;
	}

	function setCurrentStudentId(evt) {
		var name = evt.target;
		var id = this.id;

		if (currentId != PRIVATE){
			if(!currentStudentId){
				currentStudentId = id;
			}
			else if(currentStudentId != id){
				document.getElementById(currentStudentId).className = "contact";
				currentStudentId = id;
			}
			document.getElementById(currentStudentId).className = "active-student";
		}
	}

	function displayMessages(evt){
		var name = evt.target;
		var id = this.id // get the contact id
		var c = currentId;
		// display on{ly the private chat messages for that id
		if (id == PRIVATE) {
            var result = authenticatePrivateMessage(function(data) { if (!result) return; });
		}

        if (c != id) {            
			if(id==PRIVATE){
				if(currentStudentId){
					document.getElementById(currentStudentId).className = "contact";
					currentStudentId = null;
				}
			}
			document.getElementById(id).className ="new-message-contact active-contact";
			document.getElementById(currentId).className = "new-message-contact"; 
			currentId = id;
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
					addMessage(mess.data, true, mess.time);
				}
				else
					addMessage(mess.data, false, mess.time);
			});
	}

	function authenticatePrivateMessage(callback) {
		// prompt the user for credentials
		var pass = prompt("Enter credentials:")
		const url = SERVERURI + "AuthenticateSupportChat/" + pass;
		var xhr = new XMLHttpRequest();
		xhr.onreadystatechange = function() {
			if (xhr.readyState == XMLHttpRequest.DONE) {
				var students = xhr.response;
                if (students == false) {
                    alert("Incorrect auth");
                    callback(false);
                }
                else
                    callback(true);
			}
		}
		xhr.responseType='json';
        xhr.open('GET', url, true);
		xhr.send(null);
	}

	function addMessage(message, left, time="") {
		var flexBox = document.createElement('div');if (!left)
		flexBox.className = "chat-bubble me";
		else
		flexBox.className = "chat-bubble you";
	//  flexBox.style = 'display: flex; ' + (left ? '' : 'justify-content: flex-end');
        if (time == "")
    		time = new Date().toLocaleTimeString();
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
					<input id="search" onkeyup="filterFunction()" type="text" class="input-search" placeholder="Search">
				</div>
			</div>
			<div class="contact-list" id="contacts"></div>
		</section>

		<section class="right">
			<div class="chat-head">
				<div class="chat-name" style="padding-left:20px">
					<h1 class="font-name" id="centerName">Center</h1>
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
