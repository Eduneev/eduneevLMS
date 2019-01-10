myapp.controller('SessionCntrl', ['$scope', '$http', 'Socket', 'TwoWaySocket', 'Constants', '$rootScope', function ($scope, $http, Socket, TwoWaySocket, Constants, $rootScope) {

    $scope.SessionID = 0;

    GetStudio();
    GetPrograms();

    function GetStudio() {
        $http.get('/SessionMgmt/GetStudios')
            .then(function (result) {
                $scope.StudioList = result.data;
                $scope.StudioTextToShow = 'Please select studio..'
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
        $http.get('/CourseMgmt/GetFaculty/' + $scope.SubjectID)
            .then(function (result) {
                $scope.Faculties = result.data;
            });
    }

    function GetSessionName() {
        $http.get('/CourseMgmt/GetSessionName/' + $scope.SubjectID)
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


    $scope.StartChat = function (SessionID, CenterID) {
        debugger;
        var promise = GetCenterNameFromSession(CenterID).then(function (response) {
            var url = "http://localhost:55082/Chat.aspx?SessionID=" + SessionID + "&CenterName=" + response + "&CenterID=" + CenterID;
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
        var url = "http://localhost:55082/StudioChat.aspx?SessionID=" + SessionID + "&StudioName=" + StudioName;
        var form = document.createElement("form");
        form.method = "POST";
        form.action = url;
        form.target = "_blank";
        document.body.appendChild(form);
        form.submit();
    };


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

        if (_SessionID == 0) {
            alert("Please select a session!");
        }
        else {
            StartTwoWayCall(btnType, _SessionID);
            
            var obs = new OBSWebSocket();
            obs.connect({ address: 'localhost:4444' })
                .catch(err => {
                    alert("Unable to Connect to 2WayLiveStudio. Please start streaming manually.")
                });
            obs.onConnectionOpened(() => {
                if (btnType === 'Start') {

                    $http.get('/SessionMgmt/GetObsStream/' + $scope.SessionID)
                        .then(function (result) {
                            ind = result.data.lastIndexOf('/');
                            server = result.data.substr(0, ind);
                            key = result.data.substr(ind + 1);
                            console.log(server + " " + key);
                            obs.startStreaming({
                                "stream": {
                                    "settings": {
                                        "server": server,
                                        "key": key
                                    }
                                }
                            });
                    });
                }
                else
                    obs.stopStreaming();
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

    $scope.DisplayClassrooms = function () {
        var _SessionID = $scope.SessionID;

        if (_SessionID == 0) {
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
        window.location.href = "/SessionMgmt/NameFaceScreen/";
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
        window.location.href = "/SessionMgmt/RRQDashboard/" + RRQ_ID + "?RRQID=" + RRQ_ID;
    };

    $scope.ShowSessions = function () {
        window.location.href = "/SessionMgmt/ViewSessions";
    }

}]);

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
    function GetCenters() {
        $http.get('/SessionMgmt/GetCentersForEntity')
            .then(function (result) {
                $scope.CentersList = result.data;
            });
    }
    $scope.GetStudentsByCenterID = function () {
        $http.get('/SessionMgmt/GetStudentsByCenterID/' + $scope.CenterID)
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
});

