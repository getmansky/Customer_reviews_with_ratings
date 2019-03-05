angular.module('CustomerReviews.Web')
    .controller('CustomerReviews.Web.customerReviewWidgetController', ['$scope', 'CustomerReviews.WebApi', 'platformWebApp.bladeNavigationService', function ($scope, reviewsApi, bladeNavigationService) {
        var blade = $scope.blade;
        var filter = {};

        function refresh() {
            $scope.loading = true;
            reviewsApi.search(filter, function (data) {
                $scope.loading = false;
                $scope.totalCount = data.totalCount;

                var ratesAverage = 0;
                if (data.totalCount !== 0) {
                    data.results.forEach(function (element) {
                        ratesAverage += element.rating;
                    });
                    ratesAverage = ratesAverage * 1.0 / data.totalCount;
                    $scope.reviewsAny = true;
                }
                $scope.ratesAverage = ratesAverage;
            });
        }

        $scope.openBlade = function () {
            if ($scope.loading || !$scope.totalCount)
                return;

            var newBlade = {
                id: "reviewsList",
                filter: filter,
                title: 'Customer reviews for "' + blade.title + '"',
                controller: 'CustomerReviews.Web.reviewsListController',
                template: 'Modules/$(CustomerReviews.Web)/Scripts/blades/reviews-list.tpl.html'
            };
            bladeNavigationService.showBlade(newBlade, blade);
        };

        $scope.$watch("blade.itemId", function (id) {
            filter.productIds = [id];

            if (id) refresh();
        });
    }]);
