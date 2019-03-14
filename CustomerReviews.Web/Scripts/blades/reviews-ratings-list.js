angular.module('CustomerReviews.Web')
    .controller('CustomerReviews.Web.reviewsListRatingsController', ['$scope', 'CustomerReviews.WebApi', 'platformWebApp.bladeUtils', 'uiGridConstants', 'platformWebApp.uiGridHelper',
        function ($scope, reviewsApi, bladeUtils, uiGridConstants, uiGridHelper) {
            $scope.uiGridConstants = uiGridConstants;

            var blade = $scope.blade;
            var bladeNavigationService = bladeUtils.bladeNavigationService;

            blade.refresh = function () {
                blade.isLoading = true;
                reviewsApi.getRatings(angular.extend({
                    id: blade.currentEntityId
                }), function (data) {
                    blade.isLoading = false;
                    $scope.pageSettings.totalItems = data.totalCount;
                    blade.currentEntities = data.results;

                    console.log(data);
                });
            };

            blade.selectNode = function (data) {
                $scope.selectedNodeId = data.id;

                console.log(data);
            }

            function openBladeNew() {
                $scope.selectedNodeId = null;

                var newBlade = {
                    id: 'storeDetails',
                    currentEntity: {},
                    title: 'stores.blades.new-store-wizard.title',
                    subtitle: 'stores.blades.new-store-wizard.subtitle',
                    controller: 'virtoCommerce.storeModule.newStoreWizardController',
                    template: 'Modules/$(VirtoCommerce.Store)/Scripts/wizards/newStore/new-store-wizard.tpl.html'
                };
                bladeNavigationService.showBlade(newBlade, blade);
            }

            blade.headIcon = 'fa-comments';

            blade.toolbarCommands = [
                {
                    name: "platform.commands.refresh", icon: 'fa fa-refresh',
                    executeMethod: blade.refresh,
                    canExecuteMethod: function () {
                        return true;
                    }
                },
            ];

            // simple and advanced filtering
            var filter = $scope.filter = blade.filter || {};

            filter.criteriaChanged = function () {
                if ($scope.pageSettings.currentPage > 1) {
                    $scope.pageSettings.currentPage = 1;
                } else {
                    blade.refresh();
                }
            };

            // ui-grid
            $scope.setGridOptions = function (gridOptions) {
                uiGridHelper.initialize($scope, gridOptions, function (gridApi) {
                    uiGridHelper.bindRefreshOnSortChanged($scope);
                });
                bladeUtils.initializePagination($scope);
            };

        }]);
