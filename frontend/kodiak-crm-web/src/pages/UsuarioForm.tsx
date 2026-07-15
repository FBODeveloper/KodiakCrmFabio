import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../api/axios';
import { useAuth } from '../contexts/AuthContext';
import type { UsuarioCreate } from '../types';

export default function UsuarioForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { isAdmin, isGerente } = useAuth();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [empresas, setEmpresas] = useState<{ cnpj: string; razaoSocial: string }[]>([]);
  const [form, setForm] = useState<UsuarioCreate>({
    nome: '',
    email: '',
    senha: '',
    perfil: 'usuario',
    avatar: '',
    dataNascimento: '',
    idEmpresa: ''
  });

  const isEdicao = !!id;

  useEffect(() => {
    if (isAdmin) {
      carregarEmpresas();
    }
  }, [isAdmin]);

  useEffect(() => {
    if (id) {
      carregarUsuario(parseInt(id));
    }
  }, [id]);

  const carregarEmpresas = async () => {
    try {
      const response = await api.get('/empresa', { params: { itensPorPagina: 100 } });
      setEmpresas(response.data.itens);
    } catch (error) {
      console.error('Erro ao carregar empresas:', error);
    }
  };

  const carregarUsuario = async (usuarioId: number) => {
    try {
      const response = await api.get(`/usuariogestao/${usuarioId}`);
      setForm({
        nome: response.data.nome,
        email: response.data.email,
        senha: '',
        perfil: response.data.perfil,
        avatar: response.data.avatar || '',
        dataNascimento: response.data.dataNascimento || '',
        idEmpresa: ''
      });
    } catch (error) {
      console.error('Erro ao carregar usuário:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      if (isEdicao) {
        const updateData: any = {
          nome: form.nome,
          perfil: form.perfil,
          avatar: form.avatar,
          dataNascimento: form.dataNascimento
        };
        if (form.senha) {
          updateData.senha = form.senha;
        }
        await api.put(`/usuariogestao/${id}`, updateData);
      } else {
        const payload: any = { ...form };
        if (!isAdmin || !payload.idEmpresa) {
          delete payload.idEmpresa;
        }
        await api.post('/usuariogestao', payload);
      }
      navigate('/usuarios');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar usuário');
    } finally {
      setCarregando(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{isEdicao ? 'Editar Usuário' : 'Novo Usuário'}</h1>
        <button onClick={() => navigate('/usuarios')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <form onSubmit={handleSubmit} className="form">
        {isAdmin && !isEdicao && (
          <div className="form-group">
            <label>Empresa *</label>
            <select name="idEmpresa" value={form.idEmpresa} onChange={handleChange} required>
              <option value="">Selecione a empresa</option>
              {empresas.map((emp) => (
                <option key={emp.cnpj} value={emp.cnpj}>
                  {emp.razaoSocial} ({emp.cnpj})
                </option>
              ))}
            </select>
          </div>
        )}

        <div className="form-row">
          <div className="form-group">
            <label>Nome *</label>
            <input
              type="text"
              name="nome"
              value={form.nome}
              onChange={handleChange}
              required
            />
          </div>
          
          <div className="form-group">
            <label>Email *</label>
            <input
              type="email"
              name="email"
              value={form.email}
              onChange={handleChange}
              required
            />
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Senha {isEdicao ? '(deixe vazio para manter)' : '*'}</label>
            <input
              type="password"
              name="senha"
              value={form.senha}
              onChange={handleChange}
              required={!isEdicao}
            />
          </div>
          
          <div className="form-group">
            <label>Perfil *</label>
            <select name="perfil" value={form.perfil} onChange={handleChange}>
              <option value="usuario">Usuário</option>
              <option value="gerente">Gerente</option>
              {isAdmin && <option value="admin">Admin</option>}
            </select>
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Avatar (URL)</label>
            <input
              type="text"
              name="avatar"
              value={form.avatar}
              onChange={handleChange}
              placeholder="https://exemplo.com/avatar.jpg"
            />
          </div>
          
          <div className="form-group">
            <label>Data de Nascimento</label>
            <input
              type="date"
              name="dataNascimento"
              value={form.dataNascimento}
              onChange={handleChange}
            />
          </div>
        </div>
        
        {erro && <div className="erro">{erro}</div>}
        
        <div className="form-actions">
          <button type="submit" className="btn-primary" disabled={carregando}>
            {carregando ? 'Salvando...' : 'Salvar'}
          </button>
        </div>
      </form>
    </div>
  );
}
