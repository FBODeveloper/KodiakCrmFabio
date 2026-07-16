import { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
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
    dataInicio: '',
    dataFim: '',
  });
  const navigate = useNavigate();

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
    { campo: 'dataInicio', label: 'Data Início', tipo: 'data' },
    { campo: 'dataFim', label: 'Data Fim', tipo: 'data' },
  ];

  const carregarOportunidades = useCallback(async () => {
    setCarregando(true);
    try {
      const params: Record<string, string | number> = { pagina, itensPorPagina: 20, busca };
      if (valoresFiltro.status) params.status = valoresFiltro.status;
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
    setValoresFiltro({ status: '', dataInicio: '', dataFim: '' });
    setPagina(1);
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
                    <button onClick={() => navigate(`/oportunidades/${oportunidade.id}`)}>
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
