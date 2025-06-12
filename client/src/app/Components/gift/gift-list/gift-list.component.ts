import { Component, inject, Output, EventEmitter, input, SimpleChanges, OnChanges } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Gift } from '../../../models/Gift.model';
import { GiftListService } from '../../../srviecs/gift-list.service';
import { Observable, of, Subject } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators'; // Import the switchMap operator for chaining
import { debounceTime } from 'rxjs/operators';
import { Donator } from '../../../models/Donator.model';
import { Category } from '../../../models/Category.model';

@Component({
    selector: 'app-gift-list',
    templateUrl: './gift-list.component.html',
    styleUrls: ['./gift-list.component.css'],
    providers: [MessageService, ConfirmationService],
    styles: [
        `:host ::ng-deep .p-dialog .product-image {
          width: 150px;
          margin: 0 auto 2rem auto;
          display: block;
      }`
    ]
})

export class GiftListComponent implements OnChanges  {
    readonly flagAdd = input<boolean>(true);
    readonly selectedId = input<number>(0);
    @Output() pageChange = new EventEmitter<number>();

    @Output() giftDialog = new EventEmitter<boolean>();
    @Output() selectedIdChange = new EventEmitter<number>();
    @Output() flagAddChange = new EventEmitter<boolean>();
    gifts: Gift[] = [];
    gift!: Gift;
    selectedGifts: Gift[] = [];
    submitted: boolean = false;
    flagEdit = new EventEmitter<boolean>();
    @Output() reload = new EventEmitter<void>();
    giftService: GiftListService = inject(GiftListService);
    list$: Observable<Gift[]> | undefined;
    select: string = ""
    giftName:string =""
    donorName:string =""
    flag:boolean=false
    SelectId:number[]=[]
    categorys: Category[]=[];
    donators!: Donator[];
    page:number=1
    constructor(private messageService: MessageService, private confirmationService: ConfirmationService) {

    }
    

    
//after we add a new gift we need to refresh the list of gifts
ngOnChanges(changes: SimpleChanges) {
    if (changes['flagAdd']) {
        console.log('flagAdd changed:', changes['flagAdd']);
        if (!changes['flagAdd'].currentValue) {
            this.GetAll();
        }
    }

    if (changes['selectedId']) {
        console.log('selectedId changed:', changes['selectedId']);
        if (changes['selectedId'].currentValue) {
            this.GetAll();
        }
    }
}



    // פונקציה שמחזירה את כל המתנות
    GetAll() {
        this.list$ = this.giftService.GetByPage(this.page).pipe(
            tap((gifts: Gift[]) => {
                console.log('Received gifts:', gifts);
                this.gifts = gifts;
            }),
            catchError(error => {
                console.error('Error fetching gifts:', error);  // הדפס את השגיאה
                return of([]);  // החזר רשימה ריקה במקרה של שגיאה
            })
        );
    }
    
    ngOnInit() {
        
         // טוען את כל התורמים
    this.giftService.GetByDonor().subscribe(data => {
        this.donators = data;
      });
    
      // טוען את כל הקטגוריות
      this.giftService.GetCategory().subscribe(data => {
        this.categorys = data;
      });
        this.GetAll();
    }

    GetByGift() {
        if (this.giftName.trim()) { // בדיקה אם הוזן טקסט
            this.list$ = this.giftService.GetByGift(this.giftName).pipe(
                tap((gifts: Gift[]) => {
                    console.log('Received gifts:', gifts);
                    this.gifts = gifts;
                }),
                catchError(error => {
                    console.error('Error fetching gifts:', error); // הדפסת שגיאה
                    return of([]); // מחזיר רשימה ריקה במקרה של שגיאה
                })
            );
        } else {
            this.GetAll();
        }
    }
    

    GetByDonator() {
        if(this.donorName.trim()) { // בדיקה אם הוזן טקסט
        this.list$ = this.giftService.GetByDonator(this.donorName).pipe(
            tap((gifts: Gift[]) => {
                console.log('Received gifts:', gifts);
                this.gifts = gifts;
            }),
            catchError(error => {
                console.error('Error fetching gifts:', error);  // הדפס את השגיאה
                return of([]);  // החזר רשימה ריקה במקרה של שגיאה
            })
        );
    }
    else {
       this.GetAll();
    }

}

    GetCheapGift() {
        this.list$ = this.giftService.GetCheapGift(this.page).pipe(
            tap((gifts: Gift[]) => {
                console.log('Received gifts:', gifts);
                this.gifts = gifts;
            }),
            catchError(error => {
                console.error('Error fetching gifts:', error);  // הדפס את השגיאה
                return of([]);  // החזר רשימה ריקה במקרה של שגיאה
            })
        );
        this.flag=true
    }

    GetExpensiveGift() {
        this.list$ = this.giftService.GetExpensiveGift(this.page).pipe(
            tap((gifts: Gift[]) => {
                console.log('Received gifts:', gifts);
                this.gifts = gifts;
            }),
            catchError(error => {
                console.error('Error fetching gifts:', error);  // הדפס את השגיאה
                return of([]);  // החזר רשימה ריקה במקרה של שגיאה
            })
        );
        this.flag=false
    }

    GetByCategory() {
        this.list$ = this.giftService.GetByCategory(this.page).pipe(
            tap((gifts: Gift[]) => {
                console.log('Received gifts:', gifts);
                this.gifts = gifts;
            }),
            catchError(error => {
                console.error('Error fetching gifts:', error);  // הדפס את השגיאה
                return of([]);  // החזר רשימה ריקה במקרה של שגיאה
            })
        );
        this.flag=false
    }



    setIsAddMode() {
        this.flagAddChange.emit(true);
        this.giftDialog.emit(true);
    }

    setIsEditMode(id: number) {
        this.selectedIdChange.emit(id);
    }

    deleteSelectedGifts() {
        this.confirmationService.confirm({
            message: 'האם אתה בטוח שאת/ה רוצה למחוק את המתנות שבחרת?',
            header: 'Confirm',
            icon: 'pi pi-exclamation-triangle',
            accept: () => {
                for (let index = 0; index < this.gifts.length; index++) {
                    if(this.selectedGifts?.includes(this.gifts[index]))
                    this.SelectId=[... this.SelectId,this.gifts[index].id]
                }
                this.giftService.DeleteMany(this.SelectId).subscribe(d => {
                    this.list$ = this.giftService.GetByPage(1);
                   });
                this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'המתנות נמחקו', life: 3000 });
            }
        });
    }

    deleteGift(gift: Gift) {
        this.confirmationService.confirm({
          message: 'האם אתה בטוח שאת/ה רוצה למחוק את המתנה  ' + gift.name + '?',
          header: 'אישור',
          icon: 'pi pi-exclamation-triangle',
          accept: () => {
            this.giftService.Delete(gift.id).subscribe(() => {
              this.GetAll();
              this.reload.emit(); // טעינת מתנות מחדש
            });
            this.messageService.add({ 
              severity: 'success', 
              summary: 'Successful', 
              detail: 'מתנה נמחקה ', 
              life: 3000 
            });
          }
        });
      }
      

    hideDialog() {
        this.submitted = false;
        this.reload.emit(); // הפעלת האירוע להורות על טעינה מחדש
    }

    findIndexById(id: number): number {
        let index = -1;
        for (let i = 0; i < this.gifts.length; i++) {
            if (this.gifts[i].id === id) {
                index = i;
                break;
            }
        }
        return index;
    }

    prevCallback(){
        if(this.page > 1){
        this.page -=1;
        console.log(this.page);
        this.pageChange.emit(this.page);
       this. GetAll();
    }
    }

    nextCallback(){
        this.page +=1;
        console.log(this.page);
        this.pageChange.emit(this.page);
        this. GetAll();
    }
}
