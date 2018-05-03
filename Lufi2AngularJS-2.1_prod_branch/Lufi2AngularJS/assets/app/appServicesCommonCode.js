var appServices = angular.module('appServicesCommonCode', []);
appServices.factory('commonCode', ['$http', '$q', function ($http, $q) {

    //var url = 'http://localhost:50086/../misc/commonCode.min.json';
    var url = "assets/misc/commonCode.min.json";
    var commonCodeMap = {};
    var isCached = false;

    var _cacheCommonCode = function (orgFunctionCalled, searchCodeType, searchCodeValue) {
        return $http.get(url).then(function (response) {
            var data = response.data;

            for (var i = 0; i < data.CommonCodeList.CommonCode.length; i++) {
                var codeType = data.CommonCodeList.CommonCode[i]._CodeType;
                var codeValue = data.CommonCodeList.CommonCode[i]._CodeValue;

                if (!(codeType in commonCodeMap)) {
                    commonCodeMap[codeType] = {};
                }
                commonCodeMap[codeType][codeValue] = data.CommonCodeList.CommonCode[i];
            }
            isCached = true;

            return orgFunctionCalled(searchCodeType, searchCodeValue);

        });
    };

    var _getCommonCode = function (codeType, codeValue) {
        var returnValue = null;

        if (angular.isString(codeType) && angular.isString(codeValue)) {

            if (!isCached) {
                return _cacheCommonCode(_getCommonCode, codeType, codeValue);
            }

            if (commonCodeMap[codeType]) {
                if (commonCodeMap[codeType][codeValue]) {
                    returnValue = commonCodeMap[codeType][codeValue]._CodeLongDescription;
                }
            } else {
                commonCodeMap[codeType] = null;
                returnValue = null;
            }
        }
        var deferred = $q.defer();
        deferred.resolve(returnValue);

        return deferred.promise;
    };

    var _getCommonCodeObject = function (codeType, codeValue) {
        var returnValue = null;

        if (angular.isString(codeType) && angular.isString(codeValue)) {

            if (!isCached) {
                return _cacheCommonCode(_getCommonCodeObject, codeType, codeValue);
            }

            if (commonCodeMap[codeType]) {
                if (commonCodeMap[codeType][codeValue]) {
                    returnValue = angular.copy(commonCodeMap[codeType][codeValue]);
                }
            } else {
                commonCodeMap[codeType] = null;
                returnValue = null;
            }
        }

        var deferred = $q.defer();
        deferred.resolve(returnValue);

        return deferred.promise;
    };

    var _getAllCommonCodeType = function (codeType) {
        var returnValue = null;

        if (angular.isString(codeType)) {
            if (!isCached) {
                return _cacheCommonCode(_getAllCommonCodeType, codeType);
            }

            if (commonCodeMap[codeType]) {
                returnValue = [];
                for (var prop in commonCodeMap[codeType]) {
                    returnValue.push(angular.copy(commonCodeMap[codeType][prop]));
                }

            } else {
                commonCodeMap[codeType] = null;
                returnValue = null;
            }
        }
        var deferred = $q.defer();
        deferred.resolve(returnValue);

        return deferred.promise;
    };


    return {
        /**
         * @param {String} codeType String name of Sterling Common Code - codeType  (a group id like: 'LEVEL_OF_SERVICE')
         * @param {String} codeValue String name of value in Sterling Common Code - codeValue (ex 'GRND')
         * 
         * @returns {String | null} Returns specified commonCode's CodeLongDescription as defined in getCommonCode API
         */
        getCommonCode: function (codeType, codeValue) {
            return _getCommonCode(codeType, codeValue);
        },
        /**
         * @param {String} codeType String name of Sterling Common Code - codeType  (a group id like: 'LEVEL_OF_SERVICE')
         * @param {String} codeValue String name of value in Sterling Common Code - codeValue (ex 'GRND')
         * 
         * @returns {Object | null} Returns one commonCode object as defined in getCommonCode API
         */
        getCommonCodeObject: function (codeType, codeValue) {
            return _getCommonCodeObject(codeType, codeValue);
        },
        /**
         * @param {String} codeType String name of Sterling Common Code - codeType  (a group id like: 'LEVEL_OF_SERVICE')
         * 
         * @returns {Array | null} Array of commonCode objects. Object has properties _Attribute as from the Sterling API call
         */
        getAllCommonCodeType: function (codeType) {
            return _getAllCommonCodeType(codeType);
        }
    };


}]);