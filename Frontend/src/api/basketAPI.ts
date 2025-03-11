import { $host } from "./index";

export const addProductBasket = async (productId: string) => {
  console.log(productId);
  const { data } = await $host.post(`api/cart?productId=${productId}`);

  return data;
};

export const getBasket = async () => {
  const from = 0;
  const to = 100;

  const { data } = await $host.get(`api/cart`, {
    params: { from, to },
  });

  return data;
};

export const basketProductCount = async (productId: string) => {
  const { data } = await $host.put(`api/cart`, {
    productId,
  });

  return data;
};

export const buyProducts = async (productId: string[] | string) => {
  const productIds = Array.isArray(productId) ? productId : [productId];

  const { data } = await $host.post(`api/cart/buy`, {
    SelectedCartProducts: productIds.map((id) => ({ ProductCartId: id })),
    DeliveryPlace: "Moscow",
  });

  return data;
};

export const updateCountProducts = async (productId: string, count: string) => {
  const { data } = await $host.put(`api/cart/${productId}`, { count: count });

  return data;
};
