import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Proposta, PaginatedResponse } from '../types';

export default function Propostas() {
  const [propostas, setPropostas] = useState<Proposta[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [statusFiltro, setStatusFiltro] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarPropostas();
  }, [pagina, busca, statusFiltro]);

  const carregarPropostas = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Proposta>>('/proposta', {
        params: { pagina, itensPorPagina: 20, busca, status: statusFiltro }
      });
      setPropostas(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar propostas:', error);
    } finally {
      setCarregando(false);
    }
  };

  const statusColors: Record<string, string> = {
    rascunho: '#6b7280',
    enviada: '#3b82f6',
    aprovada: '#10b981',
    rejeitada: '#ef4444',
    cancelada: '#f59e0b'
  };

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
        <select value={statusFiltro} onChange={(e) => { setStatusFiltro(e.target.value); setPagina(1); }}>
          <option value="">Todos os status</option>
          <option value="rascunho">Rascunho</option>
          <option value="enviada">Enviada</option>
          <option value="aprovada">Aprovada</option>
          <option value="rejeitada">Rejeitada</option>
          <option value="cancelada">Cancelada</option>
        </select>
      </div>
      
      {carregando ? (
        <div className="carregando">Carregando...</div>
      ) : (
        <>
          <table className="tabela">
            <thead>
              <tr>
                <th>Título</th>
                <th>Parceiro</th>
                <th>Valor Total</th>
                <th>Validade</th>
                <th>Status</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {propostas.map((proposta) => (
                <tr key={proposta.id}>
                  <td>{proposta.titulo}</td>
                  <td>{proposta.parceiroNome || '-'}</td>
                  <td>
                    {proposta.valorTotal
                      ? new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(proposta.valorTotal)
                      : '-'}
                  </td>
                  <td>{proposta.dataValidade ? new Date(proposta.dataValidade).toLocaleDateString('pt-BR') : '-'}</td>
                  <td>
                    <span 
                      className="status-badge"
                      style={{ backgroundColor: statusColors[proposta.status] || '#6b7280' }}
                    >
                      {proposta.status}
                    </span>
                  </td>
                  <td>
                    <button onClick={() => navigate(`/propostas/${proposta.id}`)}>
                      Editar
                    </button>
                  </td>
                </tr>
              ))}
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
