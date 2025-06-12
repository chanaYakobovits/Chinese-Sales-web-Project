import { HttpHeaders } from '@angular/common/http';
import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { USERService } from './srviecs/user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  items: MenuItem[] | undefined;
  isAdmin: boolean = false;
  authService: USERService= inject(USERService);

  constructor(private router: Router,private cdr: ChangeDetectorRef) { }

  checkUserRole(): void {
    const role = this.authService.getUserRole();
    this.isAdmin = role === 'admin';
    
  }

  ngOnInit() {
    this.updateMenuItems();
    this.cdr.detectChanges();
    // this.checkUserRole();
  }

    updateMenuItems(): void {  
    this.items = [
      {
        label: 'דף הבית',
        icon: 'pi pi-home',
        command: () => {
          this.router.navigate(['']);
        }
      },
      {
        label: 'ניהול מתנות',
        icon: 'pi pi-cog',
        command: () => {
          this.router.navigate(['/gifts']);
        },
        // visible: this.isAdmin
      },
      {
        label: 'ניהול תורמים',
        icon: 'pi pi-id-card',
        command: () => {
          this.router.navigate(['/donors']);
        },
        // visible: this.isAdmin
      },
      {
        label: 'ניהול רכישות',
        icon: 'pi pi-id-card',
        command: () => {
          this.router.navigate(['/purchase']);
        },
        // visible: this.isAdmin
      },
      {
        label: 'מתנות',
        icon: 'pi pi-gift',
        command: () => {
          this.router.navigate(['/customer']);
        }
      },
      {
        label: 'הגרלה',
        icon: 'pi pi-sparkles',
        command: () => {
          this.router.navigate(['/lottery']);
        },
        // visible: this.isAdmin
      },
      {
        icon: 'pi pi-user',
        command: () => {
          this.router.navigate(['/auth']);
        }
      },
      {
        icon: 'pi pi-question-circle',
      },
      {
        icon: 'pi pi-sign-out',
        command: () => {
          this.logout();
        }
      }
    ];
    this.cdr.detectChanges();
  }

  logout() {
    localStorage.removeItem('token');
    this.isAdmin = false; // Reset isAdmin to false on logout
    this.updateMenuItems(); // Update menu items after logout
    this.router.navigate(['']);
    this.cdr.detectChanges(); // Detect changes after logout
  } 

}