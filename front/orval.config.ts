import { defineConfig } from 'orval';
import dotenv from 'dotenv';
dotenv.config();

const toCamelCase = (value: string): string => {
  const normalized = value
    .replace(/Controller_/g, '')
    .replace(/[{}]/g, '')
    .replace(/[^a-zA-Z0-9]+(.)/g, (_, group: string) => group.toUpperCase())
    .replace(/^[^a-zA-Z]+/, '');

  if (!normalized) return 'operation';
  return normalized.charAt(0).toLowerCase() + normalized.slice(1);
};

const resolveOperationName = (
  operation: unknown,
  route: string,
  verb: string,
): string => {
  const operationLike = operation as {
    operationId?: string;
  };

  const fallback = `${verb ?? 'operation'}_${route ?? 'root'}`;

  return toCamelCase(operationLike.operationId ?? fallback);
};

const API_URL = process.env.VITE_API_ENDPOINT || 'https://localhost:7248';
const LOCAL_SCHEMA = './src/api/openapi.json';
const SCHEMA_PATH = process.env.USE_LOCAL_SCHEMA
  ? LOCAL_SCHEMA
  : `${API_URL}/openapi/v1.json`;
const OUTPUT_WORKSPACE = 'src/api/generated';

export default defineConfig({
  api: {
    input: { target: SCHEMA_PATH },
    output: {
      mode: 'tags-split', // Melhora a organização de arquivos por domínio
      workspace: OUTPUT_WORKSPACE,
      target: './endpoints',
      schemas: './models',
      client: 'react-query',
      indexFiles: true,
      override: {
        mutator: {
          path: '../http/axios-instance.ts', // Caminho relativo ao workspace
          name: 'axiosInstance',
        },
        operationName: (operation, route, verb) =>
          resolveOperationName(operation, route, verb),
        query: {
          useQuery: true,
          useInfinite: true,
          useInfiniteQueryParam: 'page', // Define o padrão da API
        },
        fetch: {
          includeHttpResponseReturnType: false,
        },
      },
    },
    hooks: {
      afterAllFilesWrite: 'eslint --fix', // Mais rápido e cobre prettier se integrado
    },
  },

  zod: {
    input: { target: SCHEMA_PATH },
    output: {
      mode: 'tags-split',
      workspace: OUTPUT_WORKSPACE,
      target: './validation',
      client: 'zod',
      override: {
        operationName: (operation, route, verb) =>
          `validate${resolveOperationName(operation, route, verb)}`,
      },
    },
  },
});
