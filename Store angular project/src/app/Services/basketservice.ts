import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { Basket, Product } from '../Interfaces/interfaces';
import { NavbarComponent } from '../components/navbar/navbar.component';
import { SnackbarService } from './snackbar.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
constructor(private _HttpClient:HttpClient, private _SnackbarService: SnackbarService,  ) { }
BaseUrl :string= "https://localhost:7078/"
basketCount:BehaviorSubject<number>=new BehaviorSubject(0);
addToBasket(basket:any):Observable<Basket>{
  return this._HttpClient.post<Basket>(`${this.BaseUrl}api/Basket`,basket);
}
deleteBasket(id:string):Observable<any>{
  return this._HttpClient.delete(`${this.BaseUrl}api/Basket/${id}`);
}
getBasket(id:string):Observable<Basket>{
  return this._HttpClient.get<Basket>(`${this.BaseUrl}api/Basket/${id}`);
}
calculateBasketCount(basket: Basket): number {
  const uniqueItems = new Set(basket.items.map(item => item.id));
  return uniqueItems.size;
}

addToCart(product: Product, quantity: number) {
  this.getBasket(NavbarComponent.basketId).subscribe({
    next: (basket) => {
      console.log(basket);
      basket.items ??= [];
      const itemIndex = basket.items?.findIndex((item) => item.id === product.id) ?? -1;
      if (itemIndex !== -1) {
        if (basket.items[itemIndex].quantity + quantity <= 10) {
          basket.items[itemIndex].quantity += quantity;
          this.updateBasket(basket, `+${quantity}`);
        } else {
          this._SnackbarService.showSnackBar(`Quantity of ${product.name} is limited to 10 in the basket`);
        }
      } else {
        const newItem = {
          id: product.id,
          productName: product.name,
          pictureUrl: product.pictureUrl,
          price: product.price,
          quantity: quantity,
          brand: product.productBrand,
          type: product.productType
        };
        basket.items.push(newItem);
        this.updateBasket(basket, 'item has been added to basket');
      }
    },
    error: (err) => {
      console.log(err);
    }
  });
}

private updateBasket(basket: Basket, successMessage: string) {
  this.addToBasket(basket).subscribe({
    next: (value) => {
      console.log(basket);
      let count = this.calculateBasketCount(basket);
      this.basketCount.next(count);
      this._SnackbarService.showSnackBar(successMessage);
    },
    error: (err) => {
      console.log(err);
    }
  });
}


}
