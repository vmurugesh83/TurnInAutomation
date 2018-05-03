describe("Item to Locate", function () {
    var itemToLocate;

    beforeEach(module('appServicesItem'));
    beforeEach(inject(function (_itemToLocate_) {
        itemToLocate = _itemToLocate_;
    }));

    it('should exist', function () {
        expect(itemToLocate).toBeDefined();
    });

    it('should set and get the savedItem object', function () {
        itemToLocate.set('test');
        expect(itemToLocate.get()).toBe('test');
        itemToLocate.set('');
        expect(itemToLocate.get()).toBe('');
    });
});