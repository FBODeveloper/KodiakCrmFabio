import { useState, useEffect } from 'react';
import { useNavigate, useParams, useLocation, useSearchParams } from 'react-router-dom';
import api from '../api/axios';
import { useAuth } from '../contexts/AuthContext';
import type { LeadCreate, LeadEstagio, UsuarioGestao, PaginatedResponse } from '../types';

export default function LeadForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const [searchParams] = useSearchParams();
  const readonly = searchParams.get('readonly') === 'true';
  const origem = (location.state as any)?.from || '/leads';
  const { usuario, isGerente } = useAuth();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [estagios, setEstagios] = useState<LeadEstagio[]>([]);
  const [usuarios, setUsuarios] = useState<UsuarioGestao[]>([]);
  const isEdicao = !!id;
  const [form, setForm] = useState<LeadCreate>({
    nome: '',
    empresa: '',
    email: '',
    telefone: '',
    source: '',
    temperatura: 'frio',
    responsavelId: !isEdicao ? usuario?.id : undefined,
    observacao: ''
  });

  const [showConverter, setShowConverter] = useState(false);
  const [convValor, setConvValor] = useState('');
  const [convPrevisao, setConvPrevisao] = useState('');
  const [convObservacao, setConvObservacao] = useState('');
  const [convertendo, setConvertendo] = useState(false);
  const [leadStatus, setLeadStatus] = useState('em_andamento');

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
      setLeadStatus(data.status);
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

  const handleConverter = async () => {
    if (!id) return;
    setConvertendo(true);
    setErro('');

    try {
      const payload: any = {};
      if (convValor) payload.valor = parseFloat(convValor);
      if (convPrevisao) payload.dataPrevisao = convPrevisao;
      if (convObservacao) payload.observacao = convObservacao;

      await api.post(`/lead/${id}/converter`, payload);
      navigate('/oportunidades');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao converter lead');
    } finally {
      setConvertendo(false);
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

  const podeConverter = isEdicao && leadStatus !== 'convertido';

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{readonly ? 'Lead' : isEdicao ? 'Editar Lead' : 'Novo Lead'}</h1>
        <div className="header-actions">
          {podeConverter && (
            <button onClick={() => setShowConverter(true)} className="btn-success">
              Converter em Oportunidade
            </button>
          )}
          <button onClick={() => navigate(origem)} className="btn-secondary">
            Voltar
          </button>
        </div>
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
              disabled={readonly}
            />
          </div>

          <div className="form-group">
            <label>Empresa</label>
            <input
              type="text"
              name="empresa"
              value={form.empresa}
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
              disabled={readonly}
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
              disabled={readonly}
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
                disabled={readonly}
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
              disabled={readonly || (!isEdicao && !isGerente)}
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
              disabled={readonly}
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

      {showConverter && (
        <div className="modal-overlay" onClick={() => setShowConverter(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>Converter Lead em Oportunidade</h2>
            <p className="modal-subtitle">
              Lead: <strong>{form.nome}</strong>
              {form.empresa && <> — {form.empresa}</>}
            </p>

            <div className="form">
              <div className="form-row">
                <div className="form-group">
                  <label>Valor (R$)</label>
                  <input
                    type="number"
                    step="0.01"
                    value={convValor}
                    onChange={e => setConvValor(e.target.value)}
                    placeholder="Opcional"
                  />
                </div>

                <div className="form-group">
                  <label>Data Previsão</label>
                  <input
                    type="date"
                    value={convPrevisao}
                    onChange={e => setConvPrevisao(e.target.value)}
                  />
                </div>
              </div>

              <div className="form-group">
                <label>Observação da Oportunidade</label>
                <textarea
                  value={convObservacao}
                  onChange={e => setConvObservacao(e.target.value)}
                  rows={3}
                  placeholder="Observações sobre a oportunidade..."
                />
              </div>
            </div>

            <div className="modal-actions">
              <button
                className="btn-secondary"
                onClick={() => setShowConverter(false)}
                disabled={convertendo}
              >
                Cancelar
              </button>
              <button
                className="btn-success"
                onClick={handleConverter}
                disabled={convertendo}
              >
                {convertendo ? 'Convertendo...' : 'Confirmar Conversão'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
