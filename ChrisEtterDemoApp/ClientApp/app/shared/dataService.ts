import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Product } from "./product";
import "rxjs/add/operator/map";
import { Order, OrderItem } from "./order";
import { Login } from "../login/login.component";
import { LoginInfo } from "../login/login";

@Injectable()
export class DataService {

    constructor(private http: HttpClient) {
    }

    public products: Product[];
    public order: Order = new Order();
    private token: string = "";
    private tokenExpiration: Date;


    private handleError(error: any) {
        let errMsg = error.message || 'Server error';
        console.error(errMsg); // log to console instead
        return Observable.throw(errMsg);
    }


    public loadProducts() : Observable<Product[]>{
        return this.http.get<Product[]>("/api/products/getallproducts")
            .map((data) => this.products = data);
    }

    public get loginRequired(): boolean {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }

    public login(credentials) {
        return this.http.post<LoginInfo>("/account/createtoken", credentials)
            .map(response => {
                this.token = response.token;
                this.tokenExpiration = response.expiration;
                return true;
            })
    }

    public checkout() {
        if (!this.order.orderNumber) {
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }
        return this.http.post("/api/orders/postorder", this.order, {
            headers: new HttpHeaders({ "Authorization": "Bearer " + this.token })
        })
            .map(response => {
                this.order = new Order();
                return true;
            });
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