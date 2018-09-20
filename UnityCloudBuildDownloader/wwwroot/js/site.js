var app = angular.module('buildsApp', ['ngResource']);

app.factory("builds", function ($scope, $resource) {
    return $resource("/builds/" + $scope.platform);
});

app.controller("buildsCtrl", function ($scope, $resource) {
    $resource("/builds/:platform", { platform: $scope.platform })
        .query(function (data) {
            $scope.builds = data;
        }, function (err) {
            console.error("Error occured: ", err);
        });
});