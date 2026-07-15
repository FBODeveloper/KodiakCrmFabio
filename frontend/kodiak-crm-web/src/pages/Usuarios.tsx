import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import { useAuth } from '../contexts/AuthContext';
import type { UsuarioGestao, PaginatedResponse } from '../types';

export default function Usuarios() {
  const [usuarios, setUsuarios] = useState<UsuarioGestao[]>([]);
  const [total, setTotal] = useState(0);
  const [pagina, setPagina] = useState(1);
  const [busca, setBusca] = useState('');
  const [carregando, setCarregando] = useState(true);
  const navigate = useNavigate();
  const { isAdmin, isGerente } = useAuth();

  useEffect(() => {
    carregarUsuarios();
  }, [pagina, busca]);

  const carregarUsuarios = async () => {
    setCarregando(true);
    try {
      const response = await api.get<PaginatedResponse<UsuarioGestao>>('/usuariogestao', {
        params: { pagina, itensPorPagina: 20, busca }
      });
      setUsuarios(response.data.itens);
      setTotal(response.data.total);
    } catch (error) {
      console.error('Erro ao carregar usuários:', error);
    } finally {
      setCarregando(false);
    }
  };

  const excluirUsuario = async (id: number) => {
    if (!confirm('Deseja excluir este usuário?')) return;
    
    try {
      await api.delete(`/usuariogestao/${id}`);
      carregarUsuarios();
    } catch (error) {
      console.error('Erro ao excluir usuário:', error);
    }
  };

  const perfilColors: Record<string, string> = {
    admin: '#ef4444',
    gerente: '#f59e0b',
    usuario: '#3b82f6'
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Usuários</h1>
        {(isAdmin || isGerente) && (
          <button onClick={() => navigate('/usuarios/novo')} className="btn-primary">
            Novo Usuário
          </button>
        )}
      </div>
      
      <div className="filtros">
        <input
          type="text"
          placeholder="Buscar usuários..."
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
                <th>Email</th>
                <th>Perfil</th>
                <th>Ações</th>
              </tr>
            </thead>
            <tbody>
              {usuarios.map((usuario) => (
                <tr key={usuario.id}>
                  <td>{usuario.nome}</td>
                  <td>{usuario.email}</td>
                  <td>
                    <span 
                      className="status-badge"
                      style={{ backgroundColor: perfilColors[usuario.perfil] || '#6b7280' }}
                    >
                      {usuario.perfil}
                    </span>
                  </td>
                  <td>
                    <button onClick={() => navigate(`/usuarios/${usuario.id}`)}>
                      Editar
                    </button>
                    <button onClick={() => excluirUsuario(usuario.id)} className="btn-danger">
                      Excluir
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
