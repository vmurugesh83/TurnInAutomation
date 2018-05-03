angular.module( 'home', ['ui.bootstrap'] )
    .controller('homeCtrl', ['$scope', 'announcements', '$modal', 'itemSearch', 'numberOnlyKeyPressValidator', 'defaultUPCScanHandler', '$filter', 'POSService', 'order', 'loggerService', '$state', 'securityService', '$timeout',
        function ($scope, $announcements, $modal, $itemSearch, $numberOnlyKeyPressValidator, defaultUPCScanHandler, $filter, $POSService, $order, loggerService, $state, securityService, $timeout) {

            $scope.isCsr = function () { return securityService.isCsr(); };

            $scope.refreshIScroll = function () {
                setTimeout(function () {
                    $scope.$parent.myScroll['AnnouncementWrapper'].refresh();
                }, 500);
            };

            $scope.upcPaste = function ($event) {
                if (typeof $event.originalEvent.clipboardData !== "undefined") {
                    handlePastedUPC($event.originalEvent.clipboardData.getData('text/plain'));
                } else { // To support browsers without clipboard API (IE and older browsers)
                    $timeout(function () {
                        handlePastedUPC(angular.element($event.currentTarget).val());
                    }, 2);
                }
            };

            var handlePastedUPC = function (pastedText) {
                var cleansedText = "";


                for (var i = 0; i < pastedText.length; i++) {
                    if ((/\d/).test(pastedText[i])) {
                        cleansedText += pastedText[i];
                    }
                }
                $timeout(function () {
                    $scope.upc = cleansedText;
                }, 3);
            };

            $scope.orderNumberPaste = function ($event) {
                if (typeof $event.originalEvent.clipboardData !== "undefined") {
                    handlePastedOrderNo($event.originalEvent.clipboardData.getData('text/plain'));
                } else { // To support browsers without clipboard API (IE and older browsers)
                    $timeout(function () {
                        handlePastedOrderNo(angular.element($event.currentTarget).val());
                    }, 2);
                }
            };

            var handlePastedOrderNo = function (pastedText) {
                var cleansedText = "";


                for (var i = 0; i < pastedText.length; i++) {
                    if ((/\d/).test(pastedText[i])) {
                        cleansedText += pastedText[i];
                    }
                }
                $timeout(function () {
                    $scope.formInput.orderNo = cleansedText;
                }, 3);
            };




        $scope.queryParameters = {};
        $scope.numberValidator = $numberOnlyKeyPressValidator;
        $scope.announcements = [];
        $scope.hideColumns = false;
        var data = [];
        var errorData = "";
        var param = "";
        var caller = "HOME";
        var announcements = [];
        var announcement = {
            key: '',
            date: '',
            header: '',
            detail: '',
            expDate: '',
            locations: '',
            resolutionDate: ''
        }

        $scope.formInput = {
            orderNo: ""
        };

        var success = function ( data )
        {

            if ( data.length > 0 ){

                //order data by Announcements -newest create Date first, then Bopis Pick & Restock oldest create date 1st
                var _sortCompareAnnouncements = function(a,b){
                    
                    var result = 0;

                    var bopisRegEx = /^(RESTOCK|PICKUP)$/i;
                    //var regularAnnouncementRegEx = /^YCD_ANNOUNCEMENT$/i;

                    if ((a.exceptionType === b.exceptionType) || (bopisRegEx.test(a.exceptionType) && bopisRegEx.test(b.exceptionType))) {
                        result = 0;
                    } else if (bopisRegEx.test(a.exceptionType)) {
                        result = 1;
                    }else{
                        result = -1;
                    }

                    if(result !== 0){
                        return result;
                    }

                    //if a and b are equal use appropriate date to sort them
                    //if a and b are bopis, sort Create Date, oldest first
                    if (bopisRegEx.test(a.exceptionType)) {
                        return a.date - b.date;
                    }
                        //else a and b are regular YCD_ANNOUNCEMENT so sort by Create Date NEWEST first
                    else{
                        return b.date - a.date;
                    }
                };

                data.sort(_sortCompareAnnouncements);

                for ( var i = 0; i < data.length; i++ ) { 
                    announcement.key = data[i].key;
                    announcement.date = data[i].date;
                    announcement.detail = data[i].header;
                    announcement.expDate = data[i].expDate;
                    announcement.locations = data[i].locations;

                    //first Push
                    announcements.push( announcement );
                    announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: ""};


                    if (data[i].detail != null && data[i].detail != undefined && data[i].detail != "") {
                    announcement.key = data[i].key;
                    announcement.date = "";
                    announcement.detail = data[i].detail;
                    announcement.expDate = data[i].expDate;
                    announcement.locations = data[i].locations;
                    //Second Push
                    announcements.push( announcement );
                    announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: ""};
                }
                }
            }

            $scope.announcements = announcements;
            
        };

        var error = function ( errorData ) {
            
        };


        if ( angular.isDefined( $POSService ) )
        {
            var posParams = $POSService.getPOSParameters();
            var storeNum = posParams.storeNumber;
            var assocId = posParams.associateId;
            if ( storeNum !== "" & assocId !== "" )
            {
                $announcements.retrieve( storeNum, caller, success, error );
            }
        }

       
        $scope.$on( 'ShowAnnouncement', function ()
        {
            var posParams = $POSService.getPOSParameters();
            storeNumber = posParams.storeNumber;
            $announcements.retrieve( storeNumber, caller, success, error );
       } );

        $scope.openAnnouncementDialog = function ()
        {
            var modalInstance = $modal
                    .open( {
                        templateUrl: 'manageAnnouncements.html',
                        controller: 'announcementCtrl',
                        size: 'lg'
                    } );

            modalInstance.result.then( function ()
            {
            }, function ()
            {

            } );
        };

        $scope.searchUPC = function () {
            $itemSearch.searchUPC($scope.upc, function (response) {
                if (response.ngroups == 0) {
                    jQuery('.not-found-alert').modal('show');
                } else if (response.isnGroup.length == 1) {
                    $itemSearch.setSelectedISNGroup(response.isnGroup[0]);
                    $state.go('itemDetail');
                } else {
                    $state.go('itemResults');
                }
            }, function (err) {
                alert(err);
            });
        };
        
        $scope.navigate = function (stateName) {
            $state.go(stateName);
        };

        $scope.openOrder = function () {
            if ($scope.formInput.orderNo) {
                param = {
                    "GetSterlingOrderListReq":
                        {
                            "_ReadFromHistory": "N",
                            "_OrderNo": $scope.formInput.orderNo,
                            "_DraftOrderFlag": ""
                        }
                };
                if (angular.isDefined($scope.formInput.orderNo)) {
                    $order.getOrderList(param, function (result) {
                        if (result === undefined) {
                            jQuery('.not-found-alert').modal('show');
                        } else {
                            $order.setSelectedOrder({ _OrderNo: $scope.formInput.orderNo, _DocumentType: "0001" });
                            $state.go('orderDetail');
                        }
                    }, function (error) { alert(error); });
                }
            }
        };

        var handleBarCodeScan = function (event, barcodeData) {
           
            if (!angular.isString(barcodeData)) {
                barcodeData = (!angular.isDefined(barcodeData) || barcodeData === null) ? '' : barcodeData.toString().trim();
           
            }
            if (barcodeData.length == 9){
                $scope.formInput.orderNo = barcodeData.substring(0, 9);
           
                $scope.openOrder();
            } else {
           
                defaultUPCScanHandler(event,barcodeData);
            }
        };

      
        var deregisterScanner = $scope.$on('Scanner_Event', handleBarCodeScan);
        $scope.$on("$destroy", function () {
            deregisterScanner();
        });


    }] )
    .controller( 'announcementDeleteCtrl', ['$scope', '$modalInstance',
    function ( $scope, $modalInstance )
    {

        $scope.yes = function ()
        {
            $modalInstance.close();
        };

        $scope.no = function ()
        {
            $modalInstance.dismiss( 'cancel' );
        };
    }] );

