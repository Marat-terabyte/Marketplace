import { FC } from "react";

const CardProduct_skeleton: FC = () => {
  return (
    <div className='w-52  sm:h-fit sm:max-w-md animate-pulse'>
      <div className='rounded-t-lg  bg-gray-300 h-44 w-44 lg:h-48 lg:w-48'></div>
      <div className='flex gap-2 mt-2'>
        <div className='bg-gray-300 rounded-md w-16 h-6'></div>
        <div className='bg-gray-300 rounded-md w-12 h-4'></div>
        <div className='bg-gray-300 rounded-md w-10 h-4'></div>
      </div>

      <div className='mt-2 bg-gray-300 rounded-md w-full h-6'></div>
    </div>
  );
};

export default CardProduct_skeleton;
