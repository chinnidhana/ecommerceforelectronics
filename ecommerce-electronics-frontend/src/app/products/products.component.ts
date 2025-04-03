import { Component, OnInit } from '@angular/core';
import { ProductService, Product } from '../product.service';

@Component({
  selector: 'app-products',
  template: `
    <h2>Products</h2>
    <ul>
      <li *ngFor="let product of products">
        {{ product.productName }} - ₹{{ product.price }}
      </li>
    </ul>
  `
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.productService.getProducts().subscribe((data: Product[]) => {
      this.products = data;
    });
  }
}
