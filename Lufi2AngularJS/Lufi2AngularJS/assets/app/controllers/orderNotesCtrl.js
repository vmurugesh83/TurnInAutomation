angular.module('orderNote', ['ui.bootstrap'])
    .controller('orderNoteCtrl', ['$scope', '$stateParams', '$state', 'order', 'loggerService', function ($scope, $stateParams, $state, $order, $loggerService) {
        var orderline = $stateParams.orderLineNo.toString().trim();
        var primeSubLineArr = [];
        $scope.notes = [];
        $scope.goToDetails = function () { $state.go('orderDetail'); };

        var _order = $order.getCurrentOrderDetails();

        var myScroll = new IScroll('#NotesDetailWrapper', { mouseWheel: true, scrollbars: true, shrinkScrollbars: 'clip' });

        $scope.refreshIScroll = function () {
            setTimeout(function () {
                myScroll.refresh();
            }, 500);
        };

        //filter out Internal Use Only Notes from $scope.notes
        var _filterInternalNotes = function () {
            for (var i = 0; i < $scope.notes.length; i++) {
                var currentNote = $scope.notes[i];

                if (angular.isString(currentNote._VisibleToAll) && (/^N$/i).test(currentNote._VisibleToAll)) {
                    $scope.notes.splice(i, 1);
                    i--;
                }
            }
        };


        if (orderline.length > 0) {
            //expected format is 1_2 for prime line 1, sub line 2
            primeSubLineArr = orderline.split('_');
        }

        if (primeSubLineArr.length > 0) {
            //find orderline
            for (var i = 0; i < _order.OrderLines.OrderLine.length; i++) {
                var currentLine = _order.OrderLines.OrderLine[i];
                if( currentLine._PrimeLineNo.toString().trim() === primeSubLineArr[0] &&
                    currentLine._SubLineNo.toString().trim() === primeSubLineArr[1]
                 ) {

                    if (currentLine.Notes && currentLine.Notes.Note) {
                        $scope.notes = currentLine.Notes.Note;
                        _filterInternalNotes();
                        $scope.refreshIScroll();
                        break;
                    }
                }
            }
        } else {
            //must be order level notes
            if (_order.Notes && _order.Notes.Note) {
                $scope.notes = _order.Notes.Note;
                _filterInternalNotes();
                $scope.refreshIScroll();
            }
        }

        var _sortNotes = function () {
            $loggerService.log($scope.notes);

            $scope.notes = $scope.notes.sort(function (a, b) {
                if (angular.isDefined(b._SequenceNo)) {
                    if (angular.isDefined(a._SequenceNo)) {
                        return Number(b._SequenceNo) - Number(a._SequenceNo);
                    } else {
                        return -1;
                    }
                } else { return 1; }
            });
            $loggerService.log($scope.notes);

            $scope.refreshIScroll();
        };

        _sortNotes();
    }]);