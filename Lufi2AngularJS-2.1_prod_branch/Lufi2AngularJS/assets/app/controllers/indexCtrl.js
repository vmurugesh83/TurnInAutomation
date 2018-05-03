angular.module('index', ['ui.bootstrap'])
    .controller('indexCtrl', ['$scope', '$modal', '$window', '$location', 'customer', 'scannerService', 'POSService', '$state', 'orderCart', 'payment', 'authenticateUser', 'loggerService', function ($scope, $modal, $window, $location, $customer, $scannerService, POSService, $state, $orderCart, $payment, $authenticateUser, $loggerService)
    {

        $scope.headerWarningText = "QA Environment";
        if ((/test.bonton.com/).test(window.location.hostname)) {
            $scope.headerWarningText = "TEST Lufi Env";
        }
        $scope.customerDetail = function () {
            if ($customer.isCustomerSelected()) {
                $location.path('/customerDetail')
            } else {
                $location.path('/customerSearch')
            }
        }
        $scope.goToState = function (stateName) {
            $scope.isShowCustomerPopup = false;
            $scope.isShowCartPopup = false;
            $state.go(stateName);
        };

        $scope.isCustomerSet = function () {
            return $orderCart.customer.isCartCustomerSet();
        };

        $scope.deleteCart = function () {
            $scope.orderCart = $orderCart.order.deleteCart();
            $scope.removeCustomer();
            $scope.goToState('home');
        };

        $scope.removeCustomer = function () {
            $customer.clearSelectedCustomer(); //removes cached customer
            $scope.orderCart = $orderCart.customer.deleteCustomer();
            $payment.clearPaymentData();
            $scope.goToState('home');
        };

        $scope.isShowCustomerPopup = false;
        $scope.toggleCustomerPopup = function () {
            $scope.isShowCustomerPopup = !$scope.isShowCustomerPopup;
            $scope.refreshIScroll();

        };

        $scope.isShowCartPopup = false;
        $scope.toggleCartPopup = function () {
            if (!$scope.isShowCartPopup) {
                $scope.orderCart = $orderCart.order.getLiveOrderCart();
            }
            $scope.isShowCartPopup = !$scope.isShowCartPopup;
            $scope.refreshIScroll();
        };

        $scope.goToCustomer = function () {
            if ($scope.isCustomerSet()) {
                $scope.goToState('customerDetail');
            } else {
                $scope.goToState('customerSearch');
            }
        };
        $scope.hideIndexPopups = function () {
            $scope.isShowCartPopup = false;
            $scope.isShowCustomerPopup = false;
        };

        $scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $scope.isShowCartPopup = false;
            $scope.isShowCustomerPopup = false;
        });
        $scope.$on('orderCartReset', function () {
            $scope.orderCart = $orderCart.order.getLiveOrderCart();
            $scope.itemCount = function () { return $orderCart.order.getItemCount(); };

        });
        $scope.itemCount = function () { return $orderCart.order.getItemCount(); };

        $scope.selectedCustomer = $customer.getSelectedCustomer();

        $scope.orderCart = $orderCart.order.getLiveOrderCart();


        //refresh scope on customer selected event
        $scope.$on('AddCustomerToCartCalled', function (event, customer) {
            $scope.selectedCustomer = $customer.getSelectedCustomer();
        });

        $scope.exit = function () {
            var modalInstance = $modal.open({
                templateUrl: 'exitLufi.html',
                controller: 'exitLufiCtrl'
            });

            modalInstance.result.then(function (exitCause) {
                $scope.exitCause = exitCause;
            }, function () {

            });
        };

        $scope.managerLogin = function () {
            var modalInstance = $modal.open({
                templateUrl: 'managerLogin.html',
                controller: 'managerLoginCtrl'
            });

            modalInstance.result.then(function () {
                $scope.authSuccess = true;
                $scope.posParam = POSService.getPOSParameters();
            }, function () {
                //window.open("http://www.bonton.com", "_self");
            });
        };

        $scope.managerLogout = function () {
            POSService.managerLogout();
            $scope.posParam = POSService.getPOSParameters();
            loginWithPOSParams();
        };

        $scope.login = function () {
            var modalInstance = $modal.open({
                templateUrl: 'html/login.html',
                controller: 'loginCtrl',
                backdrop: 'static'
            });

            modalInstance.result.then(function () {
                $scope.authSuccess = true;
                $scope.posParam = POSService.getPOSParameters();
            }, function () {
                $('#loadDIV').show();
                $scannerService.stopServer();
                setTimeout(function () {
                    modalInstance.close('exit');
                    location.href = 'http://saphere-solutions.com/?exit';
                }, 2000);
            });
        };

        var loginWithPOSParams = function () {
            //pop up blocking modal
            var authCheckingModal = $modal.open({
                templateUrl: 'html/modalTemplates/authModal.html',
                controller: 'authModalCtrl',
                backdrop: 'static',
                animation: false,
                resolve: {
                    'idPass': function () { return { ldapId: $scope.posParam.ldapId, ldapPassword: $scope.posParam.ldapPassword }; }
                }
            });

            authCheckingModal.result.then(function (tokenObj) {
                if (tokenObj.token) {
                    //successful auth
                    $scope.posParam = POSService.getPOSParameters();
                    xAuth = tokenObj.token;
                    $scope.authSuccess = true;
                } else {
                    $scope.login();
                }
            }, function (data) {
                //modal was dismissed so login.
                swal({ title: "Alert!", text: "Authentication Failed \n <span style='word-wrap:break-word'>" + window.location.href + "</span>", showConfirmButton: true, html: true });
                $loggerService.error(data);
                $scope.login();
            });
        }

        $scope.authSuccess = false;
        POSService.setPOSTParametersFromURL(window.location.href);
        $scope.posParam = POSService.getPOSParameters();

        var headeriScroll;
        $scope.refreshIScroll = function () {
            if (!headeriScroll) {
                headeriScroll = new IScroll('#HeaderiScrollWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' })
            }
            setTimeout(function () {
                headeriScroll.refresh();
            }, 500);
        };
        
        console.log('CloseSplashScreen');

        if (!($scope.posParam.ldapId && $scope.posParam.ldapId.length > 0 && $scope.posParam.ldapPassword && $scope.posParam.ldapPassword.length > 0)) {
            $scope.login();
        } else {
            loginWithPOSParams();
        }
    }])
    .controller('authModalCtrl', ['$scope', '$modalInstance', 'scannerService', '$location', 'loggerService', 'authenticateUser', 'idPass', '$timeout',
        function ($scope, $modalInstance, $scannerService, $location, $loggerService, $authenticateUser, $idPass, $timeout) {

            $scope.isShowModal = false;

            $timeout(function () { $scope.isShowModal = true; }, 1000);

            $authenticateUser($idPass.ldapId, $idPass.ldapPassword).then(function (resp) {
                var response = angular.fromJson(resp.data);

                //successful auth
                if (angular.isObject(response.Authresp) && angular.isDefined(response.Authresp.ReturnCode) && response.Authresp.ReturnCode == 0) {
                    $modalInstance.close({ token: response.Authresp.Token });
                } else {
                    $modalInstance.dismiss(resp.data);
                }

            }, function (resp) {
                $modalInstance.dismiss(resp.data);
            });

            $scope.switchUser = function () {
                $modalInstance.close({token: null});
            };


            $scope.exit = function () {
                $('#loadDIV').show();
                $scannerService.stopServer();

                setTimeout(function () {
                    location.href = 'http://saphere-solutions.com/?exit'
                }, 2000);
            };



    }])
    .controller('exitLufiCtrl', ['$scope', '$modalInstance', 'scannerService', '$location', 'order', 'orderCart', 'loggerService', function ($scope, $modalInstance, $scannerService, $location, $order, $orderCart, $loggerService) {
        $scope.exit = function () {
            $('#loadDIV').show();
            $scannerService.stopServer();
            setTimeout(function () {
                $modalInstance.close('exit');
                location.href = 'http://saphere-solutions.com/?exit'
            }, 2000);
        };
        $scope.noPrompt = false

        $scope.cart = $orderCart.order.getLiveOrderCart();
        $scope.orderLinesArray = $scope.cart.OrderLines.OrderLine;

        if (!$scope.cart || !$scope.cart.PersonInfoBillTo._PersonInfoKey || $scope.orderLinesArray.length == 0) {
            $scope.noPrompt = true;
        }

        $scope.discard = function () {
            $scope.exit() //discard's order information
        }
        $scope.cancel = function () {
            $modalInstance.close('Cancel');
        }
        $scope.save = function () {
            //create draft order
            $scope.cart._DraftOrderFlag = 'Y';
            delete $scope.cart.PaymentMethods;
            $order.createOrder($scope.cart, function (response) {
                swal({
                    title: 'Order Number',
                    text: angular.fromJson(response)._OrderNumber,
                    showCancelButton: false,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Ok",
                    closeOnConfirm: false
                },
                function () {
                    $scope.exit();
                });

            }, function (error) {
                $loggerService.log(error);
            });
        }

    }])
    .controller( 'loginCtrl', ['$scope', '$modalInstance', 'authenticateUser', 'POSService', 'loggerService', 'numberKeyPressValidator', '$rootScope', function ($scope, $modalInstance, $authenticateUser, $POSService, $loggerService, $numberKeyPressValidator, $rootScope)
    {
        $scope.numberValidator = $numberKeyPressValidator;
        $scope.login = function () {
            $authenticateUser($scope.associateId, $scope.password).success(function (data) {
                var response = angular.fromJson(data);
                var storeNumber = $scope.storeNumber;
                if (!(storeNumber && storeNumber.length > 0)) {
                    var posParams = $POSService.getPOSParameters();
                    storeNumber = posParams.storeNumber;
                }

                $POSService.setPOSTParameters(response.Authresp.AssociateNum, storeNumber, null);
                $modalInstance.close('closed');
                xAuth = response.Authresp.Token;

                $rootScope.$broadcast( 'ShowAnnouncement');
            }).error(function (data) {
                swal({ title: "Alert!", text: "Authentication Failed", showConfirmButton: true });
                $loggerService.log(data);
            });
        };

        $scope.contract = function () {
            return angular.toJson({
                "Authreq": {
                    "uid": $scope.associateId,
                    "pwd": $scope.password
                }
            })
        };




        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };

    }])
    .controller('managerLoginCtrl', ['$scope', '$modalInstance', 'authenticateUser', 'POSService', 'loggerService', function ($scope, $modalInstance, $authenticateUser, $POSService, $loggerService) {
        $scope.login = function () {
            $authenticateUser($scope.manager.userName, $scope.manager.password).success(function (data) {
                var response = angular.fromJson(data);
                var storeNumber = $scope.storeNumber;
                if (!(storeNumber && storeNumber.length > 0)) {
                    var posParams = $POSService.getPOSParameters();
                    storeNumber = posParams.storeNumber;
                }
                var roles = response.Authresp.Control;

                $POSService.setPOSTParameters(response.Authresp.AssociateNum, storeNumber, null, roles);
            $modalInstance.close('closed');
                xAuth = response.Authresp.Token;
            }).error(function (data) {
                swal({ title: "Alert!", text: "Authentication Failed", showConfirmButton: true });
                $loggerService.log(data);
            });
        };

        $scope.contract = function () {
            return angular.toJson({
                "Authreq": {
                    "uid": $scope.associateId,
                    "pwd": $scope.password
                }
            })
        };

        $scope.cancel = function () {
            $modalInstance.dismiss('cancel');
        };
    }]);