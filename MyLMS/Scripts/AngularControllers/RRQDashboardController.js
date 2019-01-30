myapp.controller('RRQDashboardCntrl', function ($scope, $http) {

    GetRRQID(window.location.href);

    $scope.LabelsArray = [];
    $scope.RRQ_ID;

    var Labels = [];
    var Questions = [];
    $scope.RevealAnswer = false;

    function GetRRQID(url) {
        var params = parseURLParams(url);
        console.log(params)
        if ("RRQID" in params) {
            $scope.RRQ_ID = params["RRQID"][0];

            $http({
                method: 'POST',
                url: '/SessionMgmt/SetSessionRRQ',
                data: { rrqID: $scope.RRQ_ID }
            })

            GetRRQQuestions();
            GetTop10Students();
        }
    }

    function GetRRQQuestions() {
        $http.get('/SessionMgmt/GetRRQQuestions/' + $scope.RRQ_ID)
            .then(function (result) {
            GetDegreeOfDifficulty();
            $scope.RRQQuestions = result.data;
            document.getElementById('RRQName').textContent = result.data[0].RRQName;
        });
    }

    // Get RRQ Questions
    $scope.GetRRQQuestionAndOptions = function (QID) {
        $scope.QID = QID;

        $http.get('/QuestionBank/GetRRQQuestionAndOptions/' + QID)
            .then(function (result) {
                console.log(result.data);
            $scope.QuestionList = result.data;
            });

        GetDashboardData($scope.QID);
        GetDashboardOptionGraph($scope.QID);
        //GetTop10FastestStudents($scope.QID);
    }

    // Get Top 10 Students
    function GetTop10Students() {
        $http.get('/SessionMgmt/GetTop10Students/' + $scope.RRQ_ID)
        .then(function (result) {
            $scope.Top10Students = result.data;
        });
    }

    // Get Top 10 Fastest Correct Students
    function GetTop10FastestStudents(QID) {
        $http.get('/SessionMgmt/GetTop10FastestStudents/' + $scope.QID)
            .then(function (result) {
                $scope.Top10FastestStudents = result.data;
            });
    }

    function GetDegreeOfDifficulty() {
        
        $http.get('/SessionMgmt/GetDegreeOfDifficulty/' + $scope.RRQ_ID)
            .then(function (result) {
                console.log(result.data)
                $scope.GraphData = result.data;
                PrepareGraphData();

                var ctx = document.getElementById("canvas").getContext("2d");
                var chart = new Chart(ctx).HorizontalBar(barChartData, {
                    responsive: true,
                    barShowStroke: false
                });
            });
            
    }
    function PrepareGraphData() {
        angular.forEach($scope.RRQQuestions, function (val, key) {
            Labels.push('Q-' + val.QUES_NO.toString());
            var check = false;
            angular.forEach($scope.GraphData, function (value, key) {
                if (value.QUES_NO.toString() == val.QUES_NO.toString()) {
                    check = true;
                    Questions.push(Math.round((1 - (value.TOTAL_CorrectResp / value.TOTAL_RESP)) * 100));
                }
            });
            if (!check)
                Questions.push(100);

        });

        /*angular.forEach($scope.GraphData, function (value, key) {
            Labels.push('Q-' + value.QUES_NO.toString());
            Questions.push(value.PERCENTAGE);
        })*/
        //console.log(JSON.stringify(Labels));
        //console.log(JSON.stringify(Questions));
    }

    var randomScalingFactor = function () {
        return Math.round(Math.random() * 100);
    };
    var barChartData = {
        labels: Labels,
        datasets: [
            {
                fillColor: "rgba(0,128,128,0.5)",
                strokeColor: "rgba(220,220,220,0.8)",
                highlightFill: "rgba(220,220,220,0.75)",
                highlightStroke: "rgba(220,220,220,1)",
                data: Questions
            }
        ]
    };

    $scope.GetStudentsByResponse = function (OptionSeq) {
        $http.get('/SessionMgmt/GetStudentsByResponse/' + $scope.QID + '?OptionSeq=' + OptionSeq)
            .then(function (result) {
                $scope.StudentsListByResp = result.data;
            });
    }
    $scope.MarkAnswer = function () {
        $scope.RevealAnswer = true;
    }
    ///////// Get Top 10 Students ////////
    function GetDashboardData(QID) {
        console.log(QID);
        console.log($scope.RRQ_ID);
        $http.get('/SessionMgmt/GetDashboardData/' + QID)
            .then(function (result) {
                console.log(result.data)
                $scope.DashboardData = result.data;
                $scope.ResponsesPrcnt = $scope.DashboardData[0].ResponsesPrcnt
                $scope.NoResponsesPrcnt = $scope.DashboardData[0].NoResponsesPrcnt
                $scope.CorrectPrcnt = $scope.DashboardData[0].CorrectPrcnt
                $scope.InCorrestPrcnt = $scope.DashboardData[0].InCorrestPrcnt
                $scope.RevealAnswer = false;
                $scope.CorrectOption = $scope.DashboardData[0].CorrectOption
            });
    }
    function GetDashboardOptionGraph(QID) {
        $http.get('/SessionMgmt/GetDashboardOptionGraph/' + QID)
            .then(function (result) {
                $scope.DashboardGraphData = result.data;
                $scope.OPTION_1_PRCNT = $scope.DashboardGraphData[0].OPTION_1_PRCNT
                $scope.OPTION_2_PRCNT = $scope.DashboardGraphData[0].OPTION_2_PRCNT
                $scope.OPTION_3_PRCNT = $scope.DashboardGraphData[0].OPTION_3_PRCNT
                $scope.OPTION_4_PRCNT = $scope.DashboardGraphData[0].OPTION_4_PRCNT

                /*
                var total = OPTION_1_PRCNT + OPTION_2_PRCNT + OPTION_3_PRCNT + OPTION_4_PRCNT;
                $scope.OPTION_1_PRCNT = Math.round((OPTION_1_PRCNT/total)*100)
                $scope.OPTION_2_PRCNT = Math.round((OPTION_2_PRCNT / total) * 100)
                $scope.OPTION_3_PRCNT = Math.round((OPTION_3_PRCNT / total) * 100)
                $scope.OPTION_4_PRCNT = Math.round((OPTION_4_PRCNT / total) * 100)                
                */

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