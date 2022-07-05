import { DecimalPipe } from "@angular/common";
export interface IBidDetails
{
    productId?:string;
    productName?:string 
    shortseceription?:string
    detaileddeceription?:string
    category?:string 
    bidenddate?:Date
    StartingPrice:DecimalPipe
    BidAmount:DecimalPipe
    BuyerName?:string 
    EmailId?:string 
    Phone?:string
}