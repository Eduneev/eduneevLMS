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
                $scope.CenterTextToShow = "Please select center.."
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
        var _CenterID = $scope.CenterID;
        var _UserName = $scope.EmailID;
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

    $scope.GetCenterUserDetails = function () {
        var _UserID = $scope.UserID;
        for (var i = 0; i < $scope.CenterUserList.length; i++) {
            e = $scope.CenterUserList[i];
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

    $scope.EditCenterUser = function () {
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
            $http.get('/Entity/GetCenterUserList/' + $scope.CenterID)
                .then(function (result) {
                    $scope.CenterUserList = result.data;
                    GetRoles();
                });
        });

    }

    $scope.UpdatePassword = function () {
        debugger;

        var _UserID = $scope.UserID
        var _Password = $scope.Password;

        $http({
            method: 'POST',
            url: '/Organisation/UpdatePassword',
            data: { UserID: _UserID, Password: _Password }
        }).then(function (result) {

        });
    }
});

myapp.controller('EntityUserCntrl', function ($scope, $http) {
    GetEntityUsersList()

    function GetEntityUsersList() {
        $http.get('/Entity/GetEntityUserList/')
            .then(function (result) {
                $scope.EntityUserList = result.data;
            });
    }

    $scope.UpdatePassword = function () {
        debugger;

        var _UserID = $scope.UserID
        var _Password = $scope.Password;

        $http({
            method: 'POST',
            url: '/Organisation/UpdatePassword',
            data: { UserID: _UserID, Password: _Password }
        }).then(function (result) {
            alert("Password updated Successfully!")
        });
    }

});

myapp.controller('FacultyCntrl', function ($scope, $http) {

    GetProgramsList();
    $scope.SubjectSet = new Set();

    function GetProgramsList() {
        $http.get('/CourseMgmt/GetPrograms')
            .then(function (result) {
                $scope.ProgramTextToShow = 'Please select..';
                $scope.ProgramsList = result.data;
            });
    }

    $scope.GetCourseList = function () {
        $http.get('/CourseMgmt/GetCourse/' + $scope.ProgID)
            .then(function (result) {
                $scope.CourseTextToShow = 'Please select..';
                $scope.CoursesList = result.data;
            });
    }

    $scope.GetSubjectsList = function () {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
            .then(function (result) {
                $scope.SubjectTextToShow = 'Please select..';
                $scope.SubjectsList = result.data;
            });
    }

    $scope.GetFaculty = function () {
        $http.get('/FacultyMgmt/GetFaculty/' + $scope.SubjectID)
            .then(function (result) {
                $scope.FacultyList = result.data;
            });
    }

    $scope.AddRemoveSubject = function (SubjectID) {
        if ($scope.SubjectSet.has(SubjectID))
            $scope.SubjectSet.delete(SubjectID);
        else
            $scope.SubjectSet.add(SubjectID);

        console.log($scope.SubjectSet);
    };


    $scope.SaveFaculty = function () {
        $scope.AddRemoveSubject = function (SubjectID) {
            if ($scope.SubjectSet.has(SubjectID))
                $scope.SubjectSet.delete(SubjectID);
            else
                $scope.SubjectSet.add(SubjectID);

            console.log($scope.SubjectSet);
        };

        for (SubjectID of $scope.SubjectSet) {
            var _FacultyName = $scope.FacultyName;
            var _Gender = $scope.Gender;
            var _ProgramID = $scope.ProgID;
            var _CourseID = $scope.CourseID;
            var _SubjectID = SubjectID;
            var _Email = $scope.Email;
            var _Mobile = $scope.Mobile;
            $http({
                method: 'POST',
                url: '/FacultyMgmt/SaveFaculty',
                data: { FacultyName: _FacultyName, ProgramID: _ProgramID, CourseID: _CourseID, SubjectID: _SubjectID, Email: _Email, Mobile: _Mobile, Gender: _Gender }
            }).then(function (result) {
                alert("Faculty Saved Successfully");
                $scope.GetFaculty();
            });
        }
    }

    $scope.RemoveFaculty = function(FacultyID) {
        $http({
            method: 'POST',
            url: '/FacultyMgmt/DeleteFaculty',
            data: { FacultyID: FacultyID, SubjectID: $scope.SubjectID }
        }).then(function (result) {
            alert("Faculty Saved Successfully");
            $scope.GetFaculty();
        });
    }

});