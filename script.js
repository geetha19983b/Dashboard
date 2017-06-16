// Code goes here

var app = angular.module('app', []);

app.controller('CompaniesController', ['$scope', '$http',
  function($scope, $http) {
    $http.get('companies.json').success(function(data) {
      $scope.companies = [];
      $scope.technologies = [];
      angular.forEach(data, function(company) {
        $scope.companies.push(company.title);
        angular.forEach(company.technologies, function(tech) {
          if ($scope.technologies.indexOf(tech) == -1) {
            $scope.technologies.push(tech);
          }
        });
      });
    });
	 $http.get('provider.json').success(function(data) {
      $scope.extracts = [];
	  
    
      angular.forEach(data, function(extract) {
        $scope.extracts.push(extract);
       
      });
    });

  }
]);