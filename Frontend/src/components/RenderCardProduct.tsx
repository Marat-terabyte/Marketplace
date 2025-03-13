import CardProcut from "./CardProcut";
import CardProduct_skeleton from "./skeleton/CardProduct_skeleton";

import { IProduct } from "../models/ProductI";
import { FC } from "react";

interface RenderCardProductProps {
  isLoading: boolean;
  data: IProduct[];
}

const RenderCardProduct: FC<RenderCardProductProps> = ({ isLoading, data }) => {
  const skeletonArray = Array(20).fill(0);
  return (
    <>
      {isLoading
        ? skeletonArray.map((_, index) => <CardProduct_skeleton key={index} />)
        : data.map((item, index) => (
            <CardProcut
              id={item.id}
              key={index}
              title={item.name}
              img={item.images?.[0]}
              count={item.price}
              storename={item.storename}
              sellerId={item.sellerId}
            />
          ))}
    </>
  );
};

export default RenderCardProduct;
