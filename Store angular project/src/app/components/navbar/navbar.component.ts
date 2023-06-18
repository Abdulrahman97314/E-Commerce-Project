import { DecodedData, UserData } from './../../Interfaces/interfaces';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/Services/auth.service';
import { BasketService } from 'src/app/Services/basketservice';
import { v4 as uuidv4 } from 'uuid';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  static basketId: string;
  isLogin: boolean = false;
  itemCount: number =0;
  isAdmin:boolean=false;
  userData:DecodedData|null=null;
  constructor(
    private _AuthService: AuthService,
    private _BasketAndOrderService: BasketService
  ) {}

  ngOnInit(): void {
    let storedBasketId = localStorage.getItem('basketId');
    if (storedBasketId) {
      NavbarComponent.basketId = storedBasketId;
    } else {
      NavbarComponent.basketId = uuidv4();
      localStorage.setItem('basketId', NavbarComponent.basketId);
    }

    let storedItemCount = localStorage.getItem('itemCount');
    if (storedItemCount) {
      this._BasketAndOrderService.basketCount.next(parseInt(storedItemCount))

    }

    this._BasketAndOrderService.basketCount.subscribe({
      next: (value) => {
        this.itemCount = value;
        localStorage.setItem('itemCount', this.itemCount.toString());
      }
    });

    this._AuthService.userData.subscribe({
      next: () => {
        this.userData =this._AuthService.userData.getValue()
        if (this.userData != null) {
          this.isLogin = true;
          if(this.userData.role=="Admin")
          {
            this.isAdmin=true
          }

        } else {
          this.isLogin = false;
        }
      }
    });

  }

  Logout() {
    this._AuthService.Logout();
  }
  collapseNavbar() {
    const navbar = document.querySelector('.navbar-collapse') as HTMLElement;
    navbar.classList.remove('show');
  }
}
