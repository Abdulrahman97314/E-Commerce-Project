import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product, PaginatedResult ,BrandOrType, AddRating} from '../Interfaces/interfaces';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
BaseUrl :string= "https://localhost:7078/"
  constructor(private _HttpClient:HttpClient) { }

  GetProducts(pageSize:number,pageIndex:Number,sort:string,brandId:number|'',typeId:number|'',search?:string):Observable<PaginatedResult>{
    return this._HttpClient.get<PaginatedResult>(`${this.BaseUrl}api/Product/GetProducts?PageSize=${pageSize}&PageIndex=${pageIndex}&Sort=${sort}&BrandId=${brandId}&TypeId=${typeId}&Search=${search}`);
  } 
  GetProductById(id:number):Observable<Product>{
    return this._HttpClient.get<Product>(`${this.BaseUrl}api/Product/GetProduct?id=${id}`);
  } 
  GetBrands():Observable<BrandOrType[]>{
    return this._HttpClient.get<BrandOrType[]>(`${this.BaseUrl}api/Product/GetProductBrands`);
  }
  GetTypes():Observable<BrandOrType[]>{
    return this._HttpClient.get<BrandOrType[]>(`${this.BaseUrl}api/Product/GetProductTypes`);
  }
  AddRating(rating :AddRating):Observable<void>{
    return this._HttpClient.post<void>(`${this.BaseUrl}api/Product/AddRating`,rating)
  }
}
