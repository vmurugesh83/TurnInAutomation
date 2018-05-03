angular.module('pickingConfirm', ['ui.bootstrap'])
.controller('pickingConfirmCtrl', ['$scope', '$state', 'pickConfirmService', 'POSService', '$modal', 'itemSearch', 'itemDetail', 'numberOnlyKeyPressValidator', 'loggerService', 'btProp',
    function ($scope, $state, $pickConfirmService, $POSService, $modal, $itemSearch, $itemDetail, $numberOnlyKeyPressValidator, $loggerService, $btProp) {
        $scope.isLoading = true;
        $scope.isZeroItemsToPick = false;
        $scope.numberValidator = $numberOnlyKeyPressValidator;

        $scope.refreshIScroll = function () {
            setTimeout(function () {
                $scope.$parent.myScroll['ItemDetailWrapper'].refresh();
            }, 500);
        };

        $scope.pickList = [];

        $scope.refreshPickList = function () {
            $scope.isLoading = true;
            $scope.isZeroItemsToPick = false;

            var storeNumber = $POSService.getPOSParameters().storeNumber;

            if (storeNumber === null || storeNumber === undefined) {
                $scope.isLoading = false;
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
                    $scope.isLoading = false;
                    return;
                }

                if (rawShipmentLineArr.length < 1) {
                    $scope.isLoading = false;
                    $scope.isZeroItemsToPick = true;
                    return;
                }

                $pickConfirmService.retrievePickingDisplayDetails(storeNumber, rawShipmentLineArr).then(
                    function (completeShipmentLines) {

                        completeShipmentLines = $pickConfirmService.fobSortShipmentLines(completeShipmentLines);

                        $scope.pickList.length = 0;

                        for (var i = 0; i < completeShipmentLines.length; i++) {
                            $scope.pickList.push(completeShipmentLines[i]);
                        }

                        $scope.isLoading = false;
                    }, function (data) {
                        $scope.isLoading = false;
                        swal({
                            title: "Pick List Load Fail",
                            text: "Pick list Solr Data Failed to Load.",
                            confirmButtonText: "OK"
                        });
                        return data;
                    }
                    );
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
       
        
        $scope.goBack = function () {
            $state.go('alternatePickAndPack');
        };
       
        $scope.print = function () {
            //iscroll to top of list else the top of the list gets cut off
            $scope.$parent.myScroll['ItemDetailWrapper'].scrollTo(0, 0);
            jQuery('#scrollTableHeader').css("transform", "translate3d(0px, 0px, 0px)");

            window.print();
        };


        $scope.confirmPick = function () {

            var shipmentLineToConfirm = {};
            var shortPickCount = 0; //if any entered input will short pick a shipment line, count

            //iterate through $scope.pickList and collect shipment Lines with input
            for (var i = 0; i < $scope.pickList.length; i++) {
                var currentLine = $scope.pickList[i];

                //if QtyPicked is String (not null, not undefined) AND has 1 to 5 digits
                if (angular.isString(currentLine.QtyPicked) && (/^\s*\d{1,5}\s*$/).test(currentLine.QtyPicked)) {
                    
                    //check input will parse to int and value is between 0 and Qty to pick
                    var intValue = parseInt(currentLine.QtyPicked);
                    if (isFinite(intValue) && intValue > -1  && intValue <= currentLine.QtyToPick) {
                        shipmentLineToConfirm[currentLine._shipmentLine._ShipmentLineKey] = currentLine;

                        //if line will be shorted
                        if (intValue < currentLine.QtyToPick) {
                            ++shortPickCount;
                        }

                    } else {
                        //invalid input stop confirm
                        swal({
                            title: "Pick Confirm Failed",
                            text: "Invalid input. Please update all red input boxes.",
                            confirmButtonText: "OK"
                        });
                        return;
                    }
                }
            }

            //give a simple dialog asking to confirm that you are shorting shipment lines?
            if (shortPickCount > 0) {
                swal({
                    title: "Confirm Short",
                    text: "You are shorting " + shortPickCount + " line" + ((shortPickCount > 1) ? "s." : "."),
                    showCancelButton: true,
                    confirmButtonText: "Confirm"
                },
                function (isConfirm) {
                    if (isConfirm) {
                        _asyncCompleteConfirmPick(shipmentLineToConfirm);
                    }
                });
            } else {
                _asyncCompleteConfirmPick(shipmentLineToConfirm);
            }
        };

        var _asyncCompleteConfirmPick = function (shipmentLineToConfirm) {

            //check if any lines have already been picked show modal of already picked lines
            var shipmentLineKeys = [];
            angular.forEach(shipmentLineToConfirm, function (value, key) {
                shipmentLineKeys.push(key);
            });

            if (shipmentLineKeys.length < 1) {
                swal({
                    title: "Pick Confirm Failed",
                    text: "No valid picks to confirm.",
                    confirmButtonText: "OK"
                });
                return;
            }

            var shipmentLinesAlreadyPicked = [];
            $pickConfirmService.validateShipmentLinesForPicking(shipmentLineKeys).then(
                function (shipmentLineKeyIsPickedDic) {

                    angular.forEach(shipmentLineToConfirm, function (value, key) {

                        if ((/^Y$/i).test(shipmentLineKeyIsPickedDic[key])) {
                            shipmentLinesAlreadyPicked.push(value);
                        }
                    });

                    if (shipmentLinesAlreadyPicked.length > 0) {
                        //TODO: open modal to show items that associate needs to put back on floor
                        {
                            var modalInstance = $modal
                                    .open({
                                        templateUrl: '/html/pickUpInStore/alreadyPickedModal.html',
                                        controller: 'alreadyPickedModCtrl',
                                        resolve: {
                                            shipmentLinesAlreadyPicked: function () {
                                                return shipmentLinesAlreadyPicked;
                                            }
                                        },
                                        size: 'lg'
                                    });

                            modalInstance.result.then(function (registryChange) {
                                //refresh pick list
                                $scope.refreshPickList();
                            });
                        };


                        

                    } else {
                        //change shipment lines
                        var arrayOfShipmentLines = [];
                        angular.forEach(shipmentLineToConfirm, function (value, key) { arrayOfShipmentLines.push(value); });
                        $pickConfirmService.confirmPicks(arrayOfShipmentLines).then(
                            function () {
                                swal({
                                    title: "Pick Confirm",
                                    text: "Pick confirmation completed.",
                                    confirmButtonText: "OK"
                                });

                                //once changed refresh pick list
                                $scope.refreshPickList();
                            },
                            function (message) {
                                swal({
                                    title: "Pick Confirm Failed",
                                    text: "Pick Confirmation Failed." + (angular.isString(message))? message: "" ,
                                    confirmButtonText: "OK"
                                });
                                $scope.refreshPickList();
                            }
                        );
                    }
                }
                );
        };


        //ui display red and yellow input boxes
        $scope.validatePickBounds = function (currentLine) {
            //if input is blank then it IS inside the picking bounds
            if((/^\s*$/).test(currentLine.QtyPicked)){
                return true;
            }
            var intValue = parseInt(currentLine.QtyPicked);
            if (isFinite(intValue) && intValue > -1 && intValue <= currentLine.QtyToPick) {
                return true;
            } else {
                return false;
            }
        };

        $scope.validatePickIsShorting = function (currentLine) {
            var intValue = parseInt(currentLine.QtyPicked);
            if (isFinite(intValue) && intValue > -1 && intValue < currentLine.QtyToPick) {
                return true;
            } else {
                return false;
            }
        };

        //inital Pick list load.
        $scope.refreshPickList();
    }])

.controller('alreadyPickedModCtrl', ['$scope', '$modalInstance', 'pickConfirmService', 'shipmentLinesAlreadyPicked', function ($scope, $modalInstance, $pickConfirmService, shipmentLinesAlreadyPicked) {
    $scope.refreshIScroll = function () {
        setTimeout(function () {
            $scope.$parent.myScroll['ModalAlreadyPickedWrapper'].refresh();
        }, 500);
    };

    $scope.pickList = $pickConfirmService.fobSortShipmentLines(shipmentLinesAlreadyPicked); 

    $scope.cancel = function () { $modalInstance.close(); };

}])
;