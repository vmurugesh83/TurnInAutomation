angular.module('packConfirm', ['ui.bootstrap'])
.controller('packConfirmCtrl', ['$scope', '$state', 'pickConfirmService', 'POSService', '$modal', 'itemSearch', 'itemDetail', 'numberOnlyKeyPressValidator', 'loggerService', 'btProp', 'ngTableParams', '$filter',
    function ($scope, $state, $pickConfirmService, $POSService, $modal, $itemSearch, $itemDetail, $numberOnlyKeyPressValidator, $loggerService, $btProp, ngTableParams, $filter) {

        $scope.isLoading = true;
        $scope.isZeroItemsToPack = false;
        $scope.numberValidator = $numberOnlyKeyPressValidator;

        //var myScroll = new IScroll('#wrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip', tap: true });
        //$scope.refreshIScroll = function () {
        //    setTimeout(function () {
        //        myScroll.refresh();
        //    }, 500);
        //};

        $scope.refreshIScroll = function () {
            setTimeout(function () {
                $scope.$parent.myScroll['wrapper'].refresh();
            }, 500);
        };

        $scope.myScrollOptions = {tap:true};
        //get items to Pack


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
                // use build-in angular filter
                console.log(params.sorting());
                console.log(params.orderBy());
                var orderedData = params.sorting() ?
                                    $filter('orderBy')($scope.packList, params.orderBy()) :
                                    $scope.packList;

                $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
                $scope.refreshIScroll();
            }
        });

        if ($scope.packList.length > 0) {
            $scope.tableParams.reload();
        }


        $scope.refreshPackList = function () {
            $scope.isLoading = true;
            $scope.isZeroItemsToPack = false;
            $scope.packList.length = 0;

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

                $scope.packList.length = 0;

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
                $scope.isLoading = false;

                $scope.tableParams.reload();
            },
            function (data) {
                $scope.isLoading = false;
                swal({
                    title: "Pick List Load Fail",
                    text: "Pick list Sterling Order Data Failed To Load.",
                    confirmButtonText: "OK"
                });
                return data;
            }
            );
        };
       
        var _remove = function (currentShipment) {

            for (var i = 0; i < $scope.packList.length; i++) {
                if ($scope.packList[i]._ShipmentKey == currentShipment._ShipmentKey) {
                    $scope.packList.splice(i, 1);
                    $scope.tableParams.reload();
                    break;
                }
            }
        };

        $scope.openConfirmOrderModal = function (currentShipment) {
            var modalInstance = $modal
                .open({
                    templateUrl: 'html/pickUpInStore/packConfirmModal.html',
                    controller: 'packConfirmModalCtrl',
                    size:"lg",
                    resolve: {
                        currentShipment: function () {
                            return currentShipment;
                        }
                    }
                });
            modalInstance.result.then(function (result) {
                if (result === true) {
                    _remove(currentShipment);
                }
            });
        };
        
        $scope.goBack = function () {
            $state.go('alternatePickAndPack');
        };

        $scope.formInput = {
            orderNo: '',
        };

        $scope.tryOpenOrder = function (orderNo) {

            //if currently loading orders to confirm, ignore scan or entry.
            if ($scope.isLoading) {
                return;
            }

            var isOrderFound = false;
            var inputStr = "";

            if (angular.isString(orderNo)) {
                inputStr = orderNo.trim();
            }

            //order number must be 9 numbers only
            if (/^\d{9}$/.test(inputStr)) {
                var numInputOrderNumber = parseInt(inputStr);

                for (var i = 0; i < $scope.packList.length; i++) {
                    if ($scope.packList[i]._display._OrderNo == numInputOrderNumber) {
                        isOrderFound = true;
                        $scope.openConfirmOrderModal($scope.packList[i]);
                        break;
                    }
                }
            }

            if (!isOrderFound) {
                $scope.formInput.orderNo = "";
                var errorText = "";

                if (inputStr.length > 0) {
                    errorText = "Order: " + inputStr + " is not in the correct state for pack confirmation.";
                } else {
                    errorText = "This order is not in the correct state for pack confirmation.";
                }

                swal({
                    title: "Cannot Pack Confirm Order",
                    text: errorText,
                    confirmButtonText: "OK"
                });
            }
        };

        var handleBarCodeScan = function (event, barcodeData) {
            if (!angular.isString(barcodeData)) {
                barcodeData = (!angular.isDefined(barcodeData) || barcodeData === null) ? '' : barcodeData.toString().trim();
            }

            //so highjump pack slips can be scanned as well truncate to 9 digits (chars)
            $scope.formInput.orderNo = barcodeData.substring(0, 9);
            $scope.tryOpenOrder($scope.formInput.orderNo);
        };

        var deregister = $scope.$on('Scanner_Event', handleBarCodeScan);

        $scope.$on("$destroy", function () {
            deregister();
        });


        //inital Pick list load.
        $scope.refreshPackList();
    }])

.controller('packConfirmModalCtrl', ['$scope', '$modalInstance', 'pickConfirmService', 'currentShipment',
    function ($scope, $modalInstance, $pickConfirmService, currentShipment) {
        $scope.currentShipment = currentShipment;
        $scope.currentShipmentLines = [];

        var gatherShipmentLineData = function () {

            if ($scope.currentShipment && $scope.currentShipment.ShipmentLines && angular.isArray($scope.currentShipment.ShipmentLines.ShipmentLine)) {
                
                if ($scope.currentShipment.ShipmentLines.ShipmentLine.length > 0) {
                    $pickConfirmService.retrievePackDisplayDetails($scope.currentShipment.ShipmentLines.ShipmentLine).then(function (displayDataArray) {
                        for (var i = 0; i < displayDataArray.length; i++) {

                            $scope.currentShipmentLines.push(displayDataArray[i]);
                        }

                        $scope.refreshIScroll();
                    });
                }
            }  
        };

        gatherShipmentLineData();
        

    $scope.refreshIScroll = function () {
        setTimeout(function () {
            $scope.$parent.myScroll['ItemDetailWrapper'].refresh();
        }, 500);
    };


    $scope.cancel = function () { $modalInstance.dismiss(); };
    $scope.successClose = function () { $modalInstance.close(true); };

    $scope.confirm = function () {
        $pickConfirmService.confirmShipmentPacking(currentShipment).then(
            function (successMessage) {
                $scope.successClose();

                swal({ title: "Order Confirmed", text: "", timer: 1500, showConfirmButton: false });
            },
            function (errorMessage) {

                swal({ title: "Order Confirm Failed", text: ""});
            }
            );
    };

}])
;