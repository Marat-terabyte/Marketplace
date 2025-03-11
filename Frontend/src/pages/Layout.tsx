import { Outlet } from "react-router-dom";
import Header from "../components/Header";
import Footer from "../components/Footer";

const Layout = () => (
  <>
    <div className='flex flex-col justify-between h-dvh'>
      <Header />
      <div className='flex-1 pt-1'>
        <Outlet />
      </div>
      <Footer />
    </div>
  </>
);

export default Layout;
