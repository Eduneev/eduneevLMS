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

    $scope.GetEntityDetails = function () {
        var _UserID = $scope.EntityID;
        for (var i = 0; i < $scope.EntityList.length; i++) {
            e = $scope.EntityList[i];
            console.log(e.EntityID);
            if (e.EntityID == _UserID) {
                $scope.EntityName = e.EntityName;
                $scope.EntityCode = e.EntityCode;
                $scope.ManagerName = e.ManagerName;
                $scope.Email = e.Email;
                $scope.Mobile = e.Mobile;
                $scope.Landline = e.Landline;
                $scope.Address = e.Address;
            }
        }
    }

    $scope.EditEntity = function () {
        var _EntityID = $scope.EntityID;
        var _EntityName = $scope.Email;
        var _EntityCode = $scope.EntityCode;
        var _ManagerName = $scope.ManagerName;
        var _Email = $scope.Email;
        var _Mobile = $scope.Mobile;
        var _Landline = $scope.Landline;
        var _Address = $scope.Address;

        $http({
            method: 'POST',
            url: '/Organisation/EditEntity',
            data: { EntityID: _EntityID, EntityName: _EntityName, EntityCode: _EntityCode, ManagerName: _ManagerName, Email: _Email, Mobile: _Mobile, Landline: _Landline, Address: _Address }
        }).then(function (result) {
            alert("Updated!");
            GetEntityList();
        });
    }
});

myapp.controller('OrgUserCntrl', function ($scope, $http) {
    GetOrgList();
    $scope.EntityTextToShow = "Please wait.."
    $scope.RoleTextToShow = "Please wait.."


    function GetOrgList() {
        $http.get('/Organisation/GetOrgUserList')
            .then(function (result) {
                $scope.OrgUserList = result.data;
            });
    }

    $scope.AddOrgUser = function () {
        debugger;

        var _UserName = $scope.EmailID;
        var _Password = $scope.Password;
        var _FullName = $scope.FullName;
        var _EmailID = $scope.EmailID;
        var _Mobile = $scope.Mobile;

        $http({
            method: 'POST',
            url: '/Organisation/AddOrgUser',
            data: { UserName: _UserName, Password: _Password, FullName: _FullName, EmailID: _EmailID, Mobile: _Mobile }
        }).then(function (result) {
            $http.get('/Organisation/GetOrgUserList')
                .then(function (result) {
                    $scope.OrgUserList = result.data;
                });
        });
    }

    $scope.GetOrgUserDetails = function () {
        var _UserID = $scope.UserID;
        for (var i = 0; i < $scope.OrgUserList.length; i++) {
            e = $scope.OrgUserList[i];
            console.log(e.UserID);
            if (e.UserID == _UserID) {
                $scope.FullName = e.FullName;
                $scope.UserName = e.UserName;
                $scope.Password = e.Password;
                $scope.EmailID = e.EmailID;
                $scope.Mobile = e.Mobile;
                $scope.RoleID = e.RoleID;
            }
        }
    }

    $scope.EditOrgUser = function () {
        var _UserName = $scope.EmailID;
        var _Password = $scope.Password;
        var _FullName = $scope.FullName;
        var _EmailID = $scope.EmailID;
        var _Mobile = $scope.Mobile;
        var _RoleID = $scope.RoleID;
        var _UserID = $scope.UserID;

        $http({
            method: 'POST',
            url: '/Organisation/EditUser',
            data: { UserName: _UserName, Password: _Password, FullName: _FullName, EmailID: _EmailID, Mobile: _Mobile, RoleID: _RoleID, UserID: _UserID }
        }).then(function (result) {
            alert("Updated!")
            $http.get('/Organisation/GetOrgUserList')
                .then(function (result) {
                    $scope.OrgUserList = result.data;
                });
        });

    }
    $scope.DeleteUser = function (UserID) {
        if (confirm("Are you sure you want to Delete this User?"))
            $http({
                method: 'POST',
                url: '/Organisation/DeleteUser',
                data: { DeleteUserID: UserID }
            }).then(function (result) {
                alert('Deleted Successfully!!');
                GetOrgList();
            });
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

    $scope.GetEntityUserDetails = function () {
        var _UserID = $scope.UserID;
        for (var i = 0; i < $scope.EntityUserList.length; i++) {
            e = $scope.EntityUserList[i];
            console.log(e.UserID);
            if (e.UserID == _UserID) {
                $scope.FullName = e.FullName;
                $scope.UserName = e.UserName;
                $scope.Password = e.Password;
                $scope.EmailID = e.EmailID;
                $scope.Mobile = e.Mobile;
            }
        }

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
        var _EntityID = $scope.EntityID;
        var _UserName = $scope.EmailID;
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
            alert("User saved successfully!");
            $http.get('/Organisation/GetEntityUserList/' + $scope.EntityID)
            .then(function (result) {
                $scope.EntityUserList = result.data;
                GetRoles();
            });
        });
    }

    $scope.EditEntityUser = function () {
        debugger;
        var _UserName = $scope.EmailID;
        var _Password = $scope.Password;
        var _FullName = $scope.FullName;
        var _EmailID = $scope.EmailID;
        var _Mobile = $scope.Mobile;
        var _RoleID = $scope.RoleID;
        var _UserID = $scope.UserID;

        $http({
            method: 'POST',
            url: '/Organisation/EditUser',
            data: { UserName: _UserName, Password: _Password, FullName: _FullName, EmailID: _EmailID, Mobile: _Mobile, RoleID: _RoleID, UserID: _UserID }
        }).then(function (result) {
            alert("Updated!")
            $http.get('/Organisation/GetEntityUserList/' + $scope.EntityID)
                .then(function (result) {
                    $scope.EntityUserList = result.data;
                    GetRoles();
                });
        });
    }
    $scope.DeleteUser = function (UserID) {
        if (confirm("Are you sure you want to Delete this User?"))
            $http({
                method: 'POST',
                url: '/Organisation/DeleteUser',
                data: { DeleteUserID: UserID }
            }).then(function (result) {
                alert('Deleted Successfully!!');
                $scope.GetEntityUsers()
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
        var _ShipmentAmount = $scope.ShipmentAmount
        var _RemoteModel = $scope.RemoteModel
        var _EntityID = $scope.SelectedEntityID

        $http({
            method: 'POST',
            url: '/Organisation/SaveShippedRemote',
            data: { ShipmentAmount: _ShipmentAmount, RemoteModel: _RemoteModel, EntityID: _EntityID }
        }).then(function (result) {
            $http.get('/Organisation/GetRemotesList/' + $scope.EntityID)
               .then(function (result) {
                   $scope.RemotesList = result.data;
               });
        });
    }
});