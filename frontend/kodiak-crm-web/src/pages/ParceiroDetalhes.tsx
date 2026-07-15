import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../api/axios';
import type { Parceiro } from '../types';

export default function ParceiroDetalhes() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [parceiro, setParceiro] = useState<Parceiro | null>(null);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    if (id) carregarParceiro(parseInt(id));
  }, [id]);

  const carregarParceiro = async (parceiroId: number) => {
    try {
      const response = await api.get(`/parceiros/${parceiroId}`);
      setParceiro(response.data);
    } catch (error) {
      console.error('Erro ao carregar parceiro:', error);
    } finally {
      setCarregando(false);
    }
  };

  if (carregando) {
    return <div className="carregando">Carregando...</div>;
  }

  if (!parceiro) {
    return <div className="erro">Parceiro não encontrado</div>;
  }

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>{parceiro.razaoSocial}</h1>
        <button onClick={() => navigate('/parceiros')} className="btn-secondary">
          Voltar
        </button>
      </div>
      
      <div className="detalhes-grid">
        <div className="detalhes-campo">
          <label>Nome Fantasia</label>
          <p>{parceiro.nomeFantasia || '-'}</p>
        </div>
        
        <div className="detalhes-campo">
          <label>CPF/CNPJ</label>
          <p>{parceiro.cpfCnpj || '-'}</p>
        </div>
        
        <div className="detalhes-campo">
          <label>Tipo Pessoa</label>
          <p>{parceiro.tipoPessoa === 'J' ? 'Jurídica' : parceiro.tipoPessoa === 'F' ? 'Física' : '-'}</p>
        </div>
        
        <div className="detalhes-campo">
          <label>Email</label>
          <p>{parceiro.email || '-'}</p>
        </div>
        
        <div className="detalhes-campo">
          <label>Telefone</label>
          <p>{parceiro.telefone || '-'}</p>
        </div>
        
        <div className="detalhes-campo">
          <label>Celular</label>
          <p>{parceiro.celular || '-'}</p>
        </div>
        
        <div className="detalhes-campo">
          <label>ID KodiakERP</label>
          <p>{parceiro.idParceiroKodiakErp || '-'}</p>
        </div>
        
        <div className="detalhes-campo">
          <label>Observação</label>
          <p>{parceiro.observacao || '-'}</p>
        </div>
      </div>
    </div>
  );
}
