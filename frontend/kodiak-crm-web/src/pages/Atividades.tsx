import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Atividade, PaginatedResponse } from '../types';
import FilterBar, { type FiltroConfig } from '../components/FilterBar';

const filtroConfigs: FiltroConfig[] = [
  {
    campo: 'tipo',
    label: 'Tipo',
    tipo: 'select',
    opcoes: [
      { valor: 'reuniao', label: 'Reunião' },
      { valor: 'ligacao', label: 'Ligação' },
      { valor: 'email', label: 'E-mail' },
      { valor: 'tarefa', label: 'Tarefa' },
      { valor: 'visita', label: 'Visita' },
      { valor: 'followup', label: 'Follow-up' }
    ]
  },
  {
    campo: 'concluida',
    label: 'Status',
    tipo: 'select',
    opcoes: [
      { valor: 'false', label: 'Pendente' },
      { valor: 'true', label: 'Concluída' }
    ]
  },
  {
    campo: 'responsavel',
    label: 'Responsável',
    tipo: 'texto',
    placeholder: 'Buscar por responsável...',
  },
  { campo: 'dataInicio', label: 'Data Início', tipo: 'data' },
  { campo: 'dataFim', label: 'Data Fim', tipo: 'data' }
];

export default function Atividades() {
  const [atividades, setAtividades] = useState<Atividade[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [valoresFiltro, setValoresFiltro] = useState<Record<string, string>>({
    tipo: '',
    concluida: '',
    responsavel: '',
    dataInicio: '',
    dataFim: ''
  });
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarAtividades();
  }, [pagina, busca, valoresFiltro]);

  const carregarAtividades = async () => {
    setCarregando(true);
    try {
      const params: Record<string, string | number> = { pagina, itensPorPagina: 20, busca };
      if (valoresFiltro.tipo) params.tipo = valoresFiltro.tipo;
      if (valoresFiltro.concluida) params.concluida = valoresFiltro.concluida;
      if (valoresFiltro.responsavel) params.responsavel = valoresFiltro.responsavel;
      if (valoresFiltro.dataInicio) params.dataInicio = valoresFiltro.dataInicio;
      if (valoresFiltro.dataFim) params.dataFim = valoresFiltro.dataFim;

      const response = await api.get<PaginatedResponse<Atividade>>('/atividade', { params });
      setAtividades(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar atividades:', error);
    } finally {
      setCarregando(false);
    }
  };

  const handleMudarFiltro = (campo: string, valor: string) => {
    setValoresFiltro(prev => ({ ...prev, [campo]: valor }));
    setPagina(1);
  };

  const handleLimparFiltros = () => {
    setValoresFiltro({ tipo: '', concluida: '', responsavel: '', dataInicio: '', dataFim: '' });
    setPagina(1);
  };

  const tipoColors: Record<string, string> = {
    ligacao: '#3b82f6',
    reuniao: '#10b981',
    visita: '#f59e0b',
    tarefa: '#8b5cf6',
    email: '#06b6d4',
    whatsapp: '#25d366'
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Atividades</h1>
        <button onClick={() => navigate('/atividades/novo')} className="btn-primary">
          Nova Atividade
        </button>
      </div>
      
      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar atividades..."
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
                <th>Título</th>
                <th>Tipo</th>
                <th>Parceiro</th>
                <th>Responsável</th>
                <th>Data Início</th>
                <th>Status</th>
                <th style={{ width: 100 }}>Ações</th>
              </tr>
            </thead>
            <tbody>
              {atividades.map((atividade) => (
                <tr key={atividade.id}>
                  <td>{atividade.titulo}</td>
                  <td>
                    <span 
                      className="status-badge"
                      style={{ backgroundColor: tipoColors[atividade.tipo] || '#6b7280' }}
                    >
                      {atividade.tipo}
                    </span>
                  </td>
                  <td>{atividade.parceiroNome || '-'}</td>
                  <td>{atividade.responsavelNome || '-'}</td>
                  <td>{atividade.dataInicio ? new Date(atividade.dataInicio).toLocaleDateString('pt-BR') : '-'}</td>
                  <td>
                    <span className={`status-badge ${atividade.concluida ? 'status-ativo' : 'status-inativo'}`}>
                      {atividade.concluida ? 'Concluída' : 'Pendente'}
                    </span>
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '2px', alignItems: 'center' }}>
                      <button className="icon-btn" title="Ver" onClick={() => navigate(`/atividades/${atividade.id}`)}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                      </button>
                      <button className="icon-btn" title="Editar" onClick={() => navigate(`/atividades/${atividade.id}`)}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                      </button>
                      <button className="icon-btn icon-btn-danger" title="Excluir" onClick={() => { if (confirm('Deseja excluir esta atividade?')) { api.delete(`/atividade/${atividade.id}`).then(() => carregarAtividades()); } }}>
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
