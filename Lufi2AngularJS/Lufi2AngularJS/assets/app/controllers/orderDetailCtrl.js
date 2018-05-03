angular.module('orderDetail', ['ui.bootstrap'])
    .controller('orderDetailCtrl', ['$scope', 'order', '$modal', '$location', 'itemSearch', 'itemProperty', 'printerService', '$rootScope', 'ngTableParams', '$filter', '$q', '$state', 'sendSMTPErrorEmail', 'btProp', 'POSService', 'securityService',
        function ($scope, $order, $modal, $location, $itemSearch, $itemProperty, $printerService, $rootScope, ngTableParams, $filter, $q, $state, $sendSMTPErrorEmail, $btProp, $POSService, $securityService) {

            $scope.isLoaded = false;
            $scope.isReturnOrder = false;
            $scope.orderDetail = {};
            $scope.isCsr = $securityService.isCsr();

            var storeNumber = "";
            storeNumber = $POSService.getPOSParameters().storeNumber;
            storeNumber = storeNumber.toString();
            
            while (storeNumber.length < 3) {
                storeNumber = "0" + storeNumber;
            }


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



            $scope.printPickUpReceipt= function (PrintType){
                $printerService.claimPrinter().then(
                    function () {
                        selectedOrder._DocumentType = "0001";
                        $printerService.printOrder(selectedOrder._OrderNo, false, "BOPISPickupReceipt");
                        $printerService.releasePrinter();
                    });
            };

            $scope.printOrderSummary = function (PrintType) {
                $printerService.claimPrinter().then(
                    function () {
                        $printerService.printOrder(selectedOrder._OrderNo, false, "LUFIOrderSummary");
                        $printerService.releasePrinter();
                    });
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

            //this is set to true in both  $scope.showPickUpReceiptButton and $scope.showConfirmPickupButton
            $scope.hasBopisLines = false;
            $scope.bopisOverallStatus = "---";

            var _determineBopisOverallStatus = function () {
                if (angular.isDefined($scope.orderDetail) &&
                    angular.isDefined($scope.orderDetail.Shipments) && angular.isArray($scope.orderDetail.Shipments.Shipment) && ($scope.orderDetail.Shipments.Shipment.length > 0)) {

                    //check all BOPIS shipmentlines to determine lowest status.
                    var statusEnum = {
                        "1100.100": 1,
                        "1400.100": 2,
                        "1600.002": 2,
                        "1600.002.001": 2,
                        "1600.002.010": 3,
                        "1600.002.020": 4,
                        "9000": 5,
                        getStatusName: function (statusInt) {
                            if (statusInt === 1) {
                                return "Picking Items";
                            } else if (statusInt === 2) {
                                return "Ready for Pick-Up";
                            } else if (statusInt === 3) {
                                return "Customer Has Picked-Up";
                            } else if (statusInt === 4) {
                                return "Returned-Restocked";
                            } else if (statusInt === 5) {
                                return "Cancelled";
                            } else {
                                return "Unknown Status";
                            }
                        }
                    };

                    var lowestStatus = undefined;

                    for (var i = 0; i < $scope.orderDetail.Shipments.Shipment.length; i++) {
                        var currentShipment = $scope.orderDetail.Shipments.Shipment[i];

                        if ((/^\s*BOPIS\s*$/i).test(currentShipment._ShipmentType)) {

                            if (lowestStatus === undefined) {
                                lowestStatus = statusEnum[currentShipment._Status];
                            } else {
                                if (angular.isDefined(currentShipment._Status) && angular.isDefined(statusEnum[currentShipment._Status]) && (statusEnum[currentShipment._Status] < lowestStatus)) {
                                    lowestStatus = statusEnum[currentShipment._Status];
                                }
                            }
                        }
                    }

                    $scope.bopisOverallStatus = statusEnum.getStatusName(lowestStatus);
                } else {
                    $scope.bopisOverallStatus = "---";
                }
            };

            _determineBopisOverallStatus();


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

                        currentLine.btLogic.orderDetail = { isReturned: false, isGiftMessage: false, isBopisLine: false, trackingNoString: '', ShipDate: '', CarrierServiceCode:''  };

                        if (currentLine.ReturnOrderLines && currentLine.ReturnOrderLines.OrderLine && (currentLine.ReturnOrderLines.OrderLine.length > 0)) {
                            currentLine.btLogic.orderDetail.isReturned = true;
                        }
                        if (angular.isString(currentLine._LineType) && (/^\s*BOPIS\s*$/i).test(currentLine._LineType)) {
                            currentLine.btLogic.orderDetail.isBopisLine = true;
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
                        if(currentLine.btLogic.orderDetail.isBopisLine){
                            currentLine.btLogic.orderDetail.trackingNoString = "---";

                        }
                        else if (currentLine.Containers && currentLine.Containers.Container && (currentLine.Containers.Container.length > 0)) {
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
                        if (currentLine.btLogic.orderDetail.isBopisLine) {
                            currentLine.btLogic.orderDetail.ShipDate = "---";
                        }
                        else if (currentLine.ShipmentLines && angular.isArray(currentLine.ShipmentLines.ShipmentLine) && (currentLine.ShipmentLines.ShipmentLine.length > 0)
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

                        //set CarrierService display text
                        if (currentLine.btLogic.orderDetail.isBopisLine) {
                            currentLine.btLogic.orderDetail.CarrierServiceCode = "---";
                        }
                        else {
                            
                            currentLine.btLogic.orderDetail.CarrierServiceCode = angular.isString(currentLine._CarrierServiceCode) ? currentLine._CarrierServiceCode : "";
                        }
                    }
                }
            };

            var _setUpDisplayTable = function (result) {

                $scope.orderDetail = result;
                _setBtLogicDetails();
                _determineBopisOverallStatus();

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
                $scope.isLoaded = false;
                $order.getOrderDetail(param, _setUpDisplayTable,
                function (error) { alert(error); });
            };

            var _initOrderDetail = function () {

                $scope.orderDetail = {};

                var previousLoadedOrder = $order.getCurrentOrderDetails();

                if (!angular.isDefined(previousLoadedOrder) || previousLoadedOrder === null ||
               previousLoadedOrder._OrderNo !== selectedOrder._OrderNo) {
                    $scope.getOrderDetails();
                } else {
                    $scope.orderDetail = previousLoadedOrder;
                    _setUpDisplayTable($scope.orderDetail);
                }
            };

            _initOrderDetail();

            $scope.goToNotes = function (orderline) {
                var orderlineNo = '';

                if (orderline) {
                    //get orderline prime sub no join with _
                    orderlineNo = orderline._PrimeLineNo.toString().trim() + '_' + orderline._SubLineNo.toString().trim();
                }


                $state.go('orderNote', { 'orderLineNo': orderlineNo });
            };

            $scope.isOrderPrintable = function () {
                if ($scope.isCsr) {
                    return false;
                }

                if ($scope.orderDetail && $scope.orderDetail._DocumentType && $scope.orderDetail._Status &&
                    $scope.orderDetail._DocumentType === "0001" && !(/(^|\s)(draft|cancelled)/).test($scope.orderDetail._Status.toString().toLowerCase())) {
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


            var _getBopisDesignee = function (orderline, order) {

                //get  { Name: "", NotificationType: "", NotificationMobileOrEmailDetail: "" };
                var designee = { Name: "", NotificationType: "", NotificationMobileOrEmailDetail: "" };

                if (orderline.btLogic.orderDetail.isBopisLine) {

                    var tempDesignee = { Name: "", NotificationType: "", MobilePhone: "", Email: "", hasName:false, hasMobilePhone:false,hasEmail:false };

                    //if there are instructions for designee, find them
                    if ($scope.orderDetail.Instructions && angular.isArray($scope.orderDetail.Instructions.Instruction)) {
                        for (var i = 0; i < $scope.orderDetail.Instructions.Instruction.length; i++) {
                            var currentInstruction = $scope.orderDetail.Instructions.Instruction[i];
                            if (angular.isString(currentInstruction._InstructionType) && angular.isString(currentInstruction._InstructionText)) {
                            
                                if ((/^\s*Designee\s*$/i).test(currentInstruction._InstructionType)) {
                                    tempDesignee.Name = currentInstruction._InstructionText.trim();
                                    if (tempDesignee.Name.length > 0) {
                                        tempDesignee.hasName = true;
                                    }
                                }
                                else if ((/^\s*Text\s*$/i).test(currentInstruction._InstructionType)) {
                                    tempDesignee.MobilePhone = currentInstruction._InstructionText.trim();
                                    if (tempDesignee.MobilePhone.length > 0) {
                                        tempDesignee.hasMobilePhone = true;
                                    }
                                }
                                else if ((/^\s*Email\s*$/i).test(currentInstruction._InstructionType)) {
                                    tempDesignee.Email = currentInstruction._InstructionText.trim();
                                    if (tempDesignee.Email.length > 0) {
                                        tempDesignee.hasEmail = true;
                                    }
                                }
                            }

                        }
                    }

                    if (!tempDesignee.hasName) {
                        //get name from bill to
                        if (order && order.PersonInfoBillTo) {
                            if (angular.isString(order.PersonInfoBillTo._FirstName)) {
                                tempDesignee.Name = order.PersonInfoBillTo._FirstName.trim();
                            }
                            if (angular.isString(order.PersonInfoBillTo._LastName)) {
                                tempDesignee.Name += " " + order.PersonInfoBillTo._LastName.trim();
                            }

                            tempDesignee.Name = tempDesignee.Name.trim();

                            if (tempDesignee.Name.length < 1) {
                                tempDesignee.Name = "Unknown";
                            }
                        }
                    }

                    if (!tempDesignee.hasEmail) {
                        //get email from bill to
                        if (order && order.PersonInfoBillTo) {
                            if (angular.isString(order.PersonInfoBillTo._EmailID)) {
                                tempDesignee.Email = order.PersonInfoBillTo._EmailID.trim();
                            }

                            if (tempDesignee.Email.length < 1) {
                                tempDesignee.Email = "Unknown";
                                tempDesignee.hasEmail = false;
                            } else {
                                tempDesignee.hasEmail = true;
                            }
                        }
                    }

                    //populate the results
                    designee.Name = tempDesignee.Name;

                    if (tempDesignee.hasMobilePhone) {
                        designee.NotificationType = "Text";
                        designee.NotificationMobileOrEmailDetail = tempDesignee.MobilePhone;
                    } else if (tempDesignee.hasEmail) {
                        designee.NotificationType = "Email";
                        designee.NotificationMobileOrEmailDetail = tempDesignee.Email;
                    } else {
                        designee.NotificationType = "None";
                        designee.NotificationMobileOrEmailDetail = "";
                    }

                }
                return designee;
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
                                },
                                bopisDesignee: function () {
                                    return _getBopisDesignee(ngTableOrderline, $scope.orderDetail);
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
                    //if ("Ready to Pick for Customer" == $scope.orderDetail._Status) {
                        return true;
                    //}
                  
                } else {
                    return false;
                }
            };

            //$scope.showBopisButtons = function () {
            //    var response = false;
            //    if (angular.isDefined($scope.orderDetail) && angular.isDefined($scope.orderDetail._OrderType) && $scope.orderDetail._OrderType == "BOPIS" &&
            //        angular.isDefined($scope.orderDetail.Shipments) && angular.isArray($scope.orderDetail.Shipments.Shipment) && ($scope.orderDetail.Shipments.Shipment.length > 0)) {
            //        response = true;
            //    }

            //    return response;
            //};


            $scope.showPickUpReceiptButton = function () {

                if ($scope.isCsr) {
                    return false;
                }

                var response = false;
                if (angular.isDefined($scope.orderDetail) &&
                    angular.isDefined($scope.orderDetail.Shipments) && angular.isArray($scope.orderDetail.Shipments.Shipment) && ( $scope.orderDetail.Shipments.Shipment.length > 0)) {

                    //check all BOPIS shipmentlines to see if they are picked.
                    // assumes only one shipment contain all Bopis order lines
                    var bopisShipment = null;
                    
                    for (var i = 0; i < $scope.orderDetail.Shipments.Shipment.length; i++) {
                        var currentShipment = $scope.orderDetail.Shipments.Shipment[i];
                        
                        if ((/^\s*BOPIS\s*$/i).test(currentShipment._ShipmentType)) {
                            $scope.hasBopisLines = true;

                            if (currentShipment._ShipNode === storeNumber) {
                                bopisShipment = currentShipment;
                                break;
                            }
                        }

                    }

                    if (bopisShipment !== null && angular.isString(bopisShipment._Status)) {
                        if (bopisShipment._Status === "1100.100" || bopisShipment._Status === "1400.100" || bopisShipment._Status === "1600.002" || bopisShipment._Status === "1600.002.001") {
                            
                            //check that all ShipmentLines have been picked
                            if (angular.isDefined(bopisShipment.ShipmentLines) && angular.isArray(bopisShipment.ShipmentLines.ShipmentLine) && (bopisShipment.ShipmentLines.ShipmentLine.length > 0)) {
                                var isAllLinesYes = true;
                                for (var p = 0; p < bopisShipment.ShipmentLines.ShipmentLine.length; p++) {
                                    var currentShipmentLine = bopisShipment.ShipmentLines.ShipmentLine[p];
                                    if (!(angular.isDefined(currentShipmentLine.Extn) && angular.isString(currentShipmentLine.Extn._ExtnIsPicked) && (currentShipmentLine.Extn._ExtnIsPicked === "Y"))) {
                                        isAllLinesYes = false;
                                        break;
                                    }
                                }
                                response = isAllLinesYes;
                            }
                        }
                    }
                } 

                return response;
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

            $scope.showConfirmPickupButton = function () {

                if ($scope.isCsr) {
                    return false;
                }

                var response = false;
                if (angular.isDefined($scope.orderDetail) &&
                    angular.isDefined($scope.orderDetail.Shipments) && angular.isArray($scope.orderDetail.Shipments.Shipment) && ($scope.orderDetail.Shipments.Shipment.length > 0)) {

                    //check all BOPIS shipmentlines to see if they are picked.
                    // assumes only one shipment contain all Bopis order lines
                    var bopisShipment = null;

                    for (var i = 0; i < $scope.orderDetail.Shipments.Shipment.length; i++) {
                        var currentShipment = $scope.orderDetail.Shipments.Shipment[i];

                        if ((/^\s*BOPIS\s*$/i).test(currentShipment._ShipmentType)) {
                            $scope.hasBopisLines = true;

                            if (currentShipment._ShipNode === storeNumber) {
                                bopisShipment = currentShipment;
                                break;
                            }
                        }

                    }

                    if (bopisShipment !== null && angular.isString(bopisShipment._Status)) {
                        if (bopisShipment._Status === "1400.100" || bopisShipment._Status === "1600.002" || bopisShipment._Status === "1600.002.001") {

                            response = true;
                        }
                    }
                }

                return response;
            };

            $scope.confirmCustomerPickup = function () {

                var modalInstance = $modal
                   .open({
                       templateUrl: 'html/pickUpInStore/confirmPickupModal.html',
                       controller: 'confirmPickupCtrl',
                       backdrop:'static',
                       resolve: {
                           cart: function () {
                               return $scope.orderDetail;
                           }
                       },
                       size: 'lg',

                   });

                modalInstance.result.then(
                    function () {
                        $scope.getOrderDetails()
                    },
                    function () {
                    });
            };

        }])
    .controller('orderDetailsOrderLineDetailCtrl', ['$scope', 'orderline', 'associateCreatedOrder', 'bopisDesignee', '$filter', '$modalInstance', 'order', function ($scope, orderline, associateCreatedOrder, bopisDesignee, $filter, $modalInstance, $order) {
        // orderline.btLogic.orderDetail = { isReturned: false, isGiftMessage: false, isBopisLine: false, trackingNoString: '', ShipDate: '', CarrierServiceCode:''  };

        $scope.orderline = orderline;
        $scope.bopisPickup = { Name: "", NotificationType: "", NotificationMobileOrEmailDetail: "" };
        $scope.associateCreatedOrder = associateCreatedOrder;

        //if it is a Bopis order than get the passed Designee info
        if ($scope.orderline.btLogic.orderDetail.isBopisLine && angular.isObject(bopisDesignee)) {
            $scope.bopisPickup.Name = angular.isString(bopisDesignee.Name) ? bopisDesignee.Name.trim() :"N/A";
            $scope.bopisPickup.NotificationType = angular.isString(bopisDesignee.NotificationType) ? bopisDesignee.NotificationType.trim() : "N/A";

            $scope.bopisPickup.NotificationMobileOrEmailDetail = angular.isString(bopisDesignee.NotificationMobileOrEmailDetail) ? bopisDesignee.NotificationMobileOrEmailDetail.trim() : "";
            // add Label to $scope.bopisPickup.NotificationMobileOrEmailDetail based on whether it is an email or phone number
            $scope.bopisPickup.NotificationMobileOrEmailDetail = ((/^\s*Text\s*$/i).test($scope.bopisPickup.NotificationType)) ?
                "Mobile Number: " + $filter('phoneFormat')($scope.bopisPickup.NotificationMobileOrEmailDetail) :
                "Email: " + $scope.bopisPickup.NotificationMobileOrEmailDetail;
        }

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

    var _IsCancelling = false;

    $scope.canCancelOrder = function () {
        return !_IsCancelling && $order.canCancelOrder(passedOrderDetail);
    };

    $scope.cancelOrder = function () {

        if ($order.canCancelOrder(passedOrderDetail)) {

            _IsCancelling = true;

            $order.cancelOrder(passedOrderDetail).then(function () {
                _IsCancelling = false;
                $modalInstance.close();
            }, function (errorMessage) {
                _IsCancelling = false;
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