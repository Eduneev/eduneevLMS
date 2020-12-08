myapp.controller('SessionCntrl', ['$scope', '$http', 'Socket', 'TwoWaySocket', 'Constants', '$rootScope', function ($scope, $http, Socket, TwoWaySocket, Constants, $rootScope) {

    $scope.SessionID = 0;
    $scope.propertyName = '';
    $scope.reverse = false;

    GetStudio();
    GetPrograms();

    function GetStudio() {
        $http.get('/SessionMgmt/GetStudios')
            .then(function (result) {
                $scope.StudioList = result.data;
                $scope.StudioTextToShow = 'Please select studio..';
            });
    }
    function GetSessionStudio(SessionID) {
        $http.get('/SessionMgmt/GetStudio/' + SessionID)
            .then(function (result) {
                return result.data[0];
            });
    }

    function GetPrograms() {
        $http.get('/CourseMgmt/GetPrograms')
            .then(function (result) {
                $scope.Programs = result.data;
                $scope.ProgramTextToShow = 'Please select program..'
                $scope.SessionNameTextToShow = "System Generated"
            });
    }

    $scope.sortBy = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };

    $scope.GetCourse = function GetCourse() {
        $http.get('/CourseMgmt/GetCourse/' + $scope.ProgID)
            .then(function (result) {
                $scope.Courses = result.data;
                $scope.CourseTextToShow = 'Please select course..'
            });
    }

    $scope.GetSubject = function GetSubject() {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
            .then(function (result) {
                $scope.Subjects = result.data;
                $scope.SubjectTextToShow = 'Please select subject..'
            });
    }


    $scope.GetTopic = function GetTopic() {
        $scope.FacultyTextToShow = 'Please select faculty...'
        $http.get('/CourseMgmt/GetTopics/' + $scope.SubjectID)
            .then(function (result) {
                $scope.Topics = result.data;
                GetFaculty();
                GetSessionName();
            });
    }

    function GetFaculty() {
        $http.get('/FacultyMgmt/GetFaculty/' + $scope.SubjectID)
            .then(function (result) {
                $scope.Faculties = result.data;

            });
    }

    function GetSessionName() {
        $http.get('/SessionMgmt/GetSessionName/' + $scope.SubjectID + "/" + $scope.CourseID)
            .then(function (result) {
                $scope.SessionName = result.data;
            });
    }

    function GetCenterNameFromSession(CenterID) {
        return $http.get('/SessionMgmt/GetCenterNameFromSession/' + CenterID)
            .then(function (result) {
                return result.data;
            });
    };

    function GetChannel(SessionID) {
        $http.get('/SessionMgmt/GetChannel/' + SessionID)
            .then(function (result) {
                return result.data;
            });
    }

    $scope.GetSessions = function () {
        $http.get('/SessionMgmt/GetSessions')
            .then(function (result) {
                $scope.SessionsList = result.data;
            });
    };


    $scope.GetSessionsForCenter = function () {
        $http.get('/SessionMgmt/GetSessionsForCenter')
            .then(function (result) {
                $scope.SessionsList = result.data;
            });
    };

    $scope.ShowConnectedClassrooms = function () {
        console.log("HERER")
        if ($scope.SessionID == 0) {
            alert("Please select a session"); return;
        }
        window.location.href = "/SessionMgmt/ConnectedClassroom?SessionID=" + $scope.SessionID;
    }


    $scope.StartChat = function (SessionID, CenterID) {
        debugger;
        var promise = GetCenterNameFromSession(CenterID).then(function (response) {
            var url = "https://portal.2waylive.com/Chat.aspx?SessionID=" + SessionID + "&CenterName=" + response + "&CenterID=" + CenterID;
            var form = document.createElement("form");
            form.method = "POST";
            form.action = url;
            form.target = "_blank";
            document.body.appendChild(form);
            form.submit();
        });
    };

    $scope.StartStudioChat = function (SessionID, StudioName) {
        //debugger;
        var url = "https://portal.2waylive.com/StudioChat.aspx?SessionID=" + SessionID + "&StudioName=" + StudioName;
        var form = document.createElement("form");
        form.method = "POST";
        form.action = url;
        form.target = "_blank";
        document.body.appendChild(form);
        form.submit();
    };

    $scope.SaveRRQSession = function () {
        $scope.FacultyID = 0;
        $scope.PlannedCoverage = "";
        $scope.StudioID = 0;

        $scope.SaveSession();
    }

    $scope.SaveSession = function () {
        debugger;
        var _SessionName = $scope.SessionName;
        var _SessionDate = $scope.SessionDate;
        var _StartTime = $scope.StartTime;
        var _EndTime = $scope.EndTime;
        var _StudioID = $scope.StudioID;
        var _ProgID = $scope.ProgID;
        var _CourseID = $scope.CourseID;
        var _SubjectID = $scope.SubjectID;
        var _TopicID = $scope.TopicID;
        var _FacultyID = $scope.FacultyID;
        var _PlannedCoverage = $scope.PlannedCoverage;

        var _ProgCode = "";
        var _CourseCode = "";
        var _SubjectCode = "";

        for (var i of $scope.Programs)
            if (i.ProgID === _ProgID)
                _ProgCode = i.ProgramCode;

        for (var i of $scope.Courses)
            if (i.CourseID === _CourseID)
                _CourseCode = i.CourseCode;

        for (var i of $scope.Subjects)
            if (i.SubjectID === _SubjectID)
                _SubjectCode = i.SubjectCode;

        $http({
            method: 'POST',
            url: '/SessionMgmt/SaveSession',
            data: { SessionName: _SessionName, SessionDate: _SessionDate, StartTime: _StartTime, EndTime: _EndTime, StudioID: _StudioID, ProgID: _ProgID, ProgCode: _ProgCode, CourseID: _CourseID, CourseCode: _CourseCode, SubjectID: _SubjectID, SubjectCode: _SubjectCode, TopicID: _TopicID, FacultyID: _FacultyID, PlannedCoverage: _PlannedCoverage }
        }).then(function (result) {
            alert("Session Successfully created!");
        });
    };

    $scope.SelectSession = function (SessionID) {
        //debugger;
        $scope.SessionID = SessionID;

        $http.get('/SessionMgmt/GetSessionsRRQ/' + SessionID)
            .then(function (result) {
                $scope.SessionRRQList = result.data;
            });
    };

    $scope.StartSessionRRQ = function (rrqID) {
        window.location.href = "/SessionMgmt/RRQIntroduction/" + rrqID + "?SessionID=" + $scope.SessionID + "&RRQID=" + rrqID;
        /*
        $http({
            method: 'POST',
            url: '/SessionMgmt/SetSessionRRQ',
            data: { rrqID: rrqID }
        }).then(function (result) {
        });
        */
    }

    function StartTwoWayCall(btnType, SessionID) {

        $http.get('/SessionMgmt/GetStudio/' + SessionID)
            .then(function (result) {
                var studio = result.data[0];

                $http.get('/SessionMgmt/GetChannel/' + SessionID)
                    .then(function (result) {
                        var channel = result.data;
                        if (btnType === 'Start') {
                            ws2 = TwoWaySocket.StartSocket();

                            ws2.onError(function () {
                                alert("Unable to reach TwoWayCall Server.");
                            })

                            //var channel = GetChannel(_SessionID);

                            ws2.onOpen(function () {
                                ws2.send(JSON.stringify({
                                    profile: Constants.Profile['TWOWAYCALL'],
                                    type: Constants.Events['CONNECTION'],
                                    StudioName: studio.StudioName,
                                    StudioId: studio.StudioID,
                                    channel: channel
                                }));
                            })
                        }
                        else {
                            var ws2 = TwoWaySocket.StartSocket();
                            ws2.onOpen(function () {
                                ws2.send(JSON.stringify({
                                    profile: Constants.Profile['TWOWAYCALL'],
                                    type: Constants.Events['CLOSE'],
                                    channel: channel
                                }));
                            })
                        }
                    });

            });
    }

    $scope.StartStopSession = function (btnType) {
        //debugger;
        var _SessionID = $scope.SessionID;

        if (_SessionID === 0) {
            alert("Please select a session!");
        }
        else {
            StartTwoWayCall(btnType, _SessionID);
            
            var obs = new OBSWebSocket();
            obs.connect({ address: 'localhost:4444' })
                .catch(err => {
                    alert("Unable to Connect to 2WayLiveStudio. Please manage streaming manually.")
                });
            obs.on('ConnectionOpened', () => {
                if (btnType === 'Start') {

                    $http.get('/SessionMgmt/GetObsStream/' + $scope.SessionID)
                        .then(function (result) {
                            ind = result.data.lastIndexOf('/');
                            server = result.data.substr(0, ind);
                            key = result.data.substr(ind + 1);
                            console.log(server, key);
                            obs.sendCallback('StartStreaming', {
                                "stream": {
                                    "settings": {
                                        "server": server,
                                        "key": key
                                    }
                                }
                            }, (error) => {
                                console.log(error);
                                });
                        });
                }
                else
                    obs.send('stopStreaming', (error) => {
                    });
            });

            

            $scope.header = 'Success Message';
            if (btnType === 'Start') {
                $scope.body = 'Session started successfully!!';
                // Start websoket connection

                var ws = Socket.StartSocket();

                ws.onError(function () {
                    alert("Unable to Connect to RRQ Server.");
                });

                $http({
                    method: 'POST',
                    url: '/SessionMgmt/SetSession',
                    data: { SessionID: $scope.SessionID }
                }).then(function (result) {
                });

                ws.onOpen(function () {
                    ws.send(JSON.stringify({
                        profile: Constants.Profile['RRQ'],
                        type: Constants.Events['CONNECTION'],
                        action: Constants.Action['TEACHERCONNECTION'],
                        SessionID: _SessionID
                    }));

                });

                
            }
            else if (btnType === 'Stop') {
                $scope.body = 'Session stopped successfully!!';
                // Stop websocket connection

                var ws = Socket.StartSocket();
                ws.onOpen(function () {
                    ws.send(JSON.stringify({
                        profile: Constants.Profile['RRQ'],
                        type: Constants.Events['CLOSE'],
                        SessionID: _SessionID
                    }));
                })
            }

            $http({
                method: 'POST',
                url: '/SessionMgmt/StartStopSession',
                data: { SessionID: $scope.SessionID, Status: btnType }
            }).then(function (result) {
                $scope.GetSessions();
                $('#pop').modal('show')
            });

        }
       
    };

    $scope.StartStopRRQSession = function (btnType) {
        //debugger;
        var _SessionID = $scope.SessionID;

        if (_SessionID === 0) {
            alert("Please select a session!");
        }
        else {
            $scope.header = 'Success Message';
            if (btnType === 'Start') {
                $scope.body = 'Session started successfully!!';
                // Start websoket connection

                var ws = Socket.StartSocket();

                ws.onError(function () {
                    alert("Unable to Connect to RRQ Server.");
                });

                $http({
                    method: 'POST',
                    url: '/SessionMgmt/SetSession',
                    data: { SessionID: $scope.SessionID }
                }).then(function (result) {
                });

                ws.onOpen(function () {
                    ws.send(JSON.stringify({
                        profile: Constants.Profile['RRQ'],
                        type: Constants.Events['CONNECTION'],
                        action: Constants.Action['TEACHERCONNECTION'],
                        SessionID: _SessionID
                    }));

                });


            }
            else if (btnType === 'Stop') {
                $scope.body = 'Session stopped successfully!!';
                // Stop websocket connection

                var ws = Socket.StartSocket();
                ws.onOpen(function () {
                    ws.send(JSON.stringify({
                        profile: Constants.Profile['RRQ'],
                        type: Constants.Events['CLOSE'],
                        SessionID: _SessionID
                    }));
                })
            }

            $http({
                method: 'POST',
                url: '/SessionMgmt/StartStopSession',
                data: { SessionID: $scope.SessionID, Status: btnType }
            }).then(function (result) {
                $scope.GetSessions();
                $('#pop').modal('show');
            });
        }
    };

    $scope.DisplayClassrooms = function () {
        var _SessionID = $scope.SessionID;

        if (_SessionID === 0) {
            alert("Please select a session!");
        }
        else {
            $http.get('/SessionMgmt/GetChannel/' + _SessionID)
                .then(function (result) {
                    var channel = result.data;
                    window.location.href = "/SessionMgmt/TwoWayCall?SessionID=" + _SessionID + "&Channel=" + channel;
                });
        }
    }

    $scope.ShowNameFaceScreen = function () {
        window.location.href = "/SessionMgmt/NameFaceScreen?SessionID=" + $scope.SessionID;
    };

    $scope.ShowStreamKey = function (streamKey) {
        debugger;
        $scope.header = "Stream key";
        $scope.body = streamKey;
        $('#pop').modal('show')
    }

    $scope.CreateRRQ = function () {
        debugger;
        var _SessionID = $scope.SessionID;
        var _RRQNo = $scope.RRQName;
        var _ActiveFromDate = $scope.ActiveFromDate;
        var _ActiveToDate = $scope.ActiveToDate;
        //window.location.href = "/SessionMgmt/CreateRRQ/";
        $http({
            method: 'POST',
            url: '/SessionMgmt/CreateNewRRQ',
            data: { RRQNo: _RRQNo, SessionID: $scope.SessionID}
        }).then(function (result) {
            window.location.href = "/SessionMgmt/CreateRRQ/";
        });
    }

    $scope.ShowRRQDashboard = function (RRQ_ID) {
        window.location.href = "/RRQReport/RRQDashboard/" + RRQ_ID + "?RRQID=" + RRQ_ID;
    };

    $scope.ShowSessions = function () {
        window.location.href = "/SessionMgmt/ViewSessions";
    }

    $scope.ShowRRQSessions = function () {
        window.location.href = "/SessionMgmt/ViewRRQSessions";
    }
    $scope.EditSession = function (SessionID) {
        window.location.href = "/SessionMgmt/EditSession?SessionID=" + SessionID;
    }
    $scope.DeleteSession = function (SessionID) {
        if (confirm("Are you sure you want to Delete this Session? You will lose all data associated with the Session, including RRQ data"))
            $http({
                method: 'POST',
                url: '/SessionMgmt/DeleteSession',
                data: { SessionID: SessionID }
            }).then(function (result) {
                alert('Deleted Successfully!!');
                $scope.GetSessions();
            });
    }

}]);

myapp.controller('EditSessionCntrl', function ($scope, $http) {

    params = parseURLParams(window.location.href);
    if ('SessionID' in params) {
        try {
            $scope.SessionID = params['SessionID'][0];
            GetStudio();
            GetSessionDetails();
        }
        catch(err) {
            alert("Something went wrong. Go back and try again.")
        }
    }

    $scope.ShowSessions = function () {
        window.location.href = "/SessionMgmt/ViewSessions";
    }

    function GetSessionDetails() {
        $http.get('/SessionMgmt/GetSessionBySessionID/' + $scope.SessionID)
            .then(function (result) {
                data = result.data[0]
                console.log(data)
                $scope.ProgramTextToShow = "Please select program";
                $scope.CourseTextToShow = "Please select course";
                $scope.SubjectTextToShow = "Please select subject";
                $scope.ProgID = data.ProgID;
                $scope.ProgramName = data.ProgramName;
                $scope.CourseID = data.CourseID;
                $scope.CourseName = data.CourseName;
                $scope.SubjectID = data.SubjectID;
                $scope.SubjectName = data.SubjectName;
                $scope.Programs = [{ "ProgID": $scope.ProgID, "ProgramName": $scope.ProgramName }];
                $scope.Courses = [{ "CourseID": $scope.CourseID, "CourseName": $scope.CourseName }];
                $scope.Subjects = [{ "SubjectID": $scope.SubjectID, "SubjectName": $scope.SubjectName }];
                $scope.FacultyID = data.FacultyID;
                $scope.FacultyName = data.FacultyName;
                $scope.PlannedCoverage = data.PlannedCoverage;
                $scope.SessionDate = data.SessionDate;
                $scope.StartTime = data.StartTime;
                $scope.EndTime = data.EndTime;
                $scope.StudioID = data.StudioID;
                $scope.StudioName = data.StudioName;
                $scope.SessionName = data.SessionName;
                $scope.StreamKey = data.StreamKey;
                GetTopic();
                GetFaculty();
            });
    }


    function GetTopic() {
        $scope.FacultyTextToShow = 'Please select faculty...'
        $http.get('/CourseMgmt/GetTopics/' + $scope.SubjectID)
            .then(function (result) {
                $scope.Topics = result.data;
            });
    }

    function GetStudio() {
        $http.get('/SessionMgmt/GetStudios')
            .then(function (result) {
                $scope.StudioList = result.data;
                $scope.StudioTextToShow = 'Please select studio..';
            });
    }

    function GetFaculty() {
        $http.get('/FacultyMgmt/GetFaculty/' + $scope.SubjectID)
            .then(function (result) {
                $scope.Faculties = result.data;

            });
    }

    $scope.EditSession = function () {        
        var _SessionID = $scope.SessionID;
        var _SessionName = $scope.SessionName;
        var _SessionDate = $scope.SessionDate;
        var _StartTime = $scope.StartTime;
        var _EndTime = $scope.EndTime;
        var _StudioID = $scope.StudioID;
        var _ProgID = $scope.ProgID;
        var _CourseID = $scope.CourseID;
        var _SubjectID = $scope.SubjectID;
        var _TopicID = $scope.TopicID;
        var _FacultyID = $scope.FacultyID;
        var _PlannedCoverage = $scope.PlannedCoverage;
        var _StreamKey = $scope.StreamKey

        $http({
            method: 'POST',
            url: '/SessionMgmt/EditSession',
            data: { SessionID: _SessionID, SessionName: _SessionName, SessionDate: _SessionDate, StartTime: _StartTime, EndTime: _EndTime, StudioID: _StudioID, ProgID: _ProgID, CourseID: _CourseID, SubjectID: _SubjectID, TopicID: _TopicID, FacultyID: _FacultyID, PlannedCoverage: _PlannedCoverage, StreamKey: _StreamKey }
        }).then(function (result) {
            alert("Session Successfully saved!");
        });
    };

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

myapp.controller('SessionAttendanceCntrl', function ($scope, $http) {
    GetProgramsList();

    function GetProgramsList() {
        $http.get('/CourseMgmt/GetProgramsForCenter')
        .then(function (result) {
            $scope.ProgramsList = result.data;
        });
    }

    $scope.GetCourseList = function () {
        $http.get('/CourseMgmt/GetCourse/' + $scope.ProgID)
        .then(function (result) {
            $scope.CoursesList = result.data;
        });
    }

    $scope.GetSubjectsList = function () {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
        .then(function (result) {
            $scope.SubjectsList = result.data;
        });
    }

    $scope.GetStartedSessions = function () {
        $http.get('/SessionMgmt/GetStartedSessions/' + $scope.SubjectID)
        .then(function (result) {
            $scope.StartedSessionsList = result.data;
        });
    }

    $scope.InitiateAttendance = function () {
        var _SessionID = $scope.SessionID;
        $http({
            method: 'POST',
            url: '/StudentMgmt/InitiateAttendance',
            data: { SessionID: _SessionID }
        }).then(function (result) {
            window.location.href = "/SessionMgmt/StudentAttendance?SessionID=" + _SessionID;
        });
    };
});

myapp.controller('NameFaceScreenCntrl', function ($scope, $http) {
    GetCenters();
    $scope.CenterID = 0;
    params = parseURLParams(window.location.href);
    if ('SessionID' in params) {
        $scope.SessionID = params['SessionID'][0];
    }

    function GetCenters() {
        $http.get('/SessionMgmt/GetCentersForEntity')
            .then(function (result) {
                $scope.CentersList = result.data;
            });
    }
    $scope.GetStudentsByCenterID = function () {
        $http.get('/SessionMgmt/GetStudentsAttendanceByCenterID/' + $scope.CenterID + '/' + $scope.SessionID)
            .then(function (result) {
                $scope.StudentsList = result.data;
            });
    }
    $scope.ElargePhoto = function (StudentID) {
        $http.get('/SessionMgmt/GetStudentsByID/' + StudentID)
            .then(function (result) {
                $scope.StudentInfo = result.data;
                $scope.StudentName = $scope.StudentInfo[0].StudentName;
                $scope.StudentImage = $scope.StudentInfo[0].StudentImageURL;
            });
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

myapp.controller('ConnectedClassroomCntrl', function ($scope, $http) {
    params = parseURLParams(window.location.href);
    if ('SessionID' in params) {
        $scope.SessionID = params['SessionID'][0];
        GetConnectedClassrooms();
    }

    function GetConnectedClassrooms() {
        $http.get('/SessionMgmt/GetConnectedClassrooms/' + $scope.SessionID)
            .then(function (result) {
                $scope.ConnectedList = result.data;
                var a = new Date()
                $('#time').text(a.toLocaleString());
            });
    }

    $scope.sortBy = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };

    var myVar = setInterval(GetConnectedClassrooms, 15000);

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