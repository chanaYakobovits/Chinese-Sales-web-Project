import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { Purchase } from '../models/Purchase.model';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {

  BASE_URL = 'http://localhost:5178/api/Winner';
  BASE_URL_Order = 'http://localhost:5178/api/Order';

  http: HttpClient = inject(HttpClient);

  constructor() { }

  GetPayment(): Observable<number> {
    const token = localStorage.getItem("token")
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })
    return this.http.get<number>(`${this.BASE_URL}/GetPayment`, { headers });
  }

  GetPurchase(page: number): Observable<Purchase[]> {
    const token = localStorage.getItem("token");
    if (!token) {
      console.error("Token is missing!");
      return throwError(() => new Error("Token is not provided"));
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    console.log("Fetching basket from URL:", `${this.BASE_URL_Order}/?page=${page}`);

    // מחזיר את ה-Observable מבלי לקרוא לו ישירות (למניעת subscribe מיותר בתוך הפונקציה)
    return this.http.get<Purchase[]>(`${this.BASE_URL_Order}/?page=${page}`, { headers });
  }

}
