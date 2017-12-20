import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Product } from "./product";
import "rxjs/add/operator/map";
@Injectable()
export class DataService {

    constructor(private http: HttpClient){
    }

    public products: Product[];

    private handleError(error: any) {
        let errMsg = error.message || 'Server error';
        console.error(errMsg); // log to console instead
        return Observable.throw(errMsg);
    }

    public loadProducts(){
        return this.http.get("/api/products/getallproducts")
            .map((data: any[]) => { 
                this.products = data; 
                return true; 
            });
           
    }

    //public loadProducts() : Observable<Product[]>{
    //    return this.http.get("/api/products/getallproducts")
    //        .map((data: Response) => this.products = data.json());
    //}

}