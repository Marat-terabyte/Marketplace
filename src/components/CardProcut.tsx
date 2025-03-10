import { FC } from "react";
import { Link, useNavigate } from "react-router-dom";
import { ICardData } from "../models/CardI";

const CardProcut: FC<ICardData> = ({
  title,
  img,
  count,
  storename,
  id,
  sellerId,
}) => {
  const navigate = useNavigate();

  return (
    <Link to={`/product/${id}`} className='block w-52 sm:h-fit sm:max-w-md'>
      <div>
        <img
          src={img}
          alt=''
          className='rounded-t-lg bg-cover h-44 w-44 lg:h-48 lg:w-48'
        />
        <div className='flex gap-2 mt-2 justify-between pr-5'>
          <h1 className='bg-gradient-to-br text-lg from-pink-500 to-pink-500 text-transparent bg-clip-text text-[#10c44c] font-bold'>
            {count}
          </h1>
          <span
            className='text-xs text-blue-700 hover:underline cursor-pointer'
            onClick={(e) => {
              e.preventDefault();
              navigate(`/shop/${sellerId}`);
            }}
          >
            {storename}
          </span>
        </div>
        <h1 className='truncate'>{title}</h1>
      </div>
    </Link>
  );
};

export default CardProcut;
