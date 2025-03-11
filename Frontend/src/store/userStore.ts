import { makeAutoObservable } from "mobx";

class UserStore {
  userAuth: boolean = true;
  activeTab: string = "auth";
  userRole = localStorage.getItem("userRole") || "guest";

  constructor() {
    makeAutoObservable(this);
  }

  setUserRole(auth: string) {
    this.userRole = auth;
  }

  setActiveTab(tab: string) {
    this.activeTab = tab;
  }

  get UserRole() {
    return localStorage.getItem("userRole") || "guest";
  }
}
const userStore = new UserStore();
export default userStore;
