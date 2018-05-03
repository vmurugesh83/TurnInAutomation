describe("Item Search", function () {
    var itemSearch, loggerService, $httpBackend, $rootScope;

    beforeEach(angular.mock.module('appUtilities', 'appServicesWebSocket', 'appServicesItem'));
    beforeEach(module('appServicesItem'));

    beforeEach(inject(function (_itemSearch_, _loggerService_, _$cacheFactory_, $injector, _$rootScope_) {
        itemSearch = _itemSearch_;
        loggerService = _loggerService_;
        $rootScope = _$rootScope_;
        $cacheFactory = _$cacheFactory_;
        $httpBackend = $injector.get('$httpBackend');
    }));

    afterEach(function () {
        $httpBackend.verifyNoOutstandingExpectation();
        $httpBackend.verifyNoOutstandingRequest();
    });

    it('should exist', function () {
        expect(itemSearch).toBeDefined();
    });

    it('should perform a search and return formatted data', function () {
        $httpBackend.when('GET', SOLRURL + '/select?q=brandlongdesc:"Dockers" AND isactive:Y AND (buyable:true OR ((buyable:false OR (*:* NOT buyable:*)) AND -pricestatus:F AND -pricestatus:P))&facet=true&group.start=0&group.limit=500&group=true&group.field=groupid&group.facet=true&group.ngroups=true&facet.sort=index&facet.mincount=1&facet.limit=-1&facet.field=facetfob&facet.field=facetbrand&facet.field=facetcolor&facet.field=facetitemsize& AND (buyable:true OR ((buyable:false OR (*:* NOT buyable:*)) AND -pricestatus:F AND -pricestatus:P))&sort=buyable desc&wt=json&indent=true&start=0&rows=20&omitHeader=true').respond({
            facet_counts: {
                facet_fields: {
                    facetitemsize: ["10", 1, "10.5", 1, "12", "12.5", "14", 1, "14.5", 1]
                }
            },
            grouped: {
                groupid: {
                    groups: [
                        {
                            doclist: {
                                docs: [
                                    {
                                        _version_: 1507321007691530200,
                                        brandlongdesc: "Nike",
                                        buyable: true,
                                        cfg: "170 - GIRLS",
                                        classlongdesc: "NIKE",
                                        cmg: "509 - CHILDRENS",
                                        colorDc: "00023 - GRY HTHER",
                                        colorattrdesc: "Dark Gray",
                                        colorcode: 23,
                                        colorfamdesc: "Black/Gray Fam",
                                        colorlongdesc: "GRY HTHER",
                                        corpdesc: "Act Pant-G2-6x",
                                        createts: "2015-07-20T03:05:31.959724",
                                        crg: "300 - CHILDRENS",
                                        deptlongdesc: "GIRLSWEAR 2-6X",
                                        desc1: "NIKE OCT",
                                        desc2: "FALL NIKE",
                                        desc4: "TODDLER GIRL",
                                        fabdtldesc: "Solid",
                                        fablongdesc: "Cotton",
                                        fob: "35 - GIRLS 2-6X",
                                        genclasslongdesc: "Long Pants",
                                        gensclalongdesc: "Active Pant",
                                        giftwrapCode: "2",
                                        groupid: "243309529",
                                        hazardCode: "1",
                                        id: 617846551889,
                                        invfillcode: "1",
                                        isactive: "Y",
                                        isairshipallowed: "Y",
                                        isgroundshipallowed: "Y",
                                        isn: 243309529,
                                        isnlongdesc: "FL14 TD CUFF FLEECE PANT GREY",
                                        isspecialhandling: "N",
                                        iswebexclusive: "N",
                                        itemsize: "3T",
                                        itemtype: "REG",
                                        labellongdesc: "Nike",
                                        pricestatus: "C",
                                        proddetail2: "Elastic Bottom",
                                        proddetail3: "Stretch Waistband",
                                        proddtllongdesc: "Unlined",
                                        size1code: "3T ",
                                        size2code: " ",
                                        sizedc: "3T ",
                                        sizesequence: 2470,
                                        sku: 424301320184,
                                        specialhandlingcode: "1",
                                        specialhandlingfee: 0,
                                        vendorstyle: "262065G"
                                    },
                                    {
                                        _version_: 1507320811782930400,
                                        brandlongdesc: "Nike",
                                        buyable: true,
                                        cfg: "170 - GIRLS",
                                        classlongdesc: "NIKE",
                                        cmg: "509 - CHILDRENS",
                                        colorDc: "00023 - GRY HTHER",
                                        colorattrdesc: "Dark Gray",
                                        colorcode: 23,
                                        colorfamdesc: "Black/Gray Fam",
                                        colorlongdesc: "GRY HTHER",
                                        corpdesc: "Act Pant-G2-6x",
                                        createts: "2015-07-20T03:05:31.959724",
                                        crg: "300 - CHILDRENS",
                                        deptlongdesc: "GIRLSWEAR 2-6X",
                                        desc1: "NIKE OCT",
                                        desc2: "FALL NIKE",
                                        desc4: "TODDLER GIRL",
                                        fabdtldesc: "Solid",
                                        fablongdesc: "Cotton",
                                        fob: "35 - GIRLS 2-6X",
                                        genclasslongdesc: "Long Pants",
                                        gensclalongdesc: "Active Pant",
                                        giftwrapCode: "2",
                                        groupid: "243309529",
                                        hazardCode: "1",
                                        id: 617846551872,
                                        invfillcode: "1",
                                        isactive: "Y",
                                        isairshipallowed: "Y",
                                        isgroundshipallowed: "Y",
                                        isn: 243309529,
                                        isnlongdesc: "FL14 TD CUFF FLEECE PANT GREY",
                                        isspecialhandling: "N",
                                        iswebexclusive: "N",
                                        itemsize: "2T",
                                        itemtype: "REG",
                                        labellongdesc: "Nike",
                                        pricestatus: "C",
                                        proddetail2: "Elastic Bottom",
                                        proddetail3: "Stretch Waistband",
                                        proddtllongdesc: "Unlined",
                                        size1code: "2T ",
                                        size2code: " ",
                                        sizedc: "2T ",
                                        sizesequence: 1360,
                                        sku: 424301320177,
                                        specialhandlingcode: "1",
                                        specialhandlingfee: 0,
                                        vendorstyle: "262065G"
                                    },
                                    {
                                        _version_: 1510463664531964000,
                                        brandlongdesc: "Nike",
                                        buyable: false,
                                        cfg: "170 - GIRLS",
                                        classlongdesc: "NIKE",
                                        cmg: "509 - CHILDRENS",
                                        colorDc: "00023 - GRY HTHER",
                                        colorattrdesc: "Dark Gray",
                                        colorcode: 23,
                                        colorfamdesc: "Black/Gray Fam",
                                        colorlongdesc: "GRY HTHER",
                                        corpdesc: "Act Pant-G2-6x",
                                        createts: "2015-07-20T03:05:31.959724",
                                        crg: "300 - CHILDRENS",
                                        deptlongdesc: "GIRLSWEAR 2-6X",
                                        desc1: "NIKE OCT",
                                        desc2: "FALL NIKE",
                                        desc4: "TODDLER GIRL",
                                        fabdtldesc: "Solid",
                                        fablongdesc: "Cotton",
                                        fob: "35 - GIRLS 2-6X",
                                        genclasslongdesc: "Long Pants",
                                        gensclalongdesc: "Active Pant",
                                        giftwrapCode: "2",
                                        groupid: "243309529",
                                        hazardCode: "1",
                                        id: 617846551896,
                                        invfillcode: "1",
                                        isactive: "Y",
                                        isairshipallowed: "Y",
                                        isgroundshipallowed: "Y",
                                        isn: 243309529,
                                        isnlongdesc: "FL14 TD CUFF FLEECE PANT GREY",
                                        isspecialhandling: "N",
                                        iswebexclusive: "N",
                                        itemsize: "4T",
                                        labellongdesc: "Nike",
                                        pricestatus: "C",
                                        proddetail2: "Elastic Bottom",
                                        proddetail3: "Stretch Waistband",
                                        proddtllongdesc: "Unlined",
                                        size1code: "4T ",
                                        size2code: " ",
                                        sizedc: "4T ",
                                        sizesequence: 3970,
                                        sku: 424301320191,
                                        specialhandlingcode: "1",
                                        specialhandlingfee: 0,
                                        vendorstyle: "262065G"
                                    }
                                ],
                                numFound: 3,
                                start: 0
                            },
                            groupValue: "243309529"
                        }]
                }
            },
            matches: 444,
            ngroups: 119
        });

        $httpBackend.expectGET(SOLRURL + '/select?q=brandlongdesc:"Dockers" AND isactive:Y AND (buyable:true OR ((buyable:false OR (*:* NOT buyable:*)) AND -pricestatus:F AND -pricestatus:P))&facet=true&group.start=0&group.limit=500&group=true&group.field=groupid&group.facet=true&group.ngroups=true&facet.sort=index&facet.mincount=1&facet.limit=-1&facet.field=facetfob&facet.field=facetbrand&facet.field=facetcolor&facet.field=facetitemsize& AND (buyable:true OR ((buyable:false OR (*:* NOT buyable:*)) AND -pricestatus:F AND -pricestatus:P))&sort=buyable desc&wt=json&indent=true&start=0&rows=20&omitHeader=true');

        var queryParam = {
            brandName: "Dockers",
            currentPage: 1,
            rows: 20,
            selectedBrands: [],
            selectedColors: [],
            selectedFOBs: [],
            selectedItemSizes: [],
            start: 0
        };

        itemSearch.search(queryParam, function (response) {
            successCalled = true;
            expect(response.itemSizes.length).toBe(5);
            expect(response.isnGroup.length).toBe(1);
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
    });

    it('should call search', function () {
        var successCalled = false, failureCalled = false;

        $httpBackend.expectGET(SOLRURL + '/select?q=id%3A888161289904&fl=productcode%2Csku%2Cid%2Cproductid%2Cisn%2Cgroupid%2Cisactive&wt=json&indent=true').respond({
            response: {
                docs: [
                    {
                        groupid: "160108393420120",
                        id: 888161289904,
                        isactive: "Y",
                        isn: 160108393,
                        productcode: 420120,
                        sku: 416000743357
                    }],
                numFound: 1,
                start: 0
            }
        });

        $httpBackend.expectGET(SOLRURL + '/select?q=( isn:160108393  OR productcode:420120) AND isactive:Y AND (buyable:true OR ((buyable:false OR (*:* NOT buyable:*)) AND -pricestatus:F AND -pricestatus:P))&facet=true&group.start=0&group.limit=500&group=true&group.field=isactive&group.facet=true&group.ngroups=true&facet.sort=index&facet.mincount=1&facet.limit=-1&facet.field=facetfob&facet.field=facetbrand&facet.field=facetcolor&facet.field=facetitemsize& AND (buyable:true OR ((buyable:false OR (*:* NOT buyable:*)) AND -pricestatus:F AND -pricestatus:P))&sort=buyable desc&wt=json&indent=true&start=0&rows=-1&omitHeader=true').respond({
            facet_counts: {
                facet_fields: {
                    facetitemsize: ["10", 1, "10.5", 1, "12", "12.5", "14", 1, "14.5", 1]
                }
            },
            grouped: {
                groupid: {
                    groups: [
                        {
                            doclist: {
                                docs: [
                                    {
                                        _version_: 1507321007691530200,
                                        brandlongdesc: "Nike",
                                        buyable: true,
                                        cfg: "170 - GIRLS",
                                        classlongdesc: "NIKE",
                                        cmg: "509 - CHILDRENS",
                                        colorDc: "00023 - GRY HTHER",
                                        colorattrdesc: "Dark Gray",
                                        colorcode: 23,
                                        colorfamdesc: "Black/Gray Fam",
                                        colorlongdesc: "GRY HTHER",
                                        corpdesc: "Act Pant-G2-6x",
                                        createts: "2015-07-20T03:05:31.959724",
                                        crg: "300 - CHILDRENS",
                                        deptlongdesc: "GIRLSWEAR 2-6X",
                                        desc1: "NIKE OCT",
                                        desc2: "FALL NIKE",
                                        desc4: "TODDLER GIRL",
                                        fabdtldesc: "Solid",
                                        fablongdesc: "Cotton",
                                        fob: "35 - GIRLS 2-6X",
                                        genclasslongdesc: "Long Pants",
                                        gensclalongdesc: "Active Pant",
                                        giftwrapCode: "2",
                                        groupid: "243309529",
                                        hazardCode: "1",
                                        id: 617846551889,
                                        invfillcode: "1",
                                        isactive: "Y",
                                        isairshipallowed: "Y",
                                        isgroundshipallowed: "Y",
                                        isn: 243309529,
                                        isnlongdesc: "FL14 TD CUFF FLEECE PANT GREY",
                                        isspecialhandling: "N",
                                        iswebexclusive: "N",
                                        itemsize: "3T",
                                        itemtype: "REG",
                                        labellongdesc: "Nike",
                                        pricestatus: "C",
                                        proddetail2: "Elastic Bottom",
                                        proddetail3: "Stretch Waistband",
                                        proddtllongdesc: "Unlined",
                                        size1code: "3T ",
                                        size2code: " ",
                                        sizedc: "3T ",
                                        sizesequence: 2470,
                                        sku: 424301320184,
                                        specialhandlingcode: "1",
                                        specialhandlingfee: 0,
                                        vendorstyle: "262065G"
                                    },
                                    {
                                        _version_: 1507320811782930400,
                                        brandlongdesc: "Nike",
                                        buyable: true,
                                        cfg: "170 - GIRLS",
                                        classlongdesc: "NIKE",
                                        cmg: "509 - CHILDRENS",
                                        colorDc: "00023 - GRY HTHER",
                                        colorattrdesc: "Dark Gray",
                                        colorcode: 23,
                                        colorfamdesc: "Black/Gray Fam",
                                        colorlongdesc: "GRY HTHER",
                                        corpdesc: "Act Pant-G2-6x",
                                        createts: "2015-07-20T03:05:31.959724",
                                        crg: "300 - CHILDRENS",
                                        deptlongdesc: "GIRLSWEAR 2-6X",
                                        desc1: "NIKE OCT",
                                        desc2: "FALL NIKE",
                                        desc4: "TODDLER GIRL",
                                        fabdtldesc: "Solid",
                                        fablongdesc: "Cotton",
                                        fob: "35 - GIRLS 2-6X",
                                        genclasslongdesc: "Long Pants",
                                        gensclalongdesc: "Active Pant",
                                        giftwrapCode: "2",
                                        groupid: "243309529",
                                        hazardCode: "1",
                                        id: 617846551872,
                                        invfillcode: "1",
                                        isactive: "Y",
                                        isairshipallowed: "Y",
                                        isgroundshipallowed: "Y",
                                        isn: 243309529,
                                        isnlongdesc: "FL14 TD CUFF FLEECE PANT GREY",
                                        isspecialhandling: "N",
                                        iswebexclusive: "N",
                                        itemsize: "2T",
                                        itemtype: "REG",
                                        labellongdesc: "Nike",
                                        pricestatus: "C",
                                        proddetail2: "Elastic Bottom",
                                        proddetail3: "Stretch Waistband",
                                        proddtllongdesc: "Unlined",
                                        size1code: "2T ",
                                        size2code: " ",
                                        sizedc: "2T ",
                                        sizesequence: 1360,
                                        sku: 424301320177,
                                        specialhandlingcode: "1",
                                        specialhandlingfee: 0,
                                        vendorstyle: "262065G"
                                    },
                                    {
                                        _version_: 1510463664531964000,
                                        brandlongdesc: "Nike",
                                        buyable: false,
                                        cfg: "170 - GIRLS",
                                        classlongdesc: "NIKE",
                                        cmg: "509 - CHILDRENS",
                                        colorDc: "00023 - GRY HTHER",
                                        colorattrdesc: "Dark Gray",
                                        colorcode: 23,
                                        colorfamdesc: "Black/Gray Fam",
                                        colorlongdesc: "GRY HTHER",
                                        corpdesc: "Act Pant-G2-6x",
                                        createts: "2015-07-20T03:05:31.959724",
                                        crg: "300 - CHILDRENS",
                                        deptlongdesc: "GIRLSWEAR 2-6X",
                                        desc1: "NIKE OCT",
                                        desc2: "FALL NIKE",
                                        desc4: "TODDLER GIRL",
                                        fabdtldesc: "Solid",
                                        fablongdesc: "Cotton",
                                        fob: "35 - GIRLS 2-6X",
                                        genclasslongdesc: "Long Pants",
                                        gensclalongdesc: "Active Pant",
                                        giftwrapCode: "2",
                                        groupid: "243309529",
                                        hazardCode: "1",
                                        id: 617846551896,
                                        invfillcode: "1",
                                        isactive: "Y",
                                        isairshipallowed: "Y",
                                        isgroundshipallowed: "Y",
                                        isn: 243309529,
                                        isnlongdesc: "FL14 TD CUFF FLEECE PANT GREY",
                                        isspecialhandling: "N",
                                        iswebexclusive: "N",
                                        itemsize: "4T",
                                        labellongdesc: "Nike",
                                        pricestatus: "C",
                                        proddetail2: "Elastic Bottom",
                                        proddetail3: "Stretch Waistband",
                                        proddtllongdesc: "Unlined",
                                        size1code: "4T ",
                                        size2code: " ",
                                        sizedc: "4T ",
                                        sizesequence: 3970,
                                        sku: 424301320191,
                                        specialhandlingcode: "1",
                                        specialhandlingfee: 0,
                                        vendorstyle: "262065G"
                                    }
                                ],
                                numFound: 3,
                                start: 0
                            },
                            groupValue: "243309529"
                        }]
                }
            },
            matches: 444,
            ngroups: 119
        });

        spyOn(itemSearch, 'search');

        itemSearch.searchUPC('888161289904', function (data) {
            successCalled = true;
        }, function () {
            failureCalled = true;
        });

        $httpBackend.flush();

        expect(successCalled).toBe(true);
        expect(failureCalled).toBe(false);
    });
});