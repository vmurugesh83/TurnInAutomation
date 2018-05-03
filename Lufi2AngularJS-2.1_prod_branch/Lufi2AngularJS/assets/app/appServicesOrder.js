var appServices = angular.module('appServicesOrder', []);
appServices.factory('order', ['$http', '$cacheFactory', 'serviceArrayFix', 'itemSearch', '$q', 'loggerService', 'sendSMTPErrorEmail', 'btProp','POSService', function ($http, $cacheFactory, serviceArrayFix, $itemSearch, $q, $loggerService, $sendSMTPErrorEmail, $btProp,$POSService) {

    var cache = $cacheFactory("orderSearch");
    cache.put("orderSearchResult", []);
    cache.put("orderSearchParam", []);

    var getOrderList = function (searchParam, success, error) {
        var url = serviceURL + "/Order/GetOrderListJSON";
        //var url = 'http://10.131.135.91:7080/Order/GetOrderListJSON';
        cache.put("orderSearchParam", searchParam);
        $http.post(url, angular.toJson(searchParam)).success(function (data) {
            serviceArrayFix(data);
            var result = data.GetSterlingOrderListResp.Order;
            cache.put("orderSearchResult", result);
            clearSelectedOrder();
            success(result);
        }).error(function (data) {
            $sendSMTPErrorEmail(data, url);
            error(data);
        })
    }
    var addSolarData = function (orderline) {
        return $itemSearch.searchItemBySKU(orderline.Item._ItemID).then(function (data) {
            if (data) {
                orderline.btDisplay = data;
            } else {
                orderline.btDisplay = {};
            }

            //set product or isn description 
            if (orderline.btDisplay.productname && (orderline.btDisplay.productname.trim() !== "")) {
                orderline.btDisplay.defaultItemDescription = orderline.btDisplay.productname;
            } else if (orderline.btDisplay.isnlongdesc && (orderline.btDisplay.isnlongdesc.trim() !== "")) {
                orderline.btDisplay.defaultItemDescription = orderline.btDisplay.isnlongdesc;
            } else {
                orderline.btDisplay.defaultItemDescription = orderline.ItemDetails.PrimaryInformation._ShortDescription;
            }

            if (angular.isDefined(orderline.btDisplay.imageid) && (orderline.btDisplay.imageid.toString().trim() !== "")) {
                orderline.btDisplay.defaultImageUrl = serviceURL.toString() + "/image/BonTon/" + orderline.btDisplay.imageid.toString().trim();
            } else {
                orderline.btDisplay.defaultImageUrl = "/assets/images/NotAvailable.jpg";
            }
            if (!angular.isDefined(orderline.btLogic)) {
                orderline.btLogic = {};
            }

            //if solr did not find info for item add upc and sku to btDisplay
            if (!angular.isDefined(orderline.btDisplay.id) || !isFinite(Number.parseInt(orderline.btDisplay.id)) || (Number.parseInt(orderline.btDisplay.id) === 0)) {
                orderline.btDisplay.id = angular.copy(orderline.Item._UPCCode);
            }
            if (!angular.isDefined(orderline.btDisplay.sku) || !isFinite(Number.parseInt(orderline.btDisplay.sku)) || (Number.parseInt(orderline.btDisplay.sku) === 0)) {
                orderline.btDisplay.sku = angular.copy(orderline.Item._ItemID);
            }

            return data;

        }, function (response) { $loggerService.log(response); return response });
    };
    var getOrderDetail = function (orderNumberParam, success, error) {
        var url = serviceURL + "/Order/GetOrderDetailJSON";
        $http.post(url, angular.toJson(orderNumberParam)).success(function (data) {
            var result = angular.fromJson(data).GetSterlingOrderDetailResp;
            serviceArrayFix(result);
            updateShipToLabel(result)

            //get solar data
            var promiseArray = [];
            for (var i = 0; i < result.OrderLines.OrderLine.length; i++) {
                (function () {
                    var currentIndex = i;
                    promiseArray.push(addSolarData(result.OrderLines.OrderLine[i]));
                })();
            }

            //get solar data for Exchanged Orders Only
            if ( result.ReturnOrders.ReturnOrder !== undefined && result.ReturnOrders.ReturnOrder.length > 0 )
            {
                for ( var i = 0; i < result.ReturnOrders.ReturnOrder.length; i++ )
                {
                    var returnOrder = result.ReturnOrders.ReturnOrder[i];
                    if ( returnOrder.ExchangeOrders.ExchangeOrder !== undefined && returnOrder.ExchangeOrders.ExchangeOrder.length > 0 )
                    {
                        for ( var j = 0; j < returnOrder.ExchangeOrders.ExchangeOrder.length; j++ )
                        {
                            var exchangeOrder = returnOrder.ExchangeOrders.ExchangeOrder[j];
                            if ( exchangeOrder.OrderLines.OrderLine !== undefined && exchangeOrder.OrderLines.OrderLine.length > 0 )
                            {
                                for ( var k = 0; k < exchangeOrder.OrderLines.OrderLine.length; k++ )
                                {
                                    ( function ()
                                    {
                                        var currentIndex = k;

                                        //promiseArray.push( addSolarData( result.ReturnOrders.ReturnOrder[i].ExchangeOrders.ExchangeOrder[j].OrderLines.OrderLine[k] ) );
                                        promiseArray.push( addSolarData( exchangeOrder.OrderLines.OrderLine[k] ) );
                                    } )();
                                }
                            }
                        }
                    }
                }
            }

            $q.all(promiseArray).finally(function () {
                cache.put("orderDetails", result);
                success(result);
            });

        }).error(function (data) {
            $sendSMTPErrorEmail(data, url);
            error(data);
        })
    };

    var getCurrentOrderDetails = function () {
        return cache.get("orderDetails");
    };

    var compareShipTos = function (shipTo1, shipTo2) {
        return angular.toJson(shipTo1) == angular.toJson(shipTo2);
    }

    var updateShipToLabel = function (orderDetail) {
        var orderLine = orderDetail.OrderLines.OrderLine;
        for (var i = 0; i < orderLine.length - 1; i++) {
            for (var j = i + 1; j < orderLine.length; j++) {
                var shipTo1 = orderLine[i].PersonInfoShipTo;
                var shipTo2 = orderLine[j].PersonInfoShipTo;
                if (!compareShipTos(shipTo1, shipTo2)) {
                    orderDetail.multipleShipTo = true;
                    return;
                }
            }

        }
        orderDetail.PersonInfoShipTo = orderLine[0].PersonInfoShipTo;
    }


    var getPreviousSearchResult = function () {
        return cache.get("orderSearchResult");
    }

    var getPreviousSearchParam = function () {
        return cache.get("orderSearchParam");
    }
    var clearSelectedOrder = function () {
        cache.remove("selectedOrder");
    }
    var getSelectedOrder = function () {
        var selectedOrder = cache.get("selectedOrder");
        return selectedOrder;
    }

    var setSelectedOrder = function (selectedOrder) {
        cache.put("selectedOrder", selectedOrder);
    }

    var calculateOrderLineMerchandiseTotal = function (orderLine) {
        return parseFloat(orderLine.LinePriceInfo._UnitPrice * orderLine._OrderedQty);
    }

    var calculateOrderLineTotal = function (orderLine) {
        //for order details look for Sterling's LineOverallTotals
        if (angular.isDefined(orderLine.LineOverallTotals) && angular.isDefined(orderLine.LineOverallTotals._ExtendedPrice)
                && isFinite(parseFloat(orderLine.LineOverallTotals._ExtendedPrice))) {
            return parseFloat(orderLine.LineOverallTotals._ExtendedPrice);
        }

        return Number(orderLine.LinePriceInfo._btAdjustedPrice) * Number(orderLine._OrderedQty)
    }

    //var calculateChargeDiscounts = function (orderObject) {
    //    //Charge Total
    //    return -1*((Number(orderObject._GiftBoxChrgTotal) +
    //        Number(orderObject._SPOChrgTotal) +
    //        Number(orderObject._ChrgMiscTotal)) -
    //        //Discount Total
    //        (Number(orderObject._CouponDiscTotal) + //
    //        Number(orderObject._GiftBoxDiscTotal) +
    //        Number(orderObject._SPODiscTotal) +
    //        Number(orderObject._MPTDiscTotal) +
    //        Number(orderObject._PromoDiscTotal) +
    //        Number(orderObject._AssociateDiscTotal) + //
    //        Number(orderObject._FirstDayDiscTotal) + //
    //        Number(orderObject._SeniorDiscTotal) +
    //        Number(orderObject._SpecialDiscTotal) +
    //        Number(orderObject._DiscMiscTotal)));
    //}

    var calculateTaxTotal = function (orderObject) {
        return Number(orderObject._GiftBoxTaxTotal) +
        Number(orderObject._SalesTaxTotal) +
        Number(orderObject._ShippingTaxTotal) +
        Number(orderObject._SpHandlingTaxTotal) +
        Number(orderObject._TaxMiscTotal);
    }

    var calculateShippingTotal = function (orderObject) {
        return Number(orderObject._ShippingChrgTotal) -
        Number(orderObject._ShippingDiscTotal) +
        Number(orderObject._SpHandlingChrgTotal) -
        Number(orderObject._SpHandlingDiscTotal);
    }

    var calculateShippingTotalWithoutHandling = function (orderObject) {
        return Number(orderObject._ShippingChrgTotal) -
        Number(orderObject._ShippingDiscTotal);
    }

    var calculateMerchandiseTotal = function (orderObject) {
        return Number(orderObject._UnitPriceTotal);
    };
    var calculateCouponDiscountTotal = function (orderObject) {
        return -1 * Number(orderObject._CouponDiscTotal);
    };
    var calculateMisChargesTotal = function (orderObject) {
        return (Number(orderObject._GiftBoxChrgTotal) + Number(orderObject._SPOChrgTotal) + Number(orderObject._ChrgMiscTotal)) - (Number(orderObject._GiftBoxDiscTotal) + Number(orderObject._SPODiscTotal) + Number(orderObject._MPTDiscTotal) + Number(orderObject._PromoDiscTotal) + Number(orderObject._SeniorDiscTotal) + Number(orderObject._SpecialDiscTotal) + Number(orderObject._DiscMiscTotal));
    };
    var calculateMisChargesTotalWithoutGiftBox = function (orderObject) {
        return (Number(orderObject._SPOChrgTotal) +
                Number(orderObject._ChrgMiscTotal)) -
                (Number(orderObject._SPODiscTotal) +
                Number(orderObject._MPTDiscTotal) +
                Number(orderObject._PromoDiscTotal) +
                Number(orderObject._SeniorDiscTotal) +
                Number(orderObject._SpecialDiscTotal) +
                Number(orderObject._DiscMiscTotal));
    };

    var calculateAssocDiscTotal = function (orderObject) {
        return -1 * (Number(orderObject._AssociateDiscTotal) + Number(orderObject._FirstDayDiscTotal));
    };

    var calculatePromoDiscountCharges = function (orderObject) {
        return calculateMisChargesTotalWithoutGiftBox(orderObject);
    }

    var calculateSpecialHandlingCharges = function (orderObject) {
        return Number(orderObject._SpHandlingChrgTotal) -
        Number(orderObject._SpHandlingDiscTotal);
    }

    var calculateGiftBoxCharges = function (orderObject) {
        return Number(orderObject._GiftBoxChrgTotal) -
        Number(orderObject._GiftBoxDiscTotal);
    }


    var lineTotal = function (orderline) {
        return Number(orderline.LinePriceInfo._UnitPrice) * parseInt(orderline._OrderedQty);

    };



    var calculateOrderSubTotal = function (orderObject) {
        return calculateMerchandiseTotal(orderObject) + calculateMisChargesTotal(orderObject) + calculateCouponDiscountTotal(orderObject) + calculateAssocDiscTotal(orderObject);
    };


    var orderPricingDetail = function (cart, isUpdateable) {
        if (!angular.isDefined(isUpdateable) || isUpdateable === null) {
            isUpdateable = true;
        }

        //do pricing detail,
        var detail = {};
        detail.category = [];
        detail.category.push({ categoryDescription: "Merchandise Total", "total": calculateMerchandiseTotal(cart) });
        detail.category.push({ categoryDescription: "Promo Discounts/Charges", "total": calculatePromoDiscountCharges(cart), "hideWhenZero": true });
        detail.category.push({ categoryDescription: "Coupon Discounts", "total": calculateCouponDiscountTotal(cart), "hideWhenZero": true });
        detail.category.push({ categoryDescription: "Associate Discount/Instant Credit", "total": calculateAssocDiscTotal(cart), "hideWhenZero": true });
        detail.category.push({ categoryDescription: "Gift Box Charge", "total": calculateGiftBoxCharges(cart), "hideWhenZero": true });
        detail.category.push({ categoryDescription: "Special Handling", "total": calculateSpecialHandlingCharges(cart), "hideWhenZero": true });
        detail.category.push({ categoryDescription: "Shipping", "total": calculateShippingTotalWithoutHandling(cart), "updateable": (isUpdateable && true) });
        detail.category.push({ categoryDescription: "Taxes", "total": calculateTaxTotal(cart) });
        detail.category.push({ categoryDescription: "Order Total", "total": cart._OrderTotal });
        detail.coupons = [];
        cart.OrderLines.OrderLine.forEach(function (orderLine) {
            orderLine.LineCharges.LineCharge.forEach(function (lineCharge) {
                if (lineCharge._ChargeCategory == 'BTN_CPN_DISC') {
                    var discAmount = _chargeAmount(orderLine._OrderedQty, lineCharge);
                    var duplicate = false;
                    angular.forEach(detail.coupons, function (coupon) {
                        if (coupon.couponCode == lineCharge._Reference) {
                            coupon.DiscountAmt += discAmount;
                            duplicate = true;
                        }
                    });

                    if (!duplicate) {
                        detail.coupons.push({ couponCode: lineCharge._Reference, "DiscountAmt": discAmount });
                    }
                }
            });
        });

        return detail;
    };

    // get total of charge line amount
    var _chargeAmount = function (orderlineQty, charge) {

        orderlineQty = isFinite(parseInt(orderlineQty)) ? parseInt(orderlineQty) : 0;

        orderlineQty = (orderlineQty && isFinite(parseInt(orderlineQty))) ? parseInt(orderlineQty) : 1;
        if (angular.isDefined(charge._ChargeAmount) && charge._ChargeAmount != null && isFinite(parseFloat(charge._ChargeAmount))) {
            return parseFloat(charge._ChargeAmount);
        }
        else {
            var total = 0;

            if (angular.isDefined(charge._ChargePerUnit) && charge._ChargePerUnit != null && isFinite(parseFloat(charge._ChargePerUnit))) {
                total = orderlineQty * parseFloat(charge._ChargePerUnit);
            }
            if (angular.isDefined(charge._ChargePerLine) && charge._ChargePerLine != null && isFinite(parseFloat(charge._ChargePerLine))) {
                total = total + parseFloat(charge._ChargePerLine);
            }

            //at this time only LineTaxes have a _Tax attribute while only LineCharges have _ChargePerUnit & _ChargePerLine
            if (angular.isDefined(charge._Tax) && charge._ChargePerLine != null && isFinite(parseFloat(charge._Tax))) {
                total = total + parseFloat(charge._Tax);
            }

            return total;
        }
    };

    var getSkyTotaling = function (cart) {

        var unitPriceTotal = 0;
        var shippingDiscountTotal = 0;
        var couponDiscountTotal = 0;
        var spHandlingDiscountTotal = 0;
        var giftBoxDiscountTotal = 0;
        var promoDiscountTotal = 0;
        var spoDiscountTotal = 0;
        var associateDiscountTotal = 0;
        var firstDayDiscountTotal = 0;
        var mptDiscountTotal = 0;
        var seniorDiscountTotal = 0;
        var specialDiscountTotal = 0;
        var discountMiscTotal = 0;
        var shippingChargeTotal = 0;
        var spHandlingChargeTotal = 0;
        var giftBoxChargeTotal = 0;
        var spoChargeTotal = 0;
        var associateChargeTotal = 0;
        var firstDayChargeTotal = 0;
        var chargeMiscTotal = 0;
        var salesTaxChargeTotal = 0;
        var shippingTaxChargeTotal = 0;
        var giftBoxTaxChargeTotal = 0;
        var spHandlingTaxChargeTotal = 0;
        var salesTaxDiscountTotal = 0;
        var shippingTaxDiscountTotal = 0;
        var giftBoxTaxDiscountTotal = 0;
        var spHandlingTaxDiscountTotal = 0;
        var taxMiscTotal = 0;

        //below is a translation of the sky C# code 7/28/2015
        angular.forEach(cart.OrderLines.OrderLine, function (x) {
            if (x.LinePriceInfo != null) {
                unitPriceTotal += (parseFloat(x.LinePriceInfo._UnitPrice) * parseFloat(x._OrderedQty));
            }

            if (angular.isDefined(x.LineCharges) && x.LineCharges != null && angular.isArray(x.LineCharges.LineCharge)) {
                angular.forEach(x.LineCharges.LineCharge, function (y) {
                    //total discounts
                    if (angular.isString(y._ChargeName)) {
                        if (y._IsDiscount == "Y") {
                            if ((/^DISC_SHIPPING.*/).test(y._ChargeName) || (/^ADJ_SHIPPING.*/).test(y._ChargeName) || (/^CREDITED_SHIP_CHRG.*/).test(y._ChargeName)) {
                                shippingDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_COUPON.*/).test(y._ChargeName) || (/^ADJ_COUPON.*/).test(y._ChargeName)) {
                                couponDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_SP_HANDLING.*/).test(y._ChargeName) || (/^ADJ_SP_HANDLING.*/).test(y._ChargeName)) {
                                spHandlingDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_GIFT_BOX.*/).test(y._ChargeName) || (/^ADJ_GIFT_BOX.*/).test(y._ChargeName)) {
                                giftBoxDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_PROMO.*/).test(y._ChargeName)) {
                                promoDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_SPO.*/).test(y._ChargeName)) {
                                spoDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_ASSOCIATE.*/).test(y._ChargeName)) {
                                associateDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_FIRST_DAY.*/).test(y._ChargeName)) {
                                firstDayDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_MPT.*/).test(y._ChargeName)) {
                                mptDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_SENIOR.*/).test(y._ChargeName)) {
                                seniorDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^DISC_SPECIAL.*/).test(y._ChargeName)) {
                                specialDiscountTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else {
                                discountMiscTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            //total charges
                        } else if (y._IsDiscount == "N") {
                            if ((/^CHRG_SHIPPING.*/).test(y._ChargeName)) {
                                shippingChargeTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^CHRG_SP_HANDLING.*/).test(y._ChargeName)) {
                                spHandlingChargeTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^CHRG_GIFT_BOX.*/).test(y._ChargeName)) {
                                giftBoxChargeTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^CHRG_SPO.*/).test(y._ChargeName)) {
                                spoChargeTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^ADJ_CHRG_ASSOCIATE.*/).test(y._ChargeName)) {
                                associateChargeTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else if ((/^ADJ_CHRG_FIRST_DAY.*/).test(y._ChargeName)) {
                                firstDayChargeTotal += _chargeAmount(x._OrderedQty, y);
                            }
                            else {
                                chargeMiscTotal += _chargeAmount(x._OrderedQty, y);
                            }
                        }
                    }
                });
            }

            if (angular.isDefined(x.LineTaxes) && x.LineTaxes !== null && angular.isArray(x.LineTaxes.LineTax)) {
                angular.forEach(x.LineTaxes.LineTax, function (y) {
                    if (angular.isString(y._ChargeName) && angular.isDefined(y._Tax) && y._Tax !== null) {
                        if ((/^TAX_SALES.*/).test(y._ChargeName)) {
                            salesTaxChargeTotal += parseFloat(y._Tax);
                        }
                        else if ((/^TAX_SHIPPING.*/).test(y._ChargeName)) {
                            shippingTaxChargeTotal += parseFloat(y._Tax);
                        }
                        else if ((/^TAX_GIFT_BOX.*/).test(y._ChargeName)) {
                            giftBoxTaxChargeTotal += parseFloat(y._Tax);
                        }
                        else if ((/^TAX_SP_HANDLING.*/).test(y._ChargeName)) {
                            spHandlingTaxChargeTotal += parseFloat(y._Tax);
                        }
                        else if ((/^ADJ_TAX_SALES.*/).test(y._ChargeName)) {
                            salesTaxDiscountTotal += parseFloat(y._Tax);
                        }
                        else if ((/^ADJ_TAX_SHIPPING.*/).test(y._ChargeName) || (/^CREDITED_SHIP_TAX.*/).test(y._ChargeName)) {
                            shippingTaxDiscountTotal += parseFloat(y._Tax);
                        }
                        else if ((/^ADJ_TAX_GIFT_BOX.*/).test(y._ChargeName)) {
                            giftBoxTaxDiscountTotal += parseFloat(y._Tax);
                        }
                        else if ((/^ADJ_TAX_SP_HANDLING.*/).test(y._ChargeName)) {
                            spHandlingTaxDiscountTotal += parseFloat(y._Tax);
                        }
                        else {
                            taxMiscTotal += parseFloat(y._Tax);
                        }
                    }
                });
            }
        });


        //Tax Total
        var SalesTaxTotal = (salesTaxChargeTotal - salesTaxDiscountTotal);
        var ShippingTaxTotal = (shippingTaxChargeTotal - shippingTaxDiscountTotal);
        var GiftBoxTaxTotal = (giftBoxTaxChargeTotal - giftBoxTaxDiscountTotal);
        var SpHandlingTaxTotal = (spHandlingTaxChargeTotal - spHandlingTaxDiscountTotal);

        //combine discount and charge
        var ShippingTotal = (shippingChargeTotal - shippingDiscountTotal) + (ShippingTaxTotal);
        var SpHandlingTotal = (spHandlingChargeTotal - spHandlingDiscountTotal) + (SpHandlingTaxTotal);
        var GiftBoxTotal = (giftBoxChargeTotal - giftBoxDiscountTotal) + (GiftBoxTaxTotal);
        var FirstDayDiscTotal = (firstDayDiscountTotal - firstDayChargeTotal);
        var AssociateDiscTotal = (associateDiscountTotal - associateChargeTotal);
        var SPODiscTotal = (spoDiscountTotal - spoChargeTotal);


        var chargeTotal = unitPriceTotal + SalesTaxTotal + taxMiscTotal + ShippingTotal + SpHandlingTotal + GiftBoxTotal + chargeMiscTotal;
        var discountTotal = FirstDayDiscTotal + AssociateDiscTotal + SPODiscTotal + couponDiscountTotal + mptDiscountTotal + promoDiscountTotal + specialDiscountTotal + seniorDiscountTotal + discountMiscTotal;
        var orderTotal = (chargeTotal - discountTotal);

        cart._OrderTotal = orderTotal;
        cart._UnitPriceTotal = unitPriceTotal;
        cart._ShippingChrgTotal = shippingChargeTotal;
        cart._ShippingDiscTotal = shippingDiscountTotal;
        cart._CouponDiscTotal = couponDiscountTotal;
        cart._SpHandlingChrgTotal = spHandlingChargeTotal;
        cart._SpHandlingDiscTotal = spHandlingDiscountTotal;
        cart._GiftBoxChrgTotal = giftBoxChargeTotal;
        cart._GiftBoxDiscTotal = giftBoxDiscountTotal;
        cart._SPOChrgTotal = spoChargeTotal;
        cart._SPODiscTotal = spoDiscountTotal;
        cart._MPTDiscTotal = mptDiscountTotal;
        cart._PromoDiscTotal = promoDiscountTotal;
        cart._AssociateDiscTotal = AssociateDiscTotal;
        cart._FirstDayDiscTotal = FirstDayDiscTotal;
        cart._SalesTaxTotal = SalesTaxTotal;
        cart._ShippingTaxTotal = ShippingTaxTotal;
        cart._GiftBoxTaxTotal = GiftBoxTaxTotal;
        cart._SpHandlingTaxTotal = SpHandlingTaxTotal;
        cart._SeniorDiscTotal = seniorDiscountTotal;
        cart._SpecialDiscTotal = specialDiscountTotal;
        cart._TaxMiscTotal = taxMiscTotal;
        cart._ChrgMiscTotal = chargeMiscTotal;
        cart._DiscMiscTotal = discountMiscTotal;

    };

    var orderLineItemPricingDetail = function (orderLine) {
        //do pricing detail,
        var detail = {};
        detail.category = [];

        var shippingTotal = 0;

        angular.forEach(orderLine.LineCharges.LineCharge, function (charge) {
            if (charge._ChargeCategory == "BTN_SHIP_CHRG") {
                shippingTotal += _chargeAmount(orderLine._OrderedQty, charge);
            } else if (charge._ChargeCategory == "BTN_SHIP_DISC") {
                shippingTotal -= _chargeAmount(orderLine._OrderedQty, charge);
            }
        });

        var couponTotal = 0;
        detail.coupons = [];
        orderLine.LineCharges.LineCharge.forEach(function (lineCharge) {
            if (lineCharge._ChargeCategory == 'BTN_CPN_DISC') {
                var discAmount = _chargeAmount(orderLine._OrderedQty, lineCharge);
                var duplicate = false;
                angular.forEach(detail.coupons, function (coupon) {
                    if (coupon.couponCode == lineCharge._Reference) {
                        coupon.DiscountAmt += discAmount;
                        duplicate = true;
                    }
                });

                if (!duplicate) {
                    detail.coupons.push({ couponCode: lineCharge._Reference, "DiscountAmt": discAmount });
                }

                couponTotal += discAmount;
            }
        });

        detail.category.push({ categoryDescription: "Merchandise Total", isDiscount: false, "total": calculateOrderLineMerchandiseTotal(orderLine) });
        orderLine.LineCharges.LineCharge.forEach(function (lineCharge) {
            if (lineCharge._ChargeCategory == "BTN_SHIP_CHRG") {
                detail.category.push({ categoryDescription: "Shipping Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge), "updateable": true });
            } else if (lineCharge._ChargeCategory != 'BTN_CPN_DISC') {
                if (lineCharge._ChargeName == "DISC_DLR") {
                    detail.category.push({ categoryDescription: "DLR Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_PERC") {
                    detail.category.push({ categoryDescription: "PERC Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_GIFT_BOX") {
                    detail.category.push({ categoryDescription: "Gift Box Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_SP_HANDLING") {
                    detail.category.push({ categoryDescription: "Special Handling Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_SHIPPING") {
                    detail.category.push({ categoryDescription: "Shipping Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_SPO") {
                    detail.category.push({ categoryDescription: "SPO Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_MPT") {
                    detail.category.push({ categoryDescription: "MPT Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_PROMO") {
                    detail.category.push({ categoryDescription: "Promo Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_WEB_PROMO") {
                    detail.category.push({ categoryDescription: "Web Promo Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_CSR") {
                    detail.category.push({ categoryDescription: "CSR Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_PREPAID_LABEL") {
                    detail.category.push({ categoryDescription: "Prepaid Label Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_SPECIAL") {
                    detail.category.push({ categoryDescription: "Special Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_SENIOR") {
                    detail.category.push({ categoryDescription: "Senior Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_FIRST_DAY") {
                    detail.category.push({ categoryDescription: "First Day Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_ASSOCIATE") {
                    detail.category.push({ categoryDescription: "Associate Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "DISC_COUPON_") {
                    detail.category.push({ categoryDescription: "Coupon Discount", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CREDITED_SHIP_TAX") {
                    detail.category.push({ categoryDescription: "Credited Ship Tax", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CREDITED_SHIP_CHRG") {
                    detail.category.push({ categoryDescription: "Credited Ship Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CHRG_RETURN_LABEL") {
                    detail.category.push({ categoryDescription: "Return Label Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CHRG_GIFT_BOX") {
                    detail.category.push({ categoryDescription: "Gift Box Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CHRG_SP_HANDLING") {
                    detail.category.push({ categoryDescription: "Special Handling Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CHRG_SHIPPING") {
                    detail.category.push({ categoryDescription: "Shipping Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CHRG_SPO") {
                    detail.category.push({ categoryDescription: "SPO Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "CHRG_CSR") {
                    detail.category.push({ categoryDescription: "CSR Charge", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "TAX_SP_HANDLING") {
                    detail.category.push({ categoryDescription: "Special Handling Tax", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "TAX_GIFT_BOX") {
                    detail.category.push({ categoryDescription: "Gift Box Tax", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "TAX_SHIPPING") {
                    detail.category.push({ categoryDescription: "Shipping Tax", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "TAX_SALES") {
                    detail.category.push({ categoryDescription: "Sales Tax", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_CHRG_FIRST_DAY") {
                    detail.category.push({ categoryDescription: "Fist Day Charge Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_CHRG_RTN_LBL") {
                    detail.category.push({ categoryDescription: "Return Label Charge Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_CHRG_ASSOCIATE") {
                    detail.category.push({ categoryDescription: "Associate Charge Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_TAX_SP_HANDLING") {
                    detail.category.push({ categoryDescription: "Special Handling Tax Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_TAX_SP_HANDLING") {
                    detail.category.push({ categoryDescription: "Special Handling Tax Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_TAX_GIFT_BOX") {
                    detail.category.push({ categoryDescription: "Gift Box Tax Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_TAX_SHIPPING") {
                    detail.category.push({ categoryDescription: "Shipping Tax Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_TAX_SALES") {
                    detail.category.push({ categoryDescription: "Sales Tax Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_ELECTIVE") {
                    detail.category.push({ categoryDescription: "Elective Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_RTN_LBL") {
                    detail.category.push({ categoryDescription: "Return Label Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_PRICE") {
                    detail.category.push({ categoryDescription: "Price Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_SP_HANDLING") {
                    detail.category.push({ categoryDescription: "Special Handling Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_GIFT_BOX") {
                    detail.category.push({ categoryDescription: "Gift Box Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_COUPON_") {
                    detail.category.push({ categoryDescription: "Coupon Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else if (lineCharge._ChargeName == "ADJ_SHIPPING") {
                    detail.category.push({ categoryDescription: "Shipping Adjustment", isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
                else {
                    detail.category.push({ categoryDescription: lineCharge._ChargeName, isDiscount: ((/^y$/i).test(lineCharge._IsDiscount)), "total": _chargeAmount(orderLine._OrderedQty, lineCharge) });
                }
            }
        });

        if (couponTotal > 0) {
            detail.category.push({ categoryDescription: "Coupon Total", isDiscount: true, "total": couponTotal });
        }

        detail.category.push({ categoryDescription: "Total", isDiscount: false, "total": calculateOrderLineTotal(orderLine) });

        detail.orderLine = orderLine;
        return detail;
    };

    var createOrder = function (orderCart, success, error) {
        var url = serviceURL + "/Order/CreateOrderJSON";
        //var url = "http://10.131.135.76:7080/Order/CreateOrderJSON";
        var contract = {
            CreateSterlingOrderReq: orderCart
        }

        $http.post(url, angular.toJson(contract)).then(function (data) {
            var result = angular.fromJson(data.data).CreateSterlingOrderResp;
            success(result);
        },
        function (response) { //error response handler
            var cartCopy = angular.fromJson(angular.copy(response.config.data));
            var reportedInfo = '';
            var additionalEmailRecipients = null;


            if (angular.isDefined(cartCopy.CreateSterlingOrderReq)) {
                var giftCardsArr = [];

                //clean payments
                for (var i = 0; i < cartCopy.CreateSterlingOrderReq.PaymentMethods.PaymentMethod.length; i++) {
                    //save giftcards
                    if (cartCopy.CreateSterlingOrderReq.PaymentMethods.PaymentMethod[i]._CreditCardType &&
                        cartCopy.CreateSterlingOrderReq.PaymentMethods.PaymentMethod[i]._CreditCardType === 'GC'
                    ) {

                        //if giftcard processAmount === 0 then ignore
                        var processedAmount = Number.parseFloat(cartCopy.CreateSterlingOrderReq.PaymentMethods.PaymentMethod[i].PaymentDetails._ProcessedAmount);
                        if (!isFinite(processedAmount) || processedAmount !== 0.00) {

                        var tempGift = angular.copy(cartCopy.CreateSterlingOrderReq.PaymentMethods.PaymentMethod[i]);
                        delete tempGift._PaymentReference2;
                        delete tempGift._SvcNo;
                        delete tempGift.PaymentDetails._Reference2;

                        giftCardsArr.push(tempGift);
                    }
                }
                }

                //replace all payments with only gift cards
                cartCopy.CreateSterlingOrderReq.PaymentMethods.PaymentMethod = giftCardsArr;

                if (giftCardsArr.length > 0) {
                    var giftcardReport = [];
                    for (var i = 0; i < giftCardsArr.length; i++) {
                        giftcardReport.push('Card:' + giftCardsArr[i]._DisplayCreditCardNo + ", Auth Ref Num:" + giftCardsArr[i]._PaymentReference1 +
                             ", Amount Processed:" + giftCardsArr[i].PaymentDetails._ProcessedAmount + ", AuthReturnCode:" + giftCardsArr[i].PaymentDetails._AuthReturnCode);
                    }
                    reportedInfo = 'Giftcard on Order: \n    ' + giftcardReport.join(' \n    ');

                    url = url + " Order Has Giftcards: " + giftCardsArr.length;

                    //get additional email recipients if in PROD
                    if ((/prod\.bonton\.com/i).test(serviceURL)) {
                        additionalEmailRecipients = $btProp.getProp('orderCreateFailureWithGiftCardEmailList');
                    }
                }
            }

            
            $sendSMTPErrorEmail(response.data, url, cartCopy, reportedInfo, additionalEmailRecipients);
            error(response.data);
        });

    }

    var canCancelOrder = function (orderDetail) {
        if (orderDetail && _isInRemorseHold(orderDetail)) {
            return true; //$securityService.canCancelOrder();
        } else {
            return false;
        }
    };

    var _isInRemorseHold = function (order) {
        if (angular.isDefined(order) && angular.isDefined(order.OrderHoldTypes) && angular.isArray(order.OrderHoldTypes.OrderHoldType) && angular.isDefined(order.OrderHoldTypes.OrderHoldType[0])) {
            return (order.OrderHoldTypes.OrderHoldType[0]._HoldType == 'REMORSE_PERIOD_HOLD' && order.OrderHoldTypes.OrderHoldType[0]._Status == '1100');
        } else {
            return false;
        }
    };

    var cancelOrder = function (orderDetail) {

        var terminalNumber = $POSService.getPOSParameters().terminalNumber;
        var storeNumber = $POSService.getPOSParameters().storeNumber;
        var associateId = $POSService.getPOSParameters().associateId;
        if (angular.isDefined(terminalNumber) && null != terminalNumber  && terminalNumber.length > 0) {
            terminalNumber = "Order Cancelled in LUFI2 by the associate " +associateId +" in the Store "+ storeNumber +" and terminal " + terminalNumber;
        } else {
            terminalNumber = "Order Cancelled in LUFI2 by the associate " +associateId;
        }
        
        var NoteText = "This Order was Cancelled by "+terminalNumber ;
        
        var contract = {
            ChangeSterlingOrderReq: {
                'Notes': {
                    'Note': [
                        {
                            '_ResonCode': '01',
                            '_NoteText': terminalNumber
                        }
                    ]
                },
                '_Action': 'CANCEL',
                '_IgnoreOrdering': 'Y',
                '_ModificationReasonCode': '01',
                '_ModificationReasonText':terminalNumber,
                '_OrderHeaderKey': orderDetail._OrderHeaderKey,
                '_Override': 'Y'
    }
        };
        
        return $http.post(serviceURL + "/Order/ChangeOrderJSON", contract);
    };

    return {
        getOrderList: getOrderList,
        getOrderDetail: getOrderDetail,
        getPreviousSearchResult: getPreviousSearchResult,
        getPreviousSearchParam: getPreviousSearchParam,
        orderPricingDetail: orderPricingDetail,
        orderLineItemPricingDetail: orderLineItemPricingDetail,
        setSelectedOrder: setSelectedOrder,
        getSelectedOrder: getSelectedOrder,
        createOrder: createOrder,
        lineTotal: lineTotal,
        calculateOrderLineTotal: calculateOrderLineTotal,
        //calculateChargeDiscounts: calculateChargeDiscounts,
        calculateTaxTotal: calculateTaxTotal,
        calculateShippingTotal: calculateShippingTotal,
        calculateMerchandiseTotal: calculateMerchandiseTotal,
        calculateCouponDiscountTotal: calculateCouponDiscountTotal,
        calculateMisChargesTotal: calculateMisChargesTotal,
        calculateAssocDiscTotal: calculateAssocDiscTotal,
        calculatePromoDiscountCharges: calculatePromoDiscountCharges,
        calculateSpecialHandlingCharges: calculateSpecialHandlingCharges,
        calculateGiftBoxCharges: calculateGiftBoxCharges,
        calculateOrderSubTotal: calculateOrderSubTotal,
        getCurrentOrderDetails: getCurrentOrderDetails,
        getSkyTotaling: getSkyTotaling,
        addSolarData: addSolarData,
        canCancelOrder: canCancelOrder,
        cancelOrder: cancelOrder
    };
}]);