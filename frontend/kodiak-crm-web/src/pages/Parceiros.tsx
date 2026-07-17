import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Parceiro, PaginatedResponse } from '../types';

export default function Parceiros() {
  const [parceiros, setParceiros] = useState<Parceiro[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    carregarParceiros();
  }, [pagina, busca]);

  const carregarParceiros = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<Parceiro>>('/parceiros', {
        params: { pagina, itensPorPagina: 20, busca }
      });
      setParceiros(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar parceiros:', error);
    } finally {
      setCarregando(false);
    }
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Parceiros</h1>
        <button onClick={() => navigate('/parceiros/novo')} className="btn-primary">
          Novo Parceiro
        </button>
      </div>
      
      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar parceiros..."
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
                <th>Nome/Razão Social</th>
                <th>Apelido/Nome Fantasia</th>
                <th>CPF/CNPJ</th>
                <th>Email</th>
                <th>Telefone</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {parceiros.map((parceiro) => (
                <tr key={parceiro.id}>
                  <td>{parceiro.razaoSocial}</td>
                  <td>{parceiro.nomeFantasia || '-'}</td>
                  <td>{parceiro.cpfCnpj || '-'}</td>
                  <td>{parceiro.email || '-'}</td>
                  <td>{parceiro.telefone || '-'}</td>
                  <td>
                    <button onClick={() => navigate(`/parceiros/${parceiro.id}`)}>
                      Ver
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
