myapp.controller('EntityCntrl', function ($scope, $http) {
    
});

myapp.controller('EquipmentCntrl', function ($scope, $http) {
    GetReceiversList();
    GetRemotesList();
   function GetReceiversList() {
        $http.get('/Entity/GetReceiversList')
        .then(function (result) {
            $scope.ReceiversList = result.data;
        });
   }

   function GetRemotesList() {
       $http.get('/Entity/GetRemotesList')
       .then(function (result) {
           $scope.RemotesList = result.data;
       });
   }

   $scope.ReceiveEquipment = function (ReceiverID) {
       debugger;
       var _ReceiverID = ReceiverID

       $http({
           method: 'POST',
           url: '/Entity/ReceiveEquipment',
           data: { ReceiverID: _ReceiverID }
       }).then(function (result) {
           GetReceiversList();
       });
   }

   $scope.RemoteReceive = function (RemoteID) {
       debugger;
       var _RemoteID = RemoteID

       $http({
           method: 'POST',
           url: '/Entity/ReceiveRemote',
           data: { RemoteID: _RemoteID }
       }).then(function (result) {
           GetRemotesList();
       });
   }
});

myapp.controller('InventoryCntrl', function ($scope, $http) {

    GetInventoryReceivers();
    GetInventoryRemotes();

    function GetInventoryReceivers() {
        $http.get('/Entity/GetInvRecE')
        .then(function (result) {
            $scope.InvRecList = result.data;
        });
    }

    function GetInventoryRemotes() {
        $http.get('/Entity/GetInvRemE')
        .then(function (result) {
            $scope.InvRemList = result.data;
        });
    }

});

myapp.controller('StudioCntrl', function ($scope, $http) {
    GetStudios();

    function GetStudios() {
        $http.get('/Entity/GetStudios')
        .then(function (result) {
            $scope.StudioList = result.data;
        });
    }

    $scope.SaveStudio = function () {
        debugger;
        var _StudioName = $scope.StudioName;
        var _StudioLocation = $scope.StudioLocation;
        var _Remarks = $scope.Remarks;
        var _Channel = $scope.ChannelName;
        $http({
            method: 'POST',
            url: '/Entity/SaveStudio',
            data: { StudioName: _StudioName, StudioLocation: _StudioLocation, Remarks: _Remarks, ChannelName: _Channel }
        }).then(function (result) {
            GetStudios();
        });
    }
});

myapp.controller('CenterUserCntrl', function ($scope, $http) {
    GetCenterList();
    $scope.CenterTextToShow = "Please wait.."
    $scope.RoleTextToShow = "Please wait.."


    function GetCenterList() {
        $http.get('/CenterMgmt/GetCenters')
            .then(function (result) {
                $scope.CenterList = result.data;
                $scope.CenterTextToShow = "Please select entity.."
            });
    }

    $scope.GetCenterUsers = function GetCenterUsersList() {
        $http.get('/Entity/GetCenterUserList/' + $scope.CenterID)
            .then(function (result) {
                $scope.CenterUserList = result.data;
                GetRoles();
            });
    }

    function GetRoles() {
        $http.get('/Entity/GetCenterCoordinatorRole')
            .then(function (result) {
                $scope.CenterCoordinatorUsers = result.data;
                $scope.RoleTextToShow = "Please select.."
            });
    }

    $scope.AddCenterUser = function () {
        debugger;
        var _CenterID = $scope.CenterID
        var _UserName = $scope.UserName
        var _Password = $scope.Password;
        var _FullName = $scope.FullName;
        var _EmailID = $scope.EmailID;
        var _Mobile = $scope.Mobile;
        var _RoleID = $scope.RoleID;

        $http({
            method: 'POST',
            url: '/Entity/AddCenterUser',
            data: { CenterID: _CenterID, UserName: _UserName, Password: _Password, FullName: _FullName, EmailID: _EmailID, Mobile: _Mobile, RoleID: _RoleID }
        }).then(function (result) {
            $http.get('/Entity/GetCenterUserList/' + $scope.CenterID)
                .then(function (result) {
                    $scope.CenterUserList = result.data;
                    GetRoles();
                });
        });
    }
});
