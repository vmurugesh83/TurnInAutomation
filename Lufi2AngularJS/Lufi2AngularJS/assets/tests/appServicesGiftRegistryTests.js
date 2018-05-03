describe("Gift Registry Service", function () {
    var giftRegistryService, loggerService, $httpBackend;

    beforeEach(angular.mock.module('appUtilities', 'appServicesWebSocket', 'appServicesItem'));
    beforeEach(module('appServicesGiftRegistry'));

    beforeEach(inject(function (_giftRegistryService_, _loggerService_, $injector) {
        giftRegistryService = _giftRegistryService_;
        loggerService = _loggerService_;
        $httpBackend = $injector.get('$httpBackend');
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(giftRegistryService).toBeDefined();
    });

    it('should call the /GiftRegistry/GetPreferredAddress endpoint', function () {
        $httpBackend.when('POST', serviceURL + '/GiftRegistry/GetPreferredAddress').respond('address');
        $httpBackend.expectPOST(serviceURL + '/GiftRegistry/GetPreferredAddress', {
            "GiftRegistryInput": {
                "GetPreferredAddress": {
                    "registryNumber": '1234'
                }
            }
        });

        giftRegistryService.preferredAddressPromise('1234');

        $httpBackend.flush();
    });

    it('should call the /GiftRegistry/IsRegistryValid endpoint', function () {
        $httpBackend.when('POST', serviceURL + '/GiftRegistry/IsRegistryValid').respond('valid');
        $httpBackend.expectPOST(serviceURL + '/GiftRegistry/IsRegistryValid', {
            "IsRegistryValidInput": {
                "IsRegistryValid": {
                    "registryNumber": '1234'
                }
            }
        });

        giftRegistryService.validateRegistryPromise('1234');

        $httpBackend.flush();
    });

    it('should return null if the parameters are null', function () {
        expect(giftRegistryService.constructRegistryAddress(null, null)).toBe(null);
    });

    it('should build the registry address when provided', function () {
        var addressInfo = {
            Address1: '123 Main St',
            Address2: 'Apt 10',
            City: 'Milwaukee',
            Country: 'US',
            Email1: 'a@bonton.com',
            FirstName: 'Bonton',
            LastName: 'Customer',
            Phone: '1234567890',
            State: 'WI',
            ZipCode: '12345'
        };

        var newAddress = giftRegistryService.constructRegistryAddress('1234', addressInfo);

        expect(newAddress).toBeDefined();
        expect(newAddress.PersonInfo._AddressLine1).toBe('123 Main St');
        expect(newAddress.PersonInfo._AddressLine2).toBe('Apt 10');
        expect(newAddress.PersonInfo._City).toBe('Milwaukee');
        expect(newAddress.PersonInfo._Country).toBe('US');
        expect(newAddress.PersonInfo._EMailID).toBe('a@bonton.com');
        expect(newAddress.PersonInfo._FirstName).toBe('Bonton');
        expect(newAddress.PersonInfo._LastName).toBe('Customer');
        expect(newAddress.PersonInfo._DayPhone).toBe('1234567890');
        expect(newAddress.PersonInfo._State).toBe('WI');
        expect(newAddress.PersonInfo._ZipCode).toBe('12345');
    });

    it('should replace an invalid email with bonton@bonton.com', function () {
        var addressInfo = {
            Address1: '123 Main St',
            Address2: 'Apt 10',
            City: 'Milwaukee',
            Country: 'US',
            Email1: 'a@bonton',
            FirstName: 'Bonton',
            LastName: 'Customer',
            Phone: '1234567890',
            State: 'WI',
            ZipCode: '12345'
        };

        var newAddress = giftRegistryService.constructRegistryAddress('1234', addressInfo);

        expect(newAddress).toBeDefined();
        expect(newAddress.PersonInfo._AddressLine1).toBe('123 Main St');
        expect(newAddress.PersonInfo._AddressLine2).toBe('Apt 10');
        expect(newAddress.PersonInfo._City).toBe('Milwaukee');
        expect(newAddress.PersonInfo._Country).toBe('US');
        expect(newAddress.PersonInfo._EMailID).toBe('bonton@bonton.com');
        expect(newAddress.PersonInfo._FirstName).toBe('Bonton');
        expect(newAddress.PersonInfo._LastName).toBe('Customer');
        expect(newAddress.PersonInfo._DayPhone).toBe('1234567890');
        expect(newAddress.PersonInfo._State).toBe('WI');
        expect(newAddress.PersonInfo._ZipCode).toBe('12345');
    });
});