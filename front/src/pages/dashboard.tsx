import {
  TrendingUp,
  TrendingDown,
  Wallet,
  LayoutDashboard,
} from 'lucide-react';
import {
  Bar,
  BarChart,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from 'recharts';

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { EmptyState } from '@/components/shared/empty-state';

import {
  useGetApiTransactionTotals,
  type ResponsePersonTotalsJson,
} from '@/api/generated';
import { formatCurrency } from '@/lib/utils';
import { cn } from '@/lib/utils';

const EMPTY_PEOPLE: ResponsePersonTotalsJson[] = [];

export default function DashboardPage() {
  const { data: totalsResponse, isLoading } = useGetApiTransactionTotals();
  const totals = totalsResponse?.data;

  if (isLoading) {
    return (
      <div className="space-y-6">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Dashboard</h1>
          <p className="text-muted-foreground">
            Resumo financeiro da residência
          </p>
        </div>
        <div className="grid gap-4 sm:grid-cols-3">
          {[1, 2, 3].map((i) => (
            <div key={i} className="h-28 animate-pulse rounded-lg bg-muted" />
          ))}
        </div>
        <div className="h-64 animate-pulse rounded-lg bg-muted" />
      </div>
    );
  }

  const totalIncome = totals?.totalIncome ?? 0;
  const totalExpense = totals?.totalExpense ?? 0;
  const balance = totals?.balance ?? 0;
  const people = totals?.people ?? EMPTY_PEOPLE;

  const hasData = totalIncome > 0 || totalExpense > 0;

  const chartData = people.map((p) => ({
    name: p.personName ?? 'Sem nome',
    Receita: p.totalIncome ?? 0,
    Despesa: p.totalExpense ?? 0,
  }));

  return (
    <div className="animate-fade-in-up space-y-6">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Dashboard</h1>
        <p className="text-muted-foreground">Resumo financeiro da residência</p>
      </div>

      {/* Stat Cards */}
      <div className="grid gap-4 sm:grid-cols-3">
        <Card className="transition-shadow duration-200 hover:shadow-md">
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Receita Total
            </CardTitle>
            <TrendingUp className="size-4 text-emerald-500" />
          </CardHeader>
          <CardContent>
            <p className="text-2xl font-bold text-emerald-600 dark:text-emerald-400">
              {formatCurrency(totalIncome)}
            </p>
          </CardContent>
        </Card>

        <Card className="transition-shadow duration-200 hover:shadow-md">
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Despesa Total
            </CardTitle>
            <TrendingDown className="size-4 text-red-500" />
          </CardHeader>
          <CardContent>
            <p className="text-2xl font-bold text-red-600 dark:text-red-400">
              {formatCurrency(totalExpense)}
            </p>
          </CardContent>
        </Card>

        <Card className="transition-shadow duration-200 hover:shadow-md">
          <CardHeader className="flex flex-row items-center justify-between pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">
              Saldo
            </CardTitle>
            <Wallet className="size-4 text-muted-foreground" />
          </CardHeader>
          <CardContent>
            <p
              className={cn(
                'text-2xl font-bold',
                balance >= 0
                  ? 'text-emerald-600 dark:text-emerald-400'
                  : 'text-red-600 dark:text-red-400',
              )}
            >
              {formatCurrency(balance)}
            </p>
          </CardContent>
        </Card>
      </div>

      {!hasData ? (
        <EmptyState
          icon={LayoutDashboard}
          title="Sem dados financeiros"
          description="Cadastre moradores, categorias e registre transações para ver o resumo aqui."
        />
      ) : (
        <>
          {/* Per-person summary table */}
          {people.length > 0 && (
            <Card>
              <CardHeader>
                <CardTitle>Resumo por Morador</CardTitle>
              </CardHeader>
              <CardContent>
                <div className="overflow-x-auto">
                  <table className="w-full text-sm">
                    <thead>
                      <tr className="border-b text-left text-muted-foreground">
                        <th className="pb-2 font-medium">Morador</th>
                        <th className="pb-2 text-right font-medium">Receita</th>
                        <th className="pb-2 text-right font-medium">Despesa</th>
                        <th className="pb-2 text-right font-medium">Saldo</th>
                      </tr>
                    </thead>
                    <tbody>
                      {people.map((person) => {
                        const personBalance = person.balance ?? 0;
                        return (
                          <tr
                            key={person.personId}
                            className="border-b last:border-0"
                          >
                            <td className="py-3 font-medium">
                              {person.personName}
                            </td>
                            <td className="py-3 text-right text-emerald-600 dark:text-emerald-400">
                              {formatCurrency(person.totalIncome)}
                            </td>
                            <td className="py-3 text-right text-red-600 dark:text-red-400">
                              {formatCurrency(person.totalExpense)}
                            </td>
                            <td
                              className={cn(
                                'py-3 text-right font-medium',
                                personBalance >= 0
                                  ? 'text-emerald-600 dark:text-emerald-400'
                                  : 'text-red-600 dark:text-red-400',
                              )}
                            >
                              {formatCurrency(personBalance)}
                            </td>
                          </tr>
                        );
                      })}
                    </tbody>
                  </table>
                </div>
              </CardContent>
            </Card>
          )}

          {/* Chart */}
          {chartData.length > 0 && (
            <Card>
              <CardHeader>
                <CardTitle>Comparativo por Morador</CardTitle>
              </CardHeader>
              <CardContent>
                <ResponsiveContainer width="100%" height={300}>
                  <BarChart data={chartData}>
                    <CartesianGrid
                      strokeDasharray="3 3"
                      className="stroke-border"
                    />
                    <XAxis dataKey="name" className="text-xs" />
                    <YAxis
                      className="text-xs"
                      tickFormatter={(v) =>
                        new Intl.NumberFormat('pt-BR', {
                          notation: 'compact',
                          style: 'currency',
                          currency: 'BRL',
                        }).format(v)
                      }
                    />
                    <Tooltip
                      formatter={(value) => formatCurrency(Number(value))}
                      contentStyle={{
                        borderRadius: '8px',
                        border: '1px solid var(--border)',
                        background: 'var(--popover)',
                        color: 'var(--popover-foreground)',
                      }}
                    />
                    <Legend />
                    <Bar
                      dataKey="Receita"
                      fill="#10b981"
                      radius={[4, 4, 0, 0]}
                    />
                    <Bar
                      dataKey="Despesa"
                      fill="#ef4444"
                      radius={[4, 4, 0, 0]}
                    />
                  </BarChart>
                </ResponsiveContainer>
              </CardContent>
            </Card>
          )}
        </>
      )}
    </div>
  );
}
