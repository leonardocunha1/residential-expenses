import { useState } from 'react';
import { Outlet } from 'react-router';
import { Menu, Home } from 'lucide-react';
import { TooltipProvider } from '@/components/ui/tooltip';
import { Button } from '@/components/ui/button';
import { Sidebar } from './sidebar';
import { cn } from '@/lib/utils';

export function AppLayout() {
  const [collapsed, setCollapsed] = useState(false);
  const [mobileOpen, setMobileOpen] = useState(false);

  return (
    <TooltipProvider>
      <div className="flex h-screen overflow-hidden">
        {/* Desktop sidebar */}
        <div className="hidden md:block">
          <Sidebar
            collapsed={collapsed}
            onToggle={() => setCollapsed((prev) => !prev)}
          />
        </div>

        {/* Mobile overlay backdrop */}
        <div
          className={cn(
            'fixed inset-0 z-40 bg-black/50 transition-opacity duration-300 md:hidden',
            mobileOpen
              ? 'opacity-100 pointer-events-auto'
              : 'opacity-0 pointer-events-none',
          )}
          onClick={() => setMobileOpen(false)}
        />

        {/* Mobile sidebar drawer */}
        <div
          className={cn(
            'fixed inset-y-0 left-0 z-50 transition-transform duration-300 md:hidden',
            mobileOpen ? 'translate-x-0' : '-translate-x-full',
          )}
        >
          <Sidebar
            collapsed={false}
            onToggle={() => setMobileOpen(false)}
            onNavigate={() => setMobileOpen(false)}
          />
        </div>

        <main className="flex-1 overflow-y-auto bg-muted/40">
          {/* Mobile top bar */}
          <div className="sticky top-0 z-30 flex h-14 items-center gap-3 border-b bg-background/95 px-4 backdrop-blur supports-backdrop-filter:bg-background/60 md:hidden">
            <Button
              variant="ghost"
              size="icon"
              onClick={() => setMobileOpen(true)}
              className="shrink-0"
            >
              <Menu className="size-5" />
              <span className="sr-only">Abrir menu</span>
            </Button>
            <div className="flex items-center gap-2">
              <div className="flex size-7 shrink-0 items-center justify-center rounded-md bg-primary">
                <Home className="size-3.5 text-primary-foreground" />
              </div>
              <span className="text-sm font-semibold">
                Residential Expenses
              </span>
            </div>
          </div>

          <div className="p-4 md:p-6">
            <Outlet />
          </div>
        </main>
      </div>
    </TooltipProvider>
  );
}
