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

            if ((/.*ResponseCode_DeclineHard.*/i).test(msrData.StatusText)) {
                $scope.$apply(function () { $modalInstance.dismiss('msrError'); });
                swal({ title: "Card Error", text: $alertMessages.paymentMessages.CC_DECLINED, showConfirmButton: true });

            } else if ((/MSR is not enabled/i).test(msrData.StatusText)) {
                //ignore

            } else if ((/Card swiped does not match requested tender type/i).test(msrData.StatusText)) {
                $scope.$apply(function () { $modalInstance.dismiss('msrError'); });
                swal({ title: "Card Error", text: $alertMessages.paymentMessages.INVALID_CARD_TYPE, showConfirmButton: true });

            } else if ((/The card swipe was cancelled by the associate/i).test(msrData.StatusText)) {
                //just fail for now.
                $scope.$apply(function () { $modalInstance.dismiss('msrError'); });
                swal({ title: "MSR Error", text: msrData.StatusText, showConfirmButton: true });

            }
            else {
                swal({ title: "MSR Error", text: msrData.StatusText, showConfirmButton: true });
                $scope.$apply(function () { $modalInstance.dismiss('msrError'); });
            }
        });


        $scope.$on("$destroy", function () {
            msrEvent();
            msrErrorEvent();
        });

        $scope.disableSwipeCard = function () {
            $msrService.disableMSR().then(
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
            $modalInstance.dismiss('closed');
        };
    }]);

