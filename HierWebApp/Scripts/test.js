var app = angular.module("Emp", ['datatables']);

app.controller("EmpController", function ($scope, $http, $window, $filter) {
    $scope.getParam = function (param) {
        var url = window.location.href.slice(window.location.href.indexOf('?') + 1);
        var a = url.split("=");
        return a[1];
    }
    $scope.GetEmployeeList = function () {
       
            $http({
                method: 'POST',
                url: '/EmployeeDataAccessLayer/GetAllEmployees',
                data: {
                    
                }
            }).then(function (d) {
                $scope.EmployeeList = d.data;
            })
        
    }
    $scope.GetCityList = function () {
       
            $http({
                method: 'POST',
                url: '/EmployeeDataAccessLayer/GetCityData',
                data: {
                    
                }
            }).then(function (d) {
                $scope.CityList = d.data;
            })
    }
    $scope.PostNewEmployee = function () {
       
            $http({
                method: 'POST',
                url: '/EmployeeDataAccessLayer/Addemployee',
                data: {
                    employee:$scope.emp
                }
            }).then(function (d) {
                $window.location = "/EmployeeDataAccessLayer/Index";
            })
    }
     $scope.PostEditEmployee = function () {
         $scope.emp.EmployeeId = $scope.getParam("id");
            $http({
                method: 'POST',
                url: '/EmployeeDataAccessLayer/UpdateEmployee',
                data: {
                    employee:$scope.emp
                }
            }).then(function (d) {
                $window.location = "/EmployeeDataAccessLayer/Index";
            })
    }
     $scope.PostDeleteEmployee = function (id) {
            $http({
                method: 'POST',
                url: '/EmployeeDataAccessLayer/DeleteEmployee',
                data: {
                    id:id
                }
            }).then(function (d) {
                $scope.GetEmployeeList();
            })
    }
    $scope.GetEmployeeByID = function () {
            $http({
                method: 'POST',
                url: '/EmployeeDataAccessLayer/GetEmployeeData',
                data: {
                    id:$scope.getParam("id")
                }
            }).then(function (d) {
                $scope.emp = d.data;
            })
    }

})