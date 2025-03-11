import { FC } from "react";
import { Link } from "react-router-dom";
import basketShop from "../../assets/shopping_basket_24dp_B7B7B7_FILL0_wght400_GRAD0_opsz24.svg";
import user from "../../assets/human.svg";

const AuthHeader: FC = () => {
  return (
    <div className='flex gap-5'>
      <Link to='/basket'>
        <button className='flex flex-col items-center'>
          <img src={basketShop} alt='' className='w-8 h-8' />
        </button>
      </Link>
      <Link to={`/user`}>
        <button className='flex flex-col items-center'>
          <img src={user} alt='' className='w-8 h-8' />
        </button>
      </Link>
    </div>
  );
};

export default AuthHeader;
