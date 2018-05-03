/// <reference path="../appFilters.js" />
angular.module('paymentsummary', ['ui.bootstrap', 'ui.router'])
    .controller('paymentSummaryCtrl', [
        '$scope',
        '$location',
        '$modal',
        '$state',
        'customer',
        '$rootScope',
        'orderCart',
        'order',
        'payment',
        'repriceService',
        'numberKeyPressValidator',
        'currencyKeyPressValidator',
        'msrService',
        'loggerService',
        'serviceArrayFix',
        'alertMessages',
        'POSService',
        '$q',
        'sendSMTPErrorEmail',
        '$timeout',
        'securityService',
        '$filter',
        'btProp',
        function ($scope, $location, $modal, $state, $customer, $rootScope, $orderCart, $order, $payment, $repriceService, $numberKeyPressValidator, $currencyKeyPressValidator, $msrService, $loggerService, $serviceArrayFix, $alertMessages, POSService, $q, $sendSMTPErrorEmail, $timeout, $securityService, $filter, $btProp) {

            //init scope
            $scope.cart = $orderCart.order.getLiveOrderCart();
            $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
            $scope.numberValidator = $numberKeyPressValidator;
            $scope.currencyValidator = $currencyKeyPressValidator;
            $scope.txtCouponFromPayment = "";
            $scope.hasConfirmButtonBeenClicked = false;
                              
            
            var isUseCsrInContactPayments = $btProp.getProp("isUseCsrInContactPayments") === null ? true : $btProp.getProp("isUseCsrInContactPayments");

            $scope.showCsrPayments = function () {
                return isUseCsrInContactPayments && $securityService.isCsr();
            };

            $scope.priceChangeArr = [];
            angular.forEach($scope.orderLinesArray, function (orderLine) {
                $scope.priceChangeArr.push(false);
            });

            //Our lovely IScroller  made us famous.
            var myScroll = new IScroll('#paymentOrderCartWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
            $scope.refreshIScroll = function () {
                setTimeout(function () {
                    myScroll.refresh();
                }, 500);
            };


            //should this moved to app.js UIRouter state listener section. It looks like we need do this a few more places.
            $orderCart.address.validateOrderAddresses().then(
                function (data) {
                    //validate order header attributes like _CustomerEMailID
                    var headerErrors = $orderCart.order.validateOrderHeader();

                    if (data.defaultBillingAddressError.hasError ||
                        data.defaultShipToAddressError.hasError ||
                        data.carrierServiceCodeError.hasError ||
                        data.bigTicketError.hasError ||
                        data.orderlineShiptoAddressError.hasError ||
                        headerErrors.customerEmailIdError.hasError
                    ) {

                        $state.go('shippingSelection');
                    }
                },
                function (data) {
                    $loggerService.log(data);
                    $state.go('shippingSelection');
                }
            );

            //Read cached tenders or ADC saved in previous sessions.
            var cachedPaymentData = $payment.getPaymentData();
            if (cachedPaymentData) {
                $scope.paymentData = cachedPaymentData;
                //it looks like persisting the cart not working. acc is getting lost.
                //keep patching....
                $scope.cart.Extn._ExtnAccID = $scope.paymentData.accNumber ? $scope.paymentData.accNumber : '';
            } else {
                $scope.paymentData = {};
                $scope.paymentData.cards = [];
                $scope.paymentData.inContactPaymentId = null;
            }


            $scope.addCoupon = function (CouponID) {
                $scope.txtCouponFromPayment = "";
                if ($scope.cart.Promotions.Promotion != undefined) {
                    var i = $scope.cart.Promotions.Promotion.length;
                    $scope.cart.Promotions.Promotion[i] = {
                        _PromotionId: CouponID,
                        _PromotionType: "PROMO",
                    }
                } else {
                    $scope.cart.Promotions = {
                        Promotion: [
                            {
                                _PromotionId: CouponID,
                                _PromotionType: "PROMO",
                                //C00250101E0  
                            }
                        ]
                    }
                }

                
                $repriceService.reprice($scope.cart).then(function (response) {
                    $scope.cart = response.data;
                    $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
                    $orderCart.order.setOrderCart($scope.cart);
                    $loggerService.log(response.data);
                    if ($scope.cart.Promotions.Promotion != undefined) {
                        var i = ($scope.cart.Promotions.Promotion.length - 1);
                        if ($scope.cart.Promotions.Promotion[i]._PromotionApplied == "Y") {
                            $scope.description = $scope.cart.Promotions.Promotion[i]._Description;
                        } else {
                            $scope.description = $scope.cart.Promotions.Promotion[i]._DenialReason;
                        }
                        swal({ title: "Coupon", text: $scope.description, timer: 3000, showConfirmButton: false });
                    }
                })

            }


            var _registerCouponHandler = function () {

                return $rootScope.$on('Scanner_Event', function (event, data) {
                    if (data && paymentSummaryScreen) {
                        $scope.txtCouponFromPayment = data;
                        $scope.addCoupon($scope.txtCouponFromPayment);
                    }
                    if ($scope.cart.Promotions.Promotion != undefined) {
                        var i = ($scope.cart.Promotions.Promotion.length - 1);
                        if ($scope.cart.Promotions.Promotion[i]._PromotionApplied == "Y") {
                            $scope.description = $scope.cart.Promotions.Promotion[i]._Description;
                        } else {
                            $scope.description = $scope.cart.Promotions.Promotion[i]._DenialReason;
                        }
                        swal({ title: "Coupon", text: $scope.description, timer: 3000, showConfirmButton: false });
                    }


                });
            };

            //Listen to Scanner_Event_Coupon_GC events. This call returns a function for unregistering the listener.
            var paymentSummaryScreen = true;
            var deregister = _registerCouponHandler();


            //unregister listeners and save cart before destroying the controller.
            $scope.$on("$destroy", function () {
                if ($orderCart.btLogic && $orderCart.btLogic.cartTimeStamp == $scope.cart.btLogic.cartTimeStamp) {
                    $orderCart.order.setOrderCart($scope.cart);
                }
                deregister();
            });



            //check if card already exists on the order by looking for matching token
            function cardExists(card) {
                for (var i = 0; i < $scope.paymentData.cards.length; i++) {
                    var temp = $scope.paymentData.cards[i];
                    if (Number(temp.token) == Number(card.token)) {
                        temp.selected = true;
                        return true;
                    }
                }
                return false;
            }

            $scope.numberOfGiftCards = function () {
                var count = 0;
                $scope.paymentData.cards.forEach(function (card) {
                    if (card.cardType == 'GIFTCARD') {
                        count++;
                    }
                });
                return count;
            }
            $scope.numberOfCreditCards = function () {
                var count = 0;
                $scope.paymentData.cards.forEach(function (card) {
                    if (card.cardType != 'GIFTCARD') {
                        count++;
                    }
                });
                return count;
            }

            $scope.$watchCollection('cart', function () {
                $scope.calculateBalanceRefunds();
            });

            $scope.numberOfCards = function () {
                return $scope.paymentData.cards.length;
            }

            $scope.$watchCollection('paymentData.cards', function () {
                $payment.setPaymentData($scope.paymentData);
                reprice().then(function () { $scope.calculateBalanceRefunds(); });
            });

            $scope.balanceRefund = 0;
            $scope.calculateBalanceRefunds = function () {
                //because Order Total is a float a float multiplication will have that can hit bad fractions. So we round to nearest integer.
                var totalTimeHundred = Math.round(Number($scope.orderTotal()) * 100);

                $scope.paymentData.cards.forEach(function (card) {
                    card.chargeAmount = 0;
                })
                //Gift First
                for (var i = 0; i < $scope.paymentData.cards.length; i++) {
                    var card = $scope.paymentData.cards[i];
                    if (card.cardType == 'GIFTCARD') {
                        if (card.gcBalance === null || !isFinite(card.gcBalance))
                        {
                            continue;
                        }

                        var curCardBalanceTimesHundred = Math.round(Number(card.gcBalance) * 100);

                        if (curCardBalanceTimesHundred >= totalTimeHundred) {
                            //this is integer divided by integer which will give proper two decimal value
                            card.chargeAmount = totalTimeHundred / 100;
                            card.chargeAmount = card.chargeAmount + "";
                            totalTimeHundred = 0;
                        } else {
                            card.chargeAmount = "" + card.gcBalance;
                            totalTimeHundred = totalTimeHundred - curCardBalanceTimesHundred;
                        }
                    }
                }

                //Last CC
                if (totalTimeHundred > 0) {
                    for (var i = 0; i < $scope.paymentData.cards.length; i++) {
                        var card = $scope.paymentData.cards[i];
                        if (card.cardType != 'GIFTCARD') {
                            card.chargeAmount = totalTimeHundred / 100;
                            card.chargeAmount = card.chargeAmount + "";
                            totalTimeHundred = 0;
                        }
                    }
                }

                $scope.balanceRefund = totalTimeHundred < 0 ? 0 : totalTimeHundred / 100;
            }

            //before calling reprice we need to add payments if we have any to get additional discounts.
            function reprice(index) {
                var defer = $q.defer();

                delete $scope.cart.PaymentMethods;
                $scope.cart.PaymentMethods = {
                    PaymentMethod: $payment.createRepricePaymentContract($scope.paymentData.cards)
                };

                $repriceService.reprice($scope.cart).then(
                    function (response) {
                        $scope.cart = response.data;
                        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
                        $orderCart.order.setOrderCart($scope.cart);
                        $loggerService.log(response);
                        if ($scope.cart.Errors &&
                            $scope.cart.Errors.ErrorList &&
                            $scope.cart.Errors.ErrorList.Error &&
                            $scope.cart.Errors.ErrorList.Error.length > 0) {
                            if ($scope.cart.Errors.ErrorList.Error[0]._ErrorCode != '00') {
                                var errorCode = $scope.cart.Errors.ErrorList.Error[0]._ErrorCode;
                                var errDescription = $scope.cart.Errors.ErrorList.Error[0]._ErrorDescription;
                                if ((angular.equals(errorCode, '99') && errDescription.indexOf("THE MAXIMUM PRICE OVERRIDE FOR A") > -1)) {
                                    if ($scope.orderLinesArray.length > index) {
                                        $scope.orderLinesArray[index].Extn._ExtnIsPriceLocked = 'N';
                                        $scope.orderLinesArray[index].LinePriceInfo._UnitPrice = "0.00";
                                        $scope.orderLinesArray[index].LinePriceInfo._ChargeAmount = "0.00";
                                        $scope.priceChangeArr[index] = false;
                                        reprice();
                                    }
                                }
                                swal({ title: "Alert!", text: errDescription, showConfirmButton: true });
                            }
                        }

                        defer.resolve();
                    },
                    function (response) {
                        $loggerService.log(response);
                        swal({ title: "Alert!", text: $alertMessages.paymentMessages.REPRICE_ERROR, showConfirmButton: true });
                        defer.resolve();
                    }
                );

                return defer.promise;
            }

            $scope.priceModified = function (index, setNotModified) {
                if (setNotModified) {
                    $scope.priceChangeArr[index] = false;
                } else {
                    $scope.priceChangeArr[index] = true;
                }
                
            };

            $scope.priceOverride = function (orderLine, index) {
                //Item Type
                var itemType = orderLine.ItemDetails.PrimaryInformation._ItemType
                if (Number($scope.priceChangeArr[index]) < 0) {
                    swal({ title: "Alert!", text: $alertMessages.paymentMessages.NEGATIVE_AMOUNT_NOT_ALLOWED, showConfirmButton: true });
                } else {
                    if (orderLine.Extn._ExtnIsPriceLocked == 'N') {
                        orderLine.Extn._ExtnIsPriceLocked = 'Y';
                        //for (var x = 0; x < $scope.priceChangeArr.length; x++) {
                        //    if (x !== index) {
                        //        $scope.priceChangeArr[x] = false;
                        //    }
                        //}
                    } else {
                        orderLine.Extn._ExtnIsPriceLocked = 'N';
                        $scope.priceChangeArr[index] = false;
                    };

                    angular.forEach($scope.cart.OrderLines.OrderLine, function (orderLine) {
                        orderLine = $orderCart.order.deleteShippingDiscount(orderLine);
                    });

                    if (orderLine.LinePriceInfo._UnitPrice == "") {
                        orderLine.LinePriceInfo._UnitPrice = "0.00";
                    }

                    reprice(index);
                }
            }

            var _priceRegEx = /^(\d+\.{0,1}|\.)\d{0,2}$/;
            $scope.onBlurCheckOverrideValue = function (orderLine, index) {

                if (angular.isObject(orderLine) && angular.isObject(orderLine.LinePriceInfo)) {
                    var stringPrice = angular.isString(orderLine.LinePriceInfo._UnitPrice) ? orderLine.LinePriceInfo._UnitPrice.trim() : "";

                    if (!_priceRegEx.test(stringPrice)) {
                        if (angular.isObject(orderLine.btDisplay) &&
                            angular.isObject(orderLine.btDisplay.itemDetail) &&
                            angular.isObject(orderLine.btDisplay.itemDetail.ComputedPrice) &&
                            angular.isString(orderLine.btDisplay.itemDetail.ComputedPrice._UnitPrice) &&
                            _priceRegEx.test(orderLine.btDisplay.itemDetail.ComputedPrice._UnitPrice))
                        {
                            var _pretty = $filter('currency')(orderLine.btDisplay.itemDetail.ComputedPrice._UnitPrice.trim(), "");
                            //clean out any extra commas from thousand...  1,000.00 -> 1000.00

                            orderLine.LinePriceInfo._UnitPrice = _pretty.replace(/,/g, '');

                            $scope.priceModified(index, true);
                        }else{
                            orderLine.LinePriceInfo._UnitPrice = "0.00";
                        }
                    }
                }
            };

            //Add tender type. this is cached in the controller first then we persist in the paymentService.
            function addCard(card) {
                if ((card.cardType != 'GIFTCARD') && $scope.numberOfCreditCards() > 0) {
                    swal({ title: "Alert!", text: $alertMessages.paymentMessages.MAX_ALLOWED_CC, showConfirmButton: true });
                    return;
                }
                if (card.cardType == 'GIFTCARD' && $scope.numberOfGiftCards() == 4) {
                    swal({ title: "Alert!", text: $alertMessages.paymentMessages.MAX_GC, showConfirmButton: true });
                    return;
                }
                if ($scope.paymentData.accNumber && card.cardType != 'GIFTCARD') {
                    swal({ title: "Alert!", text: $alertMessages.paymentMessages.CC_NOT_ALLOWED_WITH_ADC, showConfirmButton: true });
                    return;
                }
                if (!cardExists(card)) {
                    $scope.paymentData.cards.push(card);
                }
            }

            $scope.calculateLineTotal = function (orderLine) {
                return $order.calculateOrderLineTotal(orderLine);
            }


            $scope.orderTotal = function () {
                return $scope.cart._OrderTotal;
            }
            $scope.taxTotal = function () {
                return $order.calculateTaxTotal($scope.cart);
            }

            $scope.shippingTotal = function () {
                return $order.calculateShippingTotal($scope.cart)
            }

            $scope.orderSubTotal = function () {
                return $order.calculateOrderSubTotal($scope.cart);
            };

            $scope.merchandiseTotal = function () {
                return $order.calculateMerchandiseTotal($scope.cart);
            };
            $scope.couponDiscountTotal = function () {
                return $order.calculateCouponDiscountTotal($scope.cart);
            };
            $scope.misChargesTotal = function () {
                return $order.calculateMisChargesTotal($scope.cart);
            };
            $scope.assocDiscTotal = function () {
                return $order.calculateAssocDiscTotal($scope.cart);
            };

            $scope.lineTotal = function (orderline) {
                return $order.lineTotal(orderline);

            };

            $scope.isPriceOverideOccuring = function () {
                for (var i = 0; i < $scope.priceChangeArr.length; i++) {
                    if ($scope.priceChangeArr[i] && (!angular.isDefined($scope.cart.OrderLines.OrderLine[i].Extn) || !angular.isDefined($scope.cart.OrderLines.OrderLine[i].Extn._ExtnIsPriceLocked) || $scope.cart.OrderLines.OrderLine[i].Extn._ExtnIsPriceLocked == 'N')) {
                        return true;
                    }
                }
                return false;
            };

            //Hard Work done. Create the order.
            $scope.confirmOrder = function () {
                $scope.hasConfirmButtonBeenClicked = true;
                //if there is giftcard being used, validate that there are no Big Ticket items on the order
                if ($scope.numberOfGiftCards() > 0) {

                    if ($orderCart.order.hasBigTicketItems($scope.cart)) {

                        swal({ title: "Big Ticket Error", text: 'Orders with Big Ticket items cannot be paid with gift cards.  Please remove all gift cards from the order.', showConfirmButton: true });
                        $scope.hasConfirmButtonBeenClicked = false;
                        return;
                    }
                }

                if ($scope.cart._OrderTotal > 0) {

                    //check cart pricing
                    var totalUnitPrice = 0;
                    angular.forEach($scope.cart.OrderLines.OrderLine, function (orderLine) {
                        var unitPrice = parseFloat(orderLine.LinePriceInfo._UnitPrice);
                        if (isFinite(unitPrice)) {
                            totalUnitPrice = totalUnitPrice + unitPrice;
                        }
                    });
                    if (totalUnitPrice == 0.00) {
                        swal({ title: "Error!", text: "Total Unit Price cannot be zero", showConfirmButton: true });
                        $scope.hasConfirmButtonBeenClicked = false;
                        return false;
                    }
                    if (totalUnitPrice < 0) {
                        swal({ title: "Error!", text: "Total Unit Price cannot less than zero", showConfirmButton: true });
                        $scope.hasConfirmButtonBeenClicked = false;
                        return false;
                    }



                    //grab orderNumber for the local $scope.cart
                    var orderNumber = "";

                    $orderCart.order.getOrderNumber().then(function (orderNo) {

                        $scope.cart._OrderNo = orderNo;

                        //call auth and add payment data to cart (during authRequest function)
                        $payment.authRequest($scope.cart, $scope.paymentData.cards).then(function () {

                            // create the order, and redirect if successful.
                            _asyncOrderCreate();

                        }, function () {
                            $scope.hasConfirmButtonBeenClicked = false;
                        });

                    }, function () {

                        swal({ title: "Order Number Error", text: 'Could not retrieve an Order Number.  Please try again.', showConfirmButton: true });
                        $scope.hasConfirmButtonBeenClicked = false;

                    });
                }
                else {
                    swal({ title: "Error!", text: "Line total cannot be zero", showConfirmButton: true });
                    $scope.hasConfirmButtonBeenClicked = false;
                    return false;
                }

            };

            var _asyncOrderCreate = function () {

                delete $scope.cart._OrderDate;

                //TODO: remove debug only
                //var report = angular.copy($scope.cart);
                //$sendSMTPErrorEmail(angular.toJson(report), "Cart sent in." + serviceURL);

                $order.createOrder($scope.cart, function (response) {
                    var selectedOrder = {};
                    selectedOrder._OrderNo = response._OrderNumber;
                    $order.setSelectedOrder(selectedOrder);

                    $orderCart.order.deleteCart();
                    $orderCart.customer.deleteCustomer();
                    $customer.clearSelectedCustomer(); //removes cached customer

                    //not necessary, but nice to do
                    $payment.clearPreviousAuthCache();

                    $payment.clearPaymentData();
                    $scope.hasConfirmButtonBeenClicked = false;
                    $rootScope.$broadcast('uiBreadcrumbDisplay', { display: true, defer: true, orderCompleteReset: true });
                    $rootScope.$broadcast('uiCheckoutStepProgressDisplay', { display: false, defer: true });
                    $location.path("/orderDetail");
                }, function (error) {

                    //check for standard JMS or Negative Orderline errors
                    $scope.hasConfirmButtonBeenClicked = false;
                    var errorAsString = angular.toJson(error);
                    if ((/Order total can not be negative/i).test(errorAsString)) {
                        swal({ title: "Order Create Error", text: 'Order total can not be negative', showConfirmButton: true });
                    } else if ((/javax.jms.JMSException/i).test(errorAsString)) {
                        swal({ title: "Order Create Error", text: 'Java JMS error. If there are not giftcards as payment. Please retry order confirmation.', showConfirmButton: true });
                    } else {
                        var errorText = '';
                        if (angular.isString(error)) {
                            errorText = error;
                        } else {
                            errorText = angular.toJson(error);
                        }
                        swal({ title: "Order Create Error", text: errorText, showConfirmButton: true });
                    }
                    $loggerService.log(error);
                });

            };

            $scope.openOrderPricingDetail = function () {
                var modalInstance = $modal
                    .open({
                        templateUrl: 'html/order/orderPricingDetail.html',
                        controller: 'orderPricingDetailCtrl',
                        resolve: {
                            cart: function () {
                                return $scope.cart;
                            }
                        }
                    });

                modalInstance.result.then(function (result) {
                    var newShippingDiscount = result.newShippingDiscount;
                    var cart = result.cart;

                    var previousOrderShippingTotal = cart._ShippingChrgTotal - cart._ShippingDiscTotal;
                    var newActualShippingDiscount = 0;

                    if (newShippingDiscount && (newShippingDiscount > 0) && (previousOrderShippingTotal > 0)) {
                        angular.forEach(cart.OrderLines.OrderLine, function (orderLine) {
                            var lineShippingTotal = 0;
                            var shippingCharge = 0;

                            //Calculate proportion of shipping accounted for by this line
                            angular.forEach(orderLine.LineCharges.LineCharge, function (lineCharge) {
                                if (lineCharge._ChargeCategory === 'BTN_SHIP_CHRG') {
                                    lineShippingTotal += parseFloat(lineCharge._ChargeAmount);
                                    shippingCharge = parseFloat(lineCharge._ChargeAmount);
                                } else if (lineCharge._ChargeCategory === 'BTN_SHIP_DISC') {
                                    lineShippingTotal -= parseFloat(lineCharge._ChargeAmount);
                                }
                            });

                            var lineProportionOfTotal = lineShippingTotal / previousOrderShippingTotal;


                            //Apply proportionate shipping charge to this line
                            var amountToCharge = (((cart._ShippingChrgTotal - newShippingDiscount) * lineProportionOfTotal / orderLine._OrderedQty).toFixed(2) * orderLine._OrderedQty).toFixed(2);
                            var hasShippingDiscount = false;
                            angular.forEach(orderLine.LineCharges.LineCharge, function (charge) {
                                if (charge._ChargeCategory === 'BTN_SHIP_DISC') {
                                    hasShippingDiscount = true;
                                    charge._ChargePerUnit = ((shippingCharge - amountToCharge) / orderLine._OrderedQty).toFixed(2);
                                    charge._ChargeAmount = charge._ChargePerUnit * orderLine._OrderedQty;
                                    newActualShippingDiscount += charge._ChargeAmount;
                                }
                            });

                            if (!hasShippingDiscount) {
                                orderLine.LineCharges.LineCharge.push({
                                    _ChargeAmount: shippingCharge - amountToCharge,
                                    _ChargeCategory: "BTN_SHIP_DISC",
                                    _ChargeName: "DISC_SHIPPING",
                                    _ChargePerLine: "0.00",
                                    _ChargePerUnit: ((shippingCharge - amountToCharge) / orderLine._OrderedQty).toFixed(2),
                                    _IsDiscount: "Y",
                                    _IsManual: "Y",
                                    _Reference: ""
                                });
                                newActualShippingDiscount += shippingCharge - amountToCharge;
                            }
                        });


                        //Distribute the remaining cents, if any, to make total correct
                        var remainingShippingCostToDistribute = (newShippingDiscount - newActualShippingDiscount).toFixed(2);
                        if (remainingShippingCostToDistribute !== 0) {


                            //Try for perfect fit
                            angular.forEach(cart.OrderLines.OrderLine, function (orderLine) {
                                if ((orderLine._OrderedQty * .01).toFixed(2) == remainingShippingCostToDistribute) {
                                    angular.forEach(orderLine.LineCharges.LineCharge, function (charge) {
                                        if (charge._ChargeCategory === 'BTN_SHIP_DISC') {
                                            if (remainingShippingCostToDistribute > 0) {
                                                var amountToAdd = .01 * orderLine._OrderedQty;
                                                charge._ChargeAmount = (parseFloat(charge._ChargeAmount) + amountToAdd).toFixed(2);
                                                charge._ChargePerUnit = (parseFloat(charge._ChargePerUnit) + .01).toFixed(2);
                                                remainingShippingCostToDistribute -= amountToAdd;
                                            } else {
                                                var amountToSubtract = .01 * orderLine._OrderedQty;
                                                charge._ChargeAmount = (parseFloat(charge._ChargeAmount) - amountToSubtract).toFixed(2);
                                                charge._ChargePerUnit = (parseFloat(charge._ChargePerUnit) - .01).toFixed(2);
                                                remainingShippingCostToDistribute += amountToSubtract;
                                            }
                                        }
                                    });
                                }
                            });
                        }


                        //Settle for as close to entered total as possible
                        if (remainingShippingCostToDistribute > 0) {
                            angular.forEach(cart.OrderLines.OrderLine, function (orderLine) {
                                var orderLineShippingTotal = 0;
                                var orderLineShippingCharge = 0;

                                angular.forEach(orderLine.LineCharges.LineCharge, function (lineCharge) {
                                    if (lineCharge._ChargeCategory === 'BTN_SHIP_CHRG') {
                                        orderLineShippingTotal += parseFloat(lineCharge._ChargeAmount);
                                        orderLineShippingCharge = parseFloat(lineCharge._ChargeAmount);
                                    } else if (lineCharge._ChargeCategory === 'BTN_SHIP_DISC') {
                                        orderLineShippingTotal -= parseFloat(lineCharge._ChargeAmount);
                                    }
                                });

                                if (remainingShippingCostToDistribute > 0 && orderLineShippingTotal > remainingShippingCostToDistribute) {
                                    angular.forEach(orderLine.LineCharges.LineCharge, function (charge) {
                                        if (charge._ChargeCategory === 'BTN_SHIP_DISC') {
                                            if (remainingShippingCostToDistribute == 0 || orderLine._ChargeAmount == 0) {
                                                //skip
                                            } else if (remainingShippingCostToDistribute < (orderLine._OrderedQty * .01).toFixed(2)) {
                                                var amountToAdd = .01 * orderLine._OrderedQty;
                                                charge._ChargeAmount = (parseFloat(charge._ChargeAmount) + amountToAdd).toFixed(2);
                                                charge._ChargePerUnit = (parseFloat(charge._ChargePerUnit) + .01).toFixed(2);
                                                remainingShippingCostToDistribute -= amountToAdd;
                                            } else {
                                                var perUnitAmountToAdd = parseFloat(charge._ChargePerUnit) + (remainingShippingCostToDistribute / orderLine._OrderedQty).toFixed(2) * orderLine._OrderedQty;
                                                charge._ChargePerUnit = perUnitAmountToAdd;
                                                charge._ChargeAmount = charge._ChargePerUnit * orderLine._OrderedQty;
                                                remainingShippingCostToDistribute -= charge._ChargeAmount;
                                            }
                                        }
                                    });
                                } else {
                                    angular.forEach(orderLine.LineCharges.LineCharge, function (charge) {
                                        if (charge._ChargeCategory === 'BTN_SHIP_DISC') {
                                            var previousChargeAmount = charge._ChargeAmount;
                                            charge._ChargePerUnit = (orderLineShippingCharge / orderLine._OrderedQty).toFixed(2);
                                            charge._ChargeAmount = charge._ChargePerUnit * orderLine._OrderedQty;
                                            remainingShippingCostToDistribute = remainingShippingCostToDistribute + previousChargeAmount - charge._ChargeAmount;
                                        }
                                    });
                                }
                            });
                        }

                        $repriceService.reprice(cart).then(function (response) {
                            $scope.cart = response.data;
                            $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
                            $orderCart.order.setOrderCart(cart);
                            $loggerService.log(response.data);
                        });
                    }
                }, function () {

                });
            };


            $scope.openOrderLinePricingDetail = function (orderLine) {
                var modalInstance = $modal
                    .open({
                        templateUrl: 'html/order/orderLineAdditionalDetails.html',
                        controller: 'orderLinePricingDetailCtrl',
                        resolve: {
                            orderLine: function () {
                                return orderLine;
                            }
                        }
                    });

                modalInstance.result.then(function (additionalDiscount) {
                    if (additionalDiscount && additionalDiscount > 0) {
                        var hasShippingDiscount = false;
                        angular.forEach(orderLine.LineCharges.LineCharge, function (charge) {
                            if (charge._ChargeCategory === 'BTN_SHIP_DISC') {
                                var newShippingDiscount = parseFloat(charge._ChargeAmount) + parseFloat(additionalDiscount);
                                hasShippingDiscount = true;
                                charge._ChargePerUnit = (newShippingDiscount / orderLine._OrderedQty).toFixed(2);
                                charge._ChargeAmount = (charge._ChargePerUnit * orderLine._OrderedQty).toFixed(2);
                            }
                        });

                        if (!hasShippingDiscount) {
                            orderLine.LineCharges.LineCharge.push({
                                _ChargeAmount: (additionalDiscount / orderLine._OrderedQty).toFixed(2) * orderLine._OrderedQty,
                                _ChargeCategory: "BTN_SHIP_DISC",
                                _ChargeName: "DISC_SHIPPING",
                                _ChargePerLine: "0.00",
                                _ChargePerUnit: (additionalDiscount / orderLine._OrderedQty).toFixed(2),
                                _IsDiscount: "Y",
                                _IsManual: "Y",
                                _Reference: ""
                            });
                        }
                    }

                    $repriceService.reprice($scope.cart).then(function (response) {
                        $scope.cart = response.data;
                        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
                        $orderCart.order.setOrderCart($scope.cart);
                        $loggerService.log(response.data);
                    });
                }, function () { });
            };

            var isRetalixReady = null;
            var retalixStatusPolling = 0;

            POSService.getPmmConfigAndStatus().then(function (data) {
                isRetalixReady = (/^true$/i).test(data.UseEPS) ? true : false;
            }, function (data) {
                isRetalixReady = false;
            });

            $scope.openPaymentEntry =
                    function () {
                        deregister()
                        paymentSummaryScreen = false;
                        var modalInstance = undefined;

                        //if user is CSR -> ( if using InContact Payment, go to Retalix payment entry, else go to old payment entry for manual entry )
                        if ($securityService.isCsr())
                        {
                            isRetalixReady =  $scope.showCsrPayments();
                        }

                        if (isRetalixReady === null) {
                            retalixStatusPolling = retalixStatusPolling + 1;

                            if (retalixStatusPolling < 5) {
                                $timeout($scope.openPaymentEntry, 200);
                            } else {
                                isRetalixReady = false;
                                $scope.openPaymentEntry();
                            }
                        }
                        else {
                            if (isRetalixReady) {
                                modalInstance = $modal
                .open({
                    templateUrl: 'html/payment/paymentEntryRetalix.html',
                    controller: 'paymentEntryRetalixCtrl',
                    backdrop: 'static',
                    resolve: {
                        billTo: function () {
                            return $scope.cart.PersonInfoBillTo;
                        },
                        paymentData: function () {
                            return $scope.paymentData;
                        },
                        orderLine: function () {
                            return $scope.orderLinesArray;
                        }
                    }
                });
                            } else {
                                modalInstance = $modal
                                    .open({
                                        templateUrl: 'html/payment/paymentEntry.html',
                                        backdrop: 'static',
                                        controller: 'paymentEntryCtrl',
                                        resolve: {
                                            billTo: function () {
                                                return $scope.cart.PersonInfoBillTo;
                                            },
                                            paymentData: function () {
                                                return $scope.paymentData;
                                            },
                                            orderLine: function () {
                                                return $scope.orderLinesArray;
                                            }

                                        }
                                    });
                            }

                            modalInstance.result.then(function (card) {
                                deregister = _registerCouponHandler();
                                addCard(card);
                                paymentSummaryScreen = true;
                            }, function () {
                                deregister = _registerCouponHandler();
                            });
                        }
                    };

            $scope.openInContactPaymentEntry =
                    function () {
                        deregister()
                        paymentSummaryScreen = false;
                        var modalInstance = $modal.open(
                            {
                                templateUrl: 'html/payment/inContactPaymentEntry.html',
                                backdrop: 'static',
                                controller: 'InContactPaymentEntryCtrl',
                                resolve: {
                                    billTo: function () {
                                        return $scope.cart.PersonInfoBillTo;
                                    },
                                    paymentData: function () {
                                        return $scope.paymentData;
                                    }
                                }
                            });

                            modalInstance.result.then(function (card) {
                                deregister = _registerCouponHandler();

                                //add payment id
                                if (card && angular.isString(card.inContactPaymentId) && card.inContactPaymentId.length > 0) {
                                    $scope.paymentData.inContactPaymentId = card.inContactPaymentId;
                                }

                                //add card
                                addCard(card);
                                paymentSummaryScreen = true;

                            }, function (inContactId) {

                                if (inContactId && angular.isString(inContactId.inContactPaymentId) && inContactId.inContactPaymentId.length > 0) {
                                    $scope.paymentData.inContactPaymentId = inContactId.inContactPaymentId;
                                }
                                deregister = _registerCouponHandler();
                            });
                        
                    };

            var saveACCtoCart = function (accNumber) {
                $scope.paymentData.accNumber = accNumber.toString();
                $scope.cart.Extn._ExtnAccID = accNumber.toString();
                reprice();
            }

            $scope.openACCEntry = function () {
                if ($scope.numberOfCreditCards() > 0) {
                    swal({ title: "Alert!", text: $alertMessages.paymentMessages.ADC_NOT_ALLOWED_WITH_CC, showConfirmButton: true });
                    return;
                }
                if ($scope.paymentData.accNumber) {
                    $scope.paymentData.accNumber = undefined;
                    delete $scope.cart.Extn._ExtnAccID;
                    reprice();
                    return;
                }
                var modalInstance = $modal
                    .open({
                        templateUrl: 'html/payment/accEntry.html',
                        controller: 'accEntryCtrl'
                    });

                modalInstance.result.then(function (accNumber) {
                    saveACCtoCart(accNumber);
                }, function () {

                });
            };

            $scope.removePayment = function (card) {
                var index = $scope.paymentData.cards.indexOf(card);
                if (index != -1) {
                    swal({
                        title: "Remove Payment",
                        text: "Do you want to remove this payment?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Yes",
                        closeOnConfirm: false
                    },
                    function (isConfirm) {
                        swal.close();
                        if (isConfirm) {
                            $scope.$apply(function () {
                                if (card.cardType != 'GIFTCARD') {
                                    //assumes only 1 credit card / PLCC can be on order
                                    $scope.paymentData.inContactPaymentId = null;
                                }
                                $scope.paymentData.cards.splice(index, 1);
                            });
                        }
                    });
                }

            };

            $scope.showOverrideButton = function (orderLine, index) {
                //Need to check the orderLine.Extn._ExtnIsPriceLocked property because gift cards always have UnitPrice and RetailPrice equal to one another
                return $scope.priceChangeArr[index] || orderLine.Extn._ExtnIsPriceLocked == 'Y';
            };

            $scope.canAddPayment = function () {
                return $payment.getAvailablePaymentTypes($scope.paymentData).length > 0;
            };

            $scope.canAddGiftcardPayment = function () {
                //cannot use giftcards on big ticket items  && there are not already more than the allotted number of giftcards placed on the order
                return !$orderCart.order.hasBigTicketItems($scope.cart) && $payment.canAddGiftcard($scope.paymentData);
            };

            $scope.canAddCreditCardOrPlccPayment = function () {
                return $payment.canAddCreditCardOrPlcc($scope.paymentData);
            };


            reprice();
            //Check if a tender swiped during customer search. 
            if ($msrService.accCard && $msrService.accCard.cardType == 'AssociateDiscountCard') {
                if ($msrService.accCard.msrData.AccountNumber) {
                    if ($scope.numberOfCreditCards() > 0) {
                        swal({ title: "Alert!", text: $alertMessages.paymentMessages.ADC_NOT_ALLOWED_WITH_CC, showConfirmButton: true });
                    } else {
                        saveACCtoCart($msrService.accCard.msrData.AccountNumber);
                    }
                    $msrService.accCard = undefined;
                }
            }

            if ($msrService.lastCard && $msrService.lastCard != 'AssociateDiscountCard') {
                $scope.openPaymentEntry();
            }

        }])
    .controller('paymentEntryCtrl', ['$scope', '$modalInstance', '$modal', 'payment', 'billTo', 'paymentData', 'numberKeyPressValidator', 'msrService', '$rootScope', 'loggerService', 'alertMessages', '$window', '$timeout', 'POSService', '$http', 'orderLine', 'securityService',
function ($scope, $modalInstance, $modal, $payment, billTo, paymentData, $numberKeyPressValidator, $msrService, $rootScope, $loggerService, $alertMessages, $window, $timeout, $POSService, $http, orderLine, $securityService)
{

    $scope.isExpError = false;
    $scope.isCsr = $securityService.isCsr();

    //Fixed Issue 462 
    $scope.giftCardRemoved = false;
    var NubridgeTokenFlag = 'T';
    $scope.paymentTypes = $payment.getAvailablePaymentTypes(paymentData);
    checkForBigTicket();
    $scope.cardTypes = $payment.cardTypes;
    var msrEvent = $rootScope.$on('MSR_Event', function (event, msrData) {
        setCCData(msrData);
    });

    var msrErrorEvent = $rootScope.$on('MSR_Event_Error', function (event, msrData) {

        if ((/^(MSR_ERR_ENABLE|MSR_ERR_TOKEN|MSR_ERR_STAND_IN)$/i).test(msrData.StatusCode)) {
            //accept as Vantiv is down
            $scope.$apply(function () { $scope.resetPaymentToBlank(); });
            swal({ title: $alertMessages.paymentMessages.BANK_CARD_SYSTEM_OFFLINE_TITLE, text: $alertMessages.paymentMessages.BANK_CARD_SYSTEM_OFFLINE_MESSAGE, showConfirmButton: true });

        } else if ((/^MSR_ERR_DECLINE$/i).test(msrData.StatusCode) || (/.*ResponseCode_DeclineHard.*/i).test(msrData.StatusText)) {
            $scope.$apply(function () { $scope.resetPaymentToBlank(); });
            swal({ title: "Card Error", text: msrData.StatusText, showConfirmButton: true });

        } else if ((/^MSR_ERR_NOT_ENABLED$/i).test(msrData.StatusCode) || (/MSR is not enabled/i).test(msrData.StatusText)) {
            //ignore

        } else if ((/^(MSR_ERR_NOT_REQUESTED|MSR_ERR_INVALID_TYPE)$/i).test(msrData.StatusCode) || (/Card swiped does not match requested tender type/i).test(msrData.StatusText)) {
            $scope.$apply(function () { $scope.swipeCard() });
            swal({ title: "Card Error", text: $alertMessages.paymentMessages.INVALID_CARD_TYPE, showConfirmButton: true });

        } else if ((/^MSR_ERR_UNKNOWN_TYPE$/i).test(msrData.StatusCode)) {
            $scope.$apply(function () { $scope.swipeCard() });
            swal({ title: "Card Error", text: $alertMessages.paymentMessages.UNKNOWN_CARD_TYPE, showConfirmButton: true });

        }
        else {
            swal({ title: "MSR Error", text: msrData.StatusText, showConfirmButton: true });
            $scope.$apply(function () { $scope.close() });
        }
    });

    $scope.$on("$destroy", function () {
        msrEvent();
        msrErrorEvent();
        deregister();
        tempPLCCErrorEventDeregister();
    });

    $scope.paymentTypeChange = function () {
        var paymentType = $scope.ccData.paymentType;
        $scope.ccData = {};
        $scope.ccData.paymentType = paymentType;
        $scope.ccDisabled = false;

        $timeout(function () {
            var element = $window.document.getElementById('cardNumInput');
            if (element) {
                element.focus();
            }
        });
    };

    var clearFields = function () {
        $scope.paymentTypeChange();
    }

    function checkForBigTicket()
    {
        for (var i = 0; i < orderLine.length; i++) {
            if (orderLine[i].btDisplay.itemtype == "BGT") {

                //find giftcard payment and remove it
                for (var index = 0; index < $scope.paymentTypes.length; index++)
                {
                    if ($scope.paymentTypes[index].contractType === 'GIFTCARD')
                    {
                        $scope.paymentTypes.splice(index, 1);
                        break;
                    }
                }

                //Fix for Issue 462
                $scope.giftCardRemoved = true;
                break;
            }
        }

    }

    function setCCData(msrData) {
        $scope.ccData.cardNumber = msrData.AccountNumber;
        if (msrData.CardType == 'PLCC') {
            $scope.ccData.paymentType = $payment.paymentTypes.PLCC;
        }

        if (msrData.FirstName && msrData.FirstName.trim().length > 0) {
            $scope.ccData.FirstName = msrData.FirstName.trim();
        }
        if (msrData.LastName && msrData.LastName.trim().length > 0) {
            $scope.ccData.LastName = msrData.LastName.trim();
        }

        var firstName = $scope.ccData.FirstName ? $scope.ccData.FirstName : '';
        var lastName = $scope.ccData.LastName ? $scope.ccData.LastName : '';
        $scope.ccData.name = firstName + ' ' + lastName;

        if (msrData.ExpirationDate) {
            $scope.ccData.exp = msrData.ExpirationDate;
        }

        $scope.ccData.swipped = true;
        $scope.tokenize();
        $scope.swipping = false;
    }

    var deregister = $rootScope.$on('Scanner_Event', function (event, barcodeData, scannerMessage) {
        if ( scannerMessage.BarcodeType == 3 )
        {
            //Fixed Issue 462
            if ( !$scope.giftCardRemoved)
            {
                $scope.ccData.paymentType = $payment.paymentTypes.GIFTCARD;
                $scope.ccData.cardNumber = barcodeData;
                $scope.ccData.scanned = true;
                $scope.tokenize();
            }
            

        } else if (scannerMessage.BarcodeType == 17 || scannerMessage.BarcodeType == 6) {
            if (barcodeData && barcodeData.trim().length > 0) {
                var url = serviceURL + '/CreditService/Lookup';
                var findCustomerObj = {
                    "LookupRequest": {
                        "_AccountNumber": barcodeData,
                        "_OriginStoreNum": $POSService.getPOSParameters().storeNumber,
                        "_SourceApplication": "Sterling",
                        "_IsTokenized": "N"
                    }
                };

                $http.post(url, findCustomerObj).success(function (result) {
                    setCCData({ CardType: $payment.paymentTypes.PLCC.msrType, AccountNumber: barcodeData, FirstName: result.LookupResponse._FirstName, LastName: result.LookupResponse._LastName });
                }).error(function (data) {
                    $loggerService.error(data);
                });
            } else {
                swal({ title: "Bad PLCC Scan", text: $alertMessages.paymentMessages.SCAN_ERROR, showConfirmButton: true });
            }
        } else {
            swal({ title: "Card Error", text: $alertMessages.paymentMessages.INVALID_CARD_TYPE, showConfirmButton: true });
        }

        
    });

    var tempPLCCErrorEventDeregister = $rootScope.$on('Scanner_Event_Error_Temp_PLCC_Error', function (event, scannerMessage) {

        if((/^SCN_ERR_INVALID_BARCODE$/i).test(scannerMessage.StatusCode))
        {
            swal({ title: "Temporary Card Error", text: $alertMessages.paymentMessages.INVALID_TEMP_PLCC_BARCODE, showConfirmButton: true });

        } else if ((/^SCN_ERR_EXPIRED$/i).test(scannerMessage.StatusCode)) {
            swal({ title: "Temporary Card Error", text: $alertMessages.paymentMessages.INVALID_TEMP_PLCC_EXPIRED, showConfirmButton: true });

        } else if ((/^SCN_ERR_INVALID_STORE$/i).test(scannerMessage.StatusCode)) {
            swal({ title: "Temporary Card Error", text: $alertMessages.paymentMessages.INVALID_TEMP_PLCC_INVALID_STORE, showConfirmButton: true });
        }
    });

    $scope.disableSwipeCard = function () {
        $msrService.disableMSR($scope.ccData.paymentType.msrType);
        $scope.swipping = false;
    }

    $scope.swipeCard = function () {
        $msrService.enableMSR($scope.ccData.paymentType.msrType);
        $scope.swipping = true;
    }

    $scope.close = function () {
        $scope.disableSwipeCard();

        $modalInstance.dismiss('closed');
    };

    $scope.numberValidator = $numberKeyPressValidator;

    $scope.ccData = {};
    $scope.ccData.paymentType = $scope.paymentTypes[0];
    $scope.ccData.billTo = billTo;
    $scope.ccData.gcBalance = 0;
    $scope.showExpCIDFields = function () {
        switch ($scope.ccData.paymentType) {
            case $payment.paymentTypes.CREDITCARD:
                return true;
            default:
                return false;
        }
    }
    $scope.isCheckBalanceEnabled = function () {
        switch ($scope.ccData.paymentType) {
            case $payment.paymentTypes.GIFTCARD:
                if ($scope.ccData.token && $scope.ccData.giftCardPin)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }

    $scope.showNameField = function () {
        switch ($scope.ccData.paymentType) {
            case $payment.paymentTypes.GIFTCARD:
                return false;
            default:
                return true;
        }
    }

    $scope.showAddress = function () {
        switch ($scope.ccData.paymentType) {
            case $payment.paymentTypes.GIFTCARD:
                return false;
            default:
                return true;
        }
    }

    $scope.giftCardPin = function () {
        switch ($scope.ccData.paymentType) {
            case $payment.paymentTypes.GIFTCARD:
                return true;
            default:
                return false;
        }
    }

    $scope.tokenize = function () {
        if ($scope.ccData.cardNumber && !$scope.ccData.token && $scope.ccData.paymentType) {
            $payment.tokenize($scope.ccData.cardNumber, $scope.ccData.paymentType.contractType,
                function (item) {
                    $scope.ccData.token = item._token;
                    $scope.ccData.AccountNumberIndicator = NubridgeTokenFlag; //added for Retalix 
                    $scope.ccData.cardNumber = $scope.ccData.cardNumber.replace(/.(?=.{4})/g, '*');
                    $scope.ccData.cardType = item._cardType;

                    //fix for JCB card
                    if ($payment.testBankCardType($scope.ccData.cardType)) {
                        $scope.ccData.cardType = $payment.translateCardType($scope.ccData.cardType);
                    }
                    //END fix for JCB card

                    if (item._cardType == 'GIFTCARD') {
                        $scope.ccData.paymentType = $payment.paymentTypes.GIFTCARD;
                    }
                    else if (item._cardType == 'PLCC') {
                        $scope.ccData.paymentType = $payment.paymentTypes.PLCC;
                    }
                    else {
                        $scope.ccData.paymentType = $payment.paymentTypes.CREDITCARD;
                    }


                    if ($scope.ccData.paymentType == $payment.paymentTypes.GIFTCARD) {
                        $scope.ccData.svcNo = $scope.ccData.cardNumber;
                    }
                    $scope.ccDisabled = true;
                    if ($scope.ccData.cardType == 'PLCC' && $scope.ccData.name) {
                        $scope.save();
                        return;
                    }
                    $scope.gcBalance();
                },
                function err(error) {
                    swal({ title: "Alert!", text: $alertMessages.paymentMessages.TOKENIZATION_ERROR, showConfirmButton: true });
                    clearFields();
                    $loggerService.log(error);
                }
            );
        }
    }

    var closeModal = function () {
        var returnData = angular.copy($scope.ccData);
        $modalInstance.close(returnData);
    }

    $scope.gcBalance = function (exit) {
        if ($scope.ccData.giftCardPin && $scope.ccData.token) {
            $payment.gcBalance($scope.ccData.token, $scope.ccData.giftCardPin,
                function (response) {
                    if (response.EGCBalanceResp._MasterResultCode == "0" && response.EGCBalanceResp.GCList) {
                        if (response.EGCBalanceResp.GCList.Item._DetailBalanceResult == "Unsuccessful") {
                            swal({ title: $alertMessages.paymentMessages.GC_PIN_ERROR, text: response.EGCBalanceResp.GCList.Item._DetailBalanceResultReason, showConfirmButton: true });
                        } else {
                            $scope.ccData.gcBalance = Number(response.EGCBalanceResp.GCList.Item._Balance);
                            if (exit) {
                                closeModal();
                            }
                        }
                    } else if (response.EGCBalanceResp._MasterResultCode != "0") {
                        swal({ title: $alertMessages.paymentMessages.GC_ALERT, text: response.EGCBalanceResp._MasterResultText, showConfirmButton: true });
                    }

                },
                function err(error) {
                    swal({ title: $alertMessages.paymentMessages.GC_ALERT, text: $alertMessages.paymentMessages.GC_BALANCE_ERROR, showConfirmButton: true });
                    clearFields();
                    $loggerService.log(error);
                }
            );
        } else if (!$scope.ccData.token) {
            $scope.tokenize();
        }
    }


    $scope.ccClicked = function () {
        if ($scope.ccDisabled) {
            clearFields();
        }
    }

    $scope.canSave = function () {
        switch ($scope.ccData.paymentType) {
            case $payment.paymentTypes.CREDITCARD:
                if ($scope.ccData.token && $scope.ccData.name && $scope.ccData.exp && $scope.ccData.cid)
                    return true;
                else
                    return false;
            case $payment.paymentTypes.GIFTCARD:
                if ($scope.ccData.token && $scope.ccData.giftCardPin)
                    return true
                else
                    return false;
            case $payment.paymentTypes.PLCC:
                if ($scope.ccData.token && $scope.ccData.name)
                    return true;
                else
                    return false;
            default:
                return false;
        }
    }

    $scope.save = function () {
        if ($scope.ccData.giftCardPin && $scope.ccData.token) {
            $scope.gcBalance(true);
        } else {

            if (($scope.ccData.paymentType === $payment.paymentTypes.CREDITCARD) && $scope.ccData.exp && isInvalidExpDate($scope.ccData.exp)) {
                $scope.isExpError = true;
            } else {
                $scope.isExpError = false;
                closeModal();
            }
        }
    };

    var isInvalidExpDate = function (date) {
        if (date.length !== 4) {
            return true;
        } else {
            var month = date.substring(0, 2);
            if (Number(month) > 12) {
                return true;
            }
        }

        return false;
    }

    //   repeated from line 843
    //            $scope.close = function () {
    //                $modalInstance.dismiss('close');
    //            };



    if ($msrService.lastCard && $msrService.lastCard != 'AssociateDiscountCard') {
        $scope.ccData = {};
        $scope.ccData.paymentType = $payment.paymentTypes.CREDITCARD;
        setCCData($msrService.lastCard.msrData);
        $msrService.lastCard = undefined;

    }
}
    ])
    .controller('paymentEntryRetalixCtrl', ['$scope', '$modalInstance', '$modal', 'payment', 'billTo', 'paymentData', 'numberKeyPressValidator', 'msrService', '$rootScope', 'loggerService', 'alertMessages', '$window', '$timeout', 'POSService', '$http', 'orderLine', 'sendSMTPErrorEmail', 'securityService',
        function ($scope, $modalInstance, $modal, $payment, billTo, paymentData, $numberKeyPressValidator, $msrService, $rootScope, $loggerService, $alertMessages, $window, $timeout, $POSService, $http, orderLine, $sendSMTPErrorEmail, $securityService)
        {

            var pmmVantivFlagIndicatorRegEx = /^(N|V)$/i;

            var backendVantivFlagIndicator = 'V';
            var backendVantivFlagIndicatorRegEx = new RegExp('^' + backendVantivFlagIndicator + '$', 'i');


            var NubridgeTokenFlag = 'T';
            $scope.isExpError = false;

            $scope.isCsr = $securityService.isCsr();
            //if this is a CSR then only allow giftcards
            $scope.paymentTypes = $payment.getAvailablePaymentTypes(paymentData, $scope.isCsr);
 

            //Fixed Issue 462 
            $scope.giftCardRemoved = false;
            checkForBigTicket();
            $scope.cardTypes = $payment.cardTypes;

            var msrEvent = $rootScope.$on('MSR_Event', function (event, msrData) {
                //TODO: remove -- Used for testing
                //var textArr = [];
                //angular.forEach(msrData, function (value, key) {
                //    textArr.push('' + key + ': ' + value);
                //});
                //swal({ title: "Msr DATA", text: textArr.join(', '), showConfirmButton: true });
                setCCData(msrData);
            });

            var msrErrorEvent = $rootScope.$on('MSR_Event_Error', function (event, msrData) {

                if ((/^(MSR_ERR_ENABLE|MSR_ERR_TOKEN|MSR_ERR_STAND_IN)$/i).test(msrData.StatusCode)) {
                    //accept as Vantiv is down
                     $scope.resetPaymentToBlank(); 
                    swal({ title: $alertMessages.paymentMessages.BANK_CARD_SYSTEM_OFFLINE_TITLE, text: $alertMessages.paymentMessages.BANK_CARD_SYSTEM_OFFLINE_MESSAGE, showConfirmButton: true });

                } else if ((/^MSR_ERR_NOT_FUNCTIONAL$/i).test(msrData.StatusCode)) {
                     $scope.resetPaymentToBlank(); 
                    swal({ title: "MSR Error", text: $alertMessages.paymentMessages.MSR_NOT_FUNCTIONAL, showConfirmButton: true });

                } else if ((/^MSR_ERR_DECLINE$/i).test(msrData.StatusCode) || (/.*ResponseCode_DeclineHard.*/i).test(msrData.StatusText)) {
                     $scope.resetPaymentToBlank(); 
                    swal({ title: "Card Error", text: msrData.StatusText, showConfirmButton: true });

                } else if ((/^MSR_ERR_NOT_ENABLED$/i).test(msrData.StatusCode) || (/MSR is not enabled/i).test(msrData.StatusText)) {
                    //ignore

                } else if ((/^(MSR_ERR_NOT_REQUESTED|MSR_ERR_INVALID_TYPE)$/i).test(msrData.StatusCode) || (/Card swiped does not match requested tender type/i).test(msrData.StatusText)) {
                    $scope.swipeCard();
                    swal({ title: "Card Error", text: $alertMessages.paymentMessages.INVALID_CARD_TYPE, showConfirmButton: true });

                } else if ((/^MSR_ERR_UNKNOWN_TYPE$/i).test(msrData.StatusCode)) {
                     $scope.swipeCard();
                    swal({ title: "Card Error", text: $alertMessages.paymentMessages.UNKNOWN_CARD_TYPE, showConfirmButton: true });

                } else if ((/^(MSR_ERR_CUSTOMER_CANCEL|MSR_ERR_BANK_CUST_CANCEL|MSR_ERR_PLCC_CUST_CANCEL)$/i).test(msrData.StatusCode) || (/The card swipe was cancelled by the associate/i).test(msrData.StatusText)) {
                    //just fail for now.
                     $scope.close();
                    swal({ title: "MSR Error", text: msrData.StatusText, showConfirmButton: true });

                } else if ((/^(MSR_ERR_DISABLE|MSR_ERR_ENABLED)$/i).test(msrData.StatusCode) || (/Cannot disable MSR.*Card swipe has already occurred/i).test(msrData.StatusText)) {
                    //vativ manual entry, we try to disable while it is still voiding the penny auth
                    //ignore message
                }
                else {
                    $scope.close();
                    swal({ title: "MSR Error", text: msrData.StatusText, showConfirmButton: true });
                    //email error
                    $sendSMTPErrorEmail(msrData, 'PMM Payment Entry Error on ENV: ' + serviceURL);
                }
            });



            $scope.$on("$destroy", function () {
                msrEvent();
                msrErrorEvent();
                deregister();
                tempPLCCErrorEventDeregister();
            });

            function checkForBigTicket() {
                for (var i = 0; i < orderLine.length; i++) {
                    if (orderLine[i].btDisplay.itemtype == "BGT") {

                        //find giftcard payment and remove it
                        for (var index = 0; index < $scope.paymentTypes.length; index++) {
                            if ($scope.paymentTypes[index].contractType === 'GIFTCARD') {
                                $scope.paymentTypes.splice(index, 1);
                                break;
                            }
                        }

                        //Fix for Issue 462
                        $scope.giftCardRemoved = true;
                        break;
                    }
                }

            }

            $scope.resetPaymentToBlank = function () {
                $scope.ccData.paymentType = '';
                $scope.paymentTypeChange();
            };

            $scope.paymentTypeChange = function () {

                var paymentType = $scope.ccData.paymentType;
                $scope.ccData = {};
                $scope.ccData.paymentType = paymentType;
                $scope.ccDisabled = false;

                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.CREDITCARD:
                    case $payment.paymentTypes.GIFTCARD:
                    case $payment.paymentTypes.PLCC:
                        if (!$scope.isCsr) {
                        $scope.swipeCard();
                        }
                        break;
                    default:
                        //else do not start swipping. Do not enable MSR
                        $scope.disableSwipeCard();
                }

            };

            var clearFields = function () {
                $scope.paymentTypeChange();

            }

            function setCCData(msrData) {

                //if Vantiv tokent
                if (pmmVantivFlagIndicatorRegEx.test(msrData.AccountNumberIndicator)) {
                    $scope.ccData.cardNumber = '************' + msrData.AccountNumberLast4;
                    $scope.ccData.token = msrData.AccountNumber;
                    msrData.AccountNumberIndicator = backendVantivFlagIndicator; //translate 'N' to 'V' for pmm Vantiv indicator to backend's vantiv indicator

                    if ($payment.testBankCardType(msrData.CardType)) {
                        $scope.ccData.cardType = $payment.translateCardType(msrData.CardType);
                    }

                } else {
                    //clear text
                    $scope.ccData.cardNumber = msrData.AccountNumber;
                }

                if (msrData.AccountNumberIndicator) {
                    $scope.ccData.AccountNumberIndicator = msrData.AccountNumberIndicator;
                } else {
                    $scope.ccData.AccountNumberIndicator = 'C'; //default to clear text account
                }

                if (msrData.CardType == 'PLCC') {
                    $scope.ccData.paymentType = $payment.paymentTypes.PLCC;
                    $scope.ccData.cardType = $payment.paymentTypes.PLCC.msrType;
                }

                if (msrData.FirstName && msrData.FirstName.trim().length > 0) {
                    $scope.ccData.FirstName = msrData.FirstName.trim();
                }
                if (msrData.LastName && msrData.LastName.trim().length > 0) {
                    $scope.ccData.LastName = msrData.LastName.trim();
                }
                //add middle initial?
                var firstName = $scope.ccData.FirstName ? $scope.ccData.FirstName : '';
                var lastName = $scope.ccData.LastName ? $scope.ccData.LastName : '';
                $scope.ccData.name = firstName + ' ' + lastName;
                $scope.ccData.name = $scope.ccData.name.trim();

                if (msrData.ExpirationDate) {
                    $scope.ccData.exp = msrData.ExpirationDate;
                }

                //TODO: find out how CID is passed
                //                if (msrData.CVV) {
                //                    $scope.ccData.cid = msrData.CVV;
                //                }

                $scope.ccData.swipped = true;
                $scope.tokenize();
            };

            var deregister = $rootScope.$on('Scanner_Event', function (event, barcodeData, scannerMessage) {
                if (scannerMessage.BarcodeType == 3) {

                    //Fixed Issue 462
                    if ( !$scope.giftCardRemoved)
                    {
                        $scope.ccData.paymentType = $payment.paymentTypes.GIFTCARD;
                        $scope.ccData.cardNumber = barcodeData;
                        $scope.ccData.scanned = true;
                        $scope.tokenize();
                    }
                } else if (scannerMessage.BarcodeType == 17 || scannerMessage.BarcodeType == 6) {
                    if (barcodeData && barcodeData.trim().length > 0) {
                        var url = serviceURL + '/CreditService/Lookup';
                        var findCustomerObj = {
                            "LookupRequest": {
                                "_AccountNumber": barcodeData,
                                "_OriginStoreNum": $POSService.getPOSParameters().storeNumber,
                                "_SourceApplication": "Sterling",
                                "_IsTokenized": "N"
                            }
                        };

                        $http.post(url, findCustomerObj).success(function (result) {
                            setCCData({ CardType: $payment.paymentTypes.PLCC.msrType, AccountNumber: barcodeData, FirstName: result.LookupResponse._FirstName, LastName: result.LookupResponse._LastName });
                        }).error(function (data) {
                            $loggerService.error(data);
                        });
                    } else {
                        swal({ title: "Bad PLCC Scan", text: $alertMessages.paymentMessages.SCAN_ERROR, showConfirmButton: true });
                    }
                } else {
                    swal({ title: "Card Error", text: $alertMessages.paymentMessages.INVALID_CARD_TYPE, showConfirmButton: true });
                }
            });

            var tempPLCCErrorEventDeregister = $rootScope.$on('Scanner_Event_Error_Temp_PLCC_Error', function (event, scannerMessage) {

                if ((/^SCN_ERR_INVALID_BARCODE$/i).test(scannerMessage.StatusCode)) {
                    swal({ title: "Temporary Card Error", text: $alertMessages.paymentMessages.INVALID_TEMP_PLCC_BARCODE, showConfirmButton: true });

                } else if ((/^SCN_ERR_EXPIRED$/i).test(scannerMessage.StatusCode)) {
                    swal({ title: "Temporary Card Error", text: $alertMessages.paymentMessages.INVALID_TEMP_PLCC_EXPIRED, showConfirmButton: true });

                } else if ((/^SCN_ERR_INVALID_STORE$/i).test(scannerMessage.StatusCode)) {
                    swal({ title: "Temporary Card Error", text: $alertMessages.paymentMessages.INVALID_TEMP_PLCC_INVALID_STORE, showConfirmButton: true });
                }
            });

            var waitingForMsr = {
                DISABLE: 0,
                ENABLE: 1,
                wait: function (waitType) {
                    if (waitType === waitingForMsr.ENABLE) {
                        $scope.msrWaitingMessage = "Enabling...";
                        $scope.msrWaiting = true;
                        $scope.swipping = true;

                    } else if (waitType === waitingForMsr.DISABLE) {
                        $scope.swipping = true;
                        $scope.msrWaitingMessage = "Disabling...";
                        $scope.msrWaiting = true;
                    }
                },
                doneWaiting: function (waitType) {
                    if (waitType === waitingForMsr.ENABLE) {
                        $scope.swipping = true;
                        $scope.msrWaiting = false;
                        $scope.msrWaitingMessage = " ";
                    } else if (waitType === waitingForMsr.DISABLE) {
                        $scope.swipping = false;
                        $scope.msrWaitingMessage = " ";
                        $scope.msrWaiting = false;
                    }
                }
            };



            $scope.disableSwipeCard = function (isIgnoreStateSendDisable) {
                if ($msrService.isEnabled() || isIgnoreStateSendDisable) {
                    waitingForMsr.wait(waitingForMsr.DISABLE);
                    $msrService.disableMSR(isIgnoreStateSendDisable).then(
                        function () { waitingForMsr.doneWaiting(waitingForMsr.DISABLE); },
                        function () { waitingForMsr.doneWaiting(waitingForMsr.DISABLE); });
                } else {
                    waitingForMsr.doneWaiting(waitingForMsr.DISABLE);
                }
            }

            $scope.swipeCard = function () {
                waitingForMsr.wait(waitingForMsr.ENABLE);
                $msrService.disableMSR().then(function () {
                    $msrService.enableMSR($scope.ccData.paymentType.msrType).then(
                        function () { waitingForMsr.doneWaiting(waitingForMsr.ENABLE); },
                        function () { waitingForMsr.doneWaiting(waitingForMsr.DISABLE); }
                    );
                },
                function (error) {
                    var message = (angular.isObject(error) && angular.isString(error.StatusText)) ? error.StatusText :"Disable MSR failed.";
                    swal({ title: "MSR Disable Error", text: message, showConfirmButton: true });
                    waitingForMsr.doneWaiting(waitingForMsr.DISABLE);
                }
                );
            }

            $scope.close = function () {
                $scope.disableSwipeCard( true );
                $modalInstance.dismiss('closed');
            };


            $scope.numberValidator = $numberKeyPressValidator;

            $scope.ccData = {};
            $scope.ccData.paymentType = '';//$scope.paymentTypes[0];
            $scope.ccData.billTo = billTo;
            $scope.ccData.gcBalance = 0;
            $scope.showCIDFields = function () {
                return false;

                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.CREDITCARD:
                        return true;
                    default:
                        return false;
                }
            };
            $scope.isCheckBalanceEnabled = function () {
                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.GIFTCARD:
                        if ($scope.ccData.token && $scope.ccData.giftCardPin)
                            return true;
                        else
                            return false;
                    default:
                        return false;
                }
            };

            $scope.showNameField = function () {
                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.PLCC:
                    case $payment.paymentTypes.CREDITCARD:
                        return true;
                    default:
                        return false;
                }
            };

            $scope.showCardNumberField = function () {
                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.PLCC:
                    case $payment.paymentTypes.CREDITCARD:
                    case $payment.paymentTypes.GIFTCARD:
                        return true;
                    default:
                        return false;
                }
            };

            $scope.showAddress = function () {
                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.GIFTCARD:
                        return false;
                    default:
                        return false;
                }
            };

            $scope.giftCardPin = function () {
                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.GIFTCARD:
                        return true;
                    default:
                        return false;
                }
            };

            //show during swiping of Giftcard Only
            $scope.showKeyInButton = function () {
                if ($scope.swipping && $scope.ccData.paymentType == $payment.paymentTypes.GIFTCARD) {
                    return true;
                } else {
                    return false;
                }
            };

            $scope.showSwipeCardButton = function () {

                if ($scope.isCsr) {
                    return false;
                }

                if (!$scope.swipping && $scope.ccData.paymentType == $payment.paymentTypes.GIFTCARD) {
                    return true;
                } else {
                    return false;
                }
            };

            $scope.showGiftcardManualEntry = function () {
                $scope.disableSwipeCard(true);
            };


            $scope.tokenize = function () {
                if ($scope.ccData.cardNumber && !$scope.ccData.token && $scope.ccData.paymentType) {
                    $payment.tokenize($scope.ccData.cardNumber, $scope.ccData.paymentType.contractType,
                        function (item) {
                            $scope.ccData.token = item._token;
                            $scope.ccData.AccountNumberIndicator = NubridgeTokenFlag;
                            $scope.ccData.cardNumber = $scope.ccData.cardNumber.replace(/.(?=.{4})/g, '*');
                            $scope.ccData.cardType = item._cardType;

                            //fix for JCB card
                            if ($payment.testBankCardType($scope.ccData.cardType)) {
                                $scope.ccData.cardType = $payment.translateCardType($scope.ccData.cardType);
                            }
                            //END fix for JCB card

                            if (item._cardType == 'GIFTCARD') {
                                $scope.ccData.paymentType = $payment.paymentTypes.GIFTCARD;
                            }
                            else if (item._cardType == 'PLCC') {
                                $scope.ccData.paymentType = $payment.paymentTypes.PLCC;
                            }
                            else {
                                $scope.ccData.paymentType = $payment.paymentTypes.CREDITCARD;
                            }


                            if ($scope.ccData.paymentType == $payment.paymentTypes.GIFTCARD) {
                                $scope.ccData.svcNo = $scope.ccData.cardNumber;
                            }
                            $scope.ccDisabled = true;
                            if ($scope.ccData.cardType == 'PLCC' && $scope.ccData.name) {
                                $scope.save();
                                return;
                            }
                            $scope.gcBalance();

                            $scope.disableSwipeCard();
                        },
                        function err(error) {
                            swal({ title: "Alert!", text: $alertMessages.paymentMessages.TOKENIZATION_ERROR, showConfirmButton: true });
                            clearFields();
                            $loggerService.log(error);
                        }
                    );
                } else if ($scope.ccData.cardNumber && $scope.ccData.token && $scope.ccData.paymentType && backendVantivFlagIndicatorRegEx.test($scope.ccData.AccountNumberIndicator)) {
                    $scope.ccDisabled = true;
                    //vantiv
                    $scope.save();
                }
            }

            var closeModal = function () {

                //TODO: remove -- Used for testing
                //var textArr = [];
                //angular.forEach($scope.ccData, function (value, key) {
                //    textArr.push('' + key + ': ' + value);
                //});
                // swal({ title: "Close Modal Return", text: textArr.join(', '), showConfirmButton: true });



                var returnData = angular.copy($scope.ccData);
                $modalInstance.close(returnData);
            }

            $scope.gcBalance = function (exit) {
                if ($scope.ccData.giftCardPin && $scope.ccData.token) {
                    $payment.gcBalance($scope.ccData.token, $scope.ccData.giftCardPin,
                        function (response) {
                            if (response.EGCBalanceResp._MasterResultCode == "0" && response.EGCBalanceResp.GCList) {
                                if (response.EGCBalanceResp.GCList.Item._DetailBalanceResult == "Unsuccessful") {
                                    swal({ title: $alertMessages.paymentMessages.GC_PIN_ERROR, text: response.EGCBalanceResp.GCList.Item._DetailBalanceResultReason, showConfirmButton: true });
                                } else {
                                    $scope.ccData.gcBalance = Number(response.EGCBalanceResp.GCList.Item._Balance);
                                    if (exit) {
                                        closeModal();
                                    }
                                }
                            } else if (response.EGCBalanceResp._MasterResultCode != "0") {
                                swal({ title: $alertMessages.paymentMessages.GC_ALERT, text: response.EGCBalanceResp._MasterResultText, showConfirmButton: true });
                            }

                        },
                        function err(error) {
                            swal({ title: $alertMessages.paymentMessages.GC_ALERT, text: $alertMessages.paymentMessages.GC_BALANCE_ERROR, showConfirmButton: true });
                            clearFields();
                            $loggerService.log(error);
                        }
                    );
                } else if (!$scope.ccData.token) {
                    $scope.tokenize();
                }
            }


            $scope.ccClicked = function () {
                if ($scope.ccDisabled) {
                    clearFields();
                }
            }

            $scope.canSave = function () {
                switch ($scope.ccData.paymentType) {
                    case $payment.paymentTypes.CREDITCARD:
                        if ($scope.ccData.token && $scope.ccData.name && $scope.ccData.exp) 
                            return true;
                        else
                            return false;
                    case $payment.paymentTypes.GIFTCARD:
                        if ($scope.ccData.token && $scope.ccData.giftCardPin)
                            return true
                        else
                            return false;
                    case $payment.paymentTypes.PLCC:
                        if ($scope.ccData.token && $scope.ccData.name)
                            return true;
                        else
                            return false;
                    default:
                        return false;
                }
            }

            $scope.save = function () {
                //TODO: remove -- Used for testing
                // swal({ title: "Save Func", text: angular.toJson($scope.ccData), showConfirmButton: true });


                if ($scope.ccData.giftCardPin && $scope.ccData.token) {
                    $scope.gcBalance(true);
                } else {

                    if (($scope.ccData.paymentType === $payment.paymentTypes.CREDITCARD) && $scope.ccData.exp && isInvalidExpDate($scope.ccData.exp)) {
                        $scope.isExpError = true;

                    } else {
                        $scope.isExpError = false;
                    }

                    if (!$scope.isExpError && $scope.ccData.paymentType && ($scope.ccData.paymentType === $payment.paymentTypes.PLCC) &&
                        angular.isString($scope.ccData.name) && $scope.ccData.name.length > 0) {
                        closeModal();
                    } else {
                        $scope.disableSwipeCard(); //show input boxes
                    }
                    if (!$scope.isExpError && $scope.ccData.paymentType && ($scope.ccData.paymentType === $payment.paymentTypes.CREDITCARD) &&
                        angular.isString($scope.ccData.name) && $scope.ccData.name.length > 0) {//&& ($scope.ccData.cid && $scope.ccData.cid.toString().length > 2)) {
                        closeModal();
                    } else {
                        $scope.disableSwipeCard(); //show input boxes
                    }

                }
            };

            var isInvalidExpDate = function (date) {
                if (date.length !== 4) {
                    return true;
                } else {
                    var month = date.substring(0, 2);
                    if (Number(month) > 12) {
                        return true;
                    }
                }

                return false;
            };

            var delayedLastCard = function () {
                if ($msrService.lastCard && $msrService.lastCard != 'AssociateDiscountCard') {
                    $scope.ccData = {};
                    $scope.ccData.paymentType = $payment.paymentTypes.CREDITCARD;

                    //TODO: remove debug only
                    //var report = angular.copy($msrService.lastCard.msrData);
                    //$sendSMTPErrorEmail(angular.toJson(report), 'LastCard Debug: ' + serviceURL);

                    setCCData($msrService.lastCard.msrData);
                    $msrService.lastCard = undefined;
                }
            };

            $timeout(delayedLastCard, 1000);

            if ($scope.isCsr) {
                $scope.ccData.paymentType = $payment.paymentTypes.GIFTCARD;
                $scope.showGiftcardManualEntry();
                $scope.swipping = false;
        }

        }
    ])
     .controller('InContactPaymentEntryCtrl', ['$scope', '$modalInstance', 'payment', 'billTo', 'paymentData', 'sendSMTPErrorEmail',
        function ($scope, $modalInstance, $payment, billTo, paymentData, $sendSMTPErrorEmail) {


            $scope.inContactPaymentId = paymentData.inContactPaymentId;
            $scope.paymentErrors = "";

            $scope.ccData = {};
            $scope.ccData.paymentType = '';
            $scope.ccData.billTo = billTo;
            $scope.ccData.gcBalance = 0;

            if ($scope.inContactPaymentId === null || $scope.inContactPaymentId === undefined) {
                //get inContact Payment ID
                $payment.getInContactPaymentId().then(function (response) {
                    $scope.inContactPaymentId = response.data.PCRN;
                },
                function (response) {
                    reportFailure("Payment ID generation failed.");
                }
                );
            }

            var reportFailure = function (displayMessage) {
                if (displayMessage.toString()) {
                    $scope.paymentErrors = displayMessage;
                } else {
                    $scope.paymentErrors = "Payment collection process failed.";
                }
            };

            $scope.getPayment = function () {
              
                $payment.getInContactPayment($scope.inContactPaymentId).then(function (response) {      

                    //blank response means no data was found for that PCRN  (payment id)
                    // test for empty
                    var isEmptyJSON = function (obj) {
                        return JSON.stringify(obj) === JSON.stringify({});
                    };

                    if (angular.isObject(response.data) && isEmptyJSON(response.data)) {
                        reportFailure("No payment with ID " + $scope.inContactPaymentId + " was found. Please try again.");
                    }
                    else if (response.data && angular.isObject(response.data.Tender) && angular.isString(response.data.Tender.TenderType) && (response.data.Tender.Token !== null && response.data.Tender.Token !== undefined)
                        && angular.isString(response.data.Tender.TokenType) && angular.isString(response.data.Tender.ExpirationDate) && angular.isString(response.data.Tender.CardType))
                    {
                        var tenderData = response.data.Tender;

                        $scope.ccData.cardNumber = '************'; 
                        $scope.ccData.cardNumber += tenderData.Last4 ? tenderData.Last4 : "****";

                        $scope.ccData.token = tenderData.Token;
                        $scope.ccData.AccountNumberIndicator = tenderData.TokenType;

                        //add billTo as credit card / PLCC Name on card
                        if (billTo._FirstName && billTo._FirstName.trim().length > 0) {
                            $scope.ccData.FirstName = billTo._FirstName.trim();
                        }
                        if (billTo._LastName && billTo._LastName.trim().length > 0) {
                            $scope.ccData.LastName = billTo._LastName.trim();
                        }
                        
                        var firstName = $scope.ccData.FirstName ? $scope.ccData.FirstName : '';
                        var lastName = $scope.ccData.LastName ? $scope.ccData.LastName : '';
                        $scope.ccData.name = firstName + ' ' + lastName;
                        $scope.ccData.name = $scope.ccData.name.trim();

                        //reformat expiration Date
                        $scope.ccData.exp = tenderData.ExpirationDate.substring(5, 7) + tenderData.ExpirationDate.substring(2, 4);

                        if ((/BankCard/i).test(tenderData.TenderType)) {
                            $scope.ccData.paymentType = $payment.paymentTypes.CREDITCARD;

                            if (tenderData.CVV && (/^\d{3,4}$/).test(tenderData.CVV.toString().trim())) {
                                $scope.ccData.cid = tenderData.CVV.toString().trim();
                            } else {
                                reportFailure("Retrieved Payment CVV is invalid.");
                                return;
                            }

                            if ($payment.testBankCardType(tenderData.CardType)) {
                                $scope.ccData.cardType = $payment.translateCardType(tenderData.CardType);
                            } else {
                                $scope.ccData.cardType = tenderData.CardType;
                            }
                        } else if ((/PLCC/i).test(tenderData.TenderType)) {
                            $scope.ccData.paymentType = $payment.paymentTypes.PLCC;
                            $scope.ccData.cardType = $payment.paymentTypes.PLCC.msrType;
                        } else {
                            reportFailure("Retrieved Payment TenderType is invalid.");
                            return;
                        }


                        //successful retrieval, so save
                        closeModal();

                    } else {
                        reportFailure("Retrieved Payment Data is invalid.");

                    }  
                }, function (response) {
                    reportFailure("InContact Payment service failed.");
                });
            };

            $scope.close = function () {
                var returnData = { inContactPaymentId: null };

                if ($scope.inContactPaymentId !== null && $scope.inContactPaymentId !== null && $scope.inContactPaymentId.toString().trim().length > 0) {
                    
                    returnData.inContactPaymentId = $scope.inContactPaymentId.toString().trim();
                }

                $modalInstance.dismiss(returnData);
            };   

            var closeModal = function () {

                var returnData = angular.copy($scope.ccData);

                returnData.inContactPaymentId = null;

                if ($scope.inContactPaymentId !== null && $scope.inContactPaymentId !== null && $scope.inContactPaymentId.toString().trim().length > 0) {
                    returnData.inContactPaymentId = $scope.inContactPaymentId.toString().trim();
                }

                $modalInstance.close(returnData);
            }
        }
     ])
    .controller('accEntryCtrl', ['$scope', '$modalInstance', '$modal', 'payment', 'numberKeyPressValidator', 'msrService', '$rootScope', 'alertMessages', 'securityService',
        function ($scope, $modalInstance, $modal, $payment, $numberKeyPressValidator, $msrService, $rootScope, $alertMessages, $securityService) {


            $scope.isCsr = $securityService.isCsr();

            var msrErrorEvent = $rootScope.$on('MSR_Event_Error', function (event, msrData) {
                swal({ title: "Alert!", text: $alertMessages.paymentMessages.CARD_ERROR, showConfirmButton: true });
   
                    $scope.swipping = false;
   
            });

            var msrEvent = $rootScope.$on('MSR_Event', function (event, msrData) {
                $modalInstance.close(msrData.AccountNumber);
            });

            $scope.$on("$destroy", function () {
                msrErrorEvent();
                msrEvent();
            });


            $scope.disableSwipeCard = function () {
                $msrService.disableMSR('AssociateDiscountCard');
                $scope.swipping = false;
            }

            $scope.swipeCard = function () {
                $msrService.enableMSR('AssociateDiscountCard');
                $scope.swipping = true;
            }

            $scope.close = function () {
                $scope.disableSwipeCard();
                $modalInstance.dismiss('closed');
            };


            $scope.numberValidator = $numberKeyPressValidator;


            $scope.canSave = function () {
                if ($scope.accNumber)
                    return true;
                else
                    return false;
            }

            $scope.save = function () {
                var firstFour = $scope.accNumber.substring(0, 4);
                if (firstFour == '2119') {
                    $modalInstance.close($scope.accNumber);
                } else {
                    $scope.cardNumValidationError = true;
                }
            };

            $scope.close = function () {
                $modalInstance.dismiss('close');
            };
        }
    ]);