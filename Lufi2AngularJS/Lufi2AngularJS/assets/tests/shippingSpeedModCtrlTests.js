describe('shippingSpeedMod Controller', function () {
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
                _SubLineNo: '456'
            }
        ];

        spyOn(orderCart.carrierService, 'setOrderlineCarrierService');
        spyOn(orderCart.order, 'deleteShippingDiscount').and.returnValue('new order line');

        controller = $controller('shippingSpeedModCtrl', { $scope: $scope, $modalInstance: modalInstance, orderlines: orderlines });
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(controller).toBeDefined();

        expect($scope.orderline).toEqual({
            _PrimeLineNo: '123',
            _SubLineNo: '456'
        });
    });

    it('should set carrier service, delete shipping discount, and close the modal', function () {
        var carrierService = {
            name: 'service'
        };

        $scope.saveService(carrierService);

        expect(orderCart.carrierService.setOrderlineCarrierService).toHaveBeenCalledWith($scope.orderline, carrierService);
        expect(orderCart.order.deleteShippingDiscount).toHaveBeenCalledWith($scope.orderline);
        expect(modalInstance.close).toHaveBeenCalled();
    });

    it('should dismiss the modal', function () {
        $scope.cancel();
        expect(modalInstance.dismiss).toHaveBeenCalled();
    });
});