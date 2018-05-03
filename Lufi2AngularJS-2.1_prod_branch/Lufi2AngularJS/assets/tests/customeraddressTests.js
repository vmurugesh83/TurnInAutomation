//describe('customeraddress Directive', function () {
//    var $compile,
//        $rootScope,
//        $httpBackend;

//    beforeEach(module('appDirectives', '../html/**/*.html'));

//    beforeEach(inject(function (_$compile_, _$rootScope_, $injector) {
//        $compile = _$compile_;
//        $rootScope = _$rootScope_;
//        //$httpBackend = $injector.get('$httpBackend');
//        //$httpBackend.expectGET('html/customer/customerAddress.html').respond(200, '');
//    }));

//    it('Replaces the element with the appropriate content', function () {
//        $rootScope.selectedCustomer = {
//            defaultAddresses: {
//                PersonInfoShipTo: {
//                    _FirstName: 'Austin'
//                }
//            }
//        };

//        $rootScope.customerDetailsHeaderAddress = 'aaa';

//        var element = $compile('<customeraddress address="selectedCustomer.defaultAddresses.PersonInfoShipTo" addressclass="customerDetailsHeaderAddress"></customeraddress>')($rootScope);
//        $rootScope.$digest();
//        expect(element.html()).toContain("lidless, wreathed in flame, 2 times");
//    });
//});