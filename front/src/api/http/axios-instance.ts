import axios, { AxiosError } from 'axios';
import type { AxiosRequestConfig } from 'axios';

const TOKEN_KEY = 'token';
const API_BASE_URL =
  (
    globalThis as {
      process?: { env?: { VITE_API_ENDPOINT?: string } };
    }
  ).process?.env?.VITE_API_ENDPOINT ?? 'https://localhost:7248';

export const httpClient = axios.create({
  baseURL: API_BASE_URL,
  timeout: 30_000,
});

const normalizeHeaders = (
  headers?: HeadersInit,
): AxiosRequestConfig['headers'] | undefined => {
  if (!headers) return undefined;

  if (headers instanceof Headers) {
    return Object.fromEntries(headers.entries());
  }

  if (Array.isArray(headers)) {
    return Object.fromEntries(headers);
  }

  return headers;
};

httpClient.interceptors.request.use((config) => {
  const token = localStorage.getItem(TOKEN_KEY);
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

httpClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401 && window.location.pathname !== '/login') {
      localStorage.removeItem(TOKEN_KEY);
      localStorage.removeItem('userName');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  },
);

export async function axiosInstance<T>(
  urlOrConfig: string | AxiosRequestConfig,
  options?: RequestInit,
): Promise<T> {
  const requestConfig: AxiosRequestConfig =
    typeof urlOrConfig === 'string'
      ? {
          url: urlOrConfig,
          method: options?.method as AxiosRequestConfig['method'],
          headers: normalizeHeaders(options?.headers),
          data: options?.body,
          signal: options?.signal ?? undefined,
        }
      : urlOrConfig;

  const { data } = await httpClient<T>(requestConfig);
  return data;
}

export default axiosInstance;
