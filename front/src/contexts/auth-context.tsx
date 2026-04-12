import { createContext, useContext } from 'react';

export interface AuthContextValue {
  isAuthenticated: boolean;
  userName: string | null;
  login: (token: string, name: string) => void;
  logout: () => void;
  updateUserName: (name: string) => void;
}

export const AuthContext = createContext<AuthContextValue | null>(null);

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
