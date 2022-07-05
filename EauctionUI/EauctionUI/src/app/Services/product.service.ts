import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { IProduct } from '../Models/IProduct.Module';
import { IBidDetails } from '../Models/IBidDetails.Module';


@Injectable({
    providedIn: 'root'
  })
  export class ProductService {
  
    readonly rootUrl = 'https://eauctiongetwayappservice.azurewebsites.net/e-auction/api/v1/Seller';
    constructor(private http: HttpClient,private router: Router) { }
  
    addProduct(productData:IProduct)
    {
      const headers = new HttpHeaders({
        'Content-Type': 'application/json'
      })
      const body: IProduct =productData;
      return this.http.post(this.rootUrl + '/addproduct', body,{headers:headers});
    }
    viewAllProducts()
    {
      const headers = new HttpHeaders({
        'Content-Type': 'application/json'
      })
        return this.http.get(this.rootUrl+'/products',{ headers: headers })
    } 
    
    viewAllBidForProducts(id:number)
    {
      const headers = new HttpHeaders({
        'Content-Type': 'application/json'
      })
        return this.http.get(this.rootUrl+'/show-bids/'+id,{ headers: headers })
    } 
  }