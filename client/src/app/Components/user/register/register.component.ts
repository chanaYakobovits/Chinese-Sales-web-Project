import { Component, inject } from '@angular/core';
import { CustomerMask } from '../../../models/CustomerMask.model';
import { USERService } from '../../../srviecs/user.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Login } from '../../../models/Login.model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
  styles: [
    `.p-stepper {
    flex-basis: 40rem;
} 
`
  ]
})
export class RegisterComponent {
  active: number | undefined = 0;
  name: string = "";
  email: string = "";
  password: string = "";
  adress: string = "";
  phone: string = "";

  // State for validation errors
  validationErrors: any = {};

  router: Router = inject(Router);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);
  userSrv: USERService = inject(USERService);

  user: CustomerMask = new CustomerMask();
  cus: Login = new Login();
  option1: boolean | undefined = false;
  option2: boolean | undefined = false;
  option3: boolean | undefined = false;
  option4: boolean | undefined = false;
  option5: boolean | undefined = false;
  option6: boolean | undefined = false;
  option7: boolean | undefined = false;
  option8: boolean | undefined = false;
  option9: boolean | undefined = false;
  option10: boolean | undefined = false
  // Check if the form is valid
  isFormValid(): boolean {
    return !Object.keys(this.validationErrors).length;
  }

  // Validate the form fields
  validateForm() {
    this.validationErrors = {};

    if (!this.user.name || this.user.name.length < 3 || this.user.name.length > 50) {
      this.validationErrors['name'] = 'שם חייב להיות בין 3 ל-50 תווים';
    }
    if (!this.user.email || !this.user.email.match(/^[^@]+@[^@]+\.[^@]+$/)) {
      this.validationErrors['email'] = 'כתובת המייל אינה תקינה';
    }
    if (!this.user.phone || this.user.phone.length !== 10) {
      this.validationErrors['phone'] = 'טלפון חייב להיות באורך 10 תווים';
    }
    if (!this.user.password || this.user.password.length < 8 || this.user.password.length > 20) {
      this.validationErrors['password'] = 'סיסמא חייבת להיות בין 8 ל-20 תווים';
    }
  }

  // Save user and proceed to next step
  save(nextCallback: any) {
    this.validateForm(); // Validate form before saving

    if (this.isFormValid()) {
      this.userSrv.Add(this.user).subscribe({
        next: (data) => {
          console.log('Success:', data);
          // this.login();
          nextCallback.emit(); // Move to next step
        },
        error: (err) => {
          console.error('Error Response:', err);
          if (err.status === 400 && err.error.errors) {
            this.validationErrors = err.error.errors;
          }
        }
      });
    } else {
      console.log('Form is invalid, cannot proceed to the next step');
    }
  }

  login(nextCallback: any): void {
    this.cus.email = this.user.email;
    this.cus.password = this.user.password;

    this.userSrv.Login(this.cus).subscribe({
      next: (data) => {
        if (data?.token) {
          localStorage.setItem('token', data.token);
          nextCallback.emit(); // Move to next step
          setTimeout(() => {
            this.router.navigateByUrl('');
          }, 5000);
        }
      },
      error: (err) => {
        console.error('Login Error:', err);
      }
    });
  }
}
