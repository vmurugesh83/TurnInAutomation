angular.module('itemDetail', ['ui.bootstrap'])
.controller('itemDetailCtrl', ['$scope', '$state', '$modal', 'itemSearch', 'itemDetail', 'orderCart', 'numberOnlyKeyPressValidator', 'bigTicketValidate', 'itemToLocate', 'loggerService', 'btProp', 'securityService',
    function ($scope, $state, $modal, $itemSearch, $itemDetail, $orderCart, $numberOnlyKeyPressValidator, $bigTicketValidate, $itemToLocate, $loggerService, $btProp, $securityService) {
    $scope.queryParameters = {}
    $scope.numberValidator = $numberOnlyKeyPressValidator;

    $scope.refreshIScroll = function () {
        setTimeout(function () {
            $scope.$parent.myScroll['ItemDetailWrapper'].refresh();
        }, 500);
    };
    

    // modal

    $scope.openItemDescription = function (currentItem) {
        var modalInstance = $modal
                .open({
                    templateUrl: 'html/item/itemDetailDescription.html',
                    controller: 'itemDescriptionCtrl',
                    resolve: {
                        currentItem: function () {
                            return $scope.currentItem;
                        },
                        isCart: function () { return false; }
                    }
                });

    };

    $scope.orderSimilarItems = function (isnGroup) {
        //$scope.isnGroup = isnGroup;
        var searchParam = $itemSearch.getCachedQueryParam();
        var originalUPC = searchParam.originalUPC;
        var upcIndex = 0;
        if (originalUPC != undefined && originalUPC != "") {
            for (var i = 0, len = isnGroup.length; i < len; i++) {
                if (isnGroup[i].id === parseInt(originalUPC)) {
                    upcIndex = i
                }
            }
            $scope.selectedRow = upcIndex;
            $scope.updateCurrentItem(isnGroup[upcIndex])
            
        } else {
            $scope.selectedRow = 0;
            $scope.updateCurrentItem(isnGroup[0])

        }
        $loggerService.log(isnGroup); 
    }, function (err) {
    };


    $scope.openGWP = function (currentItem) {
        var modalInstance = $modal
                .open({
                    templateUrl: 'html/item/itemDetailGWP.html',
                    controller: 'gwpCtrl',
                    resolve: {
                        currentItem: function () {
                            return $scope.currentItem;
                        }
                    }
                });
    };
    $scope.openZipCodeValidation = function (currentItem) {
        var modalInstance = $modal
            .open({
                templateUrl: 'zipCodeValidation.html',
                controller: 'zipCodeValidationCtrl',
                resolve: {
                    currentItem: function () {
                        return $scope.currentItem;
                    }
                }
            });
    };

    $scope.openOrderCart = function (currentItem) {
        var qty = $scope.txtQtyInput
        if (currentItem.itemtype == "BGT") {
            $scope.openZipCodeValidation(currentItem)
        } else {
            try {
                $orderCart.orderLine.addOrderLine(currentItem, qty);
                $state.go('orderCart');
            } catch (ex) {

                swal({ title: "Cannot Add Item", text: ex.message, showConfirmButton: true });   
            }
        }
    };

    $scope.openItemLocate = function (currentItem) {
        $itemToLocate.set(currentItem);
        $state.go('itemLocate');

};


    // modal
    $scope.isnGroup = $itemSearch.getSelectedISNGroup();

    $scope.isPricingOnlySingleItems = $btProp.getProp('isSingleItemPricingOnly');
    $scope.isPricingOnlySingleItems = ($scope.isPricingOnlySingleItems !== true) ? false : true;

    $scope.maxItemsForBulkPricing = parseInt($btProp.getProp('maxItemsAllowedToCallBulkPricingOnItemDetails'));
    $scope.maxItemsForBulkPricing = (isFinite($scope.maxItemsForBulkPricing) && ($scope.maxItemsForBulkPricing > -1)) ? $scope.maxItemsForBulkPricing : 10;
    

    var _getItemPricing = function (item) {

        var configObj;

        //if item is passed, find in list of unsorted $itemSearch.getSelectedISNGroup()
        var index = -1;
        if (angular.isObject(item)) {
            index = $scope.isnGroup.productList.indexOf(item);
        }

        if (index > -1) {
            configObj = { index: index };
        }

        _callItemService(configObj);
    };

    var _callItemService = function (configObj) {
        $itemDetail($scope.isnGroup, function (isnGroup) {
        $scope.isnGroup = isnGroup;
            $scope.refreshIScroll();
        $loggerService.log(isnGroup);
    }, function (err) {
            $scope.refreshIScroll();
        }, configObj);
    };

        //load whole inventory and price in background
    if (!$scope.isPricingOnlySingleItems && ($scope.isnGroup.productList.length <= $scope.maxItemsForBulkPricing)) {
        _callItemService({ isBlocked: false });
    }

    $scope.updateCurrentItem = function (item) {
        if (!angular.isDefined(item.itemDetail) || (item.itemDetail === null) ||
            !angular.isDefined(item._AvailableQty) || (item._AvailableQty === null)) {
            _getItemPricing(item);
        }
    $scope.currentItem = item;
    if ($scope.currentItem.webid) {
        var webidstring = $scope.currentItem.webid;
        $scope.currentItem.webid = webidstring.replace(/,/g, ', ');
    }
    
    if ($scope.currentItem.colorimageid) {
        $scope.ImageURL = serviceURL + "/image/BonTon/" + $scope.currentItem.colorimageid;
    } else if ($scope.currentItem.imageid) {
        $scope.ImageURL = serviceURL + "/image/BonTon/" + $scope.currentItem.imageid;
    } else {
        $scope.ImageURL = "/assets/images/NotAvailable.jpg";
    }
    $scope.txtQtyInput = 1;
        //sky itemlist response does not return specialhandlingcode.. It is in solr and being mapped to the sky response
    if ($scope.currentItem.itemDetail) {
        $scope.currentItem.itemDetail.ComputedPrice.Extn._ExtnSpecialHandlingCode = $scope.currentItem.specialhandlingcode;
    }
    if (item.productname === "" || item.productname == undefined) {
        $scope.showProductName = false;
        } else {
        $scope.showProductName = true;
    }
        //**PICKUPALLOWED
        if (item.pickupallowed === "" || item.pickupallowed == undefined) {
        $scope.showpickUpAllowed = false;
        } else {
        $scope.showPickUpAllowed = true;
    }

    };

    $scope.disableCartButton = function (item) { 
        var returnVal = true;
        if (item == undefined || item.itemDetail == undefined || (item.itemDetail === null) ||
            !angular.isDefined(item._AvailableQty) || (item._AvailableQty === null)) {
            returnVal = true;
        } else {
            var qty = $scope.txtQtyInput
            if (parseInt(item._AvailableQty) < qty || parseInt(item._AvailableQty) == '0' ||
                item.itemDetail.ComputedPrice._RetailPrice == '-999999.99' || item.itemDetail.ComputedPrice._RetailPrice == '-8888888.88') {
                returnVal = true;
            } else if (item.isgwp == "Y") {
                if ($securityService.isCsr()) {
                    returnVal = false;
                }else{
                    returnVal = true;
                }
            }
            else {
                returnVal = false;
            }
        }
        return returnVal;
    };


   $scope.selectedRow = null;
   $scope.setClickedRow = function (index) {
     $scope.selectedRow = index;
   }

   $scope.pricingServiceDown = function (item) {

       if (item == undefined || item.itemDetail == undefined) {
           item = false;
        } else {
            if (item.itemDetail.ComputedPrice._RetailPrice == '-999999.99' || item.itemDetail.ComputedPrice._RetailPrice == '-8888888.88') {
                item = true;
            } else {
                item = false;
                }
        }
    return item;
    };

}])
.controller('itemDescriptionCtrl', ['$scope', '$modalInstance', 'currentItem', 'isCart','$state','orderCart', '$modal', 'securityService',
				function ($scope, $modalInstance, currentItem, isCart, $state, $orderCart, $modal, $securityService) {

    $scope.currentItem = currentItem;
    $scope.isCart = isCart;

	$scope.close = function () {
		$modalInstance.dismiss('closed');
	};

	$scope.openOrderCart = function (currentItem) {
	    if (currentItem.itemtype == "BGT") {
	        $modalInstance.dismiss('closed');
	        $scope.openZipCodeValidation(currentItem)
	    } else {
	        try{
	            $orderCart.orderLine.addOrderLine(currentItem, "1");
	            $state.go('orderCart');
	            $modalInstance.dismiss('closed');
	        } catch (ex) {
	            swal({ title: "Cannot Add Item", text: ex.message, showConfirmButton: true });
	        }
	    }
	};

	$scope.disableCartButton = function (item) {
	    var returnVal = true;
	    if (item == undefined || item.itemDetail == undefined || (item.itemDetail === null) ||
            !angular.isDefined(item._AvailableQty) || (item._AvailableQty === null)) {
	        returnVal = true;
	    } else {
	        if ( parseInt(item._AvailableQty) == '0' ||
                item.itemDetail.ComputedPrice._RetailPrice == '-999999.99' || item.itemDetail.ComputedPrice._RetailPrice == '-8888888.88') {
	            returnVal = true;
	        } else if (item.isgwp == "Y") {
	            if ($securityService.isCsr()) {
	                returnVal = false;
	            } else {
	                returnVal = true;
	            }
	        }
	        else {
	            returnVal = false;
	        }
	    }
	    return returnVal;
	};

    $scope.openZipCodeValidation = function (currentItem) {
        var modalInstance = $modal
            .open({
                templateUrl: 'zipCodeValidation.html',
                controller: 'zipCodeValidationCtrl',
                resolve: {
                    currentItem: function () {
                        return $scope.currentItem;
                    }
                }
            });
    };



	$scope.showValidDesc = function (desc) {
	    if (desc === "" || desc == undefined || desc == "None" || desc == "Not Assigned") {
	        desc = false;
	    } else {
	        desc = true;
	    }
	    return desc;
	};
}])

.controller('gwpCtrl', ['$scope', '$modalInstance', 'currentItem', 
				function ($scope, $modalInstance, currentItem) {

				    $scope.currentItem = currentItem;

				    $scope.close = function () {
				        $modalInstance.dismiss('closed');
				    };

}])
.controller('zipCodeValidationCtrl', ['$scope', '$modalInstance', 'currentItem', '$state', 'orderCart', 'bigTicketValidate', 'numberOnlyKeyPressValidator', 'loggerService', 'btProp',
				function ($scope, $modalInstance, currentItem, $state, $orderCart, $bigTicketValidate, $numberOnlyKeyPressValidator, $loggerService, btProp) {
				    $scope.isBigTicketPurchaseDisabled = btProp.getProp("isBigTicketPurchaseDisabled");

				    $scope.isBigTicketPurchaseDisabled = $scope.isBigTicketPurchaseDisabled ? true : false;

				    $scope.numberValidator = $numberOnlyKeyPressValidator;
				    $scope.currentItem = currentItem;
				    $scope.isValid = true;

                


				    $scope.validate = function () {
				        var zip = $scope.txtZip
				        $bigTicketValidate.bigTicketPromise(zip).then(function (response) {
				            $loggerService.log(response);
				            if (response.data.ValidateBigTicketZipCodeResp._isValid == "true") {
				                $scope.openOrderCart(currentItem)
				            } else {
				            $scope.isValid = false;
				            }
				        });
				    };

				    $scope.close = function () {
				        $modalInstance.dismiss('closed');
				        };

				    $scope.openOrderCart = function (currentItem) {
				        try{
				            $orderCart.orderLine.addOrderLine(currentItem, "1", true);
				            $state.go('orderCart');
				            $modalInstance.dismiss('closed');
				        } catch (ex) {

				            swal({ title: "Cannot Add Item", text: ex.message, showConfirmButton: true });   
				        }
				    };



}]);