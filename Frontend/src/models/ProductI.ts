export interface IProduct {
  id: string;
  sellerId: string;
  storename: string;
  name: string;
  description: string;
  category: string;
  price: number;
  stock: number;
  createdAt: string;
  updatedAt: string | null;
  images: [string];
  attributes: null | [string];
}
