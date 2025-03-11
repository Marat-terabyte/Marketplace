import React, { FC } from "react";
import { IProduct } from "../models/ProductI";

import NotFound from "../pages/NotFound";
import { addProductBasket } from "../api/basketAPI";
interface ProductItemProps {
  data: IProduct | null;
  id: string;
}

const ProductItem: FC<ProductItemProps> = ({ data, id }) => {
  if (!data) {
    return <NotFound />;
  }
  return (
    <div className='flex justify-evenly h-dvh mt-20'>
      <div className='flex gap-2'>
        <div className='relative'>
          <ul className='max-h-72 custom-scroll overflow-y-scroll invisible-scroll scrollbar-hidden'>
            {data.images.map((data, key) => (
              <li className='' key={key}>
                <img className='w-16 h-20' src={data} key={key} alt='' />
              </li>
            ))}
          </ul>
        </div>
        <img
          className=' max-w-72 min-w-72 max-h-96'
          src={data.images[0]}
          alt=''
        />
      </div>

      <div className='flex flex-col h-fit min-w-96'>
        <h1 className='font-dmSANS text-3xl  n max-w-96  duration-500 ease-in-out  break-all'>
          {data.name}
        </h1>

        <div className='mt-5'>
          <div className='mt-5'>
            <div className='mt-5 shadow-lg'>
              <h1>О товаре</h1>
              <dl className='flex justify-between'>
                <dt>Категория</dt>
                <dd>{data.category}</dd>
              </dl>
              <dl className='mt-3 flex justify-between '>
                {data.attributes &&
                  Object.entries(data.attributes).map(([key, value]) => (
                    <React.Fragment key={key}>
                      <div>
                        <dt>{key}</dt>
                        <dd>
                          {typeof value === "object"
                            ? JSON.stringify(value)
                            : value}
                        </dd>
                      </div>
                    </React.Fragment>
                  ))}
              </dl>
            </div>
          </div>
        </div>
      </div>

      <div className='flex flex-col justify-evenly shadow-lg h-fit w-fit min-h-32 min-w-48 bg-slate-100  '>
        <div className='flex gap-2 ml-3'>
          <h1 className='text-white rounded-2xl font-bold bg-[#10c44c] text-2xl'>
            1000 Р
          </h1>
        </div>
        <div className='mt-5'>
          <button
            className='h-fit p-2 w-56 rounded-lg shadow-xl'
            type='button'
            onClick={() => addProductBasket(id)}
          >
            Добавить в корзину
          </button>
        </div>
      </div>
    </div>
  );
};

export default ProductItem;
