describe("ErrorObj Service", function () {
    var errorObj;

    beforeEach(module('appUtilities'));

    beforeEach(inject(function (_errorObj_, $injector) {
        errorObj = _errorObj_;
    }));

    it('should exist', function () {
        expect(errorObj).toBeDefined();
    });

    it('should create an error object with the parameters passed in', function () {
        expect(errorObj.newError('a', 'b', 'c', 'd', 'e')).toEqual({
            name: 'a',
            message: 'b',
            details: 'c',
            code: 'd',
            level: 'e'
        });
    });

    it('should set the level to warn when not passed', function () {
        expect(errorObj.newError('a', 'b', 'c', 'd')).toEqual({
            name: 'a',
            message: 'b',
            details: 'c',
            code: 'd',
            level: 'WARN'
        });
    });
});