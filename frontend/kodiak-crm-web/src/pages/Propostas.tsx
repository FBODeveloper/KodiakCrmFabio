import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../api/axios';
import type { Proposta, PaginatedResponse } from '../types';
import FilterBar from '../components/FilterBar';
import type { FiltroConfig } from '../components/FilterBar';

const filtroConfigs: FiltroConfig[] = [
  {
    campo: 'status',
    label: 'Status',
    tipo: 'select',
    opcoes: [
      { valor: 'rascunho', label: 'Rascunho' },
      { valor: 'enviada', label: 'Enviada' },
      { valor: 'aprovada', label: 'Aprovada' },
      { valor: 'rejeitada', label: 'Rejeitada' },
    ],
  },
  {
    campo: 'dataInicio',
    label: 'Data Início',
    tipo: 'data',
  },
  {
    campo: 'dataFim',
    label: 'Data Fim',
    tipo: 'data',
  },
];

export default function Propostas() {
  const [propostas, setPropostas] = useState<Proposta[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const [valoresFiltro, setValoresFiltro] = useState<Record<string, string>>({
    status: '',
    dataInicio: '',
    dataFim: '',
  });
  const navigate = useNavigate();
  const location = useLocation();

  const [showRejeicaoModal, setShowRejeicaoModal] = useState(false);
  const [rejeitandoId, setRejeitandoId] = useState<number | null>(null);
  const [motivoRejeicao, setMotivoRejeicao] = useState('');
  const [alterandoStatus, setAlterandoStatus] = useState(false);

  useEffect(() => {
    carregarPropostas();
  }, [pagina, busca, valoresFiltro]);

  const carregarPropostas = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Proposta>>('/proposta', {
        params: {
          pagina,
          itensPorPagina: 20,
          busca,
          status: valoresFiltro.status || undefined,
          dataInicio: valoresFiltro.dataInicio || undefined,
          dataFim: valoresFiltro.dataFim || undefined,
        },
      });
      setPropostas(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar propostas:', error);
    } finally {
      setCarregando(false);
    }
  };

  const handleMudarFiltro = (campo: string, valor: string) => {
    setValoresFiltro(prev => ({ ...prev, [campo]: valor }));
    setPagina(1);
  };

  const handleLimparFiltros = () => {
    setValoresFiltro({ status: '', dataInicio: '', dataFim: '' });
    setPagina(1);
  };

  const handleExcluir = async (id: number) => {
    if (!confirm('Tem certeza que deseja excluir esta proposta?')) return;
    try {
      await api.delete(`/proposta/${id}`);
      carregarPropostas();
    } catch (error) {
      console.error('Erro ao excluir proposta:', error);
    }
  };

  const handleAlterarStatus = async (id: number, novoStatus: string) => {
    if (novoStatus === 'rejeitada') {
      setRejeitandoId(id);
      setMotivoRejeicao('');
      setShowRejeicaoModal(true);
      return;
    }

    setAlterandoStatus(true);
    try {
      await api.patch(`/proposta/${id}/status`, { status: novoStatus });
      carregarPropostas();
    } catch (error) {
      console.error('Erro ao alterar status:', error);
    } finally {
      setAlterandoStatus(false);
    }
  };

  const handleConfirmarRejeicao = async () => {
    if (!rejeitandoId) return;
    setAlterandoStatus(true);
    try {
      await api.patch(`/proposta/${rejeitandoId}/status`, {
        status: 'rejeitada',
        motivoRejeicao: motivoRejeicao || null,
      });
      setShowRejeicaoModal(false);
      setRejeitandoId(null);
      setMotivoRejeicao('');
      carregarPropostas();
    } catch (error) {
      console.error('Erro ao rejeitar proposta:', error);
    } finally {
      setAlterandoStatus(false);
    }
  };

  const statusColors: Record<string, string> = {
    rascunho: '#6b7280',
    enviada: '#3b82f6',
    aprovada: '#10b981',
    rejeitada: '#ef4444',
    cancelada: '#f59e0b'
  };

  const statusBotoes: { status: string; label: string; cor: string }[] = [
    { status: 'pendente', label: 'Pendente', cor: '#f59e0b' },
    { status: 'enviada', label: 'Enviada', cor: '#3b82f6' },
    { status: 'aprovada', label: 'Aprovada', cor: '#10b981' },
    { status: 'rejeitada', label: 'Rejeitada', cor: '#ef4444' },
  ];

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Propostas</h1>
        <button onClick={() => navigate('/propostas/novo')} className="btn-primary">
          Nova Proposta
        </button>
      </div>

      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar propostas..."
          value={busca}
          onChange={(e) => { setBusca(e.target.value); setPagina(1); }}
        />
      </div>

      <FilterBar
        filtros={filtroConfigs}
        valores={valoresFiltro}
        onMudar={handleMudarFiltro}
        onLimpar={handleLimparFiltros}
      />

      {carregando ? (
        <div className="carregando">Carregando...</div>
      ) : (
        <>
          <table className="tabela">
            <thead>
              <tr>
                <th>Número</th>
                <th>Título</th>
                <th>Cliente/Contato</th>
                <th>Data Proposta</th>
                <th>Valor Total</th>
                <th>Validade</th>
                <th>Status</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {propostas.map((proposta) => {
                const podeEditar = proposta.status === 'rascunho' || proposta.status === 'pendente';
                return (
                <tr key={proposta.id}>
                  <td>{proposta.numero || '-'}</td>
                  <td>{proposta.titulo}</td>
                  <td>{proposta.clienteNome || proposta.contatoNome || '-'}</td>
                  <td>{proposta.dataProposta ? new Date(proposta.dataProposta + 'T00:00:00').toLocaleDateString('pt-BR') : '-'}</td>
                  <td>
                    {proposta.valorTotal
                      ? new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(proposta.valorTotal)
                      : '-'}
                  </td>
                  <td>{proposta.dataValidade ? new Date(proposta.dataValidade + 'T00:00:00').toLocaleDateString('pt-BR') : '-'}</td>
                  <td>
                    <span
                      className="status-badge"
                      style={{ backgroundColor: statusColors[proposta.status] || '#6b7280' }}
                    >
                      {proposta.status}
                    </span>
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '2px', alignItems: 'center', flexWrap: 'wrap' }}>
                      {statusBotoes.map((btn) => {
                        if (proposta.status === btn.status) return null;
                        return (
                          <button
                            key={btn.status}
                            onClick={() => handleAlterarStatus(proposta.id, btn.status)}
                            disabled={alterandoStatus}
                            title={btn.label}
                            style={{
                              fontSize: '0.65rem',
                              padding: '2px 6px',
                              borderRadius: '4px',
                              border: `1px solid ${btn.cor}`,
                              background: 'transparent',
                              color: btn.cor,
                              cursor: alterandoStatus ? 'not-allowed' : 'pointer',
                              fontWeight: 600,
                              opacity: alterandoStatus ? 0.6 : 1,
                            }}
                          >
                            {btn.label}
                          </button>
                        );
                      })}
                      <button
                        className="icon-btn"
                        title="Ver"
                        onClick={() => navigate(`/propostas/${proposta.id}?readonly=true`, { state: { from: location.pathname } })}
                      >
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                      </button>
                      {podeEditar && (
                        <button
                          className="icon-btn"
                          title="Editar"
                          onClick={() => navigate(`/propostas/${proposta.id}`, { state: { from: location.pathname } })}
                        >
                          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                        </button>
                      )}
                      <button
                        className="icon-btn icon-btn-danger"
                        title="Excluir"
                        onClick={() => handleExcluir(proposta.id)}
                      >
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
            <span>Página {pagina} de {Math.ceil(total / 20) || 1}</span>
            <button disabled={pagina >= Math.ceil(total / 20)} onClick={() => setPagina(p => p + 1)}>
              Próxima
            </button>
          </div>
        </>
      )}

      {showRejeicaoModal && (
        <div className="modal-overlay" onClick={() => !alterandoStatus && setShowRejeicaoModal(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>Rejeitar Proposta</h2>
            <p className="modal-subtitle">
              Informe o motivo da rejeição (opcional):
            </p>

            <div className="form">
              <div className="form-group">
                <label>Motivo da Rejeição</label>
                <textarea
                  value={motivoRejeicao}
                  onChange={e => setMotivoRejeicao(e.target.value)}
                  rows={3}
                  placeholder="Descreva o motivo da rejeição..."
                />
              </div>
            </div>

            <div className="modal-actions">
              <button
                className="btn-secondary"
                onClick={() => setShowRejeicaoModal(false)}
                disabled={alterandoStatus}
              >
                Não
              </button>
              <button
                className="btn-danger"
                onClick={handleConfirmarRejeicao}
                disabled={alterandoStatus}
              >
                {alterandoStatus ? 'Salvando...' : 'Sim'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
