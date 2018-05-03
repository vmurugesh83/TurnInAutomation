describe("ISN Model Helper", function () {
    var isnModelHelper, loggerService, $httpBackend;

    beforeEach(angular.mock.module('appUtilities', 'appServicesWebSocket'));
    beforeEach(module('appServicesItem'));

    beforeEach(inject(function (_ISNModelHelper_) {
        isnModelHelper = _ISNModelHelper_;
    }));

    it('should exist', function () {
        expect(isnModelHelper).toBeDefined();
    });

    it('should create a facet item list', function () {
        var items = ['a', 1, 'b', 2, 'c', 3];

        var facetItemList = isnModelHelper.createFacetItem(items);
        expect(facetItemList[0].name).toBe('a');
        expect(facetItemList[0].rawName).toBe('a');
        expect(facetItemList[0].count).toBe(1);
        expect(facetItemList[1].name).toBe('b');
        expect(facetItemList[1].rawName).toBe('b');
        expect(facetItemList[1].count).toBe(2);
        expect(facetItemList[2].name).toBe('c');
        expect(facetItemList[2].rawName).toBe('c');
        expect(facetItemList[2].count).toBe(3);
    });

    it('should populate the ISN Model data', function () {
        var data = {
            facet_counts: {

            },
            grouped: {
                groupid: {
                    groups: [
                        {
                            groupValue: '12345',
                            doclist: {
                                docs: [
                                    {
                                        isnlongdesc: 'dockers shoes',
                                        vendorstyle: 'shoes',
                                        imageid: 'shoePic',
                                        id: '123',
                                        colorfamdesc: 'brown',
                                        itemsize: '10'
                                    },
                                    {
                                        isnlongdesc: 'dockers pants',
                                        vendorstyle: 'pants',
                                        imageid: 'pantsPic',
                                        id: '456',
                                        colorfamdesc: 'black',
                                        itemsize: '32x32'
                                    }
                                ]
                            },
                            numFound: 2
                        }
                    ],
                    matches: 1000,
                    ngroups: 100
                }
            }
        };

        var response = isnModelHelper.populateISNModel(data);

        expect(response).toBeDefined();
        expect(response.isnGroup.length).toBe(1);
        expect(response.isnGroup[0].productList.length).toBe(2);

        expect(response.isnGroup[0].colorSize[0].color).toBe('brown');
        expect(response.isnGroup[0].colorSize[0].size).toBe('10');
        expect(response.isnGroup[0].colorSize[1].color).toBe('black');
        expect(response.isnGroup[0].colorSize[1].size).toBe('32x32');

        expect(response.isnGroup[0].imageId).toBe('pantsPic');
        expect(response.isnGroup[0].upc).toBe('456');
        expect(response.isnGroup[0].isnNumber).toBe('12345');
        expect(response.isnGroup[0].isnLongDescription).toBe('N/A');
    });

    it('should return the UPC query string', function () {
        var upc = '123';
        expect(isnModelHelper.createUPCQueryString(upc)).toBe(SOLRURL + '/select?q=id%3A' + upc + '&fl=productcode%2Csku%2Cid%2Cproductid%2Cisn%2Cgroupid%2Cisactive&wt=json&indent=true');
    });

    it('should return the SKU query string', function () {
        var sku = '123';
        expect(isnModelHelper.createSKUQueryString(sku)).toBe(SOLRURL + '/select?q=sku%3A' + sku + '&fl=sku%2Cid%2Cproductid%2Cisn%2Cgroupid%2Cisactive&wt=json&indent=true');
    });

    it('should return the brand query string', function () {
        expect(isnModelHelper.createBrandQueryString()).toBe(SOLRURL + '/select?q=*%3A*&wt=json&indent=true&facet.sort=index&facet.field=facetbrand&facet=true&rows=0&facet.limit=-1');
    });

    it('should return the product hierarchy query string', function () {
        expect(isnModelHelper.createProductHierarchyQueryString()).toBe(SOLRURL + '/select?q=*%3A*&wt=json&indent=true&rows=0&facet=true&facet.limit=-1&facet.pivot=cmg,cfg,fob&facet.mincount=1&facet.limit=-1');
    });
});