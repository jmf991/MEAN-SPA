var app = angular.module("MainCtrl", []);

app.controller("MainController", function ($scope) {
    $scope.tagline = 'MainController Ok';
    $scope.main = {
        username: "Parker"
    };
});

app.controller("MyNestedCtrl", function ($scope) {
});