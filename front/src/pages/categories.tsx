import { useState } from 'react';
import {
  getCoreRowModel,
  useReactTable,
  type ColumnDef,
} from '@tanstack/react-table';
import { toast } from 'sonner';
import { Plus, Tag } from 'lucide-react';
import { useQueryClient } from '@tanstack/react-query';

import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { DataTable } from '@/components/shared/data-table';
import { EmptyState } from '@/components/shared/empty-state';
import { CategoryFormDialog } from '@/components/features/categories/category-form-dialog';

import {
  useGetApiCategory,
  usePostApiCategory,
  getGetApiCategoryQueryKey,
  type ResponseShortCategoryJson,
} from '@/api/generated';
import { CATEGORY_PURPOSE, CATEGORY_PURPOSE_LABELS } from '@/constants/enums';
import { getApiErrorMessages, unwrapApiData } from '@/lib/utils';

const EMPTY_CATEGORIES: ResponseShortCategoryJson[] = [];

function PurposeBadge({ purpose }: { purpose?: number }) {
  if (purpose === CATEGORY_PURPOSE.EXPENSE) {
    return <Badge variant="destructive">Despesa</Badge>;
  }
  if (purpose === CATEGORY_PURPOSE.INCOME) {
    return (
      <Badge className="bg-emerald-100 text-emerald-700 hover:bg-emerald-100 dark:bg-emerald-900/30 dark:text-emerald-400">
        Receita
      </Badge>
    );
  }
  return (
    <Badge variant="secondary">
      {CATEGORY_PURPOSE_LABELS[purpose ?? -1] ?? 'Desconhecido'}
    </Badge>
  );
}

export default function CategoriesPage() {
  const queryClient = useQueryClient();
  const [formOpen, setFormOpen] = useState(false);

  const { data: categoriesResponse, isLoading } = useGetApiCategory();
  const categories =
    unwrapApiData<ResponseShortCategoryJson[]>(categoriesResponse?.data) ??
    EMPTY_CATEGORIES;

  const createMutation = usePostApiCategory({
    mutation: {
      onSuccess: () => {
        toast.success('Categoria criada com sucesso!');
        setFormOpen(false);
        queryClient.invalidateQueries({
          queryKey: getGetApiCategoryQueryKey(),
        });
      },
      onError: (error) =>
        toast.error(getApiErrorMessages(error, 'Erro ao criar categoria.')),
    },
  });

  function handleFormSubmit(data: { description: string; purpose: number }) {
    createMutation.mutate({ data });
  }

  const columns: ColumnDef<ResponseShortCategoryJson, unknown>[] = [
    {
      accessorKey: 'description',
      header: 'Descrição',
    },
    {
      accessorKey: 'purpose',
      header: 'Propósito',
      cell: ({ row }) => <PurposeBadge purpose={row.original.purpose} />,
    },
  ];

  const table = useReactTable({
    data: categories,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Categorias</h1>
          <p className="text-muted-foreground">
            Gerencie as categorias de transação
          </p>
        </div>
        <div className="h-48 animate-pulse rounded-md bg-muted" />
      </div>
    );
  }

  return (
    <div className="animate-fade-in-up space-y-6">
      <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Categorias</h1>
          <p className="text-muted-foreground">
            Gerencie as categorias de transação
          </p>
        </div>
        <Button onClick={() => setFormOpen(true)} className="w-full sm:w-auto">
          <Plus className="mr-1 size-4" />
          Nova Categoria
        </Button>
      </div>

      {categories.length === 0 ? (
        <EmptyState
          icon={Tag}
          title="Nenhuma categoria cadastrada"
          description="Crie categorias para organizar suas transações por tipo."
          actionLabel="Criar Categoria"
          onAction={() => setFormOpen(true)}
        />
      ) : (
        <DataTable table={table} columns={columns} />
      )}

      <CategoryFormDialog
        open={formOpen}
        onOpenChange={setFormOpen}
        onSubmit={handleFormSubmit}
        isPending={createMutation.isPending}
      />
    </div>
  );
}
