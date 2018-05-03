describe('One Line Address Filter:', function () {

    var $filter, addressObj;

    beforeEach(module('appFilters'));
    beforeEach(inject(function (_$filter_) {
        $filter = _$filter_;

        addressObj = {
            _FirstName: 'Franklin',
            _MiddleName: 'Delanor',
            _LastName: 'Roosevelt',
            _AddressLine1: '123 Main Street',
            _City: 'Milwaukee',
            _State: 'WI',
            _ZipCode: '12345',
        };
    }));

    it('returns null when given null', function () {
        var addressFilter = $filter('oneLineAddress');
        expect(addressFilter(null)).toEqual(null);
    });

    it('returns a short address when short is passed in', function () {
        var addressFilter = $filter('oneLineAddress');
        expect(addressFilter(addressObj, 'short')).toEqual('123 Main Street');
    });

    it('returns a long addres when long is passed in', function () {
        var addressFilter = $filter('oneLineAddress');
        expect(addressFilter(addressObj, 'long')).toEqual('123 Main Street, Milwaukee, WI 12345');
    });

    it('returns an address excluding the country is the country is US', function () {
        var addressFilter = $filter('oneLineAddress');
        var localAddress = addressObj;
        localAddress._Country = 'US';
        expect(addressFilter(localAddress, 'long')).toEqual('123 Main Street, Milwaukee, WI 12345');
    });

    it('returns an address with an ID if it exists', function () {
        var addressFilter = $filter('oneLineAddress');
        var localAddress = addressObj;
        localAddress._AddressID = 'Home';
        expect(addressFilter(localAddress, 'long', '', 'id')).toEqual('(Home) 123 Main Street, Milwaukee, WI 12345');
    });

    it('returns an address with a name if specified', function () {
        var addressFilter = $filter('oneLineAddress');
        expect(addressFilter(addressObj, 'long', 'name', 'id')).toEqual('Franklin Delanor Roosevelt, 123 Main Street, Milwaukee, WI 12345');
    });

    it('returns an address with the country is the country is not US', function () {
        var addressFilter = $filter('oneLineAddress');
        var canadaAddress = {
            _FirstName: 'Wayne',
            _LastName: 'Gretzsky',
            _AddressLine1: '123 Hockey Street',
            _City: 'Toronto',
            _State: 'ON',
            _ZipCode: '12345',
            _Country: 'CAN'
        }
        expect(addressFilter(canadaAddress, 'long')).toEqual('123 Hockey Street, Toronto, ON 12345, CAN');
    });
});