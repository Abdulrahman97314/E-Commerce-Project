import { Component, OnInit } from '@angular/core';
import { BasketService } from 'src/app/Services/basketservice';
import { NavbarComponent } from '../navbar/navbar.component';
import { Basket, BasketItem } from 'src/app/Interfaces/interfaces';
@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss'],
})
export class BasketComponent implements OnInit {
  constructor(
    private _BasketService: BasketService) {}

  basket: Basket = { id: '', items: [] };

  ngOnInit(): void {
    this.GetBasket();
  }

  GetBasket() {
    this._BasketService.getBasket(NavbarComponent.basketId).subscribe({
      next: (value) => {
        console.log(value);
        this.basket = value;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  DeleteBasket() {
    this._BasketService
      .deleteBasket(NavbarComponent.basketId)
      .subscribe({
        next: (value) => {
          console.log(value);
          this.basket.items = [];
          this._BasketService.basketCount.next(0);
        },
        error: (err) => {
          console.log(err);
        },
      });
  }

  getTotalPrice(): number {
    let totalPrice = 0;
    for (let item of this.basket.items) {
      totalPrice += item.price * item.quantity;
    }
    return totalPrice;
  }

  removeItem(itemId: number) {
    this.basket.items = this.basket.items.filter((item) => item.id !== itemId);
    this.UpdateBasket();
    let count = this._BasketService.calculateBasketCount(this.basket);
    this._BasketService.basketCount.next(count);
  }

  updateQuantity(item: BasketItem, quantity: number) {
    item.quantity = quantity;
    this.UpdateBasket();
  }

  UpdateBasket() {
    this._BasketService.addToBasket(this.basket).subscribe({
      next: (value) => {},
      error: (err) => {
        console.log(err);
      },
    });
  }
}
