import { FC, useEffect, useState } from "react";
import RenderCardProduct from "../components/RenderCardProduct";
import { searchMoreProduct } from "../api/productAPI";
import productStore from "../store/productStore";
import NotFound from "./NotFound";

const HomePage: FC = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [eror, setEror] = useState();
  useEffect(() => {
    setIsLoading(true);
    if (
      !Array.isArray(productStore.products) ||
      productStore.products.length === 0
    ) {
      searchMoreProduct()
        .then((response) => {
          productStore.setProducts(response);
        })
        .catch((error) => {
          setEror(error);
        })
        .finally(() => {
          setTimeout(() => setIsLoading(false), 200);
        });
    } else {
      setIsLoading(false);
    }
  }, []);
  console.log(productStore.products);
  return (
    <>
      <div className='bg-[#ffffff] m-0 '>
        <div className='max-w-full  flex '>
          {eror ? (
            <NotFound />
          ) : (
            <div className='grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-4 xl:grid-cols-5 2xl:grid-cols-5 gap-8 mt-5 w-full h-fit'>
              <RenderCardProduct
                isLoading={isLoading}
                data={productStore.products}
              />
            </div>
          )}
        </div>
      </div>
    </>
  );
};

export default HomePage;
