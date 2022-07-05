import { DecimalPipe } from "@angular/common";

export interface IProduct
{
    productId?:string;
    productName?:string 
    shortseceription?:string
    detaileddeceription?:string
    category?:string 
    bidenddate?:Date
    StartingPrice:DecimalPipe
}