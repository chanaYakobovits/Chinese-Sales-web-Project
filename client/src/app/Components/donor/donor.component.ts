import { Component, inject } from '@angular/core';
import { Donator } from '../../models/Donator.model';
import { Observable, catchError, of, tap } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DonorService } from '../../srviecs/donor.service';


@Component({
  selector: 'app-donor',
  templateUrl: './donor.component.html',
  styleUrl: './donor.component.css',
  providers: [MessageService, ConfirmationService],
})
export class DonorComponent {
 
    dialogGift!: boolean;
    donorDialog!: boolean;
    donors: Donator[] = []
    donor: Donator = new Donator
    SelectedDonors: Donator[] = [];
    list$: Observable<Donator[]> | undefined;
    submitted!: boolean;
    flagAdd: boolean = false;
    router: Router = inject(Router);
    activatedRoute: ActivatedRoute = inject(ActivatedRoute);
    messages: any[] = [];  // מערך לאחסון ההודעות
    validationErrors: { [key: string]: string } = {};
    donorForm: FormGroup;  // הוספתי את FormGroup
    idd: number = 0;
    
    constructor( private fb: FormBuilder,private messageService: MessageService, private confirmationService: ConfirmationService, private donorSrv: DonorService) {
      // יצירת טופס עם וולידציות
      this.donorForm = this.fb.group({
        name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
        email: ['', [Validators.required, Validators.email]],
        adress: [''],
        phone: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(10)]],
      });
      
    }
  
    // פונקציה לקבלת השגיאות מהטופס
    getValidationErrors(): { [key: string]: string } {
      const errors: { [key: string]: string } = {};
      const controls = this.donorForm.controls;
    
      for (const key in controls) {
        if (controls[key].invalid && controls[key].touched) {
          // הוספת הודעת שגיאה מותאמת אישית לכל שדה
          if (controls[key].hasError('required')) {
            errors[key] = 'שדה זה חייב להיות מלא';
          } else if (controls[key].hasError('email')) {
            errors[key] = 'הכנס כתובת מייל תקינה';
          } else if (controls[key].hasError('minlength')) {
            errors[key] = 'הכנס ערך ארוך יותר';
          } else if (controls[key].hasError('maxlength')) {
            errors[key] = 'הכנס ערך קצר יותר';
          }
        }
      }
      return errors;
    }
    
    // פונקציה שמחזירה את כל המתנות
    GetAll() {
      this.list$ = this.donorSrv.GetByPage(1).pipe(
        tap((donors: Donator[]) => {
          console.log('Received donors:', donors);
          this.donors = donors;
          console.log('Received ', donors);
        }),
        catchError(error => {
          console.error('Error fetching donors:', error);  // הדפס את השגיאה
          return of([]);  // החזר רשימה ריקה במקרה של שגיאה
        })
      );
    }
  
    ngOnInit() {
      this.GetAll();
    }
  
    //add donator
    openNew() {
      this.donor = new Donator(); // איפוס האובייקט
      this.submitted = false;
      this.flagAdd = true;
      this.donorForm.reset(); // איפוס הטופס
      
      // אם ברצונך להוסיף ערכי ברירת מחדל, תוכל לעשות זאת כך:
      this.donorForm.patchValue({
        name: '',
        email: '',
        adress: '',
        phone: ''
      });
    
      this.donorDialog = true;
    }
    
  
    deleteSelectedDonor() {
      this.confirmationService.confirm({
        message: 'Are you sure you want to delete the selected donors?',
        header: 'Confirm',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          for (let index = 0; index < this.SelectedDonors.length; index++) {
            this.donorSrv.Delete(this.SelectedDonors[index].id).subscribe((donors) => {
              this.list$ = this.donorSrv.GetByPage(1);
            })
          }
          this.SelectedDonors = [];
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Donor Deleted', life: 3000 })
        }
      });
    }
  
    editDonor(donor: Donator) {
      console.log(donor);
      this.idd = donor.id;
      this.donorDialog = true;
      this.donor = donor;
      console.log(this.donor);
      
      // עדכון הערכים בטופס
      this.donorForm.patchValue({
        name: donor.name,
        email: donor.email,
        adress: donor.adress,
        phone: donor.phone
      });
    }
    
  
    deleteDonor(Donator: Donator) {
      this.confirmationService.confirm({
        message: 'האם אתה בטוח שברצונך למחוק את התורם ' + Donator.name + '?',
        header: 'אישור',
        icon: 'pi pi-exclamation-triangle',
        accept: () => {
          console.log(Donator.id);
    
          this.donorSrv.Delete(Donator.id).subscribe({
            next: (donors) => {
              // הצלחה במחיקה
              this.list$ = this.donorSrv.GetByPage(1);
              this.messageService.add({
                severity: 'success',
                summary: 'Successful',
                detail: 'התורם נמחק בהצלחה',
                life: 3000
              });
            },
            error: (err) => {
              // שגיאה במחיקה
              console.error("Error occurred:", err);
              this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: 'הייתה בעיה במחיקת התורם.',
                life: 3000
              });
            }
          });
        }
      });
    }
    
    hideDialog() {
      this.donorDialog = false;
      this.submitted = false;
     
    }
  
    saveDonor() {
      if (this.donorForm.invalid) {
        // אם הטופס לא תקין, נציג את השגיאות
        this.validationErrors = this.getValidationErrors();
        return;
      }
      else{
      if (this.flagAdd) {
       this.Add()
      }
      else
        this.update()
      this.submitted = true;
    }
    }
  
  
    Add() {
      this.donor= this.donorForm.value;
      this.donorSrv.Add(this.donor).subscribe({
        next: (data) => {
          console.log("Data received:", data);
          
          // איפוס שגיאות במקרה של הצלחה
          this.validationErrors = {}; 
    
          // הצגת הודעת הצלחה
          this.messageService.add({
            severity: 'success',
            summary: 'התורם נשמר בהצלחה!',
            detail: 'התורם נוסף למערכת.',
            life: 3000
          });
    
          // סגירת דיאלוג
          this.donorDialog = false;
    
          // קריאה ל-GetAll אחרי 2 שניות
          setTimeout(() => {
            this.router.navigateByUrl('/gifts');
          }, 2000);
        },
        error: (err) => {
          console.error("Error occurred:", err);
    
          // בדיקת שגיאות מהשרת
          if (err.error && err.error.errors) {
            const errors = err.error.errors;
    
            // מאחד את כל הודעות השגיאה למערך
            const errorMessages: string[] = Object.values(errors).flat() as string[];
    
            // בחירת הודעת השגיאה הראשונה
            const firstError: string = errorMessages.length > 0 ? errorMessages[0] : 'שגיאה לא ידועה התרחשה';
    
            // הצגת הודעת שגיאה
            this.messageService.add({
              severity: 'error',
              summary: 'שגיאה',
              detail: firstError,
              life: 3000
            });
    
            // עדכון ה-validationErrors
            this.validationErrors = errors;
          } else {
            // במקרה שאין פרטים על השגיאה, הצגת הודעת שגיאה כללית
            this.validationErrors = {};
            this.messageService.add({
              severity: 'error',
              summary: 'שגיאה',
              detail: 'הייתה שגיאה בעדכון התורם',
              life: 3000
            });
          }
        }
      });
    }
  
  
  
    update() {
     
      this.donor= this.donorForm.value;
      console.log(this.donor);
       this.donor.id = this.idd;
      this.donorSrv.Update(this.donor).subscribe({
        next: (data) => {
          console.log("Data received:", data);
          
          // איפוס שגיאות במקרה של הצלחה
          this.validationErrors = {}; 
    
          // הצגת הודעת הצלחה
          this.messageService.add({
            severity: 'success',
            summary: 'התורם עודכן בהצלחה!',
            detail: 'התורם עודכן במערכת.',
            life: 3000
          });
    
          // סגירת דיאלוג
          this.donorDialog = false;
    
          // קריאה ל-GetAll אחרי 2 שניות
          setTimeout(() => {
            this.GetAll();
          }, 2000);
        },
        error: (err) => {
          console.error("Error occurred:", err);
    
          // בדיקת שגיאות מהשרת
          if (err.error && err.error.errors) {
            const errors = err.error.errors;
    
            // מאחד את כל הודעות השגיאה למערך
            const errorMessages: string[] = Object.values(errors).flat() as string[];
    
            // בחירת הודעת השגיאה הראשונה
            const firstError: string = errorMessages.length > 0 ? errorMessages[0] : 'שגיאה לא ידועה התרחשה';
    
            // הצגת הודעת שגיאה
            this.messageService.add({
              severity: 'error',
              summary: 'שגיאה',
              detail: firstError,
              life: 3000
            });
    
            // עדכון ה-validationErrors
            this.validationErrors = errors;
          } else {
            // במקרה שאין פרטים על השגיאה, הצגת הודעת שגיאה כללית
            this.validationErrors = {};
            this.messageService.add({
              severity: 'error',
              summary: 'שגיאה',
              detail: 'הייתה שגיאה בעדכון התורם',
              life: 3000
            });
          }
        }
      });
    }
    
  
    donarGift() {
      this.dialogGift = true
      console.log(this.donor);
    }
    trackByIndex(index: number, item: any): number {
      return index;
    }
    
  }
  
