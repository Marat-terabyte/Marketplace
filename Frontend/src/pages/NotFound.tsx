import { FC } from "react";
import { Link } from "react-router-dom";

const NotFound: FC = () => {
  return (
    <div className='mx-auto mt-10 bg-slate-200 w-11/12 h-[512px] rounded-md flex flex-col justify-between p-10 select-none'>
      <div>
        <h1 className='text-6xl font-bold'>Упс, произошла ошибка</h1>
        <h1 className='text-5xl mt-1'>
          Перезагрузите страницу или попробуйте позже
        </h1>
        <div className='mt-3 text-2xl animate-pulse text-blue-500 hover:text-pink-400 hover:animate-none w-fit'>
          <Link to={"/"}>Вернуться на главную</Link>
        </div>
      </div>

      <div className='mt- flex gap-5'>
        <span className='text-3xl font-mono'>Если ошибка сохраняется:</span>
        <button className='text-2xl hover:text-blue-600'>Сообщите</button>
      </div>
    </div>
  );
};

export default NotFound;
