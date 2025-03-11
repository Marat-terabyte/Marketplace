import { $host } from "./index";

export const getInfoAccount = async (id: string) => {
  const { data } = await $host.get(`api/account/${id}`, {});

  return data;
};
