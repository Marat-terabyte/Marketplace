import { FC, useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { getBalance, topUpBalance } from "../../api/balanceAPI";
import { getInfoAccount } from "../../api/userAPI";

interface FormData {
  amount: number;
}

interface UserData {
  id: number;
  name: string;
  userType: number;
  description: null;
}

const UserMain: FC = () => {
  const [balance, setBalance] = useState(0);
  const [balanceUpdate, setBalanceUpdate] = useState(0);
  const { register, handleSubmit, reset } = useForm<FormData>({});
  const [userData, setUserData] = useState<UserData | null>(null);
  const id = localStorage.getItem("id");
  useEffect(() => {
    getInfoAccount(id!).then((r) => setUserData(r));
  }, []);

  useEffect(() => {
    getBalance().then((data) => {
      setBalance(data.account);
    });
  }, [balanceUpdate]);

  const onSubmit = (data: FormData) => {
    topUpBalance(data.amount).then(() => {
      reset();
      setBalanceUpdate((prev) => prev + 1);
    });
  };
  return (
    <div>
      <h1 className='font-bold text-3xl mb-5'>
        Добро пожаловать, {userData?.name}
      </h1>
      <div className='grid grid-cols-1 sm:grid-cols-2 gap-6'>
        <div className='w-full'>
          <div>
            <h1 className='text-2xl font-bold'>Учетные данные</h1>
            <div className='flex gap-5 mt-3'>
              <div className='flex flex-col'>
                <h1 className='text-xs text-blue-600'>ФИО</h1>
                <h1 className='font-bold'>{userData?.name}</h1>
              </div>
            </div>
          </div>

          <div className='mt-5'>
            <div className='flex flex-col'>
              <h1 className='text-2xl'>Баланс</h1>
              <h1 className='font-bold'>{balance}</h1>
              <form
                onSubmit={handleSubmit(onSubmit)}
                className='flex flex-col gap-2'
              >
                <input
                  type='number'
                  className='border border-sky-500 p-1 '
                  placeholder='Введите сумму'
                  {...register("amount", { valueAsNumber: true })}
                />
                <button
                  type='submit'
                  className='bg-blue-400 h-8 w-40 rounded-lg'
                >
                  Пополнить баланс
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default UserMain;
