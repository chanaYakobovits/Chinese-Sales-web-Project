import { Component, inject } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
import { Gift } from '../../models/Gift.model';
import { Category } from '../../models/Category.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Order } from '../../models/Order.model';
import { Basket } from '../../models/Basket.model';
import { GiftListService } from '../../srviecs/gift-list.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { BasketService } from '../../srviecs/basket.service';

@Component({
  selector: 'app-cus',
  templateUrl: './cus.component.html',
  styleUrl: './cus.component.css',
  providers: [MessageService, ConfirmationService],
  styles: [
    `:host ::ng-deep .p-dialog .product-image {
      width: 150px;
      margin: 0 auto 2rem auto;

      display: block;
  }`
]
})
export class CusComponent {

    layout: any = 'grid';
    sidebarVisible: boolean = false;
    gifts: Gift[] = [];
    gift!: Gift;
    order!: Basket;
    orders: Basket[] = []
    ordersMask: Basket[] = []
    selectedGifts: Gift[] = [];
    submitted: boolean = false;
    giftService: GiftListService = inject(GiftListService);
    list$: Observable<Gift[]> | undefined;
    listOrd$: Observable<Basket[]> | undefined;
    select: string = ""
    giftName: string = ""
    donorName: string = ""
    flag: boolean = false
    SelectId: number[] = []
    Order: Order = new Order()
    lastFlag: boolean = false;
    router: Router = inject(Router);
    activatedRoute: ActivatedRoute = inject(ActivatedRoute);
    categorys: Category[] = [];
    page: number = 1;
    sum: number = 0;

    constructor(private messageService: MessageService, private confirmationService: ConfirmationService, private ordSrv: BasketService) {
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
        this.giftService.GetCategory().subscribe(data => {
            this.categorys = data;
        });
        this.GetAll();
    }

    AddBasket(giftId: number) {
        // נניח שיש שיטה שמחזירה אם המשתמש מחובר
        const isLoggedIn = this.checkIfLoggedIn();

        if (!isLoggedIn) {
            this.messageService.add({
                severity: 'error',
                summary: 'Error',
                detail: "עליך להירשם כדי להוסיף מוצרים לסל.",
                life: 3000
            });
            setTimeout(() => {
                this.router.navigate(['/auth']); // הפניה לדף ההרשמה
            }, 3000);
            return;
        }

        // אם המשתמש מחובר, המשך להוסיף לסל
        this.Order.giftId = giftId;
        console.log(this.Order);

        this.ordSrv.Add(this.Order).subscribe({
            next: (data: any) => {
                console.log("Data received:", data);
                console.log("!!נוסף לסל");
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: "המתנה נוספה לסל שלך בהצלחה",
                    life: 3000
                });

            },
            error: (err: any) => {
                console.error("Error occurred:", err);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: "אירעה שגיאה בהוספת המתנה לסל שלך.",
                    life: 3000
                });
            },
        });
    }

    // פונקציה לבדיקה אם המשתמש מחובר
    checkIfLoggedIn(): boolean {
        return !!localStorage.getItem('token');
    }

    GetBasket() {
        this.ordSrv.GetBasket().subscribe({
            next: (orders: Basket[]) => {
                this.orders = orders;
                this.sumPayment();
            },
            error: (err: any) => {
                console.error("Error fetching basket:", err);
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: "אירעה שגיאה בעת טעינת סל הקניות. אנא נסה שוב מאוחר יותר.",
                    life: 3000
                });
            },
        });
        setTimeout(() => {
            this.sidebarVisible = true;
        }, 500);
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
        if (this.donorName.trim()) { // בדיקה אם הוזן טקסט
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

    Buy() {
        this.router.navigate(['/payment'], { relativeTo: this.activatedRoute })
    }

    Delete(name: string) {
        this.ordSrv.Delete(name).subscribe({
            next: data => {
                this.GetBasket()
                this.messageService.add({
                    severity: 'success',
                    summary: 'Successful',
                    detail: "המתנה נמחקה מהסל שלך בהצלחה",
                    life: 3000
                });
            },
            error: err => {
                console.error("Error occurred:", err)
                console.log("!!נוסף לסל");
                this.messageService.add({
                    severity: 'error',
                    summary: 'Error',
                    detail: "שגיאה במחיקת המתנה",
                    life: 3000
                });
            }
        });
    }

    prevCallback() {
        if (this.page > 1) {
            this.page -= 1;
            console.log(this.page);
            this.GetAll();
        }
    }

    nextCallback() {
        this.page += 1;
        console.log(this.page);
        this.GetAll();
    }

    sumPayment() {
        let sum = 0;
        for (let i = 0; i < this.orders.length; i++) {
            sum += this.orders[i].pay * this.orders[i].count;
        }
        this.sum = sum;
    }
}




