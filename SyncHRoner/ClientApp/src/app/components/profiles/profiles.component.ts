import { Component, OnInit } from '@angular/core';
import { ProfileService } from '../../services/profile-service';
import { ServiceError } from '../../data/models/serviceError';
import { MatDialog } from '@angular/material/dialog';
import { AddProfileDialogComponent } from '../add-profile-dialog/add-profile-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ProfileDto } from '../../data/dtos/profileDto';

@Component({
  selector: 'app-profiles',
  templateUrl: './profiles.component.html',
  styleUrls: ['./profiles.component.scss']
})
export class ProfilesComponent implements OnInit {


  
  allProfiles: ProfileDto[];
  loading: boolean = false;


  constructor(private profileService: ProfileService,
              private snackBar: MatSnackBar,
              public dialog: MatDialog) { } 

  ngOnInit(): void {

    this.getData();

  }

  getData(): void {
    this.loading = true;
    this.profileService.getAllProfiles()
      .subscribe((data: ProfileDto[]) => this.allProfiles = data,
        (error: ServiceError) => this.openSnackBar(error.friednlyMessage, "Failure"),
        () => this.loading = false
      )
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddProfileDialogComponent, {
      width: '330px'
    });

    dialogRef.afterClosed()
      .subscribe(data => {
        if (data !== undefined) {

          //just refresh
          //in real world there would be more efficient implementation like adding added item to the array of all profiles
          this.getData();
        }
      })
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 5000
    });
  }

  getRatingColor(rating: number): string {
    if (rating < 1) {
      return "rating-terrible";
    }
    else if (rating <= 2) {
      return "rating-bad";
    }
    else if (rating < 4) {
      return "rating-average";
    }
    else if (rating < 5) {
      return "rating-good";
    }
    else
      return "rating-perfect";
  }

}
