import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { DashboardResumo, DashboardLeadRecente, DashboardLeadsPorEstagio, DashboardTicketMedio, DashboardConversao, DashboardProdutividade } from '../types';

export default function Dashboard() {
  const [resumo, setResumo] = useState<DashboardResumo | null>(null);
  const [leadsRecentes, setLeadsRecentes] = useState<DashboardLeadRecente[]>([]);
  const [leadsPorEstagio, setLeadsPorEstagio] = useState<DashboardLeadsPorEstagio[]>([]);
  const [ticketMedio, setTicketMedio] = useState<DashboardTicketMedio | null>(null);
  const [conversao, setConversao] = useState<DashboardConversao | null>(null);
  const [produtividade, setProdutividade] = useState<DashboardProdutividade[]>([]);
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarDados();
  }, []);

  const carregarDados = async () => {
    try {
      const [resumoRes, recentesRes, estagiosRes, ticketRes, convRes, prodRes] = await Promise.all([
        api.get('/dashboard/resumo'),
        api.get('/dashboard/leads-recentes', { params: { quantidade: 5 } }),
        api.get('/dashboard/leads-por-estagio'),
        api.get('/dashboard/ticket-medio'),
        api.get('/dashboard/conversao'),
        api.get('/dashboard/produtividade')
      ]);
      setResumo(resumoRes.data);
      setLeadsRecentes(recentesRes.data);
      setLeadsPorEstagio(estagiosRes.data);
      setTicketMedio(ticketRes.data);
      setConversao(convRes.data);
      setProdutividade(prodRes.data);
    } catch (error) {
      console.error('Erro ao carregar dashboard:', error);
    } finally {
      setCarregando(false);
    }
  };

  const statusColors: Record<string, string> = {
    novo: '#3b82f6',
    contato: '#f59e0b',
    qualificado: '#10b981',
    convertido: '#6366f1',
    perdido: '#ef4444'
  };

  const totalEstagio = leadsPorEstagio.reduce((acc, e) => acc + e.quantidade, 0);

  const formatarMoeda = (valor: number) =>
    new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valor);

  const formatarPercentual = (valor: number) => `${valor.toFixed(1)}%`;

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
          <p className="numero">{formatarMoeda(ticketMedio?.ticketMedio || 0)}</p>
          <p className="subtitulo">{ticketMedio?.totalComValor || 0} com valor</p>
        </div>
        
        <div className="dashboard-card">
          <h3>Taxa Conversão Lead</h3>
          <p className="numero">{formatarPercentual(conversao?.taxaConversao || 0)}</p>
          <p className="subtitulo">{conversao?.leadsConvertidos || 0} de {conversao?.totalLeads || 0}</p>
        </div>
        
        <div className="dashboard-card">
          <h3>Oportunidades Ganhas</h3>
          <p className="numero" style={{ color: '#10b981' }}>{conversao?.oportunidadesGanhas || 0}</p>
        </div>
        
        <div className="dashboard-card">
          <h3>Oportunidades Perdidas</h3>
          <p className="numero" style={{ color: '#ef4444' }}>{conversao?.oportunidadesPerdidas || 0}</p>
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
                leadsRecentes.map((lead) => (
                  <tr key={lead.id} onClick={() => navigate(`/leads/${lead.id}`)} style={{ cursor: 'pointer' }}>
                    <td><strong>{lead.nome}</strong></td>
                    <td>{lead.empresa || '-'}</td>
                    <td>{lead.telefone || '-'}</td>
                    <td>
                      <span className="status-badge" style={{ backgroundColor: statusColors[lead.status] || '#6b7280' }}>
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
            <h3>Leads por Estágio</h3>
          </div>
          {leadsPorEstagio.length === 0 ? (
            <p className="vazio">Nenhum estágio configurado</p>
          ) : (
            <div className="pie-chart-container">
              <div className="pie-chart">
                <svg viewBox="0 0 36 36">
                  {leadsPorEstagio.reduce<{ element: React.JSX.Element; offset: number }>((acc, estagio, i) => {
                    const percent = totalEstagio > 0 ? (estagio.quantidade / totalEstagio) * 100 : 0;
                    const dashArray = `${percent} ${100 - percent}`;
                    const element = (
                      <circle
                        key={i}
                        cx="18"
                        cy="18"
                        r="15.91549430918954"
                        fill="transparent"
                        stroke={estagio.cor}
                        strokeWidth="3.5"
                        strokeDasharray={dashArray}
                        strokeDashoffset={acc.offset}
                      />
                    );
                    return { element: <>{acc.element}{element}</>, offset: acc.offset - percent };
                  }, { element: <></>, offset: 25 }).element}
                </svg>
                <div className="pie-center">
                  <span className="pie-total">{totalEstagio}</span>
                  <span className="pie-label">leads</span>
                </div>
              </div>
              <div className="pie-legend">
                {leadsPorEstagio.map((estagio, i) => (
                  <div key={i} className="legend-item">
                    <span className="legend-dot" style={{ background: estagio.cor }}></span>
                    <span className="legend-label">{estagio.estagioNome}</span>
                    <span className="legend-value">{estagio.quantidade}</span>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>
      </div>

      {produtividade.length > 0 && (
        <div className="dashboard-row">
          <div className="dashboard-panel">
            <div className="panel-header">
              <h3>Produtividade por Vendedor</h3>
            </div>
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
                  <tr key={vendedor.usuarioId}>
                    <td><strong>{vendedor.usuarioNome}</strong></td>
                    <td>{vendedor.totalOportunidades}</td>
                    <td style={{ color: '#10b981' }}>{vendedor.oportunidadesGanhas}</td>
                    <td>{formatarMoeda(vendedor.valorTotal)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
}
