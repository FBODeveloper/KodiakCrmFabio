import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../api/axios';
import type { Lead, PaginatedResponse } from '../types';

export default function Leads() {
  const [leads, setLeads] = useState<Lead[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [statusFiltro, setStatusFiltro] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    carregarLeads();
  }, [pagina, busca, statusFiltro]);

  const carregarLeads = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Lead>>('/lead', {
        params: { pagina, itensPorPagina: 20, busca, status: statusFiltro }
      });
      setLeads(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar leads:', error);
    } finally {
      setCarregando(false);
    }
  };

  const excluirLead = async (id: number) => {
    if (!confirm('Deseja excluir este lead?')) return;

    try {
      await api.delete(`/lead/${id}`);
      carregarLeads();
    } catch (error) {
      console.error('Erro ao excluir lead:', error);
    }
  };

  const statusColors: Record<string, string> = {
    novo: '#3b82f6',
    contato: '#f59e0b',
    qualificado: '#10b981',
    convertido: '#6366f1',
    perdido: '#ef4444'
  };

  const temperaturaInfo: Record<string, { color: string; label: string }> = {
    quente: { color: '#ef4444', label: 'Quente' },
    morno: { color: '#f59e0b', label: 'Morno' },
    frio: { color: '#3b82f6', label: 'Frio' }
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

      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar leads..."
          value={busca}
          onChange={(e) => { setBusca(e.target.value); setPagina(1); }}
        />
        <select value={statusFiltro} onChange={(e) => { setStatusFiltro(e.target.value); setPagina(1); }}>
          <option value="">Todos os status</option>
          <option value="novo">Novo</option>
          <option value="contato">Contato</option>
          <option value="qualificado">Qualificado</option>
          <option value="convertido">Convertido</option>
          <option value="perdido">Perdido</option>
        </select>
      </div>

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
                <th>Status</th>
                <th>Estágio</th>
                <th>Origem</th>
                <th>Responsável</th>
                <th>Ações</th>
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
                    <td>
                      <span
                        className="status-badge"
                        style={{ backgroundColor: statusColors[lead.status] || '#6b7280' }}
                      >
                        {lead.status}
                      </span>
                    </td>
                    <td>{lead.estagioNome || '-'}</td>
                    <td>{lead.source || '-'}</td>
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
                      <button onClick={() => navigate(`/leads/${lead.id}`, { state: { from: location.pathname } })}>Ver</button>
                      <button onClick={() => navigate(`/leads/${lead.id}/editar`, { state: { from: location.pathname } })} className="btn-secondary">
                        Editar
                      </button>
                      <button onClick={() => excluirLead(lead.id)} className="btn-danger">
                        Excluir
                      </button>
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
            <span>Página {pagina} de {Math.ceil(total / 20) || 1}</span>
            <button disabled={pagina >= Math.ceil(total / 20)} onClick={() => setPagina(p => p + 1)}>
              Próxima
            </button>
          </div>
        </>
      )}
    </div>
  );
}
