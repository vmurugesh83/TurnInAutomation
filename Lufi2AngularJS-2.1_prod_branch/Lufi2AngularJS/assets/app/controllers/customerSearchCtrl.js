angular.module('customerSearch', [])
    .controller('customerSearchCtrl', ['$scope', 'customer', '$location', 'numberOnlyKeyPressValidator',
        '$modal', 'msrService', '$rootScope', '$state', 'loggerService', '$http', 'POSService', '$filter', 'orderCart',
        function ($scope, $customer, $location, $numberOnlyKeyPressValidator, $modal, $msrService, $rootScope, $state, loggerService, $http, $POSService, $filter, $orderCart) {
            $scope.numberValidator = $numberOnlyKeyPressValidator;

        var myScroll = new IScroll('#CustomerSearchWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip', tap: true });
        $scope.refreshIScroll = function () {
            setTimeout(function () {
                myScroll.refresh();
            }, 500);
        };

        var _groupCustomers = function (customerArray) {
                if (!angular.isArray(customerArray)) {
                return customerArray;
                } else {
                var customerKeyMap = {};

                    for (var i = 0; i < customerArray.length; i++) {

                    if (customerArray[i].CustomerKey.trim() in customerKeyMap) {
                        customerKeyMap[customerArray[i].CustomerKey.trim()].push(customerArray[i]);
                    } else {
                        customerKeyMap[customerArray[i].CustomerKey.trim()] = [customerArray[i]];
                    }
                }

                var returnArray = [];
                for (var prop in customerKeyMap) {
                    if (prop.length !== 0) {
                        returnArray.push(customerKeyMap[prop]);
                    }
                }

                returnArray.sort(function (a, b) {
                    var aNum = parseInt(a[0].CustomerKey);
                    var bNum = parseInt(b[0].CustomerKey);
                    if (aNum === Number.NaN && bNum === Number.NaN) {
                        return 0;
                    }
                    else if (aNum === Number.NaN) {
                        return 1;
                    }
                    else if (bNum === Number.NaN) {
                        return -1;
                    }

                    return bNum - aNum;
                });

                if ('' in customerKeyMap) {
                    for (var p = 0; p < customerKeyMap[''].length; p++) {
                        returnArray.push([customerKeyMap[''][p]]);
                    }
                }
                
                return returnArray;
            }
        };

        $scope.getDisplay = function (searchAddress) {
            //make into standard person info address
            var standardAddress = {};
            for (var prop in searchAddress) {
                    if ((/^AddressId$/).test(prop.toString())) {
                    standardAddress['_AddressID'] = searchAddress[prop];
                } else if ((/^Zipcode$/).test(prop.toString())) {
                    standardAddress['_ZipCode'] = searchAddress[prop];
                } else {
                    standardAddress['_' + prop.toString()] = searchAddress[prop];
                }
            }
            return $filter('oneLineAddress')(standardAddress, 'long', 'name', 'id');
        };

        $scope.addToCart = function (customer) {
                $customer.customerAction(customer, function (customerDetail) {
                    $customer.addToCart(customerDetail);
                    if ($orderCart.orderLine.util.orderlineCount() > 0) {
                        $state.go("shippingSelection");
                    } else {
                        $state.go('itemSearch') // The: $location.path('/customerSearch');  does not work for repeated hits of Shipping header button.
                        return; //stops async $state from going on to following code.
                    }
                }, function (error) {
                    swal({ title: "Error!", text: error, showConfirmButton: true });
                });
        };

        $scope.customerDetail = function (customer) {
                $customer.customerAction(customer, function (data) {
                    $location.path("/customerDetail");
                }, function (error) {
                    swal({ title: "Error!", text: error, showConfirmButton: true });
                });
        }

        var createSearchContract = function (manualSearch) {
            var contract = {};
            contract.Customer = angular.copy($scope.searchParam);
            for (key in contract.Customer) {
                if (contract.Customer[key] == '')
                    delete contract.Customer[key];
                else if (manualSearch)
                        contract.Customer[key] = contract.Customer[key];
            }

            if (angular.isString(contract.Customer.PhoneNumber)) {
                contract.Customer.PhoneNumber = $filter('phoneFormatRemove')($scope.searchParam.PhoneNumber);
            }
            return contract;
        }

        $scope.search = function (manualSearch) {
            var searchContract = createSearchContract(manualSearch);

            if (Object.keys(searchContract.Customer).length > 0) {
                $customer.customerSearch(searchContract,
                function (data) {
                    $scope.customers = _groupCustomers(data);
                    if ($scope.customers.length==0) {
                        jQuery('.not-found-alert').modal('show');
                    } else {
                        swal({ title:"", text: '<span style="color:#FF2222">Verify address information with customer before making selection.</span>', html: true, timer: 5000, showConfirmButton: false });
                    }
                }, function (err) {
                    alert(err);
                });
            } else {

                swal({ title: "No Search Input", text: "No search input available for customer search.", timer: 4000, showConfirmButton: true });
            }

            $scope.refreshIScroll();

        };

        var resetParameters = function () {
            $scope.searchParam = {};
            $scope.searchParam.FirstName = '';
            $scope.searchParam.LastName = '';
            $scope.searchParam.EmailAddress = '';
            $scope.searchParam.PhoneNumber = '';
            $scope.customers = [];
            $scope.refreshIScroll();
        };

        $scope.clearForm = function () {
            resetParameters();
            $customer.clearCachedResult();
        };

        $scope.createCustomer = function () {
            $state.go('addModifyAddress');
        };

        var showResults = function () {
            var cachedResult = $customer.getCachedResult();
            var cachedQueryParam = $customer.getCachedQueryParam();
            if (cachedQueryParam != undefined) {
                $scope.customers = _groupCustomers(cachedResult);
                $scope.searchParam = cachedQueryParam.Customer;
                    if (angular.isString($scope.searchParam.PhoneNumber)) {
                    $scope.searchParam.PhoneNumber = $filter('phoneFormat')($scope.searchParam.PhoneNumber);
                }
            } else {
                resetParameters();
            }
        }
        
        $scope.formatPhone = function () {
            $scope.searchParam.PhoneNumber = $filter('phoneFormat')($scope.searchParam.PhoneNumber);
        };
      
        showResults();

        $scope.isSearchable = function () {
            if (($scope.searchParam.FirstName && $scope.searchParam.LastName && $scope.searchParam.FirstName.length + $scope.searchParam.LastName.length > 3) || ($scope.searchParam.EmailAddress && $scope.searchParam.EmailAddress.length > 3) || ($scope.searchParam.PhoneNumber && $filter('phoneFormatRemove')($scope.searchParam.PhoneNumber).length == 10)) {
                return true;
            }
            return false;
        }

        $scope.customerData = {};
        $scope.openCustomerScanOptions = function () {
            var modalInstance = $modal
                    .open({
                        templateUrl: 'html/customer/customerSearchSwipeCard.html',
                        controller: 'swipeCardCtrl',
                        backdrop: 'static',
                            keyboard: false,
                            size: 'sm'
                    });

            modalInstance.result.then(function (response) {
                $scope.msrData = response.msrData;
                $scope.cardType = response.cardType;

                    if ($scope.cardType == "AssociateDiscountCard") {

                    if ($scope.msrData.AccountNumber && $scope.msrData.AccountNumber.trim().length > 0) {
                        var url = serviceURL + '/CreditService/Lookup';
                        var findCustomerObj = {
                            "LookupRequest": {
                                "_AccountNumber": $scope.msrData.AccountNumber,
                                "_OriginStoreNum": $POSService.getPOSParameters().storeNumber,
                                "_SourceApplication": "Sterling",
                                "_IsTokenized": "N"
                            }
                        };

                        $http.post(url, findCustomerObj).success(function (result) {
                            $scope.searchParam.LastName = result.LookupResponse._LastName;
                            $scope.searchParam.FirstName = result.LookupResponse._FirstName;
                            $scope.search(false);
                        }).error(function (data) {
                            loggerService(data);
                        });
                    }

                }
                else {
                    $scope.searchParam.LastName = $scope.msrData.LastName;
                    $scope.searchParam.FirstName = $scope.msrData.FirstName;
                    $scope.search(false);
                }   

            }, function () {
                
            });
        };

        $scope.openCustomerScanOptions();

        $scope.searchFieldEnabled = function (fieldName) {
            switch (fieldName) {
                case 'email':
                    return ($scope.searchParam.FirstName || $scope.searchParam.LastName || $scope.searchParam.PhoneNumber);
                    case 'phone':
                    return ($scope.searchParam.FirstName || $scope.searchParam.LastName || $scope.searchParam.EmailAddress);
                case 'name':
                    return ($scope.searchParam.PhoneNumber || $scope.searchParam.EmailAddress);
            }
            }
    }]);