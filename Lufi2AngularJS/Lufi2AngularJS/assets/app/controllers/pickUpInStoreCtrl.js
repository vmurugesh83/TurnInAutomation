angular.module('pickUpInStore', ['ui.bootstrap'])
    .controller('pickUpInStoreCtrl', ['$scope', '$modal', 'order',  '$rootScope', '$filter', '$q', '$state', 'sendSMTPErrorEmail', 'pickUpInStore','POSService','printerService', 'ngTableParams', '$interval',
        function ($scope, $modal, $order, $rootScope, $filter, $q, $state, $sendSMTPErrorEmail, $pickUpInStore, $POSService, $printerService, ngTableParams, $interval) {

            $scope.isPrintingPackReceipts = false;

            //$scope.readyforCustomerShipmentresponseArray = [];

            //$scope.refreshIScroll = function () {
            //    setTimeout(function () {
            //        $scope.$parent.myScroll['wrapper'].refresh();
            //    }, 500);
            //};

            //$scope.myScrollOptions = { tap: true };

            //$scope.tableParams = new ngTableParams({
            //    page: 1,            // show first page
            //    count: 100000,          // count per page
            //    sorting: {
            //        "_orderNumber": "asc"    // initial sorting
            //    }
            //}, {
            //    total: $scope.readyforCustomerShipmentresponseArray.length, // length of data
            //    counts: [],
            //    defaultSort: "asc",
            //    getData: function ($defer, params) {
            //        var orderedData = params.sorting() ?
            //                            $filter('orderBy')($scope.readyforCustomerShipmentresponseArray, params.orderBy()) :
            //                            $scope.readyforCustomerShipmentresponseArray;

            //        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            //        $scope.refreshIScroll();
            //    }
            //});
           

            $scope.openOrder = function () {
                if ($scope.formInput.orderNo) {
                    param = {
                        "GetSterlingOrderListReq":
                            {
                                "_ReadFromHistory": "N",
                                "_OrderNo": $scope.formInput.orderNo,
                                "_DraftOrderFlag": "N"
                            }
                    };
                    if (angular.isDefined($scope.formInput.orderNo)) {
                             $order.getOrderList(param, function (result) {
                            if (result === undefined) {
                                jQuery('.not-found-alert').modal('show');
                            } else {
                                $scope.openOrderDetail($scope.formInput.orderNo);
                            }
                        }, function (error) { alert(error); });
                    }
                }
            };
            $scope.batchprintresponseArray = {};
            $scope.batchLength ="--";

            var _refreshBatchPrint = function () {
                $scope.batchLength = "--";

                $pickUpInStore.getBatchPrintShipmentListArray($POSService.getPOSParameters().storeNumber).then(
                    function (map) {
                        $scope.batchprintresponseArray = map;
                        $scope.batchLength = "" + Object.keys($scope.batchprintresponseArray).length;
                    });
            };

            _refreshBatchPrint();

            $interval(_refreshBatchPrint, 660000);

            //$pickUpInStore.getReadyforCustomerShipmentListArray($POSService.getPOSParameters().storeNumber).then(function (arrayResult) {
            //    $scope.readyforCustomerShipmentresponseArray = arrayResult;
            //    $scope.tableParams.reload();
            //});
            
            $scope.openOrderDetail = function (orderNo) {
                $order.setSelectedOrder({ _OrderNo: orderNo, _DocumentType: "0001" });
                $state.go("orderDetail");
            }

            $scope.navigateToOrderSearchPage = function(){
                $state.go("orderSearch");
            }

            $scope.navigateToBopisBackupPick = function () {
                $state.go("alternatePickAndPack");
            };

            var deregisterPrinterErrorEvent = $rootScope.$on('Printer_Error', function (event, errorData) {
                swal({ title: "Alert!", text: errorData.StatusText, showConfirmButton: true });
            });

            $scope.$on('destroy', function () {
                deregisterPrinterErrorEvent();
            });

            $scope.navigateToPickListPage = function () {
                $state.go('pickingConfirm');
            }
            $scope.navigateToPackConfirmPage = function () {
                $state.go('packingConfirm');
            }


            $scope.printPendingReceipts = function (PrintType) {
                if (!$scope.isPrintingPackReceipts) {
                    $printerService.claimPrinter().then(function () {

                        $scope.isPrintingPackReceipts = true;

                        var promiseArray = [];

                        for (var key in $scope.batchprintresponseArray) {
                            if ($scope.batchprintresponseArray.hasOwnProperty(key)) {
                                var currentShipment = $scope.batchprintresponseArray[key];
                                if (angular.isDefined(currentShipment) && angular.isDefined(currentShipment.ShipmentLines) && angular.isArray(currentShipment.ShipmentLines.ShipmentLine) && (currentShipment.ShipmentLines.ShipmentLine.length > 0)) {
                                    var OrderNo = currentShipment.ShipmentLines.ShipmentLine[0]._OrderNo;
                                
                                    //$order.setSelectedOrder({ _OrderNo: OrderNo, _DocumentType: "0001" });
                                    $printerService.printOrder(OrderNo, false, "BOPISPickupReceipt");
                                
                                    promiseArray.push( $pickUpInStore.changeShipment(currentShipment._ShipmentKey, $POSService.getPOSParameters.storeNumber));
                                }
                            }
                        }

                        $printerService.releasePrinter();

                        //First set batch print count to zero while we wait for refresh of count.
                        $scope.batchLength = "--";

                        //refresh batch print
                        $q.all(promiseArray).then(function () {
                            $scope.batchprintresponseArray = {};
                            $scope.isPrintingPackReceipts = false;
                            _refreshBatchPrint();
                        },
                        function () {
                            $scope.isPrintingPackReceipts = false;
                            _refreshBatchPrint();
                        });
                    });

                };

            };

            $scope.formInput = {
                orderNo: '',
            };

            var handleBarCodeScan = function (event, barcodeData) {
                if (!angular.isString(barcodeData)) {
                    barcodeData = (!angular.isDefined(barcodeData) || barcodeData === null) ? '' : barcodeData.toString().trim();
                }

                //so highjump pack slips can be scanned as well truncate to 9 digits (chars)
                $scope.formInput.orderNo = barcodeData.substring(0, 9);
                $scope.openOrder();
            };

            var deregister = $scope.$on('Scanner_Event', handleBarCodeScan);

            $scope.$on("$destroy", function () {
                deregister();
            });

        }])
.controller('alternatePickAndPackCtrl', ['$scope', '$modal', 'order', '$rootScope', '$filter', '$q', '$state', 'sendSMTPErrorEmail', 'pickUpInStore', 'POSService', 'printerService', 'ngTableParams', '$interval',
        function ($scope, $modal, $order, $rootScope, $filter, $q, $state, $sendSMTPErrorEmail, $pickUpInStore, $POSService, $printerService, ngTableParams, $interval) {

            $scope.isPrintingPackReceipts = false;

            $scope.goBack = function () {
                $state.go('pickUpInStore');
            };

            //$scope.readyforCustomerShipmentresponseArray = [];

            //$scope.refreshIScroll = function () {
            //    setTimeout(function () {
            //        $scope.$parent.myScroll['wrapper'].refresh();
            //    }, 500);
            //};

            //$scope.myScrollOptions = { tap: true };

            //$scope.tableParams = new ngTableParams({
            //    page: 1,            // show first page
            //    count: 100000,          // count per page
            //    sorting: {
            //        "_orderNumber": "asc"    // initial sorting
            //    }
            //}, {
            //    total: $scope.readyforCustomerShipmentresponseArray.length, // length of data
            //    counts: [],
            //    defaultSort: "asc",
            //    getData: function ($defer, params) {
            //        var orderedData = params.sorting() ?
            //                            $filter('orderBy')($scope.readyforCustomerShipmentresponseArray, params.orderBy()) :
            //                            $scope.readyforCustomerShipmentresponseArray;

            //        $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            //        $scope.refreshIScroll();
            //    }
            //});


            $scope.openOrder = function () {
                if ($scope.formInput.orderNo) {
                    param = {
                        "GetSterlingOrderListReq":
                            {
                                "_ReadFromHistory": "N",
                                "_OrderNo": $scope.formInput.orderNo,
                                "_DraftOrderFlag": "N"
                            }
                    };
                    if (angular.isDefined($scope.formInput.orderNo)) {
                        $order.getOrderList(param, function (result) {
                            if (result === undefined) {
                                jQuery('.not-found-alert').modal('show');
                            } else {
                                $scope.openOrderDetail($scope.formInput.orderNo);
                            }
                        }, function (error) { alert(error); });
                    }
                }
            };
            $scope.batchprintresponseArray = {};
            $scope.batchLength = "--";

            var _refreshBatchPrint = function () {
                $scope.batchLength = "--";

                $pickUpInStore.getBatchPrintShipmentListArray($POSService.getPOSParameters().storeNumber).then(
                    function (map) {
                        $scope.batchprintresponseArray = map;
                        $scope.batchLength = "" + Object.keys($scope.batchprintresponseArray).length;
                    });
            };

            $interval(_refreshBatchPrint, 660000);

            _refreshBatchPrint();

            //$pickUpInStore.getReadyforCustomerShipmentListArray($POSService.getPOSParameters().storeNumber).then(function (arrayResult) {
            //    $scope.readyforCustomerShipmentresponseArray = arrayResult;
            //    $scope.tableParams.reload();
            //});

            $scope.openOrderDetail = function (orderNo) {
                $order.setSelectedOrder({ _OrderNo: orderNo, _DocumentType: "0001" });
                $state.go("orderDetail");
            }

            $scope.navigateToOrderSearchPage = function () {
                $state.go("orderSearch");
            }

            var deregisterPrinterErrorEvent = $rootScope.$on('Printer_Error', function (event, errorData) {
                swal({ title: "Alert!", text: errorData.StatusText, showConfirmButton: true });
            });

            $scope.$on('destroy', function () {
                deregisterPrinterErrorEvent();
            });

            $scope.navigateToPickListPage = function () {
                $state.go('pickingConfirm');
            }
            $scope.navigateToPackConfirmPage = function () {
                $state.go('packingConfirm');
            }


            $scope.printPendingReceipts = function (PrintType) {
                if (!$scope.isPrintingPackReceipts) {
                    $printerService.claimPrinter().then(function () { 
                        $scope.isPrintingPackReceipts = true;

                            var promiseArray = [];
                            for (var key in $scope.batchprintresponseArray) {
                                if ($scope.batchprintresponseArray.hasOwnProperty(key)) {
                                    var currentShipment = $scope.batchprintresponseArray[key];
                                    if (angular.isDefined(currentShipment) && angular.isDefined(currentShipment.ShipmentLines) && angular.isArray(currentShipment.ShipmentLines.ShipmentLine) && (currentShipment.ShipmentLines.ShipmentLine.length > 0)) {
                                        var OrderNo = currentShipment.ShipmentLines.ShipmentLine[0]._OrderNo;
                                
                                        //$order.setSelectedOrder({ _OrderNo: OrderNo, _DocumentType: "0001" });
                                        $printerService.printOrder(OrderNo, false, "BOPISPickupReceipt");
                                    
                                        promiseArray.push($pickUpInStore.changeShipment(currentShipment._ShipmentKey, $POSService.getPOSParameters.storeNumber));
                                    }
                                }
                            }
                            //First set batch print count to zero while we wait for refresh of count.
                            $scope.batchLength = "--";

                            $printerService.releasePrinter();

                            //refresh batch print
                            $q.all(promiseArray).then(function () {
                                $scope.batchprintresponseArray = {};
                                $scope.isPrintingPackReceipts = false;
                                _refreshBatchPrint();
                            },
                            function () {
                                $scope.isPrintingPackReceipts = false;
                                _refreshBatchPrint();
                            });
                    });
                };

            };

            $scope.formInput = {
                orderNo: '',
            };

            var handleBarCodeScan = function (event, barcodeData) {
                if (!angular.isString(barcodeData)) {
                    barcodeData = (!angular.isDefined(barcodeData) || barcodeData === null) ? '' : barcodeData.toString().trim();
                }

                //so highjump pack slips can be scanned as well truncate to 9 digits (chars)
                $scope.formInput.orderNo = barcodeData.substring(0, 9);
                $scope.openOrder();
            };

            var deregister = $scope.$on('Scanner_Event', handleBarCodeScan);

            $scope.$on("$destroy", function () {
                deregister();
            });

        }])
.controller('bopisReportCtrl', ['$scope', 'pickConfirmService',  'order',   '$q', '$state',  'pickUpInStore', 'POSService','ngTableParams', "$filter",
        function ($scope, $pickConfirmService,  $order,   $q, $state,  $pickUpInStore, $POSService, ngTableParams, $filter) {

            $scope.readyforCustomerShipmentresponseArray = [];
            
            $scope._loadingReadyForCustomer = false;
            $scope._loadingPickList = false;
            $scope._loadingPackList = false;
            $scope._loadingRestockList = false;

            $scope.openOrder = function (orderNo) {
                if (orderNo) {
                    param = {
                        "GetSterlingOrderListReq":
                            {
                                "_ReadFromHistory": "N",
                                "_OrderNo": orderNo,
                                "_DraftOrderFlag": "N"
                            }
                    };
                    if (angular.isDefined(orderNo)) {
                        $order.getOrderList(param, function (result) {
                            if (result === undefined) {
                                jQuery('.not-found-alert').modal('show');
                            } else {
                                $scope.openOrderDetail(orderNo);
                            }
                        }, function (error) { alert(error); });
                    }
                }
            };


            $scope.openOrderDetail = function (orderNo) {
                $order.setSelectedOrder({ _OrderNo: orderNo, _DocumentType: "0001" });
                $state.go("orderDetail");
            }

            $scope.refresh = function () {
                $scope._loadingReadyForCustomer = true;
                $scope._loadingPickList = true;
                $scope._loadingPackList = true;
                $scope._loadingRestockList = true;

                $scope.refreshReadyForCustomer();
                $scope.refreshPickList();
                $scope.refreshPackList();
                $scope.getRestockList();
            };

            $scope.refreshReadyForCustomer = function () {
                var storeNumber = $POSService.getPOSParameters().storeNumber;

                if (storeNumber === null || storeNumber === undefined) {
                    return;
                }

                storeNumber = storeNumber.toString();

                while (storeNumber.length < 3) {
                    storeNumber = "0" + storeNumber;
                }

                $pickUpInStore.getReadyforCustomerShipmentListArray(storeNumber).then(function (arrayResult) {
                    $scope.readyforCustomerShipmentresponseArray = arrayResult;
                    $scope._loadingReadyForCustomer = false;
                });
            };

            //*******************Pick List code

            $scope.orderToPickArr = [];

            $scope.refreshPickList = function () {

                var storeNumber = $POSService.getPOSParameters().storeNumber;

                if (storeNumber === null || storeNumber === undefined) {
                    return;
                }

                storeNumber = storeNumber.toString();

                while (storeNumber.length < 3) {
                    storeNumber = "0" + storeNumber;
                }

                $pickConfirmService.getUnpickedShipmentLines(storeNumber).then(function (rawShipmentLineArr) {
                    if (!angular.isArray(rawShipmentLineArr)) {
                        swal({
                            title: "Pick List Load Fail",
                            text: "Pick list Sterling Order Data Failed To Load.",
                            confirmButtonText: "OK"
                        });
                        return;
                    }

                    if (rawShipmentLineArr.length < 1) {
                        $scope._loadingPickList = false;
                        return;
                    }

                    $scope.orderToPickArr = $pickConfirmService.getPickingShipmentSummaryDisplay(rawShipmentLineArr);
                   
                    $scope._loadingPickList = false;
                    
                },
                function (data) {
                    swal({
                        title: "Pick List Load Fail",
                        text: "Pick list Sterling Order Data Failed To Load.",
                        confirmButtonText: "OK"
                    });
                    return data;
                }
                );
            };

            //***********************Pack / Confirm Code

            //**************Pack confirm list
            $scope.packList = [];

            $scope.tableParams = new ngTableParams({
                page: 1,            // show first page
                count: 100000,          // count per page
                sorting: {
                    "_display._OrderNo": "asc"    // initial sorting
                }
            }, {
                total: $scope.packList.length, // length of data
                counts: [],
                defaultSort: "asc",
                getData: function ($defer, params) {

                    var orderedData = params.sorting() ?
                                        $filter('orderBy')($scope.packList, params.orderBy()) :
                                        $scope.packList;

                    $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                }
            });

            if ($scope.packList.length > 0) {
                $scope.tableParams.reload();
            }



            $scope.refreshPackList = function () {

                var storeNumber = $POSService.getPOSParameters().storeNumber;

                if (storeNumber === null || storeNumber === undefined) {
                    $scope.isLoading = false;
                    return;
                }

                storeNumber = storeNumber.toString();

                while (storeNumber.length < 3) {
                    storeNumber = "0" + storeNumber;
                }

                $pickConfirmService.getUnConfirmedShipmentList(storeNumber).then(function (dictionaryShipmentNoToShipments) {
                    $scope.packList = [];

                    angular.forEach(dictionaryShipmentNoToShipments, function (value, key) {
                        //create display data
                        var orderno = null;
                        if (value.ShipmentLines && angular.isArray(value.ShipmentLines.ShipmentLine) && value.ShipmentLines.ShipmentLine.length > 0 && value.ShipmentLines.ShipmentLine[0]._OrderNo) {
                            orderno = parseInt(value.ShipmentLines.ShipmentLine[0]._OrderNo);

                            if (!isFinite(orderno)) {
                                orderno = null;
                            }
                        }

                        var displayName = null;
                        if (value.BillToAddress) {
                            if (angular.isString(value.BillToAddress._LastName)) {
                                displayName = value.BillToAddress._LastName.trim();
                            }
                            if (angular.isString(value.BillToAddress._FirstName)) {
                                displayName += ", " + value.BillToAddress._FirstName.trim();
                            }
                        }

                        var isReceiptPrinted = "No";
                        if (value.Extn && value.Extn._ExtnBatchPackSlipPrinted) {
                            if ((/^\s*Y\s*$/i).test(value.Extn._ExtnBatchPackSlipPrinted)) {
                                isReceiptPrinted = "Yes";
                            } else {
                                isReceiptPrinted = "No";
                            }
                        }


                        value._display = { _OrderNo: orderno, _CustomerName: displayName, _IsReceiptPrintedStr: isReceiptPrinted };

                        $scope.packList.push(value);
                    });

                    $scope.tableParams.reload();

                    $scope._loadingPackList = false;

                },
                function (data) {
                    swal({
                        title: "Pick List Load Fail",
                        text: "Pick list Sterling Order Data Failed To Load.",
                        confirmButtonText: "OK"
                    });
                    return data;
                }
                );
            };

            $scope.restockList = [];

            $scope.getRestockList = function () {
                var storeNumber = $POSService.getPOSParameters().storeNumber;

                if (storeNumber === null || storeNumber === undefined) {
                    return;
                }

                storeNumber = storeNumber.toString();

                while (storeNumber.length < 3) {
                    storeNumber = "0" + storeNumber;
                }

                $pickUpInStore.getListOfShipmentToRestock(storeNumber).then(function (shipmentArray) {
                    $scope.restockList = shipmentArray;

                    $scope._loadingRestockList = false;
                });
            };


            $scope.refresh();
        }]);