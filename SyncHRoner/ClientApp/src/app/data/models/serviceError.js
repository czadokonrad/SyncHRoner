"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ServiceError = /** @class */ (function () {
    function ServiceError(errorNumber, message, friendlyMessage) {
        this.errorNumber = errorNumber;
        this.message = message;
        this.friednlyMessage = friendlyMessage;
    }
    return ServiceError;
}());
exports.ServiceError = ServiceError;
//# sourceMappingURL=serviceError.js.map