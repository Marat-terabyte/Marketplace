import loupe from "../assets/loupe-search.svg";
import { Link } from "react-router-dom";
import { FC } from "react";
import AuthHeader from "./header/AuthHeader";
import UnAuthHeader from "./header/UnAuthHeader";
import SellerHeader from "./header/SellerHeader";
import userStore from "../store/userStore";
import { observer } from "mobx-react-lite";

const Header: FC = observer(() => {
  const activeTab = userStore.userRole;

  return (
    <div className='relative h-28  '>
      <div className='flex items-center justify-between h-full gap-9 px-4'>
        <div className='flex items-center -mt-3'>
          <h1 className='text-[#3D00B7] font-integralCF text-2xl'>
            <Link to={"/"}>
              <button className='text-7xl leading-[5rem]'>Ozon</button>
            </Link>
          </h1>
        </div>

        <div className='relative flex-1'>
          <input
            placeholder='Найти что-нибудь'
            type='text'
            className='font-dmSANS px-4 py-2 border rounded-md border-black w-full'
          />
          <img
            src={loupe}
            className='w-5 h-5 text-gray-400 absolute right-3 top-1/2 transform -translate-y-1/2'
          />
        </div>

        <div className='flex items-center'>
          {activeTab === "consumer" && <AuthHeader />}
          {activeTab === "seller" && <SellerHeader />}
          {activeTab === "guest" && <UnAuthHeader />}
        </div>
      </div>
      <div className='absolute top-24 left-0 w-full h-px bg-[#EFEFEF]'></div>
    </div>
  );
});

export default Header;
