import { FC } from "react";
import sellerStore from "../../store/sellerStore";

const SellerMain: FC = () => {
  const name = sellerStore.shopName;
  return (
    <>
      <h1 className='font-bold text-3xl mb-5'>Добро пожаловать {name}</h1>
      <div className='grid grid-cols-1 sm:grid-cols-2 gap-6'>
        <div className='col-start-1 col-end-3 flex gap-5'>
          <div className='p-3 bg-slate-200 w-[150px] h-[75px]'>
            <h1>Баланс магазина</h1>
            <h1 className='font-bold'>3000 ₽</h1>
          </div>
          <div className='p-3 bg-slate-200 w-[150px] h-[75px]'>
            <h1>Заказов</h1>
            <h1 className='font-bold'>3000</h1>
          </div>
          <div className='p-3 bg-slate-200 w-[150px] h-[75px]'>
            <h1>Остаток товара</h1>
            <h1 className='font-bold'>3000</h1>
          </div>
        </div>
        {/* <div className='w-full'>
          <h2 className='text-center mb-4'>Заказы</h2>
          <ResponsiveContainer width='100%' height={300}>
            <LineChart data={data}>
              <XAxis dataKey='date' />
              <YAxis />
              <Tooltip />
              <CartesianGrid strokeDasharray='3 3' />
              <Line type='monotone' dataKey='orders' stroke='#000080' />
            </LineChart>
          </ResponsiveContainer>
        </div>
        <div className='w-full'>
          <h2 className='text-center mb-4'>Баланс</h2>
          <ResponsiveContainer width='100%' height={300}>
            <LineChart data={data1}>
              <XAxis dataKey='date' />
              <YAxis />
              <Tooltip />
              <CartesianGrid strokeDasharray='3 3' />
              <Line type='monotone' dataKey='balance' stroke='#000080' />
            </LineChart>
          </ResponsiveContainer>
        </div> */}
      </div>
    </>
  );
};

export default SellerMain;
