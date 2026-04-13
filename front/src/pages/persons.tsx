import { useState } from 'react';
import {
  getCoreRowModel,
  useReactTable,
  type ColumnDef,
} from '@tanstack/react-table';
import { toast } from 'sonner';
import { MoreHorizontal, Pencil, Plus, Trash2, Users } from 'lucide-react';
import { useQueryClient } from '@tanstack/react-query';

import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { DataTable } from '@/components/shared/data-table';
import { EmptyState } from '@/components/shared/empty-state';
import { ConfirmDialog } from '@/components/shared/confirm-dialog';
import { PersonFormDialog } from '@/components/features/persons/person-form-dialog';

import {
  useGetApiPerson,
  usePostApiPerson,
  usePutApiPersonId,
  useDeleteApiPersonId,
  getGetApiPersonQueryKey,
  type ResponseShortPersonJson,
} from '@/api/generated';
import { getApiErrorMessages } from '@/lib/utils';

const EMPTY_PERSONS: ResponseShortPersonJson[] = [];

export default function PersonsPage() {
  const queryClient = useQueryClient();
  const [formOpen, setFormOpen] = useState(false);
  const [deleteOpen, setDeleteOpen] = useState(false);
  const [selectedPerson, setSelectedPerson] =
    useState<ResponseShortPersonJson | null>(null);

  const { data: personsResponse, isLoading } = useGetApiPerson();
  const persons = personsResponse ?? EMPTY_PERSONS;

  const invalidatePersons = () =>
    queryClient.invalidateQueries({ queryKey: getGetApiPersonQueryKey() });

  const createMutation = usePostApiPerson({
    mutation: {
      onSuccess: () => {
        toast.success('Morador cadastrado com sucesso!');
        setFormOpen(false);
        invalidatePersons();
      },
      onError: (error) =>
        toast.error(getApiErrorMessages(error, 'Erro ao cadastrar morador.')),
    },
  });

  const updateMutation = usePutApiPersonId({
    mutation: {
      onSuccess: () => {
        toast.success('Morador atualizado com sucesso!');
        setFormOpen(false);
        setSelectedPerson(null);
        invalidatePersons();
      },
      onError: (error) =>
        toast.error(getApiErrorMessages(error, 'Erro ao atualizar morador.')),
    },
  });

  const deleteMutation = useDeleteApiPersonId({
    mutation: {
      onSuccess: () => {
        toast.success('Morador excluído com sucesso!');
        setDeleteOpen(false);
        setSelectedPerson(null);
        invalidatePersons();
      },
      onError: (error) =>
        toast.error(getApiErrorMessages(error, 'Erro ao excluir morador.')),
    },
  });

  function handleCreate() {
    setSelectedPerson(null);
    setFormOpen(true);
  }

  function handleEdit(person: ResponseShortPersonJson) {
    setSelectedPerson(person);
    setFormOpen(true);
  }

  function handleDeleteClick(person: ResponseShortPersonJson) {
    setSelectedPerson(person);
    setDeleteOpen(true);
  }

  function handleFormSubmit(data: { name: string; age: number }) {
    if (selectedPerson?.id != null) {
      updateMutation.mutate({ id: selectedPerson.id, data });
    } else {
      createMutation.mutate({ data });
    }
  }

  function handleDeleteConfirm() {
    if (selectedPerson?.id != null) {
      deleteMutation.mutate({ id: selectedPerson.id });
    }
  }

  const columns: ColumnDef<ResponseShortPersonJson, unknown>[] = [
    {
      accessorKey: 'name',
      header: 'Nome',
    },
    {
      accessorKey: 'age',
      header: 'Idade',
      cell: ({ row }) => `${row.original.age ?? '-'} anos`,
    },
    {
      id: 'actions',
      header: () => <span className="sr-only">Ações</span>,
      cell: ({ row }) => (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" size="icon-sm">
              <MoreHorizontal className="size-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuItem onClick={() => handleEdit(row.original)}>
              <Pencil className="mr-2 size-4" />
              Editar
            </DropdownMenuItem>
            <DropdownMenuItem
              onClick={() => handleDeleteClick(row.original)}
              className="text-destructive focus:text-destructive"
            >
              <Trash2 className="mr-2 size-4" />
              Excluir
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      ),
    },
  ];

  // TanStack Table is currently flagged by React Compiler's incompatible-library rule.
  // eslint-disable-next-line react-hooks/incompatible-library
  const table = useReactTable({
    data: persons,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Moradores</h1>
          <p className="text-muted-foreground">
            Gerencie os moradores da residência
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
          <h1 className="text-2xl font-semibold tracking-tight">Moradores</h1>
          <p className="text-muted-foreground">
            Gerencie os moradores da residência
          </p>
        </div>
        <Button onClick={handleCreate} className="w-full sm:w-auto">
          <Plus className="mr-1 size-4" />
          Novo Morador
        </Button>
      </div>

      {persons.length === 0 ? (
        <EmptyState
          icon={Users}
          title="Nenhum morador cadastrado"
          description="Cadastre o primeiro morador para começar a registrar transações."
          actionLabel="Cadastrar Morador"
          onAction={handleCreate}
        />
      ) : (
        <DataTable table={table} columns={columns} />
      )}

      <PersonFormDialog
        open={formOpen}
        onOpenChange={setFormOpen}
        person={selectedPerson}
        onSubmit={handleFormSubmit}
        isPending={createMutation.isPending || updateMutation.isPending}
      />

      <ConfirmDialog
        open={deleteOpen}
        onOpenChange={setDeleteOpen}
        title="Excluir Morador"
        description={`Tem certeza que deseja excluir "${selectedPerson?.name}"? Esta ação não pode ser desfeita.`}
        onConfirm={handleDeleteConfirm}
        isPending={deleteMutation.isPending}
      />
    </div>
  );
}
