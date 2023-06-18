import { Component, OnInit } from '@angular/core';
import { ProductsService } from 'src/app/Services/products.service';
import { Product } from '../../Interfaces/interfaces';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/Services/auth.service';
import { BasketService } from 'src/app/Services/basketservice';
import { SnackbarService } from 'src/app/Services/snackbar.service';
@Component({
  selector: 'app-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent implements OnInit {
  isLogin:boolean=false
  loading:boolean =false
  quantity:number=1
  constructor(
    private _ProductsService: ProductsService,
    private _ActivatedRoute: ActivatedRoute,
    private _AuthService:AuthService,
    private _SnackbarService: SnackbarService,
    private _BasketService:BasketService
  ) {}
  product?: Product
  id:number=0
  ratingForm!:FormGroup;
  ngOnInit(): void {
    this._AuthService.userData.subscribe({
      next:()=> {
        if(this._AuthService.userData.getValue()!=null){
          this.isLogin=true
        }
        else{
          this.isLogin =false
        }
      }
    })
    this._ActivatedRoute.params.subscribe(params => {
      this.id = +params['id'];
      this.getProduct(this.id);
      this.ratingForm=new FormGroup({
        productId:new FormControl(this.id),
        rating:new FormControl('',[Validators.required,Validators.min(1),Validators.max(5)]),
        message:new FormControl('',[Validators.required])
      })
    });
  }
  addToCart(product:Product, Quantity: number){
    this._BasketService.addToCart(product,Quantity)
  }
  getProduct(id: number) {
    this._ProductsService.GetProductById(this.id).subscribe({
      next: (value) => {
        this.product = value;
        console.log(value);

      }
    });
  }
  AddRating() {
    this.loading = true;
    this._ProductsService.AddRating(this.ratingForm.value).subscribe({
      next: () => {
        this.getProduct(this.id);
        this._SnackbarService.showSnackBar('Rating has been added');
        this.loading = false;
      },
      error: (err) => {
        console.log(err);
        this.loading = false;
      },
    });
  }

}
