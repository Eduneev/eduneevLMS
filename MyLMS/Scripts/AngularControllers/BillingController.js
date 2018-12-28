myapp.controller('BillingCntrl', function ($scope, $http) {
    GetCenters();
    GetEntityList();
    $scope.CenterID = 0;
    $scope.EntityID = 0;
    $scope.ClassRoomID = 0;
    $scope.StartDate = 0;
    $scope.EndDate = 0;

    $scope.EntityTextToShow = 'Please select..';
    
    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
            .then(function (result) {
                $scope.EntityList = result.data;
                console.log(result.data);
            });
    }

    function GetCenters() {
        $http.get('/SessionMgmt/GetCentersForEntity')
            .then(function (result) {
                $scope.CentersList = result.data;
                $scope.ClassroomTextToShow = 'Please select..';
            });
    }

    $scope.GetCentersForEntity = function () {
        $http.get('/SessionMgmt/GetCentersForSelectedEntity/' + $scope.EntityID)
            .then(function (result) {
                $scope.CentersList = result.data;
                $scope.CenterTextToShow = 'Please select..';
            });
    }

    $scope.GetClassrooms = function() {
        $http.get('/CenterMgmt/GetClassroomsForCenter/' + $scope.CenterID)
            .then(function (result) {
                $scope.ClassroomList = result.data;
                $scope.ClassroomTextToShow = 'Please select..';
            });
    }

    $scope.GetStreamList = function () {
        dates = GetDate();
        $http.get('/Organisation/GetStreamLogsForClassroom/' + $scope.ClassRoomID + "/" + dates[0] + "/" + dates[1])
            .then(function (result) {
                $scope.StreamList = result.data;
            });
    }

    function GetDate() {
        startDate = $scope.StartDate;
        day = startDate.getUTCDate(); if (day < 10) { day = "0" + day; }
        month = startDate.getMonth() + 1; if (month < 10) { month = "0" + month;}
        s = startDate.getFullYear() + "-" + month + "-" + day;
        endDate = $scope.EndDate;
        day = endDate.getUTCDate(); if (day < 10) { day = "0" + day; }
        month = endDate.getMonth() + 1; if (month < 10) { month = "0" + month; }
        t = endDate.getFullYear() + "-" + month + "-" + day;

        return [s, t];
    }
});