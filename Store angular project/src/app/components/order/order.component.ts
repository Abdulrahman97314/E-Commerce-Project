import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import { Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement, loadStripe } from '@stripe/stripe-js';
import { CdkStepper } from '@angular/cdk/stepper';
import { Basket, DeliveryMethod } from 'src/app/Interfaces/interfaces';
import { SnackbarService } from 'src/app/Services/snackbar.service';
import { Router } from '@angular/router';
import { BasketService } from 'src/app/Services/basketservice';
import { OrderService } from 'src/app/Services/order.service';
@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss'],
  providers:[CdkStepper]
})
export class OrderComponent implements OnInit {
  constructor(private _BasketService:BasketService,
              private _OrderService:OrderService,
              private _Router: Router,
              private _SnackbarService:SnackbarService
              ){}
  isEditable:boolean=true
  DeliveryMethods!:DeliveryMethod[]  ;
  basket!:Basket;
  stripe:Stripe|any;
  cardNumber?:StripeCardNumberElement;
  cardExpriy?:StripeCardExpiryElement;
  cardCvc?:StripeCardCvcElement;
  cardErros:any
  isNextLoading:boolean=false
  isCompleteLoading:boolean=false
  orderForm: FormGroup=new FormGroup({
    basketId: new FormControl(NavbarComponent.basketId, Validators.required),
    deliveryMethodId: new FormControl('', Validators.required),
    shippingAddress: new FormGroup({
      firstName: new FormControl('', Validators.required),
      lastName: new FormControl('', Validators.required),
      street: new FormControl('', Validators.required),
      city: new FormControl('', Validators.required),
      country: new FormControl('', Validators.required),
    })
  });
  async ngOnInit() {
    this.GetDeliveryMethods()
    loadStripe("pk_test_51N9ZapLBEmO7Dl6OZp5JCQWxdqGrFIRWY3EXTPn2iq3e2sg4TTp7EbNhdxSTF90jbcYhfGSsTXeXTeTZVQ5MNwDm00JGzmZpGY")
    .then(stripe=>{
      this.stripe=stripe;
      const elements=stripe?.elements()
      if(elements){
        this.cardNumber=elements.create("cardNumber")
        this.cardNumber.on('change',event=>{
          if(event.error)this.cardErros=event.error.message
          else this.cardErros=null
        })
        this.cardNumber.mount("#cardNumber")

        this.cardExpriy=elements.create("cardExpiry");
        this.cardExpriy.mount("#cardExpiry")

        this.cardCvc=elements.create("cardCvc");
        this.cardCvc.mount("#cardCvc")
      }
    }
      )
  }
  shippingPrice:number=0
  getTotalPrice(): number {
    let totalPrice = 0;
    if(this.basket){
    for (let item of this.basket.items) {
      totalPrice += item.price * item.quantity;
    }}
    return totalPrice + this.shippingPrice;
  }
  getbasket(){
    this._BasketService.getBasket(NavbarComponent.basketId).subscribe({
      next:(value)=> {
        this.basket = value
        console.log(value);
      },
    })
  }
  GetDeliveryMethods(){
    this._OrderService.GetDeliveryMethods().subscribe({
      next:(value)=> {
          console.log(value);
          this.DeliveryMethods = value
      },
    });
  }
  createPaymentIntent(){
    this._OrderService.CreatePaymentIntent(NavbarComponent.basketId).subscribe({
      next:(value)=> {
        this.getbasket();
      },
    })
  }
  confirmPaymentWithStripe(){
    return this.stripe.confirmCardPayment(this.basket?.clientSecret!,{
      payment_method:{
        card:this.cardNumber!,
        billing_details:{
          name:this.orderForm.get('shippingAddress.firstName')?.value +" "+ this.orderForm.get('shippingAddress.lastName')?.value
        }
      }
    },

    )
  }
  onSubmit(orderForm:FormGroup) {
    this.isCompleteLoading=true;
    this._OrderService.CreateOrder(orderForm.value).subscribe({
      next:async (value)=> {
       let result= await this.confirmPaymentWithStripe()
          if(result.paymentIntent){
           this._BasketService.deleteBasket(NavbarComponent.basketId).subscribe({
              next:(value)=> {
                this.isCompleteLoading=false
              },
            })
            this._BasketService.basketCount.next(0);
            this._SnackbarService.showSnackBar("Payment succeeded the order is on its way")
            this._Router.navigate(['/products'])
          }
          else{
            this._SnackbarService.showSnackBar("Payment failed")
          }
      },
      error:(err)=> {
          this.isCompleteLoading=false

      },
    })
  }
}

