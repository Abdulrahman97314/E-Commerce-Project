import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProductsService } from 'src/app/Services/products.service';
import { PaginatedResult, BrandOrType } from '../../Interfaces/interfaces';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit, OnDestroy {
  paginatedResult: PaginatedResult = { pageIndex: 0, pageSize: 0, count: 0, data: [] };
  pageSize = 12;
  pageIndex = 1;
  sort:string = '';
  search:string = '';
  brands: BrandOrType[] = [];
  types: BrandOrType[] = [];
  selectedBrandId: number | '' = '';
  selectedTypeId: number | '' = '';
  pageNumbers: number[] = [];
  totalPage = 0;
  paginationLimit = 5;
  private subscriptions: Subscription[] = [];

  constructor(private productsService: ProductsService) {}

  ngOnInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

  getProducts(): void {
    const subscription = this.productsService
      .GetProducts(this.pageSize, this.pageIndex, this.sort, this.selectedBrandId, this.selectedTypeId, this.search)
      .subscribe({
        next: (value) => {
          this.paginatedResult = value;
          console.log(this.paginatedResult);
          this.totalPage = Math.ceil(this.paginatedResult.count / this.paginatedResult.pageSize);
          const startIndex = Math.max(Number(this.pageIndex) - Math.floor(Number(this.paginationLimit) / 2), 1);
          const endIndex = Math.min(startIndex + this.paginationLimit - 1, this.totalPage);

          this.pageNumbers = Array(endIndex - startIndex + 1)
            .fill(0)
            .map((x, i) => i + startIndex);
        },
        error: (err) => {
          console.log(err);
        },
        complete: () => {
          window.scrollTo(0, 0);
        }
      });

    this.subscriptions.push(subscription);
  }

  getBrands(): void {
    const subscription = this.productsService.GetBrands().subscribe({
      next: (brands) => {
        this.brands = brands;
        console.log(brands);
      },
      error: (err) => {
        console.log(err);
      }
    });

    this.subscriptions.push(subscription);
  }

  getTypes(): void {
    const subscription = this.productsService.GetTypes().subscribe({
      next: (types) => {
        this.types = types;
        console.log(types);
      },
      error: (err) => {
        console.log(err);
      }
    });

    this.subscriptions.push(subscription);
  }
}