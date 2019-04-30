﻿myapp.controller('StudentCntrl', function ($scope, $http) {
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

    $scope.EditStudent = function (code) {
        window.location.href = "/StudentMgmt/EditStudent?StudentCode=" + code;
    }

    $scope.SaveImage = function () {
        debugger;
        //var _StudentImage = $('#mce').contents().find('#select_name').val();
        window.frames[0].postMessage("imagedata", "*");

        window.addEventListener("message", receiveMessage, false);

        function receiveMessage(event) {
            var _StudentImage = "" + event.data;
            $http({
                method: 'POST',
                url: '/StudentMgmt/SetStudentPhoto',
                data: { StudentID: $scope.StudentID, StudentImage: _StudentImage }
            }).then(function (result) {
            });
        }
    }

    $scope.clear = function () {
        debugger;
        $scope.imageCropStep = 1;
        delete $scope.imgSrc;
        delete $scope.result;
        delete $scope.resultBlob;
    };
});

myapp.controller('EditStudentCntrl', function ($scope, $http) {

    SetParams(window.location.href);

    $scope.StudentSubjects = {};
    $scope.SubjectSet = new Set();

    function SetParams(url) {
        try {
            var params = parseURLParams(url);
            if ('StudentCode' in params) {
                $scope.StudentCode = params['StudentCode'][0];
                GetStudentDetails();
            }
        }
        catch (e) {
            console.log(e);
        }
    }

    // Create a SubjectSet that tracks the subjects the student is registered for
    function GetStudentDetails() {
        $http.get('/StudentMgmt/GetStudentsByCode/' + $scope.StudentCode)
            .then(function (result) {
                var data = result.data;

                console.log(data)
                for (let i = 0; i < data.length; i++) {
                    $scope.StudentSubjects[data[i].SubjectID] = data[i].StudentID;
                    $scope.SubjectSet.add(data[i].SubjectID);
                }

                if (data.length > 0) {
                    $scope.StudentName = data[0].StudentName;
                    $scope.Code = data[0].Code;
                    $scope.Gender = data[0].Gender;
                    $('#drpGender').val($scope.Gender);

                    $scope.ProgID = data[0].ProgID;
                    $scope.ProgramsList = [{ 'ProgID': $scope.ProgID, 'ProgramName': data[0].ProgramName }];
                    console.log($scope.ProgramsList)
                    $scope.CourseID = data[0].CourseID;
                    $scope.CoursesList = [{ 'CourseID': $scope.CourseID, 'CourseName': data[0].CourseName }];

                    $scope.Email = data[0].Email;
                    $scope.Mobile = data[0].Mobile;
                    $scope.Landline = data[0].Landline;
                    $scope.GuardianName = data[0].GuardianName;
                    $scope.GuardianContactNo = data[0].GuardianContactNo;
                    $scope.SchoolName = data[0].SchoolName;
                    $scope.Address = data[0].Address;
                    $scope.PinCode = data[0].PinCode;
                    $scope.StudentImageURL = data[0].StudentImageURL;

                    GetSubjectsList();
                }
            });
    }

    $scope.AddRemoveSubject = function (SubjectID) {
        if ($scope.SubjectSet.has(SubjectID))
            $scope.SubjectSet.delete(SubjectID);
        else
            $scope.SubjectSet.add(SubjectID);

        console.log($scope.SubjectSet);
    };

    function GetSubjectsList() {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
            .then(function (result) {
                $scope.SubjectsList = result.data;

                console.log($scope.StudentSubjects)
                for (var i = 0; i < $scope.SubjectsList.length; i++) {
                    if ($scope.SubjectsList[i].SubjectID in $scope.StudentSubjects)
                        $scope.SubjectsList[i].selected = true;
                    else
                        $scope.SubjectsList[i].selected = false;
                }
                console.log($scope.SubjectsList)
            });
    };

    // When one of the subjects is removed from the set, call delete StudentFunctionality 
    // with that SubjectID and StudentID, Else for any new subjects added, call SaveStudent
    // For the remaining subjects that have been neither added nor removed, loop through
    // and call EditStudent with StudentID
    $scope.EditStudent = function () {
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

            //Edit Student
            if (SubjectID in $scope.StudentSubjects) {
                $http({
                    method: 'POST',
                    url: '/StudentMgmt/EditStudent',
                    data: { StudentID: $scope.StudentSubjects[SubjectID], StudentName: _StudentName, Code: _Code, Gender: _Gender, ProgramID: _ProgramID, CourseID: _CourseID, SubjectID: _SubjectID, Email: _Email, Mobile: _Mobile, Landline: _Landline, GuardianName: _GuardianName, GuardianContactNo: _GuardianContactNo, BirthPlace: _BirthPlace, SchoolName: _SchoolName, Address: _Address, Pincode: _Pincode }
                }).then(function (result) {
                });
            }
            // Save Student
            else {
                $http({
                    method: 'POST',
                    url: '/StudentMgmt/SaveStudent',
                    data: { StudentName: _StudentName, Code: _Code, Gender: _Gender, ProgramID: _ProgramID, CourseID: _CourseID, SubjectID: _SubjectID, Email: _Email, Mobile: _Mobile, Landline: _Landline, GuardianName: _GuardianName, GuardianContactNo: _GuardianContactNo, BirthPlace: _BirthPlace, SchoolName: _SchoolName, Address: _Address, Pincode: _Pincode }
                }).then(function (result) {
                });
            }
        }

        for (SubjectID in $scope.StudentSubjects) {
            if (!(SubjectID in $scope.SubjectSet)) {
                // Delete Student
                $http({
                    method: 'POST',
                    url: '/StudentMgmt/DeleteStudent',
                    data: { StudentID: $scope.SubjectSet[SubjectID] }
                }).then(function (result) {
                });

            }

        }
        alert("Student Saved Successfully");
    }

    $scope.ViewStudentsList = function () {
        window.location.href = "/StudentMgmt/ViewStudents/";
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

    $scope.MarkAttendance = function (StudentID) {
        debugger;
        $http({
            method: 'POST',
            url: '/StudentMgmt/MarkAttendance',
            data: { SessionID: $scope.SessionID, StudentID: StudentID}
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
    $scope.ReceiverSerialNo = null;

    function GetClassroomList() {
        $http.get('/CenterMgmt/GetClassroomReceiver')
            .then(function (result) {
                $scope.ClassroomTextToShow = "Please Select....";
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
        $scope.ClassroomID = 0;
        for (var i = 0; i < $scope.ClassroomList.length; i++)
            if ($scope.ClassroomList[i].ReceiverSerialNo == $scope.ReceiverSerialNo)
                $scope.ClassroomID = $scope.ClassroomList[i].ClassRoomID;
        console.log($scope.ClassroomID)

        $http.get('/StudentMgmt/GetStudentsForRemoteAllocation/' + $scope.SubjectID + '/' + $scope.ClassroomID)
        .then(function (result) {
            $scope.StudentsList = result.data;
        });
    }

    $scope.AssignRemoteToStudent = function (StudentID, RemoteNumber) {
        debugger;

        if ($scope.ClassroomID != 0) {
            var _StudentID = StudentID;
            var _RemoteNumber = RemoteNumber + "-" + $scope.ReceiverSerialNo;
            var _SubjectID = $scope.SubjectID;
            var _ClassroomID = $scope.ClassroomID;
            $http({
                method: 'POST',
                url: '/StudentMgmt/AssignRemoteToStudent',
                data: { StudentID: _StudentID, RemoteNumber: _RemoteNumber, SubjectID: _SubjectID, ClassroomID: _ClassroomID }
            }).then(function (result) {
                $scope.GetStudentsList();
                //window.location.href = "/SessionMgmt/StudentAttendance/";
            });
        }
    };
});