import { useState, useEffect } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import api from '../api/axios';

export default function ContatoForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const readonly = searchParams.get('readonly') === 'true';
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [clientes, setClientes] = useState<{ id: number; razaoSocial: string }[]>([]);
  const [form, setForm] = useState({
    nome: '',
    cargo: '',
    email: '',
    telefone: '',
    celular: '',
    idCliente: '',
    idParceiro: '',
    observacao: ''
  });

  const isEdicao = !!id;

  useEffect(() => {
    carregarClientes();
    if (id) {
      carregarContato(parseInt(id));
    } else {
      const clienteId = searchParams.get('cliente');
      if (clienteId) {
        setForm(prev => ({ ...prev, idCliente: clienteId }));
      }
    }
  }, [id]);

  const carregarClientes = async () => {
    try {
      const response = await api.get('/cliente', { params: { itensPorPagina: 100 } });
      setClientes(response.data.itens);
    } catch (error) {
      console.error('Erro ao carregar clientes:', error);
    }
  };

  const carregarContato = async (contatoId: number) => {
    try {
      const response = await api.get(`/contato/${contatoId}`);
      const data = response.data;
      setForm({
        nome: data.nome,
        cargo: data.cargo || '',
        email: data.email || '',
        telefone: data.telefone || '',
        celular: data.celular || '',
        idCliente: data.idCliente?.toString() || '',
        idParceiro: data.idParceiro?.toString() || '',
        observacao: data.observacao || ''
      });
    } catch (error) {
      console.error('Erro ao carregar contato:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      const payload = {
        nome: form.nome,
        cargo: form.cargo || null,
        email: form.email || null,
        telefone: form.telefone || null,
        celular: form.celular || null,
        idCliente: form.idCliente ? parseInt(form.idCliente) : null,
        idParceiro: form.idParceiro ? parseInt(form.idParceiro) : null,
        observacao: form.observacao || null
      };

      if (isEdicao) {
        await api.put(`/contato/${id}`, payload);
      } else {
        await api.post('/contato', payload);
      }
      navigate('/contatos');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar contato');
    } finally {
      setCarregando(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{isEdicao ? 'Editar Contato' : 'Novo Contato'}</h1>
        <button onClick={() => navigate('/contatos')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <form onSubmit={handleSubmit} className="form">
        {!isEdicao && (
          <div className="form-row">
            <div className="form-group">
              <label>Data Cadastro</label>
              <input
                type="text"
                value={new Date().toLocaleDateString('pt-BR')}
                disabled
              />
            </div>
            <div></div>
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
              disabled={readonly}
            />
          </div>
          
          <div className="form-group">
            <label>Cargo</label>
            <input
              type="text"
              name="cargo"
              value={form.cargo}
              onChange={handleChange}
              disabled={readonly}
            />
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              name="email"
              value={form.email}
              onChange={handleChange}
              disabled={readonly}
            />
          </div>
          
          <div className="form-group">
            <label>Telefone</label>
            <input
              type="text"
              name="telefone"
              value={form.telefone}
              onChange={handleChange}
              placeholder="(00) 0000-0000"
              disabled={readonly}
            />
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Celular</label>
            <input
              type="text"
              name="celular"
              value={form.celular}
              onChange={handleChange}
              placeholder="(00) 00000-0000"
              disabled={readonly}
            />
          </div>
          
          <div className="form-group">
            <label>Cliente</label>
            <select name="idCliente" value={form.idCliente} onChange={handleChange} disabled={readonly}>
              <option value="">Nenhum cliente</option>
              {clientes.map(cliente => (
                <option key={cliente.id} value={cliente.id}>{cliente.razaoSocial}</option>
              ))}
            </select>
          </div>
        </div>
        
        <div className="form-group">
          <label>Observação</label>
          <textarea
            name="observacao"
            value={form.observacao}
            onChange={handleChange}
            rows={3}
            disabled={readonly}
          />
        </div>
        
        {erro && <div className="erro">{erro}</div>}
        
        {!readonly && (
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
