import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { Login } from '../../../models/Login.model';
import { USERService } from '../../../srviecs/user.service';
import { Message, MessageService } from 'primeng/api';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [MessageService], // חובה להוסיף את השירות כאן או במודול
})
export class LoginComponent {
  messages: Message[] = [];
  email: string = '';
  password: string = '';
  active: number | undefined = 0;
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
  // Angular Dependency Injection
  router: Router = inject(Router);
  userSrv: USERService = inject(USERService);
  messageService: MessageService = inject(MessageService);

  cus: Login = new Login();

  save(): void {
    this.userSrv.Login(this.cus).subscribe({
      next: (data) => {
        if (data?.token) {
          localStorage.setItem('token', data.token);
          this.router.navigateByUrl('');
        } else {
          this.messages = [
            {
              severity: 'warn',
              summary: 'Login Failed',
              detail: 'No token received. Please try again.',
            },
          ];
        }
      },
      error: (err) => {
        const apiError = err.error?.error || 'An unknown error occurred';
        this.messages = [
          {
            severity: 'error',
            summary: 'Login Error',
            detail: apiError,
          },
        ];
      },
    });
  }
  register(): void {
    this.router.navigateByUrl('auth/register');
  }
}
