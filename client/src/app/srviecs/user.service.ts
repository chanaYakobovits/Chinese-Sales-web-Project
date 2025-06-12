import { inject, Injectable } from '@angular/core';
import { CustomerMask } from '../models/CustomerMask.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Login } from '../models/Login.model';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class USERService {

  BASE_URL = 'http://localhost:5178/api/Auth';

  http: HttpClient = inject(HttpClient);

  constructor() { }

  
   Add(user: CustomerMask):Observable<string>{
        console.log(this.BASE_URL, user);
        return this.http.post<string>(this.BASE_URL, JSON.stringify(user), {
          headers: {
            'Content-Type': 'application/json'
          }
        }); 
      }
    
      Login(user: Login):Observable<any>{
        console.log(this.BASE_URL, user);
        return this.http.post<any>(`${this.BASE_URL}/login`, JSON.stringify(user), {
          headers: {
            'Content-Type': 'application/json'
          }
        }); 
      }   

      getUserRole(): string {
        const token = localStorage.getItem('token');
        if (token) {
          const decodedToken: any = jwtDecode(token);
          console.log('Decoded Token:', decodedToken);
          const roleKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
          if (decodedToken && decodedToken[roleKey]) {
            console.log('User Role:', decodedToken[roleKey]);
            return decodedToken[roleKey].includes('Admin') ? 'admin' : 'user';
          } else {
            console.error('Role not found in token');
          }
        } else {
          console.error('Token not found in local storage');
        }
        return '';
      }
}
// function jwt_decode(token: string): any {
//   throw new Error('Function not implemented.');
// }

