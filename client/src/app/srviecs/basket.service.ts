import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, of, tap, throwError } from 'rxjs';
import { Order } from '../models/Order.model';
import { Basket } from '../models/Basket.model';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  BASE_URL = 'http://localhost:5178/api/Order';
  BASE_URL_cus = 'http://localhost:5178/api/Customer';

  http: HttpClient = inject(HttpClient);

  constructor() { }

  Add(order: Order): Observable<string> {
    const token = localStorage.getItem("token")
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })

    console.log(this.BASE_URL, order, { headers });
    return this.http.post<string>(this.BASE_URL, order, { headers });
  }

  GetBasket(): Observable<Basket[]> {
    const token = localStorage.getItem("token");
    if (!token) {
      console.error("Token is missing!");
      return throwError(() => new Error("Token is not provided"));
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  
    console.log("Headers:", headers);
    console.log("Fetching basket from URL:", this.BASE_URL_cus);
  
    // מחזיר את ה-Observable מבלי לקרוא לו ישירות (למניעת subscribe מיותר בתוך הפונקציה)
    return this.http.get<Basket[]>(this.BASE_URL_cus, { headers });
  } 

  Buy(): Observable<string> {
    const token = localStorage.getItem("token");
    if (!token) {
      console.error("Token is missing!");
      return throwError(() => new Error("Token is not provided"));
    }
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  
    console.log("Headers:", headers);
    console.log("Fetching basket from URL:", this.BASE_URL);
  
    // מחזיר את ה-Observable מבלי לקרוא לו ישירות (למניעת subscribe מיותר בתוך הפונקציה)
    return this.http.get<string>(`${this.BASE_URL}/Buy`, { headers });
  } 

  Delete(name: string){
    const token = localStorage.getItem("token")
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    }) 
    console.log("Fetching basket from URL:", `${this.BASE_URL}/${name}`);
    return this.http.delete(`${this.BASE_URL}/${name}`, { headers });
  }
  

}