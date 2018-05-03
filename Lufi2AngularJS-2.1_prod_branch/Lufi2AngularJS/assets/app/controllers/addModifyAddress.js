angular.module('addModifyAddress', ['ui.bootstrap'])
    .controller('addModifyAddressCtrl', ['$scope', '$stateParams', 'customer', '$state', 'appState', 'orderCart', '$modal', '$q', '$filter', 'loggerService', 'isPoBoxAddress', 'customerKeyPressValidator',
        function ($scope, $stateParams, $customer, $state, $appState, $orderCart, $modal, $q, $filter, $loggerService, $isPoBoxAddress, $customerKeyPressValidator) {
        $scope.title = 'Add Address';
        $scope.address = {};
        $scope.addressResponse = false;
        $scope.emailResponse = false;
        $scope.emailValidated = false;
        $scope.addressValidated = false;
        $scope.AllEntriesValid = true;
        $scope.EnterAddress = false;
        $scope.EnterCity = false;
        $scope.EnterState = false;
        $scope.EnterZip = false;
        $scope.EnterFirstName = false;
        $scope.EnterLastName = false;
        $scope.EnterPhone = false;
        $scope.EnterEmail = false;
        $scope.international = false;
        $scope.disableShipTo = false;
        $scope.disableBillTo = false;
        $scope.disableCountry = false;
        $scope.avsOverride = false;




        $scope.keyPressValidator = $customerKeyPressValidator;
        var additionalAddressModel = {
            "_AddressType": "SB",
            "_CustomerAdditionalAddressID": "",
            "_IsBillTo": "Y",
            "_IsDefaultBillTo": "N",
            "_IsDefaultShipTo": "N",
            "_IsShipTo": "Y",
            "PersonInfo": {
                "_AddressID": "",
                "_AddressLine1": "",
                "_AddressLine2": "",
                "_AddressLine3": "",
                "_AddressLine4": "",
                "_AddressLine5": "",
                "_AddressLine6": "",
                "_City": "",
                "_Country": "US",
                "_DayPhone": "",
                "_EMailID": "",
                "_EveningPhone": "",
                "_FirstName": "",
                "_IsCommercialAddress": "N",
                "_LastName": "",
                "_MiddleName": "",
                "_MobilePhone": "",
                "_OtherPhone": "",
                "_State": "",
                "_Suffix": "",
                "_Title": "",
                "_ZipCode": ""
            }
        };

        //customerContactID/:customerAdditionalAddressID
        var contactID = $stateParams.customerContactID.toString().trim();
        var addressID = $stateParams.customerAdditionalAddressID.toString().trim();
        var customer = $customer.getSelectedCustomer();
        var isNewCustomer = false;
        var realAdditionalAddress = {};

        var _locateContact = function () {
            for (var i = 0; i < customer.CustomerContactList.CustomerContact.length; i++) {
                if (customer.CustomerContactList.CustomerContact[i]._CustomerContactID.toString().trim() == contactID) {
                    return customer.CustomerContactList.CustomerContact[i];
                }
            }

            throw { name: "No contact found! " };
        };

        var _locateAddress = function () {

            //find matching contact
            var contact = _locateContact();

            for (var i = 0; i < contact.CustomerAdditionalAddressList.CustomerAdditionalAddress.length; i++) {
                if (contact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._CustomerAdditionalAddressID.toString().trim() === addressID) {
                    realAdditionalAddress = contact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i];
                    $scope.address = angular.copy(contact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]);

                    //change phonenumbers to phoneFormat
                    $scope.formatPhone('_DayPhone');
                    $scope.formatPhone('_EveningPhone');
                }
            }
        };

        var _setNewCustomerAddress = function () {

            //add on any search parameters use in customer search
            var parms = $customer.getCachedQueryParam();
            if (angular.isDefined(parms) && parms !== null &&
                angular.isDefined(parms.Customer) && parms.Customer !== null) {
                if (angular.isString(parms.Customer.FirstName)) {
                    $scope.address.PersonInfo._FirstName = $filter('titlecase')(parms.Customer.FirstName.trim());
                }
                if (angular.isString(parms.Customer.LastName)) {
                    $scope.address.PersonInfo._LastName = $filter('titlecase')(parms.Customer.LastName.trim());
                }
                if (angular.isString(parms.Customer.EmailAddress)) {
                    $scope.address.PersonInfo._EMailID = parms.Customer.EmailAddress.trim();
                }
                if (angular.isString(parms.Customer.PhoneNumber)) {
                    $scope.address.PersonInfo._DayPhone = $filter('phoneFormat')(parms.Customer.PhoneNumber.trim());
                }
            }
            $scope.address._IsDefaultBillTo = 'Y';
            $scope.address._IsDefaultShipTo = 'Y';
            isNewCustomer = true;
        };

        var _initializeData = function () {
            if (contactID.length > 0 && addressID.length > 0) {
                $scope.title = 'Modify Address';
                _locateAddress();
            } else if (contactID.length > 0) {
                $scope.title = 'Add Address';
                $scope.address = angular.copy(additionalAddressModel);
            } else {
                $scope.title = 'New Customer';
                $scope.address = angular.copy(additionalAddressModel);
                _setNewCustomerAddress();
            }
                $scope.setCheckBoxFromFlag();
        };

        var _copyAllAttributes = function (source, destination) {
            var keyArr = Object.keys(source);

            for (var i = 0; i < keyArr.length; i++) {
                //if key is attribute
                if ((/^_/).test(keyArr[i])) {

                    destination[keyArr[i]] = source[keyArr[i]];
                }
            }
        };
        $scope.formatPhone = function (addressAttrName) {
            $scope.address.PersonInfo[addressAttrName] = $filter('phoneFormat')($scope.address.PersonInfo[addressAttrName]);
        };

        $scope.cancel = function () {
            if (addressChangeObj.previousStateName) {
                $state.go(addressChangeObj.previousStateName.toString().trim());
            } else if (isNewCustomer) {
                $state.go('customerSearch');
            } else {
                $state.go('customerDetail');
            }
        };

        $scope.reset = function () {
            if (addressID) {
                $scope.address = realAdditionalAddress;

                //change phonenumbers to phoneFormat
                $scope.formatPhone('_DayPhone');
                $scope.formatPhone('_EveningPhone');
                $scope.setCheckBoxFromFlag();

            } else {
                $scope.address = angular.copy(additionalAddressModel);

                if (isNewCustomer) {
                    _setNewCustomerAddress();
                }
                $scope.setCheckBoxFromFlag();
            }
        };


        $scope.setCheckBoxFromFlag = function() {
                if ($scope.address._IsDefaultBillTo == 'Y') {
                    $scope.defaultBilling = true;
                    $scope.disableBillTo = true;
                    } else {
                    $scope.defaultBilling = false;
                }
                if ($scope.address._IsDefaultShipTo == 'Y') {
                    $scope.defaultShipping = true;
                    $scope.disableShipTo = true;
                    } else {
                    $scope.defaultShipping = false;
    }

                    }
        $scope.setFlagFromCheckBox = function() {
            if ($scope.defaultBilling) {
                $scope.address._IsDefaultBillTo = 'Y';
            } else {
                $scope.address._IsDefaultBillTo = 'N';
                 }
            if ($scope.defaultShipping) {
                $scope.address._IsDefaultShipTo = 'Y';
            } else {
                $scope.address._IsDefaultShipTo = 'N';
        }
    };

        //check for appServiceAppState data
        var addressChangeObj = $appState.getAddressChange();
        //previousStateName: null, customerKey: null, setShipToOrderLinePrimeSubLineObjArray: []

        if (addressChangeObj.customerKey !== null && addressChangeObj.customerKey.toString().trim().length > 0) {
            //check that selected customer is same as one we are changing.
            if (customer._CustomerKey != addressChangeObj.customerKey.toString().trim()) {
                //get customer
                $customer.retrieveCustomerDetail("", addressChangeObj.customerKey.toString(), function () {
                    _initializeData();
                }, function () { });
            } else {
                _initializeData();
            }

        } else {
            _initializeData();
            };

        if ($scope.address.PersonInfo._Country != "US") {
            $scope.international = true;
            }

        $scope.save = function () {
            $scope.setFlagFromCheckBox();
            //Avoid AVS for international billing address
            $scope.address.PersonInfo._Country = $scope.address.PersonInfo._Country.toUpperCase();
            if ($scope.address.PersonInfo._Country.trim() != "US" && $scope.address.PersonInfo._Country.trim() != "UNITED STATES OF AMERICA") {
                $scope.international = true;
            } else {
                $scope.international = false;
        }
            if ($scope.international == true) {
                $scope.validateInternational();
                if ($scope.AllEntriesValid == true) {
                    if ($scope.defaultBilling == true) {
                        $scope.address._IsDefaultBillTo == 'Y';
                    } else {
                        $scope.address._IsDefaultBillTo == 'N';
                }
                    $scope.updateCustomer();
                }
                //Avoid AVS for address override 
            } else if ($scope.avsOverride == true){
                $scope.validateFields();
                if ($scope.AllEntriesValid == false) { }
                else {
                    $scope.updateCustomer();
                }            
            }else{
                //AVS FOR US ADDRESS
            $scope.validateFields();
                if ($scope.AllEntriesValid == false) { }
                else {
                $scope.addressResponse = true;
                $scope.emailResponse = true;
                $scope.emailValidated = false;
                $scope.addressValidated = false;
                addressToVerify =[];
                addressToVerify.line1 = $scope.address.PersonInfo._AddressLine1;
                addressToVerify.line2 = $scope.address.PersonInfo._AddressLine2;
                addressToVerify.line3 = $scope.address.PersonInfo._AddressLine3;
                addressToVerify.city = $scope.address.PersonInfo._City;
                addressToVerify.state = $scope.address.PersonInfo._State;
                addressToVerify.zip = $scope.address.PersonInfo._ZipCode;
                addressToVerify.country = $scope.address.PersonInfo._Country;
                var emailToVerify = $scope.address.PersonInfo._EMailID;
                if (addressToVerify.line2 === null) {
                    addressToVerify.line2 = "";
                }
                if (addressToVerify.line3 === null) {
                    addressToVerify.line3 = "";
                }
                address = addressToVerify.line1 + "|" +addressToVerify.line2 + "|" +addressToVerify.line3 + "|" +addressToVerify.city + "|" +addressToVerify.state + "|" +addressToVerify.zip;

                $customer.verifyCustomerAddress(address).success(function (response) {
                    $scope.addressResponse = false;
                    if (response.QASearchResult._VerifyLevel == "Verified") {
                        $scope.address.PersonInfo._AddressLine1 = response.QASearchResult.QAAddress.AddressLine[0].Line;
                        $scope.address.PersonInfo._AddressLine2 = response.QASearchResult.QAAddress.AddressLine[1].Line;
                        $scope.address.PersonInfo._AddressLine3 = response.QASearchResult.QAAddress.AddressLine[2].Line;
                        $scope.address.PersonInfo._City = response.QASearchResult.QAAddress.AddressLine[3].Line;
                        $scope.address.PersonInfo._State = response.QASearchResult.QAAddress.AddressLine[4].Line;
                        $scope.address.PersonInfo._ZipCode = response.QASearchResult.QAAddress.AddressLine[5].Line.substring(0, 5);
                        if (response.QASearchResult.QAAddress.AddressLine[6].Line = "UNITED STATES OF AMERICA") {
                            $scope.address.PersonInfo._Country = "US";
                    }
                            if ($scope.address.PersonInfo._AddressLine2 === null) { $scope.address.PersonInfo._AddressLine2 = "";
                    }
                            if ($scope.address.PersonInfo._AddressLine3 === null) { $scope.address.PersonInfo._AddressLine3 = "";
                    }
                        $scope.addressValidated = true;
                        if ($scope.addressValidated == true && $scope.emailValidated == true && $scope.AllEntriesValid == true) {
                                $scope.updateCustomer();
                    }
                    } else if ((response.QASearchResult._VerifyLevel == "PremisesPartial") || (response.QASearchResult._VerifyLevel == "StreetPartial") || (response.QASearchResult._VerifyLevel == "Multiple")) {
                        $scope.openAddressSelection(response);

                    } else {
                        swal({title: "Error!", text: "Please enter a valid address", showConfirmButton: true
                    });
                }
                    $loggerService.log(response);
                }).error(function (data) {
                    // swal({title: "Error!", text: data, showConfirmButton: true
                    $scope.addressValidated = true;
                    if ($scope.addressValidated == true && $scope.emailValidated == true && $scope.AllEntriesValid == true) {
                        $scope.updateCustomer();
                    }
                });

                    $customer.verifyEmail(emailToVerify).success(function (response) {
                        $scope.emailResponse = false;
                        if (response.Certainty != "undeliverable") {
                            $scope.address.PersonInfo._EMailID = emailToVerify;
                            $loggerService.log(response);
                            $scope.EnterEmail = false;
                            $scope.emailValidated = true;

                            if ($scope.addressValidated == true && $scope.emailValidated == true && $scope.AllEntriesValid == true) {
                                $scope.updateCustomer();
                        }

                        } else {
                            $loggerService.log(response);
                            $scope.EnterEmail = true;
                    }
                    }).error(function (data) {
                        $scope.emailValidated = true;
                        if ($scope.addressValidated == true && $scope.emailValidated == true && $scope.AllEntriesValid == true) {
                            $scope.updateCustomer();
                        }
                });

            }

            }
    
};

        $scope.updateCustomer = function () {
                            $scope.address.PersonInfo._DayPhone = $filter('phoneFormatRemove') ($scope.address.PersonInfo._DayPhone);
                            $scope.address.PersonInfo._EveningPhone = $filter('phoneFormatRemove') ($scope.address.PersonInfo._EveningPhone);
							var stateName = 'customerDetail';
							if (addressChangeObj.previousStateName) {
							stateName = addressChangeObj.previousStateName.toString().trim();
							}
                            if (isNewCustomer) {
                                $customer.createCustomerByAddress($scope.address, function (response) { $state.go(stateName);
                            }, function (response) {
                               $loggerService.log(response);
                            });
                            }
                            else {
                                $customer.addModifyAddress(customer._CustomerID, contactID, $scope.address, function () {
                                    if (angular.isDefined(addressChangeObj.setShipToOrderLinePrimeSubLineObjArray) && addressChangeObj.setShipToOrderLinePrimeSubLineObjArray.length > 0) {
                                        try {
                                            $orderCart.address.setOrderLineShipToAddresses(addressChangeObj.setShipToOrderLinePrimeSubLineObjArray, $scope.address, false);
                                            } catch (e) { //do nothing on failure of setting ship to, just change state
                                            $loggerService.log(e);
                                    }
                                        } else if (addressChangeObj.setBillTo) {
                                        $orderCart.address.setBillingAddress($scope.address)
                                }
                                    $state.go(stateName);
                            });
        }

        }

        $scope.countryInfoMessage = function () {
            country = $scope.address.PersonInfo._Country;
            if (country != "US") {
                swal({ title: "", text: "International address can only be used as a billing address", showConfirmButton: true });
                $scope.defaultShipping = false;
                $scope.disableShipTo = true;
                $scope.international = true;
            } else {
                $scope.international = false;
                if ($scope.address._IsDefaultShipTo == 'Y') {
                    $scope.disableShipTo = true;
                } else {
                    $scope.disableShipTo = false;
}    
            }

            if ($scope.defaultShipping == true) {
                $scope.address._IsDefaultShipTo == 'Y';
            } else {
                $scope.address._IsDefaultShipTo == 'N';
            }
        }

        $scope.validateFields = function () {

            //if ($scope.address._IsDefaultShipTo || $scope.address._IsShipTo) {
            if ($scope.address._IsDefaultShipTo == 'Y') {
                if ($isPoBoxAddress($scope.address.PersonInfo)) {
                    swal({ title: "", text: "Ship to address can not be a PO Box", showConfirmButton: true });
                    var isPoBox = true;
                } else {
                    isPoBox = false;
                }
            }

if ($scope.address.PersonInfo._AddressLine1 == "") {
                $scope.EnterAddress = true;
} else {
                $scope.EnterAddress = false;
                };
if ($scope.address.PersonInfo._City == "") {
                $scope.EnterCity = true;
                } else {
                $scope.EnterCity = false;
                };
if ($scope.address.PersonInfo._State == "") {
                $scope.EnterState = true;
                } else {
                $scope.EnterState = false;
                };
if ($scope.address.PersonInfo._ZipCode == "") {
                $scope.EnterZip = true;
                } else {
                $scope.EnterZip = false;
                };
if ($scope.address.PersonInfo._FirstName == "") {
                $scope.EnterFirstName = true;
                } else {
                $scope.EnterFirstName = false;
                };
if ($scope.address.PersonInfo._LastName == "") {
                $scope.EnterLastName = true;
                } else {
                $scope.EnterLastName = false;
                };
if (($scope.address.PersonInfo._EMailID == "") || ($scope.address.PersonInfo._EMailID.indexOf('@') === -1) || ($scope.address.PersonInfo._EMailID.indexOf('.') === -1)) {
                $scope.EnterEmail = true;
                } else {
                $scope.EnterEmail = false;
                };
if ($scope.address.PersonInfo._DayPhone == "") {
                $scope.EnterPhone = true;
                } else {
                $scope.EnterPhone = false;
                };
if ($scope.EnterAddress|| $scope.EnterCity|| $scope.EnterState|| $scope.EnterZip|| $scope.EnterFirstName|| $scope.EnterLastName|| $scope.EnterEmail|| $scope.EnterPhone || isPoBox) {
                $scope.AllEntriesValid = false;
                } else {
                $scope.AllEntriesValid = true;
    };
        };

        $scope.validateInternational = function () {
            if ($scope.address.PersonInfo._AddressLine1 == "") {
                $scope.EnterAddress = true;
        } else {
                $scope.EnterAddress = false;
        };
        if ($scope.address.PersonInfo._FirstName == "") {
                $scope.EnterFirstName = true;
        } else {
                $scope.EnterFirstName = false;
        };
        if ($scope.address.PersonInfo._LastName == "") {
                $scope.EnterLastName = true;
        } else {
                $scope.EnterLastName = false;
        };
        if (($scope.address.PersonInfo._EMailID == "") || ($scope.address.PersonInfo._EMailID.indexOf('@') === -1) || ($scope.address.PersonInfo._EMailID.indexOf('.') === -1)){
                $scope.EnterEmail = true;
        } else {
                $scope.EnterEmail = false;
        };
        if ($scope.address.PersonInfo._DayPhone == "") {
                $scope.EnterPhone = true;
        } else {
                $scope.EnterPhone = false;
        };
        if ($scope.EnterAddress == true || $scope.EnterFirstName == true || $scope.EnterLastName == true || $scope.EnterEmail == true || $scope.EnterPhone == true) {
                $scope.AllEntriesValid = false;
        } else {
                $scope.AllEntriesValid = true;
        };

        }


        $scope.CountryOptions = [
        { id: 'AC', country: 'Ascension Island'},
        {
            id: 'AD', country: 'Andorra'},
        {
            id: 'AE', country: 'United Arab Emirates'},
        {
            id: 'AF', country: 'Afghanistan'},
        {
            id: 'AG', country: 'Antigua and Barbuda'
            },
        {
            id: 'AI', country: 'Anguilla'
            },
        {
            id: 'AL', country: 'Albania'
            },
        {
            id: 'AM', country: 'Armenia'
            },
        {
            id: 'AN', country: 'Netherlands Antilles'
            },
        {
            id: 'AO', country: 'Angola'},
        {
            id: 'AQ', country: 'Antarctica'
            },
        {
            id: 'AR', country: 'Argentina'
            },
        {
            id: ' AS', country: 'American Samoa'
            },
        {
            id: 'AT', country: 'Austria'
            },
        {
            id: 'AU', country: 'Australia'
            },
        {
            id: 'AW', country: 'Aruba'
        },
        {
            id: 'AX', country: 'Aland Islands'
            },
        {
            id: 'AZ', country: 'Azerbaijan'
        },
        {
            id: 'BA', country: 'Bosnia and Herzegovina'
            },
                {id: 'BB', country: 'Barbados'
        },
               {id: 'BD', country: 'Bangladesh'
        },
               {id: 'BE', country: 'Belgium'
        },
                    { id: 'BF', country: 'Burkina Faso'
        },
                                {id: 'BG', country: 'Bulgaria'
        },
                                    {id: 'BH', country: 'Bahrain'
        },
        {
            id: 'BI', country: 'Burundi'},
                        {id: 'BJ', country: 'Benin'},
        {
            id: ' BM', country: 'Bermuda'
            },
                            {id: 'BN', country: 'Brunei Darussalam'
        },
                        {id: 'BO', country: 'Bolivia'},
                                {id: 'BR', country: 'Brazil' },
                                        {id: 'BS', country: 'Bahamas'
        },
        {
            id: 'BT', country: 'Bhutan'
            },
        {
            id: 'BV', country: 'Bouvet Island'
            },
        {
            id: 'BW', country: 'Botswana'
            },
        {
            id: 'BY', country: 'Belarus'
            },
        {
            id: 'BZ', country: 'Belize'
            },
        {
            id: 'CA', country: 'Canada'
            },
        {
            id: 'CC', country: 'Cocos (Keeling) Islands'
        },
        {
            id: 'CD', country: 'Congo, Democratic Republic'
            },
                                {id: 'CF', country: 'Central African Republic'},
        {
            id: 'CG', country: 'Congo'
            },
                {id: 'CH', country: 'Switzerland'
        },
            {id: 'CI', country: 'Cote DIvoire (Ivory Coast)'
            },
                {id: 'CK', country: 'Cook Islands'
                },
                    {id: 'CL', country: 'Chile'
        },
                {id: 'CM', country: 'Cameroon'},
        {
            id: 'CN', country: 'China'},
        {
            id: 'CO', country: 'Colombia'
            },
        {
            id: 'CR', country: 'Costa Rica'
            },
        {
            id: 'CS', country: 'Czechoslovakia (former)'
        },
                                {id: 'CU', country: 'Cuba'
                                },
                                    {id: 'CV', country: 'Cape Verde'
        },
        { id: 'CX', country: 'Christmas Island'
        },
        {
            id: 'CY', country: 'Cyprus'},
        {
            id: 'CZ', country: 'Czech Republic'
            },
        {
            id: 'DE', country: 'Germany'
            },
            {
            id: 'DJ', country: 'Djibouti'
            },
            {id: 'DK', country: 'Denmark'
            },
                {id: 'DM', country: 'Dominica'
                },
                    {id: 'DO', country: 'Dominican Republic'
                    },
        {
            id: 'DZ', country: 'Algeria'
            },
                {
                id: 'EC', country: 'Ecuador'
                },
                    {id: 'EE', country: 'Estonia'
        },
            {id: 'EG', country: 'Egypt'
        },
                { id: 'EH', country: 'Western Sahara'
        },
            {
            id: 'ER', country: 'Eritrea'
        },
                {id: 'ES', country: 'Spain'
        },
            {id: 'ET', country: 'Ethiopia'},
        {
            id: 'EU', country: 'European Union'
        },
                {id: 'FI', country: 'Finland'
        },
                {id: 'FJ', country: 'Fiji'
        },
                {id: 'FK', country: 'Falkland Islands (Malvinas)'
        },
            { id: 'FM', country: 'Micronesia'
        },
            {id: 'FO', country: 'Faroe Islands'
            },
                {id: 'FR', country: 'France'},
        {
            id: 'FX', country: 'France, Metropolitan' },
        {
            id: 'GA', country: 'Gabon'},
                                {id: 'GB', country: 'Great Britain (UK)'
                                },
                                    {id: 'GD', country: 'Grenada'
        },
                {id: 'GE', country: 'Georgia'
        },
        {
            id: 'GF', country: 'French Guiana'
            },
            {
            id: 'GG', country: 'Guernsey'
            },
                {
                id: 'GH', country: 'Ghana'
                },
                    {id: 'GI', country: 'Gibraltar'
                    },
                        {id: 'GL', country: 'Greenland'
                        },
                            {id: 'GM', country: 'Gambia'
        },
            {id: 'GN', country: 'Guinea'
        },
            {id: 'GP', country: 'Guadeloupe'
        },
            {id: 'GQ', country: 'Equatorial Guinea'
            },
        {
            id: 'GR', country: 'Greece'
            },
        {
            id: 'GS', country: 'S. Georgia and S. Sandwich Isls.'
        },
            {id: 'GT', country: 'Guatemala'},
        {
            id: 'GU', country: 'Guam'
            },
                {id: 'GW', country: 'Guinea-Bissau'
                },
                    {id: 'GY', country: 'Guyana'},
            { id: 'HK', country: 'Hong Kong'},
        {
            id: 'HM', country: 'Heard and McDonald Islands'
            },
                {id: 'HN', country: 'Honduras'
        },
                {id: 'HR', country: 'Croatia (Hrvatska)'
                },
                    {id: 'HT', country: 'Haiti'
        },
               {
               id: 'HU', country: 'Hungary'},
        {
            id: 'ID', country: 'Indonesia'
            },
        {
            id: 'IE', country: 'Ireland'
            },
            {id: 'IL', country: 'Israel'
            },
                {id: 'IM', country: 'Isle of Man'
                },
        {
            id: 'IN', country: 'India'},
        {
            id: 'IO', country: 'British Indian Ocean Territory'
        },
            { id: 'IQ', country: 'Iraq'
        },
                {id: 'IR', country: 'Iran'},
        {
            id: 'IS', country: 'Iceland'
            },
                {id: 'IT', country: 'Italy'
                },
                    {id: 'JE', country: 'Jersey'},
        {
            id: 'JM', country: 'Jamaica'},
        {
            id: 'JO', country: 'Jordan'},
            {
                id: 'JP', country: 'Japan'},
        {
            id: 'KE', country: 'Kenya'
            },
        {
            id: 'KG', country: 'Kyrgyzstan'
            },
        {
            id: 'KH', country: 'Cambodia'
            },
        {
            id: 'KI', country: 'Kiribati'
            },
        {
            id: 'KM', country: 'Comoros'
            },
        {
            id: 'KN', country: 'Saint Kitts and Nevis'
        },
                {id: 'KP', country: 'Korea (North)'
        },
            { id: 'KR', country: 'Korea (South)'
        },
            {id: 'KW', country: 'Kuwait'
        },
                { id: 'KY', country: 'Cayman Islands'
        },
            {id: 'KZ', country: 'Kazakhstan'
        },
                {id: 'LA', country: 'Laos'},
                                {id: 'LB', country: 'Lebanon'},
        {
            id: 'LC', country: 'Saint Lucia'
        },
            { id: 'LI', country: 'Liechtenstein'},
        {
            id: 'LK', country: 'Sri Lanka'},
        {
            id: 'LR', country: 'Liberia'},
        {
            id: 'LS', country: 'Lesotho'
            },
                {id: 'LT', country: 'Lithuania '
                },
                    {id: 'LU', country: 'Luxembourg'
                    },
                        {id: 'LV', country: 'Latvia'
        },
            {
            id: 'LY', country: 'Libya'
        },
                {id: 'MA', country: 'Morocco'
        },
                {id: 'MC', country: 'Monaco'},
                {id: 'MD', country: 'Moldova'},
        {
            id: 'ME', country: 'Montenegro'},
        {
            id: 'MG', country: 'Madagascar'
            },
        {
            id: 'MH', country: 'Marshall Islands' },
        {
            id: 'MK', country: 'F.Y.R.O.M. (Macedonia)'
        },
                { id: 'ML', country: 'Mali'
        },
                {id: 'MM', country: 'Myanmar'
        },
                {id: 'MN', country: 'Mongolia'},
        {
            id: 'MO', country: 'Macau'},
        {
            id: 'MP', country: 'Northern Mariana Islands' },
                {id: 'MQ', country: 'Martinique'},
        {
            id: 'MR', country: 'Mauritania'
            },
        {
            id: 'MS', country: 'Montserrat'},
        {
            id: 'MT', country: 'Malta'},
        {
            id: 'MU', country: 'Mauritius'},
                                    {id: 'MV', country: 'Maldives'
        },
                {id: 'MW', country: 'Malawi'},
        {
            id: 'MX', country: 'Mexico'},
        {
            id: 'MY', country: 'Malaysia'
            },
        {
            id: 'MZ', country: 'Mozambique'
            },
                                { id: 'NA', country: 'Namibia'
        },
        {
            id: 'NC', country: 'New Caledonia'
            },
        {
            id: 'NE', country: 'Niger'
            },
        {
            id: 'NF', country: 'Norfolk Island'
            },
        {
            id: 'NG', country: 'Nigeria'
            },
        {
            id: 'NI', country: 'Nicaragua'
            },
            {
            id: 'NL', country: 'Netherlands'
        },
            { id: 'NO', country: 'Norway'
        },
                                    {id: 'NP', country: 'Nepal'
                                    },
                                        {id: 'NR', country: 'Nauru'
                                        },
                                            {id: 'NT', country: 'Neutral Zone'
        },
            {id: 'NU', country: 'Niue'
        },
        {
            id: 'NZ', country: 'New Zealand (Aotearoa)' },
        {
            id: 'OM', country: 'Oman'},
        {
            id: 'PA', country: 'Panama'
        },
            {id: 'PE', country: 'Peru'
        },
        {
            id: 'PF', country: 'French Polynesia'
            },
        {
            id: 'PG', country: 'Papua New Guinea'
            },
        {
            id: 'PH', country: 'Philippines'
            },
                {id: 'PK', country: 'Pakistan'
        },
                                    { id: 'PL', country: 'Poland'
                                    },
                                        {id: 'PM', country: 'St. Pierre and Miquelon'
                                        },
        {
            id: 'PN', country: 'Pitcairn'},
                                    {id: 'PR', country: 'Puerto Rico'},
        {
            id: 'PS', country: 'Palestinian Territory, Occupied'
            },
        {
            id: 'PT', country: 'Portugal'
        },
        {
            id: 'PW', country: 'Palau'
            },
        {
            id: 'PY', country: 'Paraguay'
        },
        {
            id: 'QA', country: 'Qatar'
        },
                                    {id: 'RE', country: 'Reunion'
                                    },
                                        {id: 'RS', country: 'Serbia'
        },
                                    {id: 'RO', country: 'Romania'
        },
                                        {id: 'RU', country: 'Russian Federation'
        },
        {
            id: 'RW', country: 'Rwanda'
        },
        {
            id: 'SA', country: 'Saudi Arabia'
            },
        {
            id: 'SB', country: 'Solomon Islands'
            },
        {
            id: 'SC', country: 'Seychelles'
            },
        {
            id: 'SD', country: 'Sudan'
            },
        {
            id: 'SE', country: 'Sweden'
        },
                                    {id: 'SG', country: 'Singapore'
        },
                                    {id: 'SH', country: 'St. Helena'
        },
                                    {id: 'SI', country: 'Slovenia'
        },
                                    {id: 'SJ', country: 'Svalbard & Jan Mayen Islands'
        },
                                    {id: 'SK', country: 'Slovak Republic'
        },
                                    {
            id: 'SL', country: 'Sierra Leone'
        },
                                {
            id: 'SM', country: 'San Marino'
        },
        {
            id: 'SN', country: 'Senegal'
        },
        {
            id: 'SO', country: 'Somalia'
        },
                                    {id: 'SR', country: 'Suriname'
        },
                                        {id: 'ST', country: 'Sao Tome and Principe'
        },
                                    {id: 'SU', country: 'USSR (former)'
        },
                                        {id: 'SV', country: 'El Salvador'
        },
                                        {id: 'SY', country: 'Syria'
        },
                                        {id: 'SZ', country: 'Swaziland'
        },
                                            {id: 'TC', country: 'Turks and Caicos Islands'
        },
            {id: 'TD', country: 'Chad'
        },
                                            {id: 'TF', country: 'French Southern Territories'
        },
            {id: 'TG', country: 'Togo'
        },
                                    {id: 'TH', country: 'Thailand'
        },
                                            {id: 'TJ', country: 'Tajikistan'
        },
                                            {id: 'TK', country: 'Tokelau'
        },
                                                { id: 'TM', country: 'Turkmenistan'
        },
                                                {id: 'TN', country: 'Tunisia'},
                                                {id: 'TO', country: 'Tonga'
        },
                                                    { id: 'TP', country: 'East Timor'
        },
        {
            id: 'TR', country: 'Turkey'
        },
        {
            id: 'TT', country: 'Trinidad and Tobago'
        },
        {
            id: 'TV', country: 'Tuvalu'
        },
                                            {
            id: 'TW', country: 'Taiwan'
        },
                                            {id: 'TZ', country: 'Tanzania'
        },
                                            {id: 'UA', country: 'Ukraine'
        },
        {
            id: 'UG', country: 'Uganda'},
        {
            id: 'UK', country: 'United Kingdom'
            },
        {
            id: 'UM', country: 'US Minor Outlying Islands'
            },
                                                    {id: 'US', country: 'United States'
        },
                                                    {id: 'UY', country: 'Uruguay'
        },
                                                {id: 'UZ', country: 'Uzbekistan'
        },
        {
            id: 'VA', country: 'Vatican City State'
        },
        {
            id: 'VC', country: 'Saint Vincent & the Grenadines'
        },
                                            {id: 'VE', country: 'Venezuela'},
        {
            id: 'VG', country: 'British Virgin Islands'
            },
        {
            id: 'VI', country: 'Virgin Islands (U.S.)'
        },
                { id: 'VN', country: 'Viet Nam'
        },
        {
            id: 'VU', country: 'Vanuatu'
            },
            {id: 'WF', country: 'Wallis and Futuna Islands'},
            {id: 'WS', country: 'Samoa'},
            {id: 'YE', country: 'Yemen'},
            {id: 'YT', country: 'Mayotte'},
            {id: 'YU', country: 'Serbia and Montenegro'},
            {id: 'ZA', country: 'South Africa'},
            {id: 'ZM', country: 'Zambia'},
            {id: 'ZR', country: 'CD Congo, Democratic Republic'},
            {id: 'ZW', country: 'Zimbabwe'},


];

            $scope.stateOptions = [
                { name: '', state: '' },
                { name: 'Alabama', state: 'AL' },
                { name: 'Alaska', state: 'AK' },
                { name: 'Arizona', state: 'AZ' },
                { name: 'Arkansas', state: 'AR' },
                { name: 'California', state: 'CA' },
                { name: 'Colorado', state: 'CO' },
                { name: 'Connecticut', state: 'CT' },
                { name: 'Delaware', state: 'DE' },
                { name: 'District of Columbia', state: 'DC' },
                { name: 'Florida', state: 'FL' },
                { name: 'Georgia', state: 'GA' },
                { name: 'Hawaii', state: 'HI' },
                { name: 'Idaho', state: 'ID' },
                { name: 'Illinois', state: 'IL' },
                { name: 'Indiana', state: 'IN' },
                { name: 'Iowa', state: 'IA' },
                { name: 'Kansas', state: 'KS' },
                { name: 'Kentucky', state: 'KY' },
                { name: 'Louisiana', state: 'LA' },
                { name: 'Maine', state: 'ME' },
                { name: 'Maryland', state: 'MD' },
                { name: 'Massachusetts', state: 'MA' },
                { name: 'Michigan', state: 'MI' },
                { name: 'Minnesota', state: 'MN' },
                { name: 'Mississippi', state: 'MS' },
                { name: 'Missouri', state: 'MO' },
                { name: 'Montana', state: 'MT' },
                { name: 'Nebraska', state: 'NE' },
                { name: 'Nevada', state: 'NV' },
                { name: 'New Hampshire', state: 'NH' },
                { name: 'New Jersey', state: 'NJ' },
                { name: 'New Mexico', state: 'NM' },
                { name: 'New York', state: 'NY' },
                { name: 'North Carolina', state: 'NC' },
                { name: 'North Dakota', state: 'ND' },
                { name: 'Ohio', state: 'OH' },
                { name: 'Oklahoma', state: 'OK' },
                { name: 'Oregon', state: 'OR' },
                { name: 'Pennsylvania', state: 'PA' },
                { name: 'Rhode Island', state: 'RI' },
                { name: 'South Carolina', state: 'SC' },
                { name: 'South Dakota', state: 'SD' },
                { name: 'Tennessee', state: 'TN' },
                { name: 'Texas', state: 'TX' },
                { name: 'Utah', state: 'UT' },
                { name: 'Vermont', state: 'VT' },
                { name: 'Virginia', state: 'VA' },
                { name: 'Washington', state: 'WA' },
                { name: 'West Virginia', state: 'WV' },
                { name: 'Wisconsin', state: 'WI' },
                { name: 'Wyoming', state: 'WY' }
            ];

$scope.openAddressSelection = function (addressPickList) {
$scope.addressPickList = addressPickList;
var modalInstance = $modal
                    .open({
                            templateUrl: 'html/customer/addressSelection.html',
                            controller: 'addressSelectionCtrl',
                        resolve: {
                            addressPickList: function () {
                    return $scope.addressPickList;
                    }
                    }
                    });
    modalInstance.result.then(function (selectedAddress) {
        $scope.address.PersonInfo._AddressLine1 = selectedAddress.Line1;
        $scope.address.PersonInfo._AddressLine2 = selectedAddress.Line2;
        $scope.address.PersonInfo._AddressLine3 = selectedAddress.Line3;
        $scope.address.PersonInfo._City = selectedAddress.City;
        $scope.address.PersonInfo._State = selectedAddress.State;
        $scope.address.PersonInfo._ZipCode = selectedAddress.Zip;
        //AVS is only US
        $scope.address.PersonInfo._Country = "US";
        $scope.addressValidated = true;
        if (selectedAddress.Line2 === null) {
            $scope.address.PersonInfo._AddressLine2 = "";
        }
        if (selectedAddress.Line3 === null) {
            $scope.address.PersonInfo._AddressLine3 = "";
        }
        if ($scope.addressValidated == true && $scope.emailValidated == true && $scope.AllEntriesValid == true) {
            $scope.updateCustomer();
        }
        
      

    });
    };





    }])


    .controller('addressSelectionCtrl', ['$scope', 'customer', '$state', 'addressPickList', '$modalInstance', function ($scope, $customer, $state, addressPickList, $modalInstance) {
                    $scope.selectedRow = 0;
                    $scope.addressPickList = addressPickList.QASearchResult.QAPicklist;
                    selectedAddress = [];
                    $scope.selectableAddress = [];
                    $scope.suggestedAddress = [];
                    $scope.disableUpdateButton = false;

                    if ($scope.addressPickList.PicklistEntry.length == undefined) {
                        if ($scope.addressPickList.PicklistEntry._FullAddress == 'true') {
                            selectlength = $scope.selectableAddress.length;
                            $scope.selectableAddress[selectlength] = angular.copy($scope.addressPickList.PicklistEntry);
                        } else {
                            suggestlength = $scope.suggestedAddress.length;
                            $scope.suggestedAddress[suggestlength] = angular.copy($scope.addressPickList.PicklistEntry);
                        }
                    } else {

                        for (var i = 0, len = $scope.addressPickList.PicklistEntry.length; i < len; i++) {
                            if ($scope.addressPickList.PicklistEntry[i]._FullAddress == 'true') {
                                selectlength = $scope.selectableAddress.length;
                                $scope.selectableAddress[selectlength] = angular.copy($scope.addressPickList.PicklistEntry[i]);
                            } else {
                                suggestlength = $scope.suggestedAddress.length;
                                $scope.suggestedAddress[suggestlength] = angular.copy($scope.addressPickList.PicklistEntry[i]);
                            }
                        }
                    }
                        if ($scope.selectableAddress.length == 0) {
                            $scope.selectAddressLabel = false;
                            $scope.disableUpdateButton = true;
                        } else {
                            $scope.selectAddressLabel = true;
                            $scope.disableUpdateButton = false;
                        }
                        if ($scope.suggestedAddress.length == 0) {
                            $scope.suggestAddressLabel = false;
                        } else {
                            $scope.suggestAddressLabel = true;

                        }
                    

		        $scope.close = function () {
				        $modalInstance.dismiss('closed');
		        }
		        

                $scope.setClickedRow = function (index) {
                        $scope.selectedRow = index;
                }

                
                $scope.refreshIScroll = function () {
                    var myScroll = new IScroll('#ItemDetailWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
                    setTimeout(function () {
                        myScroll.refresh();
                    }, 500);
                };

                $scope.formatSelectedAddress = function () {
                    $scope.moniker = $scope.selectableAddress[$scope.selectedRow].Moniker;

                $customer.verifySelectedAddress($scope.moniker).success(function (response) {
                    if (response.Address.QAAddress.AddressLine.length === 7) {
                        //formataddress
                    selectedAddress.Line1 = response.Address.QAAddress.AddressLine[0].Line;
                    selectedAddress.Line2 = response.Address.QAAddress.AddressLine[1].Line;
                    selectedAddress.Line3 = response.Address.QAAddress.AddressLine[2].Line;
                    selectedAddress.City = response.Address.QAAddress.AddressLine[3].Line;
                    selectedAddress.State = response.Address.QAAddress.AddressLine[4].Line;
                    selectedAddress.Zip = response.Address.QAAddress.AddressLine[5].Line.substring(0, 5);
                    selectedAddress.Country = response.Address.QAAddress.AddressLine[6].Line;
                    if (response.Address.QAAddress.AddressLine[6].Line = "UNITED STATES OF AMERICA") {
                        selectedAddress.Country = "US";
                    }
                    if (selectedAddress.Line2 === null) {
                        addressToVerify.line2 = "";
                }
                    if (selectedAddress.Line3 === null) {
                        addressToVerify.line3 = "";
                    }
                    $modalInstance.close(selectedAddress);
                } else { }
            }).error(function (data) {
                swal({
                    title: "Error!", text: data, showConfirmButton: true
                });
                    });
                
                
                }


}])



