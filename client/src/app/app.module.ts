import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {NoopAnimationsModule} from '@angular/platform-browser/animations'
import {ImportsModule} from './imports'
import { CommonModule } from '@angular/common';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { GiftComponent } from './Components/gift/gift.component';
import { GiftListComponent } from './Components/gift/gift-list/gift-list.component';
import { EditGiftComponent } from './Components/gift/edit-gift/edit-gift.component';
import { UserComponent } from './Components/user/user.component';
import { LoginComponent } from './Components/user/login/login.component';
import { RegisterComponent } from './Components/user/register/register.component';
import { CusComponent } from './Components/cus/cus.component';
import { DonorComponent } from './Components/donor/donor.component';
import { DataViewModule } from 'primeng/dataview';
import { LotteryComponent } from './Components/lottery/lottery.component';
import { HomeComponent } from './Components/home/home.component';
import { PaymentComponent } from './Components/payment/payment.component';
import { LayotComponent } from './Components/layot/layot.component';
import { PurchaseComponent } from './Components/purchase/purchase.component';

@NgModule({
  declarations: [
    AppComponent,
    GiftComponent,
    GiftListComponent,
    EditGiftComponent,
    UserComponent,
    RegisterComponent,
    LoginComponent,
    DonorComponent,
    CusComponent,
    LotteryComponent,
    HomeComponent,
    PaymentComponent,
    LayotComponent,
    PurchaseComponent 
  ],
  imports: [    
    BrowserModule,
    AppRoutingModule,
    NoopAnimationsModule,
    ImportsModule,
    CommonModule,
    ToastModule,
    DataViewModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
