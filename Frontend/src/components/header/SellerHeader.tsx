import { FC } from "react";
import { Link } from "react-router-dom";
import store from "../../assets/store_24dp_000000_FILL0_wght400_GRAD0_opsz24.svg";
import sellerStore from "../../store/sellerStore";
const SellerHeader: FC = () => {
  const name = sellerStore.shopName;
  return (
    <div className='flex gap-2'>
      <Link to='/seller'>
        <button className=' flex flex-col items-center'>
          <img src={store} alt='' className='min-w-max ' />
          <h1 className='font-dmSANS'>{name}</h1>
        </button>
      </Link>
    </div>
  );
};

export default SellerHeader;
