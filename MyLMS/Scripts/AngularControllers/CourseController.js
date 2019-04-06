myapp.controller('CourseCntrl', function ($scope, $http) {
    GetCourseDetails();
    $scope.ProgramTextToShow = 'Select Program..';
    $scope.CourseTextToShow = 'Select Course..'
    $scope.SubjectTextToShow = 'Select Subject..'
    
    $scope.SaveProgram = function () {
        debugger;
        var _ProgramName = $scope.ProgramName;
        var _ProgramCode = $scope.ProgramCode;
        $http({
            method: 'POST',
            url: '/CourseMgmt/SaveProgram',
            data: { ProgramName: _ProgramName, ProgramCode: _ProgramCode }
        }).then(function (result) {
            GetCourseDetails();
        });
    }

    $scope.SaveCourse = function () {
        debugger;
        var _ProgID = $scope.ProgID
        var _CourseName = $scope.CourseName;
        var _CourseCode = $scope.CourseCode;
        
        $http({
            method: 'POST',
            url: '/CourseMgmt/SaveCourse',
            data: { ProgID: _ProgID, CourseName: _CourseName, CourseCode: _CourseCode }
        }).then(function (result) {
            GetCourseDetails();
        });
    }

    $scope.SaveSubject = function () {
        debugger;
        var _CourseID = $scope.CourseID
        var _SubjectName = $scope.SubjectName;
        var _SubjectCode = $scope.SubjectCode;
        $http({
            method: 'POST',
            url: '/CourseMgmt/SaveSubject',
            data: { CourseID: _CourseID, SubjectName: _SubjectName, SubjectCode: _SubjectCode }
        }).then(function (result) {
            GetCourseDetails();
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
    }

    $scope.sortBy = function (propertyName) {
        $scope.reverse = (propertyName !== null && $scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };
});