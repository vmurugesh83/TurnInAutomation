describe('ShipToMod Controller', function () {
    beforeEach(module('shippingSelection', 'ui.bootstrap', 'ui.bootstrap.modal', 'appServiceOrderCart', 'appServicesGiftRegistry', 'appUtilities', 'appServicesWebSocket', 'appServicesItem', 'appServiceReprice', 'appServicesCustomer', 'appServiceAppState'));

    var $controller, controller, $httpBackend;
    var orderCart, modalInstance;

    beforeEach(inject(function (_$controller_, $rootScope, $injector, _orderCart_, _$state_) {
        $controller = _$controller_;
        $scope = $rootScope.$new();
        $httpBackend = $injector.get('$httpBackend');
        $state = _$state_;

        orderCart = _orderCart_;

        modalInstance = {
            close: jasmine.createSpy('modalInstance.close'),
            dismiss: jasmine.createSpy('modalInstance.dismiss'),
            result: {
                then: jasmine.createSpy('modalInstance.result.then')
            }
        };

        var orderlines = [
            {
                _PrimeLineNo: '123',
                _SubLineNo: '456',
                Extn: {
                    _ExtnGiftRegistryNo: '111111'
                },
                btDisplay: {}
            }
        ];

        var availableShippingAddresses = {
            123456789123456789: {
                PersonInfo: {
                    _AddressID: "111111",
                    _AddressLine1: "123 Main Street",
                    _AddressLine2: "",
                    _AddressLine3: "",
                    _AddressLine4: "",
                    _AddressLine5: "",
                    _AddressLine6: "",
                    _AlternateEmailID: "",
                    _Beeper: "",
                    _City: "Milwaukee",
                    _Company: "",
                    _Country: "US",
                    _DayFaxNo: "",
                    _DayPhone: "1234567890",
                    _Department: "",
                    _EMailID: "person@gmail.com",
                    _ErrorTxt: "",
                    _EveningFaxNo: "",
                    _EveningPhone: "",
                    _FirstName: "Customer",
                    _HttpUrl: "",
                    _JobTitle: "",
                    _LastName: "Person",
                    _MiddleName: "",
                    _MobilePhone: "",
                    _OtherPhone: "",
                    _PersonID: "",
                    _PersonInfoKey: "123456749856",
                    _PreferredShipAddress: "",
                    _State: "WI",
                    _Suffix: "",
                    _Title: "",
                    _UseCount: "0",
                    _VerificationStatus: "",
                    _ZipCode: "12345"
                },
                _AddressType: "SB",
                _CustomerAdditionalAddressID: "654321",
                _IsBillTo: "Y",
                _IsDefaultBillTo: "N",
                _IsDefaultShipTo: "N",
                _IsShipTo: "Y",
                _IsSoldTo: "Y",
                btIsValid: true,
                btLogic: {
                    isRegistryAddress: false
                }
            },
            987654321987654321: {
                PersonInfo: {
                    _AddressID: "654321",
                    _AddressLine1: "456 Main Street",
                    _AddressLine2: "",
                    _AddressLine3: "",
                    _AddressLine4: "",
                    _AddressLine5: "",
                    _AddressLine6: "",
                    _AlternateEmailID: "",
                    _Beeper: "",
                    _City: "Milwaukee",
                    _Company: "",
                    _Country: "US",
                    _DayFaxNo: "",
                    _DayPhone: "1234567890",
                    _Department: "",
                    _EMailID: "user@gmail.com.com",
                    _ErrorTxt: "",
                    _EveningFaxNo: "",
                    _EveningPhone: "",
                    _FirstName: "Customer",
                    _HttpUrl: "",
                    _JobTitle: "",
                    _LastName: "Person",
                    _MiddleName: "",
                    _MobilePhone: "",
                    _OtherPhone: "",
                    _PersonID: "",
                    _PersonInfoKey: "987654321987654321",
                    _PreferredShipAddress: "",
                    _State: "WI",
                    _Suffix: "",
                    _Title: "",
                    _UseCount: "0",
                    _VerificationStatus: "",
                    _ZipCode: "12345"
                },
                _AddressType: "SB",
                _CustomerAdditionalAddressID: "987654",
                _IsBillTo: "Y",
                _IsDefaultBillTo: "Y",
                _IsDefaultShipTo: "Y",
                _IsShipTo: "Y",
                _IsSoldTo: "Y",
                btIsValid: true,
                btLogic: {
                    isRegistryAddress: false
                }
            }
        }

        spyOn(orderCart.giftOptions, 'getRegistryAddressSet').and.returnValue('123 Maple Drive');
        spyOn($state, 'go');

        controller = $controller('shipToModCtrl', { $scope: $scope, $modalInstance: modalInstance, orderlines: orderlines, availableShippingAddresses: availableShippingAddresses, customerContactID: '1234567' });
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(controller).toBeDefined();

        expect($scope.addressKeyToRegistryMap).toEqual({});
        expect($scope.isBigTicket).toBe(false);
        expect($scope.availableShippingAddresses).toEqual({
            '123456749856': {
                PersonInfo: {
                    _AddressID: '111111',
                    _AddressLine1: '123 Main Street',
                    _AddressLine2: '',
                    _AddressLine3: '',
                    _AddressLine4: '',
                    _AddressLine5: '',
                    _AddressLine6: '',
                    _AlternateEmailID: '',
                    _Beeper: '',
                    _City: 'Milwaukee',
                    _Company: '',
                    _Country: 'US',
                    _DayFaxNo: '',
                    _DayPhone: '1234567890',
                    _Department: '',
                    _EMailID: 'person@gmail.com',
                    _ErrorTxt: '',
                    _EveningFaxNo: '',
                    _EveningPhone: '',
                    _FirstName: 'Customer',
                    _HttpUrl: '',
                    _JobTitle: '',
                    _LastName: 'Person',
                    _MiddleName: '',
                    _MobilePhone: '',
                    _OtherPhone: '',
                    _PersonID: '',
                    _PersonInfoKey: '123456749856',
                    _PreferredShipAddress: '',
                    _State: 'WI',
                    _Suffix: '',
                    _Title: '',
                    _UseCount: '0',
                    _VerificationStatus: '',
                    _ZipCode: '12345'
                },
                _AddressType: 'SB',
                _CustomerAdditionalAddressID: '654321',
                _IsBillTo: 'Y',
                _IsDefaultBillTo: 'N',
                _IsDefaultShipTo: 'N',
                _IsShipTo: 'Y',
                _IsSoldTo: 'Y',
                btIsValid: true,
                btLogic: {
                    isRegistryAddress: false
                }
            },
            '987654321987654321': {
                PersonInfo: {
                    _AddressID: '654321',
                    _AddressLine1: '456 Main Street',
                    _AddressLine2: '',
                    _AddressLine3: '',
                    _AddressLine4: '',
                    _AddressLine5: '',
                    _AddressLine6: '',
                    _AlternateEmailID: '',
                    _Beeper: '',
                    _City: 'Milwaukee',
                    _Company: '',
                    _Country: 'US',
                    _DayFaxNo: '',
                    _DayPhone: '1234567890',
                    _Department: '',
                    _EMailID: 'user@gmail.com.com',
                    _ErrorTxt: '',
                    _EveningFaxNo: '',
                    _EveningPhone: '',
                    _FirstName: 'Customer',
                    _HttpUrl: '',
                    _JobTitle: '',
                    _LastName: 'Person',
                    _MiddleName: '',
                    _MobilePhone: '',
                    _OtherPhone: '',
                    _PersonID: '',
                    _PersonInfoKey: '987654321987654321',
                    _PreferredShipAddress: '',
                    _State: 'WI',
                    _Suffix: '',
                    _Title: '',
                    _UseCount: '0',
                    _VerificationStatus: '',
                    _ZipCode: '12345'
                },
                _AddressType: 'SB',
                _CustomerAdditionalAddressID: '987654',
                _IsBillTo: 'Y',
                _IsDefaultBillTo: 'Y',
                _IsDefaultShipTo: 'Y',
                _IsShipTo: 'Y',
                _IsSoldTo: 'Y',
                btIsValid: true,
                btLogic: {
                    isRegistryAddress: false
                }
            }
        });
    });

    it('should save the non-registry address', function () {
        var addressObj = {
            btLogic: {
                isRegistryAddress: false
            },
            btIsValid: true
        };

        spyOn(orderCart.address, 'setOrderLineShipToAddresses');

        $scope.saveAddress(addressObj);

        expect(orderCart.address.setOrderLineShipToAddresses).toHaveBeenCalled();
        expect(modalInstance.close).toHaveBeenCalled();
    });

    it('should save the registry address', function () {
        var addressObj = {
            btLogic: {
                isRegistryAddress: true
            },
            btIsValid: true,
            PersonInfo: {
                _PersonInfoKey: '111111'
            }
        };

        spyOn(orderCart.address, 'setOrderLineShipToAddresses');

        $scope.saveAddress(addressObj);

        expect(orderCart.address.setOrderLineShipToAddresses).toHaveBeenCalled();
        expect(modalInstance.close).toHaveBeenCalled();
    });

    it('should dismiss the modal', function () {
        $scope.cancel();
        expect(modalInstance.dismiss).toHaveBeenCalled();
    });

    it('should go to the addModifyAddress state', function () {
        $scope.openEditAddress();

        expect($state.go).toHaveBeenCalledWith('addModifyAddress', {
            customerContactID: '1234567',
            customerAdditionalAddressID: ''
        });
        expect(modalInstance.dismiss).toHaveBeenCalled();
    });
});