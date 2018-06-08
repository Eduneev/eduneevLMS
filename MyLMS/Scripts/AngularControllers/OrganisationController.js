myapp.controller('OrganisationCntrl', function ($scope, $http) {
    GetEntityList();
    $scope.SaveEntity = function () {
        debugger;
        var _EntityName = $scope.EntityName
        var _EntityCode = $scope.EntityCode;
        var _ManagerName = $scope.ManagerName;
        var _Email = $scope.Email;
        var _Mobile = $scope.Mobile;
        var _Landline = $scope.Landline;
        var _Address = $scope.Address;
        
        $http({
            method: 'POST',
            url: '/Organisation/CreateEntity',
            data: { EntityName: _EntityName, EntityCode: _EntityCode, ManagerName: _ManagerName, Email: _Email, Mobile: _Mobile, Landline: _Landline, Address: _Address }
        }).then(function (result) {
            GetEntityList();
        });
    }

    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
        .then(function (result) {
            $scope.EntityList = result.data;
        });
    }

    $scope.OpenViewEntityList = function () {
        window.location.href = "/Organisation/Entity/";
    }
});

myapp.controller('EntityUserCntrl', function ($scope, $http) {
    GetEntityList();
    $scope.EntityTextToShow = "Please wait.."
    $scope.RoleTextToShow = "Please wait.."


    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
        .then(function (result) {
            $scope.EntityList = result.data;
            $scope.EntityTextToShow = "Please select entity.."
        });
    }

    $scope.GetEntityUsers = function GetEntityUsersList() {
        $http.get('/Organisation/GetEntityUserList/' + $scope.EntityID)
        .then(function (result) {
            $scope.EntityUserList = result.data;
            GetRoles();
        });
    }

    function GetRoles() {
        $http.get('/Organisation/GetEntityAdminRole')
        .then(function (result) {
            $scope.EntityAdminUsers = result.data;
            $scope.RoleTextToShow = "Please select.."
        });
    }

    $scope.AddEntityUser = function () {
        debugger;
        var _EntityID = $scope.EntityID
        var _UserName = $scope.UserName
        var _Password = $scope.Password;
        var _FullName = $scope.FullName;
        var _EmailID = $scope.EmailID;
        var _Mobile = $scope.Mobile;
        var _RoleID = $scope.RoleID;

        $http({
            method: 'POST',
            url: '/Organisation/AddEntityUser',
            data: {EntityID: _EntityID, UserName: _UserName, Password: _Password, FullName: _FullName, EmailID: _EmailID, Mobile: _Mobile, RoleID: _RoleID }
        }).then(function (result) {
            $http.get('/Organisation/GetEntityUserList/' + $scope.EntityID)
            .then(function (result) {
                $scope.EntityUserList = result.data;
                GetRoles();
            });
        });
    }
});

myapp.controller('ReceiverCntrl', function ($scope, $http) {
    GetEntityList();

    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
        .then(function (result) {
            $scope.EntityList = result.data;
            $scope.EntityTextToShow = "Please select entity.."
        });
    }

    $scope.GetReceiversList = function () {
        $http.get('/Organisation/GetReceiversList/' + $scope.EntityID)
        .then(function (result) {
            $scope.ReceiversList = result.data;
        });
    }

    $scope.SaveReceiver = function () {
        debugger;
        var _ReceiverSerialNo = $scope.ReceiverSerialNo
        var _ReceiverModel = $scope.ReceiverModel
        var _EntityID = $scope.SelectedEntityID

        $http({
            method: 'POST',
            url: '/Organisation/SaveShippedReceiver',
            data: { ReceiverSerialNo: _ReceiverSerialNo, ReceiverModel: _ReceiverModel, EntityID: _EntityID }
        }).then(function (result) {
            $http.get('/Organisation/GetReceiversList/' + $scope.EntityID)
               .then(function (result) {
                   $scope.ReceiversList = result.data;
               });
        });
    }
});

myapp.controller('RemoteCntrl', function ($scope, $http) {
    GetEntityList();

    function GetEntityList() {
        $http.get('/Organisation/GetEntityList')
        .then(function (result) {
            $scope.EntityList = result.data;
            $scope.EntityTextToShow = "Please select entity.."
        });
    }

    $scope.GetRemotesList = function () {
        $http.get('/Organisation/GetRemotesList/' + $scope.EntityID)
        .then(function (result) {
            $scope.RemotesList = result.data;
        });
    }

    $scope.SaveRemote = function () {
        debugger;
        var _RemoteSerialNo = $scope.RemoteSerialNo
        var _RemoteModel = $scope.RemoteModel
        var _EntityID = $scope.SelectedEntityID

        $http({
            method: 'POST',
            url: '/Organisation/SaveShippedRemote',
            data: { RemoteSerialNo: _RemoteSerialNo, RemoteModel: _RemoteModel, EntityID: _EntityID }
        }).then(function (result) {
            $http.get('/Organisation/GetRemotesList/' + $scope.EntityID)
               .then(function (result) {
                   $scope.RemotesList = result.data;
               });
        });
    }
});