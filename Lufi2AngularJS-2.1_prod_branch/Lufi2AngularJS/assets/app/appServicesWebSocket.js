var appServices = angular.module('appServicesWebSocket', []);
/*
 *   WebSoketService factory is used to communicate POS devices via Websocket protocol. 
 *   UPC scanner, CC communication occur via websocket. Any event raised by POS propagated via broadcast.
 */
appServices.factory('webSocketService', ['$rootScope', '$interval', '$log', function ($rootScope, $interval, $log) {
    var webSocketCheckInterval = 20000;
    var ws;
    initWebSocket();
    $interval(function () {
        initWebSocket();
    }, webSocketCheckInterval);

    function initWebSocket() {
        if (angular.isUndefined(ws) || ws.readyState > 1) {
            ws = new WebSocket(POSURL);
            ws.onerror = function (event) {
                $log.error(event);
            }
            ws.onopen = function (event) {
                $rootScope.$broadcast('WS_Open_Event', event.eventData);
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

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Error') {
                    $rootScope.$broadcast('MSR_Event_Error', eventData);
                    $rootScope.$broadcast('MSR_Event_Disabled', eventData);//MSR always disabled after error

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Success' && (/MSR successfully disabled/i).test(eventData.StatusText)) {
                    //swal({ title: "MSR Disable Success", text: "MSR Disable Success", showConfirmButton: true });
                    $rootScope.$broadcast('MSR_Event_Disabled', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'MSR' && eventData.ResponseType == 'Success' && (/.*(MSR successfully enabled|payment terminal successfully enabled).*/i).test(eventData.StatusText)) {

                    $rootScope.$broadcast('MSR_Event_Enabled', eventData);

                }
                else if (eventData.MessageType == 'Response' && eventData.Device == 'Printer' && eventData.ResponseType == 'Error') {
                    $rootScope.$broadcast('Printer_Error', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Server' && eventData.ResponseType == 'Data') {
                    $rootScope.$broadcast('Config_Event', eventData);

                } else if (eventData.MessageType == 'Response' && eventData.Device == 'Server' && eventData.ResponseType == 'Error') {
                    $rootScope.$broadcast('Config_Error', eventData);
                }
            };
        }
    }
    return ws;
}])

.factory('scannerService', ['$rootScope', '$interval', '$log', 'itemSearch', '$location', 'webSocketService', 'orderCart', 'itemDetail', 'errorObj', 'loggerService', function ($rootScope, $interval, $log, $itemSearch, $location, ws, orderCart, $itemDetail, errorObj, $loggerService) {
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
        var controllers = ['indexCtrl', 'homeCtrl', 'itemSearchCtrl', 'orderCartCtrl', 'paymentSummaryCtrl', 'couponCtrl', 'orderSearchCtrl'];
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
    return scannerFunctions;
}])
    .factory('msrService', ['$rootScope', '$interval', '$log', '$location', 'webSocketService', '$q', '$timeout', function ($rootScope, $interval, $log, $location, ws, $q, $timeout) {

        var isEnabled = false;
        var lastEnableDisableDateTime = new Date();

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

                        setIsEnabled(true, data.Timestamp);
                        deregister();
                        defer.resolve(cardType);
                    });


                try {
                    ws.send(angular.toJson(enableMSR(cardType)));

                } catch (err) {
                        deregister();
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
                    //on success message
                    var deregister = $rootScope.$on('MSR_Event_Disabled', function (event, data) {

                        var delayResolve = function () {
                            setIsEnabled(false, data.Timestamp);
                            deregister();
                            defer.resolve();
                        };

                        delayResolve();
                        // $timeout(delayResolve, 5000); //delay half a second for retalix to fully disable

                    });


                try {
                    ws.send(angular.toJson(disableMSR));

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

        MSRFunctions.claimMSR();

        return MSRFunctions;
    }])
        .factory('printerService', ['$rootScope', '$interval', '$log', '$location', 'webSocketService', function ($rootScope, $interval, $log, $location, ws) {
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

            var printOrder = function (orderNumber, retry) {
                return {
                    "MessageType": "Request",
                    "Device": "Printer",
                    "Command": "Print",
                    "OrderNumber": orderNumber,
                    "Retry": retry
                }
            };


            $rootScope.$on('WS_Close_Event', function (event) {
                MSRFunctions.releaseMSR();
            });

            var lastCard = undefined;
            var printerFunctions = {
                claimPrinter: function () {
                    try {
                        ws.send(angular.toJson(claimPrinter));
                    } catch (err) {
                        $log.error(err.message);
                    }
                },
                releasePrinter: function () {
                    try {
                        ws.send(angular.toJson(releasePrinter));
                    } catch (err) {
                        $log.error(err.message);
                    }
                },
                printOrder: function (orderNumber, retry) {
                    try {
                        ws.send(angular.toJson(printOrder(orderNumber, retry)));
                    } catch (err) {
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