import { createBrowserRouter, RouterProvider } from "react-router-dom";
import NotFound from "./NotFound";
import { privateRoutes, publicRoutes } from "../Routes";
import userStore from "../store/userStore";
import { FC } from "react";
import Layout from "./Layout";
import { observer } from "mobx-react-lite";

const App: FC = observer(() => {
  const auth = userStore.userRole !== "guest";
  const routes = auth ? [...publicRoutes, ...privateRoutes] : publicRoutes;
  const router = createBrowserRouter([
    {
      path: "/",
      element: <Layout />,
      errorElement: <NotFound />,
      children: routes.map(({ path, Component }) => ({
        path,
        element: Component ? <Component /> : undefined,
      })),
    },
  ]);

  return (
    <>
      <RouterProvider router={router} />
    </>
  );
});

export default App;
