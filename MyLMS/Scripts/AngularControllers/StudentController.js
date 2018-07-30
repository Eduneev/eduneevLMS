myapp.controller('StudentCntrl', function ($scope, $http) {
    $scope.ProgramTextToShow = 'Please select..'
    $scope.CourseTextToShow = 'Please select..'
    GetCourseDetails();
    function GetCourseDetails() {
        $http.get('/CourseMgmt/GetCourseDetails')
        .then(function (result) {
            $scope.CourseList = result.data;
        });
    }

    $scope.SaveStudent = function () {
        debugger;
        var _StudentName = $scope.StudentName
        var _Code = $scope.Code;
        var _Gender = $scope.Gender;
        var _ProgramID = $scope.ProgID
        var _CourseID = $scope.CourseID;
        var _Email = $scope.Email;
        var _Mobile = $scope.Mobile
        var _Landline = $scope.Landline;
        var _GuardianName = $scope.GuardianName;
        var _GuardianContactNo = $scope.GuardianContactNo
        var _BirthPlace = $scope.BirthPlace;
        var _SchoolName = $scope.SchoolName;
        var _Address = $scope.Address
        var _Pincode = $scope.PinCode;

        $http({
            method: 'POST',
            url: '/StudentMgmt/SaveStudent',
            data: { StudentName: _StudentName, Code: _Code, Gender: _Gender, ProgramID: _ProgramID, CourseID: _CourseID, Email: _Email, Mobile: _Mobile, Landline: _Landline, GuardianName: _GuardianName, GuardianContactNo: _GuardianContactNo, BirthPlace: _BirthPlace, SchoolName: _SchoolName, Address: _Address, Pincode: _Pincode }
        }).then(function (result) {
            GetCourseDetails();
        });
    }

    $scope.ViewStudentsList = function () {
        window.location.href = "/StudentMgmt/ViewStudents/";
    }

});

myapp.controller('StudentViewCntrl', function ($scope, $http) {
    GetStudents();
    function GetStudents() {
        $http.get('/StudentMgmt/GetStudents')
        .then(function (result) {
            $scope.StudentsList = result.data;
        });
    }
    $scope.AddStudent = function () {
        window.location.href = "/StudentMgmt/RegisterStudent/";
    }
});

myapp.controller('StudentAttendanceCntrl', function ($scope, $http) {
    function GetStudents() {
        $http.get('/StudentMgmt/GetStudentsForAttendance')
        .then(function (result) {
            $scope.StudentsList = result.data;
        });
    }

    $scope.MarkAttendance = function (SessionID, StudentID) {
        debugger;
        $http({
            method: 'POST',
            url: '/StudentMgmt/MarkAttendance',
            data: { SessionID: SessionID, StudentID: StudentID}
        }).then(function (result) {
            GetStudents();
        });
    }
    $scope.AddStudent = function () {
        window.location.href = "/StudentMgmt/RegisterStudent/";
    }
});


myapp.controller('AllocateRemoteCntrl', function ($scope, $http) {
    GetProgramsList();

    function GetProgramsList() {
        $http.get('/CourseMgmt/GetPrograms')
        .then(function (result) {
            $scope.ProgramsList = result.data;
        });
    }

    $scope.GetCourseList = function () {
        $http.get('/CourseMgmt/GetCourse/' + $scope.ProgID)
        .then(function (result) {
            $scope.CoursesList = result.data;
        });
    }

    $scope.GetStudentsList = function () {
        debugger;
        $http.get('/StudentMgmt/GetStudentsForRemoteAllocation/' + $scope.CourseID)
        .then(function (result) {
            $scope.StudentsList = result.data;
        });
    }

    $scope.AssignRemoteToStudent = function (StudentID, RemoteNumber) {
        debugger;
        var _StudentID = StudentID;
        var _RemoteNumber = RemoteNumber;
        $http({
            method: 'POST',
            url: '/StudentMgmt/AssignRemoteToStudent',
            data: { StudentID: _StudentID, RemoteNumber: _RemoteNumber }
        }).then(function (result) {
            $scope.GetStudentsList();
            //window.location.href = "/SessionMgmt/StudentAttendance/";
        });
    };
});