describe("Delete orderline from cart", function () {


    var orderCart;
    var refCart;
    var $state;

    beforeEach(angular.mock.module("propertiesService", "appUtilities", "appServiceOrderCart", 'appServicesWebSocket', 'appServicesItem', 'appServiceReprice', 'appServicesCustomer'));
    beforeEach(module('ui.router'));

    beforeEach(inject(function (_orderCart_, _$state_) {
        $state = _$state_;
        orderCart = _orderCart_;
    }));

    beforeEach(function () {
        refCart = orderCart.order.getLiveOrderCart();
        angular.copy({

            //Order attributes
            AuthorizedClient: "Store", //"Store" for LUFI
            BillToID: "",
            CustomerContactID: "",
            CustomerEMailID: "garrett.stibb@bonton.com",
            DocumentType: "0001", //Alway "0001" for LUFI Sales Order
            DraftOrderFlag: "Y", // "Y" for Draft Order
            EnteredBy: "014265", // sales associates ID "014222"
            EnterpriseCode: "BONTON", //always "BONTON"
            EntryType: "Store", //channel order was created from "Store" for LUFI. Also saw "ELD" aka elderman
            OrderNo: "", //if blank Sterling will autogenerate new one
            PaymentRuleId: "BT_DEF_PAYMENT_RULE", //what payment rules are used
            SellerOrganizationCode: "101", //Store Node /number ex: "101"
            TaxExemptFlag: "N",
            TaxExemptionCertificate: "",

            //OrderLines
            OrderLines: {
                OrderLine: [{
                    "_CarrierServiceCode": "UPS-GRND",
                    "_DeliveryMethod": "DEL",
                    "_GiftFlag": "N",
                    "_GiftWrap": "N",
                    "_LevelOfService": "GRND",
                    "_OrderedQty": "2.0",
                    "_PrimeLineNo": 1,
                    "_SCAC": "UPS",
                    "_ScacAndService": "UPS-GRND",
                    "_SubLineNo": 1,
                    "Item": {
                        "_ItemID": "425800414756",
                        "_ProductClass": "NEW",
                        "_UnitCost": "26.98000",
                        "_UnitOfMeasure": "EACH",
                        "_UPCCode": "0888590925992"
                    },
                    "PersonInfoShipTo": {
                        "_AddressID": "LOGON5918 Alias like Home",
                        "_AddressLine1": "2412 56th ave",
                        "_AddressLine2": "Unit 3",
                        "_AddressLine3": "Bld 2",
                        "_City": "kenosha",
                        "_Country": "US",
                        "_DayPhone": "2629605804",
                        "_EMailID": "garrett.stibb@bonton.com",
                        "_EveningPhone": "1019991234",
                        "_FirstName": "Garrett",
                        "_LastName": "Stibb",
                        "_MiddleName": "C just initial ever",
                        "_PersonInfoKey": "20140819160910224059208",
                        "_State": "WI",
                        "_ZipCode": "53144"
                    },
                    "LinePriceInfo": {
                        "_ListPrice": "40.00000",
                        "_RetailPrice": "26.98000",
                        "_UnitPrice": "40.00"
                    },
                    "LineCharges": {
                        "LineCharge": [
                          {
                              "_ChargeCategory": "BTN_SHIP_CHRG",
                              "_ChargeName": "CHRG_SHIPPING",
                              "_ChargePerUnit": "0.76",
                              "_Reference": ""
                          },
                          {
                              "_ChargeCategory": "BTN_SHIP_DISC",
                              "_ChargeName": "DISC_SHIPPING",
                              "_ChargePerUnit": "0.77",
                              "_Reference": "FREESHIP75"
                          },
                          {
                              "_ChargeCategory": "BTN_SALES_DISC",
                              "_ChargeName": "DISC_PROMO",
                              "_ChargePerUnit": "13.04",
                              "_Reference": ""
                          }
                        ]
                    },
                    "LineTaxes": {
                        "LineTax": [
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SALES",
                              "_Tax": "3.78000",
                              "_TaxName": "SALES",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          },
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SHIPPING",
                              "_Tax": "0.00000",
                              "_TaxName": "SHIPPING",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          }
                        ]
                    },
                    "Notes": {
                        "Note": [
                          {
                              "_NoteText": "Hi",
                              "_ReasonCode": "GIFT_MESSAGE"
                          },
                          {
                              "_NoteText": "Garrett",
                              "_ReasonCode": "GIFT_FROM"
                          },
                          {
                              "_NoteText": "Sister",
                              "_ReasonCode": "GIFT_TO"
                          }
                        ]
                    },
                    "Extn": {
                        "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
                        "_ExtnClass": "",
                        "_ExtnDepartment": "",
                        "_ExtnGiftItemId": "",
                        "_ExtnGiftPurchaseRecordID": "",
                        "_ExtnGiftRegistryNo": "",
                        "_ExtnGWP": "",
                        "_ExtnIsPriceLocked": "Y",
                        "_ExtnMPTID": "",
                        "_ExtnMPTOfferDetail": "",
                        "_ExtnMPTOfferMsg1": "",
                        "_ExtnMPTOfferMsg2": "",
                        "_ExtnParentLineNo": "",
                        "_ExtnPriceStatus": "",
                        "_ExtnPromoNumber": "",
                        "_ExtnSpecialHandlingCd": "1",
                        "_ExtnSPOID": "",
                        "_ExtnSPOOfferDetail": "",
                        "_ExtnSPOOfferMsg1": "",
                        "_ExtnSPOOfferMsg2": "",
                        "_ExtnSREligible": "1",
                        "_ExtnTranID": ""
                    },
                    "btDisplay": {
                        "itemDescription": "Dress Blue Medium",
                        "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
                        "itemType": "REG",
                        "storageType": "R",
                        "orderableInventory": 2000,
                        "alternateUPCs": [
                          "031290088157"
                        ],
                        "imageURL": "http://broker.bonton.com/123456",
                        "brandLongDesc": "Carter's",
                        "cfgDesc": "GIRLS",
                        "classLongDesc": "CARTERS",
                        "cmgDesc": "CHILDRENS",
                        "colorAttrDesc": "Asst Lt/Pale",
                        "colorFamDesc": "Asst Family",
                        "colorLongDesc": "PRINT (969",
                        "crgDesc": "CHILDRENS",
                        "deptLongDesc": "GIRLSWEAR 2-6X",
                        "desc1": "TEE",
                        "desc2": "FALL CARTERS OP",
                        "desc3": "CARTERS OCT",
                        "desc4": "4-6X GIRL",
                        "fabDtlDesc": "Screen Print",
                        "fabLongDesc": "Cotton Blend-With Stretch",
                        "fobDesc": "GIRLS 2-6X",
                        "genClassLongDesc": "Knit Tops",
                        "gensClaLongDesc": "Tee",
                        "isn": "243306978",
                        "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
                        "itemSize": "4",
                        "labelLongDesc": "Carter's",
                        "prodDetail2": "Scoop Neck",
                        "prodDetail3": "Knit-Interlock/Jersey",
                        "prodDtlLongDesc": "Short Sleeve",
                        "vendorStyleNum": "273B053"
                    },
                    "btLogic": {
                        "isDeliveryAllowed": "N",
                        "isHazmat": "N",
                        "isParcelShippingAllowed": "Y",
                        "isPickupAllowed": "N",
                        "isReturnable": "Y",
                        "isShippingAllowed": "Y",
                        "isPriceOverridden": "Y",
                        "priceOverrideValue": 21.33,
                        "isBigTicket": "Y",
                        "btIsWebExclusive": "false",
                        "btIsWebOnlyEvent": "false",
                        "btIsIVPEvent": "false",
                        "btIsBonusEvent": "false",
                        "btIsDoorBusterEvent": "false",
                        "btIsNightOwlEvent": "false",
                        "btIsOtherSpecialEvent": "false",
                        "btIsSpecialHandling": "false",
                        "btSpecialHandlingFee": "0"
                    }
                }]
            },

            //PersonInfoBillTo Header Level
            PersonInfoBillTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },

            //PersonInfoShipTo Header Level
            PersonInfoShipTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },
            PaymentMethods: {                       //xml-json issue xml goes <PaymentMethods><PaymentMethod /><PaymentMethod /> ...
                //so do PaymentMethods:[]  and have broker know the tag needs to be PaymentMethod OR make another object {PaymentMethod:[]}
                PaymentMethod: [
                        {
                            UnlimitedCharges: "Y",
                            PaymentType: "CREDIT_CARD",
                            DisplayCreditCardNo: "0691",
                            CreditCardType: "CP",
                            CreditCardNo: "8231211627154941",
                            CreditCardExpDate: "04/2018",

                            PaymentDetails: {
                                ProcessedAmount: "102.65000",
                                ChargeType: "AUTHORIZATION",
                                AuthorizationID: "001060",
                                AuthCode: "001060"
                            }
                        }]
            },

            PriceInfo: { Currency: "USD" } //price infor child node
        }, refCart);
    });

    it("Control Case: Order has one OrderLine", function () {
        expect(refCart.OrderLines.OrderLine.length).toEqual(1);
    })

    it("Delete OrderLine", function () {
        orderCart.orderLine.deleteOrderLine([{ PrimeLine: 1, SubLine: 1 }]);
        expect(refCart.OrderLines.OrderLine.length).toEqual(0);
    })

    it("Delete String Prime OrderLine", function () {
        orderCart.orderLine.deleteOrderLine([{ PrimeLine: "1", SubLine: 1 }]);
        expect(refCart.OrderLines.OrderLine.length).toEqual(0);
    })

    it("Delete Prime/Subline Object not enclosed in array", function () {
        orderCart.orderLine.deleteOrderLine({ PrimeLine: 1, SubLine: 1 });
        expect(refCart.OrderLines.OrderLine.length).toEqual(0);
    })

    it("Delete OrderLine with only PrimeLine passed", function () {
        orderCart.orderLine.deleteOrderLine({ PrimeLine: 1 });
        expect(refCart.OrderLines.OrderLine.length).toEqual(0);
    })

    it("Delete none existant orderline", function () {
        orderCart.orderLine.deleteOrderLine({ PrimeLine: 3 });
        expect(refCart.OrderLines.OrderLine.length).toEqual(1);
    })

    //it("Delete second orderline", function () {
    //    refCart.OrderLines.OrderLine.push({
    //        "_CarrierServiceCode": "UPS-GRND",
    //        "_DeliveryMethod": "DEL",
    //        "_GiftFlag": "N",
    //        "_GiftWrap": "N",
    //        "_LevelOfService": "GRND",
    //        "_OrderedQty": "2.0",
    //        "_PrimeLineNo": 2,
    //        "_SCAC": "UPS",
    //        "_ScacAndService": "UPS-GRND",
    //        "_SubLineNo": 3,
    //        "Item": {
    //            "_ItemID": "425800414756",
    //            "_ProductClass": "NEW",
    //            "_UnitCost": "26.98000",
    //            "_UnitOfMeasure": "EACH",
    //            "_UPCCode": "0888590925992"
    //        },
    //        "PersonInfoShipTo": {
    //            "_AddressID": "LOGON5918 Alias like Home",
    //            "_AddressLine1": "2412 56th ave",
    //            "_AddressLine2": "Unit 3",
    //            "_AddressLine3": "Bld 2",
    //            "_City": "kenosha",
    //            "_Country": "US",
    //            "_DayPhone": "2629605804",
    //            "_EMailID": "garrett.stibb@bonton.com",
    //            "_EveningPhone": "1019991234",
    //            "_FirstName": "Garrett",
    //            "_LastName": "Stibb",
    //            "_MiddleName": "C just initial ever",
    //            "_PersonInfoKey": "20140819160910224059208",
    //            "_State": "WI",
    //            "_ZipCode": "53144"
    //        },
    //        "LinePriceInfo": {
    //            "_ListPrice": "40.00000",
    //            "_RetailPrice": "26.98000",
    //            "_UnitPrice": "40.00"
    //        },
    //        "LineCharges": {
    //            "LineCharge": [
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_CHRG",
    //                  "_ChargeName": "CHRG_SHIPPING",
    //                  "_ChargePerUnit": "0.76",
    //                  "_Reference": ""
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_DISC",
    //                  "_ChargeName": "DISC_SHIPPING",
    //                  "_ChargePerUnit": "0.77",
    //                  "_Reference": "FREESHIP75"
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SALES_DISC",
    //                  "_ChargeName": "DISC_PROMO",
    //                  "_ChargePerUnit": "13.04",
    //                  "_Reference": ""
    //              }
    //            ]
    //        },
    //        "LineTaxes": {
    //            "LineTax": [
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SALES",
    //                  "_Tax": "3.78000",
    //                  "_TaxName": "SALES",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SHIPPING",
    //                  "_Tax": "0.00000",
    //                  "_TaxName": "SHIPPING",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              }
    //            ]
    //        },
    //        "Notes": {
    //            "Note": [
    //              {
    //                  "_NoteText": "Hi",
    //                  "_ReasonCode": "GIFT_MESSAGE"
    //              },
    //              {
    //                  "_NoteText": "Garrett",
    //                  "_ReasonCode": "GIFT_FROM"
    //              },
    //              {
    //                  "_NoteText": "Sister",
    //                  "_ReasonCode": "GIFT_TO"
    //              }
    //            ]
    //        },
    //        "Extn": {
    //            "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
    //            "_ExtnClass": "",
    //            "_ExtnDepartment": "",
    //            "_ExtnGiftItemId": "",
    //            "_ExtnGiftPurchaseRecordID": "",
    //            "_ExtnGiftRegistryNo": "",
    //            "_ExtnGWP": "",
    //            "_ExtnIsPriceLocked": "Y",
    //            "_ExtnMPTID": "",
    //            "_ExtnMPTOfferDetail": "",
    //            "_ExtnMPTOfferMsg1": "",
    //            "_ExtnMPTOfferMsg2": "",
    //            "_ExtnParentLineNo": "",
    //            "_ExtnPriceStatus": "",
    //            "_ExtnPromoNumber": "",
    //            "_ExtnSpecialHandlingCd": "1",
    //            "_ExtnSPOID": "",
    //            "_ExtnSPOOfferDetail": "",
    //            "_ExtnSPOOfferMsg1": "",
    //            "_ExtnSPOOfferMsg2": "",
    //            "_ExtnSREligible": "1",
    //            "_ExtnTranID": ""
    //        },
    //        "btDisplay": {
    //            "itemDescription": "Dress Blue Medium",
    //            "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
    //            "itemType": "REG",
    //            "storageType": "R",
    //            "orderableInventory": 2000,
    //            "alternateUPCs": [
    //              "031290088157"
    //            ],
    //            "imageURL": "http://broker.bonton.com/123456",
    //            "brandLongDesc": "Carter's",
    //            "cfgDesc": "GIRLS",
    //            "classLongDesc": "CARTERS",
    //            "cmgDesc": "CHILDRENS",
    //            "colorAttrDesc": "Asst Lt/Pale",
    //            "colorFamDesc": "Asst Family",
    //            "colorLongDesc": "PRINT (969",
    //            "crgDesc": "CHILDRENS",
    //            "deptLongDesc": "GIRLSWEAR 2-6X",
    //            "desc1": "TEE",
    //            "desc2": "FALL CARTERS OP",
    //            "desc3": "CARTERS OCT",
    //            "desc4": "4-6X GIRL",
    //            "fabDtlDesc": "Screen Print",
    //            "fabLongDesc": "Cotton Blend-With Stretch",
    //            "fobDesc": "GIRLS 2-6X",
    //            "genClassLongDesc": "Knit Tops",
    //            "gensClaLongDesc": "Tee",
    //            "isn": "243306978",
    //            "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
    //            "itemSize": "4",
    //            "labelLongDesc": "Carter's",
    //            "prodDetail2": "Scoop Neck",
    //            "prodDetail3": "Knit-Interlock/Jersey",
    //            "prodDtlLongDesc": "Short Sleeve",
    //            "vendorStyleNum": "273B053"
    //        },
    //        "btLogic": {
    //            "isDeliveryAllowed": "N",
    //            "isHazmat": "N",
    //            "isParcelShippingAllowed": "Y",
    //            "isPickupAllowed": "N",
    //            "isReturnable": "Y",
    //            "isShippingAllowed": "Y",
    //            "isPriceOverridden": "Y",
    //            "priceOverrideValue": 21.33,
    //            "isBigTicket": "Y",
    //            "btIsWebExclusive": "false",
    //            "btIsWebOnlyEvent": "false",
    //            "btIsIVPEvent": "false",
    //            "btIsBonusEvent": "false",
    //            "btIsDoorBusterEvent": "false",
    //            "btIsNightOwlEvent": "false",
    //            "btIsOtherSpecialEvent": "false",
    //            "btIsSpecialHandling": "false",
    //            "btSpecialHandlingFee": "0"
    //        }
    //    });
    //    orderCart.orderLine.deleteOrderLine({ PrimeLine: 2, SubLine: 3 });
    //    expect(refCart.OrderLines.OrderLine.length).toEqual(1);
    //})
});

describe("Set Gift Message orderline from cart", function () {

    var orderCart;
    var refCart;
    var $state;

    beforeEach(angular.mock.module("appServiceOrderCart", "appUtilities", "appServicesWebSocket", "appServicesItem", "appServiceReprice", "appServicesCustomer"));
    beforeEach(module('ui.router'));
    beforeEach(inject(function (_orderCart_, _$state_) {
        $state = _$state_;
        orderCart = _orderCart_;
    }));

    beforeEach(function () {
        refCart = orderCart.order.getLiveOrderCart();
        angular.copy({

            //Order attributes
            AuthorizedClient: "Store", //"Store" for LUFI
            BillToID: "",
            CustomerContactID: "",
            CustomerEMailID: "garrett.stibb@bonton.com",
            DocumentType: "0001", //Alway "0001" for LUFI Sales Order
            DraftOrderFlag: "Y", // "Y" for Draft Order
            EnteredBy: "014265", // sales associates ID "014222"
            EnterpriseCode: "BONTON", //always "BONTON"
            EntryType: "Store", //channel order was created from "Store" for LUFI. Also saw "ELD" aka elderman
            OrderNo: "", //if blank Sterling will autogenerate new one
            PaymentRuleId: "BT_DEF_PAYMENT_RULE", //what payment rules are used
            SellerOrganizationCode: "101", //Store Node /number ex: "101"
            TaxExemptFlag: "N",
            TaxExemptionCertificate: "",

            //OrderLines
            OrderLines: {
                OrderLine: [{
                    "_CarrierServiceCode": "UPS-GRND",
                    "_DeliveryMethod": "DEL",
                    "_GiftFlag": "N",
                    "_GiftWrap": "N",
                    "_LevelOfService": "GRND",
                    "_OrderedQty": "2.0",
                    "_PrimeLineNo": 1,
                    "_SCAC": "UPS",
                    "_ScacAndService": "UPS-GRND",
                    "_SubLineNo": 1,
                    "Item": {
                        "_ItemID": "425800414756",
                        "_ProductClass": "NEW",
                        "_UnitCost": "26.98000",
                        "_UnitOfMeasure": "EACH",
                        "_UPCCode": "0888590925992"
                    },
                    "PersonInfoShipTo": {
                        "_AddressID": "LOGON5918 Alias like Home",
                        "_AddressLine1": "2412 56th ave",
                        "_AddressLine2": "Unit 3",
                        "_AddressLine3": "Bld 2",
                        "_City": "kenosha",
                        "_Country": "US",
                        "_DayPhone": "2629605804",
                        "_EMailID": "garrett.stibb@bonton.com",
                        "_EveningPhone": "1019991234",
                        "_FirstName": "Garrett",
                        "_LastName": "Stibb",
                        "_MiddleName": "C just initial ever",
                        "_PersonInfoKey": "20140819160910224059208",
                        "_State": "WI",
                        "_ZipCode": "53144"
                    },
                    "LinePriceInfo": {
                        "_ListPrice": "40.00000",
                        "_RetailPrice": "26.98000",
                        "_UnitPrice": "40.00"
                    },
                    "LineCharges": {
                        "LineCharge": [
                          {
                              "_ChargeCategory": "BTN_SHIP_CHRG",
                              "_ChargeName": "CHRG_SHIPPING",
                              "_ChargePerUnit": "0.76",
                              "_Reference": ""
                          },
                          {
                              "_ChargeCategory": "BTN_SHIP_DISC",
                              "_ChargeName": "DISC_SHIPPING",
                              "_ChargePerUnit": "0.77",
                              "_Reference": "FREESHIP75"
                          },
                          {
                              "_ChargeCategory": "BTN_SALES_DISC",
                              "_ChargeName": "DISC_PROMO",
                              "_ChargePerUnit": "13.04",
                              "_Reference": ""
                          }
                        ]
                    },
                    "LineTaxes": {
                        "LineTax": [
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SALES",
                              "_Tax": "3.78000",
                              "_TaxName": "SALES",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          },
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SHIPPING",
                              "_Tax": "0.00000",
                              "_TaxName": "SHIPPING",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          }
                        ]
                    },
                    "Notes": {
                        "Note": [
                          {
                              "_NoteText": "Hi",
                              "_ReasonCode": "GIFT_MESSAGE"
                          },
                          {
                              "_NoteText": "Garrett",
                              "_ReasonCode": "GIFT_FROM"
                          },
                          {
                              "_NoteText": "Sister",
                              "_ReasonCode": "GIFT_TO"
                          }
                        ]
                    },
                    "Extn": {
                        "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
                        "_ExtnClass": "",
                        "_ExtnDepartment": "",
                        "_ExtnGiftItemId": "",
                        "_ExtnGiftPurchaseRecordID": "",
                        "_ExtnGiftRegistryNo": "",
                        "_ExtnGWP": "",
                        "_ExtnIsPriceLocked": "Y",
                        "_ExtnMPTID": "",
                        "_ExtnMPTOfferDetail": "",
                        "_ExtnMPTOfferMsg1": "",
                        "_ExtnMPTOfferMsg2": "",
                        "_ExtnParentLineNo": "",
                        "_ExtnPriceStatus": "",
                        "_ExtnPromoNumber": "",
                        "_ExtnSpecialHandlingCd": "1",
                        "_ExtnSPOID": "",
                        "_ExtnSPOOfferDetail": "",
                        "_ExtnSPOOfferMsg1": "",
                        "_ExtnSPOOfferMsg2": "",
                        "_ExtnSREligible": "1",
                        "_ExtnTranID": ""
                    },
                    "btDisplay": {
                        "itemDescription": "Dress Blue Medium",
                        "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
                        "itemType": "REG",
                        "storageType": "R",
                        "orderableInventory": 2000,
                        "alternateUPCs": [
                          "031290088157"
                        ],
                        "imageURL": "http://broker.bonton.com/123456",
                        "brandLongDesc": "Carter's",
                        "cfgDesc": "GIRLS",
                        "classLongDesc": "CARTERS",
                        "cmgDesc": "CHILDRENS",
                        "colorAttrDesc": "Asst Lt/Pale",
                        "colorFamDesc": "Asst Family",
                        "colorLongDesc": "PRINT (969",
                        "crgDesc": "CHILDRENS",
                        "deptLongDesc": "GIRLSWEAR 2-6X",
                        "desc1": "TEE",
                        "desc2": "FALL CARTERS OP",
                        "desc3": "CARTERS OCT",
                        "desc4": "4-6X GIRL",
                        "fabDtlDesc": "Screen Print",
                        "fabLongDesc": "Cotton Blend-With Stretch",
                        "fobDesc": "GIRLS 2-6X",
                        "genClassLongDesc": "Knit Tops",
                        "gensClaLongDesc": "Tee",
                        "isn": "243306978",
                        "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
                        "itemSize": "4",
                        "labelLongDesc": "Carter's",
                        "prodDetail2": "Scoop Neck",
                        "prodDetail3": "Knit-Interlock/Jersey",
                        "prodDtlLongDesc": "Short Sleeve",
                        "vendorStyleNum": "273B053"
                    },
                    "btLogic": {
                        "isDeliveryAllowed": "N",
                        "isHazmat": "N",
                        "isParcelShippingAllowed": "Y",
                        "isPickupAllowed": "N",
                        "isReturnable": "Y",
                        "isShippingAllowed": "Y",
                        "isPriceOverridden": "Y",
                        "priceOverrideValue": 21.33,
                        "isBigTicket": "Y",
                        "btIsWebExclusive": "false",
                        "btIsWebOnlyEvent": "false",
                        "btIsIVPEvent": "false",
                        "btIsBonusEvent": "false",
                        "btIsDoorBusterEvent": "false",
                        "btIsNightOwlEvent": "false",
                        "btIsOtherSpecialEvent": "false",
                        "btIsSpecialHandling": "false",
                        "btSpecialHandlingFee": "0"
                    }
                }]
            },

            //PersonInfoBillTo Header Level
            PersonInfoBillTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },

            //PersonInfoShipTo Header Level
            PersonInfoShipTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },
            PaymentMethods: {                       //xml-json issue xml goes <PaymentMethods><PaymentMethod /><PaymentMethod /> ...
                //so do PaymentMethods:[]  and have broker know the tag needs to be PaymentMethod OR make another object {PaymentMethod:[]}
                PaymentMethod: [
                        {
                            UnlimitedCharges: "Y",
                            PaymentType: "CREDIT_CARD",
                            DisplayCreditCardNo: "0691",
                            CreditCardType: "CP",
                            CreditCardNo: "8231211627154941",
                            CreditCardExpDate: "04/2018",

                            PaymentDetails: {
                                ProcessedAmount: "102.65000",
                                ChargeType: "AUTHORIZATION",
                                AuthorizationID: "001060",
                                AuthCode: "001060"
                            }
                        }]
            },

            PriceInfo: { Currency: "USD" } //price infor child node
        }, refCart);
    });

    it("Control Case: Order has one OrderLine", function () {
        expect(refCart.OrderLines.OrderLine.length).toEqual(1);
    })

    it("Control Case: Order has gift message", function () {
        expect(refCart.OrderLines.OrderLine[0].Notes.Note[0]._NoteText).toEqual("Hi");
        expect(refCart.OrderLines.OrderLine[0].Notes.Note[1]._NoteText).toEqual("Garrett");
        expect(refCart.OrderLines.OrderLine[0].Notes.Note[2]._NoteText).toEqual("Sister");
    })

    //it("Replace Orderline 1's Gift messages", function () {
    //    orderCart.giftOptions.setGiftMessage([{ PrimeLine: "1", SubLine: 1 }], { To: "Jackie", From: "Garrett", Message: "Awesome Boss" });

    //    expect(refCart.OrderLines.OrderLine[0].Notes.Note.length).toEqual(3);
    //    expect(refCart.OrderLines.OrderLine[0].Notes.Note[0]._NoteText).toEqual("Jackie");
    //    expect(refCart.OrderLines.OrderLine[0].Notes.Note[1]._NoteText).toEqual("Garrett");
    //    expect(refCart.OrderLines.OrderLine[0].Notes.Note[2]._NoteText).toEqual("Awesome Boss");
    //})

    //it("Delete gift message only from second orderline", function () {
    //    refCart.OrderLines.OrderLine.push({
    //        "_CarrierServiceCode": "UPS-GRND",
    //        "_DeliveryMethod": "DEL",
    //        "_GiftFlag": "N",
    //        "_GiftWrap": "N",
    //        "_LevelOfService": "GRND",
    //        "_OrderedQty": "2.0",
    //        "_PrimeLineNo": 2,
    //        "_SCAC": "UPS",
    //        "_ScacAndService": "UPS-GRND",
    //        "_SubLineNo": 3,
    //        "Item": {
    //            "_ItemID": "425800414756",
    //            "_ProductClass": "NEW",
    //            "_UnitCost": "26.98000",
    //            "_UnitOfMeasure": "EACH",
    //            "_UPCCode": "0888590925992"
    //        },
    //        "PersonInfoShipTo": {
    //            "_AddressID": "LOGON5918 Alias like Home",
    //            "_AddressLine1": "2412 56th ave",
    //            "_AddressLine2": "Unit 3",
    //            "_AddressLine3": "Bld 2",
    //            "_City": "kenosha",
    //            "_Country": "US",
    //            "_DayPhone": "2629605804",
    //            "_EMailID": "garrett.stibb@bonton.com",
    //            "_EveningPhone": "1019991234",
    //            "_FirstName": "Garrett",
    //            "_LastName": "Stibb",
    //            "_MiddleName": "C just initial ever",
    //            "_PersonInfoKey": "20140819160910224059208",
    //            "_State": "WI",
    //            "_ZipCode": "53144"
    //        },
    //        "LinePriceInfo": {
    //            "_ListPrice": "40.00000",
    //            "_RetailPrice": "26.98000",
    //            "_UnitPrice": "40.00"
    //        },
    //        "LineCharges": {
    //            "LineCharge": [
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_CHRG",
    //                  "_ChargeName": "CHRG_SHIPPING",
    //                  "_ChargePerUnit": "0.76",
    //                  "_Reference": ""
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_DISC",
    //                  "_ChargeName": "DISC_SHIPPING",
    //                  "_ChargePerUnit": "0.77",
    //                  "_Reference": "FREESHIP75"
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SALES_DISC",
    //                  "_ChargeName": "DISC_PROMO",
    //                  "_ChargePerUnit": "13.04",
    //                  "_Reference": ""
    //              }
    //            ]
    //        },
    //        "LineTaxes": {
    //            "LineTax": [
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SALES",
    //                  "_Tax": "3.78000",
    //                  "_TaxName": "SALES",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SHIPPING",
    //                  "_Tax": "0.00000",
    //                  "_TaxName": "SHIPPING",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              }
    //            ]
    //        },
    //        "Notes": {
    //            "Note": [
    //              {
    //                  "_NoteText": "Hi",
    //                  "_ReasonCode": "GIFT_MESSAGE"
    //              },
    //              {
    //                  "_NoteText": "Garrett",
    //                  "_ReasonCode": "GIFT_FROM"
    //              },
    //              {
    //                  "_NoteText": "Sister",
    //                  "_ReasonCode": "GIFT_TO"
    //              }
    //            ]
    //        },
    //        "Extn": {
    //            "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
    //            "_ExtnClass": "",
    //            "_ExtnDepartment": "",
    //            "_ExtnGiftItemId": "",
    //            "_ExtnGiftPurchaseRecordID": "",
    //            "_ExtnGiftRegistryNo": "",
    //            "_ExtnGWP": "",
    //            "_ExtnIsPriceLocked": "Y",
    //            "_ExtnMPTID": "",
    //            "_ExtnMPTOfferDetail": "",
    //            "_ExtnMPTOfferMsg1": "",
    //            "_ExtnMPTOfferMsg2": "",
    //            "_ExtnParentLineNo": "",
    //            "_ExtnPriceStatus": "",
    //            "_ExtnPromoNumber": "",
    //            "_ExtnSpecialHandlingCd": "1",
    //            "_ExtnSPOID": "",
    //            "_ExtnSPOOfferDetail": "",
    //            "_ExtnSPOOfferMsg1": "",
    //            "_ExtnSPOOfferMsg2": "",
    //            "_ExtnSREligible": "1",
    //            "_ExtnTranID": ""
    //        },
    //        "btDisplay": {
    //            "itemDescription": "Dress Blue Medium",
    //            "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
    //            "itemType": "REG",
    //            "storageType": "R",
    //            "orderableInventory": 2000,
    //            "alternateUPCs": [
    //              "031290088157"
    //            ],
    //            "imageURL": "http://broker.bonton.com/123456",
    //            "brandLongDesc": "Carter's",
    //            "cfgDesc": "GIRLS",
    //            "classLongDesc": "CARTERS",
    //            "cmgDesc": "CHILDRENS",
    //            "colorAttrDesc": "Asst Lt/Pale",
    //            "colorFamDesc": "Asst Family",
    //            "colorLongDesc": "PRINT (969",
    //            "crgDesc": "CHILDRENS",
    //            "deptLongDesc": "GIRLSWEAR 2-6X",
    //            "desc1": "TEE",
    //            "desc2": "FALL CARTERS OP",
    //            "desc3": "CARTERS OCT",
    //            "desc4": "4-6X GIRL",
    //            "fabDtlDesc": "Screen Print",
    //            "fabLongDesc": "Cotton Blend-With Stretch",
    //            "fobDesc": "GIRLS 2-6X",
    //            "genClassLongDesc": "Knit Tops",
    //            "gensClaLongDesc": "Tee",
    //            "isn": "243306978",
    //            "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
    //            "itemSize": "4",
    //            "labelLongDesc": "Carter's",
    //            "prodDetail2": "Scoop Neck",
    //            "prodDetail3": "Knit-Interlock/Jersey",
    //            "prodDtlLongDesc": "Short Sleeve",
    //            "vendorStyleNum": "273B053"
    //        },
    //        "btLogic": {
    //            "isDeliveryAllowed": "N",
    //            "isHazmat": "N",
    //            "isParcelShippingAllowed": "Y",
    //            "isPickupAllowed": "N",
    //            "isReturnable": "Y",
    //            "isShippingAllowed": "Y",
    //            "isPriceOverridden": "Y",
    //            "priceOverrideValue": 21.33,
    //            "isBigTicket": "Y",
    //            "btIsWebExclusive": "false",
    //            "btIsWebOnlyEvent": "false",
    //            "btIsIVPEvent": "false",
    //            "btIsBonusEvent": "false",
    //            "btIsDoorBusterEvent": "false",
    //            "btIsNightOwlEvent": "false",
    //            "btIsOtherSpecialEvent": "false",
    //            "btIsSpecialHandling": "false",
    //            "btSpecialHandlingFee": "0"
    //        }
    //    });

    //    orderCart.giftOptions.setGiftMessage([{ PrimeLine: "2", SubLine: "3" }], {});

    //    expect(refCart.OrderLines.OrderLine[1].Notes.Note.length).toEqual(0);
    //})
});

describe("Get Gift Message from orderline", function () {

    var orderCart;
    var refCart;
    var $state;

    beforeEach(angular.mock.module("appServiceOrderCart", "appUtilities", "appServicesWebSocket", "appServicesItem", "appServiceReprice", "appServicesCustomer"));
    beforeEach(module('ui.router'));
    beforeEach(inject(function (_orderCart_, _$state_) {
        $state = _$state_;
        orderCart = _orderCart_;
    }));

    beforeEach(function () {
        refCart = orderCart.order.getLiveOrderCart();
        angular.copy({

            //Order attributes
            AuthorizedClient: "Store", //"Store" for LUFI
            BillToID: "",
            CustomerContactID: "",
            CustomerEMailID: "garrett.stibb@bonton.com",
            DocumentType: "0001", //Alway "0001" for LUFI Sales Order
            DraftOrderFlag: "Y", // "Y" for Draft Order
            EnteredBy: "014265", // sales associates ID "014222"
            EnterpriseCode: "BONTON", //always "BONTON"
            EntryType: "Store", //channel order was created from "Store" for LUFI. Also saw "ELD" aka elderman
            OrderNo: "", //if blank Sterling will autogenerate new one
            PaymentRuleId: "BT_DEF_PAYMENT_RULE", //what payment rules are used
            SellerOrganizationCode: "101", //Store Node /number ex: "101"
            TaxExemptFlag: "N",
            TaxExemptionCertificate: "",

            //OrderLines
            OrderLines: {
                OrderLine: [{
                    "_CarrierServiceCode": "UPS-GRND",
                    "_DeliveryMethod": "DEL",
                    "_GiftFlag": "N",
                    "_GiftWrap": "N",
                    "_LevelOfService": "GRND",
                    "_OrderedQty": "2.0",
                    "_PrimeLineNo": 1,
                    "_SCAC": "UPS",
                    "_ScacAndService": "UPS-GRND",
                    "_SubLineNo": 1,
                    "Item": {
                        "_ItemID": "425800414756",
                        "_ProductClass": "NEW",
                        "_UnitCost": "26.98000",
                        "_UnitOfMeasure": "EACH",
                        "_UPCCode": "0888590925992"
                    },
                    "PersonInfoShipTo": {
                        "_AddressID": "LOGON5918 Alias like Home",
                        "_AddressLine1": "2412 56th ave",
                        "_AddressLine2": "Unit 3",
                        "_AddressLine3": "Bld 2",
                        "_City": "kenosha",
                        "_Country": "US",
                        "_DayPhone": "2629605804",
                        "_EMailID": "garrett.stibb@bonton.com",
                        "_EveningPhone": "1019991234",
                        "_FirstName": "Garrett",
                        "_LastName": "Stibb",
                        "_MiddleName": "C just initial ever",
                        "_PersonInfoKey": "20140819160910224059208",
                        "_State": "WI",
                        "_ZipCode": "53144"
                    },
                    "LinePriceInfo": {
                        "_ListPrice": "40.00000",
                        "_RetailPrice": "26.98000",
                        "_UnitPrice": "40.00"
                    },
                    "LineCharges": {
                        "LineCharge": [
                          {
                              "_ChargeCategory": "BTN_SHIP_CHRG",
                              "_ChargeName": "CHRG_SHIPPING",
                              "_ChargePerUnit": "0.76",
                              "_Reference": ""
                          },
                          {
                              "_ChargeCategory": "BTN_SHIP_DISC",
                              "_ChargeName": "DISC_SHIPPING",
                              "_ChargePerUnit": "0.77",
                              "_Reference": "FREESHIP75"
                          },
                          {
                              "_ChargeCategory": "BTN_SALES_DISC",
                              "_ChargeName": "DISC_PROMO",
                              "_ChargePerUnit": "13.04",
                              "_Reference": ""
                          }
                        ]
                    },
                    "LineTaxes": {
                        "LineTax": [
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SALES",
                              "_Tax": "3.78000",
                              "_TaxName": "SALES",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          },
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SHIPPING",
                              "_Tax": "0.00000",
                              "_TaxName": "SHIPPING",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          }
                        ]
                    },
                    "Notes": {
                        "Note": [
                          {
                              "_NoteText": "Hi",
                              "_ReasonCode": "GIFT_MESSAGE"
                          },
                          {
                              "_NoteText": "Garrett",
                              "_ReasonCode": "GIFT_FROM"
                          },
                          {
                              "_NoteText": "Sister",
                              "_ReasonCode": "GIFT_TO"
                          }
                        ]
                    },
                    "Extn": {
                        "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
                        "_ExtnClass": "",
                        "_ExtnDepartment": "",
                        "_ExtnGiftItemId": "",
                        "_ExtnGiftPurchaseRecordID": "",
                        "_ExtnGiftRegistryNo": "",
                        "_ExtnGWP": "",
                        "_ExtnIsPriceLocked": "Y",
                        "_ExtnMPTID": "",
                        "_ExtnMPTOfferDetail": "",
                        "_ExtnMPTOfferMsg1": "",
                        "_ExtnMPTOfferMsg2": "",
                        "_ExtnParentLineNo": "",
                        "_ExtnPriceStatus": "",
                        "_ExtnPromoNumber": "",
                        "_ExtnSpecialHandlingCd": "1",
                        "_ExtnSPOID": "",
                        "_ExtnSPOOfferDetail": "",
                        "_ExtnSPOOfferMsg1": "",
                        "_ExtnSPOOfferMsg2": "",
                        "_ExtnSREligible": "1",
                        "_ExtnTranID": ""
                    },
                    "btDisplay": {
                        "itemDescription": "Dress Blue Medium",
                        "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
                        "itemType": "REG",
                        "storageType": "R",
                        "orderableInventory": 2000,
                        "alternateUPCs": [
                          "031290088157"
                        ],
                        "imageURL": "http://broker.bonton.com/123456",
                        "brandLongDesc": "Carter's",
                        "cfgDesc": "GIRLS",
                        "classLongDesc": "CARTERS",
                        "cmgDesc": "CHILDRENS",
                        "colorAttrDesc": "Asst Lt/Pale",
                        "colorFamDesc": "Asst Family",
                        "colorLongDesc": "PRINT (969",
                        "crgDesc": "CHILDRENS",
                        "deptLongDesc": "GIRLSWEAR 2-6X",
                        "desc1": "TEE",
                        "desc2": "FALL CARTERS OP",
                        "desc3": "CARTERS OCT",
                        "desc4": "4-6X GIRL",
                        "fabDtlDesc": "Screen Print",
                        "fabLongDesc": "Cotton Blend-With Stretch",
                        "fobDesc": "GIRLS 2-6X",
                        "genClassLongDesc": "Knit Tops",
                        "gensClaLongDesc": "Tee",
                        "isn": "243306978",
                        "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
                        "itemSize": "4",
                        "labelLongDesc": "Carter's",
                        "prodDetail2": "Scoop Neck",
                        "prodDetail3": "Knit-Interlock/Jersey",
                        "prodDtlLongDesc": "Short Sleeve",
                        "vendorStyleNum": "273B053"
                    },
                    "btLogic": {
                        "isDeliveryAllowed": "N",
                        "isHazmat": "N",
                        "isParcelShippingAllowed": "Y",
                        "isPickupAllowed": "N",
                        "isReturnable": "Y",
                        "isShippingAllowed": "Y",
                        "isPriceOverridden": "Y",
                        "priceOverrideValue": 21.33,
                        "isBigTicket": "Y",
                        "btIsWebExclusive": "false",
                        "btIsWebOnlyEvent": "false",
                        "btIsIVPEvent": "false",
                        "btIsBonusEvent": "false",
                        "btIsDoorBusterEvent": "false",
                        "btIsNightOwlEvent": "false",
                        "btIsOtherSpecialEvent": "false",
                        "btIsSpecialHandling": "false",
                        "btSpecialHandlingFee": "0"
                    }
                }]
            },

            //PersonInfoBillTo Header Level
            PersonInfoBillTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },

            //PersonInfoShipTo Header Level
            PersonInfoShipTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },
            PaymentMethods: {                       //xml-json issue xml goes <PaymentMethods><PaymentMethod /><PaymentMethod /> ...
                //so do PaymentMethods:[]  and have broker know the tag needs to be PaymentMethod OR make another object {PaymentMethod:[]}
                PaymentMethod: [
                        {
                            UnlimitedCharges: "Y",
                            PaymentType: "CREDIT_CARD",
                            DisplayCreditCardNo: "0691",
                            CreditCardType: "CP",
                            CreditCardNo: "8231211627154941",
                            CreditCardExpDate: "04/2018",

                            PaymentDetails: {
                                ProcessedAmount: "102.65000",
                                ChargeType: "AUTHORIZATION",
                                AuthorizationID: "001060",
                                AuthCode: "001060"
                            }
                        }]
            },

            PriceInfo: { Currency: "USD" } //price infor child node
        }, refCart);
    });

    it("Control Case: Order has one OrderLine", function () {
        expect(refCart.OrderLines.OrderLine.length).toEqual(1);
    })

    it("Control Case: Order has gift message", function () {
        expect(refCart.OrderLines.OrderLine[0].Notes.Note[0]._NoteText).toEqual("Hi");
        expect(refCart.OrderLines.OrderLine[0].Notes.Note[1]._NoteText).toEqual("Garrett");
        expect(refCart.OrderLines.OrderLine[0].Notes.Note[2]._NoteText).toEqual("Sister");
    })

    //it("Get giftMessages", function () {

    //    var giftObject = orderCart.giftOptions.getGiftOptionFromOrderLines({ PrimeLine: 1, SubLine: 1 });
    //    expect(giftObject.To).toEqual("Sister");
    //    expect(giftObject.From).toEqual("Garrett");
    //    expect(giftObject.Message).toEqual("Hi");
    //})
});

describe("Set OrderLine Quantity", function () {

    var orderCart;
    var refCart;
    var $state;

    beforeEach(angular.mock.module("appServiceOrderCart", "appUtilities", "appServicesWebSocket", "appServicesItem", "appServiceReprice", "appServicesCustomer"));
    beforeEach(module('ui.router'));
    beforeEach(inject(function (_orderCart_, _$state_) {
        $state = _$state_;
        orderCart = _orderCart_;
    }));

    beforeEach(function () {
        refCart = orderCart.order.getLiveOrderCart();
        angular.copy({

            //Order attributes
            AuthorizedClient: "Store", //"Store" for LUFI
            BillToID: "",
            CustomerContactID: "",
            CustomerEMailID: "garrett.stibb@bonton.com",
            DocumentType: "0001", //Alway "0001" for LUFI Sales Order
            DraftOrderFlag: "Y", // "Y" for Draft Order
            EnteredBy: "014265", // sales associates ID "014222"
            EnterpriseCode: "BONTON", //always "BONTON"
            EntryType: "Store", //channel order was created from "Store" for LUFI. Also saw "ELD" aka elderman
            OrderNo: "", //if blank Sterling will autogenerate new one
            PaymentRuleId: "BT_DEF_PAYMENT_RULE", //what payment rules are used
            SellerOrganizationCode: "101", //Store Node /number ex: "101"
            TaxExemptFlag: "N",
            TaxExemptionCertificate: "",

            //OrderLines
            OrderLines: {
                OrderLine: [{
                    "_CarrierServiceCode": "UPS-GRND",
                    "_DeliveryMethod": "DEL",
                    "_GiftFlag": "N",
                    "_GiftWrap": "N",
                    "_LevelOfService": "GRND",
                    "_OrderedQty": "13.0",
                    "_PrimeLineNo": 1,
                    "_SCAC": "UPS",
                    "_ScacAndService": "UPS-GRND",
                    "_SubLineNo": 1,
                    "Item": {
                        "_ItemID": "425800414756",
                        "_ProductClass": "NEW",
                        "_UnitCost": "26.98000",
                        "_UnitOfMeasure": "EACH",
                        "_UPCCode": "0888590925992"
                    },
                    "PersonInfoShipTo": {
                        "_AddressID": "LOGON5918 Alias like Home",
                        "_AddressLine1": "2412 56th ave",
                        "_AddressLine2": "Unit 3",
                        "_AddressLine3": "Bld 2",
                        "_City": "kenosha",
                        "_Country": "US",
                        "_DayPhone": "2629605804",
                        "_EMailID": "garrett.stibb@bonton.com",
                        "_EveningPhone": "1019991234",
                        "_FirstName": "Garrett",
                        "_LastName": "Stibb",
                        "_MiddleName": "C just initial ever",
                        "_PersonInfoKey": "20140819160910224059208",
                        "_State": "WI",
                        "_ZipCode": "53144"
                    },
                    "LinePriceInfo": {
                        "_ListPrice": "40.00000",
                        "_RetailPrice": "26.98000",
                        "_UnitPrice": "40.00"
                    },
                    "LineCharges": {
                        "LineCharge": [
                          {
                              "_ChargeCategory": "BTN_SHIP_CHRG",
                              "_ChargeName": "CHRG_SHIPPING",
                              "_ChargePerUnit": "0.76",
                              "_Reference": ""
                          },
                          {
                              "_ChargeCategory": "BTN_SHIP_DISC",
                              "_ChargeName": "DISC_SHIPPING",
                              "_ChargePerUnit": "0.77",
                              "_Reference": "FREESHIP75"
                          },
                          {
                              "_ChargeCategory": "BTN_SALES_DISC",
                              "_ChargeName": "DISC_PROMO",
                              "_ChargePerUnit": "13.04",
                              "_Reference": ""
                          }
                        ]
                    },
                    "LineTaxes": {
                        "LineTax": [
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SALES",
                              "_Tax": "3.78000",
                              "_TaxName": "SALES",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          },
                          {
                              "_ChargeCategory": "BTN_TAX_CHRG",
                              "_ChargeName": "TAX_SHIPPING",
                              "_Tax": "0.00000",
                              "_TaxName": "SHIPPING",
                              "Extn": {
                                  "_ExtnTaxPerUnit": "1.89"
                              }
                          }
                        ]
                    },
                    "Notes": {
                        "Note": [
                          {
                              "_NoteText": "Hi",
                              "_ReasonCode": "GIFT_MESSAGE"
                          },
                          {
                              "_NoteText": "Garrett",
                              "_ReasonCode": "GIFT_FROM"
                          },
                          {
                              "_NoteText": "Sister",
                              "_ReasonCode": "GIFT_TO"
                          }
                        ]
                    },
                    "Extn": {
                        "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
                        "_ExtnClass": "",
                        "_ExtnDepartment": "",
                        "_ExtnGiftItemId": "",
                        "_ExtnGiftPurchaseRecordID": "",
                        "_ExtnGiftRegistryNo": "",
                        "_ExtnGWP": "",
                        "_ExtnIsPriceLocked": "Y",
                        "_ExtnMPTID": "",
                        "_ExtnMPTOfferDetail": "",
                        "_ExtnMPTOfferMsg1": "",
                        "_ExtnMPTOfferMsg2": "",
                        "_ExtnParentLineNo": "",
                        "_ExtnPriceStatus": "",
                        "_ExtnPromoNumber": "",
                        "_ExtnSpecialHandlingCd": "1",
                        "_ExtnSPOID": "",
                        "_ExtnSPOOfferDetail": "",
                        "_ExtnSPOOfferMsg1": "",
                        "_ExtnSPOOfferMsg2": "",
                        "_ExtnSREligible": "1",
                        "_ExtnTranID": ""
                    },
                    "btDisplay": {
                        "itemDescription": "Dress Blue Medium",
                        "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
                        "itemType": "REG",
                        "storageType": "R",
                        "orderableInventory": 15,
                        "alternateUPCs": [
                          "031290088157"
                        ],
                        "imageURL": "http://broker.bonton.com/123456",
                        "brandLongDesc": "Carter's",
                        "cfgDesc": "GIRLS",
                        "classLongDesc": "CARTERS",
                        "cmgDesc": "CHILDRENS",
                        "colorAttrDesc": "Asst Lt/Pale",
                        "colorFamDesc": "Asst Family",
                        "colorLongDesc": "PRINT (969",
                        "crgDesc": "CHILDRENS",
                        "deptLongDesc": "GIRLSWEAR 2-6X",
                        "desc1": "TEE",
                        "desc2": "FALL CARTERS OP",
                        "desc3": "CARTERS OCT",
                        "desc4": "4-6X GIRL",
                        "fabDtlDesc": "Screen Print",
                        "fabLongDesc": "Cotton Blend-With Stretch",
                        "fobDesc": "GIRLS 2-6X",
                        "genClassLongDesc": "Knit Tops",
                        "gensClaLongDesc": "Tee",
                        "isn": "243306978",
                        "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
                        "itemSize": "4",
                        "labelLongDesc": "Carter's",
                        "prodDetail2": "Scoop Neck",
                        "prodDetail3": "Knit-Interlock/Jersey",
                        "prodDtlLongDesc": "Short Sleeve",
                        "vendorStyleNum": "273B053"
                    },
                    "btLogic": {
                        "isDeliveryAllowed": "N",
                        "isHazmat": "N",
                        "isParcelShippingAllowed": "Y",
                        "isPickupAllowed": "N",
                        "isReturnable": "Y",
                        "isShippingAllowed": "Y",
                        "isPriceOverridden": "Y",
                        "priceOverrideValue": 21.33,
                        "isBigTicket": "Y",
                        "btIsWebExclusive": "false",
                        "btIsWebOnlyEvent": "false",
                        "btIsIVPEvent": "false",
                        "btIsBonusEvent": "false",
                        "btIsDoorBusterEvent": "false",
                        "btIsNightOwlEvent": "false",
                        "btIsOtherSpecialEvent": "false",
                        "btIsSpecialHandling": "false",
                        "btSpecialHandlingFee": "0"
                    }
                }]
            },

            //PersonInfoBillTo Header Level
            PersonInfoBillTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },

            //PersonInfoShipTo Header Level
            PersonInfoShipTo: {
                AddressID: "LOGON5918",
                AddressLine1: "2412 56th ave",
                City: "kenosha",
                Country: "US",
                DayPhone: "2629605804",
                EMailID: "garrett.stibb@bonton.com",
                FirstName: "Garrett",
                LastName: "Stibb",
                PersonID: "8701684",
                PersonInfoKey: "20140819160910224059208",
                State: "WI",
                ZipCode: "53144"
            },
            PaymentMethods: {                       //xml-json issue xml goes <PaymentMethods><PaymentMethod /><PaymentMethod /> ...
                //so do PaymentMethods:[]  and have broker know the tag needs to be PaymentMethod OR make another object {PaymentMethod:[]}
                PaymentMethod: [
                        {
                            UnlimitedCharges: "Y",
                            PaymentType: "CREDIT_CARD",
                            DisplayCreditCardNo: "0691",
                            CreditCardType: "CP",
                            CreditCardNo: "8231211627154941",
                            CreditCardExpDate: "04/2018",

                            PaymentDetails: {
                                ProcessedAmount: "102.65000",
                                ChargeType: "AUTHORIZATION",
                                AuthorizationID: "001060",
                                AuthCode: "001060"
                            }
                        }]
            },

            PriceInfo: { Currency: "USD" } //price infor child node
        }, refCart);
    });

    it("Control Case: Order has one OrderLine", function () {
        expect(refCart.OrderLines.OrderLine.length).toEqual(1);
    })

    it("Decrease Orderline", function () {
        orderCart.orderLine.setOrderLineQuantity({ PrimeLine: 1, SubLine: 1 }, 12);
        expect(parseInt(refCart.OrderLines.OrderLine[0]._OrderedQty)).toEqual(12);
    })

    //it("Increase Orderline", function () {
    //    expect(orderCart.orderLine.setOrderLineQuantity({ PrimeLine: 1, SubLine: 1 }, 14)).toBeTruthy();
    //    expect(parseInt(refCart.OrderLines.OrderLine[0]._OrderedQty)).toEqual(14);
    //})

    //it("Return false for OrderLine not found", function () {
    //    expect(orderCart.orderLine.setOrderLineQuantity({ PrimeLine: 2, SubLine: 1 }, 2)).toBeFalsy();
    //})

    //it("Return false for no quantity defined", function () {
    //    expect(orderCart.orderLine.setOrderLineQuantity({ PrimeLine: 1, SubLine: 1 })).toBeFalsy();
    //})

    //it("Return false for null primeLine subLine Object", function () {
    //    expect(orderCart.orderLine.setOrderLineQuantity(null, 1)).toBeFalsy();
    //})

    //it("Increase Orderline above availabe inventory", function () {
    //    expect(orderCart.orderLine.setOrderLineQuantity({ PrimeLine: 1, SubLine: 1 }, 20)).toBeTruthy();
    //    expect(parseInt(refCart.OrderLines.OrderLine[0]._OrderedQty)).toEqual(15);
    //})

    //it("Increase Second OrderLine to over available quantity.", function () {
    //    refCart.OrderLines.OrderLine.push({
    //        "_CarrierServiceCode": "UPS-GRND",
    //        "_DeliveryMethod": "DEL",
    //        "_GiftFlag": "N",
    //        "_GiftWrap": "N",
    //        "_LevelOfService": "GRND",
    //        "_OrderedQty": "2.0",
    //        "_PrimeLineNo": 2,
    //        "_SCAC": "UPS",
    //        "_ScacAndService": "UPS-GRND",
    //        "_SubLineNo": 3,
    //        "Item": {
    //            "_ItemID": "425800414756",
    //            "_ProductClass": "NEW",
    //            "_UnitCost": "26.98000",
    //            "_UnitOfMeasure": "EACH",
    //            "_UPCCode": "0888590925992"
    //        },
    //        "PersonInfoShipTo": {
    //            "_AddressID": "LOGON5918 Alias like Home",
    //            "_AddressLine1": "2412 56th ave",
    //            "_AddressLine2": "Unit 3",
    //            "_AddressLine3": "Bld 2",
    //            "_City": "kenosha",
    //            "_Country": "US",
    //            "_DayPhone": "2629605804",
    //            "_EMailID": "garrett.stibb@bonton.com",
    //            "_EveningPhone": "1019991234",
    //            "_FirstName": "Garrett",
    //            "_LastName": "Stibb",
    //            "_MiddleName": "C just initial ever",
    //            "_PersonInfoKey": "20140819160910224059208",
    //            "_State": "WI",
    //            "_ZipCode": "53144"
    //        },
    //        "LinePriceInfo": {
    //            "_ListPrice": "40.00000",
    //            "_RetailPrice": "26.98000",
    //            "_UnitPrice": "40.00"
    //        },
    //        "LineCharges": {
    //            "LineCharge": [
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_CHRG",
    //                  "_ChargeName": "CHRG_SHIPPING",
    //                  "_ChargePerUnit": "0.76",
    //                  "_Reference": ""
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_DISC",
    //                  "_ChargeName": "DISC_SHIPPING",
    //                  "_ChargePerUnit": "0.77",
    //                  "_Reference": "FREESHIP75"
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SALES_DISC",
    //                  "_ChargeName": "DISC_PROMO",
    //                  "_ChargePerUnit": "13.04",
    //                  "_Reference": ""
    //              }
    //            ]
    //        },
    //        "LineTaxes": {
    //            "LineTax": [
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SALES",
    //                  "_Tax": "3.78000",
    //                  "_TaxName": "SALES",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SHIPPING",
    //                  "_Tax": "0.00000",
    //                  "_TaxName": "SHIPPING",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              }
    //            ]
    //        },
    //        "Notes": {
    //            "Note": [
    //              {
    //                  "_NoteText": "Hi",
    //                  "_ReasonCode": "GIFT_MESSAGE"
    //              },
    //              {
    //                  "_NoteText": "Garrett",
    //                  "_ReasonCode": "GIFT_FROM"
    //              },
    //              {
    //                  "_NoteText": "Sister",
    //                  "_ReasonCode": "GIFT_TO"
    //              }
    //            ]
    //        },
    //        "Extn": {
    //            "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
    //            "_ExtnClass": "",
    //            "_ExtnDepartment": "",
    //            "_ExtnGiftItemId": "",
    //            "_ExtnGiftPurchaseRecordID": "",
    //            "_ExtnGiftRegistryNo": "",
    //            "_ExtnGWP": "",
    //            "_ExtnIsPriceLocked": "Y",
    //            "_ExtnMPTID": "",
    //            "_ExtnMPTOfferDetail": "",
    //            "_ExtnMPTOfferMsg1": "",
    //            "_ExtnMPTOfferMsg2": "",
    //            "_ExtnParentLineNo": "",
    //            "_ExtnPriceStatus": "",
    //            "_ExtnPromoNumber": "",
    //            "_ExtnSpecialHandlingCd": "1",
    //            "_ExtnSPOID": "",
    //            "_ExtnSPOOfferDetail": "",
    //            "_ExtnSPOOfferMsg1": "",
    //            "_ExtnSPOOfferMsg2": "",
    //            "_ExtnSREligible": "1",
    //            "_ExtnTranID": ""
    //        },
    //        "btDisplay": {
    //            "itemDescription": "Dress Blue Medium",
    //            "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
    //            "itemType": "REG",
    //            "storageType": "R",
    //            "orderableInventory": 15,
    //            "alternateUPCs": [
    //              "031290088157"
    //            ],
    //            "imageURL": "http://broker.bonton.com/123456",
    //            "brandLongDesc": "Carter's",
    //            "cfgDesc": "GIRLS",
    //            "classLongDesc": "CARTERS",
    //            "cmgDesc": "CHILDRENS",
    //            "colorAttrDesc": "Asst Lt/Pale",
    //            "colorFamDesc": "Asst Family",
    //            "colorLongDesc": "PRINT (969",
    //            "crgDesc": "CHILDRENS",
    //            "deptLongDesc": "GIRLSWEAR 2-6X",
    //            "desc1": "TEE",
    //            "desc2": "FALL CARTERS OP",
    //            "desc3": "CARTERS OCT",
    //            "desc4": "4-6X GIRL",
    //            "fabDtlDesc": "Screen Print",
    //            "fabLongDesc": "Cotton Blend-With Stretch",
    //            "fobDesc": "GIRLS 2-6X",
    //            "genClassLongDesc": "Knit Tops",
    //            "gensClaLongDesc": "Tee",
    //            "isn": "243306978",
    //            "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
    //            "itemSize": "4",
    //            "labelLongDesc": "Carter's",
    //            "prodDetail2": "Scoop Neck",
    //            "prodDetail3": "Knit-Interlock/Jersey",
    //            "prodDtlLongDesc": "Short Sleeve",
    //            "vendorStyleNum": "273B053"
    //        },
    //        "btLogic": {
    //            "isDeliveryAllowed": "N",
    //            "isHazmat": "N",
    //            "isParcelShippingAllowed": "Y",
    //            "isPickupAllowed": "N",
    //            "isReturnable": "Y",
    //            "isShippingAllowed": "Y",
    //            "isPriceOverridden": "Y",
    //            "priceOverrideValue": 21.33,
    //            "isBigTicket": "Y",
    //            "btIsWebExclusive": "false",
    //            "btIsWebOnlyEvent": "false",
    //            "btIsIVPEvent": "false",
    //            "btIsBonusEvent": "false",
    //            "btIsDoorBusterEvent": "false",
    //            "btIsNightOwlEvent": "false",
    //            "btIsOtherSpecialEvent": "false",
    //            "btIsSpecialHandling": "false",
    //            "btSpecialHandlingFee": "0"
    //        }
    //    });
    //    expect(orderCart.orderLine.setOrderLineQuantity({ PrimeLine: 2, SubLine: 3 }, 20)).toBeTruthy();
    //    expect(parseInt(refCart.OrderLines.OrderLine[1]._OrderedQty)).toEqual(2);
    //})

    //it("Increase Second OrderLine to highest available quantity of all orderlines.", function () {
    //    refCart.OrderLines.OrderLine[0].btDisplay.orderableInventory = 20;
    //    refCart.OrderLines.OrderLine.push({
    //        "_CarrierServiceCode": "UPS-GRND",
    //        "_DeliveryMethod": "DEL",
    //        "_GiftFlag": "N",
    //        "_GiftWrap": "N",
    //        "_LevelOfService": "GRND",
    //        "_OrderedQty": "2.0",
    //        "_PrimeLineNo": 2,
    //        "_SCAC": "UPS",
    //        "_ScacAndService": "UPS-GRND",
    //        "_SubLineNo": 3,
    //        "Item": {
    //            "_ItemID": "425800414756",
    //            "_ProductClass": "NEW",
    //            "_UnitCost": "26.98000",
    //            "_UnitOfMeasure": "EACH",
    //            "_UPCCode": "0888590925992"
    //        },
    //        "PersonInfoShipTo": {
    //            "_AddressID": "LOGON5918 Alias like Home",
    //            "_AddressLine1": "2412 56th ave",
    //            "_AddressLine2": "Unit 3",
    //            "_AddressLine3": "Bld 2",
    //            "_City": "kenosha",
    //            "_Country": "US",
    //            "_DayPhone": "2629605804",
    //            "_EMailID": "garrett.stibb@bonton.com",
    //            "_EveningPhone": "1019991234",
    //            "_FirstName": "Garrett",
    //            "_LastName": "Stibb",
    //            "_MiddleName": "C just initial ever",
    //            "_PersonInfoKey": "20140819160910224059208",
    //            "_State": "WI",
    //            "_ZipCode": "53144"
    //        },
    //        "LinePriceInfo": {
    //            "_ListPrice": "40.00000",
    //            "_RetailPrice": "26.98000",
    //            "_UnitPrice": "40.00"
    //        },
    //        "LineCharges": {
    //            "LineCharge": [
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_CHRG",
    //                  "_ChargeName": "CHRG_SHIPPING",
    //                  "_ChargePerUnit": "0.76",
    //                  "_Reference": ""
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SHIP_DISC",
    //                  "_ChargeName": "DISC_SHIPPING",
    //                  "_ChargePerUnit": "0.77",
    //                  "_Reference": "FREESHIP75"
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_SALES_DISC",
    //                  "_ChargeName": "DISC_PROMO",
    //                  "_ChargePerUnit": "13.04",
    //                  "_Reference": ""
    //              }
    //            ]
    //        },
    //        "LineTaxes": {
    //            "LineTax": [
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SALES",
    //                  "_Tax": "3.78000",
    //                  "_TaxName": "SALES",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              },
    //              {
    //                  "_ChargeCategory": "BTN_TAX_CHRG",
    //                  "_ChargeName": "TAX_SHIPPING",
    //                  "_Tax": "0.00000",
    //                  "_TaxName": "SHIPPING",
    //                  "Extn": {
    //                      "_ExtnTaxPerUnit": "1.89"
    //                  }
    //              }
    //            ]
    //        },
    //        "Notes": {
    //            "Note": [
    //              {
    //                  "_NoteText": "Hi",
    //                  "_ReasonCode": "GIFT_MESSAGE"
    //              },
    //              {
    //                  "_NoteText": "Garrett",
    //                  "_ReasonCode": "GIFT_FROM"
    //              },
    //              {
    //                  "_NoteText": "Sister",
    //                  "_ReasonCode": "GIFT_TO"
    //              }
    //            ]
    //        },
    //        "Extn": {
    //            "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
    //            "_ExtnClass": "",
    //            "_ExtnDepartment": "",
    //            "_ExtnGiftItemId": "",
    //            "_ExtnGiftPurchaseRecordID": "",
    //            "_ExtnGiftRegistryNo": "",
    //            "_ExtnGWP": "",
    //            "_ExtnIsPriceLocked": "Y",
    //            "_ExtnMPTID": "",
    //            "_ExtnMPTOfferDetail": "",
    //            "_ExtnMPTOfferMsg1": "",
    //            "_ExtnMPTOfferMsg2": "",
    //            "_ExtnParentLineNo": "",
    //            "_ExtnPriceStatus": "",
    //            "_ExtnPromoNumber": "",
    //            "_ExtnSpecialHandlingCd": "1",
    //            "_ExtnSPOID": "",
    //            "_ExtnSPOOfferDetail": "",
    //            "_ExtnSPOOfferMsg1": "",
    //            "_ExtnSPOOfferMsg2": "",
    //            "_ExtnSREligible": "1",
    //            "_ExtnTranID": ""
    //        },
    //        "btDisplay": {
    //            "itemDescription": "Dress Blue Medium",
    //            "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
    //            "itemType": "REG",
    //            "storageType": "R",
    //            "orderableInventory": 15,
    //            "alternateUPCs": [
    //              "031290088157"
    //            ],
    //            "imageURL": "http://broker.bonton.com/123456",
    //            "brandLongDesc": "Carter's",
    //            "cfgDesc": "GIRLS",
    //            "classLongDesc": "CARTERS",
    //            "cmgDesc": "CHILDRENS",
    //            "colorAttrDesc": "Asst Lt/Pale",
    //            "colorFamDesc": "Asst Family",
    //            "colorLongDesc": "PRINT (969",
    //            "crgDesc": "CHILDRENS",
    //            "deptLongDesc": "GIRLSWEAR 2-6X",
    //            "desc1": "TEE",
    //            "desc2": "FALL CARTERS OP",
    //            "desc3": "CARTERS OCT",
    //            "desc4": "4-6X GIRL",
    //            "fabDtlDesc": "Screen Print",
    //            "fabLongDesc": "Cotton Blend-With Stretch",
    //            "fobDesc": "GIRLS 2-6X",
    //            "genClassLongDesc": "Knit Tops",
    //            "gensClaLongDesc": "Tee",
    //            "isn": "243306978",
    //            "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
    //            "itemSize": "4",
    //            "labelLongDesc": "Carter's",
    //            "prodDetail2": "Scoop Neck",
    //            "prodDetail3": "Knit-Interlock/Jersey",
    //            "prodDtlLongDesc": "Short Sleeve",
    //            "vendorStyleNum": "273B053"
    //        },
    //        "btLogic": {
    //            "isDeliveryAllowed": "N",
    //            "isHazmat": "N",
    //            "isParcelShippingAllowed": "Y",
    //            "isPickupAllowed": "N",
    //            "isReturnable": "Y",
    //            "isShippingAllowed": "Y",
    //            "isPriceOverridden": "Y",
    //            "priceOverrideValue": 21.33,
    //            "isBigTicket": "Y",
    //            "btIsWebExclusive": "false",
    //            "btIsWebOnlyEvent": "false",
    //            "btIsIVPEvent": "false",
    //            "btIsBonusEvent": "false",
    //            "btIsDoorBusterEvent": "false",
    //            "btIsNightOwlEvent": "false",
    //            "btIsOtherSpecialEvent": "false",
    //            "btIsSpecialHandling": "false",
    //            "btSpecialHandlingFee": "0"
    //        }
    //    });
    //    expect(orderCart.orderLine.setOrderLineQuantity({ PrimeLine: 2, SubLine: 3 }, 20)).toBeTruthy();
    //    expect(parseInt(refCart.OrderLines.OrderLine[1]._OrderedQty)).toEqual(7);
    //})
});



describe("Padding Utility: ", function () {

    var orderCart;
    var $state;

    beforeEach(angular.mock.module("appServiceOrderCart", "appUtilities", "appServicesWebSocket", "appServicesItem", "appServiceReprice", "appServicesCustomer"));
    beforeEach(module('ui.router'));
    beforeEach(inject(function (_orderCart_, _$state_) {
        $state = _$state_;
        orderCart = _orderCart_;
    }));

    it("pad with i", function () {
        expect(orderCart.util.leftPad(24, 12, "i")).toEqual("iiiiiiiiii24");
    });

    it("pad to twelve", function () {
        expect(orderCart.util.leftPad(24, 12)).toEqual("000000000024");
    })

    it("pad string to twelve chars", function () {
        expect(orderCart.util.leftPad("24", 12)).toEqual("000000000024");
    })

    it("pad error no length specified", function () {
        expect(orderCart.util.leftPad("24")).toBeNull();
    })

    it("pad no inputted parameters", function () {
        expect(orderCart.util.leftPad()).toBeNull();
    })

    it("pad error no length specified", function () {
        expect(orderCart.util.leftPad("", 6)).toEqual("000000");
    })

    it("pad with long optional string", function () {
        expect(orderCart.util.leftPad("", 12, "Beast")).toEqual("stBeastBeast");
    })
});

describe("Set Customer", function () {

    var orderCart;
    var refCart;
    var $state;

    beforeEach(angular.mock.module("appServiceOrderCart", "appUtilities", "appServicesWebSocket", "appServicesItem", "appServiceReprice", "appServicesCustomer"));
    beforeEach(module('ui.router'));
    beforeEach(inject(function (_orderCart_, _$state_) {
        $state = _$state_;
        orderCart = _orderCart_;
    }));

    beforeEach(function () {
        refCart = orderCart.order.getLiveOrderCart();
        angular.copy({
            "_AuthorizedClient": "Store",
            "_BillToID": "",
            "_CustomerContactID": "",
            "_CustomerEMailID": "",
            "_DocumentType": "0001",
            "_DraftOrderFlag": "Y",
            "_EnteredBy": "014265",
            "_EnterpriseCode": "BONTON",
            "_EntryType": "Store",
            "_OrderNo": "",
            "_PaymentRuleId": "BT_DEF_PAYMENT_RULE",
            "_SellerOrganizationCode": "101",
            "_TaxExemptFlag": "N",
            "_TaxExemptionCertificate": "",
            "Extn": {
                "_ExtnSRAuthToken": "",
                "_ExtnAssociateId": "707075"
            },
            "Notes": {
                "Note": [
                  {
                      "_NoteText": "2000 max length"
                  }
                ]
            },
            "OrderLines": {
                "OrderLine": [
                  {
                      "_CarrierServiceCode": "UPS-GRND",
                      "_DeliveryMethod": "DEL",
                      "_GiftFlag": "N",
                      "_GiftWrap": "N",
                      "_LevelOfService": "GRND",
                      "_OrderedQty": "2.0",
                      "_PrimeLineNo": 1,
                      "_SCAC": "UPS",
                      "_ScacAndService": "UPS-GRND",
                      "_SubLineNo": 1,
                      "Item": {
                          "_ItemID": "425800414756",
                          "_ProductClass": "NEW",
                          "_UnitCost": "26.98000",
                          "_UnitOfMeasure": "EACH",
                          "_UPCCode": "0888590925992"
                      },
                      "PersonInfoShipTo": {
                          "_AddressID": "",
                          "_AddressLine1": "",
                          "_AddressLine2": "",
                          "_AddressLine3": "",
                          "_City": "",
                          "_Country": "",
                          "_DayPhone": "",
                          "_EMailID": "",
                          "_EveningPhone": "",
                          "_FirstName": "",
                          "_LastName": "",
                          "_MiddleName": "",
                          "_PersonInfoKey": "",
                          "_State": "",
                          "_ZipCode": ""
                      },
                      "LinePriceInfo": {
                          "_ListPrice": "40.00000",
                          "_RetailPrice": "26.98000",
                          "_UnitPrice": "40.00"
                      },
                      "LineCharges": {
                          "LineCharge": [
                              {
                                  "_ChargeCategory": "BTN_SHIP_CHRG",
                                  "_ChargeName": "CHRG_SHIPPING",
                                  "_ChargePerUnit": "0.76",
                                  "_Reference": ""
                              },
                              {
                                  "_ChargeCategory": "BTN_SHIP_DISC",
                                  "_ChargeName": "DISC_SHIPPING",
                                  "_ChargePerUnit": "0.77",
                                  "_Reference": "FREESHIP75"
                              },
                              {
                                  "_ChargeCategory": "BTN_SALES_DISC",
                                  "_ChargeName": "DISC_PROMO",
                                  "_ChargePerUnit": "13.04",
                                  "_Reference": ""
                              }
                          ]
                      },
                      "LineTaxes": {
                          "LineTax": [
                              {
                                  "_ChargeCategory": "BTN_TAX_CHRG",
                                  "_ChargeName": "TAX_SALES",
                                  "_Tax": "3.78000",
                                  "_TaxName": "SALES",
                                  "Extn": {
                                      "_ExtnTaxPerUnit": "1.89"
                                  }
                              },
                              {
                                  "_ChargeCategory": "BTN_TAX_CHRG",
                                  "_ChargeName": "TAX_SHIPPING",
                                  "_Tax": "0.00000",
                                  "_TaxName": "SHIPPING",
                                  "Extn": {
                                      "_ExtnTaxPerUnit": "1.89"
                                  }
                              }
                          ]
                      },
                      "Notes": {
                          "Note": [
                              {
                                  "_NoteText": "Hi",
                                  "_ReasonCode": "GIFT_MESSAGE"
                              },
                              {
                                  "_NoteText": "Garrett",
                                  "_ReasonCode": "GIFT_FROM"
                              },
                              {
                                  "_NoteText": "Sister",
                                  "_ReasonCode": "GIFT_TO"
                              }
                          ]
                      },
                      "Extn": {
                          "_ExtnBadgingText": "1:Coupon Excluded;4:Incredible Value",
                          "_ExtnClass": "",
                          "_ExtnDepartment": "",
                          "_ExtnGiftItemId": "",
                          "_ExtnGiftPurchaseRecordID": "",
                          "_ExtnGiftRegistryNo": "",
                          "_ExtnGWP": "",
                          "_ExtnIsPriceLocked": "Y",
                          "_ExtnMPTID": "",
                          "_ExtnMPTOfferDetail": "",
                          "_ExtnMPTOfferMsg1": "",
                          "_ExtnMPTOfferMsg2": "",
                          "_ExtnParentLineNo": "",
                          "_ExtnPriceStatus": "",
                          "_ExtnPromoNumber": "",
                          "_ExtnSpecialHandlingCd": "1",
                          "_ExtnSPOID": "",
                          "_ExtnSPOOfferDetail": "",
                          "_ExtnSPOOfferMsg1": "",
                          "_ExtnSPOOfferMsg2": "",
                          "_ExtnSREligible": "1",
                          "_ExtnTranID": ""
                      },
                      "btDisplay": {
                          "itemDescription": "Dress Blue Medium",
                          "extendedDescription": "Here we are <ul><li>Blue</li></ul>",
                          "itemType": "REG",
                          "storageType": "R",
                          "orderableInventory": 2000,
                          "alternateUPCs": [
                              "031290088157"
                          ],
                          "imageURL": "http://broker.bonton.com/123456",
                          "brandLongDesc": "Carter's",
                          "cfgDesc": "GIRLS",
                          "classLongDesc": "CARTERS",
                          "cmgDesc": "CHILDRENS",
                          "colorAttrDesc": "Asst Lt/Pale",
                          "colorFamDesc": "Asst Family",
                          "colorLongDesc": "PRINT (969",
                          "crgDesc": "CHILDRENS",
                          "deptLongDesc": "GIRLSWEAR 2-6X",
                          "desc1": "TEE",
                          "desc2": "FALL CARTERS OP",
                          "desc3": "CARTERS OCT",
                          "desc4": "4-6X GIRL",
                          "fabDtlDesc": "Screen Print",
                          "fabLongDesc": "Cotton Blend-With Stretch",
                          "fobDesc": "GIRLS 2-6X",
                          "genClassLongDesc": "Knit Tops",
                          "gensClaLongDesc": "Tee",
                          "isn": "243306978",
                          "isnLongDesc": "GREEN FLORAL BABYDOLL TOP",
                          "itemSize": "4",
                          "labelLongDesc": "Carter's",
                          "prodDetail2": "Scoop Neck",
                          "prodDetail3": "Knit-Interlock/Jersey",
                          "prodDtlLongDesc": "Short Sleeve",
                          "vendorStyleNum": "273B053"
                      },
                      "btLogic": {
                          "isDeliveryAllowed": "N",
                          "isHazmat": "N",
                          "isParcelShippingAllowed": "Y",
                          "isPickupAllowed": "N",
                          "isReturnable": "Y",
                          "isShippingAllowed": "Y",
                          "isPriceOverridden": "Y",
                          "priceOverrideValue": 21.33,
                          "isBigTicket": "Y",
                          "btIsWebExclusive": "false",
                          "btIsWebOnlyEvent": "false",
                          "btIsIVPEvent": "false",
                          "btIsBonusEvent": "false",
                          "btIsDoorBusterEvent": "false",
                          "btIsNightOwlEvent": "false",
                          "btIsOtherSpecialEvent": "false",
                          "btIsSpecialHandling": "false",
                          "btSpecialHandlingFee": "0"
                      }
                  }
                ]
            },
            "PersonInfoBillTo": {
                "_AddressID": "",
                "_AddressLine1": "",
                "_AddressLine2": "",
                "_AddressLine3": "",
                "_City": "",
                "_Country": "",
                "_DayPhone": "",
                "_EMailID": "",
                "_EveningPhone": "",
                "_FirstName": "",
                "_LastName": "",
                "_MiddleName": "",
                "_PersonInfoKey": "",
                "_State": "",
                "_ZipCode": ""
            },
            "PersonInfoShipTo": {
                "_AddressID": "",
                "_AddressLine1": "",
                "_AddressLine2": "",
                "_AddressLine3": "",
                "_City": "",
                "_Country": "",
                "_DayPhone": "",
                "_EMailID": "",
                "_EveningPhone": "",
                "_FirstName": "",
                "_LastName": "",
                "_MiddleName": "",
                "_PersonInfoKey": "",
                "_State": "",
                "_ZipCode": ""
            },
            "PaymentMethods": {
                "PaymentMethod": [
                  {
                      "_CreditCardType": "CP",
                      "_CreditCardNo": "8231211627154941",
                      "_CreditCardExpDate": "04/2018",
                      "_DisplayCreditCardNo": "0691",
                      "_PaymentType": "CREDIT_CARD",
                      "_UnlimitedCharges": "Y",
                      "PaymentDetails": {
                          "_AuthCode": "001060",
                          "_AuthorizationID": "001060",
                          "_ChargeType": "AUTHORIZATION",
                          "_ProcessedAmount": "102.65000"
                      }
                  },
                  {
                      "_BillToKey": "",
                      "_ChargeSequence": "1",
                      "_CreditCardExpDate": "04/2018",
                      "_CreditCardName": "Garrett C Stibb",
                      "_CreditCardNo": "8231211627154941",
                      "_CreditCardType": "VISA",
                      "_DisplayCreditCardNo": "1111",
                      "_FirstName": "",
                      "_LastName": "",
                      "_MaxChargeLimit": "256.67",
                      "_MiddleName": "",
                      "_PaymentType": "CREDIT_CARD",
                      "_UnlimitedCharges": "Y",
                      "PaymentDetails": {
                          "_AuthAvs": "",
                          "_AuthCode": "001060",
                          "_AuthorizationID": "001060",
                          "_AuthorizationExpirationDate": "2018-04-09T00:00:00",
                          "_AuthReturnCode": "APPROVED",
                          "_AuthReturnFlag": "Y",
                          "_AuthReturnMessage": "202134",
                          "_AuthTime": "2015-04-06T00:00:00",
                          "_ChargeType": "AUTHORIZATION",
                          "_InternalReturnCode": "",
                          "_InternalReturnFlag": "",
                          "_InternalReturnMessage": "",
                          "_ProcessedAmount": "102.65",
                          "_Reference1": "",
                          "_Reference2": "",
                          "_RequestAmount": "256.67",
                          "_RequestId": "20150406143904330063537",
                          "_RequestProcessed": "Y",
                          "_TranReturnCode": "APPROVED",
                          "_TranReturnFlag": "SUCCESS",
                          "_TranReturnMessage": "",
                          "_TranType": "AUTHORIZATION"
                      }
                  }
                ]
            },
            "PriceInfo": {
                "_Currency": "USD"
            }
        }, refCart);
    });

    it("Control Case: Order has one OrderLine", function () {
        expect(refCart.OrderLines.OrderLine.length).toEqual(1);
    })

    //it("Set new Customer on blank orderCart", function () {
    //    orderCart.customer.setCustomer({
    //        "_CustomerType": "02",
    //        "_OrganizationCode": "BONTON",
    //        "_Status": "10",
    //        "_ExternalCustomerID": "LOGON5918",
    //        "_RegisteredDate": "2014-08-19T11:24:05-04:00",
    //        "_CustomerID": "W231905",
    //        "_CustomerKey": "20140819112410223924123",
    //        "CustomerContactList": {
    //            "_TotalNumberOfRecords": "1",
    //            "CustomerContact": {
    //                "_AggregateStatus": "10",
    //                "CustomerAdditionalAddressList": {
    //                    "_TotalNumberOfRecords": "2",
    //                    "CustomerAdditionalAddress": [
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "Y",
    //                            "_CustomerAdditionalAddressID": "100053538",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "Y",
    //                            "PersonInfo": {
    //                                "_EMailID": "garrett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329128955",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "6969696969",
    //                                "_LastName": "Stibb",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701684",
    //                                "_FirstName": "Garrett",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "53144",
    //                                "_Title": "",
    //                                "_City": "kenosha",
    //                                "_AddressID": "LOGON5918",
    //                                "_MiddleName": "C",
    //                                "_State": "WI",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "2623333333",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "2412 56th ave",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Unit 3",
    //                                "_AddressLine3": "Bld 34"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        },
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "N",
    //                            "_CustomerAdditionalAddressID": "100053539",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "N",
    //                            "PersonInfo": {
    //                                "_EMailID": "garrett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329128956",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "6969696969",
    //                                "_LastName": "Stibb",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701711",
    //                                "_FirstName": "Garrett",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "53132",
    //                                "_Title": "",
    //                                "_City": "Franklin",
    //                                "_AddressID": "ShopRunner billing",
    //                                "_MiddleName": "",
    //                                "_State": "WI",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "8878 S 84th Street",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "",
    //                                "_AddressLine3": ""
    //                            },
    //                            "_IsBillTo": "Y"
    //                        }
    //                    ]
    //                },
    //                "_UserID": "",
    //                "_DayPhone": "6969696969",
    //                "_LastName": "Stibb",
    //                "_Title": "",
    //                "_EveningFaxNo": "",
    //                "_CustomerContactID": "W231777",
    //                "_DayFaxNo": "",
    //                "_MiddleName": "",
    //                "_FirstName": "Garrett",
    //                "_EveningPhone": "",
    //                "_Company": "",
    //                "_MobilePhone": "",
    //                "_EmailID": "stibb@bonton.com"
    //            }
    //        }
    //    });

    //    //top level Order attributes set to default
    //    expect(refCart._BillToID).toEqual("W231905");
    //    expect(refCart._CustomerContactID).toEqual("W231777");
    //    expect(refCart._CustomerEMailID).toEqual("stibb@bonton.com");

    //    // all orderlines to get the default
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._FirstName).toEqual("Garrett");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._LastName).toEqual("Stibb");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._MiddleName).toEqual("C");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._ZipCode).toEqual("53144");

    //    // refCart.PersonInfoBillTo
    //    expect(refCart.PersonInfoBillTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.PersonInfoBillTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.PersonInfoBillTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.PersonInfoBillTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.PersonInfoBillTo._City).toEqual("kenosha");
    //    expect(refCart.PersonInfoBillTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoBillTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.PersonInfoBillTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.PersonInfoBillTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.PersonInfoBillTo._FirstName).toEqual("Garrett");
    //    expect(refCart.PersonInfoBillTo._LastName).toEqual("Stibb");
    //    expect(refCart.PersonInfoBillTo._MiddleName).toEqual("C");
    //    expect(refCart.PersonInfoBillTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.PersonInfoBillTo._State).toEqual("WI");
    //    expect(refCart.PersonInfoBillTo._ZipCode).toEqual("53144");

    //    // refCart.PersonInfoShipTo
    //    expect(refCart.PersonInfoShipTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.PersonInfoShipTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.PersonInfoShipTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.PersonInfoShipTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoShipTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.PersonInfoShipTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.PersonInfoShipTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.PersonInfoShipTo._FirstName).toEqual("Garrett");
    //    expect(refCart.PersonInfoShipTo._LastName).toEqual("Stibb");
    //    expect(refCart.PersonInfoShipTo._MiddleName).toEqual("C");
    //    expect(refCart.PersonInfoShipTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.PersonInfoShipTo._ZipCode).toEqual("53144");
    //})

    //it("Set new Customer with different default ship and bill", function () {
    //    orderCart.customer.setCustomer({
    //        "_CustomerType": "02",
    //        "_OrganizationCode": "BONTON",
    //        "_Status": "10",
    //        "_ExternalCustomerID": "LOGON5918",
    //        "_RegisteredDate": "2014-08-19T11:24:05-04:00",
    //        "_CustomerID": "W231905",
    //        "_CustomerKey": "20140819112410223924123",
    //        "CustomerContactList": {
    //            "_TotalNumberOfRecords": "1",
    //            "CustomerContact": {
    //                "_AggregateStatus": "10",
    //                "CustomerAdditionalAddressList": {
    //                    "_TotalNumberOfRecords": "2",
    //                    "CustomerAdditionalAddress": [
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "N",
    //                            "_CustomerAdditionalAddressID": "100053538",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "Y",
    //                            "PersonInfo": {
    //                                "_EMailID": "garrett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329128955",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "6969696969",
    //                                "_LastName": "Stibb",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701684",
    //                                "_FirstName": "Garrett",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "53144",
    //                                "_Title": "",
    //                                "_City": "kenosha",
    //                                "_AddressID": "LOGON5918",
    //                                "_MiddleName": "C",
    //                                "_State": "WI",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "2623333333",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "2412 56th ave",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Unit 3",
    //                                "_AddressLine3": "Bld 34"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        },
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "Y",
    //                            "_CustomerAdditionalAddressID": "100053539",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "N",
    //                            "PersonInfo": {
    //                                "_EMailID": "rett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329120000",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "0000000000",
    //                                "_LastName": "Stibby",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701711",
    //                                "_FirstName": "Papa",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "60605",
    //                                "_Title": "",
    //                                "_City": "Chicago",
    //                                "_AddressID": "ShopRunner billing",
    //                                "_MiddleName": "",
    //                                "_State": "IL",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "777 W East St",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Cloud 9",
    //                                "_AddressLine3": "Puff #4"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        }
    //                    ]
    //                },
    //                "_UserID": "",
    //                "_DayPhone": "6969696969",
    //                "_LastName": "Stibb",
    //                "_Title": "",
    //                "_EveningFaxNo": "",
    //                "_CustomerContactID": "W231777",
    //                "_DayFaxNo": "",
    //                "_MiddleName": "",
    //                "_FirstName": "Garrett",
    //                "_EveningPhone": "",
    //                "_Company": "",
    //                "_MobilePhone": "",
    //                "_EmailID": "stibb@bonton.com"
    //            }
    //        }
    //    });

    //    //top level Order attributes set to default
    //    expect(refCart._BillToID).toEqual("W231905");
    //    expect(refCart._CustomerContactID).toEqual("W231777");
    //    expect(refCart._CustomerEMailID).toEqual("stibb@bonton.com");

    //    // all orderlines to get the default
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._FirstName).toEqual("Garrett");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._LastName).toEqual("Stibb");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._MiddleName).toEqual("C");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._ZipCode).toEqual("53144");

    //    // refCart.PersonInfoBillTo
    //    expect(refCart.PersonInfoBillTo._AddressID).toEqual("ShopRunner billing");
    //    expect(refCart.PersonInfoBillTo._AddressLine1).toEqual("777 W East St");
    //    expect(refCart.PersonInfoBillTo._AddressLine2).toEqual("Cloud 9");
    //    expect(refCart.PersonInfoBillTo._AddressLine3).toEqual("Puff #4");
    //    expect(refCart.PersonInfoBillTo._City).toEqual("Chicago");
    //    expect(refCart.PersonInfoBillTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoBillTo._DayPhone).toEqual("0000000000");
    //    expect(refCart.PersonInfoBillTo._EMailID).toEqual("rett.stibb@bonton.com");
    //    expect(refCart.PersonInfoBillTo._EveningPhone).toEqual("");
    //    expect(refCart.PersonInfoBillTo._FirstName).toEqual("Papa");
    //    expect(refCart.PersonInfoBillTo._LastName).toEqual("Stibby");
    //    expect(refCart.PersonInfoBillTo._MiddleName).toEqual("");
    //    expect(refCart.PersonInfoBillTo._PersonInfoKey).toEqual("20150402190721329120000");
    //    expect(refCart.PersonInfoBillTo._State).toEqual("IL");
    //    expect(refCart.PersonInfoBillTo._ZipCode).toEqual("60605");

    //    // refCart.PersonInfoShipTo
    //    expect(refCart.PersonInfoShipTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.PersonInfoShipTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.PersonInfoShipTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.PersonInfoShipTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoShipTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.PersonInfoShipTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.PersonInfoShipTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.PersonInfoShipTo._FirstName).toEqual("Garrett");
    //    expect(refCart.PersonInfoShipTo._LastName).toEqual("Stibb");
    //    expect(refCart.PersonInfoShipTo._MiddleName).toEqual("C");
    //    expect(refCart.PersonInfoShipTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.PersonInfoShipTo._ZipCode).toEqual("53144");
    //})

    //it("Set same customer twice so do not overwrite.", function () {
    //    orderCart.customer.setCustomer({
    //        "_CustomerType": "02",
    //        "_OrganizationCode": "BONTON",
    //        "_Status": "10",
    //        "_ExternalCustomerID": "LOGON5918",
    //        "_RegisteredDate": "2014-08-19T11:24:05-04:00",
    //        "_CustomerID": "W231905",
    //        "_CustomerKey": "20140819112410223924123",
    //        "CustomerContactList": {
    //            "_TotalNumberOfRecords": "1",
    //            "CustomerContact": {
    //                "_AggregateStatus": "10",
    //                "CustomerAdditionalAddressList": {
    //                    "_TotalNumberOfRecords": "2",
    //                    "CustomerAdditionalAddress": [
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "N",
    //                            "_CustomerAdditionalAddressID": "100053538",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "Y",
    //                            "PersonInfo": {
    //                                "_EMailID": "garrett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329128955",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "6969696969",
    //                                "_LastName": "Stibb",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701684",
    //                                "_FirstName": "Garrett",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "53144",
    //                                "_Title": "",
    //                                "_City": "kenosha",
    //                                "_AddressID": "LOGON5918",
    //                                "_MiddleName": "C",
    //                                "_State": "WI",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "2623333333",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "2412 56th ave",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Unit 3",
    //                                "_AddressLine3": "Bld 34"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        },
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "Y",
    //                            "_CustomerAdditionalAddressID": "100053539",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "N",
    //                            "PersonInfo": {
    //                                "_EMailID": "rett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329120000",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "0000000000",
    //                                "_LastName": "Stibby",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701711",
    //                                "_FirstName": "Papa",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "60605",
    //                                "_Title": "",
    //                                "_City": "Chicago",
    //                                "_AddressID": "ShopRunner billing",
    //                                "_MiddleName": "",
    //                                "_State": "IL",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "777 W East St",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Cloud 9",
    //                                "_AddressLine3": "Puff #4"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        }
    //                    ]
    //                },
    //                "_UserID": "",
    //                "_DayPhone": "6969696969",
    //                "_LastName": "Stibb",
    //                "_Title": "",
    //                "_EveningFaxNo": "",
    //                "_CustomerContactID": "W231777",
    //                "_DayFaxNo": "",
    //                "_MiddleName": "",
    //                "_FirstName": "Garrett",
    //                "_EveningPhone": "",
    //                "_Company": "",
    //                "_MobilePhone": "",
    //                "_EmailID": "stibb@bonton.com"
    //            }
    //        }
    //    });

    //    orderCart.customer.setCustomer({
    //        "_CustomerType": "02",
    //        "_OrganizationCode": "BONTON",
    //        "_Status": "10",
    //        "_ExternalCustomerID": "LOGON5918",
    //        "_RegisteredDate": "2014-08-19T11:24:05-04:00",
    //        "_CustomerID": "W231905",
    //        "_CustomerKey": "20140819112410223924123",
    //        "CustomerContactList": {
    //            "_TotalNumberOfRecords": "1",
    //            "CustomerContact": {
    //                "_AggregateStatus": "10",
    //                "CustomerAdditionalAddressList": {
    //                    "_TotalNumberOfRecords": "2",
    //                    "CustomerAdditionalAddress": [
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "Y",
    //                            "_CustomerAdditionalAddressID": "100053538",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "Y",
    //                            "PersonInfo": {
    //                                "_EMailID": "garrett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329128955",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "6969696969",
    //                                "_LastName": "Stibb",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701684",
    //                                "_FirstName": "Garrett",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "53144",
    //                                "_Title": "",
    //                                "_City": "kenosha",
    //                                "_AddressID": "LOGON5918",
    //                                "_MiddleName": "C",
    //                                "_State": "WI",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "2623333333",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "2412 56th ave",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Unit 3",
    //                                "_AddressLine3": "Bld 34"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        },
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "N",
    //                            "_CustomerAdditionalAddressID": "100053539",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "N",
    //                            "PersonInfo": {
    //                                "_EMailID": "garrett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329128956",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "6969696969",
    //                                "_LastName": "Stibb",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701711",
    //                                "_FirstName": "Garrett",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "53132",
    //                                "_Title": "",
    //                                "_City": "Franklin",
    //                                "_AddressID": "ShopRunner billing",
    //                                "_MiddleName": "",
    //                                "_State": "WI",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "8878 S 84th Street",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "",
    //                                "_AddressLine3": ""
    //                            },
    //                            "_IsBillTo": "Y"
    //                        }
    //                    ]
    //                },
    //                "_UserID": "",
    //                "_DayPhone": "6969696969",
    //                "_LastName": "Stibb",
    //                "_Title": "",
    //                "_EveningFaxNo": "",
    //                "_CustomerContactID": "W231777",
    //                "_DayFaxNo": "",
    //                "_MiddleName": "",
    //                "_FirstName": "Garrett",
    //                "_EveningPhone": "",
    //                "_Company": "",
    //                "_MobilePhone": "",
    //                "_EmailID": "stibb@bonton.com"
    //            }
    //        }
    //    });
    //    //top level Order attributes set to default
    //    expect(refCart._BillToID).toEqual("W231905");
    //    expect(refCart._CustomerContactID).toEqual("W231777");
    //    expect(refCart._CustomerEMailID).toEqual("stibb@bonton.com");

    //    // all orderlines to get the default
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._FirstName).toEqual("Garrett");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._LastName).toEqual("Stibb");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._MiddleName).toEqual("C");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._ZipCode).toEqual("53144");

    //    // refCart.PersonInfoBillTo
    //    expect(refCart.PersonInfoBillTo._AddressID).toEqual("ShopRunner billing");
    //    expect(refCart.PersonInfoBillTo._AddressLine1).toEqual("777 W East St");
    //    expect(refCart.PersonInfoBillTo._AddressLine2).toEqual("Cloud 9");
    //    expect(refCart.PersonInfoBillTo._AddressLine3).toEqual("Puff #4");
    //    expect(refCart.PersonInfoBillTo._City).toEqual("Chicago");
    //    expect(refCart.PersonInfoBillTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoBillTo._DayPhone).toEqual("0000000000");
    //    expect(refCart.PersonInfoBillTo._EMailID).toEqual("rett.stibb@bonton.com");
    //    expect(refCart.PersonInfoBillTo._EveningPhone).toEqual("");
    //    expect(refCart.PersonInfoBillTo._FirstName).toEqual("Papa");
    //    expect(refCart.PersonInfoBillTo._LastName).toEqual("Stibby");
    //    expect(refCart.PersonInfoBillTo._MiddleName).toEqual("");
    //    expect(refCart.PersonInfoBillTo._PersonInfoKey).toEqual("20150402190721329120000");
    //    expect(refCart.PersonInfoBillTo._State).toEqual("IL");
    //    expect(refCart.PersonInfoBillTo._ZipCode).toEqual("60605");

    //    // refCart.PersonInfoShipTo
    //    expect(refCart.PersonInfoShipTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.PersonInfoShipTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.PersonInfoShipTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.PersonInfoShipTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoShipTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.PersonInfoShipTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.PersonInfoShipTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.PersonInfoShipTo._FirstName).toEqual("Garrett");
    //    expect(refCart.PersonInfoShipTo._LastName).toEqual("Stibb");
    //    expect(refCart.PersonInfoShipTo._MiddleName).toEqual("C");
    //    expect(refCart.PersonInfoShipTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.PersonInfoShipTo._ZipCode).toEqual("53144");
    //})


    //it("Set new Customer, but keep registry address", function () {

    //    refCart.OrderLines.OrderLine[0]._GiftFlag = "Y";
    //    refCart.OrderLines.OrderLine[0].Extn._ExtnGiftRegistryNo = "GT8000";
    //    orderCart.giftOptions.setGiftRegistry([{ PrimeLine: 1, SubLine: 1 }], "GT8000", {
    //        "_IsShipTo": "Y",
    //        "_IsDefaultSoldTo": "N",
    //        "_IsDefaultBillTo": "Y",
    //        "_CustomerAdditionalAddressID": "100053538",
    //        "_AddressType": "SB",
    //        "_IsSoldTo": "Y",
    //        "_IsDefaultShipTo": "Y",
    //        "PersonInfo": {
    //            "_EMailID": "gift@bonton.com",
    //            "_PersonInfoKey": "20150402190000000000000",
    //            "_VerificationStatus": "",
    //            "_PreferredShipAddress": "",
    //            "_DayPhone": "7773331111",
    //            "_LastName": "Dang",
    //            "_EveningFaxNo": "",
    //            "_OtherPhone": "",
    //            "_DayFaxNo": "",
    //            "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //            "_PersonID": "8701600",
    //            "_FirstName": "Mylinh",
    //            "_MobilePhone": "",
    //            "_Company": "",
    //            "_Department": "",
    //            "_AlternateEmailID": "",
    //            "_Suffix": "",
    //            "_Country": "US",
    //            "_ErrorTxt": "",
    //            "_ZipCode": "53144",
    //            "_Title": "",
    //            "_City": "kenosha",
    //            "_AddressID": "Home",
    //            "_MiddleName": "",
    //            "_State": "WI",
    //            "_AddressLine4": "",
    //            "_AddressLine5": "",
    //            "_AddressLine6": "",
    //            "_EveningPhone": "",
    //            "_JobTitle": "",
    //            "_Beeper": "",
    //            "_AddressLine1": "2 11 St",
    //            "_UseCount": "0",
    //            "_AddressLine2": "",
    //            "_AddressLine3": ""
    //        },
    //        "_IsBillTo": "Y"
    //    });

    //    orderCart.customer.setCustomer({
    //        "_CustomerType": "02",
    //        "_OrganizationCode": "BONTON",
    //        "_Status": "10",
    //        "_ExternalCustomerID": "LOGON5918",
    //        "_RegisteredDate": "2014-08-19T11:24:05-04:00",
    //        "_CustomerID": "W231905",
    //        "_CustomerKey": "20140819112410223924123",
    //        "CustomerContactList": {
    //            "_TotalNumberOfRecords": "1",
    //            "CustomerContact": {
    //                "_AggregateStatus": "10",
    //                "CustomerAdditionalAddressList": {
    //                    "_TotalNumberOfRecords": "2",
    //                    "CustomerAdditionalAddress": [
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "N",
    //                            "_CustomerAdditionalAddressID": "100053538",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "Y",
    //                            "PersonInfo": {
    //                                "_EMailID": "garrett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329128955",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "6969696969",
    //                                "_LastName": "Stibb",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701684",
    //                                "_FirstName": "Garrett",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "53144",
    //                                "_Title": "",
    //                                "_City": "kenosha",
    //                                "_AddressID": "LOGON5918",
    //                                "_MiddleName": "C",
    //                                "_State": "WI",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "2623333333",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "2412 56th ave",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Unit 3",
    //                                "_AddressLine3": "Bld 34"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        },
    //                        {
    //                            "_IsShipTo": "Y",
    //                            "_IsDefaultSoldTo": "N",
    //                            "_IsDefaultBillTo": "Y",
    //                            "_CustomerAdditionalAddressID": "100053539",
    //                            "_AddressType": "SB",
    //                            "_IsSoldTo": "Y",
    //                            "_IsDefaultShipTo": "N",
    //                            "PersonInfo": {
    //                                "_EMailID": "rett.stibb@bonton.com",
    //                                "_PersonInfoKey": "20150402190721329120000",
    //                                "_VerificationStatus": "",
    //                                "_PreferredShipAddress": "",
    //                                "_DayPhone": "0000000000",
    //                                "_LastName": "Stibby",
    //                                "_EveningFaxNo": "",
    //                                "_OtherPhone": "",
    //                                "_DayFaxNo": "",
    //                                "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
    //                                "_PersonID": "8701711",
    //                                "_FirstName": "Papa",
    //                                "_MobilePhone": "",
    //                                "_Company": "",
    //                                "_Department": "",
    //                                "_AlternateEmailID": "",
    //                                "_Suffix": "",
    //                                "_Country": "US",
    //                                "_ErrorTxt": "",
    //                                "_ZipCode": "60605",
    //                                "_Title": "",
    //                                "_City": "Chicago",
    //                                "_AddressID": "ShopRunner billing",
    //                                "_MiddleName": "",
    //                                "_State": "IL",
    //                                "_AddressLine4": "",
    //                                "_AddressLine5": "",
    //                                "_AddressLine6": "",
    //                                "_EveningPhone": "",
    //                                "_JobTitle": "",
    //                                "_Beeper": "",
    //                                "_AddressLine1": "777 W East St",
    //                                "_UseCount": "0",
    //                                "_AddressLine2": "Cloud 9",
    //                                "_AddressLine3": "Puff #4"
    //                            },
    //                            "_IsBillTo": "Y"
    //                        }
    //                    ]
    //                },
    //                "_UserID": "",
    //                "_DayPhone": "6969696969",
    //                "_LastName": "Stibb",
    //                "_Title": "",
    //                "_EveningFaxNo": "",
    //                "_CustomerContactID": "W231777",
    //                "_DayFaxNo": "",
    //                "_MiddleName": "",
    //                "_FirstName": "Garrett",
    //                "_EveningPhone": "",
    //                "_Company": "",
    //                "_MobilePhone": "",
    //                "_EmailID": "stibb@bonton.com"
    //            }
    //        }
    //    });

    //    //top level Order attributes set to default
    //    expect(refCart._BillToID).toEqual("W231905");
    //    expect(refCart._CustomerContactID).toEqual("W231777");
    //    expect(refCart._CustomerEMailID).toEqual("stibb@bonton.com");

    //    // all orderlines to get gift registry address
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressID).toEqual("Home");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine1).toEqual("2 11 St");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine2).toEqual("");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._AddressLine3).toEqual("");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._DayPhone).toEqual("7773331111");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EMailID).toEqual("gift@bonton.com");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._EveningPhone).toEqual("");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._FirstName).toEqual("Mylinh");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._LastName).toEqual("Dang");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._MiddleName).toEqual("");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._PersonInfoKey).toEqual("20150402190000000000000");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.OrderLines.OrderLine[0].PersonInfoShipTo._ZipCode).toEqual("53144");

    //    // refCart.PersonInfoBillTo
    //    expect(refCart.PersonInfoBillTo._AddressID).toEqual("ShopRunner billing");
    //    expect(refCart.PersonInfoBillTo._AddressLine1).toEqual("777 W East St");
    //    expect(refCart.PersonInfoBillTo._AddressLine2).toEqual("Cloud 9");
    //    expect(refCart.PersonInfoBillTo._AddressLine3).toEqual("Puff #4");
    //    expect(refCart.PersonInfoBillTo._City).toEqual("Chicago");
    //    expect(refCart.PersonInfoBillTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoBillTo._DayPhone).toEqual("0000000000");
    //    expect(refCart.PersonInfoBillTo._EMailID).toEqual("rett.stibb@bonton.com");
    //    expect(refCart.PersonInfoBillTo._EveningPhone).toEqual("");
    //    expect(refCart.PersonInfoBillTo._FirstName).toEqual("Papa");
    //    expect(refCart.PersonInfoBillTo._LastName).toEqual("Stibby");
    //    expect(refCart.PersonInfoBillTo._MiddleName).toEqual("");
    //    expect(refCart.PersonInfoBillTo._PersonInfoKey).toEqual("20150402190721329120000");
    //    expect(refCart.PersonInfoBillTo._State).toEqual("IL");
    //    expect(refCart.PersonInfoBillTo._ZipCode).toEqual("60605");

    //    // refCart.PersonInfoShipTo
    //    expect(refCart.PersonInfoShipTo._AddressID).toEqual("LOGON5918");
    //    expect(refCart.PersonInfoShipTo._AddressLine1).toEqual("2412 56th ave");
    //    expect(refCart.PersonInfoShipTo._AddressLine2).toEqual("Unit 3");
    //    expect(refCart.PersonInfoShipTo._AddressLine3).toEqual("Bld 34");
    //    expect(refCart.PersonInfoShipTo._City).toEqual("kenosha");
    //    expect(refCart.PersonInfoShipTo._Country).toEqual("US");
    //    expect(refCart.PersonInfoShipTo._DayPhone).toEqual("6969696969");
    //    expect(refCart.PersonInfoShipTo._EMailID).toEqual("garrett.stibb@bonton.com");
    //    expect(refCart.PersonInfoShipTo._EveningPhone).toEqual("2623333333");
    //    expect(refCart.PersonInfoShipTo._FirstName).toEqual("Garrett");
    //    expect(refCart.PersonInfoShipTo._LastName).toEqual("Stibb");
    //    expect(refCart.PersonInfoShipTo._MiddleName).toEqual("C");
    //    expect(refCart.PersonInfoShipTo._PersonInfoKey).toEqual("20150402190721329128955");
    //    expect(refCart.PersonInfoShipTo._State).toEqual("WI");
    //    expect(refCart.PersonInfoShipTo._ZipCode).toEqual("53144");
    //})
});