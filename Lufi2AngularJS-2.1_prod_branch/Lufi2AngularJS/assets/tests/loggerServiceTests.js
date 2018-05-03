describe("Logger Service", function () {
    var loggerService, mockPOSLogSocketService, $log;

    beforeEach(angular.mock.module('appServicesWebSocket', 'appServicesItem'));
    beforeEach(module('appUtilities'));

    beforeEach(function () {
        mockPOSLogSocketService = {
            logToPOS: jasmine.createSpy('logToPOS')
        };

        module(function ($provide) {
            $provide.value('POSLogSocketService', mockPOSLogSocketService);
        });
    });

    beforeEach(inject(function (_loggerService_, _$log_) {
        loggerService = _loggerService_;
        $log = _$log_;
    }));

    it('should exist', function () {
        expect(loggerService).not.toBeUndefined();
    });

    it('should log to pos and console when error is called', function () {
        spyOn($log, 'error');
        loggerService.error('message');
        expect(mockPOSLogSocketService.logToPOS).toHaveBeenCalledWith('message');
        expect($log.error).toHaveBeenCalledWith('message');
    });

    it('should log to pos when posLog is called', function () {
        loggerService.posLog('message');
        expect(mockPOSLogSocketService.logToPOS).toHaveBeenCalledWith('message');
    });

    it('should log to console when log is called', function () {
        spyOn($log, 'info');
        loggerService.log('message');
        expect($log.info).toHaveBeenCalledWith('message');
    });

    it('should log to console when warn is called', function () {
        spyOn($log, 'warn');
        loggerService.warn('message');
        expect($log.warn).toHaveBeenCalledWith('message');
    });
});