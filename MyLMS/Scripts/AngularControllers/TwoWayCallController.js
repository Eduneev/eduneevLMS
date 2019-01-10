myapp.controller('TwoWayCallCntrl', function ($scope, $http, TwoWaySocket, Constants) {
    //GetClassrooms();
    //function GetClassrooms() {
    //    $http.get('/SessionMgmt/GetClassrooms')
    //    .then(function (result) {
    //        $scope.ClassroomsList = result.data;
    //    });
    //}

    $scope.channel = 0;
    $scope.SessionID = 0;
    $scope.ConnectedList = [];

    SetChannel(window.location.href);

    function SetChannel (url) {
        var params = parseURLParams(url);
        if ('Channel' in params) {
            $scope.channel = params['Channel'][0];
            $scope.SessionID = params['SessionID'][0];
            StartSocket();
        }
    }

    function StartSocket() {

        $http.get('/SessionMgmt/GetStudio/' + $scope.SessionID)
            .then(function (result) {
                var studio = result.data[0];
                $scope.ws = TwoWaySocket.StartSocket();
                $scope.ws.onOpen(function () {
                    s = document.getElementById('connect');
                    s.style.color = "green";
                    s.textContent = "Connected";

                    $scope.ws.send(JSON.stringify({
                        profile: Constants.Profile['TWOWAYCALL'],
                        type: Constants.Events['CONNECTION'],
                        StudioName: studio.StudioName,
                        StudioId: studio.StudioID,
                        channel: $scope.channel
                    }));
                });

                ManageSocket();
            });
    }

    function ManageSocket() {
        $scope.ws.onMessage(function (input) {
            var data = input.data;
            var obj = JSON.parse(data);
            if (obj.type == Constants.Events['CONNECTION']) {
                // add to the List
                $scope.ConnectedList.push(obj);
            }
            else if (obj.type == Constants.Events['DISCONNECTION']) {                
                for (var i = 0; i < $scope.ConnectedList.length; i++) {
                    if (obj.wsID == $scope.ConnectedList[i].wsID) {
                        $scope.ConnectedList.splice(i, 1);
                        break;
                    }
                }
            }           
            
        });

        $scope.ws.onClose(function () {
            for (var i = 0; i < $scope.ConnectedList.length; i++) {
                    $scope.ConnectedList.splice(i, 1);
            }
            s = document.getElementById('connect');
            s.textContent = "Disconnected";
            s.style.color = "red";
        });
    }

    $scope.CallEvent = function (wsID, call) {
        if (call) {
            $scope.ws.send(
                JSON.stringify({
                    profile: Constants.Profile['TWOWAYCALL'],
                    type: Constants.Events['MESSAGE'],
                    action: Constants.Action['JOIN'],
                    wsID: wsID,
                    channel: $scope.channel
                }));
        }
        else {
            $scope.ws.send(
                JSON.stringify({
                    profile: Constants.Profile['TWOWAYCALL'],
                    type: Constants.Events['MESSAGE'],
                    action: Constants.Action['LEAVE'],
                    wsID: wsID,
                    channel: $scope.channel
                }));
        }
    }

    function parseURLParams(url) {
        var queryStart = url.indexOf("?") + 1,
            queryEnd = url.indexOf("#") + 1 || url.length + 1,
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
});