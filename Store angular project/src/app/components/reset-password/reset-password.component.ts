import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/Services/auth.service';
import { SnackbarService } from 'src/app/Services/snackbar.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit{
constructor(
  private _AuthService:AuthService,
  private route: ActivatedRoute,
  private _SnackbarService: SnackbarService,
  private _Router:Router
  ){}
  hide:boolean=true
  hideConfirm:boolean=true
loading:boolean=false
  forgetPassword!:FormGroup;
resetPasswordForm!:FormGroup;
  ngOnInit(): void {
    const email = this.route.snapshot.queryParamMap.get('email');
    const encodedToken = this.route.snapshot.queryParamMap.get('token')??'';
    let token:any = decodeURIComponent(encodedToken);
    this.resetPasswordForm=new FormGroup({
      email:new FormControl(email,Validators.required),
      newPassword:new FormControl("",[Validators.required,Validators.maxLength(30),Validators.minLength(8)]),
      confirmPassword:new FormControl("",[Validators.required,this.matchPasswords.bind(this)]),
      token:new FormControl(token,[Validators.required])
    })
      this.resetPasswordForm.controls['newPassword'].valueChanges.subscribe(() => {
      this.resetPasswordForm.controls['confirmPassword'].updateValueAndValidity();
    })};
  matchPasswords(control: FormControl): {[s: string]: boolean} | null {
    const parent = control.parent;
    return control.value === parent?.get('newPassword')?.value ? null : { notMatched: true };
  }
  onResetPasswordSubmit(resetPasswordForm:FormGroup){
    this.loading=true;
  this._AuthService.ResetPassword(resetPasswordForm.value).subscribe({
    next:(value)=> {
      console.log(value);
      
      this._SnackbarService.showSnackBar(`${value.message}`)
      this.loading=false;
      this._Router.navigate(['/login'])
    },
    error:(err)=> {
      this._SnackbarService.showSnackBar(`${err.error.message}`)  
      this.loading=false;
    },
  })
}
}
