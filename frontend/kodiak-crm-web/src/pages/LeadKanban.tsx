import { useState, useEffect, useRef } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../api/axios';
import type { LeadKanban, Lead } from '../types';

export default function LeadKanban() {
  const navigate = useNavigate();
  const location = useLocation();
  const [kanban, setKanban] = useState<LeadKanban | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [leadArrastando, setLeadArrastando] = useState<number | null>(null);
  const [colunasAtivas, setColunasAtivas] = useState<Set<number>>(new Set());

  useEffect(() => {
    carregarKanban();
  }, []);

  const carregarKanban = async () => {
    setCarregando(true);
    try {
      const response = await api.get<LeadKanban>('/lead/kanban');
      setKanban(response.data);
    } catch (error) {
      console.error('Erro ao carregar kanban:', error);
    } finally {
      setCarregando(false);
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

  const handleDragLeave = (e: React.DragEvent, estagioId: number) => {
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
          <button onClick={() => navigate('/leads')} className="btn-secondary">
            Voltar
          </button>
        </div>
        <p>Nenhum estágio configurado. Cadastre estágios em Leads &gt; Kanban.</p>
      </div>
    );
  }

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Leads - Kanban</h1>
        <div className="pagina-header-actions">
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

                    <div className="kanban-card-acoes">
                      <button
                        onClick={() => navigate(`/leads/${lead.id}`, { state: { from: location.pathname } })}
                        className="btn-small"
                      >
                        Ver
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
    </div>
  );
}
