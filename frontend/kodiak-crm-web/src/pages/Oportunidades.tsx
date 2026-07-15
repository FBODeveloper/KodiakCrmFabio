import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Oportunidade, PaginatedResponse } from '../types';

export default function Oportunidades() {
  const [oportunidades, setOportunidades] = useState<Oportunidade[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarOportunidades();
  }, [pagina, busca]);

  const carregarOportunidades = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Oportunidade>>('/oportunidade', {
        params: { pagina, itensPorPagina: 20, busca }
      });
      setOportunidades(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar oportunidades:', error);
    } finally {
      setCarregando(false);
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
                  <td>{oportunidade.titulo}</td>
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
