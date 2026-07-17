import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Funil } from '../types';

export default function Funis() {
  const [funis, setFunis] = useState<Funil[]>([]);
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarFunis();
  }, []);

  const carregarFunis = async () => {
    setCarregando(true);
    try {
      const response = await api.get<Funil[]>('/funil');
      setFunis(response.data);
    } catch (error) {
      console.error('Erro ao carregar funis:', error);
    } finally {
      setCarregando(false);
    }
  };

  const handleExcluir = async (id: number) => {
    if (!confirm('Tem certeza que deseja excluir este funil?')) return;
    try {
      await api.delete(`/funil/${id}`);
      carregarFunis();
    } catch (error) {
      console.error('Erro ao excluir funil:', error);
    }
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Funis</h1>
        <button onClick={() => navigate('/funis/novo')} className="btn-primary">
          Novo Funil
        </button>
      </div>

      {carregando ? (
        <div className="carregando">Carregando...</div>
      ) : (
        <table className="tabela">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Estágios</th>
              <th>Status</th>
              <th style={{ width: 100 }}>Ações</th>
            </tr>
          </thead>
          <tbody>
            {funis.map((funil) => (
              <tr key={funil.id}>
                <td>
                  <div>
                    <strong>{funil.nome}</strong>
                    {funil.estagios && funil.estagios.length > 0 && (
                      <div style={{ fontSize: '0.8rem', color: 'var(--text-secondary, #888)', marginTop: 2 }}>
                        {funil.estagios.map(e => e.nome).join(' → ')}
                      </div>
                    )}
                  </div>
                </td>
                <td>{funil.estagios ? funil.estagios.length : 0}</td>
                <td>
                  <span
                    className="status-badge"
                    style={{ backgroundColor: funil.ativo ? '#10b981' : '#6b7280' }}
                  >
                    {funil.ativo ? 'Ativo' : 'Inativo'}
                  </span>
                </td>
                <td>
                  <div style={{ display: 'flex', gap: '2px', alignItems: 'center' }}>
                    <button
                      className="icon-btn"
                      title="Ver"
                      onClick={() => navigate(`/funis/${funil.id}?readonly=true`)}
                    >
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                    </button>
                    <button
                      className="icon-btn"
                      title="Editar"
                      onClick={() => navigate(`/funis/${funil.id}`)}
                    >
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                    </button>
                    <button
                      className="icon-btn icon-btn-danger"
                      title="Excluir"
                      onClick={() => handleExcluir(funil.id)}
                    >
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/></svg>
                    </button>
                  </div>
                </td>
              </tr>
            ))}
            {funis.length === 0 && (
              <tr><td colSpan={4} className="vazio">Nenhum funil encontrado</td></tr>
            )}
          </tbody>
        </table>
      )}
    </div>
  );
}
