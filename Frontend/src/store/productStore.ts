import { makeAutoObservable } from "mobx";
import { IProduct } from "../models/ProductI";

class ProductStore {
  productFrom: number = 0;
  productDATASearchID: string[] = [];
  products: IProduct[] = [];

  constructor() {
    makeAutoObservable(this);
  }

  setProducts(product: IProduct[]) {
    this.products = product;
  }

  setProductFrom(productFrom: number) {
    this.productFrom = productFrom;
  }

  incrementProductFrom() {
    this.productFrom += 20;
  }

  setProductDATASearchID(product: string[]) {
    this.productDATASearchID = product;
  }

  get productTo() {
    return this.productFrom + 20;
  }
}

const productStore = new ProductStore();
export default productStore;
