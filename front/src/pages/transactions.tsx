import { useState, useMemo } from 'react';
import {
  getCoreRowModel,
  useReactTable,
  type ColumnDef,
} from '@tanstack/react-table';
import { toast } from 'sonner';
import { ArrowLeftRight, Plus } from 'lucide-react';
import { useQueryClient } from '@tanstack/react-query';

import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { DataTable } from '@/components/shared/data-table';
import { EmptyState } from '@/components/shared/empty-state';
import { TransactionFormDialog } from '@/components/features/transactions/transaction-form-dialog';

import {
  useGetApiPerson,
  useGetApiCategory,
  useGetApiTransactionPersonPersonId,
  usePostApiTransaction,
  getGetApiTransactionPersonPersonIdQueryKey,
  getGetApiTransactionTotalsQueryKey,
  type ResponseShortPersonJson,
  type ResponseShortCategoryJson,
  type ResponseShortTransactionJson,
} from '@/api/generated';
import { TRANSACTION_TYPE } from '@/constants/enums';
import {
  formatCurrency,
  getApiErrorMessages,
  unwrapApiData,
} from '@/lib/utils';

const EMPTY_PERSONS: ResponseShortPersonJson[] = [];
const EMPTY_CATEGORIES: ResponseShortCategoryJson[] = [];
const EMPTY_TRANSACTIONS: ResponseShortTransactionJson[] = [];

export default function TransactionsPage() {
  const queryClient = useQueryClient();
  const [formOpen, setFormOpen] = useState(false);
  const [selectedPersonId, setSelectedPersonId] = useState<number | null>(null);

  const { data: personsResponse } = useGetApiPerson();
  const { data: categoriesResponse } = useGetApiCategory();
  const persons =
    unwrapApiData<ResponseShortPersonJson[]>(personsResponse?.data) ??
    EMPTY_PERSONS;
  const categories =
    unwrapApiData<ResponseShortCategoryJson[]>(categoriesResponse?.data) ??
    EMPTY_CATEGORIES;

  const categoryMap = useMemo(() => {
    const map = new Map<number, string>();
    for (const cat of categories) {
      if (cat.id != null) map.set(cat.id, cat.description ?? '');
    }
    return map;
  }, [categories]);

  const { data: transactionsResponse, isLoading: isLoadingTransactions } =
    useGetApiTransactionPersonPersonId(selectedPersonId ?? 0, {
      query: { enabled: selectedPersonId != null },
    });
  const transactions = useMemo(() => {
    const unwrapped = unwrapApiData<ResponseShortTransactionJson[]>(
      transactionsResponse?.data,
    );

    return Array.isArray(unwrapped) ? unwrapped : EMPTY_TRANSACTIONS;
  }, [transactionsResponse?.data]);

  const createMutation = usePostApiTransaction({
    mutation: {
      onSuccess: () => {
        toast.success('Transação registrada com sucesso!');
        setFormOpen(false);
        if (selectedPersonId != null) {
          queryClient.invalidateQueries({
            queryKey:
              getGetApiTransactionPersonPersonIdQueryKey(selectedPersonId),
          });
        }
        queryClient.invalidateQueries({
          queryKey: getGetApiTransactionTotalsQueryKey(),
        });
      },
      onError: (error) =>
        toast.error(getApiErrorMessages(error, 'Erro ao registrar transação.')),
    },
  });

  function handleFormSubmit(data: {
    description: string;
    value: number;
    type: number;
    categoryId: number;
    personId: number;
  }) {
    createMutation.mutate({ data });
  }

  const columns: ColumnDef<ResponseShortTransactionJson, unknown>[] = useMemo(
    () => [
      {
        accessorKey: 'description',
        header: 'Descrição',
      },
      {
        accessorKey: 'value',
        header: 'Valor',
        cell: ({ row }) => (
          <span
            className={
              row.original.type === TRANSACTION_TYPE.INCOME
                ? 'font-medium text-emerald-600 dark:text-emerald-400'
                : 'font-medium text-red-600 dark:text-red-400'
            }
          >
            {row.original.type === TRANSACTION_TYPE.INCOME ? '+' : '-'}{' '}
            {formatCurrency(row.original.value)}
          </span>
        ),
      },
      {
        accessorKey: 'type',
        header: 'Tipo',
        cell: ({ row }) =>
          row.original.type === TRANSACTION_TYPE.INCOME ? (
            <Badge className="bg-emerald-100 text-emerald-700 hover:bg-emerald-100 dark:bg-emerald-900/30 dark:text-emerald-400">
              Receita
            </Badge>
          ) : (
            <Badge variant="destructive">Despesa</Badge>
          ),
      },
      {
        accessorKey: 'categoryId',
        header: 'Categoria',
        cell: ({ row }) =>
          categoryMap.get(row.original.categoryId ?? 0) ?? 'Sem categoria',
      },
    ],
    [categoryMap],
  );

  // TanStack Table is currently flagged by React Compiler's incompatible-library rule.
  // eslint-disable-next-line react-hooks/incompatible-library
  const table = useReactTable({
    data: transactions,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <div className="animate-fade-in-up space-y-6">
      <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Transações</h1>
          <p className="text-muted-foreground">
            Registre e consulte receitas e despesas
          </p>
        </div>
        <Button onClick={() => setFormOpen(true)} className="w-full sm:w-auto">
          <Plus className="mr-1 size-4" />
          Nova Transação
        </Button>
      </div>

      <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:gap-3">
        <span className="text-sm font-medium">Filtrar por morador:</span>
        <Select
          value={selectedPersonId != null ? String(selectedPersonId) : ''}
          onValueChange={(val) => setSelectedPersonId(Number(val))}
        >
          <SelectTrigger className="w-full sm:w-60">
            <SelectValue placeholder="Selecione um morador" />
          </SelectTrigger>
          <SelectContent>
            {persons.map((person) => (
              <SelectItem key={person.id} value={String(person.id)}>
                {person.name}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      {selectedPersonId == null ? (
        <EmptyState
          icon={ArrowLeftRight}
          title="Selecione um morador"
          description="Escolha um morador acima para ver suas transações."
        />
      ) : isLoadingTransactions ? (
        <div className="h-48 animate-pulse rounded-md bg-muted" />
      ) : transactions.length === 0 ? (
        <EmptyState
          icon={ArrowLeftRight}
          title="Nenhuma transação"
          description="Este morador ainda não possui transações registradas."
          actionLabel="Registrar Transação"
          onAction={() => setFormOpen(true)}
        />
      ) : (
        <DataTable table={table} columns={columns} />
      )}

      <TransactionFormDialog
        open={formOpen}
        onOpenChange={setFormOpen}
        onSubmit={handleFormSubmit}
        isPending={createMutation.isPending}
      />
    </div>
  );
}
