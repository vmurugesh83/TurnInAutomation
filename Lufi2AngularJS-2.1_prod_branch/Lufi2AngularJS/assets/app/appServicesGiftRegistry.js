angular.module('appServicesGiftRegistry',[])
.factory('giftRegistryService', ['$http', 'loggerService', 'sendSMTPErrorEmail', function ($http, $loggerService, $sendSMTPErrorEmail) {

    var _preferredAddressPromise = function (registryNo) {

        var input = {
            "GiftRegistryInput": {
                "GetPreferredAddress": {
                    "registryNumber": registryNo.toString()
                }
            }
        };
        var url = serviceURL.toString() + '/GiftRegistry/GetPreferredAddress';
        return $http.post(url, input);
    }

    var _validateRegistryPromise = function (registryNo) {

        var input = {
            "IsRegistryValidInput": {
                "IsRegistryValid": {
                    "registryNumber": registryNo.toString()
                }
            }
        };

        var url = serviceURL.toString() + '/GiftRegistry/IsRegistryValid';
        return $http.post(url, input);
    };

    var _constructRegistryAddress = function (registryValue, response){
        if(!angular.isDefined(registryValue) || !angular.isObject(response)){
            return null;
        }
       
        var temp = {
            "_IsShipTo": "Y",
            "_IsDefaultSoldTo": "N",
            "_IsDefaultBillTo": "N",
            "_CustomerAdditionalAddressID": "",
            "_AddressType": "S",
            "_IsDefaultShipTo": "N",
            "PersonInfo": {
                "_RegTitle":"",
                "_EMailID": "",
                "_PersonInfoKey": "" + registryValue,
                "_VerificationStatus": "",
                "_PreferredShipAddress": "",
                "_DayPhone": "",
                "_LastName": "",
                "_EveningFaxNo": "",
                "_OtherPhone": "",
                "_DayFaxNo": "",
                "_HttpUrl": "",
                "_PersonID": "",
                "_FirstName": "",
                "_MobilePhone": "",
                "_AlternateEmailID": "",
                "_Suffix": "",
                "_Country": "",
                "_ZipCode": "",
                "_Title": "",
                "_City": "",
                "_AddressID": "Registry Address",
                "_MiddleName": "",
                "_State": "",
                "_AddressLine4": "",
                "_AddressLine5": "",
                "_AddressLine6": "",
                "_EveningPhone": "",
                "_AddressLine1": "",
                "_AddressLine2": "",
                "_AddressLine3": ""
            }
        };
        if(angular.isDefined(response.Address1) && angular.isString(response.Address1)){
            temp.PersonInfo._AddressLine1 = response.Address1;
        }else{
            throw errorObj.newError('_constructRegistryAddress():GiftRegAddressError', "There is an error in the Gift Registry for Registry Number: " + registryValue + ". Please contact Help Desk.",
                    "_constructRegistryAddress(): Registry Number: " + registryValue + " has bad Address1 data returned from Gift Registry Service.", 'GiftRegAddressError', 'WARN');
        }
        if(angular.isDefined(response.Address2) && angular.isString(response.Address2)){
            temp.PersonInfo._AddressLine2 = response.Address2;
        } 
        if(angular.isDefined(response.Address3) && angular.isString(response.Address3)){
            temp.PersonInfo._AddressLine3 = response.Address3;
        }
        
        if(angular.isDefined(response.City) && angular.isString(response.City)){
            temp.PersonInfo._City = response.City;
        }else{
            throw errorObj.newError('_constructRegistryAddress():GiftRegAddressError', "There is an error in the Gift Registry for Registry Number: " + registryValue + ". Please contact Help Desk.",
                    "_constructRegistryAddress(): Registry Number: " + registryValue + " has bad City data returned from Gift Registry Service.", 'GiftRegAddressError', 'WARN');
        }
        if(angular.isDefined(response.Country) && angular.isString(response.Country)){
            temp.PersonInfo._Country = response.Country;
        }else{
            throw errorObj.newError('_constructRegistryAddress():GiftRegAddressError', "There is an error in the Gift Registry for Registry Number: " + registryValue + ". Please contact Help Desk.",
                    "_constructRegistryAddress(): Registry Number: " + registryValue + " has bad Country data returned from Gift Registry Service.", 'GiftRegAddressError', 'WARN');
        }
        //if email is defined, is a string, AND match at least one char, @ , one char , a period, and one char at end, else put in dummy
        if (angular.isDefined(response.Email1) && angular.isString(response.Email1) && (/.+@.+\..+/).test(response.Email1.toString().trim())) {
            temp.PersonInfo._EMailID = response.Email1;
        } else {
            temp.PersonInfo._EMailID = "bonton@bonton.com";
            $loggerService.log('Gift Registry had invalid email address of: "' + response.Email1 + '". Replaced with: "' + temp.PersonInfo._EMailID + '"');
        }
        if(angular.isDefined(response.FirstName) && angular.isString(response.FirstName)){
            temp.PersonInfo._FirstName = response.FirstName;
        }
        if(angular.isDefined(response.LastName) && angular.isString(response.LastName)){
            temp.PersonInfo._LastName = response.LastName;
        }
        if(angular.isDefined(response.Phone) && angular.isString(response.Phone)){
            temp.PersonInfo._DayPhone = response.Phone;
        }
        if(angular.isDefined(response.RegTitle) && angular.isString(response.RegTitle)){
            temp.PersonInfo._RegTitle = response.RegTitle;
        }
        if(angular.isDefined(response.State) && angular.isString(response.State)){
            temp.PersonInfo._State = response.State;
        }else{
            throw errorObj.newError('_constructRegistryAddress():GiftRegAddressError', "There is an error in the Gift Registry for Registry Number: " + registryValue + ". Please contact Help Desk.",
                    "_constructRegistryAddress(): Registry Number: " + registryValue + " has bad State data returned from Gift Registry Service.", 'GiftRegAddressError', 'WARN');
        }
        if(angular.isDefined(response.ZipCode) && angular.isString(response.ZipCode)){
            temp.PersonInfo._ZipCode = response.ZipCode;
        }else{
            throw errorObj.newError('_constructRegistryAddress():GiftRegAddressError', "There is an error in the Gift Registry for Registry Number: " + registryValue + ". Please contact Help Desk.",
                    "_constructRegistryAddress(): Registry Number: " + registryValue + " has bad ZipCode data returned from Gift Registry Service.", 'GiftRegAddressError', 'WARN');
        }
        return temp;
    };

    return {

        preferredAddressPromise: _preferredAddressPromise,
        validateRegistryPromise: _validateRegistryPromise,
        constructRegistryAddress: _constructRegistryAddress

    };
}])
;