import { Gender } from '../enums/gender';
import { Country } from '../enums/country';


export class ProfileCreateDto {
  firstName: string;
  lastName: string;
  birthDate: Date;
  gender: Gender;
  nationalities: Country[];
  phone: string;
  email: string;
  rating: number;

  constructor(firstName: string, lastName: string, birthDate: Date, gender: Gender, nationalities: Country[],
    phone: string, email: string, rating: number) {
    this.firstName = firstName;
    this.lastName = lastName;
    this.birthDate = birthDate;
    this.nationalities = nationalities;
    this.gender = gender;
    this.phone = phone;
    this.email = email;
    this.rating = rating;
  }

  static empty(): ProfileCreateDto {
    return new ProfileCreateDto('', '', new Date(), Gender.Unknown, [], '', '', 0);
  }
}
