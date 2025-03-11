import { FC } from "react";
import { Link } from "react-router-dom";
import human from "../../assets/human.svg";

const UnAuthHeader: FC = () => {
  return (
    <Link to='/auth/'>
      <button className='flex flex-col items-center'>
        <img src={human} alt='' className='w-8 h-8' />
        <h1 className='font-dmSANS'>Войти</h1>
      </button>
    </Link>
  );
};

export default UnAuthHeader;
