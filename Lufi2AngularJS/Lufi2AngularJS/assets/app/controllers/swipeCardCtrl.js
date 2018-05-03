angular.module('swipeCard', [])
    .controller('swipeCardCtrl', ['$scope', '$rootScope', '$modalInstance', 'msrService', 'alertMessages',function ($scope, $rootScope, $modalInstance, $msrService, $alertMessages) {
        var cardType;
        var msrEvent = $rootScope.$on('MSR_Event', function (event, msrData) {
            var returnVar = { msrData: msrData, cardType: cardType };
            $modalInstance.close(returnVar);


            var card = {
                cardType: cardType,
                msrData: msrData
            }

            if (cardType == "AssociateDiscountCard") {
                $msrService.accCard = card;
                $msrService.lastCard = undefined;
            } else {
                $msrService.lastCard = card;
                $msrService.accCard = {};
            }
        });

        //var msrErrorEvent = $rootScope.$on('MSR_Event_Error', function (event, msrData) {
        //    swal({ title: $alertMessages.paymentMessages.CARD_ERROR, text: $alertMessages.paymentMessages.INVALID_CARD_TYPE, showConfirmButton: true });
        //    $modalInstance.dismiss('msrError');
        //});
        var msrErrorEvent = $rootScope.$on('MSR_Event_Error', function (event, msrData) {

            if ((/^(MSR_ERR_ENABLE|MSR_ERR_TOKEN|MSR_ERR_STAND_IN)$/i).test(msrData.StatusCode)) {
                //accept as Vantiv is down
                 $modalInstance.dismiss('msrError'); 
                swal({ title: $alertMessages.paymentMessages.BANK_CARD_SYSTEM_OFFLINE_TITLE, text: $alertMessages.paymentMessages.BANK_CARD_SYSTEM_OFFLINE_MESSAGE, showConfirmButton: true });

            } else if ((/^MSR_ERR_DECLINE$/i).test(msrData.StatusCode) || (/.*ResponseCode_DeclineHard.*/i).test(msrData.StatusText)) {
                $modalInstance.dismiss('msrError'); 
                swal({ title: "Card Error", text: msrData.StatusText, showConfirmButton: true });

            } else if ((/^MSR_ERR_NOT_ENABLED$/i).test(msrData.StatusCode) || (/MSR is not enabled/i).test(msrData.StatusText)) {
                //ignore

            } else if ((/^(MSR_ERR_NOT_REQUESTED|MSR_ERR_INVALID_TYPE)$/i).test(msrData.StatusCode) || (/Card swiped does not match requested tender type/i).test(msrData.StatusText)) {
                 $modalInstance.dismiss('msrError'); 
                swal({ title: "Card Error", text: $alertMessages.paymentMessages.INVALID_CARD_TYPE, showConfirmButton: true });

            } else if ((/^MSR_ERR_UNKNOWN_TYPE$/i).test(msrData.StatusCode)) {
                 $modalInstance.dismiss('msrError'); 
                swal({ title: "Card Error", text: $alertMessages.paymentMessages.UNKNOWN_CARD_TYPE, showConfirmButton: true });

            }
            else {
                $modalInstance.dismiss('msrError');
                swal({ title: "MSR Error", text: msrData.StatusText, showConfirmButton: true });         
            }
        });


        $scope.$on("$destroy", function () {
            msrEvent();
            msrErrorEvent();
        });

        $scope.disableSwipeCard = function (isIgnoreEnabled) {
            isIgnoreEnabled = isIgnoreEnabled === true ? true : false;

            $msrService.disableMSR(isIgnoreEnabled).then(
                function () {
                    $scope.showSwipping = false;
                    cardType = undefined;
                },
                function () {
                    $scope.showSwipping = false;
                    cardType = undefined;
                });
        };
        $scope.isProcessingMSREnable = false;

        $scope.swipeCard = function (swipeCardType) {
            cardType = swipeCardType;
            $scope.isProcessingMSREnable = true;
            $msrService.enableMSR(cardType).then(
                function () {
                    $scope.showSwipping = true;
                    $scope.isProcessingMSREnable = false;
                }, function () {
                    $scope.showSwipping = false;
                    $scope.isProcessingMSREnable = false;
                });
        }

        $scope.close = function () {
            if ($scope.isProcessingMSREnable || $scope.showSwipping) {
                $scope.disableSwipeCard(true);
            }
            $modalInstance.dismiss('closed');
        };
    }]);

