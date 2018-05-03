angular.module('index', ['ui.bootstrap'])
    .controller('indexCtrl', ['$scope', '$modal', '$location', 'customer', 'scannerService', 'POSService', '$state', 'orderCart', 'payment', 'authenticateUser', 'loggerService', 'securityService', 
        function ($scope, $modal, $location, $customer, $scannerService, POSService, $state, $orderCart, $payment, $authenticateUser, $loggerService, securityService)
    {

        $scope.headerWarningText = "QA Environment";
        if ((/test.bonton.com/).test(window.location.hostname)) {
            $scope.headerWarningText = "TEST Lufi Env";
        }
        if ((/scint.bonton.com/).test(window.location.hostname)) {
            $scope.headerWarningText = "SCINT Lufi Env";
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
                if ((/discardOrder/i).test(exitCause)) {
                    $scope.deleteCart();
                }
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
                $scope.isCsr = securityService.isCsr();
                if ($scope.isCsr) {
                    $scope.headerWarningText += " CSR";
                }
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
                    $scope.isCsr = securityService.isCsr();
                    if ($scope.isCsr) {
                        $scope.headerWarningText += " CSR";
                    }
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

    //login with POS parameters
    .controller('authModalCtrl', ['$scope', '$modalInstance', 'scannerService', '$location', 'loggerService', 'authenticateUser', 'idPass', '$timeout', 'btProp', 'POSService',
function ($scope, $modalInstance, $scannerService, $location, $loggerService, $authenticateUser, $idPass, $timeout, btProp, $POSService) {

            $scope.isShowModal = false;

            $timeout(function () { $scope.isShowModal = true; }, 1000);

            $authenticateUser($idPass.ldapId, $idPass.ldapPassword).then(function (resp) {
                var response = angular.fromJson(resp.data);

                //successful auth
                if (angular.isObject(response.Authresp) && angular.isDefined(response.Authresp.ReturnCode) && response.Authresp.ReturnCode == 0) {

                    //if this is a CSR, then force to csrStoreNode (store 192)
                    var isCsr = false;
                    var roles = null;
                    if (angular.isObject(response.Authresp) && angular.isObject(response.Authresp.Control)) {
                        roles = response.Authresp.Control;

                        if (angular.isString(response.Authresp.Control.CREATE_ORDER_CALLCENTER) && (/^true$/i).test(response.Authresp.Control.CREATE_ORDER_CALLCENTER)) {

                            storeNumber = btProp.getProp("csrStoreNode");
                            $POSService.setPOSTParameters(null, storeNumber, null, roles);
                        }else{
                            $POSService.setPOSTParameters(null, null, null, roles);
                        }
                    }

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
    .controller('exitLufiCtrl', ['$scope', '$modalInstance', 'scannerService', '$location', 'order', 'orderCart', 'loggerService', 'securityService', 
        function ($scope, $modalInstance, $scannerService, $location, $order, $orderCart, $loggerService, $securityService) {

            $scope.noDraftOrderPrompt = "Do you want to close Let Us Find It and return to the POS application?";
            $scope.noDraftOrderExitButtonText = "No";
            $scope.withDraftOrderSaveButtonText = "Save Order & Close";

            $scope.isCsr = function () { return $securityService.isCsr();};

            if ($securityService.isCsr()) {
                $scope.noDraftOrderPrompt = "To close Let Us Find It, please use browser's exit button.";
                $scope.noDraftOrderExitButtonText = "Cancel";
                $scope.withDraftOrderSaveButtonText = "Save Order";
            }

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
            if ($scope.isCsr()) {
                $modalInstance.close('discardOrder'); //discard's order information
            } else {
                $scope.exit();
            }
        }
        $scope.cancel = function () {
            $modalInstance.close('Cancel');
        }
        $scope.save = function () {
            //create draft order
            $scope.cart._DraftOrderFlag = 'Y';
            delete $scope.cart.PaymentMethods;
            $order.createOrder($scope.cart, function (response) {

                if ($scope.isCsr()) {

                    var orderNumber = (response && angular.isString(response._OrderNumber))?response._OrderNumber : "";

                    swal(
                        {
                            title: '',
                            text: '<div class="savedDraftExitText">Draft Order Saved.</div><div class="savedDraftExitText">Order Number: ' + orderNumber + '</div>',
                            html: true
                        });

                    $scope.discard();

                } else {
                    swal(
                        {
                            title: '',
                            text: '<span class="savedDraftExitText">Draft Order Saved</span>',
                            html: true, timer: 3000, showConfirmButton: false, showCancelButton: false
                        },
                    function () {
                        $scope.exit();
                    });
                }

            }, function (error) {
                $loggerService.log(error);
            });
        }

    }])
    //ask for login creds
    .controller('loginCtrl', ['$scope', '$modalInstance', 'authenticateUser', 'POSService', 'loggerService', 'numberOnlyKeyPressValidator', '$rootScope', 'btProp',
        function ($scope, $modalInstance, $authenticateUser, $POSService, $loggerService, $numberOnlyKeyPressValidator, $rootScope, btProp)
    {
            $scope.numberValidator = $numberOnlyKeyPressValidator;
            $scope.storePattern = "\\d{1,3}";

        $scope.login = function () {
            $authenticateUser($scope.associateId, $scope.password).success(function (data) {
                var response = angular.fromJson(data);
                //if error returned
                if (data === undefined || (data.Authresp && data.Authresp.ReturnCode && (data.Authresp.ReturnCode) !== 0)) {
                    swal({ title: "Alert!", text: "Authentication Failed", showConfirmButton: true });
                    $loggerService.log(data);
                    return;
                }

                var storeNumber = $scope.storeNumber;
                if (!(storeNumber && storeNumber.length > 0)) {
                    var posParams = $POSService.getPOSParameters();
                    storeNumber = posParams.storeNumber;
                }

                //if this is a CSR, then force to csrStoreNode (store 192)
                var isCsr = false;
                var roles = null;
                if(angular.isObject(response.Authresp) && angular.isObject(response.Authresp.Control)){
                    roles = response.Authresp.Control;

                    if (angular.isString(response.Authresp.Control.CREATE_ORDER_CALLCENTER)) {
                        if ((/^true$/i).test(response.Authresp.Control.CREATE_ORDER_CALLCENTER)) {
                            isCsr = true;
                            storeNumber = btProp.getProp("csrStoreNode");
                        }
                    }
                }

                //fix bad AssociateNum from broker call
                var assocNum = "";
                if(response.Authresp.AssociateNum !== undefined && response.Authresp.AssociateNum !== null){
                    assocNum = response.Authresp.AssociateNum.toString().trim();
                }

                if ((/^\d+\.$/).test(assocNum)) {
                    //remove ending period 
                    assocNum = assocNum.substring(0, assocNum.length - 1);
                    if(assocNum.length < 6) {
                        assocNum = "000000" + assocNum;
                        assocNum = assocNum.slice(-6);
                    }
                }

                //if assocNum is less than 6 digits and only digits, pad front
                if ((/^\d{1,5}$/).test(assocNum)) {
                    assocNum = "000000" + assocNum;
                    assocNum = assocNum.slice(-6);
                }


                if (storeNumber.length < 3) {
                    storeNumber = "000" + storeNumber;
                    storeNumber = storeNumber.slice(-3);
                }
                //associateId,storeNumber,terminalNumber,roles
                $POSService.setPOSTParameters(assocNum, storeNumber, null, roles);
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