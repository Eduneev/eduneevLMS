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
        $http({
            method: 'POST',
            url: '/Entity/SaveStudio',
            data: { StudioName: _StudioName, StudioLocation: _StudioLocation, Remarks: _Remarks }
        }).then(function (result) {
            GetStudios();
        });
    }
});