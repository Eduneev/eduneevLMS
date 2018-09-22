﻿myapp.factory('Socket', function ($websocket) {
    // Open a WebSocket connection
    var ws;

    function StartSocket() {
        this.ws = $websocket('ws://localhost:3000');
    }

    function SetWs(websockconn) {
        ws = websockconn;
    }
    var methods = {
        ws: ws,
        StartSocket: function () {
            ws = $websocket('ws://localhost:3000');
            return ws;
        }
    };
    return methods;
});