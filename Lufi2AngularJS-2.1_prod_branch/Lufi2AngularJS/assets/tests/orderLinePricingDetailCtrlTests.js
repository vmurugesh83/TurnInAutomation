describe('Order Line Pricing Detail Controller:', function () {
    beforeEach(module('orderDetail', 'ui.bootstrap', 'ui.bootstrap.modal'));
    beforeEach(angular.mock.module('appServicesOrder', 'appServicesItem', 'appServicesWebSocket'));

    var $controller, scope, controller, loggerService;

    var mockOrderLine = {
        LineCharges: {
            LineCharge: [{
                _ChargeCategory: 'BTN_SHIP_CHRG',
                _ChargeAmount: '10'
            },
            {
                _ChargeCategory: 'BTN_SHIP_DISC',
                _ChargeAmount: '1'
            }]
        }
    };

    var mockOrder = {
        orderLineItemPricingDetail: function (cartInst, isModalCalledFromOrderDetails) {
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
            $provide.value('orderLine', mockOrderLine);
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

        controller = $controller('orderLinePricingDetailCtrl', { $scope: $scope, $modalInstance: modalInstance });
    }));

    it('should exist', function () {
        expect(controller).toBeDefined();
    });

    it('should set up variables for the other details page', function () {
        $scope.openModifyCharge();
        expect($scope.item.total).toBe(9);
        expect($scope.newCharge).toBe(9);
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
        $scope.newCharge = 9;
        $scope.item = {
            categoryDescription: 'shipping',
            total: 10
        };

        $scope.update();

        expect($scope.pricingDetail.category[0].total).toBe(9);
        expect($scope.updatingPrice).toBe(false);
        expect($scope.newCharge).toBe(0);
    });

    it('should flip the updatingPrice switch', function () {
        $scope.updatingPrice = true;
        $scope.cancel();
        expect($scope.updatingPrice).toBe(false);
    });

    it('should close the modal window', function () {
        $scope.close();
        expect(modalInstance.dismiss).toHaveBeenCalledWith('closed');
    });

    it('should close the modal and pass back the totalShippingDiscount', function () {
        $scope.newCharge = 9;
        $scope.item = {
            categoryDescription: 'shipping',
            total: 10
        };

        $scope.update();
        $scope.save();
        expect(modalInstance.close).toHaveBeenCalledWith('1.00');
    });
});