import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Atividade, PaginatedResponse } from '../types';

export default function Atividades() {
  const [atividades, setAtividades] = useState<Atividade[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [tipoFiltro, setTipoFiltro] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarAtividades();
  }, [pagina, busca, tipoFiltro]);

  const carregarAtividades = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Atividade>>('/atividade', {
        params: { pagina, itensPorPagina: 20, busca, tipo: tipoFiltro }
      });
      setAtividades(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar atividades:', error);
    } finally {
      setCarregando(false);
    }
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
        <select value={tipoFiltro} onChange={(e) => { setTipoFiltro(e.target.value); setPagina(1); }}>
          <option value="">Todos os tipos</option>
          <option value="ligacao">Ligação</option>
          <option value="reuniao">Reunião</option>
          <option value="visita">Visita</option>
          <option value="tarefa">Tarefa</option>
          <option value="email">E-mail</option>
          <option value="whatsapp">WhatsApp</option>
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
                <th>Tipo</th>
                <th>Parceiro</th>
                <th>Responsável</th>
                <th>Data Início</th>
                <th>Status</th>
                <th>Ações</th>
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
                    <button onClick={() => navigate(`/atividades/${atividade.id}`)}>
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
