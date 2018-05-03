var appServices = angular.module('appServicesItem', ['appUtilities']);

appServices.service('ISNModelHelper', [function () {
    /*
        The ISNModelHelper namespace is used to create and populate ItemSearch Model and Facets from SOLR queries.
    */

    var createFacetItem = function (items) {
        var facet = [];
        if (items) {
            for (var i = 0; i < items.length - 1; i = i + 2) {
                if (items[i] && items[i].trim().length > 0) {
                    var item = {
                        rawName: items[i],
                        //name: items[i].indexOf('-') == -1 ? items[i] : items[i].substring(items[i].indexOf('-') + 2),
                        name: items[i],
                        count: items[i + 1]
                    };
                    facet.push(item);
                }
            }
        }
        return facet;
    }


    var populateISNModel = function (data) {
        response = {};
        for (groupField in data.grouped) break;
        response.ngroups = data.grouped[groupField].ngroups;
        var isnGroup = [];

        data.grouped[groupField].groups.forEach(function (item) {
            var isn = {};
            isn.isnNumber = item.groupValue;
            isn.isnLongDescription = "N/A";
            isn.variations = item.doclist.numFound;
            if (item.doclist.docs.length > 0) {
                isn.productname = item.doclist.docs[0].isnlongdesc;
                isn.vendorstyle = item.doclist.docs[0].vendorstyle;
            }

            var productList = [];
            var imageId = '';
            var upc = '';
            var colorSize = [];
            item.doclist.docs.forEach(function (docItem) {
                productList.push(docItem);
                imageId = docItem.imageid;
                upc = docItem.id;
                colorSize.push({
                    color: docItem.colorfamdesc,
                    size: docItem.itemsize,
                    docItem: docItem
                });
            });

            isn.imageId = imageId;
            isn.upc = upc;
            isn.colorSize = colorSize;
            isn.productList = productList;
            isnGroup.push(isn);
        })
        response.isnGroup = isnGroup;
        return response;
    }

    //creates SOLR Query String from given queryParam
    var createSOLRQueryString = function (queryParam) {
        var createFacetQueryString = function (facetSelectionArray, fieldName) {
            if (facetSelectionArray.length == 0) {
                return undefined;
            } else if (facetSelectionArray.length == 1) {
                return fieldName + ':\"' + solrEncoding(facetSelectionArray[0].name) + '\"'
            } else {
                var facet = '(';
                facetSelectionArray.forEach(function (item) {
                    if (facet === '(') {
                        facet = facet + fieldName + ':\"' + solrEncoding(item.name) + '\"'
                    } else {
                        facet = facet + ' OR ' + fieldName + ':\"' + solrEncoding(item.name) + '\"'
                    }

                })
                return facet + ')';
            }
        };

        //Creates Facet Filter String {!tag=dt} allows filter to keep tagged Facet unchanged.
        var createFacetFilterString = function (facetSelectionArray, fieldName) {
            if (facetSelectionArray.length == 0) {
                return undefined;
            } else if (facetSelectionArray.length == 1) {
                return 'fq={!tag=dt}' + fieldName + ':\"' + solrEncoding(facetSelectionArray[0].name) + '\"'
            } else {
                var facet = '(';
                facetSelectionArray.forEach(function (item) {
                    if (facet === '(') {
                        facet = facet + fieldName + ':\"' + solrEncoding(item.name) + '\"'
                    } else {
                        facet = facet + ' OR ' + fieldName + ':\"' + solrEncoding(item.name) + '\"'
                    }

                })
                return 'fq={!tag=dt}' + facet + ')';
            }
        };


        function replaceAll(find, replace, str) {
            return str.replace(new RegExp(find, 'g'), replace);
        }

        var isnQuery = queryParam.isnAndProductCode ? ' isn:' + queryParam.isnAndProductCode.isn : '';
        var productCodeQuery = queryParam.isnAndProductCode && queryParam.isnAndProductCode.productcode ? '  OR productcode:' + queryParam.isnAndProductCode.productcode : '';
        var isnAndProductCode = queryParam.isnAndProductCode ? '(' + isnQuery + productCodeQuery + ')' : undefined;

        var buyable = queryParam.buyable ? 'buyable:true' : undefined;

        var upc = queryParam.upc ? 'id:\"' + queryParam.upc + '\"' : undefined;

        if(angular.isString(queryParam.webIdImageId))
        {
            queryParam.webIdImageId = queryParam.webIdImageId.trim();
        }

        var webIdSearchSection = [];

        //if valid input push onto array
        if (queryParam.webIdImageId && (/^\d{1,10}$/).test(queryParam.webIdImageId) && (parseInt(queryParam.webIdImageId) < 2147483600))
        {
            webIdSearchSection.push('imageid:\"' + queryParam.webIdImageId + '\"');
        }

        if(queryParam.webIdImageId)
        {
            webIdSearchSection.push('webid:(\*' + solrEncoding(queryParam.webIdImageId) + '\*)');

            webIdSearchSection.push('keywords:"' + solrEncoding(queryParam.webIdImageId) + '"');
        }

        var imageWebId = webIdSearchSection.length > 0 ? "(" + webIdSearchSection.join(" OR ") + ")": undefined;

        // var imageWebId = queryParam.webIdImageId ? 'imagewebid:\*' + queryParam.webIdImageId.toString().trim() + '\*' : undefined;

         var selectedCMG = queryParam.selectedCMG ? 'cmg:\"' + solrEncoding(queryParam.selectedCMG) + '\"' : undefined;
         var selectedCFG = queryParam.selectedCFG ? 'cfg:\"' + solrEncoding(queryParam.selectedCFG) + '\"' : undefined;
         var selectedFOB = queryParam.selectedFOB ? 'fob:\"' + solrEncoding(queryParam.selectedFOB) + '\"' : undefined;
         var brandName = queryParam.brandName ? 'brandlongdesc:\"' + solrEncoding(queryParam.brandName) + '\"' : undefined;
         var vendorStyle = queryParam.vendorStyle ? 'vendorstyle:\"' + solrEncoding(queryParam.vendorStyle) + '\"' : undefined;
         var description = queryParam.description ? 'descriptions:(' + solrEncoding(splitDescriptionTerms(queryParam.description)) + ')' : undefined;
         var isActive = 'isactive:Y';
         var start = queryParam.start;
         var rows = queryParam.rows;
         var colorFilter = undefined;
         var size1Filter = undefined;
         var size2Filter = undefined;

         if (queryParam.colorFilter)
         {
             if (queryParam.isnAndProductCode.colorcode !== null && queryParam.isnAndProductCode.colorcode !== undefined)
             {
                 colorFilter = 'colorcode:\"' + queryParam.isnAndProductCode.colorcode + '\"'; //colorCode is an int
             }
         }

         if (queryParam.sizeFilter)
         {
             
             if (angular.isString(queryParam.isnAndProductCode.size1code))
             {
                 size1Filter = queryParam.isnAndProductCode.size1code.trim().length > 0 ? 'size1code:\"' + solrEncoding(queryParam.isnAndProductCode.size1code) + '\"' : undefined; //size1code is a text type
             }

             if (angular.isString(queryParam.isnAndProductCode.size2code)) {
                 size2Filter = queryParam.isnAndProductCode.size2code.trim().length > 0 ? 'size2code:\"' + solrEncoding(queryParam.isnAndProductCode.size2code) + '\"' : undefined; //size2code is a text type
             }
         }


        var q = undefined;

        function buildQ(qItems) {
            if (qItems) {
                qItems.forEach(function (item) {
                    if (item && !q) {
                        q = item;
                    } else if (q && item) {
                        q = q + ' AND ' + item;
                }
        });
    }
        }

        buildQ([buyable, isnAndProductCode, upc, colorFilter, size1Filter, size2Filter, imageWebId, selectedCMG, selectedCFG, selectedFOB, brandName, vendorStyle, description, isActive]);

        var facetFields = undefined;
        var fq = [];
        var requestedFilterFacets = {};
        if (angular.isString(queryParam.filterFacet) && (queryParam.filterFacet.trim().length > 0)) {
            requestedFilterFacets[queryParam.filterFacet] = true;
            } else if (angular.isArray(queryParam.filterFacet)) {
                for (var p = 0; p < queryParam.filterFacet.length; p++) {
                    requestedFilterFacets[queryParam.filterFacet[p]] = true;
    }
        }

        if (requestedFilterFacets.fob) {
            facetFields = "facet.field={!ex=dt}facetfob";
            fq.push(createFacetFilterString(queryParam.selectedFOBs, 'facetfob'));
        } else {
            facetFields = "facet.field=facetfob";
            buildQ([createFacetQueryString(queryParam.selectedFOBs, 'facetfob')]);
        }
        if (requestedFilterFacets.brand) {
            facetFields = facetFields + "&facet.field={!ex=dt}facetbrand";
            fq.push(createFacetFilterString(queryParam.selectedBrands, 'facetbrand'));
        } else {
            facetFields = facetFields + "&facet.field=facetbrand";
            buildQ([createFacetQueryString(queryParam.selectedBrands, 'facetbrand')]);
        }
        if (requestedFilterFacets.color) {
            facetFields = facetFields + "&facet.field={!ex=dt}facetcolor";
            fq.push(createFacetFilterString(queryParam.selectedColors, 'colorattrdesc'));
        } else {
            facetFields = facetFields + "&facet.field=facetcolor";
            buildQ([createFacetQueryString(queryParam.selectedColors, 'colorattrdesc')]);
        }
        if (requestedFilterFacets.itemSize) {
            facetFields = facetFields + "&facet.field={!ex=dt}facetitemsize";
            fq.push(createFacetFilterString(queryParam.selectedItemSizes, 'itemsize'));
        } else {
            facetFields = facetFields + "&facet.field=facetitemsize";
            buildQ([createFacetQueryString(queryParam.selectedItemSizes, 'itemsize')]);
    }

        // 1st version // var extraFilter = " AND (buyable:true OR (-buyable:[* TO *] AND -pricestatus:P AND -pricestatus:F))";
        // 2nd version //var extraFilter = " AND (buyable:true OR ((*:* NOT buyable:*) AND -pricestatus:P AND -pricestatus:F))";
        // var extraFilter = " AND (buyable:true OR ((buyable:false OR (*:* NOT buyable:*)) AND -pricestatus:F AND -pricestatus:P))";
        //extraFilter = "";
        if (!q) {
            q = '*:*';
    }

        //q = q + extraFilter;

        queryGroupField = !isnAndProductCode ? 'groupid' : 'isactive';

        var facetDef = "facet=true&group.start=0&group.limit=500&group=true&group.field=" + queryGroupField + "&group.facet=true&group.ngroups=true&facet.sort=index&facet.mincount=1&facet.limit=-1";
        var format = 'wt=json&indent=true';
        var range = 'start=' + start + '&rows=' + rows;
        var sort = 'sort=buyable desc';
        var responseHeaderSetting = "omitHeader=true";
        //var queryString = SOLRURL + "/select?q=" + q + '&' + facetDef + '&' + facetFields + '&' + extraFilter + '&' + sort + '&' + format + '&' + range + '&' + responseHeaderSetting;

        var queryString = SOLRURL + "/select?q=" + q + '&' + facetDef + '&' + facetFields + '&' + sort + '&' + format + '&' + range + '&' + responseHeaderSetting;
        for (var p = 0; p < fq.length; p++) {
            queryString = queryString + '&' + fq[p];
        }
        return queryString;
    }
    var solrEncoding = function (data) {
        //+\-!\(\){}\[\]^"~*?:\\
        var result = data.replace(/\\/g, '\\\\');
        result = result.replace(/-/g, '\\-');
        result = result.replace(/\//g, '\\/');
        result = result.replace(/!/g, '\\!');
        result = result.replace(/\(/g, '\\(');
        result = result.replace(/\)/g, '\\)');
        result = result.replace(/{/g, '\\{');
        result = result.replace(/}/g, '\\}');
        result = result.replace(/\[/g, '\\[');
        result = result.replace(/\]/g, '\\]');
        result = result.replace(/\^/g, '\\^');
        result = result.replace(/"/g, '\\"');
        result = result.replace(/~/g, '\\~');
        result = result.replace(/\*/g, '\\*');
        result = result.replace(/\?/g, '\\?');
        result = result.replace(/:/g, '\\:');
        result = result.replace(/\+/g, '\\+');

        result = encodeURIComponent(result)
        return result;
    };

    var splitDescriptionTerms = function (DesString) {

        var result = DesString.trim().toLowerCase().replace(/(\s*,\s*|\s+)/g, ",").split(",");
        result = result.filter(function (item) {

            var commaSpaceRegex = /(\s+|.*,.*)/g;
            if(item.length > 0 && !(commaSpaceRegex.test(item))){
                return true;
            }else{
                return false;
            }
        });

        var resultString = result.join(" AND ");

        if (resultString.length < 1) {
            return '""';
        } else {
            return resultString;
        }
    };

    var populateProductHierarchy = function (data) {
        var cmg = [];
        var cfg = [];
        var fob = [];
        data.forEach(function (solrCMGItem) {
            var cmgItem = {};
            cmgItem.value = solrCMGItem.value.substring(solrCMGItem.value.indexOf('-') + 2);
            cmgItem.originalValue = solrCMGItem.value;
            cmgItem.visible = true;
            cmg.push(cmgItem);
            solrCMGItem.pivot.forEach(function (solrCFGItem) {
                var cfgItem = {};
                cfgItem.cmgItemOriginalValue = cmgItem.originalValue;
                cfgItem.value = solrCFGItem.value.substring(solrCFGItem.value.indexOf('-') + 2);
                cfgItem.originalValue = solrCFGItem.value;
                cfgItem.visible = true;
                cfg.push(cfgItem);
                solrCFGItem.pivot.forEach(function (solrFOBItem) {
                    var fobItem = {};
                    fobItem.cmgItemOriginalValue = cmgItem.originalValue;
                    fobItem.cfgItemOriginalValue = cfgItem.originalValue;
                    fobItem.value = solrFOBItem.value.substring(solrFOBItem.value.indexOf('-') + 2);
                    fobItem.originalValue = solrFOBItem.value;
                    fobItem.visible = true;

                    fob.push(fobItem);
                });
            });
        });

        function compare(a, b) {
            if (a.value < b.value)
                return -1;
            if (a.value > b.value)
                return 1;
            return 0;
        }

        cmg.sort(compare);
        cfg.sort(compare);
        fob.sort(compare);


        return {
            cmgData: cmg,
            cfgData: cfg,
            fobData: fob
        };
    }

    var createSKUQueryString = function (sku) {
        return SOLRURL + '/select?q=sku%3A' + sku + '&fl=sku%2Cid%2Cproductid%2Cisn%2Cgroupid%2Cisactive&wt=json&indent=true'
    }


    var createUPCQueryString = function (upc) {
        //return SOLRURL + '/select?q=*%3A*&wt=json&indent=true&facet.sort=index&facet.field=isn&facet.field=productcode&facet=true&rows=0';
        return SOLRURL + '/select?q=id%3A' + upc + '&fl=productcode%2Csku%2Cid%2Cproductid%2Cisn%2Cgroupid%2Cisactive%2Ccolorcode%2Csize1code%2Csize2code&wt=json&indent=true'
    }

    var createBrandQueryString = function () {
        return SOLRURL + '/select?q=*%3A*&wt=json&indent=true&facet.sort=index&facet.field=facetbrand&facet=true&rows=0&facet.limit=-1';
    }

    var createProductHierarchyQueryString = function () {
        return SOLRURL + '/select?q=*%3A*&wt=json&indent=true&rows=0&facet=true&facet.limit=-1&facet.pivot=cmg,cfg,fob&facet.mincount=1&facet.limit=-1';
    }



    this.getParameterByName = function (name) {
        name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
        return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    }

    return {
        createFacetItem: createFacetItem,
        populateISNModel: populateISNModel,
        createSOLRQueryString: createSOLRQueryString,
        populateProductHierarchy: populateProductHierarchy,
        createUPCQueryString: createUPCQueryString,
        createSKUQueryString: createSKUQueryString,
        createBrandQueryString: createBrandQueryString,
        createProductHierarchyQueryString: createProductHierarchyQueryString
    };

}]);



appServices.factory('itemLocate', ['$http', 'sendSMTPErrorEmail', '$filter', function ($http, $sendSMTPErrorEmail, $filter) {
    var locate = function (zip, city, radius, state, id, store, success, error) {
        //var url = serviceURL + "/Item/ItemLocate";
        var url = serviceURL + "/Inventory/GetAvailableStores";
        //var url = "http://10.131.135.91:7080/Item/ItemLocate";
        if (angular.isString(city)) {
            city = city.toUpperCase();
        }

        var contract = {
            "GetAvailableStoresReq": {
                "_Radius": radius,
                "_Zipcode": (angular.isString(zip) && zip.trim().length > 0) ? zip.trim() : "",
                "_City": (angular.isString(city) && city.trim().length > 0) ? city.trim() : "",
                "_State": (angular.isString(state) && state.trim().length > 0) ? state.trim() : "",
                "_BOPISEligible": "N",
                "_ResultBy": "Store",
                "_ATSType": "Store",
                "ItemList": [
                  {
                      "_ItemID": id
                  }
                ]
            }
        };

        return $http.post(url, angular.toJson(contract)).then(function (response) {
            var stores;
            var results = [];

            if (response.data === null || !angular.isObject(response.data.GetAvailableStoresResp) || response.data.GetAvailableStoresResp.StoreList === null) {
                return [];
            } else if (angular.isArray(response.data.GetAvailableStoresResp.StoreList)) {
                stores = response.data.GetAvailableStoresResp.StoreList;
            } else if (angular.isObject(response.data.GetAvailableStoresResp.StoreList) && angular.isObject(response.data.GetAvailableStoresResp.StoreList.Store)) {
                stores = [];
                stores.push(response.data.GetAvailableStoresResp.StoreList.Store);
            } else {
                return [];
            }

            for (var i = 0; i < stores.length; i++) {
                results.push(
                    {
                        StoreName: stores[i]._StoreID + "-" + stores[i]._StoreName,
                        Address: stores[i].Address._Line1,
                        City: stores[i].Address._City,
                        Phone: $filter('phoneFormat')(stores[i]._MainPhone),
                        Inventory: stores[i].ItemList.Item._AvailableQty,
                        Distance: Number(stores[i]._Distance)
                    }
                    );
            }

            //order by distance
            return $filter('orderBy')(results, "Distance");


        }, function (response) {
            $sendSMTPErrorEmail(response, url);
            error(response.data.err);
        });
    };

      
    var getOrganizationList = function (store, success, error) {
        var url = serviceURL + "/Organization/GetStoreInfo";
        var contract = {
            GetStoreInfoReq: {
                Stores: [{
                    _StoreID: store
                }]
            }
        }
    
        return $http.post(url, angular.toJson(contract)).success(function (data) {
        }).error(function (data) {
            $sendSMTPErrorEmail(data, url);
            error(data.err);
        });
    };


    return {
        locate: locate,
        getOrganizationList: getOrganizationList

    };
}]);
appServices.factory('itemToLocate', function () {
    var currentItem = []
    function set(data) {
        savedItem = data;
    }
    function get() {
        return savedItem;
    }

    return {
        set: set,
        get: get
    }
});




appServices.factory('itemDetail', ['$http', 'itemInventory', 'loggerService', '$q', 'sendSMTPErrorEmail', 'POSService', 'loadingIconService', function ($http, $itemInventory, $loggerService, $q, $sendSMTPErrorEmail, $POSService, $loadingIconService) {
    return function (isnGroup, success, error, config) {

        var localConfig = { index: null, isBlocked: true };

        //config is object {start:0, end:20, index:2, isBlocked:false}
        //start and end have been depreciated
        //isBlocked tells whether to use the loading icon to block user interaction
        // OR if index is present, only that item with get priced.
        if (angular.isObject(config)) {
            if (angular.isDefined(config.index) && isFinite(parseInt(config.index))) {
                localConfig.index = parseInt(config.index);

                if (localConfig.index >= isnGroup.productList.length) {
                    localConfig.index = null;
                }
            }
            if (angular.isDefined(config.isBlocked) && !config.isBlocked) {
                localConfig.isBlocked = false;
            }
        }

        $loadingIconService.resetIcon(); //global variable for loading icon. Changing to never show icon.
        POSParamStore = $POSService.getPOSParameters();
        var url = serviceURL + "/Price/GetItemListPrices";
        //var url = "http://10.131.135.91:7080/Price/GetItemListPrices";
        var contract = {
        };
        contract.ItemListIn = {
        };
        contract.ItemListIn._CallingOrganizationCode = POSParamStore.storeNumber;
        contract.ItemListIn._PricingDate = new Date().toJSON();


        var Item = [];

        var mergeInventoryResult = function (group, ItemList) {
            if (!angular.isArray(ItemList)) {
                ItemList = [ItemList.Item];
            }
            group.isnGroupAvailableQty = 0;
            group.productList.forEach(function (product) {
                if (!angular.isDefined(product._AvailableQty)) {
                    product._AvailableQty = null;
                }

                for (var i = 0; i < ItemList.length; i = i + 1) {
                    if (ItemList[i]._ItemID == parseInt(product.sku)) {
                        product._AvailableQty = ItemList[i]._AvailableQty;
                        break;
                    }
                }
                group.isnGroupAvailableQty = group.isnGroupAvailableQty + parseInt(product._AvailableQty);
            });
        };

        var createInventoryContract = function (group) {
            var contract = {
            };
            contract.GetItemListForOrderingReq = {
            };
            var ItemList = [];
            var createItemInput = function (product) {
                var productId = {
                };
                productId._ItemID = '' + product.sku;
                ItemList.push(productId);
            };

            if (localConfig.index !== null) {
                createItemInput(group.productList[localConfig.index]);
            } else {
                group.productList.forEach(createItemInput);
            }

            contract.GetItemListForOrderingReq.ItemList = ItemList;
            return contract;
        };

        var createItemPricingItemInput = function (productItem) {
            var tempItem = {
                "_ItemID": productItem.sku,
                "_ItemKey": productItem.sku,
                "_OrganizationCode": "BONTON",
                "PrimaryInformation": {
                    "_IsAirShippingAllowed": "?",
                    "_ItemType": "REG",
                    "_UnitPrice": "0.00"
                },
                "ItemAliasList": {
                    "ItemAlias": [
                {
                    "_AliasName": "ACTIVE_UPC",
                    "_AliasValue": productItem.id
                }
                    ]
                },
                "Extn": {
                    "_ExtnSpecialHandlingCode": productItem.specialhandlingcode
                }
            };


            Item.push(tempItem);

        };

        if (localConfig.index !== null) {
            createItemPricingItemInput(isnGroup.productList[localConfig.index]);
        } else {
            isnGroup.productList.forEach(createItemPricingItemInput);
        }

        contract.ItemListIn.Item = Item;

        var promiseArray = [];

        promiseArray.push($http.post(url, angular.toJson(contract)).success(function (data) {
            var ItemListOut = angular.fromJson(data).ItemListOut;
            if (ItemListOut._ResponseCode === '9999') {
                error(data);
            }
            else {
                isnGroup.productList.forEach(function (productItem) {

                    if (!angular.isDefined(productItem.itemDetail)) {
                        productItem.itemDetail = null;
                    }

                    if (ItemListOut.Item.length != undefined) {
                        for (var i = 0; i < ItemListOut.Item.length; i++) {
                            var itemDetail = angular.fromJson(ItemListOut.Item[i]);
                            if (productItem.sku == parseInt(itemDetail._ItemID)) {
                                productItem.itemDetail = itemDetail;
                                productItem.itemDetail.ComputedPrice.Extn._ExtnSpecialHandlingCode = productItem.specialhandlingcode;
                                break;
                            }
                        }
                    }
                    else {
                        var itemDetail = angular.fromJson(ItemListOut.Item);
                        if (productItem.sku == parseInt(itemDetail._ItemID)) {
                            productItem.itemDetail = itemDetail;
                            productItem.itemDetail.ComputedPrice.Extn._ExtnSpecialHandlingCode = productItem.specialhandlingcode;
                        }
                    }
                });
            }
        }));
        if (!localConfig.isBlocked) {
            $loadingIconService.resetIcon();
        }

        var contract = createInventoryContract(isnGroup);
        promiseArray.push($itemInventory.getSafetyInventory(contract).success(function (inventoryResponse) {
            mergeInventoryResult(isnGroup, angular.fromJson(inventoryResponse).GetItemListForOrderingResp.ItemList)
        }));

        if (!localConfig.isBlocked) {
            $loadingIconService.resetIcon();
        }

        $q.all(promiseArray).then(function () {
            $loadingIconService.resetIcon();
            success(isnGroup)
        }, function (err) {
            $loadingIconService.resetIcon();
           // $sendSMTPErrorEmail(err, url);
            error(err);
        });

        if (!localConfig.isBlocked) {
            $loadingIconService.resetIcon();
        }

    }
}]);
    

appServices.factory('itemInventory', ['$http', 'sendSMTPErrorEmail', 'serviceArrayFix', function ($http, $sendSMTPErrorEmail, $serviceArrayFix) {
    var _getSafetyInventory = function (contract) {
        //var url = serviceURL + "/Inventory/GetItemListForOrdering"; //old inventory call
        var url = serviceURL + "/Inventory/GetAvailable"; //new Bikash optimized Inventory call
        return $http.post(url, angular.toJson(contract)).error(function (error) {
            $sendSMTPErrorEmail(error, url);
        });
    };

    var _getOnHandInventoryFromShipnodeAndSkuArray = function (shipNode, skuArray) {

        if (!(
                (angular.isString(shipNode) && (/^\d+$/i).test(shipNode.trim())) ||
                (Number.isInteger(shipNode) && (shipNode > -1))
            )
        ) {
            var defer = $q.defer();
            defer.reject('invalid shipNode value.');
            return defer.promise;
        }

        //Change number to string
        if (angular.isNumber(shipNode)) {
            shipNode = shipNode.toString();
        }

        //trim whitespace
        shipNode = shipNode.trim();

        //pad to 3 digits
        while (shipNode.length < 3) {
            shipNode = "0" + shipNode;
        }

        //strip if over 3 digits
        shipNode = shipNode.substring(0, 3);

        var skuOnHandDict = {};

        angular.forEach(skuArray, function (value) {
            skuOnHandDict[value] = 0;
        });
        //var url = "http://10.131.135.75:7080" + "/Inventory/getSupplyDetails";
        var url = serviceURL + "/Inventory/getSupplyDetails";

        var contract = {
            "GetSupplyDetailsReq": {
                "input": {
                    "getSupplyDetails": []
                }
            }
        };

        angular.forEach(skuOnHandDict, function (value, key) {
            contract.GetSupplyDetailsReq.input.getSupplyDetails.push({
                "_ItemID": "" + key,
                "_OrganizationCode": "BONTON",
                "_ProductClass": "NEW",
                "_ShipNode": shipNode,
                "_UnitOfMeasure": "EACH"
            });
        });

        return $http.post(url, angular.toJson(contract)).then(
                function (response) {
                    $serviceArrayFix(response.data);

                    var items = [];

                    if (response.data.GetSupplyDetailsRes && response.data.GetSupplyDetailsRes.Items) {
                        items = response.data.GetSupplyDetailsRes.Items.Item;
                    }

                    for (var i = 0; i < items.length; i++) {
                        var sku = items[i]._ItemID;
                        var inventoryOnHand = 0;
                        if(angular.isObject(items[i].ShipNodes) &&
                            angular.isArray(items[i].ShipNodes.ShipNode) &&
                            items[i].ShipNodes.ShipNode.length > 0 &&
                            angular.isObject(items[i].ShipNodes.ShipNode[0].Supplies) &&
                            angular.isObject(items[i].ShipNodes.ShipNode[0].Supplies.Supply) &&
                            angular.isString(items[i].ShipNodes.ShipNode[0].Supplies.Supply._TotalQuantity)) {

                            inventoryOnHand = parseInt(items[i].ShipNodes.ShipNode[0].Supplies.Supply._TotalQuantity);
                        }

                        skuOnHandDict[sku] = inventoryOnHand;
                    }

                    return skuOnHandDict;

                },
                function (response) {
                    $sendSMTPErrorEmail(response, url);
                    return response;
                });
    };

    return {
        getSafetyInventory: _getSafetyInventory,
        getOnHandInventoryFromShipnodeAndSkuArray: _getOnHandInventoryFromShipnodeAndSkuArray
    }
}]);


appServices.factory('itemSearch', ['$rootScope', '$http', '$cacheFactory', 'ISNModelHelper', '$q', 'sendSMTPErrorEmail', function ($rootScope, $http, $cacheFactory, ISNModelHelper, $q, $sendSMTPErrorEmail) {
    var cache = $cacheFactory('itemResult');

    var cleanSelectedFacets = function (selectedValues, responseFacetValues) {
        if (selectedValues && selectedValues == []) {
            return [];
        }
        //We don't want to keep selected facet values if the facets from SOLR doesn't have the selected values.
        selectedValues.forEach(function (selectedItem) {
            selectedItem.keep = false;
        });

        responseFacetValues.forEach(function (responseItem) {
            selectedValues.forEach(function (selectedItem) {
                if (responseItem.name === selectedItem.name) {
                    selectedItem.keep = true;
                };
            });
        });

        var fixedSelectedValues = [];
        selectedValues.forEach(function (selectedItem) {
            if (selectedItem.keep) {
                fixedSelectedValues.push(selectedItem);
            };
        });

        return fixedSelectedValues;
    };

    var search = function (queryParam, success, error) {
        var url = ISNModelHelper.createSOLRQueryString(queryParam);
        $http.get(url).success(function (data) {
            var response = ISNModelHelper.populateISNModel(data);
            response.itemSizes = ISNModelHelper.createFacetItem(data.facet_counts.facet_fields.facetitemsize);
            response.colors = ISNModelHelper.createFacetItem(data.facet_counts.facet_fields.facetcolor);
            response.brands = ISNModelHelper.createFacetItem(data.facet_counts.facet_fields.facetbrand);
            response.fobs = ISNModelHelper.createFacetItem(data.facet_counts.facet_fields.facetfob);
            queryParam.selectedItemSizes = cleanSelectedFacets(queryParam.selectedItemSizes, response.itemSizes);
            queryParam.selectedColors = cleanSelectedFacets(queryParam.selectedColors, response.colors);
            queryParam.selectedBrands = cleanSelectedFacets(queryParam.selectedBrands, response.brands);
            queryParam.selectedFOBs = cleanSelectedFacets(queryParam.selectedFOBs, response.fobs);

            if (!queryParam.nocache) {
                cache.put('cachedResult', response);
                cache.put('queryParam', queryParam);
            }

            response.isnGroup.forEach(function (group) {
                group.isnGroupAvailable = false;
                for (var i = 0; i < group.productList.length; i++) {
                    var product = group.productList[i];
                    if (!product.buyable) {
                        group.isnGroupAvailable = group.isnGroupAvailable || false;
                    } else {
                        group.isnGroupAvailable = group.isnGroupAvailable || true;
                    }
                }
            });
            success(response);
        }).error(function (data) {
            $sendSMTPErrorEmail(data, url);
            error(data.err);
        });
    };
    var MAX_NUMBER_OF_ROWS = 20;

    var retrieveSKU = function (sku) {
        var query = ISNModelHelper.createSKUQueryString(sku);
        return $http.get(query)
    }

    var searchItemBySKU = function (sku) {
        var query = SOLRURL + '/select?q=sku%3A"' + sku + '"+%0AAND+isactive%3A"Y"&wt=json';
        return $http.get(query).then(function (response) { return response.data.response.docs[0]; });
    }

    //returns dictionary of found skus
    var searchItemBySKUArray = function (skuArr) {
        if (!angular.isArray(skuArr) || skuArr.length < 1) {
            var defer = $q.defer();
            defer.resolve([]);
            return defer.promise;
        }

        var skuString = skuArr.join('"OR"');
        skuString = '"' + skuString + '"';
        skuString = encodeURIComponent(skuString);
        var query = SOLRURL + '/select?q=sku%3A(' + skuString + ')+AND+isactive%3A%22Y%22&start=0&rows=' + (skuArr.length + 10).toString() + '&wt=json';
        return $http.get(query).then(function (response) {
            var skuDict = {};
            angular.forEach(response.data.response.docs, function (value) {
                skuDict[value.sku] = value;
            });
            return skuDict;
        });
    }

    var retrieveUPC = function (upc) {
        var query = ISNModelHelper.createUPCQueryString(upc);
        return $http.get(query)
    }

    //We need productcode, and isn numbers for variaties.
    var retrieveIds = function (upc) {
        var defered = $q.defer();
        retrieveUPC(upc).then(function (result) {
            var docs = result.data.response.docs;
            if (docs.length == 0) {
                defered.reject("notfound");
            } else {
                if (docs[0].isactive == 'Y') {
                    defered.resolve(docs[0]);
                } else {
                    retrieveSKU(docs[0].sku).then(function (result) {
                        var docs = result.data.response.docs;
                        for (var i = 0; i < docs.length; i++) {
                            if (docs[i].isactive == 'Y') {
                                defered.resolve(docs[0]);
                            }
                        }
                    });
                }
            }
        });
        return defered.promise;
    }

    var searchUPCWithFilter = function (upc, filterBy, success, error) {
        retrieveIds(upc).then(function (doc) {
            var queryParameters = {}
            queryParameters.originalUPC = upc;
            queryParameters.isnAndProductCode = doc;
            queryParameters.start = 0;
            queryParameters.currentPage = 1;
            queryParameters.rows = -1;
            queryParameters.selectedBrands = [];
            queryParameters.selectedColors = [];
            queryParameters.selectedItemSizes = [];
            queryParameters.selectedFOBs = [];
            queryParameters.colorFilter = angular.isString(filterBy) && (filterBy === 'color') ? true : false; 
            queryParameters.sizeFilter = angular.isString(filterBy) && (filterBy === 'size') ? true : false; 

            search(queryParameters, success, error);
        }).catch(function (error) {
            var response = {};
            response.ngroups = 0;
            success(response);
        })
    };

    var searchUPC = function (upc, success, error) {
        searchUPCWithFilter(upc, null, success, error);
    }

    var searchUPCMoreColors = function (upc, success, error) {
        searchUPCWithFilter(upc, "size", success, error);
    }

    var searchUPCMoreSizes = function (upc, success, error) {
        searchUPCWithFilter(upc, "color", success, error);
    }

    var searchISNORProductCode = function (doc, success, error) {
        var queryParameters = {}
        queryParameters.isnAndProductCode = doc;
        queryParameters.start = 0;
        queryParameters.currentPage = 1;
        queryParameters.rows = -1;
        queryParameters.selectedBrands = [];
        queryParameters.selectedColors = [];
        queryParameters.selectedItemSizes = [];
        queryParameters.selectedFOBs = [];
        queryParameters.nocache = true;
        search(queryParameters, success, error);
    }

    var filter = function (queryParam, success, error) {
        search(queryParam, success, error);
    };

    var getCachedResult = function () {
        return cache.get('cachedResult');
    };

    var getCachedQueryParam = function () {
        return cache.get('queryParam');
    };

    var getSelectedISNGroup = function () {
        return cache.get("selectedISNGroup");
    };

    var setSelectedISNGroup = function (item) {
        cache.put('selectedISNGroup', item);
    }

    var retrieveBrands = function (success, error) {
        $http.get(ISNModelHelper.createBrandQueryString(), { cache: true }).success(function (data) {
            var brands = ISNModelHelper.createFacetItem(data.facet_counts.facet_fields.facetbrand);
            success(brands);
        }).error(function (data) {
            error(data.err);
        })
    };

    var retrieveProductHierarchy = function (success, error) {
        $http.get(ISNModelHelper.createProductHierarchyQueryString(), { cache: true }).success(function (data) {
            var temp = ISNModelHelper.populateProductHierarchy(angular.fromJson(data).facet_counts.facet_pivot["cmg,cfg,fob"]);
            success(temp);
        }).error(function (data) {
            error(data.err);
        })
    };

    var clearCachedResult = function () {
        cache.remove('cachedResult');
        cache.remove('queryParam');
    }

    var clearSelectedISNGroup = function () {
        cache.remove('selectedISNGroup');
    }
    var itemSearchFunctions = {
        search: search,
        searchUPC: searchUPC,
        searchUPCMoreColors: searchUPCMoreColors,
        searchUPCMoreSizes: searchUPCMoreSizes,
        searchISNORProductCode: searchISNORProductCode,
        filter: filter,
        getCachedResult: getCachedResult,
        getCachedQueryParam: getCachedQueryParam,
        setSelectedISNGroup: setSelectedISNGroup,
        getSelectedISNGroup: getSelectedISNGroup,
        clearSelectedISNGroup: clearSelectedISNGroup,
        retrieveBrands: retrieveBrands,
        retrieveProductHierarchy: retrieveProductHierarchy,
        clearCachedResult: clearCachedResult,
        MAX_NUMBER_OF_ROWS: MAX_NUMBER_OF_ROWS,
        searchItemBySKU: searchItemBySKU,
        searchItemBySKUArray: searchItemBySKUArray
    }

    return itemSearchFunctions;

}]);

appServices.factory('itemProperty', [function () {
    var _isItemActiveBuyable = function (solrItemDoc) {
        if (angular.isDefined(solrItemDoc.isactive) && (/^N$/i).test(solrItemDoc.isactive)) {
            return false;
        }

        if (!angular.isDefined(solrItemDoc.pricestatus)) {
            return true;
        } else {
            if ((/^(f|p)$/i).test(solrItemDoc.pricestatus)) {
                if (!angular.isDefined(solrItemDoc.buyable)) {
                    return false;
                } else {
                    return (/^true$/i).test(solrItemDoc.buyable) ? true : false;
                }
            } else {
                return true;
            }
        }

    };

    return { isItemActiveBuyable: _isItemActiveBuyable };
}]);
appServices.factory('bigTicketValidate', ['$http', 'errorObj', '$q', function ($http, errorObj, $q) {
    var cache = {};

    var _bigTicketPromise = function (zipCode) {
        //validate number/number string passed
        if (!isFinite(parseInt(zipCode))) {
            throw errorObj.newError('_bigTicketPromise():InputError', "Zip code must be 5 digits.",
                "_bigTicketPromise():InputError.  The zip code passed was: " + zipCode.toString(), 'InputNotNumber', 'WARN');
        }

        //parse as Int will remove any secondary zip code like 60605-1100.  From parseInt('60605-1100') => 60605
        zipCode = parseInt(zipCode).toString();

        if (zipCode.length !== 5) {
            throw errorObj.newError('_bigTicketPromise():InputError', "Zip code must be 5 digits only.",
                "_bigTicketPromise():InputError.  The zip code passed was too long.  Input was cleansed with code: parseInt(zipCode).toString(); Result:" + zipCode.toString(), 'InputLengthInvalid', 'WARN');
        }


        if (zipCode in cache) {

            return $q.when(
             {
                 data: {
                     "ValidateBigTicketZipCodeResp": {
                         "_isValid": cache[zipCode].toString(),
                         "_status": "00",
                         "_statusMessage": "Success"
                     }
                 }
             });
        } else {

            var input = {
                "ValidateBigTicketZipCodeReq": {
                    "_ZipCode": zipCode
                }
            }

            return $http.post(serviceURL.toString() + '/Utility/ValidateBigTicketZipCode', input).then(function (response) {
                if ('Success' === response.data.ValidateBigTicketZipCodeResp._statusMessage) {
                    cache[zipCode] = response.data.ValidateBigTicketZipCodeResp._isValid.toString();
                }

                return response;
            });
        }
    };

    return {

        bigTicketPromise: _bigTicketPromise

    };
}]);