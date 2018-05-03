describe("btProp Service", function () {
    var btProp;

    beforeEach(module('propertiesService'));

    beforeEach(inject(function (_btProp_) {
        btProp = _btProp_;
    }));

    it('should exist', function () {
        expect(btProp).toBeDefined();
    });

    it('should retrieve the property requested', function () {
        expect(btProp.getProp('btGiftMessageReasonTo')).toBe('GIFT_MESSAGE_TO');
    });
});