import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
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
                <th>Cliente</th>
                <th>Valor Total</th>
                <th>Validade</th>
                <th>Status</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {propostas.map((proposta) => (
                <tr key={proposta.id}>
                  <td>{proposta.numero || '-'}</td>
                  <td>{proposta.titulo}</td>
                  <td>{proposta.clienteNome || proposta.parceiroNome || '-'}</td>
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
                    <button onClick={() => handleExcluir(proposta.id)} className="btn-danger btn-small">
                      Excluir
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
