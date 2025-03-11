import { makeAutoObservable } from "mobx";

class SellerStore {
  shopName: string = localStorage.getItem("storeName") || "";

  constructor() {
    makeAutoObservable(this);
  }

  setShopName(shop: string) {
    this.shopName = shop;
  }
}
const sellerStore = new SellerStore();
export default sellerStore;
