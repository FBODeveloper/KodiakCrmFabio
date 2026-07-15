import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { ParceiroCreate } from '../types';

export default function ParceiroForm() {
  const navigate = useNavigate();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [form, setForm] = useState<ParceiroCreate>({
    razaoSocial: '',
    nomeFantasia: '',
    cpfCnpj: '',
    tipoPessoa: 'J',
    email: '',
    telefone: '',
    celular: '',
    observacao: ''
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      await api.post('/parceiros', form);
      navigate('/parceiros');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao criar parceiro');
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
        <h1>Novo Parceiro</h1>
        <button onClick={() => navigate('/parceiros')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <form onSubmit={handleSubmit} className="form">
        <div className="form-row">
          <div className="form-group">
            <label>Razão Social *</label>
            <input
              type="text"
              name="razaoSocial"
              value={form.razaoSocial}
              onChange={handleChange}
              required
            />
          </div>
          
          <div className="form-group">
            <label>Nome Fantasia</label>
            <input
              type="text"
              name="nomeFantasia"
              value={form.nomeFantasia}
              onChange={handleChange}
            />
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Tipo Pessoa</label>
            <select name="tipoPessoa" value={form.tipoPessoa} onChange={handleChange}>
              <option value="J">Jurídica</option>
              <option value="F">Física</option>
            </select>
          </div>
          
          <div className="form-group">
            <label>CPF/CNPJ</label>
            <input
              type="text"
              name="cpfCnpj"
              value={form.cpfCnpj}
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
          
          <div className="form-group">
            <label>Celular</label>
            <input
              type="text"
              name="celular"
              value={form.celular}
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
