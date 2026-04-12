import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { toast } from "sonner";
import { Loader2 } from "lucide-react";
import { useNavigate } from "react-router";

import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Separator } from "@/components/ui/separator";
import { ConfirmDialog } from "@/components/shared/confirm-dialog";

import { useGetApiUser, usePutApiUser, useDeleteApiUser } from "@/api/generated";
import { useAuth } from "@/contexts/auth-context";
import { getApiErrorMessages } from "@/lib/utils";

const profileSchema = z.object({
  name: z.string().min(2, "Nome deve ter pelo menos 2 caracteres"),
  email: z.string().email("Email inválido"),
});

const passwordSchema = z.object({
  oldPassword: z.string().min(1, "Senha atual é obrigatória"),
  newPassword: z.string().min(6, "Nova senha deve ter pelo menos 6 caracteres"),
});

type ProfileForm = z.infer<typeof profileSchema>;
type PasswordForm = z.infer<typeof passwordSchema>;

export default function ProfilePage() {
  const { logout, updateUserName } = useAuth();
  const navigate = useNavigate();
  const [deleteOpen, setDeleteOpen] = useState(false);

  const { data: profile } = useGetApiUser();

  const profileForm = useForm<ProfileForm>({
    resolver: zodResolver(profileSchema),
    defaultValues: { name: "", email: "" },
  });

  useEffect(() => {
    if (profile?.data) {
      profileForm.reset({
        name: profile.data.name ?? "",
        email: profile.data.email ?? "",
      });
    }
  }, [profile, profileForm]);

  const passwordForm = useForm<PasswordForm>({
    resolver: zodResolver(passwordSchema),
    defaultValues: { oldPassword: "", newPassword: "" },
  });

  const updateMutation = usePutApiUser({
    mutation: {
      onSuccess: (response) => {
        if (response.data?.name) {
          updateUserName(response.data.name);
        }
        toast.success("Perfil atualizado com sucesso!");
      },
      onError: (error) => toast.error(getApiErrorMessages(error, "Erro ao atualizar perfil.")),
    },
  });

  const changePasswordMutation = usePutApiUser({
    mutation: {
      onSuccess: () => {
        toast.success("Senha alterada com sucesso!");
        passwordForm.reset();
      },
      onError: (error) => toast.error(getApiErrorMessages(error, "Erro ao alterar senha.")),
    },
  });

  const deleteMutation = useDeleteApiUser({
    mutation: {
      onSuccess: () => {
        toast.success("Conta excluída.");
        logout();
        navigate("/login");
      },
      onError: (error) => toast.error(getApiErrorMessages(error, "Erro ao excluir conta.")),
    },
  });

  function onProfileSubmit(data: ProfileForm) {
    updateMutation.mutate({ data });
  }

  function onPasswordSubmit(data: PasswordForm) {
    const currentProfile = profileForm.getValues();
    changePasswordMutation.mutate({
      data: { ...currentProfile, ...data },
    });
  }

  function handleDeleteConfirm() {
    deleteMutation.mutate();
  }

  return (
    <div className="mx-auto max-w-2xl animate-fade-in-up space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Perfil</h1>
        <p className="text-muted-foreground">
          Gerencie suas informações pessoais
        </p>
      </div>

      {/* Profile Info */}
      <Card>
        <CardHeader>
          <CardTitle>Dados Pessoais</CardTitle>
          <CardDescription>Atualize seu nome e email</CardDescription>
        </CardHeader>
        <CardContent>
          <form
            onSubmit={profileForm.handleSubmit(onProfileSubmit)}
            className="space-y-4"
          >
            <div className="space-y-2">
              <Label htmlFor="name">Nome</Label>
              <Input id="name" {...profileForm.register("name")} />
              {profileForm.formState.errors.name && (
                <p className="text-sm text-destructive">
                  {profileForm.formState.errors.name.message}
                </p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="email">Email</Label>
              <Input id="email" type="email" {...profileForm.register("email")} />
              {profileForm.formState.errors.email && (
                <p className="text-sm text-destructive">
                  {profileForm.formState.errors.email.message}
                </p>
              )}
            </div>
            <Button type="submit" disabled={updateMutation.isPending}>
              {updateMutation.isPending && (
                <Loader2 className="animate-spin" />
              )}
              Salvar
            </Button>
          </form>
        </CardContent>
      </Card>

      {/* Change Password */}
      <Card>
        <CardHeader>
          <CardTitle>Alterar Senha</CardTitle>
          <CardDescription>
            Informe a senha atual e a nova senha
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form
            onSubmit={passwordForm.handleSubmit(onPasswordSubmit)}
            className="space-y-4"
          >
            <div className="space-y-2">
              <Label htmlFor="oldPassword">Senha Atual</Label>
              <Input
                id="oldPassword"
                type="password"
                {...passwordForm.register("oldPassword")}
              />
              {passwordForm.formState.errors.oldPassword && (
                <p className="text-sm text-destructive">
                  {passwordForm.formState.errors.oldPassword.message}
                </p>
              )}
            </div>
            <div className="space-y-2">
              <Label htmlFor="newPassword">Nova Senha</Label>
              <Input
                id="newPassword"
                type="password"
                {...passwordForm.register("newPassword")}
              />
              {passwordForm.formState.errors.newPassword && (
                <p className="text-sm text-destructive">
                  {passwordForm.formState.errors.newPassword.message}
                </p>
              )}
            </div>
            <Button type="submit" disabled={changePasswordMutation.isPending}>
              {changePasswordMutation.isPending && (
                <Loader2 className="animate-spin" />
              )}
              Alterar Senha
            </Button>
          </form>
        </CardContent>
      </Card>

      <Separator />

      {/* Delete Account */}
      <Card className="border-destructive/50">
        <CardHeader>
          <CardTitle className="text-destructive">Zona de Perigo</CardTitle>
          <CardDescription>
            Ações irreversíveis para sua conta
          </CardDescription>
        </CardHeader>
        <CardContent>
          <Button variant="destructive" onClick={() => setDeleteOpen(true)}>
            Excluir Minha Conta
          </Button>
        </CardContent>
      </Card>

      <ConfirmDialog
        open={deleteOpen}
        onOpenChange={setDeleteOpen}
        title="Excluir Conta"
        description="Tem certeza que deseja excluir sua conta? Todos os seus dados serão removidos permanentemente. Esta ação não pode ser desfeita."
        onConfirm={handleDeleteConfirm}
        isPending={deleteMutation.isPending}
      />
    </div>
  );
}
