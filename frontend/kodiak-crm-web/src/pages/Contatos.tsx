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
                <th>Data Cadastro</th>
                <th style={{ width: 100 }}>Ações</th>
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
                  <td>{contato.dataCadastro ? new Date(contato.dataCadastro).toLocaleDateString('pt-BR') : '-'}</td>
                  <td>
                    <div style={{ display: 'flex', gap: '2px', alignItems: 'center' }}>
                      <button className="icon-btn" title="Ver" onClick={() => navigate(`/contatos/${contato.id}?readonly=true`)}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"/><circle cx="12" cy="12" r="3"/></svg>
                      </button>
                      <button className="icon-btn" title="Editar" onClick={() => navigate(`/contatos/${contato.id}`)}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"/><path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"/></svg>
                      </button>
                      <button className="icon-btn icon-btn-danger" title="Excluir" onClick={() => { if (confirm('Deseja excluir este contato?')) { api.delete(`/contato/${contato.id}`).then(() => carregarContatos()); } }}>
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="3 6 5 6 21 6"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/></svg>
                      </button>
                    </div>
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
