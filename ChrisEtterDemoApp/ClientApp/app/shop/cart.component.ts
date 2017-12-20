import { Component } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Order } from "../shared/order";

@Component({
    selector: "cart",
    templateUrl: "cart.component.html",
    styleUrls: []
})

export class Cart {

    constructor(private data: DataService) { }

    
}