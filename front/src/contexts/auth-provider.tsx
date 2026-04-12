import { useCallback, useMemo, useState, type ReactNode } from 'react';

import { TOKEN_KEY, USER_NAME_KEY } from '@/constants/enums';
import { AuthContext, type AuthContextValue } from '@/contexts/auth-context';

export function AuthProvider({ children }: { children: ReactNode }) {
  const [token, setToken] = useState<string | null>(() =>
    localStorage.getItem(TOKEN_KEY),
  );
  const [userName, setUserName] = useState<string | null>(() =>
    localStorage.getItem(USER_NAME_KEY),
  );

  const login = useCallback((accessToken: string, name: string) => {
    localStorage.setItem(TOKEN_KEY, accessToken);
    localStorage.setItem(USER_NAME_KEY, name);
    setToken(accessToken);
    setUserName(name);
  }, []);

  const logout = useCallback(() => {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_NAME_KEY);
    setToken(null);
    setUserName(null);
  }, []);

  const updateUserName = useCallback((name: string) => {
    localStorage.setItem(USER_NAME_KEY, name);
    setUserName(name);
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      isAuthenticated: !!token,
      userName,
      login,
      logout,
      updateUserName,
    }),
    [token, userName, login, logout, updateUserName],
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
