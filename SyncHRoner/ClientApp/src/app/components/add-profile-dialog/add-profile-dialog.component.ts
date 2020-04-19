import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { ProfileCreateDto } from '../../data/dtos/profileCreateDto';
import { ProfileService } from '../../services/profile-service';
import { ServiceError } from '../../data/models/serviceError';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { Gender } from '../../data/enums/gender';
import { Country } from '../../data/enums/country';

@Component({
  selector: 'app-add-profile-dialog',
  templateUrl: './add-profile-dialog.component.html',
  styleUrls: ['./add-profile-dialog.component.scss'],
})
export class AddProfileDialogComponent implements OnInit {
 

  genders = Gender;
  countries = Country;
  newProfile: ProfileCreateDto = ProfileCreateDto.empty();
  newProfileForm: FormGroup;


  constructor(private profileService: ProfileService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<AddProfileDialogComponent>) {
  }

  ngOnInit(): void {
    this.newProfileForm = new FormGroup({
      firstName: new FormControl(this.newProfile.firstName,
        [Validators.required, Validators.minLength(3), Validators.maxLength(50), Validators.pattern("^([a-zA-Z])+$")]),
      lastName: new FormControl(this.newProfile.lastName,
        [Validators.required, Validators.minLength(3), Validators.maxLength(50), Validators.pattern("^([a-zA-Z])+$")]),
      birthDate: new FormControl(this.newProfile.birthDate,
        [Validators.required]),
      gender: new FormControl(this.newProfile.gender,
        [Validators.required]),
      phone: new FormControl(this.newProfile.phone,
        [Validators.required, Validators.minLength(9), Validators.maxLength(11), Validators.pattern("^((48)?[0-9]{9})$")]),
      email: new FormControl(this.newProfile.email,
        [Validators.required, Validators.email, Validators.maxLength(255)]),
      nationalities: new FormControl(this.newProfile.nationalities,
        [Validators.required]),
      rating: new FormControl(this.newProfile.rating,
        [Validators.required, Validators.max(5), Validators.min(0), Validators.pattern("^([0-9]{1}(\.[0-9])?)$")])
    });

  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  saveProfile(): void {


    const newProfile: ProfileCreateDto = { ...this.newProfileForm.value };

    let saved: boolean = false;

    this.profileService.createProfile(newProfile)
      .subscribe(success => { this.openSnackBar('Profile has been saved', 'Success'); saved = true },
        (error: ServiceError) => this.openSnackBar(error.friednlyMessage, 'Failure'),
        () => this.dialogRef.close(saved ? newProfile : undefined));


    
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 5000
    });
  }

}
