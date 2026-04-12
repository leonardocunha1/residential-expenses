import { useEffect, useMemo } from 'react';
import { useForm, Controller, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { Loader2 } from 'lucide-react';

import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';

import {
  useGetApiPerson,
  useGetApiCategory,
  type ResponseShortPersonJson,
  type ResponseShortCategoryJson,
} from '@/api/generated';
import {
  TRANSACTION_TYPE,
  TRANSACTION_TYPE_LABELS,
  CATEGORY_PURPOSE,
} from '@/constants/enums';

const EMPTY_CATEGORIES: ResponseShortCategoryJson[] = [];
const EMPTY_PERSONS: ResponseShortPersonJson[] = [];

const transactionSchema = z.object({
  description: z.string().min(1, 'Descrição é obrigatória'),
  value: z.number().positive('Valor deve ser maior que zero'),
  type: z.number().min(0).max(1),
  categoryId: z.number().min(1, 'Selecione uma categoria'),
  personId: z.number().min(1, 'Selecione um morador'),
});

type TransactionForm = z.infer<typeof transactionSchema>;

function filterCategories(
  categories: ResponseShortCategoryJson[],
  type: number,
) {
  return categories.filter((c) => {
    if (c.purpose === CATEGORY_PURPOSE.BOTH) return true;
    if (type === TRANSACTION_TYPE.EXPENSE)
      return c.purpose === CATEGORY_PURPOSE.EXPENSE;
    if (type === TRANSACTION_TYPE.INCOME)
      return c.purpose === CATEGORY_PURPOSE.INCOME;
    return false;
  });
}

interface TransactionFormDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onSubmit: (data: TransactionForm) => void;
  isPending: boolean;
}

export function TransactionFormDialog({
  open,
  onOpenChange,
  onSubmit,
  isPending,
}: TransactionFormDialogProps) {
  const { data: personsResponse } = useGetApiPerson();
  const { data: categoriesResponse } = useGetApiCategory();

  const persons = personsResponse?.data ?? EMPTY_PERSONS;
  const allCategories = categoriesResponse?.data ?? EMPTY_CATEGORIES;

  const {
    register,
    handleSubmit,
    reset,
    control,
    setValue,
    formState: { errors },
  } = useForm<TransactionForm>({
    resolver: zodResolver(transactionSchema),
    defaultValues: {
      description: '',
      value: 0,
      type: TRANSACTION_TYPE.EXPENSE,
      categoryId: 0,
      personId: 0,
    },
  });

  const selectedType = useWatch({ control, name: 'type' });
  const selectedCategoryId = useWatch({ control, name: 'categoryId' });

  const filteredCategories = useMemo(
    () => filterCategories(allCategories, selectedType),
    [allCategories, selectedType],
  );

  // Reset category when current selection is invalid for selected type.
  useEffect(() => {
    if (!selectedCategoryId) return;

    const isStillValid = filteredCategories.some(
      (c) => c.id === selectedCategoryId,
    );
    if (!isStillValid) {
      setValue('categoryId', 0);
    }
  }, [selectedCategoryId, filteredCategories, setValue]);

  useEffect(() => {
    if (open) {
      reset({
        description: '',
        value: 0,
        type: TRANSACTION_TYPE.EXPENSE,
        categoryId: 0,
        personId: 0,
      });
    }
  }, [open, reset]);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>Nova Transação</DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="description">Descrição</Label>
            <Input
              id="description"
              placeholder="Ex: Conta de luz, Salário..."
              {...register('description')}
            />
            {errors.description && (
              <p className="text-sm text-destructive">
                {errors.description.message}
              </p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="value">Valor (R$)</Label>
            <Input
              id="value"
              type="number"
              step="0.01"
              min="0"
              placeholder="0,00"
              {...register('value', { valueAsNumber: true })}
            />
            {errors.value && (
              <p className="text-sm text-destructive">{errors.value.message}</p>
            )}
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label>Tipo</Label>
              <Controller
                name="type"
                control={control}
                render={({ field }) => (
                  <Select
                    value={String(field.value)}
                    onValueChange={(val) => field.onChange(Number(val))}
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue />
                    </SelectTrigger>
                    <SelectContent>
                      {Object.entries(TRANSACTION_TYPE).map(([key, value]) => (
                        <SelectItem key={key} value={String(value)}>
                          {TRANSACTION_TYPE_LABELS[value]}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                )}
              />
            </div>

            <div className="space-y-2">
              <Label>Categoria</Label>
              <Controller
                name="categoryId"
                control={control}
                render={({ field }) => (
                  <Select
                    value={field.value ? String(field.value) : ''}
                    onValueChange={(val) => field.onChange(Number(val))}
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Selecione" />
                    </SelectTrigger>
                    <SelectContent>
                      {filteredCategories.map((cat) => (
                        <SelectItem key={cat.id} value={String(cat.id)}>
                          {cat.description}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                )}
              />
              {errors.categoryId && (
                <p className="text-sm text-destructive">
                  {errors.categoryId.message}
                </p>
              )}
            </div>
          </div>

          <div className="space-y-2">
            <Label>Morador</Label>
            <Controller
              name="personId"
              control={control}
              render={({ field }) => (
                <Select
                  value={field.value ? String(field.value) : ''}
                  onValueChange={(val) => field.onChange(Number(val))}
                >
                  <SelectTrigger className="w-full">
                    <SelectValue placeholder="Selecione o morador" />
                  </SelectTrigger>
                  <SelectContent>
                    {persons.map((person) => (
                      <SelectItem key={person.id} value={String(person.id)}>
                        {person.name}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              )}
            />
            {errors.personId && (
              <p className="text-sm text-destructive">
                {errors.personId.message}
              </p>
            )}
          </div>

          <div className="flex justify-end gap-2">
            <Button
              type="button"
              variant="outline"
              onClick={() => onOpenChange(false)}
              disabled={isPending}
            >
              Cancelar
            </Button>
            <Button type="submit" disabled={isPending}>
              {isPending && <Loader2 className="animate-spin" />}
              Criar
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
