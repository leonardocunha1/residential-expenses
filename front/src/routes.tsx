import { createBrowserRouter, Navigate, Outlet } from 'react-router';

import { AuthProvider } from '@/contexts/auth-provider';
import { AuthGuard } from '@/components/layout/auth-guard';
import { AppLayout } from '@/components/layout/app-layout';

import LoginPage from '@/pages/login';
import RegisterPage from '@/pages/register';
import DashboardPage from '@/pages/dashboard';
import PersonsPage from '@/pages/persons';
import CategoriesPage from '@/pages/categories';
import TransactionsPage from '@/pages/transactions';
import ProfilePage from '@/pages/profile';

export const router = createBrowserRouter([
  {
    element: (
      <AuthProvider>
        <Outlet />
      </AuthProvider>
    ),
    children: [
      {
        path: '/login',
        element: <LoginPage />,
      },
      {
        path: '/registro',
        element: <RegisterPage />,
      },
      {
        element: <AuthGuard />,
        children: [
          {
            element: <AppLayout />,
            children: [
              { index: true, element: <DashboardPage /> },
              { path: 'moradores', element: <PersonsPage /> },
              { path: 'categorias', element: <CategoriesPage /> },
              { path: 'transacoes', element: <TransactionsPage /> },
              { path: 'perfil', element: <ProfilePage /> },
            ],
          },
        ],
      },
      {
        path: '*',
        element: <Navigate to="/" replace />,
      },
    ],
  },
]);
