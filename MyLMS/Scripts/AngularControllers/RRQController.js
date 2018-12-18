myapp.controller('RRQCntrl', function ($scope, $http) {

    $scope.CreateNewRRQ = function () {
        window.location.href = "/SessionMgmt/NewRRQ/";
    }

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

});

myapp.controller('NewRRQCntrl', function ($scope, $http) {
    function StartSessionSocket() {
        console.log("Starting Socket session");
    }

    $scope.CreateNewRRQ = function () {
        window.location.href = "/SessionMgmt/CreateRRQ/";
    }

});