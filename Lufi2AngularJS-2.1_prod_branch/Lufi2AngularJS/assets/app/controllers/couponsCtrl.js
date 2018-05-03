angular.module('coupons', ['ui.bootstrap'])
.controller('couponsCtrl', ['$scope', '$location', '$modal', '$log', function ($scope, $location, $modal, $log) {

    //mock orderlines
    $scope.orderlines = [
        {
            imageSrc: "assets/images/count.jpg",
            title: "Sesame Street&reg; - The Count Adult CostumeWeb exclusive!",
            price: "68.00",
            unitPrice: "136.00",
            yellowDot: false,
            brand: "Emerald Sundae",
            color: "Emerald",
            size: "Small",
            available: 500,
            upc: 98400222,
            itemId: 445301268573,
            imageId: 11235,
            webIds: [1089772, 23156, 15648, 12346],
            vendorStyle: "CHK1023093",
            unitOfMeasure: "Each",
            longDesc: "Featured in black Adjustable straps Padded bust Glitter accents all over Ruched sides Side slit Lined Polyester / spandex Imported",
            storePickup: true,
            shipToCust: false,

            giftregistry: 'FSK8A34',
            quantity: 1,
            ispriceoverride: true,
            priceoverride: "59.00",
            isorderlinechecked: false
        },
        {
            imageSrc: "assets/images/mixer.jpg",
            title: "KitchenAid&reg; Artisan&reg; Design 5-Qt. Glass Bowl Stand Mixer + Free Food Grinder Attachment",
            price: "239.00",
            unitPrice: "239.00",
            yellowDot: false,
            brand: "Emerald Sundae",
            color: "Emerald",
            size: "Small",
            available: 500,
            upc: 700154422,
            itemId: 445301268573,
            imageId: 11235,
            webIds: [7154422, 23156, 15648, 12346],
            vendorStyle: "CHK1023093",
            unitOfMeasure: "Each",
            longDesc: "Featured in black Adjustable straps Padded bust Glitter accents all over Ruched sides Side slit Lined Polyester / spandex Imported",
            storePickup: true,
            shipToCust: false,

            giftregistry: 'FSK8A34',
            quantity: 1,
            ispriceoverride: false,
            priceoverride: "",
            isorderlinechecked: false
        },
        {
            imageSrc: "assets/images/count.jpg",
            title: "Sesame Street&reg; - The Count Adult CostumeWeb exclusive!",
            price: "68.00",
            unitPrice: "136.00",
            yellowDot: false,
            brand: "Emerald Sundae",
            color: "Emerald",
            size: "Small",
            available: 500,
            upc: 98400222,
            itemId: 445301268573,
            imageId: 11235,
            webIds: [1089772, 23156, 15648, 12346],
            vendorStyle: "CHK1023093",
            unitOfMeasure: "Each",
            longDesc: "Featured in black Adjustable straps Padded bust Glitter accents all over Ruched sides Side slit Lined Polyester / spandex Imported",
            storePickup: true,
            shipToCust: false,

            giftregistry: 'FSK8A34',
            quantity: 1,
            ispriceoverride: true,
            priceoverride: "59.00",
            isorderlinechecked: false
        }
    ];
    //end mock orderlines
    $scope.customerInfo = function () { $location.path('/customerInfo'); };
    $scope.deleteOrderLine = function (line) {
        var index = $scope.orderlines.indexOf(line);
        $scope.orderlines.splice(index, 1);
    };
    $scope.removePriceOverride = function () {
        angular.forEach($scope.orderlines, function (orderline) {
            if (orderline.isorderlinechecked && orderline.ispriceoverride) {
                orderline.priceoverride = "";
                orderline.ispriceoverride = false;
            }
        });
    };

    $scope.incrementItems = function (orderline) {

        if (angular.isObject(orderline)) {
            ++orderline.quantity;
        }
        return false;
    };
    $scope.decrementItems = function (orderline) {

        if (angular.isObject(orderline)) {
            --orderline.quantity;
            if (orderline.quantity < 0) {
                orderline.quantity = 0;
            }
        }
        return false;
    };

    $scope.getIsPriceDiscounted = function (orderline) {
        if (orderline.ispriceoverride) {
            return true;
        } else {
            return false;
        }
    };

    //master check all button/checkbox and "update gift registry" and "remove price override" 
    $scope.checkButtonFont = 'fa-square-o';
    var checkButtonIsChecked = false;
    $scope.priceoverrideDisable = true;
    $scope.updateGiftRegistryDisable = true;
    var changeMasterCheckboxDisplay = function (isCheckIt) {
        if (isCheckIt) {
            $scope.checkButtonFont = 'fa-check-square-o';
            checkButtonIsChecked = true;
        } else {
            $scope.checkButtonFont = 'fa-square-o';
            checkButtonIsChecked = false;
        }
    };
    $scope.toggleCheckAll = function () {
        //if checkButtonIsChecked then uncheck ALL checkboxes
        if (checkButtonIsChecked) {
            changeMasterCheckboxDisplay(false);
            angular.forEach($scope.orderlines, function (orderline) {
                orderline.isorderlinechecked = false;
            });
            $scope.priceoverrideDisable = true;
            $scope.updateGiftRegistryDisable = true;
        } else { //else if checkButtonIsChecked = false. Check All boxes and see if any are price overriden 
            changeMasterCheckboxDisplay(true);
            var _isPriceOverriden = false;
            angular.forEach($scope.orderlines, function (orderline) {
                orderline.isorderlinechecked = true;
                if (orderline.ispriceoverride) {
                    _isPriceOverriden = true;
                }
            });
            $scope.priceoverrideDisable = !_isPriceOverriden;
            $scope.updateGiftRegistryDisable = false;
        }
    };

    $scope.updateMasterCheckbox = function () {
        var isPriceoverriden = false; //is one of the checked orderlines a priceoverride line?
        var isChecked = false; //is one of the order lines checked?
        var isAllChecked = true; //are all of the order lines checked?

        angular.forEach($scope.orderlines, function (orderline) {
            if (orderline.isorderlinechecked) {
                isChecked = true;

                if (orderline.ispriceoverride) {
                    isPriceoverriden = true;
                }
            } else {
                isAllChecked = false;
            }

        });

        if (isAllChecked) {
            changeMasterCheckboxDisplay(true);
        } else {
            changeMasterCheckboxDisplay(false);
            //disable gift registry and price override buttons
        }
        if (isChecked && isPriceoverriden) {
            $scope.priceoverrideDisable = false;
        } else {
            $scope.priceoverrideDisable = true;
        }
        if (isChecked) {
            $scope.updateGiftRegistryDisable = false;
        } else {
            $scope.updateGiftRegistryDisable = true;
        }

    };

    //gift Registry Modal
    $scope.open = function (size) {

        var modalInstance = $modal
                .open({
                    templateUrl: 'giftRegTpl.html',
                    controller: 'ModalGiftRegCtrl',
                    size: size
                });

        modalInstance.result.then(function (regNo) {
            angular.forEach($scope.orderlines, function (orderline) {
                if (orderline.isorderlinechecked) {
                    orderline.giftregistry = regNo;
                }
            });
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
    // modal

    //Item Detail/ Order line detail
    $scope.openDetailModal = function (size) {

        var modalInstance2 = $modal
                .open({
                    templateUrl: 'orderLineContent.html',
                    controller: 'ModalOrderLineCtrl',
                    size: size
                });

        modalInstance2.result.then(function (registryNo) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
    // modal

}]).controller('ModalGiftRegCtrl', ['$scope', '$modalInstance',
				function ($scope, $modalInstance) {

				    $scope.registryNo = "";

				    $scope.ok = function () {
				        $modalInstance.close($scope.registryNo);
				    };

				    $scope.cancel = function () {
				        $modalInstance.dismiss('cancel');
				    };


				}])
.controller('ModalOrderLineCtrl', ['$scope', '$modalInstance', 'items',
				function ($scope, $modalInstance, items) {

				    $scope.items = items;
				    $scope.selected = {
				        item: $scope.items[0]
				    };

				    $scope.ok = function () {
				        $modalInstance.close($scope.selected.item);
				    };

				    $scope.cancel = function () {
				        $modalInstance.dismiss('cancel');
				    };


				}])
;