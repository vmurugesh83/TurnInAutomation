var app = angular.module('app', [
    'ui.router',
    'ngAnimate',
    'angularUtils.directives.uiBreadcrumbs',
    'home',
    'index',
    'swipeCard',
    'itemSearch',
    'itemResults',
    'itemDetail',
    'itemLocate',
    'customerSearch',
    'customerDetail',
    'modifyCustomer',
    'orderSearch',
    'orderCart',
    'shippingSelection',
    'coupons',
    'ui.bootstrap',
    'paymentsummary',
    'appDirectives',
    'appFilters',
    'orderDetail',
    'appServicesCustomer',
    'appServicesPayment',
    'appServicesOrder',
    'appServicesItem',
    'appServicesWebSocket',
    'appServicesAnnouncements',
    'appUtilities',
    'propertiesService',
    'appServicesCommonCode',
    'ngSanitize',
    'appServiceOrderCart',
    'appServicesGiftRegistry',
    'appServiceReprice',
    'addModifyAddress',
    'appServiceAppState',
    'orderNote',
    'returnSummary'
]);
app.controller('footerCtrl', ['$scope', 'btProp',function ($scope, btProp) {
    $scope.footerText = btProp.getProp('btFooterText');
}]);
app.controller('breadCtl', ['$scope', function ($scope) {
}]);
app.controller('checkoutProgressCtl', ['$scope', function ($scope) {
    $scope.checkoutDisplay = false;
    $scope.progressSteps = [
        { display: 'Home', routerstate: 'home', isFinished: true, isActive: false, isHideOnClick:true },
        { display: 'Cart', routerstate: 'orderCart', isFinished: true, isActive: false, isHideOnClick: false },
        { display: 'Customer Info', routerstate: 'customerSearch', isFinished: true, isActive: true },
        { display: 'Shipping', routerstate: 'shippingSelection', isFinished: false, isActive: false },
        { display: 'Payment', routerstate: 'paymentSummary', isFinished: false, isActive: false }
    ];
    $scope.isCouponLinkAdded = false;
    
    $scope.$on('breadCrumbReset', function (event, args) {
        $scope.checkoutDisplay = false;
        $scope.progressSteps[3].isFinished = false;
        $scope.progressSteps[3].isActive = false;
        $scope.progressSteps[4].isFinished = false;
        $scope.progressSteps[4].isActive = false;
    });

    $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        var stateName = toState.name;
        //if (stateName == 'customerDetail' || stateName == 'customerSearch') {
        //    for (var i = 0; i < $scope.progressSteps.length; i++) {
        //        var stepState = $scope.progressSteps[i].routerstate;
        //        if (stepState == 'shippingSelection' || stepState == 'payment') {
        //            $scope.progressSteps[i].isFinished = false;
        //        }
        //    }  
        //}

        //TODO: would they like to have coupons link show up?
        //if (stateName == 'couponSummary') {
        //    if (! $scope.isCouponLinkAdded){
        //        $scope.progressSteps.splice(4, 0, { display: 'Coupons', routerstate: 'couponSummary', isFinished: true, isActive: true });
        //        $scope.$broadcast('uiCheckoutStepProgressUpdate');
        //        $scope.isCouponLinkAdded = true;
        //    }
        //}
       
    });
}]);
var  loadingIcon = {
        isLockedBusyIcon: false,
        showBusyIcon: function showBusyIcon() {
            $('#loadDIV').show();
        },

        hideBusyIcon: function() {
            if (!loadingIcon.isLockedBusyIcon) {
                $('#loadDIV').hide();
            }
        },
        resetIcon: function () {
            loadingIcon.isLockedBusyIcon = false;
            loadingIcon.hideBusyIcon();
        }
};

app.config(['$httpProvider', '$stateProvider', '$urlRouterProvider', function ($httpProvider, $stateProvider, $urlRouterProvider) {
    
    delete $httpProvider.defaults.headers.common["X-Requested-With"];
    $httpProvider.defaults.useXDomain = true;

    //================================================
    // Add an interceptor for AJAX errors
    //================================================
    $httpProvider.interceptors.push(['$q', '$log', 'loadingIconService', function ($q, $log, $loadingIconService) {
        function setCustomHeaders(config) {
            if (xAuth && (config.url.indexOf('http://broker') > -1 || config.url.indexOf('https://broker') > -1)) {
                config.headers['X-Auth-Token'] = xAuth;
            }
        }

        loadingIcon.resetIcon();

        return {
            request: function (config) {
                loadingIcon.showBusyIcon();
                setCustomHeaders(config);
                return config || $q.when(config);
            },
            requestError: function (request) {
                loadingIcon.hideBusyIcon();
                return $q.reject(request);
            },
            response: function (response) {
                loadingIcon.hideBusyIcon();
                //$log.info(response);
                return response || $q.when(response);
            },
            responseError: function (response) {
                loadingIcon.hideBusyIcon();
                $log.error(response);
                if (response.status === 401)
                    alert(response)
                return $q.reject(response);
            }
        };
    }]);

   
    $stateProvider.state('home', {
        url: '/home',
        views: {
            'main@': {
        templateUrl: 'Home.html',
                controller: 'homeCtrl',
            }
        },
        data: {
            displayName: 'Home',
            isBreadCrumbDisplay: true
        }
    })
    .state('orderSearch', {
        url: '/orderSearch',
        views: {
            'main@': {
        templateUrl: 'html/order/orderSearch.html',
                controller: 'orderSearchCtrl',
            }
        },
        data: {
            displayName: 'Order Search',
            isBreadCrumbDisplay: true
        }
    })
    .state('orderDetail', {
        url: '/orderDetail',
        views: {
            'main@': {
                templateUrl: 'html/order/orderDetail.html',
                controller: 'orderDetailCtrl',
            }
        },
        data: {
            displayName: 'Order Detail',
            isBreadCrumbDisplay: true
        }
    })
    .state('orderNote', {
        url: '/orderNote/:orderLineNo',
        views: {
            'main@': {
        templateUrl: 'html/order/orderNote.html',
        controller: 'orderNoteCtrl',
            isBreadCrumbDisplay: true
            }
        },
        data: {
            displayName: 'Order Note',
            isBreadCrumbDisplay: true
        }
    })
    .state('returnSummary', {
        url: '/returnSummary',
        views: {
            'main@': {
                templateUrl: 'html/order/returnSummary.html',
                controller: 'returnSummaryCtrl',
                isBreadCrumbDisplay: true
            }
        },
        data: {
            displayName: 'Returns',
            isBreadCrumbDisplay: true
        }
    })
    .state('itemSearch', {
        url: '/itemSearch',
        views: {
            'main@': {
        templateUrl: 'html/item/itemSearch.html',
                controller: 'itemSearchCtrl',
            }
        },
        data: {
            displayName: 'Item Search',
            isBreadCrumbDisplay: true
        }
    })

    .state('itemSearchupc', {
        url: '/itemSearch/upc/:upc',
        views: {
            'main@': {
                templateUrl: 'html/item/itemSearch.html',
                controller: 'itemSearchCtrl',
            }
        },
        data: {
            displayName: 'Item Search',
            isBreadCrumbDisplay: true
        }
    })

    .state('itemResults', {
        url: '/itemResults',
        views: {
            'main@': {
                templateUrl: 'html/item/itemResults.html',
                controller: 'itemResultsCtrl'
            }
        },
        data: {
            displayName: 'Item Results',
            isBreadCrumbDisplay: true
        }
    })

    .state('coupons', {
        url: '/coupons',
        views: {
            'main@': {
                templateUrl: 'html/coupons/coupons.html',
                controller: 'couponCtrl',
            }
        },
        data: {
            displayName: 'Coupon Summary',
            isBreadCrumbDisplay: true
        }
    })

    .state('itemLocate', {
        url: '/itemLocate',
        views: {
            'main@': {
                templateUrl: 'html/item/itemLocate.html',
                controller: 'itemLocateCtrl',
            }
        },
        data: {
            displayName: 'Item Locate',
            isBreadCrumbDisplay: true
        }
    })

    .state('itemDetail', {
        url: '/itemDetail',
        views: {
            'main@': {
                templateUrl: 'html/item/itemDetail.html',
                controller: 'itemDetailCtrl',
            }
        },
        data: {
            displayName: 'Item Detail',
            isBreadCrumbDisplay: true
        }
    })

    .state('paymentSummary', {
        url: '/paymentSummary',
        views: {
            'main@': {
                templateUrl: 'html/payment/paymentSummary.html',
                controller: 'paymentSummaryCtrl',
            }
        },
        data: {
            displayName: 'Payment',
            isBreadCrumbDisplay: true
        }
    })

    .state('orderCart', {
        url: '/orderCart/:loadDraft',
        views: {
            'main@': {
        templateUrl: 'orderCart.html',
                controller: 'orderCartCtrl',
            }
        },
        data: {
            displayName: 'Order Cart',
            isBreadCrumbDisplay: true
        }
    })
    .state('addModifyAddress', {
            url: '/addModifyAddress/:customerContactID/:customerAdditionalAddressID',
    views: {
        'main@': {
                templateUrl: 'html/customer/addModifyAddress.html',
                controller: 'addModifyAddressCtrl',
        }
    },
        data: {
        displayName: 'Modify Address',
        }
    })
    .state('customerSearch', {
        url: '/customerSearch',
        views: {
            'main@': {
                templateUrl: 'html/customer/customerSearch.html',
                controller: 'customerSearchCtrl',
            }
        },
        data: {
            displayName: 'Customer Search',
            isBreadCrumbDisplay: true
        }
    })
    .state('customerDetail', {
        url: '/customerDetail',
        views: {
            'main@': {
        templateUrl: 'html/customer/customerDetail.html',
                controller: 'customerDetailCtrl',
            }
        },
        data: {
            displayName: 'Customer Detail',
            isBreadCrumbDisplay: true
        }
    })
    .state('shippingSelection', {
        url: '/shippingSelection',
        views: {
            'main@': {
        templateUrl: 'shippingSelection.html',
                controller: 'shippingSelectionCtrl',
            }
        },
        data: {
            displayName: 'Shipping Selection',
            isBreadCrumbDisplay: false
        }
    })

        

    $urlRouterProvider.otherwise('/home');



   
}]);

