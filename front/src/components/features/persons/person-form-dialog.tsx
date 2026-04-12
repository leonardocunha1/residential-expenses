import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Loader2 } from "lucide-react";

import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

import type { ResponseShortPersonJson } from "@/api/generated";

const personSchema = z.object({
  name: z.string().min(2, "Nome deve ter pelo menos 2 caracteres"),
  age: z.number().int().min(0, "Idade deve ser positiva").max(150, "Idade inválida"),
});

type PersonForm = z.infer<typeof personSchema>;

interface PersonFormDialogProps {
  open: boolean;
  onOpenChange: (open: boolean) => void;
  person?: ResponseShortPersonJson | null;
  onSubmit: (data: PersonForm) => void;
  isPending: boolean;
}

export function PersonFormDialog({
  open,
  onOpenChange,
  person,
  onSubmit,
  isPending,
}: PersonFormDialogProps) {
  const isEditing = !!person;

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<PersonForm>({
    resolver: zodResolver(personSchema),
  });

  useEffect(() => {
    if (open) {
      reset(
        person
          ? { name: person.name ?? "", age: person.age ?? 0 }
          : { name: "", age: 0 },
      );
    }
  }, [open, person, reset]);

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent>
        <DialogHeader>
          <DialogTitle>
            {isEditing ? "Editar Morador" : "Novo Morador"}
          </DialogTitle>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="name">Nome</Label>
            <Input
              id="name"
              placeholder="Nome do morador"
              {...register("name")}
            />
            {errors.name && (
              <p className="text-sm text-destructive">{errors.name.message}</p>
            )}
          </div>

          <div className="space-y-2">
            <Label htmlFor="age">Idade</Label>
            <Input
              id="age"
              type="number"
              placeholder="Idade"
              {...register("age", { valueAsNumber: true })}
            />
            {errors.age && (
              <p className="text-sm text-destructive">{errors.age.message}</p>
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
              {isEditing ? "Salvar" : "Criar"}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
