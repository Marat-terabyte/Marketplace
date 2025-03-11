import { IProduct } from "../models/ProductI";
import productStore from "../store/productStore";
import { $host } from "./index";

export const searchMoreProduct = async (): Promise<IProduct[]> => {
  const from = productStore.productFrom;
  const to = productStore.productTo;
  const q = "продукт";
  const { data } = await $host.get(`api/search`, {
    params: { q, from, to },
  });
  productStore.incrementProductFrom();

  const products = await Promise.all(
    data.map((product: { id: string }) => searchProduct(product.id))
  );

  return products;
};

export const searchProduct = async (id: string): Promise<IProduct> => {
  const { data } = await $host.get("api/products", {
    params: { id },
  });

  return data;
};

export const addProduct = async (
  name: string,
  description: string,
  category: string,
  price: number,
  stock: number,
  images?: string[],
  attributes?: { [key: string]: string }
) => {
  const { data } = await $host.post("api/products", {
    name,
    description,
    category,
    price,
    stock,
    images,
    attributes,
  });

  console.log(data);
  return data;
};

export const updateProduct = async (
  id: string,
  name?: string,
  description?: string,
  category?: string,
  price?: number,
  stock?: number,
  images?: string[],
  attributes?: { [key: string]: string }
) => {
  const { data } = await $host.put(`/api/products?id=${id}`, {
    name,
    description,
    category,
    price,
    stock,
    images,
    attributes,
  });

  return data;
};

export const deleteProduct = async (id: string) => {
  const { data } = await $host.delete(`/api/products?id=${id}`, {});

  return data;
};

export const getProductsSeller = async (id: string) => {
  const from = 0;
  const to = 20;

  const { data } = await $host.get(`api/products/seller`, {
    params: { sellerId: id, from, to },
  });

  return data;
};
