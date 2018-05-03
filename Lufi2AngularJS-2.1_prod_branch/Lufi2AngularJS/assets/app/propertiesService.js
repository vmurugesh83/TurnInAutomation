angular.module('propertiesService', [])
.factory('btProp', ["$log", function ($log) {

    var properties =
        {
            btGiftMessageReasonTo: "GIFT_MESSAGE_TO",
            btGiftMessageReasonFrom: "GIFT_MESSAGE_FROM",
            btGiftMessageReasonMessage: "GIFT_MESSAGE",
            btGiftMessageIconGrey: "assets/images/GreyGiftMessage.gif",
            btGiftMessageIcon: "assets/images/GiftMessage.gif",
            btGiftBoxIcon: "assets/images/giftBox.gif",
            btGiftBoxIconGrey: "assets/images/greyGiftBox.gif",
            btFooterText: "For technical issues with this web site, please contact the BonTon Help Desk at 1-800-585-7209",
            excludedItemTypesForLUFIOrder: ['BGT'],
            isBigTicketPurchaseDisabled: false,
            isExcludeGwpFromLufiOrder: true,
            isSingleItemPricingOnly: false,
            maxItemsAllowedToCallBulkPricingOnItemDetails: 25,
            orderCreateFailureWithGiftCardEmailList: "Derek.Donaldson@bonton.com;Colleen.Eimers@bonton.com",
            orderDetailCanCancelOrder: false, //turn off Cancel Order for all Users
            orderDetailCanLoadDraftOrder: false //turn off Draft Order and Re-Order for all Users

        };

    return {
        getProp: function (propName) {
            propName = "" + propName;
            if (propName in properties) {
                return properties[propName];
            } else {
                // TODO: Log missing property to error log.
                return null;
            }
        }
    }
}]);