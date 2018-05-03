angular.module('itemLocate', [])
    .controller('itemLocateCtrl', ['$scope', 'itemToLocate', 'itemLocate', 'POSService', '$location', function ($scope, $itemToLocate, $itemLocate, POSService, $location) {
        $scope.stores = [];
        $scope.currentItem = $itemToLocate.get();
        $scope.posParam = POSService.getPOSParameters();
        
        if ($scope.currentItem.colorimageid) {
            $scope.ImageURL = serviceURL + "/image/BonTon/" + $scope.currentItem.colorimageid;
        } else if ($scope.currentItem.imageid) {
            $scope.ImageURL = serviceURL + "/image/BonTon/" + $scope.currentItem.imageid;
        } else {
            $scope.ImageURL = "/assets/images/NotAvailable.jpg";
        }


        var myScroll = new IScroll('#ItemDetailWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });
        $scope.refreshIScroll = function () {
            setTimeout(function () {
                myScroll.refresh();
            }, 500);
        };

            if ($scope.currentItem.productname === "" || $scope.currentItem.productname == undefined) {
            $scope.showProductName = false;
            } else {
            $scope.showProductName = true;
            }
            
            $scope.getZip = function () {
                $itemLocate.getOrganizationList($scope.posParam.storeNumber).success(function (response) {
                    $scope.itemLocateZip = response.GetStoreInfoResp.Stores.Store.Address._ZipCode;
                    $scope.SearchType();
                    $scope.search();
                }).error(function (data) {
                    swal({ title: "Error!", text: data, showConfirmButton: true });
                });
            }

            $scope.SearchType = function () {
                if ($scope.itemLocateZip == "" && ($scope.itemLocateCity != "" || $scope.itemLocateState != "")) {
                    $scope.notZipSearch = true;
                    $scope.notCityStateSearch = false;
                } else if ($scope.itemLocateZip != "") {
                    $scope.notZipSearch = false;
                    $scope.notCityStateSearch = true;
                }
                else {
                    $scope.notZipSearch = false;
                    $scope.notCityStateSearch = false;
                }
            }
            $scope.close = function() {
            $location.path('/itemDetail');
    }
            
        $scope.search = function () {
            
            if ($scope.itemLocateZip == undefined) { $scope.itemLocateZip = "" }
            if ($scope.itemLocateCity == undefined) { $scope.itemLocateCity = "" }
            if ($scope.itemLocateRadius == undefined) { $scope.itemLocateRadius = "" }
            if ($scope.itemLocateState == undefined) { $scope.itemLocateState = "" }
            if ($scope.posParam.storeNumber == undefined) { $scope.posParam.storeNumber = "" }
            if (($scope.itemLocateCity == "" || $scope.itemLocateState == "") && ($scope.itemLocateZip == "")) {
                swal({ title: "Error!", text: "Must enter a Zip Code or a combination of City and State", showConfirmButton: true });
            } else {

                $itemLocate.locate($scope.itemLocateZip, $scope.itemLocateCity, $scope.itemLocateRadius, $scope.itemLocateState, $scope.currentItem.sku, $scope.posParam.storeNumber).then(function (response) {
                    $scope.Stores = response

                    if (!angular.isArray($scope.Stores) || $scope.Stores.length < 1) {
                        $scope.noNodesToDisplay = true;
                    } else {
                        $scope.noNodesToDisplay = false;
                    }

                }, function (data) {
                    swal({ title: "Error!", text: data, showConfirmButton: true });
                });
            }
            }
           

                $scope.radiusOptions = [
                { name: '25 Miles', radius: '25' },
                { name: '50 Miles', radius: '50' },
                { name: '75 Miles', radius: '75' },
                { name: '100 Miles', radius: '100' }
                ];
                
        

                $scope.stateOptions = [
                { name: '', state: '' },
                { name: 'Alabama', state: 'AL' },
                { name: 'Alaska', state: 'AK' },
                { name: 'Arizona', state: 'AZ' },
                { name: 'Arkansas', state: 'AR' },
                { name: 'California', state: 'CA' },
                { name: 'Colorado', state: 'CO' },
                { name: 'Connecticut', state: 'CT' },
                { name: 'Delaware', state: 'DE' },
                { name: 'District of Columbia', state: 'DC' },
                { name: 'Florida', state: 'FL' },
                { name: 'Georgia', state: 'GA' },
                { name: 'Hawaii', state: 'HI' },
                { name: 'Idaho', state: 'ID' },
                { name: 'Illinois', state: 'IL' },
                { name: 'Indiana', state: 'IN' },
                { name: 'Iowa', state: 'IA' },
                { name: 'Kansas', state: 'KS' },
                { name: 'Kentucky', state: 'KY' },
                { name: 'Louisiana', state: 'LA' },
                { name: 'Maine', state: 'ME' },
                { name: 'Maryland', state: 'MD' },
                { name: 'Massachusetts', state: 'MA' },
                { name: 'Michigan', state: 'MI' },
                { name: 'Minnesota', state: 'MN' },
                { name: 'Mississippi', state: 'MS' },
                { name: 'Missouri', state: 'MO' },
                { name: 'Montana', state: 'MT' },
                { name: 'Nebraska', state: 'NE' },
                { name: 'Nevada', state: 'NV' },
                { name: 'New Hampshire', state: 'NH' },
                { name: 'New Jersey', state: 'NJ' },
                { name: 'New Mexico', state: 'NM' },
                { name: 'New York', state: 'NY' },
                { name: 'North Carolina', state: 'NC' },
                { name: 'North Dakota', state: 'ND' },
                { name: 'Ohio', state: 'OH' },
                { name: 'Oklahoma', state: 'OK' },
                { name: 'Oregon', state: 'OR' },
                { name: 'Pennsylvania', state: 'PA' },
                { name: 'Rhode Island', state: 'RI' },
                { name: 'South Carolina', state: 'SC' },
                { name: 'South Dakota', state: 'SD' },
                { name: 'Tennessee', state: 'TN' },
                { name: 'Texas', state: 'TX' },
                { name: 'Utah', state: 'UT' },
                { name: 'Vermont', state: 'VT' },
                { name: 'Virginia', state: 'VA' },
                { name: 'Washington', state: 'WA' },
                { name: 'West Virginia', state: 'WV' },
                { name: 'Wisconsin', state: 'WI' },
                { name: 'Wyoming', state: 'WY' }
            ];


        
    }]);

