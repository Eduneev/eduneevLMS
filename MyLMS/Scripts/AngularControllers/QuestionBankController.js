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

    $scope.HandleFiles = function () {
        console.log("handling file")
        l = document.getElementById("left");
        var x = document.createElement("INPUT");
        x.id = "files";
        x.setAttribute("type", "file");
        left.appendChild(x);

        $(document).ready(function () {
            $('#files').bind('change', handleCSVFile);
        });
    }

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
            console.log(e);
        }
    }

    $scope.AddQuestion = function () {
        window.location.href = "/QuestionBank/AddQuestion";
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

    $scope.GetSelectedQuestionDetails = function (QID) {
        window.location.href = "/QuestionBank/EditQuestion" + "?QID=" + QID;
    }


    function handleCSVFile(event) {
        console.log("GETTING HERE");
        var files = event.target.files;
        var file = files[0]
        var reader = new FileReader();
        reader.readAsText(file);
        reader.onload = function (event) {
            var csv = event.target.result;
            var data = $.csv.toObjects(csv);
            var added = 0;
            var not_added = 0;
            for (obj of data) {
                var val = SaveCSVQuestion(obj);
                if (val == 1)
                    added += 1
                else
                    not_added += 1
            }
            s = "Added " + added + " questions. Error in adding " + not_added + " questions";
            alert(s);
        }
    }
    

    function SaveCSVQuestion(data) {
        console.log(data)
        try {
            var QTag = data.QTag
            for (tag of $scope.TagsList) {
                if (QTag == tag.TagText) {
                    var _QTagID = tag.QTagID;
                    var _QuestionText = data.QuestionText;
                    var _DisplayTime = parseInt(data.DisplayTime);
                    var Option1Text = data.Option1Text;
                    var Option1Mark = parseInt(data.Option1Mark);
                    var IsOption1Correct = (data.Option1Correct.toLowerCase() == "true");
                    var Option2Text = data.Option2Text;
                    var Option2Mark = parseInt(data.Option2Mark);
                    var IsOption2Correct = (data.Option2Correct.toLowerCase() == "true");
                    var Option3Text = data.Option3Text;
                    var Option3Mark = parseInt(data.Option3Mark);
                    var IsOption3Correct = (data.Option3Correct.toLowerCase() == "true");
                    var Option4Text = data.Option4Text;
                    var Option4Mark = parseInt(data.Option4Mark);
                    var IsOption4Correct = (data.Option4Correct.toLowerCase() == "true");


                    if (_DisplayTime == null)
                        _DisplayTime = 60;
                    $http({
                        method: 'POST',
                        url: '/QuestionBank/SaveQuestion',
                        data: { QuestionText: _QuestionText, QTagID: _QTagID, DisplayTime: _DisplayTime }
                    }).then(function (result) {
                        ////// Save Options //////
                        $http({
                            method: 'POST',
                            url: '/QuestionBank/SaveOptions',
                            data: { OptionSeq: 1, OptionText: Option1Text, OptionMark: Option1Mark, IsOptionCorrect: IsOption1Correct }
                        }).then(function (result) {
                        });

                        $http({
                            method: 'POST',
                            url: '/QuestionBank/SaveOptions',
                            data: { OptionSeq: 2, OptionText: Option2Text, OptionMark: Option2Mark, IsOptionCorrect: IsOption2Correct }
                        }).then(function (result) {
                        });


                        $http({
                            method: 'POST',
                            url: '/QuestionBank/SaveOptions',
                            data: { OptionSeq: 3, OptionText: Option3Text, OptionMark: Option3Mark, IsOptionCorrect: IsOption3Correct }
                        }).then(function (result) {
                        });

                        $http({
                            method: 'POST',
                            url: '/QuestionBank/SaveOptions',
                            data: { OptionSeq: 4, OptionText: Option4Text, OptionMark: Option4Mark, IsOptionCorrect: IsOption4Correct }
                        }).then(function (result) {
                        });
                    });
                    return 1;
                }
            }
            return 0;
        }
        catch{
            return 0;
        }
        return 0
    }

    $scope.SaveQuestion = function () {
        debugger;
        var _QuestionText = $scope.QuestionText
        var _QTagID = $scope.QTagID
        var _DisplayTime = $scope.DisplayTime
        if (_DisplayTime == null)
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

    $scope.StartRRQ = function (RRQID) {
        $http({
            method: 'POST',
            url: '/QuestionBank/StartRRQ',
            data: { RRQID: RRQID }
        }).then(function (result) {
            window.location.pathname = '/SessionMgmt/ViewRRQ'
        });
    }

    $scope.RRQEnd = false;

    $scope.StopQuestion = function () {
        $scope.ws.send(JSON.stringify({
            profile: Constants.Profile['RRQ'],
            type: Constants.Events['MESSAGE'],
            action: Constants.Action['STOP'],
            RrqID: $scope.RRQID,
            qID: $scope.RRQQuestionList[$scope.currentPage].Question.QID,
            SessionID: $scope.SessionID
        }));
        display = document.querySelector('#time');
        CallstartTimer(0, display);
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
        window.clearInterval(prcntclock);
        window.clearInterval(timerclock);
        StartTimer();
    }

    $scope.NextQuestion = function () {
        $scope.currentPage = $scope.currentPage + 1
        $scope.DisplayOption = 'False';
    }

    $scope.EndRRQ = function () {
        $scope.RRQEnd = true;
        $scope.ws.send(JSON.stringify({
            profile: Constants.Profile['RRQ'],
            type: Constants.Events['MESSAGE'],
            action: Constants.Action['END'],
            RrqID: $scope.RRQID,
            qID: $scope.RRQQuestionList[$scope.currentPage].Question.QID,
            SessionID: $scope.SessionID
        }));
        display = document.querySelector('#time');
        window.clearInterval(timerclock);
        window.clearInterval(prcntclock);
        CallstartTimer(0, display);

        $http({
            method: 'POST',
            url: '/QuestionBank/EndRRQ',
            data: { RRQID: $scope.RRQID }
        });
    }

    $scope.DisplayDashboard = function() {
        window.location.href = "/RRQReport/RRQDashboard/" + $scope.RRQID + "?RRQID=" + $scope.RRQID;
    }

    $scope.currentPage = 0;
    $scope.pageSize = 5;
    $scope.numberOfPages = function () {
        return Math.ceil($scope.QuestionList.length / $scope.pageSize);
    }

    $scope.RRQpageSize = 1;
    $scope.RRQnumberOfPages = function () {
        return Math.ceil($scope.RRQQuestionList.length / $scope.RRQpageSize);
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


    function GetRRQAnswerPercentage() {
        console.log($scope.RRQQuestionList[$scope.currentPage].Question.QID)
        $http.get('/QuestionBank/GetRRQAnswerPercentage/' + $scope.RRQQuestionList[$scope.currentPage].Question.QID)
            .then(function (result) {
                display = document.querySelector('#prcnt');
                val = Math.ceil((result.data[0].Responses / result.data[0].AttendingStudents) * 100);
                display.textContent = val
            });
    }

    // RRQ Question timer
    var timerclock = 0; 
    var prcntclock = 0;

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
            if (minutes == 0 && seconds == 0)
                ClearTimer();

            if (diff <= 0) {
                // add one second so that the count down starts at the full duration
                // example 05:00 not 04:59
                start = Date.now() + 1000;
            }

        };
        function prcnt() {
            // call function to show how many students have answered 
            GetRRQAnswerPercentage()
        }
        // we don't want to wait a full second before the timer starts
        timer();
        timerclock = setInterval(timer, 1000);
        prcntclock = setInterval(prcnt, 3000);

        function ClearTimer() {
            window.clearInterval(timerclock);
            window.clearInterval(prcntclock);
        }
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

myapp.controller('QuestionBankCntrl', function ($scope, $http) {

    GetTags();


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
});

myapp.controller('EditQuestionCntrl', function ($scope, $http) {
    GetTags();

    SetParams(window.location.href);

    function GetTags() {
        $http.get('/QuestionBank/GetTags')
            .then(function (result) {
                $scope.TagsList = result.data;
                GetQuestionDetails($scope.QID);
            });
    }

    function SetParams(url) {
        try {
            var params = parseURLParams(url);
            if ('QID' in params) {
                $scope.QID = params['QID'][0];
            }
        }
        catch (e) {
            console.log(e);
        }
    }

    function GetQuestionDetails(QID) {
        $http.get('/QuestionBank/GetQuestionAndOptions/' + QID)
            .then(function (result) {
                $scope.Question = result.data;
                console.log($scope.Question)
                $scope.QuestionText = $scope.Question[0].Question.QuestionText;

                var QTagID = $scope.Question[0].Question.QTagID;
                for (var i = 0; i < $scope.TagsList.length; i++) {
                    if (QTagID == $scope.TagsList[i].QTagID) {
                        $scope.QTagID = QTagID;
                        $('#drpQuestionTags').val(QTagID);
                        break;
                    }
                }

                $scope.DisplayTime = $scope.Question[0].Question.QTime;

                var options = $scope.Question[0].Options;
                console.log(options)
                $scope.IsOption1Correct = options[0].IsCorrect;
                $scope.Option1Mark = options[0].Mark;
                $scope.Option1Text = options[0].OptionText;
                $scope.Option1ID = options[0].OptionID;

                $scope.IsOption2Correct = options[1].IsCorrect;
                $scope.Option2Mark = options[1].Mark;
                $scope.Option2Text = options[1].OptionText;
                $scope.Option2ID = options[1].OptionID;

                $scope.IsOption3Correct = options[2].IsCorrect;
                $scope.Option3Mark = options[2].Mark;
                $scope.Option3Text = options[2].OptionText;
                $scope.Option3ID = options[2].OptionID;

                $scope.IsOption4Correct = options[3].IsCorrect;
                $scope.Option4Mark = options[3].Mark;
                $scope.Option4Text = options[3].OptionText;
                $scope.Option4ID = options[3].OptionID;

            });
    }

    $scope.SaveQuestion = function () {
        debugger;
        var _QuestionText = $scope.QuestionText
        var _QTagID = $scope.QTagID
        var _DisplayTime = $scope.DisplayTime
        if (_DisplayTime == null)
            _DisplayTime = 20;
        $http({
            method: 'POST',
            url: '/QuestionBank/SaveQuestion',
            data: { QuestionText: _QuestionText, QTagID: _QTagID, DisplayTime: _DisplayTime }
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

    $scope.EditQuestion = function () {
        debugger;
        var _QuestionText = $scope.QuestionText
        var _QTagID = $scope.QTagID
        var _DisplayTime = $scope.DisplayTime
        if (_DisplayTime == null)
            _DisplayTime = 20;
        var _QID = $scope.QID;
        $http({
            method: 'POST',
            url: '/QuestionBank/EditQuestion',
            data: { QuestionText: _QuestionText, QTagID: _QTagID, DisplayTime: _DisplayTime, QID: _QID }
        }).then(function (result) {

            // 

            ////// Edit Options //////
            $http({
                method: 'POST',
                url: '/QuestionBank/EditOptions',
                data: { OptionSeq: 1, OptionText: $scope.Option1Text, OptionMark: $scope.Option1Mark, IsOptionCorrect: $scope.IsOption1Correct, OptionID: $scope.Option1ID }
            }).then(function (result) {
            });

            $http({
                method: 'POST',
                url: '/QuestionBank/EditOptions',
                data: { OptionSeq: 2, OptionText: $scope.Option2Text, OptionMark: $scope.Option2Mark, IsOptionCorrect: $scope.IsOption2Correct, OptionID: $scope.Option2ID }
            }).then(function (result) {
            });


            $http({
                method: 'POST',
                url: '/QuestionBank/EditOptions',
                data: { OptionSeq: 3, OptionText: $scope.Option3Text, OptionMark: $scope.Option3Mark, IsOptionCorrect: $scope.IsOption3Correct, OptionID: $scope.Option3ID }
            }).then(function (result) {
            });

            $http({
                method: 'POST',
                url: '/QuestionBank/EditOptions',
                data: { OptionSeq: 4, OptionText: $scope.Option4Text, OptionMark: $scope.Option4Mark, IsOptionCorrect: $scope.IsOption4Correct, OptionID: $scope.Option4ID }
            }).then(function (result) {
            });

        });
        alert('Saved Successfully!!');
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

myapp.controller('TagCntrl', function ($scope, $http) {
    GetTags();

    function GetTags() {
        $http.get('/QuestionBank/GetTags')
            .then(function (result) {
                $scope.TagsList = result.data;
            });
    }

    $scope.DeleteTag = function (QTagID) {
        var ask = confirm("Are you sure you wish to delete Tag? You will not be able to sort questions using this tag again!");
        if (ask)
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

    $scope.EditTag = function () {
        var _TagText = $scope.UpdateTagText;
        var _QTagID = $scope.QTagID;
        if (_QTagID !== null)
            $http({
                method: 'POST',
                url: '/QuestionBank/EditTag',
                data: { QTagID: _QTagID, TagText: _TagText }
            }).then(function (result) {
                GetTags()
            });
    }
});