export interface IOrder {
  id: string;
  consumerId: string;
  sellerId: string;
  productId: string;
  price: number;
  createdAt: string;
  deliveryPlace: string;
  isDelivered: boolean;
  deliveredAt: string;
}
