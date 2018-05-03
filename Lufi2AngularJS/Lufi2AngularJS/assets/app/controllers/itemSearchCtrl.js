angular.module('itemSearch', ['ui.bootstrap']).controller('itemSearchCtrl', ['$scope', '$location', 'numberOnlyKeyPressValidator', '$rootScope', 'itemSearch', '$stateParams', '$timeout', function ($scope, $location, $numberOnlyKeyPressValidator, $rootScope, $itemSearch, $stateParams, $timeout) {
       $scope.queryParameters = {}
       $scope.numberValidator = $numberOnlyKeyPressValidator;
       $scope.cmgData = [];
       $scope.cfgData = [];
       $scope.fobData = [];

       $scope.specialCharRemove = function (keyEvent) {
           var theEvent = keyEvent || window.event;
           var key = theEvent.keyCode || theEvent.which;
           if (key == 13) {
               return;
           }
           key = String.fromCharCode(key);
           var regex = /[0-9a-zA-Z ]/;
           if (!regex.test(key)) {
               theEvent.returnValue = false;
               if (theEvent.preventDefault) theEvent.preventDefault();
           }
       };


       $scope.upcPaste = function ($event, inputQueryName) {
           if (typeof $event.originalEvent.clipboardData !== "undefined") {
               handlePastedUPC($event.originalEvent.clipboardData.getData('text/plain'), inputQueryName);
           } else { // To support browsers without clipboard API (IE and older browsers)
               $timeout(function () {
                   handlePastedUPC(angular.element($event.currentTarget).val(), inputQueryName);
               }, 2);
           }
       };

       var handlePastedUPC = function (pastedText, inputQueryName) {
           var cleansedText = "";
           var inputField = angular.isString(inputQueryName) && (inputQueryName.length > 0) ? inputQueryName : "upc";


           for (var i = 0; i < pastedText.length; i++) {
               if ((/\d/).test(pastedText[i])) {
                   cleansedText += pastedText[i];
               }
           }
           $timeout(function () {
               $scope.queryParameters[inputField] = cleansedText;
           }, 3);
       };

       $scope.webIdPaste = function ($event) {
           if (typeof $event.originalEvent.clipboardData !== "undefined") {
               handlePastedWedId($event.originalEvent.clipboardData.getData('text/plain'));
           } else { // To support browsers without clipboard API (IE and older browsers)
               $timeout(function () {
                   handlePastedWedId(angular.element($event.currentTarget).val());
               }, 2);
           }
       };

       var handlePastedWedId = function (pastedText) {
           var cleansedText = "";


           for (var i = 0; i < pastedText.length; i++) {
               if ((/[0-9a-zA-Z ]/).test(pastedText[i])) {
                   cleansedText += pastedText[i];
               }
           }
           $timeout(function () {
               $scope.queryParameters.webIdImageId = cleansedText;
           }, 3);
       };




       $scope.cleanSearchParameters = function () {
           $itemSearch.clearCachedResult();
           $scope.queryParameters = {}
           $scope.cmgData.forEach(function (item) {
               item.visible = true;
           });
           $scope.cfgData.forEach(function (item) {
               item.visible = true;
           });
           $scope.fobData.forEach(function (item) {
               item.visible = true;
           });
       }

       $scope.isSearchable = function () {
           var result = $scope.queryParameters.upc ? true : false ||
                     $scope.queryParameters.webIdImageId ? true : false ||
                     $scope.queryParameters.brandName ? true : false ||
                     $scope.queryParameters.vendorStyle ? true : false ||
                     $scope.queryParameters.description ? true : false ||
                     $scope.queryParameters.selectedCFG ? true : false ||
                     $scope.queryParameters.selectedCMG ? true : false ||
                     $scope.queryParameters.selectedFOB ? true : false ||
                     $scope.queryParameters.upcWSizeFilter ? true : false ||
                     $scope.queryParameters.upcWColorFilter ? true : false;
           return result;
       }

       $scope.search = function () {
           if ($scope.isSearchable()) {

               $scope.queryParameters.start = 0;
               $scope.queryParameters.currentPage = 1;
               $scope.queryParameters.rows = $itemSearch.MAX_NUMBER_OF_ROWS;
               $scope.queryParameters.selectedBrands = [];
               $scope.queryParameters.selectedColors = [];
               $scope.queryParameters.selectedItemSizes = [];
               $scope.queryParameters.selectedFOBs = [];


               var searchFunction = null;
               var searchParams = null;

               if ($scope.queryParameters.upc)
               {
                   searchFunction = $itemSearch.searchUPC;
                   searchParams = $scope.queryParameters.upc;
               }
               else if ($scope.queryParameters.upcWSizeFilter) {
                   searchFunction = $itemSearch.searchUPCMoreColors;
                   searchParams = $scope.queryParameters.upcWSizeFilter;
               }
               else if ($scope.queryParameters.upcWColorFilter) {
                   searchFunction = $itemSearch.searchUPCMoreSizes;
                   searchParams = $scope.queryParameters.upcWColorFilter;
               }
               else {
                   searchFunction = $itemSearch.search;
                   searchParams = $scope.queryParameters;
               }


               //$scope.queryParameters.upc ? $itemSearch.searchUPC : $itemSearch.search

               //var searchParams = $scope.queryParameters.upc ? $scope.queryParameters.upc : $scope.queryParameters
               searchFunction(searchParams, function (response) {
                   if (response.ngroups == 0) {
                       jQuery('.not-found-alert').modal('show');
                   } else if (response.isnGroup.length == 1) {
                       $itemSearch.setSelectedISNGroup(response.isnGroup[0]);
                       $location.path('/itemDetail');
                   }  else {
                       $location.path("/itemResults");
                   }
               }, function (err) {
                   alert(err);
               });
           }
       };

       if (angular.isNumber($stateParams.upc)) {
           $scope.cleanSearchParameters();
           $scope.queryParameters.upc = $stateParams.upc;
           $scope.search();
           $stateParams.upc = "";
       }

       $itemSearch.retrieveBrands(
           function (data) {
               $scope.brandsData = data;
           },
           function error(err) {
              alert(err)  
           }
       );


       $itemSearch.retrieveProductHierarchy(
           function (data) {
               $scope.cmgData = data.cmgData;
               $scope.cfgData = data.cfgData;
               $scope.fobData = data.fobData;
           }, function (err) {
           alert(err);
       });

       function angularToItemStructureItem(itemStructureArray) {
           return function populate(item) { 
              itemStructureArray.push(item);
           }                
       }

//       var cachedParams = $itemSearch.getCachedQueryParam()
//       if (cachedParams) {
//               $scope.queryParameters.upc = cachedParams.originalUPC;
//               $scope.queryParameters.webIdImageId = cachedParams.webIdImageId;
//               $scope.queryParameters.brandName = cachedParams.brandName;
//               $scope.queryParameters.vendorStyle = cachedParams.vendorStyle;
//               $scope.queryParameters.description = cachedParams.description;
//               $scope.queryParameters.selectedCMG = cachedParams.selectedCMG;
//               $scope.queryParameters.selectedCFG = cachedParams.selectedCFG;
//               $scope.queryParameters.selectedFOB = cachedParams.selectedFOB;
//      }

       var filterItems = function (arrayData, prop, selectedValue) {
           arrayData.forEach(function (item) {
               if (selectedValue) {
                   if (item[prop] == selectedValue)
                       item.visible = true;
                   else
                       item.visible = false;
               } else {
                   item.visible = true;
               }
           });
       }
        
       $scope.cmgClick = function () {
           filterItems($scope.cfgData, 'cmgItemOriginalValue', $scope.queryParameters.selectedCMG);
           filterItems($scope.fobData, 'cmgItemOriginalValue', $scope.queryParameters.selectedCMG);
           $scope.queryParameters.selectedCFG = undefined;
           $scope.queryParameters.selectedFOB = undefined;
       }

       $scope.cfgClick = function () {
           if ($scope.queryParameters.selectedCFG) {
               filterItems($scope.fobData, 'cfgItemOriginalValue', $scope.queryParameters.selectedCFG);
               $scope.queryParameters.selectedFOB = undefined;
           }
       }


       var localUPCScanHandler = function (event, upc) {
           var location = document.activeElement.id;
           if (location == "upcSizesField") {
               $scope.queryParameters.upcWSizeFilter = upc;
           } else if (location == "upcColorsField") {
               $scope.queryParameters.upcWColorFilter = upc;
           } else {
               $scope.queryParameters.upc = upc;
           }
           $scope.search();
       };

       var deregister = $rootScope.$on('Scanner_Event', localUPCScanHandler);

       $scope.$on("$destroy", function () {
           deregister();
       });

    }]);