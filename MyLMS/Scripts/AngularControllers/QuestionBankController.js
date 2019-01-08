myapp.controller('QuestionCntrl', function ($scope, $http, $rootScope, Socket, Constants) {

    /*
     * {
            RRQID,
            QID,
            Start,
            Next,
            Stop,
            End        
        }
     * 
     */

    GetTags();

    SetParams(window.location.href);

    function SetParams(url) {
        try {
            var params = parseURLParams(url);
            if ('SessionID' in params) {
                $scope.SessionID = params['SessionID'][0];
                $scope.ws = Socket.StartSocket();
                $scope.ws.onOpen(function () {
                    console.log("Started socket.");
                    $scope.ws.send(JSON.stringify({
                        profile: Constants.Profile['RRQ'],
                        type: Constants.Events['CONNECTION'],
                        action: Constants.Action['TEACHERCONNECTION'],
                        SessionID: $scope.SessionID
                    }));

                });
            }
            else
                alert("Error in retrieving session. Click Start Session again!")
            if ('RRQID' in params) {
                $scope.RRQID = params['RRQID'][0];
            }
            else
                alert("Error in retrieving RRQ. Select RRQ again!")
        }
        catch (e) {
            
        }
    }

    /*
    $http({
        method: 'GET',
        url: '/SessionMgmt/GetSession'
    }).then(function (result) {
        result.data = parseInt(result.data);
        if (result.data == -1)
            alert("Error in retrieving session. Click Start Session again!")
        else {
            $scope.SessionID = result.data;
            console.log($scope.SessionID);

            $scope.ws = Socket.StartSocket();
            $scope.ws.onOpen(function () {
                console.log("Started socket.");
                $scope.ws.send(JSON.stringify({
                    profile: Constants.Profile['RRQ'],
                    type: Constants.Events['CONNECTION'],
                    action: Constants.Action['TEACHERCONNECTION'],
                    SessionID: $scope.SessionID
                }));

            });
        }
    });

    $http({
        method: 'GET',
        url: '/SessionMgmt/GetSessionRRQ'
    }).then(function (result) {
        result.data = parseInt(result.data);
        if (result.data == -1)
            alert("Error in retrieving RRQ. Select RRQ again!")
        else
            $scope.RRQID = result.data;
        console.log($scope.RRQID);
    });
    */

    $scope.AddQuestion = function () {
        window.location.pathname = "/QuestionBank/AddQuestion";
    }

    // DeleteQuestion
    $scope.DeleteQuestion = function (QID) {
        $http({
            method: 'POST',
            url: '/QuestionBank/DeleteQuestion',
            data: { QID: QID }
        }).then(function (result) {
            alert('Deleted Successfully!!');
        });
    }

    $scope.SaveQuestion = function () {
        debugger;
        var _QuestionText = $scope.QuestionText
        var _QTagID = $scope.QTagID
        var _DisplayTime = $scope.DisplayTime
        if (_DisplayTime === null)
            _DisplayTime = 20;
        $http({
            method: 'POST',
            url: '/QuestionBank/SaveQuestion',
            data: { QuestionText: _QuestionText,QTagID: _QTagID, DisplayTime: _DisplayTime }
        }).then(function (result) {

            // 

            ////// Save Options //////
            $http({
                method: 'POST',
                url: '/QuestionBank/SaveOptions',
                data: { OptionSeq: 1, OptionText: $scope.Option1Text, OptionMark: $scope.Option1Mark, IsOptionCorrect: $scope.IsOption1Correct }
            }).then(function (result) {
                });

            $http({
                method: 'POST',
                url: '/QuestionBank/SaveOptions',
                data: { OptionSeq: 2, OptionText: $scope.Option2Text, OptionMark: $scope.Option2Mark, IsOptionCorrect: $scope.IsOption2Correct }
            }).then(function (result) {
                });


            $http({
                method: 'POST',
                url: '/QuestionBank/SaveOptions',
                data: { OptionSeq: 3, OptionText: $scope.Option3Text, OptionMark: $scope.Option3Mark, IsOptionCorrect: $scope.IsOption3Correct }
            }).then(function (result) {
                });

            $http({
                method: 'POST',
                url: '/QuestionBank/SaveOptions',
                data: { OptionSeq: 4, OptionText: $scope.Option4Text, OptionMark: $scope.Option4Mark, IsOptionCorrect: $scope.IsOption4Correct }
            }).then(function (result) {
                });

        });
        alert('Saved Successfully!!');
            /////////////
    }

    $scope.AddRemoveQuestionToRRQ = function (QID) {
        $http({
            method: 'POST',
            url: '/QuestionBank/AddRemoveQuestionToRRQ',
            data: { QID: QID }
        }).then(function (result) {
        });
    }

    $scope.QuestionList = '';
    ///////// Get Questions ////////
    $scope.GetQuestionDetails = function () {
        $http.get('/QuestionBank/GetQuestions')
            .then(function (result) {
            $scope.QuestionList = result.data;
        });
    }

    $scope.RRQQuestionList = '';
    ///////// Get RRQ Questions ////////
    $scope.GetRRQQuestionDetails = function () {
        $http.get('/QuestionBank/GetRRQQuestions')
            .then(function (result) {
                $scope.RRQQuestionList = result.data;
            });
    }

    $scope.StartRRQ = function () {
        window.location.pathname = '/SessionMgmt/ViewRRQ'
    }

    $scope.StopQuestion = function () {
        $scope.ws.send(JSON.stringify({
            profile: Constants.Profile['RRQ'],
            type: Constants.Events['MESSAGE'],
            action: Constants.Action['STOP'],
            RrqID: $scope.RRQID,
            qID: $scope.RRQQuestionList[$scope.currentPage].Question.QID,
            SessionID: $scope.SessionID
        }));
    }

    $scope.DisplayOptions = function () {
       // debugger;
        // Send message to server to start current question polling
        // Get QID somehow
        console.log($scope.RRQQuestionList[$scope.currentPage].Question.QID);
        $scope.ws.send(JSON.stringify({
            profile: Constants.Profile['RRQ'],
            type: Constants.Events['MESSAGE'],
            action: Constants.Action['START'],
            RrqID: $scope.RRQID,
            qID: $scope.RRQQuestionList[$scope.currentPage].Question.QID, 
            SessionID: $scope.SessionID
        }));

        $scope.DisplayOption = 'True';
        StartTimer();
    }

    $scope.NextQuestion = function () {
        $scope.currentPage = $scope.currentPage + 1
        $scope.DisplayOption = 'False';
    }

    $scope.EndRRQ = function () {
        $scope.ws.send(JSON.stringify({
            profile: Constants.Profile['RRQ'],
            type: Constants.Events['MESSAGE'],
            action: Constants.Action['END'],
            RrqID: $scope.RRQID,
            qID: $scope.RRQQuestionList[$scope.currentPage].Question.QID,
            SessionID: $scope.SessionID
        }));
    }

    $scope.currentPage = 0;
    $scope.pageSize = 5;
    $scope.numberOfPages = function () {
        return Math.ceil($scope.QuestionList.length / $scope.pageSize);
    }

    $scope.RRQpageSize = 1;
    $scope.RRQnumberOfPages = function () {
        return Math.ceil($scope.QuestionList.length / $scope.RRQpageSize);
    }


    $scope.TagTextToShow = "All Question Tags";
    function GetTags() {
        $http.get('/QuestionBank/GetTags')
            .then(function (result) {
                $scope.TagsList = result.data;
            });
    }

    $scope.SetTagFilter = function () {
        console.log($scope.QTagID);

        if ($scope.QTagID == null)
            $scope.GetQuestionDetails();
        else
            GetQuestionByTag();
    }


    function GetQuestionByTag() {
        $http.get('/QuestionBank/GetQuestionsByTag/' + $scope.QTagID)
            .then(function (result) {
                $scope.QuestionList = result.data;
            });
    }

    // DeleteQuestion
    $scope.DeleteTag = function (QTagID) {
        $http({
            method: 'POST',
            url: '/QuestionBank/DeleteTag',
            data: { QTagID: QTagID }
        }).then(function (result) {
            alert('Deleted Successfully!!');
            GetTags()
        });
    }

    $scope.CreateTags = function (TagText) {
        $http({
            method: 'POST',
            url: '/QuestionBank/CreateTags',
            data: { TagText: TagText }
        }).then(function (result) {
            alert('Created Successfully!!');
            GetTags()
        });
    }

    function CallstartTimer(duration, display) {
        var start = Date.now(),
            diff,
            minutes,
            seconds;
        function timer() {
            // get the number of seconds that have elapsed since 
            // startTimer() was called
            diff = duration - (((Date.now() - start) / 1000) | 0);

            // does the same job as parseInt truncates the float
            minutes = (diff / 60) | 0;
            seconds = (diff % 60) | 0;

            minutes = minutes < 10 ? "0" + minutes : minutes;
            seconds = seconds < 10 ? "0" + seconds : seconds;

            display.textContent = minutes + ":" + seconds;

            if (diff <= 0) {
                // add one second so that the count down starts at the full duration
                // example 05:00 not 04:59
                start = Date.now() + 1000;
            }
        };
        // we don't want to wait a full second before the timer starts
        timer();
        setInterval(timer, 1000);
    }

    function StartTimer() {
        //var oneMinute = 60,
        console.log($scope.RRQQuestionList[$scope.currentPage].Question.QTime);
        var oneMinute = $scope.RRQQuestionList[$scope.currentPage].Question.QTime,
            display = document.querySelector('#time');
        CallstartTimer(oneMinute, display);
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

myapp.filter('startFrom', function () {
    return function (input, start) {
        start = +start; //parse to int
        return input.slice(start);
    }
});