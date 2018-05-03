var appServices = angular.module('appServicesPayment', []);
appServices.factory('payment', ['$http', '$cacheFactory', 'POSService', '$rootScope', 'loggerService', 'sendSMTPErrorEmail', 'securityService', '$q', 'alertMessages',
    function ($http, $cacheFactory, $POSService, $rootScope, $loggerService, $sendSMTPErrorEmail, $securityService, $q, $alertMessages) {

    var giftCard = {
        label: 'Gift Card',
        contractType: 'GIFTCARD',
        authType: 'GiftCard',
        msrType: "GiftCard"
    };

    var creditCard = {
        label: 'Bank/Credit Card',
        contractType: 'CREDITCARD',
        authType: 'BankCard',
        msrType: "BankCard"
    };

    var PLCC = {
        label: 'Store Charge',
        contractType: 'CREDITCARD',
        authType: 'PLCC',
        msrType: "PLCC"
    };

    var paymentTypes = {
        GIFTCARD: giftCard,
        CREDITCARD: creditCard,
        PLCC: PLCC
    };

    var cardTypes = [
        { type: 'VISA', label: 'Visa' },
        { type: 'DISCOVER', label: 'Discover Network' },
        { type: 'AMEX', label: 'American Express' },
        { type: 'MASTER', label: 'MasterCard' },
        { type: 'BT', label: 'Bon-Ton' },
        { type: 'BG', label: 'Bergner\’s' },
        { type: 'BS', label: 'Boston Store' },
        { type: 'CP', label: 'Carson\’s' },
        { type: 'EB', label: 'Elder-Beerman' },
        { type: 'HB', label: 'Herberger\’s' },
        { type: 'YK', label: 'Younkers' },
    ]

    var bankCardRegEx = /^(MasterCard|VISA|AmericanExpress|Discover|DinersClub|JCB)$/i;

    var testBankCardType = function (cardTypeString) {
        return bankCardRegEx.test(cardTypeString);
    };

    var translateCardType = function (cardTypeString) {
        if ((/^MasterCard$/i).test(cardTypeString)) {
            return "MASTER";
        } else if ((/^VISA$/i).test(cardTypeString)) {
            return "VISA";
        } else if ((/^(Discover|DinersClub|JCB)$/i).test(cardTypeString)) {
            return "DISCOVER";
        } else if ((/^AmericanExpress$/i).test(cardTypeString)) {
            return "AMEX";
        } else {
            return '';
        }
    };

    var getExpDateCC = function () {
        var date = new Date();
        var year = date.getFullYear() + 1;
        return "12/" + year;
    }
    var getExpDateAuth = function () {
        var date = new Date();
        var year = date.getFullYear() + 1;
        return year + "-12-31";
    }
    var daysInMonth = function (year, month) {
        return new Date(year, month, 0).getDate();
    }

    var createExpDateFormatForCC = function (exp) {
        if (exp && exp.length == 4) {
            var year = '20' + exp.substring(2, 4);
            var month = exp.substring(0, 2);
            var day = daysInMonth(Number(year), Number(month));
            return month + '/' + year;
        } else {
            return getExpDateCC();
        }

    }

    var createExpDateFormatForCCAuth = function (exp) {
        if (exp && exp.length == 4) {
            var year = '20' + exp.substring(2, 4);
            var month = exp.substring(0, 2);
            var day = daysInMonth(Number(year), Number(month));
            return year + '-' + month + '-' + day;
        } else {
            return getExpDateAuth();
        }
    }

    var pciCleanInfo = function (authFullHttpResponse, cartCards) {
        var cleanPaymentHttp = {
            IsRetalixActive: "",
            AuthRequest: {

                _OriginStoreNum: "",
                _FulfillmentStoreNum: "",
                _SourceApplication: "",
                _TotalToAuthorize: "",
                _ProvideAddressFlag: "",
                _ValidateAVSInd: "",
                _TenderCount: "",
                _FloorLimitEnable: "",
                TenderList: []
            },

            AuthResponse: {
                _AuthorizationResult: "",
                _MasterErrorDetailLocation: "",
                _MasterResultAltReasonCode: "",
                _MasterResultCode: "",
                _MasterResultReasonCode: "",
                _MasterResultReasonText: "",
                _MasterResultText: "",
                TenderList: []
            },

            CardDetailInCart: []
        };

        //check if retalix flag was set true or false
        if (angular.isUndefined(isRetalixReady)) {
            cleanPaymentHttp.IsRetalixActive = "undefined";
        } else if (isRetalixReady === null) {
            cleanPaymentHttp.IsRetalixActive = "null";
        } else {
            cleanPaymentHttp.IsRetalixActive = isRetalixReady.toString();
        }

        //clean authRequest
        if (angular.isObject(authFullHttpResponse) && angular.isObject(authFullHttpResponse.config) && angular.isObject(authFullHttpResponse.config.data) && angular.isObject(authFullHttpResponse.config.data.CreditAuthRequest)) {
            var requestPayload = authFullHttpResponse.config.data.CreditAuthRequest;

            cleanPaymentHttp.AuthRequest._OriginStoreNum = requestPayload._OriginStoreNum ? requestPayload._OriginStoreNum : "";
            cleanPaymentHttp.AuthRequest._FulfillmentStoreNum = requestPayload._FulfillmentStoreNum ? requestPayload._FulfillmentStoreNum : "";
            cleanPaymentHttp.AuthRequest._SourceApplication = requestPayload._SourceApplication ? requestPayload._SourceApplication : "";
            cleanPaymentHttp.AuthRequest._TotalToAuthorize = requestPayload._TotalToAuthorize ? requestPayload._TotalToAuthorize : "";
            cleanPaymentHttp.AuthRequest._ProvideAddressFlag = requestPayload._ProvideAddressFlag ? requestPayload._ProvideAddressFlag : "";
            cleanPaymentHttp.AuthRequest._ValidateAVSInd = requestPayload._ValidateAVSInd ? requestPayload._ValidateAVSInd : "";
            cleanPaymentHttp.AuthRequest._TenderCount = requestPayload._TenderCount ? requestPayload._TenderCount : "";
            cleanPaymentHttp.AuthRequest._FloorLimitEnable = requestPayload._FloorLimitEnable ? requestPayload._FloorLimitEnable : "";

            //tender list
            if (angular.isArray(requestPayload.TenderList)) {
                for (var i = 0; i < requestPayload.TenderList.length; i++) {
                    var cleanCard = {
                        _TenderType: "",
                        _Token: "",
                        _TokenType: ""
                    };

                    cleanCard._TenderType = requestPayload.TenderList[i]._TenderType ? requestPayload.TenderList[i]._TenderType : "";
                    cleanCard._Token = requestPayload.TenderList[i]._Token ? requestPayload.TenderList[i]._Token : "";
                    cleanCard._TokenType = requestPayload.TenderList[i]._TokenType ? requestPayload.TenderList[i]._TokenType : "";

                    cleanPaymentHttp.AuthRequest.TenderList.push(cleanCard);
                }
            }
        }

        //clean auth Response
        if (angular.isObject(authFullHttpResponse) && angular.isObject(authFullHttpResponse.data) && angular.isObject(authFullHttpResponse.data.CreditAuthResponse)) {
            var responseData = authFullHttpResponse.data.CreditAuthResponse;

            cleanPaymentHttp.AuthResponse._AuthorizationResult = responseData._AuthorizationResult ? responseData._AuthorizationResult : "";
            cleanPaymentHttp.AuthResponse._MasterErrorDetailLocation = responseData._MasterErrorDetailLocation ? responseData._MasterErrorDetailLocation : "";
            cleanPaymentHttp.AuthResponse._MasterResultAltReasonCode = responseData._MasterResultAltReasonCode ? responseData._MasterResultAltReasonCode : "";
            cleanPaymentHttp.AuthResponse._MasterResultCode = responseData._MasterResultCode ? responseData._MasterResultCode : "";
            cleanPaymentHttp.AuthResponse._MasterResultReasonCode = responseData._MasterResultReasonCode ? responseData._MasterResultReasonCode : "";
            cleanPaymentHttp.AuthResponse._MasterResultReasonText = responseData._MasterResultReasonText ? responseData._MasterResultReasonText : "";
            cleanPaymentHttp.AuthResponse._MasterResultText = responseData._MasterResultText ? responseData._MasterResultText : "";

            //tender list
            //check for none array response
            //if it is not array make it array
            if (!angular.isArray(responseData.TenderList.Item)) {
                responseData.TenderList = [responseData.TenderList.Item];
            } else {
                responseData.TenderList = responseData.TenderList.Item;
            }

            if (angular.isArray(responseData.TenderList)) {
                for (var i = 0; i < responseData.TenderList.length; i++) {
                    var cleanCard = {
                        _AccountType: "",
                        _DetailAuthorizationResult: "",
                        _ReferenceNum: "",
                        _Token: "",
                        _VendorResponseCode: ""
                    };

                    cleanCard._AccountType = responseData.TenderList[i]._AccountType ? responseData.TenderList[i]._AccountType : "";
                    cleanCard._DetailAuthorizationResult = responseData.TenderList[i]._DetailAuthorizationResult ? responseData.TenderList[i]._DetailAuthorizationResult : "";
                    cleanCard._ReferenceNum = responseData.TenderList[i]._ReferenceNum ? responseData.TenderList[i]._ReferenceNum : "";
                    cleanCard._Token = responseData.TenderList[i]._Token ? responseData.TenderList[i]._Token : "";
                    cleanCard._VendorResponseCode = responseData.TenderList[i]._VendorResponseCode ? responseData.TenderList[i]._VendorResponseCode : "";

                    cleanPaymentHttp.AuthResponse.TenderList.push(cleanCard);
                }
            }
        }


        //add cards from cart for comparison
        if (angular.isArray(cartCards)) {
            for (var i = 0; i < cartCards.length; i++) {
                var cleanCard = {
                    paymentType: {
                        label: "",
                        contractType: "",
                        authType: "",
                        msrType: ""
                    },
                    cardNumber: "",
                    token: "",
                    AccountNumberIndicator: "",
                    cardType: ""
                };

                var curCard = cartCards[i];

                cleanCard.cardNumber = angular.isString(curCard.cardNumber) ? curCard.cardNumber.slice(-4) : "";
                cleanCard.token = curCard.token ? curCard.token : "";
                cleanCard.AccountNumberIndicator = curCard.AccountNumberIndicator ? curCard.AccountNumberIndicator : "";
                cleanCard.cardType = curCard.cardType ? curCard.cardType : "";

                if (angular.isObject(curCard.paymentType)) {
                    cleanCard.paymentType.label = curCard.paymentType.label ? curCard.paymentType.label : "";
                    cleanCard.paymentType.contractType = curCard.paymentType.contractType ? curCard.paymentType.contractType : "";
                    cleanCard.paymentType.authType = curCard.paymentType.authType ? curCard.paymentType.authType : "";
                    cleanCard.paymentType.msrType = curCard.paymentType.msrType ? curCard.paymentType.msrType : "";
                }

                cleanPaymentHttp.CardDetailInCart.push(cleanCard);
            }

        }

        return cleanPaymentHttp;
    };



    var _previousAuth = {
        isAuthSuccess: false,
        authOrderTotal: "",
        previousTenders: [],
        previousOrderNo:""
    };

    var clearPreviousAuthCache = function ()
    {
        _previousAuth.isAuthSuccess = false;
        _previousAuth.authOrderTotal = "";
        _previousAuth.previousTenders = [];
        _previousAuth.previousOrderNo = "";
    };

    var _isAlreadyAuthed = function (cart, authRequest)
    {
        //check that all input matches a successful previous Auth

        //if there was a successful previous auth AND the cart's OrderNo matches the previous auth's OrderNo AND the auth Total amount matches
        if (_previousAuth.isAuthSuccess && 
            _previousAuth.previousOrderNo === cart._OrderNo && 
            _previousAuth.authOrderTotal === cart._OrderTotal) {

            //check all card tokens and charge amounts are the same and number of cards is the same
            if (_previousAuth.previousTenders.length === authRequest.CreditAuthRequest.TenderList.length)
            {
                for (var i = 0; i < _previousAuth.previousTenders.length; i++)
                {
                    var prevToken = _previousAuth.previousTenders[i]._Token;
                    var prevAmount = _previousAuth.previousTenders[i]._Amount;

                    var reqToken = null;
                    var reqAmount = null;

                    //find corresponding card in the auth request's list
                    for (var p = 0; p < authRequest.CreditAuthRequest.TenderList.length; p++)
                    {
                        if (authRequest.CreditAuthRequest.TenderList[p]._Token === prevToken)
                        {
                            reqToken = authRequest.CreditAuthRequest.TenderList[p]._Token;
                            reqAmount = authRequest.CreditAuthRequest.TenderList[p]._Amount;
                            break;
                        }
                    }

                    //if previous auth's card was not found in the request for auth  OR the Amount does not match, return false
                    if (reqToken === null ||
                        prevToken !== reqToken ||
                        prevAmount !== reqAmount)
                    {
                        return false;
                    }
                }

                //all cards passed check
                return true;
            }
        }

        return false;
    };

    var authRequest = function (cart, cards) {
       
        var defer = $q.defer();

        if (cards && cards.length == 0) {
            defer.reject();
            return defer.promise;
        }

        function createCardContract(card) {

            //clean unicode nulls from card.name, we will actual take out the first set of control characters before unicode Space (\u0020)
            if (card && angular.isString(card.name)) {
                card.name = card.name.replace(/[\u0000-\u001F]/g, "");
            }

            var tenderContract = {};
            if (card.cardType == 'GIFTCARD') {
                tenderContract._TenderType = giftCard.authType;
                tenderContract._Token = card.token.toString();
                tenderContract._TokenType = card.AccountNumberIndicator.toString();
                tenderContract._ExpirationDate = getExpDateAuth();
                tenderContract._PIN = card.giftCardPin.toString();
                tenderContract._Amount = card.chargeAmount.toString();
            } else if (card.cardType == 'PLCC') {
                tenderContract._TenderType = card.paymentType.authType;
                tenderContract._Token = card.token.toString();
                tenderContract._TokenType = card.AccountNumberIndicator.toString();
                tenderContract._ExpirationDate = createExpDateFormatForCCAuth(card.exp)
                tenderContract._Name = card.name;
                tenderContract._Amount = card.chargeAmount.toString();
            } else {
                tenderContract._TenderType = card.paymentType.authType;
                tenderContract._Token = card.token.toString();
                tenderContract._TokenType = card.AccountNumberIndicator.toString();
                tenderContract._ExpirationDate = createExpDateFormatForCCAuth(card.exp)
                tenderContract._Name = card.name;
                if (card.cid && (card.cid.toString().trim().length > 0)) {
                    tenderContract._PIN = card.cid;
                }
                tenderContract._Amount = card.chargeAmount.toString();
            }
            return tenderContract;
        }

        var authRequestContract = {
            "CreditAuthRequest": {
                "_OriginStoreNum": "0",
                "_SourceApplication": "LUFI",
                "_ProvideAddressFlag": "N",
                "_OrderNo":"",
                "TenderList": [],
                "OrderLines": [],
                "BillTo": {
                    "_FirstName": "",
                    "_LastName": "",
                    "_AddressLine1": "",
                    "_AddressLine2": "",
                    "_City": "",
                    "_State": "",
                    "_ZipCode": "",
                    "_Country":"",
                    "_EmailAddress":""
                }
            }
        };

        var copyAuthAddress = function (source, destination) {
            destination._FirstName = source._FirstName ? angular.copy(source._FirstName) : "";
            destination._LastName = source._LastName ? angular.copy(source._LastName) : "";
            destination._AddressLine1 = source._AddressLine1 ? angular.copy(source._AddressLine1) : "";
            destination._AddressLine2 = source._AddressLine2 ? angular.copy(source._AddressLine2) : "";
            destination._City = source._City ? angular.copy(source._City) : "";
            destination._State = source._State ? angular.copy(source._State) : "";
            destination._ZipCode = source._ZipCode ? angular.copy(source._ZipCode) : "";
            destination._Country = source._Country ? angular.copy(source._Country) : "";
            destination._EmailAddress = source._EMailID ? angular.copy(source._EMailID) : "";
        };

        authRequestContract.CreditAuthRequest._OriginStoreNum = $POSService.getPOSParameters().storeNumber;
        authRequestContract.CreditAuthRequest._OrderNo = angular.copy(cart._OrderNo);
        copyAuthAddress(cart.PersonInfoBillTo ,authRequestContract.CreditAuthRequest.BillTo);
        
        var TenderList = [];

        cards.forEach(function (card) {
            if (card.chargeAmount > 0) {
                TenderList.push(createCardContract(card));
                if (cards.length > 1 && card.cardType == 'GIFTCARD') {
                    TenderList.push(TenderList.shift());
                }
            }
        });

        authRequestContract.CreditAuthRequest.TenderList = TenderList;

        for (var i = 0; i < cart.OrderLines.OrderLine.length; i++)
        {
            var currentLine = cart.OrderLines.OrderLine[i];

            var tempOrderLine = {
                "_ItemID": "",
                "_UPC":"",
                "_Quantity": "0",
                "_UnitPrice": "0.00",
                "_ItemName": "",
                "ShipTo": {
                    "_FirstName": "",
                    "_LastName": "",
                    "_AddressLine1": "",
                    "_AddressLine2": "",
                    "_City": "",
                    "_State": "",
                    "_ZipCode": "",
                    "_Country":"",
                    "_EmailAddress":""
                }
            };

            tempOrderLine._ItemID = currentLine.Item._ItemID ? angular.copy(currentLine.Item._ItemID) : ""; // they what UPC not SKU for Fraud Checks
            tempOrderLine._UPC = currentLine.Item._UPCCode ? angular.copy(currentLine.Item._UPCCode): ""; // they what UPC not SKU for Fraud Checks
            tempOrderLine._Quantity = currentLine._OrderedQty ? angular.copy(currentLine._OrderedQty) : "";
            tempOrderLine._UnitPrice = currentLine.LinePriceInfo._UnitPrice ? angular.copy(currentLine.LinePriceInfo._UnitPrice) : "";
            tempOrderLine._ItemName = currentLine.btDisplay.defaultItemDescription ? angular.copy(currentLine.btDisplay.defaultItemDescription) : "";

            copyAuthAddress(currentLine.PersonInfoShipTo, tempOrderLine.ShipTo);

            authRequestContract.CreditAuthRequest.OrderLines.push(tempOrderLine);
        }


        //Before sending another Auth, check that we did not already Auth this order
        if (_isAlreadyAuthed(cart, authRequestContract))
        {
            //if auth has already went through it must have all the necessary payments set on the cart so just resolve
            defer.resolve();
            return defer.promise;
        }

        var url = serviceURL + "/CreditService/AuthRequest";

        $http.post(url, authRequestContract).then(function (HttpResponse) {
            try {
                //check auth success of each payment
                var authFailed = false;
                var failureReason = "";
                var isUnknownAuthResponse = false;

                var response = angular.fromJson(HttpResponse.data);
                var tenderList = null;

                if (angular.isDefined(response.CreditAuthResponse) && angular.isDefined(response.CreditAuthResponse.TenderList)) {
                    tenderList = response.CreditAuthResponse.TenderList;
                }

                if (tenderList === null || tenderList.Item === undefined || tenderList === null) {
                    authFailed = true;
                    failureReason += "Payment Auth returned no tenderList or no tenderList.Item ";
                } else {

                    //if it is not array make it array
                    if (!angular.isArray(tenderList.Item)) {
                        tenderList = [tenderList.Item];
                    } else {
                        tenderList = tenderList.Item;
                    }

                    //add auth info to each card that has chargeAmount > 0
                    for (var i = 0; i < cards.length; i++) {
                        var card = cards[i];
                        if (card.chargeAmount > 0) {
                            for (var j = 0; j < tenderList.length; j++) {
                                var cardReturned = tenderList[j];
                                if (Number(cardReturned._Token) == Number(card.token)) {
                                    card.authInfo = cardReturned;
                                    break;
                                }
                            }
                        }
                    }

                    //Verify that every card with charge amount > 0 : has auth info and that auth
                    for (var i = 0; i < cards.length; i++) {
                        var card = cards[i];

                        if (card.chargeAmount > 0) {
                            if (!angular.isObject(card.authInfo) || !angular.isString(card.authInfo._DetailAuthorizationResult)) {
                                authFailed = true;
                                failureReason += "Card has no authInfo. "; //TODO: add card details SEE MY JSON Captures In the TASKS folders
                            } else if (card.authInfo._DetailAuthorizationResult != 'Approve') {
                                authFailed = true;
                                failureReason += "Card has DetailAuthorizationResult != Approve. ";

                                if ((/Unknown/i).test(card.authInfo._DetailAuthorizationResult)) {
                                    isUnknownAuthResponse = true;
                                }
                            }
                        }
                    }
                }


                // TODO: if authFailed then we would need to de-auth Giftcards no?
                if (authFailed) {
                    swal({ title: "Alert!", text: $alertMessages.paymentMessages.ACC_NOT_AUTHORIZED, showConfirmButton: true });

                    //report card failures
                    if (isUnknownAuthResponse) {
                        $sendSMTPErrorEmail("One or more cards have failed auth.", $alertMessages.paymentMessages.FAILURE_ON_AUTH, pciCleanInfo(HttpResponse, cards), failureReason);
                    }

                    defer.reject();

                } else {

                    //add Auth infor to payment service's cached _previousAuth
                    _previousAuth.isAuthSuccess = true;
                    _previousAuth.authOrderTotal = angular.copy(cart._OrderTotal);
                    _previousAuth.previousOrderNo = angular.copy(cart._OrderNo);
                    _previousAuth.previousTenders = [];
                    for (var r = 0; r < tenderList.length; r++)
                    {
                        _previousAuth.previousTenders.push(
                            {
                                _Token: "" + tenderList[r]._Token,
                                _Amount: "" + tenderList[r]._AmountAuthorized
                            }
                        );
                    }

                    delete cart.PaymentMethods;
                    cart.PaymentMethods = {
                        PaymentMethod: createOrderPaymentContract(cards)
                    };

                    cart.CreditAuthResponse = {
                        "_AuthorizationResult": "",
                        "_MasterErrorDetailLocation": "",
                        "_MasterResultAltReasonCode": "",
                        "_MasterResultCode": "",
                        "_MasterResultReasonCode": "",
                        "_MasterResultReasonText": "",
                        "_MasterResultText": ""
                    };

                    if (angular.isDefined(response.CreditAuthResponse)) {
                        cart.CreditAuthResponse._AuthorizationResult = response.CreditAuthResponse._AuthorizationResult ? angular.copy(response.CreditAuthResponse._AuthorizationResult) : "";
                        cart.CreditAuthResponse._MasterErrorDetailLocation = response.CreditAuthResponse._MasterErrorDetailLocation ? angular.copy(response.CreditAuthResponse._MasterErrorDetailLocation) : "";
                        cart.CreditAuthResponse._MasterResultAltReasonCode = response.CreditAuthResponse._MasterResultAltReasonCode ? angular.copy(response.CreditAuthResponse._MasterResultAltReasonCode) : "";
                        cart.CreditAuthResponse._MasterResultCode = response.CreditAuthResponse._MasterResultCode ? angular.copy(response.CreditAuthResponse._MasterResultCode) : "";
                        cart.CreditAuthResponse._MasterResultReasonCode = response.CreditAuthResponse._MasterResultReasonCode ? angular.copy(response.CreditAuthResponse._MasterResultReasonCode) : "";
                        cart.CreditAuthResponse._MasterResultReasonText = response.CreditAuthResponse._MasterResultReasonText ? angular.copy(response.CreditAuthResponse._MasterResultReasonText) : "";
                        cart.CreditAuthResponse._MasterResultText = response.CreditAuthResponse._MasterResultText ? angular.copy(response.CreditAuthResponse._MasterResultText) : "";
                    }

                    defer.resolve();
                }
            } catch (exception) {
                swal({ title: "Alert!", text: $alertMessages.paymentMessages.FAILURE_DURING_ORDER_CREATE, showConfirmButton: true });
                $sendSMTPErrorEmail("Exception Throw: " + angular.toJson(exception) + "\n \n HTTPResponse: " + angular.toJson(HttpResponse, 2), $alertMessages.paymentMessages.FAILURE_DURING_ORDER_CREATE);

                defer.reject();
            }
        }, function (response) {
            var errorText = '';
            if (angular.isString(response.data)) {
                errorText = response.data;
            } else {
                errorText = angular.toJson(response.data);
            }
            swal({ title: "Authorization Error", text: errorText, showConfirmButton: true });

            defer.reject();
        });

        return defer.promise;
    };

    var tokenize = function (cardNumber, contractType, success, error) {
        if (cardNumber && contractType) {
            var tokenizationContract = {
                "TokenizationRequest": [
                    {
                        "_data": cardNumber,
                        "_type": contractType
                    }
                ]
            };

            $http.post(tokenizeURL, tokenizationContract).success(function (data) {
                var response = angular.fromJson(data);
                if (response.TokenizationResponse.Item._ReturnCode == '0')
                    success(response.TokenizationResponse.Item);
                else
                    error("Can't Create Token:" + response);
            }).error(function (data) {
                $sendSMTPErrorEmail(data, tokenizeURL);
                error(data);
            });
        }
    }

    var gcBalance = function (token, pin, success, error) {
        var contract = {
            "EGCBalanceReq": {
                "_OriginStoreNum": $POSService.getPOSParameters().storeNumber,
                "_FulfillmentStoreNum": $POSService.getPOSParameters().storeNumber,
                "_SourceApplication": "Sterling",
                "GCList": [
                      {
                          "_Token": token,
                          "_PIN": pin
                      }
                ]
            }
        };


        var url = serviceURL + "/CreditService/GCBalance";
        $http.post(url, contract).success(function (data) {
            var response = angular.fromJson(data);
            success(angular.fromJson(response));
        }).error(function (data) {
            $sendSMTPErrorEmail(data, url);
            error(data);
        });
    }
    var cache = $cacheFactory('payments');
    var setPaymentData = function (paymentData) {
        cache.put('paymentData', paymentData);
    }
    var getPaymentData = function () {
        return cache.get('paymentData');
    }

    var clearPaymentData = function () {
        cache.remove('paymentData');
    }



    var setCreateOrderPaymentContract = function (card) {

        //(second check) clean unicode nulls from card.name, we will actual take out the first set of control characters before unicode Space (\u0020)
        if (card && angular.isString(card.name)) {
            card.name = card.name.replace(/[\u0000-\u001F]/g, "");
        }

        if (card.cardType == 'GIFTCARD') {
            card.exp = getExpDateCC();
            var contract = {
                "_BillToKey": "",
                "_ChargeSequence": "1",
                "_CreditCardExpDate": createExpDateFormatForCC(card.exp),
                "_CreditCardName": "",
                "_CreditCardNo": card.cardNumber.replace(/\*/g, ''),
                "_CreditCardType": "GC",
                "_DisplayCreditCardNo": card.cardNumber.replace(/\*/g, ''),
                "_MaxChargeLimit": card.chargeAmount,
                "_PaymentReference1": (card.authInfo && card.authInfo._ReferenceNum) ? card.authInfo._ReferenceNum : "",
                "_PaymentReference2": card.giftCardPin,
                "_PaymentReference3": card.AccountNumberIndicator.toString(),
                "_PaymentType": "GC",
                "_SvcNo": card.token,
                "_UnlimitedCharges": "N",
                "PaymentDetails": {
                    "_AuthAvs": "",
                    "_AuthCode": "",
                    "_AuthorizationExpirationDate": "",
                    "_AuthorizationID": "",
                    "_AuthReturnCode": "APPROVED",
                    "_AuthReturnFlag": "Y",
                    "_AuthReturnMessage": (card.authInfo && card.authInfo._ReferenceNum) ? card.authInfo._ReferenceNum : "",
                    "_AuthTime": "",
                    "_ChargeType": "AUTHORIZATION",
                    "_InternalReturnCode": "",
                    "_InternalReturnFlag": "",
                    "_InternalReturnMessage": "",
                    "_ProcessedAmount": card.chargeAmount,
                    "_Reference1": card.authInfo ? card.authInfo._ReferenceNum : '',
                    "_Reference2": card.giftCardPin,
                    "_RequestAmount": card.chargeAmount,
                    "_RequestId": "",
                    "_RequestProcessed": "Y",
                    "_TranReturnCode": "APPROVED",
                    "_TranReturnFlag": "SUCCESS",
                    "_TranReturnMessage": "",
                    "_TranType": "AUTHORIZATION"
                }
            }
            card.createOrderContract = contract;
        } else {
            var contract = {
                "_BillToKey": "",
                "_ChargeSequence": "1",
                "_CreditCardExpDate": createExpDateFormatForCC(card.exp),
                "_CreditCardName": card.name,
                "_CreditCardNo": card.token,
                "_CreditCardType": card.cardType,
                "_DisplayCreditCardNo": card.cardNumber.replace(/\*/g, ''),
                "_FirstName": "",
                "_LastName": "",
                "_MiddleName": "",
                "_PaymentType": "CREDIT_CARD",
                "_UnlimitedCharges": "Y",
                "_PaymentReference3": card.AccountNumberIndicator.toString(),
                "PaymentDetails": {
                    "_AuthAvs": "",
                    "_AuthCode": card.authInfo ? card.authInfo._AuthorizationCode : '',
                    "_AuthorizationID": card.authInfo ? card.authInfo._AuthorizationCode : '',
                    "_AuthorizationExpirationDate": "",
                    "_AuthReturnCode": "APPROVED",
                    "_AuthReturnFlag": "Y",
                    "_AuthReturnMessage": (card.authInfo && card.authInfo._ReferenceNum) ? card.authInfo._ReferenceNum : "",
                    "_AuthTime": "",
                    "_ChargeType": "AUTHORIZATION",
                    "_InternalReturnCode": "",
                    "_InternalReturnFlag": "",
                    "_InternalReturnMessage": "",
                    "_ProcessedAmount": card.chargeAmount,
                    "_Reference1": "",
                    "_Reference2": "",
                    "_RequestAmount": card.chargeAmount,
                    "_RequestId": (card.authInfo && card.authInfo._ReferenceNum) ? card.authInfo._ReferenceNum : "",
                    "_RequestProcessed": "Y",
                    "_TranReturnCode": "APPROVED",
                    "_TranReturnFlag": "SUCCESS",
                    "_TranReturnMessage": "",
                    "_TranType": "AUTHORIZATION"
                }
            }
            card.createOrderContract = contract;
        }

    }
    //Create PaymentMethod node for create Order
    var createOrderPaymentContract = function (cards) {
        var PaymentMethod = [];
        cards.forEach(setCreateOrderPaymentContract);
        cards.forEach(function (card) {
            PaymentMethod.push(card.createOrderContract);
        })
        return PaymentMethod;
    }


    //Create PaymentMethod node for repricing;
    var setRepriceCardContract = function (card) {
        card.rePriceContract = {
            _CreditCardNo: card.token,
            _CreditCardType: card.cardType == 'PLCC' ? 'BS' : card.cardType,
            _PaymentType: card.cardType == 'GIFTCARD' ? 'GC' : 'CREDIT_CARD',
        }
    }

    var createRepricePaymentContract = function (cards) {
        var PaymentMethod = [];
        cards.forEach(setRepriceCardContract);
        cards.forEach(function (card) {
            if (card.cardType != 'GIFTCARD')
                PaymentMethod.push(card.rePriceContract);
        })
        return PaymentMethod;
    }

    var getAvailablePaymentTypes = function (paymentData, isPaymentForCsr) {
        var types = [paymentTypes.GIFTCARD, paymentTypes.CREDITCARD, paymentTypes.PLCC];

        //if Associate Discount Card is on order then only giftcards can be used
        if (isPaymentForCsr === true || (paymentData.accNumber && paymentData.accNumber.length > 0)) {
            types.splice(1, 2);
        }

        if (paymentData.cards.length > 0) {
            var gcCount = 0;
            angular.forEach(paymentData.cards, function (card) {
                if (card.paymentType.authType == paymentTypes.PLCC.authType || card.paymentType.authType == paymentTypes.CREDITCARD.authType) {
                    types.splice(1, 2);
                } else if (card.paymentType.authType == paymentTypes.GIFTCARD.authType) {
                    gcCount++;
                }
            });

            if (gcCount >= 4) {
                types.splice(0, 1);
            }
        }
        return types;
    }

    var canAddCreditCardOrPlcc = function (paymentData) {
        var availablePayments = getAvailablePaymentTypes(paymentData);
        if (angular.isArray(availablePayments)) {
            for (var i = 0; i < availablePayments.length; i++) {
                if ((availablePayments[i] == paymentTypes.CREDITCARD) || (availablePayments[i] == paymentTypes.PLCC)) {
                    return true;
                }
            }
        }

        return false;
    };
    var canAddGiftcard = function (paymentData) {
        var availablePayments = getAvailablePaymentTypes(paymentData);
        if (angular.isArray(availablePayments)) {
            for (var i = 0; i < availablePayments.length; i++) {
                if (availablePayments[i] == paymentTypes.GIFTCARD) {
                    return true;
                }
            }
        }

        return false;
    };

    var getInContactPaymentId = function () {
        var url = httpsServiceURL + "/OrderProcess/ConfirmOrder?Action=getPCRN";
        return $http.get(url).then(function (response) { return response; }, function (response) {

            $sendSMTPErrorEmail("InContact Payment ID generation failed.", url, response);

            $q.reject(response);
        });;
    };
    
    var getInContactPayment = function (paymentId) {
        if (paymentId === null || paymentId === undefined) {
            $q.reject('No paymentId');
        } else {
            var url = httpsServiceURL + "/OrderProcess/ConfirmOrder?Action=getTender&PCRN=" + paymentId;
            return $http.get(url).then(function (response) { return response; }, function (response) {

                $sendSMTPErrorEmail("InContact Payment Call failed.", url, response);

                $q.reject(response);
            });
        }
    };


    //watch events. 
    $rootScope.$on('orderCartDeleted', function (event, paymentData) {
        clearPaymentData();
    });


    $rootScope.$on('selectedCustomerCleared', function (event, paymentData) {
        clearPaymentData();
    });

    return {
        tokenize: tokenize,
        gcBalance: gcBalance,
        paymentTypes: paymentTypes,
        authRequest: authRequest,
        clearPreviousAuthCache: clearPreviousAuthCache,
        setPaymentData: setPaymentData,
        getPaymentData: getPaymentData,
        clearPaymentData: clearPaymentData,
        cardTypes: cardTypes,
        createRepricePaymentContract: createRepricePaymentContract,
        createOrderPaymentContract: createOrderPaymentContract,
        getAvailablePaymentTypes: getAvailablePaymentTypes,
        canAddGiftcard: canAddGiftcard,
        canAddCreditCardOrPlcc: canAddCreditCardOrPlcc,
        testBankCardType: testBankCardType,
        translateCardType: translateCardType,
        getInContactPaymentId: getInContactPaymentId,
        getInContactPayment: getInContactPayment
    };
}]);

