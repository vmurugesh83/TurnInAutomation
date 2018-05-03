angular.module('orderCart', ['ui.bootstrap', 'ui.router', 'pageslide-directive'])
.controller('orderCartCtrl', ['$scope', '$location', '$modal', '$state', '$log', 'customer', '$rootScope', 'orderCart', 'bigTicketValidate', '$document', 'numberOnlyKeyPressValidator', 'itemSearch', 'loggerService','$stateParams', '$timeout',
    function ($scope, $location, $modal, $state, $log, customer, $rootScope, orderCart, bigTicketValidate, $document, $numberOnlyKeyPressValidator, $itemSearch, $loggerService, $stateParams, $timeout) {

        //orderCart/:loadDraft
        var isLoadDraftOrder = false;
        if ($stateParams.loadDraft) {
            isLoadDraftOrder = $stateParams.loadDraft.toString().trim().length > 0 ? true : false;
        }

        $scope.numberValidator = $numberOnlyKeyPressValidator;

        $scope.upcPaste = function ($event) {
            if (typeof $event.originalEvent.clipboardData !== "undefined") {
                handlePastedUPC($event.originalEvent.clipboardData.getData('text/plain'));
            } else { // To support browsers without clipboard API (IE and older browsers)
                $timeout(function () {
                    handlePastedUPC(angular.element($event.currentTarget).val());
                }, 2);
            }
        };

        var handlePastedUPC = function (pastedText) {
            var cleansedText = "";

            for (var i = 0; i < pastedText.length; i++) {
                if ((/\d/).test(pastedText[i])) {
                    cleansedText += pastedText[i];
                }
            }

            $timeout(function () {
                //max length is 13
                $scope.addUpcInput = cleansedText.substring(0,13);
            }, 3);
        };

    $scope.cart = orderCart.order.getLiveOrderCart();
    $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;

    var refreshOrderLineTempQty = function (orderline) {
        orderline.btLogic.orderSummaryPageTempQty = parseInt(orderline._OrderedQty);
    };

    angular.forEach($scope.orderLinesArray, function (orderline) {
        refreshOrderLineTempQty(orderline);
    });

    //on this scope's deletion/$destroy do this.
    $scope.$on("$destroy", function () {
        orderCart.order.cleanOrderCart(); //clean out zero quantity lines
    });

    $scope.checkout = function () {
        orderCart.order.cleanOrderCart(); //clean out zero quantity lines
        if (orderCart.orderLine.util.orderlineCount() < 1) {
            return;
        }

        $rootScope.$broadcast('uiBreadcrumbDisplay', { display: false, defer: true });
        $rootScope.$broadcast('uiCheckoutStepProgressDisplay', { display: true, defer: true });
        if (customer.isCustomerSelected()) {
            $location.path('/shippingSelection')
        } else {
            $location.path('/customerSearch')
        }
    };
    $scope.deleteOrderLine = function (line) {
        var primeLineObject = { PrimeLine: line._PrimeLineNo, SubLine: line._SubLineNo};
        orderCart.orderLine.deleteOrderLine(primeLineObject);
    };

    var myScroll = new IScroll('#ItemDetailWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
    $scope.refreshIScroll = function () {
        setTimeout(function () {
            myScroll.refresh();
        }, 500);
    };

    $scope.lineTotal = function (orderline) {
        var price = null;

        //if LinePriceInfo 's _btAdjustedPrice exists then use that price.
        if (orderline.LinePriceInfo && orderline.LinePriceInfo._btAdjustedPrice && isFinite(parseFloat(orderline.LinePriceInfo._btAdjustedPrice)) ) {
            price = parseFloat(orderline.LinePriceInfo._btAdjustedPrice);
        }else{
            //if unit price is defined and a finite number
            if (angular.isDefined(orderline.LinePriceInfo._UnitPrice) && isFinite(parseFloat(orderline.LinePriceInfo._UnitPrice))) {
                price = parseFloat(orderline.LinePriceInfo._UnitPrice);
            }

            if (price !== null) {
                //if sale price is defined, a finite number, AND less than price
                if (angular.isDefined(orderline.LinePriceInfo._RetailPrice) && isFinite(parseFloat(orderline.LinePriceInfo._RetailPrice))) {
                    var temp = parseFloat(orderline.LinePriceInfo._RetailPrice);

                    if (temp < price) {
                        price = temp;
                    }
                }
        
            }else if (angular.isDefined(orderline.LinePriceInfo._RetailPrice) && isFinite(parseFloat(orderline.LinePriceInfo._RetailPrice))) {
                price = parseFloat(orderline.LinePriceInfo._RetailPrice);
            }
        }

        var quantity = isFinite(parseInt(orderline._OrderedQty)) ? parseInt(orderline._OrderedQty) : 1;

        return price ? quantity * price : 'N/A';

    };

    $scope.orderSubtotal = function () {
        var runningTotal = 0.0;

        angular.forEach($scope.orderLinesArray, function (orderline) {
            runningTotal += $scope.lineTotal(orderline);
        });

        return isFinite(runningTotal) ? runningTotal : 'N/A';
    };

    $scope.changeRouteState = function (stateName) {
        $state.go(stateName);
    };

    $scope.addUpcInput = '';

    $scope.addUpc = function () {
        if ($scope.addUpcInput.trim() !== '') {
         $rootScope.$broadcast('Scanner_Event',  $scope.addUpcInput.trim() );
        }
    };

    $scope.$on('scannerAddUpcError', function (event, error) {
        if (error.code === 'InvalidItem:BGT') {
            $loggerService.log("Big Ticket Rejected.");
            $scope.openZipCodeValidation(error.itemObject);
            $loggerService.log(error);
            //$rootScope.$broadcast('Scanner_Event', error.upc, true);
        } else {
            $scope.openErrorModal(error);
        }
        });
    $scope.$on('scannerAddUpcClearInput', function (event) {
         $scope.addUpcInput = '';
    });
    $scope.$on('orderCartDeleted', function (event, data) {
        $scope.cart = orderCart.order.getLiveOrderCart();
        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
        $scope.refreshIScroll();
        angular.forEach($scope.orderLinesArray, function (orderline) {
            refreshOrderLineTempQty(orderline);
        });
        $scope.currentItem = {};
    });

    $scope.currentItem = {};
    $scope.compareCurrentOrderline = function (orderlineInput) {
        if ($scope.isSliderOpen && $scope.currentItem.orderline) {
            return $scope.currentItem.orderline === orderlineInput;
        } else {
            return false;
        }
    }
    $scope.isSliderOpen = false;
    $scope.openQtyModal = function (orderline) {
        //reset all slider
        $scope.sliderShowInputBox = false;
        $scope.sliderInputError = false;
        $scope.sliderInput = '';

        var availableQtyObj = orderCart.orderLine.util.getAvailableInventoryAndOrderedQty(orderline);
        $scope.currentItem.Available = availableQtyObj.largestAvailableInventory - (availableQtyObj.orderedQty - parseInt(orderline._OrderedQty));
        $scope.currentItem.orderline = orderline;
        
        $scope.isSliderOpen = !$scope.isSliderOpen;
    };
    $scope.sliderInput = '';
    $scope.sliderInputError = false;
    var sliderKeyPressHandler = function (event) {
        if (event.keyCode === 27) {
            $scope.sliderClose();
        }
        if (event.keyCode === 13) {
            $scope.sliderSave();
        }
    };
    $scope.sliderClick = function (numberClicked) {
        if ($scope.sliderShowInputBox) {
            if (numberClicked === -1) {
                $scope.sliderInput = "";

                return;
            }
            $scope.sliderInput = $scope.sliderInput + numberClicked.toString();

            if (parseInt($scope.sliderInput) > $scope.currentItem.Available) {
                $scope.sliderInputError = true;
                $scope.sliderInput = $scope.currentItem.Available.toString();
            }
        } else {
            if (parseInt($scope.sliderInput) > $scope.currentItem.Available) {
                numberClicked = $scope.currentItem.Available;
            }
            orderCart.orderLine.setOrderLineQuantity($scope.currentItem.orderline, numberClicked);
            $scope.sliderClose();
        }

    };
    $scope.sliderInputChange = function () {
        if (parseInt($scope.sliderInput) > $scope.currentItem.Available) {
            $scope.sliderInputError = true;
            $scope.sliderInput = $scope.currentItem.Available.toString();
        }
    }
    $scope.sliderChangeInput = function () {
        $scope.sliderShowInputBox = true;
        $document.find('input.sliderButton').first().attr("autofocus", "autofocus") ;
    };
    $scope.sliderButtonDisable = function (buttonValue) {
        if (buttonValue <= $scope.currentItem.Available) {
            return false;
        } else {
            return true;
        }
    };
    $scope.sliderShowInputBox = false;
    $scope.sliderSave = function () {
        if (isFinite(parseInt($scope.sliderInput))) {
            $scope.sliderClose();
            $scope.sliderClick(parseInt($scope.sliderInput));
        } else {
            $scope.sliderClose();
        }
        $loggerService.log('submitted');

    };
    $scope.itemDetail = function (upc) {
        $itemSearch.searchUPC(upc, function (response) {
            if (response.ngroups && response.ngroups == 0) {
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
    };

    $scope.validUPC = function (upc) {
        var isValid = false;
        if (upc) {
            //clean to just numbers
            upc = upc.replace(/\D/g, '');

            if (upc.length > 9) {
                isValid = true;
            }
        }
         return isValid;
    };

    $scope.sliderClose = function () {
        $scope.isSliderOpen = false;
        $scope.sliderInputError = false;
        $scope.sliderShowInputBox = false;
        $document.find('input.sliderButton').first().removeAttr("autofocus");
    };
    $scope.alwaysBlur = function (event) {
        angular.element(event.target).each(function () { this.blur() });
        angular.element(event.target).closest('form').find('input').each(function () { this.focus() });
    };

    // modal
    $scope.openItemDescription = function (currentItem) {

        var modalInstance = $modal
                .open({
                    templateUrl: 'html/item/itemDetailDescription.html',
                    controller: 'itemDescriptionCtrl',
                    resolve: {
                        currentItem: function () {
                            return currentItem;
                        },
                        isCart: function () { return true;}
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
                        return currentItem;
                    }
                }
            });
        modalInstance.result.then(function (result) {
            if ('name' in result && 'message' in result) {
                $scope.openErrorModal(result);
            }
        });
    };

$scope.openErrorModal = function (error) {

        var modalInstance = $modal
                .open({
                    templateUrl: 'html/modalTemplates/error.html',
                    controller: 'cartSummaryErrorCtrl',
                    resolve: {
                                currentError: function () {  return error; }
                             }
                });

};

$scope.openLoadDraftOrder = function () {

    var modalInstance = $modal
            .open({
                templateUrl: 'html/order/draftOrderLoad.html',
                controller: 'draftOrderLoadCtrl',
                backdrop: 'static',
                keyboard: false,
                animation: false
            });
    modalInstance.result.then(function () {
        $scope.cart = orderCart.order.getLiveOrderCart();
        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
        $scope.refreshIScroll();
        angular.forEach($scope.orderLinesArray, function (orderline) {
            refreshOrderLineTempQty(orderline);
        });
        $scope.currentItem = {};
    }, function () {
        $scope.cart = orderCart.order.getLiveOrderCart();
        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
        $scope.refreshIScroll();
        angular.forEach($scope.orderLinesArray, function (orderline) {
            refreshOrderLineTempQty(orderline);
        });
        $scope.currentItem = {};
    })

};

if (isLoadDraftOrder) {
    $scope.openLoadDraftOrder();
}

    }])
    .controller('draftOrderLoadCtrl', ['$scope', '$modalInstance', 'orderCart', 'order', 'customer', 'payment', 'orderCart', 'itemDetail', '$timeout',
        function ($scope, $modalInstance, $orderCart, $order, $customer, $payment, $orderCart, $itemDetail, $timeout) {

            $scope.errorMessages = [];
            $scope.isLoaded = false;

            var myScroll;
            setTimeout(function () {
                myScroll = new IScroll('#ErrorWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
            }, 500);

            $scope.refreshIScroll = function () {
                setTimeout(function () {
                    myScroll.refresh();
                }, 500);
            };

        var _init = function () {
            //get cached order from order service
            var draftOrder = $order.getCurrentOrderDetails();

            //if no order close modal
            if (draftOrder === undefined || draftOrder === null || !angular.isDefined(draftOrder.OrderLines) ||
                !angular.isArray(draftOrder.OrderLines.OrderLine) || (draftOrder.OrderLines.OrderLine.length < 1)) {

                $scope.isLoaded = true;
                $timeout($modalInstance.dismiss, 500);
                return;
            } 
            //delete OrderCart
            $orderCart.order.deleteCart();

            //delete customer from customer and orderCart
            $customer.clearSelectedCustomer(); //removes cached customer
            $orderCart.customer.deleteCustomer();

            //remove any saved paymentData
            $payment.clearPaymentData();

            //find customer which sets customer in the customer service
            if (angular.isString(draftOrder._BillToID) && (draftOrder._BillToID.length > 0)) {
                $customer.retrieveCustomerDetail(draftOrder._BillToID, '', function (customer) {
                    if (angular.isString(customer._CustomerKey) && customer._CustomerKey.length > 0) {
                        $customer.addToCart(customer);
                        $orderCart.customer.setCustomer(customer);
                    }
                });
            }
        

            //gather orderlines to add
            var pricingAndInventoryObject = { productList: [] };
            var skuAddedToPricingAndInventoryObject = {};

            for (var i = 0; i < draftOrder.OrderLines.OrderLine.length; i++) {

                var solarDoc = null;

                //if the btDisplay is a record from solr it will have a _version_ number, if not in solr do not add item
                if (angular.isObject(draftOrder.OrderLines.OrderLine[i].btDisplay) && angular.isDefined(draftOrder.OrderLines.OrderLine[i].btDisplay._version_)) {
                    solarDoc = angular.copy(draftOrder.OrderLines.OrderLine[i].btDisplay);
                } else {

                    var itemName = '';
                    if (angular.isObject(draftOrder.OrderLines.OrderLine[i].btDisplay) && angular.isDefined(draftOrder.OrderLines.OrderLine[i].btDisplay.defaultItemDescription)
                        && angular.isString(draftOrder.OrderLines.OrderLine[i].btDisplay.defaultItemDescription) && (draftOrder.OrderLines.OrderLine[i].btDisplay.defaultItemDescription.length > 0)) {                  
                        itemName = '"' +draftOrder.OrderLines.OrderLine[i].btDisplay.defaultItemDescription + '" ';
                    }
                    $scope.errorMessages.push('Item ' + itemName + (angular.isObject(draftOrder.OrderLines.OrderLine[i].btDisplay) ? (' with UPC: ' + draftOrder.OrderLines.OrderLine[i].btDisplay.id ): '' ) + 
                        ' cannot be found.');
                    continue;
                }

                if (!(solarDoc.sku in skuAddedToPricingAndInventoryObject)) {
                    pricingAndInventoryObject.productList.push(solarDoc);
                    skuAddedToPricingAndInventoryObject[solarDoc.sku] = solarDoc.sku;
                }

            }
            
            //add orderlines and catch and display any exceptions
            var addOrderlines = function (pricePricingAndInventoryObject) {
                if ($orderCart.orderLine.util.orderlineCount() > 0) {
                    var orderCart = $orderCart.order.getLiveOrderCart();

                    var deleteOrderLines = [];
                    for (var p = 0; p < orderCart.OrderLines.OrderLine.length; p++) {
                        deleteOrderLines.push($orderCart.orderLine.util.getPrimeSubLineObject(orderCart.OrderLines.OrderLine[p]));
                    }

                    $orderCart.orderLine.deleteOrderLine(deleteOrderLines);
                }

                for (var i = 0; i < draftOrder.OrderLines.OrderLine.length; i++) {
                    var currentOrderLine = draftOrder.OrderLines.OrderLine[i];
                    var amountToOrder = isFinite(Number.parseInt(currentOrderLine._OrderedQty)) ? Number.parseInt(currentOrderLine._OrderedQty) : 1;
                    if (amountToOrder < 0) {
                        amountToOrder = 1;
                    }

                    var itemPriced = null;

                    for (var j = 0; j < pricePricingAndInventoryObject.productList.length; j++) {
                        if (pricePricingAndInventoryObject.productList[j].sku === currentOrderLine.btDisplay.sku) {

                            itemPriced = pricePricingAndInventoryObject.productList[j];
                            break;
                        }
                    }

                    try{
                        $orderCart.orderLine.addOrderLine(itemPriced, amountToOrder, true);

                    } catch (error) {

                        $scope.errorMessages.push(error.message);
                        $scope.refreshIScroll();
                    }
                }

                $scope.isLoaded = true;
                $scope.refreshIScroll();

                if (angular.isArray($scope.errorMessages) && $scope.errorMessages.length === 0) {

                    $timeout($modalInstance.dismiss, 500);
                }
            };

            //price each unique itemObject
            if (pricingAndInventoryObject.productList.length > 0) {
                $itemDetail(pricingAndInventoryObject, addOrderlines, function () { });
            } else {
                $scope.isLoaded = true;
                $scope.refreshIScroll();
            }
        }

        $scope.close = function () { $modalInstance.close(); };

        _init();
    }])
.controller('quantityEntryCtrl', ['$scope', '$modalInstance', 'orderline', 'orderCart',
				function ($scope, $modalInstance, orderline, orderCart) {

				    var initial = parseInt(orderline._OrderedQty);
				    var displayNew = '';
				    $scope.display = function () {
				        if (displayNew.trim() === "") {
				            return initial;
				        } else {
				            return displayNew;
				        }
				    }
				    //object form: {largestAvailableInventory:20, orderedQty: 0}
				    var availableQtyObj = orderCart.orderLine.util.getAvailableInventoryAndOrderedQty(orderline);
				    $scope.maxAvailable = availableQtyObj.largestAvailableInventory - (availableQtyObj.orderedQty - parseInt(orderline._OrderedQty));

				    $scope.input = function (input) {
				        displayNew = displayNew.toString() + input.toString();

				        if (parseInt(displayNew) > $scope.maxAvailable) {
				            displayNew = $scope.maxAvailable.toString();
				        }
				        
				    }
				    $scope.backspace = function () {
				        displayNew = displayNew.substring(0, displayNew.length - 1);

				        if (parseInt(displayNew) > $scope.maxAvailable) {
				            displayNew = $scope.maxAvailable.toString();
				        }
				    };
				    $scope.save = function () {
				    };

				    $scope.cancel = function () {
				        $modalInstance.dismiss('cancel');
				    };

				}])
.controller('cartSummaryErrorCtrl',['$scope','$modalInstance','currentError', function($scope, $modalInstance, currentError){
    $scope.currentError = currentError;
    $scope.cancel = function () {
				        $modalInstance.dismiss('cancel');
				    };
}])

;