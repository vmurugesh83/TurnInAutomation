var appServices = angular.module('appUtilities', []);

appServices.service('numberKeyPressValidator', [function () {
    return function (keyEvent) {
        var theEvent = keyEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        if (key == 13) {
            return;
        }
        key = String.fromCharCode(key);
        var regex = /[0-9]|\./;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }
}]);
appServices.service('numberOnlyKeyPressValidator', [function () {
    return function (keyEvent) {
        var theEvent = keyEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        if (key == 13) {
            return;
        }
        key = String.fromCharCode(key);
        var regex = /[0-9]/;
        if (!regex.test(key)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }
}]);

appServices.service('customerKeyPressValidator', [function () {
    return function (keyEvent) {
        var theEvent = keyEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        if (key == 13) {
            return;
        }
        key = String.fromCharCode(key);
        if (key == '&') {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) { theEvent.preventDefault(); }

            swal({ title: "Input Error", text: "Ampersand (&) is not allowed in this field.", showConfirmButton: true });
        }
    }
}]);

appServices.service('currencyKeyPressValidator', [function () {
    return function (keyEvent, curValue) {
        var theEvent = keyEvent || window.event;
        var key = theEvent.keyCode || theEvent.which;
        if (key == 13) {
            return;
        }
        key = String.fromCharCode(key);

        if(curValue.indexOf('.') !== -1) {
            if (key == '.') {
                theEvent.returnValue = false;
                if (theEvent.preventDefault) {
                    theEvent.preventDefault();
                }
            } else {
                if (curValue.length - curValue.indexOf('.') == 3) {
                    theEvent.returnValue = false;
                    if (theEvent.preventDefault) {
                        theEvent.preventDefault();
                    }
                }
            }
        }
    }
}]);


appServices.service('serviceArrayFixByCount', [function () {
    var fix=function (obj) {
        if (angular.isObject(obj)) {
            for (key in obj) {
                if (Object.keys(obj).length == 2 && key.indexOf("Count") > 1) {
                    for (tempKey in obj) {
                        if (tempKey != key && !angular.isArray(obj[tempKey])) {
                            obj[tempKey] = [obj[tempKey]];
                        }
                    }
                }
               fix(obj[key]);
            }
        }
    }
    return fix;

}]);

appServices.service('serviceArrayFix', [function () {
    var fix = function (obj) {
        if (angular.isObject(obj)) {

            if(obj.hasOwnProperty('_IsArray') && obj['_IsArray'].toString().trim().toLowerCase() === "true"){
                delete obj['_IsArray'];
                for (key in obj) {
                    //could change to look only for keys that are elements AKA do not start with _AttributeName
                    if (!angular.isArray(obj[key])) {
                        obj[key] = [obj[key]];
                    }
                    fix(obj[key]);
                }
            } else {
                for (key in obj) {
                    fix(obj[key]);
                }
            }
        }
    }
    return fix;

}]);

appServices.factory('POSService', ['$cacheFactory', '$rootScope', '$q', 'webSocketService', function ($cacheFactory, $rootScope, $q, ws) {
    var cache = $cacheFactory('POS');
    var previousParams = {};
    var setPOSTParametersFromURL = function (locationURL) {
        var associateId = getParameterByName('AssociateId', locationURL);
        var storeNumber = getParameterByName('StoreNumber', locationURL);
        var terminalNumber = getParameterByName('TerminalNumber', locationURL);
        var ldapId = getParameterByName('LdapId', locationURL);
        var ldapPassword = getParameterByName('LdapPassword', locationURL);
        var parameters = {
            associateId: associateId,
            storeNumber: storeNumber,
            terminalNumber: terminalNumber,
            ldapId: ldapId,
            ldapPassword: ldapPassword
        }
        cache.put('parameters', parameters);
    }

    var setPOSTParameters = function (associateId,storeNumber,terminalNumber,roles) {
        var oldParms = cache.get('parameters');
        if (!oldParms) {
            oldParms = {
                associateId: '',
                storeNumber: '',
                terminalNumber: ''
            };
        }

        if (roles && Object.keys(roles).length > 0) {
            previousParams = oldParms;
        }

        var parameters = {
            associateId: associateId ? associateId : oldParms.associateId,
            storeNumber: storeNumber ? storeNumber : oldParms.storeNumber,
            terminalNumber: terminalNumber ? terminalNumber : oldParms.terminalNumber,
            roles: roles
        };

        cache.put('parameters', parameters);

        $rootScope.$broadcast('POSParmsSet', angular.copy(parameters));
    }

    var managerLogout = function () {
        cache.put('parameters', previousParams);
    }

    var getParameterByName = function (name,locationURL) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(locationURL);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    var getPmmConfigAndStatus = function (refresh) {

        refresh = refresh ? true : false;

        var defer = $q.defer();


        var message = cache.get('configAndStatus');

        if (refresh === true || message === undefined) {

            if (angular.isDefined(ws) && ws.readystate === 1) {
                requestPmmConfig(defer);
            } else {
                $rootScope.$on('WS_Open_Event', function () { requestPmmConfig(defer); });
            }
        } else {
            defer.resolve(message);
        }

        return defer.promise;

    };

    var requestPmmConfig = function (defer) {
        try {

            var deregister = $rootScope.$on('Config_Event', function (event, data) {
                deregister();
                deregisterError();
                cache.put('configAndStatus', data);

                defer.resolve(data);
            });

            var deregisterError = $rootScope.$on('Config_Error', function (event, data) {
                deregisterError();
                deregister();
                defer.reject({ Error: data, ErrorText: 'Error reading Config from Websocket.' });
            });

            ws.send(angular.toJson({
                "MessageType": "Request",
                "Device": "Server",
                "Command": "Configure",
                "Info": "true"
            }));

        } catch (err) {
            defer.reject({ Error: err, ErrorText: 'Error on Websocket send during request for PMM Config.' });
        }
    };

    //get PMM config right away
    getPmmConfigAndStatus();

    return {
        setPOSTParametersFromURL: setPOSTParametersFromURL,
        setPOSTParameters: setPOSTParameters,
        managerLogout: managerLogout,
        getPOSParameters: function () {
            return cache.get('parameters')
        },
        getPmmConfigAndStatus: getPmmConfigAndStatus
    };

}]);

appServices.factory('authenticateUser', ['$http', function ($http) {
    return function (userName, password) {
        var contract = {
            Authreq: {
                uid: userName,
                pwd: password
            }
        };

        return $http.post(authURL, angular.toJson(contract));
    }
}]);


appServices.factory('errorObj', function () {
    return {
        newError: function (name, message, details, code, level) {
            var obj = {};
            obj.name = name.toString();
            obj.message = message.toString();
            obj.details = details.toString();
            obj.code = code;

            if (level) {
                obj.level = level;
            } else {
                obj.level = 'WARN';
            }

            return obj;
        }
    };
});

appServices.factory('loggerService', ['POSLogSocketService', '$log', function ($POSLogSocketService, $log) {
    var _warn = function (message) {
        $log.warn(message);
    };

    var _log = function (message) {
        $log.info(message);
    };

    var _error = function (message) {
        $log.error(message);
        $POSLogSocketService.logToPOS(message);
    };

    var _posLog = function (message) {
        $POSLogSocketService.logToPOS(message);
    };

    var logFunctions = {
        warn: _warn,
        log: _log,
        error: _error,
        posLog: _posLog
    };
    return logFunctions;
}]);

appServices.service('isPoBoxAddress', [function () {
    return function (personInfoAddress) {
        if (angular.isObject(personInfoAddress) && '_AddressLine1' in personInfoAddress) {
            
            ///   reg ->>>>>>>     /^(?:p(?:ost)?)[ .\W_]*(?:o(?:ffice)?)[ .\W_]*b(?:ox)?[ 0-9]*[^[a-z ]]*/
             
            var PO_REG_EX = [/(?:p(?:ost)?)[ .\W_]*(?:o(?:ffice)?)[ .\W_]*b(?:ox)?([ 0-9]+[ a-z]*)*$/,
                            /^(?:po|pob|pobox|pbox|box)(?:[ 0-9]+)/];

            var testString = '';

            //requires _AddressLine1 to exist and contain one non-blank character
            if ('_AddressLine1' in personInfoAddress && personInfoAddress._AddressLine1.toString().trim().length > 0) {
                testString = personInfoAddress._AddressLine1.toString().trim().toLowerCase();

                for (var i = 0; i < PO_REG_EX.length; i++) {
                    if (PO_REG_EX[i].test(testString)) {
                        return true;
                    }
                }
            } else {
                return null;
            }

            if ('_AddressLine2' in personInfoAddress && personInfoAddress._AddressLine2.toString().trim().length > 0) {
                testString = personInfoAddress._AddressLine2.toString().trim().toLowerCase();

                for (var i = 0; i < PO_REG_EX.length; i++) {
                    if (PO_REG_EX[i].test(testString)) {
                        return true;
                    }
                }
            }

            if ('_AddressLine3' in personInfoAddress && personInfoAddress._AddressLine3.toString().trim().length > 0) {
                testString = personInfoAddress._AddressLine3.toString().trim().toLowerCase();

                for (var i = 0; i < PO_REG_EX.length; i++) {
                    if (PO_REG_EX[i].test(testString)) {
                        return true;
                    }
                }
            }


            return false;
        } else {
            return null;
        }
    }

}]);

appServices.factory('sendSMTPErrorEmail', ['$http', 'POSService', function ($http, $POSService) {
    var url = serviceURL + "/Email/SendSMTPEmail";
    //var url = "http://localhost:7080/Email/SendSMTPEmail";
    return function (data, failedServiceURL, httpInputObj, headerString, additionalEmailRecipients ) {

        //var terminalNumber = $POSService.getPOSParameters().terminalNumber;
        //var storeNumber = $POSService.getPOSParameters().storeNumber;
        //if (angular.isDefined(terminalNumber) && null != terminalNumber  && terminalNumber.length > 0) {
        //    terminalNumber = " in the Store "+ storeNumber +" and terminal " + terminalNumber;
        //} else {
        //    terminalNumber = "";
        //}
        var terminalNumber = "";
        if (angular.isDefined($POSService.getPOSParameters().terminalNumber) && $POSService.getPOSParameters().terminalNumber != null) {
            terminalNumber = $POSService.getPOSParameters().terminalNumber;
        }
        var storeNumber = "";
        if (angular.isDefined($POSService.getPOSParameters().storeNumber) && $POSService.getPOSParameters().storeNumber != null) {
            storeNumber = $POSService.getPOSParameters().storeNumber
        }
        if (angular.isDefined(terminalNumber) && null != terminalNumber && terminalNumber.length > 0) {
            terminalNumber = " in the Store " + storeNumber + " and terminal " + terminalNumber;
        } else {
            terminalNumber = "";
        }

        var subject = "LUFI-2" + terminalNumber + " Failed while executing " + failedServiceURL;
       
        var errorMessage = "";
        var exceptionList = "";
        if (angular.isDefined(data) && (data !== null) && angular.isDefined(data.Error)){
            errorMessage = data.Error.Message;
            exceptionList = data.Error.ExceptionList;
        } else {

            if (angular.isString(data)) {
                errorMessage = data;
                exceptionList = "";
            } else {
                errorMessage = angular.toJson(data);
                exceptionList = "";
            }
        }

        var inputData = '';
        if (angular.isDefined(httpInputObj) && (httpInputObj !== null)) {
            if (angular.isString(httpInputObj)) {
                inputData = httpInputObj;
            } else {
                inputData = angular.toJson(httpInputObj, 2);
            }
        }

        var headerText = '';
        if (angular.isDefined(headerString) && (headerString !== null)) {
            if (angular.isString(headerString)) {
                headerText = headerString;
            } else {
                headerText = angular.toJson(headerString);
            }
        }


        var bodyText = " Error Messsage = " + errorMessage + "\n \n Exception List = " + exceptionList;

        if (inputData) {
            bodyText = " Http Input Data = " + inputData + "\n \n" + bodyText;
        }

        if (headerText) {
            bodyText = headerText + "\n \n" + bodyText;
        }

        var recipientsString = "OMSTeam@bonton.com";
        if (angular.isDefined(additionalEmailRecipients) && (additionalEmailRecipients !== null)) {
            if (angular.isString(additionalEmailRecipients)) {
                recipientsString = recipientsString + ";" + additionalEmailRecipients;
            } else if (angular.isArray(additionalEmailRecipients)) {
                recipientsString = recipientsString + ";" + additionalEmailRecipients.join(';');
            }
        }

       // alert(bodyText);
        var contract = {
            Email: {
                To: recipientsString,
                From: "DoNotReply@bonton.com",

                //To: "renuka.nagammanavar@bonton.com",
                //From: "renuka.nagammanavar@bonton.com",
                Subject: subject,
                Body: bodyText
            }
        };

        $http.post(url, angular.toJson(contract));
    }
}]);

appServices.factory('alertMessages', [function () {
    var messages = {
        paymentMessages: {
            ACC_NOT_AUTHORIZED: "** This account could not be authorized. Please ask the customer for another form of tender. **",
            MAX_ALLOWED_CC: "Max Allowed Credit/PLCC Cards 1",
            MAX_GC: "You can enter up to four gift cards.",
            CC_NOT_ALLOWED_WITH_ADC: "CC OR PLCC is not allowed for Associate Discount Card accounts.",
            NEGATIVE_AMOUNT_NOT_ALLOWED: "Entering Negative Amount is not allowed",
            REPRICE_ERROR: "Reprice Error",
            ADC_NOT_ALLOWED_WITH_CC: "Associate Discount Card is not allowed with Credit Cards.",
            INVALID_CARD_TYPE: "Invalid Card Type!",
            TOKENIZATION_ERROR: "Tokenization Error",
            GC_PIN_ERROR: "Gift Card pin validation error",
            GC_ALERT: "GC Alert!",
            GC_BALANCE_ERROR: "GC Balance Error",
            CARD_ERROR: "Card Error",
            SCAN_ERROR: "Scan Error",
            CC_DECLINED: "Credit Card was declined."
        }
    }
    return messages;
}]);

appServices.service('loadingIconService', [function () {
    this.isLockedBusyIcon = false;

    this.lockIcon = function () {
        this.isLockedBusyIcon = true;
    };

    this.showBusyIcon = function () {
        $('#loadDIV').show();
    };

    this.hideBusyIcon = function () {
        if (!this.isLockedBusyIcon) {
            $('#loadDIV').hide();
        }
    };

    this.resetIcon = function () {
        this.isLockedBusyIcon = false;
        this.hideBusyIcon();
    };
}]);

appServices.factory('securityService', ['POSService', function ($POSService) {
    return {
        canCancelOrder: canCancelOrder
    };

    function canCancelOrder() {
        var returnVar = false;

        var params = $POSService.getPOSParameters();
        if (params && params.roles) {
            returnVar = params.roles['MODIFY ORDER'] == 'True';
        }
        return returnVar;
    };
}]);

