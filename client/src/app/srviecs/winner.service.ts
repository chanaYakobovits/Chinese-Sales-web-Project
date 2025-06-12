import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Winner } from '../models/Winner.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WinnerService {

  BASE_URL = 'http://localhost:5178/api/Winner';

  http: HttpClient = inject(HttpClient);

  constructor() { }

  GetWinner(): Observable<Winner[]> {
    const token = localStorage.getItem("token")
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })
    return this.http.get<Winner[]>(this.BASE_URL,{headers});
  }
}

