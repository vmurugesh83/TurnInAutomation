describe('shippingSpeedMultiMod Controller', function () {
    beforeEach(module('shippingSelection', 'ui.bootstrap', 'ui.bootstrap.modal', 'appServiceOrderCart', 'appServicesGiftRegistry', 'appUtilities', 'appServicesWebSocket', 'appServicesItem', 'appServiceReprice', 'appServicesCustomer', 'appServiceAppState'));

    var $controller, controller, $httpBackend;
    var modalInstance, orderCart;

    beforeEach(inject(function (_$controller_, $rootScope, $injector, _orderCart_) {
        $controller = _$controller_;
        $scope = $rootScope.$new();
        $httpBackend = $injector.get('$httpBackend');
        orderCart = _orderCart_;

        modalInstance = {
            close: jasmine.createSpy('modalInstance.close'),
            dismiss: jasmine.createSpy('modalInstance.dismiss'),
            result: {
                then: jasmine.createSpy('modalInstance.result.then')
            }
        };

        var orderlines = [
            {
                _PrimeLineNo: '123',
                _SubLineNo: '456',
                CarrierServiceList: {
                    CarrierService: [
                    {
                        _CarrierServiceCode: 'UPS-GRND',
                        _CarrierServiceDesc: 'Ground Shipping'
                    },
                    {
                        _CarrierServiceCode: 'UPS-NDAY',
                        _CarrierServiceDesc: 'N Day Shipping'
                    }
                    ]
                }
            },
            {
                _PrimeLineNo: '234',
                _SubLineNo: '567',
                CarrierServiceList: {
                    CarrierService: [
                    {
                        _CarrierServiceCode: 'UPS-GRND',
                        _CarrierServiceDesc: 'Ground Shipping'
                    },
                    {
                        _CarrierServiceCode: 'UPS-NDAY',
                        _CarrierServiceDesc: 'N Day Shipping'
                    }
                    ]
                }
            }
        ];

        var expectedShipDatesMap = {
            'UPS-GRND': "Sep 22, 2015",
            'UPS-NDAY': "Sep 16, 2015",
            'UPS-SR2DAY': "Sep 17, 2015",
            'UPS-TDAY': "Sep 17, 2015"
        };

        spyOn(orderCart.carrierService, 'setOrderlineCarrierService');
        spyOn(orderCart.order, 'deleteShippingDiscount').and.returnValue('new order line');

        controller = $controller('shippingSpeedMultiModCtrl', { $scope: $scope, $modalInstance: modalInstance, orderlines: orderlines, expectedShipDatesMap: expectedShipDatesMap });
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(controller).toBeDefined();

        expect($scope.shippingSpeeds).toEqual([
            {
                _CarrierServiceCode: 'UPS-GRND',
                _CarrierServiceDesc: 'Ground Shipping',
                _Currency: 'USD',
                _Price: '-1'
            },
            {
                _CarrierServiceCode: 'UPS-NDAY',
                _CarrierServiceDesc: 'N Day Shipping',
                _Currency: 'USD',
                _Price: '-1'
            }
        ]);
    });

    it('should set the service to the lowest cost shipping', function () {
        var carrierService = {
            _CarrierServiceDesc: 'Lowest Cost Shipping'
        };

        spyOn(orderCart.carrierService, 'setOrderLineCarrierServiceToLowestPrice');

        $scope.saveService(carrierService);
        expect(orderCart.carrierService.setOrderLineCarrierServiceToLowestPrice).toHaveBeenCalledWith({
            _PrimeLineNo: '123',
            _SubLineNo: '456',
            CarrierServiceList: {
                CarrierService: [
                    {
                        _CarrierServiceCode: 'UPS-GRND',
                        _CarrierServiceDesc: 'Ground Shipping'
                    },
                    {
                        _CarrierServiceCode: 'UPS-NDAY',
                        _CarrierServiceDesc: 'N Day Shipping'
                    }
                ]
            }
        });
        expect(modalInstance.close).toHaveBeenCalled();
    });

    it('should set the service to the fastest shipping', function () {
        var carrierService = {
            _CarrierServiceDesc: 'Fastest Shipping'
        };

        $scope.saveService(carrierService);
        expect(orderCart.carrierService.setOrderlineCarrierService).toHaveBeenCalledWith({
            _PrimeLineNo: '123',
            _SubLineNo: '456',
            CarrierServiceList: {
                CarrierService: [
                {
                    _CarrierServiceCode: 'UPS-GRND',
                    _CarrierServiceDesc: 'Ground Shipping'
                },
                {
                    _CarrierServiceCode: 'UPS-NDAY',
                    _CarrierServiceDesc: 'N Day Shipping'
                }
                ]
            }
        }, {
            _CarrierServiceCode: 'UPS-NDAY',
            _CarrierServiceDesc: 'N Day Shipping'
        });
        expect(modalInstance.close).toHaveBeenCalled();
    });

    it('should set the shipping for each line of the order', function () {
        var carrierService = {
            _CarrierServiceDesc: 'Ground Shipping',
            _CarrierServiceCode: 'UPS-GRND'
        };

        $scope.saveService(carrierService);
        expect(orderCart.carrierService.setOrderlineCarrierService.calls.count()).toBe(2);
    });

    it('should return an empty string when there is no price', function () {
        var carrierService = {
            _Price: ''
        };

        expect($scope.priceText(carrierService)).toBe('');
    });

    it('should return a message when the price is -1', function () {
        var carrierService = {
            _Price: '-1'
        };

        expect($scope.priceText(carrierService)).toBe('Various Orderline Shipping Prices');
    });

    it('should return the price', function () {
        var carrierService = {
            _Price: '10'
        };

        expect($scope.priceText(carrierService)).toBe('$10.00');
    });

    it('should dismiss the modal', function () {
        $scope.cancel();

        expect(modalInstance.dismiss).toHaveBeenCalled();
    });
});