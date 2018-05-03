/// <reference path="C:\Users\014265\VisualStudioWorkSpace\Workspaces\Order Management.LUFI\dev\Lufi2AngularJS\Lufi2AngularJS\html/modalTemplates/shippingSpeedMultiModal.html" />
/// <reference path="C:\Users\014265\VisualStudioWorkSpace\Workspaces\Order Management.LUFI\dev\Lufi2AngularJS\Lufi2AngularJS\html/modalTemplates/shippingSpeedMultiModal.html" />
angular.module('shippingSelection', ['ui.bootstrap', 'appServiceOrderCart', 'ui.router'])
.filter('filterShipToList', function () {
    return function (orderlineArray, addressFilterObj) {
        if (angular.isArray(orderlineArray) && angular.isObject(addressFilterObj) && angular.isString(addressFilterObj.key) && angular.isString(addressFilterObj.carrierServiceCode)) {
            var tempArray = [];
            for (var i = 0; i < orderlineArray.length; i++) {

                if ((orderlineArray[i].PersonInfoShipTo._PersonInfoKey === addressFilterObj.key) &&
                        (orderlineArray[i]._CarrierServiceCode === addressFilterObj.carrierServiceCode)) {

                    tempArray.push(orderlineArray[i]);
                }
            }

            return tempArray;
        } else {

            return orderlineArray;
        }
    };
})
.controller('shippingSelectionCtrl', ['$scope', '$location', 'orderCart', '$cacheFactory', '$modal', '$state', 'customer', '$filter', 'expectedDeliveryService', 'loggerService', 'loadingIconService', function ($scope, $location, orderCart, $cacheFactory, $modal, $state, $customer, $filter, expectedDeliveryService, $loggerService, $loadingIconService) {

    //hide all giftbox.  Giftbox has been turned off because of West Jefferson. noted 5/7/2015
    $scope.isDisplayGiftBoxComponents = false;

    $scope.isPageDataLoaded = false;
    $scope.isCheckOutButtonDisabled = true;
    $scope.cart = orderCart.order.getLiveOrderCart();
    $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
    //TODO: when in $cacheFactory
    orderCart.order.getOrderNumber();



    if ($customer.isCustomerSelected()) {
        $scope.customer = $customer.getSelectedCustomer();
        orderCart.customer.setCustomer($scope.customer);
    } else {
        //if no customer go back to customer info page

        $state.go('customerSearch') // The: $location.path('/customerSearch');  does not work for repeated hits of Shipping header button.
        return; //stops async $state from going on to following code.
    }

    if ($scope.orderLinesArray.length < 1) {

        $state.go('itemSearch') // The: $location.path('/customerSearch');  does not work for repeated hits of Shipping header button.
        return; //stops async $state from going on to following code.
    }

    $scope.availableShippingAddresses = {}; //only customer's addresses no gift reg addresses
    $scope.orderedOrderLineAddressPersonKey = [];

    $scope.getLowestPrice = function (orderline) {
        return orderCart.orderLine.util.getLowestSalePrice(orderline);
    };

    $scope.getConcatAddresses = function (address) {
        var addressesArray = [];

        if (address._AddressLine1.toString().trim()) {
            addressesArray.push(address._AddressLine1.toString());
        }
        if (angular.isDefined(address._AddressLine2) && address._AddressLine2.toString().trim()) {
            addressesArray.push(address._AddressLine2.toString());
        }
        if (angular.isDefined(address._AddressLine3) && address._AddressLine3.toString().trim()) {
            addressesArray.push(address._AddressLine3.toString());
        }

        return addressesArray.join(", ");
    };

    $scope.isValidNonRegistryAddressObject = function (addressObject) {
        if (!angular.isObject(addressObject)) {
            return false;
        }
        if (angular.isObject(addressObject.btLogic) && addressObject.btLogic.isRegistryAddress) {
            return true;  //do not validate registry address
        }

        var validObj = orderCart.address.validAddress(addressObject.PersonInfo);

        return validObj.isValid;
    }

    $scope.printPrice = function (price) {
        if (angular.isNumber(price) || (angular.isString(price) && price.length > 0)) {
            return $filter('currency')(price);
        } else {
            return 'N/A';
        }
    };

    $scope.getLineTotal = function (orderline) {
        return orderCart.subtotal.computeShippingOrderLineSubtotal(orderline);
    };

    var shipToKeySet = {};

    var _addCustomerAddressesToAvailableShippingAddress = function () {
        angular.forEach($scope.customer.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress, function (addressObject) {
            if (addressObject._IsShipTo === "Y") {
                addressObject.btLogic = { isRegistryAddress: false };
                $scope.availableShippingAddresses[addressObject.PersonInfo._PersonInfoKey] = addressObject;
            }
        });
    };

    var _addRegistryAddressToAvailableShippingAddresses = function (addressObject) {
        if (!(addressObject.PersonInfo._PersonInfoKey in $scope.availableShippingAddresses)) {
            addressObject.btLogic = { isRegistryAddress: true };
            $scope.availableShippingAddresses[addressObject.PersonInfo._PersonInfoKey] = addressObject;
        }
    };

    var _addAllCurrentRegistryAddressesToAvailableShippingAddresses = function () {

        return orderCart.giftOptions.loadRegistryAddresses().then(function (listOfRegAddr) {
            angular.forEach(listOfRegAddr, _addRegistryAddressToAvailableShippingAddresses);
        });
    };

    var _initAvailableShippingAddresses = function () {
        //clear $scope.availableShippingAddresses
        var keys = Object.keys($scope.availableShippingAddresses);
        for (var index = 0; index < keys.length; index++) {
            delete $scope.availableShippingAddresses[keys[index]];
        }

        //add addresses from current cached customer
        _addCustomerAddressesToAvailableShippingAddress();

        //add registry address that are on the order currently
        _addAllCurrentRegistryAddressesToAvailableShippingAddresses();

    };

    var _regroupOrderLines = function () {
        //clear $scope.orderedOrderLineAddressPersonKey  AND shipToKeySet
        $scope.orderedOrderLineAddressPersonKey.splice(0, $scope.orderedOrderLineAddressPersonKey.length);
        var keys = Object.keys(shipToKeySet);
        for (var index = 0; index < keys.length; index++) {
            delete shipToKeySet[keys[index]];
        }

        for (var i = 0; i < $scope.orderLinesArray.length; i++) {
            var orderLineShipKey = $scope.orderLinesArray[i].PersonInfoShipTo._PersonInfoKey;

            //if address in not in the set add it with shipping speed
            if (!(orderLineShipKey in shipToKeySet)) {
                shipToKeySet[orderLineShipKey] = [$scope.orderLinesArray[i]._CarrierServiceCode]; //code can be empty string. 
                $scope.orderedOrderLineAddressPersonKey.push(
                        {
                            key: orderLineShipKey,
                            carrierServiceCode: $scope.orderLinesArray[i]._CarrierServiceCode,
                            carrierServicePrice: $scope.orderLinesArray[i].btDisplay.shippingMethodPrice,
                            carrierServiceDescription: $scope.orderLinesArray[i].btDisplay.shippingMethodDescription,
                            isRegistryAddress: $scope.orderLinesArray[i].btLogic.isGiftRegistryAddress,
                            registryNo: $scope.orderLinesArray[i].Extn._ExtnGiftRegistryNo
                        });
            } else {
                //check if shipToKeySet already has a group for the address's specific carrierServiceCode (speed like "UPS-GRND")
                var carrierArray = shipToKeySet[orderLineShipKey];
                var currentServiceCode = $scope.orderLinesArray[i]._CarrierServiceCode;
                var isCarrierAlreadyInArray = false;
                for (var q = 0; q < carrierArray.length; q++) {
                    if (carrierArray[q].toString() === currentServiceCode.toString()) {
                        isCarrierAlreadyInArray = true;
                        break;
                    }
                }

                if (!isCarrierAlreadyInArray) {
                    shipToKeySet[orderLineShipKey].push($scope.orderLinesArray[i]._CarrierServiceCode); //code can be empty string. 
                    $scope.orderedOrderLineAddressPersonKey.push(
                        {
                            key: orderLineShipKey,
                            carrierServiceCode: $scope.orderLinesArray[i]._CarrierServiceCode,
                            carrierServicePrice: $scope.orderLinesArray[i].btDisplay.shippingMethodPrice,
                            carrierServiceDescription: $scope.orderLinesArray[i].btDisplay.shippingMethodDescription,
                            isRegistryAddress: $scope.orderLinesArray[i].btLogic.isGiftRegistryAddress,
                            registryNo: $scope.orderLinesArray[i].Extn._ExtnGiftRegistryNo
                        });
                }
            }
        }
    };

    var _resetShippingDisplayData = function () {
        _initAvailableShippingAddresses();
        _regroupOrderLines();
    };

    var _init = function () {
        _resetShippingDisplayData();

        //reset checkboxes on lines
        $scope.uncheckAll(null);
        $scope.isPageDataLoaded = true;
    };

    var _repriceReset = function () {
        $loggerService.log('Refreshing controllers $scope with orderCart.');

        $scope.cart = orderCart.order.getLiveOrderCart();
        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
        _resetShippingDisplayData();
        $scope.refreshIScroll();

    };

    //utility functions
    var _getCustomerBillingAddresses = function () {
        var billingAddressObj = {};
        angular.forEach($scope.customer.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress, function (addressObject) {
            if (addressObject._IsBillTo === "Y") {

                var validObj = orderCart.address.validAddress(addressObject.PersonInfo, true);
                var copyBilling = angular.copy(addressObject);
                copyBilling.btIsValid = validObj.isValid;

                billingAddressObj[addressObject.PersonInfo._PersonInfoKey] = copyBilling;

            }
        });
        return billingAddressObj;
    };


    $scope.goToPayment = function () {

        orderCart.address.validateOrderAddresses().then(
            function (data) {
                //validate order header attributes like _CustomerEMailID
                var headerErrors = orderCart.order.validateOrderHeader();


                if (!data.defaultBillingAddressError.hasError &&
                    !data.defaultShipToAddressError.hasError &&
                    !data.carrierServiceCodeError.hasError &&
                    !data.bigTicketError.hasError &&
                    !data.orderlineShiptoAddressError.hasError &&
                    !headerErrors.customerEmailIdError.hasError
                ) {

                    $state.go('paymentSummary');
                } else {
                    var ErrorTextArr = [];
                    if (headerErrors.customerEmailIdError.hasError) {
                        ErrorTextArr.push(headerErrors.customerEmailIdError.errorText);
                    }
                    if (data.defaultBillingAddressError.hasError) {
                        ErrorTextArr.push(data.defaultBillingAddressError.errorText);
                    }
                    if (data.defaultShipToAddressError.hasError) {
                        ErrorTextArr.push(data.defaultShipToAddressError.errorText);
                    }
                    if (data.carrierServiceCodeError.hasError) {
                        ErrorTextArr.push(data.carrierServiceCodeError.errorText);
                    }
                    if (data.bigTicketError.hasError) {
                        ErrorTextArr.push(data.bigTicketError.errorText);
                    }
                    if (data.orderlineShiptoAddressError.hasError) {
                        ErrorTextArr.push(data.orderlineShiptoAddressError.errorText);
                    }


                    swal({ title: "Alert!", text: ErrorTextArr.join('. '), showConfirmButton: true });
                }


            }, function (data) {
                swal({ title: "Alert!", text: 'validate order addresses ajax error.', showConfirmButton: true });
                $loggerService.log(data);
            }
        );
    };

    // page user actions
    $scope.getGiftMessage = function (orderLine) {
        var primeSubLine = {};
        primeSubLine.PrimeLine = orderLine._PrimeLineNo;
        if (orderLine._SubLineNo) {
            primeSubLine.SubLine = orderLine._SubLineNo;
        }
        var messages = orderCart.giftOptions.getGiftOptionFromOrderLines(primeSubLine);
        if (messages) {
            return "To: " + messages.To + " From: " + messages.From + " Message: " + messages.Message;
        } else {
            return "";
        }
    };

    $scope.collapseGroup = function (event) {
        var current = jQuery(event.currentTarget);
        current.toggleClass('shippingSelectGroupHeaderCollapsed');
        current.toggleClass('shippingSelectGroupHeader');
        current.find('.minIcon').toggle();
        current.find('.maxIcon').toggle();
        current.nextAll('.groupRow').toggle();
        $scope.refreshIScroll();
    };

    //START -- master check all
    var _selectedLinesCount = 0;
    $scope.isLineSelected = false;

    $scope.allLinesChecked = function () {
        return _selectedLinesCount == $scope.orderLinesArray.length;
    };

    $scope.isAllAddressGroupChecked = function (addressFilterObj) {
        var linesToCheck = $filter('filterShipToList')($scope.orderLinesArray, addressFilterObj);
        for (var i = 0; i < linesToCheck.length; i++) {
            if (!angular.isDefined(linesToCheck[i].btLogic.isSelected)) {
                return false;
            } else if (linesToCheck[i].btLogic.isSelected === false) {
                return false;
            }
        }

        return true;
    };

    var _updateSelectionCount = function (count) {
        if (angular.isNumber(parseInt(count))) {
            count = parseInt(count);
            _selectedLinesCount += count;
            if (_selectedLinesCount < 1) {
                _selectedLinesCount = 0;
                $scope.isLineSelected = false;
            } else {
                $scope.isLineSelected = true;
            }
        }
    };

    $scope.selectLine = function (orderline) {
        if (orderline.btLogic.isSelected) {
            orderline.btLogic.isSelected = false;
            _updateSelectionCount(-1);
        } else {
            orderline.btLogic.isSelected = true;
            _updateSelectionCount(1);
        }
    };

    $scope.checkedButtonFont = 'fa-check fa-2x';

    //TODO: need to change to reflect new address AND shipping speed grouping
    $scope.checkAll = function (addressFilterObj) {
        if (angular.isObject(addressFilterObj) && angular.isString(addressFilterObj.key) && angular.isString(addressFilterObj.carrierServiceCode)) {
            for (var i = 0; i < $scope.orderLinesArray.length; i++) {
                if (($scope.orderLinesArray[i].PersonInfoShipTo._PersonInfoKey === addressFilterObj.key) &&
                        ($scope.orderLinesArray[i]._CarrierServiceCode === addressFilterObj.carrierServiceCode)
                   ) {
                    if (!angular.isDefined($scope.orderLinesArray[i].btLogic.isSelected)) {
                        $scope.orderLinesArray[i].btLogic.isSelected = true;
                        _updateSelectionCount(1);
                    } else if (angular.isDefined($scope.orderLinesArray[i].btLogic.isSelected) && $scope.orderLinesArray[i].btLogic.isSelected == false) {
                        $scope.orderLinesArray[i].btLogic.isSelected = true;
                        _updateSelectionCount(1);
                    } else {
                        $scope.orderLinesArray[i].btLogic.isSelected = true;
                    }

                }
            }
        } else {
            for (var p = 0; p < $scope.orderLinesArray.length; p++) {

                if (!angular.isDefined($scope.orderLinesArray[p].btLogic.isSelected)) {
                    $scope.orderLinesArray[p].btLogic.isSelected = true;
                    _updateSelectionCount(1);
                } else if (angular.isDefined($scope.orderLinesArray[p].btLogic.isSelected) && $scope.orderLinesArray[p].btLogic.isSelected == false) {
                    $scope.orderLinesArray[p].btLogic.isSelected = true;
                    _updateSelectionCount(1);
                } else {
                    $scope.orderLinesArray[p].btLogic.isSelected = true;
                }
            }
        }
    };

    //TODO: need to change to reflect new address AND shipping speed grouping
    $scope.uncheckAll = function (addressFilterObj) {
        if (angular.isObject(addressFilterObj) && angular.isString(addressFilterObj.key) && angular.isString(addressFilterObj.carrierServiceCode)) {
            for (var i = 0; i < $scope.orderLinesArray.length; i++) {
                if (($scope.orderLinesArray[i].PersonInfoShipTo._PersonInfoKey === addressFilterObj.key) &&
                        ($scope.orderLinesArray[i]._CarrierServiceCode === addressFilterObj.carrierServiceCode)
                   ) {
                    $scope.orderLinesArray[i].btLogic.isSelected = false;
                    _updateSelectionCount(-1);
                }
            }
        } else {
            for (var p = 0; p < $scope.orderLinesArray.length; p++) {
                $scope.orderLinesArray[p].btLogic.isSelected = false;
                _updateSelectionCount(-1);
            }

            _updateSelectionCount(-1000);
        }
    };

    //END -- master check all
    //START -- gift options modal
    $scope.giftOpen = function (orderline, isGiftMessageShow, isGiftRegistyShow, isGiftBoxShow) {
        var modalInstance = $modal
                .open({
                    templateUrl: 'giftMessageModal.html',
                    controller: 'GiftRegModCtrl',
                    resolve: {
                        orderlines: function () {
                            if (!angular.isArray(orderline)) {
                                var wrap = [];
                                wrap.push(orderline);
                                return wrap;
                            } else {
                                return orderline;
                            }
                        },
                        show: function () {
                            return { 'isGiftMessageShow': isGiftMessageShow, 'isGiftRegistyShow': isGiftRegistyShow, 'isGiftBoxShow': isGiftBoxShow };
                        }
                    },
                    size: 'lg'
                });

        modalInstance.result.then(function (registryChange) {

            if (registryChange.isRegistryChanged) {
                if (registryChange.registryAddressAdded) {
                    _addRegistryAddressToAvailableShippingAddresses(registryChange.registryAddressAdded);
                }
                _addressChangeResetProcess();
            }
        });
    };

    //END -- gift options modal

    //START -- Ship To Modal

    $scope.updateShipTo = function (orderline) {
        var modalInstance = $modal
                .open({
                    templateUrl: 'shipToModal.html',
                    controller: 'shipToModCtrl',
                    resolve: {
                        orderlines: function () {
                            if (!angular.isArray(orderline)) {
                                var wrap = [];
                                wrap.push(orderline);
                                return wrap;
                            } else {
                                return orderline;
                            }
                        },
                        availableShippingAddresses: function () {
                            var validatedAddresses = {};

                            for (key in $scope.availableShippingAddresses) {
                                var address = $scope.availableShippingAddresses[key].PersonInfo;

                                var validObj = orderCart.address.validAddress(address);

                                if (angular.isDefined(address._State) && address._State !== null && ((/^(aa|ae|ap)$/).test(address._State.toString().toLowerCase()))) {
                                    //cannot ship by military or diplomatic post
                                    continue;
                                } else {
                                    var addressCopy = angular.copy($scope.availableShippingAddresses[key]);
                                    addressCopy.btIsValid = validObj.isValid;

                                    validatedAddresses[key] = addressCopy;
                                }

                            }
                            return validatedAddresses;
                        },
                        customerContactID: function () { return $scope.customer.CustomerContactList.CustomerContact[0]._CustomerContactID; }
                    },
                    size: 'lg'
                });

        modalInstance.result.then(function () {
            _addressChangeResetProcess();
        });
    };
    //END -- Ship To Modal

    $scope.updateBillingAddress = function () {
        var billingAddresses = angular.copy(_getCustomerBillingAddresses());
        if (!angular.isObject(billingAddresses)) {
            billingAddresses = {};
        }
        $loggerService.log(billingAddresses);
        var modalInstance = $modal
                .open({
                    templateUrl: 'html/customer/billingAddressModal.html',
                    controller: 'billingAddressModCtrl',
                    resolve: {
                        billingAddresses: function () { return billingAddresses; },
                        customerContactID: function () { return $scope.customer.CustomerContactList.CustomerContact[0]._CustomerContactID; }
                    },
                    size: 'lg'
                });
    };

    $scope.updateTaxExempt = function () {
        var modalInstance = $modal
                .open({
                    templateUrl: 'taxExemptModal.html',
                    controller: 'taxExemptModCtrl',
                    size: 'lg'
                });
        modalInstance.result.then(function () {
            _speedChangeResetProcess();
        });
    };


    $scope.updateShippingSpeed = function (orderline) {
        var modalInstance = $modal
                .open({
                    templateUrl: 'shippingSpeedModal.html',
                    controller: 'shippingSpeedModCtrl',
                    resolve: {
                        orderlines: function () {
                            if (!angular.isArray(orderline)) {
                                var wrap = [];
                                wrap.push(orderline);
                                return wrap;
                            } else {
                                return orderline;
                            }
                        }
                    },
                    size: 'lg'
                });

        modalInstance.result.then(function () {
            _speedChangeResetProcess();
        });
    };

    $scope.updateMultiGiftMessage = function () {
        var orderlines = [];

        for (var i = 0; i < $scope.orderLinesArray.length; i++) {
            if ($scope.orderLinesArray[i].btLogic.isSelected) {
                orderlines.push($scope.orderLinesArray[i]);
            }
        }

        if (orderlines.length > 0) {
            $scope.giftOpen(orderlines, true, false, false);
        }

    };

    $scope.updateMultiGiftRegistry = function () {
        var orderlines = [];

        for (var i = 0; i < $scope.orderLinesArray.length; i++) {
            if ($scope.orderLinesArray[i].btLogic.isSelected) {
                orderlines.push($scope.orderLinesArray[i]);
            }
        }

        if (orderlines.length > 0) {
            $scope.giftOpen(orderlines, false, true, false);
        }
    };

    $scope.addMultiGiftBox = function () {
        var orderlines = [];

        for (var i = 0; i < $scope.orderLinesArray.length; i++) {
            if ($scope.orderLinesArray[i].btLogic.isSelected) {
                orderlines.push($scope.orderLinesArray[i]);
            }
        }

        if (orderlines.length > 0) {
            //get prime line subline numbers
            var primeSubLineObjArray = [];
            for (var i = 0; i < orderlines.length; i++) {
                var tempObj = {};

                if (orderlines[i]._PrimeLineNo) {
                    tempObj.PrimeLine = orderlines[i]._PrimeLineNo;

                    if (orderlines[i]._SubLineNo) {
                        tempObj.SubLine = orderlines[i]._SubLineNo;
                    }

                    primeSubLineObjArray.push(tempObj);
                }

            }
            //update cart

            orderCart.giftOptions.addGiftBox(primeSubLineObjArray);
        }
    };

    $scope.deleteMultiGiftBox = function () {
        var orderlines = [];

        for (var i = 0; i < $scope.orderLinesArray.length; i++) {
            if ($scope.orderLinesArray[i].btLogic.isSelected) {
                orderlines.push($scope.orderLinesArray[i]);
            }
        }

        if (orderlines.length > 0) {
            //get prime line subline numbers
            var primeSubLineObjArray = [];
            for (var i = 0; i < orderlines.length; i++) {
                var tempObj = {};

                if (orderlines[i]._PrimeLineNo) {
                    tempObj.PrimeLine = orderlines[i]._PrimeLineNo;

                    if (orderlines[i]._SubLineNo) {
                        tempObj.SubLine = orderlines[i]._SubLineNo;
                    }

                    primeSubLineObjArray.push(tempObj);
                }

            }
            //update cart

            orderCart.giftOptions.deleteGiftBox(primeSubLineObjArray);
        }
    };

    $scope.updateMultiShipTo = function () {
        var orderlines = [];

        for (var i = 0; i < $scope.orderLinesArray.length; i++) {
            if ($scope.orderLinesArray[i].btLogic.isSelected) {
                orderlines.push($scope.orderLinesArray[i]);
            }
        }

        if (orderlines.length > 0) {
            $scope.updateShipTo(orderlines);
        }
    };

    $scope.updateMultiShippingSpeed = function () {
        var orderlines = [];

        for (var i = 0; i < $scope.orderLinesArray.length; i++) {
            if ($scope.orderLinesArray[i].btLogic.isSelected) {
                orderlines.push($scope.orderLinesArray[i]);
            }
        }

        var modalInstance = $modal
                .open({
                    templateUrl: '/html/modalTemplates/shippingSpeedMultiModal.html',
                    controller: 'shippingSpeedMultiModCtrl',
                    resolve: {
                        orderlines: function () {
                            return orderlines;
                        },
                        expectedShipDatesMap: function () {
                            //map is { 'UPS-NDAY' : 'Jul 12, 2015',..}
                            var transformed = angular.copy($scope.expectedDeliveryDateMap);
                            for (key in transformed) {
                                transformed[key] = new Date(transformed[key]);
                            }
                            return transformed;
                        }
                    },
                    size: 'lg'
                });

        modalInstance.result.then(function () {
            _speedChangeResetProcess();
        });
    };

    var _reportRepriceErrors = function () {
        var ErrorText = "Alert Help Desk. ";
        var ErrorFound = false;

        for (var i = 0; i < $scope.cart.Errors.ErrorList.Error.length; i++) {
            if ($scope.cart.Errors.ErrorList.Error[i]._ErrorCode == '00') {
                continue;
            } else {
                ErrorFound = true;
                ErrorText += " " + i.toString() + ") " + $scope.cart.Errors.ErrorList.Error[i]._ErrorDescription.toString();

                $loggerService.log("REPRICE ERRORS:");
                $loggerService.log($scope.cart.Errors.ErrorList.Error[i]);
            }
        }

        if (ErrorFound) {
            swal({ title: "Alert!", text: ErrorText, showConfirmButton: true });
        }
    };

    var _addressChangeResetProcess = function () {
        $scope.isCheckOutButtonDisabled = true;

        $loadingIconService.lockIcon();

        orderCart.carrierService.updateCarrierServiceList().then(function () {
            orderCart.order.repriceOrder(true).then(function () {
                _repriceReset();

                _reportRepriceErrors();
                $scope.isCheckOutButtonDisabled = false;
                $loadingIconService.resetIcon();
            });
        }, function (error) {
            swal({ title: "Alert!", text: error.message, showConfirmButton: true });
            $loadingIconService.resetIcon();
            $scope.isCheckOutButtonDisabled = false;
            _resetShippingDisplayData();
            $scope.refreshIScroll();
        });

    };

    var _speedChangeResetProcess = function () {
        $scope.isCheckOutButtonDisabled = true;
        orderCart.order.repriceOrder(true).then(function () {
            _repriceReset();
            _reportRepriceErrors();
            $scope.isCheckOutButtonDisabled = false;
        });
    };

    //initialize page open display details
    _init();

    var myScroll = new IScroll('#GroupWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
    $scope.refreshIScroll = function () {
        setTimeout(function () {
            myScroll.refresh();
        }, 500);
    };

    expectedDeliveryService.getDates().success(
        function (data) {
            var serviceMap = {};
            for (var i = 0; i < data.ExpectedDeliveryDates.length; i++) {
                serviceMap[data.ExpectedDeliveryDates[i].CarrierServiceCode] = data.ExpectedDeliveryDates[i].Date;
            }
            $scope.expectedDeliveryDateMap = serviceMap;
        }
        );

    _addressChangeResetProcess();

}])

.controller('GiftRegModCtrl', ['$scope', '$modalInstance', 'orderlines', 'show', 'orderCart', 'giftRegistryService', '$q', 'loggerService', function ($scope, $modalInstance, orderlines, show, orderCart, giftRegistryService, $q, $loggerService) {
    $scope.orderlines = orderlines;
    $scope.To = "";
    $scope.From = "";
    $scope.Message = "";
    $scope.Registry = "";
    $scope.isGiftMessageShow = show.isGiftMessageShow;
    $scope.isGiftRegistyShow = show.isGiftRegistyShow;
    $scope.isGiftBoxShow = show.isGiftBoxShow;

    $scope.isShowInvalidRegError = false;


    var primeSubLineObjArray = [];
    for (var i = 0; i < $scope.orderlines.length; i++) {
        var tempObj = {};

        if ($scope.orderlines[i]._PrimeLineNo) {
            tempObj.PrimeLine = $scope.orderlines[i]._PrimeLineNo;

            if ($scope.orderlines[i]._SubLineNo) {
                tempObj.SubLine = $scope.orderlines[i]._SubLineNo;
            }

            primeSubLineObjArray.push(tempObj);
        }

    }

    var originalGiftMessageObj = orderCart.giftOptions.getGiftOptionFromOrderLines(primeSubLineObjArray);

    if (originalGiftMessageObj) {
        $scope.To = angular.isDefined(originalGiftMessageObj.To) ? originalGiftMessageObj.To : '';
        $scope.From = angular.isDefined(originalGiftMessageObj.From) ? originalGiftMessageObj.From : '';
        $scope.Message = angular.isDefined(originalGiftMessageObj.Message) ? originalGiftMessageObj.Message : '';
        $scope.Registry = angular.isDefined(originalGiftMessageObj.Registry) ? originalGiftMessageObj.Registry : '';
    }

    $scope.cancel = function () { $modalInstance.dismiss(); };

    $scope.save = function () {

        var registryAddressAdded = null;
        var isRegistryChanged = false;

        //save gift message
        if ($scope.isGiftMessageShow) {
            orderCart.giftOptions.setGiftMessage(primeSubLineObjArray, { To: $scope.To.trim().substring(0, 30), From: $scope.From.trim().substring(0, 40), Message: $scope.Message.trim().substring(0, 130) });
        }

        //save gift box
        if ($scope.isGiftBoxShow) {
            //TODO: implement giftbox
            $loggerService.log("GiftBox is not implemented");
            saveGiftBox();
        }

        //if giftRegistry is showing, do not close modal.  Must wait for promise to resolve
        if (!$scope.isGiftRegistyShow) {
            $modalInstance.close({ 'isRegistryChanged': isRegistryChanged, 'registryAddressAdded': registryAddressAdded });
        }

        //save registry
        if ($scope.isGiftRegistyShow) {

            saveRegistry(primeSubLineObjArray, $scope.Registry.trim()).then(
                //success
                function (response) {
                    $loggerService.log(response);
                    if (response.invalidRegNo) {
                        $scope.isShowInvalidRegError = true;
                    } else {
                        $scope.isShowInvalidRegError = false;
                    }
                    if (response.success) {
                        $modalInstance.close({ 'isRegistryChanged': true, 'registryAddressAdded': response.regAddress });
                    }

                }
                );

        }
    };

    var saveRegistry = function (primeSubLineObjArray, registry) {
        var success = { success: false, regAddress: null, invalidRegNo: false };
        var tempAddress = null;
        if (registry) {
            registry = registry.toString().trim();
        }
        var deferred = $q.defer();

        if (registry) {

            giftRegistryService.preferredAddressPromise(registry).success(function (data) {
                if (data.PreferredAddressOutput.PreferredAddressResponse.IsValid === 'true' &&
                        data.PreferredAddressOutput.PreferredAddressResponse.HasErrors === 'false') {
                    //add addresses to giftRegistryAddressSet
                    try {
                        tempAddress = giftRegistryService.constructRegistryAddress(registry, data.PreferredAddressOutput.PreferredAddressResponse);
                        success = { success: true, regAddress: tempAddress, invalidRegNo: false };
                        orderCart.giftOptions.setGiftRegistry(primeSubLineObjArray, registry, tempAddress);
                    } catch (error) {
                        //TODO: deal with errors
                        alert('Gift Registry Error. Returned Registry Address is invalid.');
                        $loggerService.log(error);
                        success = { success: false, regAddress: null, invalidRegNo: false };
                    }
                } else {
                    if (data.PreferredAddressOutput.PreferredAddressResponse.ErrorMessage === "No data exists for the row/column.") {
                        //invalid registry no
                        success = { success: false, regAddress: null, invalidRegNo: true };
                    }
                    else if (data.PreferredAddressOutput.PreferredAddressResponse.ErrorMessage === "Object reference not set to an instance of an object.") {
                        //bad input by us -- programmers
                        success = { success: false, regAddress: null, invalidRegNo: false };
                        $loggerService.log('Programmer Error.');
                        $loggerService.log(data);
                    } else {
                        //service is down so just take the registry number
                        success = { success: true, regAddress: null, invalidRegNo: false };
                        $loggerService.log(data);
                        orderCart.giftOptions.setGiftRegistry(primeSubLineObjArray, registry, tempAddress);
                    }
                }

                deferred.resolve(success);
            }).error(function (data) {
                //service is down so just accept
                success = { success: true, regAddress: null, invalidRegNo: false };
                $loggerService.log(data);
                orderCart.giftOptions.setGiftRegistry(primeSubLineObjArray, registry, tempAddress);
                deferred.resolve(success);
            });

        } else {
            success = { success: true, regAddress: null, invalidRegNo: false };

            orderCart.giftOptions.setGiftRegistry(primeSubLineObjArray, registry, tempAddress);
            deferred.resolve(success);
        }

        return deferred.promise;
    };

    var saveGiftBox = function () { };


    $scope.clear = function () {
        $scope.To = "";
        $scope.From = "";
        $scope.Message = "";
        $scope.Registry = "";
    };

}])
.controller('shipToModCtrl', ['$scope', '$modalInstance', 'orderlines', 'availableShippingAddresses', 'orderCart', '$q', 'bigTicketValidate', 'appState', '$state', 'customerContactID', 'loggerService', function ($scope, $modalInstance, orderlines, availableShippingAddresses, orderCart, $q, bigTicketValidate, $appState, $state, customerContactID, $loggerService) {

    //get all gift registry and create map address key -> gift registry no
    //   and set up primeline subline numbers
    $scope.addressKeyToRegistryMap = {};
    var primeSubLineObjArray = [];
    var registryAddressFromOrderCart = orderCart.giftOptions.getRegistryAddressSet();
    $scope.isBigTicket = false;

    var myScroll;
    setTimeout(function () {
        myScroll = new IScroll('#ItemDetailWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
    }, 500);

    $scope.refreshIScroll = function () {
        setTimeout(function () {
            myScroll.refresh();
        }, 500);
    };

    for (var i = 0; i < orderlines.length; i++) {
        var tempObj = {};

        if (orderlines[i]._PrimeLineNo) {
            tempObj.PrimeLine = orderlines[i]._PrimeLineNo;

            if (orderlines[i]._SubLineNo) {
                tempObj.SubLine = orderlines[i]._SubLineNo;
            }

            primeSubLineObjArray.push(tempObj);
        }

        //check for giftRegistry on any orderlines
        if (orderlines[i].Extn._ExtnGiftRegistryNo && (orderlines[i].Extn._ExtnGiftRegistryNo.toString().trim() !== "")) {
            var tempRegNo = orderlines[i].Extn._ExtnGiftRegistryNo.toString().trim();
            if (angular.isDefined(registryAddressFromOrderCart[tempRegNo])) {
                $scope.addressKeyToRegistryMap[registryAddressFromOrderCart[tempRegNo].PersonInfo._PersonInfoKey] = tempRegNo;
            }
        }

        //check if any of the orderlines have a bigticket item
        if (orderlines[i].btDisplay.itemtype == 'BGT') {
            $scope.isBigTicket = true;
        }
    }

    $scope.availableShippingAddresses = {};

    var setAvailableShippingAddresses = function () {
        var tempAddressSet = {};

        //add normal addresses and only registry addresses from availableShippingAddresses that are in the orderlines
        for (var addressKey in availableShippingAddresses) {

            if (availableShippingAddresses[addressKey].btLogic && availableShippingAddresses[addressKey].btLogic.isRegistryAddress) {

                if (availableShippingAddresses[addressKey].PersonInfo._PersonInfoKey in $scope.addressKeyToRegistryMap) {
                    //validate as big ticket address
                    tempAddressSet[availableShippingAddresses[addressKey].PersonInfo._PersonInfoKey] = availableShippingAddresses[addressKey];
                }
            } else {
                tempAddressSet[availableShippingAddresses[addressKey].PersonInfo._PersonInfoKey] = availableShippingAddresses[addressKey];
            }
        }

        //if there is bigticket items filter out non-big ticket addresses
        if ($scope.isBigTicket) {

            //get all zipCodes
            var zipCodeSet = {};

            for (var prop in tempAddressSet) {
                var addressZip = tempAddressSet[prop].PersonInfo._ZipCode.toString();
                zipCodeSet[addressZip] = 'false';
            }

            var promiseArr = [];
            var zipArr = Object.keys(zipCodeSet);
            for (var i = 0; i < zipArr.length; i++) {
                (function () {
                    var zip = zipArr[i];
                    promiseArr.push(
                            bigTicketValidate.bigTicketPromise(zip).then(function (response) {
                                if (response.data.ValidateBigTicketZipCodeResp._statusMessage.toString() === 'Success') {
                                    zipCodeSet[zip] = response.data.ValidateBigTicketZipCodeResp._isValid;
                                }
                            })
                            );
                })();
            }

            $q.all(promiseArr).then(function () {
                //add only valid addresses to scope
                for (var prop in tempAddressSet) {
                    var checkzip = tempAddressSet[prop].PersonInfo._ZipCode.toString();
                    if (zipCodeSet[checkzip] === 'true') {
                        $scope.availableShippingAddresses[tempAddressSet[prop].PersonInfo._PersonInfoKey] = tempAddressSet[prop];
                    }
                }

            });


        } else {
            $scope.availableShippingAddresses = tempAddressSet;
        }
    };

    setAvailableShippingAddresses();

    $scope.saveAddress = function (address) {

        //if address is gift registry address then apply only to order lines with same registry no
        var isGiftRegAddress = false;
        if (address.btLogic && address.btLogic.isRegistryAddress) {
            //reset 
            isGiftRegAddress = true;
            primeSubLineObjArray = [];
            for (var i = 0; i < orderlines.length; i++) {
                var tempObj = {};
                $loggerService.log(orderlines[i]);
                $loggerService.log($scope.addressKeyToRegistryMap);
                if (orderlines[i].Extn._ExtnGiftRegistryNo === $scope.addressKeyToRegistryMap[address.PersonInfo._PersonInfoKey]) {
                    if (orderlines[i]._PrimeLineNo) {
                        tempObj.PrimeLine = orderlines[i]._PrimeLineNo;

                        if (orderlines[i]._SubLineNo) {
                            tempObj.SubLine = orderlines[i]._SubLineNo;
                        }

                        primeSubLineObjArray.push(tempObj);
                    }
                }
            }
        }

        if (address.btIsValid) {
            delete address.btIsValid;
            orderCart.address.setOrderLineShipToAddresses(primeSubLineObjArray, address, isGiftRegAddress);
            $modalInstance.close();
        } else {
            swal({ title: "Alert!", text: 'Cannot set an invalid address.', showConfirmButton: true });
            $loggerService.log('Tried to set invalid address for ship to.');
        }
    };

    $scope.cancel = function () { $modalInstance.dismiss(); };

    $scope.openEditAddress = function (address) {
        if (!angular.isDefined(address)) {
            address = { _CustomerAdditionalAddressID: '' };
        }

        $appState.setAddressChange({ previousStateName: 'shippingSelection', customerKey: orderCart.customer.getCustomerKey(), setShipToOrderLinePrimeSubLineObjArray: primeSubLineObjArray });
        $state.go('addModifyAddress', { 'customerContactID': customerContactID, 'customerAdditionalAddressID': address._CustomerAdditionalAddressID });
        $modalInstance.dismiss();

    };
}])
.controller('billingAddressModCtrl', ['$scope', '$modalInstance', 'billingAddresses', 'orderCart', 'appState', '$state', 'customerContactID', 'loggerService', function ($scope, $modalInstance, billingAddresses, orderCart, $appState, $state, customerContactID, $loggerService) {

    $scope.billingAddresses = billingAddresses;

    var myScroll;
    setTimeout(function () {
        myScroll = new IScroll('#ItemDetailWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
    }, 500);

    $scope.refreshIScroll = function () {
        setTimeout(function () {
            myScroll.refresh();
        }, 500);
    };

    $scope.saveAddress = function (address) {
        if (address.btIsValid) {
            delete address.btIsValid;
            orderCart.address.setBillingAddress(address);
            $modalInstance.close();
        } else {
            swal({ title: "Alert!", text: 'Cannot set an invalid address.', showConfirmButton: true });
            $loggerService.log('Tried to set invalid address for bill to.');
        }
    };

    $scope.openEditAddress = function (address) {
        if (!angular.isDefined(address)) {
            address = { _CustomerAdditionalAddressID: '' };
        }

        $appState.setAddressChange({ previousStateName: 'shippingSelection', customerKey: orderCart.customer.getCustomerKey(), setBillTo: true });
        $state.go('addModifyAddress', { 'customerContactID': customerContactID, 'customerAdditionalAddressID': address._CustomerAdditionalAddressID });
        $modalInstance.dismiss();

    };

    $scope.cancel = function () { $modalInstance.dismiss(); };
}])
.controller('taxExemptModCtrl', ['$scope', '$modalInstance', 'orderCart', function ($scope, $modalInstance, orderCart) {

    $scope.TaxExemptCertificate = orderCart.order.getTaxExemptionCertificate();

    $scope.save = function () {

        orderCart.order.setTaxExemptionCertificate($scope.TaxExemptCertificate);
        $modalInstance.close();
    };

    $scope.clear = function () { $scope.TaxExemptCertificate = ""; };
    $scope.cancel = function () { $modalInstance.dismiss(); };
}])
.controller('shippingSpeedModCtrl', ['$scope', '$modalInstance', 'orderlines', 'orderCart', function ($scope, $modalInstance, orderlines, orderCart) {
    $scope.orderline = orderlines[0];

    $scope.saveService = function (carrierService) {
        orderCart.carrierService.setOrderlineCarrierService(orderlines[0], carrierService);

        //Need to remove shipping discount because price of shipping has changed
        angular.forEach(orderlines, function (orderLine) {
            orderLine = orderCart.order.deleteShippingDiscount(orderLine);
        });

        $modalInstance.close();
    };

    $scope.cancel = function () { $modalInstance.dismiss(); };
}])
.controller('shippingSpeedMultiModCtrl', ['$scope', '$modalInstance', 'orderlines', 'expectedShipDatesMap', 'orderCart', '$filter', function ($scope, $modalInstance, orderlines, expectedShipDatesMap, orderCart, $filter) {

    $scope.shippingSpeeds = []; // [{ _CarrierServiceCode: '', _CarrierServiceDesc: "Lowest Cost Shipping", _Currency: "USD", _Price: "" }, { _CarrierServiceCode: '', _CarrierServiceDesc: "Fastest Shipping", _Currency: "USD", _Price: "" }];

    var _merge = function (possibleCarrierServiceArr, secondCarrierServiceArr) {
        var temp = [];

        for (var i = 0; i < possibleCarrierServiceArr.length; i++) {
            for (var p = 0; p < secondCarrierServiceArr.length; p++) {
                if (possibleCarrierServiceArr[i]._CarrierServiceCode === secondCarrierServiceArr[p]._CarrierServiceCode) {

                    if (parseFloat(possibleCarrierServiceArr[i]._Price) === parseFloat(secondCarrierServiceArr[p]._Price)) {
                        temp.push(possibleCarrierServiceArr[i]);
                    }
                    else {
                        possibleCarrierServiceArr[i]._Price = "-1";
                        temp.push(possibleCarrierServiceArr[i]);
                    }
                    break;
                }
            }
        }
        return temp;
    };

    var possibleCarrierService = [];
    if (orderlines.length == 1) {
        angular.copy(orderlines[0].CarrierServiceList.CarrierService, possibleCarrierService);
    } else if (orderlines.length > 1) {

        for (var i = 0; i < orderlines[0].CarrierServiceList.CarrierService.length; i++) {
            possibleCarrierService.push({
                _CarrierServiceCode: orderlines[0].CarrierServiceList.CarrierService[i]._CarrierServiceCode,
                _CarrierServiceDesc: orderlines[0].CarrierServiceList.CarrierService[i]._CarrierServiceDesc,
                _Currency: "USD",
                _Price: orderlines[0].CarrierServiceList.CarrierService[i]._Price
            });
        }

        for (var i = 1; i < orderlines.length; i++) {
            possibleCarrierService = _merge(possibleCarrierService, orderlines[i].CarrierServiceList.CarrierService);
        }
    }

    $scope.shippingSpeeds = $scope.shippingSpeeds.concat(possibleCarrierService);

    $scope.saveService = function (carrierService) {
        if (carrierService._CarrierServiceDesc === "Lowest Cost Shipping") {
            for (var i = 0; i < orderlines.length; i++) {
                orderCart.carrierService.setOrderLineCarrierServiceToLowestPrice(orderlines[i]);
            }
        } else if (carrierService._CarrierServiceDesc === "Fastest Shipping") {
            for (var i = 0; i < orderlines.length; i++) {

                var currentOrderline = orderlines[i];

                if (angular.isDefined(currentOrderline.CarrierServiceList) && angular.isArray(currentOrderline.CarrierServiceList.CarrierService) &&
                    currentOrderline.CarrierServiceList.CarrierService.length > 0) {

                    var bestService = currentOrderline.CarrierServiceList.CarrierService[0];

                    for (var p = 1; p < currentOrderline.CarrierServiceList.CarrierService.length; p++) {
                        if (expectedShipDatesMap[bestService._CarrierServiceCode] > expectedShipDatesMap[currentOrderline.CarrierServiceList.CarrierService[p]._CarrierServiceCode]) {
                            bestService = currentOrderline.CarrierServiceList.CarrierService[p];
                        } else if (expectedShipDatesMap[bestService._CarrierServiceCode] === expectedShipDatesMap[currentOrderline.CarrierServiceList.CarrierService[p]._CarrierServiceCode]) {
                            if (parseFloat(bestService._Price) >= parseFloat(currentOrderline.CarrierServiceList.CarrierService[p]._Price)) {
                                bestService = currentOrderline.CarrierServiceList.CarrierService[p];
                            }
                        }
                    }
                    orderCart.carrierService.setOrderlineCarrierService(currentOrderline, bestService);
                }
            }
        } else {
            //set selected service code on each orderline
            for (var i = 0; i < orderlines.length; i++) {

                var currentOrderline = orderlines[i];

                if (angular.isDefined(currentOrderline.CarrierServiceList) && angular.isArray(currentOrderline.CarrierServiceList.CarrierService) &&
                    currentOrderline.CarrierServiceList.CarrierService.length > 0) {

                    for (var p = 0; p < currentOrderline.CarrierServiceList.CarrierService.length; p++) {
                        if (currentOrderline.CarrierServiceList.CarrierService[p]._CarrierServiceCode === carrierService._CarrierServiceCode) {
                            orderCart.carrierService.setOrderlineCarrierService(currentOrderline, currentOrderline.CarrierServiceList.CarrierService[p]);
                            break;
                        }
                    }
                }
            }
        }

        $modalInstance.close();
    };
    $scope.priceText = function (carrierService) {
        if (carrierService._Price.toString().trim().length > 0) {
            if (parseFloat(carrierService._Price) === -1) {
                return 'Various Orderline Shipping Prices';
            } else {
                return $filter('currency')(carrierService._Price);
            }
        } else {
            return '';
        }
    };

    $scope.cancel = function () { $modalInstance.dismiss(); };
}])
;