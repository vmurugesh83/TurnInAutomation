angular.module('appServiceAppState', [])
.factory('appState', [function () {
    var state = {};


    state.model = { addressChange: { previousStateName: null, customerKey: null, setShipToOrderLinePrimeSubLineObjArray: [] } };
    state.addressChange = { previousStateName: null, customerKey: null, setShipToOrderLinePrimeSubLineObjArray: [] };

    var _getAddressChange = function () {

        var copy = angular.copy(state.addressChange);
        state.addressChange = angular.copy(state.model.addressChange);
        return copy;
    };

    var _setAddressChange = function (addressChange) {

        state.addressChange = addressChange;
    };

    return {
        getAddressChange: _getAddressChange,
        setAddressChange: _setAddressChange
    };
}])
;