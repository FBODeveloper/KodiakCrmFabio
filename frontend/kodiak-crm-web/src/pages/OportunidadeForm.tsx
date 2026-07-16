import { useState, useEffect } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import api from '../api/axios';
import type { Funil, FunilEstagio } from '../types';

export default function OportunidadeForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const readonly = searchParams.get('readonly') === 'true';
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [funis, setFunis] = useState<Funil[]>([]);
  const [estagios, setEstagios] = useState<FunilEstagio[]>([]);
  const [form, setForm] = useState({
    titulo: '',
    idParceiro: '',
    idEstagio: '',
    funilId: '',
    valor: '',
    dataPrevisao: '',
    observacao: '',
    motivoPerda: ''
  });
  const [showPerder, setShowPerder] = useState(false);
  const [motivoPerdaInput, setMotivoPerdaInput] = useState('');
  const [perdendo, setPerdendo] = useState(false);

  const isEdicao = !!id;

  useEffect(() => {
    carregarFunis();
    if (id) {
      carregarOportunidade(parseInt(id));
    }
  }, [id]);

  const carregarFunis = async () => {
    try {
      const response = await api.get('/funil');
      setFunis(response.data);
    } catch (error) {
      console.error('Erro ao carregar funis:', error);
    }
  };

  const carregarEstagios = async (funilId: number) => {
    try {
      const response = await api.get(`/funil/${funilId}`);
      setEstagios(response.data.estagios || []);
    } catch (error) {
      console.error('Erro ao carregar estágios:', error);
    }
  };

  const carregarOportunidade = async (oportunidadeId: number) => {
    try {
      const response = await api.get(`/oportunidade/${oportunidadeId}`);
      const data = response.data;
      setForm({
        titulo: data.titulo,
        idParceiro: data.idParceiro?.toString() || '',
        idEstagio: data.idEstagio?.toString() || '',
        funilId: data.funilId?.toString() || '',
        valor: data.valor?.toString() || '',
        dataPrevisao: data.dataPrevisao?.split('T')[0] || '',
        observacao: data.observacao || '',
        motivoPerda: data.motivoPerda || ''
      });
      if (data.funilId) {
        carregarEstagios(data.funilId);
      }
    } catch (error) {
      console.error('Erro ao carregar oportunidade:', error);
    }
  };

  const handleFunilChange = (funilId: string) => {
    setForm({ ...form, funilId, idEstagio: '' });
    if (funilId) {
      carregarEstagios(parseInt(funilId));
    } else {
      setEstagios([]);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      const payload = {
        titulo: form.titulo,
        idParceiro: form.idParceiro ? parseInt(form.idParceiro) : null,
        idEstagio: form.idEstagio ? parseInt(form.idEstagio) : null,
        valor: form.valor ? parseFloat(form.valor) : null,
        dataPrevisao: form.dataPrevisao || null,
        observacao: form.observacao || null,
        motivoPerda: form.motivoPerda || null
      };

      if (isEdicao) {
        await api.put(`/oportunidade/${id}`, payload);
      } else {
        await api.post('/oportunidade', payload);
      }
      navigate('/oportunidades');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar oportunidade');
    } finally {
      setCarregando(false);
    }
  };

  const handleMarcarPerdida = async () => {
    if (!id) return;
    setPerdendo(true);
    setErro('');

    try {
      await api.put(`/oportunidade/${id}`, {
        titulo: form.titulo,
        idParceiro: form.idParceiro ? parseInt(form.idParceiro) : null,
        idEstagio: form.idEstagio ? parseInt(form.idEstagio) : null,
        valor: form.valor ? parseFloat(form.valor) : null,
        dataPrevisao: form.dataPrevisao || null,
        observacao: form.observacao || null,
        motivoPerda: motivoPerdaInput || null
      });
      navigate('/oportunidades');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao marcar como perdida');
    } finally {
      setPerdendo(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{readonly ? 'Oportunidade' : isEdicao ? 'Editar Oportunidade' : 'Nova Oportunidade'}</h1>
        <div className="header-actions">
          {isEdicao && !readonly && !form.motivoPerda && (
            <button onClick={() => setShowPerder(true)} className="btn-danger">
              Marcar como Perdida
            </button>
          )}
          <button onClick={() => navigate('/oportunidades')} className="btn-secondary">
            Voltar
          </button>
        </div>
      </div>
      
      {form.motivoPerda && (
        <div className="alert alert-danger">
          <strong>Perdida:</strong> {form.motivoPerda}
        </div>
      )}
      
      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label>Título *</label>
          <input
            type="text"
            name="titulo"
            value={form.titulo}
            onChange={handleChange}
            required
            disabled={readonly}
          />
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Funil</label>
            <select name="funilId" value={form.funilId} onChange={(e) => handleFunilChange(e.target.value)} disabled={readonly}>
              <option value="">Selecione o funil</option>
              {funis.map((funil) => (
                <option key={funil.id} value={funil.id}>{funil.nome}</option>
              ))}
            </select>
          </div>
          
          <div className="form-group">
            <label>Estágio</label>
            <select name="idEstagio" value={form.idEstagio} onChange={handleChange} disabled={readonly}>
              <option value="">Selecione o estágio</option>
              {estagios.map((estagio) => (
                <option key={estagio.id} value={estagio.id}>{estagio.nome}</option>
              ))}
            </select>
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Valor (R$)</label>
            <input
              type="number"
              name="valor"
              value={form.valor}
              onChange={handleChange}
              step="0.01"
              min="0"
              disabled={readonly}
            />
          </div>
          
          <div className="form-group">
            <label>Data de Previsão</label>
            <input
              type="date"
              name="dataPrevisao"
              value={form.dataPrevisao}
              onChange={handleChange}
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

      {showPerder && (
        <div className="modal-overlay" onClick={() => setShowPerder(false)}>
          <div className="modal" onClick={e => e.stopPropagation()}>
            <h2>Marcar Oportunidade como Perdida</h2>
            <p className="modal-subtitle">
              Tem certeza que deseja marcar esta oportunidade como perdida?
            </p>

            <div className="form">
              <div className="form-group">
                <label>Motivo da Perda</label>
                <textarea
                  value={motivoPerdaInput}
                  onChange={e => setMotivoPerdaInput(e.target.value)}
                  rows={3}
                  placeholder="Descreva o motivo da perda..."
                />
              </div>
            </div>

            <div className="modal-actions">
              <button
                className="btn-secondary"
                onClick={() => setShowPerder(false)}
                disabled={perdendo}
              >
                Cancelar
              </button>
              <button
                className="btn-danger"
                onClick={handleMarcarPerdida}
                disabled={perdendo}
              >
                {perdendo ? 'Salvando...' : 'Confirmar Perda'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
