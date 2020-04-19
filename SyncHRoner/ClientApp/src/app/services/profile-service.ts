import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';
import { ServiceError } from '../data/models/serviceError';
import { ProfileDto } from '../data/dtos/profileDto';
import { ProfileCreateDto } from '../data/dtos/profileCreateDto';
import { ProfileUpdateDto } from '../data/dtos/profileUpdateDto';

@Injectable({
  providedIn: 'root'
})

export class ProfileService {

  private profileApiEndpoint: string = "http://localhost:50992/api/profiles";


  constructor(private http: HttpClient) { }

  getAllProfiles(): Observable<ProfileDto[] | ServiceError> {
    return this.http.get<ProfileDto[]>(this.profileApiEndpoint)
      .pipe(retry(2), catchError(this.handleError))
  }

  getProfileById(id: number): Observable<ProfileDto | ServiceError> {
    return this.http.get<ProfileDto>(`${this.profileApiEndpoint}/${id}`)
      .pipe(retry(2), catchError(this.handleError))
  }

  createProfile(dto: ProfileCreateDto): Observable<any | ServiceError> {
    return this.http.post(this.profileApiEndpoint, dto)
      .pipe(catchError(this.handleError));
  }

  updateProfile(dto: ProfileUpdateDto): Observable<any | ServiceError> {
    return this.http.put(this.profileApiEndpoint, dto)
      .pipe(catchError(this.handleError));
  }

  deleteProfile(id: number): Observable<any | ServiceError> {
    return this.http.delete(`${this.profileApiEndpoint}/${id}`)
      .pipe(catchError(this.handleError));
  }


  handleError(error: HttpErrorResponse): Observable<ServiceError> {

    let friendlyError: string = "";

    if (error.status === 409)
      friendlyError = "Profile with passed data already exists";
    else if (error.status === 400)
      friendlyError = `Provided invalid input: ${error.statusText}`;
    else if (error.status === 404)
      friendlyError = `Item not found`;
    else
      friendlyError = "Unexpected error occured";

    const serviceError = new ServiceError(error.status, error.statusText, friendlyError);

    return throwError(serviceError);
  }

}
