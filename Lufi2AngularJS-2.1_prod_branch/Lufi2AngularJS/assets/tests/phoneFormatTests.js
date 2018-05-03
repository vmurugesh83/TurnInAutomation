describe('Phone Format Filter:', function () {

    var $filter;

    beforeEach(module('appFilters'));
    beforeEach(inject(function (_$filter_) {
        $filter = _$filter_;
    }));

    it('returns null when given null', function () {
        var phoneFormatFilter = $filter('phoneFormat');
        expect(phoneFormatFilter(null)).toEqual(null);
    });

    it('returns an object when given an object', function () {
        var phoneFormatFilter = $filter('phoneFormat');
        expect(phoneFormatFilter({ a: 'a' })).toEqual({ a: 'a' });
    });

    it('returns a formatted 10 digit phone number', function () {
        var phoneFormatFilter = $filter('phoneFormat');
        expect(phoneFormatFilter('1234567890')).toEqual('(123)456-7890');
    });

    it('returns the area code with a preceeding parenthesis', function () {
        var phoneFormatFilter = $filter('phoneFormat');
        expect(phoneFormatFilter('123')).toEqual('(123');
    });
});