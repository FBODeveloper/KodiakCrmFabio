import { useState, useEffect } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import api from '../api/axios';
import { useAuth } from '../contexts/AuthContext';

interface ClienteOption {
  id: number;
  razaoSocial: string;
  nomeFantasia?: string;
}

interface UsuarioOption {
  id: number;
  nome: string;
}

export default function AtividadeForm() {
  const { id } = useParams();
  const [searchParams] = useSearchParams();
  const readonly = searchParams.get('readonly') === 'true';
  const navigate = useNavigate();
  const { usuario, isGerente } = useAuth();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [clientes, setClientes] = useState<ClienteOption[]>([]);
  const [usuarios, setUsuarios] = useState<UsuarioOption[]>([]);

  const formatarDateTimeLocal = (date: Date) => {
    const pad = (n: number) => n.toString().padStart(2, '0');
    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}`;
  };

  const isEdicao = !!id;
  const [form, setForm] = useState(() => {
    const now = formatarDateTimeLocal(new Date());
    return {
      tipo: 'ligacao',
      titulo: '',
      descricao: '',
      idParceiro: '',
      clienteId: '',
      responsavelId: '',
      status: 'pendente',
      dataInicio: now,
      dataFim: ''
    };
  });
  const [statusOriginal, setStatusOriginal] = useState('pendente');

  useEffect(() => {
    carregarClientes();
    carregarUsuarios();
    if (id) {
      carregarAtividade(parseInt(id));
    }
  }, [id]);

  const carregarClientes = async () => {
    try {
      const response = await api.get('/cliente', { params: { itensPorPagina: 9999 } });
      setClientes(response.data.itens || response.data || []);
    } catch (error) {
      console.error('Erro ao carregar clientes:', error);
    }
  };

  const carregarUsuarios = async () => {
    try {
      const response = await api.get('/usuariogestao', { params: { itensPorPagina: 9999 } });
      setUsuarios(response.data.itens || response.data || []);
    } catch (error) {
      console.error('Erro ao carregar usuários:', error);
    }
  };

  const carregarAtividade = async (atividadeId: number) => {
    try {
      const response = await api.get(`/atividade/${atividadeId}`);
      const data = response.data;
      const status = data.status || 'pendente';
      setForm({
        tipo: data.tipo,
        titulo: data.titulo,
        descricao: data.descricao || '',
        idParceiro: data.idParceiro?.toString() || '',
        clienteId: data.clienteId?.toString() || '',
        responsavelId: data.responsavelId?.toString() || '',
        status,
        dataInicio: data.dataInicio?.slice(0, 16) || '',
        dataFim: data.dataFim?.slice(0, 16) || ''
      });
      setStatusOriginal(status);
    } catch (error) {
      console.error('Erro ao carregar atividade:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      const payload = {
        tipo: form.tipo,
        titulo: form.titulo,
        descricao: form.descricao || null,
        idParceiro: form.idParceiro ? parseInt(form.idParceiro) : null,
        clienteId: form.clienteId ? parseInt(form.clienteId) : null,
        responsavelId: form.responsavelId ? parseInt(form.responsavelId) : null,
        status: form.status,
        dataInicio: form.dataInicio || null,
        dataFim: form.dataFim || null
      };

      if (isEdicao) {
        await api.put(`/atividade/${id}`, payload);
      } else {
        await api.post('/atividade', payload);
      }
      navigate('/atividades');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar atividade');
    } finally {
      setCarregando(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const isFinalizada = statusOriginal === 'concluido' || statusOriginal === 'cancelado';
  const podeDefinirResponsavel = isGerente;
  const responsavelDefault = !podeDefinirResponsavel && usuario ? usuario.id.toString() : form.responsavelId;

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{readonly ? 'Ver Atividade' : isEdicao ? 'Editar Atividade' : 'Nova Atividade'}</h1>
        <button onClick={() => navigate('/atividades')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <form onSubmit={handleSubmit} className="form">
        {isFinalizada && (
          <div className="alert alert-warning" style={{ marginBottom: 16, padding: '8px 12px', background: 'rgba(245, 158, 11, 0.1)', border: '1px solid rgba(245, 158, 11, 0.3)', borderRadius: '6px', color: '#f59e0b' }}>
            Esta atividade está {statusOriginal === 'concluido' ? 'concluída' : 'cancelada'} e não pode ser alterada.
          </div>
        )}
        <div className="form-row">
          <div className="form-group">
            <label>Tipo *</label>
            <select name="tipo" value={form.tipo} onChange={handleChange} disabled={readonly || isFinalizada}>
              <option value="ligacao">Ligação</option>
              <option value="reuniao">Reunião</option>
              <option value="visita">Visita</option>
              <option value="tarefa">Tarefa</option>
              <option value="email">E-mail</option>
              <option value="whatsapp">WhatsApp</option>
            </select>
          </div>
          
          <div className="form-group">
            <label>Título *</label>
            <input
              type="text"
              name="titulo"
              value={form.titulo}
              onChange={handleChange}
              required
              disabled={readonly || isFinalizada}
            />
          </div>
        </div>
        
        <div className="form-group">
          <label>Descrição</label>
          <textarea
            name="descricao"
            value={form.descricao}
            onChange={handleChange}
            rows={4}
            disabled={readonly || isFinalizada}
          />
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Cliente</label>
            <select name="clienteId" value={form.clienteId} onChange={handleChange} disabled={readonly || isFinalizada}>
              <option value="">Nenhum</option>
              {clientes.map(c => (
                <option key={c.id} value={c.id}>{c.nomeFantasia || c.razaoSocial}</option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Responsável</label>
            <select
              name="responsavelId"
              value={responsavelDefault}
              onChange={handleChange}
              disabled={readonly || isFinalizada || !podeDefinirResponsavel}
            >
              <option value="">Nenhum</option>
              {usuarios.map(u => (
                <option key={u.id} value={u.id}>{u.nome}</option>
              ))}
            </select>
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Data/Hora Início</label>
            <input
              type="datetime-local"
              name="dataInicio"
              value={form.dataInicio}
              onChange={handleChange}
              disabled={readonly || isFinalizada}
            />
          </div>
          
          <div className="form-group">
            <label>Data/Hora Fim</label>
            <input
              type="datetime-local"
              name="dataFim"
              value={form.dataFim}
              onChange={handleChange}
              disabled={readonly || isFinalizada}
            />
          </div>
        </div>
        
        {erro && <div className="erro">{erro}</div>}
        
        {!readonly && !isFinalizada && (
          <div className="form-actions">
            <button type="submit" className="btn-primary" disabled={carregando}>
              {carregando ? 'Salvando...' : 'Salvar'}
            </button>
          </div>
        )}
      </form>
    </div>
  );
}
