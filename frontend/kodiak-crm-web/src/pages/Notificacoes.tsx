import { useState, useEffect } from 'react';
import api from '../api/axios';
import type { Notificacao } from '../types';

export default function Notificacoes() {
  const [notificacoes, setNotificacoes] = useState<Notificacao[]>([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    carregarNotificacoes();
  }, []);

  const carregarNotificacoes = async () => {
    try {
      const res = await api.get('/notificacao');
      setNotificacoes(res.data);
    } catch (error) {
      console.error('Erro ao carregar notificações:', error);
    } finally {
      setCarregando(false);
    }
  };

  const marcarLida = async (id: number) => {
    try {
      await api.put(`/notificacao/${id}/lida`);
      setNotificacoes(notificacoes.map(n => n.id === id ? { ...n, lida: true } : n));
    } catch (error) {
      console.error('Erro ao marcar como lida:', error);
    }
  };

  const marcarTodasLidas = async () => {
    try {
      await api.put('/notificacao/lidas');
      setNotificacoes(notificacoes.map(n => ({ ...n, lida: true })));
    } catch (error) {
      console.error('Erro ao marcar todas como lidas:', error);
    }
  };

  const excluir = async (id: number) => {
    try {
      await api.delete(`/notificacao/${id}`);
      setNotificacoes(notificacoes.filter(n => n.id !== id));
    } catch (error) {
      console.error('Erro ao excluir notificação:', error);
    }
  };

  const tipoCores: Record<string, string> = {
    sistema: '#6b7280',
    followup: '#3b82f6',
    atividade: '#f59e0b',
    oportunidade: '#10b981',
    lead: '#8b5cf6'
  };

  const naoLidas = notificacoes.filter(n => !n.lida).length;

  if (carregando) {
    return <div className="carregando">Carregando...</div>;
  }

  return (
    <div className="notificacoes-page">
      <div className="panel-header">
        <h1>Notificações {naoLidas > 0 && <span className="badge-count">{naoLidas}</span>}</h1>
        {naoLidas > 0 && (
          <button className="btn btn-secondary btn-sm" onClick={marcarTodasLidas}>
            Marcar todas como lidas
          </button>
        )}
      </div>

      {notificacoes.length === 0 ? (
        <div className="config-section">
          <p className="vazio">Nenhuma notificação</p>
        </div>
      ) : (
        <div className="notificacoes-lista">
          {notificacoes.map((notificacao) => (
            <div
              key={notificacao.id}
              className={`notificacao-item ${notificacao.lida ? '' : 'nao-lida'}`}
            >
              <div className="notificacao-dot" style={{ background: tipoCores[notificacao.tipo] || '#6b7280' }}></div>
              <div className="notificacao-conteudo">
                <div className="notificacao-header">
                  <strong className="notificacao-titulo">{notificacao.titulo}</strong>
                  <span className="notificacao-data">{new Date(notificacao.dataCriacao).toLocaleString('pt-BR')}</span>
                </div>
                <p className="notificacao-mensagem">{notificacao.mensagem}</p>
                <div className="notificacao-footer">
                  <span className="notificacao-tipo-badge" style={{ color: tipoCores[notificacao.tipo] || '#6b7280' }}>
                    {notificacao.tipo}
                  </span>
                  <div className="notificacao-actions">
                    {!notificacao.lida && (
                      <button className="btn-link" onClick={() => marcarLida(notificacao.id)}>
                        Marcar como lida
                      </button>
                    )}
                    <button className="btn-link btn-danger-link" onClick={() => excluir(notificacao.id)}>
                      Excluir
                    </button>
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
