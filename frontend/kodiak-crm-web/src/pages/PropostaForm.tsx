import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../api/axios';

interface PropostaItemForm {
  descricao: string;
  quantidade: number;
  valorUnitario: number;
}

export default function PropostaForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [form, setForm] = useState({
    titulo: '',
    idParceiro: '',
    idOportunidade: '',
    dataValidade: '',
    observacao: ''
  });
  const [itens, setItens] = useState<PropostaItemForm[]>([
    { descricao: '', quantidade: 1, valorUnitario: 0 }
  ]);

  const isEdicao = !!id;

  useEffect(() => {
    if (id) {
      carregarProposta(parseInt(id));
    }
  }, [id]);

  const carregarProposta = async (propostaId: number) => {
    try {
      const response = await api.get(`/proposta/${propostaId}`);
      const data = response.data;
      setForm({
        titulo: data.titulo,
        idParceiro: data.idParceiro?.toString() || '',
        idOportunidade: data.idOportunidade?.toString() || '',
        dataValidade: data.dataValidade?.split('T')[0] || '',
        observacao: data.observacao || ''
      });
      if (data.itens && data.itens.length > 0) {
        setItens(data.itens.map((item: any) => ({
          descricao: item.descricao,
          quantidade: item.quantidade,
          valorUnitario: item.valorUnitario
        })));
      }
    } catch (error) {
      console.error('Erro ao carregar proposta:', error);
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
        idOportunidade: form.idOportunidade ? parseInt(form.idOportunidade) : null,
        dataValidade: form.dataValidade || null,
        observacao: form.observacao || null,
        itens: itens.filter(item => item.descricao.trim() !== '').map(item => ({
          descricao: item.descricao,
          quantidade: item.quantidade,
          valorUnitario: item.valorUnitario
        }))
      };

      if (isEdicao) {
        await api.put(`/proposta/${id}`, payload);
      } else {
        await api.post('/proposta', payload);
      }
      navigate('/propostas');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar proposta');
    } finally {
      setCarregando(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleItemChange = (index: number, field: keyof PropostaItemForm, value: string | number) => {
    const novosItens = [...itens];
    novosItens[index] = { ...novosItens[index], [field]: value };
    setItens(novosItens);
  };

  const addItem = () => {
    setItens([...itens, { descricao: '', quantidade: 1, valorUnitario: 0 }]);
  };

  const removeItem = (index: number) => {
    if (itens.length > 1) {
      setItens(itens.filter((_, i) => i !== index));
    }
  };

  const valorTotal = itens.reduce((acc, item) => acc + (item.quantidade * item.valorUnitario), 0);

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{isEdicao ? 'Editar Proposta' : 'Nova Proposta'}</h1>
        <button onClick={() => navigate('/propostas')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <form onSubmit={handleSubmit} className="form">
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
        
        <div className="form-row">
          <div className="form-group">
            <label>Data de Validade</label>
            <input
              type="date"
              name="dataValidade"
              value={form.dataValidade}
              onChange={handleChange}
            />
          </div>
        </div>
        
        <div className="form-group">
          <label>Observação</label>
          <textarea
            name="observacao"
            value={form.observacao}
            onChange={handleChange}
            rows={3}
          />
        </div>
        
        <div className="itens-section">
          <h3>Itens da Proposta</h3>
          
          {itens.map((item, index) => (
            <div key={index} className="item-row">
              <div className="form-group item-descricao">
                <label>Descrição</label>
                <input
                  type="text"
                  value={item.descricao}
                  onChange={(e) => handleItemChange(index, 'descricao', e.target.value)}
                  placeholder="Descrição do item"
                />
              </div>
              
              <div className="form-group item-quantidade">
                <label>Qtd</label>
                <input
                  type="number"
                  value={item.quantidade}
                  onChange={(e) => handleItemChange(index, 'quantidade', parseInt(e.target.value) || 0)}
                  min="1"
                />
              </div>
              
              <div className="form-group item-valor">
                <label>Valor Unit.</label>
                <input
                  type="number"
                  value={item.valorUnitario}
                  onChange={(e) => handleItemChange(index, 'valorUnitario', parseFloat(e.target.value) || 0)}
                  step="0.01"
                  min="0"
                />
              </div>
              
              <div className="form-group item-total">
                <label>Total</label>
                <p className="item-total-valor">
                  {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
                    .format(item.quantidade * item.valorUnitario)}
                </p>
              </div>
              
              {itens.length > 1 && (
                <button type="button" onClick={() => removeItem(index)} className="btn-danger btn-small">
                  Remover
                </button>
              )}
            </div>
          ))}
          
          <button type="button" onClick={addItem} className="btn-secondary">
            + Adicionar Item
          </button>
          
          <div className="proposta-total">
            <strong>Valor Total: </strong>
            {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(valorTotal)}
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
