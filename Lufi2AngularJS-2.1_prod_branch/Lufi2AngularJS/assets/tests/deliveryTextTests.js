describe('Delivery Text Filter:', function () {

    var $filter;

    beforeEach(module('appFilters'));
    beforeEach(inject(function (_$filter_) {
        $filter = _$filter_;
    }));

    it('returns empty string when given null', function () {
        var deliveryTextFilter = $filter('deliveryText');
        expect(deliveryTextFilter(null)).toEqual(null);
    });

    it('returns the shipping message', function () {
        var deliveryTextFilter = $filter('deliveryText');
        expect(deliveryTextFilter('shp')).toEqual('Shipping');
    });

    it('returns the pickup message', function () {
        var deliveryTextFilter = $filter('deliveryText');
        expect(deliveryTextFilter('pick')).toEqual('Pick Up In Store');
    });

    it('returns the parameter if it does not match any of the choices', function () {
        var deliveryTextFilter = $filter('deliveryText');
        expect(deliveryTextFilter('other')).toEqual('other');
    });
});