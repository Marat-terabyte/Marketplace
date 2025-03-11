import { FC, useEffect, useState } from "react";
import { getSellerOrders } from "../../api/orderAPI";
import { IOrder } from "../../models/OrderdI";

const SellerOrders: FC = () => {
  const [orders, setOrders] = useState<IOrder[]>([]);
  useEffect(() => {
    getSellerOrders().then((responce) => {
      setOrders(responce);
    });
  }, []);
  return (
    <div className='overflow-x-auto'>
      <table className='min-w-full table-auto border-collapse'>
        <thead>
          <tr className='bg-gray-200'>
            <th className='border-b px-4 py-2 text-left'>ID</th>
            <th className='border-b px-4 py-2 text-left'>Заказчик</th>
            <th className='border-b px-4 py-2 text-left'>Дата</th>
            <th className='border-b px-4 py-2 text-left'>Статус</th>
            <th className='border-b px-4 py-2 text-left'>Сумма заказа</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id} className='hover:bg-gray-100'>
              <td className='border-b px-4 py-2'>{order.id}</td>
              <td className='border-b px-4 py-2'>{order.deliveryPlace}</td>
              <td className='border-b px-4 py-2'>
                {new Date(order.createdAt).toLocaleDateString("ru-RU", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                })}
              </td>
              <td className='border-b px-4 py-2'>
                {order.isDelivered ? "Получен" : "Доставляется"}
              </td>
              <td className='border-b px-4 py-2'>{order.price}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default SellerOrders;
