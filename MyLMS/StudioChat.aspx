﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudioChat.aspx.cs" Inherits="MyLMS.StudioChat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8"/>
	<title>Chat</title>
	<link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet" />
	<script src="https://use.fontawesome.com/1c6f725ec5.js"></script>
	<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
	<link rel="stylesheet" href="Chat.css" />
	<script>

	// centers = {id: name, unread}
		// messages = {id: [{type: you/me, data: data, name: name},]}
		// Current issues: 
		//	1) Message from center being displayed and added to the currentId message group. Can the center send private messages?
		//  2) if teacher connection closes and center tries to send message, server gets an error and shuts down

		var urlParams = parseURLParams(window.location.href);
		console.log(urlParams);
		
		var SESSION;
		var STUDIONAME;

		if ("SessionID" in urlParams) {
			SESSION = urlParams["SessionID"][0];
		} else {
			SESSION = "0";
			alert("Exception: Unable to read Session from URL");
		}
		console.log(SESSION);

		if ("StudioName" in urlParams) {
			STUDIONAME = urlParams["StudioName"][0];
		} else {
			STUDIONAME = "Studio";
			console.log("Exception: Unable to read Session from URL");
		}
		console.log(STUDIONAME);


		const GROUP = "group";
		const UNREAD = "unread";

		var messagesPanel, typedMessage, first = true, currentId=GROUP;
		const TESTINGURI = "ws://localhost:4000";
		const PRODURI = "ws://ec2-18-218-225-8.us-east-2.compute.amazonaws.com:4000";
		var centers = {}
		var messages = {};				
		var statusBar;
		function main() {
		
			messagesPanel = document.getElementById("messagePanel");
			typedMessage = document.getElementById("typedMessage");
			statusBar = document.getElementById("statusBar");
			var list = document.getElementById("contacts");
				
				var contact = document.createElement("div");
				contact.className = "new-message-contact active-contact";
				var text = document.createElement("div");
				contact.onclick = displayMessages;
				contact.id = GROUP;
				text.className = "contact-text";
				text.textContent = "Group Message";
				var newMessage = document.createElement("div");
				newMessage.id = "nm";
				contact.appendChild(newMessage);
				contact.appendChild(text);
				list.appendChild(contact);
				centers[GROUP] = {"name": GROUP, "unread": 0};
	

			messages[GROUP] = [];
			
			const ws = new WebSocket(TESTINGURI);
		
			ws.onerror = function(e){
				alert("Not able to connect to server");
			}
			
			ws.onopen = function(e) {
			statusBar.textContent = "Connected";
			ws.send(JSON.stringify({
				session: SESSION,
					type: "teacherAdd",
					name: STUDIONAME
			}));
			}
			
			ws.onmessage = function ws_message(input){
				var data = input.data;
				console.log(input);
				if (isJson(data)){
					console.log("Received message!" + data);
					var obj = JSON.parse(data);
					if (obj.type == "centerAdd"){
						// add to the List								
						addCenter(obj.id, obj.name);
					}
					else if (obj.type == "centerRemove"){
						var contact = document.getElementById(obj.id);
						delete centers[obj.id];
						console.log(centers);
						contact.remove();
					}
					else if (obj.data){
						console.log("Received id: " + obj.id);
						var name = centers[obj.id]["name"];
						if(obj.student != ""){
							name += " - " + obj.student;
						}
						var messageWrapper = {'type': 'you', 'data': obj.data,'name': name};
						if(obj.type == "groupMessage"){
							if (currentId==GROUP)
								addMessage(obj.data, true, name);
							else{
								addUnread(GROUP);
							}
							messages[GROUP].push(messageWrapper);
						}
						else{
							if(obj.id == currentId)
								addMessage(obj.data, true, name)
							else{
								addUnread(obj.id);
							}

							messages[obj.id].push(messageWrapper);

						}
						console.log(messages);

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
				addMessage(typedMessage.value, false);
				var messageWrapper = {'type': 'me', 'data': typedMessage.value};
				messages[currentId].push(messageWrapper);
				console.log(messages);
		
				if(currentId == GROUP){
					ws.send(JSON.stringify({
								type: "groupMessage",
								group: "teacher",
					session: SESSION,
					data: typedMessage.value
					}));
				}
				else{
				ws.send(JSON.stringify({
						type: "singleMessage",
						group: "teacher",
						session: SESSION,
						data: typedMessage.value,
						id: currentId
					}));
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
		

		function isJson(str) {
			try {
				JSON.parse(str);
			} catch (e) {
				console.log(e);
				return false;
			}
			return true;
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
		
		function addCenter(id, name){
			centers[id] = {"name": name, "unread": 0};
			var list = document.getElementById("contacts");
			var contact = document.createElement("div");
			contact.id = id;
			contact.onclick = displayMessages;
			contact.className = "new-message-contact";
			var text = document.createElement("div");
			text.className = "contact-text";
			text.textContent = name;
			var newMessage = document.createElement("div");
			newMessage.id = "nm"
			contact.appendChild(newMessage);
			contact.appendChild(text);
			list.appendChild(contact);
			messages[id] = [];

		}

		function addUnread(id){
			var unread = centers[id][UNREAD];
			unread = unread+1;
			centers[id][UNREAD] = unread;
			var group = document.getElementById(id);
			var m = group.childNodes.item("nm");
			m.className = "new-message";
			m.textContent = unread;
		}

		function displayMessages(evt){
			var name = evt.target;
			console.log("reaching here");
			var id = this.id // get the contact id
			var c = currentId;
			if (id == GROUP){
				// display all group messages						
				if (c != GROUP){
					currentId = GROUP;
					document.getElementById(GROUP).className ="new-message-contact active-contact";
					document.getElementById(c).className = "new-message-contact"; 
					console.log("Current id: " + currentId);
					messagesPanel.innerHTML = "";
					var messageList = messages[GROUP];
				}
			}
			else{
				// display only the private chat messages for that id
				if (c != id){
					document.getElementById(id).className ="new-message-contact active-contact";
					document.getElementById(currentId).className = "new-message-contact"; 
					currentId = id;
					console.log("Current id: " + currentId);
					messagesPanel.innerHTML = "";
					var messageList = messages[id];
				}
			}

			var g = document.getElementById(currentId);
			var s = g.childNodes.item("nm");
			s.className = "";
			s.textContent = "";
			centers[currentId][UNREAD] = 0;

			// display appropriate messages
			messageList.forEach(function(mess){
					if(mess.type == "you"){
						addMessage(mess.data, true, mess.name);
					}
					else
						addMessage(mess.data, false);
				});

		}

		function addMessage(message, left, name = "") {
			console.log("name: " + name);
			var flexBox = document.createElement('div');
			if (!left)
			flexBox.className = "chat-bubble me";
			else
			flexBox.className = "chat-bubble you";
		//  flexBox.style = 'display: flex; ' + (left ? '' : 'justify-content: flex-end');
			var time = new Date().toLocaleTimeString();
			var messageBox = document.createElement('p');
			messageBox.className = "content";
			var timeShow = document.createElement('p');
			var nameShow = document.createElement('p');
			timeShow.className = "time";
			timeShow.textContent = name + " " + time;
			
			nameShow.className = "name";
			nameShow.textContent = name;
			
			messageBox.textContent = message;
			flexBox.appendChild(messageBox);
			flexBox.appendChild(timeShow);
		//  flexBox.appendChild(nameShow);
		//  if (messages.length == 50) {
		//    messagesPanel.removeChild(messages.shift());
		//  }
		//  messages.push(flexBox);
			setTimeout(function() {
			messageBox.className = "show";
			}, 10);
		
			var wereBottomScrolled = true;//messagesPanel.scrollTop == messagesPanel.scrollHeight;
			messagesPanel.appendChild(flexBox);
			if (wereBottomScrolled) {
			messagesPanel.scrollTop = messagesPanel.scrollHeight;
			}
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
					<input type="text" class="input-search" placeholder="Search"/>
				</div>
			</div>
			<div class="contact-list" id="contacts"></div>
		</section>

		<section class="right">
			<div class="chat-head">
				<div class="chat-name" style="padding-left:20px">
					<h1 class="font-name">Studio</h1>
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
					<input type="text" id="typedMessage" class="input-message" placeholder="Type Message" />
				</div>
			</div>
		</section>
	</div>
		

</body>
</html>
