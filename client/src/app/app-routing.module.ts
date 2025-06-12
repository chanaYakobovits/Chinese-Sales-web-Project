import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GiftComponent } from './Components/gift/gift.component';
import { UserComponent } from './Components/user/user.component';
import { LoginComponent } from './Components/user/login/login.component';
import { RegisterComponent } from './Components/user/register/register.component';
import { NotFoundComponent } from './Components/not-found/not-found.component';
import { DonorComponent } from './Components/donor/donor.component';
import { GiftListComponent } from './Components/gift/gift-list/gift-list.component';
import { CusComponent } from './Components/cus/cus.component';
import { LotteryComponent } from './Components/lottery/lottery.component';
import { HomeComponent } from './Components/home/home.component';
import { PaymentComponent } from './Components/payment/payment.component';
import { LayotComponent } from './Components/layot/layot.component';
import { PurchaseComponent } from './Components/purchase/purchase.component';

const routes: Routes = [
   //{path: '', redirectTo:'start', pathMatch:'full'},
   { path: '', component: HomeComponent },
  { path: 'gifts', component: GiftComponent },
  {
    path: 'customer', component: CusComponent
  },
  {
    path: 'auth', component: UserComponent, children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent }
    ]
  },
  { path: 'donors', component: DonorComponent },
  
  { path: 'lottery', component: LotteryComponent },
  { path: 'payment', component: PaymentComponent },
  { path: 'layot', component: LayotComponent },
  { path: 'purchase', component: PurchaseComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
