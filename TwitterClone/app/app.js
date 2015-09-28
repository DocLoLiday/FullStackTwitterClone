
var app = angular.module("twitterClone", ['ngRoute']);

app.config(['$routeProvider','$locationProvider', function ($routeProvider, $locationProvider) {

    $routeProvider
      .when('/', {
          templateUrl: '/Account/Login'
      })
        
      .when('/timeline/:id', {
          controller: 'TimelineController',
          templateUrl: 'Home/Timeline'
      })
        .when('/register', {
            templateUrl: '/Account/Register'
        })
        .when('/profile/:id', {
            controller: 'ProfileController',
            templateUrl: 'Home/UserProfile'
        })
      .otherwise({
          redirectTo: '/'
      });


}]);



app.controller('TimelineController', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {
   

    $http.get("/App/Timeline/" + $routeParams.id).
        success(function (data, status) {
            $scope.data = JSON.parse(data);
            $scope.tweets = $scope.data.Tweets;
            $scope.user = $scope.data.User;
        }).error(function (data, status) {
            $scope.alerts.push(data);
        });

    

}]);

app.controller('ProfileController', ['$scope', '$http', '$routeParams', function ($scope, $http, $routeParams) {


    $http.get("/App/UserProfile/" + $routeParams.id).
        success(function (data, status) {
            $scope.data = JSON.parse(data);
            $scope.tweets = $scope.data.Tweets;
            $scope.user = $scope.data.User;
            $scope.followers = $scope.data.Followers;
        }).error(function (data, status) {
            $scope.alerts.push(data);
        });



}]);



