import { $host } from "./index";

export const topUpBalance = async (amount: number) => {
  const { data } = await $host.post(`api/balance/topup`, {
    amount: amount,
  });

  return data;
};
export const getBalance = async () => {
  const { data } = await $host.get(`api/balance`);

  return data;
};
