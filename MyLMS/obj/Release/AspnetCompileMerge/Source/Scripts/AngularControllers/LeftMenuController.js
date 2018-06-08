myapp.controller('LmsMenuCntrl', function ($scope, $http) {
    GetMenus();
    function GetMenus() {
        debugger;
        $http.get('/LeftMenu/GetMenus')
        .then(function (result) {
            $scope.MenusList = result.data;
        });
    };
});

