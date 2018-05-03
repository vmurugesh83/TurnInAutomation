angular.module('orderSearch', ['ngTable'])
.controller('orderSearchCtrl', ['$scope', 'order', '$filter', 'ngTableParams', '$location', 'numberOnlyKeyPressValidator', 'loggerService', function ($scope, $order, $filter, ngTableParams, $location, $numberOnlyKeyPressValidator, $loggerService) {

    $scope.numberValidator = $numberOnlyKeyPressValidator;
    $scope.search = function () {
        if ($scope.validation.form.valid) {
            $scope.orders = angular.copy([], $scope.orders);

            var param = {};

            if ($scope.formInput.orderNumber) {
                param = {
                    "GetSterlingOrderListReq":
                        {
                            "_CustomerEmailID": "",
                            "_CustomerPhoneNo": "",
                            "_CustomerLastName": "",
                            "_CustomerLastNameQryType": "FLIKE",
                            "_CustomerFirstName": "",
                            "_CustomerFirstNameQryType": "FLIKE",
                            "_ZipCode": "",
                            "_GiftRegistryID": "",
                            "_ReadFromHistory": $scope.formInput.showHistoryOrders ? "Y" : "N", // "N" for no, "Y" for history "B" for both
                            "_OrderNo": $scope.formInput.orderNumber,
                            "_DraftOrderFlag": $scope.formInput.showHistoryOrders ? "N" : ""
                        }
                };
            } else {
                param = {
                    "GetSterlingOrderListReq":
                        {
                            "_CustomerEmailID": $scope.formInput.email,
                            "_CustomerPhoneNo":  $filter('phoneFormatRemove')($scope.formInput.phoneNumber),
                            "_CustomerLastName": $scope.formInput.lastName,
                            "_CustomerLastNameQryType": "FLIKE",
                            "_CustomerFirstName": $scope.formInput.firstName,
                            "_CustomerFirstNameQryType": "FLIKE",
                            "_ZipCode": $scope.formInput.zipCode,
                            "_GiftRegistryID": $scope.formInput.giftRegistry,
                            "_ReadFromHistory": $scope.formInput.showHistoryOrders ? "Y" : "N",
                            "_OrderNo": $scope.formInput.orderNumber,
                            "_DraftOrderFlag": $scope.formInput.showHistoryOrders ? "N" : ""
                        }
                };
            }

            $order.getOrderList(param, function (result) {
                if (result === undefined) {
                    jQuery('.not-found-alert').modal('show');
                }else{
                    //change totals to floats for sorting
                    //add _orderType
                    for (var i = 0; i < result.length; i++) {
                        result[i]._GrandTotal = parseFloat(result[i]._GrandTotal);

                        if ((/^0003$/).test(result[i]._DocumentType)) {
                            
                            result[i]._orderType = 'Return';
                            
                        } else if((/^0001$/).test(result[i]._DocumentType)) {
                            if ((/^Y$/i).test(result[i]._DraftOrderFlag)) {
                                result[i]._orderType = 'Draft';
                            }else{
                                result[i]._orderType = 'Order';
                            }
                        } else {
                            result[i]._orderType = '';
                        }
                    }
                    angular.copy(result, $scope.orders);
                }
                $scope.tableParams.reload();
                $scope.refreshIScroll();

                if ($scope.orders.length === 1) {
                    $scope.openOrderDetail($scope.orders[0]);
                }

            }, function (error) { alert(error); });
        }
    };

    $scope.clearSearch = function () {
        $scope.resetForm();
        $scope.orders = angular.copy([], $scope.orders);
        $scope.tableParams.reload();
    }
    $scope.checkedButtonFont = 'fa-check fa-2x';

    $scope.toggleHistorySearch = function () {
        if ($scope.formInput.showHistoryOrders) {
            $scope.formInput.showHistoryOrders = '';
        } else {
            $scope.formInput.showHistoryOrders = 'Y';
        }
    };

    $scope.orders = [];

    $scope.tableParams = new ngTableParams({
        page: 1,            // show first page
        count: 100000,          // count per page
        sorting: {
            _OrderDate: 'desc'     // initial sorting
        }
    }, {
        total: $scope.orders.length, // length of data
        counts: [],
        getData: function ($defer, params) {
            // use build-in angular filter
            var orderedData = params.sorting() ?
                                $filter('orderBy')($scope.orders, params.orderBy()) :
                                $scope.orders;

            $defer.resolve(orderedData.slice((params.page() - 1) * params.count(), params.page() * params.count()));
            $scope.refreshIScroll();
        }
    });

    if ($scope.orders.length > 0) {
        $scope.tableParams.reload();
    }

    $scope.openOrderDetail = function (selectedOrder) {
        $order.setSelectedOrder(selectedOrder);
        $location.path("/orderDetail");
    };

    $scope.data = [];

    $scope.formInput = {
        orderNumber: '',
        phoneNumber: '',
        firstName: '',
        lastName: '',
        email: '',
        zipCode: '',
        giftRegistry: '',
        showHistoryOrders: ''
    };

    $scope.validation = {
        email: { valid: true, isShowError: false, error: '' },
        firstName: { valid: true, isShowError: false, error: '' },
        fullName: { valid: true, isShowError: false, error: '' },
        giftRegistry: { valid: true, isShowError: false, error: '' },
        lastName: { valid: true, isShowError: false, error: '' },
        orderNumber: { valid: true, isShowError: false, error: '' },
        phoneNumber: { valid: true, isShowError: false, error: '' },
        zipCode: { valid: true, isShowError: false, error: '' },
        form: { valid: false, isShowError: false, error: '' }
    };

    var _resetValidation = function () {
        for (prop in $scope.validation) {
            $scope.validation[prop].valid = true;
            $scope.validation[prop].isShowError = false;
            $scope.validation[prop].error = '';
        }
        $scope.validation.form.valid = false;
    };

    $scope.resetForm = function () {
        for (prop in $scope.formInput) {
            $scope.formInput[prop] = '';
        }

        _resetValidation();
    };

    $scope.formatPhone = function () {
        $scope.formInput.phoneNumber = $filter('phoneFormat')($scope.formInput.phoneNumber);
    };

    $scope.validate = function () {

        _resetValidation();

        //trim all inputs
        for (prop in $scope.formInput) {
            $scope.formInput[prop] = $scope.formInput[prop].trim();
        }

        //order number
        if ($scope.formInput.orderNumber) {
            //if any other fields have input, warn that only orderNumber will be used
            if ($scope.formInput.phoneNumber || $scope.formInput.firstName || $scope.formInput.lastName ||
                $scope.formInput.email || $scope.formInput.zipCode || $scope.formInput.giftRegistry) {
                $scope.validation.orderNumber.isShowError = true;
                $scope.validation.orderNumber.error = 'Only order number will be searched.';
            }

            $scope.validation.form.valid = true;
        }

        //email
        if ($scope.formInput.email) {
            if ($scope.formInput.email.length < 5) {
                $scope.validation.email.valid = false;
                $scope.validation.email.error = 'Email requires five characters.';
                $scope.validation.email.isShowError = true;
            }

            if ($scope.validation.email.valid) {
                $scope.validation.form.valid = true;
            }
        }

        //phoneNumber
        if ($scope.formInput.phoneNumber) {
            var testNumbers = $filter('phoneFormatRemove')($scope.formInput.phoneNumber);
            if (testNumbers.length != 10) {
                $scope.validation.phoneNumber.valid = false;
                $scope.validation.phoneNumber.error = 'Phone must be 10 digits.';
                $scope.validation.phoneNumber.isShowError = true;
            }

            if ($scope.validation.phoneNumber.valid) {
                $scope.validation.form.valid = true;
            }
        }

        //first and last name
        if ($scope.formInput.firstName || $scope.formInput.lastName) {

            if ($scope.formInput.firstName.length < 1) {
                $scope.validation.firstName.valid = false;
                $scope.validation.firstName.error = 'First Name requires one character.';
                $scope.validation.firstName.isShowError = true;
            }

            if ($scope.formInput.lastName.length < 1) {
                $scope.validation.lastName.valid = false;
                $scope.validation.lastName.error = 'Last Name requires one character.'; //'Last Name: 1 character min';
                $scope.validation.lastName.isShowError = true;
            }

            var tempTotal = $scope.formInput.firstName.length + $scope.formInput.lastName.length;

            if (tempTotal < 4) {
                $scope.validation.fullName.valid = false;
                $scope.validation.fullName.error = 'Four character minimum between First and Last Name.';//'Min 4 character between First and Last Name';
                $scope.validation.fullName.isShowError = true;
            }

            if ($scope.validation.fullName.valid && $scope.validation.firstName.valid && $scope.validation.lastName.valid) {
                $scope.validation.form.valid = true;
            }

        }

        //zipCode
        if ($scope.formInput.zipCode) {
            if ($scope.formInput.zipCode.length != 5) {
                $scope.validation.zipCode.valid = false;
                $scope.validation.zipCode.error = 'Zip must be 5 digits.';
                $scope.validation.zipCode.isShowError = true;
            } else {
                var otherValidInput = false;

                if ($scope.formInput.orderNumber && $scope.validation.orderNumber.valid) {
                    otherValidInput = true;
                }
                if ($scope.formInput.email && $scope.validation.email.valid) {
                    otherValidInput = true;
                }
                if ($scope.formInput.phoneNumber && $scope.validation.phoneNumber.valid) {
                    otherValidInput = true;
                }
                if ($scope.formInput.firstName && $scope.validation.firstName.valid) {
                    otherValidInput = true;
                }
                if ($scope.formInput.lastName && $scope.validation.lastName.valid) {
                    otherValidInput = true;
                }
                if ($scope.formInput.giftRegistry && $scope.validation.giftRegistry.valid) {
                    otherValidInput = true;
                }

                if (!otherValidInput) {
                    $scope.validation.zipCode.valid = false;
                    $scope.validation.zipCode.error = 'Zip cannot be searched alone. Fill in secondary input.';
                    $scope.validation.zipCode.isShowError = true;
                }
            }

            if ($scope.validation.zipCode.valid) {
                $scope.validation.form.valid = true;
            }
        }


        if ($scope.formInput.giftRegistry && $scope.validation.giftRegistry.valid) {
            $scope.validation.form.valid = true;
        }

        //if phone number or zipcode is not valid block search unless there is an order number input
        if (!$scope.formInput.orderNumber && (!$scope.validation.phoneNumber.valid || !$scope.validation.zipCode.valid)) {
            $scope.validation.form.valid = false;
        }
    };

    $scope.refreshIScroll = function () {
        setTimeout(function () {
            $scope.$parent.myScroll['wrapper'].refresh();
        }, 500);
    };

    var _init = function () {
        $scope.orders = $order.getPreviousSearchResult();
        $scope.tableParams.reload();
        $scope.refreshIScroll();
    };

    _init();

    var handleBarCodeScan = function (event, barcodeData) {
        if (!angular.isString(barcodeData)) {
            barcodeData = (!angular.isDefined(barcodeData) || barcodeData === null) ? '' : barcodeData.toString().trim();
        }

        //so highjump pack slips can be scanned as well truncate to 9 digits (chars)
        $scope.formInput.orderNumber = barcodeData.substring(0,9);
        $scope.validate();
        $scope.search(false);
    };

    var deregister = $scope.$on('Scanner_Event', handleBarCodeScan);

    $scope.$on("$destroy", function () {
        deregister();
    });
}]);

