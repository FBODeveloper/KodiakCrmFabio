import { useState, useEffect } from 'react';
import { useNavigate, useParams, useSearchParams } from 'react-router-dom';
import api from '../api/axios';
import type { FunilEstagio } from '../types';

interface FunilEstagioForm {
  id?: number;
  nome: string;
  ordem: number;
  probabilidade: number;
}

const emptyEstagio: FunilEstagioForm = { nome: '', ordem: 1, probabilidade: 0 };

export default function FunilForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const readonly = searchParams.get('readonly') === 'true';
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [nome, setNome] = useState('');
  const [estagios, setEstagios] = useState<FunilEstagioForm[]>([{ ...emptyEstagio }]);

  const isEdicao = !!id;

  useEffect(() => {
    if (id) {
      carregarFunil(parseInt(id));
    }
  }, [id]);

  const carregarFunil = async (funilId: number) => {
    try {
      const response = await api.get(`/funil/${funilId}`);
      const data = response.data;
      setNome(data.nome || '');
      if (data.estagios && data.estagios.length > 0) {
        setEstagios(data.estagios.map((e: FunilEstagio) => ({
          id: e.id,
          nome: e.nome,
          ordem: e.ordem,
          probabilidade: e.probabilidade,
        })));
      }
    } catch (error) {
      console.error('Erro ao carregar funil:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      const payload = {
        nome,
        estagios: estagios
          .filter(e => e.nome.trim() !== '')
          .map((e, index) => ({
            id: e.id,
            nome: e.nome,
            ordem: index + 1,
            probabilidade: e.probabilidade,
          })),
      };

      if (isEdicao) {
        await api.put(`/funil/${id}`, payload);
      } else {
        await api.post('/funil', payload);
      }
      navigate('/funis');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar funil');
    } finally {
      setCarregando(false);
    }
  };

  const handleAddEstagio = () => {
    setEstagios([...estagios, { ...emptyEstagio, ordem: estagios.length + 1 }]);
  };

  const handleRemoveEstagio = (index: number) => {
    if (estagios.length <= 1) return;
    const novos = estagios.filter((_, i) => i !== index);
    novos.forEach((e, i) => (e.ordem = i + 1));
    setEstagios(novos);
  };

  const handleEstagioChange = (index: number, field: keyof FunilEstagioForm, value: string | number) => {
    const novos = [...estagios];
    novos[index] = { ...novos[index], [field]: value };
    setEstagios(novos);
  };

  const handleMoverEstagio = (index: number, direcao: -1 | 1) => {
    const novoIndex = index + direcao;
    if (novoIndex < 0 || novoIndex >= estagios.length) return;
    const novos = [...estagios];
    [novos[index], novos[novoIndex]] = [novos[novoIndex], novos[index]];
    novos.forEach((e, i) => (e.ordem = i + 1));
    setEstagios(novos);
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{readonly ? 'Funil' : isEdicao ? 'Editar Funil' : 'Novo Funil'}</h1>
        <button onClick={() => navigate('/funis')} className="btn-secondary">
          Voltar
        </button>
      </div>

      <form onSubmit={handleSubmit} className="form">
        <div className="form-group">
          <label>Nome do Funil *</label>
          <input
            type="text"
            value={nome}
            onChange={(e) => setNome(e.target.value)}
            required
            placeholder="Nome do funil"
            disabled={readonly}
          />
        </div>

        <div className="itens-section">
          <h3>Estágios</h3>

          {estagios.map((estagio, index) => (
            <div key={index} className="item-row">
              <div className="form-group" style={{ width: 50, textAlign: 'center' }}>
                <label>Ordem</label>
                <p style={{ marginTop: 8, fontWeight: 600 }}>{index + 1}</p>
              </div>

              <div className="form-group item-descricao">
                <label>Nome *</label>
                <input
                  type="text"
                  value={estagio.nome}
                  onChange={(e) => handleEstagioChange(index, 'nome', e.target.value)}
                  placeholder="Nome do estágio"
                  disabled={readonly}
                />
              </div>

              <div className="form-group item-quantidade">
                <label>Probabilidade (%)</label>
                <input
                  type="number"
                  value={estagio.probabilidade}
                  onChange={(e) => handleEstagioChange(index, 'probabilidade', parseInt(e.target.value) || 0)}
                  min="0"
                  max="100"
                  disabled={readonly}
                />
              </div>

              {!readonly && (
                <div style={{ display: 'flex', gap: 4, alignItems: 'flex-end', paddingBottom: 4 }}>
                  <button
                    type="button"
                    onClick={() => handleMoverEstagio(index, -1)}
                    disabled={index === 0}
                    className="icon-btn"
                    title="Mover para cima"
                    style={{ opacity: index === 0 ? 0.3 : 1 }}
                  >
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="18 15 12 9 6 15"/></svg>
                  </button>
                  <button
                    type="button"
                    onClick={() => handleMoverEstagio(index, 1)}
                    disabled={index === estagios.length - 1}
                    className="icon-btn"
                    title="Mover para baixo"
                    style={{ opacity: index === estagios.length - 1 ? 0.3 : 1 }}
                  >
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><polyline points="6 9 12 15 18 9"/></svg>
                  </button>
                  <button
                    type="button"
                    onClick={() => handleRemoveEstagio(index)}
                    disabled={estagios.length <= 1}
                    className="icon-btn icon-btn-danger"
                    title="Remover estágio"
                    style={{ opacity: estagios.length <= 1 ? 0.3 : 1 }}
                  >
                    <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/></svg>
                  </button>
                </div>
              )}
            </div>
          ))}

          {!readonly && (
            <button type="button" onClick={handleAddEstagio} className="btn-secondary">
              + Adicionar Estágio
            </button>
          )}
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
