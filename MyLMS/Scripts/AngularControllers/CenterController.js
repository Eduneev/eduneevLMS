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

    $scope.EditCenter = function () {
        var _CenterID = $scope.CenterID;
        var _CenterName = $scope.CenterName;
        var _Email = $scope.Email;
        var _CenterCode = $scope.CenterCode;
        var _Landline1 = $scope.Landline1;
        var _Mobile = $scope.Mobile;
        var _Address = $scope.Address;
        var _PinCode = $scope.PinCode;

        $http({
            method: 'POST',
            url: '/CenterMgmt/EditCenter',
            data: { CenterID: _CenterID, CenterName: _CenterName, CenterCode: _CenterCode, Email: _Email, Landline1: _Landline1, Mobile: _Mobile, Address: _Address, PinCode: _PinCode }
        }).then(function (result) {
            alert('Saved Successfully!!');
            GetCentersList();
        });
    }

    $scope.GetCenterDetails = function () {
        var _CenterID = $scope.CenterID;
        for (var i = 0; i < $scope.CentersList.length; i++) {
            e = $scope.CentersList[i];
            if (_CenterID == e.CenterID) {
                $scope.CenterName = e.CenterName;
                $scope.CenterCode = e.CenterCode;
                $scope.Email = e.Email;
                $scope.Landline1 = e.Landline1;
                $scope.Landline2 = e.Landline2;
                $scope.Mobile = e.Mobile;
                $scope.Address = e.Address;
                $scope.PinCode = e.PinCode;
            }
        }
    }

    $scope.DeleteCenter = function (CenterID) {
        if (confirm("Are you sure you want to Delete this Center?")) {
            $http({
                method: 'POST',
                url: '/CenterMgmt/DeleteCenter',
                data: { CenterID: CenterID }
            }).then(function (result) {
                alert('Deleted Successfully!!');
                GetCentersList();
            });
        }
    }

    function GetCentersList() {
        $http.get('/CenterMgmt/GetCenters')
            .then(function (result) {
            console.log(result.data)
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

myapp.controller('ClassroomCntrl', function ($scope, $http) {

    GetCenter();

    function GetCenter() {
        $http.get('/CenterMgmt/GetCenters')
            .then(function (result) {
                console.log(result.data)
                var CentersList = result.data;
                $scope.Center = CentersList[0];
                $scope.CenterID = $scope.Center.CenterID;

                GetClassrooms();
            });
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
            GetClassrooms();
        });
    }

    $scope.DeleteClassroom = function (ClassRoomID) {
        if (confirm("Are you sure you want to Delete this Classroom?"))
            $http({
                method: 'POST',
                url: '/CenterMgmt/DeleteClassroom',
                data: { ClassRoomID: ClassRoomID }
            }).then(function (result) {
                alert('Deleted Successfully!!');
                GetClassrooms();
            });
    }

});

myapp.controller('CSVCenterCntrl', function ($scope, $http, $q) {
    promises = []
    function handleCSVFile(event) {
        var files = event.target.files;
        var file = files[0]
        var reader = new FileReader();
        reader.readAsText(file);
        reader.onload = function (event) {
            try {
                var csv = event.target.result;
                var data = $.csv.toObjects(csv);
            }
            catch {
                removeFileElement(); alert("Error Reading CSV");
            }

            if (!CheckCSVColumns(data[0])) {
                removeFileElement(); alert("Error: Incorrect CSV Columns");
                return;
            }

            for (const [idx, obj] of data.entries()) {
                var val = CheckValidRow(obj, idx);
                if (val == 0) {
                    removeFileElement(); alert("Error in row " + idx + ".Students not added");
                    return;
                }
            }

            $q.all(promises).then(function (results) {
                console.log("ALL HTTP requests done");
                console.log(results)
                for (result of results) {
                    if (result[0] == 0) {
                        removeFileElement(); alert("Error in row " + result[1] + ".Centers not added");
                        return;
                    }
                }

                for (val of data)
                    SaveCSVRow(val);

                res = "Saved " + data.length + " centers successfully!";
                removeFileElement();
                alert(res);
            });

        }
    }

    function CheckValidRow(data, idx) {
        try {
            var EntityName = data.Entity;
            var Classrooms = [];
            Classrooms.push(data.Classroom1);
            Classrooms.push(data.Classroom2);
            Classrooms.push(data.Classroom3);
            Classrooms.push(data.Classroom4);
            var Email = data.Email;
            var Password = data.Password;

            Classrooms = Classrooms.filter(function (el) { return el; });

            // Check data validity

            if (Classrooms.length == 0) throw '';

            if (Email == '' || Password == '') throw '';

            var result = $http.get('/Entity/CheckEntity/' + EntityName)
                .then(function (result) {
                    if (result.data == 'Failure') return [0, idx];
                    return [1, idx];
                });
            promises.push(result)
        }
        catch{
            return 0;
        }
        return 1;
    }

    function removeFileElement() {
        document.getElementById("files").remove();
    }

    function SaveCSVRow(data) {
        var _EntityName = data.Entity;
        var _CenterName = data.Center;
        var _CenterCode = data.CenterCode;
        var _Classrooms = [];
        _Classrooms.push(data.Classroom1);
        _Classrooms.push(data.Classroom2);
        _Classrooms.push(data.Classroom3);
        _Classrooms.push(data.Classroom4);
        _Classrooms = _Classrooms.filter(function (el) { return el; });

        var _Email = data.Email;
        var _Password = data.Password;
        var _Fullname = data.CoordinatorName;
        var _Mobile = data.Mobile;
        var _Landline = data.Landline;
        var _Address = data.Address;
        var _Pincode = data.PinCode;

        $http({
            method: 'POST',
            url: '/CenterMgmt/SaveCSVCenterAndClassroomAndCoordinator',
            data: { EntityName: _EntityName, CenterName: _CenterName, CenterCode: _CenterCode, Password: _Password, Fullname: _Fullname, Email: _Email, Mobile: _Mobile, Landline1: _Landline, Address: _Address, PinCode: _Pincode, Classrooms: _Classrooms }
        }).then(function (result) {
        });
    }

    function CheckCSVColumns(data) {
        console.log(data)
        var EntityName = data.Entity;
        var CenterName = data.Center;
        var CenterCode = data.CenterCode;
        var Classrooms = [];
        Classrooms.push(data.Classroom1);
        Classrooms.push(data.Classroom2);
        Classrooms.push(data.Classroom3);
        Classrooms.push(data.Classroom4);

        var Email = data.Email;
        var Mobile = data.Mobile;
        var Landline = data.Landline;
        var Address = data.Address;
        var Pincode = data.PinCode;
        var Password = data.Password;
        var Fullname = data.CoordinatorName;

        if (EntityName == undefined || CenterName == undefined || CenterCode == undefined || Password == undefined || Fullname == undefined || Email == undefined || Mobile == undefined || Landline == undefined || Address == undefined || Pincode == undefined || Classrooms.includes(undefined))
            return false
        return true
    }

    $scope.HandleFiles = function () {
        l = document.getElementById("left");
        var x = document.createElement("INPUT");
        x.id = "files";
        x.setAttribute("type", "file");
        left.appendChild(x);

        $(document).ready(function () {
            $('#files').bind('change', handleCSVFile);
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
        for (Center of $scope.CentersList) {
            if (Center.CenterID == SelectedCenterID)
                $scope.CenterName = Center.CenterName;
        }
        GetClassrooms();
    }

    function GetClassrooms() {
        $http.get('/CenterMgmt/GetClassroomsForCenter/' + $scope.CenterID)
            .then(function (result) {
                $scope.ClassroomList = result.data;
            });
    }

    $scope.DownloadBatch = function (ClassroomID, ClassroomName) {

        var result = "SET /a classroom=" + ClassroomID + "\n";
        result = result + "SET /a i=0\nsetlocal EnableDelayedExpansion\nSET RES=\nFOR /F %%X IN ('wmic path win32_computersystemproduct get uuid') DO (";
        result = result + "SET VAR=%%X\nIF !i! EQU 1 (SET RES=!VAR!)\nSET /a i+=1\n@echo !i!\n)\n";
        result = result + "SET URL=http://portal.2WayLive.com/api/SetClassroomAuth/!classroom!/!RES!\ncurl !URL!";
        name = $scope.CenterName + "-" + ClassroomName + ".bat";
        var a = $('<a/>', {
            style: 'display:none',
            href: 'data:application/octet-stream;base64,' + btoa(result),
            download: name
        }).appendTo('body');
        a[0].click();
        a.remove();
    }

    $scope.DownloadAuth = function (auth) {
        
        $http.get('/CenterMgmt/GetClassroomPackage/' + auth)
            .then(function (result) {
                
                var a = $('<a/>', {
                    style: 'display:none',
                    href: "http://portal.2WayLive.com/Scripts/2WayLive.zip",
                    download: '2WayLive.zip'
                }).appendTo('body');
                a[0].click();
                a.remove();
                
            });

        setTimeout(function () { alert('Please wait as package is being created'); }, 1);
        
        /*
        var a = $('<a/>', {
            style: 'display:none',
            href: 'data:application/octet-stream;base64,' + btoa(auth),
            download: 'auth.pem'
        }).appendTo('body');
        a[0].click();
        a.remove();
        */
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