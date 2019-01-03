myapp.controller('BillingCntrl', function ($scope, $http) {
    GetCenters();
    GetEntityList();
    $scope.CenterID = 0;
    $scope.EntityID = 0;
    $scope.ClassRoomID = 0;
    $scope.StartDate = 0;
    $scope.EndDate = 0;
    $scope.Allowed = 0;

    $scope.EntityTextToShow = 'Please select..';
    
    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
            .then(function (result) {
                $scope.EntityList = result.data;
                console.log(result.data);
                $scope.Allowed = 0;
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
        $http.get('/BillingMgmt/GetStreamLogsForClassroom/' + $scope.ClassRoomID + "/" + dates[0] + "/" + dates[1])
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

myapp.controller("BillingAgreementCntrl", function ($scope, $http) {
    $scope.Allowed = 0;
    $scope.EntityTextToShow = "Please select Entity..";

    GetEntityList();
    GetBillingDetails();

    function GetBillingDetails() {
        $http.get('/BillingMgmt/GetBillingDetails')
            .then(function (result) {
                $scope.BillingList = result.data;
            });
    }

    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
            .then(function (result) {
                $scope.EntityList = result.data;
                console.log(result.data);
                $scope.Allowed = 0;
            });
    }

    $scope.GetBillingTypes = function () {
        $http.get('/BillingMgmt/GetBillingTypes')
            .then(function (result) {
                $scope.BillingTypeList = result.data;
                $scope.BillingTextToShow = 'Please select Billing Type..';
            });
    }

    $scope.GetStreamTypes = function () {
        $http.get('/BillingMgmt/GetStreamTypes')
            .then(function (result) {
                $scope.StreamTypeList = result.data;
                $scope.StreamTextToShow = "Please select Stream Type..";
                $scope.Allowed = 1;
            });
    }


    $scope.SetBilling = function (Cost) {
        var _EntityID = $scope.EntityID;
        var _BillingTypeID = $scope.BillingTypeID;
        var _StreamTypeID = $scope.StreamTypeID;
        var _Cost = Cost;

        $http({
            method: 'POST',
            url: '/BillingMgmt/CreateEntityBilling',
            data: { EntityID: _EntityID, BillingTypeID: _BillingTypeID, StreamTypeID:_StreamTypeID, Cost: _Cost }
        }).then(function (result) {
            alert("Saved Successfully");
            GetBillingDetails();
        });
    };

});