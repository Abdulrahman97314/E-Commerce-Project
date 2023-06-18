import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/Services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  constructor(private _AuthService:AuthService, private _Router:Router){
  }
  hide:boolean = true;
  hideConfirm:boolean = true;
  message:string="";
  loading:boolean = false;
  register:FormGroup= new FormGroup({
    displayName:new FormControl("",[Validators.required,Validators.minLength(3),Validators.maxLength(30)]),
    email:new FormControl("",[Validators.required,Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(8),Validators.maxLength(30)]),
    confirmPassword:new FormControl("",[Validators.required, this.matchPasswords.bind(this)]),
    phoneNumber:new FormControl("",[Validators.required,Validators.pattern(/^\+?\d{1,3}[-.\s]?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{2,4}$/)])
  })
  
  matchPasswords(control: FormControl): {[s: string]: boolean} | null {
    const parent = control.parent;
    return control.value === parent?.get('password')?.value ? null : { notMatched: true };
  }
  ngOnInit() {
    this._AuthService.userData.subscribe({
      next:()=> {
        if(this._AuthService.userData.getValue()!=null){
          this._Router.navigate(['/products'])
        }
      }
    })  
    this.register.controls['password'].valueChanges.subscribe(() => {
      this.register.controls['confirmPassword'].updateValueAndValidity();
    });

  }
  onLoginSubmit(register:FormGroup) {

    if (this.register.valid) 
    {
      this._AuthService.Register(register.value).subscribe({
        next:(value)=> {
          localStorage.setItem('userToken',value.token)
          this._AuthService.SaveUserData();
          this._Router.navigate(['/home'])
        },
        error:(err)=> {
          this.message =err.error.message
        }
      })
    }
  }
}