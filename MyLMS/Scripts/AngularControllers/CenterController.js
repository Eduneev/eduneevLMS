myapp.controller('CenterCntrl', function ($scope, $http) {
    GetCentersList();
    GetAccountID();
    $scope.SaveCenter = function () {
        debugger;
        var _CenterName = $scope.CenterName
        var _CenterCode = $scope.CenterCode
        var _Email = $scope.Email
        var _Landline1 = $scope.Landline1
        var _Landline2 = $scope.Landline2
        var _Mobile = $scope.Mobile
        var _Address = $scope.Address
        var _PinCode = $scope.PinCode

        $http({
            method: 'POST',
            url: '/CenterMgmt/SaveCenter',
            data: { CenterName: _CenterName, CenterCode: _CenterCode, Email: _Email, Landline1: _Landline1, Landline2: _Landline2, Mobile: _Mobile, Address: _Address, PinCode: _PinCode }
        }).then(function (result) {
            alert('Saved Successfully!!');
        });
    }

    function GetCentersList() {
        $http.get('/CenterMgmt/GetCenters')
        .then(function (result) {
            $scope.CentersList = result.data;
        });
    }

     function GetAccountID() {
        $http.get('/CenterMgmt/GetAccountID')
        .then(function (result) {
            $scope.AccountID = result.data;
        });
    }
});
