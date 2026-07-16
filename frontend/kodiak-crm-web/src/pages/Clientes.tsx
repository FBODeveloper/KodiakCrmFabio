import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { ClienteDTO, PaginatedResponse } from '../types';
import FilterBar, { type FiltroConfig } from '../components/FilterBar';

export default function Clientes() {
  const [clientes, setClientes] = useState<ClienteDTO[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [filtros, setFiltros] = useState<Record<string, string>>({
    origem: '',
    dataInicio: '',
    dataFim: '',
  });
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  const filtroConfigs: FiltroConfig[] = [
    {
      campo: 'origem',
      label: 'Origem',
      tipo: 'select',
      opcoes: [
        { valor: 'conversao', label: 'Conversão' },
        { valor: 'cadastro_direto', label: 'Cadastro Direto' },
        { valor: 'indicacao', label: 'Indicação' },
        { valor: 'outro', label: 'Outro' },
      ],
    },
    { campo: 'dataInicio', label: 'Data Início', tipo: 'data' },
    { campo: 'dataFim', label: 'Data Fim', tipo: 'data' },
  ];

  useEffect(() => {
    carregarClientes();
  }, [pagina, busca, filtros]);

  const carregarClientes = async () => {
    setCarregando(true);
    try {
      const params: Record<string, string | number> = { pagina, itensPorPagina: 20, busca };
      if (filtros.origem) params.origem = filtros.origem;
      if (filtros.dataInicio) params.dataInicio = filtros.dataInicio;
      if (filtros.dataFim) params.dataFim = filtros.dataFim;
      const response = await api.get<PaginatedResponse<ClienteDTO>>('/cliente', { params });
      setClientes(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar clientes:', error);
    } finally {
      setCarregando(false);
    }
  };

  const handleFiltroMudar = (campo: string, valor: string) => {
    setFiltros(prev => ({ ...prev, [campo]: valor }));
    setPagina(1);
  };

  const handleFiltroLimpar = () => {
    setFiltros({ origem: '', dataInicio: '', dataFim: '' });
    setPagina(1);
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Clientes</h1>
        <button onClick={() => navigate('/clientes/novo')} className="btn-primary">
          Novo Cliente
        </button>
      </div>
      
      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar clientes..."
          value={busca}
          onChange={(e) => { setBusca(e.target.value); setPagina(1); }}
        />
      </div>

      <FilterBar
        filtros={filtroConfigs}
        valores={filtros}
        onMudar={handleFiltroMudar}
        onLimpar={handleFiltroLimpar}
      />
      
      {carregando ? (
        <div className="carregando">Carregando...</div>
      ) : (
        <>
          <table className="tabela">
            <thead>
              <tr>
                <th>Razão Social</th>
                <th>Nome Fantasia</th>
                <th>CNPJ/CPF</th>
                <th>Email</th>
                <th>Telefone</th>
                <th>Origem</th>
                <th style={{ width: 100 }}>Ações</th>
              </tr>
            </thead>
            <tbody>
              {clientes.map((cliente) => (
                <tr key={cliente.id}>
                  <td>{cliente.razaoSocial}</td>
                  <td>{cliente.nomeFantasia || '-'}</td>
                  <td>{cliente.cnpjCpf || '-'}</td>
                  <td>{cliente.email || '-'}</td>
                  <td>{cliente.telefone || '-'}</td>
                  <td>
                    {cliente.origem === 'oportunidade' && (
                      <span className="badge-perdida" style={{ background: 'rgba(16, 185, 129, 0.15)', color: '#10b981' }}>
                        Oportunidade
                      </span>
                    )}
                    {cliente.origem !== 'oportunidade' && (cliente.origem || '-')}
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '2px', alignItems: 'center' }}>
                      <button className="icon-btn" title="Ver" onClick={() => navigate(`/clientes/${cliente.id}`)}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                      </button>
                      <button className="icon-btn" title="Editar" onClick={() => navigate(`/clientes/${cliente.id}`)}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                      </button>
                      <button className="icon-btn icon-btn-danger" title="Excluir" onClick={() => { if (confirm('Deseja excluir este cliente?')) { api.delete(`/cliente/${cliente.id}`).then(() => carregarClientes()); } }}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/></svg>
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {clientes.length === 0 && (
                <tr><td colSpan={7} className="vazio">Nenhum cliente encontrado</td></tr>
              )}
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
