myapp.controller('QuestionCntrl', function ($scope, $http) {

    $scope.SaveQuestion = function () {
        debugger;
        var _QuestionText = $scope.QuestionText
        $http({
            method: 'POST',
            url: '/QuestionBank/SaveQuestion',
            data: { QuestionText: _QuestionText}
        }).then(function (result) {

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
            /////////////

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

    $scope.StartRRQ = function () {
        window.location.pathname = '/SessionMgmt/ViewRRQ'
    }

    $scope.DisplayOptions = function () {
        $scope.DisplayOption = 'True';
        StartTimer();
    }

    $scope.NextQuestion = function () {
        $scope.currentPage = $scope.currentPage + 1
        $scope.DisplayOption = 'False';
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
        var fiveMinutes = 60 * 5,
            display = document.querySelector('#time');
        CallstartTimer(fiveMinutes, display);
    };
       
});

myapp.filter('startFrom', function () {
    return function (input, start) {
        start = +start; //parse to int
        return input.slice(start);
    }
});