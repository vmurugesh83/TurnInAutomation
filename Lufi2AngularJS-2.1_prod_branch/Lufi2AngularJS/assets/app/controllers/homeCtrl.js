angular.module( 'home', ['ui.bootstrap'] )
    .controller( 'homeCtrl', ['$scope', 'announcements', '$modal', '$rootScope', 'itemSearch', 'numberOnlyKeyPressValidator', '$location', 'defaultUPCScanHandler', '$filter', 'POSService', function ( $scope, $announcements, $modal, $rootScope, $itemSearch, $numberOnlyKeyPressValidator, $location, defaultUPCScanHandler, $filter, $POSService )
    {
        $scope.showAnnouncement = false;
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

        var success = function ( data )
        {

            if ( data.length > 0 ){
                $scope.showAnnouncement = true;

                for ( var i = 0; i < data.length; i++ ) { 
                    announcement.key = data[i].key;
                    announcement.date = data[i].date;
                    announcement.detail = data[i].header;
                    announcement.expDate = data[i].expDate;
                    announcement.locations = data[i].locations;
                    announcement.resolutionDate = data[i].resolutionDate;

                    //first Push
                    announcements.push( announcement );
                    announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: "", resolutionDate: ""};

                    announcement.key = data[i].key;
                    announcement.date = "";
                    announcement.detail = data[i].detail;
                    announcement.expDate = data[i].expDate;
                    announcement.locations = data[i].locations;
                    announcement.resolutionDate = data[i].resolutionDate;

                    //Second Push
                    announcements.push( announcement );
                    announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: "", resolutionDate: "" };
                }
            }

            $scope.announcements = $filter( 'orderBy' )( announcements, ['+resolutionDate', '-key', '-date'] );
            
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

       
        $rootScope.$on( 'ShowAnnouncement', function ()
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

        $scope.searchUPC = function ()
        {
            $itemSearch.searchUPC( $scope.upc, function ( response )
            {
                if ( response.ngroups == 0 )
                {
                    jQuery( '.not-found-alert' ).modal( 'show' );
                } else if ( response.isnGroup.length == 1 )
                {
                    $itemSearch.setSelectedISNGroup( response.isnGroup[0] );
                    $location.path( '/itemDetail' );
                } else
                {
                    $location.path( "/itemResults" );
                }
            }, function ( err )
            {
                alert( err );
            } );
        }

        var deregister = $rootScope.$on( 'Scanner_Event', defaultUPCScanHandler );

        $scope.$on( "$destroy", function ()
        {
            deregister();
        } );


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

