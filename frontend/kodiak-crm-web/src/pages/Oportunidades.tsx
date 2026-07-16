import { useState, useEffect, useCallback } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import api from '../api/axios';
import type { Oportunidade, PaginatedResponse } from '../types';
import FilterBar, { type FiltroConfig } from '../components/FilterBar';

export default function Oportunidades() {
  const [oportunidades, setOportunidades] = useState<Oportunidade[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const [valoresFiltro, setValoresFiltro] = useState<Record<string, string>>({
    status: '',
    responsavel: '',
    dataInicio: '',
    dataFim: '',
  });
  const navigate = useNavigate();
  const location = useLocation();

  const filtroConfigs: FiltroConfig[] = [
    {
      campo: 'status',
      label: 'Status',
      tipo: 'select',
      opcoes: [
        { valor: 'aberta', label: 'Aberta' },
        { valor: 'ganha', label: 'Ganha' },
        { valor: 'perdida', label: 'Perdida' },
      ],
    },
    {
      campo: 'responsavel',
      label: 'Responsável',
      tipo: 'texto',
      placeholder: 'Buscar por responsável...',
    },
    { campo: 'dataInicio', label: 'Data Início', tipo: 'data' },
    { campo: 'dataFim', label: 'Data Fim', tipo: 'data' },
  ];

  const carregarOportunidades = useCallback(async () => {
    setCarregando(true);
    try {
      const params: Record<string, string | number> = { pagina, itensPorPagina: 20, busca };
      if (valoresFiltro.status) params.status = valoresFiltro.status;
      if (valoresFiltro.responsavel) params.responsavel = valoresFiltro.responsavel;
      if (valoresFiltro.dataInicio) params.dataInicio = valoresFiltro.dataInicio;
      if (valoresFiltro.dataFim) params.dataFim = valoresFiltro.dataFim;

      const response = await api.get<PaginatedResponse<Oportunidade>>('/oportunidade', { params });
      setOportunidades(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar oportunidades:', error);
    } finally {
      setCarregando(false);
    }
  }, [pagina, busca, valoresFiltro]);

  useEffect(() => {
    carregarOportunidades();
  }, [carregarOportunidades]);

  const handleMudarFiltro = (campo: string, valor: string) => {
    setValoresFiltro(prev => ({ ...prev, [campo]: valor }));
    setPagina(1);
  };

  const handleLimparFiltros = () => {
    setValoresFiltro({ status: '', responsavel: '', dataInicio: '', dataFim: '' });
    setPagina(1);
  };

  const handleExcluir = async (id: number) => {
    if (!confirm('Tem certeza que deseja excluir esta oportunidade?')) return;
    try {
      await api.delete(`/oportunidade/${id}`);
      carregarOportunidades();
    } catch (error) {
      console.error('Erro ao excluir oportunidade:', error);
    }
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Oportunidades</h1>
        <button onClick={() => navigate('/oportunidades/novo')} className="btn-primary">
          Nova Oportunidade
        </button>
      </div>
      
      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar oportunidades..."
          value={busca}
          onChange={(e) => { setBusca(e.target.value); setPagina(1); }}
        />
        <FilterBar
          filtros={filtroConfigs}
          valores={valoresFiltro}
          onMudar={handleMudarFiltro}
          onLimpar={handleLimparFiltros}
        />
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
                <th>Estágio</th>
                <th>Valor</th>
                <th>Previsão</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {oportunidades.map((oportunidade) => (
                <tr key={oportunidade.id}>
                  <td>
                    {oportunidade.titulo}
                    {oportunidade.motivoPerda && (
                      <span className="badge-perdida">Perdida</span>
                    )}
                  </td>
                  <td>{oportunidade.parceiroNome || '-'}</td>
                  <td>{oportunidade.estagioNome || '-'}</td>
                  <td>
                    {oportunidade.valor
                      ? new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(oportunidade.valor)
                      : '-'}
                  </td>
                  <td>{oportunidade.dataPrevisao ? new Date(oportunidade.dataPrevisao).toLocaleDateString('pt-BR') : '-'}</td>
                  <td>
                    <div style={{ display: 'flex', gap: '2px', alignItems: 'center' }}>
                      <button
                        className="icon-btn"
                        title="Ver"
                        onClick={() => navigate(`/oportunidades/${oportunidade.id}?readonly=true`, { state: { from: location.pathname } })}
                      >
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                      </button>
                      <button
                        className="icon-btn"
                        title="Editar"
                        onClick={() => navigate(`/oportunidades/${oportunidade.id}`, { state: { from: location.pathname } })}
                      >
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                      </button>
                      <button
                        className="icon-btn icon-btn-danger"
                        title="Excluir"
                        onClick={() => handleExcluir(oportunidade.id)}
                      >
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/></svg>
                      </button>
                    </div>
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
