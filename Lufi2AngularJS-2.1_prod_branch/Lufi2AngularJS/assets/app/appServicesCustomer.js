var appServices = angular.module('appServicesCustomer', []);
appServices.factory('customer', ['$http', '$cacheFactory', 'serviceArrayFix', '$rootScope', 'isPoBoxAddress', 'loggerService', 'sendSMTPErrorEmail',
    function ($http, $cacheFactory, serviceArrayFix, $rootScope, $isPoBoxAddress, $loggerService, $sendSMTPErrorEmail) {
    var cache = $cacheFactory('customer');
    var _definedSuccessError = function (functionDef) {
        if (!angular.isFunction(functionDef)) {
            functionDef = function () { };
        }
        return functionDef;
    };

    var customerSearch = function (queryParam, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);
        var url = serviceURL + "/customerSearchJSON";
        $http.post(url, angular.toJson(queryParam)).success(function (data) {
            var response = angular.fromJson(data);
            //var customerArray=normalizeCustomers(response);
            var customerArray = [];
            if (!angular.isArray(response.Customer.Addresses)) {
                response.Customer.Addresses = [response.Customer.Addresses.Address];
            }

            response.Customer.Addresses.forEach(function (address) {
                if (address.IsDefaultBillTo == 'Y') {
                    customerArray.push(address);
                }
            });
            response.Customer.Addresses.forEach(function (address) {
                if (address.IsDefaultShipTo == 'Y' && customerArray.indexOf(address) == -1) {
                    customerArray.push(address);
                }
            });

            response.Customer.Addresses.forEach(function (address) {
                if (address.CustomerKey.trim().length > 0 && customerArray.indexOf(address) == -1) {
                    customerArray.push(address);
                }
            });

            response.Customer.Addresses.forEach(function (address) {
                if (customerArray.indexOf(address) == -1) {
                    customerArray.push(address);
                }
            });


            cache.put('cachedResult', customerArray);
            cache.put('queryParam', queryParam);

            success(customerArray);
        }).error(function (data) {
            error(data);
        });
    };

    var _clearResultsOnly = function () {
        cache.remove('cachedResult');
    };

    var clearCachedResult = function () {
        _clearResultsOnly();
        cache.remove('queryParam');
    }

    var clearSelectedCustomer = function () {
        var previousCustomer = getSelectedCustomer();
        clearCachedResult();
        cache.remove('selectedCustomer');
        $rootScope.$broadcast('selectedCustomerCleared', previousCustomer);

    };

    var setSelectedCustomer = function (customer) {
        $loggerService.log(customer);
        cache.put('selectedCustomer', customer);
        $rootScope.$broadcast('customerSelected', customer);
    }

    var normalizeCustomers = function (response) {
        var customers = {};
        if (!angular.isArray(response.Customer.Addresses)) {
            response.Customer.Addresses = [response.Customer.Addresses.Address];
        }
        response.Customer.Addresses.forEach(function (address) {
            if (address.CustomerId.trim().length > 0) {
                if (!customers[address.CustomerId]) {
                    customers[address.CustomerId] = {
                        addresses: []
                    };
                }
                var customer = customers[address.CustomerId];
                if (address.IsDefaultShipTo == 'Y') {
                    customer.defaultShipToAddress = address;
                }
                if (address.IsDefaultBillTo == 'Y') {
                    customer.defaultBillToAddress = address;
                }
                customer.addresses.push(address);
            }
        });

        var customerArray = [];

        for (key in customers) {
            var customer = customers[key];
            if (customer.defaultBillToAddress) {
                customerArray.push(customer);
            }
        }
        var customerArray = [];
        for (key in customers) {
            var customer = customers[key];
            if (customer.defaultBillToAddress) {
                customerArray.push(customer);
            }
        }

        return customerArray;
    }


    var getSelectedCustomer = function () {
        return cache.get('selectedCustomer');
    }

    var retrieveOrders = function (orderType, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var selectedCustomer = cache.get('selectedCustomer');
        if (selectedCustomer) {
            if (selectedCustomer[orderType]) {
                success(selectedCustomer[orderType]);
            } else {
                var contract = {
                    "GetSterlingOrderListReq":
                    {
                        "_ReadFromHistory": "N",
                        "_DocumentType": "",
                        "_DraftOrderFlag": "",
                        "_BillToID": selectedCustomer._CustomerID,
                        "_BillToIDQryType": "EQ"
                    }
                };
                var url = serviceURL + "/Order/GetOrderListJSON";

                switch (orderType) {
                    case 'draftOrders':
                        contract.GetSterlingOrderListReq._DocumentType = '0001';
                        contract.GetSterlingOrderListReq._DraftOrderFlag = 'Y';
                        break;
                    case 'confirmOrders':
                        contract.GetSterlingOrderListReq._DocumentType = '0001';
                        contract.GetSterlingOrderListReq._DraftOrderFlag = 'N';
                        break;
                    case 'returnOrders':
                        contract.GetSterlingOrderListReq._DocumentType = '0003';
                        contract.GetSterlingOrderListReq._DraftOrderFlag = '';
                        break;
                }
                $http.post(url, contract).success(function (data) {
                    serviceArrayFix(data);
                    var ordersArr = data.GetSterlingOrderListResp.Order ? data.GetSterlingOrderListResp.Order : [];
                    selectedCustomer[orderType] = ordersArr;
                    success(ordersArr);
                }).error(function (data) {
                    $sendSMTPErrorEmail(data, url,contract);
                    serviceArrayFix(data);
                    error(data);
                })
            }
        } else {
            error("No Customer Selected. Use Customer Search first.");
        }
    };

    $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
        var stateName = toState.name;
        if (stateName === 'home') {
            var selectedCustomer = cache.get('selectedCustomer');
            //delete cached draft orders, confirmed orders, and return orders
            if (selectedCustomer) {
                selectedCustomer.draftOrders = false;
                selectedCustomer.confirmOrders = false;
                selectedCustomer.returnOrders = false;
            }
        }

    });

    var retrieveDraftOrders = function (success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        retrieveOrders('draftOrders', success, error);
    }

    var retrieveConfirmedOrders = function (success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        retrieveOrders('confirmOrders', success, error);
    }

    var retrieveReturnOrders = function (success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        retrieveOrders('returnOrders', success, error);
    }

    var getCachedResult = function () {
        return cache.get('cachedResult');
    };

    var getCachedQueryParam = function () {
        return cache.get('queryParam');
    };
    var manageCustomerUrl = serviceURL + "/Customer/manage";

    var createCustomer = function (customer) {

        var url = manageCustomerUrl;
        //var url = "http://10.131.135.91:7080/Customer/manage";

        //clean zipcode so zip with 60605-1234 are changed to 60605
        if (isFinite(parseInt(customer.Zipcode))) {
            customer.Zipcode = parseInt(customer.Zipcode);
        }

        var contract = {
            manageCustomerReq: {
                input: {
                    CustomerContactList: {
                        CustomerContact: [{
                            CustomerAdditionalAddressList: {
                                CustomerAdditionalAddress: [{
                                    PersonInfo: {
                                        _AddressLine1: customer.AddressLine1,
                                        _AddressLine2: customer.AddressLine2,
                                        _AddressLine3: customer.AddressLine3,
                                        _City: customer.City,
                                        _Country: customer.Country,
                                        _IsCommercialAddress: "N",
                                        _Lockid: "0",
                                        _State: customer.State,
                                        _UseCount: "0",
                                        _ZipCode: customer.Zipcode,
                                        _isHistory: "N",
                                        _EmailID: customer.EmailAddress,
                                        _PersonInfoKey: customer.PersonInfoKey
                                    },
                                    _IsBillTo: "Y",
                                    _IsDefaultBillTo: "Y",
                                    _IsDefaultShipTo: "Y",
                                    _IsShipTo: "Y"
                                }],
                                _Reset: "Y"
                            },
                            _DateOfBirth: "",
                            _DayFaxNo: "",
                            _DayPhone: customer.DayPhone,
                            _Department: "Legends",
                            _EmailID: customer.EmailAddress,
                            _EveningFaxNo: "",
                            _EveningPhone: "",
                            _FirstName: customer.FirstName,
                            _JobTitle: "",
                            _LastName: customer.LastName,
                            _MiddleName: customer.MiddleName,
                            _MobilePhone: "",
                            _SpouseDateOfBirth: "",
                            _Title: "",
                            _UserID: "",
                            _WeddingAnniversaryDate: ""
                        }]
                    },
                    _CustomerType: "02",
                    _OrganizationCode: "BONTON",
                    _Status: "10"
                }
            }
        }
        return $http.post(url, angular.toJson(contract));
    }

    var retrieveCustomerDetail = function (customerId, customerKey, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var url = serviceURL + "/Customer/getDetails";
        //var url = "http://10.131.135.91:7080/Customer/getDetails";
        var contract = {
            getCustomerDetailsReq: {
                input: {
                    _CustomerID: customerId,
                    _CustomerKey: customerKey,
                    _InheritFromParents: "?",
                    _OrganizationCode: "BONTON",
                    CustomerContact: {
                        _CustomerContactID: "",
                        _UserID: ""
                    }
                }
            }
        };

        $http.post(url, angular.toJson(contract)).success(function (data) {
            var customer = angular.fromJson(data).getCustomerDetailsResponse.Customer;
            serviceArrayFix(customer);
            setSelectedCustomer(customer);
            setDefaultBillToShipTo(customer);
            success(customer);
        }).error(function (data) {
            $sendSMTPErrorEmail(data, url,contract);
            error(data);
        });
    }

    /*
       This method will create customer if the customer key is missing then rely the result.
       Otherwise just calls retrieveCustomer and returns the result.
    */
    var customerAction = function (customer, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        if (customer.CustomerId.trim().length == 0 || customer.CustomerKey.trim().length == 0) {
            //create customer
            createCustomer(customer).success(function (data) {
                var response = angular.fromJson(data);
                var customerKey = response.manageCustomerResp.return._CustomerKey;
                var customerId = response.manageCustomerResp.return._CustomerID;
                customer.CustomerId = customerId;
                customer.CustomerKey = customerKey;
                _clearResultsOnly();
                retrieveCustomerDetail(customerId, customerKey, success, error);
            }).error(function (data) {
                $sendSMTPErrorEmail(data, manageCustomerUrl,customer);
                error(data);
            });
        } else {
            retrieveCustomerDetail(customer.CustomerId, customer.CustomerKey, success, error);
        }
    };

    var addToCart = function (customer) {
        if (customer) {
            setSelectedCustomer(customer);
        } else {
            customer = getSelectedCustomer();
        }
        $rootScope.$broadcast('AddCustomerToCartCalled', customer);
    }


    var setDefaultBillToShipTo = function (customer) {
        var defaultAddresses = {};
        defaultAddresses['PersonInfoBillTo'] = null;
        defaultAddresses['PersonInfoShipTo'] = null;
        var isDefaultsFound = false;

        var contacts = customer.CustomerContactList.CustomerContact;
        for (var i = 0; (i < contacts.length) && !isDefaultsFound; i++) {
            var contact = contacts[i];

            if (angular.isObject(contact) && angular.isObject(contact.CustomerAdditionalAddressList) && angular.isArray(contact.CustomerAdditionalAddressList.CustomerAdditionalAddress)) {
            var addresses = contact.CustomerAdditionalAddressList.CustomerAdditionalAddress;
                for (var j = 0; (j < addresses.length) && !isDefaultsFound; j++) {
                var address = addresses[j];
                if (address._IsDefaultBillTo == "Y") {
                    defaultAddresses['PersonInfoBillTo'] = address.PersonInfo;
                }
                if (address._IsDefaultShipTo == "Y") {
                    defaultAddresses['PersonInfoShipTo'] = address.PersonInfo;
                }

                    if (defaultAddresses['PersonInfoBillTo'] !== null && defaultAddresses['PersonInfoShipTo'] !== null) {
                        isDefaultsFound = true;
                    }

                }
            }
        }

        //if no defaults take first address
        if (!isDefaultsFound) {
            var contact = contacts[0];
            if (angular.isObject(contact) && angular.isObject(contact.CustomerAdditionalAddressList) && angular.isArray(contact.CustomerAdditionalAddressList.CustomerAdditionalAddress)
                && contact.CustomerAdditionalAddressList.CustomerAdditionalAddress.length > 0) {

                var address = contact.CustomerAdditionalAddressList.CustomerAdditionalAddress[0];

                if (defaultAddresses['PersonInfoBillTo'] === null) {
                    defaultAddresses['PersonInfoBillTo'] = address.PersonInfo;
                }
                if (defaultAddresses['PersonInfoShipTo'] === null) {
                    defaultAddresses['PersonInfoShipTo'] = address.PersonInfo;
                }
            }
        }

        customer.defaultAddresses = defaultAddresses;
    }

    var isCustomerSelected = function () {
        return getSelectedCustomer() != undefined;
    }

    var _setDefaults = function (customer, contactID, modifiedAddress) {

        var isNewDefaultBillTo = modifiedAddress._IsDefaultBillTo == 'Y';
        var isNewDefaultShipTo = modifiedAddress._IsDefaultShipTo == 'Y';
        var selectedContact = null;

        //find contact
        for (var i = 0; i < customer.CustomerContactList.CustomerContact.length; i++) {
            if (customer.CustomerContactList.CustomerContact[i]._CustomerContactID == contactID) {
                selectedContact = customer.CustomerContactList.CustomerContact[i];
                break;
            }
        }

        if (selectedContact) {
            for (var i = 0; i < selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress.length; i++) {

                if (selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._CustomerAdditionalAddressID == modifiedAddress._CustomerAdditionalAddressID) {
                    selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._IsDefaultBillTo = isNewDefaultBillTo ? 'Y' : 'N';
                    selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._IsDefaultShipTo = isNewDefaultShipTo ? 'Y' : 'N';
                } else {
                    if (isNewDefaultBillTo) {
                        selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._IsDefaultBillTo = 'N';
                    }
                    if (isNewDefaultShipTo) {
                        selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._IsDefaultShipTo = 'N';
                    }
                }
            }
        }

        return customer;
    };

    var verifyCustomerAddress = function (address, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var url = serviceURL + "/Utility/AVS/DoSearch";
        //var url = "http://10.131.135.83:7080/Utility/AVS/DoSearch";

        var contract = {
            AVSInput: {
                QASearch: {
                    Country: "USA",
                    Engine: {
                        _Flatten: "true",
                        _Intensity: "Close",
                        _PromptSet: "Default",
                        _Threshold: "100",
                        "#text": "Verification",
                    },
                    "Layout": "Database layout",
                    "Search": address,
                    "FormattedAddressInPicklist": "false",
                    "_Localisation": "USA"

                }
            }
        }
        return $http.post(url, angular.toJson(contract), {timeout: 12000}).success(function (data) {
        }).error(function (data) {
            $sendSMTPErrorEmail(data, url);
            error(data.err);
        });
    };


    var verifySelectedAddress = function (Moniker, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var url = serviceURL + "/Utility/AVS/DoGetAddress";
        var contract = {
            AVSInput: {
                QAGetAddress: {
                    "_Localisation": "USA",
                    "Layout": {
                        "#text": "Database layout"
                    },
                    "Moniker": Moniker,

                }
            }
        }
        return $http.post(url, angular.toJson(contract)).success(function (data) {

        }).error(function (data) {
            $sendSMTPErrorEmail(data, url,contract);
            error(data.err);
        });
    };

    var verifyEmail = function (email, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var url = serviceURL + "/Validate/Email";
        var contract = {
            Email: email,
            Timeout: "3"
        }

        return $http.post(url, angular.toJson(contract), {timeout: 12000}).success(function (data) {

        }).error(function (data) {
            $sendSMTPErrorEmail(data, url);
            error(data.err);
        });
    };

    var _cleanAddress = function (modifiedAddress) {
        //sanity check, clean out null input or undefined input.
        for (key in modifiedAddress) {
            if (!angular.isDefined(modifiedAddress[key]) || modifiedAddress[key] === null) {
                try { delete modifiedAddress[key]; } catch (e) { }
            }
        }
        for (key in modifiedAddress.PersonInfo) {
            if (!angular.isDefined(modifiedAddress.PersonInfo[key]) || modifiedAddress.PersonInfo[key] === null) {
                try { delete modifiedAddress.PersonInfo[key]; } catch (e) { }
            }
        }
    };
    var createCustomerByAddress = function (modifiedAddress, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        _cleanAddress(modifiedAddress);

        modifiedAddress._AddressType = "SB";
        modifiedAddress._CustomerAdditionalAddressID = "";
        modifiedAddress._IsBillTo = "Y";
        modifiedAddress._IsDefaultBillTo = "Y";
        modifiedAddress._IsDefaultShipTo = "Y";
        modifiedAddress._IsShipTo = "Y";
        modifiedAddress._Operation = "";

        var input = {
            "manageCustomerReq": {
                "input": {
                    "_CustomerID": "",
                    "_CustomerKey": "",
                    "_CustomerType": "02",
                    "_OrganizationCode": "BONTON",
                    "_Status": "10",
                    "CustomerContactList": {
                        "CustomerContact": [{
                            "_CustomerContactID": "",
                            "CustomerAdditionalAddressList": { "CustomerAdditionalAddress": [modifiedAddress] },
                            "_DateOfBirth": "",
                            "_DayFaxNo": "",
                            "_DayPhone": modifiedAddress.PersonInfo._DayPhone,
                            "_Department": "",
                            "_EmailID": modifiedAddress.PersonInfo._EMailID,
                            "_EveningFaxNo": "",
                            "_EveningPhone": modifiedAddress.PersonInfo._EveningPhone,
                            "_FirstName": modifiedAddress.PersonInfo._FirstName,
                            "_JobTitle": "",
                            "_LastName": modifiedAddress.PersonInfo._LastName,
                            "_MiddleName": modifiedAddress.PersonInfo._MiddleName,
                            "_MobilePhone": "",
                            "_SpouseDateOfBirth": "",
                            "_Title": "",
                            "_UserID": "",
                            "_WeddingAnniversaryDate": ""
                        }]
                    }
                }
            }
        };

        $http.post(manageCustomerUrl, input).success(function (data) {
            var result = data.manageCustomerResp.return;

            retrieveCustomerDetail(result._CustomerID, result._CustomerKey, success, error);
        }).error(function (data) {
            $sendSMTPErrorEmail(data, url,input);
            error(data);
        });
    };

    var _validAddress = function (address, isBillingAddress) {

        isBillingAddress = isBillingAddress ? isBillingAddress : false;

        //billing address checks
        var valid = true;

        var rollingErrorTextArray = [];

        if (!angular.isDefined(address) || address === null) {
            return { isValid: false, errorMessage: 'No Address Defined.' };
        }

        //check that we are in the personInfo part of an address
        if(angular.isDefined(address.PersonInfo)){
            address = address.PersonInfo;
        }

        if (!angular.isDefined(address._FirstName) || address._FirstName === null || address._FirstName.toString().trim() === '') {
            valid = false;
            rollingErrorTextArray.push('No First Name');
        }
        if (!angular.isDefined(address._LastName) || address._LastName === null || address._LastName.toString().trim() === '') {
            valid = false;
            rollingErrorTextArray.push('No Last Name');
        }
        if (!angular.isDefined(address._EMailID) || address._EMailID === null || address._EMailID.toString().trim() === '') {
            valid = false;
            rollingErrorTextArray.push('No Email');
        }
        if (!angular.isDefined(address._DayPhone) || address._DayPhone === null || address._DayPhone.toString().trim() === '') {
            valid = false;
            rollingErrorTextArray.push('No Primary Phone');
        }
        if (!angular.isDefined(address._AddressLine1) || address._AddressLine1 === null || address._AddressLine1.toString().trim() === '') {
            valid = false;
            rollingErrorTextArray.push('No Address Line 1');
        }
        if (!angular.isDefined(address._Country) || address._Country === null || address._Country.toString().trim() === '') {
            valid = false;
            rollingErrorTextArray.push('No Country');
        }

        //shipping address only validation OR billing addresses with US country
        if (!isBillingAddress || (angular.isDefined(address._Country) && address._Country !== null && (/^us$/).test(address._Country.toString().trim().toLowerCase()))) {

            if (!isBillingAddress && angular.isDefined(address._Country) && address._Country !== null && !((/^us$/).test(address._Country.toString().trim().toLowerCase()))) {
                valid = false;
                rollingErrorTextArray.push('Ship Country (' + address._Country + ') not US');
            }

            if (!angular.isDefined(address._City) || address._City === null || address._City.toString().trim() === '') {
                valid = false;
                rollingErrorTextArray.push('No City');
            }
            if (!angular.isDefined(address._State) || address._State === null || address._State.toString().trim() === '') {
                valid = false;
                rollingErrorTextArray.push('No State');
            }
            if (angular.isDefined(address._State) && address._State !== null && !((/^[a-zA-Z]{2}$/).test(address._State.toString()))) {
                valid = false;
                rollingErrorTextArray.push('State (' + address._State + ') not state abbreviation');
            }
            if (!isBillingAddress && angular.isDefined(address._State) && address._State !== null && ((/^(aa|ae|ap)$/).test(address._State.toString().toLowerCase()))) {
                valid = false;
                rollingErrorTextArray.push('Cannot Ship by military or diplomatic post: (' + address._State + ') ');
            }

            if (!angular.isDefined(address._ZipCode) || address._ZipCode === null || address._ZipCode.toString().trim() === '') {
                valid = false;
                rollingErrorTextArray.push('No Zip Code');
            }
            if (angular.isDefined(address._ZipCode) && address._ZipCode !== null && !((/^[0-9]{5}$/).test(address._ZipCode.toString().trim()))) {
                valid = false;
                rollingErrorTextArray.push('5 Digit Zip Code Only');
            }

            if (!isBillingAddress && angular.isDefined(address._AddressLine1) && address._AddressLine1 !== null && $isPoBoxAddress(address)) {
                valid = false;
                rollingErrorTextArray.push('Cannot Ship to PO Box');
            }
        }

        return { isValid: valid, errorMessage: rollingErrorTextArray.join(', ') };
    };


    var updateCustomerContact = function (customer, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var copyCustomer = angular.copy(customer);
        //strip out addresses from all customers
        if (copyCustomer.CustomerContactList && angular.isArray(copyCustomer.CustomerContactList.CustomerContact)) {
            for (var i = 0; i < copyCustomer.CustomerContactList.CustomerContact.length; i++) {
                var currentContact = copyCustomer.CustomerContactList.CustomerContact[i];
                if ('CustomerAdditionalAddressList' in currentContact) {
                    //Add reset flag
                    //currentContact.CustomerAdditionalAddressList._Reset = 'Y';


                    delete currentContact.CustomerAdditionalAddressList;
                }

            }

            if ('defaultAddresses' in copyCustomer) {
                //delete this Services added defaultAddresses
                delete copyCustomer.defaultAddresses;
            }
            

            var input = { manageCustomerReq: { input: copyCustomer } };
            $http.post(manageCustomerUrl, input).success(function (data) {
                var result = data.manageCustomerResp.return;
                retrieveCustomerDetail(result._CustomerID, result._CustomerKey, success, error);

            }).error(function (data) {
                $sendSMTPErrorEmail(data, manageCustomerUrl,input);
                error(data);
            });

        }

    };

    var addModifyAddress = function (customerID, customerContactID, modifiedAddress, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var customer = angular.copy(getSelectedCustomer());

        _cleanAddress(modifiedAddress);

        //set _IsShipTo and _IsBillTo
        var resultValidBusiness = _validAddress(modifiedAddress, true);
        var resultValidShipping = _validAddress(modifiedAddress, false);

        if (!resultValidBusiness.isValid && !resultValidShipping.isValid) {
            error('Not a valid billing or shipping address.');
            return;
        }

        if (resultValidShipping.isValid) {
            modifiedAddress._IsShipTo = 'Y';
        } else {
            modifiedAddress._IsShipTo = 'N';
            modifiedAddress._IsDefaultShipTo = 'N'
        }

        if (resultValidBusiness.isValid) {
            modifiedAddress._IsBillTo = 'Y';
        } else {
            modifiedAddress._IsBillTo = 'N';
            modifiedAddress._IsDefaultBillTo = 'N'
        }


        //check that customer is same as selectedCustomer
        if (customerID == customer._CustomerID) {
            var input = { "manageCustomerReq": { "input": {} } };

            //if address is defaultBilling = 'Y' or defaultShipping = 'Y', change all other contact's additionalAddress to 'N'
            if (modifiedAddress._IsDefaultBillTo == 'Y' || modifiedAddress._IsDefaultShipTo == 'Y') {
                customer = _setDefaults(customer, customerContactID, modifiedAddress);
            }

            //find address and replace
            var selectedContact = null;

            //find contact
            for (var i = 0; i < customer.CustomerContactList.CustomerContact.length; i++) {
                if (customer.CustomerContactList.CustomerContact[i]._CustomerContactID == customerContactID) {
                    selectedContact = customer.CustomerContactList.CustomerContact[i];

                }
            }

            if (selectedContact) {

                //if no AddressID we must be adding a new address, else find address to modify
                if (modifiedAddress._CustomerAdditionalAddressID.toString().trim() == "") {
                    selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress.push(modifiedAddress);
                } else {
                    for (var i = 0; i < selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress.length; i++) {

                        if (selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._CustomerAdditionalAddressID == modifiedAddress._CustomerAdditionalAddressID) {
                            selectedContact.CustomerAdditionalAddressList.CustomerAdditionalAddress[i] = modifiedAddress;
                            break;
                        }
                    }
                }

                //Add reset flag
                selectedContact.CustomerAdditionalAddressList._Reset = 'Y';
            }

            //delete this Services added defaultAddresses
            delete customer.defaultAddresses;

            input.manageCustomerReq.input = customer;

            _clearResultsOnly();

            $http.post(manageCustomerUrl, input).success(function (data) {
                var result = data.manageCustomerResp.return;

                retrieveCustomerDetail(result._CustomerID, result._CustomerKey, success, error);
            }).error(function (data) {
                $sendSMTPErrorEmail(data, manageCustomerUrl,input);
                error(data);
            });

        } else {
            retrieveCustomerDetail(customerID, "",
                function (customer) {
                    if (customer._CustomerID == customerID) {
                        addModifyAddress(customerID, customerContactID, modifiedAddress, success, error);
                    } else {//fail
                        throw { name: 'customer.addModifyAddress  service failed.' };
                    }
                },
                function (data) { $loggerService.log('$customer.addModifyAddres called retrieve customer with error.'); $loggerService.log(data); });
        }

    };

    var deleteAddress = function (customerID, customerContactID, customerAdditionalAddressID, success, error) {
        success = _definedSuccessError(success);
        error = _definedSuccessError(error);

        var input = {
            "manageCustomerReq": {
                "input": {
                    "_CustomerID": customerID.toString(),
                    "_CustomerType": "02",
                    "_OrganizationCode": "BONTON",
                    "CustomerContactList": {
                        "CustomerContact": [
                          {
                              "_CustomerContactID": customerContactID.toString(),
                              "CustomerAdditionalAddressList": {
                                  "CustomerAdditionalAddress": [
                                    {
                                        "_CustomerAdditionalAddressID": customerAdditionalAddressID.toString(),
                                        "_Operation": "Delete"
                                    }
                                  ]
                              }
                          }
                        ]
                    },
                    "_Status": "10"
                }
            }
        };

        _clearResultsOnly();

        $http.post(manageCustomerUrl, input).success(function (data) {
            var customer = getSelectedCustomer();
            if ((customer === undefined || customer === null) || (customer && (customerID == customer._CustomerID))) {
                var result = data.manageCustomerResp.return;

                retrieveCustomerDetail(result._CustomerID, result._CustomerKey, success, error);
            }
        }).error(function (data) {
            $sendSMTPErrorEmail(data, manageCustomerUrl,input);
            error(data);
        });
    };


    //watch cart events. 
    $rootScope.$on('orderCartDeleted', function (event, customerData) {
        clearSelectedCustomer();
    });


    return {
        verifyEmail: verifyEmail,
        verifySelectedAddress: verifySelectedAddress,
        customerSearch: customerSearch,
        setSelectedCustomer: setSelectedCustomer,
        isCustomerSelected: isCustomerSelected,
        getSelectedCustomer: getSelectedCustomer,
        retrieveDraftOrders: retrieveDraftOrders,
        retrieveConfirmedOrders: retrieveConfirmedOrders,
        retrieveReturnOrders: retrieveReturnOrders,
        clearCachedResult: clearCachedResult,
        getCachedResult: getCachedResult,
        getCachedQueryParam: getCachedQueryParam,
        customerAction: customerAction,
        addToCart: addToCart,
        addModifyAddress: addModifyAddress,
        verifyCustomerAddress: verifyCustomerAddress,
        deleteAddress: deleteAddress,
        clearSelectedCustomer: clearSelectedCustomer,
        retrieveCustomerDetail: retrieveCustomerDetail,
        createCustomerByAddress: createCustomerByAddress,
        updateCustomerContact: updateCustomerContact,
        validateAddress: _validAddress
    };

}]);

