import { Gender } from '../enums/gender';
import { Country } from '../enums/country';

export class ProfileDto {
  readonly id: number;
  readonly firstName: string;
  readonly lastName: string;
  readonly birthDate: Date;
  readonly gender: Gender;
  readonly nationalities: Country[];
  readonly phone: string;
  readonly email: string;
  readonly rating: number;

  constructor(id: number, firstName: string, lastName: string, birthDate: Date, gender: Gender,
    nationalities: Country[],
    phone: string, email: string, rating: number) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.birthDate = birthDate;
    this.nationalities = nationalities;
    this.gender = gender;
    this.phone = phone;
    this.email = email;
    this.rating = rating;
  }

}
