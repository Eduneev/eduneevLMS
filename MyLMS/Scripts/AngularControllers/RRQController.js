myapp.controller('RRQCntrl', function ($scope, $http) {

    GetSessionList();

    function GetSessionList() {
        $http.get('/SessionMgmt/GetSessions')
            .then(function (result) {
                $scope.SessionsList = result.data;
                $scope.RRQTextToShow = "Select..";
            });
    }

    $scope.GetSessionRRQ = function () {
        var _SessionName = $scope.SessionName;
        console.log(_SessionName)

        if (_SessionName === null) {
            $("#rrq_search").val("").trigger('input');
            //GetSessionList();
        }
        else
            $("#rrq_search").val(_SessionName).trigger('input');
            
    };

    $scope.CreateNewRRQ = function () {
        window.location.href = "/SessionMgmt/NewRRQ/";
    };

    $scope.GetRRQ = function () {
        $http.get('/SessionMgmt/GetRRQList')
            .then(function (result) {
                $scope.RRQList = result.data;
            });
    }

    $scope.CreateRRQ = function () {

        $http({
            method: 'POST',
            url: '/SessionMgmt/CreateRRQ',
            data: { RRQNo: _RRQNo }
        }).then(function (result) {
            $http.get('/SessionMgmt/GetRRQList')
                .then(function (result) {
                    $scope.RRQList = result.data;
                });
        });
    }

    $scope.DeleteRRQ = function (RRQId) {
        $http({
            method: 'POST',
            url: '/SessionMgmt/DeleteRRQ',
            data: { RRQ_ID: RRQId }
        }).then(function (result) {
            $http.get('/SessionMgmt/GetRRQList')
                .then(function (result) {
                    $scope.RRQList = result.data;
                });
        });
    }

});

myapp.controller('NewRRQCntrl', function ($scope, $http) {
    function StartSessionSocket() {
        console.log("Starting Socket session");
    }

    $scope.CreateNewRRQ = function () {
        window.location.href = "/SessionMgmt/CreateRRQ/";
    }

});

myapp.controller('EditRRQCntrl', function ($scope, $http) {
    $scope.RRQQuestionList = '';
    SetParams(window.location.href);

    $scope.currentPage = 0;
    $scope.RRQpageSize = 5;
    $scope.RRQnumberOfPages = function () {
        return Math.ceil($scope.RRQQuestionList.length / $scope.RRQpageSize);
    }

    function SetParams(url) {
        try {
            var params = parseURLParams(url);
            if ('RRQ_ID' in params) {
                $scope.RRQID = params['RRQ_ID'][0];
            }
        }
        catch (e) {
            console.log(e);
        }
    }

    $scope.AddQuestion = function () {
        window.location.href = "/SessionMgmt/AddQuestionsToRRQ/" + $scope.RRQID;
    }

    // Get RRQ Questions
    $scope.GetRRQQuestionDetails = function () {
        $http.get('/QuestionBank/GetRRQQuestions')
            .then(function (result) {
                $scope.RRQQuestionList = result.data;
                console.log($scope.RRQQuestionList)
            });
    }

    // Remove Question From RRQ
    $scope.RemoveQuestionFromRRQ = function (QID) {
        $http({
            method: 'POST',
            url: '/QuestionBank/AddRemoveQuestionToRRQ',
            data: { QID: QID }
        }).then(function (result) {
            $scope.GetRRQQuestionDetails();
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