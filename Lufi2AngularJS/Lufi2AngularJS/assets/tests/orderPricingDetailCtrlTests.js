describe('Order Pricing Detail Controller:', function () {
    beforeEach(module('orderDetail', 'ui.bootstrap', 'ui.bootstrap.modal'));
    beforeEach(angular.mock.module('appServicesOrder', 'appServicesItem', 'appServicesWebSocket'));

    var $controller, scope, controller, loggerService;

    var mockCart = {
        _UnitPriceTotal: 10,
        _OrderTotal: 15,
        _ShippingChrgTotal: 5,
        _ShippingDiscTotal: 0
    };

    var mockOrder = {
        orderPricingDetail: function (cartInst, isModalCalledFromOrderDetails) {
            return {
                category: [{
                    categoryDescription: 'shipping',
                    total: '5'
                },
                {
                    categoryDescription: 'order total',
                    total: 15
                }]
            };
        }
    };

    beforeEach(function () {
        module(function ($provide) {
            $provide.value('cart', mockCart);
            $provide.value('order', mockOrder);
        });
    });

    beforeEach(inject(function (_$controller_, $rootScope, _loggerService_) {
        $controller = _$controller_;
        $scope = $rootScope.$new();
        loggerService = _loggerService_;

        modalInstance = {
            close: jasmine.createSpy('modalInstance.close'),
            dismiss: jasmine.createSpy('modalInstance.dismiss'),
            result: {
                then: jasmine.createSpy('modalInstance.result.then')
            }
        };

        controller = $controller('orderPricingDetailCtrl', { $scope: $scope, $modalInstance: modalInstance });
    }));

    it('should exist', function () {
        expect(controller).toBeDefined();
    });

    it('should set up variables for the other details page', function () {
        var item = { total: '10' }
        $scope.openModifyCharge(item);
        expect($scope.item).toBe(item);
        expect($scope.newCharge).toBe('10');
        expect($scope.updatingPrice).toBe(true);
    });

    it('should set the new charge to the flat rate if it is cheaper', function () {
        $scope.flatRate = 10;
        $scope.item = { total: 15 };
        $scope.calculateFlatRate();
        expect($scope.newCharge).toBe(10);
    });

    it('should not set the new charge to the flat rate if it is more expensive', function () {
        $scope.flatRate = 10;
        $scope.item = { total: 5 };
        $scope.calculateFlatRate();
        expect($scope.newCharge).toBe(5);
    });

    it('should reduce the price of shipping and order total', function () {
        $scope.newCharge = 4;

        $scope.update();

        expect($scope.pricingDetail.category[0].total).toBe(4);
        expect($scope.pricingDetail.category[1].total).toBe(14);
        expect($scope.updatingPrice).toBe(false);
        expect($scope.newCharge).toBe(0);
    });

    it('should flip the updatingPrice switch', function () {
        $scope.updatingPrice = true;
        $scope.cancel();
        expect($scope.updatingPrice).toBe(false);
    });

    it('should close the modal window and pass back the discount and cart', function () {
        $scope.newCharge = 4;
        $scope.update();
        $scope.close();
        expect(modalInstance.close).toHaveBeenCalledWith({ newShippingDiscount: '1.00', cart: mockCart });
    });

    it('should dismiss the modal', function () {
        $scope.dismiss();
        expect(modalInstance.dismiss).toHaveBeenCalled();
    });
});