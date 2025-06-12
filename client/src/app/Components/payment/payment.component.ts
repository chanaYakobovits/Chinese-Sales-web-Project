import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { BasketService } from '../../srviecs/basket.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css',
  providers: [MessageService]
})
export class PaymentComponent {
  paymentInformation: any;
  basketSrv: BasketService = inject(BasketService)
  router: Router = inject(Router);
  activatedRoute: ActivatedRoute = inject(ActivatedRoute);

constructor(private messageService: MessageService) { }
  pay() {

    this.basketSrv.Buy().subscribe((data) => {
      var d = data

    })
    this.messageService.add({
      severity: 'success',
      summary: 'Successful',
      detail: "שולם בהצלחה",
      life: 3000
    });
    setTimeout(() => {
      this.router.navigate(['/'], { relativeTo: this.activatedRoute })
    }, 3000)

  }

}

