var appServices = angular.module('appServicesPayment', []);
appServices.factory('payment', ['$http', '$cacheFactory', 'POSService', '$rootScope', 'loggerService', 'sendSMTPErrorEmail', function ($http, $cacheFactory, $POSService, $rootScope, $loggerService, $sendSMTPErrorEmail) {

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


    var authRequest = function (PersonInfoBillTo, cards) {

        if (cards && cards.length == 0) {
            return;
        }


        function createCardContract(card) {
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

        var total = 0;
        var tenderCount = 0;
        cards.forEach(function (card) {
            if (card.chargeAmount > 0) {
                tenderCount = tenderCount + 1;
            }
            total = total + card.chargeAmount;
        });



        var authRequestContract = {};
        authRequestContract.CreditAuthRequest = {};
        authRequestContract.CreditAuthRequest._OriginStoreNum = $POSService.getPOSParameters().storeNumber;
        authRequestContract.CreditAuthRequest._FulfillmentStoreNum = $POSService.getPOSParameters().storeNumber;
        authRequestContract.CreditAuthRequest._SourceApplication = 'Sterling';
        authRequestContract.CreditAuthRequest._TotalToAuthorize = Number(total).toString();
        authRequestContract.CreditAuthRequest._ProvideAddressFlag = "N";
        //authRequestContract.CreditAuthRequest._ValidateAVSInd = "HouseNumAndZip";
        authRequestContract.CreditAuthRequest._ValidateAVSInd = "None";
        authRequestContract.CreditAuthRequest._CustomerAddress = PersonInfoBillTo._AddressLine1;
        authRequestContract.CreditAuthRequest._CustomerZipCode = PersonInfoBillTo._ZipCode;
        //authRequestContract.CreditAuthRequest._TenderCount = cards.length.toString();
        authRequestContract.CreditAuthRequest._TenderCount = Number(tenderCount).toString();
        authRequestContract.CreditAuthRequest._FloorLimitEnable = "N";

        var TenderList = [];

        cards.forEach(function (card) {
            if (card.chargeAmount > 0) {
                TenderList.push(createCardContract(card));
                if (cards.length > 1 && card.cardType == 'GIFTCARD') {
                    TenderList.push(TenderList.shift());
                }

            }
        });

        Array.prototype.moveUp = function (value) {
            var index = this.indexOf(value),
                newPos = index - 1;

            if (index === -1)
                throw new Error("Element not found in array");

            this.splice(index, 1);
            if (index === 0)
                newPos = this.length;
            this.splice(newPos, 0, value);
        };
        var index = 0;
        cards.forEach(function (card) {
            index++;
            if (card.chargeAmount > 0 && cards.length > 1 && card.cardType == 'GIFTCARD') {
                //var itemToMove = TenderList[index];
                // TenderList.moveUp(itemToMove);
            }
        });

        authRequestContract.CreditAuthRequest.TenderList = TenderList;

        var url = serviceURL + "/CreditService/AuthRequest";
        return $http.post(url, authRequestContract);
    }

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

    var getAvailablePaymentTypes = function (paymentData) {
        var types = [paymentTypes.GIFTCARD, paymentTypes.CREDITCARD, paymentTypes.PLCC];

        if (paymentData.accNumber && paymentData.accNumber.length > 0) {
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
        setPaymentData: setPaymentData,
        getPaymentData: getPaymentData,
        clearPaymentData: clearPaymentData,
        cardTypes: cardTypes,
        createRepricePaymentContract: createRepricePaymentContract,
        createOrderPaymentContract: createOrderPaymentContract,
        getAvailablePaymentTypes: getAvailablePaymentTypes,
        testBankCardType: testBankCardType,
        translateCardType: translateCardType
    };
}]);

