angular.module('modifyCustomer', ['ui.bootstrap'])
.controller('modifyCustomerCtrl', ['$scope', 'customer', '$filter', 'contactIdArg', '$modalInstance', 'loggerService', 'customerKeyPressValidator', function ($scope, $customer, $filter, contactIdArg, $modalInstance, $loggerService, $customerKeyPressValidator) {

    $scope.contact = {};
    var contactID = contactIdArg;
    var customer = {};
    $scope.isShowValidationMessages = false;
    $scope.keyPressValidator = $customerKeyPressValidator;



    var _locateContact = function () {
        for (var i = 0; i < customer.CustomerContactList.CustomerContact.length; i++) {
            if (customer.CustomerContactList.CustomerContact[i]._CustomerContactID.toString().trim() == contactID) {
                return customer.CustomerContactList.CustomerContact[i];
            }
        }

        $loggerService.log( "No contact found! " );
    };

    var _init = function () {
        customer = angular.copy($customer.getSelectedCustomer());
        $scope.isValid = angular.copy(initValid);
        $scope.contact = _locateContact();

        $scope.formatPhone('_DayPhone');
        $scope.formatPhone('_EveningPhone');
        $scope.isShowValidationMessages = false;
    };

    $scope.formatPhone = function (contactAttrName) {
        $scope.contact[contactAttrName] = $filter('phoneFormat')($scope.contact[contactAttrName]);
    };

    $scope.cancel = function () { $modalInstance.dismiss(); };

    $scope.reset = function () {
        _init();
    };

    var initValid = {
        _FirstName: {isValid:true, errorText:''},
        _MiddleName: { isValid: true, errorText: '' },
        _LastName: { isValid: true, errorText: '' },
        _EmailID: { isValid: true, errorText: '' },
        _DayPhone: { isValid: true, errorText: '' },
        _EveningPhone: { isValid: true, errorText: '' },

    };

    $scope.isValid = {};

    var _allValidationIsValid = function () {
        
        for (var prop in $scope.isValid) {
            if (!$scope.isValid[prop].isValid) {
                return false;
            }
        }

        return true;
    };

    $scope.validate = function () {
        $scope.isValid = angular.copy(initValid);

        if (!$scope.contact._FirstName || !$scope.contact._FirstName.trim()) {
            $scope.isValid._FirstName.isValid = false;
            $scope.isValid._FirstName.errorText = 'First Name Required';
        }
        if (!$scope.contact._LastName || !$scope.contact._LastName.trim()) {
            $scope.isValid._LastName.isValid = false;
            $scope.isValid._LastName.errorText = 'Last Name Required';
        }
        if (!$scope.contact._DayPhone || !$scope.contact._DayPhone.trim()) {
            $scope.isValid._DayPhone.isValid = false;
            $scope.isValid._DayPhone.errorText = 'Primary Phone Number Required';
        } else {
            var phoneTemp = $filter('phoneFormatRemove')($scope.contact._DayPhone);
            if (phoneTemp.length !== 10) {
                $scope.isValid._DayPhone.isValid = false;
                $scope.isValid._DayPhone.errorText = 'Enter 10 Digit Phone Number';
            }
        }

        if ($scope.contact._EveningPhone && $scope.contact._EveningPhone.trim()) {
            var phoneTemp = $filter('phoneFormatRemove')($scope.contact._EveningPhone);
            if (phoneTemp.length !== 10) {
                $scope.isValid._EveningPhone.isValid = false;
                $scope.isValid._EveningPhone.errorText = 'Enter 10 Digit Phone Number';
            }
        }

        if (!$scope.contact._EmailID || !$scope.contact._EmailID.trim()) {
            $scope.isValid._EmailID.isValid = false;
            $scope.isValid._EmailID.errorText = 'Email Required';
        } else {
            if (!(/.+@.+\..+/).test($scope.contact._EmailID)) {
                $scope.isValid._EmailID.isValid = false;
                $scope.isValid._EmailID.errorText = 'Invalid Email';
            }
       }
    };


    $scope.save = function () {
        $scope.validate();
        $scope.isShowValidationMessages = true;

        if (_allValidationIsValid()) {
            $customer.updateCustomerContact(customer, function () { $modalInstance.close(); }, function (data) { $loggerService.log(data); });
        }

    };
        
        _init();

}]);