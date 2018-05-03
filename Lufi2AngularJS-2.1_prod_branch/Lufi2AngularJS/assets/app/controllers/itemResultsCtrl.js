angular.module('itemResults', ['ui.bootstrap'])
.controller('itemResultsCtrl', ['$scope', '$location', '$http', 'itemSearch', '$modal', '$rootScope', function ($scope, $location, $http, $itemSearch, $modal, $rootScope) {
    var openItemDetailAll = function (isnGroupItem) {
        $itemSearch.setSelectedISNGroup(isnGroupItem);
        $location.path('/itemDetail');
    };



    var openItemDetail = function (isnGroupItem) {
        $itemSearch.searchISNORProductCode(isnGroupItem.productList[0], function (response) {
            if (response.ngroups == 0) {
                jQuery('.not-found-alert').modal('show');
            } else if (response.isnGroup.length == 1) {
                $itemSearch.setSelectedISNGroup(response.isnGroup[0]);
                $location.path('/itemDetail');
            } else {
                $location.path("/itemResults");
            }
        }, function (err) {
            alert(err);
        });
    }



    var showResults = function () {
        var cachedResult = $itemSearch.getCachedResult();
        if (cachedResult === undefined) {
            $location.path("/itemSearch");
        } else {
            $scope.response = cachedResult;
            $scope.page = {};
            $scope.page.numberOfItems = $scope.response.ngroups;
            $scope.page.currentPage = $itemSearch.getCachedQueryParam().currentPage;
            $scope.page.maxSize = 5;
            $scope.page.itemsPerPage = $itemSearch.MAX_NUMBER_OF_ROWS;
            $scope.page.numberOfPages = Math.ceil($scope.response.ngroups / $scope.page.itemsPerPage);
            var cachedParam = $itemSearch.getCachedQueryParam();
            $scope.selectedBrands = cachedParam.selectedBrands ? cachedParam.selectedBrands : [];
            $scope.selectedColors = cachedParam.selectedColors ? cachedParam.selectedColors : [];
            $scope.selectedItemSizes = cachedParam.selectedItemSizes ? cachedParam.selectedItemSizes : [];
            $scope.selectedFOBs = cachedParam.selectedFOBs ? cachedParam.selectedFOBs : [];
        }
    }

    $scope.listClick = function (item) {
        openItemDetail(item);
    };


    $scope.showAvailableItemsAction = function () {
        $scope.showAvailableItems = !$scope.showAvailableItems;
        var queryParam = $itemSearch.getCachedQueryParam();
        delete queryParam.buyable;
        if ($scope.showAvailableItems) {
            queryParam.buyable = true;
        }
        $scope.filter(queryParam)
    };


    $scope.pageChanged = function () {
        var queryParam = $itemSearch.getCachedQueryParam();
        queryParam.start = ($scope.page.currentPage - 1) * $itemSearch.MAX_NUMBER_OF_ROWS;
        queryParam.currentPage = $scope.page.currentPage;
        $scope.filter(queryParam)
    };

    var _allSelectedFacetCriteria = [];

    $scope.facetItemClicket = function (array, value, filterFacet) {
        var index = -1;
        for (var i = 0; i < array.length; i++) {
            if (array[i].name === value.name) {
                index = i;
                break;
            }
        }

        if (index != -1) {
            array.splice(index, 1);

            //find in allSelected
            for (var p = 0; p < _allSelectedFacetCriteria.length; p++) {
                if((_allSelectedFacetCriteria[p].name === value.name) &&
                   (_allSelectedFacetCriteria[p].filterFacet === filterFacet)) {

                    _allSelectedFacetCriteria.splice(p, 1);
                    break;
                }
            }

        } else {
            value.filterFacet = filterFacet;
            array.push(value);
            _allSelectedFacetCriteria.push(value);
        }

        var queryParam = $itemSearch.getCachedQueryParam();
        queryParam.selectedBrands = $scope.selectedBrands;
        queryParam.selectedColors = $scope.selectedColors;
        queryParam.selectedItemSizes = $scope.selectedItemSizes;
        queryParam.selectedFOBs = $scope.selectedFOBs;

        var filterFacetArray = [];
        var filterSet = {};
        for (var p = 0; p < _allSelectedFacetCriteria.length; p++) {
            if (_allSelectedFacetCriteria[p].filterFacet in filterSet) {
                continue;
            } else {
                filterFacetArray.push(_allSelectedFacetCriteria[p].filterFacet);
                filterSet[_allSelectedFacetCriteria[p].filterFacet] = true;
            }
        }
        queryParam.filterFacet = filterFacetArray;
        queryParam.start = 0;
        queryParam.currentPage = 1;
        $scope.filter(queryParam)
    }

    $scope.itemExist = function (array, value) {
        for (var i = 0; i < array.length; i++) {
            if (array[i].name === value.name) {
                return true;
            }
        }
        return false;
    }

    $scope.filter = function (queryParam) {
        $itemSearch.filter(queryParam, function (response) {
            if (response.ngroups == 0) {
                jQuery('.not-found-alert').modal('show');
            } else {
                showResults();
            }
        }, function (err) {
            alert(err);
        });
    };

    $scope.openItemSizes = function (isnGroupItem) {
        if (isnGroupItem.colorSize.length > 1) {
            var modalInstance = $modal
                    .open({
                        templateUrl: 'itemColorSize.html',
                        controller: 'itemColorSizeCtrl',
                        resolve: {
                            isnGroupItem: function () {
                                return isnGroupItem;
                            }
                        }
                    });
        } else {
            openItemDetail(isnGroupItem);
        }
    };


    showResults();


    var iScrollWrappers = {};
    $scope.refreshScroll = function (scrollId) {
        setTimeout(function () {
            var scroll = iScrollWrappers[scrollId];
            if (scroll) {
                scroll.refresh();
                scroll.scrollToElement('td', 500, null, null, null);
            } else {
                iScrollWrappers[scrollId] = new IScroll('#' + scrollId, { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });

            }
        }, 500);
    };



    $scope.itemTitleClass = function (isnGroup) {
        return isnGroup.isnGroupAvailable ? 'itemResultsTitle' : 'zeroInventoryGroup';
    }
    $scope.showAvailableItems = false;

    $scope.left = $rootScope.left;
    $scope.moveFacets = function () {
        $scope.left = !$scope.left;
        $rootScope.left = $scope.left;
    }

}]);