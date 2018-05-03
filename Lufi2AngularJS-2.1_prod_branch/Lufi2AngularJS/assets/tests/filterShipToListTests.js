describe('Ship To List Filter:', function () {

    var $filter, filterShipToList;

    beforeEach(module('shippingSelection'));
    beforeEach(inject(function (_$filter_) {
        $filter = _$filter_;
        filterShipToList = $filter('filterShipToList');
    }));

    it()

    it('returns null when given null', function () {
        expect(filterShipToList(null)).toEqual(null);
    });

    it('returns the full array if the addressFilterObj doesnt have a carrierService property', function () {
        var orderlineArray = [
            { a: 'a' },
            { b: 'b' }
        ];

        var addressFilterObj = {
            key: 'abc'
        };

        expect(filterShipToList(orderlineArray, addressFilterObj)).toEqual(orderlineArray);
    });

    it('returns the full array if the addressFilterObj doesnt have a key property', function () {
        var orderlineArray = [
            { a: 'a' },
            { b: 'b' }
        ];

        var addressFilterObj = {
            carrierService: 'abc'
        };

        expect(filterShipToList(orderlineArray, addressFilterObj)).toEqual(orderlineArray);
    });

    it('returns only the order lines that match the key and service code', function () {
        var orderlineArray = [
            {
                PersonInfoShipTo: {
                    _PersonInfoKey: '123'
                },
                _CarrierServiceCode: 'abc'
            },
            {
                PersonInfoShipTo: {
                    _PersonInfoKey: '456'
                },
                _CarrierServiceCode: 'def'
            }
        ];

        var addressFilterObj = {
            key: '123',
            carrierServiceCode: 'abc'
        };

        expect(filterShipToList(orderlineArray, addressFilterObj)).toEqual([
            {
                PersonInfoShipTo: {
                    _PersonInfoKey: '123'
                },
                _CarrierServiceCode: 'abc'
            }
        ]);
    });
});