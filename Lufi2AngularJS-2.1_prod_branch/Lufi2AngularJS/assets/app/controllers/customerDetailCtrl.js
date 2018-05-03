angular.module('customerDetail', [])
    .controller('customerDetailCtrl', ['$scope', 'customer', '$rootScope', '$state', '$location', '$filter', 'ngTableParams', 'order', '$modal', 'loggerService', 'orderCart',
        function ($scope, $customer, $rootScope, $state, $location, $filter, ngTableParams, $order, $modal, $loggerService, $orderCart) {

        var customerAddressIScroll = {};
        $scope.refreshIScroll = function (iScrollName) {
            if (!customerAddressIScroll[iScrollName]) {
                customerAddressIScroll[iScrollName] = new IScroll('#' + iScrollName, { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip',scrollX:true});
            }
            setTimeout(function () {
                customerAddressIScroll[iScrollName].refresh();
            }, 500);
        };

        $scope.selectedCustomer = $customer.getSelectedCustomer();
        $scope.selectedContact = $scope.selectedCustomer.CustomerContactList.CustomerContact[0];
        $scope.draftOrders = [];
        $scope.confirmOrders = [];
        $scope.returnOrders = [];

        //refresh list after delete
        var scopeRefresh = function () {
            $scope.selectedCustomer = $customer.getSelectedCustomer();
            $scope.selectedContact = $scope.selectedCustomer.CustomerContactList.CustomerContact[0];
        };

        $scope.contactClicked = function (contact) {
            $scope.selectedContact = contact;
        }
        
        $scope.modifyAddress = function (contactID, addressID) {
            $state.go('addModifyAddress', {'customerContactID': contactID , 'customerAdditionalAddressID': addressID});
        };

        $scope.deleteAddress = function ( customerContactID, customerAdditionalAddressID) {
            $customer.deleteAddress($scope.selectedCustomer._CustomerID, customerContactID, customerAdditionalAddressID, function () { scopeRefresh(); }, function () { });
        };

        $loggerService.log($scope.selectedCustomer);

        $scope.isDraftOrdersQueried = false;
        $scope.draftOrders = function () {
            $customer.retrieveDraftOrders(function (data) {
                //fix for total sorting
                for (var i = 0; i < data.length; i++) {
                    data[i]._GrandTotal = parseFloat(data[i]._GrandTotal);
                }

                $scope.draftOrders = data;
                $scope.draftTableParams.reload();
                $scope.refreshIScroll('CustomerDraftWrapper');
                $scope.isDraftOrdersQueried = true;

            }, function (data) {
                $loggerService.log(data);
            });
        };

        $scope.isConfirmedOrdersQueried = false;
        $scope.confirmedOrders = function () {
            $customer.retrieveConfirmedOrders(function (data) {
                //fix for total sorting
                for (var i = 0; i < data.length; i++) {
                    data[i]._GrandTotal = parseFloat(data[i]._GrandTotal);
                }

                $scope.confirmOrders = data;
                $scope.confirmedTableParams.reload();
                $scope.refreshIScroll('CustomerConfirmedWrapper');
                $scope.isConfirmedOrdersQueried = true;
            }, function (data) {
                $loggerService.log(data);
            });
        };

        $scope.isReturnOrdersQueried = false;
        $scope.returnOrders = function () {
            $customer.retrieveReturnOrders(function (data) {
                //fix for total sorting
                for (var i = 0; i < data.length; i++) {
                    data[i]._GrandTotal = parseFloat(data[i]._GrandTotal);
                }

                $scope.returnOrders = data;
                $scope.returnTableParams.reload();
                $scope.refreshIScroll('CustomerReturnsWrapper');
                $scope.isReturnOrdersQueried = true;

            }, function (data) {
                $loggerService.log(data);
            });
        };
      
        $scope.addToCart = function () {
                $customer.addToCart($scope.selectedCustomer);
                if ($orderCart.orderLine.util.orderlineCount() > 0) {
                    $state.go("shippingSelection");
                } else {
                    $state.go('itemSearch') // The: $location.path('/customerSearch');  does not work for repeated hits of Shipping header button.
                    return; //stops async $state from going on to following code.
            }
        };

        $scope.goToState = function (stateName, contactID) {
            if (contactID) {
                params = { 'customerContactID': contactID };
            }

            $state.go(stateName, params);
        };

        //START: ngTable's for order lists
        $scope.draftTableParams = new ngTableParams({
            page: 1,            // show first page
            count: 100000,          // count per page
            sorting: {
                _OrderDate: 'desc'     // initial sorting
            }
        }, {
            total: $scope.draftOrders.length, // length of data
            counts: [],
            getData: function ($defer, params) {
                // use build-in angular filter
                var orderedData = params.sorting() ?
                                    $filter('orderBy')($scope.draftOrders, params.orderBy()) :
                                    $scope.draftOrders;

                $defer.resolve(orderedData);
            }
        });

        $scope.confirmedTableParams = new ngTableParams({
            page: 1,            // show first page
            count: 100000,          // count per page
            sorting: {
                _OrderDate: 'desc'     // initial sorting
            }
        }, {
            total: $scope.confirmOrders.length, // length of data
            counts: [],
            getData: function ($defer, params) {
                // use build-in angular filter
                var orderedData = params.sorting() ?
                                    $filter('orderBy')($scope.confirmOrders, params.orderBy()) :
                                    $scope.confirmOrders;

                $defer.resolve(orderedData);
            }
        });

        $scope.returnTableParams = new ngTableParams({
            page: 1,            // show first page
            count: 100000,          // count per page
            sorting: {
                _OrderDate: 'desc'     // initial sorting
            }
        }, {
            total: $scope.returnOrders.length, // length of data
            counts: [],
            getData: function ($defer, params) {
                // use build-in angular filter
                var orderedData = params.sorting() ?
                                    $filter('orderBy')($scope.returnOrders, params.orderBy()) :
                                    $scope.returnOrders;

                $defer.resolve(orderedData);
            }
        });
        //END: ngTable's for order lists

        $scope.openOrderDetail = function (selectedOrder) {
            $order.setSelectedOrder(selectedOrder);
            $location.path("/orderDetail");
            return;
        };

        $scope.updateCustomer = function () {
            var modalInstance = $modal
                    .open({
                        templateUrl: 'html/customer/modifyCustomer.html',
                        controller: 'modifyCustomerCtrl',
                        resolve: {
                            contactIdArg: function () {
                                return $scope.selectedContact._CustomerContactID;
                            }
                        },
                        size: 'lg'
                    });

            modalInstance.result.then(function () {
                scopeRefresh();
            });
        };

    }])
;