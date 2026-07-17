import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../api/axios';
import type { Lead, LeadStats, PaginatedResponse } from '../types';
import FilterBar, { type FiltroConfig } from '../components/FilterBar';

export default function Leads() {
  const [leads, setLeads] = useState<Lead[]>([]);
  const [total, setTotal] = useState(0);
  const [stats, setStats] = useState<LeadStats | null>(null);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [filtros, setFiltros] = useState<Record<string, string>>({status: '', temperatura: '', responsavel: '', dataInicio: '', dataFim: ''});
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();

  const filtroConfigs: FiltroConfig[] = [
    {
      campo: 'status',
      label: 'Estágio',
      tipo: 'select',
      opcoes: [
        { valor: 'novo', label: 'Novo' },
        { valor: 'contato', label: 'Contato' },
        { valor: 'qualificado', label: 'Qualificado' },
        { valor: 'convertido', label: 'Convertido' },
        { valor: 'perdido', label: 'Perdido' },
      ],
    },
    {
      campo: 'temperatura',
      label: 'Temperatura',
      tipo: 'select',
      opcoes: [
        { valor: 'quente', label: 'Quente' },
        { valor: 'morno', label: 'Morno' },
        { valor: 'frio', label: 'Frio' },
      ],
    },
    {
      campo: 'responsavel',
      label: 'Responsável',
      tipo: 'texto',
      placeholder: 'Buscar por responsável...',
    },
    { campo: 'dataInicio', label: 'Data Inicio', tipo: 'data' },
    { campo: 'dataFim', label: 'Data Fim', tipo: 'data' },
  ];

  useEffect(() => {
    carregarLeads();
    carregarStats();
  }, [pagina, busca, filtros]);

  const carregarLeads = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Lead>>('/lead', {
        params: { pagina, itensPorPagina: 50, busca, ...filtros }
      });
      setLeads(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar leads:', error);
    } finally {
      setCarregando(false);
    }
  };

  const carregarStats = async () => {
    try {
      const response = await api.get<LeadStats>('/lead/stats');
      setStats(response.data);
    } catch (error) {
      console.error('Erro ao carregar stats:', error);
    }
  };

  const excluirLead = async (id: number) => {
    if (!confirm('Deseja excluir este lead?')) return;
    try {
      await api.delete(`/lead/${id}`);
      carregarLeads();
      carregarStats();
    } catch (error) {
      console.error('Erro ao excluir lead:', error);
    }
  };

  const temperaturaInfo: Record<string, { color: string; label: string }> = {
    quente: { color: '#ef4444', label: 'Quente' },
    morno: { color: '#f59e0b', label: 'Morno' },
    frio: { color: '#3b82f6', label: 'Frio' }
  };

  const handleFiltroMudar = (campo: string, valor: string) => {
    setFiltros(prev => ({ ...prev, [campo]: valor }));
    setPagina(1);
  };

  const handleFiltroLimpar = () => {
    setFiltros({ status: '', temperatura: '', responsavel: '', dataInicio: '', dataFim: '' });
    setPagina(1);
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Leads</h1>
        <div className="pagina-header-actions">
          <button onClick={() => navigate('/leads/kanban')} className="btn-secondary">
            Kanban
          </button>
          <button onClick={() => navigate('/leads/novo')} className="btn-primary">
            Novo Lead
          </button>
        </div>
      </div>

      {stats && (
        <div className="stats-cards">
          <div className="stat-card stat-total">
            <div className="stat-info">
              <span className="stat-label">Total Leads</span>
              <span className="stat-valor">{stats.total}</span>
            </div>
            <div className="stat-icon" style={{ background: 'rgba(59, 130, 246, 0.1)', color: '#3b82f6' }}>
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M22 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>
            </div>
          </div>
          <div className="stat-card stat-novos">
            <div className="stat-info">
              <span className="stat-label">Novos</span>
              <span className="stat-valor">{stats.novos}</span>
            </div>
            <div className="stat-icon" style={{ background: 'rgba(16, 185, 129, 0.1)', color: '#10b981' }}>
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><line x1="19" y1="8" x2="19" y2="14"/><line x1="22" y1="11" x2="16" y2="11"/></svg>
            </div>
          </div>
          <div className="stat-card stat-conversao">
            <div className="stat-info">
              <span className="stat-label">Taxa Conversão</span>
              <span className="stat-valor">{stats.taxaConversao}%</span>
            </div>
            <div className="stat-icon" style={{ background: 'rgba(99, 102, 241, 0.1)', color: '#6366f1' }}>
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="23 6 13.5 15.5 8.5 10.5 1 18"/><polyline points="17 6 23 6 23 12"/></svg>
            </div>
          </div>
          <div className="stat-card stat-followup">
            <div className="stat-info">
              <span className="stat-label">Follow-up Pendente</span>
              <span className="stat-valor">{stats.followupPendente}</span>
            </div>
            <div className="stat-icon" style={{ background: 'rgba(245, 158, 11, 0.1)', color: '#f59e0b' }}>
              <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><circle cx="12" cy="12" r="10"/><polyline points="12 6 12 12 16 14"/></svg>
            </div>
          </div>
        </div>
      )}

      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar leads..."
          value={busca}
          onChange={(e) => { setBusca(e.target.value); setPagina(1); }}
        />
      </div>

      <FilterBar
        filtros={filtroConfigs}
        valores={filtros}
        onMudar={handleFiltroMudar}
        onLimpar={handleFiltroLimpar}
      />

      {carregando ? (
        <div className="carregando">Carregando...</div>
      ) : (
        <>
          <table className="tabela">
            <thead>
              <tr>
                <th>Temperatura</th>
                <th>Nome</th>
                <th>Empresa</th>
                <th>Email</th>
                <th>Telefone</th>
                <th>Estágio</th>
                <th>Responsável</th>
                <th style={{ width: 100 }}></th>
              </tr>
            </thead>
            <tbody>
              {leads.map((lead) => {
                const temp = temperaturaInfo[lead.temperatura] || temperaturaInfo.frio;
                return (
                  <tr key={lead.id}>
                    <td>
                      <span
                        className="temperatura-badge"
                        style={{
                          backgroundColor: temp.color,
                          color: 'white',
                          padding: '2px 8px',
                          borderRadius: '12px',
                          fontSize: '0.75rem',
                          fontWeight: 'bold'
                        }}
                      >
                        {temp.label}
                      </span>
                    </td>
                    <td>{lead.nome}</td>
                    <td>{lead.empresa || '-'}</td>
                    <td>{lead.email || '-'}</td>
                    <td>{lead.telefone || '-'}</td>
                    <td>{lead.estagioNome || '-'}</td>
                    <td>
                      {lead.responsavelNome ? (
                        <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                          {lead.responsavelAvatar ? (
                            <img src={lead.responsavelAvatar} alt="" style={{ width: 24, height: 24, borderRadius: '50%', objectFit: 'cover' }} />
                          ) : (
                            <div style={{ width: 24, height: 24, borderRadius: '50%', background: 'var(--primary)', color: 'white', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '0.7rem', fontWeight: 600, flexShrink: 0 }}>
                              {lead.responsavelNome.charAt(0).toUpperCase()}
                            </div>
                          )}
                          <span>{lead.responsavelNome}</span>
                        </div>
                      ) : '-'}
                    </td>
                    <td>
                      <div style={{ display: 'flex', gap: '2px', alignItems: 'center' }}>
                        <button className="icon-btn" title="Ver" onClick={() => navigate(`/leads/${lead.id}?readonly=true`, { state: { from: location.pathname } })}>
                          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                        </button>
                        <button className="icon-btn" title="Editar" onClick={() => navigate(`/leads/${lead.id}/editar`, { state: { from: location.pathname } })}>
                          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                        </button>
                        <button className="icon-btn icon-btn-danger" title="Excluir" onClick={() => excluirLead(lead.id)}>
                          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/></svg>
                        </button>
                      </div>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>

          <div className="paginacao">
            <button disabled={pagina <= 1} onClick={() => setPagina(p => p - 1)}>
              Anterior
            </button>
            <span>Página {pagina} de {Math.ceil(total / 50) || 1}</span>
            <button disabled={pagina >= Math.ceil(total / 50)} onClick={() => setPagina(p => p + 1)}>
              Próxima
            </button>
          </div>
        </>
      )}
    </div>
  );
}
