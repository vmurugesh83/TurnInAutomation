angular.module('appServiceReprice', ['appUtilities'])
.factory('repriceService', ['$http', 'serviceArrayFix', '$q', 'loggerService', 'sendSMTPErrorEmail', function ($http, serviceArrayFix, $q, $loggerService, $sendSMTPErrorEmail) {
    var count = 0;

    var _addTempOrderLineKeys = function (orderCart) {
        //assign orderline dummy Keys of the form "RPQtemp1_1" (for orderline primeNo = 1 sublineNo = 1) only IF no orderline key exists
        for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
            var currentLine = orderCart.OrderLines.OrderLine[i];

            if (angular.isDefined(currentLine._OrderLineKey)) {
                //if orderline key is blank OR the orderline key starts with "RPQtemp" then overwrite with temp key
                if (currentLine._OrderLineKey.toString().trim() === "" || (/^RPQtemp/).test(currentLine._OrderLineKey.toString().trim())) {
                    currentLine._OrderLineKey = "RPQtemp" + currentLine._PrimeLineNo.toString() + "_" + currentLine._SubLineNo.toString();
                }
                //else leave orderline key alone

            } else {
                currentLine._OrderLineKey = "RPQtemp" + currentLine._PrimeLineNo.toString() + "_" + currentLine._SubLineNo.toString();
            }
        }

        return orderCart;
    };

    var _cleanOrderCart = function (copyCart) {
        for (var i = 0; i < copyCart.OrderLines.OrderLine.length; i++) {
            var currentLine = copyCart.OrderLines.OrderLine[i];
            delete currentLine.btDisplay;
            delete currentLine.btLogic;
            delete currentLine.CarrierServiceList;

            //TODO: remove this delete: "delete currentLine.LineTaxes.LineTax"  This is fix for broker error of TaxLines
            //delete currentLine.LineTaxes.LineTax
        }
        return copyCart;
    };

    var _copyAllAttributes = function (source, destination) {
        var keyArr = Object.keys(source);

        for (var i = 0; i < keyArr.length; i++) {
            //if key is attribute
            if ((/^_/).test(keyArr[i])) {

                destination[keyArr[i]] = source[keyArr[i]];
            }
        }
    };

    var _copyMissingAttributes = function (source, destination) {
        var keyArr = Object.keys(source);

        for (var i = 0; i < keyArr.length; i++) {
            //if key is attribute
            if ((/^_/).test(keyArr[i])) {

                //if key is NOT in destination object
                if (!angular.isDefined(destination[keyArr[i]])) {
                    destination[keyArr[i]] = source[keyArr[i]];
                }
            }
        }
    };

    var _mergeCart = function (orderCart, responseCart, lCount) {

        //order header level, copy all attributes to orderCart from responseCart
        _copyAllAttributes(responseCart, orderCart);

        //order level copy to orderCart from responseCart and OVERWRITE Errors object, Promotions and btResponses (when btResponses gets included in SKY response) 
        //TODO: add btResponses if it is added to response through Broker
        orderCart.Errors = responseCart.Errors;
        orderCart.Promotions = responseCart.Promotions;
        orderCart.btResponses = responseCart.btResponses;


        // REVERSE THIS -- add detail from orderCart to ANY matching orderLine key !!!!

        //---START code already reversed ---------------

        //first group responseCart orderline keys into object{ 'RPQtemp1_1': [orderline, orderline2], ...}
        var keyToResOrderLineMap = {};

        for (var i = 0; i < responseCart.OrderLines.OrderLine.length; i++) {
            var currentLine = responseCart.OrderLines.OrderLine[i];

            if (currentLine._OrderLineKey in keyToResOrderLineMap) {
                keyToResOrderLineMap[currentLine._OrderLineKey].push(currentLine);
            } else {
                keyToResOrderLineMap[currentLine._OrderLineKey] = [currentLine];
            }
        }

        for (var key in keyToResOrderLineMap) {

            var currentOrderCartOrderline = null;

            for (var p = 0; p < orderCart.OrderLines.OrderLine.length; p++) {
                if (orderCart.OrderLines.OrderLine[p]._OrderLineKey.toString() === key) {
                    currentOrderCartOrderline = orderCart.OrderLines.OrderLine[p];
                    break;
                }
            }

            if (currentOrderCartOrderline !== null) {
                for (var i = 0; i < keyToResOrderLineMap[key].length; i++) {
                    var currentRespOrderline = keyToResOrderLineMap[key][i];

                    //for each matching order line key copy CarrierServiceList over
                    currentRespOrderline.CarrierServiceList = angular.copy(currentOrderCartOrderline.CarrierServiceList);

                    //for each matching order line key copy any missing ATTRIBUTES in Item of orderCart over to responseCart
                    _copyMissingAttributes(currentOrderCartOrderline.Item, currentRespOrderline.Item);

                    //for each matching order line key copy any missing ATTRIBUTES in PrimaryInformation of orderCart over to responseCart
                    _copyMissingAttributes(currentOrderCartOrderline.ItemDetails.PrimaryInformation, currentRespOrderline.ItemDetails.PrimaryInformation);

                    //for each matching order line key copy Notes object from orderCart over to each responseCart line
                    currentRespOrderline.Notes = angular.copy(currentOrderCartOrderline.Notes);

                    //for each matching order line key copy PersonInfoShipTo object from orderCart over to each responseCart line
                    currentRespOrderline.PersonInfoShipTo = angular.copy(currentOrderCartOrderline.PersonInfoShipTo);

                    //for each matching order line key copy any missing orderline ATTRIBUTES from orderCart over to responseCart
                    _copyMissingAttributes(currentOrderCartOrderline, currentRespOrderline);

                    //for each matching order line key copy btDisplay & btLogic from orderCart over to responseCart
                    currentRespOrderline.btDisplay = angular.copy(currentOrderCartOrderline.btDisplay);
                    currentRespOrderline.btLogic = angular.copy(currentOrderCartOrderline.btLogic);
                }
            } else {
                $loggerService.log('Cart_Pricing:_merge ERROR. OrderLine key not found in orginal order cart: ' + key.toString() + ' reprice: ' + lCount);
            }
        }
        //---END code already reversed ---------------


        //now that I'm only using a copy we do not need to splice and push the orderlines, just add them on
        //REMOVE ALL orderlines from orderCart and then copy over (by assigning the reference) the responseCart's lines to the orderCart
        //       orderCart.OrderLines.OrderLine.splice(0, orderCart.OrderLines.OrderLine.length);

        //       for (var i = 0; i < responseCart.OrderLines.OrderLine.length; i++) {
        //           orderCart.OrderLines.OrderLine.push(responseCart.OrderLines.OrderLine[i]);
        //       }

        orderCart.OrderLines.OrderLine = responseCart.OrderLines.OrderLine;

        return orderCart;

    };

    var _removeTempOrderLineKeys = function (orderCart) {
        //remove dummy orderline keys
        for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
            var currentLine = orderCart.OrderLines.OrderLine[i];

            if ((/^RPQtemp/).test(currentLine._OrderLineKey.toString().trim())) {
                currentLine._OrderLineKey = "";
            }

            //TODO: remove work-around for Order Cart Pricing returning "_ExtnGiftRegistryNumber" instead of sky's "_ExtnGiftRegistryNo"
            if ('_ExtnGiftRegistryNumber' in currentLine.Extn) {
                currentLine.Extn._ExtnGiftRegistryNo = currentLine.Extn._ExtnGiftRegistryNumber;
            }
        }

        return orderCart;
    };

    var _sortOrderCartOrderlines = function (orderCart) {

        orderCart.OrderLines.OrderLine.sort(
                 function (a, b) {
                     var aPrimeLine = parseInt(a._PrimeLineNo);

                     var aSubLine;
                     if (a._SubLineNo && isFinite(parseInt(a._SubLineNo))) {
                         aSubLine = parseInt(a._SubLineNo);
                     } else {
                         aSubLine = 1;
                     }

                     var bPrimeLine = parseInt(b._PrimeLineNo);

                     var bSubLine;
                     if (b._SubLineNo && isFinite(parseInt(b._SubLineNo))) {
                         bSubLine = parseInt(b._SubLineNo);
                     } else {
                         bSubLine = 1;
                     }



                     if ((aPrimeLine - bPrimeLine) === 0) {
                         return aSubLine - bSubLine;
                     } else {
                         return aPrimeLine - bPrimeLine;
                     }
                 }
                );
    };

    //not using _renumberOrderLines() any more
    var _renumberOrderLines = function (orderCart) {
        _sortOrderCartOrderlines(orderCart);

        var primeLineNoCount = 1;
        var subLineNoCount = 1;

        var currentLinePrimeNo = -1;
        var primeLineGroups = [];
        var currentPrimeGroup = [];

        //group lines by prime line number
        for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
            //if orderline prime no is different OR this is the last orderline, then push group
            if ((parseInt(orderCart.OrderLines.OrderLine[i]._PrimeLineNo) !== currentLinePrimeNo)) {

                if (currentPrimeGroup.length > 0) {
                    primeLineGroups.push(currentPrimeGroup);
                }
                currentLinePrimeNo = parseInt(orderCart.OrderLines.OrderLine[i]._PrimeLineNo);
                currentPrimeGroup = [];
            }

            currentPrimeGroup.push(orderCart.OrderLines.OrderLine[i]);

            //if it is the last orderline then push the group on
            if (i === (orderCart.OrderLines.OrderLine.length - 1)) {
                if (currentPrimeGroup.length > 0) {
                    primeLineGroups.push(currentPrimeGroup);
                }
            }
        }


        for (var i = 0; i < primeLineGroups.length; i++) {
            subLineNoCount = 1;

            currentPrimeGroup = primeLineGroups[i];
            for (var q = 0; q < currentPrimeGroup.length; q++) {
                currentPrimeGroup[q]._PrimeLineNo = primeLineNoCount;
                currentPrimeGroup[q]._SubLineNo = subLineNoCount;

                ++subLineNoCount;
            }

            subLineNoCount = 1;
            ++primeLineNoCount;
        }

    };

    //Generate Timestamp in LocalISOString
    function toLocalISOString() {
        var d = new Date();
        var off = d.getTimezoneOffset();
        return new Date(d.getFullYear(), d.getMonth(), d.getDate(), d.getHours(), d.getMinutes() - off, d.getSeconds(), d.getMilliseconds()).toISOString();
    }

    var _reprice = function (orderCart) {
        if (angular.isDefined(orderCart.OrderLines) && angular.isDefined(orderCart.OrderLines.OrderLine) && orderCart.OrderLines.OrderLine.length > 0) {
            var localCount = ++count;

            $loggerService.log('reprice: ' + localCount);

            var copyCart = angular.copy(orderCart);

            _addTempOrderLineKeys(copyCart);

            var inputCart = angular.copy(copyCart);
            _cleanOrderCart(inputCart);

            var input = {
                "GetOrderCartPriceIn": inputCart
            };

            //update order time to now
            input.GetOrderCartPriceIn._OrderDate = toLocalISOString(); //gives "2015-06-09T21:43:49.868Z" in chrome and firefox

            //check for valid unitprice in orderline
            var validPrice = function (price) {

                if (!(angular.isString(price) || angular.isNumber(price)))
                {
                    return false;
                }

                var numericValue;

                if (angular.isString(price)) {
                    if (price.trim().length > 0)
                    {
                        numericValue = Number(price);
                    } else
                    {
                        return false;
                    }
                } else {

                    numericValue = price;
                }

                if (isFinite(numericValue) && (numericValue >= 0))
                {
                    return true;
                } 

                return false;
                
            };

            //fix invalid unitprice
            var resetLineUnitPrice = function (line)
            {
                if (line.LinePriceInfo._RetailPrice && validPrice(line.LinePriceInfo._RetailPrice))
                {
                    line.LinePriceInfo._UnitPrice = angular.copy(line.LinePriceInfo._RetailPrice);

                } else if (line.LinePriceInfo._ListPrice && validPrice(line.LinePriceInfo._ListPrice))
                {
                    line.LinePriceInfo._UnitPrice = angular.copy(line.LinePriceInfo._ListPrice);
                } else
                {
                    line.LinePriceInfo._UnitPrice = "0.00";
                }
            };

            //check for missing LinePriceInfo._UnitPrice on each orderline
            if (input.GetOrderCartPriceIn.OrderLines && input.GetOrderCartPriceIn.OrderLines.OrderLine) {
                for (var i = 0; i < input.GetOrderCartPriceIn.OrderLines.OrderLine.length; i++) {
                    var line = input.GetOrderCartPriceIn.OrderLines.OrderLine[i];

                    if(!validPrice(line.LinePriceInfo._UnitPrice))
                    {
                        resetLineUnitPrice(line);
                    }
                }
            }

            //in promise chaining, an error will go down the chain until the first chain that has an error function
            //  therefore errors will be passed to caller.
            var url = serviceURL.toString() + '/Price/GetOrderCartPrice';
            //var url = 'http://10.131.135.91:7080/Price/GetOrderCartPrice';

            return $http.post(url, input).then(function (response) {
                //promise success function

                serviceArrayFix(response.data);

                _mergeCart(copyCart, response.data.GetOrderCartPriceOut, localCount);
                _removeTempOrderLineKeys(copyCart);
                _sortOrderCartOrderlines(copyCart);
                response.data = copyCart;

                $loggerService.log('reprice: ' + localCount);
                return response;
            }, function (response) {
                $sendSMTPErrorEmail(response, url, input);
                return $q.reject(response);
            });

        } else {
            //return input as promise reject with Error "no orderlines found"
            var response = { data: angular.copy(orderCart) };
            response.data.Errors = {
                ErrorList: {
                    Error: [{
                        "_ErrorCode": "-1",
                        "_ErrorDescription": "No OrderLines found on input orderCart.",
                        "_ErrorRelatedMoreInfo": "",
                        "Attribute": {
                            "_Name": "",
                            "_Value": ""
                        },
                        "Stack": []
                    }]
                }
            };

            var defer = $q.defer();
            defer.reject(response);
            return defer.promise;
        }
    };


    return {
        reprice: _reprice,
        copyAllAttributes: _copyAllAttributes,
        copyMissingAttributes: _copyMissingAttributes

    };
}])
;