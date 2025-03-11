import { FC } from "react";

const ProductItem_skeleton: FC = () => {
  return (
    <div className='flex justify-evenly h-dvh animate-pulse'>
      <div className='flex gap-2'>
        <div className='w-16 h-72 bg-gray-300 rounded'></div>

        <div className='w-72 h-96 bg-gray-300 rounded'></div>
      </div>

      <div className='flex flex-col gap-4'>
        <div className='w-64 h-8 bg-gray-300 rounded'></div>

        <div className='w-24 h-4 bg-gray-300 rounded'></div>

        <div className='flex gap-2'>
          <div className='w-6 h-6 bg-gray-300 rounded-full'></div>
          <div className='w-10 h-4 bg-gray-300 rounded'></div>
          <div className='w-6 h-6 bg-gray-300 rounded-full'></div>
          <div className='w-10 h-4 bg-gray-300 rounded'></div>
        </div>

        <div>
          <div className='w-32 h-4 bg-gray-300 rounded mb-2'></div>
          <div className='flex gap-2'>
            <div className='w-14 h-16 bg-gray-300 rounded'></div>
            <div className='w-14 h-16 bg-gray-300 rounded'></div>
            <div className='w-14 h-16 bg-gray-300 rounded'></div>
          </div>
        </div>

        <div>
          <div className='flex justify-between'>
            <div className='w-24 h-4 bg-gray-300 rounded'></div>
            <div className='w-24 h-4 bg-gray-300 rounded'></div>
          </div>
          <div className='mt-3 space-y-2'>
            <div className='flex justify-between'>
              <div className='w-16 h-4 bg-gray-300 rounded'></div>
              <div className='w-16 h-4 bg-gray-300 rounded'></div>
            </div>
            <div className='flex justify-between'>
              <div className='w-16 h-4 bg-gray-300 rounded'></div>
              <div className='w-16 h-4 bg-gray-300 rounded'></div>
            </div>
          </div>
        </div>

        <div>
          <div className='w-40 h-4 bg-gray-300 rounded mb-2'></div>
          <div className='flex gap-2'>
            <div className='w-14 h-16 bg-gray-300 rounded'></div>
            <div className='w-14 h-16 bg-gray-300 rounded'></div>
            <div className='w-14 h-16 bg-gray-300 rounded'></div>
          </div>
        </div>
      </div>

      <div className='flex flex-col gap-4 justify-evenly shadow-lg h-fit w-fit min-h-32 min-w-48 bg-slate-100 pl-2'>
        <div className='flex gap-2'>
          <div className='w-16 h-8 bg-gray-300 rounded'></div>
          <div className='w-16 h-8 bg-gray-300 rounded line-through'></div>
        </div>
        <div>
          <div className='w-32 h-10 bg-gray-300 rounded'></div>
        </div>
      </div>
    </div>
  );
};

export default ProductItem_skeleton;
