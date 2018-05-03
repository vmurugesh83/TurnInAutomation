describe('taxExemptMod Controller', function () {
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

        spyOn(orderCart.order, 'getTaxExemptionCertificate').and.returnValue('cert');

        controller = $controller('taxExemptModCtrl', { $scope: $scope, $modalInstance: modalInstance });
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(controller).toBeDefined();
        expect(orderCart.order.getTaxExemptionCertificate).toHaveBeenCalled();
    });

    it('should set the tax exemption certificate', function () {
        spyOn(orderCart.order, 'setTaxExemptionCertificate');
        $scope.save();
        expect(orderCart.order.setTaxExemptionCertificate).toHaveBeenCalledWith('cert');
        expect(modalInstance.close).toHaveBeenCalled();
    });

    it('should clear the tax exempt certificate scope variable', function () {
        expect($scope.TaxExemptCertificate).toBe('cert');
        $scope.clear();
        expect($scope.TaxExemptCertificate).toBe('');
    });

    it('should dismiss the modal', function () {
        $scope.cancel();
        expect(modalInstance.dismiss).toHaveBeenCalled();
    });
});