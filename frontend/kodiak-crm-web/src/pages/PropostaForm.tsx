import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../api/axios';
import type { ClienteDTO, ContatoDTO } from '../types';

interface PropostaItemForm {
  descricao: string;
  quantidade: number;
  valorUnitario: number;
}

interface PropostaFormState {
  titulo: string;
  dataProposta: string;
  formaPagamento: string;
  prazoEntrega: string;
  clienteId: string;
  contatoId: string;
  idOportunidade: string;
  dataValidade: string;
  observacao: string;
}

const emptyForm: PropostaFormState = {
  titulo: '',
  dataProposta: '',
  formaPagamento: '',
  prazoEntrega: '',
  clienteId: '',
  contatoId: '',
  idOportunidade: '',
  dataValidade: '',
  observacao: ''
};

const emptyItem: PropostaItemForm = { descricao: '', quantidade: 1, valorUnitario: 0 };

export default function PropostaForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [form, setForm] = useState<PropostaFormState>(emptyForm);
  const [itens, setItens] = useState<PropostaItemForm[]>([{ ...emptyItem }]);
  const [numeroGerado, setNumeroGerado] = useState('');

  const [clientes, setClientes] = useState<ClienteDTO[]>([]);
  const [contatos, setContatos] = useState<ContatoDTO[]>([]);

  const isEdicao = !!id;

  useEffect(() => {
    carregarClientes();
    if (id) {
      carregarProposta(parseInt(id));
    }
  }, [id]);

  useEffect(() => {
    if (form.clienteId) {
      carregarContatos(parseInt(form.clienteId));
    } else {
      setContatos([]);
      if (form.contatoId) {
        setForm(prev => ({ ...prev, contatoId: '' }));
      }
    }
  }, [form.clienteId]);

  const carregarClientes = async () => {
    try {
      const response = await api.get('/cliente', { params: { itensPorPagina: 9999 } });
      setClientes(response.data.itens || []);
    } catch (error) {
      console.error('Erro ao carregar clientes:', error);
    }
  };

  const carregarContatos = async (clienteId: number) => {
    try {
      const response = await api.get(`/contato/cliente/${clienteId}`);
      setContatos(Array.isArray(response.data) ? response.data : []);
    } catch (error) {
      console.error('Erro ao carregar contatos:', error);
      setContatos([]);
    }
  };

  const carregarProposta = async (propostaId: number) => {
    try {
      const response = await api.get(`/proposta/${propostaId}`);
      const data = response.data;
      setNumeroGerado(data.numero || '');
      setForm({
        titulo: data.titulo || '',
        dataProposta: data.dataProposta?.split('T')[0] || '',
        formaPagamento: data.formaPagamento || '',
        prazoEntrega: data.prazoEntrega || '',
        clienteId: data.clienteId?.toString() || '',
        contatoId: data.contatoId?.toString() || '',
        idOportunidade: data.idOportunidade?.toString() || '',
        dataValidade: data.dataValidade?.split('T')[0] || '',
        observacao: data.observacao || ''
      });
      if (data.clienteId) {
        await carregarContatos(data.clienteId);
      }
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

  const resetForm = () => {
    setForm({ ...emptyForm });
    setItens([{ ...emptyItem }]);
    setContatos([]);
    setNumeroGerado('');
    setErro('');
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      const payload = {
        titulo: form.titulo,
        clienteId: form.clienteId ? parseInt(form.clienteId) : null,
        contatoId: form.contatoId ? parseInt(form.contatoId) : null,
        idOportunidade: form.idOportunidade ? parseInt(form.idOportunidade) : null,
        dataProposta: form.dataProposta || null,
        formaPagamento: form.formaPagamento || null,
        prazoEntrega: form.prazoEntrega || null,
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

      if (!isEdicao) {
        resetForm();
        setErro('');
      } else {
        navigate('/propostas');
      }
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
    setItens([...itens, { ...emptyItem }]);
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
        <div className="form-row">
          <div className="form-group">
            <label>Número</label>
            <input
              type="text"
              value={isEdicao ? numeroGerado : '(gerado ao salvar)'}
              readOnly
              style={{ backgroundColor: 'var(--background)', cursor: 'not-allowed' }}
            />
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

        <div className="form-row">
          <div className="form-group">
            <label>Data da Proposta</label>
            <input
              type="date"
              name="dataProposta"
              value={form.dataProposta}
              onChange={handleChange}
            />
          </div>
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

        <div className="form-row">
          <div className="form-group">
            <label>Cliente</label>
            <select
              name="clienteId"
              value={form.clienteId}
              onChange={handleChange}
            >
              <option value="">Selecione o cliente</option>
              {clientes.map(c => (
                <option key={c.id} value={c.id}>
                  {c.razaoSocial}{c.nomeFantasia ? ` (${c.nomeFantasia})` : ''}
                </option>
              ))}
            </select>
          </div>
          <div className="form-group">
            <label>Contato</label>
            <select
              name="contatoId"
              value={form.contatoId}
              onChange={handleChange}
              disabled={!form.clienteId}
            >
              <option value="">
                {form.clienteId ? 'Selecione o contato' : 'Selecione um cliente primeiro'}
              </option>
              {contatos.map(ct => (
                <option key={ct.id} value={ct.id}>
                  {ct.nome}{ct.cargo ? ` - ${ct.cargo}` : ''}
                </option>
              ))}
            </select>
          </div>
        </div>

        <div className="form-row">
          <div className="form-group">
            <label>Forma de Pagamento</label>
            <input
              type="text"
              name="formaPagamento"
              value={form.formaPagamento}
              onChange={handleChange}
              placeholder="Ex: PIX, Boleto, Cartão..."
            />
          </div>
          <div className="form-group">
            <label>Prazo de Entrega</label>
            <input
              type="text"
              name="prazoEntrega"
              value={form.prazoEntrega}
              onChange={handleChange}
              placeholder="Ex: 15 dias úteis"
            />
          </div>
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

        <div className="form-group">
          <label>Observação</label>
          <textarea
            name="observacao"
            value={form.observacao}
            onChange={handleChange}
            rows={3}
          />
        </div>

        {erro && <div className="erro">{erro}</div>}

        <div className="form-actions">
          <button type="submit" className="btn-primary" disabled={carregando}>
            {carregando ? 'Salvando...' : 'Salvar'}
          </button>
          {!isEdicao && (
            <button type="button" className="btn-secondary" onClick={resetForm}>
              Limpar
            </button>
          )}
        </div>
      </form>
    </div>
  );
}
