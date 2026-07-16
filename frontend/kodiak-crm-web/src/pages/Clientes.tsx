import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { ClienteDTO, PaginatedResponse } from '../types';

export default function Clientes() {
  const [clientes, setClientes] = useState<ClienteDTO[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarClientes();
  }, [pagina, busca]);

  const carregarClientes = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<ClienteDTO>>('/cliente', {
        params: { pagina, itensPorPagina: 20, busca }
      });
      setClientes(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar clientes:', error);
    } finally {
      setCarregando(false);
    }
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
                <th>Ações</th>
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
                    <button onClick={() => navigate(`/clientes/${cliente.id}`)}>
                      Editar
                    </button>
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
