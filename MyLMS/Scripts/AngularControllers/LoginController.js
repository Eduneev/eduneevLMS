myapp.controller('LoginCntrl', function ($scope, $http) {
    $scope.Test = "Forget Password?";

    $scope.ValidateUser = function () {
//        debugger;
        var _UserName = $scope.UserName;
        var _Password = $scope.Password;

        //var _UserName = 'just.chandan@gmail.com';
        //var _Password = 'zerosoft';

        $http.get('/Home/ValidateUser', {
            params: { UserName: _UserName, Password: _Password }
        }).then(function (result) {
            if (result.data !="false")
            {              
                window.location.href = result.data;
            }
            else
            {
                window.location.href = "/Home/Login/";
            }
            console.log(result.data);
        }, function (result) {
            //some error
        });
    };
});