describe("Authenticate User Service", function () {
    var authenticateUser, $httpBackend;

    beforeEach(module('appUtilities'));

    beforeEach(inject(function (_authenticateUser_, $injector) {
        authenticateUser = _authenticateUser_;
        $httpBackend = $injector.get('$httpBackend');
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(authenticateUser).toBeDefined();
    });

    it('should attempt to authenticate the user', function () {
        $httpBackend.expectPOST(authURL, {
            Authreq: {
                uid: 'username',
                pwd: 'password'
            }
        }).respond('good');

        authenticateUser('username', 'password');

        $httpBackend.flush();
    });
});