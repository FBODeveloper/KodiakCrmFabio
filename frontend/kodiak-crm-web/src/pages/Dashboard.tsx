import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, PieChart, Pie, Cell, AreaChart, Area, ResponsiveContainer } from 'recharts';
import api from '../api/axios';
import type {
  DashboardResumo,
  DashboardLeadRecente,
  DashboardProdutividade,
  DashboardFunil,
  DashboardLeadsStatus,
  DashboardAtividadeTipo,
  DashboardTimeline,
} from '../types';

const FUNIL_COLORS = ['#bfdbfe', '#93c5fd', '#60a5fa', '#3b82f6', '#2563eb', '#1d4ed8', '#1e40af', '#1e3a8a'];

const STATUS_COLORS = ['#3b82f6', '#f59e0b', '#10b981', '#6366f1', '#ef4444', '#8b5cf6', '#06b6d4', '#ec4899'];

const ATIVIDADE_COLORS: Record<string, string> = {
  followup: '#3b82f6',
  reuniao: '#10b981',
  visita: '#f97316',
  whatsapp: '#34d399',
  ligacao: '#8b5cf6',
  email: '#06b6d4',
  tarefa: '#f59e0b',
};

export default function Dashboard() {
  const [resumo, setResumo] = useState<DashboardResumo | null>(null);
  const [leadsRecentes, setLeadsRecentes] = useState<DashboardLeadRecente[]>([]);
  const [funil, setFunil] = useState<DashboardFunil[]>([]);
  const [leadsStatus, setLeadsStatus] = useState<DashboardLeadsStatus[]>([]);
  const [atividadesTipo, setAtividadesTipo] = useState<DashboardAtividadeTipo[]>([]);
  const [timeline, setTimeline] = useState<DashboardTimeline[]>([]);
  const [produtividade, setProdutividade] = useState<DashboardProdutividade[]>([]);
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    try {
      const [
        resumoRes,
        recentesRes,
        funilRes,
        statusRes,
        atividadesRes,
        timelineRes,
        prodRes,
      ] = await Promise.all([
        api.get('/dashboard/resumo'),
        api.get('/dashboard/leads-recentes', { params: { quantidade: 5 } }),
        api.get('/dashboard/funil'),
        api.get('/dashboard/leads-status'),
        api.get('/dashboard/atividades-tipo'),
        api.get('/dashboard/timeline'),
        api.get('/dashboard/produtividade'),
      ]);
      setResumo(resumoRes.data);
      setLeadsRecentes(recentesRes.data);
      setFunil(funilRes.data);
      setLeadsStatus(statusRes.data);
      setAtividadesTipo(atividadesRes.data);
      setTimeline(timelineRes.data);
      setProdutividade(prodRes.data);
    } catch (error) {
      console.error('Erro ao carregar dashboard:', error);
    } finally {
      setCarregando(false);
    }
  };

  const formatarMoeda = (valor: number) =>
    new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor);

  const formatarPercentual = (valor: number) => `${valor.toFixed(1)}%`;

  const statusLeadColors: Record<string, string> = {
    novo: '#3b82f6',
    contato: '#f59e0b',
    qualificado: '#10b981',
    convertido: '#6366f1',
    perdido: '#ef4444',
  };

  if (carregando) {
    return <div className="carregando">Carregando...</div>;
  }

  return (
    <div className="dashboard">
      <h1>Dashboard</h1>

      <div className="dashboard-grid">
        <div className="dashboard-card">
          <h3>Leads</h3>
          <p className="numero">{resumo?.totalLeads || 0}</p>
          <p className="subtitulo">{resumo?.leadsNovos || 0} novos</p>
        </div>

        <div className="dashboard-card">
          <h3>Oportunidades</h3>
          <p className="numero">{resumo?.totalOportunidades || 0}</p>
        </div>

        <div className="dashboard-card">
          <h3>Valor do Funil</h3>
          <p className="numero">{formatarMoeda(resumo?.valorFunil || 0)}</p>
        </div>

        <div className="dashboard-card">
          <h3>Ticket Médio</h3>
          <p className="numero">{formatarMoeda(resumo?.ticketMedio || 0)}</p>
        </div>

        <div className="dashboard-card">
          <h3>Taxa de Conversão</h3>
          <p className="numero">{formatarPercentual(resumo?.taxaConversaoLead || 0)}</p>
        </div>

        <div className="dashboard-card">
          <h3>Oportunidades Ganhas</h3>
          <p className="numero" style={{ color: '#10b981' }}>{resumo?.totalOportunidadesGanhas || 0}</p>
        </div>

        <div className="dashboard-card">
          <h3>Oportunidades Perdidas</h3>
          <p className="numero" style={{ color: '#ef4444' }}>{resumo?.totalOportunidadesPerdidas || 0}</p>
        </div>

        <div className="dashboard-card">
          <h3>Clientes</h3>
          <p className="numero">{resumo?.totalClientes || 0}</p>
        </div>

        <div className="dashboard-card">
          <h3>Atividades Pendentes</h3>
          <p className="numero">{resumo?.atividadesPendentes || 0}</p>
        </div>

        <div className="dashboard-card">
          <h3>Propostas Enviadas</h3>
          <p className="numero">{resumo?.propostasEnviadas || 0}</p>
        </div>
      </div>

      <div className="dashboard-row">
        <div className="dashboard-panel">
          <div className="panel-header">
            <h3>Funil de Vendas</h3>
          </div>
          {funil.length === 0 ? (
            <p className="vazio">Nenhum dado disponível</p>
          ) : (
            <div style={{ padding: '1rem 1.5rem' }}>
              <ResponsiveContainer width="100%" height={300}>
                <BarChart data={funil} layout="vertical">
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis type="number" />
                  <YAxis dataKey="estagio" type="category" width={140} />
                  <Tooltip
                    formatter={(value: any, name: any) =>
                      name === 'valor' ? formatarMoeda(Number(value)) : value
                    }
                  />
                  <Legend />
                  <Bar dataKey="quantidade" name="Quantidade" fill="#3b82f6">
                    {funil.map((_entry, index) => (
                      <Cell key={index} fill={FUNIL_COLORS[index % FUNIL_COLORS.length]} />
                    ))}
                  </Bar>
                  <Bar dataKey="valor" name="Valor" fill="#1e40af">
                    {funil.map((_entry, index) => (
                      <Cell key={index} fill={FUNIL_COLORS[Math.min(index, FUNIL_COLORS.length - 1)]} fillOpacity={0.6} />
                    ))}
                  </Bar>
                </BarChart>
              </ResponsiveContainer>
            </div>
          )}
        </div>

        <div className="dashboard-panel">
          <div className="panel-header">
            <h3>Leads por Status</h3>
          </div>
          {leadsStatus.length === 0 ? (
            <p className="vazio">Nenhum dado disponível</p>
          ) : (
            <div style={{ padding: '1rem 1.5rem' }}>
              <ResponsiveContainer width="100%" height={300}>
                <PieChart>
                  <Pie
                    data={leadsStatus}
                    dataKey="quantidade"
                    nameKey="status"
                    cx="50%"
                    cy="50%"
                    outerRadius={100}
                    label
                  >
                    {leadsStatus.map((entry, index) => (
                      <Cell
                        key={index}
                        fill={statusLeadColors[entry.status] || STATUS_COLORS[index % STATUS_COLORS.length]}
                      />
                    ))}
                  </Pie>
                  <Tooltip />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            </div>
          )}
        </div>
      </div>

      <div className="dashboard-row">
        <div className="dashboard-panel">
          <div className="panel-header">
            <h3>Atividades por Tipo</h3>
          </div>
          {atividadesTipo.length === 0 ? (
            <p className="vazio">Nenhum dado disponível</p>
          ) : (
            <div style={{ padding: '1rem 1.5rem' }}>
              <ResponsiveContainer width="100%" height={300}>
                <PieChart>
                  <Pie
                    data={atividadesTipo}
                    dataKey="quantidade"
                    nameKey="tipo"
                    cx="50%"
                    cy="50%"
                    outerRadius={100}
                    label
                  >
                    {atividadesTipo.map((entry, index) => (
                      <Cell
                        key={index}
                        fill={ATIVIDADE_COLORS[entry.tipo] || STATUS_COLORS[index % STATUS_COLORS.length]}
                      />
                    ))}
                  </Pie>
                  <Tooltip />
                  <Legend />
                </PieChart>
              </ResponsiveContainer>
            </div>
          )}
        </div>

        <div className="dashboard-panel">
          <div className="panel-header">
            <h3>Timeline de Oportunidades</h3>
          </div>
          {timeline.length === 0 ? (
            <p className="vazio">Nenhum dado disponível</p>
          ) : (
            <div style={{ padding: '1rem 1.5rem' }}>
              <ResponsiveContainer width="100%" height={300}>
                <AreaChart data={timeline}>
                  <defs>
                    <linearGradient id="gradQuantidade" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor="#3b82f6" stopOpacity={0.3} />
                      <stop offset="95%" stopColor="#3b82f6" stopOpacity={0} />
                    </linearGradient>
                    <linearGradient id="gradValor" x1="0" y1="0" x2="0" y2="1">
                      <stop offset="5%" stopColor="#10b981" stopOpacity={0.3} />
                      <stop offset="95%" stopColor="#10b981" stopOpacity={0} />
                    </linearGradient>
                  </defs>
                  <CartesianGrid strokeDasharray="3 3" />
                  <XAxis dataKey="mes" />
                  <YAxis />
                  <Tooltip
                    formatter={(value: any, name: any) =>
                      name === 'valor' ? formatarMoeda(Number(value)) : value
                    }
                  />
                  <Legend />
                  <Area
                    type="monotone"
                    dataKey="quantidade"
                    name="Quantidade"
                    stroke="#3b82f6"
                    fill="url(#gradQuantidade)"
                  />
                  <Area
                    type="monotone"
                    dataKey="valor"
                    name="Valor"
                    stroke="#10b981"
                    fill="url(#gradValor)"
                  />
                </AreaChart>
              </ResponsiveContainer>
            </div>
          )}
        </div>
      </div>

      <div className="dashboard-row">
        <div className="dashboard-panel">
          <div className="panel-header">
            <h3>Leads Recentes</h3>
            <button className="btn-link" onClick={() => navigate('/leads')}>Ver todos</button>
          </div>
          <table className="tabela-mini">
            <thead>
              <tr>
                <th>Nome</th>
                <th>Empresa</th>
                <th>Telefone</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {leadsRecentes.length === 0 ? (
                <tr><td colSpan={4} className="vazio">Nenhum lead encontrado</td></tr>
              ) : (
                leadsRecentes.map((lead, index) => (
                  <tr key={index}>
                    <td><strong>{lead.nome}</strong></td>
                    <td>{lead.empresa || '-'}</td>
                    <td>{lead.telefone || '-'}</td>
                    <td>
                      <span className="status-badge" style={{ backgroundColor: statusLeadColors[lead.status] || '#6b7280' }}>
                        {lead.status}
                      </span>
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>

        <div className="dashboard-panel">
          <div className="panel-header">
            <h3>Produtividade por Vendedor</h3>
          </div>
          {produtividade.length === 0 ? (
            <p className="vazio">Nenhum vendedor encontrado</p>
          ) : (
            <table className="tabela-mini">
              <thead>
                <tr>
                  <th>Vendedor</th>
                  <th>Oportunidades</th>
                  <th>Ganhas</th>
                  <th>Valor Total</th>
                </tr>
              </thead>
              <tbody>
                {produtividade.map((vendedor) => (
                  <tr key={vendedor.vendedorId}>
                    <td><strong>{vendedor.vendedorNome}</strong></td>
                    <td>{vendedor.totalOportunidades}</td>
                    <td style={{ color: '#10b981' }}>{vendedor.oportunidadesGanhas}</td>
                    <td>{formatarMoeda(vendedor.valorTotal)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </div>
  );
}
