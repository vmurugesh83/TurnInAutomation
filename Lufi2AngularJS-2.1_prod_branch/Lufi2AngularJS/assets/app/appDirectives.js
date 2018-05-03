angular.module('appDirectives', [])
    .directive("btFocus", ['$timeout', function ($timeout) {
        return function (scope, element) {
            $timeout(function () { element[0].focus(); });
        }
    }]).directive('onFinishRender', ['$timeout', function ($timeout) {
        return {
            restrict: 'A',
            link: function (scope, element, attr) {
                if (scope.$last === true) {
                    scope.$evalAsync(attr.onFinishRender);
                }
            }
        }
    }])
.directive('uiCheckoutProgress', ['$state', function ($state) {
    return {
        restrict: 'E',
        templateUrl: 'checkoutStepProgress.tpl.html',
        scope: {
            steps: "=stepobjectarray", //example: $scope.progressSteps = [{ display: 'Home', routerstate: 'home', isFinished: true, isActive: false, isHideOnClick:true },..]
            isCheckoutProgressDisplay: "=isstepdisplay"
        },
        link: function (scope, element, attrs) {

            if (angular.isDefined(scope.isCheckoutProgressDisplay)) {
                scope.isCheckoutProgressDisplay = !!scope.isCheckoutProgressDisplay;
            } else {
                scope.isCheckoutProgressDisplay = false;
            }

            scope.olClass = 'progtrckr' + scope.steps.length;
            scope.alwaysBlur = function (event) {
                $(event.target).each(function () { this.blur() });
            };
            scope.activate = function (step) {
                angular.forEach(scope.steps,
                    function (step) {
                        if (angular.isObject(step)) {
                            step.isActive = false;
                        }
                    });
                step.isActive = true;
            };

            scope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
                var stateName = $state.current.name;
                var currentStepIndex = -1;

                for (var i = 0; i < scope.steps.length; i++) {
                    //if hide property is set then hide directive
                    if (scope.steps[i].routerstate === stateName) {
                        if (scope.steps[i].isHideOnClick) {
                            scope.isCheckoutProgressDisplay = false;
                        }
                    }

                    //if state matches a step.routerState than set as active and isFinished
                    if (scope.steps[i].routerstate === stateName) {
                        currentStepIndex = i;
                    }
                }

                for (var i = 0; i < scope.steps.length; i++) {
                    //set isActive = false and isFinished=true for all index less than current state.
                    if (i <= currentStepIndex) {
                        scope.steps[i].isFinished = true;
                    }

                    //set isActive = false unless it is the currentStepIndex
                    if (currentStepIndex >= 0) {
                        if (i === currentStepIndex) {
                            scope.steps[i].isActive = true;
                        } else {
                            scope.steps[i].isActive = false;
                        }
                    }
                }

            });

            scope.$on('uiCheckoutStepProgressDisplay', function (event, args) {
                scope.isCheckoutProgressDisplay = !!args.display;

            });

            scope.$on('uiCheckoutStepProgressUpdate', function (event, args) {
                scope.olClass = 'progtrckr' + scope.steps.length;
            });
        }
    };
}])

.directive('customeraddress', function () {
    var directive = {};

    directive.restrict = 'E';

    directive.templateUrl = "html/customer/customerAddress.html";

    directive.scope = {
        address: "=address",
        addressclass: "=addressclass"
    }

    return directive;
})
.directive("iscrolltap", function () {
    //must add iscroll option tap:true to use
    return {
        link: function (scope, element, attrs) {
            var propExpression = attrs["iscrolltap"];
            element.on('tap', function () { scope.$eval(propExpression) });
        },
        restrict: "A"
    }

})
.directive('iscroll', function () {
    return {
        replace: false,
        restrict: 'A',
        link: function (scope, element, attr) {
            // default timeout
            var ngiScroll_timeout = 5;

            var ngiScroll_opts = {
                mouseWheel: true,
                scrollbars: true,
                shrinkScrollbars: 'clip',
                probeType: 3
            };

            // scroll key /id
            var scroll_key = attr.iscroll;

            if (scroll_key === '') {
                scroll_key = attr.id;
            }

            if (scope.$parent.myScrollOptions) {
                for (var i in scope.$parent.myScrollOptions) {
                    if (typeof (scope.$parent.myScrollOptions[i]) !== "object") {
                        ngiScroll_opts[i] = scope.$parent.myScrollOptions[i];
                    } else if (i === scroll_key) {
                        for (var k in scope.$parent.myScrollOptions[i]) {
                            ngiScroll_opts[k] = scope.$parent.myScrollOptions[i][k];
                        }
                    }
                }
            }

            // iScroll initialize function
            function setScroll() {
                if (scope.$parent.myScroll === undefined) {
                    scope.$parent.myScroll = [];
                }

                scope.$parent.myScroll[scroll_key] = new IScroll(element[0], ngiScroll_opts);
                scope.$parent.myScroll[scroll_key].enableStickyHeaders('thead');
            }

            // new specific setting for setting timeout using: iscroll-timeout='{val}'
            if (attr.ngIscrollDelay !== undefined) {
                ngiScroll_timeout = attr.ngIscrollDelay;
            }

            // watch for 'iscroll' directive in html code
            scope.$watch(attr.iscroll, function () {
                setTimeout(setScroll, ngiScroll_timeout);
            });

            // add ng-iscroll-refresher for watching dynamic content inside iscroll
            if (attr.ngIscrollRefresher !== undefined) {
                scope.$watch(attr.ngIscrollRefresher, function () {
                    if (scope.$parent.myScroll[scroll_key] !== undefined) scope.$parent.myScroll[scroll_key].refresh();
                });
            }

            // destroy the iscroll instance if we are moving away from a state to another
            // the DOM has changed and he only instance is not necessary any more
            scope.$on('$destroy', function () {
                scope.$parent.myScroll[scroll_key].destroy();
            });
        }
    };
})
;