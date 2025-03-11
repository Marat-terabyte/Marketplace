/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    extend: {
      fontFamily: {
        dmSANS: ["DmSans"],
        integralCF: ["IntegtalCF"],
      },
      colors: {
        button: "#3D00B7",
      },
    },
  },
  plugins: [],
};
