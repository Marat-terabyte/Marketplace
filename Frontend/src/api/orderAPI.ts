import { IOrder } from "../models/OrderdI";
import productStore from "../store/productStore";
import { $host } from "./index";

export const searchMoreProduct = async () => {
  const from = productStore.productFrom;
  const to = productStore.productTo;
  const { data } = await $host.get(`api/consumer`, {
    params: { from, to },
  });

  return data;
};
export const getSellerOrders = async (): Promise<IOrder[]> => {
  const from = 0;
  const to = 100;
  const { data } = await $host.get(`api/order/seller`, {
    params: { from, to },
  });

  return data;
};
export const getUserOrders = async (): Promise<IOrder[]> => {
  const from = 0;
  const to = 100;
  const { data } = await $host.get(`api/order/consumer`, {
    params: { from, to },
  });

  return data;
};
