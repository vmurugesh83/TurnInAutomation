describe("Payment service", function () {
    var payment, POSService, $httpBackend;

    beforeEach(angular.mock.module("appServicesPayment", "appUtilities", "appServiceOrderCart", 'appServicesWebSocket', 'appServicesItem', 'appServiceReprice', 'appServicesCustomer'));
    beforeEach(module('ui.router'));

    beforeEach(inject(function (_payment_, _$state_, _POSService_, _$httpBackend_) {
        $state = _$state_;
        POSService = _POSService_;
        $httpBackend = _$httpBackend_;
        payment = _payment_;
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should initialize public variables', function () {
        expect(payment).toBeDefined();
        expect(payment.paymentTypes.GIFTCARD).toEqual({
            label: 'Gift Card',
            contractType: 'GIFTCARD',
            authType: 'GiftCard',
            msrType: "GiftCard"
        });
        expect(payment.paymentTypes.CREDITCARD).toEqual({
            label: 'Bank/Credit Card',
            contractType: 'CREDITCARD',
            authType: 'BankCard',
            msrType: "BankCard"
        });
        expect(payment.paymentTypes.PLCC).toEqual({
            label: 'Store Charge',
            contractType: 'CREDITCARD',
            authType: 'PLCC',
            msrType: "PLCC"
        });
        expect(payment.cardTypes).toEqual([
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
        ]);
    });

    it('should do nothing when the card number is null', function () {
        payment.tokenize(null, 'type', function () { }, function () { });
    });

    it('should do nothing when the contract type is null', function () {
        payment.tokenize('1234', null, function () { }, function () { });
    });

    it('should call the tokenize endpoint', function () {
        var successCalled = false, failureCalled = false;
        var cardNumber = '1234', contractType = 'card'
        var tokenizationContract = {
            "TokenizationRequest": [
                {
                    "_data": cardNumber,
                    "_type": contractType
                }
            ]
        };

        $httpBackend.expectPOST(tokenizeURL, tokenizationContract).respond({
            TokenizationResponse: {
                Item: {
                    _ReturnCode: 0
                }
            }
        });

        payment.tokenize(cardNumber, contractType, function (item) {
            successCalled = true;
            expect(item._ReturnCode).toBe(0);
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });

    it('should return an error message when the return code is not 0', function () {
        var successCalled = false, failureCalled = false;
        var cardNumber = '1234', contractType = 'card'
        var tokenizationContract = {
            "TokenizationRequest": [
                {
                    "_data": cardNumber,
                    "_type": contractType
                }
            ]
        };

        $httpBackend.expectPOST(tokenizeURL, tokenizationContract).respond({
            TokenizationResponse: {
                Item: {
                    _ReturnCode: 1
                }
            }
        });

        payment.tokenize(cardNumber, contractType, function () {
            successCalled = true;
        }, function (message) {
            failureCalled = true;
            expect(message).toBe("Can't Create Token:" + {
                TokenizationResponse: {
                    Item: {
                        _ReturnCode: 1
                    }
                }
            });
        });

        $httpBackend.flush();

        expect(successCalled).toBe(false);
        expect(failureCalled).toBe(true);
    });

    it('should call the gift card service endpoing', function () {
        var successCalled = false, failureCalled = false;
        var token = 'abc', pin = '123';

        spyOn(POSService, 'getPOSParameters').and.callFake(function () {
            return {
                associateId: '111111',
                storeNumber: '101',
                terminalNumber: '123',
                roles: 'ENDUSER, LUFI_ORDER_MODIFY'
            };
        });

        var contract = {
            "EGCBalanceReq": {
                "_OriginStoreNum": "101",
                "_FulfillmentStoreNum": "101",
                "_SourceApplication": "Sterling",
                "GCList": [
                      {
                          "_Token": token,
                          "_PIN": pin
                      }
                ]
            }
        };

        $httpBackend.expectPOST(serviceURL + "/CreditService/GCBalance", contract).respond({ success: 'success' });

        payment.gcBalance(token, pin, function (data) {
            successCalled = true;
            expect(data.success).toBe('success');
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });

    it('should just return if cards is empty', function () {
        payment.authRequest(null, []);
    });

    it('should call the payment service auth request endpoint with giftcard', function () {
        var cards = [
        {
            chargeAmount: 10,
            cardType: 'GIFTCARD',
            token: '11111',
            giftCardPin: '123456'
        }];

        spyOn(POSService, 'getPOSParameters').and.callFake(function () {
            return {
                associateId: '111111',
                storeNumber: '101',
                terminalNumber: '123',
                roles: 'ENDUSER, LUFI_ORDER_MODIFY'
            };
        });

        $httpBackend.expectPOST(serviceURL + "/CreditService/AuthRequest", {
            "CreditAuthRequest": {
                "_OriginStoreNum": "101",
                "_FulfillmentStoreNum": "101",
                "_SourceApplication": "Sterling",
                "_TotalToAuthorize": "10",
                "_ProvideAddressFlag": "N",
                "_ValidateAVSInd": "None",
                "_TenderCount": "1",
                "_FloorLimitEnable": "N",
                "TenderList": [
                    {
                        "_TenderType": "GiftCard",
                        "_Token": "11111",
                        "_ExpirationDate": "2016-12-31",
                        "_PIN": "123456",
                        "_Amount": "10"
                    }]
            }
        }).respond({ success: 'success' });

        payment.authRequest('info', cards);

        $httpBackend.flush();
    });

    it('should call the payment service auth request endpoint with PLCC', function () {
        var cards = [
        {
            chargeAmount: 10,
            cardType: 'PLCC',
            token: '11111',
            paymentType: {
                authType: 'BankCard'
            }
        }];

        spyOn(POSService, 'getPOSParameters').and.callFake(function () {
            return {
                associateId: '111111',
                storeNumber: '101',
                terminalNumber: '123',
                roles: 'ENDUSER, LUFI_ORDER_MODIFY'
            };
        });

        $httpBackend.expectPOST(serviceURL + "/CreditService/AuthRequest", {
            "CreditAuthRequest": {
                "_OriginStoreNum": "101",
                "_FulfillmentStoreNum": "101",
                "_SourceApplication": "Sterling",
                "_TotalToAuthorize": "10",
                "_ProvideAddressFlag": "N",
                "_ValidateAVSInd": "None",
                "_TenderCount": "1",
                "_FloorLimitEnable": "N",
                "TenderList": [
                    {
                        "_TenderType": "BankCard",
                        "_Token": "11111",
                        "_ExpirationDate": "2016-12-31",
                        "_Amount": "10"
                    }]
            }
        }).respond({ success: 'success' });

        payment.authRequest('info', cards);

        $httpBackend.flush();
    });

    it('should call the payment service auth request endpoint with Credit Card', function () {
        var cards = [
        {
            chargeAmount: 10,
            cardType: 'CreditCard',
            token: '11111',
            paymentType: {
                authType: 'Credit'
            }
        }];

        spyOn(POSService, 'getPOSParameters').and.callFake(function () {
            return {
                associateId: '111111',
                storeNumber: '101',
                terminalNumber: '123',
                roles: 'ENDUSER, LUFI_ORDER_MODIFY'
            };
        });

        $httpBackend.expectPOST(serviceURL + "/CreditService/AuthRequest", {
            "CreditAuthRequest":
                {
                    "_OriginStoreNum": "101",
                    "_FulfillmentStoreNum": "101",
                    "_SourceApplication": "Sterling",
                    "_TotalToAuthorize": "10",
                    "_ProvideAddressFlag": "N",
                    "_ValidateAVSInd": "None",
                    "_TenderCount": "1",
                    "_FloorLimitEnable": "N",
                    "TenderList": [
                        {
                            "_TenderType": "Credit",
                            "_Token": "11111",
                            "_ExpirationDate": "2016-12-31",
                            "_Amount": "10"
                        }]
                }
        }).respond({ success: 'success' });

        payment.authRequest('info', cards);

        $httpBackend.flush();
    });

    it('should get, set, and clear the payment data', function () {
        expect(payment.getPaymentData()).toBe(undefined);
        payment.setPaymentData('abc');
        expect(payment.getPaymentData()).toBe('abc');
        payment.clearPaymentData();
        expect(payment.getPaymentData()).toBe(undefined);
    });

    it('should create a reprice payment contract', function () {
        var cards = [
            {
                chargeAmount: 10,
                cardType: 'PLCC',
                token: '11111',
                paymentType: {
                    authType: 'BankCard'
                }
            },
            {
                chargeAmount: 10,
                cardType: 'GIFTCARD',
                token: '11111',
                giftCardPin: '123456'
            },
            {
                chargeAmount: 10,
                cardType: 'CreditCard',
                token: '22222',
                paymentType: {
                    authType: 'Credit'
                }
            }
        ];

        expect(payment.createRepricePaymentContract(cards)).toEqual([
            {
                _CreditCardNo: '11111',
                _CreditCardType: 'BS',
                _PaymentType: 'CREDIT_CARD'
            },
            {
                _CreditCardNo: '22222',
                _CreditCardType: 'CreditCard',
                _PaymentType: 'CREDIT_CARD'
            }
        ]);
    });

    it('should create an order payment contract', function () {
        var cards = [
            {
                chargeAmount: 10,
                cardType: 'PLCC',
                token: '11111',
                cardNumber: '1111222233334444',
                paymentType: {
                    authType: 'BankCard'
                }
            },
            {
                chargeAmount: 10,
                cardType: 'GIFTCARD',
                token: '11111',
                cardNumber: '2222333344445555',
                giftCardPin: '123456'
            },
            {
                chargeAmount: 10,
                cardType: 'CreditCard',
                token: '22222',
                cardNumber: '3333444455556666',
                paymentType: {
                    authType: 'Credit'
                }
            }
        ];

        expect(payment.createOrderPaymentContract(cards)).toEqual([
            { 
                _BillToKey: '',
                _ChargeSequence: '1',
                _CreditCardExpDate: '12/2016',
                _CreditCardName: undefined,
                _CreditCardNo: '11111',
                _CreditCardType: 'PLCC',
                _DisplayCreditCardNo: '1111222233334444',
                _FirstName: '',
                _LastName: '',
                _MiddleName: '',
                _PaymentType: 'CREDIT_CARD',
                _UnlimitedCharges: 'Y',
                PaymentDetails: {
                    _AuthAvs: '',
                    _AuthCode: '',
                    _AuthorizationID: '',
                    _AuthorizationExpirationDate: '',
                    _AuthReturnCode: 'APPROVED',
                    _AuthReturnFlag: 'Y',
                    _AuthReturnMessage: '202134',
                    _AuthTime: '',
                    _ChargeType: 'AUTHORIZATION',
                    _InternalReturnCode: '',
                    _InternalReturnFlag: '',
                    _InternalReturnMessage: '',
                    _ProcessedAmount: 10,
                    _Reference1: '',
                    _Reference2: '',
                    _RequestAmount: 10,
                    _RequestId: undefined,
                    _RequestProcessed: 'Y',
                    _TranReturnCode: 'APPROVED',
                    _TranReturnFlag: 'SUCCESS',
                    _TranReturnMessage: '',
                    _TranType: 'AUTHORIZATION'
                }
            },
            {
                _BillToKey: '',
                _ChargeSequence: '1',
                _CreditCardExpDate: '12/2016',
                _CreditCardName: '',
                _CreditCardNo: '2222333344445555',
                _CreditCardType: 'GC',
                _DisplayCreditCardNo: '2222333344445555',
                _MaxChargeLimit: 10,
                _PaymentReference1: '',
                _PaymentReference2: '123456',
                _PaymentType: 'GC',
                _SvcNo: '11111',
                _UnlimitedCharges: 'N',
                PaymentDetails: {
                    _AuthAvs: '',
                    _AuthCode: '',
                    _AuthorizationExpirationDate: '',
                    _AuthorizationID: '',
                    _AuthReturnCode: 'APPROVED',
                    _AuthReturnFlag: 'Y',
                    _AuthReturnMessage: '',
                    _AuthTime: '',
                    _ChargeType: 'AUTHORIZATION',
                    _InternalReturnCode: '',
                    _InternalReturnFlag: '',
                    _InternalReturnMessage: '',
                    _ProcessedAmount: 10,
                    _Reference1: '',
                    _Reference2: '123456',
                    _RequestAmount: 10,
                    _RequestId: '',
                    _RequestProcessed: 'Y',
                    _TranReturnCode: 'APPROVED',
                    _TranReturnFlag: 'SUCCESS',
                    _TranReturnMessage: '',
                    _TranType: 'AUTHORIZATION'
                }
            },
            {
                _BillToKey: '',
                _ChargeSequence: '1',
                _CreditCardExpDate: '12/2016',
                _CreditCardName: undefined,
                _CreditCardNo: '22222',
                _CreditCardType: 'CreditCard',
                _DisplayCreditCardNo: '3333444455556666',
                _FirstName: '',
                _LastName: '',
                _MiddleName: '',
                _PaymentType: 'CREDIT_CARD',
                _UnlimitedCharges: 'Y',
                PaymentDetails: {
                    _AuthAvs: '',
                    _AuthCode: '',
                    _AuthorizationID: '',
                    _AuthorizationExpirationDate: '',
                    _AuthReturnCode: 'APPROVED',
                    _AuthReturnFlag: 'Y',
                    _AuthReturnMessage: '202134',
                    _AuthTime: '',
                    _ChargeType: 'AUTHORIZATION',
                    _InternalReturnCode: '',
                    _InternalReturnFlag: '',
                    _InternalReturnMessage: '',
                    _ProcessedAmount: 10,
                    _Reference1: '',
                    _Reference2: '',
                    _RequestAmount: 10,
                    _RequestId: undefined,
                    _RequestProcessed: 'Y',
                    _TranReturnCode: 'APPROVED',
                    _TranReturnFlag: 'SUCCESS',
                    _TranReturnMessage: '',
                    _TranType: 'AUTHORIZATION'
                }
            }
        ]);
    });

    it('should return only gift cards when a credit card has been added', function () {
        var paymentData = {
            cards: [
                {
                    cardNumber: "************0007",
                    cardType: "VISA",
                    chargeAmount: "22.00",
                    cid: "382",
                    exp: "1215",
                    name: "user",
                    token: "11111",
                    paymentType: {
                        authType: "BankCard",
                        contractType: "CREDITCARD",
                        label: "Bank/Credit Card",
                        msrType: "BankCard"
                    },
                    rePriceContract: {
                        _CreditCardNo: "3293111521374641",
                        _CreditCardType: "VISA",
                        _PaymentType: "CREDIT_CARD"
                    }
                }
            ]
        };
        expect(payment.getAvailablePaymentTypes(paymentData)).toEqual([
            {
                label: 'Gift Card',
                contractType: 'GIFTCARD',
                authType: 'GiftCard',
                msrType: 'GiftCard'
            }
        ]);
    });

    it('should return only gift cards when a PLCC has been added', function () {
        var paymentData = {
            cards: [
                {
                    cardNumber: "************0007",
                    cardType: "PLCC",
                    chargeAmount: "22.00",
                    name: "user",
                    token: "11111",
                    paymentType: {
                        label: 'Store Charge',
                        contractType: 'CREDITCARD',
                        authType: 'PLCC',
                        msrType: "PLCC"
                    }
                }
            ]
        };
        expect(payment.getAvailablePaymentTypes(paymentData)).toEqual([
            {
                label: 'Gift Card',
                contractType: 'GIFTCARD',
                authType: 'GiftCard',
                msrType: 'GiftCard'
            }
        ]);
    });

    it('should return all types when 1 gift card has been added', function () {
        var paymentData = {
            cards: [
                {
                    cardNumber: "************0007",
                    pin: "123456",
                    chargeAmount: "22.00",
                    token: "11111",
                    paymentType: {
                        label: 'Gift Card',
                        contractType: 'GIFTCARD',
                        authType: 'GiftCard',
                        msrType: "GiftCard"
                    }
                }
            ]
        };
        expect(payment.getAvailablePaymentTypes(paymentData)).toEqual([
            {
                label: 'Gift Card',
                contractType: 'GIFTCARD',
                authType: 'GiftCard',
                msrType: 'GiftCard'
            },
            {
                label: 'Bank/Credit Card',
                contractType: 'CREDITCARD',
                authType: 'BankCard',
                msrType: 'BankCard'
            },
            {
                label: 'Store Charge',
                contractType: 'CREDITCARD',
                authType: 'PLCC',
                msrType: 'PLCC'
            }
        ]);
    });

    it('should return all types except gift card when 4 gift cards have been added', function () {
        var paymentData = {
            cards: [
                {
                    cardNumber: "************0007",
                    pin: "123456",
                    chargeAmount: "22.00",
                    token: "11111",
                    paymentType: {
                        label: 'Gift Card',
                        contractType: 'GIFTCARD',
                        authType: 'GiftCard',
                        msrType: "GiftCard"
                    }
                },
                {
                    cardNumber: "************0008",
                    pin: "123456",
                    chargeAmount: "22.00",
                    token: "11111",
                    paymentType: {
                        label: 'Gift Card',
                        contractType: 'GIFTCARD',
                        authType: 'GiftCard',
                        msrType: "GiftCard"
                    }
                },
                {
                    cardNumber: "************0009",
                    pin: "123456",
                    chargeAmount: "22.00",
                    token: "11111",
                    paymentType: {
                        label: 'Gift Card',
                        contractType: 'GIFTCARD',
                        authType: 'GiftCard',
                        msrType: "GiftCard"
                    }
                },
                {
                    cardNumber: "************0000",
                    pin: "123456",
                    chargeAmount: "22.00",
                    token: "11111",
                    paymentType: {
                        label: 'Gift Card',
                        contractType: 'GIFTCARD',
                        authType: 'GiftCard',
                        msrType: "GiftCard"
                    }
                }
            ]
        };
        expect(payment.getAvailablePaymentTypes(paymentData)).toEqual([
            {
                label: 'Bank/Credit Card',
                contractType: 'CREDITCARD',
                authType: 'BankCard',
                msrType: 'BankCard'
            },
            {
                label: 'Store Charge',
                contractType: 'CREDITCARD',
                authType: 'PLCC',
                msrType: 'PLCC'
            }
        ]);
    });

    it('should return only gift card when an ACC number has been added', function () {
        var paymentData = {
            accNumber: '12341234',
            cards: []
        };

        expect(payment.getAvailablePaymentTypes(paymentData)).toEqual([
            {
                label: 'Gift Card',
                contractType: 'GIFTCARD',
                authType: 'GiftCard',
                msrType: "GiftCard"
            }
        ]);
    });
});