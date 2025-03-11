import React, { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { login } from "../../api/AuthAPI";
import { useNavigate } from "react-router-dom";
import userStore from "../../store/userStore";

interface FormData {
  name: string;
  surname: string;
  email: string;
  password: string;
  confirmPassword: string;
}

const Auth: React.FC = () => {
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<FormData>();
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [status, setStatus] = useState(false);

  const onSubmit = async (data: FormData) => {
    try {
      await login(data).then((data) => setStatus(data));
    } catch (error) {
      console.error("Ошибка при авторизации:", error);
    }
  };

  useEffect(() => {
    if (status) {
      navigate("/");
    }
  }, [navigate, status]);
  return (
    <>
      <div className='m-0 lg:mx-28'>
        <div className='max-w-sm mx-auto p-4 bg-white border rounded-lg shadow-lg'>
          <h2 className='text-2xl font-bold text-center mb-4'>Авторизация</h2>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className='mb-4'>
              <label
                className='block text-sm font-medium text-gray-700'
                htmlFor='email'
              >
                Email
              </label>
              <input
                type='email'
                id='email'
                {...register("email", {
                  required: "Введите email",
                  pattern: {
                    value: /\S+@\S+\.\S+/,
                    message: "Введите корректный email",
                  },
                })}
                className='w-full px-4 py-2 border rounded-lg'
                placeholder='Введите ваш email'
              />
              {errors.email && (
                <p className='text-red-500 text-sm'>{errors.email.message}</p>
              )}
            </div>

            <div className='mb-4'>
              <label
                className='block text-sm font-medium text-gray-700'
                htmlFor='password'
              >
                Пароль
              </label>
              <div className='relative'>
                <input
                  type={showPassword ? "text" : "password"}
                  id='password'
                  {...register("password", {
                    required: "Введите пароль",
                    minLength: {
                      value: 8,
                      message: "Пароль должен содержать минимум 8 символов",
                    },
                  })}
                  className='w-full px-4 py-2 border rounded-lg'
                  placeholder='Введите ваш пароль'
                />
                <button
                  type='button'
                  onClick={() => setShowPassword((prev) => !prev)}
                  className='absolute right-2 top-2 text-blue-500 hover:text-blue-700'
                >
                  {showPassword ? "Скрыть" : "Показать"}
                </button>
              </div>
              {errors.password && (
                <p className='text-red-500 text-sm'>
                  {errors.password.message}
                </p>
              )}
            </div>

            <div className='mb-4'>
              <label
                className='block text-sm font-medium text-gray-700'
                htmlFor='confirmPassword'
              >
                Подтверждение пароля
              </label>
              <input
                type='password'
                id='confirmPassword'
                {...register("confirmPassword", {
                  required: "Подтвердите пароль",
                  validate: (value) =>
                    value === watch("password") || "Пароли не совпадают",
                })}
                className='w-full px-4 py-2 border rounded-lg'
                placeholder='Подтвердите ваш пароль'
              />
              {errors.confirmPassword && (
                <p className='text-red-500 text-sm'>
                  {errors.confirmPassword.message}
                </p>
              )}
            </div>

            <button
              type='submit'
              className='w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600'
            >
              Авторизоваться
            </button>
          </form>
          <div className='flex justify-between mt-2 '>
            <button
              className='text-blue-600 hover:text-blue-900'
              onClick={() => userStore.setActiveTab("userReg")}
            >
              Нет аккаунта?
            </button>
            <button
              className='text-blue-600 hover:text-blue-900'
              onClick={() => userStore.setActiveTab("sellerReg")}
            >
              Вы продавец?
            </button>
          </div>
        </div>
      </div>
    </>
  );
};

export default Auth;
