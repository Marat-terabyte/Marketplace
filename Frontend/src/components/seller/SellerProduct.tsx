import { FC, useEffect, useState } from "react";
import ProductModal from "../ui/ProductModal";
import { getProductsSeller } from "../../api/productAPI";
import { Link } from "react-router-dom";

interface Product {
  id: string;
  name: string;
  category: string;
  stock: number;
  price: number;
  date: string;
  createdAt: string;
}

const SellerProduct: FC = () => {
  const [product, setProduct] = useState<Product[]>([]);
  const id = localStorage.getItem("id");
  const [isModalOpen, setIsModalOpen] = useState(false);
  console.log(product);
  useEffect(() => {
    getProductsSeller(id!).then((responce) => setProduct(responce));
  }, [id]);
  return (
    <div className='overflow-x-auto'>
      <div className='w-full flex justify-end mb-2'>
        <button
          className='border bg-slate-200 shadow-2xl w-fit text-center rounded-lg p-2 font-bold hover:bg-blue-400'
          onClick={() => setIsModalOpen(true)}
        >
          Добавить товар
        </button>
        <ProductModal
          isOpen={isModalOpen}
          onClose={() => setIsModalOpen(false)}
        />
      </div>
      <table className='min-w-full table-auto border-collapse'>
        <thead>
          <tr className='bg-gray-200'>
            <th className='border-b px-4 py-2 text-left'>ID</th>
            <th className='border-b px-4 py-2 text-left'>Имя</th>
            <th className='border-b px-4 py-2 text-left'>Категория</th>
            <th className='border-b px-4 py-2 text-left'>В наличии</th>
            <th className='border-b px-4 py-2 text-left'>Сумма</th>
            <th className='border-b px-4 py-2 text-left'>Дата</th>
          </tr>
        </thead>
        <tbody>
          {product.map((item) => (
            <tr key={item.id} className='hover:bg-gray-100'>
              <td className='border-b px-4 py-2 text-cyan-400 '>
                <Link
                  target='_blank'
                  rel='noopener noreferrer'
                  to={`/product/${item.id}`}
                >
                  {item.id}{" "}
                </Link>
              </td>
              <td className='border-b px-4 py-2'>{item.name}</td>
              <td className='border-b px-4 py-2'>{item.category}</td>
              <td className='border-b px-4 py-2'>{item.stock}</td>
              <td className='border-b px-4 py-2'>{item.price} ₽</td>
              <td className='border-b px-4 py-2'>
                {new Date(item.createdAt).toLocaleDateString("ru-RU", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                })}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default SellerProduct;
