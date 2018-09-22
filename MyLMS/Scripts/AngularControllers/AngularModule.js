var myapp;
(function () {
    myapp = angular.module('MyModule', ['ngSanitize', 'ngWebSocket']);
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
    },

    Action: {
        START: "start",
        STOP: "stop",
        END: "end",
        NEXT: "next",
        TEACHERCONNECTION: "teacher",
        CLASSROOMCONNECTION: "classroom"
    },

    Message: {
        PROFILE: 'profile',
        TYPE: 'type',
        SESSIONID: 'SessionID',
        ACTION: 'action',
        RRQID: "RrqID",
        QID: "qID",
        WSID: "wsID"
    }
});