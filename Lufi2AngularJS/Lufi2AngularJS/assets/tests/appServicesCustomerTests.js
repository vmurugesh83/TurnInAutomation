describe("Customer", function () {
    var customer, loggerService, $httpBackend, $rootScope;

    beforeEach(angular.mock.module('appUtilities', 'appServicesWebSocket', 'appServicesItem'));
    beforeEach(module('appServicesCustomer'));

    beforeEach(inject(function (_customer_, _loggerService_, _$cacheFactory_, $injector, _$rootScope_) {
        customer = _customer_;
        loggerService = _loggerService_;
        $rootScope = _$rootScope_;
        $cacheFactory = _$cacheFactory_;
        $httpBackend = $injector.get('$httpBackend');
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(customer).not.toBeUndefined();
    });

    it('should call service to verify the email passed in', function () {
        $httpBackend.when('POST', serviceURL + '/Validate/Email').respond('valid');
        $httpBackend.expectPOST(serviceURL + '/Validate/Email');
        customer.verifyEmail();
        $httpBackend.flush();
    });

    it('should call the AVS service to verify selected address', function () {
        $httpBackend.when('POST', serviceURL + '/Utility/AVS/DoGetAddress').respond('valid');
        $httpBackend.expectPOST(serviceURL + '/Utility/AVS/DoGetAddress');
        customer.verifySelectedAddress();
        $httpBackend.flush();
    });

    it('should call customer search and call success function', function () {
        $httpBackend.when('POST', serviceURL + "/customerSearchJSON").respond({
            Customer: {
                Addresses: {
                    Address: {
                        CustomerKey: "123",
                        IsDefaultBillTo: 'Y'
                    }
                }
            }
        });
        $httpBackend.expectPOST(serviceURL + "/customerSearchJSON");

        var successCalls = 0;
        customer.customerSearch('{user: user}', function (customerArray) {
            expect(customerArray.length).toBe(1);
            successCalls = successCalls + 1;
        }, function () { });

        $httpBackend.flush();
        expect(successCalls).toBe(1);
    });

    it('should set the selected customer in the cache and broadcast it', function () {
        spyOn(loggerService, 'log');
        var broadcastCalled;
        $rootScope.$on('customerSelected', function () {
            broadcastCalled = true;
        });

        customer.setSelectedCustomer('customerObj');

        expect(loggerService.log).toHaveBeenCalledWith('customerObj');
        expect(customer.getSelectedCustomer()).toBe('customerObj');
        expect(broadcastCalled).toBe(true);
    });

    it('should not retrieve orders if no customer is set', function () {
        var failureCalled = false;
        customer.retrieveDraftOrders(function () { }, function (message) {
            expect(message).toBe("No Customer Selected. Use Customer Search first.");
            failureCalled = true;
        });
        expect(failureCalled).toBeTruthy();
    });

    it('should use selected customer to do draft order search', function () {
        customer.setSelectedCustomer({ _CustomerID: '1234' });

        var successCalled = false;
        var failureCalled = false;

        var orders = [{ name: 'order1' }, { name: 'order2' }];

        $httpBackend.when('POST', serviceURL + "/Order/GetOrderListJSON").respond({
            GetSterlingOrderListResp: {
                Order: orders
            }
        });
        $httpBackend.expectPOST(serviceURL + "/Order/GetOrderListJSON", {
            "GetSterlingOrderListReq":
            {
                "_ReadFromHistory": "N",
                "_DocumentType": "0001",
                "_DraftOrderFlag": "Y",
                "_BillToID": "1234",
                "_BillToIDQryType": "EQ"
            }
        });

        customer.retrieveDraftOrders(function (returnedOrders) {
            expect(returnedOrders.length).toBe(2);
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });

    it('should use selected customer to do a confirmed order search', function () {
        customer.setSelectedCustomer({ _CustomerID: '1234' });

        var successCalled = false;
        var failureCalled = false;

        var orders = [{ name: 'order1' }, { name: 'order2' }];

        $httpBackend.when('POST', serviceURL + "/Order/GetOrderListJSON").respond({
            GetSterlingOrderListResp: {
                Order: orders
            }
        });
        $httpBackend.expectPOST(serviceURL + "/Order/GetOrderListJSON", {
            "GetSterlingOrderListReq":
            {
                "_ReadFromHistory": "N",
                "_DocumentType": "0001",
                "_DraftOrderFlag": "N",
                "_BillToID": "1234",
                "_BillToIDQryType": "EQ"
            }
        });

        customer.retrieveConfirmedOrders(function (returnedOrders) {
            expect(returnedOrders.length).toBe(2);
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });

    it('should use selected customer to do a returned order search', function () {
        customer.setSelectedCustomer({ _CustomerID: '1234' });

        var successCalled = false;
        var failureCalled = false;

        var orders = [{ name: 'order1' }, { name: 'order2' }];

        $httpBackend.when('POST', serviceURL + "/Order/GetOrderListJSON").respond({
            GetSterlingOrderListResp: {
                Order: orders
            }
        });
        $httpBackend.expectPOST(serviceURL + "/Order/GetOrderListJSON", {
            "GetSterlingOrderListReq":
            {
                "_ReadFromHistory": "N",
                "_DocumentType": "0003",
                "_DraftOrderFlag": "",
                "_BillToID": "1234",
                "_BillToIDQryType": "EQ"
            }
        });

        customer.retrieveReturnOrders(function (returnedOrders) {
            expect(returnedOrders.length).toBe(2);
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });

    it('should call getDetails when a customer id and key are provided', function () {
        var custObj = {
            CustomerId: '123',
            CustomerKey: 'ABC'
        };

        $httpBackend.when('POST', serviceURL + "/Customer/getDetails").respond({
            getCustomerDetailsResponse: {
                Customer: {
                    CustomerContactList: {
                        CustomerContact: {}
                    }
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/getDetails");

        customer.customerAction(custObj, function () { }, function () { });

        $httpBackend.flush();
    });

    it('should create a customer then call getDetails when a customer id and key are not provided', function () {
        var successCalled = false;
        var failureCalled = false;

        var custObj = {
            CustomerId: '',
            CustomerKey: ''
        };

        $httpBackend.when('POST', serviceURL + "/Customer/manage").respond({
            manageCustomerResp: {
                'return': {
                    _CustomerKey: '123',
                    _CustomerID: 'ABC'
                }
            }
        });

        $httpBackend.when('POST', serviceURL + "/Customer/getDetails").respond({
            getCustomerDetailsResponse: {
                Customer: {
                    CustomerContactList: {
                        CustomerContact: {}
                    }
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/manage");
        $httpBackend.expectPOST(serviceURL + "/Customer/getDetails");

        customer.customerAction(custObj, function () {
            successCalled = true;
            expect(custObj.CustomerId).toBe('ABC');
            expect(custObj.CustomerKey).toBe('123');
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });

    it('should add the customer to the cart when a customer is provided', function () {
        var customerSelected = false;
        var addedToCart = false;

        spyOn(loggerService, 'log');

        var custObj = { CustomerId: 'abc', CustomerKey: '123' };

        $rootScope.$on('customerSelected', function () {
            customerSelected = true;
        });

        $rootScope.$on('AddCustomerToCartCalled', function (returnedCustomer) {
            addedToCart = true;
        });

        customer.addToCart(custObj);
        
        expect(customerSelected).toBe(true);
        expect(addedToCart).toBe(true);
        expect(loggerService.log).toHaveBeenCalledWith(custObj);
    });

    it('should retrieve the selected customer for the cart when no customer is provided', function () {
        var addedToCart = false;

        $rootScope.$on('AddCustomerToCartCalled', function (returnedCustomer) {
            addedToCart = true;
        });

        customer.setSelectedCustomer('Test Customer');

        customer.addToCart();

        expect(addedToCart).toBe(true);
    });

    it('should call service to verify the address', function () {
        var address = '123 Main St';
        var errorCalled = false;

        $httpBackend.when('POST', serviceURL + "/Utility/AVS/DoSearch").respond({});

        $httpBackend.expectPOST(serviceURL + "/Utility/AVS/DoSearch", {
            AVSInput: {
                QASearch: {
                    Country: "USA",
                    Engine: {
                        _Flatten: "true",
                        _Intensity: "Close",
                        _PromptSet: "Default",
                        _Threshold: "100",
                        "#text": "Verification",
                    },
                    "Layout": "Database layout",
                    "Search": address,
                    "FormattedAddressInPicklist": "false",
                    "_Localisation": "USA"

                }
            }
        });

        customer.verifyCustomerAddress(address, function () { }, function () {
            errorCalled = true;
        });

        $httpBackend.flush();

        expect(errorCalled).toBe(false);
    });

    it('should call the service to delete the address', function () {
        var successCalled = false;

        var customerID = '123', customerContactID = '456', customerAdditionalAddressID = '789';

        $httpBackend.when('POST', serviceURL + "/Customer/manage").respond({
            manageCustomerResp: {
                'return': {
                    _CustomerID: '234',
                    _CustomerKey: 'abc'
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/manage", {
            "manageCustomerReq": {
                "input": {
                    "_CustomerID": customerID.toString(),
                    "_CustomerType": "02",
                    "_OrganizationCode": "BONTON",
                    "CustomerContactList": {
                        "CustomerContact": [
                          {
                              "_CustomerContactID": customerContactID.toString(),
                              "CustomerAdditionalAddressList": {
                                  "CustomerAdditionalAddress": [
                                    {
                                        "_CustomerAdditionalAddressID": customerAdditionalAddressID.toString(),
                                        "_Operation": "Delete"
                                    }
                                  ]
                              }
                          }
                        ]
                    },
                    "_Status": "10"
                }
            }
        });

        $httpBackend.when('POST', serviceURL + "/Customer/getDetails").respond({
            getCustomerDetailsResponse: {
                Customer: {
                    CustomerContactList: {
                        CustomerContact: {}
                    }
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/getDetails");

        customer.deleteAddress(customerID, customerContactID, customerAdditionalAddressID, function () {
            successCalled = true;
        }, function () { });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
    });

    it('should clear the selected customer from the cache', function () {
        customer.clearSelectedCustomer();
        expect(customer.getSelectedCustomer()).toBeUndefined();
    });

    it('should retrieve the customer details', function () {
        var customerId= '123', customerKey = 'abc';
        var successCalled = false, failureCalled = false, customerSelected = false;

        $httpBackend.when('POST', serviceURL + "/Customer/getDetails").respond({
            getCustomerDetailsResponse: {
                Customer: {
                    CustomerContactList: {
                        CustomerContact: {}
                    }
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/getDetails", {
            getCustomerDetailsReq: {
                input: {
                    _CustomerID: customerId,
                    _CustomerKey: customerKey,
                    _InheritFromParents: "?",
                    _OrganizationCode: "BONTON",
                    CustomerContact: {
                        _CustomerContactID: "",
                        _UserID: ""
                    }
                }
            }
        });

        $rootScope.$on('customerSelected', function (returnedCustomer) {
            customerSelected = true;
        });

        spyOn(loggerService, 'log');

        customer.retrieveCustomerDetail(customerId, customerKey, function () {
            successCalled = true;
        }, function() {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(loggerService.log).toHaveBeenCalledWith({
            CustomerContactList: {
                CustomerContact: {}
            },
            defaultAddresses: {
                PersonInfoBillTo: null,
                PersonInfoShipTo: null
            }
        });
        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
        expect(customerSelected).toBe(true);
    });

    it('should create a customer from an address', function () {
        var successCalled = false, failureCalled = false;

        var address = {
            PersonInfo: {
                _DayPhone: '1234567890',
                _EMailID: 'a@a.com',
                _EveningPhone: '2345678901',
                _FirstName: 'Test',
                _LastName: 'User',
                _MiddleName: 'Customer'
            }
        };

        $httpBackend.when('POST', serviceURL + "/Customer/manage").respond({
            manageCustomerResp: {
                'return': {
                    _CustomerID: '123',
                    _CustomerKey: 'abc'
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/manage", {
            "manageCustomerReq": {
                "input": {
                    "_CustomerID": "",
                    "_CustomerKey": "",
                    "_CustomerType": "02",
                    "_OrganizationCode": "BONTON",
                    "_Status": "10",
                    "CustomerContactList": {
                        "CustomerContact": [{
                            "_CustomerContactID": "",
                            "CustomerAdditionalAddressList": { "CustomerAdditionalAddress": [address] },
                            "_DateOfBirth": "",
                            "_DayFaxNo": "",
                            "_DayPhone": address.PersonInfo._DayPhone,
                            "_Department": "",
                            "_EmailID": address.PersonInfo._EMailID,
                            "_EveningFaxNo": "",
                            "_EveningPhone": address.PersonInfo._EveningPhone,
                            "_FirstName": address.PersonInfo._FirstName,
                            "_JobTitle": "",
                            "_LastName": address.PersonInfo._LastName,
                            "_MiddleName": address.PersonInfo._MiddleName,
                            "_MobilePhone": "",
                            "_SpouseDateOfBirth": "",
                            "_Title": "",
                            "_UserID": "",
                            "_WeddingAnniversaryDate": ""
                        }]
                    }
                }
            }
        });

        $httpBackend.when('POST', serviceURL + "/Customer/getDetails").respond({
            getCustomerDetailsResponse: {
                Customer: {
                    CustomerContactList: {
                        CustomerContact: {}
                    }
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/getDetails", {
            getCustomerDetailsReq: {
                input: {
                    _CustomerID: '123',
                    _CustomerKey: 'abc',
                    _InheritFromParents: "?",
                    _OrganizationCode: "BONTON",
                    CustomerContact: {
                        _CustomerContactID: "",
                        _UserID: ""
                    }
                }
            }
        });

        customer.createCustomerByAddress(address, function () {
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });

    it('should not update when there is no customer contact', function () {
        var successCalled = false, failureCalled = false;
        var customerObj = {
            CustomerContactList: {}
        };

        customer.updateCustomerContact(customerObj, function () {
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        expect(successCalled).toBe(false);
        expect(failureCalled).toBe(false);
    });

    it('should update the customer contact information', function () {
        var successCalled = false, failureCalled = false;
        var customerObj = {
            CustomerContactList: {
                CustomerContact: []
            }
        };

        $httpBackend.expectPOST(serviceURL + '/Customer/manage', { manageCustomerReq: { input: customerObj } }).respond({
            manageCustomerResp: {
                'return': {
                    _CustomerID: '123',
                    _CustomerKey: 'abc'
                }
            }
        });

        $httpBackend.when('POST', serviceURL + "/Customer/getDetails").respond({
            getCustomerDetailsResponse: {
                Customer: {
                    CustomerContactList: {
                        CustomerContact: {}
                    }
                }
            }
        });

        $httpBackend.expectPOST(serviceURL + "/Customer/getDetails", {
            getCustomerDetailsReq: {
                input: {
                    _CustomerID: '123',
                    _CustomerKey: 'abc',
                    _InheritFromParents: "?",
                    _OrganizationCode: "BONTON",
                    CustomerContact: {
                        _CustomerContactID: "",
                        _UserID: ""
                    }
                }
            }
        });

        customer.updateCustomerContact(customerObj, function () {
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });
});