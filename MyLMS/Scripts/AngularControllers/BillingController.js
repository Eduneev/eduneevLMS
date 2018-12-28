myapp.controller('BillingCntrl', function ($scope, $http) {
    GetCenters();
    GetEntities();
    $scope.CenterID = 0;
    $scope.EntityID = 0;
    $scope.ClassroomID = 0;
    
    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
            .then(function (result) {
                $scope.EntityList = result.data;
            });
    }

    function GetCenters() {
        $http.get('/SessionMgmt/GetCentersForEntity')
            .then(function (result) {
                $scope.CentersList = result.data;
            });
    }

    function GetCentersForEntity() {
        $http.get('/SessionMgmt/GetCentersForSelectedEntity/' + $scope.EntityID)
            .then(function (result) {
                $scope.CentersList = result.data;
            });
    }

    function GetClassrooms() {
        $http.get('/CenterMgmt/GetClassroomsForCenter/' + $scope.CenterID)
            .then(function (result) {
                $scope.ClassroomList = result.data;
            });
    }
});