angular.module( 'manageAnnouncement', ['ui.bootstrap'] )
    .controller( 'announcementCtrl', ['$scope', 'announcements', '$location', '$filter', '$modal', function ( $scope, $announcements, $location, $filter, $modal )
    {
    
        $scope.refreshIScroll = function ()
        {
            //var myScroll = new IScroll( '#containerEasyScroller', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' } );
            setTimeout( function ()
            {
                //myScroll.refresh();
            }, 500 );
        };

        $scope.hideColumns = false;
        $scope.queryParameters = {}
        $scope.announcements = []
        $scope.selectedNodes = [];
        $scope.allStoresSelected = true;
        var storeId;
        var caller = "MANAGER";
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

        var selectionsHolder = [];
        var data = [];
        var data1 = [];
        var data2 = [];
        var data3 = [];
        var errorData1 = "";
        var errorData2 = "";
        var errorData3 = "";
        var data4 = [];
        var errorData4 = "";
        var param = "";

        var createExceptionRequest = {
            "CreateExceptionReq": {
                "input": {
                    "_AutoResolvedFlag": "Y",
                    "_Description": "",
                    "_DetailDescription": "",
                    "_DisplayLocalizedFieldInLocale": "en_US_EST",
                    "_EnterpriseKey": "DEFAULT",
                    "_ExceptionType": "YCD_ANNOUNCEMENT",
                    "_GeneratedOn": "",
                    "_ExpirationDays": "",
                    "_ListDescription": "",
                    "_Priority": "1",
                    "_QueueKey": "DEFAULT_Q12",
                    "_ResolutionDate": "",
                    "_ShipnodeKey": ""
                }
            }
        }

        var success1 = function ( data1 )
        {
            var compareData = angular.copy( data1 );
            var finalData = [];
            for ( var i = 0; i < data1.length; i++ )
            {
                var key = data1[i].key;
                var date = data1[i].date;
                var header = data1[i].header;
                var detail = data1[i].detail;
                var expDate = data1[i].expDate;
                var locations = data1[i].locations;
                var resolutionDate = data1[i].resolutionDate;

                announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: "", resolutionDate: "" };

                if (finalData.length==0){
                    announcement = { key: key, date: date, header: header, detail: detail, expDate: expDate, locations: locations, resolutionDate: resolutionDate };
                    finalData.push( announcement );
                }
                else{
                    for ( var k = 0; k < finalData.length; k++ )
                    {
                        if ( key == finalData[k].key )
                        { 
                            break;
                        }
                        else
                        {
                            if ( ( k + 1 ) == finalData.length )
                            {
                                announcement = { key: key, date: date, header: header, detail: detail, expDate: expDate, locations: locations, resolutionDate: resolutionDate};
                                finalData.push( announcement );
                            }
                            else
                            {
                                continue;
                            }

                        }
                        
                    }

                }
               
            }

            $scope.announcements = $filter( 'orderBy' )( finalData, '-resolutionDate');
            var emptyArray = [];
            finalData = emptyArray;
        };
        var error1 = function ( errorData1 )
        {
        };
        var success2 = function ( data2)
        {
            $scope.selectedNodes = $filter( 'orderBy' )( data2, '+key' );
            //$scope.refreshIScroll();
        };
        var error2 = function ( errorData2 ){
        };
        var success3 = function (data3)
        {
            $scope.announcements = $filter( 'orderBy' )( data3, '-resolutionDate' );
        };
        var error3 = function ( errorData3 ){
        };

        var success4 = function (data4)
        {
            var emptyArray = [];
            $scope.header = "";
            $scope.detail = "";
            $scope.expDate = "";

            //$scope.announcements = data4;
            if ( angular.isArray( data4 ) )
            {
                for ( var i = 0; i < data4.length; i++ )
                {
                    selectionsHolder[i]=data4[i].locations;        
                }
            }
            else
            {
                selectionsHolder[0] = data4.locations
            }
            $announcements.retrieve( selectionsHolder, caller, success1, error1 )
            selectionsHolder = emptyArray;

        };
        var error4 = function ( errorData4 )
        {
        };
        var error4 = function ( errorData4 ){
        };

        $announcements.populateStoreList( param, success2, error2 );
        $announcements.retrieve( param, caller, success1, error1 );
        
        function showDeleteDialog(item)
        {
            var modalInstance = $modal
                    .open( {
                        templateUrl: 'deleteConfirmationDialog.html',
                        controller: 'announcementDeleteCtrl'
                    } );

            modalInstance.result.then( function ()
            {
                //do delete here
                $scope.announcements.splice(item, 1 );
            }, function ()
            {
                //cancelled
            } );
        };

        $scope.deleteItem = function ( item )
        {
            showDeleteDialog( item );
        }

        var addDays = function ( theDate, myDays )
        { 
            var expDate = new Date( theDate.getTime() + myDays * 24 * 60 * 60 * 1000 );
            expDate = expDate.getFullYear() + "-" + ( ( ( expDate.getMonth() + 1 ) < 10 ) ? ( "0" + ( expDate.getMonth() + 1 ) ) : ( expDate.getMonth() + 1 ) ) + "-" + ( ( expDate.getDate() < 10 ) ? ( "0" + expDate.getDate().toString() ) : expDate.getDate() ) ;
            /*expDate = expDate.getFullYear() + "-" + (( ( expDate.getMonth() + 1 ) < 10 ) ? ( "0" + (expDate.getMonth() + 1) ):( expDate.getMonth() + 1 ) )  + "-" 
                + ( ( expDate.getDate() < 10 ) ? ( "0" + expDate.getDate().toString() ) : expDate.getDate() ) + " " + ( ( new Date().getHours() < 10 ) ? ( "0" + new Date().getHours().toString() ) : new Date().getHours() ) + ":" + ( ( new Date().getMinutes() < 10 ) ? ( "0" + new Date().getMinutes().toString() ) : new Date().getMinutes() ) + ":" + ( ( new Date().getSeconds() < 10 ) ? ( "0" + new Date().getSeconds().toString() ) : new Date().getSeconds() ) + ".0";*/
            return expDate;
        
        }
       
        var getExpirationDate = function ( theDate, myDays )
        {
            var expDate = new Date( theDate.getTime() + myDays * 24 * 60 * 60 * 1000 );
            expDate = expDate.getFullYear() + "-" + ( expDate.getMonth() + 1 ) + "-" + expDate.getDate();
            return expDate;
        }

        $scope.save = function ()
        {
            var parsedDate = addDays( $scope.expDate, 1 );
            createExceptionRequest.CreateExceptionReq.input._Description = $scope.header;
            createExceptionRequest.CreateExceptionReq.input._DetailDescription = $scope.detail;
            createExceptionRequest.CreateExceptionReq.input._GeneratedOn = new Date();

           var testDate =   new Date(getExpirationDate( $scope.expDate, 2 )) ;

           var expirationDays = ( getExpirationDays( testDate)).toString();

            createExceptionRequest.CreateExceptionReq.input._ExpirationDays = expirationDays;
            createExceptionRequest.CreateExceptionReq.input._ResolutionDate = parsedDate;

            $announcements.addException( createExceptionRequest, $scope.displayFor, success4, error4 );

        }

        $scope.close = function ()
        {
            open('http://saphere-solutions.com/?exit', '_self' ).close();

            //setTimeout( function ()
            //{
                //window.close( 'exit' );
                //location.href = 'http://saphere-solutions.com/?exit';
           // }, 100 );

        }

        Date.prototype.addHours = function ( h )
        {
            this.setHours( this.getHours() + h );
            return this;
        }
        function getExpirationDays( endDate )
        {
            startDate = new Date();
            // adjust diff for for daylight savings
            var hoursToAdjust = Math.abs( startDate.getTimezoneOffset() / 60 ) - Math.abs( endDate.getTimezoneOffset() / 60 );

            // apply the tz offset
            endDate.addHours( hoursToAdjust );

            // The number of milliseconds in one day
            var ONE_DAY = 1000 * 60 * 60 * 24

            // Convert both dates to milliseconds
            var startDate_ms = startDate.getTime()
            var endDate_ms = endDate.getTime()

            // Calculate the difference in milliseconds
            var difference_ms = Math.abs( startDate_ms - endDate_ms )

            // Convert back to days and return
            return Math.round( difference_ms / ONE_DAY )

        }

        Date.prototype.today = function () { 
            return ((this.getDate() < 10)?"0":"") + this.getDate() +"/"+(((this.getMonth()+1) < 10)?"0":"") + (this.getMonth()+1) +"/"+ this.getFullYear();
        }

        Date.prototype.addHours= function(h){
            this.setHours(this.getHours()+h);
            return this;
        }

        function getUnitNode( node, array )
        {
            for ( var i = 0; i < array.length; i++ )
            {
                if ( node === array[i] ) return node.key;
            }
            return "";
        }

        var truncateScopeAnnouncements = function(){
            if ( angular.isDefined( $scope.announcements && $scope.announcements.length > 0 ) )
            {
                for ( var i = ($scope.announcements.length - 1); i >= 0 ; i-- )
                {
                    $scope.announcements.splice(i, 1);
                }
            }       
        }

       
        $scope.nodeSelectionChanged = function ()
        {
            truncateScopeAnnouncements();
            var emptyArray = [];
            for ( var i = 0; i < $scope.displayFor.length; i++){
                if ( $scope.displayFor[i] == "All Stores" )
                {
                    selectionsHolder.push( "" );
                    //$announcements.retrieve( "", caller, success1, error1);
                   // break;
                }
                else
                {
                    selectionsHolder.push( $scope.displayFor[i] );
                }         
            }

            if ( angular.isDefined( selectionsHolder ) && selectionsHolder.length > 0)
            {
                $announcements.retrieve( selectionsHolder, caller, success1, error1 )
            }
            selectionsHolder = emptyArray;
        };

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

