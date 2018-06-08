myapp.controller('RRQDashboardCntrl', function ($scope, $http) {
    GetRRQQuestions();
    GetTop10Students();
    function GetRRQQuestions() {
        debugger;
        $http.get('/SessionMgmt/GetRRQQuestions/')
        .then(function (result) {
            $scope.RRQQuestions = result.data;
        });
    }

    ///////// Get Questions ////////
    $scope.GetQuestionAndOptions = function (QID) {
        $http.get('/QuestionBank/GetQuestionAndOptions/' + QID)
        .then(function (result) {
            $scope.QuestionList = result.data;
        });
    }

    ///////// Get Top 10 Students ////////
    function GetTop10Students() {
        $http.get('/SessionMgmt/GetTop10Students/')
        .then(function (result) {
            $scope.Top10Students = result.data;
        });
    }

});