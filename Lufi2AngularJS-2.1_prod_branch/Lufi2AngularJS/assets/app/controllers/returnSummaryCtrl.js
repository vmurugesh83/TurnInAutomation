angular.module('returnSummary', ['ngTable'])
.controller('returnSummaryCtrl', ['$scope', 'order', '$filter', 'ngTableParams', 'numberOnlyKeyPressValidator', 'loggerService', '$state',
    function ($scope, $order, $filter, ngTableParams, $numberOnlyKeyPressValidator, $loggerService, $state) {

    var returnScroll = new IScroll('#returnsWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
    $scope.refreshReturnScroll = function () {
        setTimeout(function () {
            returnScroll.refresh();
        }, 500);
    };

    var exchangeScroll = new IScroll('#exchangesWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
    $scope.refreshExchangeScroll = function () {
        setTimeout(function () {
            exchangeScroll.refresh();
        }, 500);
    };

    $scope.returnTotal = 0;
    $scope.exchangeTotal = 0;
    $scope.refundTotal = 0;
    $scope.returnOrderlines = [];
    $scope.exchangeOrderlines = [];
    $scope.orderDetail = {};

    var _addOrderlineDetails = function (cart, currentObj) {

        var salesOrderline = null;

        for ( var i = 0; i < cart.OrderLines.OrderLine.length; i++ )
        {
            if (cart.OrderLines.OrderLine[i].Item._ItemID === currentObj.itemId )
            {
                salesOrderline = cart.OrderLines.OrderLine[i];
                break;
            }
        }

        if (salesOrderline !== null) { 
        
            currentObj.imageUrl = salesOrderline.btDisplay.defaultImageUrl;
            currentObj.upc = parseInt(salesOrderline.btDisplay.id);
            currentObj.description = salesOrderline.btDisplay.defaultItemDescription;

        } else {
            //get data from solr, except that is async //$order.addSolarData();
        }
    };

    var _init = function () {

        $scope.returnTotal = 0;
        $scope.exchangeTotal = 0;
        $scope.refundTotal = 0;
        $scope.returnOrderlines = [];
        $scope.exchangeOrderlines = [];
        $scope.orderDetail = {};

        $scope.orderDetail = $order.getCurrentOrderDetails();

        if (!angular.isDefined($scope.orderDetail) || $scope.orderDetail === null) {
            $scope.getOrderDetails();
        } else {
            if ($scope.orderDetail.ReturnOrders && $scope.orderDetail.ReturnOrders !== null && angular.isArray($scope.orderDetail.ReturnOrders.ReturnOrder)) {
                for (var i = 0; i < $scope.orderDetail.ReturnOrders.ReturnOrder.length; i++) {
                    
                    var currentReturn = $scope.orderDetail.ReturnOrders.ReturnOrder[i];
                    var currentOrderNo = currentReturn._OrderNo;
                    var currentOrderDate = currentReturn._OrderDate;
                    if (angular.isDefined(currentReturn.OverallTotals) && currentReturn.OverallTotals !== null &&
                        angular.isDefined(currentReturn.OverallTotals._GrandTotal) && isFinite(parseFloat(currentReturn.OverallTotals._GrandTotal))) {

                        $scope.returnTotal = $scope.returnTotal + parseFloat(currentReturn.OverallTotals._GrandTotal);
                        $scope.refundTotal = $scope.refundTotal + parseFloat(currentReturn.OverallTotals._GrandTotal);

                    }

                    if(angular.isDefined(currentReturn.OrderLines) && currentReturn.OrderLines !== null && angular.isArray(currentReturn.OrderLines.OrderLine)){
                        for (var p = 0; p < currentReturn.OrderLines.OrderLine.length; p++) {
                            var orderline = currentReturn.OrderLines.OrderLine[p];

                            var currentObj = {
                                orderNo: parseInt(currentOrderNo),
                                orderDate: currentOrderDate,
                                imageUrl: '',
                                upc: '',
                                itemId: orderline.Item._ItemID,
                                description: '',
                                returnQty: parseInt(orderline._OrderedQty),
                                unitPrice: parseFloat(orderline.LinePriceInfo._UnitPrice),
                                status: orderline._Status,
                                returnReason: orderline._ReturnReasonShortDesc,
                                customerKeep: ( orderline._ReturnReason && orderline._ReturnReason !== null && ( /^(10|11|12|13)$/ ).test( orderline._ReturnReason.toString().trim() ) ) ? 'Yes' : 'No'
                            };

                            //get remaining details from original sales order
                            _addOrderlineDetails($scope.orderDetail, currentObj);

                            $scope.returnOrderlines.push(currentObj);
                        }
                    }

                    //exchange orders
                    if (currentReturn.ExchangeOrders && currentReturn.ExchangeOrders !== null && angular.isArray(currentReturn.ExchangeOrders.ExchangeOrder)) {
                        for (var q = 0; q < currentReturn.ExchangeOrders.ExchangeOrder.length; q++) {

                            var currentExchange = currentReturn.ExchangeOrders.ExchangeOrder[q];
                            var currentExchangeOrderNo = currentExchange._OrderNo;
                            var currentExchangeOrderDate = currentExchange._OrderDate;

                            if (angular.isDefined(currentExchange.OverallTotals) && currentExchange.OverallTotals !== null &&
                                angular.isDefined(currentExchange.OverallTotals._GrandTotal) && isFinite(parseFloat(currentExchange.OverallTotals._GrandTotal))) {

                                $scope.exchangeTotal = $scope.exchangeTotal + parseFloat(currentExchange.OverallTotals._GrandTotal);
                                $scope.refundTotal = $scope.refundTotal - parseFloat(currentExchange.OverallTotals._GrandTotal);
                            }

                            if (angular.isDefined(currentExchange.OrderLines) && currentExchange.OrderLines !== null && angular.isArray(currentExchange.OrderLines.OrderLine)) {
                                for (var p = 0; p < currentExchange.OrderLines.OrderLine.length; p++) {
                                    var orderline = currentExchange.OrderLines.OrderLine[p];

                                    var currentObj = {
                                        orderNo: parseInt(currentExchangeOrderNo),
                                        orderDate: currentExchangeOrderDate,
                                        imageUrl: orderline.btDisplay.defaultImageUrl,
                                        upc: orderline.btDisplay.id,
                                        itemId: orderline.Item._ItemID,
                                        description: orderline.btDisplay.defaultItemDescription,
                                        returnQty: parseInt(orderline._OrderedQty),
                                        unitPrice: parseFloat(orderline.LinePriceInfo._UnitPrice),
                                        status: orderline._Status,
                                        returnReason: '',
                                        customerKeep: ''
                                    };

                                    //get remaining details from original sales order
                                    _addOrderlineDetails($scope.orderDetail, currentObj);

                                    $scope.exchangeOrderlines.push(currentObj);
                                }
                            }
                        }
                    }
                }
            }
            
            $scope.refundTotal = ($scope.refundTotal < 0) ? 0 : $scope.refundTotal;
            _setUpReturnsDisplayTable();
            _setUpExchangesDisplayTable();
        }
    };

    var _setUpReturnsDisplayTable = function () {
        $scope.returnsTable = new ngTableParams({
                    page: 1,            // show first page
                    count: 100000,          // count per page
                    sorting: {
                        _OrderDate: 'desc'     // initial sorting
                    }
                }, {
                    total: $scope.returnOrderlines.length, // length of data
                    counts: [],
                    getData: function ($defer, params) {
                        // use build-in angular filter
                        var orderedData = $scope.returnOrderlines;
                            //params.sorting() ?     $filter('orderBy')($scope.orders, params.orderBy()) :   $scope.orders;

                        $defer.resolve(orderedData);
                        $scope.refreshReturnScroll();
                    }
                });
    };

    var _setUpExchangesDisplayTable = function () {
        $scope.exchangesTable = new ngTableParams({
            page: 1,            // show first page
            count: 100000,          // count per page
            sorting: {
                _OrderDate: 'desc'     // initial sorting
            }
        }, {
            total: $scope.exchangeOrderlines.length, // length of data
            counts: [],
            getData: function ($defer, params) {
                // use build-in angular filter
                var orderedData = $scope.exchangeOrderlines;
                //params.sorting() ?     $filter('orderBy')($scope.orders, params.orderBy()) :   $scope.orders;

                $defer.resolve(orderedData);
                $scope.refreshExchangeScroll();
            }
        });
    };
    
    $scope.goToSalesDetail = function () { $state.go('orderDetail');};
    $scope.openOrderDetail = function (orderNo, orderType) {

        var docType = '0001';

        if(orderType && orderType==="return"){
            docType = '0003';
        }
        $order.setSelectedOrder({_OrderNo: orderNo, _DocumentType: docType});

        $scope.goToSalesDetail();
        
    };

    _init();
}]);

