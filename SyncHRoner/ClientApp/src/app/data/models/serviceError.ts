export class ServiceError {
  errorNumber: number;
  message: string;
  friednlyMessage: string;


  constructor(errorNumber: number, message: string, friendlyMessage: string) {
    this.errorNumber = errorNumber;
    this.message = message;
    this.friednlyMessage = friendlyMessage;
  }
}
