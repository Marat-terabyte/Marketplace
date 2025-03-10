import React, { useState } from "react";
import { SubmitHandler, useForm } from "react-hook-form";
import { login, registration } from "../../api/AuthAPI";
import { useNavigate } from "react-router-dom";
import { IUserSignUp } from "../../models/UserI";
import userStore from "../../store/userStore";

interface IFormRegister {
  email: string;
  password: string;
  сonfirmPassword: string;
  name: string;
  surname: string;
}

const UserRegister: React.FC = () => {
  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm<IFormRegister>();
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);

  const onSubmit: SubmitHandler<IFormRegister> = async (data) => {
    const userData: IUserSignUp = {
      email: data.email,
      password: data.password,
      confirmPassword: data.сonfirmPassword,
      name: data.name,
      surname: data.surname,
    };

    try {
      await registration(userData);

      await login({
        email: userData.email,
        password: userData.password,
      });
      navigate("/");
    } catch (error) {
      console.error("Ошибка при регистрации или входе:", error);
    }
  };

  return (
    <div className='m-0 lg:mx-28'>
      <div className='max-w-sm mx-auto p-4 bg-white border rounded-lg shadow-lg'>
        <h1 className='text-2xl font-bold text-center mb-4'>Регистрация</h1>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className='mb-4'>
            <label
              htmlFor='name'
              className='block text-sm font-medium text-gray-700'
            >
              Имя
            </label>
            <input
              type='text'
              id='name'
              {...register("name", { required: "Введите имя" })}
              className='w-full px-4 py-2 border rounded-lg'
              placeholder='Введите ваше имя'
            />
            {errors.name && (
              <p className='text-red-500 text-sm'>{errors.name.message}</p>
            )}
          </div>

          <div className='mb-4'>
            <label
              htmlFor='surname'
              className='block text-sm font-medium text-gray-700'
            >
              Фамилия
            </label>
            <input
              type='text'
              id='surname'
              {...register("surname", { required: "Введите фамилию" })}
              className='w-full px-4 py-2 border rounded-lg'
              placeholder='Введите вашу фамилию'
            />
            {errors.surname && (
              <p className='text-red-500 text-sm'>{errors.surname.message}</p>
            )}
          </div>

          <div className='mb-4'>
            <label
              htmlFor='email'
              className='block text-sm font-medium text-gray-700'
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
              htmlFor='password'
              className='block text-sm font-medium text-gray-700'
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
              <p className='text-red-500 text-sm'>{errors.password.message}</p>
            )}
          </div>

          <div className='mb-4'>
            <label
              htmlFor='сonfirmPassword'
              className='block text-sm font-medium text-gray-700'
            >
              Подтверждение пароля
            </label>
            <input
              type='password'
              id='сonfirmPassword'
              {...register("сonfirmPassword", {
                required: "Подтвердите пароль",
                validate: (value) =>
                  value === watch("password") || "Пароли не совпадают",
              })}
              className='w-full px-4 py-2 border rounded-lg'
              placeholder='Подтвердите ваш пароль'
            />
            {errors.сonfirmPassword && (
              <p className='text-red-500 text-sm'>
                {errors.сonfirmPassword.message}
              </p>
            )}
          </div>

          <button
            type='submit'
            className='w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600'
          >
            Зарегистрироваться
          </button>
        </form>
        <div className='flex justify-between mt-2'>
          <button
            className='text-blue-600 hover:text-blue-900'
            onClick={() => userStore.setActiveTab("auth")}
          >
            Есть аккаунт?
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
  );
};

export default UserRegister;
