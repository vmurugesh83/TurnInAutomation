describe("App State Factory", function () {
    var appState, addressChange;

    beforeEach(module('appServiceAppState'));

    beforeEach(inject(function (_appState_) {
        appState = _appState_;

        addressChange = { previousStateName: 'a', customerKey: '123', setShipToOrderLinePrimeSubLineObjArray: ['a', 'b', 'c'] };
    }));

    it('should exist', function () {
        expect(appState).not.toBeUndefined();
    });

    it('should have default addressChange value set', function () {
        expect(appState.getAddressChange().previousStateName).toBe(null);
        expect(appState.getAddressChange().customerKey).toBe(null);
        expect(appState.getAddressChange().setShipToOrderLinePrimeSubLineObjArray.length).toBe(0);
    })

    it('should set and retrieve the addressChange', function () {
        appState.setAddressChange(addressChange);
        var returned = appState.getAddressChange();
        expect(returned.previousStateName).toBe('a');
        expect(returned.customerKey).toBe('123');
        expect(returned.setShipToOrderLinePrimeSubLineObjArray.length).toBe(3);
    });

    it('should reset the addressChange after calling get once', function () {
        appState.setAddressChange(addressChange);
        var returned = appState.getAddressChange();
        expect(returned.previousStateName).toBe('a');
        expect(returned.customerKey).toBe('123');
        expect(returned.setShipToOrderLinePrimeSubLineObjArray.length).toBe(3);

        returned = appState.getAddressChange();
        expect(returned.previousStateName).toBe(null);
        expect(returned.customerKey).toBe(null);
        expect(returned.setShipToOrderLinePrimeSubLineObjArray.length).toBe(0);
    });
});