import { FC, useEffect, useState } from "react";
import RenderCardProduct from "../components/RenderCardProduct";
import { getProductsSeller } from "../api/productAPI";
import { useParams } from "react-router-dom";
import { IProduct } from "../models/ProductI";

const SellerShopPage: FC = () => {
  const { id } = useParams<{ id: string }>();
  const [product, setProduct] = useState<IProduct[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    getProductsSeller(id!).then((r: IProduct[]) => {
      setProduct(r);
      setLoading(false);
      console.log(r);
    });
  }, [id]);

  return (
    <div className=''>
      <div>
        <h1 className='text-4xl font-bold'>
          Страница магазина <span className='text-blue-500'>{"132"}</span>
        </h1>
        <div className='mt-5 w-full h-px bg-[#EFEFEF]'></div>
      </div>
      <div className=''>
        <div className='w-fit'>
          <RenderCardProduct isLoading={loading} data={product} />
        </div>
      </div>
    </div>
  );
};

export default SellerShopPage;
