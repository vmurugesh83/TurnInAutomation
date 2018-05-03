describe("Sent SMTP Error Email Service", function () {
    var sendSMTPService, $httpBackend;

    beforeEach(module('appUtilities'));

    beforeEach(inject(function (_sendSMTPErrorEmail_, $injector, _POSService_) {
        sendSMTPService = _sendSMTPErrorEmail_;
        $httpBackend = $injector.get('$httpBackend');
        POSService = _POSService_;
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(sendSMTPService).toBeDefined();
    });

    it('should call the smtp email endpoint', function () {
        spyOn(POSService, 'getPOSParameters').and.returnValue({
            terminalNumber: '4',
            storeNumber: '101'
        });

        $httpBackend.expectPOST(serviceURL + "/Email/SendSMTPEmail", {
            "Email": {
                "To":"OMSTeam@bonton.com",
                "From":"DoNotReply@bonton.com",
                "Subject": "LUFI-2 in the Store 101 and terminal 4 Failed while executing passedUrl",
                "Body": " Error Messsage = \"failed\" Exception List = \n  JSONInput = \nundefined"
            }
        }).respond('success');

        sendSMTPService('failed', 'passedUrl');

        $httpBackend.flush();
    });
});