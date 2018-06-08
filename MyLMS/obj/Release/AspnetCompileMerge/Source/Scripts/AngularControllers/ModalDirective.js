myapp.directive('modal', function () {
    return {
        restrict: 'EA',
        scope: {
            title: '=modalTitle',
            header: '=modalHeader',
            body: '=modalBody',
            footer: '=modalFooter',
            callbackbuttonleft: '&ngClickLeftButton',
            callbackbuttonright: '&ngClickRightButton',
            handler: '=lolo'
        },
        templateUrl: '../Content/defaultmodalpopup.html',
        transclude: true,
        controller: function ($scope) {
            $scope.handler = 'pop';
        },
    };
});