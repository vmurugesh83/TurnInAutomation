angular.module('orderPickup', ['ui.bootstrap'])
   .controller('confirmPickupCtrl', ['$scope', '$modalInstance', '$modal', '$state', 'cart', 'pickUpInStore', 'POSService', 'PaymentTerminalService', '$timeout', 'sendSMTPErrorEmail', 'btProp',
       function ($scope, $modalInstance, $modal, $state, $cart, $pickUpInStore, $POSService, $PaymentTerminalService, $timeout, $sendSMTPErrorEmail, $btProp) {
       $scope.pickup = {};
       $scope.pickup.govtId = false;
       $scope.pickup.orderConfirmEmail = false;
       $scope.pickup.paymentTender = false;
       $scope.pickup.customerName = "";
       var errorMessage = "";
       $scope.disableConfirmButton = false;
       var isSignatureCaptured = false;
       var isRetalixReady = false;
       var isPaymentTerminalReleased = true;
       $scope.processingConfirm = false;
       $scope.processingConfirmSuccess = false;
       var isSignatureCapturedFailed = false;

        //check EPOS status
       $POSService.getPmmConfigAndStatus().then(function (data) {
           isRetalixReady = (/^\s*true\s*$/i).test(data.UseEPS) ? true : false;

           if (isRetalixReady) {
               claimSigCap();
           }
       }, function (data) {
           isRetalixReady = false;
           });

       $scope.openErrorModal = function (error) {
           $scope.disableConfirmButton = false;
           swal({title:"Pickup Confirm Error", text:error});

       };

       var EventHandler = function () {

           isPaymentTerminalReleased = true;
           $PaymentTerminalService.releasePaymentTerminal();

       }

       var lastPmmBopisError = null;

       var EventHandlerError = function (eventData) {

           isSignatureCapturedFailed = true;

           $PaymentTerminalService.releasePaymentTerminal();
           isPaymentTerminalReleased = true;

           if (!angular.isObject(eventData)) {
               eventDate = { StatusText: "There was an error.", eventData:eventData };
           }

           lastPmmBopisError = angular.copy(eventData);

           var errorString = "";

           if (angular.isString(eventData.StatusText)) {
               errorString += eventData.StatusText.trim();
           }

           if ((/^PMT_ERR_NOT_FUNCTIONAL$/i).test(eventData.StatusCode)) {
               swal({ title: "Pickup Confirm Error", text: "Signature capture is not functioning.  Please complete confirmation without Signature." });
               $sendSMTPErrorEmail(eventData, 'PMM - BOPIS Signature Capture Failure');
               //$scope.cancelConfirmPickup(false);

           } else if ((/^PMT_ERR_SIG_CUST_CANCEL$/i).test(eventData.StatusCode)) {
               swal({ title: "Pickup Confirm Error", text: "Signature capture was canceled by the customer.  Please retry." });
               isSignatureCapturedFailed = false;
               $scope.cancelConfirmPickup(false);
           }
           else if ((/^(PMT_ERR_CLAIMED|PMT_ERR_ENABLED)$/i).test(eventData.StatusCode)) {
               //this should never run because cases are now in PaymentTeminal service, because
               //    I can only get 1 error or success from a Promise, so I cannot ignore a failure and still take a success!
               //ignore these errors of already claimed and already enabled
               isSignatureCapturedFailed = false;
           }
            else if ((/^LUFI_BOPIS_TIMEOUT_ON_CAPTURE$/i).test(eventData.StatusCode))
           {
               swal({ title: "Pickup Confirm Error", text: errorString });
           
           }
           else if ((/^(LUFI_BOPIS_WEBSOCKET_DOWN|LUFI_BOPIS_TIMEOUT)$/i).test(eventData.StatusCode))
           {
                errorString += "  Please complete confirmation without Signature.";
                swal({ title: "Pickup Confirm Error", text: errorString });
           
           }
           else {
               errorString += "  Please complete confirmation without Signature.";
               swal({ title: "Pickup Confirm Error", text: errorString });
               $sendSMTPErrorEmail(eventData, 'PMM - BOPIS Signature Capture Failure');
           }

           
       };

       var claimSigCap = function () {

           isPaymentTerminalReleased = false;
           var associateId = $POSService.getPOSParameters().associateId;
           associateId = associateId ? associateId : "0";

           $PaymentTerminalService.getSignature($cart._OrderNo, associateId).then(
               function () {
                   //we got the signature
                   isSignatureCaptured = true;
                   EventHandler();
               },
               function (data) {
                   EventHandlerError(data);
               }
               );
       };

       $scope.checkedButtonFont = 'fa-check fa-2x';
       $scope.uncheckedButtonFont = 'fa-check fa-2x whiteText';
       $scope.checkboxClick = function (inputName) {
           $scope.pickup[inputName] = !$scope.pickup[inputName];
       };

       $scope.processConfirmPickup = function () {
           var idSelected = false;
           var errorMessage;
        
           if ($scope.pickup.orderConfirmEmail) {
               idSelected = true;
           } else if ($scope.pickup.govtId) {
               idSelected = true;
           } else if ($scope.pickup.paymentTender) {
               idSelected = true;
            
           }
           if (!idSelected) {
               errorMessage = "Please confirm the selection of the identification method that the customer presented.";
           }
           else if ($scope.pickup.customerName === undefined || $scope.pickup.customerName === null || $scope.pickup.customerName.toString().trim() == "") {
               errorMessage = "Please enter the customer's name.";
           }

           if (angular.isDefined(errorMessage) && errorMessage != "") {
               $scope.openErrorModal(errorMessage);
               return;
           } else {
               $scope.disableConfirmButton = true;
               var orderNo = $cart._OrderNo;
               var shipNode = $POSService.getPOSParameters().storeNumber;
               var IdType = "";
               var designee = "";
               if ($scope.pickup.govtId) {
                   IdType = "Government ID";
               }
               if ($scope.pickup.orderConfirmEmail) {
                   IdType = "Order Confirmation Email";
               }
               if ($scope.pickup.paymentTender) {
                   IdType = "Payment Tender";
               }
               if (!angular.isArray($cart.Shipments.Shipment)) {
                   $cart.Shipments.Shipment = [$cart.Shipments.Shipment];
               }

               //get first shipment that matches current Lufi Shipnode
               var shipmentKey = "";
               for (var it = 0; it < $cart.Shipments.Shipment.length; it++) {

                   //check shipNode as integer that it equals lufi store number
                   var currentShipmentShipNodeInt = parseInt($cart.Shipments.Shipment[it]._ShipNode);
                   if (isFinite(currentShipmentShipNodeInt) &&
                       (parseInt(shipNode) === currentShipmentShipNodeInt) && 
                       (/^\s*BOPIS\s*$/i).test($cart.Shipments.Shipment[it]._ShipmentType)) {
                       shipmentKey = $cart.Shipments.Shipment[it]._ShipmentKey;
                       break;
                   }
               }

               if (shipmentKey === null || shipmentKey === undefined || (shipmentKey.toString().trim().length < 1)) {

                   errorMessage = "No shipment in this order matches your current store number: " + shipNode;
                   $scope.openErrorModal(errorMessage);
                   return;
               }

               if (angular.isDefined($scope.pickup.customerName)) {
                   designee = $scope.pickup.customerName;
               }

               //check that signature was captured or EPOS is off
               if (!isRetalixReady || (isRetalixReady && (isSignatureCaptured || isSignatureCapturedFailed))) {

                   $scope.processingConfirm = true;

                   //if order is being confirmed without signature, email error
                   if (isRetalixReady && isSignatureCapturedFailed) {
                       // data, failedServiceURL, httpInputObj, headerString, additionalEmailRecipients 
                       $sendSMTPErrorEmail(lastPmmBopisError,
                           'PMM - BOPIS Confirmed without Signature',
                           null,
                           "Confirmed BOPIS OrderNo: " + orderNo + " without collecting a signature on a Retalix ready system.",
                           $btProp.getProp('bopisFailedSigCapEmailList'));
                   }

                   $pickUpInStore.changeShipmentStatus(orderNo, shipmentKey, designee, IdType, shipNode).then(
                       function () {
                           $scope.processingConfirmSuccess = true;
                           //place timeout to wait a little before closing.
                           $timeout(function () { $scope.modalCloseSuccess(); }, 3000);
                           
                       }, function () {
                           $scope.processingConfirm = false;
                           $scope.processingConfirmSuccess = false;
                           $scope.openErrorModal("Failed to update shipment status. Please try again.");
                       });

               } else {
                   $scope.openErrorModal("Please have the customer sign for the merchandise.");
               }
           }
       };

       $scope.cancelConfirmPickup = function (isSuccessfulConfirm) {
           if (!isPaymentTerminalReleased) {
               $PaymentTerminalService.releasePaymentTerminal();
               // $PaymentTerminalService.disablePaymentTerminal();
           }
           if (isSuccessfulConfirm) {
               $modalInstance.close();
           } else {
               $modalInstance.dismiss('closed');
           }
       };

       $scope.modalCloseSuccess = function () {
           $scope.cancelConfirmPickup(true);
       };
   }])
.controller('confirmPickupErrorCtrl', ['$scope', '$modalInstance', 'currentError', function ($scope, $modalInstance, currentError) {
    $scope.currentError = currentError;

    $scope.close = function () {
        $modalInstance.dismiss('closed');
    };

}]);
