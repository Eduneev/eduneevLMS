myapp.controller('HelpdeskCntrl', function ($scope, $http) {

    $scope.SaveNewTicket = function () {
        debugger;
        $http({
            method: 'POST',
            url: '/Helpdesk/SaveNewTicket',
            data: { Subject: $scope.Subject, TicketContent: $scope.TicketContent }
        }).then(function (result) {

        });
    }

    $scope.GetMyTickets = function () {
        debugger;
        $http.get('/Helpdesk/GetMyTickets')
            .then(function (result) {
                $scope.TicketsList = result.data;
            });
    }

    $scope.GetTicketByID = function () {
        debugger;
        $http.get('/Helpdesk/GetTicketByTicketID')
            .then(function (result) {
                $scope.TicketInfo = result.data;
                GetTicketDetails();
            });
    }

    function GetTicketDetails() {
        debugger;
        $http.get('/Helpdesk/GetTicketDetails')
            .then(function (result) {
                $scope.TicketDetails = result.data;
            });
    }

});