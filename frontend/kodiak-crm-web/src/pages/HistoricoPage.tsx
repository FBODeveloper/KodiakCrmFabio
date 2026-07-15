import { useState, useEffect } from 'react';
import api from '../api/axios';
import type { Historico } from '../types';

const entidadeLabels: Record<string, string> = {
  lead: 'Lead',
  oportunidade: 'Oportunidade',
  proposta: 'Proposta',
  atividade: 'Atividade',
  parceiro: 'Parceiro',
  empresa: 'Empresa',
  usuario: 'Usuário'
};

const acaoColors: Record<string, string> = {
  criado: '#10b981',
  alterado: '#3b82f6',
  etapa_alterada: '#f59e0b',
  status_alterada: '#f59e0b',
  excluido: '#ef4444',
  convertido: '#6366f1'
};

const acaoIcons: Record<string, string> = {
  criado: '+',
  alterado: '~',
  etapa_alterada: '>',
  status_alterada: '>',
  excluido: 'x',
  convertido: '>>'
};

export default function HistoricoPage() {
  const [historicos, setHistoricos] = useState<Historico[]>([]);
  const [carregando, setCarregando] = useState(true);

  useEffect(() => {
    carregarHistorico();
  }, []);

  const carregarHistorico = async () => {
    try {
      const response = await api.get<Historico[]>('/historico/recentes', { params: { limite: 50 } });
      setHistoricos(response.data);
    } catch (error) {
      console.error('Erro ao carregar histórico:', error);
    } finally {
      setCarregando(false);
    }
  };

  const formatarData = (data: string) => {
    const d = new Date(data);
    const agora = new Date();
    const diffMs = agora.getTime() - d.getTime();
    const diffMin = Math.floor(diffMs / 60000);
    const diffH = Math.floor(diffMin / 60);
    const diffD = Math.floor(diffH / 24);

    if (diffMin < 1) return 'Agora';
    if (diffMin < 60) return `${diffMin}min atrás`;
    if (diffH < 24) return `${diffH}h atrás`;
    if (diffD < 7) return `${diffD}d atrás`;
    return d.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit', year: '2-digit' });
  };

  return (
    <div className="pagina">
      <div className="pagina-header">
        <h1>Histórico</h1>
      </div>

      {carregando ? (
        <div className="carregando">Carregando...</div>
      ) : historicos.length === 0 ? (
        <div className="vazio">Nenhum registro encontrado</div>
      ) : (
        <div className="historico-timeline">
          {historicos.map((h) => (
            <div key={h.id} className="historico-item">
              <div className="historico-dot" style={{ background: acaoColors[h.acao] || '#6b7280' }}>
                <span>{acaoIcons[h.acao] || '?'}</span>
              </div>
              <div className="historico-conteudo">
                <div className="historico-header">
                  <span className="historico-entidade" style={{ color: acaoColors[h.acao] || '#6b7280' }}>
                    {entidadeLabels[h.entidade] || h.entidade}
                  </span>
                  <span className="historico-data">{formatarData(h.dataAcao)}</span>
                </div>
                <p className="historico-descricao">{h.descricao}</p>
                {h.usuarioNome && (
                  <span className="historico-usuario">por {h.usuarioNome}</span>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
