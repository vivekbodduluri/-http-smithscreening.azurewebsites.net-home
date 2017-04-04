angular.module('todoManager', ['smart-table'])
    .controller('todoManagerCtrl', ['$scope', '$http', '$rootScope', function ($scope, $http, $rootScope) {
        $scope.showTable = false;
        $scope.title = '';
        $scope.Rating = '';
        $scope.Review = '';
        $scope.pages = 0;
        $scope.getList = function ()
        {
            $http.get('api/Reviews/Getreviews')
                .success(function (data, status, headers, config) {
                    $scope.todoList = data;
                    $scope.pages = $scope.todoList.length;
                    $scope.showTable = !$scope.showTable;
                });
        }
        $scope.saveReview = function()
        {
            item =
                {
                    UserName: $rootScope.username,
                    Rating: $scope.Rating,
                    ReviewTitle: $scope.title,
                    ReviewComment: $scope.Review
                };

         
            $http.post('/api/Reviews/PostReview', item)
                    .success(function (data, status, headers, config) {
                      
                        $scope.getList();
                    });
          
        }

        $scope.callServer = function callServer(tableState) {

            $scope.isLoading = true;

            var pagination = tableState.pagination;
            console.log(tableState.sort.predicate);
            var start = pagination.start || 0;     // This is NOT the page number, but the index of item in the list that you want to use to display the table.
            var number = pagination.number || 2;  // Number of entries showed per page.
            $http.get('api/Reviews/GetReview/' + start + '/' + 2 + '/' + tableState.sort.predicate + '/' + tableState.sort.reverse)
                .success(function (data, status, headers, config) {                  
                    $scope.todoList = data.reviews;
                    $scope.displayed = data.reviews; 
                    tableState.pagination.numberOfPages =  Math.ceil(data.totalPages / number);//set the number of pages so the pagination can update
                    $scope.isLoading = false;
                });
           
        };

     

      
    }]);