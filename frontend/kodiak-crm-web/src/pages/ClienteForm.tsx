import { useState, useEffect } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import api from '../api/axios';

export default function ClienteForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const readonly = searchParams.get('readonly') === 'true';
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [form, setForm] = useState({
    razaoSocial: '',
    nomeFantasia: '',
    cnpjCpf: '',
    email: '',
    telefone: '',
    celular: '',
    endereco: '',
    observacao: ''
  });

  const isEdicao = !!id;

  useEffect(() => {
    if (id) {
      carregarCliente(parseInt(id));
    }
  }, [id]);

  const carregarCliente = async (clienteId: number) => {
    try {
      const response = await api.get(`/cliente/${clienteId}`);
      const data = response.data;
      setForm({
        razaoSocial: data.razaoSocial,
        nomeFantasia: data.nomeFantasia || '',
        cnpjCpf: data.cnpjCpf || '',
        email: data.email || '',
        telefone: data.telefone || '',
        celular: data.celular || '',
        endereco: data.endereco || '',
        observacao: data.observacao || ''
      });
    } catch (error) {
      console.error('Erro ao carregar cliente:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      if (isEdicao) {
        await api.put(`/cliente/${id}`, form);
      } else {
        await api.post('/cliente', form);
      }
      navigate('/clientes');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar cliente');
    } finally {
      setCarregando(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{isEdicao ? 'Editar Cliente' : 'Novo Cliente'}</h1>
        <button onClick={() => navigate('/clientes')} className="btn-secondary">
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
            <label>Nome/Razão Social *</label>
            <input
              type="text"
              name="razaoSocial"
              value={form.razaoSocial}
              onChange={handleChange}
              required
              disabled={readonly}
            />
          </div>
          
          <div className="form-group">
            <label>Apelido/Nome Fantasia</label>
            <input
              type="text"
              name="nomeFantasia"
              value={form.nomeFantasia}
              onChange={handleChange}
              disabled={readonly}
            />
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>CNPJ/CPF</label>
            <input
              type="text"
              name="cnpjCpf"
              value={form.cnpjCpf}
              onChange={handleChange}
              placeholder="000.000.000-00"
              disabled={readonly}
            />
          </div>
          
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
        </div>
        
        <div className="form-row">
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
        </div>
        
        <div className="form-group">
          <label>Endereço</label>
          <input
            type="text"
            name="endereco"
            value={form.endereco}
            onChange={handleChange}
            disabled={readonly}
          />
        </div>
        
        <div className="form-group">
          <label>Observação</label>
          <textarea
            name="observacao"
            value={form.observacao}
            onChange={handleChange}
            rows={4}
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
