var appServices = angular.module('appServicesWebSocket', []);
/*
 *   WebSoketService factory is used to communicate POS devices via Websocket protocol. 
 *   UPC scanner, CC communication occur via websocket. Any event raised by POS propagated via broadcast.
 */
appServices.factory('webSocketService', ['$rootScope', '$interval', '$log', 'sendPmmErrorEmail', '$timeout', function ($rootScope, $interval, $log, sendPmmErrorEmail, $timeout) {
    var webSocketCheckInterval = 20000;
    var ws;
    var webSocketErrorCount = 0;
    var reportInterval = 15; // 5 minutes starting at three.
    var firstFailure = null;
    var isResponseFromPmmInfoCall = false;

    function initWebSocket() {
        if (angular.isUndefined(ws) || ws.readyState > 1) {
            ws = new WebSocket(POSURL);
            ws.onerror = function (event) {
                webSocketErrorCount += 1;
                $log.error(event);

                if ((webSocketErrorCount - reportInterval) == 0) {

                    reportInterval = parseInt(reportInterval * 1.5);

                    if (firstFailure === null) {
                        firstFailure = new Date();
                    }

                    var currentDate = new Date();

                    sendPmmErrorEmail("WebSocket will not connect to PMM. Failed " + webSocketErrorCount + " times. \n \n   First failure at: \n      " + firstFailure.toString() + "\n \n   This failure at: \n      " + currentDate.toString(), "Websocket Connection with PMM");
                }
            }
            ws.onopen = function (event) {
                webSocketErrorCount = 0;
                $rootScope.$broadcast('WS_Open_Event', event.eventData);
                requestPosConfig();
            };
            ws.close = function (event) {
                $rootScope.$broadcast('WS_Close_Event', event.eventData);
            };
            ws.onmessage = function (event) {
                var eventData = angular.fromJson(event.data);
                if (eventData.BarcodeData) {
                    $rootScope.$broadcast('Scanner_Event', eventData.BarcodeData, eventData);
                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Data') {
                    $rootScope.$broadcast('MSR_Event', eventData);
                    $rootScope.$broadcast('MSR_Event_Disabled', eventData);//MSR always disabled after data returned

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Scanner' && eventData.ResponseType == 'Success' && (/^SCN_SCS_CLAIMED$/i).test(eventData.StatusCode)) {
                    $rootScope.$broadcast('Scanner_Event_Claim_Success', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Scanner' && eventData.ResponseType == 'Error' && (/^SCN_ERR_CLAIMED$/i).test(eventData.StatusCode)) {
                    $rootScope.$broadcast('Scanner_Event_Error_Already_Claimed', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Scanner' && eventData.ResponseType == 'Error' && (/^SCN_ERR_NOT_CLAIMED$/i).test(eventData.StatusCode)) {
                    $rootScope.$broadcast('Scanner_Event_Error_Not_Claimed', eventData);

                }  else if (eventData.MessageType == 'Response' && eventData.Device == 'Scanner' && eventData.ResponseType == 'Error' && (/^(SCN_ERR_INVALID_BARCODE|SCN_ERR_EXPIRED|SCN_ERR_INVALID_STORE)$/i).test(eventData.StatusCode)) {
                    $rootScope.$broadcast('Scanner_Event_Error_Temp_PLCC_Error', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Success' && (/^MSR_SCS_CLAIMED$/i).test(eventData.StatusCode)) {
                    $rootScope.$broadcast('MSR_Event_Claim_Success', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Error' && (/^MSR_ERR_CLAIMED$/i).test(eventData.StatusCode)) {
                    $rootScope.$broadcast('MSR_Event_Error_Already_Claimed', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Error' && (/^MSR_ERR_NOT_CLAIMED$/i).test(eventData.StatusCode)) {
                    $rootScope.$broadcast('MSR_Event_Error_Not_Claimed', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Error') { //all other MSR errors
                    $rootScope.$broadcast('MSR_Event_Error', eventData);
                    $rootScope.$broadcast('MSR_Event_Disabled', eventData);//MSR always disabled after error

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Success' && ((/^MSR_SCS_DISABLED$/i).test(eventData.StatusCode) || (/MSR successfully disabled/i).test(eventData.StatusText))) {
                    //swal({ title: "MSR Disable Success", text: "MSR Disable Success", showConfirmButton: true });
                    $rootScope.$broadcast('MSR_Event_Disabled', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Success' && ((/^MSR_SCS_ENABLED$/i).test(eventData.StatusCode) || (/.*(MSR successfully enabled|payment terminal successfully enabled).*/i).test(eventData.StatusText))) {

                    $rootScope.$broadcast('MSR_Event_Enabled', eventData);

                }
                else if (eventData.MessageType == 'Response' && eventData.Device == 'Printer' && eventData.ResponseType == 'Error') {

                    $rootScope.$broadcast('Printer_Error', eventData);

                    //skip cover is open and receipt paper empty
                    if (!(/.*Printer\s*error:\s*\(114\).*/i).test(eventData.StatusText)) {
                        sendPmmErrorEmail("PMM Printer Error.", "PMM Printing Error", eventData);
                    }

                    // swal({ title: "Printer Message Error", text: JSON.stringify(eventData, null, 2), showConfirmButton: true });

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Printer' && eventData.ResponseType == 'Data' && (/^PRT_SCS_PRINTED$/i).test(eventData.StatusCode)) {


                    // swal({ title: "Printer Message Nonerror", text: JSON.stringify(eventData, null, 2), showConfirmButton: true });

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Server' && eventData.ResponseType == 'Data') {
                    isResponseFromPmmInfoCall = true;
                    $rootScope.$broadcast('Config_Event', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Server' && eventData.ResponseType == 'Error') {
                    $rootScope.$broadcast('Config_Error', eventData);
                } else if (eventData.MessageType == 'Response' && eventData.Device == 'PaymentTerminal' && eventData.ResponseType == 'Error') {
                    $rootScope.$broadcast('PaymentTerminal_Event_Error', eventData);
                }
                else if (eventData.MessageType == 'Response' && eventData.Device == 'PaymentTerminal' && eventData.ResponseType == 'Success' && ((/^PMT_SCS_SIGNATURE$/i).test(eventData.StatusCode) || (/.*Signature captured successfully.*/i).test(eventData.StatusText))) {
                    $rootScope.$broadcast('PMT_SCS_SIGNATURE', eventData);
                } else if (eventData.MessageType == 'Response' && eventData.Device == 'PaymentTerminal' && eventData.ResponseType == 'Success' && ((/^PMT_SCS_ENABLED$/i).test(eventData.StatusCode) || (/.*The payment terminal was successfully enabled.*/i).test(eventData.StatusText))) {
                    $rootScope.$broadcast('PMT_SCS_ENABLED', eventData);
                } else if (eventData.MessageType == 'Response' && eventData.Device == 'PaymentTerminal' && eventData.ResponseType == 'Success' && ((/^PMT_SCS_CLAIMED$/i).test(eventData.StatusCode) || (/.*The payment terminal was successfully claimed.*/i).test(eventData.StatusText))) {
                    $rootScope.$broadcast('PMT_SCS_CLAIMED', eventData);
                } 
            };
        }
    }
    
    var requestPosConfig = function () {
        try {

            $timeout(function () {
                if (!isResponseFromPmmInfoCall) {
                    $rootScope.$broadcast('Config_Error', {});
                    sendPmmErrorEmail("PMM information / configuration call did not return within 45 seconds.", "Websocket Connection PMM Config Call Error");
                }
             }, 45000);

            ws.send(angular.toJson({
                "MessageType": "Request",
                "Device": "Server",
                "Command": "Configure",
                "Info": "true"
            }));
        } catch (e) {
            $rootScope.$broadcast('WS_Config_Request_Error');
        }
    };

    $rootScope.$on('WS_Get_Config', requestPosConfig);

    initWebSocket();
    $interval(function () {
        initWebSocket();
    }, webSocketCheckInterval);

    return {
        send: function (message) { 
            try {
                ws.send(message);
            } catch (ex) {
                console.log("WebSocket Send error:" + ex );
            }
        },
        readyState: function () {
            var value = 3;
            try{
                value = ws.readyState;
            } catch (e) {

                value = 3;
            }

            return value;
        }
    };
}])

.factory('scannerService', ['$rootScope', '$interval', '$log', 'itemSearch', '$location', 'webSocketService', 'orderCart', 'itemDetail', 'errorObj', 'loggerService', function ($rootScope, $interval, $log, $itemSearch, $location, ws, orderCart, $itemDetail, errorObj, $loggerService) {

    var isClaimed = false;
    var claimIntervalPromise = null;

    //on claim success, set isClaimed
    $rootScope.$on('Scanner_Event_Claim_Success', function (event, data) {
        isClaimed = true;
        if (claimIntervalPromise !== null) {
            $interval.cancel(claimIntervalPromise);
            claimIntervalPromise = null;
        }

        scannerFunctions.enableScanner();
    });

    //scanner already claimed, turn off claim calls
    $rootScope.$on('Scanner_Event_Error_Already_Claimed', function (event, data) {
        isClaimed = true;
        if (claimIntervalPromise !== null) {
            $interval.cancel(claimIntervalPromise);
            claimIntervalPromise = null;
        }
    });

    //scanner not claimed, retry claiming
    $rootScope.$on('Scanner_Event_Error_Not_Claimed', function (event, data) {
        isClaimed = false;
        startIntervalClaim();
    });

    var claimScanner = {
        "MessageType": "Request",
        "Device": "Scanner",
        "Command": "Claim"
    };

    var releaseScanner = {
        "MessageType": "Request",
        "Device": "Scanner",
        "Command": "Release"
    };

    var enableScanner = {
        "MessageType": "Request",
        "Device": "Scanner",
        "Command": "Enable"
    };

    var disableScanner = {
        "MessageType": "Request",
        "Device": "Scanner",
        "Command": "Disable"
    };


    var stopServer = {
        "MessageType": "Request",
        "Device": "Server",
        "Command": "Configure",
        "Stop": "true",
    };

    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        //Controller where the scanner will be claimed.
        var controllers = ['indexCtrl', 'homeCtrl', 'itemSearchCtrl', 'orderCartCtrl', 'paymentSummaryCtrl', 'couponCtrl', 'orderSearchCtrl', 'pickUpInStoreCtrl', 'packConfirmCtrl'];
        scannerEventUnregisterWrapper();
        try {
            scannerFunctions.disableScanner();
            if (toState.views != null && controllers.indexOf(toState.views['main@'].controller) != -1) {
                //set Event handlers
                setEventHandlers(toState.views['main@'].controller);
                scannerFunctions.enableScanner();
            }
        } catch (err) {
            $loggerService.log(err);
        }
    });

    var eventHandlersByController = {
        'orderCartCtrl': {
            scannerEventHandler: function (event, upc, isBigTicketValidated) {
                $itemSearch.searchUPC(upc, function (response) {
                    if (response.ngroups == 0) {
                        jQuery('.not-found-alert').modal('show');
                    } else if (response.isnGroup.length == 1) {

                        var temp = [];
                        for (var i = 0; i < response.isnGroup[0].productList.length; i++) {
                            if (parseInt(response.isnGroup[0].productList[i].id) == parseInt(upc)) {
                                temp.push(angular.copy(response.isnGroup[0].productList[i]));
                                break;
                            }
                        }
                        response.isnGroup[0].productList = temp;
                        //get price (inventory is part of item search currently)
                        if (temp.length > 0) {
                            $itemDetail(response.isnGroup[0], function (isnGroup) {
                                try {
                                    orderCart.orderLine.addOrderLine(isnGroup.productList[0], 1, isBigTicketValidated);
                                    $rootScope.$broadcast('scannerAddUpcClearInput');
                                } catch (error) {
                                    error.itemObject = isnGroup.productList[0];
                                    $rootScope.$broadcast('scannerAddUpcClearInput');
                                    $rootScope.$broadcast('scannerAddUpcError', error);
                                }
                            });
                        } else {
                            $rootScope.$broadcast('scannerAddUpcError', errorObj.newError('Item Not Found', "UPC: " + upc + " could not be found.", "UPC: " + upc + " could not be found.", '', 'WARN'));
                        }
                    } else {
                        $location.path("/itemResults");
                    }
                }, function (err) {
                    alert(err);
                });
            }
        }
    };

    var scannerEventUnregisterWrapper = function () {
        scannerEventUnregister();
        scannerEventUnregister = function () { };
    };
    var currentScannerEventHandler = function () { };
    var scannerEventUnregister = function () { };

    var setEventHandlers = function (controllerName) {
        if (angular.isDefined(eventHandlersByController[controllerName])) {
            //check for alternate scanner  event handler
            if (angular.isFunction(eventHandlersByController[controllerName].scannerEventHandler)) {
                currentScannerEventHandler = eventHandlersByController[controllerName].scannerEventHandler;

                scannerEventUnregister = $rootScope.$on('Scanner_Event', currentScannerEventHandler);

            }
        }

    };


    $rootScope.$on('WS_Open_Event', function (event) {
        scannerFunctions.claimScanner();
        scannerFunctions.enableScanner();
    });

    $rootScope.$on('WS_Close_Event', function (event) {
        scannerFunctions.releaseScanner();
    });


    var scannerFunctions = {
        claimScanner: function () {
            try {
                ws.send(angular.toJson(claimScanner));
            } catch (err) {
                $log.error(err.message);
            }
        },
        releaseScanner: function () {
            try {
                ws.send(angular.toJson(releaseScanner));
            } catch (err) {
                $log.error(err.message);
            }
        },

        enableScanner: function () {
            try {
                ws.send(angular.toJson(enableScanner));
            } catch (err) {
                $log.error(err.message);
            }
        },

        disableScanner: function () {
            try {
                ws.send(angular.toJson(disableScanner));
            } catch (err) {
                $log.error(err.message);
            }
        },
        stopServer: function () {
            try {
                ws.send(angular.toJson(stopServer));
                ws.close();
            } catch (err) {
                $log.error(err.message);
            }
        }
    }

    // call for Scanner claim every 10 seconds until success or 20 tries....
    var startIntervalClaim = function () {
        if (claimIntervalPromise !== null) {
            $interval.cancel(claimIntervalPromise);
            claimIntervalPromise = null;
        }

        claimIntervalPromise = $interval(
            function () {
                if (ws.readyState() == 1) {
                    scannerFunctions.claimScanner();
                }
            }, 10000, 20);
    };

    startIntervalClaim();

    return scannerFunctions;
}])
    .factory('msrService', ['$rootScope', '$interval', '$log', '$location', 'webSocketService', '$q', '$timeout', function ($rootScope, $interval, $log, $location, ws, $q, $timeout) {

        var isEnabled = false;
        var lastEnableDisableDateTime = new Date();

        var isClaimed = false;
        var claimIntervalPromise = null;

        var timeoutEnablePromise = null;
        var timeoutDisablePromise = null;

        //on claim success, set isClaimed
        $rootScope.$on('MSR_Event_Claim_Success', function (event, data) {
            isClaimed = true;
            if (claimIntervalPromise !== null) {
                $interval.cancel(claimIntervalPromise);
                claimIntervalPromise = null;
            }
        });

        //MSR already claimed, turn off claim calls
        $rootScope.$on('MSR_Event_Error_Already_Claimed', function (event, data) {
            isClaimed = true;
            if (claimIntervalPromise !== null) {
                $interval.cancel(claimIntervalPromise);
                claimIntervalPromise = null;
            }
        });

        //MSR not claimed, retry claiming
        $rootScope.$on('MSR_Event_Error_Not_Claimed', function (event, data) {
            isClaimed = false;
            startIntervalClaim();
        });

        
        var setIsEnabled = function (enable, dateTimeString) {
            var dateTime = null;

            if (enable === null || enable === undefined) {
                return;
            }

            if (angular.isString(dateTimeString)) {
                dateTime = new Date(dateTimeString);

            }


            if (dateTime && (dateTime >= lastEnableDisableDateTime)) {
                lastEnableDisableDateTime = dateTime;
                isEnabled = enable;
            } else if (dateTime === null) {
                isEnabled = enable;
            }
            //else {
            //        swal({ title: "MSR Out of Order", text: "Tried: " + enable + " @ " + dateTime.toString() + " *** Was: " + isEnabled + " @ " + lastEnableDisableDateTime.toString(), showConfirmButton: true });
            //}
        };

        var claimMSR = {
            "MessageType": "Request",
            "Device": "MSR",
            "Command": "Claim"
        };

        var releaseMSR = {
            "MessageType": "Request",
            "Device": "MSR",
            "Command": "Release"
        };

        var enableMSR = function (cardType) {
            var tenderType = cardType ? cardType : "BankCard";
            return {
                "MessageType": "Request",
                "Device": "MSR",
                "Command": "Enable",
                "TenderType": tenderType
            }
        };

        var disableMSR = {
            "MessageType": "Request",
            "Device": "MSR",
            "Command": "Disable"
        };

        $rootScope.$on('WS_Close_Event', function (event) {
            MSRFunctions.releaseMSR();
        });

        var lastCard = undefined;
        var MSRFunctions = {
            claimMSR: function () {
                try {
                    ws.send(angular.toJson(claimMSR));
                } catch (err) {
                    $log.error(err.message);
                }
            },
            releaseMSR: function () {
                try {
                    ws.send(angular.toJson(releaseMSR));
                } catch (err) {
                    $log.error(err.message);
                }
            },
            enableMSR: function (cardType) {

                var defer = $q.defer();

                if (isEnabled) {
                    defer.reject();
                } else {

                    //setIsEnabled(true);

                    //on success message
                    var deregister = $rootScope.$on('MSR_Event_Enabled', function (event, data) {
                        if (timeoutEnablePromise !== null) {
                            $timeout.cancel(timeoutEnablePromise);
                            timeoutEnablePromise = null;
                        }
                        setIsEnabled(true, data.Timestamp);
                        deregister();
                        deregisterForErrors();
                        defer.resolve(cardType);
                    });

                    var deregisterForErrors = $rootScope.$on('MSR_Event_Error', function (event, data) {
                        
                        if ((/^(MSR_ERR_NOT_FUNCTIONAL|MSR_ERR_NOT_CLAIMED|MSR_ERR_NOT_ENABLED|MSR_ERR_ENABLE|MSR_ERR_UNEXPECTED)$/i).test(data.StatusCode)) {
                            deregister();
                            deregisterForErrors();
                            setIsEnabled(false);
                            MSRFunctions.disableMSR(true);
                            defer.reject(data);
                        } else if ((/^MSR_ERR_ENABLED$/i).test(data.StatusCode)) {
                            setIsEnabled(true, data.Timestamp);
                            deregister();
                            deregisterForErrors();
                            defer.resolve(cardType);

                        }

                        if (timeoutEnablePromise !== null) {
                            $timeout.cancel(timeoutEnablePromise);
                            timeoutEnablePromise = null;
                        }
                    });


                    try {
                        if (ws.readyState() == 1) {
                            ws.send(angular.toJson(enableMSR(cardType)));
                            timeoutEnablePromise = $timeout(function () {
                                deregister();
                                deregisterForErrors();
                                setIsEnabled(false);
                                $rootScope.$broadcast('MSR_Event_Error', { StatusText: "Timeout. Cannot enable MSR.", StatusCode: "LUFI_MSR_TIMEOUT" });
                                defer.reject({ StatusText: "Timeout. Cannot enable MSR.", StatusCode: "LUFI_MSR_TIMEOUT" });
                            }, 11000);
                        }
                        else {
                            deregister();
                            deregisterForErrors();
                            setIsEnabled(false);
                            $rootScope.$broadcast('MSR_Event_Error', { StatusText: "WebSocket to MSR communication is down.", StatusCode: "LUFI_MSR_WEBSOCKET_DOWN" });
                            defer.reject({ StatusText: "WebSocket to MSR communication is down.", StatusCode: "LUFI_MSR_WEBSOCKET_DOWN" });
                        }
                        

                    } catch (err) {
                        deregister();
                        deregisterForErrors();
                        setIsEnabled(false);
                        defer.reject(err);
                        $log.error(err.message);
                    }
                }

                return defer.promise;
            },

            disableMSR: function (isIgnoreStateSendDisable) {

                var defer = $q.defer();

                if (isEnabled || isIgnoreStateSendDisable) {

                    //clear enable timeout
                    if (timeoutEnablePromise !== null) {
                        $timeout.cancel(timeoutEnablePromise);
                        timeoutEnablePromise = null;
                    }

                    //on success message
                    var deregister = $rootScope.$on('MSR_Event_Disabled', function (event, data) {

                        var delayResolve = function () {
                            if (timeoutDisablePromise !== null) {
                                $timeout.cancel(timeoutDisablePromise);
                                timeoutDisablePromise = null;
                            }
                            
                            setIsEnabled(false, data.Timestamp);
                            deregister();
                            defer.resolve();
                        };

                        delayResolve();
                        // $timeout(delayResolve, 5000); //delay half a second for retalix to fully disable

                    });


                    try {
                        if (ws.readyState() == 1) {
                            ws.send(angular.toJson(disableMSR));
                            timeoutDisablePromise = $timeout(function () {
                                deregister();
                                setIsEnabled(false);
                                defer.reject({ StatusText: "Timeout disable of MSR.", StatusCode: "LUFI_MSR_TIMEOUT" });
                            }, 7000);
                        }
                        else {
                            deregister();
                            setIsEnabled(false);
                            defer.reject({ StatusText: "WebSocket to MSR communication is down.", StatusCode: "LUFI_MSR_WEBSOCKET_DOWN" });
                        }
                } catch (err) {
                        deregister();
                        setIsEnabled(false);
                        defer.reject(err);
                    $log.error(err.message);
                }
            } else {
                defer.resolve();
            }

            return defer.promise;
        },
        isEnabled: function () { return isEnabled; },
        lastCard: lastCard
    }


        $rootScope.$on('MSR_Event_Disabled', function (event, data) {
            setIsEnabled(false, data.Timestamp);
        });

        // call for MSR claim every 10 seconds until success or 20 tries....
        var startIntervalClaim = function () {
            if (claimIntervalPromise !== null) {
                $interval.cancel(claimIntervalPromise);
                claimIntervalPromise = null;
            }

            claimIntervalPromise = $interval(
            function ()
            {
                if (ws.readyState() == 1) {
                    MSRFunctions.claimMSR();
                }
            }, 10000, 20);

            //send 1st request immediately
            if (ws.readyState() == 1) {
                MSRFunctions.claimMSR();
            }
        };

        startIntervalClaim();

        return MSRFunctions;
    }])
        .factory('printerService', ['$rootScope', '$interval', '$log', '$location', 'webSocketService', '$q', function ($rootScope, $interval, $log, $location, ws, $q) {
            var claimPrinter = {
                "MessageType": "Request",
                "Device": "Printer",
                "Command": "Claim"
            };

            var releasePrinter = {
                "MessageType": "Request",
                "Device": "Printer",
                "Command": "Release"
            };

            var printOrder = function (orderNumber, retry, PrintType) {
                return {
                    "MessageType": "Request",
                    "Device": "Printer",
                    "Command": "Print",
                    "OrderNumber": orderNumber,
                    "Retry": retry,
                    "ReceiptType": PrintType
                }
            };


            $rootScope.$on('WS_Close_Event', function (event) {
                MSRFunctions.releaseMSR();
            });

            var lastCard = undefined;
            var printerFunctions = {
                claimPrinter: function () {
                    var defer = $q.defer();

                    try {

                        if (ws.readyState() == 1) {
                            ws.send(angular.toJson(claimPrinter));
                            defer.resolve();
                        } else {
                            var eventData = {
                                MessageType: 'Response',
                                Device: 'Printer',
                                ResponseType: 'Error',
                                StatusText: 'WebSocket communication is down. Claim of Printer failed.'
                            };

                            $rootScope.$broadcast('Printer_Error', eventData);
                            defer.reject();
                        }
                    } catch (err) {
                        var eventData = {
                            MessageType: 'Response',
                            Device: 'Printer',
                            ResponseType: 'Error',
                            StatusText: 'WebSocket communication is down. Claim of Printer failed.'
                        };

                        $rootScope.$broadcast('Printer_Error', eventData);
                        $log.error(err.message);
                    }

                    return defer.promise;
                },
                releasePrinter: function () {
                    try {
                        ws.send(angular.toJson(releasePrinter));
                    } catch (err) {
                        $log.error(err.message);
                    }
                },
                printOrder: function (orderNumber, retry,PrintType) {
                    try {
                        if (ws.readyState() == 1) {
                            ws.send(angular.toJson(printOrder(orderNumber, retry,PrintType)));
                        } else {
                            var eventData = {
                                MessageType: 'Response',
                                Device: 'Printer',
                                ResponseType: 'Error',
                                StatusText: 'WebSocket communication is down. Claim of Printer failed.'
                            };

                            $rootScope.$broadcast('Printer_Error', eventData);
                        }
                    } catch (err) {

                        var eventData = {
                            MessageType: 'Response',
                            Device: 'Printer',
                            ResponseType: 'Error',
                            StatusText: 'WebSocket communication is down. Printing of Printer failed.'
                        };

                        $rootScope.$broadcast('Printer_Error', eventData);

                        $log.error(err.message);
                    }
                }
            }

            return printerFunctions;
        }])

.factory('defaultUPCScanHandler', ['itemSearch', '$location', function ($itemSearch, $location) {
    return function (event, upc) {
        $itemSearch.searchUPC(upc, function (response) {
            if (response.ngroups == 0) {
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
}])

    .factory('POSLogSocketService', ['webSocketService', function (ws) {
        var POSLogMessage = function (logText) {
            return {
                "MessageType": "Request",
                "Device": "Server",
                "Command": "Configure",
                "LogText": logText
            }
        };

        var POSLogFunctions = {
            logToPOS: function (logText) {
                try {
                    ws.send(angular.toJson(POSLogMessage(logText)));
                } catch (err) {
                    console.error(err.message);
                }
            }
        };
        return POSLogFunctions;
    }])

 .factory('PaymentTerminalService', ['webSocketService','$rootScope', '$q', '$timeout', function (ws,$rootScope, $q, $timeout) {
     var claimPaymentTerminal = {
         "MessageType": "Request",
         "Device": "PaymentTerminal",
         "Command": "Claim"
     };

     var releasePaymentTerminal = {
         "MessageType": "Request",
         "Device": "PaymentTerminal",
         "Command": "Release"
     };

     var PaymentTerminalSigCapReq = function(OrderNo,AssociateID) {
         return {
             "MessageType": "Request",
             "Device": "PaymentTerminal",
             "Command": "CollectSignature",
             "AssociateID": AssociateID,
             "OrderNumber": OrderNo
        }
     };

     
     

     $rootScope.$on('WS_Close_Event', function (event) {
         paymentTerminalFunctions.releasePaymentTerminal();
     });
     var timeoutPromise = null;

     var claimPayment = function () {

         var defer = $q.defer();

         

         var deregisterHandlers = function () {
             if (timeoutPromise != null) {
                 $timeout.cancel(timeoutPromise);
                 timeoutPromise = null;
             }

             unregEventSucc();
             unregEventError();
         };

         var success = function () {
             deregisterHandlers();
             defer.resolve();
         };

         var failed = function (event, data) {
             //take already claimed and already enabled as claim success...
             if ((/^(PMT_ERR_CLAIMED|PMT_ERR_ENABLED)$/i).test(data.StatusCode)) {
                 success();
             } else {
                 deregisterHandlers();
                 defer.reject(data);
             }
         };


         var unregEventSucc = $rootScope.$on('PMT_SCS_CLAIMED', success);
         var unregEventError = $rootScope.$on('PaymentTerminal_Event_Error', failed);

         try {
             if (ws.readyState() == 1) {

                 ws.send(angular.toJson(claimPaymentTerminal));
                 timeoutPromise = $timeout(function () {
                     failed(null, { "StatusText": "Timeout. Claim of Signature Capture failed.", StatusCode:"LUFI_BOPIS_TIMEOUT" });
                 }, 22000);
             } else {
                 failed(null, { "StatusText": "WebSocket communication is down. Claim of Signature Capture failed.", StatusCode: "LUFI_BOPIS_WEBSOCKET_DOWN" });
             }
        } catch (err) {
            failed();
            $log.error(err.message);
        }

        return defer.promise;
     };


     var sigCapTimeoutPromise = null;

     var getSignature = function (orderNumber, AssociateID) {

         var defer = $q.defer();

         var deregisterHandlers = function () {
             if (sigCapTimeoutPromise != null) {
                 $timeout.cancel(sigCapTimeoutPromise);
                 sigCapTimeoutPromise = null;
             }
             unregEventSucc();
             unregEventError();
         };

         var success = function () {
             deregisterHandlers();
             defer.resolve();
         };

         var failed = function (event, data) {
             //take already claimed and already enabled skip and wait for Sig Capture success
             if (!(/^(PMT_ERR_CLAIMED|PMT_ERR_ENABLED|PMT_ERR_RELEASE)$/i).test(data.StatusCode)) {
                 deregisterHandlers();
                 defer.reject(data);
             }
         };


         var unregEventSucc = function () { };
         var unregEventError = function () { };

         var sendRequest = function () {

             unregEventSucc = $rootScope.$on('PMT_SCS_SIGNATURE', success);
             unregEventError = $rootScope.$on('PaymentTerminal_Event_Error', failed);

             try {
                 var cleanAssociateId = AssociateID ? AssociateID : "0";

                 if (orderNumber !== undefined && orderNumber !== null) {
                     ws.send(angular.toJson(PaymentTerminalSigCapReq(orderNumber, cleanAssociateId)));
                     sigCapTimeoutPromise = $timeout(function () {
                         failed(null, { "StatusText": "Timeout. Signature was not captured after 3 minutes. If MSR is working, please Cancel out of Pick-up Confirm and retry. If MSR is not responding, complete Pick-up without signature.", StatusCode: "LUFI_BOPIS_TIMEOUT_ON_CAPTURE" });
                     }, 180000);
                 } else {
                     failed(null, { "StatusText": "Invalid Order Number was received.", StatusCode: "LUFI_BOPIS_INVALID_INPUT" });
                 }
             } catch (err) {
                 failed(null, { "StatusText": "Communication to PMM failed.", StatusCode: "LUFI_BOPIS_WEBSOCKET_DOWN" });
                 $log.error(err.message);
             }
         };

         claimPayment().then(function () {
             sendRequest();
         },
         function (data) {
             failed(null, data);
         });

         return defer.promise;
     };


     var paymentTerminalFunctions = {
         releasePaymentTerminal: function () {
                 //release timeout from Claim
                 if (timeoutPromise != null) {
                     $timeout.cancel(timeoutPromise);
                     timeoutPromise = null;
                 }

                 if (sigCapTimeoutPromise != null) {
                     $timeout.cancel(sigCapTimeoutPromise);
                     sigCapTimeoutPromise = null;
                 }

                 try {
                     ws.send(angular.toJson(releasePaymentTerminal));
                 } catch (err) {
                     $log.error(err.message);
                 }
             },
             getSignature: getSignature
         };

     return paymentTerminalFunctions;
 }])
 