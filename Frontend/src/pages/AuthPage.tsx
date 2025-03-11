import UserRegister from "../components/auth/UserRegister";
import UserAuth from "../components/auth/Auth";
import { FC } from "react";
import SellerRegister from "../components/auth/SellerRegister";
import { observer } from "mobx-react-lite";
import userStore from "../store/userStore";

const UserAuthPage: FC = observer(() => {
  const activeTab = userStore.activeTab;
  return (
    <>
      {activeTab === "userReg" && <UserRegister />}
      {activeTab === "auth" && <UserAuth />}
      {activeTab === "sellerReg" && <SellerRegister />}
    </>
  );
});

export default UserAuthPage;
