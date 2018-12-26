myapp.factory('Socket', function ($websocket) {
    // Open a WebSocket connection
    var ws;

    function SetWs(websockconn) {
        ws = websockconn;
    }
    var methods = {
        ws: ws,
        StartSocket: function () {
            ws = $websocket("ws://52.15.186.193:3000");
            //ws = $websocket('ws://localhost:3000');
            return ws;
        }
    };
    return methods;
});

myapp.factory('TwoWaySocket', function ($websocket) {
    var ws;

    var methods = {
        ws: ws,
        StartSocket: function () {
            ws = $websocket("ws://52.15.186.193:2000");
            //ws = $websocket("ws://localhost:2000");
            return ws;
        }
    }
    return methods;
});