describe('Phone Format Remove Filter:', function () {

    var $filter;

    beforeEach(module('appFilters'));
    beforeEach(inject(function (_$filter_) {
        $filter = _$filter_;
    }));

    it('returns null when given null', function () {
        var phoneFormatRemoveFilter = $filter('phoneFormatRemove');
        expect(phoneFormatRemoveFilter(null)).toEqual(null);
    });

    it('returns digits when given a phone number', function () {
        var phoneFormatRemoveFilter = $filter('phoneFormatRemove');
        expect(phoneFormatRemoveFilter('(123)456-7890')).toEqual('1234567890');
    });

    it('returns digits when given a partial phone number', function () {
        var phoneFormatRemoveFilter = $filter('phoneFormatRemove');
        expect(phoneFormatRemoveFilter('(123)456')).toEqual('123456');
    });
});