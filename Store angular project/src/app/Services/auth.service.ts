import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable ,BehaviorSubject} from 'rxjs';
import { RegisterForm,LoginForm, UserData, DecodedData,ForgetPassword, ResetPassword, ApiResponse} from '../Interfaces/interfaces';
import jwtDecode from 'jwt-decode';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private _httpClient:HttpClient,private _Router:Router) {
    if((localStorage.getItem('userToken'))!=null){
      this.SaveUserData();
    }
  }
userData:BehaviorSubject<DecodedData|null>=new BehaviorSubject<DecodedData|null>(null)
url:string="https://localhost:7078/"
SaveUserData(): void{
  let encodedToken:string = JSON.stringify(localStorage.getItem('userToken'))
  let decodedToken: DecodedData = jwtDecode(encodedToken);
  this.userData.next(decodedToken);
  }

  Register(registerForm:RegisterForm):Observable<UserData>{
    return this._httpClient.post<UserData>(`${this.url}api/Accounts/Register`, registerForm);
  }
  Login(loginForm:LoginForm):Observable<UserData>{
    return this._httpClient.post<UserData>(`${this.url}api/Accounts/Login`, loginForm);
  }
  ForgetPassword(ForgetPassword:ForgetPassword):Observable<ApiResponse>{
    return this._httpClient.post<ApiResponse>(`${this.url}api/Accounts/ForgotPassword`, ForgetPassword)
  }
  ResetPassword(resetPassword:ResetPassword):Observable<ApiResponse>{
    return this._httpClient.post<ApiResponse>(`${this.url}api/Accounts/ResetPassword`, resetPassword)
  }
  changePassword(changePasswordForm:FormGroup):Observable<ApiResponse>{
    return this._httpClient.post<ApiResponse>(`${this.url}api/Accounts/ChangePassword`, changePasswordForm);
  }
  Logout(){
    localStorage.removeItem("userToken")
    this.userData.next(null)
    this._Router.navigate(['/login'])
  }
}
