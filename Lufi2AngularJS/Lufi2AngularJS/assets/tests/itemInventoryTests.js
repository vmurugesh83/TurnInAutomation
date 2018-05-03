describe("Item Inventory", function () {
    var itemLocate;
    beforeEach(module('appServicesItem'));

    beforeEach(inject(function (_itemInventory_, $injector) {
        itemInventory = _itemInventory_;
        $httpBackend = $injector.get('$httpBackend');
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(itemInventory).toBeDefined();
    });

    it('should call GetItemListForOrdering', function () {
        var successCalled = false, failureCalled = false;
        $httpBackend.whenPOST(serviceURL + '/Inventory/GetItemListForOrdering').respond('success');
        $httpBackend.expectPOST(serviceURL + '/Inventory/GetItemListForOrdering', { contractContent: 'abc'});

        itemInventory({contractContent: 'abc'}).then(function (response) {
            expect(response.data).toBe('success');
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();
        expect(failureCalled).toBe(false);
        expect(successCalled).toBe(true);
    });
});