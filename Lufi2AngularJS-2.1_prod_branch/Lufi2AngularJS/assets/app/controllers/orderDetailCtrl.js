angular.module('orderDetail', ['ui.bootstrap'])
    .controller('orderDetailCtrl', ['$scope', 'order', '$modal', '$location', 'itemSearch', 'itemProperty', 'printerService', '$rootScope', 'ngTableParams', '$filter', '$q', '$state', 'sendSMTPErrorEmail', 'btProp',
        function ($scope, $order, $modal, $location, $itemSearch, $itemProperty, $printerService, $rootScope, ngTableParams, $filter, $q, $state, $sendSMTPErrorEmail, $btProp) {

            $scope.isLoaded = false;
            $scope.isReturnOrder = false;

            var selectedOrder = $order.getSelectedOrder();
            if (selectedOrder == undefined) {
                $location.path("/orderSearch");
            };

            var param = {
                "GetSterlingOrderDetailReq": {
                    "_OrderNo": selectedOrder._OrderNo,
                    "_EnterpriseCode": "BONTON"
                }
            };

            if (angular.isDefined(selectedOrder._DocumentType) && angular.isString(selectedOrder._DocumentType)) {
                param.GetSterlingOrderDetailReq._DocumentType = selectedOrder._DocumentType;
            }

            var deregisterPrinterErrorEvent = $rootScope.$on('Printer_Error', function (event, errorData) {
                swal({ title: "Alert!", text: errorData.StatusText, showConfirmButton: true });
            });

            $scope.$on('destroy', function () {
                deregisterPrinterErrorEvent();
            });


            $scope.printOrderSummary = function () {
                $printerService.claimPrinter();
                $printerService.printOrder(selectedOrder._OrderNo, false);
                $printerService.releasePrinter();
            }

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

            var myIScroll = null;
            $scope.refreshIScroll = function () {
                if (!myIScroll) {
                    myIScroll = new IScroll('#orderLineWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip', scrollX: true });
                }
                setTimeout(function () {
                    myIScroll.refresh();
                }, 500);
            };

            var returnsIScroll = null;
            $scope.refreshReturnsIScroll = function () {
                if (!returnsIScroll) {
                    returnsIScroll = new IScroll('#orderLineWrapperReturns', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip', scrollX: true });
                }
                setTimeout(function () {
                    returnsIScroll.refresh();
                }, 500);
            };

            $scope.selectLineItem = function (lineItem) {
                $scope.selectedLineItem = lineItem;
            };

            var _setBtLogicDetails = function () {

                if ($scope.orderDetail && $scope.orderDetail.OrderLines && $scope.orderDetail.OrderLines.OrderLine) {

                    var shipmentKepMap = {};
                    if ($scope.orderDetail.Shipments && angular.isArray($scope.orderDetail.Shipments.Shipment)) {
                        for (var p = 0; p < $scope.orderDetail.Shipments.Shipment.length; p++) {
                            if ($scope.orderDetail.Shipments.Shipment[p]._ShipmentKey && angular.isString($scope.orderDetail.Shipments.Shipment[p]._ShipmentKey) &&
                                ($scope.orderDetail.Shipments.Shipment[p]._ShipmentKey.trim().length > 0)) {
                                shipmentKepMap[$scope.orderDetail.Shipments.Shipment[p]._ShipmentKey.trim()] = $scope.orderDetail.Shipments.Shipment[p];
                            }
                        }
                    }

                    for (var i = 0; i < $scope.orderDetail.OrderLines.OrderLine.length; i++) {
                        var currentLine = $scope.orderDetail.OrderLines.OrderLine[i];

                        currentLine.btLogic.orderDetail = { isReturned: false, isGiftMessage: false, trackingNoString: '', ShipDate: '' };

                        if (currentLine.ReturnOrderLines && currentLine.ReturnOrderLines.OrderLine && (currentLine.ReturnOrderLines.OrderLine.length > 0)) {
                            currentLine.btLogic.orderDetail.isReturned = true;
                        }
                        currentLine.btLogic.orderDetail.sortableUnitPrice = parseFloat(currentLine.LinePriceInfo._UnitPrice);
                        currentLine.btLogic.orderDetail.sortableQuantity = parseFloat(currentLine._OrderedQty);
                        if (currentLine.btDisplay && currentLine.btDisplay.id) {
                            currentLine.btLogic.orderDetail.sortableUpc = parseInt(currentLine.btDisplay.id);
                        } else {
                            currentLine.btLogic.orderDetail.sortableUpc = isFinite(parseInt(currentLine.Item._UPCCode)) ? parseInt(currentLine.Item._UPCCode) : -1;
                        }

                        if (currentLine.Notes && currentLine.Notes.Note) {
                            var isGiftNotes = false;
                            for (var p = 0; p < currentLine.Notes.Note.length; p++) {
                                if ((/^GIFT_(?:MESSAGE|FROM|TO)/).test(currentLine.Notes.Note[p]._ReasonCode)) {
                                    isGiftNotes = true;
                                    break;
                                }
                            }
                            currentLine.btLogic.orderDetail.isGiftMessage = isGiftNotes;
                        }

                        //tracking numbers join
                        if (currentLine.Containers && currentLine.Containers.Container && (currentLine.Containers.Container.length > 0)) {
                            var tempStringArray = [];
                            for (var x = 0; x < currentLine.Containers.Container.length; x++) {
                                if (currentLine.Containers.Container[x]._TrackingNo) {
                                    tempStringArray.push(currentLine.Containers.Container[x]._TrackingNo.toString());
                                }
                            }

                            //sort strings to keep consistant across order lines
                            tempStringArray.sort();

                            currentLine.btLogic.orderDetail.trackingNoString = tempStringArray.join(', ');
                        }

                        //expected or actual ship date of first shipment line only
                        if (currentLine.ShipmentLines && angular.isArray(currentLine.ShipmentLines.ShipmentLine) && (currentLine.ShipmentLines.ShipmentLine.length > 0)
                            && angular.isString(currentLine.ShipmentLines.ShipmentLine[0]._ShipmentKey)) {

                            var shipment = shipmentKepMap[currentLine.ShipmentLines.ShipmentLine[0]._ShipmentKey.trim()];
                            if (shipment) {
                                if (angular.isString(shipment._ShipDate) && (shipment._ShipDate.trim().length > 0)) {
                                    currentLine.btLogic.orderDetail.ShipDate = shipment._ShipDate.trim();
                                } else if (angular.isString(shipment._ExpectedShipmentDate) && (shipment._ExpectedShipmentDate.trim().length > 0)) {
                                    currentLine.btLogic.orderDetail.ShipDate = shipment._ExpectedShipmentDate.trim();
                                }
                            }
                        }
                    }
                }
            };

            var _setUpDisplayTable = function (result) {

                $scope.orderDetail = result;
                _setBtLogicDetails();

                if ($scope.orderDetail._DocumentType && $scope.orderDetail._DocumentType == '0003') {
                    $scope.isReturnOrder = true;
                }

                $scope.selectLineItem($scope.orderDetail.OrderLines.OrderLine[0]);


                //if the Table has already been created, then reload/refresh from current data
                if (angular.isObject($scope.orderlineTableReturnParams)) {
                    $scope.orderlineTableReturnParams.reload();
                } else {
                    $scope.orderlineTableReturnParams = new ngTableParams({
                        page: 1,            // show first page
                        count: 100000          // count per page
                        //sorting: {
                        //    _OrderDate: 'desc'     // initial sorting
                        //}
                    }, {
                        total: $scope.orderDetail.OrderLines.OrderLine.length, // length of data
                        counts: [],
                        getData: function ($defer, params) {
                            // use build-in angular filter
                            var orderedData = params.sorting() ?
                                                $filter('orderBy')($scope.orderDetail.OrderLines.OrderLine, params.orderBy()) :
                                                $scope.orderDetail.OrderLines.OrderLine;

                            $defer.resolve(orderedData);
                            $scope.refreshReturnsIScroll();
                        }
                    });
                }

                //if the Table has already been created, then reload/refresh from current data
                if (angular.isObject($scope.orderlineTableParams)) {
                    $scope.orderlineTableParams.reload();
                } else {
                    $scope.orderlineTableParams = new ngTableParams({
                        page: 1,            // show first page
                        count: 100000          // count per page
                        //sorting: {
                        //    _OrderDate: 'desc'     // initial sorting
                        //}
                    }, {
                        total: $scope.orderDetail.OrderLines.OrderLine.length, // length of data
                        counts: [],
                        getData: function ($defer, params) {
                            // use build-in angular filter
                            var orderedData = params.sorting() ?
                                                $filter('orderBy')($scope.orderDetail.OrderLines.OrderLine, params.orderBy()) :
                                                $scope.orderDetail.OrderLines.OrderLine;

                            $defer.resolve(orderedData);
                            $scope.refreshIScroll();
                        }
                    });
                }

                $scope.isLoaded = true;
                if ($scope.isReturnOrder) {
                    $scope.refreshIScroll();
                } else {
                    $scope.refreshReturnsIScroll();
                }
            };

            $scope.getOrderDetails = function () {
                $order.getOrderDetail(param, _setUpDisplayTable,
                function (error) { alert(error); });
            };

            $scope.orderDetail = $order.getCurrentOrderDetails();

            if (!angular.isDefined($scope.orderDetail) || $scope.orderDetail === null ||
               $scope.orderDetail._OrderNo !== selectedOrder._OrderNo) {
                $scope.getOrderDetails();
            } else {
                _setUpDisplayTable($scope.orderDetail);
            }

            $scope.goToNotes = function (orderline) {
                var orderlineNo = '';

                if (orderline) {
                    //get orderline prime sub no join with _
                    orderlineNo = orderline._PrimeLineNo.toString().trim() + '_' + orderline._SubLineNo.toString().trim();
                }


                $state.go('orderNote', { 'orderLineNo': orderlineNo });
            };

            $scope.isOrderPrintable = function () {
                if ($scope.orderDetail && $scope.orderDetail._DocumentType && $scope.orderDetail._Status &&
                    $scope.orderDetail._DocumentType === "0001" && !(/(^|\s)draft\s/).test($scope.orderDetail._Status.toString().toLowerCase())) {
                    return true;
                } else {
                    return false;
                }
            };
            $scope.isOrderHasReturns = function () {
                if ($scope.orderDetail && $scope.orderDetail.ReturnOrders && $scope.orderDetail.ReturnOrders.ReturnOrder && ($scope.orderDetail.ReturnOrders.ReturnOrder.length > 0)) {
                    return true;
                } else {
                    return false;
                }
            };

            $scope.isShopRunnerOrder = function () {
                if ($scope.orderDetail && $scope.orderDetail.Extn &&
                        angular.isString($scope.orderDetail.Extn._ExtnSRAuthToken) && ($scope.orderDetail.Extn._ExtnSRAuthToken.length > 0)) {
                    return true;
                } else {
                    return false;
                }
            };

            $scope.isBuyablePriceStatusFOrP = function (orderline) {
                if (angular.isDefined(orderline.btDisplay)) {

                    return $itemProperty.isItemActiveBuyable(orderline.btDisplay);
                }

                return false;
            };

            $scope.openOrderDetail = function (orderNo) {
                $order.setSelectedOrder({'_OrderNo':angular.copy(orderNo)});
                param.GetSterlingOrderDetailReq._OrderNo = orderNo;
                $scope.orderDetail._OrderNo = orderNo;
                $scope.isReturnOrder = false;
                $scope.isLoaded = false;
                $scope.getOrderDetails();

            };

            $scope.openOrderPricingDetail = function () {

                var modalInstance = $modal
                        .open({
                            templateUrl: 'html/order/orderPricingDetail.html',
                            controller: 'orderDetailsOrderPricingDetailCtrl',
                            resolve: {
                                cart: function () {
                                    return $scope.orderDetail;
                                }
                            }
                        });

            };

            $scope.openOrderLineDetail = function (ngTableOrderline) {

                var modalInstance = $modal
                        .open({
                            templateUrl: 'html/order/orderDetailOrderLineAdditionalDetails.html',
                            controller: 'orderDetailsOrderLineDetailCtrl',
                            resolve: {
                                orderline: function () {
                                    return ngTableOrderline;
                                },
                                associateCreatedOrder: function () {
                                    if ($scope.orderDetail.Extn) {
                                        return $scope.orderDetail.Extn._ExtnAssociateId;
                                    } else {
                                        return '';
                                    }
                                }
                            },
                            size: 'lg'
                        });

            };

            $scope.returnSummary = function () { $state.go('returnSummary'); };

            $scope.showCancelOrderButton = function () {

               // temporary if to hide Cancel Order Button for ALL users
                if ($btProp.getProp('orderDetailCanCancelOrder') === false) {
                    return false;
                }
                else {
                    return $order.canCancelOrder($scope.orderDetail);
                }
            };

            $scope.cancelOrder = function (orderDetail) {

                var modalInstance = $modal
                        .open({
                            templateUrl: 'html/order/cancelOrderModal.html',
                            controller: 'cancelOrderCtrl',
                            resolve: {
                                passedOrderDetail: function () {
                                    return orderDetail;
                                }
                            }
                        });

                modalInstance.result.then(
                    function () {
                        $order.getOrderDetail(param, function (result) {
                            $scope.orderDetail = result;
                            $scope.orderlineTableParams.reload();
                        }, function (error) {
                            alert(error);
                        });
                    },
                    function () {
                    });
            };
            $scope.showDraftReOrderButton = function () {

                // temporary if to hide Cancel Order Button for ALL users
                if ($btProp.getProp('orderDetailCanLoadDraftOrder') === false) {
                    return false;
                }

                if (angular.isDefined($scope.orderDetail) && angular.isDefined($scope.orderDetail._DocumentType) && $scope.orderDetail._DocumentType == "0001") {
                    return true;
                } else {
                    return false;
                }
            };

            $scope.draftButtonText = function () {
                if (angular.isDefined($scope.orderDetail) && angular.isDefined($scope.orderDetail._DraftOrderFlag) && $scope.orderDetail._DraftOrderFlag === "Y") {
                    return 'Continue Draft';
                } else {
                    return 'Re-Order';
                }
            };
            $scope.loadDraftOrder = function () {
                $rootScope.$broadcast('breadCrumbReset');
                // TODO: delete Draft order before loading it??

                //order is on the appServicesOrder.cache
                $state.go('orderCart', { 'loadDraft': 'LoadDraft' });

            };
        }])
    .controller('orderDetailsOrderLineDetailCtrl', ['$scope', 'orderline', 'associateCreatedOrder', '$filter', '$modalInstance', 'order', function ($scope, orderline, associateCreatedOrder, $filter, $modalInstance, $order) {

        $scope.orderline = orderline;
        $scope.associateCreatedOrder = associateCreatedOrder;
        var pricingWithBadTotal = $order.orderLineItemPricingDetail(orderline);
        if (pricingWithBadTotal && angular.isArray(pricingWithBadTotal.category)) {
            for (var i = 0; i < pricingWithBadTotal.category.length; i++) {
                if ((/^total$/i).test(pricingWithBadTotal.category[i].categoryDescription)) {
                    pricingWithBadTotal.category.splice(i, 1);
                    break;
                }
            }
        }
        
        $scope.pricingDetail = pricingWithBadTotal;
        $scope.hasCoupons = angular.isArray($scope.pricingDetail.coupons) && ($scope.pricingDetail.coupons.length > 0);

        var myModalIScroll = null;
        $scope.refreshModalIScroll = function () {
            if (!myModalIScroll) {
                myModalIScroll = new IScroll("#discountsWrapper", { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
            }
            setTimeout(function () {
                myModalIScroll.refresh();
            }, 500);
        };

        $scope.close = function () {
            $modalInstance.close();
        };
    }])
    .controller('orderDetailsOrderPricingDetailCtrl', ['$scope', 'order', 'cart', '$modalInstance', '$modal', 'numberOnlyKeyPressValidator', function ($scope, order, cart, $modalInstance, $modal, $numberOnlyKeyPressValidator) {
        var totalShippingDiscount;
        $scope.updatingPrice = false;
        $scope.isModalCalledFromOrderDetails = true;
        order.getSkyTotaling(cart);


        $scope.openModifyCharge = function (item) {
            $scope.item = item;
            $scope.newCharge = item.total;
            $scope.updatingPrice = true;
        };

        $scope.numberValidator = $numberOnlyKeyPressValidator;

        $scope.calculateFlatRate = function () {
            $scope.newCharge = $scope.item.total;
            $scope.percentage = undefined;
            if ($scope.flatRate && isFinite($scope.flatRate)) {
                $scope.newCharge = $scope.flatRate > $scope.item.total || $scope.flatRate < 0 ? $scope.item.total : $scope.flatRate;
            }
        }

        $scope.update = function () {
            totalShippingDiscount = (parseFloat(cart._ShippingChrgTotal) - parseFloat($scope.newCharge)).toFixed(2);
            var orderTotal = 0;
            angular.forEach($scope.pricingDetail.category, function (value, key) {
                if (value.categoryDescription.toLowerCase() === 'shipping') {
                    value.total = $scope.newCharge;
                }

                if (value.categoryDescription.toLowerCase() === 'order total') {
                    value.total = cart._OrderTotal - ((cart._ShippingChrgTotal - $scope.newCharge) - cart._ShippingDiscTotal);
                }
            });

            $scope.updatingPrice = false;
            $scope.newCharge = 0;

            $scope.close();
        }

        $scope.cancel = function () {
            $scope.updatingPrice = false;
        }

        $scope.pricingDetail = order.orderPricingDetail(cart, !$scope.isModalCalledFromOrderDetails);

        $scope.close = function () {
            var returnVar = {
                newShippingDiscount: totalShippingDiscount,
                cart: cart
            };
            $modalInstance.close(returnVar);
        };

        $scope.dismiss = function () {
            $modalInstance.dismiss();
        };
    }])
    .controller('orderPricingDetailCtrl', ['$scope', 'order', 'cart', '$modalInstance', '$modal', 'numberOnlyKeyPressValidator', function ($scope, order, cart, $modalInstance, $modal, $numberOnlyKeyPressValidator) {
        var totalShippingDiscount;
        $scope.isModalCalledFromOrderDetails = false;
        $scope.updatingPrice = false;

        //if no _UnitPriceTotal then we must be coming from the order search's order details screen. 
        //(so this is a Sterling order and therefore has not gone through Sky's pricing)
        if (!angular.isDefined(cart._UnitPriceTotal) || cart._UnitPriceTotal === null) {
            $scope.isModalCalledFromOrderDetails = true;
            order.getSkyTotaling(cart);

        }

        $scope.openModifyCharge = function (item) {
            $scope.item = item;
            $scope.newCharge = item.total;
            $scope.updatingPrice = true;
        };

        $scope.numberValidator = $numberOnlyKeyPressValidator;

        $scope.calculateFlatRate = function () {
            $scope.newCharge = $scope.item.total;
            $scope.percentage = undefined;
            if ($scope.flatRate && isFinite($scope.flatRate)) {
                $scope.newCharge = $scope.flatRate > $scope.item.total || $scope.flatRate < 0 ? $scope.item.total : $scope.flatRate;
            }
        }

        $scope.update = function () {
            totalShippingDiscount = (parseFloat(cart._ShippingChrgTotal) - parseFloat($scope.newCharge)).toFixed(2);
            var orderTotal = 0;
            angular.forEach($scope.pricingDetail.category, function (value, key) {
                if (value.categoryDescription.toLowerCase() === 'shipping') {
                    value.total = $scope.newCharge;
                }

                if (value.categoryDescription.toLowerCase() === 'order total') {
                    value.total = cart._OrderTotal - ((cart._ShippingChrgTotal - $scope.newCharge) - cart._ShippingDiscTotal);
                }
            });

            $scope.updatingPrice = false;
            $scope.newCharge = 0;

            $scope.close();
        }

        $scope.cancel = function () {
            $scope.updatingPrice = false;
        }

        $scope.pricingDetail = order.orderPricingDetail(cart, !$scope.isModalCalledFromOrderDetails);

        $scope.close = function () {
            var returnVar = {
                newShippingDiscount: totalShippingDiscount,
                cart: cart
            };
            $modalInstance.close(returnVar);
        };

        $scope.dismiss = function () {
            $modalInstance.dismiss();
        };
    }])
    .controller('orderLinePricingDetailCtrl', ['$scope', 'order', 'orderLine', '$modalInstance', '$modal', 'numberOnlyKeyPressValidator', function ($scope, order, orderLine, $modalInstance, $modal, $numberOnlyKeyPressValidator) {
        var totalShippingDiscount = 0;
        $scope.openModifyCharge = function () {
            var item = {};
            var lineShippingTotal = 0;

            angular.forEach(orderLine.LineCharges.LineCharge, function (lineCharge) {
                if (lineCharge._ChargeCategory === 'BTN_SHIP_CHRG') {
                    lineShippingTotal += parseFloat(lineCharge._ChargeAmount);
                    shippingCharge = parseFloat(lineCharge._ChargeAmount);
                } else if (lineCharge._ChargeCategory === 'BTN_SHIP_DISC') {
                    lineShippingTotal -= parseFloat(lineCharge._ChargeAmount);
                }
            });
            item.total = lineShippingTotal;

            $scope.item = item;
            $scope.newCharge = item.total;
            $scope.updatingPrice = true;
        };

        $scope.numberValidator = $numberOnlyKeyPressValidator;

        $scope.calculateFlatRate = function () {
            $scope.newCharge = $scope.item.total;
            $scope.percentage = undefined;
            if ($scope.flatRate && isFinite($scope.flatRate)) {
                $scope.newCharge = $scope.flatRate > $scope.item.total || $scope.flatRate < 0 ? $scope.item.total : $scope.flatRate;
            }
        }

        $scope.cancel = function () {
            $scope.updatingPrice = false;
        }
        $scope.update = function () {
            totalShippingDiscount = ($scope.item.total - parseFloat($scope.newCharge)).toFixed(2);
            angular.forEach($scope.pricingDetail.category, function (value, key) {
                if (value.categoryDescription === $scope.item.categoryDescription) {
                    value.total = $scope.newCharge;
                }
            });
            $scope.updatingPrice = false;
            $scope.newCharge = 0;

            $scope.save();
        }

        $scope.pricingDetail = order.orderLineItemPricingDetail(orderLine);

        $scope.close = function () {
            $modalInstance.dismiss('closed');
        };

        $scope.save = function () {
            $modalInstance.close(totalShippingDiscount);
        };
    }])
.controller('cancelOrderCtrl', ['$scope', 'passedOrderDetail', '$modalInstance', 'order', 'sendSMTPErrorEmail', function ($scope, passedOrderDetail, $modalInstance, $order, $sendSMTPErrorEmail) {

    $scope.canCancelOrder = function () {
        return $order.canCancelOrder(passedOrderDetail);
    };

    $scope.cancelOrder = function () {

        if ($order.canCancelOrder(passedOrderDetail)) {
            $order.cancelOrder(passedOrderDetail).then(function () {
                $modalInstance.close();
            }, function (errorMessage) {
                $modalInstance.dismiss();
                $sendSMTPErrorEmail(errorMessage, serviceURL + "/Order/ChangeOrderJSON");
            });
        } else {
            $scope.dismiss();
        }
    };

    $scope.dismiss = function () {
        $modalInstance.dismiss();
    };
}]);