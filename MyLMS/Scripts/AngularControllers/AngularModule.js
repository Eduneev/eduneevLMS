var myapp;
(function () {
    myapp = angular.module('MyModule', ['ngSanitize', 'ngWebSocket','ngQuickDate']);
})();

myapp.constant('Constants', {
    Events: {
        CONNECTION: "Connection",
        MESSAGE: "Message",
        DISCONNECTION: "Disconnect",
        CLOSE: "Close"
    },

    Profile: {
        RRQ: "rrq",
        TWOWAYCALL: "2WayCall"
    },

    Action: {
        START: "start",
        STOP: "stop",
        END: "end",
        NEXT: "next",
        TEACHERCONNECTION: "teacher",
        CLASSROOMCONNECTION: "classroom",
        JOIN: "join",
        LEAVE: "leave"
    },

    Message: {
        PROFILE: 'profile',
        TYPE: 'type',
        SESSIONID: 'SessionID',
        ACTION: 'action',
        RRQID: "RrqID",
        QID: "qID",
        WSID: "wsID",
        CHANNEL: "channel",
        STUDIONAME: "StudioName",
        STUDIOID: "StudioID",
        CLASSROOMNAME: "ClassroomName",
        CLASSROOMID: "ClassroomID"
    }
});