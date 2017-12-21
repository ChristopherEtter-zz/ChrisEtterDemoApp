"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/common/http");
var core_1 = require("@angular/core");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/map");
var order_1 = require("./order");
var DataService = /** @class */ (function () {
    function DataService(http) {
        this.http = http;
        this.order = new order_1.Order();
        this.token = "";
    }
    DataService.prototype.handleError = function (error) {
        var errMsg = error.message || 'Server error';
        console.error(errMsg); // log to console instead
        return Observable_1.Observable.throw(errMsg);
    };
    DataService.prototype.loadProducts = function () {
        var _this = this;
        return this.http.get("/api/products/getallproducts")
            .map(function (data) { return _this.products = data; });
    };
    Object.defineProperty(DataService.prototype, "loginRequired", {
        get: function () {
            return this.token.length == 0 || this.tokenExpiration > new Date();
        },
        enumerable: true,
        configurable: true
    });
    DataService.prototype.login = function (credentials) {
        var _this = this;
        return this.http.post("/account/createtoken", credentials)
            .map(function (response) {
            _this.token = response.token;
            _this.tokenExpiration = response.expiration;
            return true;
        });
    };
    DataService.prototype.checkout = function () {
        var _this = this;
        if (!this.order.orderNumber) {
            this.order.orderNumber = this.order.orderDate.getFullYear().toString() + this.order.orderDate.getTime().toString();
        }
        return this.http.post("/api/orders/postorder", this.order, {
            headers: new http_1.HttpHeaders({ "Authorization": "Bearer " + this.token })
        })
            .map(function (response) {
            _this.order = new order_1.Order();
            return true;
        });
    };
    DataService.prototype.addToOrder = function (product) {
        var item = this.order.items.find(function (i) { return i.productId == product.id; });
        if (item) {
            item.quantity++;
        }
        else {
            item = new order_1.OrderItem();
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
    };
    DataService = __decorate([
        core_1.Injectable(),
        __metadata("design:paramtypes", [http_1.HttpClient])
    ], DataService);
    return DataService;
}());
exports.DataService = DataService;
//# sourceMappingURL=dataService.js.map