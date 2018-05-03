var godmode = { print: null, reprice: null, setExample: null, goState: null, addLine: null };
angular.module('appServiceOrderCart', ['propertiesService', 'appUtilities', 'appServicesGiftRegistry'])
.factory('orderCart', ["$log", "btProp", 'POSService', '$http', '$rootScope', 'errorObj', 'giftRegistryService', 'serviceArrayFix', 'repriceService', '$q', '$state', 'customer', 'isPoBoxAddress', 'bigTicketValidate', 'itemProperty', 'loggerService', 'sendSMTPErrorEmail', 'securityService', '$filter',
     function ($log, btProp, POSService, $http, $rootScope, errorObj, giftRegistryService, serviceArrayFix, repriceService, $q, $state, $customer, $isPoBoxAddress, $bigTicketValidate, $itemProperty, $loggerService, $sendSMTPErrorEmail, $securityService, $filter) {
         godmode.print = function () { $loggerService.log(_getLiveOrderCart()); };
         godmode.reprice = function (isGetCarrierServices) {
             _repriceOrder(isGetCarrierServices).then(function (response) { $loggerService.log(response.data); },
                 function (response) { $loggerService.log(response.data); }
                 );

         };
         godmode.setExample = function (inputCart) {
             if (inputCart) {
                 orderCart = inputCart;
             } else {
                 _setExampleOrder();
             }
         };
         godmode.goState = function (stateName) {
             $state.go(stateName);
         };
         godmode.addLine = function (itemType) {
             var bigTicketItem = {
                 "id": 450300070386,
                 "isn": 503103422,
                 "sku": 450300070386,
                 "isnlongdesc": "iComf Prodigy Everfeel Plsh-Qn",
                 "vendorstyle": "PRODIGYPLUSH-QN",
                 "genclasslongdesc": "Mattresses",
                 "colorfamdesc": "No Color",
                 "colorattrdesc": "No Color",
                 "itemsize": "",
                 "desc1": "PLUSH",
                 "brandlongdesc": "Serta",
                 "proddtllongdesc": "Plush Top",
                 "deptlongdesc": "MATTRESSES",
                 "cmg": "220 - FURNITURE",
                 "cfg": "460 - FURNITURE",
                 "fob": "84 - MATTRESSES",
                 "crg": "600 - HOME",
                 "classlongdesc": "SERTA ICOMFORT",
                 "gensclalongdesc": "Mattress-Specialty",
                 "fablongdesc": "None",
                 "fabdtldesc": "None",
                 "proddetail2": "Queen",
                 "proddetail3": "None",
                 "labellongdesc": "Serta",
                 "productcode": 340867,
                 "productname": "iComfort&reg; by Serta&reg; Prodigy Everfeel Plush Mattress & Box Spring Set",
                 "imageid": 742183,
                 "keywords": ",,iComfort by Serta Prodigy Everfeel Plush Mattress & Box Spring Set,BTI,iComfort by Serta,iComfort by Serta mattress and box spring set,iComfort by Serta sleep set,gel memory foam,12\" mattress height",
                 "webcatprodshortdesc": "Sleep soundly on this luxurious mattress set.",
                 "webcatprodlongdesc": "<p><strong>Enter ZIP code to confirm your address is located within our delivery area. If so, you may call with questions or to place an order. If your furniture order is placed online, we'll contact you to set up delivery.</strong></p>\n<p>The iComfort by Serta Prodigy Everfeel Plush Mattress & Box Spring Set gives the targeted support your body needs while cradling you in ultra-plush comfort. With three layers advanced memory foam to relieve pressure and provide even cooling throughout the night, this mattress and box spring set is sure to provide years of superior rest.</p>\n<ul>\n    <li>White/Black </li>\n    <li>Sold as a set with box spring </li>\n    <li>Choose between a 9\"H standard or 5.5\" low profile box spring </li>\n    <li>12\" mattress height </li>\n</ul>\n<p><strong>Construction:<br />\n</strong></p>\n<ul>\n    <li>The only iComfort&reg; model to feature 3 layers of advanced memory foams </li>\n    <li>Support layer features ComfortLast&reg; Foam Core with Ultimate Edge&reg; Mattress Edge Support </li>\n    <li>1.5\" Cool Action&trade; Dual Effects&reg; Gel Memory Foam helps relieve pressure while providing targeted support and enhanced cooling comfort </li>\n    <li>0.75\" EverFeel&trade; Technology for a difference you&rsquo;ll feel the moment you lie down </li>\n    <li>2\" EverCool&reg; Memory Foam and 1&rdquo; PillowSoft&trade; Foam </li>\n    <li>Cool Action&trade; Dual Effects&reg; Gel Memory Foam </li>\n    <li>1\" Cool Action&trade; Gel Memory Foam </li>\n    <li>Made in the USA </li>\n</ul>",
                 "itemtype": "BGT",
                 "pricestatus": "R",
                 "hazardCode": "1",
                 "giftwrapCode": "1",
                 "specialhandlingcode": "1",
                 "isactive": "Y",
                 "isgwp": "N",
                 "iswebexclusive": "N",
                 "isspecialhandling": "Y",
                 "isairshipallowed": "N",
                 "isgroundshipallowed": "Y",
                 "specialhandlingfee": 0,
                 "groupid": "503103422340867",
                 "_version_": 1503035902681677800,
                 "itemDetail": {
                     "_ItemID": "450300070386",
                     "_ItemKey": "450300070386",
                     "_OrganizationCode": "BONTON",
                     "_UPCCode": "450300070386",
                     "ComputedPrice": {
                         "_ListPrice": "4710",
                         "_RetailPrice": "4710",
                         "_UnitPrice": "4710",
                         "Extn": {
                             "_btIsBonusEvent": "false",
                             "_btIsDoorBusterEvent": "false",
                             "_btIsHazMat": "false",
                             "_btIsIVPEvent": "false",
                             "_btIsNightOwlEvent": "false",
                             "_btIsOtherSpecialEvent": "false",
                             "_btIsSpecialHandling": "false",
                             "_btIsWebExclusive": "false",
                             "_btIsWebOnlyEvent": "false",
                             "_btSpecialHandlingFee": "0",
                             "_ExtnBadgingText": "Regular",
                             "_ExtnClass": "806",
                             "_ExtnDepartment": "503",
                             "_ExtnMPTID": "",
                             "_ExtnMPTOfferDetail": "",
                             "_ExtnMPTOfferMsg1": "",
                             "_ExtnMPTOfferMsg2": "",
                             "_ExtnPriceStatus": "R",
                             "_ExtnPromoNumber": "0",
                             "_ExtnSpecialHandlingCode": "0",
                             "_ExtnSPOID": "0",
                             "_ExtnSPOOfferDetail": "",
                             "_ExtnSPOOfferMsg1": "",
                             "_ExtnSPOOfferMsg2": ""
                         }
                     }
                 },
                 "_AvailableQty": "50",
                 "defaultItemDescription": "iComfort&reg; by Serta&reg; Prodigy Everfeel Plush Mattress & Box Spring Set",
                 "defaultImageUrl": "http://broker.qa.bonton.com:7080/image/BonTon/742183"
             };

             var itemObject = {
                 "id": 755179389899,
                 "isn": 400101743,
                 "sku": 440000688653,
                 "isnlongdesc": "tay charc deq c/s ruched sq nk",
                 "vendorstyle": "3150M",
                 "genclasslongdesc": "Dresses",
                 "colorlongdesc": "CHARCOAL",
                 "colorfamdesc": "Black/Gray Fam",
                 "colorattrdesc": "Charcoal Gray",
                 "itemsize": "10       R",
                 "sizesequence": 11020,
                 "desc1": "OCTOBER 2011",
                 "desc2": "LBD",
                 "desc3": "SHTH (SHEATH)",
                 "desc4": "MATTE JERSEY",
                 "brandlongdesc": "Taylor Dresses",
                 "proddtllongdesc": "Cap Sleeve",
                 "deptlongdesc": "BETTER DRESSES",
                 "cmg": "110 - DRESSES",
                 "cfg": "10 - MISSES DRESSES",
                 "fob": "101 - BETTER DRESSES",
                 "crg": "100 - READY TO WEAR",
                 "classlongdesc": "TAYLOR",
                 "gensclalongdesc": "1pc Dress",
                 "fablongdesc": "Poly Blend-With Stretch",
                 "fabdtldesc": "Embellished",
                 "proddetail2": "Above the Knee Length",
                 "proddetail3": "Knit",
                 "labellongdesc": "Taylor Dresses",
                 "productcode": 157134,
                 "productname": "Taylor Charcoal Sequin Knit Cap-Sleeve Squareneck Party Dress",
                 "imageid": 431692,
                 "keywords": ",,1,10,8,6,4,2,Taylor,Charcoal,Sequin Knit,Cap Sleeve,Squareneck Party Dress,party dress,cap sleeve dress,squareneck dress,knit dresses,cocktail dresses,women,apparel,",
                 "webcatprodshortdesc": "Mingle in high style in this gorgeous, sequin-knit cocktail dress, characterized by its ruched, pleated texture and revealing cap sleeves.",
                 "webcatprodlongdesc": "<ul>\n    <li>Squareneck party dress with cap sleeves</li>\n    <li>Ruched pleats over sequin knit fabric</li>\n    <li>Back-zip closure</li>\n    <li>Acrylic/nylon</li>\n    <li>Imported</li>\n</ul>",
                 "colorcode": 10,
                 "colorDc": "00010 - CHARCOAL",
                 "size1code": "10  ",
                 "size2code": "R   ",
                 "sizedc": "10    R   ",
                 "itemtype": "REG",
                 "pricestatus": "P",
                 "hazardCode": "1",
                 "giftwrapCode": "2",
                 "specialhandlingcode": "1",
                 "isactive": "Y",
                 "isgwp": "N",
                 "iswebexclusive": "Y",
                 "isspecialhandling": "N",
                 "isairshipallowed": "Y",
                 "isgroundshipallowed": "Y",
                 "specialhandlingfee": 0,
                 "groupid": "400101743157134",
                 "buyable": true,
                 "_version_": 1503052571298758700,
                 "itemDetail": {
                     "_ItemID": "440000688653",
                     "_ItemKey": "440000688653",
                     "_OrganizationCode": "BONTON",
                     "_UPCCode": "755179389899",
                     "ComputedPrice": {
                         "_ListPrice": "158",
                         "_RetailPrice": "77.99",
                         "_UnitPrice": "77.99",
                         "Extn": {
                             "_btIsBonusEvent": "false",
                             "_btIsDoorBusterEvent": "false",
                             "_btIsHazMat": "false",
                             "_btIsIVPEvent": "false",
                             "_btIsNightOwlEvent": "false",
                             "_btIsOtherSpecialEvent": "false",
                             "_btIsSpecialHandling": "false",
                             "_btIsWebExclusive": "false",
                             "_btIsWebOnlyEvent": "false",
                             "_btSpecialHandlingFee": "0",
                             "_ExtnBadgingText": "Black Dot",
                             "_ExtnClass": "600",
                             "_ExtnDepartment": "400",
                             "_ExtnMPTID": "",
                             "_ExtnMPTOfferDetail": "",
                             "_ExtnMPTOfferMsg1": "",
                             "_ExtnMPTOfferMsg2": "",
                             "_ExtnPriceStatus": "P",
                             "_ExtnPromoNumber": "0",
                             "_ExtnSpecialHandlingCode": "0",
                             "_ExtnSPOID": "0",
                             "_ExtnSPOOfferDetail": "",
                             "_ExtnSPOOfferMsg1": "",
                             "_ExtnSPOOfferMsg2": ""
                         }
                     }
                 },
                 "_AvailableQty": "10",
                 "defaultItemDescription": "Taylor Charcoal Sequin Knit Cap-Sleeve Squareneck Party Dress",
                 "defaultImageUrl": "http://broker.qa.bonton.com:7080/image/BonTon/431692",
                 "shippingMethodPrice": "0.00",
                 "shippingMethodDescription": "UPS Ground/Standard"
             };

             if (itemType === 'BGT') {
                 _addItem(bigTicketItem, 1, true);
             } else {
                 _addItem(itemObject, 1, false);
             }

         };

         var _isLogCart = false;
         var _currentCustomerKey = '';
         var _giftRegistryAddressSet = {};
         var orderLineModel = {
             "_CarrierServiceCode": "UPS-GRND",
             "_DeliveryMethod": "SHP",
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
                 "_UnitPrice": "40.00",
                 "_TaxableFlag": "Y"
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
                 "_AvailableQty": 2000,
                 "alternateUPCs": [
                   "031290088157"
                 ],
                 "imageURL": "/assets/images/865391.jpg",
                 "brandlongdesc": "Carter's",
                 "cfgdesc": "GIRLS",
                 "cfgid": 101,
                 "classlongdesc": "CARTERS",
                 "cmgdesc": "CHILDRENS",
                 "cmgid": 10,
                 "colorattrdesc": "Asst Lt/Pale",
                 "colorcode": "",
                 "colorDc": "",
                 "colorfamdesc": "Asst Family",
                 "colorlongdesc": "Navy",
                 "corpdesc": "",
                 "crgdesc": "CHILDRENS",
                 "crgid": 0,
                 "deptlongdesc": "GIRLSWEAR 2-6X",
                 "desc1": "TEE",
                 "desc2": "FALL CARTERS OP",
                 "desc3": "CARTERS OCT",
                 "desc4": "4-6X GIRL",
                 "fabdtldesc": "Screen Print",
                 "fablongdesc": "Cotton Blend-With Stretch",
                 "fobdesc": "GIRLS 2-6X",
                 "fobid": 28,
                 "genclasslongdesc": "Knit Tops",
                 "gensclalongdesc": "Tee",
                 "imageid": "",
                 "isn": "243306978",
                 "isnlongdesc": "GREEN FLORAL BABYDOLL TOP",
                 "itemsize": "4",
                 "keywords": "",
                 "labellongdesc": "Carter's",
                 "longpatterndesc": "",
                 "proddetail2": "Scoop Neck",
                 "proddetail3": "Knit-Interlock/Jersey",
                 "proddtllongdesc": "Short Sleeve",
                 "productcode": "",
                 "productname": "Green Floral Babydoll Top",
                 "size1code": "",
                 "size2code": "",
                 "sizedc": "",
                 "sizesequence": "1",
                 "vendorstyle": "273B053",
                 "webcatprodlongdesc": "",
                 "webid": ""
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
                 "btSpecialHandlingFee": "0",
                 giftImgUrl: "assets/images/GiftMessage.gif",
                 isGiftRegistryAddress: false,
                 giftBoxImgUrl: "assets/images/giftBox.gif"
             }
         };

         var _INIT_CART = {

             //Order attributes
             _AuthorizedClient: "Store", //"Store" for LUFI
             _BillToID: "",
             _CustomerContactID: "",
             _CustomerEMailID: "",
             _DocumentType: "0001", //Alway "0001" for LUFI Sales Order
             _DraftOrderFlag: "N", // "Y" for Draft Order
             _EnteredBy: "", // should be "Store" if any store node is passed to chromium
             _EnterpriseCode: "BONTON", //always "BONTON"
             _EntryType: "BTN", //should be like "ELD" aka elderman OR else GetOrderCartPrice Will fail DO NOT USE 'Store'
             _OrderNo: "", //if blank Sterling will autogenerate new one
             _PaymentRuleId: "BT_DEF_PAYMENT_RULE", //what payment rules are used
             _SellerOrganizationCode: "", //Store Node /number ex: "101"
             _TaxExemptFlag: "N",
             _TaxExemptionCertificate: "",

             //OrderLines
             OrderLines: {
                 OrderLine: []
             },

             //PersonInfoBillTo Header Level
             PersonInfoBillTo: {},

             //PersonInfoShipTo Header Level
             PersonInfoShipTo: {},
             PaymentMethods: {                       //xml-json issue xml goes <PaymentMethods><PaymentMethod /><PaymentMethod /> ...
                 //so do PaymentMethods:[]  and have broker know the tag needs to be PaymentMethod OR make another object {PaymentMethod:[]}
                 PaymentMethod: []
             },
             Extn: {},
             PriceInfo: { _Currency: "USD" }, //price info child node
             btLogic: {}
         };

         var _INIT_ORDER_LINE = {
             _CarrierServiceCode: "",
             _DeliveryMethod: "SHP", //TODO: change when pick up in store starts. Options: PICK, SHP, or DEL (DEL is not currently used by us ever).
             _GiftFlag: "N",
             _GiftWrap: "N",
             _LevelOfService: "",
             _OrderedQty: "1",
             _PrimeLineNo: 1,
             _SCAC: "",
             _ScacAndService: "",
             _SubLineNo: 1,
             Item: {
                 _ItemID: "",
                 _ProductClass: "NEW",
                 _UnitCost: "",
                 _UnitOfMeasure: "EACH",
                 _UPCCode: ""
             },
             ItemDetails: {
                 _ItemID: "",
                 PrimaryInformation: {
                     _IsAirShippingAllowed: "",
                     _ItemType: "REG",
                     _IsParcelShippingAllowed: ""
                 },
                 "ItemAliasList": {
                     "ItemAlias": [
                       {
                           "_AliasName": "ACTIVE_UPC",
                           "_AliasValue": ""
                       }
                     ]
                 }
             },
             PersonInfoShipTo: {
                 _AddressID: "",
                 _AddressLine1: "",
                 _AddressLine2: "",
                 _AddressLine3: "",
                 _City: "",
                 _Country: "US", //TODO: must be US for ship to address
                 _DayPhone: "",
                 _EMailID: "",
                 _EveningPhone: "",
                 _FirstName: "",
                 _LastName: "",
                 _MiddleName: "",
                 _PersonInfoKey: "",
                 _State: "",
                 _ZipCode: ""
             },
             LinePriceInfo: {
                 _ListPrice: "",
                 _RetailPrice: "",
                 _UnitPrice: "",
                 _TaxableFlag: "Y"
             },
             LineCharges: {
                 LineCharge: [//TODO: is this needed for CarrierServiceCodes????
                     {
                         _ChargeAmount: "0.00",
                         _ChargeCategory: "BTN_SHIP_CHRG",
                         _ChargeName: "CHRG_SHIPPING",
                         _ChargePerLine: "0.00",
                         _ChargePerUnit: "0.00",
                         _IsDiscount: "N"
                     }
                 ]
             },
             LineTaxes: {
                 LineTax: []
             },
             Notes: {
                 Note: []
             },
             Extn: {},
             btDisplay: {
                 defaultItemDescription: "",
                 defaultImageUrl: ""
             },
             btLogic: {
                 cartTimeStamp: new Date().getTime(),
                 isPriceOverridden: "N",
                 priceOverrideValue: 0,
                 isBigTicket: "N",
                 giftImgUrl: "assets/images/GreyGiftMessage.gif", //used in shippingSelection page
                 isGiftRegistryAddress: false,
                 giftBoxImgUrl: "assets/images/greyGiftBox.gif"  //used in shippingSelection page
             }
         };

         // can save any with draft order flag, so what do I NEED for easy order retrieval of draft order? Customer Name...
         var orderCart = {};

         var orderCartModel = {

             //Order attributes
             _AuthorizedClient: "Store", //"Store" for LUFI
             _BillToID: "",
             _CustomerContactID: "",
             _CustomerEMailID: "garrett.stibb@bonton.com",
             _DocumentType: "0001", //Alway "0001" for LUFI Sales Order
             _DraftOrderFlag: "Y", // "Y" for Draft Order
             _EnteredBy: "014265", // sales associates ID "014222"
             _EnterpriseCode: "BONTON", //always "BONTON"
             _EntryType: "BTN", //should be like "ELD" aka elderman OR else GetOrderCartPrice Will fail DO NOT USE 'Store'
             _OrderNo: "", //if blank Sterling will autogenerate new one
             _PaymentRuleId: "BT_DEF_PAYMENT_RULE", //what payment rules are used
             _SellerOrganizationCode: "101", //Store Node /number ex: "101"
             _TaxExemptFlag: "N",
             _TaxExemptionCertificate: "",

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
                         "_UnitPrice": "40.00",
                         "_TaxableFlag": "Y"
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
                         "itemType": "REG",
                         "storageType": "R",
                         "_AvailableQty": 2000,
                         "alternateUPCs": [
                           "031290088157"
                         ],
                         "imageURL": "/assets/images/865391.jpg",
                         "brandlongdesc": "Carter's",
                         "cfgdesc": "GIRLS",
                         "cfgid": 101,
                         "classlongdesc": "CARTERS",
                         "cmgdesc": "CHILDRENS",
                         "cmgid": 10,
                         "colorattrdesc": "Asst Lt/Pale",
                         "colorcode": "",
                         "colorDc": "",
                         "colorfamdesc": "Asst Family",
                         "colorlongdesc": "Navy",
                         "corpdesc": "",
                         "crgdesc": "CHILDRENS",
                         "crgid": 0,
                         "deptlongdesc": "GIRLSWEAR 2-6X",
                         "desc1": "TEE",
                         "desc2": "FALL CARTERS OP",
                         "desc3": "CARTERS OCT",
                         "desc4": "4-6X GIRL",
                         "fabdtldesc": "Screen Print",
                         "fablongdesc": "Cotton Blend-With Stretch",
                         "fobdesc": "GIRLS 2-6X",
                         "fobid": 28,
                         "genclasslongdesc": "Knit Tops",
                         "gensclalongdesc": "Tee",
                         "imageid": "",
                         "isn": "243306978",
                         "isnlongdesc": "GREEN FLORAL BABYDOLL TOP",
                         "itemsize": "4",
                         "keywords": "",
                         "labellongdesc": "Carter's",
                         "longpatterndesc": "",
                         "proddetail2": "Scoop Neck",
                         "proddetail3": "Knit-Interlock/Jersey",
                         "proddtllongdesc": "Short Sleeve",
                         "productcode": "",
                         "productname": "Green Floral Babydoll Top",
                         "size1code": "",
                         "size2code": "",
                         "sizedc": "",
                         "sizesequence": "1",
                         "vendorstyle": "273B053",
                         "webcatprodlongdesc": "",
                         "webid": ""
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
                         "btSpecialHandlingFee": "0",
                         giftImgUrl: "assets/images/GiftMessage.gif",
                         isGiftRegistryAddress: false,
                         giftBoxImgUrl: "assets/images/giftBox.gif"
                     }
                 }]
             },

             //PersonInfoBillTo Header Level
             PersonInfoBillTo: {
                 _AddressID: "LOGON5918",
                 _AddressLine1: "2412 56th ave",
                 _City: "kenosha",
                 _Country: "US",
                 _DayPhone: "2629605804",
                 _EMailID: "garrett.stibb@bonton.com",
                 _FirstName: "Garrett",
                 _LastName: "Stibb",
                 _PersonID: "8701684",
                 _PersonInfoKey: "20140819160910224059208",
                 _State: "WI",
                 _ZipCode: "53144"
             },

             //PersonInfoShipTo Header Level
             PersonInfoShipTo: {
                 _AddressID: "LOGON5918",
                 _AddressLine1: "2412 56th ave",
                 _City: "kenosha",
                 _Country: "US",
                 _DayPhone: "2629605804",
                 _EMailID: "garrett.stibb@bonton.com",
                 _FirstName: "Garrett",
                 _LastName: "Stibb",
                 _PersonID: "8701684",
                 _PersonInfoKey: "20140819160910224059208",
                 _State: "WI",
                 _ZipCode: "53144"
             },
             PaymentMethods: {                       //xml-json issue xml goes <PaymentMethods><PaymentMethod /><PaymentMethod /> ...
                 //so do PaymentMethods:[]  and have broker know the tag needs to be PaymentMethod OR make another object {PaymentMethod:[]}
                 PaymentMethod: [
                         {
                             _UnlimitedCharges: "Y",
                             _PaymentType: "CREDIT_CARD",
                             _DisplayCreditCardNo: "0691",
                             _CreditCardType: "CP",
                             _CreditCardNo: "8231211627154941",
                             _CreditCardExpDate: "04/2018",

                             PaymentDetails: {
                                 _ProcessedAmount: "102.65000",
                                 _ChargeType: "AUTHORIZATION",
                                 _AuthorizationID: "001060",
                                 _AuthCode: "001060"
                             }
                         }]
             },

             Extn: {},

             PriceInfo: { _Currency: "USD" } //price infor child node
         };

         //based on sterling getCustomerDetails
         var customerModel = {
             "Customer": {
                 "_CustomerType": "02",
                 "_OrganizationCode": "BONTON",
                 "_Status": "10",
                 "_ExternalCustomerID": "LOGON5918",
                 "_RegisteredDate": "2014-08-19T11:24:05-04:00",
                 "_CustomerID": "W231905",
                 "_CustomerKey": "20140819112410223924123",
                 "CustomerContactList": {
                     "_TotalNumberOfRecords": "1",
                     "CustomerContact": {
                         "_AggregateStatus": "10",
                         "CustomerAdditionalAddressList": {
                             "_TotalNumberOfRecords": "9",
                             "CustomerAdditionalAddress": [
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "Y",
                                     "_CustomerAdditionalAddressID": "100053538",
                                     "_AddressType": "SB",
                                     "_IsSoldTo": "Y",
                                     "_IsDefaultShipTo": "Y",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20150402190721329128955",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "6969696969",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
                                         "_PersonID": "8701684",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "53144",
                                         "_Title": "",
                                         "_City": "kenosha",
                                         "_AddressID": "LOGON5918",
                                         "_MiddleName": "",
                                         "_State": "WI",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "2412 56th ave",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100053539",
                                     "_AddressType": "SB",
                                     "_IsSoldTo": "Y",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20150402190721329128956",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "6969696969",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
                                         "_PersonID": "8701711",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "53132",
                                         "_Title": "",
                                         "_City": "Franklin",
                                         "_AddressID": "ShopRunner billing",
                                         "_MiddleName": "",
                                         "_State": "WI",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "8878 S 84th Street",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100053540",
                                     "_AddressType": "SB",
                                     "_IsSoldTo": "Y",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20150402190721329128957",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "6969696969",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "uid=logon5918,o=default organization,o=root organization",
                                         "_PersonID": "8701710",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "53144",
                                         "_Title": "",
                                         "_City": "kenosha",
                                         "_AddressID": "ShopRunner shipping",
                                         "_MiddleName": "",
                                         "_State": "WI",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "2412 56th ave",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100054291",
                                     "_AddressType": "",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20141009153257247979216",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "4143475323",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "",
                                         "_PersonID": "",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "53144",
                                         "_Title": "",
                                         "_City": "kenosha",
                                         "_AddressID": "LOGON5918",
                                         "_MiddleName": "",
                                         "_State": "WI",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "2412 56th ave",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100054305",
                                     "_AddressType": "",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20150402190721329128958",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "6969696969",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "",
                                         "_PersonID": "",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "96821",
                                         "_Title": "",
                                         "_City": "Honolulu",
                                         "_AddressID": "LOGON5918",
                                         "_MiddleName": "",
                                         "_State": "HI",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "34 Akilolo St",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100054614",
                                     "_AddressType": "",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20150402190721329128959",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "6969696969",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "",
                                         "_PersonID": "",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "53144",
                                         "_Title": "",
                                         "_City": "kenosha",
                                         "_AddressID": "LOGON5918",
                                         "_MiddleName": "",
                                         "_State": "WI",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "2412 22th street",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100054627",
                                     "_AddressType": "",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20150402190721329128960",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "6969696969",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "",
                                         "_PersonID": "",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "33019",
                                         "_Title": "",
                                         "_City": "Hollywood",
                                         "_AddressID": "Modified",
                                         "_MiddleName": "",
                                         "_State": "FL",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "904 N North Lake Dr",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100054629",
                                     "_AddressType": "",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20141215171030280899222",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "2625512222",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "",
                                         "_PersonID": "",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "33019",
                                         "_Title": "",
                                         "_City": "Hollywood",
                                         "_AddressID": "Florida",
                                         "_MiddleName": "",
                                         "_State": "FL",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "904 N North Lake Dr",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 },
                                 {
                                     "_IsShipTo": "Y",
                                     "_IsDefaultSoldTo": "N",
                                     "_IsDefaultBillTo": "N",
                                     "_CustomerAdditionalAddressID": "100055317",
                                     "_AddressType": "",
                                     "_IsDefaultShipTo": "N",
                                     "PersonInfo": {
                                         "_EMailID": "garrett.stibb@bonton.com",
                                         "_PersonInfoKey": "20150402190721329128961",
                                         "_VerificationStatus": "",
                                         "_PreferredShipAddress": "",
                                         "_DayPhone": "6969696969",
                                         "_LastName": "Stibb",
                                         "_EveningFaxNo": "",
                                         "_OtherPhone": "",
                                         "_DayFaxNo": "",
                                         "_HttpUrl": "",
                                         "_PersonID": "",
                                         "_FirstName": "Garrett",
                                         "_MobilePhone": "",
                                         "_Company": "",
                                         "_Department": "",
                                         "_AlternateEmailID": "",
                                         "_Suffix": "",
                                         "_Country": "US",
                                         "_ErrorTxt": "",
                                         "_ZipCode": "33019",
                                         "_Title": "",
                                         "_City": "Hollywood",
                                         "_AddressID": "Florida",
                                         "_MiddleName": "",
                                         "_State": "FL",
                                         "_AddressLine4": "",
                                         "_AddressLine5": "",
                                         "_AddressLine6": "",
                                         "_EveningPhone": "",
                                         "_JobTitle": "",
                                         "_Beeper": "",
                                         "_AddressLine1": "904 N North Lake Dr",
                                         "_UseCount": "0",
                                         "_AddressLine2": "",
                                         "_AddressLine3": ""
                                     },
                                     "_IsBillTo": "Y"
                                 }
                             ]
                         },
                         "_UserID": "",
                         "_DayPhone": "6969696969",
                         "_LastName": "Stibb",
                         "_Title": "",
                         "_EveningFaxNo": "",
                         "_CustomerContactID": "W231905",
                         "_DayFaxNo": "",
                         "_MiddleName": "",
                         "_FirstName": "Garrett",
                         "_EveningPhone": "",
                         "_Company": "",
                         "_MobilePhone": "",
                         "_EmailID": "garrett.stibb@bonton.com"
                     }
                 }
             }
         };

         $rootScope.$on('AddCustomerToCartCalled', function (event, customer) {
             _setCustomer(customer);
             //_repriceOrder(null, true);
         });

         var _setStoreAndAssociateId = function (storeNumber, associateId) {
             if (angular.isDefined(storeNumber) && storeNumber.toString().trim().length > 0) {
                 orderCart._EnteredBy = "Store";
             }
             if (angular.isDefined(associateId)) {
                 orderCart.Extn._ExtnAssociateId = associateId.toString();
             }
             if (angular.isDefined(storeNumber) && storeNumber.length > 0) {
                 orderCart._SellerOrganizationCode = storeNumber.toString();
            
                 var url = serviceURL + "/Organization/GetStoreInfo";
                 var contract = {
                     GetStoreInfoReq: {
                         Stores: [{
                             _StoreID: storeNumber
                         }]
                     }
                 }
                 $http.post(url, angular.toJson(contract)).success(function (data) {
                     if (data.GetStoreInfoResp.Stores && data.GetStoreInfoResp.Stores) {
                         orderCart.Extn._EntryType = data.GetStoreInfoResp.Stores[0];
                         orderCart._EntryType = data.GetStoreInfoResp.Stores[0];
                     } else {
                         orderCart.Extn._EntryType = 'BTN';
                         orderCart._EntryType = 'BTN';
                     }
                }).error(function (data) {
                     $sendSMTPErrorEmail(data, url, contract);
                     orderCart.Extn._EntryType = 'BTN';
                     orderCart._EntryType = 'BTN';
                 });
         };

     };

         $rootScope.$on('POSParmsSet', function (event, parameters) {
             _setStoreAndAssociateId(parameters.storeNumber, parameters.associateId);
         });

         //utility to create {PrimeLine:1, SubLine:1}
         var _getPrimeSubLineObject = function (orderline) {

             var object = {};
             if (angular.isDefined(orderline._PrimeLineNo)) {
                 object.PrimeLine = orderline._PrimeLineNo;
             } else {
                 return null;
             }

             if (angular.isDefined(orderline._SubLineNo)) {
                 object.SubLine = orderline._SubLineNo;
             } else {
                 object.SubLine = 1;
             }
             return object;
         };

         var _findPrimeSubInCart = function (primeSubObj, cart) {

             var returnedOrderlines = [];
             primeSubObj = _primeSubLineObjectCleaner(primeSubObj);

             if (angular.isDefined(cart) && angular.isDefined(cart.OrderLines) && angular.isArray(cart.OrderLines.OrderLine)) {

                 for (var i = 0; i < cart.OrderLines.OrderLine.length; i++) {

                     if (cart.OrderLines.OrderLine[i]._PrimeLineNo == primeSubObj.PrimeLine &&
                             cart.OrderLines.OrderLine[i]._SubLineNo == primeSubObj.SubLine) {

                         returnedOrderlines.push(cart.OrderLines.OrderLine[i]);

                     }
                 }
             }

             return returnedOrderlines;
         };
         //{PrimeLine:1, SubLine:1}
         var _deleteOrderLine = function (arrayOfPrimeSubLineObjects, _isCleanCartToo) {


             //clean out zero quanty and renumber prime and subline numbers
             if (_isCleanCartToo) {
                 _cleanOrderCart();
             }

             arrayOfPrimeSubLineObjects = _arrayOfPrimeSubCleaner(arrayOfPrimeSubLineObjects);

             //assumes orderCart orderlines are sorted
             for (var i = 0; arrayOfPrimeSubLineObjects.length > 0 && i < orderCart.OrderLines.OrderLine.length; i++) {

                 if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == arrayOfPrimeSubLineObjects[0].PrimeLine &&
                             orderCart.OrderLines.OrderLine[i]._SubLineNo == arrayOfPrimeSubLineObjects[0].SubLine) {

                     orderCart.OrderLines.OrderLine.splice(i, 1);
                     arrayOfPrimeSubLineObjects.shift();

                 }
             }


         };

         var _setOrderLineShipToAddresses = function (arrayOfPrimeSubLineObjects, addressObject, isGiftRegAddress) {
             arrayOfPrimeSubLineObjects = _arrayOfPrimeSubCleaner(arrayOfPrimeSubLineObjects);

             //assumes orderCart orderlines are sorted
             for (var i = 0; arrayOfPrimeSubLineObjects.length > 0 && i < orderCart.OrderLines.OrderLine.length; i++) {

                 if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == arrayOfPrimeSubLineObjects[0].PrimeLine &&
                             orderCart.OrderLines.OrderLine[i]._SubLineNo == arrayOfPrimeSubLineObjects[0].SubLine) {

                     _copyAddress(addressObject.PersonInfo, orderCart.OrderLines.OrderLine[i].PersonInfoShipTo);
                     orderCart.OrderLines.OrderLine[i].btLogic.shippingAddress = angular.copy(addressObject);

                     if (isGiftRegAddress) {
                         orderCart.OrderLines.OrderLine[i].btLogic.isGiftRegistryAddress = true;
                     } else {
                         orderCart.OrderLines.OrderLine[i].btLogic.isGiftRegistryAddress = false;
                     }

                     arrayOfPrimeSubLineObjects.shift();

                 }
             }
         };

         var _validAddress = function (address, isBillingAddress) {

             return $customer.validateAddress(address, isBillingAddress);
         };

         var _validateOrderHeader = function (inputCart) {
             if (!angular.isDefined(inputCart)) {
                 inputCart = orderCart;
             }

             var result = {
                 customerEmailIdError: { hasError: false, errorText: '' }
             };

             if (!angular.isDefined(inputCart._CustomerEMailID) || inputCart._CustomerEMailID === null || inputCart._CustomerEMailID.toString().trim() === '') {
                 result.customerEmailIdError.hasError = true;
                 result.customerEmailIdError.errorText = 'Order Header Error: No Customer Contact Email.';
             }

             return result;

         };

         var _hasBigTicketItems = function (inputCart) {
             if (!angular.isDefined(inputCart)) {
                 inputCart = orderCart;
             }

             if (angular.isDefined(inputCart.OrderLines) && angular.isArray(inputCart.OrderLines.OrderLine)) {

                 //test each orderline for item type of BGT
                 for (var i = 0; i < inputCart.OrderLines.OrderLine.length; i++) {
                     var currentLine = inputCart.OrderLines.OrderLine[i];

                     if (angular.isDefined(currentLine.ItemDetails) && angular.isDefined(currentLine.ItemDetails.PrimaryInformation) && angular.isDefined(currentLine.ItemDetails.PrimaryInformation._ItemType)) {
                         if ((/BGT/i).test(currentLine.ItemDetails.PrimaryInformation._ItemType)) {
                             return true;
                         }
                     } else if (angular.isDefined(currentLine.Extn) && angular.isDefined(currentLine.Extn._btIsBigTicket)) {
                         if ((/BGT/i).test(currentLine.Extn._btIsBigTicket)) {
                             return true;
                         }
                     }
                 }

                 return false;
             } else {
                 return false;
             }

         };

         var _validateOrderAddresses = function (inputCart) {
             if (!angular.isDefined(inputCart)) {
                 inputCart = orderCart;
             }

             var result = {
                 defaultBillingAddressError: { hasError: false, errorText: '' },
                 defaultShipToAddressError: { hasError: false, errorText: '' },
                 carrierServiceCodeError: { hasError: false, errorText: '', orderlines: [] },
                 bigTicketError: { hasError: false, errorText: '', orderlines: [] },
                 orderlineShiptoAddressError: { hasError: false, errorText: '', orderlines: [] }
             };

             //validate default billing address
             var validObj = _validAddress(inputCart.PersonInfoBillTo, true);

             result.defaultBillingAddressError.hasError = !validObj.isValid;
             result.defaultBillingAddressError.errorText = 'Order Default Bill To Errors: ' + validObj.errorMessage;

             //validate  default shipping address
             var validObj = _validAddress(inputCart.PersonInfoShipTo);

             result.defaultShipToAddressError.hasError = !validObj.isValid;
             result.defaultShipToAddressError.errorText = 'Order Default Ship To Errors: ' + validObj.errorMessage;

             var carrierErrorArray = [];//array of string to join() at end
             var bigTicketErrorArray = [];
             var orderlineErrorArray = [];
             var bigTicketPromiseArray = [];

             //validate orderline shipping addresses
             for (var i = 0; i < inputCart.OrderLines.OrderLine.length; i++) {
                 var currentOrderLine = inputCart.OrderLines.OrderLine[i];
                 var lineNo = _getPrimeSubLineObject(currentOrderLine);//{PrimeLine:1, SubLine:1}

                 //carrierServiceCodeError
                 if (!angular.isDefined(currentOrderLine._CarrierServiceCode) || currentOrderLine._CarrierServiceCode.toString().trim() === '') {
                     result.carrierServiceCodeError.hasError = true;
                     result.carrierServiceCodeError.orderlines.push(currentOrderLine);

                     carrierErrorArray.push('Line ' + lineNo.PrimeLine + '.' + lineNo.SubLine + ': Not set');
                 }

                 //orderlineShiptoAddressError
                 var validObj = _validAddress(currentOrderLine.PersonInfoShipTo);
                 if (!validObj.isValid) {
                     result.orderlineShiptoAddressError.hasError = true;
                     orderlineErrorArray.push('Line ' + lineNo.PrimeLine + '.' + lineNo.SubLine);
                     result.orderlineShiptoAddressError.orderlines.push(
                             {
                                 orderline: currentOrderLine,
                                 errorText:
                                     'Line ' + lineNo.PrimeLine + '.' + lineNo.SubLine + ' errors: ' + validObj.errorMessage
                             });
                 }

                 //bigTicketError
                 if (!angular.isDefined(currentOrderLine.ItemDetails) ||
                         !angular.isDefined(currentOrderLine.ItemDetails.PrimaryInformation) ||
                         !angular.isDefined(currentOrderLine.ItemDetails.PrimaryInformation._ItemType)) {
                     result.bigTicketError.hasError = true;
                     bigTicketErrorArray.push('Line ' + lineNo.PrimeLine + '.' + lineNo.SubLine + ' No ItemType');
                 } else if ((/^BGT$/).test(currentOrderLine.ItemDetails.PrimaryInformation._ItemType.toString().trim())) {
                     //check that address has valid big ticket delivery zip code.
                     try {
                         (function () {
                             var cPromise = $bigTicketValidate.bigTicketPromise(currentOrderLine.PersonInfoShipTo._ZipCode);
                             var line = currentOrderLine;

                             cPromise.then(function (response) {
                                 if (response.data.ValidateBigTicketZipCodeResp._statusMessage === "Success") {
                                     if (response.data.ValidateBigTicketZipCodeResp._isValid == "false") {
                                         result.bigTicketError.hasError = true;
                                         var cLineNo = _getPrimeSubLineObject(line);
                                         bigTicketErrorArray.push('Line ' + cLineNo.PrimeLine + '.' + cLineNo.SubLine + ' Cannot Deliver to zip:' +
                                                 line.PersonInfoShipTo._ZipCode);
                                         result.orderlineShiptoAddressError.orderlines.push(line);
                                     }
                                 } else {
                                     result.bigTicketError.hasError = true;
                                     var cLineNo = _getPrimeSubLineObject(line);
                                     bigTicketErrorArray.push('Line ' + cLineNo.PrimeLine + '.' + cLineNo.SubLine + ' Big Ticket Service Down. Please retry.');
                                 }
                             });

                             bigTicketPromiseArray.push(cPromise);
                         })();
                     } catch (e) { }
                 }
             }

             return $q.all(bigTicketPromiseArray).then(function () {
                 result.carrierServiceCodeError.errorText = 'CarrierService Errors: ' + carrierErrorArray.join(', ');
                 result.bigTicketError.errorText = 'BigTicket Errors: ' + bigTicketErrorArray.join(', ');
                 result.orderlineShiptoAddressError.errorText = 'Orderline Ship To Address Errors: ' + orderlineErrorArray.join('; ');

                 return result;
             });

         };



         var _setBillingAddress = function (addressObject) {
             if (addressObject) {
                 _copyAddress(addressObject.PersonInfo, orderCart.PersonInfoBillTo);
                 orderCart.btLogic.defaultBillToAddress = angular.copy(addressObject);
             } else {
                 _copyAddress(null, orderCart.PersonInfoBillTo);
                 orderCart.btLogic.defaultBillToAddress = null;
             }
         };
         var _setDefaultShipToAddress = function (addressObject) {
             if (addressObject) {
                 _copyAddress(addressObject.PersonInfo, orderCart.PersonInfoShipTo);
                 orderCart.btLogic.defaultShipToAddress = angular.copy(addressObject);
             } else {
                 _copyAddress(null, orderCart.PersonInfoShipTo);
                 orderCart.btLogic.defaultShipToAddress = null;
             }
         };

         var _getRegistryAddressSet = function () { return _giftRegistryAddressSet; };

         var _loadRegistryAddresses = function () {

             var registryOnOrderSet = {};
             //get all giftRegNo's on orderlines
             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
                 if (angular.isDefined(orderCart.OrderLines.OrderLine[i].Extn._ExtnGiftRegistryNo) &&
                         orderCart.OrderLines.OrderLine[i].Extn._ExtnGiftRegistryNo.toString().trim().length > 0) {
                     var registryValue = orderCart.OrderLines.OrderLine[i].Extn._ExtnGiftRegistryNo;
                     if (!(registryValue in registryOnOrderSet)) {
                         registryOnOrderSet[registryValue] = registryValue;
                     }
                 }

             }

             var missingRegistryAddress = {};
             //check if giftRegNo has address in giftRegistryAddressSet
             for (var registryValue in registryOnOrderSet) {
                 if (!(registryValue in _giftRegistryAddressSet)) {
                     missingRegistryAddress[registryValue] = registryValue;
                 }
             }

             var promiseArr = [];
             //all addresses missing from giftRegistryAddressSet must get from GiftReg Web Service
             for (var registryValue in missingRegistryAddress) {

                 // call GiftReg Web Service
                 promiseArr.push(giftRegistryService.preferredAddressPromise(registryValue).then(function (response) {
                     if (response.data.PreferredAddressOutput.PreferredAddressResponse.IsValid === 'true' &&
                             response.data.PreferredAddressOutput.PreferredAddressResponse.HasErrors === 'false') {
                         //add addresses to giftRegistryAddressSet
                         var normalizedAddress = giftRegistryService.constructRegistryAddress(registryValue, response.data.PreferredAddressOutput.PreferredAddressResponse);
                         _giftRegistryAddressSet[registryValue.toString().trim()] = normalizedAddress;

                     } else {
                         //warn that registry number is invalid but leave it
                         // Can I throw inside of a promise?
                         var ErrorMessage = '';
                         if (response.data.PreferredAddressOutput.PreferredAddressResponse.HasErrors === 'true') {
                             ErrorMessage = response.data.PreferredAddressOutput.PreferredAddressResponse.ErrorMessage;
                         }
                         throw errorObj.newError('_loadRegistryAddresses():InvalidRegNo', "The Registry Number: " + registryValue + " is invalid.",
                         "_loadRegistryAddresses(): The Registry Number: " + registryValue + " is invalid. Error Message: '" + ErrorMessage + "'", 'InvalidRegNo', 'WARN');
                     }
                 }, function (data) {
                     $sendSMTPErrorEmail(data, serviceURL.toString() + '/GiftRegistry/GetPreferredAddress');
                 }));
             }


             return $q.all(promiseArr).then(function () { return _giftRegistryAddressSet; });

         };

         var _getCustomersDefaultBillToAndShipTo = function (customerObject) {
             var defaultShipToKey = null;
             var defaultShipToAddress = null;

             var defaultBillToKey = null;
             var defaultBillToAddress = null;

             for (var i = 0; i < customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress.length; i++) {
                 if (customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._IsDefaultBillTo == 'Y') {
                     defaultBillToAddress = customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i];
                     defaultBillToKey = defaultBillToAddress.PersonInfo._PersonInfoKey;
                 }

                 if (customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._IsDefaultShipTo == 'Y') {
                     defaultShipToAddress = customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i];
                     defaultShipToKey = defaultShipToAddress.PersonInfo._PersonInfoKey;
                 }

                 if (defaultBillToKey && defaultShipToKey) {
                     break;
                 }
             }

             if (defaultBillToKey === null || defaultShipToKey === null) {
                 if (defaultBillToKey === null) {
                     if (customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress.length > 0) {
                         defaultBillToAddress = customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[0];
                         defaultBillToKey = defaultBillToAddress.PersonInfo._PersonInfoKey;
                     }
                 }
                 if (defaultShipToKey === null) {
                     //must be US country for ship to address
                     for (var i = 0; i < customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress.length; i++) {

                         if (customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i].PersonInfo._Country.toString().trim().toUpperCase() == 'US') {
                             defaultShipToAddress = customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i];
                             defaultShipToKey = defaultShipToAddress.PersonInfo._PersonInfoKey;
                             break;
                         }
                     }
                 }
             }

             return {
                 defaultShipToKey: defaultShipToKey,
                 defaultShipToAddress: defaultShipToAddress,
                 defaultBillToKey: defaultBillToKey,
                 defaultBillToAddress: defaultBillToAddress
             };
         };

         var _resetOrderAddresses = function (customerObject) {

             //reset order header to customer info, shipto and billto
             var defaultObject = _getCustomersDefaultBillToAndShipTo(customerObject);

             if (defaultObject.defaultBillToAddress === null || defaultObject.defaultShipToAddress === null) {

                 $loggerService.log('_resetOrderAddresses() did not find a default Bill To or Default Ship To for customer. defaultBillToKey:' +
                         defaultObject.defaultBillToAddress + '  defaultShipToKey:' + defaultObject.defaultShipToAddress);
                 return;
             }

             orderCart._BillToID = customerObject._CustomerID;
             orderCart._CustomerContactID = customerObject.CustomerContactList.CustomerContact[0]._CustomerContactID;
             orderCart._CustomerEMailID = customerObject.CustomerContactList.CustomerContact[0]._EmailID;

             if (defaultObject.defaultBillToAddress) {
                 _setBillingAddress(defaultObject.defaultBillToAddress);
             }

             if (defaultObject.defaultShipToAddress) {
                 _setDefaultShipToAddress(defaultObject.defaultShipToAddress);
             }
             //reset order lines to default shipto and add isGiftRegistryAddress to btLogic
             //  gift registry (multiple addresses?) to gift address

             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
                 var tempOrderline = orderCart.OrderLines.OrderLine[i];
                 var tempShipToAddress = defaultObject.defaultShipToAddress;

                 if (tempOrderline.Extn._ExtnGiftRegistryNo && tempOrderline.Extn._ExtnGiftRegistryNo.trim()) {
                     if (tempOrderline.Extn._ExtnGiftRegistryNo in _giftRegistryAddressSet) {
                         tempShipToAddress = _giftRegistryAddressSet[tempOrderline.Extn._ExtnGiftRegistryNo];
                         tempOrderline.btLogic.isGiftRegistryAddress = true;
                     }
                 }

                 _copyAddress(tempShipToAddress.PersonInfo, tempOrderline.PersonInfoShipTo);
                 tempOrderline.btLogic.shippingAddress = angular.copy(tempShipToAddress);
             }
         };

         var _customerAddressCheck = function (customerObject) {

             var addressMap = {};

             //reset Order Header level email (the customer contact's email)
             orderCart._CustomerEMailID = customerObject.CustomerContactList.CustomerContact[0]._EmailID;

             for (var i = 0; i < customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress.length; i++) {
                 addressMap[customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i]._CustomerAdditionalAddressID] = customerObject.CustomerContactList.CustomerContact[0].CustomerAdditionalAddressList.CustomerAdditionalAddress[i];
             }
             //get customer defaults and reset ship to default
             var defaultObject = _getCustomersDefaultBillToAndShipTo(customerObject);

             if (defaultObject.defaultBillToAddress === null || defaultObject.defaultShipToAddress === null) {

                 $loggerService.log('_resetOrderAddresses() did not find a default Bill To or Default Ship To for customer. defaultBillToKey:' +
                         defaultObject.defaultBillToAddress + '  defaultShipToKey:' + defaultObject.defaultShipToAddress);
                 return;
             }

             if (defaultObject.defaultShipToAddress) {
                 _setDefaultShipToAddress(defaultObject.defaultShipToAddress);
             }

             //reset bill to address to default only if no customer additional address matches.
             if (defaultObject.defaultBillToAddress) {

                 if (orderCart.btLogic.defaultBillToAddress._CustomerAdditionalAddressID.toString() in addressMap) {

                     //if _PersonInfoKey in customer does not match orderline's personInfoKey then reset, else skip to next line
                     if (orderCart.PersonInfoBillTo._PersonInfoKey.toString().trim() !==
                             addressMap[orderCart.btLogic.defaultBillToAddress._CustomerAdditionalAddressID.toString()].PersonInfo._PersonInfoKey.toString().trim()) {

                         _setBillingAddress(addressMap[orderCart.btLogic.defaultBillToAddress._CustomerAdditionalAddressID.toString()]);
                     }
                 }//else set to default found
                 else {
                     _setBillingAddress(defaultObject.defaultBillToAddress);
                 }
             }


             //for each orderline match _CustomerAdditionalAddressID
             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {

                 var currentOrderLine = orderCart.OrderLines.OrderLine[i];
                 //if _CustomerAdditionalAddressID in addressMap
                 if (currentOrderLine.btLogic.shippingAddress._CustomerAdditionalAddressID.toString() in addressMap) {

                     //if _PersonInfoKey in customer does not match orderline's personInfoKey then reset, else skip to next line
                     if (currentOrderLine.PersonInfoShipTo._PersonInfoKey.toString().trim() !==
                             addressMap[currentOrderLine.btLogic.shippingAddress._CustomerAdditionalAddressID.toString()].PersonInfo._PersonInfoKey.toString().trim()) {

                         _setOrderLineShipToAddresses(currentOrderLine, addressMap[currentOrderLine.btLogic.shippingAddress._CustomerAdditionalAddressID.toString()], false);
                     }
                 }//else if ship to address is NOT a gift registry address set line to default ship to
                 else if (!angular.isDefined(currentOrderLine.btLogic.isGiftRegistryAddress) || !currentOrderLine.btLogic.isGiftRegistryAddress) {
                     _setOrderLineShipToAddresses(currentOrderLine, defaultObject.defaultShipToAddress, false);

                 } else {
                     $loggerService.log('Skipping _customerAddressCheck of gift reg address');
                 }
             }
         };

         var _setCustomer = function (customerObject) {

             if (customerObject._CustomerKey !== _currentCustomerKey) {
                 _currentCustomerKey = customerObject._CustomerKey;
                 return _resetOrderAddresses(customerObject);
             } else {
                 //update all addresses in case of customer modifications
                 _customerAddressCheck(customerObject);
                 return true; //TODO: Is return correct. Should be same style as _resetOrderAddresses
             }
         };

         var _getCustomerKey = function () {

             if (_currentCustomerKey.length > 0) {
                 return _currentCustomerKey;
             } else {
                 return null;
             }
         };

         var _isCartCustomerSet = function () {

             if (_currentCustomerKey.length > 0) {
                 return true;
             } else {
                 return false;
             }
         };

         var _deleteCustomer = function () {

             _currentCustomerKey = '';
             orderCart._BillToID = '';
             orderCart._CustomerContactID = '';
             orderCart._CustomerEMailID = '';

             _setBillingAddress(null);

             _setDefaultShipToAddress(null);

             //reset order lines to default shipto and add isGiftRegistryAddress to btLogic
             //  gift registry (multiple addresses?) to gift address

             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
                 var tempOrderline = orderCart.OrderLines.OrderLine[i];
                 var tempShipToAddress = null;

                 if (tempOrderline.Extn._ExtnGiftRegistryNo && tempOrderline.Extn._ExtnGiftRegistryNo.trim()) {
                     if (tempOrderline.Extn._ExtnGiftRegistryNo in _giftRegistryAddressSet) {
                         tempShipToAddress = _giftRegistryAddressSet[tempOrderline.Extn._ExtnGiftRegistryNo];
                         tempOrderline.btLogic.isGiftRegistryAddress = true;
                     }
                 } else {
                     tempOrderline.btLogic.isGiftRegistryAddress = false;
                 }

                 if (tempShipToAddress) {
                     _copyAddress(tempShipToAddress.PersonInfo, tempOrderline.PersonInfoShipTo);
                     tempOrderline.btLogic.shippingAddress = angular.copy(tempShipToAddress);
                 } else {
                     _copyAddress(null, tempOrderline.PersonInfoShipTo);
                     tempOrderline.btLogic.shippingAddress = null;
                 }
             }

             $rootScope.$broadcast('orderCartReset', { cart: _getLiveOrderCart() });
         };

         var _copyAddress = function (source, destination) {

             if (!destination) { return false; };
             if (source) {
                 destination._AddressID = source._AddressID ? source._AddressID : "";
                 destination._AddressLine1 = source._AddressLine1 ? source._AddressLine1 : "";
                 destination._AddressLine2 = source._AddressLine2 ? source._AddressLine2 : "";
                 destination._AddressLine3 = source._AddressLine3 ? source._AddressLine3 : "";
                 destination._City = source._City ? source._City : "";
                 destination._Country = source._Country ? source._Country : "";
                 destination._DayPhone = source._DayPhone ? $filter('phoneFormatRemove')(source._DayPhone) : "";
                 destination._EveningPhone = source._EveningPhone ? $filter('phoneFormatRemove')(source._EveningPhone) : "";
                 destination._EMailID = source._EMailID ? source._EMailID : "";
                 destination._FirstName = source._FirstName ? source._FirstName : "";
                 destination._MiddleName = source._MiddleName ? source._MiddleName : "";
                 destination._LastName = source._LastName ? source._LastName : "";
                 destination._PersonID = source._PersonID ? source._PersonID : "";
                 destination._PersonInfoKey = source._PersonInfoKey ? source._PersonInfoKey : "";
                 destination._State = source._State ? source._State : "";
                 destination._ZipCode = source._ZipCode ? source._ZipCode : "";
             }
             else {
                 destination._AddressID = "";
                 destination._AddressLine1 = "";
                 destination._AddressLine2 = "";
                 destination._AddressLine3 = "";
                 destination._City = "";
                 destination._Country = "";
                 destination._DayPhone = "";
                 destination._EveningPhone = "";
                 destination._EMailID = "";
                 destination._FirstName = "";
                 destination._MiddleName = "";
                 destination._LastName = "";
                 destination._PersonID = "";
                 destination._PersonInfoKey = "";
                 destination._State = "";
                 destination._ZipCode = "";
             }
             return true;
         };

         var _setGiftMessage = function (arrayOfPrimeSubLineObjects, giftMessageInputs) {

             arrayOfPrimeSubLineObjects = _arrayOfPrimeSubCleaner(arrayOfPrimeSubLineObjects);

             //clean giftMessageInputs
             if (!giftMessageInputs) {
                 giftMessageInputs = {};
             }
             if (!giftMessageInputs.To) { giftMessageInputs.To = ""; }
             if (!giftMessageInputs.From) { giftMessageInputs.From = ""; }
             if (!giftMessageInputs.Message) { giftMessageInputs.Message = ""; }
             giftMessageInputs.To = giftMessageInputs.To.trim();
             giftMessageInputs.From = giftMessageInputs.From.trim();
             giftMessageInputs.Message = giftMessageInputs.Message.trim();

             //assumes orderCart orderlines are sorted
             for (var i = 0; arrayOfPrimeSubLineObjects.length > 0 && i < orderCart.OrderLines.OrderLine.length; i++) {

                 if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == arrayOfPrimeSubLineObjects[0].PrimeLine &&
                             orderCart.OrderLines.OrderLine[i]._SubLineNo == arrayOfPrimeSubLineObjects[0].SubLine) {

                     var noteArr = orderCart.OrderLines.OrderLine[i].Notes.Note;

                     //clean out gift message notes
                     for (var h = 0; h < noteArr.length; h++) {
                         var reasonCode = noteArr[h]._ReasonCode;
                         if (reasonCode === btProp.getProp("btGiftMessageReasonTo") ||
                             reasonCode === btProp.getProp("btGiftMessageReasonFrom") ||
                             reasonCode === btProp.getProp("btGiftMessageReasonMessage")) {
                             noteArr.splice(h, 1);
                             --h;
                         }
                     }

                     //add gift notes and update orderline's btLogic.giftImgUrl: "assets/images/GiftMessage.gif"
                     var noteSet = false;
                     if (giftMessageInputs.To) {
                         noteArr.push({ "_NoteText": giftMessageInputs.To.toString(), "_ReasonCode": btProp.getProp("btGiftMessageReasonTo").toString() });
                         noteSet = true;
                     }
                     if (giftMessageInputs.From) {
                         noteArr.push({ "_NoteText": giftMessageInputs.From.toString(), "_ReasonCode": btProp.getProp("btGiftMessageReasonFrom").toString() });
                         noteSet = true;
                     }
                     if (giftMessageInputs.Message) {
                         noteArr.push({ "_NoteText": giftMessageInputs.Message.toString(), "_ReasonCode": btProp.getProp("btGiftMessageReasonMessage").toString() });
                         noteSet = true;
                     }
                     if (noteSet) {
                         orderCart.OrderLines.OrderLine[i].btLogic.giftImgUrl = btProp.getProp("btGiftMessageIcon");
                     } else {
                         orderCart.OrderLines.OrderLine[i].btLogic.giftImgUrl = btProp.getProp("btGiftMessageIconGrey");
                     }

                     arrayOfPrimeSubLineObjects.shift();
                 }
             }
         };

         // STUB OF _setGiftRegistry
         var _setGiftRegistry = function (arrayOfPrimeSubLineObjects, regNo, regAddress) {

             arrayOfPrimeSubLineObjects = _arrayOfPrimeSubCleaner(arrayOfPrimeSubLineObjects);

             if (regNo) {
                 regNo = regNo.toString().trim();
             } else {
                 regNo = '';
             }

             //incase gift registry is down, check if address is passed
             if (regNo && regAddress) {

                 //fix if reg is missing phone number
                 if (!(regAddress.PersonInfo && angular.isString(regAddress.PersonInfo._DayPhone) && (regAddress.PersonInfo._DayPhone.length === 10))) {
                     //default to default billing phone number
                     regAddress.PersonInfo._DayPhone = $filter('phoneFormatRemove')(orderCart.btLogic.defaultBillToAddress.PersonInfo._DayPhone.toString());

                     if (!(regAddress.PersonInfo._DayPhone.length === 10)) {
                         regAddress.PersonInfo._DayPhone = "8009454438";
                     }
                 }

                 regNo = regNo.toString().trim();
                 if (!(regNo in _giftRegistryAddressSet)) {
                     _giftRegistryAddressSet[regNo] = regAddress;
                 }

                 //use address in _giftRegistryAddressSet
                 _setOrderLineShipToAddresses(angular.copy(arrayOfPrimeSubLineObjects), _giftRegistryAddressSet[regNo], true);

             } else {
                 //default address is one on order header shipTo
                 _setOrderLineShipToAddresses(angular.copy(arrayOfPrimeSubLineObjects), { PersonInfo: orderCart.PersonInfoShipTo }, false);
             }

             //assumes orderCart orderlines are sorted
             for (var i = 0; arrayOfPrimeSubLineObjects.length > 0 && i < orderCart.OrderLines.OrderLine.length; i++) {

                 if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == arrayOfPrimeSubLineObjects[0].PrimeLine &&
                             orderCart.OrderLines.OrderLine[i]._SubLineNo == arrayOfPrimeSubLineObjects[0].SubLine) {

                     orderCart.OrderLines.OrderLine[i].Extn._ExtnGiftRegistryNo = regNo;

                     //if there is a registry number then set line as gift line
                     if (regNo.length > 0) {
                         orderCart.OrderLines.OrderLine[i]._GiftFlag = 'Y';
                     }

                     arrayOfPrimeSubLineObjects.shift();

                 }
             }
         };

         var _getGiftOptionFromOrderLines = function (primeSubLineObject) {

             var arrayOfPrimeSubLineObjects = _arrayOfPrimeSubCleaner(primeSubLineObject);

             var returnObject = { To: "", From: "", Message: "", Registry: "" };

             for (var index = 0; index < arrayOfPrimeSubLineObjects.length; index++) {

                 for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {

                     if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == arrayOfPrimeSubLineObjects[index].PrimeLine &&
                                 orderCart.OrderLines.OrderLine[i]._SubLineNo == arrayOfPrimeSubLineObjects[index].SubLine) {

                         //if a to, from, or message has already been found, skip
                         if (!(returnObject.To || returnObject.From || returnObject.Message)) {
                             var noteArr = orderCart.OrderLines.OrderLine[i].Notes.Note;

                             for (var h = 0; h < noteArr.length; h++) {
                                 var reasonCode = noteArr[h]._ReasonCode;
                                 if (reasonCode === btProp.getProp("btGiftMessageReasonTo")) {
                                     returnObject.To = noteArr[h]._NoteText.toString();
                                 }
                                 if (reasonCode === btProp.getProp("btGiftMessageReasonFrom")) {
                                     returnObject.From = noteArr[h]._NoteText.toString();
                                 }
                                 if (reasonCode === btProp.getProp("btGiftMessageReasonMessage")) {
                                     returnObject.Message = noteArr[h]._NoteText.toString();
                                 }
                             }
                         }

                         //if no Registry value found yet in iteration, get Registry Value
                         if (!returnObject.Registry) {
                             returnObject.Registry = orderCart.OrderLines.OrderLine[i].Extn._ExtnGiftRegistryNo;
                         }

                         if ((returnObject.To || returnObject.From || returnObject.Message) && returnObject.Registry) {
                             return returnObject;
                         }
                     } else {
                         continue;
                     }
                 }
             }

             if (returnObject.To || returnObject.From || returnObject.Message || returnObject.Registry) {
                 return returnObject;
             } else {
                 return null;
             }
         };

         var _arrayOfPrimeSubCleaner = function (arrayOfPrimeSubLineObjects) {
             var returnArray = [];
             if (angular.isObject(arrayOfPrimeSubLineObjects) && !angular.isArray(arrayOfPrimeSubLineObjects)) {
                 arrayOfPrimeSubLineObjects = [arrayOfPrimeSubLineObjects];
             }
             if (angular.isArray(arrayOfPrimeSubLineObjects)) {
                 angular.forEach(arrayOfPrimeSubLineObjects, function (primeSubLineObject) {
                     var tempObject = _primeSubLineObjectCleaner(primeSubLineObject);
                     if (tempObject) {
                         returnArray.push(tempObject);
                     }
                 });
             }

             returnArray.sort(
                     function (a, b) {
                         if ((a.PrimeLine - b.PrimeLine) === 0) {
                             return a.SubLine - b.SubLine;
                         } else {
                             return a.PrimeLine - b.PrimeLine;
                         }
                     });

             return returnArray;
         };

         //input can be orderline or primeSubObject {PrimeLine:1,SubLine:3}
         var _primeSubLineObjectCleaner = function (primeSubObject) {
             var returnObject = null;

             //check if object is not a primeSubObject
             if (!angular.isDefined(primeSubObject.PrimeLine)) {
                 primeSubObject = _getPrimeSubLineObject(primeSubObject);
             }

             if (angular.isObject(primeSubObject) && isFinite(parseInt(primeSubObject.PrimeLine))) {
                 returnObject = {};
                 returnObject.PrimeLine = parseInt(primeSubObject.PrimeLine);
                 returnObject.SubLine = (primeSubObject.SubLine && isFinite(parseInt(primeSubObject.SubLine))) ? parseInt(primeSubObject.SubLine) : 1;
             }
             return returnObject;
         };

         var _sortOrderCartOrderlines = function () {

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

         var _padFront = function (stringOrNum, totalLength, optionalPadChar) {

             if (isFinite(parseInt(totalLength))) {
                 totalLength = parseInt(totalLength);
             } else {
                 return null;
             }

             if (angular.isNumber(stringOrNum)) {
                 stringOrNum = stringOrNum.toString();
             }

             var padChar = "0"; //zero
             if (angular.isDefined(optionalPadChar)) {
                 try {
                     padChar = "" + optionalPadChar;
                 } catch (e) {
                     padChar = "0";
                 }
             }

             if (stringOrNum.length >= totalLength) {
                 return stringOrNum;
             }

             while (padChar.length <= totalLength) {
                 padChar += padChar;
             }

             return (padChar + stringOrNum).slice(0 - totalLength);
         };

         var _getAvailableInventoryAndOrderedQty = function (orderline) {

             var sku = orderline.Item._ItemID;
             return _getLargestAvailableInventoryAndOrderedQtyForSkuInCart(sku);
         };
         /**
          *_getLargestAvailableInventoryAndOrderedQtyForSkuInCart loops through all orderlines
          *  and sums up the quantity currently ordered for that SKU.  It also finds the
          *  LARGEST available quantity reported matching skus.
          * 
          * @param {item id/sku} itemSku
          * @returns {Object} Object {largestAvailableInventory:20, orderedQty: 0}
          */
         var _getLargestAvailableInventoryAndOrderedQtyForSkuInCart = function (itemSku) {

             var largestAvailableInventory = 0;
             var orderedQty = 0;

             //count quantity of lines with sku and determine largest available quantity for sku
             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {

                 if (itemSku == orderCart.OrderLines.OrderLine[i].Item._ItemID) {
                     orderedQty += parseInt(orderCart.OrderLines.OrderLine[i]._OrderedQty);

                     if (isFinite(parseInt(orderCart.OrderLines.OrderLine[i].btDisplay._AvailableQty)) &&
                             largestAvailableInventory < parseInt(orderCart.OrderLines.OrderLine[i].btDisplay._AvailableQty)) {

                         largestAvailableInventory = parseInt(orderCart.OrderLines.OrderLine[i].btDisplay._AvailableQty);
                     }
                 }
             }

             return { largestAvailableInventory: largestAvailableInventory, orderedQty: orderedQty };
         };

         var _setOrderLineQuantity = function (primeSubLineObject, newQuantity) {

             primeSubLineObject = _primeSubLineObjectCleaner(primeSubLineObject);
             newQuantity = parseInt(newQuantity);

             if (primeSubLineObject === null) {

                 throw errorObj.newError('_setOrderLineQuantity():InputError', "Coding Error, please contact Help Desk.",
                     "_setOrderLineQuantity(): passed PrimeLineNo / SubLineNo is invalid.", 'InvalidPrimeLineObject', 'WARN');
             }

             if (!isFinite(newQuantity)) {

                 throw errorObj.newError('_setOrderLineQuantity():InputError', "Coding Error, please contact Help Desk.",
                     "_setOrderLineQuantity(): Quantity of " + newQuantity + " must be positive, finite integer.", 'InvalidQuantity', 'WARN');
             }
             if ((newQuantity < 0)) {
                 return;
             }

             var totalOrderedQuantityOfSku = 0;
             var refToChangingOrderLine = null;
             var largestAvailableQuantity = undefined;
             var itemSku = "";

             //find Orderline and sku
             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
                 if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == primeSubLineObject.PrimeLine &&
                             orderCart.OrderLines.OrderLine[i]._SubLineNo == primeSubLineObject.SubLine) {

                     refToChangingOrderLine = orderCart.OrderLines.OrderLine[i];
                     itemSku = orderCart.OrderLines.OrderLine[i].Item._ItemID;
                     largestAvailableQuantity = parseInt(orderCart.OrderLines.OrderLine[i].btDisplay._AvailableQty);

                     if (!(angular.isDefined(largestAvailableQuantity)) || !isFinite(largestAvailableQuantity) || largestAvailableQuantity < 0) {
                         $loggerService.log("JS: appServiceOrderCart: invalid  orderable inventory value: " + largestAvailableQuantity.toString());
                         largestAvailableQuantity = 1000000;
                     }
                 }
             }

             if (refToChangingOrderLine === null) {
                 throw errorObj.newError('_setOrderLineQuantity():InputError', "Coding Error, please contact Help Desk.",
                     "_setOrderLineQuantity(): Could not find PrimeLine: " + primeSubLineObject.PrimeLine +
                         " Subline: " + primeSubLineObject.SubLine + " in the order.", 'PrimelineSublineNotFound', 'WARN');
             }

             //if newQuantity is less than old quantity, just set it
             if (newQuantity <= parseInt(refToChangingOrderLine._OrderedQty)) {
                 refToChangingOrderLine._OrderedQty = "" + newQuantity;
                 return;

             } else {

                 //count quantity of lines with sku and determine largest available quantity for sku
                 var temp = _getLargestAvailableInventoryAndOrderedQtyForSkuInCart(itemSku);
                 totalOrderedQuantityOfSku = temp.orderedQty;
                 largestAvailableQuantity = temp.largestAvailableInventory;
                 var oldValue = '';


                 //if newQuantity create order larger than available inventory, set newQuantity to max possible
                 if ((newQuantity + totalOrderedQuantityOfSku - parseInt(refToChangingOrderLine._OrderedQty)) >
                        largestAvailableQuantity) {

                     oldValue = newQuantity.toString();

                     newQuantity = largestAvailableQuantity -
                             (totalOrderedQuantityOfSku - parseInt(refToChangingOrderLine._OrderedQty));

                     if (newQuantity < 0) {
                         newQuantity = 0;
                     }
                 }

                 refToChangingOrderLine._OrderedQty = "" + newQuantity;

                 if (oldValue) {
                     throw errorObj.newError('_setOrderLineQuantity():AvailableQtyError', "The item: " + refToChangingOrderLine.btDisplay.defaultItemDescription.toString() +
                             ' has available inventory of ' + largestAvailableQuantity.toString() + ". We lowered the line's quantity to " + newQuantity + ".",
                     '_setOrderLineQuantity(): had to lower quantity amount', 'AvailableQtyError', 'WARN');

                 }
             }
         };

         var _addGiftBox = function (arrayOfPrimeSubLineObjects) {
             $loggerService.log("method is not fully implemented");

             arrayOfPrimeSubLineObjects = _arrayOfPrimeSubCleaner(arrayOfPrimeSubLineObjects);

             //assumes orderCart orderlines are sorted
             for (var i = 0; arrayOfPrimeSubLineObjects.length > 0 && i < orderCart.OrderLines.OrderLine.length; i++) {

                 if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == arrayOfPrimeSubLineObjects[0].PrimeLine &&
                             orderCart.OrderLines.OrderLine[i]._SubLineNo == arrayOfPrimeSubLineObjects[0].SubLine) {

                     orderCart.OrderLines.OrderLine[i].btLogic.giftBoxImgUrl = btProp.getProp("btGiftBoxIcon");

                     arrayOfPrimeSubLineObjects.shift();
                 }
             }
         };
         var _deleteGiftBox = function (arrayOfPrimeSubLineObjects) {
             $loggerService.log("method is not fully implemented");

             arrayOfPrimeSubLineObjects = _arrayOfPrimeSubCleaner(arrayOfPrimeSubLineObjects);

             //assumes orderCart orderlines are sorted
             for (var i = 0; arrayOfPrimeSubLineObjects.length > 0 && i < orderCart.OrderLines.OrderLine.length; i++) {

                 if (orderCart.OrderLines.OrderLine[i]._PrimeLineNo == arrayOfPrimeSubLineObjects[0].PrimeLine &&
                             orderCart.OrderLines.OrderLine[i]._SubLineNo == arrayOfPrimeSubLineObjects[0].SubLine) {

                     orderCart.OrderLines.OrderLine[i].btLogic.giftBoxImgUrl = btProp.getProp("btGiftBoxIconGrey");

                     arrayOfPrimeSubLineObjects.shift();
                 }
             }
         };
         var _getTaxExemptionCertificate = function () {

             var certValue = "";
             if (orderCart._TaxExemptFlag && orderCart._TaxExemptFlag === 'Y') {
                 if (orderCart._TaxExemptionCertificate) {
                     certValue = orderCart._TaxExemptionCertificate.toString().trim();
                 }
             }

             return certValue;
         };
         var _setTaxExemptionCertificate = function (taxCert) {
             if (angular.isString(taxCert) && (taxCert.trim() !== "")) {
                 orderCart._TaxExemptFlag = 'Y';
                 orderCart._TaxExemptionCertificate = taxCert.trim();
             } else {
                 orderCart._TaxExemptFlag = 'N';
                 orderCart._TaxExemptionCertificate = "";
             }
         };

         var _constructOrderLine = function (itemObject, quantity) {

             var copyItemObject = angular.copy(itemObject);

             var workingOrderLine = angular.copy(_INIT_ORDER_LINE);

             //set product or isn description 
             if (copyItemObject.productname && (copyItemObject.productname.trim() !== "")) {
                 copyItemObject.defaultItemDescription = copyItemObject.productname;
             } else {
                 copyItemObject.defaultItemDescription = copyItemObject.isnlongdesc;
             }

             //TODO: get broker URL from properties Service colorimageid
             if(angular.isString(copyItemObject.colorimageid) && (copyItemObject.colorimageid.trim() !== "")){
                 copyItemObject.defaultImageUrl = serviceURL.toString() + "/image/BonTon/" + copyItemObject.colorimageid.trim();
             } else if (angular.isDefined(copyItemObject.imageid) && (copyItemObject.imageid.toString().trim() !== "")) {
                 copyItemObject.defaultImageUrl = serviceURL.toString() + "/image/BonTon/" + copyItemObject.imageid.toString().trim();
             } else {
                 copyItemObject.defaultImageUrl = "/assets/images/NotAvailable.jpg";

             }



             workingOrderLine.btDisplay = copyItemObject;

             //add next prime line no
             if (orderCart.OrderLines.OrderLine.length > 0) {
                 var lastOrderLine = orderCart.OrderLines.OrderLine.slice(-1)[0];
                 workingOrderLine._PrimeLineNo = parseInt(lastOrderLine._PrimeLineNo) + 1;
                 workingOrderLine._SubLineNo = 1;
             } else {
                 workingOrderLine._PrimeLineNo = 1;
                 workingOrderLine._SubLineNo = 1;
             }

             //default quantity to 1
             quantity = parseInt(quantity);
             quantity = (isFinite(quantity) && quantity > 0) ? quantity : 1;
             workingOrderLine._OrderedQty = quantity.toString();

             //add sku/ item id and UPC
             //TODO: check that we don't need to pad with zeros
             workingOrderLine.Item._ItemID = copyItemObject.sku.toString();
             workingOrderLine.ItemDetails._ItemID = copyItemObject.sku.toString();
             workingOrderLine.Item._UPCCode = copyItemObject.id.toString();

             workingOrderLine.ItemDetails.ItemAliasList.ItemAlias[0]._AliasValue = copyItemObject.id.toString();

             //add PrimaryInformation's _IsAirShippingAllowed, _ItemType, and _IsParcelShipingAllowed
             workingOrderLine.ItemDetails.PrimaryInformation._IsAirShippingAllowed = copyItemObject.isairshipallowed;
             workingOrderLine.ItemDetails.PrimaryInformation._ItemType = copyItemObject.itemtype;

             //TODO verify the the isgroundshipallowed SOLR attribute IS the value for _IsParcelShippingAllowed
             workingOrderLine.ItemDetails.PrimaryInformation._IsParcelShippingAllowed = "Y";

             //add computed prices
             workingOrderLine.Extn = angular.copy(copyItemObject.itemDetail.ComputedPrice.Extn);
             workingOrderLine.LinePriceInfo._ListPrice = copyItemObject.itemDetail.ComputedPrice._ListPrice;
             workingOrderLine.LinePriceInfo._RetailPrice = copyItemObject.itemDetail.ComputedPrice._RetailPrice;
             workingOrderLine.LinePriceInfo._UnitPrice = copyItemObject.itemDetail.ComputedPrice._UnitPrice;


             //set default shipTo address if customer is set
             if (_currentCustomerKey.trim().length > 0) {
                 _copyAddress(orderCart.PersonInfoShipTo, workingOrderLine.PersonInfoShipTo);
                 workingOrderLine.btLogic.shippingAddress = angular.copy(orderCart.btLogic.defaultShipToAddress);
             }


             return workingOrderLine;
         };

         /**
          * 
          * @param {type} itemObject
          * @param {type} quantity
          * @param {Boolean} isBigTicketValidated If set to true, Big Ticket items can be added (if allowed by propertyService).
          * @returns {deferedError Object} Returns error object to be throw by calling function IF quantity is larger than available inventory {newQuantity: quantity, errorMessage: deferedError}
          */
         var _validateItemObject = function (itemObject, quantity, isBigTicketValidated) {

             var itemName = "";

             if (angular.isString(itemObject.productname) && (itemObject.productname.trim() !== "")) {
                 itemName = itemObject.productname;
             } else {
                 if (angular.isString(itemObject.isnlongdesc)) {
                     itemName = itemObject.isnlongdesc;
                 }
             }


             //validate that item isBuyable and isActive and follows rules for F and P
             if (!$itemProperty.isItemActiveBuyable(itemObject)) {
                 throw errorObj.newError('addOrderLine():ItemInactiveOrNotBuyable', "The item" + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id +
                             ' is no longer available for purchase.',
                             "addOrderLine():The item" + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id +
                             ' is no longer available for purchase.', 'InvalidItem', 'WARN');

             };

             //TODO: log error if itemDetail is not defined, this means itemDetail sky calls failed
             if (!angular.isDefined(itemObject.itemDetail)) {
                 throw errorObj.newError('addOrderLine:SkyError', 'Code Error, Call Helpdesk.',
                 'addOrderLine(): addOrderLine() was passed an item with no itemDetail properties. These are the properties retrieved from SKY.', 'skyError', 'FATAL');
             }

             //TODO: log error if _AvailableQty is not defined
             if (!angular.isDefined(itemObject._AvailableQty)) {
                 throw errorObj.newError('addOrderLine:SkyError', 'Code Error, Call Helpdesk.',
                 'addOrderLine(): addOrderLine() was passed an item with no _AvailableQty defined.', 'skyError', 'FATAL');
             } else if (!isFinite(parseInt(itemObject._AvailableQty))) {
                 throw errorObj.newError('addOrderLine:SkyError', 'Code Error, Call Helpdesk.',
                 'addOrderLine(): addOrderLine() was passed an item with invalid _AvailableQty defined. Qty = ' + parseInt(itemObject._AvailableQty).toString(), 'skyError', 'FATAL');
             }

             //validate item type can is NOT in excluded types
             var excludedTypesArray = btProp.getProp("excludedItemTypesForLUFIOrder");
             var isBigTicketPurchaseDisabled = btProp.getProp("isBigTicketPurchaseDisabled");

             isBigTicketPurchaseDisabled = isBigTicketPurchaseDisabled ? true : false;

             var bigTicketException = function (itemObject, isBigTicketPurchaseDisabled, isBigTicketValidated) {
                 if (itemObject.itemtype == 'BGT' && (isBigTicketPurchaseDisabled || !angular.isDefined(isBigTicketValidated) || !isBigTicketValidated)) {
                     throw errorObj.newError('addOrderLine:InvalidItem', 'The item' + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id + ' cannot be ordered on LUFI.',
                     'addOrderLine(): The item' + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id + ' is part of the Excluded Item Types. Cannot add type: ' + excludedTypesArray[i], 'InvalidItem:' + excludedTypesArray[i], 'WARN');
                 }
             };

             if (excludedTypesArray !== null) {
                 for (var i = 0; i < excludedTypesArray.length; i++) {
                     if (itemObject.itemtype == excludedTypesArray[i]) {
                         //Do not throw error is call to add item says Zip code was validated
                         if (itemObject.itemtype == 'BGT') {
                             bigTicketException(itemObject, isBigTicketPurchaseDisabled, isBigTicketValidated);
                         } else {
                             throw errorObj.newError('addOrderLine:InvalidItem', 'The item' + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id + ' cannot be ordered on LUFI.',
                             'addOrderLine(): The item' + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id + ' is part of the Excluded Item Types. Cannot add type: ' + excludedTypesArray[i], 'InvalidItem:' + excludedTypesArray[i], 'WARN');
                         }
                     }
                 }
             } else {

                 $loggerService.log("var excludedTypesArray = btProp.getProp(excludedItemTypesForLUFIOrder); RETURNED NULL!!");
                 //check BGT if it is disabled
                 if (itemObject.itemtype == 'BGT') {
                     bigTicketException(itemObject, isBigTicketPurchaseDisabled, isBigTicketValidated);
                 }
             }

             //validate - IF isExcludeGwpFromLufiOrder is true, then do not allow GWP to be added to order
             var isExcludeGwpFromLufiOrder = btProp.getProp("isExcludeGwpFromLufiOrder");

             if (isExcludeGwpFromLufiOrder !== null) {
                 //if GWP are generally excluded from Lufi
                 if (isExcludeGwpFromLufiOrder) {

                     //if CSR's are allowed to add GWP and this User is a CSR, then skip the check for this exception
                     if (!(btProp.getProp("isAllowCsrToAddExcludedGwpFromLufiOrder") && $securityService.isCsr())) {

                         if (itemObject.isgwp == 'Y') {
                             throw errorObj.newError('addOrderLine:InvalidItem', 'The item' + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id + ' cannot be ordered on LUFI.',
                             'addOrderLine(): The item' + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id + ' is a GWP (gift with purchase). You may not add gift to LUFI order unless property: "isExcludeGwpFromLufiOrder" is set to false.', 'InvalidItem:GWP', 'WARN');
                         }
                     }

                 }
             } else {
                 $loggerService.log("var excludedTypesArray = btProp.getProp(isExcludeGwpFromLufiOrder); RETURNED NULL!!");
             }

             //validate that this sku's quantity added is less than or equal to largest available inventory reported on order. 
             var temp = _getLargestAvailableInventoryAndOrderedQtyForSkuInCart(itemObject.sku);
             var largestAvailableInventory = temp.largestAvailableInventory;
             var orderCartQty = temp.orderedQty;

             //if passed itemObject's available quantity is larger then set as largest
             if (parseInt(itemObject._AvailableQty) > largestAvailableInventory) {
                 largestAvailableInventory = parseInt(itemObject._AvailableQty);
             }

             var oldValue = '';
             var deferedError = null;


             //if newQuantity create order larger than available inventory, set newQuantity to max possible
             if ((quantity + orderCartQty) > largestAvailableInventory) {

                 oldValue = quantity.toString();

                 quantity = largestAvailableInventory - orderCartQty;

                 if (quantity < 0) {
                     quantity = 0;
                 }
             }

             //if quantity value had to be changed because of available inventory...
             if (oldValue) {
                 var currentCartQtyMessage = '';
                 if (orderCartQty > 0) {
                     currentCartQtyMessage = "Cart already contains " + orderCartQty.toString() + " of this item.";
                 }

                 //if the new quantity placed in quantity variable is zero or less than just throw error and do not add line
                 if (quantity < 1) {

                     throw errorObj.newError('addOrderLine():AvailableQtyError', "The item" + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id +
                             ' has available inventory of up to ' + largestAvailableInventory.toString() + ". More of this item cannot be added. " + currentCartQtyMessage,
                             "addOrderLine():The item" + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id + ' has available inventory of up to ' + largestAvailableInventory.toString() +
                             ". More of this item cannot be added. " + currentCartQtyMessage, 'ZeroInventory', 'WARN');

                     //else save an Error to be sent later once line is added
                 } else {

                     deferedError = errorObj.newError('addOrderLine():AvailableQtyError',
                             "The item" + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id +
                             ' has available inventory of up to ' + largestAvailableInventory.toString() + ". We have added only " + quantity.toString() +
                             " of this item. " + currentCartQtyMessage,
                             "addOrderLine():The item" + (itemName ? ' "' + itemName + '"' : '') + " with UPC: " + itemObject.id +
                             ' has available inventory of up to ' + largestAvailableInventory.toString() + ". We have added only " + quantity.toString() +
                             " of this item. " + currentCartQtyMessage,
                             'ReducedQuantity',
                             'WARN');

                     return { newQuantity: quantity, errorMessage: deferedError };
                 }
             }
         };

         var _addItem = function (itemObject, quantity, isBigTicketValidated) {

             if (!angular.isObject(itemObject)) {
                 throw errorObj.newError('addOrderLine:CallerError', 'Code Error, Call Helpdesk.', 'addOrderLine(): Item passed to addOrderLine() is not an object.', 'CallerError', 'FATAL');
             }
             //default quantity to 1
             quantity = parseInt(quantity);
             quantity = (isFinite(quantity) && quantity > 0) ? quantity : 1;

             //validate function may return {newQuantity: quantity, errorMessage: deferedError}
             var newQuantityError = _validateItemObject(itemObject, quantity, isBigTicketValidated);

             if (angular.isDefined(newQuantityError) && angular.isDefined(newQuantityError.newQuantity)) {
                 quantity = newQuantityError.newQuantity;
             }

             angular.forEach(orderCart.OrderLines.OrderLine, function (orderLine) {
                 orderLine = _deleteShippingDiscount(orderLine);
             });

             orderCart.OrderLines.OrderLine.push(_constructOrderLine(itemObject, quantity));

             if (angular.isDefined(newQuantityError) && angular.isDefined(newQuantityError.errorMessage)) {
                 throw newQuantityError.errorMessage;
             }
         };

         var _removeQtyZeroOrderLines = function () {
             var orderlines = angular.copy(orderCart.OrderLines.OrderLine);
             for (var i = 0; i < orderlines.length; i++) {
                 var lineQty = parseInt(orderlines[i]._OrderedQty);
                 if (lineQty < 1) {
                     _deleteOrderLine(_getPrimeSubLineObject(orderlines[i]), false);
                 }
             }
         };

         var _renumberOrderLines = function () {
             _sortOrderCartOrderlines();

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


         var _cleanOrderCart = function () {

             _removeQtyZeroOrderLines();
             _renumberOrderLines();

         };

         var _getOrderNumber = function ()
         {
             var defer = $q.defer();
             var regExOrderNo = /^\d{7,20}$/;

             //If OrderNo already set, return
             if (angular.isDefined(orderCart._OrderNo) && orderCart._OrderNo !== null && regExOrderNo.test(orderCart._OrderNo)) {
                 defer.resolve(orderCart._OrderNo);
             } else {

                 //call to Broker to get Order Number (it is in JSON Number form, not String form)
                 $http.get(serviceURL.toString() + '/OrderProcess/getNextOrderNo').then(function (response) {

                     if(response !== null && response.data !== null && angular.isDefined(response.data.NextOrderNo) &&  response.data.NextOrderNo !== null && regExOrderNo.test("" + response.data.NextOrderNo))
                     {
                         //set Order No
                         orderCart._OrderNo = "" + response.data.NextOrderNo;

                         defer.resolve(orderCart._OrderNo);
                     }
                     else {

                         $sendSMTPErrorEmail(response, serviceURL.toString() + '/OrderProcess/getNextOrderNo', null, "Invalid Order Number returned from Broker's get Order Number.");

                         defer.reject();
                     }
                    
                 }, function (error) {

                     $sendSMTPErrorEmail(error, serviceURL.toString() + '/OrderProcess/getNextOrderNo', null, "Call to Broker failed.");

                     defer.reject(error);
                 });  
             }

             return defer.promise;
         };

         var _getLiveOrderCart = function () {
             return orderCart;
         };

         var _orderlineCount = function () { return orderCart.OrderLines.OrderLine.length; };

         var _setOrderLineCarrierService = function (orderline, CarrierServiceObj) {
             if (!angular.isDefined(CarrierServiceObj)) { CarrierServiceObj = {}; }
             CarrierServiceObj._CarrierServiceCode = CarrierServiceObj._CarrierServiceCode ? CarrierServiceObj._CarrierServiceCode : "";
             CarrierServiceObj._Price = CarrierServiceObj._Price ? CarrierServiceObj._Price : "";
             CarrierServiceObj._CarrierServiceDesc = CarrierServiceObj._CarrierServiceDesc ? CarrierServiceObj._CarrierServiceDesc : "Cannot Ship to Address";

             orderline._CarrierServiceCode = CarrierServiceObj._CarrierServiceCode;
             orderline._ScacAndService = CarrierServiceObj._CarrierServiceCode;

             //spilt carrier Service code like "UPS-GRND" 
             var carrierAndLevelArray = CarrierServiceObj._CarrierServiceCode.toString().split("-");

             if (carrierAndLevelArray[0].toLowerCase() === 'email') {
                 carrierAndLevelArray[0] = 'ELECTRONIC';
             }

             orderline._SCAC = carrierAndLevelArray.shift();

             orderline._LevelOfService = carrierAndLevelArray.join("-");

             //add price to btDisplay
             orderline.btDisplay.shippingMethodPrice = CarrierServiceObj._Price;
             orderline.btDisplay.shippingMethodDescription = CarrierServiceObj._CarrierServiceDesc;

             ////get shipping charge changes
             //var inputCart = angular.copy(orderCart);

             //return _repriceOrder().then(function (response) {
             //    console.log('setOrderLineCarrierService:Start = ' + (new Date()).toISOString());
             //    console.log(response);
             //    _arraySplicePush(response.data.OrderLines.OrderLine, orderCart.OrderLines.OrderLine);
             //    console.log(orderCart);
             //    console.log('setOrderLineCarrierService:End = ' + (new Date()).toISOString());
             //});
         };

         //Orderline should already have CarrierServiceCodes populated, this finds the lowest priced shipping and sets that as default to orderline.
         var _setOrderLineCarrierServiceToLowestPrice = function (orderline) {
             if (angular.isDefined(orderline) && angular.isDefined(orderline.CarrierServiceList)
                     && angular.isArray(orderline.CarrierServiceList.CarrierService) && angular.isDefined(orderline.CarrierServiceList.CarrierService[0])) {
                 var lowestCarrierService = orderline.CarrierServiceList.CarrierService[0];
                 var lowestPrice = parseFloat(orderline.CarrierServiceList.CarrierService[0]._Price);

                 //TODO send error if CarrierServiceList has bad Price
                 if (!isFinite(lowestPrice)) {
                     $loggerService.log("Order in orderCart has invalid CarrierServiceList prices");
                     lowestPrice = 10000000;
                 }

                 for (var i = 1; i < orderline.CarrierServiceList.CarrierService.length; i++) {
                     var tempPrice = parseFloat(orderline.CarrierServiceList.CarrierService[i]._Price);
                     //TODO send error if CarrierServiceList has bad Price
                     if (!isFinite(tempPrice)) {
                         $loggerService.log("Order in orderCart has invalid CarrierServiceList prices");
                     } else {
                         if (tempPrice < lowestPrice) {
                             lowestCarrierService = orderline.CarrierServiceList.CarrierService[i];
                             lowestPrice = tempPrice;
                         }
                     }
                 }

                 _setOrderLineCarrierService(orderline, lowestCarrierService);
             }
         };
         //Orderline should already have CarrierServiceCodes populated, this function checks that the orderline's selected carrierService
         //   is still available in the list of CarrierServeCodes. If so, it resets the price and description, if not it resets to default lowest price shipping
         var _validateOrderLineCarrierService = function (orderline) {

             if (!angular.isDefined(orderline.CarrierServiceList.CarrierService) ||
                  orderline.CarrierServiceList.CarrierService.length < 1) {
                 _setOrderLineCarrierService(orderline);
             } else if (orderline._CarrierServiceCode.trim() === "") {
                 _setOrderLineCarrierServiceToLowestPrice(orderline);

             } else {
                 var selectedCarrierCode = orderline._CarrierServiceCode.toString();
                 var matchingCarrierCodeObj = null;

                 for (var i = 0; i < orderline.CarrierServiceList.CarrierService.length; i++) {
                     if (orderline.CarrierServiceList.CarrierService[i]._CarrierServiceCode.toString() === selectedCarrierCode) {
                         matchingCarrierCodeObj = orderline.CarrierServiceList.CarrierService[i];
                         break;
                     }
                 }

                 if (matchingCarrierCodeObj !== null) {
                     _setOrderLineCarrierService(orderline, matchingCarrierCodeObj);
                 } else {
                     _setOrderLineCarrierServiceToLowestPrice(orderline);
                 }
             }
         };

         var concatRolling = function (rollingText, concatText) {
             if (rollingText === undefined || rollingText === null || rollingText === "") {
                 rollingText = concatText;
             }
             else { rollingText += " and " + concatText; }
             return rollingText;
         }

         var _updateCarrierServiceList = function () {

             var _isValidShipTo = function (currentLine, errorMessage) {

                 var cntRolling = 0;
                 var valid = true;
                 var shipTo = currentLine.PersonInfoShipTo;
                 var primelineText = ' Line: ' + currentLine._PrimeLineNo + "." + currentLine._SubLineNo;
                 var rollingErrorText = '';

                 if (angular.isDefined(shipTo._DayPhone) && shipTo._DayPhone.toString().trim().length < 10) {
                     valid = false;
                     errorMessage.message = "Customer profile contains invalid phone number." + "\n" + "Update profile with the correct phone number."
                     return valid;
                 }

                 if (!angular.isDefined(shipTo._FirstName) || shipTo._FirstName.toString().trim() === '') {
                     valid = false;
                     rollingErrorText += 'First Name ';
                     cntRolling = cntRolling + 1;
                 }
                 if (!angular.isDefined(shipTo._LastName) || shipTo._LastName.toString().trim() === '') {
                     valid = false;
                     rollingErrorText = concatRolling(rollingErrorText, 'Last Name');
                     cntRolling = cntRolling + 1;
                 }
                 if (!angular.isDefined(shipTo._EMailID) || shipTo._EMailID.toString().trim() === '') {
                     valid = false;
                     rollingErrorText = concatRolling(rollingErrorText, 'Email');
                     cntRolling = cntRolling + 1;
                 }
                 if (!angular.isDefined(shipTo._DayPhone) || shipTo._DayPhone.toString().trim() === '') {
                     valid = false;
                     rollingErrorText = concatRolling(rollingErrorText, 'Primary Phone');
                     cntRolling = cntRolling + 1;
                 }
                 if (!angular.isDefined(shipTo._AddressLine1) || shipTo._AddressLine1.toString().trim() === '') {
                     valid = false;
                     rollingErrorText = concatRolling(rollingErrorText, 'Address Line 1');
                     cntRolling = cntRolling + 1;
                 }
                 if (!angular.isDefined(shipTo._City) || shipTo._City.toString().trim() === '') {
                     valid = false;
                     rollingErrorText = concatRolling(rollingErrorText, 'City');
                     cntRolling = cntRolling + 1;
                 }
                 if (!angular.isDefined(shipTo._State) || shipTo._State.toString().trim() === '') {
                     valid = false;
                     rollingErrorText = concatRolling(rollingErrorText, 'State');
                     cntRolling = cntRolling + 1;
                 }
                 if (!angular.isDefined(shipTo._ZipCode) || shipTo._ZipCode.toString().trim() === '') {
                     valid = false;
                     rollingErrorText = concatRolling(rollingErrorText, 'Zip Code');
                     cntRolling = cntRolling + 1;
                 }
                 if (!valid) {
                     errorMessage.message += ' is missing ' + rollingErrorText;
                     if (cntRolling === 1) {
                         errorMessage.message += ". Update profile with a valid " + rollingErrorText + ".";
                     }
                     else if (cntRolling > 1) {
                         errorMessage.message += ". Update profile with the required information.";
                     }
                 }
                 return valid;
             };

             //create copy of only lines with shipTo addresses
             var copyOfObj = { OrderLine: [] };

             var isAllLineValid = true;
             var errorMessage = { message: 'Customer profile ' };

             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
                 var currentLine = orderCart.OrderLines.OrderLine[i];

                 //clean old CarrierServiceList
                 currentLine.CarrierServiceList = { CarrierService: [] };
                 currentLine.btDisplay.shippingMethodPrice = '';
                 currentLine.btDisplay.shippingMethodDescription = '';
                 //_validateOrderLineCarrierService(currentLine);

                 var isCurrentLineValid = _isValidShipTo(currentLine, errorMessage);

                 isAllLineValid = isAllLineValid && isCurrentLineValid;

                 if (isCurrentLineValid) {
                     var linecopy = angular.copy(currentLine);

                     //TODO: Remove work-a-round. need to delete lineTax for now
                     // delete linecopy.LineTaxes.LineTax;

                     /*********************************************************
                      * ******************************************************
                     *********   TODO: START: work-around for gift card errors 
                     *********************************************************/

                     if (!angular.isArray(linecopy.LineCharges.LineCharge)) {
                         linecopy.LineCharges.LineCharge = [{
                             "_ChargeAmount": "0.00",
                             "_ChargeCategory": "BTN_SHIP_CHRG",
                             "_ChargeName": "CHRG_SHIPPING",
                             "_ChargePerLine": "0.00",
                             "_ChargePerUnit": "0.00",
                             "_IsDiscount": "N",
                             "_IsManual": "N",
                             "_Reference": "UPS-GRND"
                         }]
                     } else {
                         var hasShipChargeObj = false;
                         for (var x = 0; x < linecopy.LineCharges.LineCharge.length; x++) {
                             if (angular.isString(linecopy.LineCharges.LineCharge[x]._ChargeName) &&
                                 (/^CHRG_SHIPPING$/).test(linecopy.LineCharges.LineCharge[x]._ChargeName)) {
                                 hasShipChargeObj = true;
                                 break;
                             }
                         }

                         if (!hasShipChargeObj) {
                             linecopy.LineCharges.LineCharge.push({
                                 "_ChargeAmount": "0.00",
                                 "_ChargeCategory": "BTN_SHIP_CHRG",
                                 "_ChargeName": "CHRG_SHIPPING",
                                 "_ChargePerLine": "0.00",
                                 "_ChargePerUnit": "0.00",
                                 "_IsDiscount": "N",
                                 "_IsManual": "N",
                                 "_Reference": "UPS-GRND"
                             });
                         }
                     }
                     /*********************************************************
                      * ******************************************************
                     *********   TODO: END: work-around for gift card errors 
                     *********************************************************/

                     copyOfObj.OrderLine.push(linecopy);
                 }
             }

             if (!isAllLineValid) {
                 //we are now ignoring the separate error messages from each orderline and just doing a generic message for any error.

                 return $q.reject(errorObj.newError('CarrierServiceCodes Error', 'Customer profile has missing or invalid information. Please update profile.',
                         '_updateCarrierServiceList found invalid Ship To address on orderlines: ' + errorMessage.message, 'CarrierService', 'FATAL'));
             }

             if (copyOfObj.OrderLine.length === 0) {
                 return $q.when('0');
             }
             var input = { GetCarrierServiceCodeIn: {} };
             input.GetCarrierServiceCodeIn._SellerOrganizationCode = orderCart._SellerOrganizationCode;
             input.GetCarrierServiceCodeIn._EnteredBy = orderCart._EnteredBy ? orderCart._EnteredBy : 'Store';
             input.GetCarrierServiceCodeIn._EnterpriseCode = orderCart._EnterpriseCode;
             input.GetCarrierServiceCodeIn._EntryType = orderCart._EntryType;

             input.GetCarrierServiceCodeIn.OrderLines = copyOfObj;



             //START make sure _UnitPrice in on each orderline

             //check for valid unitprice in orderline
             var validPrice = function (price) {

                 if (!(angular.isString(price) || angular.isNumber(price))) {
                     return false;
                 }

                 var numericValue;

                 if (angular.isString(price)) {
                     if (price.trim().length > 0) {
                         numericValue = Number(price);
                     } else {
                         return false;
                     }
                 } else {

                     numericValue = price;
                 }

                 if (isFinite(numericValue) && (numericValue >= 0)) {
                     return true;
                 }

                 return false;

             };

             //fix invalid unitprice
             var resetLineUnitPrice = function (line) {
                 if (line.LinePriceInfo._RetailPrice && validPrice(line.LinePriceInfo._RetailPrice)) {
                     line.LinePriceInfo._UnitPrice = angular.copy(line.LinePriceInfo._RetailPrice);

                 } else if (line.LinePriceInfo._ListPrice && validPrice(line.LinePriceInfo._ListPrice)) {
                     line.LinePriceInfo._UnitPrice = angular.copy(line.LinePriceInfo._ListPrice);
                 } else {
                     line.LinePriceInfo._UnitPrice = "0.00";
                 }
             };

             //check for missing LinePriceInfo._UnitPrice on each orderline
             if (input.GetCarrierServiceCodeIn.OrderLines && input.GetCarrierServiceCodeIn.OrderLines.OrderLine) {
                 for (var i = 0; i < input.GetCarrierServiceCodeIn.OrderLines.OrderLine.length; i++) {
                     var line = input.GetCarrierServiceCodeIn.OrderLines.OrderLine[i];

                     if (!validPrice(line.LinePriceInfo._UnitPrice)) {
                         resetLineUnitPrice(line);
                     }
                 }
             }
             //END make sure _UnitPrice in on each orderline



             //assign orderline dummy Keys of the form "temp1_1" (for orderline primeNo = 1 sublineNo = 1) only IF no orderline key exists
             for (var i = 0; i < input.GetCarrierServiceCodeIn.OrderLines.OrderLine.length; i++) {
                 var currentLine = input.GetCarrierServiceCodeIn.OrderLines.OrderLine[i];

                 if (angular.isDefined(currentLine._OrderLineKey)) {
                     //if orderline key is blank OR the orderline key starts with "temp" then overwrite with temp key
                     if (currentLine._OrderLineKey.toString().trim() === "" || (/^temp/).test(currentLine._OrderLineKey.toString().trim())) {
                         currentLine._OrderLineKey = "temp" + currentLine._PrimeLineNo.toString() + "_" + currentLine._SubLineNo.toString();
                     }
                     //else leave orderline key alone

                 } else {
                     currentLine._OrderLineKey = "temp" + currentLine._PrimeLineNo.toString() + "_" + currentLine._SubLineNo.toString();
                 }
             }

             return $http.post(serviceURL.toString() + '/Price/GetCarrierServiceCodes', input).then(function (response) {

                 serviceArrayFix(response);

                 var data = response.data;
                 //TODO: error handling if data is Error code Object
                 var errorsArray = data.GetCarrierServiceCodeOut.Errors.ErrorList.Error;

                 if (angular.isDefined(errorsArray)) {
                     var ErrorText = 'CarrierServiceCodes Error. ';

                     for (var i = 0; i < errorsArray.length; i++) {
                         ErrorText += ' ' + (i + 1).toString() + ') ' + "Error Code: " + errorsArray[i]._ErrorCode.toString() + ": " + errorsArray[i]._ErrorDescription.toString();
                         $loggerService.log("CarrierServiceCodes Error: " + errorsArray[i]._ErrorCode.toString() + ": " + errorsArray[i]._ErrorDescription.toString());
                     }

                     return $q.reject(errorObj.newError('CarrierServiceCodes Error', ErrorText,
                         'CarrierService Broker call returned errors: ' + ErrorText, 'CarrierService', 'FATAL'));

                 }

                 //add service info to orderline 
                 var orderlinearray = data.GetCarrierServiceCodeOut.OrderLines.OrderLine;

                 for (var i = 0; i < orderlinearray.length; i++) {
                     var orderlinekey = orderlinearray[i]._OrderLineKey.toString();

                     //because service does not return prime line no & subline no get from inputCart
                     for (var q = 0; q < input.GetCarrierServiceCodeIn.OrderLines.OrderLine.length; q++) {
                         if (input.GetCarrierServiceCodeIn.OrderLines.OrderLine[q]._OrderLineKey == orderlinekey) {
                             orderlinearray[i]._PrimeLineNo = input.GetCarrierServiceCodeIn.OrderLines.OrderLine[q]._PrimeLineNo;
                             orderlinearray[i]._SubLineNo = input.GetCarrierServiceCodeIn.OrderLines.OrderLine[q]._SubLineNo;
                         }
                     }

                     //find matching orderline in orderCart
                     var primeNoObject = _getPrimeSubLineObject(orderlinearray[i]);
                     var orderlineArr = _findPrimeSubInCart(primeNoObject, orderCart);

                     for (var q = 0; q < orderlineArr.length; q++) {
                         orderlineArr[q].CarrierServiceList = angular.copy(orderlinearray[i].CarrierServiceList);

                         // set default method to lowest cost IF service isn't set on line
                         _validateOrderLineCarrierService(orderlineArr[q]);
                     }
                 }

             }, function (response) {
                 $sendSMTPErrorEmail(response, serviceURL.toString() + '/Price/GetCarrierServiceCodes', input);
             });


         };

         //uses passed orderline not necessarily a cart orderline
         var _getLowestOrderLinePrice = function (orderline) {
             var lowestprice;

             if (isFinite(parseFloat(orderline.LinePriceInfo._btAdjustedPrice))) {
                 lowestprice = parseFloat(orderline.LinePriceInfo._btAdjustedPrice);
                 orderline.btLogic.lowestprice = lowestprice;
             }
             else if (isFinite(parseFloat(orderline.btLogic.lowestprice))) {
                 lowestprice = parseFloat(orderline.btLogic.lowestprice);
             } else {
                 var unitprice = (isFinite(parseFloat(orderline.LinePriceInfo._UnitPrice))) ? parseFloat(orderline.LinePriceInfo._UnitPrice) : null;
                 var retailprice = (isFinite(parseFloat(orderline.LinePriceInfo._RetailPrice))) ? parseFloat(orderline.LinePriceInfo._RetailPrice) : null;
                 //TODO what about price override


                 if (unitprice) {
                     lowestprice = unitprice;
                 }
                 if (retailprice) {
                     if (lowestprice) {
                         if (lowestprice > retailprice) {
                             lowestprice = retailprice;
                         }
                     } else {
                         lowestprice = retailprice;
                     }
                 }

                 orderline.btLogic.lowestprice = lowestprice;
             }

             return lowestprice;
         };

         var _computeShippingOrderLineSubtotal = function (orderline) {
             var subtotal = 0;
             if (angular.isDefined(orderline.LinePriceInfo) && angular.isDefined(orderline.LinePriceInfo._btAdjustedPrice)) {
                 subtotal = parseFloat(orderline.LinePriceInfo._btAdjustedPrice) * parseInt(orderline._OrderedQty);
             }
             else {
                 _getLowestOrderLinePrice(orderline) * parseInt(orderline._OrderedQty);
             }

             if (angular.isDefined(orderline.LineCharges) && angular.isArray(orderline.LineCharges.LineCharge)) {
                 for (var i = 0; i < orderline.LineCharges.LineCharge.length; i++) {
                     var currentCharge = orderline.LineCharges.LineCharge[i];
                     if ((/^BTN_SHIP.*/i).test(currentCharge._ChargeCategory) || (/^BTN_HNDL.*/i).test(currentCharge._ChargeCategory)) {
                         if ((/^Y$/i).test(currentCharge._IsDiscount)) {
                             subtotal = subtotal - (parseFloat(currentCharge._ChargePerUnit) * parseInt(orderline._OrderedQty) + parseFloat(currentCharge._ChargePerLine));
                         } else {
                             subtotal = subtotal + (parseFloat(currentCharge._ChargePerUnit) * parseInt(orderline._OrderedQty) + parseFloat(currentCharge._ChargePerLine));
                         }
                     }
                 }
             }

             return subtotal;
         };

         var _setExampleOrder = function () {
             orderCart = {
                 "_AuthorizedClient": "Store",
                 "_BillToID": "W231905",
                 "_CustomerContactID": "W231905",
                 "_CustomerEMailID": "garrett.stibb@bonton.com",
                 "_DocumentType": "0001",
                 "_DraftOrderFlag": "N",
                 "_EnteredBy": "Store",
                 "_EnterpriseCode": "BONTON",
                 "_EntryType": "BTN",
                 "_OrderNo": "",
                 "_PaymentRuleId": "BT_DEF_PAYMENT_RULE",
                 "_SellerOrganizationCode": "101",
                 "_TaxExemptFlag": "N",
                 "_TaxExemptionCertificate": "",
                 "OrderLines": {
                     "OrderLine": [
                       {
                           "_CarrierServiceCode": "UPS-GRND",
                           "_GiftWrap": "N",
                           "_HoldFlag": "N",
                           "_OrderedQty": "5.00",
                           "_OrderLineKey": "",
                           "_OriginalOrderedQty": "0",
                           "_PrimeLineNo": 1,
                           "_LevelOfService": "GRND",
                           "_ScacAndService": "UPS-GRND",
                           "_SCAC": "UPS",
                           "_Status": "",
                           "_SubLineNo": 1,
                           "_Timezone": "",
                           "Extn": {
                               "_ExtnBadgingText": "Black Dot",
                               "_ExtnClass": "600",
                               "_ExtnDepartment": "400",
                               "_ExtnIsPriceLocked": "N",
                               "_ExtnMPTID": "",
                               "_ExtnMPTOfferDeatil": "",
                               "_ExtnMPTOfferMessage1": "",
                               "_ExtnMPTOfferMessage2": "",
                               "_ExtnPriceStatus": "P",
                               "_ExtnPromoNumber": "0",
                               "_ExtnSpecialHandlingCode": "0",
                               "_ExtnSPOID": "0",
                               "_ExtnSPOOfferDetail": "",
                               "_ExtnSPOOfferMsg1": "",
                               "_ExtnSPOOfferMsg2": ""
                           },
                           "Item": {
                               "_ItemID": "440000688653",
                               "_UPCCode": "755179389899",
                               "_ProductClass": "NEW",
                               "_UnitCost": "",
                               "_UnitOfMeasure": "EACH"
                           },
                           "ItemDetails": {
                               "_ItemID": "440000688653",
                               "ItemAliasList": {
                                   "ItemAlias": [
                                     {
                                         "_AliasName": "ACTIVE_UPC",
                                         "_AliasValue": "755179389899"
                                     }
                                   ]
                               },
                               "PrimaryInformation": {
                                   "_IsAirShippingAllowed": "Y",
                                   "_ItemType": "REG",
                                   "_IsParcelShippingAllowed": "Y"
                               }
                           },
                           "LineCharges": {
                               "LineCharge": [
                                 {
                                     "_ChargeAmount": "0.00",
                                     "_ChargeCategory": "BTN_SHIP_CHRG",
                                     "_ChargeName": "CHRG_SHIPPING",
                                     "_ChargePerLine": "0.00",
                                     "_ChargePerUnit": "0.00",
                                     "_IsDiscount": "N",
                                     "_IsManual": "N",
                                     "_Reference": "UPS-GRND"
                                 }
                               ]
                           },
                           "LineTaxes": {
                               "LineTax": [
                                 {
                                     "_ChargeName": "TAX_SALES",
                                     "_ChargeCategory": "BTN_TAX_CHRG",
                                     "_Tax": "21.44",
                                     "_TaxName": "SALES",
                                     "_TaxPercentage": "5.50",
                                     "Extn": {
                                         "_ExtnTaxPerUnit": "4.28"
                                     }
                                 }
                               ]
                           },
                           "Promotions": {},
                           "LinePriceInfo": {
                               "_ListPrice": "158.00",
                               "_RetailPrice": "77.99",
                               "_TaxableFlag": "Y",
                               "_TaxExemptionCertificate": "N",
                               "_UnitPrice": "77.99"
                           },
                           "PersonInfoShipTo": {
                               "_AddressID": "LOGON5918",
                               "_AddressLine1": "2412 56th ave",
                               "_AddressLine2": "",
                               "_AddressLine3": "",
                               "_City": "kenosha",
                               "_Country": "US",
                               "_DayPhone": "6969696969",
                               "_EMailID": "garrett.stibb@bonton.com",
                               "_EveningPhone": "",
                               "_FirstName": "Garrett",
                               "_LastName": "Stibb",
                               "_MiddleName": "",
                               "_PersonInfoKey": "20150402190721329128955",
                               "_State": "WI",
                               "_ZipCode": "53144",
                               "_PersonID": "8701684"
                           },
                           "CarrierServiceList": {
                               "CarrierService": [
                                 {
                                     "_CarrierServiceCode": "UPS-GRND",
                                     "_CarrierServiceDesc": "UPS Ground/Standard",
                                     "_Currency": "USD",
                                     "_Price": "0.00"
                                 },
                                 {
                                     "_CarrierServiceCode": "UPS-TDAY",
                                     "_CarrierServiceDesc": "UPS 2-day ship",
                                     "_Currency": "USD",
                                     "_Price": "19.95"
                                 },
                                 {
                                     "_CarrierServiceCode": "UPS-NDAY",
                                     "_CarrierServiceDesc": "UPS Next day ship",
                                     "_Currency": "USD",
                                     "_Price": "29.95"
                                 }
                               ]
                           },
                           "Notes": {
                               "Note": []
                           },
                           "_DeliveryMethod": "SHP",
                           "_GiftFlag": "N",
                           "_LevelOfService": "GRND",
                           "_ScacAndService": "",
                           "btDisplay": {
                               "id": 755179389899,
                               "isn": 400101743,
                               "sku": 440000688653,
                               "isnlongdesc": "tay charc deq c/s ruched sq nk",
                               "vendorstyle": "3150M",
                               "genclasslongdesc": "Dresses",
                               "colorlongdesc": "CHARCOAL",
                               "colorfamdesc": "Black/Gray Fam",
                               "colorattrdesc": "Charcoal Gray",
                               "itemsize": "10       R",
                               "sizesequence": 11020,
                               "desc1": "OCTOBER 2011",
                               "desc2": "LBD",
                               "desc3": "SHTH (SHEATH)",
                               "desc4": "MATTE JERSEY",
                               "brandlongdesc": "Taylor Dresses",
                               "proddtllongdesc": "Cap Sleeve",
                               "deptlongdesc": "BETTER DRESSES",
                               "cmg": "110 - DRESSES",
                               "cfg": "10 - MISSES DRESSES",
                               "fob": "101 - BETTER DRESSES",
                               "crg": "100 - READY TO WEAR",
                               "classlongdesc": "TAYLOR",
                               "gensclalongdesc": "1pc Dress",
                               "fablongdesc": "Poly Blend-With Stretch",
                               "fabdtldesc": "Embellished",
                               "proddetail2": "Above the Knee Length",
                               "proddetail3": "Knit",
                               "labellongdesc": "Taylor Dresses",
                               "productcode": 157134,
                               "productname": "Taylor Charcoal Sequin Knit Cap-Sleeve Squareneck Party Dress",
                               "imageid": 431692,
                               "keywords": ",,1,10,8,6,4,2,Taylor,Charcoal,Sequin Knit,Cap Sleeve,Squareneck Party Dress,party dress,cap sleeve dress,squareneck dress,knit dresses,cocktail dresses,women,apparel,",
                               "webcatprodshortdesc": "Mingle in high style in this gorgeous, sequin-knit cocktail dress, characterized by its ruched, pleated texture and revealing cap sleeves.",
                               "webcatprodlongdesc": "<ul>\n    <li>Squareneck party dress with cap sleeves</li>\n    <li>Ruched pleats over sequin knit fabric</li>\n    <li>Back-zip closure</li>\n    <li>Acrylic/nylon</li>\n    <li>Imported</li>\n</ul>",
                               "colorcode": 10,
                               "colorDc": "00010 - CHARCOAL",
                               "size1code": "10  ",
                               "size2code": "R   ",
                               "sizedc": "10    R   ",
                               "itemtype": "REG",
                               "pricestatus": "P",
                               "hazardCode": "1",
                               "giftwrapCode": "2",
                               "specialhandlingcode": "1",
                               "isactive": "Y",
                               "isgwp": "N",
                               "iswebexclusive": "Y",
                               "isspecialhandling": "N",
                               "isairshipallowed": "Y",
                               "isgroundshipallowed": "Y",
                               "specialhandlingfee": 0,
                               "groupid": "400101743157134",
                               "buyable": true,
                               "_version_": 1503052571298758700,
                               "itemDetail": {
                                   "_ItemID": "440000688653",
                                   "_ItemKey": "440000688653",
                                   "_OrganizationCode": "BONTON",
                                   "_UPCCode": "755179389899",
                                   "ComputedPrice": {
                                       "_ListPrice": "158",
                                       "_RetailPrice": "77.99",
                                       "_UnitPrice": "77.99",
                                       "Extn": {
                                           "_btIsBonusEvent": "false",
                                           "_btIsDoorBusterEvent": "false",
                                           "_btIsHazMat": "false",
                                           "_btIsIVPEvent": "false",
                                           "_btIsNightOwlEvent": "false",
                                           "_btIsOtherSpecialEvent": "false",
                                           "_btIsSpecialHandling": "false",
                                           "_btIsWebExclusive": "false",
                                           "_btIsWebOnlyEvent": "false",
                                           "_btSpecialHandlingFee": "0",
                                           "_ExtnBadgingText": "Black Dot",
                                           "_ExtnClass": "600",
                                           "_ExtnDepartment": "400",
                                           "_ExtnMPTID": "",
                                           "_ExtnMPTOfferDetail": "",
                                           "_ExtnMPTOfferMsg1": "",
                                           "_ExtnMPTOfferMsg2": "",
                                           "_ExtnPriceStatus": "P",
                                           "_ExtnPromoNumber": "0",
                                           "_ExtnSpecialHandlingCode": "0",
                                           "_ExtnSPOID": "0",
                                           "_ExtnSPOOfferDetail": "",
                                           "_ExtnSPOOfferMsg1": "",
                                           "_ExtnSPOOfferMsg2": ""
                                       }
                                   }
                               },
                               "_AvailableQty": "10",
                               "defaultItemDescription": "Taylor Charcoal Sequin Knit Cap-Sleeve Squareneck Party Dress",
                               "defaultImageUrl": "http://broker.qa.bonton.com:7080/image/BonTon/431692",
                               "shippingMethodPrice": "0.00",
                               "shippingMethodDescription": "UPS Ground/Standard"
                           },
                           "btLogic": {
                               "isPriceOverridden": "N",
                               "priceOverrideValue": 0,
                               "isBigTicket": "N",
                               "giftImgUrl": "assets/images/GreyGiftMessage.gif",
                               "isGiftRegistryAddress": false,
                               "giftBoxImgUrl": "assets/images/greyGiftBox.gif",
                               "orderSummaryPageTempQty": 5,
                               "isSelected": false
                           }
                       }
                     ]
                 },
                 "PersonInfoBillTo": {
                     "_AddressID": "LOGON5918",
                     "_AddressLine1": "2412 56th ave",
                     "_AddressLine2": "",
                     "_AddressLine3": "",
                     "_City": "kenosha",
                     "_Country": "US",
                     "_DayPhone": "6969696969",
                     "_EveningPhone": "",
                     "_EMailID": "garrett.stibb@bonton.com",
                     "_FirstName": "Garrett",
                     "_MiddleName": "",
                     "_LastName": "Stibb",
                     "_PersonID": "8701684",
                     "_PersonInfoKey": "20150402190721329128955",
                     "_State": "WI",
                     "_ZipCode": "53144"
                 },
                 "PersonInfoShipTo": {
                     "_AddressID": "LOGON5918",
                     "_AddressLine1": "2412 56th ave",
                     "_AddressLine2": "",
                     "_AddressLine3": "",
                     "_City": "kenosha",
                     "_Country": "US",
                     "_DayPhone": "6969696969",
                     "_EveningPhone": "",
                     "_EMailID": "garrett.stibb@bonton.com",
                     "_FirstName": "Garrett",
                     "_MiddleName": "",
                     "_LastName": "Stibb",
                     "_PersonID": "8701684",
                     "_PersonInfoKey": "20150402190721329128955",
                     "_State": "WI",
                     "_ZipCode": "53144"
                 },
                 "PaymentMethods": {
                     "PaymentMethod": []
                 },
                 "Extn": {
                     "_ExtnAssociateId": "898989"
                 },
                 "PriceInfo": {
                     "_Currency": "USD"
                 },
                 "_OrderDate": "2015-06-10T21:36:18.037",
                 "_OrderHeaderKey": "",
                 "_OrderType": "",
                 "Errors": {
                     "ErrorList": {
                         "Error": [
                           {
                               "_ErrorCode": "00",
                               "_ErrorDescription": "Pricing: Successful",
                               "_ErrorRelatedMoreInfo": "",
                               "Attribute": {
                                   "_Name": "",
                                   "_Value": ""
                               },
                               "Stack": []
                           }
                         ]
                     }
                 },
                 "Promotions": {}
             };
         };
         var _arraySplicePush = function (source, destination) {
             if (angular.isArray(source) && angular.isArray(destination)) {
                 destination.splice(0, destination.length);
                 for (var i = 0; i < source.length; i++) {
                     destination.push(source[i]);
                 }
             }
         };

         var _mergeCart = function (mergedCart) {
             //repriceService.copyAllAttributes(source, dest);
             //repriceService.copyMissingAttributes(source, dest);
             repriceService.copyAllAttributes(mergedCart, orderCart);

             _arraySplicePush(mergedCart.OrderLines.OrderLine, orderCart.OrderLines.OrderLine);

             //Add Errors And Promotion lists
             if (!angular.isDefined(orderCart.Errors)) {
                 orderCart.Errors = { ErrorList: { Error: [] } };
             }
             _arraySplicePush(mergedCart.OrderLines.OrderLine, orderCart.OrderLines.OrderLine);

         };

         var _getItemCount = function () {
             var quantity = 0;

             for (var i = 0; i < orderCart.OrderLines.OrderLine.length; i++) {
                 var lineQty = parseInt(orderCart.OrderLines.OrderLine[i]._OrderedQty);

                 if (isFinite(lineQty)) {
                     quantity += lineQty;
                 }
             }
             return quantity;
         };

         var _repriceOrder = function (inputCart, isSetToOrderCart) {
             if (angular.isObject(inputCart)) {
                 return repriceService.reprice(inputCart).then(
                     function (response) {
                         if (isSetToOrderCart) {
                             orderCart = response.data;
                             $rootScope.$broadcast('orderCartReset', { cart: _getLiveOrderCart() });
                         }
                         return response;
                     }
                     );
             } else {
                 return repriceService.reprice(_getLiveOrderCart()).then(
                     function (response) {
                         if (isSetToOrderCart) {
                             orderCart = response.data;
                             $rootScope.$broadcast('orderCartReset', { cart: _getLiveOrderCart() });
                         }
                         return response;
                     }
                     );
             }

             //var copyCart = angular.copy(_getLiveOrderCart());
             //if (isGetCarrierService) {
             //    var promise = repriceService.reprice(copyCart);

             //    if (promise) {
             //        promise.then(function (response) {
             //            _mergeCart(response.mergedCart);
             //            _updateCarrierServiceList();
             //            return response;
             //        });
             //    }
             //    return promise;

             //} else {
             //    var promise = repriceService.reprice(copyCart);

             //    if (promise) {
             //        promise.then(function (response) {
             //            _mergeCart(response.mergedCart);
             //            return response;
             //        });
             //    }
             //    return promise;
             //}
         };

         var _setOrderCart = function (newCart) {
             orderCart = newCart;
             $rootScope.$broadcast('orderCartReset', { cart: _getLiveOrderCart() });
         };

         var _initNewCart = function () {

             //reset every model / local variable
             orderCart = angular.copy(_INIT_CART);
             _currentCustomerKey = '';
             _giftRegistryAddressSet = {};

             //set initial store / user specific data onto cart
             var posParams = POSService.getPOSParameters();
             if (!angular.isDefined(posParams)) {
                 POSService.setPOSTParametersFromURL(window.location.href);
                 posParams = POSService.getPOSParameters();
             }

             _setStoreAndAssociateId(posParams.storeNumber, posParams.associateId);

             //TODO: set _EntryType to ELK or BOS ?? //should be like "ELD" aka elderman OR else GetOrderCartPrice Will fail DO NOT USE 'Store'
             //TODO: set _PaymentRuleId to "BT_DEF_PAYMENT_RULE" ?? or just remove and leave blank?

             //if customer is set in program / customerService set defaults to customer
             //TODO: change _resetOrderAddresses to use customerService
             if ($customer.isCustomerSelected()) {
                 _setCustomer($customer.getSelectedCustomer());
             }
         };

         var _broadcastCartDeleteEvent = function () {
             $rootScope.$broadcast('orderCartDeleted', { cart: _getLiveOrderCart() });
         };

         var _deleteShippingDiscount = function (orderLine) {
             var itemToDelete = -1;
             angular.forEach(orderLine.LineCharges.LineCharge, function (lineCharge, index) {
                 if (lineCharge._ChargeCategory === 'BTN_SHIP_DISC') {
                     itemToDelete = index;
                 }
             });
             if (itemToDelete > -1) {
                 orderLine.LineCharges.LineCharge.splice(itemToDelete, 1);
             }

             return orderLine;
         }

         _initNewCart();

         var returnObject = {
             address: {
                 /**
                  * Sets the ShipTo address of all OrderLines passed in.
                  * 
                  * @param {Array} arrayOfPrimeSubLineObjects is an array of objects that indicate prime and sublines. [ {PrimeLine:1, SubLine:1} ]
                  * @param {Object} addressObject Full address object { "_IsShipTo":"Y",... "PersonInfo":{_EMailID:....} }
                  * @param {Boolean} isGiftRegAddress Indicates if address is a gift Registry address
                  */
                 setOrderLineShipToAddresses: function (arrayOfPrimeSubLineObjects, addressObject, isGiftRegAddress) {
                     var result = _setOrderLineShipToAddresses(angular.copy(arrayOfPrimeSubLineObjects), addressObject, isGiftRegAddress);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setOrderLineShipToAddresses");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * Sets the Billing address of Order Header.
                  * 
                  * @param {Object} addressObject Full address object { "_IsShipTo":"Y",... "PersonInfo":{_EMailID:....} }
                  */
                 setBillingAddress: function (addressObject) {
                     var result = _setBillingAddress(addressObject);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setBillingAddress");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * Checks all addresses and returns errors.
                  * @param {Object} inputCart Validates provided cart, else defaults to $appServiceOrderCart's orderCart.
                  * 
                  * @return {Promise} errors The data from promise is {
                                         defaultBillingAddressError:{hasError: false, errorText: ''},
                                         defaultShipToAddressError:{hasError: false, errorText: ''},
                                         carrierServiceCodeError:{hasError: false, errorText: '', orderlines:[]},
                                         bigTicketError:{hasError: false, errorText: '', orderlines:[]},
                                         orderlineShiptoAddressError:{hasError: false, errorText: '', orderlines:[{ orderline:currentOrderLine,  errorText: 'Line 1.1 errors:... ' }]}
                                     }
                  */
                 validateOrderAddresses: function (inputCart) {
                     return _validateOrderAddresses(inputCart);
                 },
                 /**
                  * 
                  * @param {Object} address A Person Info address
                  * @param {Boolean} isBillingAddress True- validates only Billing criteria.
                  * @returns {Object} { isValid: true, errorMessage: 'error, error2,' }
                  */

                 validAddress: function (address, isBillingAddress) { return _validAddress(address, isBillingAddress); }
             },
             carrierService: {
                 updateCarrierServiceList: function () {
                     var result = _updateCarrierServiceList();
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "updateCarrierServiceList");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 setOrderlineCarrierService: function (orderline, carrierService) {
                     var result = _setOrderLineCarrierService(orderline, carrierService);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setOrderlineCarrierService");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 setOrderLineCarrierServiceToLowestPrice: function (orderline) {
                     return _setOrderLineCarrierServiceToLowestPrice(orderline);
                 }
             },
             customer: {
                 /**
                  * Sets Customer to order header and orderline addresses if customerKey is different. Sets all Orderlines to default Ship To address.
                  * 
                  * @param {Object} customerObject is complete customer details object including billing address, shipTo address, and additional addresses
                  */
                 setCustomer: function (customerObject) {
                     var result = _setCustomer(customerObject);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setCustomer");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 getCustomerKey: function () {
                     return _getCustomerKey();
                 },
                 isCartCustomerSet: function () {
                     return _isCartCustomerSet();
                 },
                 /**
                  * Returns orderCarts Additional Addresses map.
                  * 
                  * @returns {Object} Map of additional addresses
                  */
                 // TODO: pob delete method
                 getAdditionalAddresses: function () {
                     var result = '';
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "getAdditionalAddresses");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * Adds address to additional address map.
                  * 
                  * @param {Object} addressObject
                  * @returns {Number} Key value from address map
                  */
                 // TODO: pob delete method
                 addAdditionalAddress: function (addressObject) {
                     var result = '';
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "addAdditionalAddress");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * This will remove customer and all none gift registry addresses
                  * @returns {undefined}
                  */
                 deleteCustomer: function () {
                     return _deleteCustomer();
                 }
             },
             discount: {
                 /**
                  * Adds coupon to list of coupons and reprices order
                  * 
                  * @param {String} couponCode 
                  * @returns {Object} status of coupon {Status:Used} {Status:Rejected}
                  */
                 addCoupon: function (couponCode) {
                     var result = '';
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "addCoupon");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * Deletes coupon from list and reprices order
                  * 
                  * @param {String} couponCode
                  */
                 deleteCoupon: function (couponCode) {
                     var result = '';
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "deleteCoupon");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 }

             },
             giftOptions: {
                 /**
                  * Return Message Object with gift message for specified orderline
                  * 
                  * @param {Object | Array} primeSubLineObject is object that indicate prime and sublines. {PrimeLine:1, SubLine:1}  Array will give back first non-empty message set.
                  * @returns {Object | null} Object of the three gift messages {To:"Dad",From:"Me",Message:"Margaritaville", Registry:"asb123"}
                  */
                 getGiftOptionFromOrderLines: function (primeSubLineObject) {
                     var result = _getGiftOptionFromOrderLines(angular.copy(primeSubLineObject));
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "getGiftOptionFromOrderLines");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 addGiftBox: function (arrayOfPrimeSubLineObjects) {
                     var result = _addGiftBox(arrayOfPrimeSubLineObjects);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "addGiftBox");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 deleteGiftBox: function (arrayOfPrimeSubLineObjects) {
                     var result = _deleteGiftBox(arrayOfPrimeSubLineObjects);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "deleteGiftBox");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * Sets Gift Message to all orderlines indicated. Overwrites old messages.
                  *
                  * @param {Array} arrayOfPrimeSubLineObjects is an array of objects that indicate prime and sublines. [ {PrimeLine:1, SubLine:1} ]
                  * @param {Object} giftMessageInputs is Object of the three gift messages {To:"Dad",From:"Me",Message:"Margaritaville"}.  
                  */
                 setGiftMessage: function (arrayOfPrimeSubLineObjects, giftMessageInputs) {
                     var result = _setGiftMessage(angular.copy(arrayOfPrimeSubLineObjects), giftMessageInputs);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setGiftMessage");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 //TODO if regNo === "" reset orderlines to default shipto
                 setGiftRegistry: function (arrayOfPrimeSubLineObjects, regNo, regAddress) {
                     var result = _setGiftRegistry(angular.copy(arrayOfPrimeSubLineObjects), regNo, regAddress);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setGiftRegistry");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 getRegistryAddressSet: function () {
                     var result = _getRegistryAddressSet();
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "getRegistryAddressSet");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * 
                  * @returns {promise} On success, addresses will be returned.
                  */
                 loadRegistryAddresses: function () {
                     var result = _loadRegistryAddresses();
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "loadRegistryAddresses");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 }
             },
             order: {
                 /**
                  * Returns reference to full order cart model that is updated by this service's methods
                  * 
                  * @returns {Object} Reference to service's orderCart
                  */
                 getLiveOrderCart: function () {
                     return _getLiveOrderCart();
                 },
                 /**
                  * Forces reprice of orderCart
                  * 
                  * @returns {Boolean} reprice success true or false 
                  */
                 repriceOrder: function (isSetToOrderCart) {
                     var result = _repriceOrder(null, isSetToOrderCart);

                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "repriceOrder");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 setOrderCart: function (newCart) {
                     return _setOrderCart(newCart);
                 },
                 getTaxExemptionCertificate: function () {
                     var result = _getTaxExemptionCertificate();
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "getTaxExemptionCertificate");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 setTaxExemptionCertificate: function (taxCert) {
                     var result = _setTaxExemptionCertificate(taxCert);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setTaxExemptionCertificate");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 getOrderNumber: function ()
                 {
                     return _getOrderNumber();

                },
                 /**
                  * Renumbers lines, removes zero qty lines, and sorts cart by prime / sub line numbers
                  */
                 cleanOrderCart: function () {
                     var result = _cleanOrderCart();
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "cleanOrderCart");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 deleteCart: function () {
                     _initNewCart();
                     _broadcastCartDeleteEvent();
                     return _getLiveOrderCart();
                 },
                 /**
                 *getLargestAvailableInventoryAndOrderedQtyForSkuInCart loops through all orderlines
                 *  and sums up the quantity currently ordered for that SKU.  It also finds the
                 *  LARGEST available quantity reported matching skus.
                 * 
                 * @param {item id/sku} itemSku
                 * @returns {Object} Object {largestAvailableInventory:20, orderedQty: 0}
                 */
                 getLargestAvailableInventoryAndOrderedQtyForSkuInCart: function (itemSku) {
                     return _getLargestAvailableInventoryAndOrderedQtyForSkuInCart(itemSku);
                 },
                 /**
                  * Submits Order
                  * 
                  * @returns {Boolean} true - order is in Sterling, false - error occured. 
                  */
                 submitOrder: function () {
                     var result = '';
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "submitOrder");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },

                 getItemCount: function () { return _getItemCount(); },

                 deleteShippingDiscount: function (orderLine) {
                     return _deleteShippingDiscount(orderLine);
                 },
                 /**
                  * validateOrderHeader check for valid attributes and elements at the header level
                  * 
                  * @param {Object} inputCart - Part in a full order cart. Default uses current service's order cart.
                  * @returns {Object} - {customerEmailIdError:{hasError:true, errorText:'Text description of error.'}}
                  */
                 validateOrderHeader: function (inputCart) { return _validateOrderHeader(inputCart); },

                 hasBigTicketItems: function (inputCart) { return _hasBigTicketItems(inputCart); }
             },
             orderLine: {
                 /**
                  * addOrderLine to order.
                  * @param {fullItemObject} fullItemObject - Must have item sku, item's solr data, pricing, promotions... PrimeLineNo and SublineNo will be overwritten
                  * @param {Int} quantity - Any positive integer between 1 and OrderableInventory. Default = 1
                  * @param {Boolean} isBigTicketValidated - Allows Big Ticket Items to be added to Order if set to true. Else addOrderLine will throw Error.
                  */
                 addOrderLine: function (fullItemObject, quantity, isBigTicketValidated) { //add to OrderLinesDisplayDetail AND Real Order Cart CREATE FAKE ORDER LINE NUMBERS THEN ERASE ALL ORDER LINE NUMBERS AT THE END

                     //$loggerService.log(fullItemObject);

                     var result = _addItem(fullItemObject, quantity, isBigTicketValidated);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "addOrderLine");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;

                 },
                 /**
                  * 
                  * @param {Object} primeSubLineObject is object that indicate prime and sublines. {PrimeLine:1, SubLine:1} 
                  * @param {Number} newQuantity number that is set for orderline's quantity. 
                  * Validated: newQuantity > -1, validate range: newQuantity + quantity of other orderlines with sku is in range [0, sku's largest found orderable quantity]  
                  * @returns {Boolean} true, orderline changed. false, error.
                  */
                 setOrderLineQuantity: function (primeSubLineObject, newQuantity) {
                     var result = _setOrderLineQuantity(primeSubLineObject, newQuantity);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "setOrderLineQuantity");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 /**
                  * Deletes from orderCart all orderlines passed in.
                  * 
                  * @param {Array} arrayOfPrimeSubLineObjects is an array of objects that indicate prime and sublines. [ {PrimeLine:1, SubLine:1} ]
                  */
                 deleteOrderLine: function (arrayOfPrimeSubLineObjects) {
                     var result = _deleteOrderLine(arrayOfPrimeSubLineObjects, true);
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "deleteOrderLine");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 },
                 util: {
                     getPrimeSubLineObject: function (orderline) {
                         return _getPrimeSubLineObject(orderline);
                     },
                     orderlineCount: function () {
                         return _orderlineCount();
                     },
                     /**
                      * 
                      * @param {orderline Object} orderline - The methods uses the passed orderline not necessarily one in the orderCart.
                      * @returns {Float} The lowest price
                      */
                     getLowestSalePrice: function (orderline) {
                         return _getLowestOrderLinePrice(orderline);
                     },
                     /**
                     * getAvailableInventoryAndOrderedQty loops through all orderlines
                     *  and sums up the quantity currently ordered for that SKU.  It also finds the
                     *  LARGEST available quantity reported matching skus.
                     * 
                     * @param {orderline} orderline
                     * @returns {Object} Object {largestAvailableInventory:20, orderedQty: 0}
                     */
                     getAvailableInventoryAndOrderedQty: function (orderline) {
                         return _getAvailableInventoryAndOrderedQty(orderline);
                     }

                 }

             },
             payment: {
                 /**
                  * Adds payment Method to order.
                  * 
                  * @param {Object} creditCardObject
                  */
                 addCreditCard: function (creditCardObject) {
                     var result = '';
                     if (_isLogCart) {
                         $loggerService.log("CURRENT ORDER CART:" + "addCreditCard");
                         $loggerService.log(_getLiveOrderCart());
                     }
                     return result;
                 }
             },
             subtotal: {
                 /**
                  * 
                  * @param {orderline Object} orderline - The methods uses the passed orderline not necessarily one in the orderCart.
                  * @returns {Float} The total cost including
                  */
                 computeShippingOrderLineSubtotal: function (orderline) {
                     return _computeShippingOrderLineSubtotal(orderline);
                 }
             },
             util: {
                 /**
                  * FOR TESTING. TODO: Delete
                  */
                 setExampleOrder: function () {
                     return _setExampleOrder();
                 },
                 /**
                  * leftPad will pad front of string to a specified length with optional padding string.
                  * 
                  * @param {String | Number} stringOrNum - the string to left-pad
                  * @param {Number} totalLength - the final length of the returned string.
                  * @param {String} [optionalPadChar="0"] - Any string to use as padding.
                  * @returns {String | null} - Padded string or null if error.
                  */
                 leftPad: function (stringOrNum, totalLength, optionalPadChar) {
                     return _padFront(stringOrNum, totalLength, optionalPadChar);
                 }
             }
         };

         return returnObject;
     }])
.factory('expectedDeliveryService', ['$http', '$filter', function ($http, $filter) {

    var _getDates = function () {
        return $http.get(serviceURL.toString() + '/Utility/GetExpectedDeliveryDates').success(function (data) {
            for (var i = 0; i < data.ExpectedDeliveryDates.length; i++) {
                //format Dates to Sep 3, 2015
                data.ExpectedDeliveryDates[i].Date = $filter('date')(data.ExpectedDeliveryDates[i].Date, 'mediumDate');

            }

            return data;
        }).error(function (data) {
            $sendSMTPErrorEmail(response, serviceURL.toString() + '/Price/GetCarrierServiceCodes', data);

        });
    };

    return { getDates: _getDates };

}])
;

/**************************************************
 * {
  "Order": {
    "_AuthorizedClient": "Store",
    "_BillToID": "",
    "_CustomerContactID": "",
    "_CustomerEMailID": "garrett.stibb@bonton.com",
    "_DocumentType": "0001",
    "_DraftOrderFlag": "Y",
    "_EnteredBy": "014265",
    "_EnterpriseCode": "BONTON",
    "_EntryType": "BTN", //should be like "ELD" aka elderman OR else GetOrderCartPrice Will fail DO NOT USE 'Store'
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
            "_OrderedQty": "2.0",
            "_PrimeLineNo": 1,
            "_LevelOfService": "GRND",
            "_ScacAndService": "UPS-GRND",
            "_SCAC": "UPS",
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
            "_UnitPrice": "40.00",
            "_TaxableFlag":"Y"
            },
            "LineCharges": {
            "LineCharge": [
            //this first line charge will fail CarrierServiceCodes call. MUST have _ChargePerLine && _ChargeAmount on first of list!
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
                "itemType": "REG",
                "storageType": "R",
                "_AvailableQty": 2000,
                "alternateUPCs": [
                  "031290088157"
                ],
                "imageURL": "/assets/images/865391.jpg",
                "brandlongdesc": "Carter's",
                "cfgdesc": "GIRLS",
                "cfgid": 101,
                "classlongdesc": "CARTERS",
                "cmgdesc": "CHILDRENS",
                "cmgid": 10,
                "colorattrdesc": "Asst Lt/Pale",
                "colorcode": "",
                "colorDc": "",
                "colorfamdesc": "Asst Family",
                "colorlongdesc": "Navy",
                "corpdesc": "",
                "crgdesc": "CHILDRENS",
                "crgid": 0,
                "deptlongdesc": "GIRLSWEAR 2-6X",
                "desc1": "TEE",
                "desc2": "FALL CARTERS OP",
                "desc3": "CARTERS OCT",
                "desc4": "4-6X GIRL",
                "fabdtldesc": "Screen Print",
                "fablongdesc": "Cotton Blend-With Stretch",
                "fobdesc": "GIRLS 2-6X",
                "fobid": 28,
                "genclasslongdesc": "Knit Tops",
                "gensclalongdesc": "Tee",
                "imageid": "",
                "isn": "243306978",
                "isnlongdesc": "GREEN FLORAL BABYDOLL TOP",
                "itemsize": "4",
                "keywords": "",
                "labellongdesc": "Carter's",
                "longpatterndesc": "",
                "proddetail2": "Scoop Neck",
                "proddetail3": "Knit-Interlock/Jersey",
                "proddtllongdesc": "Short Sleeve",
                "productcode": "",
                "productname": "Green Floral Babydoll Top",
                "size1code": "",
                "size2code": "",
                "sizedc": "",
                "sizesequence": "1",
                "vendorstyle": "273B053",
                "webcatprodlongdesc": "",
                "webid": ""
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
            "btSpecialHandlingFee": "0",
            giftImgUrl: "assets/images/GiftMessage.gif",
            isGiftRegistryAddress: false,
            giftBoxImgUrl: "assets/images/giftBox.gif"
            }
        }
        }
      ]
    },
    "PersonInfoBillTo": {
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
  }
}
 * 
 */