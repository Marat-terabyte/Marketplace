import { jwtDecode, JwtPayload } from "jwt-decode";
import { ISellerSignUp } from "../models/SellerI";
import { IUserSigIn, IUserSignUp } from "../models/UserI";
import userStore from "../store/userStore";
import { $host } from "./index";

interface CustomJwtPayload extends JwtPayload {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
}

export const registration = async (dataUser: IUserSignUp) => {
  await $host.post("api/consumer/signup", {
    email: dataUser.email,
    password: dataUser.password,
    confirmPassword: dataUser.confirmPassword,
    name: dataUser.name,
    surname: dataUser.surname,
  });
};

export const registerSeller = async (dataSeller: ISellerSignUp) => {
  await $host.post("api/seller/signup", {
    email: dataSeller.email,
    password: dataSeller.password,
    ConfirmPassword: dataSeller.confirmPassword,
    storename: dataSeller.storename,
  });
};

export const login = async (dataUser: IUserSigIn) => {
  const { data } = await $host.post("api/account/signin", {
    email: dataUser.email,
    password: dataUser.password,
  });

  if (data) {
    localStorage.setItem("userRole", data.role);
    localStorage.setItem("id", data.id);
    userStore.setUserRole(localStorage.getItem("userRole")!);
    const jwtData = jwtDecode(data.token) as CustomJwtPayload;
    const nameStore =
      jwtData["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    localStorage.setItem("storeName", nameStore);
  }
  return data;
};
