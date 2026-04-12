import { NavLink, useNavigate } from 'react-router';
import {
  LayoutDashboard,
  Users,
  Tag,
  ArrowLeftRight,
  UserCircle,
  LogOut,
  Home,
  PanelLeftClose,
  PanelLeft,
} from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Separator } from '@/components/ui/separator';
import { useAuth } from '@/contexts/auth-context';
import { cn } from '@/lib/utils';

const navItems = [
  { to: '/', label: 'Dashboard', icon: LayoutDashboard },
  { to: '/moradores', label: 'Moradores', icon: Users },
  { to: '/categorias', label: 'Categorias', icon: Tag },
  { to: '/transacoes', label: 'Transações', icon: ArrowLeftRight },
];

interface SidebarProps {
  collapsed: boolean;
  onToggle: () => void;
  onNavigate?: () => void;
}

export function Sidebar({ collapsed, onToggle, onNavigate }: SidebarProps) {
  const { userName, logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate('/login');
    onNavigate?.();
  }

  return (
    <aside
      className={cn(
        'flex h-screen flex-col border-r bg-sidebar text-sidebar-foreground transition-all duration-300',
        collapsed ? 'w-24' : 'w-64',
      )}
    >
      {/* Header */}
      <div className="flex h-14 items-center gap-2 border-b px-3">
        <div className="flex size-8 shrink-0 items-center justify-center rounded-md bg-primary">
          <Home className="size-4 text-primary-foreground" />
        </div>
        {!collapsed && (
          <span className="truncate text-sm font-semibold">
            Despesas Residenciais
          </span>
        )}
        <Button
          variant="ghost"
          size="icon-sm"
          className={cn('shrink-0', collapsed ? 'mx-auto' : 'ml-auto')}
          onClick={onToggle}
        >
          {collapsed ? (
            <PanelLeft className="size-4" />
          ) : (
            <PanelLeftClose className="size-4" />
          )}
        </Button>
      </div>

      {/* Navigation */}
      <nav className="flex-1 p-2">
        {navItems.map((item) => (
          <NavLink
            to={item.to}
            end={item.to === '/'}
            onClick={onNavigate}
            className={({ isActive }) =>
              cn(
                'flex w-full flex-row items-center gap-3 rounded-md px-3 py-2  text-sm font-medium transition-colors',
                'hover:bg-sidebar-accent hover:text-sidebar-accent-foreground',
                isActive && 'bg-sidebar-accent text-sidebar-accent-foreground',
                collapsed && 'justify-center px-0',
              )
            }
          >
            <item.icon className="size-4 shrink-0" />
            {!collapsed && <span>{item.label}</span>}
          </NavLink>
        ))}
      </nav>

      <Separator />

      {/* Footer */}
      <div className="space-y-1 p-2">
        <NavLink
          to="/perfil"
          onClick={onNavigate}
          className={({ isActive }) =>
            cn(
              'flex w-full flex-row items-center gap-3 rounded-md px-3 py-2 text-sm font-medium transition-colors',
              'hover:bg-sidebar-accent hover:text-sidebar-accent-foreground',
              isActive && 'bg-sidebar-accent text-sidebar-accent-foreground',
              collapsed && 'justify-center px-0',
            )
          }
        >
          <UserCircle className="size-4 shrink-0" />
          {!collapsed && (
            <span className="truncate">{userName || 'Perfil'}</span>
          )}
        </NavLink>

        <button
          onClick={handleLogout}
          className={cn(
            'flex w-full cursor-pointer items-center gap-3 rounded-md px-3 py-2 text-sm font-medium transition-colors',
            'text-muted-foreground hover:bg-destructive/10 hover:text-destructive',
            collapsed && 'justify-center px-0',
          )}
        >
          <LogOut className="size-4 shrink-0" />
          {!collapsed && <span>Sair</span>}
        </button>
      </div>
    </aside>
  );
}
