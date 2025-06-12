
import { Component, inject, ViewChild } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { GiftListComponent } from './gift-list/gift-list.component';

@Component({
    selector: 'app-gift',
    templateUrl: './gift.component.html',
    styleUrl: './gift.component.css',
    providers: [MessageService, ConfirmationService],
    styles: [
        `:host ::ng-deep .p-dialog .product-image {
          width: 150px;
          margin: 0 auto 2rem auto;
   
          display: block;
      }`
    ]
})
export class GiftComponent {

  giftDialog:boolean = true;
  flagAdd:boolean = false
  selectedId:number = 0;
  Id:number = 0;
  page: number=1
  @ViewChild(GiftListComponent) giftsListComponent!: GiftListComponent;
  pageChange(page: number) {
    this.page = page;
    this.reloadGifts(); // קריאה לטעינת מתנות
  }
  reloadGifts() {
    // הפעלת קריאה מחדש לרשימת המתנות
    this.giftDialog = false; // ודא שסוגר את הדיאלוג
    this.flagAdd = false;
    this.selectedId = 0;
  }
    closeAdd(loed:boolean){
this.giftsListComponent.GetAll()
    }
}
