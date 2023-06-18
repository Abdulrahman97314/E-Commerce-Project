import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/Services/auth.service';
import { SnackbarService } from 'src/app/Services/snackbar.service';

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.scss']
})
export class ForgetPasswordComponent {
  constructor(
    private _AuthService:AuthService,
    private _SnackbarService: SnackbarService,
  ){}
loading:boolean=false
  forgetPassword!:FormGroup;
  ngOnInit(): void {
    this.forgetPassword=new FormGroup({
      email:new FormControl("",[Validators.required,Validators.email]),
    })
  }
  onForgetPasswordSubmit(forgetPassword:FormGroup){
    this.loading=true;
    this._AuthService.ForgetPassword(forgetPassword.value).subscribe({
      next:(value)=> {
        this._SnackbarService.showSnackBar(`${value.message}`);
        this.loading=false;
      },
      error:(err)=> {
        if(err.status==500){
          this._SnackbarService.showSnackBar(`Internal Server error plaese try again later`);
        }
        else{
          this._SnackbarService.showSnackBar(`${err.error.message}`);
        }        
        this.loading=false;
      },
    })
  }
}
