import { Component, EventEmitter, Input, Output, OnInit, output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import { GiftListService } from '../../../srviecs/gift-list.service';
import { Gift } from '../../../models/Gift.model';
import { Donator } from '../../../models/Donator.model';
import { Category } from '../../../models/Category.model';

@Component({
  selector: 'app-edit-gift',
  templateUrl: './edit-gift.component.html',
  styleUrl: './edit-gift.component.css',
  providers: [MessageService, ConfirmationService],
  styles: [
    `:host ::ng-deep .p-dialog .product-image {
          width: 150px;
          margin: 0 auto 2rem auto;
          display: block;
      }`
  ]
})
export class EditGiftComponent implements OnInit {
  @Input() selectedId: number = 0;
  @Input() giftDialog: boolean = true;
  @Input() page: number = 0;
  @Output() selectedIdChange: EventEmitter<number> = new EventEmitter<number>();
  @Output() flagAdd: EventEmitter<boolean> = new EventEmitter<boolean>();

  giftForm!: FormGroup;
  donators!: Donator[];
  categorys: Category[] = [];
  gift: Gift = new Gift();
  validationErrors: { [key: string]: string } = {};

  constructor(
    private fb: FormBuilder,
    private giftSrv: GiftListService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) { }

  ngOnInit() {
    this.initializeForm();

    if (this.selectedId !== 0) {
      this.giftSrv.GetByPage(this.page).subscribe(data => {
        const selectedGift = data.find(i => i.id === this.selectedId);
        if (selectedGift) {
          this.gift = selectedGift;
          this.giftForm.patchValue(this.gift);
        }
      });
    }

    this.giftSrv.GetByDonor().subscribe(data => (this.donators = data));
    this.giftSrv.GetCategory().subscribe(data => (this.categorys = data));
  }

  initializeForm() {
    this.giftForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      donatorId: [null, Validators.required],
      categoryId: [null],
      pay: [10, [Validators.required, Validators.min(10), Validators.max(100)]],
      image: ['']
    });
  }

  getValidationErrors(): { [key: string]: string } {
    const errors: { [key: string]: string } = {};
    Object.keys(this.giftForm.controls).forEach(key => {
      const control = this.giftForm.get(key);
      if (control?.invalid && control.touched) {
        if (control.hasError('required')) errors[key] = 'שדה זה הוא חובה';
        if (control.hasError('minlength')) errors[key] = 'אורך שדה קצר מדי';
        if (control.hasError('maxlength')) errors[key] = 'אורך שדה ארוך מדי';
        if (control.hasError('min')) errors[key] = 'הערך נמוך מדי';
        if (control.hasError('max')) errors[key] = 'הערך גבוה מדי';
      }
    });
    return errors;
  }

  save() {
    if (this.giftForm.invalid) {
      this.validationErrors = this.getValidationErrors();
      return;
    }
    this.gift = { ...this.gift, ...this.giftForm.value };
  
    if (this.selectedId !== 0) {
      this.update();
    } else {
      this.add();
    }
    this.flagAdd.emit(false); // עדכון מצב
  }
  
  cancel() {
    this.flagAdd.emit(false);
    this.selectedIdChange.emit(0);
  }
  

  update() {
    this.giftSrv.Update(this.gift).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'המתנה עודכנה בהצלחה!',
          detail: 'המתנה עודכנה במערכת.',
          life: 3000
        });
        setTimeout(() => {
          this.flagAdd.emit(false);
          this.selectedIdChange.emit(0);
        }, 1300)
      },
      error: err => console.error(err)
    });
  }

  add() {
    this.giftSrv.Add(this.gift).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'המתנה נשמרה בהצלחה!',
          detail: 'המתנה נוספה למערכת.',
          life: 3000
        });
        setTimeout(() => {
          this.flagAdd.emit(false);
          this.selectedIdChange.emit(0);
        }, 1300)
      },
      error: err => console.error(err)
    });
  }

}
