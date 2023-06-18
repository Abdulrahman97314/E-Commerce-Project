import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Services/auth.service';
import { SnackbarService } from 'src/app/Services/snackbar.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  constructor(private _AuthService:AuthService, private _Router:Router,    
    private _SnackbarService: SnackbarService,
    ){}
  ngOnInit(): void {
    this._AuthService.userData.subscribe({
      next:()=> {
        if(this._AuthService.userData.getValue()!=null){
          this._Router.navigate(['/products'])
        }
      }
    })  
  }
  hide:boolean = true;
  message:string="";
  loading:boolean =false;
  login:FormGroup= new FormGroup({
    email:new FormControl("",[Validators.required,Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8),Validators.maxLength(30)]),
  })
  onLoginSubmit(login:FormGroup) {
    this.loading=true
    if (this.login.valid) 
    {
      this._AuthService.Login(login.value).subscribe({
        next:(value)=> {
          localStorage.setItem('userToken',value.token)
          this._AuthService.SaveUserData();
          this._SnackbarService.showSnackBar(`Welcome Back ${this._AuthService.userData.getValue()?.given_name}`);
          this._Router.navigate(['/products'])
        },
        error:(err)=> {
          this.message =err.error.message
          this.loading=false
        },
        complete:()=> {
          this.loading=false
        },
      })
    }
  }
}
