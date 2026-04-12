export const TRANSACTION_TYPE = {
  EXPENSE: 0,
  INCOME: 1,
} as const;

export const TRANSACTION_TYPE_LABELS: Record<number, string> = {
  [TRANSACTION_TYPE.EXPENSE]: "Despesa",
  [TRANSACTION_TYPE.INCOME]: "Receita",
};

export const CATEGORY_PURPOSE = {
  EXPENSE: 0,
  INCOME: 1,
  BOTH: 2,
} as const;

export const CATEGORY_PURPOSE_LABELS: Record<number, string> = {
  [CATEGORY_PURPOSE.EXPENSE]: "Despesa",
  [CATEGORY_PURPOSE.INCOME]: "Receita",
  [CATEGORY_PURPOSE.BOTH]: "Ambos",
};

export const TOKEN_KEY = "token";
export const USER_NAME_KEY = "userName";
