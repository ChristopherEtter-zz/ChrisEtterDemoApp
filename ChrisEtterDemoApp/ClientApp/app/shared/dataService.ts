import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Product } from "./product";
import "rxjs/add/operator/map";
import { Order, OrderItem } from "./order";

@Injectable()
export class DataService {

    constructor(private http: HttpClient) {
    }

    public products: Product[];
    public order: Order = new Order();
    


    private handleError(error: any) {
        let errMsg = error.message || 'Server error';
        console.error(errMsg); // log to console instead
        return Observable.throw(errMsg);
    }


    public loadProducts() : Observable<Product[]>{
        return this.http.get<Product[]>("/api/products/getallproducts")
            .map((data) => this.products = data);
    }

    public addToOrder(product: Product) {
   
        let item: OrderItem = this.order.items.find(i => i.productId == product.id);
        if (item) {

            item.quantity++;

        } else {

            item = new OrderItem();
            item.productId = product.id;
            item.productArtist = product.artist;
            item.productCategory = product.category;
            item.productArtId = product.artId;
            item.productTitle = product.title;
            item.productSize = product.size;
            item.unitPrice = product.price;
            item.quantity = 1;

            this.order.items.push(item);
        }
       

       

    }

}