import { Component } from "@angular/core";
import { DataService } from "../shared/dataService";
import { Router } from "@angular/router";

@Component({
    selector: "login",
    templateUrl: "login.component.html",
    styleUrls: []

})


export class Login {
    errorMessage: string;
    constructor(private data: DataService, private router: Router) {
    }

    public credentials = {
        username: "",
        password: ""
    }

    onLogin() {
        this.data.login(this.credentials)
            .subscribe(success => {
                if (success) {
                    if (this.data.order.items.length == 0) {
                        this.router.navigate([""]);
                    } else {
                        this.router.navigate(["checkout"]);
                    }
                }
            }, error => this.errorMessage = "Failed to login.")
    }
}