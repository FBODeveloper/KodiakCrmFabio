import { useState } from 'react';
import api from '../api/axios';
import type { RelatorioVendas, RelatorioAtividades, RelatorioPerformance } from '../types';

type Tab = 'vendas' | 'atividades' | 'performance';

export default function Relatorios() {
  const [tab, setTab] = useState<Tab>('vendas');
  const [carregando, setCarregando] = useState(false);
  const [filtro, setFiltro] = useState({
    dataInicio: '',
    dataFim: '',
    status: '',
    responsavelId: '',
    tipoAtividade: ''
  });
  const [vendas, setVendas] = useState<RelatorioVendas | null>(null);
  const [atividades, setAtividades] = useState<RelatorioAtividades | null>(null);
  const [performance, setPerformance] = useState<RelatorioPerformance | null>(null);

  const formatarMoeda = (v: number) => new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(v);
  const formatarPercentual = (v: number) => `${v.toFixed(1)}%`;

  const gerarRelatorio = async () => {
    setCarregando(true);
    try {
      const params: Record<string, any> = {};
      if (filtro.dataInicio) params.dataInicio = filtro.dataInicio;
      if (filtro.dataFim) params.dataFim = filtro.dataFim;
      if (filtro.status) params.status = filtro.status;
      if (filtro.responsavelId) params.responsavelId = parseInt(filtro.responsavelId);
      if (filtro.tipoAtividade) params.tipo = filtro.tipoAtividade;

      if (tab === 'vendas') {
        const res = await api.get('/relatorio/vendas', { params });
        setVendas(res.data);
      } else if (tab === 'atividades') {
        const res = await api.get('/relatorio/atividades', { params });
        setAtividades(res.data);
      } else {
        const res = await api.get('/relatorio/performance', { params });
        setPerformance(res.data);
      }
    } catch (error) {
      console.error('Erro ao gerar relatório:', error);
    } finally {
      setCarregando(false);
    }
  };

  return (
    <div className="relatorios">
      <h1>Relatórios</h1>

      <div className="config-tabs">
        <button className={`config-tab ${tab === 'vendas' ? 'active' : ''}`} onClick={() => setTab('vendas')}>Vendas</button>
        <button className={`config-tab ${tab === 'atividades' ? 'active' : ''}`} onClick={() => setTab('atividades')}>Atividades</button>
        <button className={`config-tab ${tab === 'performance' ? 'active' : ''}`} onClick={() => setTab('performance')}>Performance</button>
      </div>

      <div className="relatorio-filtros">
        <div className="form-grid">
          <div className="form-group">
            <label>Data Início</label>
            <input type="date" value={filtro.dataInicio} onChange={e => setFiltro({ ...filtro, dataInicio: e.target.value })} />
          </div>
          <div className="form-group">
            <label>Data Fim</label>
            <input type="date" value={filtro.dataFim} onChange={e => setFiltro({ ...filtro, dataFim: e.target.value })} />
          </div>
          {tab === 'vendas' && (
            <div className="form-group">
              <label>Status</label>
              <select value={filtro.status} onChange={e => setFiltro({ ...filtro, status: e.target.value })}>
                <option value="">Todos</option>
                <option value="ganha">Ganha</option>
                <option value="perdida">Perdida</option>
                <option value="aberta">Aberta</option>
              </select>
            </div>
          )}
          {tab === 'atividades' && (
            <div className="form-group">
              <label>Tipo</label>
              <select value={filtro.tipoAtividade} onChange={e => setFiltro({ ...filtro, tipoAtividade: e.target.value })}>
                <option value="">Todos</option>
                <option value="reuniao">Reunião</option>
                <option value="ligacao">Ligação</option>
                <option value="email">Email</option>
                <option value="tarefa">Tarefa</option>
                <option value="visita">Visita</option>
              </select>
            </div>
          )}
        </div>
        <button className="btn btn-primary" onClick={gerarRelatorio} disabled={carregando}>
          {carregando ? 'Gerando...' : 'Gerar Relatório'}
        </button>
      </div>

      {carregando && <div className="carregando">Gerando relatório...</div>}

      {tab === 'vendas' && vendas && (
        <div className="relatorio-resultado">
          <div className="dashboard-grid">
            <div className="dashboard-card"><h3>Total</h3><p className="numero">{vendas.totalOportunidades}</p></div>
            <div className="dashboard-card"><h3>Ganhas</h3><p className="numero" style={{ color: '#10b981' }}>{vendas.oportunidadesGanhas}</p></div>
            <div className="dashboard-card"><h3>Perdidas</h3><p className="numero" style={{ color: '#ef4444' }}>{vendas.oportunidadesPerdidas}</p></div>
            <div className="dashboard-card"><h3>Abertas</h3><p className="numero" style={{ color: '#f59e0b' }}>{vendas.oportunidadesAbertas}</p></div>
            <div className="dashboard-card"><h3>Valor Total</h3><p className="numero">{formatarMoeda(vendas.valorTotal)}</p></div>
            <div className="dashboard-card"><h3>Valor Ganho</h3><p className="numero" style={{ color: '#10b981' }}>{formatarMoeda(vendas.valorGanho)}</p></div>
            <div className="dashboard-card"><h3>Ticket Médio</h3><p className="numero">{formatarMoeda(vendas.ticketMedio)}</p></div>
            <div className="dashboard-card"><h3>Taxa Conversão</h3><p className="numero">{formatarPercentual(vendas.taxaConversao)}</p></div>
          </div>

          {vendas.porResponsavel.length > 0 && (
            <div className="config-section">
              <h3>Por Responsável</h3>
              <table className="tabela">
                <thead><tr><th>Vendedor</th><th>Total</th><th>Ganhas</th><th>Valor</th></tr></thead>
                <tbody>
                  {vendas.porResponsavel.map((r, i) => (
                    <tr key={i}><td><strong>{r.responsavelNome}</strong></td><td>{r.total}</td><td style={{ color: '#10b981' }}>{r.ganhas}</td><td>{formatarMoeda(r.valorTotal)}</td></tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}

      {tab === 'atividades' && atividades && (
        <div className="relatorio-resultado">
          <div className="dashboard-grid">
            <div className="dashboard-card"><h3>Total</h3><p className="numero">{atividades.totalAtividades}</p></div>
            <div className="dashboard-card"><h3>Concluídas</h3><p className="numero" style={{ color: '#10b981' }}>{atividades.concluidas}</p></div>
            <div className="dashboard-card"><h3>Pendentes</h3><p className="numero" style={{ color: '#f59e0b' }}>{atividades.pendentes}</p></div>
            <div className="dashboard-card"><h3>Taxa Conclusão</h3><p className="numero">{formatarPercentual(atividades.taxaConclusao)}</p></div>
          </div>

          {atividades.porTipo.length > 0 && (
            <div className="config-section">
              <h3>Por Tipo</h3>
              <table className="tabela">
                <thead><tr><th>Tipo</th><th>Total</th><th>Concluídas</th></tr></thead>
                <tbody>
                  {atividades.porTipo.map((t, i) => (
                    <tr key={i}><td><strong>{t.tipo}</strong></td><td>{t.quantidade}</td><td style={{ color: '#10b981' }}>{t.concluidas}</td></tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}

          {atividades.porResponsavel.length > 0 && (
            <div className="config-section">
              <h3>Por Responsável</h3>
              <table className="tabela">
                <thead><tr><th>Vendedor</th><th>Total</th><th>Concluídas</th></tr></thead>
                <tbody>
                  {atividades.porResponsavel.map((r, i) => (
                    <tr key={i}><td><strong>{r.responsavelNome}</strong></td><td>{r.total}</td><td style={{ color: '#10b981' }}>{r.concluidas}</td></tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}

      {tab === 'performance' && performance && (
        <div className="relatorio-resultado">
          {performance.vendedores.length === 0 ? (
            <p className="vazio">Nenhum vendedor com dados no período</p>
          ) : (
            <div className="config-section">
              <h3>Performance por Vendedor</h3>
              <table className="tabela">
                <thead><tr><th>Vendedor</th><th>Leads</th><th>Oportunidades</th><th>Ganhas</th><th>Conversão</th><th>Atividades</th><th>Valor</th></tr></thead>
                <tbody>
                  {performance.vendedores.map((v, i) => (
                    <tr key={i}>
                      <td><strong>{v.vendedorNome}</strong></td>
                      <td>{v.totalLeads}</td>
                      <td>{v.totalOportunidades}</td>
                      <td style={{ color: '#10b981' }}>{v.oportunidadesGanhas}</td>
                      <td>{formatarPercentual(v.taxaConversao)}</td>
                      <td>{v.totalAtividades}</td>
                      <td>{formatarMoeda(v.valorTotal)}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
