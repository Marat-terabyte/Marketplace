import { FC, useEffect, useState } from "react";
import { buyProducts, getBasket } from "../api/basketAPI";
import { observer } from "mobx-react-lite";
import BasketCard from "../components/BasketCard";
interface BasketItem {
  amount: number;
  consumerId: string;
  count: number;
  createdAt: string;
  id: string;
  priceOfProduct: number;
  productId: string;
  sellerId: string;
}

const Basket: FC = observer(() => {
  const [data, setData] = useState<BasketItem[]>([]);
  const [selectedItems, setSelectedItems] = useState<boolean[]>(
    Array(1).fill(true)
  );
  const [trigger, setTrigger] = useState(false);

  useEffect(() => {
    getBasket()
      .then((r) => setData(r))
      .catch(() => setData([]));
  }, [trigger]);

  console.log(data);
  const toggleSelectAll = () => {
    const allSelected = selectedItems.every((item) => item);
    setSelectedItems(Array(selectedItems.length).fill(!allSelected));
  };

  const toggleItem = (index: number) => {
    setSelectedItems((prev) =>
      prev.map((item, i) => (i === index ? !item : item))
    );
  };

  const totalPrice = data.reduce((accumulator, current) => {
    return accumulator + (current.priceOfProduct || 0);
  }, 0);

  const getSelectedProductIds = () => {
    return data
      .filter((_, index) => selectedItems[index])
      .map((item) => item.id);
  };

  const handleCheckout = () => {
    const selectedProductIds = getSelectedProductIds();

    buyProducts(selectedProductIds).finally(() => setTrigger((prev) => !prev));
  };

  if (!data || data.length === 0) {
    return <div>Корзина пуста</div>;
  }

  return (
    <div className='mt-12'>
      <button
        onClick={toggleSelectAll}
        className='bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600'
      >
        {selectedItems.every((item) => item) ? "Снять выбор" : "Выбрать все"}
      </button>
      <div className='flex'>
        <div className='flex flex-col gap-2 min-w-fit'>
          {data.map((item, index) => (
            <BasketCard
              count={item.count}
              key={index}
              productId={item.productId}
              id={item.id}
              isSelected={selectedItems[index]}
              toggleSelect={() => toggleItem(index)}
              onButtonClick={() => setTrigger((prev) => !prev)}
            />
          ))}
        </div>

        <div className='min-w-96 p-5 flex flex-col gap-5'>
          <div className='bg-white p-4 rounded-lg shadow-md sticky top-1'>
            <h2 className='text-lg font-bold mb-2'>Информация о корзине</h2>
            <p className='text-base text-gray-500'>{data.length} товара</p>
            <div className='border-t my-3'></div>

            <div className='flex justify-between items-center mb-2'>
              <span className='text-gray-600'>Товары ({data.length})</span>
              <span className='font-semibold'>{totalPrice}</span>
            </div>

            <div className='flex-col flex items-center justify-center mt-5'>
              <button
                className='bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600'
                onClick={() => {
                  handleCheckout();
                }}
              >
                Перейти к оформлению
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
});

export default Basket;
