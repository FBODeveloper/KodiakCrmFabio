import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../api/axios';
import type { LeadKanban, LeadEstagio } from '../types';

export default function LeadKanban() {
  const navigate = useNavigate();
  const location = useLocation();
  const [kanban, setKanban] = useState<LeadKanban | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [leadArrastando, setLeadArrastando] = useState<number | null>(null);
  const [colunasAtivas, setColunasAtivas] = useState<Set<number>>(new Set());

  const [showEstagios, setShowEstagios] = useState(false);
  const [estagios, setEstagios] = useState<LeadEstagio[]>([]);
  const [editEstagio, setEditEstagio] = useState<LeadEstagio | null>(null);
  const [formNome, setFormNome] = useState('');
  const [formCor, setFormCor] = useState('#3b82f6');
  const [formOrdem, setFormOrdem] = useState(1);
  const [erroEstagio, setErroEstagio] = useState('');

  useEffect(() => {
    carregarKanban();
  }, []);

  const carregarKanban = async () => {
    setCarregando(true);
    try {
      const response = await api.get<LeadKanban>('/lead/kanban');
      setKanban(response.data);
      setEstagios(response.data.estagios);
    } catch (error) {
      console.error('Erro ao carregar kanban:', error);
    } finally {
      setCarregando(false);
    }
  };

  const carregarEstagios = async () => {
    try {
      const response = await api.get<LeadEstagio[]>('/lead/estagios');
      setEstagios(response.data);
    } catch (error) {
      console.error('Erro ao carregar estágios:', error);
    }
  };

  const abrirFormEstagio = (estagio?: LeadEstagio) => {
    if (estagio) {
      setEditEstagio(estagio);
      setFormNome(estagio.nome);
      setFormCor(estagio.cor);
      setFormOrdem(estagio.ordem);
    } else {
      setEditEstagio(null);
      setFormNome('');
      setFormCor('#3b82f6');
      setFormOrdem(estagios.length + 1);
    }
    setErroEstagio('');
  };

  const salvarEstagio = async () => {
    if (!formNome.trim()) {
      setErroEstagio('Nome é obrigatório');
      return;
    }

    try {
      if (editEstagio) {
        await api.put(`/lead/estagios/${editEstagio.id}`, {
          nome: formNome.trim(),
          cor: formCor,
          ordem: formOrdem
        });
      } else {
        await api.post('/lead/estagios', {
          nome: formNome.trim(),
          cor: formCor,
          ordem: formOrdem
        });
      }
      await carregarEstagios();
      await carregarKanban();
      setEditEstagio(null);
      setFormNome('');
      setFormCor('#3b82f6');
      setFormOrdem(estagios.length + 1);
    } catch (error: any) {
      setErroEstagio(error.response?.data?.mensagem || 'Erro ao salvar estágio');
    }
  };

  const excluirEstagio = async (id: number) => {
    if (!confirm('Excluir este estágio? Leads nele perderão a vinculação.')) return;
    try {
      await api.delete(`/lead/estagios/${id}`);
      await carregarEstagios();
      await carregarKanban();
    } catch (error) {
      console.error('Erro ao excluir estágio:', error);
    }
  };

  const moverLead = async (leadId: number, novoEstagioId: number) => {
    try {
      await api.post(`/lead/${leadId}/mover`, { idEstagio: novoEstagioId });

      if (kanban) {
        const novasColunas = kanban.colunas.map(col => ({
          ...col,
          leads: col.leads.filter(l => l.id !== leadId)
        }));

        const colunaDestino = novasColunas.find(col => col.estagioId === novoEstagioId);
        if (colunaDestino) {
          const lead = kanban.colunas
            .flatMap(c => c.leads)
            .find(l => l.id === leadId);
          if (lead) {
            colunaDestino.leads.push({ ...lead, idEstagio: novoEstagioId });
          }
        }

        setKanban({ ...kanban, colunas: novasColunas });
      }
    } catch (error) {
      console.error('Erro ao mover lead:', error);
      carregarKanban();
    }
  };

  const excluirLead = async (id: number) => {
    if (!confirm('Deseja excluir este lead?')) return;
    try {
      await api.delete(`/lead/${id}`);
      carregarKanban();
    } catch (error) {
      console.error('Erro ao excluir lead:', error);
    }
  };

  const handleDragStart = (e: React.DragEvent, leadId: number) => {
    e.dataTransfer.setData('text/plain', leadId.toString());
    e.dataTransfer.effectAllowed = 'move';
    setLeadArrastando(leadId);
  };

  const handleDragEnd = () => {
    setLeadArrastando(null);
    setColunasAtivas(new Set());
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
    e.dataTransfer.dropEffect = 'move';
  };

  const handleDragEnter = (e: React.DragEvent, estagioId: number) => {
    e.preventDefault();
    setColunasAtivas(prev => {
      const next = new Set(prev);
      next.add(estagioId);
      return next;
    });
  };

  const handleDragLeave = (_e: React.DragEvent, estagioId: number) => {
    setColunasAtivas(prev => {
      const next = new Set(prev);
      next.delete(estagioId);
      return next;
    });
  };

  const handleDrop = (e: React.DragEvent, estagioId: number) => {
    e.preventDefault();
    const leadId = parseInt(e.dataTransfer.getData('text/plain'));
    if (leadId) {
      moverLead(leadId, estagioId);
    }
    setLeadArrastando(null);
    setColunasAtivas(new Set());
  };

  const temperaturaInfo: Record<string, { color: string; label: string }> = {
    quente: { color: '#ef4444', label: 'Quente' },
    morno: { color: '#f59e0b', label: 'Morno' },
    frio: { color: '#3b82f6', label: 'Frio' }
  };

  if (carregando) {
    return <div className="carregando">Carregando...</div>;
  }

  if (!kanban || kanban.colunas.length === 0) {
    return (
      <div className="pagina">
        <div className="pagina-header">
          <h1>Leads - Kanban</h1>
          <div className="pagina-header-actions">
            <button onClick={() => setShowEstagios(true)} className="btn-secondary">
              Gerenciar Estágios
            </button>
            <button onClick={() => navigate('/leads')} className="btn-secondary">
              Voltar
            </button>
          </div>
        </div>
        <p>Nenhum estágio configurado. Clique em "Gerenciar Estágios" para criar.</p>
      </div>
    );
  }

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Leads - Kanban</h1>
        <div className="pagina-header-actions">
          <button onClick={() => setShowEstagios(true)} className="btn-secondary">
            Gerenciar Estágios
          </button>
          <button onClick={() => navigate('/leads')} className="btn-secondary">
            Lista
          </button>
          <button onClick={() => navigate('/leads/novo')} className="btn-primary">
            Novo Lead
          </button>
        </div>
      </div>

      <div className="kanban-container">
        {kanban.colunas.map(coluna => (
          <div
            key={coluna.estagioId}
            className={`kanban-coluna ${colunasAtivas.has(coluna.estagioId) ? 'kanban-coluna-hover' : ''}`}
            onDragOver={handleDragOver}
            onDragEnter={(e) => handleDragEnter(e, coluna.estagioId)}
            onDragLeave={(e) => handleDragLeave(e, coluna.estagioId)}
            onDrop={(e) => handleDrop(e, coluna.estagioId)}
          >
            <div
              className="kanban-coluna-header"
              style={{ borderBottomColor: coluna.cor }}
            >
              <h3 style={{ color: coluna.cor }}>{coluna.estagioNome}</h3>
              <span className="kanban-count">{coluna.leads.length}</span>
            </div>

            <div className="kanban-coluna-body">
              {coluna.leads.map(lead => {
                const temp = temperaturaInfo[lead.temperatura] || temperaturaInfo.frio;

                return (
                  <div
                    key={lead.id}
                    className={`kanban-card ${leadArrastando === lead.id ? 'kanban-card-arrastando' : ''}`}
                    draggable
                    onDragStart={(e) => handleDragStart(e, lead.id)}
                    onDragEnd={handleDragEnd}
                  >
                    <div className="kanban-card-header">
                      <span
                        className="temperatura-dot"
                        style={{ backgroundColor: temp.color }}
                        title={temp.label}
                      />
                      <strong>{lead.nome}</strong>
                      {lead.responsavelAvatar ? (
                        <img
                          src={lead.responsavelAvatar}
                          alt={lead.responsavelNome || ''}
                          title={lead.responsavelNome || ''}
                          style={{ width: 20, height: 20, borderRadius: '50%', objectFit: 'cover', marginLeft: 'auto' }}
                        />
                      ) : lead.responsavelNome ? (
                        <div
                          title={lead.responsavelNome}
                          style={{ width: 20, height: 20, borderRadius: '50%', background: 'var(--primary)', color: 'white', display: 'flex', alignItems: 'center', justifyContent: 'center', fontSize: '0.6rem', fontWeight: 600, marginLeft: 'auto', flexShrink: 0 }}
                        >
                          {lead.responsavelNome.charAt(0).toUpperCase()}
                        </div>
                      ) : null}
                    </div>

                    {lead.empresa && (
                      <p className="kanban-card-empresa">{lead.empresa}</p>
                    )}

                    {lead.email && (
                      <p className="kanban-card-contato">{lead.email}</p>
                    )}

                    <div className="kanban-card-acoes" style={{ display: 'flex', gap: '2px', justifyContent: 'flex-end' }}>
                      <button className="icon-btn" title="Ver" onClick={() => navigate(`/leads/${lead.id}?readonly=true`, { state: { from: location.pathname } })}>
                        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                      </button>
                      <button className="icon-btn" title="Editar" onClick={() => navigate(`/leads/${lead.id}/editar`, { state: { from: location.pathname } })}>
                        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                      </button>
                      <button className="icon-btn icon-btn-danger" title="Excluir" onClick={() => excluirLead(lead.id)}>
                        <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/></svg>
                      </button>
                    </div>
                  </div>
                );
              })}

              {coluna.leads.length === 0 && (
                <p className="kanban-vazio">Nenhum lead neste estágio</p>
              )}
            </div>
          </div>
        ))}
      </div>

      {showEstagios && (
        <div className="modal-overlay" onClick={() => setShowEstagios(false)}>
          <div className="modal modal-lg" onClick={e => e.stopPropagation()}>
            <h2>Gerenciar Estágios do Pipeline</h2>

            <div className="estagios-lista">
              {estagios.map(estagio => (
                <div key={estagio.id} className="estagio-item">
                  <div className="estagio-info">
                    <span className="estagio-cor" style={{ backgroundColor: estagio.cor }} />
                    <span className="estagio-ordem">{estagio.ordem}.</span>
                    <span className="estagio-nome">{estagio.nome}</span>
                  </div>
                  <div className="estagio-acoes">
                    <button className="btn-small" onClick={() => abrirFormEstagio(estagio)}>
                      Editar
                    </button>
                    <button className="btn-small btn-danger" onClick={() => excluirEstagio(estagio.id)}>
                      Excluir
                    </button>
                  </div>
                </div>
              ))}
            </div>

            <div className="estagio-form-inline">
              <h3>{editEstagio ? 'Editar Estágio' : 'Novo Estágio'}</h3>
              <div className="form-row">
                <div className="form-group">
                  <label>Nome *</label>
                  <input
                    type="text"
                    value={formNome}
                    onChange={e => setFormNome(e.target.value)}
                    placeholder="Ex: Novo, Contato, Proposta..."
                  />
                </div>
                <div className="form-group" style={{ maxWidth: 100 }}>
                  <label>Ordem</label>
                  <input
                    type="number"
                    value={formOrdem}
                    onChange={e => setFormOrdem(parseInt(e.target.value) || 1)}
                    min={1}
                  />
                </div>
                <div className="form-group" style={{ maxWidth: 100 }}>
                  <label>Cor</label>
                  <input
                    type="color"
                    value={formCor}
                    onChange={e => setFormCor(e.target.value)}
                    style={{ height: 38, padding: 2, cursor: 'pointer' }}
                  />
                </div>
              </div>

              {erroEstagio && <div className="erro">{erroEstagio}</div>}

              <div className="form-actions">
                {editEstagio && (
                  <button
                    className="btn-secondary"
                    onClick={() => { setEditEstagio(null); setFormNome(''); setFormCor('#3b82f6'); setFormOrdem(estagios.length + 1); }}
                  >
                    Cancelar
                  </button>
                )}
                <button className="btn-primary" onClick={salvarEstagio}>
                  {editEstagio ? 'Atualizar' : 'Criar'}
                </button>
              </div>
            </div>

            <div className="modal-actions">
              <button className="btn-secondary" onClick={() => setShowEstagios(false)}>
                Fechar
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
