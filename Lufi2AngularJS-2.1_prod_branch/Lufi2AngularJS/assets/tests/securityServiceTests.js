describe("Security Service", function () {
    var securityService;

    beforeEach(module('appUtilities'));
    beforeEach(inject(function (_securityService_, _POSService_) {
        securityService = _securityService_;
        POSService = _POSService_;
    }));

    it('should exist', function () {
        expect(securityService).toBeDefined();
    });

    it('should return true when the user can cancel orders', function () {
        spyOn(POSService, 'getPOSParameters').and.callFake(function () {
            return {
                associateId: '111111',
                storeNumber: '101',
                terminalNumber: '123',
                roles: 'ENDUSER, LUFI_ORDER_MODIFY'
            };
        });

        expect(securityService.canCancelOrder()).toBe(true);
    });

    it('should return false when the user cannot cancel orders', function () {
        spyOn(POSService, 'getPOSParameters').and.callFake(function () {
            return {
                associateId: '111111',
                storeNumber: '101',
                terminalNumber: '123',
                roles: 'ENDUSER'
            };
        });

        expect(securityService.canCancelOrder()).toBe(false);
    });
});