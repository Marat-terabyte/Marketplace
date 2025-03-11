import { FC } from "react";
import { useForm, SubmitHandler, useFieldArray } from "react-hook-form";
import { addProduct } from "../../api/productAPI";

interface ProductFormData {
  name: string;
  descriptions: string;
  category: string;
  price: number;
  stock: number;
  images: string;
  attributes: { key: string; value: string }[];
}

interface ModalProps {
  isOpen: boolean;
  onClose: () => void;
}

const ProductModal: FC<ModalProps> = ({ isOpen, onClose }) => {
  const {
    register,
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<ProductFormData>({
    defaultValues: { attributes: [{ key: "", value: "" }] },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "attributes",
  });

  if (!isOpen) return null;

  const submitHandler: SubmitHandler<ProductFormData> = (data) => {
    const imagesArray = data.images.split(",").map((img) => img.trim());

    const attributesObject = data.attributes.reduce((acc, cur) => {
      if (cur.key) {
        acc[cur.key] = cur.value;
      }
      return acc;
    }, {} as { [key: string]: string });
    addProduct(
      data.name,
      data.descriptions,
      data.category,
      data.price,
      data.stock,
      imagesArray,
      attributesObject
    );
    onClose();
  };

  return (
    <div className='fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50'>
      <div className='bg-white p-6 rounded-lg shadow-lg w-[400px] relative max-h-[80vh] overflow-y-auto custom-scroll'>
        <button
          className='absolute top-2 right-3 text-xl font-bold text-gray-700 hover:text-black'
          onClick={onClose}
        >
          ✖
        </button>
        <h2 className='text-xl font-bold mb-4'>Добавить продукт</h2>
        <form
          onSubmit={handleSubmit(submitHandler)}
          className='flex flex-col gap-3'
        >
          <input
            className='border p-2 rounded'
            placeholder='Название'
            {...register("name", { required: "Название обязательно" })}
          />
          {errors.name && <p className='text-red-500'>{errors.name.message}</p>}

          <textarea
            className='border p-2 rounded'
            placeholder='Описание'
            {...register("descriptions", { required: "Описание обязательно" })}
          ></textarea>
          {errors.descriptions && (
            <p className='text-red-500'>{errors.descriptions.message}</p>
          )}

          <input
            className='border p-2 rounded'
            placeholder='Категория'
            {...register("category", { required: "Категория обязательна" })}
          />
          {errors.category && (
            <p className='text-red-500'>{errors.category.message}</p>
          )}

          <input
            className='border p-2 rounded'
            type='number'
            placeholder='Цена'
            {...register("price", {
              required: "Цена обязательна",
              valueAsNumber: true,
            })}
          />
          {errors.price && (
            <p className='text-red-500'>{errors.price.message}</p>
          )}

          <input
            className='border p-2 rounded'
            type='number'
            placeholder='Количество на складе'
            {...register("stock", {
              required: "Количество обязательно",
              valueAsNumber: true,
            })}
          />
          {errors.stock && (
            <p className='text-red-500'>{errors.stock.message}</p>
          )}

          <input
            className='border p-2 rounded'
            placeholder='URL изображений (через запятую)'
            {...register("images", { required: "Минимум одно изображение" })}
          />
          {errors.images && (
            <p className='text-red-500'>{errors.images.message}</p>
          )}

          <h3 className='font-bold mt-2'>Атрибуты</h3>
          {fields.map((field, index) => (
            <div key={field.id} className='flex gap-2'>
              <input
                className='border p-2 rounded w-1/2'
                placeholder='Ключ'
                {...register(`attributes.${index}.key`, {
                  required: "Обязательно",
                })}
              />
              <input
                className='border p-2 rounded w-1/2'
                placeholder='Значение'
                {...register(`attributes.${index}.value`, {
                  required: "Обязательно",
                })}
              />
              <button
                type='button'
                className='text-red-500'
                onClick={() => remove(index)}
              >
                ✖
              </button>
            </div>
          ))}
          <button
            type='button'
            className='text-blue-500'
            onClick={() => append({ key: "", value: "" })}
          >
            + Добавить атрибут
          </button>

          <button
            className='bg-blue-600 text-white p-2 rounded hover:bg-blue-700 mt-4'
            type='submit'
          >
            Сохранить
          </button>
        </form>
      </div>
    </div>
  );
};

export default ProductModal;
