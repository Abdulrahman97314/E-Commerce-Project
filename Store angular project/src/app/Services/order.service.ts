import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Basket, DeliveryMethod, Order, OrderReturn } from '../Interfaces/interfaces';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  BaseUrl :string= "https://localhost:7078/"
  constructor(private _HttpClient:HttpClient) { }
  GetDeliveryMethods():Observable<DeliveryMethod[]>{
    return this._HttpClient.get<DeliveryMethod[]>(`${this.BaseUrl}api/Order/GetDeliveryMethods`)
  }
  CreateOrder(order:Order):Observable<OrderReturn>{
    return this._HttpClient.post<OrderReturn>(`${this.BaseUrl}api/Order/CreateOrder`,order)
  }
  CreatePaymentIntent(basketId:string):Observable<Basket>{
    return this._HttpClient.post<Basket>(`${this.BaseUrl}api/Payment?BasketId=${basketId}`,null)
  }
  getMyOrders():Observable<OrderReturn[]>{
    return this._HttpClient.get<OrderReturn[]>(`${this.BaseUrl}api/Order/GetOrdersForUser`)
  }
}
