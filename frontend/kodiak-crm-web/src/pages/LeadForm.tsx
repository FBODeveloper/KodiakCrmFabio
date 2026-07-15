import { useState, useEffect } from 'react';
import { useNavigate, useParams, useLocation } from 'react-router-dom';
import api from '../api/axios';
import type { LeadCreate, LeadEstagio, UsuarioGestao, PaginatedResponse } from '../types';

export default function LeadForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const origem = (location.state as any)?.from || '/leads';
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [estagios, setEstagios] = useState<LeadEstagio[]>([]);
  const [usuarios, setUsuarios] = useState<UsuarioGestao[]>([]);
  const [form, setForm] = useState<LeadCreate>({
    nome: '',
    empresa: '',
    email: '',
    telefone: '',
    source: '',
    temperatura: 'frio',
    responsavelId: undefined,
    observacao: ''
  });

  const isEdicao = !!id;

  useEffect(() => {
    carregarDados();
    if (id) {
      carregarLead(parseInt(id));
    }
  }, [id]);

  const carregarDados = async () => {
    try {
      const [estagiosRes, usuariosRes] = await Promise.all([
        api.get<LeadEstagio[]>('/lead/estagios'),
        api.get<PaginatedResponse<UsuarioGestao>>('/usuariogestao', { params: { itensPorPagina: 100 } })
      ]);
      setEstagios(estagiosRes.data);
      setUsuarios(usuariosRes.data.itens);
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
    }
  };

  const carregarLead = async (leadId: number) => {
    try {
      const response = await api.get(`/lead/${leadId}`);
      const data = response.data;
      setForm({
        nome: data.nome,
        empresa: data.empresa || '',
        email: data.email || '',
        telefone: data.telefone || '',
        source: data.source || '',
        temperatura: data.temperatura || 'frio',
        idEstagio: data.idEstagio || undefined,
        responsavelId: data.responsavelId || undefined,
        observacao: data.observacao || ''
      });
    } catch (error) {
      console.error('Erro ao carregar lead:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      if (isEdicao) {
        await api.put(`/lead/${id}`, form);
      } else {
        await api.post('/lead', form);
      }
      navigate(origem);
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar lead');
    } finally {
      setCarregando(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSelectChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const { name, value } = e.target;
    setForm({
      ...form,
      [name]: value === '' ? undefined : parseInt(value)
    });
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{isEdicao ? 'Editar Lead' : 'Novo Lead'}</h1>
        <button onClick={() => navigate(origem)} className="btn-secondary">
          Voltar
        </button>
      </div>

      <form onSubmit={handleSubmit} className="form">
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
            <label>Empresa</label>
            <input
              type="text"
              name="empresa"
              value={form.empresa}
              onChange={handleChange}
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
            />
          </div>

          <div className="form-group">
            <label>Telefone</label>
            <input
              type="text"
              name="telefone"
              value={form.telefone}
              onChange={handleChange}
            />
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Temperatura</label>
            <select
              name="temperatura"
              value={form.temperatura}
              onChange={handleChange}
            >
              <option value="frio">Frio</option>
              <option value="morno">Morno</option>
              <option value="quente">Quente</option>
            </select>
          </div>

          {estagios.length > 0 && (
            <div className="form-group">
              <label>Estágio</label>
              <select
                name="idEstagio"
                value={form.idEstagio || ''}
                onChange={handleSelectChange}
              >
                <option value="">Selecione...</option>
                {estagios.map(estagio => (
                  <option key={estagio.id} value={estagio.id}>
                    {estagio.nome}
                  </option>
                ))}
              </select>
            </div>
          )}
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Responsável</label>
            <select
              name="responsavelId"
              value={form.responsavelId || ''}
              onChange={handleSelectChange}
            >
              <option value="">Sem responsável</option>
              {usuarios.map(usuario => (
                <option key={usuario.id} value={usuario.id}>
                  {usuario.nome}
                </option>
              ))}
            </select>
          </div>

          <div className="form-group">
            <label>Origem</label>
            <input
              type="text"
              name="source"
              value={form.source}
              onChange={handleChange}
              placeholder="Ex: Site, Indicação, Feira"
            />
          </div>
        </div>

        <div className="form-group">
          <label>Observação</label>
          <textarea
            name="observacao"
            value={form.observacao}
            onChange={handleChange}
            rows={4}
          />
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
