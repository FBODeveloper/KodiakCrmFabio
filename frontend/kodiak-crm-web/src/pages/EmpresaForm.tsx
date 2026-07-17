import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../api/axios';
import type { EmpresaCreate } from '../types';

export default function EmpresaForm() {
  const { cnpj } = useParams();
  const navigate = useNavigate();
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState('');
  const [form, setForm] = useState<EmpresaCreate>({
    cnpj: '',
    razaoSocial: '',
    nomeFantasia: '',
    quantidadeUsuariosContratados: 1
  });

  const isEdicao = !!cnpj;

  useEffect(() => {
    if (cnpj) {
      carregarEmpresa(cnpj);
    }
  }, [cnpj]);

  const carregarEmpresa = async (cnpj: string) => {
    try {
      const response = await api.get(`/empresa/${cnpj}`);
      setForm({
        cnpj: response.data.cnpj,
        razaoSocial: response.data.razaoSocial,
        nomeFantasia: response.data.nomeFantasia,
        quantidadeUsuariosContratados: response.data.quantidadeUsuariosContratados
      });
    } catch (error) {
      console.error('Erro ao carregar empresa:', error);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro('');
    setCarregando(true);

    try {
      if (isEdicao) {
        await api.put(`/empresa/${cnpj}`, {
          razaoSocial: form.razaoSocial,
          nomeFantasia: form.nomeFantasia,
          quantidadeUsuariosContratados: form.quantidadeUsuariosContratados
        });
      } else {
        await api.post('/empresa', form);
      }
      navigate('/empresas');
    } catch (error: any) {
      setErro(error.response?.data?.mensagem || 'Erro ao salvar empresa');
    } finally {
      setCarregando(false);
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    if (name === 'cnpj') {
      setForm({ ...form, cnpj: value.replace(/\D/g, '').slice(0, 14) });
    } else {
      setForm({ ...form, [name]: value });
    }
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{isEdicao ? 'Editar Empresa' : 'Nova Empresa'}</h1>
        <button onClick={() => navigate('/empresas')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <form onSubmit={handleSubmit} className="form">
        <div className="form-row">
          <div className="form-group">
            <label>CNPJ *</label>
            <input
              type="text"
              name="cnpj"
              value={form.cnpj}
              onChange={handleChange}
              disabled={isEdicao}
              required
            />
          </div>
          
          <div className="form-group">
            <label>Nome/Razão Social *</label>
            <input
              type="text"
              name="razaoSocial"
              value={form.razaoSocial}
              onChange={handleChange}
              required
            />
          </div>
        </div>
        
        <div className="form-row">
          <div className="form-group">
            <label>Apelido/Nome Fantasia</label>
            <input
              type="text"
              name="nomeFantasia"
              value={form.nomeFantasia}
              onChange={handleChange}
            />
          </div>
          
          <div className="form-group">
            <label>Qtd Usuários Contratados *</label>
            <input
              type="number"
              name="quantidadeUsuariosContratados"
              value={form.quantidadeUsuariosContratados}
              onChange={handleChange}
              min="1"
              required
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
