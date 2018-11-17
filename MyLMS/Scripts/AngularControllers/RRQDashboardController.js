myapp.controller('RRQDashboardCntrl', function ($scope, $http) {
    GetRRQQuestions();
    GetDegreeOfDifficulty();
    GetTop10Students();

    $scope.LabelsArray = [];
    $scope.RRQ_ID = 1;
    var Labels = [];
    var Questions = [];

    function GetRRQQuestions() {
        $http.get('/SessionMgmt/GetRRQQuestions/')
        .then(function (result) {
            $scope.RRQQuestions = result.data;
        });
    }

    ///////// Get Questions ////////
    $scope.GetQuestionAndOptions = function (QID) {
        $scope.QID = QID;

        $http.get('/QuestionBank/GetQuestionAndOptions/' + QID)
        .then(function (result) {
            $scope.QuestionList = result.data;
            });

        GetDashboardData($scope.QID);
        GetDashboardOptionGraph($scope.QID);
    }

    ///////// Get Top 10 Students ////////
    function GetTop10Students() {
        $http.get('/SessionMgmt/GetTop10Students/')
        .then(function (result) {
            $scope.Top10Students = result.data;
        });
    }

    function GetDegreeOfDifficulty() {
        $http.get('/SessionMgmt/GetDegreeOfDifficulty/')
            .then(function (result) {
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
        angular.forEach($scope.GraphData, function (value, key) {
            Labels.push('Q-' + value.QUES_NO.toString());
            Questions.push(value.PERCENTAGE);
        })
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
    ///////// Get Top 10 Students ////////
    function GetDashboardData(QID) {
        $http.get('/SessionMgmt/GetDashboardData/' + QID)
            .then(function (result) {
                $scope.DashboardData = result.data;
                $scope.ResponsesPrcnt = $scope.DashboardData[0].ResponsesPrcnt
                $scope.NoResponsesPrcnt = $scope.DashboardData[0].NoResponsesPrcnt
                $scope.CorrectPrcnt = $scope.DashboardData[0].CorrectPrcnt
                $scope.InCorrestPrcnt = $scope.DashboardData[0].InCorrestPrcnt
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
            });
    }
});