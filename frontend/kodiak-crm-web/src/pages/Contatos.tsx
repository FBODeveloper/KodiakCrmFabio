import { useState, useEffect } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import api from '../api/axios';
import type { ContatoDTO, PaginatedResponse } from '../types';

export default function Contatos() {
  const [contatos, setContatos] = useState<ContatoDTO[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const idCliente = searchParams.get('cliente') || undefined;
  const idParceiro = searchParams.get('parceiro') || undefined;

  useEffect(() => {
    carregarContatos();
  }, [pagina, busca, idCliente, idParceiro]);

  const carregarContatos = async () => {
    setCarregando(true);
    try {
      const params: any = { pagina, itensPorPagina: 20, busca };
      if (idCliente) params.idCliente = idCliente;
      if (idParceiro) params.idParceiro = idParceiro;

      const response = await api.get<PaginatedResponse<ContatoDTO>>('/contato', { params });
      setContatos(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar contatos:', error);
    } finally {
      setCarregando(false);
    }
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Contatos</h1>
        <button onClick={() => navigate('/contatos/novo')} className="btn-primary">
          Novo Contato
        </button>
      </div>
      
      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar contatos..."
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
                <th>Nome</th>
                <th>Cargo</th>
                <th>Email</th>
                <th>Telefone</th>
                <th>Cliente</th>
                <th>Parceiro</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {contatos.map((contato) => (
                <tr key={contato.id}>
                  <td>{contato.nome}</td>
                  <td>{contato.cargo || '-'}</td>
                  <td>{contato.email || '-'}</td>
                  <td>{contato.telefone || contato.celular || '-'}</td>
                  <td>{contato.clienteNome || '-'}</td>
                  <td>{contato.parceiroNome || '-'}</td>
                  <td>
                    <button onClick={() => navigate(`/contatos/${contato.id}`)}>
                      Editar
                    </button>
                  </td>
                </tr>
              ))}
              {contatos.length === 0 && (
                <tr><td colSpan={7} className="vazio">Nenhum contato encontrado</td></tr>
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
