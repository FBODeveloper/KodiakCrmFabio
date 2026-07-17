import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';
import type { Empresa, Historico, PaginatedResponse } from '../types';

type Tab = 'empresa' | 'usuarios' | 'logs';

export default function Configuracoes() {
  const [tab, setTab] = useState<Tab>('empresa');
  const [empresa, setEmpresa] = useState<Empresa | null>(null);
  const [usuarios, setUsuarios] = useState<PaginatedResponse<any> | null>(null);
  const [logs, setLogs] = useState<Historico[]>([]);
  const [carregando, setCarregando] = useState(true);
  const [salvando, setSalvando] = useState(false);
  const [mensagem, setMensagem] = useState<{ tipo: 'sucesso' | 'erro'; texto: string } | null>(null);
  const navigate = useNavigate();

  const [formEmpresa, setFormEmpresa] = useState({
    razaoSocial: '',
    nomeFantasia: '',
    telefone: '',
    email: '',
    endereco: '',
    quantidadeUsuariosContratados: 1
  });

  const [formConfig, setFormConfig] = useState({
    tema: 'light',
    fusoHorario: 'America/Sao_Paulo',
    moeda: 'BRL',
    idioma: 'pt-BR',
    notificacoesEmail: true,
    notificacoesSistema: true
  });

  useEffect(() => {
    carregarDados();
  }, []);

  useEffect(() => {
    if (tab === 'usuarios') carregarUsuarios();
    if (tab === 'logs') carregarLogs();
  }, [tab]);

  const carregarDados = async () => {
    try {
      const [empresaRes, configRes] = await Promise.all([
        api.get('/config/empresa'),
        api.get('/config')
      ]);
      setEmpresa(empresaRes.data);
      setFormEmpresa({
        razaoSocial: empresaRes.data.razaoSocial || '',
        nomeFantasia: empresaRes.data.nomeFantasia || '',
        telefone: empresaRes.data.telefone || '',
        email: empresaRes.data.email || '',
        endereco: empresaRes.data.endereco || '',
        quantidadeUsuariosContratados: empresaRes.data.quantidadeUsuariosContratados || 1
      });
      setFormConfig({
        tema: configRes.data.tema || 'light',
        fusoHorario: configRes.data.fusoHorario || 'America/Sao_Paulo',
        moeda: configRes.data.moeda || 'BRL',
        idioma: configRes.data.idioma || 'pt-BR',
        notificacoesEmail: configRes.data.notificacoesEmail ?? true,
        notificacoesSistema: configRes.data.notificacoesSistema ?? true
      });
    } catch (error) {
      console.error('Erro ao carregar configurações:', error);
    } finally {
      setCarregando(false);
    }
  };

  const carregarUsuarios = async () => {
    try {
      const res = await api.get('/usuariogestao', { params: { itensPorPagina: 50 } });
      setUsuarios(res.data);
    } catch (error) {
      console.error('Erro ao carregar usuários:', error);
    }
  };

  const carregarLogs = async () => {
    try {
      const res = await api.get('/historico/recentes', { params: { limite: 50 } });
      setLogs(res.data);
    } catch (error) {
      console.error('Erro ao carregar logs:', error);
    }
  };

  const salvarEmpresa = async () => {
    setSalvando(true);
    setMensagem(null);
    try {
      await api.put('/config/empresa', formEmpresa);
      setMensagem({ tipo: 'sucesso', texto: 'Dados da empresa atualizados!' });
    } catch (error: any) {
      setMensagem({ tipo: 'erro', texto: error.response?.data?.mensagem || 'Erro ao salvar' });
    } finally {
      setSalvando(false);
    }
  };

  const salvarConfig = async () => {
    setSalvando(true);
    setMensagem(null);
    try {
      await api.put('/config', formConfig);
      setMensagem({ tipo: 'sucesso', texto: 'Configurações salvas!' });
    } catch (error: any) {
      setMensagem({ tipo: 'erro', texto: error.response?.data?.mensagem || 'Erro ao salvar' });
    } finally {
      setSalvando(false);
    }
  };

  const perfilCores: Record<string, string> = {
    admin: '#ef4444',
    gerente: '#f59e0b',
    usuario: '#3b82f6'
  };

  const acaoCores: Record<string, string> = {
    criado: '#10b981',
    alterado: '#3b82f6',
    excluido: '#ef4444',
    etapa_alterada: '#f59e0b',
    convertido: '#8b5cf6'
  };

  if (carregando) {
    return <div className="carregando">Carregando...</div>;
  }

  return (
    <div className="configuracoes">
      <h1>Configurações</h1>

      <div className="config-tabs">
        <button
          className={`config-tab ${tab === 'empresa' ? 'active' : ''}`}
          onClick={() => { setTab('empresa'); setMensagem(null); }}
        >
          Empresa
        </button>
        <button
          className={`config-tab ${tab === 'usuarios' ? 'active' : ''}`}
          onClick={() => { setTab('usuarios'); setMensagem(null); }}
        >
          Usuários
        </button>
        <button
          className={`config-tab ${tab === 'logs' ? 'active' : ''}`}
          onClick={() => { setTab('logs'); setMensagem(null); }}
        >
          Logs de Atividade
        </button>
      </div>

      {mensagem && (
        <div className={`alert alert-${mensagem.tipo}`}>
          {mensagem.texto}
        </div>
      )}

      {tab === 'empresa' && (
        <div className="config-content">
          <div className="config-section">
            <h3>Dados da Empresa</h3>
            <div className="form-grid">
              <div className="form-group">
                <label>Nome/Razão Social</label>
                <input
                  type="text"
                  value={formEmpresa.razaoSocial}
                  onChange={e => setFormEmpresa({ ...formEmpresa, razaoSocial: e.target.value })}
                  placeholder="Nome/Razão Social"
                />
              </div>
              <div className="form-group">
                <label>Apelido/Nome Fantasia</label>
                <input
                  type="text"
                  value={formEmpresa.nomeFantasia}
                  onChange={e => setFormEmpresa({ ...formEmpresa, nomeFantasia: e.target.value })}
                  placeholder="Apelido/Nome Fantasia"
                />
              </div>
              <div className="form-group">
                <label>CNPJ</label>
                <input
                  type="text"
                  value={empresa?.cnpj || ''}
                  disabled
                  className="disabled"
                />
              </div>
              <div className="form-group">
                <label>Telefone</label>
                <input
                  type="text"
                  value={formEmpresa.telefone}
                  onChange={e => setFormEmpresa({ ...formEmpresa, telefone: e.target.value })}
                  placeholder="(11) 99999-9999"
                />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input
                  type="email"
                  value={formEmpresa.email}
                  onChange={e => setFormEmpresa({ ...formEmpresa, email: e.target.value })}
                  placeholder="contato@empresa.com"
                />
              </div>
              <div className="form-group full-width">
                <label>Endereço</label>
                <input
                  type="text"
                  value={formEmpresa.endereco}
                  onChange={e => setFormEmpresa({ ...formEmpresa, endereco: e.target.value })}
                  placeholder="Rua, Número, Bairro, Cidade - UF"
                />
              </div>
            </div>
            <button className="btn btn-primary" onClick={salvarEmpresa} disabled={salvando}>
              {salvando ? 'Salvando...' : 'Salvar Dados'}
            </button>
          </div>

          <div className="config-section">
            <h3>Configurações do Sistema</h3>
            <div className="form-grid">
              <div className="form-group">
                <label>Tema</label>
                <select
                  value={formConfig.tema}
                  onChange={e => setFormConfig({ ...formConfig, tema: e.target.value })}
                >
                  <option value="light">Claro</option>
                  <option value="dark">Escuro</option>
                </select>
              </div>
              <div className="form-group">
                <label>Fuso Horário</label>
                <select
                  value={formConfig.fusoHorario}
                  onChange={e => setFormConfig({ ...formConfig, fusoHorario: e.target.value })}
                >
                  <option value="America/Sao_Paulo">São Paulo (GMT-3)</option>
                  <option value="America/Manaus">Manaus (GMT-4)</option>
                  <option value="America/Noronha">Fernando de Noronha (GMT-2)</option>
                  <option value="America/Belem">Belém (GMT-3)</option>
                  <option value="America/Fortaleza">Fortaleza (GMT-3)</option>
                  <option value="America/Recife">Recife (GMT-3)</option>
                  <option value="America/Bahia">Salvador (GMT-3)</option>
                </select>
              </div>
              <div className="form-group">
                <label>Moeda</label>
                <select
                  value={formConfig.moeda}
                  onChange={e => setFormConfig({ ...formConfig, moeda: e.target.value })}
                >
                  <option value="BRL">BRL - Real</option>
                  <option value="USD">USD - Dólar</option>
                  <option value="EUR">EUR - Euro</option>
                </select>
              </div>
              <div className="form-group">
                <label>Idioma</label>
                <select
                  value={formConfig.idioma}
                  onChange={e => setFormConfig({ ...formConfig, idioma: e.target.value })}
                >
                  <option value="pt-BR">Português (Brasil)</option>
                  <option value="en-US">English (US)</option>
                  <option value="es">Español</option>
                </select>
              </div>
            </div>
            <div className="form-check-group">
              <label className="form-check">
                <input
                  type="checkbox"
                  checked={formConfig.notificacoesEmail}
                  onChange={e => setFormConfig({ ...formConfig, notificacoesEmail: e.target.checked })}
                />
                Receber notificações por email
              </label>
              <label className="form-check">
                <input
                  type="checkbox"
                  checked={formConfig.notificacoesSistema}
                  onChange={e => setFormConfig({ ...formConfig, notificacoesSistema: e.target.checked })}
                />
                Receber notificações no sistema
              </label>
            </div>
            <button className="btn btn-primary" onClick={salvarConfig} disabled={salvando}>
              {salvando ? 'Salvando...' : 'Salvar Configurações'}
            </button>
          </div>
        </div>
      )}

      {tab === 'usuarios' && (
        <div className="config-content">
          <div className="config-section">
            <div className="panel-header">
              <h3>Usuários da Empresa</h3>
              <button className="btn btn-primary btn-sm" onClick={() => navigate('/usuarios/novo')}>
                + Novo Usuário
              </button>
            </div>
            {!usuarios || usuarios.itens.length === 0 ? (
              <p className="vazio">Nenhum usuário encontrado</p>
            ) : (
              <table className="tabela">
                <thead>
                  <tr>
                    <th>Nome</th>
                    <th>Email</th>
                    <th>Perfil</th>
                    <th>Status</th>
                    <th>Cadastro</th>
                  </tr>
                </thead>
                <tbody>
                  {usuarios.itens.map((u: any) => (
                    <tr key={u.id} onClick={() => navigate(`/usuarios/${u.id}`)} style={{ cursor: 'pointer' }}>
                      <td><strong>{u.nome}</strong></td>
                      <td>{u.email}</td>
                      <td>
                        <span className="status-badge" style={{ backgroundColor: perfilCores[u.perfil] || '#6b7280' }}>
                          {u.perfil}
                        </span>
                      </td>
                      <td>
                        <span className={`status-badge ${u.ativo ? 'status-ativo' : 'status-inativo'}`}>
                          {u.ativo ? 'Ativo' : 'Inativo'}
                        </span>
                      </td>
                      <td>{new Date(u.dataCadastro).toLocaleDateString('pt-BR')}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        </div>
      )}

      {tab === 'logs' && (
        <div className="config-content">
          <div className="config-section">
            <h3>Últimas Atividades</h3>
            {logs.length === 0 ? (
              <p className="vazio">Nenhum log encontrado</p>
            ) : (
              <div className="historico-timeline">
                {logs.map((log) => (
                  <div key={log.id} className="historico-item">
                    <div className="historico-dot" style={{ background: acaoCores[log.acao] || '#6b7280' }}></div>
                    <div className="historico-conteudo">
                      <div className="historico-header">
                        <span className="historico-entidade">{log.entidade}</span>
                        <span className="historico-data">{new Date(log.dataAcao).toLocaleString('pt-BR')}</span>
                      </div>
                      <p className="historico-descricao">{log.descricao}</p>
                      {log.usuarioNome && (
                        <span className="historico-usuario">por {log.usuarioNome}</span>
                      )}
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
