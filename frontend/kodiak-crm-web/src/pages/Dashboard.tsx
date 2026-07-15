import { useState, useEffect } from 'react';
import api from '../api/axios';
import type { DashboardResumo } from '../types';

export default function Dashboard() {
  const [resumo, setResumo] = useState<DashboardResumo | null>(null);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    carregarResumo();
  }, []);

  const carregarResumo = async () => {
    try {
      const response = await api.get('/dashboard/resumo');
      setResumo(response.data);
    } catch (error) {
      console.error('Erro ao carregar dashboard:', error);
    } finally {
      setCarregando(false);
    }
  };

  if (carregando) {
    return <div className="carregando">Carregando...</div>;
  }

  return (
    <div className="dashboard">
      <h1>Dashboard</h1>
      
      <div className="dashboard-grid">
        <div className="dashboard-card">
          <h3>Parceiros</h3>
          <p className="numero">{resumo?.totalParceiros || 0}</p>
        </div>
        
        <div className="dashboard-card">
          <h3>Leads</h3>
          <p className="numero">{resumo?.totalLeads || 0}</p>
          <p className="subtitulo">{resumo?.leadsNovos || 0} novos</p>
        </div>
        
        <div className="dashboard-card">
          <h3>Oportunidades</h3>
          <p className="numero">{resumo?.totalOportunidades || 0}</p>
        </div>
        
        <div className="dashboard-card">
          <h3>Valor do Funil</h3>
          <p className="numero">
            {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' })
              .format(resumo?.valorFunil || 0)}
          </p>
        </div>
        
        <div className="dashboard-card">
          <h3>Atividades Pendentes</h3>
          <p className="numero">{resumo?.atividadesPendentes || 0}</p>
        </div>
        
        <div className="dashboard-card">
          <h3>Propostas Enviadas</h3>
          <p className="numero">{resumo?.propostasEnviadas || 0}</p>
        </div>
      </div>
    </div>
  );
}
