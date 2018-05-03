angular.module('appFilters', ['ui.bootstrap'])
    .filter('structurefilter', function () {
        return function (items) {
            var temp = [];
            if (items) {
                items.forEach(function (item) {
                    if (item.visible) {
                        temp.push(item);
                    }
                });
            }
            return temp;
        };
    })
.filter('phoneFormat', function () {
    return function (digits) {
        
        if (angular.isString(digits) || (angular.isNumber(digits) && isFinite(digits))) {
            if (angular.isString(digits)) {
                //clean to just numbers
                digits = digits.replace(/\D/g,'');
            }

            if (angular.isNumber(digits)) {
                digits = parseInt(digits);
            }

            var temp = digits.toString().trim();
            var areacode = temp.slice(0, 3);
            var first = temp.slice(3, 6);
            var last = temp.slice(6, 10);
            temp = "";

            if (areacode.length > 0) {
                temp = "(" + areacode.slice(0, 3) ;
            }

            if(first.length > 0){
                temp = temp + ")" + first;
            }
            if (last.length > 0) {
                temp = temp + "-" + last;
            }
            return temp;
        } else {
            return digits;
        }
    };
})
.filter('phoneFormatRemove', function () {
    return function (digits) {
            if (angular.isString(digits)) {
                //clean to just numbers
                digits = digits.replace(/\D/g, '');
                return digits;
            } else {
                return digits;
            }
    };
})
.filter('oneLineAddress', ['$filter', function ($filter) {
    return function (address, addressType, name, id) {
        if (angular.isObject(address) && !angular.isArray(address)) {
            
            name = name ? 'name' : '';
            id = id ? 'id' : '';
            addressType = (addressType && angular.isString(addressType)) ? addressType : 'long';

            var addressesArray = [];

            //if name add First, Middle, Last name
            if (name === 'name') {
                var tempNameArr = [];
                if (address._FirstName && address._FirstName.toString().trim()) {
                    tempNameArr.push(address._FirstName.toString().trim());
                }
                if (address._MiddleName && address._MiddleName.toString().trim()) {
                    tempNameArr.push(address._MiddleName.toString().trim());
                }
                if (address._LastName && address._LastName.toString().trim()) {
                    tempNameArr.push(address._LastName.toString().trim());
                }

                if (tempNameArr.length > 0) {
                    addressesArray.push(tempNameArr.join(" "));
                }

            }

            if (addressType !== 'none') {
                if (address._AddressLine1 && address._AddressLine1.toString().trim()) {
                    addressesArray.push(address._AddressLine1.toString().trim());
                }
                if (address._AddressLine2 && address._AddressLine2.toString().trim()) {
                    addressesArray.push(address._AddressLine2.toString().trim());
                }
                if (address._AddressLine3 && address._AddressLine3.toString().trim()) {
                    addressesArray.push(address._AddressLine3.toString().trim());
                }
                if (address._AddressLine4 && address._AddressLine4.toString().trim()) {
                    addressesArray.push(address._AddressLine4.toString().trim());
                }
                if (address._AddressLine5 && address._AddressLine5.toString().trim()) {
                    addressesArray.push(address._AddressLine5.toString().trim());
                }
                if (address._AddressLine6 && address._AddressLine6.toString().trim()) {
                    addressesArray.push(address._AddressLine6.toString().trim());
                }

                if (addressType === 'long') {
                    if (address._City && address._City.toString().trim()) {
                        addressesArray.push($filter('titlecase')(address._City.toString().trim()));
                    }

                    //group state and zip together so no comma between
                    var tempStateZipArray = [];
                    if (address._State && address._State.toString().trim()) {
                        tempStateZipArray.push(address._State.toString().trim());
                    }

                    if (address._ZipCode && address._ZipCode.toString().trim()) {
                        tempStateZipArray.push(address._ZipCode.toString().trim());
                    }

                    if (tempStateZipArray.length > 0) {
                        addressesArray.push(tempStateZipArray.join(" "));
                    }
                
                    //country if not "US"
                    if (address._Country && address._Country.toString().trim() && !(/us/).test(address._Country.toString().trim().toLowerCase())) {
                        addressesArray.push(address._Country.toString().trim());
                    }
                }
            }

            var resultString = addressesArray.join(", ");

            if (id === 'id') {
                if (address._AddressID && address._AddressID.toString().trim()) {
                    resultString = '(' + address._AddressID.toString().trim() + ') ' + resultString;
                }
            }

            return resultString;

        } else {
            return address;
        }
    };
}])
.filter('titlecase', function () {
    return function (s) {
        s = (s === undefined || s === null) ? '' : s;
        return s.toString().toLowerCase().replace(/\b([a-z])/g, function (ch) {
            return ch.toUpperCase();
        });
    };
})
.filter('deliveryText', function () {
    return function (method) {
        if (angular.isString(method)) {

            switch (method.toLowerCase()) {
                case 'shp': method = 'Shipping'; break;
                case 'pick': method = 'Pick Up In Store'; break;
            }

            return method;
        } else { return method; }
    };
})
;