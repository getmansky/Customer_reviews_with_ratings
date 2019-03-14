angular.module('CustomerReviews.Web')
.factory('CustomerReviews.WebApi', ['$resource', function ($resource) {
    return $resource('api/customerReviews', {}, {
        search: { method: 'POST', url: 'api/customerReviews/search' },
        average: {
            method: 'GET',
            url: 'api/customerReviews/average/:id'
        },
        update: { method: 'PUT' },
        getRatings: {
            method: 'GET',
            url: 'api/customerReviews/getReviewRatings/:id'
        }
    });
}]);
