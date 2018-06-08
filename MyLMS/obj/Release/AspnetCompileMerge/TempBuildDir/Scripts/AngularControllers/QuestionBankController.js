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

    $scope.currentPage = 0;
    $scope.pageSize = 5;
    $scope.numberOfPages = function () {
        return Math.ceil($scope.QuestionList.length / $scope.pageSize);
    }

    $scope.RRQpageSize = 1;
    $scope.RRQnumberOfPages = function () {
        return Math.ceil($scope.QuestionList.length / $scope.RRQpageSize);
    }
       
});

myapp.filter('startFrom', function () {
    return function (input, start) {
        start = +start; //parse to int
        return input.slice(start);
    }
});