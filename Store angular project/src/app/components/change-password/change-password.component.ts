import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiResponse } from 'src/app/Interfaces/interfaces';
import { AuthService } from 'src/app/Services/auth.service';
import { SnackbarService } from 'src/app/Services/snackbar.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {
  constructor(private _AuthService:AuthService,
              private _SnackbarService:SnackbarService,
              private _Router:Router) { }
  ngOnInit(): void {
    this.changePasswordForm.controls['newPassword'].valueChanges.subscribe(() => {
      this.changePasswordForm.controls['confirmNewPassword'].updateValueAndValidity();
    }) }
  currentPasswordHide:boolean = true;
  newPasswordHide:boolean = true;
  confirmNewPasswordHide:boolean = true;
  loading:boolean =false;
  changePasswordForm:FormGroup=new FormGroup({
    currentPassword:new FormControl("",Validators.required),
    newPassword:new FormControl("",[Validators.required,Validators.minLength(8)]),
    confirmNewPassword:new FormControl("",[Validators.required,this.matchPasswords.bind(this)]),
  })
  matchPasswords(control: FormControl): {[s: string]: boolean} | null {
    const parent = control.parent;
    return control.value === parent?.get('newPassword')?.value ? null : { notMatched: true };
  }

  onFormSubmit(changePasswordForm:FormGroup){
    this.loading = true;
    this._AuthService.changePassword(changePasswordForm.value).subscribe({
      next: (value:ApiResponse) => {
        this.loading=false;
        this._SnackbarService.showSnackBar(value.message)
        this._Router.navigate(['/products'])
      },
      error: (error:any)=>
      {
        this.loading=false;
        this._SnackbarService.showSnackBar(error.error.message)
      }
    })
  }
}
