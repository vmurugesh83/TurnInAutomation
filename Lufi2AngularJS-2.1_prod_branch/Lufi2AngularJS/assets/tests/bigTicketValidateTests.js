describe("Big Ticket Validate Service", function () {
    var bigTicketValidate, $httpBackend;

    var mockErrorObj = {
        newError: function () { }
    };

    beforeEach(module('appServicesItem'));

    beforeEach(function () {
        module(function ($provide) {
            $provide.value('errorObj', mockErrorObj);
        });
    });

    beforeEach(inject(function (_bigTicketValidate_, $injector) {
        bigTicketValidate = _bigTicketValidate_;
        $httpBackend = $injector.get('$httpBackend');
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(bigTicketValidate).toBeDefined();
    });

    it('should call the big ticket zip code validation endpoing', function () {
        $httpBackend.expectPOST(serviceURL + '/Utility/ValidateBigTicketZipCode', {
            "ValidateBigTicketZipCodeReq": {
                "_ZipCode": '12345'
            }
        });
        $httpBackend.whenPOST(serviceURL + '/Utility/ValidateBigTicketZipCode').respond({
            ValidateBigTicketZipCodeResp: {
                _statusMessage: 'Success',
                _isValid: 'Y'
            }
        });

        bigTicketValidate.bigTicketPromise('12345');

        $httpBackend.flush();
    });

    it('should throw an error if the zip code is non numerical', function () {
        var exceptionThrown = false;

        spyOn(mockErrorObj, 'newError');

        try {
            bigTicketValidate.bigTicketPromise('ABCDE');
        } catch (ex) {
            exceptionThrown = true;
        }

        expect(mockErrorObj.newError).toHaveBeenCalled();
        expect(exceptionThrown).toBe(true);
    });

    it('should throw an error if the zip code is not 5 digits', function () {
        var exceptionThrown = false;

        spyOn(mockErrorObj, 'newError');

        try {
            bigTicketValidate.bigTicketPromise('1234');
        } catch (ex) {
            exceptionThrown = true;
        }

        expect(mockErrorObj.newError).toHaveBeenCalled();
        expect(exceptionThrown).toBe(true);
    });
});