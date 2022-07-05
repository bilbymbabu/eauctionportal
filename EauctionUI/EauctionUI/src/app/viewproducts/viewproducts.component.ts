import { Component, OnInit } from '@angular/core';
import { ProductService } from '../Services/product.service';

@Component({
  selector: 'app-viewproducts',
  templateUrl: './viewproducts.component.html',
  styleUrls: ['./viewproducts.component.css']
})
export class ViewproductsComponent implements OnInit {

  constructor(private productService:ProductService) { }
  bidInfo: any;
  productId: any;
  productInfo: any;
  selectedProductId: any;
  ngOnInit(): void {
    this.getAllProducts();
    
  }
  getAllProducts() {
    this.productService.viewAllProducts().subscribe(data => {
      if (data) {
        this.productInfo = data;
      }
    });
  }

  getBidDeatils() {
    console.log(this.selectedProductId);
    this.productService.viewAllBidForProducts(this.selectedProductId).subscribe({
      next: (data) => {
        this.bidInfo = data;
      },
      error: (err) => { },
      complete: () => { }
    });
  }
  
}
