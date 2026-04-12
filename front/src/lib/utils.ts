import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { isAxiosError } from 'axios';

type Envelope<T> = {
  data?: T | null;
};

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function getApiErrorMessages(error: unknown, fallback: string): string {
  if (isAxiosError(error)) {
    const messages: string[] | undefined =
      error.response?.data?.errors ?? error.response?.data?.errorMessages;
    if (messages?.length) {
      return messages.join('\n');
    }
  }
  return fallback;
}

const currencyFormatter = new Intl.NumberFormat('pt-BR', {
  style: 'currency',
  currency: 'BRL',
});

export function formatCurrency(value: number | undefined | null): string {
  return currencyFormatter.format(value ?? 0);
}

export function unwrapApiData<T>(value: unknown): T | undefined {
  if (value == null) return undefined;

  if (typeof value === 'object' && 'data' in value) {
    return (value as Envelope<T>).data ?? undefined;
  }

  return value as T;
}
