import { FC, useEffect, useState } from "react";
import { getUserOrders } from "../../api/orderAPI";
import { searchProduct } from "../../api/productAPI";
import { IProduct } from "../../models/ProductI";
import { Link } from "react-router-dom";

interface Product {
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

const UserOrder: FC = () => {
  const [order, setOrder] = useState<Product[]>([]);
  const [products, setProducts] = useState<IProduct[]>([]);

  useEffect(() => {
    getUserOrders().then((response) => setOrder(response));
  }, []);

  useEffect(() => {
    if (order.length > 0) {
      Promise.all(order.map((item) => searchProduct(item.productId))).then(
        (responses) => setProducts(responses)
      );
    }
  }, [order]);

  return (
    <div className='overflow-x-auto p-4'>
      <h1 className='text-xl font-bold mb-4'>Ваши заказы</h1>
      <div className='grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4'>
        {order.length > 0 ? (
          order.map((item, index) => {
            const product = products[index];

            return (
              <div key={item.id} className='border rounded-lg p-2 shadow-md'>
                <div className='flex justify-between '>
                  <h2 className='text-lg font-semibold text-nowrap'>
                    Заказ от {new Date(item.createdAt).toLocaleDateString()}
                  </h2>
                  {item.isDelivered ? (
                    <h2 className='text-base bg-green-400 font-semibold bg-clip-text'>
                      Доставлен
                    </h2>
                  ) : (
                    <h2 className='text-base rounded-lg shadow-xl font-semibold'>
                      В пути
                    </h2>
                  )}
                </div>
                {product && (
                  <Link to={`/product/${product.id}`} className='block mt-2'>
                    <div className='flex gap-4 items-center'>
                      <img
                        src={product.images?.[0] || "/placeholder.jpg"}
                        alt={product.name}
                        className='w-16 h-16 object-cover rounded-md'
                      />
                      <div>
                        <h3 className='text-md font-medium'>{product.name}</h3>
                        <p className='text-gray-500'>Цена: {item.price} ₽</p>
                        <p className='text-gray-500'>
                          Доставка: {item.deliveryPlace}
                        </p>
                      </div>
                    </div>
                  </Link>
                )}
              </div>
            );
          })
        ) : (
          <p className='text-gray-600'>У вас пока нет заказов.</p>
        )}
      </div>
    </div>
  );
};

export default UserOrder;
