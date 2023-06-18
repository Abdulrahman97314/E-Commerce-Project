import { Component, OnInit } from '@angular/core';
import { OrderReturn } from 'src/app/Interfaces/interfaces';
import { OrderService } from 'src/app/Services/order.service';

@Component({
  selector: 'app-my-orders',
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.scss']
})
export class MyOrdersComponent implements OnInit{
  constructor(private _OrderService:OrderService) {}
  ngOnInit(): void {
    this.getOrders()
  }
  orders!:OrderReturn[]
  displayedColumns: string[] = ['id', 'orderDate', 'status', 'total']
  getOrders(){
    this._OrderService.getMyOrders().subscribe({
      next:(value)=> {
          console.log(value);
          this.orders = value;
      },
    })
  }
}
