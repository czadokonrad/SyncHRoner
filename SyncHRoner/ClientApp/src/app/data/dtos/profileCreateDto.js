"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var gender_1 = require("../enums/gender");
var ProfileCreateDto = /** @class */ (function () {
    function ProfileCreateDto(firstName, lastName, birthDate, gender, nationalities, phone, email, rating) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.birthDate = birthDate;
        this.nationalities = nationalities;
        this.gender = gender;
        this.phone = phone;
        this.email = email;
        this.rating = rating;
    }
    ProfileCreateDto.empty = function () {
        return new ProfileCreateDto('', '', new Date(), gender_1.Gender.Unknown, [], '', '', 0);
    };
    return ProfileCreateDto;
}());
exports.ProfileCreateDto = ProfileCreateDto;
//# sourceMappingURL=profileCreateDto.js.map