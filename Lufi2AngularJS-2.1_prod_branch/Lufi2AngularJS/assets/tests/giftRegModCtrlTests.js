describe('GiftRegMod Controller', function () {
    beforeEach(module('shippingSelection', 'ui.bootstrap', 'ui.bootstrap.modal', 'appServiceOrderCart', 'appServicesGiftRegistry', 'appUtilities', 'appServicesWebSocket', 'appServicesItem', 'appServiceReprice', 'appServicesCustomer'));

    var $controller, controller, $httpBackend;
    var orderCart, modalInstance, logerService, giftRegistryService;

    beforeEach(inject(function (_orderCart_, _loggerService_, _giftRegistryService_, _$controller_, $rootScope, $injector) {
        $controller = _$controller_;
        $scope = $rootScope.$new();
        $httpBackend = $injector.get('$httpBackend');

        orderCart = _orderCart_;
        loggerService = _loggerService_;
        giftRegistryService = _giftRegistryService_;

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
                _SubLineNo: '456'
            }
        ];
        var show = { 'isGiftMessageShow': true, 'isGiftRegistyShow': true, 'isGiftBoxShow': true };
        spyOn(orderCart.giftOptions, 'getGiftOptionFromOrderLines').and.returnValue({
            To: 'recipient',
            From: 'sender',
            Message: 'hello',
            Registry: '12345'
        });

        controller = $controller('GiftRegModCtrl', { $scope: $scope, $modalInstance: modalInstance, orderlines: orderlines, show: show });
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(controller).toBeDefined();

        expect($scope.orderlines).toEqual([{
            _PrimeLineNo: '123',
            _SubLineNo: '456'
        }]);
        expect($scope.To).toBe("recipient");
        expect($scope.From).toBe("sender");
        expect($scope.Message).toBe("hello");
        expect($scope.Registry).toBe("12345");
        expect($scope.isGiftMessageShow).toBe(true);
        expect($scope.isGiftRegistyShow).toBe(true);
        expect($scope.isGiftBoxShow).toBe(true);
        expect($scope.isShowInvalidRegError).toBe(false);

        expect(orderCart.giftOptions.getGiftOptionFromOrderLines).toHaveBeenCalledWith([{
            PrimeLine: '123',
            SubLine: '456'
        }]);
    });

    it('should dismiss the modal', function () {
        $scope.cancel();
        expect(modalInstance.dismiss).toHaveBeenCalled();
    });

    it('should save the gift registry', function () {
        spyOn(orderCart.giftOptions, 'setGiftMessage');
        spyOn(loggerService, 'log');

        $httpBackend.expectPOST(serviceURL.toString() + '/GiftRegistry/GetPreferredAddress').respond({
            PreferredAddressOutput: {
                PreferredAddressResponse: {
                    IsValid: 'true',
                    HasErrors: 'false'
                }
            }
        });
        spyOn(giftRegistryService, 'constructRegistryAddress').and.returnValue('123 Main St');
        spyOn(orderCart.giftOptions, 'setGiftRegistry');

        $scope.save();

        expect(orderCart.giftOptions.setGiftMessage).toHaveBeenCalledWith([
            {
                PrimeLine: '123',
                SubLine: '456'
            }
        ], {
            To: 'recipient',
            From: 'sender',
            Message: 'hello',
        });

        expect(loggerService.log).toHaveBeenCalledWith('GiftBox is not implemented');
        

        $httpBackend.flush();

        expect(giftRegistryService.constructRegistryAddress).toHaveBeenCalledWith('12345', {
            IsValid: 'true',
            HasErrors: 'false'
        });

        expect(orderCart.giftOptions.setGiftRegistry).toHaveBeenCalledWith([{
            PrimeLine: '123',
            SubLine: '456'
        }], '12345', '123 Main St');

        expect($scope.isShowInvalidRegError).toBe(false);
    });

    it('should clear out the registry scope variables', function () {
        $scope.clear();
        expect($scope.To).toBe('');
        expect($scope.From).toBe('');
        expect($scope.Message).toBe('');
        expect($scope.Registry).toBe('');
    })
});