describe('Title Case Filter:', function () {

    var $filter;

    beforeEach(module('appFilters'));
    beforeEach(inject(function (_$filter_) {
        $filter = _$filter_;
    }));

    it('returns empty string when given null', function () {
        var titleCaseFilter = $filter('titlecase');
        expect(titleCaseFilter(null)).toEqual('');
    });

    it('returns capitalized string when given a lowercase string', function () {
        var titleCaseFilter = $filter('titlecase');
        expect(titleCaseFilter('string')).toEqual('String');
    });

    it('returns capitalized string when given multiple words', function () {
        var titleCaseFilter = $filter('titlecase');
        expect(titleCaseFilter('string two')).toEqual('String Two');
    });
});