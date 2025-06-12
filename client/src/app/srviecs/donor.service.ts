import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { Donator } from '../models/Donator.model';

@Injectable({
  providedIn: 'root'
})
export class DonorService {
  BASE_URL = 'http://localhost:5178/api/Donator';
   
  http: HttpClient = inject(HttpClient);
  constructor() { }

  GetByPage(page: number): Observable<Donator[]>{
     const token = localStorage.getItem("token")
        console.log(token);
        const headers = new HttpHeaders({
          'Authorization': `Bearer ${token}`
        })
    console.log(`${this.BASE_URL}${page}`);
    return this.http.get<Donator[]>(`${this.BASE_URL}/${page}`,{headers})
  }
  
  Add(donor: Donator):Observable<string>{
    const token = localStorage.getItem("token")
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })
    console.log(this.BASE_URL, donor);
    return this.http.post<string>(this.BASE_URL,donor,{headers}); 
  }

  Update(donor: Donator):Observable<string>{
    const token = localStorage.getItem("token")
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })
    console.log(this.BASE_URL, donor); 
    return this.http.put<string>(this.BASE_URL, donor,{headers});
  }

  Delete(id: number){
    const token = localStorage.getItem("token")
    console.log(token);
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })
    console.log(this.BASE_URL + id);
    return this.http.delete(`${this.BASE_URL}/${id}`,{headers});
  }

}
