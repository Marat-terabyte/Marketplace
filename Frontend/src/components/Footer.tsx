import { FC } from "react";

const Footer: FC = () => {
  return (
    <footer className='shadow-2xl text-black py-6 mt-10'>
      <div className='container mx-auto text-center'>
        <p className='text-sm'>
          &copy; {new Date().getFullYear()} MyApp. Все права защищены.
        </p>
        <div className='flex justify-center gap-4 mt-2'>
          <a href='#' className='hover:text-gray-400'>
            Политика конфиденциальности
          </a>
          <span>|</span>
          <a href='#' className='hover:text-gray-400'>
            Условия использования
          </a>
          <span>|</span>
          <a href='#' className='hover:text-gray-400'>
            Связаться с нами
          </a>
        </div>
      </div>
    </footer>
  );
};

export default Footer;
