myapp.controller('CenterCntrl', function ($scope, $http) {
    GetCentersList();
    GetAccountID();
    $scope.CenterID = 0;
    $scope.SaveCenter = function () {
        debugger;
        var _CenterName = $scope.CenterName
        var _CenterCode = $scope.CenterCode
        var _Email = $scope.Email
        var _Landline1 = $scope.Landline1
        var _Landline2 = $scope.Landline2
        var _Mobile = $scope.Mobile
        var _Address = $scope.Address
        var _PinCode = $scope.PinCode

        $http({
            method: 'POST',
            url: '/CenterMgmt/SaveCenter',
            data: { CenterName: _CenterName, CenterCode: _CenterCode, Email: _Email, Landline1: _Landline1, Landline2: _Landline2, Mobile: _Mobile, Address: _Address, PinCode: _PinCode }
        }).then(function (result) {
            alert('Saved Successfully!!');
        });
    }

    function GetCentersList() {
        $http.get('/CenterMgmt/GetCenters')
        .then(function (result) {
            $scope.CentersList = result.data;
        });
    }

     function GetAccountID() {
        $http.get('/CenterMgmt/GetAccountID')
        .then(function (result) {
            $scope.AccountID = result.data;
        });
    }

    $scope.SelectedCenter = function (SelectedCenterID) {
        $scope.CenterID = SelectedCenterID;
        GetClassrooms();
    }

    function GetClassrooms() {
        $http.get('/CenterMgmt/GetClassroomsForCenter/' + $scope.CenterID)
            .then(function (result) {
                $scope.ClassroomList = result.data;
            });
    }

    $scope.SaveClassroom = function () {
        debugger;

        $http({
            method: 'POST',
            url: '/CenterMgmt/SaveClassroom',
            data: { ClassRoomName: $scope.ClassroomName, CenterID: $scope.CenterID, SittingCapacity: $scope.SittingCapacity }
        }).then(function (result) {
            alert('Saved Successfully!!');
        });
    }
});

myapp.controller('OrgCntrl', function ($scope, $http) {
    GetEntityList();
    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
            .then(function (result) {
                $scope.EntityList = result.data;
                $scope.EntityTextToShow = 'Please select..';
            });
    }

    $scope.GetCentersForEntity = function () {
        $http.get('/SessionMgmt/GetCentersForSelectedEntity/' + $scope.EntityID)
            .then(function (result) {
                $scope.CentersList = result.data;
                $scope.CenterTextToShow = 'Please select..';
            });
    }

    $scope.SelectedCenter = function (SelectedCenterID) {
        $scope.CenterID = SelectedCenterID;
        GetClassrooms();
    }

    function GetClassrooms() {
        $http.get('/CenterMgmt/GetClassroomsForCenter/' + $scope.CenterID)
            .then(function (result) {
                $scope.ClassroomList = result.data;
            });
    }
});

myapp.controller('AllocateReceiverCntrl', function ($scope, $http) {
    GetClassroomList();
    $scope.ReceiverSerialNo = null;

    function GetClassroomList() {
        $http.get('/CenterMgmt/GetClassroomReceiver')
            .then(function (result) {
                $scope.ClassroomList = result.data;
            });
    }

    $scope.AssignReceiver = function (ClassroomID, ReceiverSerialNo) {
        var _ClassRoomID = ClassroomID;
        var _ReceiverSerialNo = ReceiverSerialNo;

        $http({
            method: 'POST',
            url: '/CenterMgmt/AssignReceiver',
            data: { ClassRoomID: _ClassRoomID, ReceiverSerialNo: _ReceiverSerialNo }
        }).then(function (result) {
            GetClassroomList();
        });
    };
});