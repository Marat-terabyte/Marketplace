import { FC, useEffect, useState } from "react";
import { searchProduct } from "../api/productAPI";
import { useParams } from "react-router-dom";
import { IProduct } from "../models/ProductI";
import ProductItem from "../components/ProductItem";
import ProductItem_skeleton from "../components/skeleton/ProductItem_skeleton";

const DevicePage: FC = () => {
  const { id } = useParams<{ id: string }>();
  const [data, setData] = useState<IProduct | null>(null);
  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    searchProduct(id!).then((r: IProduct) => {
      setData(r);
      setLoading(true);
    });
  }, []);

  return (
    <>
      {loading ? (
        <ProductItem data={data!} id={id!} />
      ) : (
        <ProductItem_skeleton />
      )}
    </>
  );
};

export default DevicePage;
