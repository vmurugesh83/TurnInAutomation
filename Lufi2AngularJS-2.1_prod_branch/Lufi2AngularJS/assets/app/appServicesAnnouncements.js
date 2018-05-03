var appServices = angular.module( 'appServicesAnnouncements', [] );
appServices.factory( 'announcements', ['$http', '$cacheFactory', '$filter', 'sendSMTPErrorEmail', '$q', function ( $http, $cacheFactory, $filter, $sendSMTPErrorEmail, $q )
{

    var _allColumnsAreValid = false;
    var announcements = [];
    var currentDate = new Date()
    var selectedNodes = [];
    var matchCnt= 0;

    var requestArray = [];
    var request = {
        "GetExceptionListReq": {
            "input": {
                "ComplexQuery": {
                    "And": {
                        "Or": {
                            "Exp": [
                               {
                                    "_Name": "ShipNode",
                                    "_Value": "",
                                    "_QryType": "EQ"
                               },
                               {
                                   "_Name": "ShipNode",
                                   "_Value": "",
                                   "_QryType": "EQ"
                               }
                            ]
                        }
                    },
                    "_Operator": "AND"
                },
                "_ActiveFlag": "Y",
                "_DisplayLocalizedFieldInLocale": "en_US_EST",
                "_ExceptionType": "YCD_ANNOUNCEMENT",
                "_MaximumRecords": "",
                "_Priority": "1",
                "_ResolutionDate": "",
                "_ResolutionDateQryType": "GT",
                "_Status": "CLOSED",
                "_StatusQryType": "NE",
                "_SubscribedQueues": "N",
                "_EnterpriseKey": "DEFAULT"
            }
        }
    }

    var createRequest = {
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

    var announcement = {
        key: '',
        date: '',
        header: '',
        detail: '',
        expDate: '',
        locations: '',
        resolutionDate: ""
    }

    function formatDate( pDate )
    {
        pDate = ( pDate.getMonth() + 1 ) + "-" + pDate.getDate() + "-" + pDate.getFullYear() + " " + pDate.getHours() + ":" + pDate.getMinutes() + ":" + pDate.getSeconds() + ".0";
        return pDate;
    }

    function addDays( theDate, myDays )
    {
        currentdate = new Date( theDate.getTime() + myDays * 24 * 60 * 60 * 1000 );
        currentdate = formatDate( currentdate );
        return currentdate;
    }

    var populateRequest = function ( param )
    {
       request.GetExceptionListReq.input.ComplexQuery.And.Or.Exp[0]._Value = param;
    }

    var truncateAnnouncementReq = function ()
    {
        if ( angular.isDefined( announcements && announcements.length > 0 ) )
        {
            for ( var i = ( announcements.length - 1 ) ; i >= 0 ; i-- )
            {
                announcements.splice( i, 1 );
            }
        }
        announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: "", resolutionDate: "" };
    }

    var foundMatch = function ( locations, location )
    {
        var match = false;
        for ( var i = 0; i < locations.length; i++ )
        {
           
            if ( locations[i].toString().trim() === location.toString().trim() )
            {
                matchCnt = matchCnt + 1;
                match = true;
                break;
            }
        }
        return match
    }

    var retrieve = function ( param, caller, success, error )
    {
        var tempParamArray = [];
        var emptyArray = [];
        if ( angular.isArray( param ) )
        {
            tempParamArray = param;
        }
        else
        {
            tempParamArray[0] = param;
        }
        var localArray = [];
        populateRequest( param );
        var isSelectedAll = false;

        for ( var i = 0; i < tempParamArray.length; i++ )
        {
            if ( tempParamArray[i].toString().trim() == "" )
            {
                isSelectedAll = true;
                break;
            }
        }

        $http.post( getAnnouncementUrl, request ).success( function ( response )
        {
            for ( var i = 0; i < response.GetExceptionListRes.return.length; i++ )
            {
                var tempKey = response.GetExceptionListRes.return[i]._InboxKey;
                var tempDate = $filter( 'date' )( new Date( response.GetExceptionListRes.return[i]._GeneratedOn ), 'MM/dd/yyyy HH:mm:ss' );
                var tempHeader = response.GetExceptionListRes.return[i]._Description;
                var tempDetail = response.GetExceptionListRes.return[i]._DetailDescription;

                var tempExpDate = addDays( new Date(), response.GetExceptionListRes.return[i]._ExpirationDays );
                var tempLocations = response.GetExceptionListRes.return[i]._ShipnodeKey;
                var resolutionDate = response.GetExceptionListRes.return[i]._ResolutionDate;

                resolutionDate = ( caller === "MANAGER" ) ? $filter( 'date' )( new Date( resolutionDate ), 'yyyy-MM-dd' ) : $filter( 'date' )( new Date( resolutionDate.replace( /-/g, '\/' ).replace( /T.+/, '' ) ), 'yyyy-MM-dd' );
                //resolutionDate = $filter( 'date' )( new Date( resolutionDate.replace( /-/g, '\/' ).replace( /T.+/, '' ) ), 'yyyy-MM-dd');
             
                announcement = { key: tempKey, date: tempDate, header: tempHeader, detail: tempDetail, expDate: tempExpDate, locations: tempLocations, resolutionDate: resolutionDate };

                localArray.push( announcement );

                announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: "", resolutionDate: "" };

            }

            if ( localArray.length > 0 )
            {
             
                for ( var p = 0; p < localArray.length; p++ )
                {
                    var arrResDate = new Date( localArray[p].resolutionDate );
                    var todaysDate = new Date();
                    var compare = ( todaysDate > arrResDate );

                    //Check if resolutionDate is less than today
                    if ( compare === true )
                    {
                        continue;
                    }
                    else if ( caller === "MANAGER" )
                    {
                        if ( localArray[p].locations.toString().trim() == "" )
                        {
                            if ( isSelectedAll )
                            {
                                announcements.push( localArray[p] );
                            }
                        }
                        else
                        {
                            if (foundMatch( tempParamArray, localArray[p].locations.toString().trim() ))
                            {
                                announcements.push( localArray[p] );
                            }

                        }

                    }
                    else if ( caller === "HOME" )
                    {

                        if (foundMatch(tempParamArray, localArray[p].locations.toString().trim()) || (localArray[p].locations.toString().trim() === "")) 
                        {
                            announcements.push( localArray[p] );
                        }
                          
                    }
                }
            }

            success( announcements );
            truncateAnnouncementReq();
            localArray = emptyArray;
            response = null;

        }, function ( response )
        {
            $sendSMTPErrorEmail( response, getUrl, request );

            error( response );
        } );
    }

    var addException = function ( request, stores, success, error )
    {
        if ( !angular.isDefined( stores ) )
        {
            stores = [];
            stores[0] = "";
        }
        var cntLoop = 0;
        for ( var p = 0; p < stores.length; p++ )
        {

            var newReq = {
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
            if ( stores[p] === "All Stores" )
            {
                newReq.CreateExceptionReq.input._ShipnodeKey = "";
            }
            else
            {
                newReq.CreateExceptionReq.input._ShipnodeKey = stores[p];
            }

            newReq.CreateExceptionReq.input._Description = angular.copy( request.CreateExceptionReq.input._Description );
            newReq.CreateExceptionReq.input._DetailDescription = angular.copy( request.CreateExceptionReq.input._DetailDescription );
            newReq.CreateExceptionReq.input._GeneratedOn = angular.copy( request.CreateExceptionReq.input._GeneratedOn );
            newReq.CreateExceptionReq.input._ExpirationDays =  angular.copy(request.CreateExceptionReq.input._ExpirationDays);
            newReq.CreateExceptionReq.input._ResolutionDate =  angular.copy(request.CreateExceptionReq.input._ResolutionDate);
            //request = newReq;

            $http.post(createAnnouncementUrl, newReq ).success( function ( response )
            {
                var tempKey = response.CreateExceptionRes.return._InboxKey;
                var tempDate = $filter( 'date' )( new Date( response.CreateExceptionRes.return._GeneratedOn ), 'MM/dd/yyyy HH:mm:ss' );
                var tempHeader = response.CreateExceptionRes.return._Description;
                var tempDetail = response.CreateExceptionRes.return._DetailDescription;
                var tempExpDays = addDays( new Date(), response.CreateExceptionRes.return._ExpirationDays );
                var tempLocations = response.CreateExceptionRes.return._ShipnodeKey;
                var tempResDate = response.CreateExceptionRes.return._ResolutionDate;

                if ( angular.isDefined(tempResDate) ){
                    tempResDate = tempResDate.getFullYear() + "-" + ( tempResDate.getMonth() + 1 ) + "-" + tempResDate.getDate();
                }

                announcement = { key: tempKey, date: tempDate, header: tempHeader, detail: tempDetail, expDate: tempExpDays, locations: tempLocations, resolutionDate: tempResDate};
                announcements.push( announcement );

                //empty out
                announcement = { key: "", date: "", header: "", detail: "", expDate: "", locations: "", resolutionDate: "" };

                cntLoop = ( cntLoop + 1 )
                if ( cntLoop === stores.length )
                {
                    success( announcements );
                    truncateAnnouncementReq();
                }

            } ).error( function ( response )
            {
                $sendSMTPErrorEmail( response, createUrl, newReq );
                error( response );
            } )
        }
    }

    var node = {
        key: '',
        type: '',
        orgName: '',
        descrip: ""
    }
    var populateStoreList = function ( param, success, error )
    {
        var request = {
            "getShipNodeList": {
                "input": {
                    "_ShipnodeKey": ""
                }
            }
        }

        $http.post( getNodeListUrl, request ).success( function ( response )
        {
            for ( var i = 0; i < response.GetShipNodeListResp.ShipNodeList.length; i++ )
            {
                var nodeKey = response.GetShipNodeListResp.ShipNodeList[i]._ShipnodeKey;
                var nodeType = response.GetShipNodeListResp.ShipNodeList[i]._NodeType;
                var nodeOrgName = response.GetShipNodeListResp.ShipNodeList[i].OwnerOrganization._OrganizationName;
                var nodeDescrip = response.GetShipNodeListResp.ShipNodeList[i]._Description;

                if ( nodeType == 'Store' || nodeType == 'StoreFulfillment' )
                {
                    node = { key: nodeKey, type: nodeType, orgName: nodeOrgName, descrip: nodeDescrip };
                    selectedNodes.push( node );
                }

            }

            success( selectedNodes );

            var emptyArray = [];
            response = emptyArray;

        }, function ( response )
        {
            $sendSMTPErrorEmail( response, nodeUrl, request );

            error( response );
        } );

    }

    var canManageAnnouncements = function ()
    {
        return $securityService.canManageAnnouncements();
    }

    function isDate( myDate )
    {
        return ( myDate.constructor.toString().indexOf( "Date" ) > -1 );
    }
    function isValidDate( value )
    {
        var dateWrapper = new Date( value );
        return !isNaN( dateWrapper.getDate() );
    }

    var allValidated = function ( data )
    {
        var valid = true;
        var errorMessage = { message: 'Announcements ' };
        var additionalMsg = "";

        var expDays = data.createExceptionRequest.CreateExceptionReq.input._ExpirationDays;
        var header = data.createExceptionRequest.CreateExceptionReq.input._Description;
        var detail = data.createExceptionRequest.CreateExceptionReq.input._DetailDescription;
        var shipnode = data.createExceptionRequest.CreateExceptionReq.input._ShipnodeKey;

        if ( !angular.isDefined( expDays ) || isNaN( expDays ) )
        {
            valid = false;
            additionalMsg = " Expiration Date is not valid.";
            return valid;
        }
        if ( !angular.isDefined( header ) || header == null || header.toString().trim() == "" )
        {
            valid = false;
            additionalMsg = " header is not valid.";
            return valid;
        }
        if ( !angular.isDefined( detail ) || detail == null || detail.toString().trim() == "" )
        {
            valid = false;
            additionalMsg = " detail is not valid.";
            return valid;
        }

        errorMessage.message += additionalMsg;
        return valid;
    }

    var success = function ( data )
    {

        return data;
    };
    var error = function ( errorData )
    {
    };

    var save = function ( data )
    {
        if ( angular.isDefined( data ) )
        {
            if ( allValidated( data ) )
            {
                addException( data, success, error );
            }
        }

    };

    return {

        addDays: addDays,
        retrieve: retrieve,
        addException: addException,
        populateStoreList: populateStoreList,
        canManageAnnouncements: canManageAnnouncements,
        isDate: isDate,
        isValidDate: isValidDate,
        allValidated: allValidated,
        save: save
    }

}] );