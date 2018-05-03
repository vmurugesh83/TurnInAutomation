describe("Item Locate", function () {
    var itemLocate;

    beforeEach(angular.mock.module('appUtilities', 'appServicesWebSocket', 'appServicesItem'));
    beforeEach(module('appServicesItem'));

    beforeEach(inject(function (_itemLocate_, $injector) {
        itemLocate = _itemLocate_;
        //$rootScope = _$rootScope_;
        //$cacheFactory = _$cacheFactory_;
        $httpBackend = $injector.get('$httpBackend');
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(itemLocate).toBeDefined();
    });

    it('should call the item locate endpoint with the parameters passed', function () {
        var successCalled = false;
        var city = 'Milwaukee', zip = '12345', radius = '10', id = '123', state = 'WI', store = '101';

        $httpBackend.when('POST', serviceURL + "/Item/ItemLocate").respond({});
        $httpBackend.expectPOST(serviceURL + '/Item/ItemLocate', {
            ItemLocateInput: {
                input: {
                    _City: city,
                    _ZipCode: zip,
                    _DistanceToConsider: radius,
                    _ItemID: id,
                    _State: state,
                    _Store: store
                }
            }
        });

        itemLocate.locate(zip, city, radius, state, id, store).then(function () {
            successCalled = true;
        }, function () { });

        $httpBackend.flush();
        expect(successCalled).toBe(true);
    });

    it('should call the get organization list endpoint with the store specified as a parameter', function () {
        var successCalled = false;
        var store = '101';

        $httpBackend.when('POST', serviceURL + "/Organization/GetOrganizationList").respond({});
        $httpBackend.expectPOST(serviceURL + '/Organization/GetOrganizationList', {
            ItemLocateInput: {
                input: {
                    _OrganizationKey: store
                }
            }
        });

        itemLocate.getOrganizationList(store).then(function () {
            successCalled = true;
        }, function () { });

        $httpBackend.flush();
        expect(successCalled).toBe(true);
    });
});