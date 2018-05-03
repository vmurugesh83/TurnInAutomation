var appServices = angular.module( 'appServicespickUpInStore', [] );
appServices.factory('pickUpInStore', ['$http', '$cacheFactory', '$filter', 'sendSMTPErrorEmail', '$q', 'POSService', 'serviceArrayFix',
    function ($http, $cacheFactory, $filter, $sendSMTPErrorEmail, $q, $POSService, serviceArrayFix) {


    var printReceiptShipmentArray = []

    var batchprintrequest = {
        "GetRestockShipmentListReq": {
            "input": {
                "_ShipNode": "438",
                "_ShipmentType": "BOPIS",
                "_StatusDate": "2016-02-01T10:24:16-05:00",
            "Extn": {
                "_ExtnBatchPackSlipPrinted": "N"
            }
        }
    }
    }

    var packListrequest = {
        "GetRestockShipmentListReq": {
            "input": {
                "_ShipNode": "438",
                "_ShipmentType": "BOPIS",
                "_StatusDate": "2016-02-01T10:24:16-05:00"
            }
        }
    }

    var restockListrequest = {
        "GetRestockShipmentListReq": {
            "input": {
                "_ShipNode": "552",
                "_ShipmentType": "BOPIS",
                "_RequestType": "RESTOCK",
                "_RequestedDeliveryDate": "2016-09-08T00:00:00-05:00"
            }
        }
    };

    var shipmentsreadyforcustomerpickuprequest = {
        "GetRestockShipmentListReq": {
            "input": {
                    "_ShipNode": "438",
                    "_Status": "",
                    "_RequestType": "SHIPMENTS",
                    "_StatusDate": "2016-02-01T10:24:16-05:00",
                },
                "Extn": {
                    "_ExtnIsPicked": "Y"
                }
        }
    }

    var changeShipmentStatusrequest =   {
        "GetChangeShipmentStatusReq": {
            "input": {
                "_ShipmentKey": "100064754",
                "_IDType": "SSN",
                "_CustomerName":"Renuka Prasad",
                "_OrderNo": "980001196"
            }
        }
    }


    var changeShipmentrequest = {
        "ChangeShipmentReq": {
            "input": {
                "_Action": "Modify",
                "_ShipNode": "438",
                "_ShipmentKey": "100064719",
                "Extn": {
                    "_ExtnBatchPackSlipPrinted": "Y"
                }

            }
        }
    }
    var changeShipment = function (shipmentKey,shipNode) {
        changeShipmentrequest.ChangeShipmentReq.input._ShipNode = shipNode;
        changeShipmentrequest.ChangeShipmentReq.input._ShipmentKey = shipmentKey;
        //var url = "http://10.131.135.75:7080" + "/Fulfillment/changeShipment";
        console.debug(angular.toJson(changeShipmentrequest));
        var url = serviceURL + "/Fulfillment/changeShipment";
        return $http.post(url, angular.toJson(changeShipmentrequest)).then(function (data) {
            var response = angular.fromJson(data.data);
        },
          function (data) {
              $sendSMTPErrorEmail(data, url, changeShipmentrequest);
          });
    }
    

    var changeShipmentStatus = function (orderNo,shipmentKey,designee,IDtype,shipNode) {
        var url = serviceURL + "/Fulfillment/updateShipmentStatus";
        //var url = "http://10.131.135.75:7080/Fulfillment/updateShipmentStatus"
        
        changeShipmentStatusrequest.GetChangeShipmentStatusReq.input._ShipmentKey = shipmentKey;
        changeShipmentStatusrequest.GetChangeShipmentStatusReq.input._IDType = IDtype;
        changeShipmentStatusrequest.GetChangeShipmentStatusReq.input._CustomerName = designee;
        changeShipmentStatusrequest.GetChangeShipmentStatusReq.input._OrderNo = orderNo;

        return $http.post(url, angular.toJson(changeShipmentStatusrequest)).then(function (data) {
            var response = angular.fromJson(data.data);
        },
           function (data) {

               $sendSMTPErrorEmail(data, url, changeShipmentStatusrequest);
               return $q.reject(data);
           });

    }
    Date.prototype.formattedDate = function () {
        var yyyy = this.getFullYear();
        var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
        var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
        var hh = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
        var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
        var ss = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();
        return "".concat(yyyy).concat("-").concat(mm).concat("-").concat(dd).concat("T").concat(hh).concat(":").concat(min).concat(":").concat(ss);
    };


    function timezone() {
        var offset = new Date().getTimezoneOffset();
        var minutes = Math.abs(offset);
        var hours = Math.floor(minutes / 60);
        var prefix = offset < 0 ? "+" : "-";
        return prefix + hours;
    }
    Date.prototype.formattedTimeStamp = function () {
        var yyyy = this.getFullYear();
        var mm = this.getMonth() < 9 ? "0" + (this.getMonth() + 1) : (this.getMonth() + 1); // getMonth() is zero-based
        var dd = this.getDate() < 10 ? "0" + this.getDate() : this.getDate();
        var hh = this.getHours() < 10 ? "0" + this.getHours() : this.getHours();
        var min = this.getMinutes() < 10 ? "0" + this.getMinutes() : this.getMinutes();
        var ss = this.getSeconds() < 10 ? "0" + this.getSeconds() : this.getSeconds();
        return "".concat(yyyy).concat("-").concat(mm).concat("-").concat(dd).concat(" ").concat(hh).concat("-").concat(min).concat("-").concat(ss);
       // return "".concat(yyyy).concat("-").concat(mm).concat("-").concat(dd).concat("T").concat(hh).concat(":").concat(min).concat(":").concat(ss).concat(timezone());
        //var d = new Date(Date.UTC(yyyy, mm, dd,hh,min,ss));
        //return d;
    };

    var ExtnIsPicked = function (shipmentLines) {
            if (!angular.isDefined(shipmentLines)) {
            return;
        }
        shipmentLines.shipmentLine.forEach(function (shipmentLine) {
            if (shipmentLine.Extn._ExtnIsPicked == 'N') {
                return false;
            }
        });

        return true;
    };

    var getReadyforCustomerShipmentListArray = function (shipNode) {
        var url = serviceURL + "/Fulfillment/getRestockShipmentList";
        //var url = "http://10.131.135.75:7080/Fulfillment/getRestockShipmentList";
        shipmentsreadyforcustomerpickuprequest.GetRestockShipmentListReq.input._ShipNode = shipNode;
        var statusDate = new Date();
        var date = statusDate.formattedDate();
      
        shipmentsreadyforcustomerpickuprequest.GetRestockShipmentListReq.input._StatusDate = date;
        console.debug(angular.toJson(shipmentsreadyforcustomerpickuprequest));
        return $http.post(url, angular.toJson(shipmentsreadyforcustomerpickuprequest)).then(function (data) {
            var response = angular.fromJson(data.data);
            if (!angular.isArray(response.GetRestockShipmentListRes.Shipments.Shipment)) {
                response.GetRestockShipmentListRes.Shipments.Shipment = [response.GetRestockShipmentListRes.Shipments.Shipment];
            }
            var shipmentListArray = [];

            for (var i = 0; i < response.GetRestockShipmentListRes.Shipments.Shipment.length; i++) {
                var currentShipment = response.GetRestockShipmentListRes.Shipments.Shipment[i];
                if (angular.isDefined(currentShipment) && angular.isDefined(currentShipment.ShipmentLines) && !angular.isArray(currentShipment.ShipmentLines.ShipmentLine)) {
                    currentShipment.ShipmentLines.ShipmentLine = [currentShipment.ShipmentLines.ShipmentLine];
                }
                var totalQuantity = 0;
                if (angular.isDefined(currentShipment)) {
                    for (var j = 0; j < currentShipment.ShipmentLines.ShipmentLine.length; j++) {
                        var shipmentLine = currentShipment.ShipmentLines.ShipmentLine[j];
                        totalQuantity += parseInt(shipmentLine._Quantity);
                        currentShipment._totalQuantity = parseInt(totalQuantity);
                    }
                    var customerName = currentShipment.BillToAddress._FirstName + " " + currentShipment.BillToAddress._LastName;
                    currentShipment._customerName = customerName;
                    if (angular.isDefined(currentShipment.Instructions.Instruction) && angular.isArray(currentShipment.Instructions.Instruction)) {
                        for (var k = 0; k < currentShipment.Instructions.Instruction.length; k++) {
                            var instruction = currentShipment.Instructions.Instruction[k];
                            if (angular.isDefined(instruction) && instruction._InstructionType === 'Designee') {
                                if (instruction._InstructionText.toString().length > 0) {
                                    currentShipment._customerName = instruction._InstructionText;
                                }
                                break;
                            }
                        }
                    }

                    currentShipment._orderNumber = 0;
                    if (currentShipment.ShipmentLines.ShipmentLine.length > 0 && currentShipment.ShipmentLines.ShipmentLine[0]._OrderNo) {
                        currentShipment._orderNumber = parseInt(currentShipment.ShipmentLines.ShipmentLine[0]._OrderNo);
                    }

                    shipmentListArray.push(currentShipment);
                }
            }

            return shipmentListArray;
        },
           function (data) {
               $sendSMTPErrorEmail(data, url, shipmentsreadyforcustomerpickuprequest);
           });

    };

    //get the batch receipt print list //as well has just all unconfirmed (not-packed) but picked orders.
    var getBatchPrintShipmentListArray = function (shipNode) {
        var url = serviceURL + "/Fulfillment/getRestockShipmentList";
        //var url = "http://10.131.135.75:7080/Fulfillment/getRestockShipmentList";
        

        batchprintrequest.GetRestockShipmentListReq.input._ShipNode = shipNode;
        var statusDate = new Date();
        var date = statusDate.formattedDate();

        batchprintrequest.GetRestockShipmentListReq.input._StatusDate = date;


        batchprintrequest.GetRestockShipmentListReq.Extn = {
                "_ExtnBatchPackSlipPrinted": "N"
        };

        //tracking issue of possible wrong store numbers
        if (!(/^\d{3}$/).test(shipNode)) {

            $sendSMTPErrorEmail(null, url, batchprintrequest, "-- Request To Get Unprinted Batch Bopis Receipt Shipments has faulty store number. --");
        }

        console.debug(angular.toJson(batchprintrequest));
        return $http.post(url, angular.toJson(batchprintrequest)).then(function (data) {
            var response = angular.fromJson(data.data);
            var shipmentListArray = [];
            if (!angular.isDefined(response.GetRestockShipmentListRes.Shipments.Shipment)) {
                response.GetRestockShipmentListRes.Shipments.Shipment = [];
            }

            if (!angular.isArray(response.GetRestockShipmentListRes.Shipments.Shipment)) {
                response.GetRestockShipmentListRes.Shipments.Shipment = [response.GetRestockShipmentListRes.Shipments.Shipment];
            }
            var shipmentListMap = {};

            for (var i = 0; i < response.GetRestockShipmentListRes.Shipments.Shipment.length; i++) {
                var currentShipment = response.GetRestockShipmentListRes.Shipments.Shipment[i];

                //fix arrays
                serviceArrayFix(currentShipment);

                if (angular.isDefined(currentShipment.ShipmentLines) && !angular.isArray(currentShipment.ShipmentLines.ShipmentLine)) {
                    currentShipment.ShipmentLines.ShipmentLine = [currentShipment.ShipmentLines.ShipmentLine];
                }
                var totalQuantity = 0;

                for (var j = 0; j < currentShipment.ShipmentLines.ShipmentLine.length; j++) {
                    var shipmentLine = currentShipment.ShipmentLines.ShipmentLine[j];
                    if (angular.isDefined(shipmentLine.Extn) && shipmentLine.Extn._ExtnIsPicked == 'N') {
                        delete shipmentListMap[currentShipment._ShipmentNo];
                        break;
                    }

                    totalQuantity += parseInt(shipmentLine._Quantity);
                    currentShipment._totalQuantity = parseInt(totalQuantity);
                    shipmentListMap[currentShipment._ShipmentNo] = currentShipment;
                    
                }
            }

            return shipmentListMap;
        },
            function (data) {
                $sendSMTPErrorEmail(data, url, batchprintrequest);
            });
    };

        //get the batch receipt print list as well has just all unconfirmed (not-packed) but picked orders.
    var getUnConfirmedShipmentArray = function (shipNode) {
        var url = serviceURL + "/Fulfillment/getRestockShipmentList";
        //var url = "http://10.131.135.75:7080/Fulfillment/getRestockShipmentList";
        packListrequest.GetRestockShipmentListReq.input._ShipNode = shipNode;
        var statusDate = new Date();
        var date = statusDate.formattedDate();

        packListrequest.GetRestockShipmentListReq.input._StatusDate = date;

        return $http.post(url, angular.toJson(packListrequest)).then(function (data) {
            var response = angular.fromJson(data.data);
            //var shipmentListArray = [];

            //check it error response reject promise.
            if (!angular.isDefined(response) || !angular.isDefined(response.GetRestockShipmentListRes)) {
                return $q.reject();
            }

            //if no results, retunr zero results
            if (!angular.isDefined(response.GetRestockShipmentListRes.Shipments) || !angular.isDefined(response.GetRestockShipmentListRes.Shipments.Shipment)) {
                return {};
            }

            if (!angular.isArray(response.GetRestockShipmentListRes.Shipments.Shipment)) {
                response.GetRestockShipmentListRes.Shipments.Shipment = [response.GetRestockShipmentListRes.Shipments.Shipment];
            }
            var shipmentListMap = {};

            for (var i = 0; i < response.GetRestockShipmentListRes.Shipments.Shipment.length; i++) {
                var currentShipment = response.GetRestockShipmentListRes.Shipments.Shipment[i];

                //fix arrays
                serviceArrayFix(currentShipment);

                if (angular.isDefined(currentShipment.ShipmentLines) && !angular.isArray(currentShipment.ShipmentLines.ShipmentLine)) {
                    currentShipment.ShipmentLines.ShipmentLine = [currentShipment.ShipmentLines.ShipmentLine];
                }
                var totalQuantity = 0;

                for (var j = 0; j < currentShipment.ShipmentLines.ShipmentLine.length; j++) {
                    var shipmentLine = currentShipment.ShipmentLines.ShipmentLine[j];
                    if (angular.isDefined(shipmentLine.Extn) && shipmentLine.Extn._ExtnIsPicked == 'N') {
                        delete shipmentListMap[currentShipment._ShipmentNo];
                        break;
                    }

                    totalQuantity += parseInt(shipmentLine._Quantity);
                    currentShipment._totalQuantity = parseInt(totalQuantity);
                    shipmentListMap[currentShipment._ShipmentNo] = currentShipment;

                }
            }

            return shipmentListMap;
        },
            function (data) {
                $sendSMTPErrorEmail(data, url, packListrequest);
            });
    };

        // get a list of shipments that need to be Restocked on the iPad
    var _getListOfShipmentToRestock = function (shipNode) {
        var url = serviceURL + "/Fulfillment/getRestockShipmentList";
        //var url = "http://10.131.135.75:7080/Fulfillment/getRestockShipmentList";
        restockListrequest.GetRestockShipmentListReq.input._ShipNode = shipNode;
        var statusDate = new Date();
        var date = statusDate.formattedDate();

        restockListrequest.GetRestockShipmentListReq.input._RequestedDeliveryDate = date;

        return $http.post(url, angular.toJson(restockListrequest)).then(function (response) {
            var result = [];
            var data = response.data;

            if (angular.isObject(data.GetRestockShipmentListRes) && angular.isObject(data.GetRestockShipmentListRes.Shipments) && data.GetRestockShipmentListRes.Shipments.Shipment !== undefined) {
                if (!angular.isArray(data.GetRestockShipmentListRes.Shipments.Shipment)) {
                    data.GetRestockShipmentListRes.Shipments.Shipment = [data.GetRestockShipmentListRes.Shipments.Shipment];
                }
                result = data.GetRestockShipmentListRes.Shipments.Shipment;
                serviceArrayFix(result);
            }


            return result;
        });
    };

    return {
        getBatchPrintShipmentListArray: function (shipNode) { return getBatchPrintShipmentListArray(shipNode); },
        getUnConfirmedShipmentList: function (shipNode) { return getUnConfirmedShipmentArray(shipNode); },
        getReadyforCustomerShipmentListArray: getReadyforCustomerShipmentListArray,
        changeShipmentStatus: changeShipmentStatus,
        changeShipment: changeShipment,
        getListOfShipmentToRestock: function (shipNode) { return _getListOfShipmentToRestock(shipNode); }
    }
 
}]);
appServices.factory('pickConfirmService', ['$http', '$q', 'serviceArrayFix', 'itemSearch', 'itemInventory', 'sendSMTPErrorEmail', '$filter', 'pickUpInStore',
    function ($http, $q, serviceArrayFix, $itemSearch, $itemInventory, $sendSMTPErrorEmail, $filter, $pickUpInStore) {
    var _getUpickedShipmentLines = function (shipNode) {
        if ( !(
                (angular.isString(shipNode) &&  (/^\d+$/i).test(shipNode.trim())) ||
                (Number.isInteger(shipNode) && (shipNode > -1))
            )
        )
        {
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

        var request = {
            "GetShipmentLineListReq": {
                "input": {
                    "Shipment": {
                        "_ShipNode": shipNode,
                        "_Status": "1100.100"
                    },
                    "Extn": {
                        "_ExtnIsPicked": "N"
                    }
                }
            }
        };

        var url = serviceURL + "/Fulfillment/getShipmentLineList";
        return $http.post(url, angular.toJson(request)).then(function (response) {
            serviceArrayFix(response.data);

            //if there are shipmentLines return them else return empty array.
            if (response.data.GetShipmentLineListResp.ShipmentLines.ShipmentLine) {
                return response.data.GetShipmentLineListResp.ShipmentLines.ShipmentLine;
            } else {
                return [];
            }
        },
          function (data) {
              $sendSMTPErrorEmail(data, url, request);
          });
    }

    //pass shipment line to return objects to hold picklist display data
    var _createDisplayDataObject = function (shipmentLine) {
        return {
            "Fob": "",
            "Brand": "",
            "Upc": "",
            "Description": shipmentLine._ItemDesc.trim(),
            "QtyInStore": 0,
            "QtyToPick": parseInt(shipmentLine._Quantity),
            "QtyPicked": "",
            "ImageURL":"",
            "OrderNo": shipmentLine._OrderNo,
            "_skuInt": parseInt(shipmentLine._ItemID),
            "_shipmentLine": shipmentLine,
            "_solrData": {},
            "_sorting": {
                "isFirstOfUpc": false, //flag for printing display to mark the first row of a group of upcs
                "isEvenFob":false //flag for printing display to mark fob as even or odd in ordered list
            }
        };
    };

    var _retrievePickingDisplayDetails = function (shipNode, shipmentLineArr,isGetStoreNodeOnhandInventory) {

        //input checks
        if (!angular.isArray(shipmentLineArr)) {
            var defer1 = $q.defer();
            defer1.resolve([]);
            return defer1.promise;
        }

        if (!(
                (angular.isString(shipNode) && (/^\d+$/i).test(shipNode.trim())) ||
                (Number.isInteger(shipNode) && (shipNode > -1))
            )
        ) {
            var defer2 = $q.defer();
            defer2.resolve([]);
            return defer2.promise;
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

        var defer = $q.defer();

        //get a distinct set of skus in the shipment lines
        var setOfSkus = {};

        for (var i = 0; i < shipmentLineArr.length; i++){
            var line = shipmentLineArr[i];
            setOfSkus[line._ItemID] = line._ItemID;
        }

        var setOfSkusArray = [];

        angular.forEach(setOfSkus, function (value) {
            setOfSkusArray.push(value);
        });

        var arrayOfPromises = [];


        var displayDataArray = [];

        angular.forEach(shipmentLineArr, function (value) {
            var currentDataDisplayObj = _createDisplayDataObject(value);

            //filter out lines with zero quantity to Pick

            if (currentDataDisplayObj.QtyToPick > 0){
                displayDataArray.push(currentDataDisplayObj);
            }
        });

        //get solr data
        arrayOfPromises.push($itemSearch.searchItemBySKUArray(angular.copy(setOfSkusArray)).then(
                function (skuDict) {
                    angular.forEach(displayDataArray, function (displayDataObject) {
                        var solrData = skuDict[displayDataObject._skuInt];

                        var fob = solrData.fob;

                        if ((/\s*\d+\s*-.*/).test(fob)) {
                            var index = fob.indexOf("-");
                            if (index > -1 && (index != fob.length -1)) {
                                fob = fob.substring(index + 1).trim();
                            }
                        }

                        displayDataObject.Fob = fob; //has numbers in front
                        displayDataObject.Brand = solrData.brandlongdesc;
                        displayDataObject.Upc = solrData.id;
                        var webCatDescript = "";
                        if (angular.isString(solrData.productname)) {
                            webCatDescript = solrData.productname.trim();

                            var sizeDesc = angular.isString(solrData.sizedc) ? solrData.sizedc.trim() : "";
                            var color = angular.isString(solrData.colorlongdesc) ? solrData.colorlongdesc.trim() : "";

                            if (webCatDescript.length > 0) {

                                webCatDescript += color.length > 0 ? " (" + $filter('titlecase')(color)  + ")" : "";
                                webCatDescript += sizeDesc.length > 0 ? " Size: " + sizeDesc : "";
                            }
                        }
                        displayDataObject.Description = webCatDescript.length > 0 ? webCatDescript : displayDataObject.Description; // if productname exist use it else keep Sterling name
                        displayDataObject._solrData = solrData;

                        //construct image URL
                        if (angular.isString(solrData.colorimageid) && (solrData.colorimageid.trim() !== "")) {
                            displayDataObject.ImageURL = serviceURL.toString() + "/image/BonTon/" + solrData.colorimageid.trim();
                        } else if (angular.isDefined(solrData.imageid) && (solrData.imageid.toString().trim() !== "")) {
                            displayDataObject.ImageURL = serviceURL.toString() + "/image/BonTon/" + solrData.imageid.toString().trim();
                        } else {
                            displayDataObject.ImageURL = "/assets/images/NotAvailable.jpg";
                        }
                    });
                }
            ));
        if (isGetStoreNodeOnhandInventory === true) {
        //get on-hand inventory for store / item
        arrayOfPromises.push($itemInventory.getOnHandInventoryFromShipnodeAndSkuArray(shipNode, angular.copy(setOfSkusArray)).then(
            function (skuDict) {
                angular.forEach(displayDataArray, function (displayDataObject) {

                    displayDataObject.QtyInStore = skuDict[displayDataObject._shipmentLine._ItemID] ? skuDict[displayDataObject._shipmentLine._ItemID] : 0;
                });
            }
            ));
        }

        $q.all(arrayOfPromises).then(function () {
            defer.resolve(displayDataArray);
        });

        return defer.promise;
    }


    var _getPickingShipmentSummaryDisplay = function (shipmentLineArr) {

        //input checks
        if (!angular.isArray(shipmentLineArr)) {
            return [];
        }

        var dicOfOrders = {};

        for (var i = 0; i < shipmentLineArr.length; i++) {
            if (angular.isObject(shipmentLineArr[i].Extn) && shipmentLineArr[i].Extn._ExtnIsPicked === 'N') {

                var curOrderNo = shipmentLineArr[i]._OrderNo;
            
                if (dicOfOrders[curOrderNo]) {
                    dicOfOrders[curOrderNo]._totalQuantity = dicOfOrders[curOrderNo]._totalQuantity + Number(shipmentLineArr[i]._Quantity);
                } else {
                    dicOfOrders[curOrderNo] = {};
                    dicOfOrders[curOrderNo]._totalQuantity = Number(shipmentLineArr[i]._Quantity);

                    if (!isFinite(dicOfOrders[curOrderNo]._totalQuantity)) {
                        dicOfOrders[curOrderNo]._totalQuantity = 1;
                    }
                }

            }
        }

        var arrayReturn = [];
        angular.forEach(dicOfOrders, function (value, key) {
            arrayReturn.push({_orderNo: "" + key, _totalQuantity: "" + value._totalQuantity});
        });

        return arrayReturn;
    }

    var _fobSortShipmentLines = function (shipmentLineArrWithSolr) {
        var sortedList = $filter('orderBy')(shipmentLineArrWithSolr, ["Fob", "Brand", "Upc", "OrderNo", "_shipmentLine._ShipmentLineKey"]);

        var currentFob = "";
        var fobIsEven = false;

        var currentSku = 0;

        for (var i = 0; i < sortedList.length; i++) {
            var item = sortedList[i];

            item._sorting.isEvenFob = fobIsEven;
            item._sorting.isFirstOfUpc = false;

            if (item.Fob !== currentFob) {
                currentFob = item.Fob;
                fobIsEven = !fobIsEven;
                item._sorting.isEvenFob = fobIsEven;
            }

            if(item._skuInt !== currentSku){
                currentSku = item._skuInt;
                item._sorting.isFirstOfUpc = true;
            }
        }

        return sortedList;
    };

    //helper for _validateShipmentLinesForPicking returns {Input:..} object for API array
    var _constructGetMultipleShipmentListsReqInputObject = function (shipmentLineKeyStr) {
        var input = {
            "Input": {
                "ShipmentLine": {
                    "_ShipmentLineKey": shipmentLineKeyStr
                }
            },
            "Template": {
                "ShipmentLines": {
                    "ShipmentLine": {
                        "Extn": ""
                    }
                }
            },
            "_Name": "getShipmentLineList"
        };

        return input;
    };

    // take in an array of shipmentLine keys and return promise that successful post will return dictionary of (shipmentLine key , isPicked = (Y |N) ) 
    var _validateShipmentLinesForPicking = function (shipmentLineKeysArray) {
        var newGetMultipleShipmentListsReq = {
            "GetMultipleShipmentListsReq": {
                "input": {
                    "API": []
                }
            }
        };

        angular.forEach(shipmentLineKeysArray, function (value) {

            if (angular.isString(value) && value.length > 0){
                newGetMultipleShipmentListsReq.GetMultipleShipmentListsReq.input.API.push(_constructGetMultipleShipmentListsReqInputObject(value));
            }
        });


        var url = serviceURL + "/Fulfillment/getMultipleShipmentLists";
        return $http.post(url, angular.toJson(newGetMultipleShipmentListsReq)).then(function (response) {
            serviceArrayFix(response.data);

            var resultDict = {};

            if(!response.data.GetMultipleShipmentListsRes || !response.data.GetMultipleShipmentListsRes.Output){
                return resultDict;
            }

            //fix Output to alway be an array
            if(!angular.isArray(response.data.GetMultipleShipmentListsRes.Output)){
                var arrayFix = [];

                if(response.data.GetMultipleShipmentListsRes.Output.ShipmentLines){
                    arrayFix.push(response.data.GetMultipleShipmentListsRes.Output);
                }

                response.data.GetMultipleShipmentListsRes.Output = arrayFix;
            }

            //add each shipment line to dictionary of (shipmentLine key , isPicked = (Y |N) ) 
            for(var i = 0; i < response.data.GetMultipleShipmentListsRes.Output.length; i++){
                var shipment = response.data.GetMultipleShipmentListsRes.Output[i];
                if(shipment.ShipmentLines && angular.isArray(shipment.ShipmentLines.ShipmentLine)){
                    for(var k=0; k < shipment.ShipmentLines.ShipmentLine.length; k++){
                        var shipmentLine = shipment.ShipmentLines.ShipmentLine[k];
                        if(angular.isString(shipmentLine._ShipmentLineKey) && angular.isString(shipmentLine.Extn._ExtnIsPicked)){
                            resultDict[shipmentLine._ShipmentLineKey] = shipmentLine.Extn._ExtnIsPicked;
                        }
                    }
                }
            }

            return resultDict;
            
        },
          function (data) {
              $sendSMTPErrorEmail(data, url, newGetMultipleShipmentListsReq);
          });

    };

    //helper for _confirmPicks returns {Input:..} object for API array OR NULL
    // input is object : { QtyPicked : "0", "_shipmentLine": shipmentLine }
    var _constructChangeMultipleShipmentsReqInputObject = function (shipmentLineTuple) {

        var pickedQty = Number.NaN;
        var originalPickQty = Number.NaN;
        var shortQty = 0;

        pickedQty = parseInt(shipmentLineTuple.QtyPicked);

        if (angular.isObject(shipmentLineTuple._shipmentLine) && shipmentLineTuple._shipmentLine._Quantity !== null && shipmentLineTuple._shipmentLine._Quantity !== undefined) {
            originalPickQty = parseInt(shipmentLineTuple._shipmentLine._Quantity);
        }

        if (!isFinite(pickedQty) || !isFinite(originalPickQty) || pickedQty < 0 || pickedQty > originalPickQty || originalPickQty < 0 ) {
            return null;
        }

        //get shorted amount
        shortQty = originalPickQty - pickedQty;


        var result = {
            "Input": {
                "Shipment": {
                    "ShipmentLines": {
                        "ShipmentLine": {
                            "Extn": {
                                "_ExtnIsPicked": "Y"
                            },
                            "_BackroomPickedQuantity": pickedQty.toString(),
                            "_Quantity": pickedQty.toString(),
                            "_ShipmentLineKey": shipmentLineTuple._shipmentLine._ShipmentLineKey,
                            "_ShortageQty": shortQty.toString()
                        }
                    },
                    "_CancelRemovedQuantity": "Y",
                    "_CancelShipmentOnZeroTotalQuantity": "Y",
                    "_OverrideModificationRules": "Y",
                    "_ShipmentKey": shipmentLineTuple._shipmentLine._ShipmentKey
                }
            },
            "Template": {
                "Shipment": ""
            },
            "_Name": "changeShipment"
        };

        return result;
    };

    //input: array of objects that have attributs : { QtyPicked : "0", "_shipmentLine": shipmentLine }
    var _confirmPicks = function (arrayOfShipmentLinesWithPickedQty) {

        var defer = $q.defer();

        if(!angular.isArray(arrayOfShipmentLinesWithPickedQty) || arrayOfShipmentLinesWithPickedQty.length < 1){
            defer.resolve("OK");
            return defer.promise;
        }

        var newChangeMultipleShipmentsReq = {
            "ChangeMultipleShipmentsReq": {
                "input": {
                    "API": []
                }
            }
        };

        angular.forEach(arrayOfShipmentLinesWithPickedQty, function (value) {

            var newChangeObj = _constructChangeMultipleShipmentsReqInputObject(value);

            //if I cannot construct input send error
            if (newChangeObj === null) {

                $sendSMTPErrorEmail("Error in Lufi PickList Confirm for ShipmentLine", "", value);

                defer.reject('Invalid Input');
                return defer.promise;
            }else {
                newChangeMultipleShipmentsReq.ChangeMultipleShipmentsReq.input.API.push(newChangeObj);
            }
        });


        var url = serviceURL + "/Fulfillment/changeMultipleShipments";

        $http.post(url, angular.toJson(newChangeMultipleShipmentsReq)).then(function (response) {
            var data = response.data;
            
            //check for successful response from Sterling
            if (angular.isObject(data) &&
                angular.isObject(data.ChangeMultipleShipmentsRes) &&
                angular.isString(data.ChangeMultipleShipmentsRes._Name) &&
                angular.isObject(data.ChangeMultipleShipmentsRes.Output)
            ){

                defer.resolve("OK");
                
            } else {
                $sendSMTPErrorEmail(data, url, newChangeMultipleShipmentsReq);
                defer.reject('Confirm failed');
            }

        },
          function (data) {
              $sendSMTPErrorEmail(data, url, newChangeMultipleShipmentsReq);
              defer.reject('Confirm failed');
          });

        return defer.promise;
    };

    // get all Picked Shipments from Ship Node that are waiting to be confirmed  (doesn't matter if is has been printed)
    var _getUpickedShipmentLines = function (shipNode) {
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

        var request = {
            "GetShipmentLineListReq": {
                "input": {
                    "Shipment": {
                        "_ShipNode": shipNode,
                        "_Status": "1100.100"
                    },
                    "Extn": {
                        "_ExtnIsPicked": "N"
                    }
                }
            }
        };

        var url = serviceURL + "/Fulfillment/getShipmentLineList";
        return $http.post(url, angular.toJson(request)).then(function (response) {
            serviceArrayFix(response.data);

            //if there are shipmentLines return them else return empty array.
            if (response.data.GetShipmentLineListResp.ShipmentLines.ShipmentLine) {
                return response.data.GetShipmentLineListResp.ShipmentLines.ShipmentLine;
            } else {
                return [];
            }
        },
          function (data) {
              $sendSMTPErrorEmail(data, url, request);
          });
    }


        // pack confirm Shipment. Input is Shipment object with {_ShipmentKey:"13241234125"}
    var _confirmShipmentPacking = function (shipment) {

        var defer = $q.defer();
        var shipmentKey = null;

        if (shipment && shipment._ShipmentKey) {
            shipmentKey = shipment._ShipmentKey.toString().trim();
        }

        if (shipmentKey === null || shipmentKey.length < 1) {
            defer.reject();
            return defer.promise;
        }

        var newConfirmShipmentReq = {
            "ConfirmShipmentReq": {
                "input": {
                    "_Action": "Modify",
                    "_ShipmentKey": shipmentKey
                }
            }
        };


        var url = serviceURL + "/Fulfillment/confirmShipment";

        $http.post(url, angular.toJson(newConfirmShipmentReq)).then(function (response) {
            var data = response.data;

            //check for successful response from Sterling
            if (angular.isObject(data) &&
                angular.isObject(data.ConfirmShipmentResp) &&
                angular.isObject(data.ConfirmShipmentResp.Shipment) &&
                angular.isString(data.ConfirmShipmentResp.Shipment._ShipmentKey) &&
                data.ConfirmShipmentResp.Shipment._ShipmentKey.trim() === shipmentKey
            ) {

                defer.resolve("OK");

            } else {
                $sendSMTPErrorEmail(data, url, newConfirmShipmentReq);
                defer.reject('Confirm failed');
            }

        },
          function (data) {
              $sendSMTPErrorEmail(data, url, newConfirmShipmentReq);
              defer.reject('Confirm failed');
          });

        return defer.promise;

    }

    return {
        getUnpickedShipmentLines: function (shipNode) { return _getUpickedShipmentLines(shipNode); },
        retrievePickingDisplayDetails: function (shipNode, shipmentLineArr) { return _retrievePickingDisplayDetails(shipNode, shipmentLineArr, true); },
        retrievePackDisplayDetails: function (shipmentLineArr) { return _retrievePickingDisplayDetails("001", shipmentLineArr, false); },
        getPickingShipmentSummaryDisplay: function (shipmentLineArr) { return _getPickingShipmentSummaryDisplay(shipmentLineArr); },
        fobSortShipmentLines: function (shipmentLineArrWithSolr) { return _fobSortShipmentLines(shipmentLineArrWithSolr); },
        validateShipmentLinesForPicking: function (shipmentLineKeysArray) { return _validateShipmentLinesForPicking(shipmentLineKeysArray); },
        confirmPicks: function (arrayOfShipmentLinesWithPickedQty) { return _confirmPicks(arrayOfShipmentLinesWithPickedQty); },
        getUnConfirmedShipmentList: function (shipNode) { return $pickUpInStore.getUnConfirmedShipmentList(shipNode); },
        confirmShipmentPacking: function (currentShipment) { return _confirmShipmentPacking(currentShipment);}
    };
}]);