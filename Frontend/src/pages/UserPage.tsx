import { FC, useState } from "react";
import userStore from "../store/userStore";
import { useNavigate } from "react-router-dom";
import UserMain from "../components/user/UserMain";
import UserOrder from "../components/user/UserOrder";

const UserPage: FC = () => {
  const navigate = useNavigate();
  const [activeTab, setActiveTab] = useState<string>("main");
  const exit = () => {
    localStorage.removeItem("storeName");
    localStorage.setItem("userRole", "guest");
    localStorage.removeItem("id");
    userStore.setUserRole("guest");
    navigate("/");
  };

  return (
    <>
      <div className='flex gap-4'>
        <div className='flex flex-col gap-5 bg-slate-200 py-3 min-h-[495px]'>
          <button
            className='border w-36 text-start pl-3 hover:cursor-pointer hover:underline'
            onClick={() => setActiveTab("main")}
          >
            Личный кабинет
          </button>
          <button
            className='border w-36 text-start pl-3 hover:cursor-pointer hover:underline'
            onClick={() => setActiveTab("orders")}
          >
            Заказы
          </button>

          <button
            className='border w-36 text-start pl-3 text-xl text-blue-500 hover:cursor-pointer hover:underline mt-auto'
            onClick={() => exit()}
          >
            Выйти
          </button>
        </div>
        <div className='w-full'>
          {activeTab === "main" && <UserMain />}
          {activeTab === "orders" && <UserOrder />}
        </div>
      </div>
    </>
  );
};

export default UserPage;
