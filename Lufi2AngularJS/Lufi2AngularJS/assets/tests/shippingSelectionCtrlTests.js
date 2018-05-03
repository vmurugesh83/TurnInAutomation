describe('Shipping Selection Controller:', function () {
    beforeEach(module('shippingSelection', 'ui.bootstrap', 'ui.router', 'appServicesCustomer', 'appUtilities', 'appServicesWebSocket', 'appServicesItem', 'appServiceReprice'));

    var $controller, controller, orderCart, customer, $httpBackend, $q;

    beforeEach(inject(function (_$controller_, $rootScope, _customer_, _loggerService_, _$state_, _orderCart_, $injector, _$q_) {
        $controller = _$controller_;
        $scope = $rootScope.$new();
        customer = _customer_;
        $state = _$state_;
        orderCart = _orderCart_;
        $q = _$q_;
        loggerService = _loggerService_;
        $httpBackend = $injector.get('$httpBackend');

        spyOn($state, 'go');
        spyOn(customer, 'isCustomerSelected').and.returnValue(true);
        spyOn(customer, 'getSelectedCustomer').and.returnValue({
            CustomerContactList: {
                CustomerContact: [
                    {
                        CustomerAdditionalAddressList: {
                            CustomerAdditionalAddress: [
                                { a: 'a' },
                                { b: 'b' }
                            ]
                        }
                    }]
            }
        });
        spyOn(orderCart.order, 'getLiveOrderCart').and.returnValue({
            OrderLines: {
                OrderLine: [
                    {
                        PersonInfoShipTo: {
                            _PersonInfoKey: '123'
                        },
                        btDisplay: {
                            shippingMethodPrice: '1'
                        },
                        btLogic: {
                            isGiftRegistryAddress: '123 Main St',
                            isSelected: true
                        },
                        Extn: {
                            _ExtnGiftRegistryNo: '12345'
                        }
                    },
                    {
                        PersonInfoShipTo: {
                            _PersonInfoKey: '456'
                        },
                        btDisplay: {
                            shippingMethodPrice: '1'
                        },
                        btLogic: {
                            isGiftRegistryAddress: '456 Main St'
                        },
                        Extn: {
                            _ExtnGiftRegistryNo: '12345'
                        }
                    }
                ]
            }
        });
        spyOn(orderCart.customer, 'setCustomer');
        spyOn(window, 'IScroll');
        spyOn(window, 'swal');

        $httpBackend.expectGET(serviceURL + '/Utility/GetExpectedDeliveryDates').respond({
            ExpectedDeliveryDates: []
        });

        controller = $controller('shippingSelectionCtrl', { $scope: $scope, orderCart: orderCart, $state: $state, $customer: customer });
    }));

    afterEach(function () {
        $httpBackend.flush();
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(controller).toBeDefined();

        expect($scope.isDisplayGiftBoxComponents).toBe(false);

        expect($scope.isPageDataLoaded).toBe(true);
        expect($scope.isCheckOutButtonDisabled).toBe(true);
        expect($scope.cart).toEqual({
            OrderLines: {
                OrderLine: [
                    {
                        PersonInfoShipTo:
                            {
                                _PersonInfoKey: '123'
                            },
                        btDisplay: {
                            shippingMethodPrice: '1'
                        },
                        btLogic: {
                            isGiftRegistryAddress: '123 Main St',
                            isSelected: false
                        },
                        Extn: {
                            _ExtnGiftRegistryNo: '12345'
                        }
                    },
                    {
                        PersonInfoShipTo: {
                            _PersonInfoKey: '456'
                        },
                        btDisplay: {
                            shippingMethodPrice: '1'
                        },
                        btLogic: {
                            isGiftRegistryAddress: '456 Main St',
                            isSelected: false
                        },
                        Extn: {
                            _ExtnGiftRegistryNo: '12345'
                        }
                    }
                ]
            }
        });
        expect($scope.orderLinesArray).toEqual([
            {
                PersonInfoShipTo: 
                    {
                        _PersonInfoKey: '123'
                    },
                btDisplay: {
                    shippingMethodPrice: '1'
                },
                btLogic: {
                    isGiftRegistryAddress: '123 Main St',
                    isSelected: false
                },
                Extn: {
                    _ExtnGiftRegistryNo: '12345'
                }
            },
            {
                PersonInfoShipTo: {
                    _PersonInfoKey: '456'
                },
                btDisplay: {
                    shippingMethodPrice: '1'
                },
                btLogic: {
                    isGiftRegistryAddress: '456 Main St',
                    isSelected: false
                },
                Extn: {
                    _ExtnGiftRegistryNo: '12345'
                }
            }
        ]);
    });

    it('should make call to get lowest price', function () {
        spyOn(orderCart.orderLine.util, 'getLowestSalePrice');

        var orderLine = { name: 'a' };

        $scope.getLowestPrice(orderLine);

        expect(orderCart.orderLine.util.getLowestSalePrice).toHaveBeenCalledWith(orderLine);
    });

    it('should return the first address line', function () {
        var address = {
            _AddressLine1: '123 Main St'
        };

        expect($scope.getConcatAddresses(address)).toEqual('123 Main St');
    });

    it('should return a csv of the 2 addresses', function () {
        var address = {
            _AddressLine1: '123 Main St',
            _AddressLine2: 'Apt 1'
        };

        expect($scope.getConcatAddresses(address)).toEqual('123 Main St, Apt 1');
    });

    it('should return a csv of the 3 addresses', function () {
        var address = {
            _AddressLine1: '123 Main St',
            _AddressLine2: 'Apt 1',
            _AddressLine3: 'line 3'
        };

        expect($scope.getConcatAddresses(address)).toEqual('123 Main St, Apt 1, line 3');
    });

    it('should return N/A when the price is an object', function () {
        expect($scope.printPrice({ abc: 'abc' })).toBe('N/A');
    });

    it('should return the currency equivalent of the number', function () {
        expect($scope.printPrice(3)).toBe('$3.00');
    });

    it('should return the currency equivalent of the string', function () {
        expect($scope.printPrice('3')).toBe('$3.00');
    });

    it('should call computer shipping order line subtotal', function () {
        spyOn(orderCart.subtotal, 'computeShippingOrderLineSubtotal');

        $scope.getLineTotal('orderLine');

        expect(orderCart.subtotal.computeShippingOrderLineSubtotal).toHaveBeenCalledWith('orderLine');
    });

    it('should go to the paymentSummay state if there are no errors', function () {
        var deferred = $q.defer();
        deferred.resolve({
            defaultBillingAddressError: {
                hasError: false
            },
            defaultShipToAddressError: {
                hasError: false
            },
            carrierServiceCodeError: {
                hasError: false
            },
            bigTicketError: {
                hasError: false
            },
            orderlineShiptoAddressError: {
                hasError: false
            }
        });

        spyOn(orderCart.address, 'validateOrderAddresses').and.returnValue(deferred.promise);

        $scope.goToPayment();
    });

    it('should display errors if validation fails', function () {
        var deferred = $q.defer();
        deferred.resolve({
            defaultBillingAddressError: {
                hasError: true,
                errorText: 'default billing error'
            },
            defaultShipToAddressError: {
                hasError: true,
                errorText: 'default ship to error'
            },
            carrierServiceCodeError: {
                hasError: true,
                errorText: 'carrier service error'
            },
            bigTicketError: {
                hasError: true,
                errorText: 'big ticket error'
            },
            orderlineShiptoAddressError: {
                hasError: true,
                errorText: 'orderline ship to error'
            }
        });

        spyOn(orderCart.address, 'validateOrderAddresses').and.returnValue(deferred.promise);

        $scope.goToPayment();
    });

    it('should log the error if the validation call fails', function () {
        var deferred = $q.defer();
        deferred.reject({
            message: 'error'
        });

        spyOn(orderCart.address, 'validateOrderAddresses').and.returnValue(deferred.promise);
        spyOn(loggerService, 'log');

        $scope.goToPayment();
    });

    it('should return an empty string if nothing is passed in', function () {
        spyOn(orderCart.giftOptions, 'getGiftOptionFromOrderLines').and.returnValue();

        expect($scope.getGiftMessage({})).toBe('');
        expect(orderCart.giftOptions.getGiftOptionFromOrderLines).toHaveBeenCalledWith({PrimeLine: undefined});
    });

    it('should return a gift message', function () {
        var orderLine = {
            _SubLineNo: '123',
            _PrimeLineNo: '456'
        };

        spyOn(orderCart.giftOptions, 'getGiftOptionFromOrderLines').and.returnValue({
            To: 'recipient',
            From: 'sender',
            Message: 'hello'
        });

        expect($scope.getGiftMessage(orderLine)).toBe('To: recipient From: sender Message: hello');
        expect(orderCart.giftOptions.getGiftOptionFromOrderLines).toHaveBeenCalledWith({
            PrimeLine: '456',
            SubLine: '123'
        });
    });

    it('should return false when no lines are selected', function () {
        expect($scope.allLinesChecked()).toBe(false);
    });

    it('should set the isLineSelected scope variable to false when the line is selected', function () {
        var orderLine = {
            PersonInfoShipTo: {
                _PersonInfoKey: '123'
            },
            btDisplay: {
                shippingMethodPrice: '1'
            },
            btLogic: {
                isGiftRegistryAddress: '123 Main St',
                isSelected: true
            },
            Extn: {
                _ExtnGiftRegistryNo: '12345'
            }
        };
        
        $scope.selectLine(orderLine);

        expect($scope.isLineSelected).toBe(false);
    });

    it('should set the isLineSelected scope variable to true when the line is not selected', function () {
        var orderLine = {
            PersonInfoShipTo: {
                _PersonInfoKey: '123'
            },
            btDisplay: {
                shippingMethodPrice: '1'
            },
            btLogic: {
                isGiftRegistryAddress: '123 Main St',
                isSelected: false
            },
            Extn: {
                _ExtnGiftRegistryNo: '12345'
            }
        };
        
        $scope.selectLine(orderLine);

        expect($scope.isLineSelected).toBe(true);
    });

    it('should call ordercart.giftOptions.addGiftBox with an array if the selected line has no PrimeLineNo', function () {
        spyOn(orderCart.giftOptions, 'addGiftBox');
        $scope.selectLine($scope.orderLinesArray[0]);
        $scope.addMultiGiftBox();
        expect(orderCart.giftOptions.addGiftBox).toHaveBeenCalledWith([]);
    });

    it('should call updateShipTo with all selected lines', function () {
        spyOn($scope, 'updateShipTo');
        $scope.selectLine($scope.orderLinesArray[0]);
        $scope.updateMultiShipTo();
        expect($scope.updateShipTo).toHaveBeenCalledWith([{
            PersonInfoShipTo: {
                _PersonInfoKey: '123'
            },
            btDisplay: {
                shippingMethodPrice: '1'
            },
            btLogic: {
                isGiftRegistryAddress: '123 Main St',
                isSelected: true
            },
            Extn: {
                _ExtnGiftRegistryNo: '12345'
            }
        }]);
    });
});