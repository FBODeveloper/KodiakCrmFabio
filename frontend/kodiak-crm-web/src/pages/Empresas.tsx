import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Empresa, PaginatedResponse } from '../types';

export default function Empresas() {
  const [empresas, setEmpresas] = useState<Empresa[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarEmpresas();
  }, [pagina, busca]);

  const carregarEmpresas = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Empresa>>('/empresa', {
        params: { pagina, itensPorPagina: 20, busca }
      });
      setEmpresas(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar empresas:', error);
    } finally {
      setCarregando(false);
    }
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Empresas</h1>
        <button onClick={() => navigate('/empresas/novo')} className="btn-primary">
          Nova Empresa
        </button>
      </div>
      
      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar empresas..."
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
                <th>CNPJ</th>
                <th>Nome/Razão Social</th>
                <th>Apelido/Nome Fantasia</th>
                <th>Usuários</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {empresas.map((empresa) => (
                <tr key={empresa.cnpj}>
                  <td>{empresa.cnpj}</td>
                  <td>{empresa.razaoSocial}</td>
                  <td>{empresa.nomeFantasia || '-'}</td>
                  <td>{empresa.totalUsuarios}/{empresa.quantidadeUsuariosContratados}</td>
                  <td>
                    <button onClick={() => navigate(`/empresas/${empresa.cnpj}`)}>
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
