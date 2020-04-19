import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, Params } from '@angular/router';
import { ProfileService } from '../../services/profile-service';
import { ServiceError } from '../../data/models/serviceError';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProfileDto } from '../../data/dtos/profileDto';
import { FormGroup, Validators, FormControl } from '@angular/forms';
import { Gender } from '../../data/enums/gender';
import { Country } from '../../data/enums/country';
import { ProfileUpdateDto } from '../../data/dtos/profileUpdateDto';


@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.scss']
})
export class ProfileDetailsComponent implements OnInit {

  loading: boolean;

  editProfileForm: FormGroup;
  genders = Gender;
  countries = Country;

  constructor(private route: ActivatedRoute,
              private router: Router,
              private snackBar: MatSnackBar,
              private profileService: ProfileService) { }

  ngOnInit(): void {

    this.loading = true;

    this.route.params.subscribe(
      (params: Params) => {
        const profileId: number = + params["id"];
        this.profileService.getProfileById(profileId)
          .subscribe((data: ProfileDto) => this.initForm(data),
            (error: ServiceError) => {
              if (error.errorNumber === 404)
                this.router.navigateByUrl('/404');
              else
                this.openSnackBar(error.friednlyMessage, 'Failure');
            },
            () => this.loading = false);
      }
    );
  }

  initForm(profileDto: ProfileDto): void {
    this.editProfileForm = new FormGroup({
      id: new FormControl(profileDto.id),
      firstName: new FormControl(profileDto.firstName,
        [Validators.required, Validators.minLength(3), Validators.maxLength(50), Validators.pattern("^([a-zA-Z])+$")]),
      lastName: new FormControl(profileDto.lastName,
        [Validators.required, Validators.minLength(3), Validators.maxLength(50), Validators.pattern("^([a-zA-Z])+$")]),
      birthDate: new FormControl(profileDto.birthDate,
        [Validators.required]),
      gender: new FormControl(profileDto.gender,
        [Validators.required]),
      phone: new FormControl(profileDto.phone,
        [Validators.required, Validators.minLength(9), Validators.maxLength(11), Validators.pattern("^((48)?[0-9]{9})$")]),
      email: new FormControl(profileDto.email,
        [Validators.required, Validators.email, Validators.maxLength(255)]),
      nationalities: new FormControl(profileDto.nationalities,
        [Validators.required]),
      rating: new FormControl(profileDto.rating,
        [Validators.required, Validators.max(5), Validators.min(0), Validators.pattern("^([0-9]{1}(\.[0-9])?)$")])
    });
  }

  openSnackBar(message: string, action: string) : void {
    this.snackBar.open(message, action, {
      duration: 5000
    });
  }

  onNoClick(): void {
    this.router.navigateByUrl('/profiles');
  }

  onDeleteClick(): void {

    const profileId: number = +this.editProfileForm.get('id').value;

    this.profileService.deleteProfile(profileId)
      .subscribe(success => {
        this.openSnackBar("Profile has been removed", "Success"); setTimeout(() => this.router.navigateByUrl('/profiles'), 2000); },
                (error: ServiceError) => this.openSnackBar(error.friednlyMessage, "Failure"));
  }

  editProfile(): void {
    const profileUpdateDto: ProfileUpdateDto = { ...this.editProfileForm.value };

    this.profileService.updateProfile(profileUpdateDto)
      .subscribe(success => this.openSnackBar("Profile has been updated", "Success"),
                (error: ServiceError) => this.openSnackBar(error.friednlyMessage, "Failure"));
  }
}
