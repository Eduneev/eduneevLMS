myapp.controller('CourseCntrl', function ($scope, $http) {
    GetCourseDetails();
    $scope.ProgramTextToShow = 'Select Program..';
    $scope.CourseTextToShow = 'Select Course..';
    $scope.SubjectTextToShow = 'Select Subject..';
    
    $scope.SaveProgram = function () {
        debugger;
        var _ProgramName = $scope.ProgramName;
        var _ProgramCode = $scope.ProgramCode;
        if (_ProgramCode.includes(" ") || _ProgramCode.includes("-")) {
            alert("Program Code cannot have '-' or spaces in it"); return;
        }
        $http({
            method: 'POST',
            url: '/CourseMgmt/SaveProgram',
            data: { ProgramName: _ProgramName, ProgramCode: _ProgramCode }
        }).then(function (result) {
            GetCourseDetails();
            $scope.ProgID = null;
            $scope.ProgrameName = null;
            $scope.ProgramCode = null;
        });
    }

    $scope.EditProgram = function () {
        debugger;
        var _ProgID = $scope.ProgID;
        var _ProgramName = $scope.ProgramName;
        var _ProgramCode = $scope.ProgramCode;
        if (_ProgramCode.includes(" ") || _ProgramCode.includes("-")) {
            alert("Program Code cannot have '-' or spaces in it"); return;
        }
        $http({
            method: 'POST',
            url: '/CourseMgmt/EditProgram',
            data: { ProgID: _ProgID, ProgramName: _ProgramName, ProgramCode: _ProgramCode }
        }).then(function (result) {
            GetCourseDetails();
            $scope.ProgID = null;
            $scope.ProgrameName = null;
            $scope.ProgramCode = null;
            $scope.CourseID = null;
            $scope.CourseName = null;
            $scope.CourseCode = null;
        });
    }

    $scope.SaveCourse = function () {
        debugger;
        var _ProgID = $scope.ProgID
        var _CourseName = $scope.CourseName;
        var _CourseCode = $scope.CourseCode;
        if (_CourseCode.includes(" ") || _CourseCode.includes("-")) {
            alert("Course Code cannot have '-' or spaces in it"); return;
        }
        $http({
            method: 'POST',
            url: '/CourseMgmt/SaveCourse',
            data: { ProgID: _ProgID, CourseName: _CourseName, CourseCode: _CourseCode }
        }).then(function (result) {
            $scope.ProgID = null;
            $scope.ProgrameName = null;
            $scope.ProgramCode = null;
            $scope.CourseID = null;
            $scope.CourseName = null;
            $scope.CourseCode = null;
            GetCourseDetails();
        });
    }

    $scope.EditCourse = function () {
        debugger;
        var _CourseID = $scope.CourseID
        var _CourseName = $scope.CourseName;
        var _CourseCode = $scope.CourseCode;
        if (_CourseCode.includes(" ") || _CourseCode.includes("-")) {
            alert("Course Code cannot have '-' or spaces in it");
            return;
        }
        $http({
            method: 'POST',
            url: '/CourseMgmt/EditCourse',
            data: { CourseID: _CourseID, CourseName: _CourseName, CourseCode: _CourseCode }
        }).then(function (result) {
            GetCourseDetails();
            $scope.ProgID = null;
            $scope.ProgrameName = null;
            $scope.ProgramCode = null;
            $scope.CourseID = null;
            $scope.CourseName = null;
            $scope.CourseCode = null;
        });
    }

    $scope.SaveSubject = function () {
        debugger;
        var _CourseID = $scope.CourseID
        var _SubjectName = $scope.SubjectName;
        var _SubjectCode = $scope.SubjectCode;
        if (_SubjectCode.includes(" ") || _SubjectCode.includes("-")) {
            alert("Subject Code cannot have '-' or spaces in it"); return;
        }
        $http({
            method: 'POST',
            url: '/CourseMgmt/SaveSubject',
            data: { CourseID: _CourseID, SubjectName: _SubjectName, SubjectCode: _SubjectCode }
        }).then(function (result) {
            GetCourseDetails();
            $scope.ProgID = null;
            $scope.ProgrameName = null;
            $scope.ProgramCode = null;
            $scope.CourseID = null;
            $scope.CourseName = null;
            $scope.CourseCode = null;
            $scope.SubjectID = null;
            $scope.SubjectName = null;
            $scope.SubjectCode = null;
        });
    }

    $scope.EditSubject = function () {
        debugger;
        var _SubjectID = $scope.SubjectID
        var _SubjectName = $scope.SubjectName;
        var _SubjectCode = $scope.SubjectCode;
        if (_SubjectCode.includes(" ") || _SubjectCode.includes("-")) {
            alert("Subject Code cannot have '-' or spaces in it"); return;
        }
        $http({
            method: 'POST',
            url: '/CourseMgmt/EditSubject',
            data: { SubjectID: _SubjectID, SubjectName: _SubjectName, SubjectCode: _SubjectCode }
        }).then(function (result) {
            GetCourseDetails();
            $scope.ProgID = null;
            $scope.ProgrameName = null;
            $scope.ProgramCode = null;
            $scope.CourseID = null;
            $scope.CourseName = null;
            $scope.CourseCode = null;
            $scope.SubjectID = null;
            $scope.SubjectName = null;
            $scope.SubjectCode = null;
        });
    }

    $scope.SaveTopic = function () {
        debugger;
        var _SubjectID = $scope.SubjectID
        var _TopicName = $scope.TopicName;
        var _TopicCode = $scope.TopicCode;
        $http({
            method: 'POST',
            url: '/CourseMgmt/SaveTopic',
            data: { SubjectID: _SubjectID, TopicName: _TopicName, TopicCode: _TopicCode }
        }).then(function (result) {
            GetCourseDetails();
        });
    }

    $scope.OpenCoursePopUp = function () {
        debugger;
        $scope.GetProgramsList();
        $('#AddCourse').modal('show');
        
    }

    $scope.OpenSubjectPopUp = function () {
        debugger;
        $scope.GetProgramsList();
        $('#AddSubject').modal('show');
    }

    $scope.OpenTopicPopUp = function () {
        debugger;
        $scope.GetProgramsList();
        $('#AddTopic').modal('show');
    }

    function GetProgramsList() {
        $http.get('/CourseMgmt/GetPrograms')
            .then(function (result) {
                $scope.ProgramsList = result.data;
            });
    }

    $scope.GetProgramsList = function() {
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

    $scope.GetSubjectsList = function () {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
        .then(function (result) {
            $scope.SubjectsList = result.data;
        });
    }

    function GetCourseDetails() {
        $http.get('/CourseMgmt/GetCourseDetails')
        .then(function (result) {
            $scope.CourseList = result.data;
            });
        GetProgramsList();
    }

    $scope.GetProgramDetails = function () {
        var _ProgID = $scope.ProgID;
        for (var i = 0; i < $scope.ProgramsList.length; i++) {
            e = $scope.ProgramsList[i];
            if (e.ProgID == _ProgID) {
                $scope.ProgramName = e.ProgramName;
                $scope.ProgramCode = e.ProgramCode;
            }
        }
    }

    $scope.GetCourseDetailsByID = function () {
        var _CourseID = $scope.CourseID;
        for (var i = 0; i < $scope.CoursesList.length; i++) {
            e = $scope.CoursesList[i];
            if (e.CourseID == _CourseID) {
                $scope.CourseName = e.CourseName;
                $scope.CourseCode = e.CourseCode;
            }
        }
    }

    $scope.GetSubjectDetails = function () {
        var _SubjectID = $scope.SubjectID;
        for (var i = 0; i < $scope.SubjectsList.length; i++) {
            e = $scope.SubjectsList[i];
            if (e.SubjectID == _SubjectID) {
                $scope.SubjectName = e.SubjectName;
                $scope.SubjectCode = e.SubjectCode;
            }
        }
    }

    $scope.sortBy = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };
});