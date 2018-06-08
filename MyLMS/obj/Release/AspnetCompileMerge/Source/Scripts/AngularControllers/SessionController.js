myapp.controller('SessionCntrl', function ($scope, $http) {
    $scope.SessionID = 0;

    GetStudio();
    GetPrograms();


    function GetStudio() {
        $http.get('/SessionMgmt/GetStudios')
        .then(function (result) {
            $scope.StudioList = result.data;
            $scope.StudioTextToShow = 'Please select studio..'
        });
    }

    function GetPrograms() {
        $http.get('/CourseMgmt/GetPrograms')
        .then(function (result) {
            $scope.Programs = result.data;
            $scope.ProgramTextToShow = 'Please select program..'
        });
    }

    $scope.GetCourse = function GetCourse() {
        $http.get('/CourseMgmt/GetCourse/' + $scope.ProgID)
        .then(function (result) {
            $scope.Courses = result.data;
            $scope.CourseTextToShow = 'Please select course..'
        });
    }

    $scope.GetSubject = function GetSubject() {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
        .then(function (result) {
            $scope.Subjects = result.data;
            $scope.SubjectTextToShow = 'Please select subject..'
        });
    }


    $scope.GetTopic = function GetTopic() {

        $scope.FacultyTextToShow = 'Please select faculty...'
        $http.get('/CourseMgmt/GetTopics/' + $scope.SubjectID)
        .then(function (result) {
            $scope.Topics = result.data;
            GetFaculty();
        });
    }

    function GetFaculty() {
        $http.get('/CourseMgmt/GetFaculty/' + $scope.SubjectID)
        .then(function (result) {
            $scope.Faculties = result.data;
        });
    }

    $scope.GetSessions = function () {
        $http.get('/SessionMgmt/GetSessions')
        .then(function (result) {
            $scope.SessionsList = result.data;
        });
    };


    $scope.SaveSession = function () {
        debugger;
        var _SessionNo = $scope.SessionNo;
        var _SessionName = $scope.SessionName;
        var _SessionDate = $scope.SessionDate;
        var _StartTime = $scope.StartTime;
        var _EndTime = $scope.EndTime;
        var _StudioID = $scope.StudioID;
        var _ProgID = $scope.ProgID;
        var _CourseID = $scope.CourseID;
        var _SubjectID = $scope.SubjectID;
        var _TopicID = $scope.TopicID;
        var _FacultyID = $scope.FacultyID;
        var _PlannedCoverage = $scope.PlannedCoverage;

        $http({
            method: 'POST',
            url: '/SessionMgmt/SaveSession',
            data: { SessionNo: _SessionNo, SessionName: _SessionName, SessionDate: _SessionDate, StartTime: _StartTime, EndTime: _EndTime, StudioID: _StudioID, ProgID: _ProgID, CourseID: _CourseID, SubjectID: _SubjectID, TopicID: _TopicID, FacultyID: _FacultyID, PlannedCoverage: _PlannedCoverage }
        }).then(function (result) {

        });
    }

    $scope.SelectSession = function (SessionID) {
        debugger;
        $scope.SessionID = SessionID;

        $http.get('/SessionMgmt/GetSessionsRRQ/' + SessionID)
        .then(function (result) {
            $scope.SessionRRQList = result.data;
        });
    };


    $scope.StartStopSession = function (btnType) {
        debugger;
        var _SessionID = $scope.SessionID;
        $scope.header = 'Success Message';
        if (btnType == 'Start')
        {
            $scope.body = 'Session started successfully!!';
        }
        else if (btnType == 'Stop')
        {
            $scope.body = 'Session stopped successfully!!';
        }
        
        $http({
            method: 'POST',
            url: '/SessionMgmt/StartStopSession',
            data: { SessionID: $scope.SessionID, Status: btnType }
        }).then(function (result) {
            $scope.GetSessions();
            $('#pop').modal('show')
        });
    };

    $scope.CreateRRQ = function () {
        debugger;
        var _SessionID = $scope.SessionID;
        var _RRQNo = $scope.RRQNo;
        var _ActiveFromDate = $scope.ActiveFromDate;
        var _ActiveToDate = $scope.ActiveToDate;
        window.location.href = "/SessionMgmt/AddQuestionsToRRQ/";
        //$http({
        //    method: 'POST',
        //    url: '/SessionMgmt/CreateRRQ',
        //    data: { RRQNo: _RRQNo, ActiveFromDate: _ActiveFromDate, ActiveToDate: _ActiveToDate}
        //}).then(function (result) {
        //    window.location.href = "/SessionMgmt/AddQuestionsToRRQ/";
        //});
    }

    $scope.ShowRRQDashboard = function (RRQ_ID) {
        window.location.href = "/SessionMgmt/RRQDashboard/" + RRQ_ID;
    };

});

myapp.controller('SessionAttendanceCntrl', function ($scope, $http) {
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

    $scope.GetSubjectsList = function () {
        $http.get('/CourseMgmt/GetSubject/' + $scope.CourseID)
        .then(function (result) {
            $scope.SubjectsList = result.data;
        });
    }

    $scope.GetStartedSessions = function () {
        $http.get('/SessionMgmt/GetStartedSessions/' + $scope.SubjectID)
        .then(function (result) {
            $scope.StartedSessionsList = result.data;
        });
    }

    $scope.InitiateAttendance = function () {
        debugger;
        var _SessionID = $scope.SessionID;
        $http({
            method: 'POST',
            url: '/StudentMgmt/InitiateAttendance',
            data: { SessionID: _SessionID }
        }).then(function (result) {
            window.location.href = "/SessionMgmt/StudentAttendance/";
        });
    };
});