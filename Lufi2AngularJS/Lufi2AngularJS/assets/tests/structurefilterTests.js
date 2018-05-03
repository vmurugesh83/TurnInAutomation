describe('Structure Filter:', function () {

    var $filter;

    beforeEach(module('appFilters'));
    beforeEach(inject(function (_$filter_) {
        $filter = _$filter_;
    }));

    it('returns [] when given null', function () {
        var structureFilter = $filter('structurefilter');
        expect(structureFilter(null)).toEqual([]);
    });

    it('returns [] if items are not visible', function () {
        var nonVisibleItems = [{ name: 'a', visible: false }, { name: 'b', visible: false }];

        var structureFilter = $filter('structurefilter');
        expect(structureFilter(nonVisibleItems)).toEqual([]);
    });

    it('returns all items if all are visible', function () {
        var allVisibleItems = [{ name: 'a', visible: true }, { name: 'b', visible: true }];

        var structureFilter = $filter('structurefilter');
        expect(structureFilter(allVisibleItems)).toEqual(allVisibleItems);
    });

    it('returns only visible items', function () {
        var someVisibleItems = [{ name: 'a', visible: true }, { name: 'b', visible: false }];

        var structureFilter = $filter('structurefilter');
        expect(structureFilter(someVisibleItems)).toEqual([{ name: 'a', visible: true }]);
    });
});