describe('Announcement Delete Controller:', function () {
    beforeEach(module('home', 'ui.bootstrap', 'ui.bootstrap.modal'));

    var $controller, scope, controller;

    beforeEach(inject(function (_$controller_, $rootScope) {
        $controller = _$controller_;
        $scope = $rootScope.$new();

        modalInstance = {
            close: jasmine.createSpy('modalInstance.close'),
            dismiss: jasmine.createSpy('modalInstance.dismiss'),
            result: {
                then: jasmine.createSpy('modalInstance.result.then')
            }
        };

        controller = $controller('announcementDeleteCtrl', { $scope: $scope, $modalInstance: modalInstance });
    }));

    describe('Scope Functions', function () {
        it('should exist', function () {
            expect(controller).not.toBeUndefined();
        });

        it('should close modal when yes function is called', function () {
            $scope.yes();
            expect(modalInstance.close).toHaveBeenCalled();
        });

        it('should dismiss modal when no function is called', function () {
            $scope.no();
            expect(modalInstance.dismiss).toHaveBeenCalledWith('cancel');
        })
    });
});