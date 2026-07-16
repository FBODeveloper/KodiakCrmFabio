import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../api/axios';

export default function AtividadeForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
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
      dataInicio: now,
      dataFim: ''
    };
  });

  useEffect(() => {
    if (id) {
      carregarAtividade(parseInt(id));
    }
  }, [id]);

  const carregarAtividade = async (atividadeId: number) => {
    try {
      const response = await api.get(`/atividade/${atividadeId}`);
      const data = response.data;
      setForm({
        tipo: data.tipo,
        titulo: data.titulo,
        descricao: data.descricao || '',
        idParceiro: data.idParceiro?.toString() || '',
        dataInicio: data.dataInicio?.slice(0, 16) || '',
        dataFim: data.dataFim?.slice(0, 16) || ''
      });
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

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{isEdicao ? 'Editar Atividade' : 'Nova Atividade'}</h1>
        <button onClick={() => navigate('/atividades')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <form onSubmit={handleSubmit} className="form">
        <div className="form-row">
          <div className="form-group">
            <label>Tipo *</label>
            <select name="tipo" value={form.tipo} onChange={handleChange}>
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
          />
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Data/Hora Início</label>
            <input
              type="datetime-local"
              name="dataInicio"
              value={form.dataInicio}
              onChange={handleChange}
            />
          </div>
          
          <div className="form-group">
            <label>Data/Hora Fim</label>
            <input
              type="datetime-local"
              name="dataFim"
              value={form.dataFim}
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
