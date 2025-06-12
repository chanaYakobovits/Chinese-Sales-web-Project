import { Component } from '@angular/core';
import { WinnerService } from '../../srviecs/winner.service';
import { Observable, catchError, of, tap } from 'rxjs';
import { Winner } from '../../models/Winner.model';

@Component({
  selector: 'app-lottery',
  templateUrl: './lottery.component.html',
  styleUrl: './lottery.component.css'
})
export class LotteryComponent {

  Dialog!: boolean;
  list$: Observable<Winner[]> | undefined;
  winners: Winner[]=[]
  winner: Winner= new Winner
  flag: boolean= false

  constructor(private winSrv : WinnerService) {
  }

  ngOnInit() {
    this.lottery()
  }

  dialog(){
    this.Dialog = true
  }

  Yes(){
    this.Dialog = false
  }

  No(){
    this.Dialog = false
  }


  lottery(){
    this.list$ = this.winSrv.GetWinner().pipe(
      tap((winners: Winner[]) => {
          console.log('Received winners:', winners);
          this.winners = winners;
      }),
      catchError(error => {
          console.error('Error fetching winners:', error);  // הדפס את השגיאה
          return of([]);  // החזר רשימה ריקה במקרה של שגיאה
      })
  ); 
  this.flag = true
  }
}
