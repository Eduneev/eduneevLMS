myapp.controller('StudentCntrl', function ($scope, $http) {
    GetProgramsList();
    $scope.SubjectSet = new Set();

    function GetProgramsList() {
        $http.get('/CourseMgmt/GetProgramsForCenter')
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
    };

    $scope.GetSubjectsList = function () {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
            .then(function (result) {
                $scope.SubjectsList = result.data;
            });
    };

    $scope.AddRemoveSubject = function (SubjectID) {
        if ($scope.SubjectSet.has(SubjectID))
            $scope.SubjectSet.delete(SubjectID);
        else
            $scope.SubjectSet.add(SubjectID);

        console.log($scope.SubjectSet);
    };

    $scope.SaveStudent = function () {
        debugger;

        $http.get('/StudentMgmt/GenerateStudentCode')
            .then(function (result) {
                $scope.Code = result.data[0].StudentEnrollmentNo;
                for (SubjectID of $scope.SubjectSet) {
                    var _StudentName = $scope.StudentName;
                    var _Code = $scope.Code;
                    var _Gender = $scope.Gender;
                    var _ProgramID = $scope.ProgID;
                    var _CourseID = $scope.CourseID;
                    var _SubjectID = SubjectID;
                    var _Email = $scope.Email;
                    var _Mobile = $scope.Mobile;
                    var _Landline = $scope.Landline;
                    var _GuardianName = $scope.GuardianName;
                    var _GuardianContactNo = $scope.GuardianContactNo;
                    var _BirthPlace = $scope.BirthPlace;
                    var _SchoolName = $scope.SchoolName;
                    var _Address = $scope.Address;
                    var _Pincode = $scope.PinCode;

                    $http({
                        method: 'POST',
                        url: '/StudentMgmt/SaveStudent',
                        data: { StudentName: _StudentName, Code: _Code, Gender: _Gender, ProgramID: _ProgramID, CourseID: _CourseID, SubjectID: _SubjectID, Email: _Email, Mobile: _Mobile, Landline: _Landline, GuardianName: _GuardianName, GuardianContactNo: _GuardianContactNo, BirthPlace: _BirthPlace, SchoolName: _SchoolName, Address: _Address, Pincode: _Pincode }
                    }).then(function (result) {
                    });
                }

            });

        alert("Student Saved Successfully");
    }

    $scope.ViewStudentsList = function () {
        window.location.href = "/StudentMgmt/ViewStudents/";
    }

});

myapp.controller('StudentViewCntrl', function ($scope, $http) {
    //GetStudents();
    function GetStudents() {
        $http.get('/StudentMgmt/GetStudents')
        .then(function (result) {
            $scope.StudentsList = result.data;
        });
    }

    $scope.GetStudentsListBySubject = function () {
        $http.get('/StudentMgmt/GetStudentsBySubject/' + $scope.SubjectID)
            .then(function (result) {
                $scope.StudentsList = result.data;
            });
    }

    $scope.StudentViewOptions = ["View Students By Subject", "View Students By Enrolment"];

    $scope.selectedItemChanged = function () {
        console.log($scope.selectedItem)
        if ($scope.selectedItem == $scope.StudentViewOptions[0])
            GetProgramsList()
        else if ($scope.selectedItem == $scope.StudentViewOptions[1])
            GetStudents()
    }

    $scope.AddStudent = function () {
        window.location.href = "/StudentMgmt/RegisterStudent/";
    }

    $scope.SelectStudent = function (StudentID) {
        $http({
            method: 'POST',
            url: '/StudentMgmt/SetStudentID',
            data: { StudentID: StudentID }
        }).then(function (result) {

        });
    }

    function GetProgramsList() {
        $http.get('/CourseMgmt/GetProgramsForCenter')
            .then(function (result) {
                $scope.ProgramTextToShow = 'Please select program..';
                $scope.ProgramsList = result.data;
            });
    }

    $scope.GetCourseList = function () {
        $http.get('/CourseMgmt/GetCourse/' + $scope.ProgID)
            .then(function (result) {
                $scope.CourseTextToShow = 'Please select Course..';
                $scope.CoursesList = result.data;
            });
    }

    $scope.GetSubjectsList = function () {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
            .then(function (result) {
                $scope.SubjectTextToShow = 'Please select Subject..';
                $scope.SubjectsList = result.data;
            });
    }


    $scope.fileChanged = function (e) {
        var files = e.target.files;
        var fileReader = new FileReader();
        fileReader.readAsDataURL(files[0]);
        fileReader.onload = function (e) {
            $scope.imgSrc = this.result;
            $scope.$apply();
        };
    }

    $scope.SelectStudent = function (StudentID) {
        $scope.StudentID = StudentID;
    }

    $scope.SaveImage = function () {
        debugger;
        //var _StudentImage = $('#mce').contents().find('#select_name').val();
        var _StudentImage = $('#mce').contents().find('#ImageData').val();
        $http({
            method: 'POST',
            url: '/StudentMgmt/SetStudentPhoto',
            data: { StudentID: $scope.StudentID, StudentImage: _StudentImage }
        }).then(function (result) {

        });
    }

    $scope.clear = function () {
        $scope.imageCropStep = 1;
        delete $scope.imgSrc;
        delete $scope.result;
        delete $scope.resultBlob;
    };
});

myapp.controller('StudentAttendanceCntrl', function ($scope, $http) {

    GetStudents();
    function GetStudents() {
        var params = parseURLParams(window.location.href)
        if ('SessionID' in params) {
            $scope.SessionID = params['SessionID'][0]
            $http.get('/StudentMgmt/GetStudentsForAttendance/' + $scope.SessionID)
                .then(function (result) {
                    $scope.StudentsList = result.data;
                });
        }
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

    function parseURLParams(url) {
        var queryStart = url.indexOf("?") + 1,
            queryEnd = url.indexOf("#") + 1 || url.length + 1,
            query = url.slice(queryStart, queryEnd - 1),
            pairs = query.replace(/\+/g, " ").split("&"),
            parms = {}, i, n, v, nv;

        if (query === url || query === "") return;

        for (i = 0; i < pairs.length; i++) {
            nv = pairs[i].split("=", 2);
            n = decodeURIComponent(nv[0]);
            v = decodeURIComponent(nv[1]);

            if (!parms.hasOwnProperty(n)) parms[n] = [];
            parms[n].push(nv.length === 2 ? v : null);
        }
        return parms;
    }
});


myapp.controller('AllocateRemoteCntrl', function ($scope, $http) {
    GetClassroomList();
    $scope.ClassroomTextToShow = "Please Select....";
    $scope.ReceiverSerialNo = null;

    function GetClassroomList() {
        $http.get('/CenterMgmt/GetClassroomReceiver')
            .then(function (result) {
                $scope.ClassroomList = result.data;
            });
    }

    $scope.GetProgramsList = function() {
        document.getElementById('receiver').textContent = $scope.ReceiverSerialNo;
        $http.get('/CourseMgmt/GetProgramsForCenter')
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

    $scope.GetStudentsList = function () {
        $http.get('/StudentMgmt/GetStudentsForRemoteAllocation/' + $scope.SubjectID)
        .then(function (result) {
            $scope.StudentsList = result.data;
        });
    }

    $scope.AssignRemoteToStudent = function (StudentID, RemoteNumber) {
        debugger;
        var _StudentID = StudentID;
        var _RemoteNumber = RemoteNumber + "-" + $scope.ReceiverSerialNo;
        var _SubjectID = $scope.SubjectID
        $http({
            method: 'POST',
            url: '/StudentMgmt/AssignRemoteToStudent',
            data: { StudentID: _StudentID, RemoteNumber: _RemoteNumber, SubjectID: _SubjectID }
        }).then(function (result) {
            $scope.GetStudentsList();
            //window.location.href = "/SessionMgmt/StudentAttendance/";
        });
    };
});