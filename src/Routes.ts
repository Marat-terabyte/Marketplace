import Basket from "./pages/BasketPage";
import DevicePage from "./pages/DevicePage";
import HomePage from "./pages/HomePage";
import UserAuthPage from "./pages/AuthPage";
import {
  AUTH_ROUTE,
  BASKET_ROUTE,
  DEVICE_ROUTE,
  SELLER_ROUTE,
  SELLER_SHOP_ROUTE,
  SHOP_ROUTE,
  USER_ROUTE,
} from "./utils/const";
import SellerPersonalPage from "./pages/SellerPersonalPage";
import SellerShopPage from "./pages/SellerShopPage";
import UserPage from "./pages/UserPage";

export const privateRoutes = [
  {
    path: SELLER_ROUTE,
    Component: SellerPersonalPage,
  },

  {
    path: BASKET_ROUTE,
    Component: Basket,
  },
  {
    path: USER_ROUTE,
    Component: UserPage,
  },
];

export const publicRoutes = [
  {
    path: AUTH_ROUTE,
    Component: UserAuthPage,
  },
  {
    path: SELLER_SHOP_ROUTE + "/:id",
    Component: SellerShopPage,
  },
  {
    path: DEVICE_ROUTE + "/:id",
    Component: DevicePage,
  },
  {
    path: SHOP_ROUTE,
    Component: HomePage,
  },
];
