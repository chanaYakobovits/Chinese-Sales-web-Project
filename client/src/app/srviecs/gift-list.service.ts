import { inject, Injectable } from '@angular/core';
import { Gift } from '../models/Gift.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { Donator } from '../models/Donator.model';
import { Category } from '../models/Category.model';


@Injectable({
  providedIn: 'root'
})
export class GiftListService {

    BASE_URL = 'http://localhost:5178/api/Gift';
    BASE_URL_d = 'http://localhost:5178/api/Donator/';
   
    http: HttpClient = inject(HttpClient);
    constructor() { }

    GetByPage(page: number): Observable<Gift[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL}${page}`);
      return this.http.get<Gift[]>(`${this.BASE_URL}/${page}`,{headers})
    }

    GetByGift(name: string): Observable<Gift[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL}GetByName`, name);
      return this.http.get<Gift[]>(`${this.BASE_URL}/GetByName?name=${name}`,{headers});
    }

    GetByDonator(name: string): Observable<Gift[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL}GetByDonator`, name);
      return this.http.get<Gift[]>(`${this.BASE_URL}/GetByDonator?name=${name}`,{headers});
    }
    
    GetCheapGift(page: number): Observable<Gift[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL}GetCheapGift?page=${page}`);
      return this.http.get<Gift[]>(`${this.BASE_URL}/GetCheapGift?page=${page}`,{headers})
    }

    GetExpensiveGift(page: number): Observable<Gift[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL}GetExpensiveGift?page=${page}`);
      return this.http.get<Gift[]>(`${this.BASE_URL}/GetExpensiveGift?page=${page}`,{headers})
    }

    GetByCategory(page: number): Observable<Gift[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL}GetByCategory?page=${page}`);
      return this.http.get<Gift[]>(`${this.BASE_URL}/GetByCategory?page=${page}`,{headers})
    }

    Add(gift: Gift):Observable<string>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(this.BASE_URL, gift);
      return this.http.post<string>(this.BASE_URL, gift,{headers}); 
    }

    Update(gift: Gift):Observable<string>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(this.BASE_URL, gift); 
      return this.http.put<string>(this.BASE_URL, gift,{headers});
    }

    Delete(id: number){
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(this.BASE_URL + id);
      return this.http.delete(this.BASE_URL+"/"+ + id,{headers});
    }
    DeleteMany(id: number[]) {
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      // ממיר את מערך ה-id לרשימה של פרמטרים ב-URL
      const params = id.map(i => `id=${i}`).join('&');
      const url = `${this.BASE_URL}/?${params}`;
      
      console.log(url);  // בדוק את ה-URL שהולך להישלח
      return this.http.delete(url,{headers});
    }

    GetByDonor(): Observable<Donator[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL_d}`);
      return this.http.get<Donator[]>(`${this.BASE_URL_d}`,{headers})
    }

    GetCategory(): Observable<Category[]>{
      const token = localStorage.getItem("token")
      console.log(token);
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      })
      console.log(`${this.BASE_URL}GetCategory`);
      return this.http.get<Category[]>(`${this.BASE_URL}/GetCategory`,{headers})
    }

    }
