import { FC, useEffect, useState } from "react";
import trash from "../assets/delete_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg";
import thunder from "../assets/electric_bolt_24dp_000000_FILL1_wght300_GRAD0_opsz24.svg";
import { useForm } from "react-hook-form";
import { searchProduct } from "../api/productAPI";
import { IProduct } from "../models/ProductI";
import { buyProducts, updateCountProducts } from "../api/basketAPI";
type BasketCardProps = {
  productId: string;
  isSelected: boolean;
  toggleSelect: () => void;
  id: string;
  count: number;
  onButtonClick: () => void;
};

type FormData = {
  count: number;
};

const BasketCard: FC<BasketCardProps> = ({
  toggleSelect,
  onButtonClick,
  isSelected,
  productId,
  id,
  count,
}) => {
  const [data, setData] = useState<IProduct>();
  const { register, handleSubmit, setValue, getValues } = useForm<FormData>({
    defaultValues: {
      count: count,
    },
  });

  useEffect(() => {
    searchProduct(productId).then((r) => setData(r));
  }, [productId]);

  const increment = () => {
    const currentValue = getValues("count");
    setValue("count", currentValue + 1);
    updateCountProducts(id, getValues("count").toString());
  };

  const decrement = () => {
    const currentValue = getValues("count");
    if (currentValue > 1) {
      setValue("count", currentValue - 1);
    }
    updateCountProducts(productId, getValues("count").toString());
  };

  const onSubmit = (data: FormData) => {
    console.log(data, productId);
  };

  return data ? (
    <div className='flex gap-2 shadow-lg max-w-5xl p-3 justify-center items-center'>
      <label>
        <input type='checkbox' checked={isSelected} onChange={toggleSelect} />
      </label>
      <div>
        <img className='w-24 h-24' src={data.images[0]} alt='Товар' />
      </div>
      <div className='max-w-72'>
        <span className='text-base'>{data.name}</span>
        <div className='flex gap-1 mt-2'>
          <button>
            <img src={trash} alt='Удалить' />
          </button>
          <button
            className='flex items-center gap-1'
            onClick={() => {
              buyProducts(id).then(() => onButtonClick());
            }}
          >
            <img className='animate-pulse' src={thunder} alt='Молния' />
            <span className='animate-pulse text-blue-600'>Купить сейчас</span>
          </button>
        </div>
      </div>
      <div className='flex gap-2'>
        <p className='bg-gradient-to-br from-pink-500 to-pink-500 text-transparent bg-clip-text font-bold ml-2 text-lg'>
          {data.price}
        </p>
      </div>
      <form
        onSubmit={handleSubmit(onSubmit)}
        className='ml-7 flex items-center gap-2 max-h-10'
      >
        <button
          type='button'
          className='select-none text-blue-700 text-3xl px-2 py-1 rounded-lg  hover:bg-blue-100'
          onClick={decrement}
          aria-label='Уменьшить количество'
        >
          -
        </button>

        <input
          type='number'
          {...register("count", { min: 1 })}
          className='w-16 text-center border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-400 focus:outline-none'
          aria-label='Количество товаров'
        />

        <button
          type='button'
          className='select-none text-blue-700 text-3xl px-2 py-1  rounded-lg hover:bg-blue-100'
          onClick={increment}
          aria-label='Увеличить количество'
        >
          +
        </button>
      </form>
    </div>
  ) : undefined;
};

export default BasketCard;
