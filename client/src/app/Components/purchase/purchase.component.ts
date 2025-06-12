import { Component, inject } from '@angular/core';
import { PurchaseService } from '../../srviecs/purchase.service';
import { GiftListService } from '../../srviecs/gift-list.service';
import { Order } from '../../models/Order.model';
import { Purchase } from '../../models/Purchase.model';
import { User } from '../../models/User.model';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrl: './purchase.component.css'
})
export class PurchaseComponent {
  
    purchaseSrv: PurchaseService = inject(PurchaseService);
    giftSrv: GiftListService = inject(GiftListService);
    //list$: Observable<Donator[]> | undefined;
    purchase$: Purchase[] = []
    Purchase: Purchase= new Purchase()
  user: User[] = []
    sumToGift: number = 0;
    purchaseGift$: Order[] = []
    totalTickets: number = 0;
    giftDialog: boolean = false;
    submitted: boolean = false;
    dialogVisible: boolean = false;
    purchases2$: Order[] = [];
    sum : number=0

    ngOnInit() {
      this.GetPayment()
      this.getPurchase();
      // this.getAllp2();
    }
  
    GetPayment(): void {
      this.purchaseSrv.GetPayment().subscribe((data) => {
        this.sum = data
      });
    }
     
    getPurchase() {
        this.purchaseSrv.GetPurchase(1).subscribe(p => {
        this.purchase$ = p;
        console.log("tttt",p);
       })
   }

    getPurchaseByGift(p: Purchase) {
      this.dialogVisible = true;
      this.Purchase = p;
      this.user= p.customers
  }

    hideDialog() {
      this.giftDialog = false;
      this.submitted = false;
    }

    // sumToGift(purchase: Purchase): number {
    //   purchase.customers.forEach((customer) => {
    //     sumToGift += customer.numberOfTickets * customer.ticketCost;
    //   }
    //   return this.sumToGift
  
    // }
    // getPurchase() {
    //   this.purchaseSrv.getAll().subscribe(p => {
    //     this.purchase$ = p;
    //     console.log(p);
    //   })
    // }
    // getTotalTickets(): number {
    //   return this.purchaseGift$.reduce((sum, purchase) => sum + purchase.numberOfTickets, 0);
    // }
  
    // getTotalPriceForGift(): number {
    //   return this.purchaseGift$.reduce((sum, purchase) => {
    //     this.totalTickets =  sum + (purchase.numberOfTickets * purchase.ticketCost);
    //     return this.totalTickets;
    //   }, 0);
    // }
    // getAllp2() {
    //   this.purchaseSrv.getAll2().subscribe((purchases2) => {
    //     this.purchases2$ = purchases2;
    //   });
    // }

    
      // const totalRevenue = this.purchases2$.reduce((sum, purchase) => {
      //   return sum + (purchase.numberOfTickets * purchase.ticketCost);
      // }, 0);
      // return totalRevenue;
    //}
    
  }
  

