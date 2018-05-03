angular.module('coupons', ['ui.bootstrap'])
    .controller('couponCtrl', ['$scope', '$location', '$modal', '$rootScope', 'orderCart', 'order', 'repriceService', 'loggerService', function ($scope, $location, $modal, $rootScope, $orderCart, $order, repriceService, $loggerService) {
        $scope.cart = $orderCart.order.getLiveOrderCart();
        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;
        $scope.txtCouponInput = "";

      
        $scope.deleteCoupon = function (rowIndex) {
            $scope.cart.Promotions.Promotion.splice(rowIndex,1)
            $scope.UpdateCart($scope.cart)

        }
       
        var deregister = $rootScope.$on('Scanner_Event', function (event, data) {
            if (data) {
                $scope.txtCouponInput = data;
                $scope.addCoupon($scope.txtCouponInput);
            }
        });

        $scope.$on("$destroy", function () {
            deregister();
        });

        $scope.addCoupon = function (CouponID) {
            $scope.txtCouponInput = "";
            if ($scope.cart.Promotions.Promotion != undefined) {
                var i = $scope.cart.Promotions.Promotion.length;
                $scope.cart.Promotions.Promotion[i] = {
                    _PromotionId: CouponID,
                    _PromotionType: "PROMO",                   
                }
            } else {
                $scope.cart.Promotions = {
                    Promotion: [
                        {
                            _PromotionId: CouponID,
                            _PromotionType: "PROMO",
                            //C00250101E0  
                        }
                    ]
                }
            }
            $scope.UpdateCart($scope.cart)
        };

        
        $scope.getLineDiscTotal = function (line) {
            var total = 0.00;
            $scope.total = parseFloat(total); 
            for (var i = 0, len = line.LineCharges.LineCharge.length; i < len; i++) {
                var name = line.LineCharges.LineCharge[i]._ChargeCategory
                if (name == "BTN_CPN_DISC") {
                    $scope.chargeamount = parseFloat(line.LineCharges.LineCharge[i]._ChargeAmount)
                    $scope.total += $scope.chargeamount;
                } else {
                }
            }
            return $scope.total;
        }
        $scope.openCouponChargeDetail = function (orderLine) {
            var modalInstance = $modal
        .open({
            templateUrl: 'html/coupons/couponChargeDesc.html',
            controller: 'couponChargeDescCtrl',
            resolve: {
                orderLine: function () {
                    return orderLine;
                },

            }
        })
    }


        $scope.UpdateCart = function (cart) {
            repriceService.reprice(cart).then(function (response) {
                cart = response.data;
                $scope.orderLinesArray = cart.OrderLines.OrderLine;
                $scope.orderPromotionArray = cart.Promotions.Promotion;
                $scope.TotalSavings = cart._CouponDiscTotal;
                $loggerService.log(response.data);
                $orderCart.order.setOrderCart(cart);
                }

                )
            }

        if ($scope.cart.Promotions.Promotion != undefined) {
            $scope.UpdateCart($scope.cart)
        }

        $scope.close = function () {
            $location.path('/paymentSummary')
        };


        $scope.showPaperless = function () {
            var modalInstance = $modal
                    .open({
                        templateUrl: 'html/coupons/paperless.html',
                        controller: 'paperlessCtrl'
                    });

            modalInstance.result.then(function () {
            }, function () {

            });
        };

    }])


    .controller('couponChargeDescCtrl', ['$scope', '$modalInstance', '$location', 'orderLine', 
				function ($scope, $modalInstance, $location, orderLine) {
                    $scope.orderLine = orderLine
                    $scope.lineChargeArray = orderLine.LineCharges.LineCharge;
                    $scope.refreshIScroll = function () {
                        setTimeout(function () {
                            myScroll.refresh();
                        }, 500);
                    };

                 
				    $scope.close = function () {
				        $modalInstance.dismiss('closed');
				    };

				}])

    .controller('paperlessCtrl', ['$scope', '$modalInstance', '$modal',
				function ($scope, $modalInstance, $announcements, $modal) {


				    $scope.ok = function () {
				        $modalInstance.close();
				    };

				    $scope.cancel = function () {
				        $modalInstance.dismiss('cancel');
				    };
				}]);
               

