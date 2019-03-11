angular.module('CustomerReviews.Web')
.factory('CustomerReviews.WebApi', ['$resource', function ($resource) {
    return $resource('api/customerReviews', {}, {
        search: { method: 'POST', url: 'api/customerReviews/search' },
        update: { method: 'PUT' },
        getRatings: {
            method: 'GET',
            url: 'api/customerReviews/getReviewRatings/:id'
        }
    });
}]);
