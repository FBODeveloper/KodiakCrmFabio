import { useState, useEffect } from 'react';
import api from '../api/axios';
import { useToast } from '../components/Toast';
import LoadingButton from '../components/LoadingButton';

interface PerfilData {
  id: number;
  nome: string;
  email: string;
  avatar?: string;
  dataNascimento?: string;
  perfil: string;
  empresaNome?: string;
  empresaCnpj?: string;
}

export default function Perfil() {
  const { showToast } = useToast();
  const [perfil, setPerfil] = useState<PerfilData | null>(null);
  const [carregando, setCarregando] = useState(true);
  const [salvando, setSalvando] = useState(false);
  const [alterandoSenha, setAlterandoSenha] = useState(false);

  const [form, setForm] = useState({ nome: '', email: '', avatar: '', dataNascimento: '' });
  const [senha, setSenha] = useState({ senhaAtual: '', novaSenha: '', confirmarSenha: '' });

  useEffect(() => { carregarPerfil(); }, []);

  const carregarPerfil = async () => {
    try {
      const res = await api.get('/perfil');
      setPerfil(res.data);
      setForm({
        nome: res.data.nome || '',
        email: res.data.email || '',
        avatar: res.data.avatar || '',
        dataNascimento: res.data.dataNascimento ? res.data.dataNascimento.split('T')[0] : ''
      });
    } catch {
      showToast('erro', 'Erro ao carregar perfil');
    } finally {
      setCarregando(false);
    }
  };

  const salvarPerfil = async () => {
    if (!form.nome.trim()) { showToast('aviso', 'Nome e obrigatorio'); return; }
    if (!form.email.trim()) { showToast('aviso', 'Email e obrigatorio'); return; }
    setSalvando(true);
    try {
      await api.put('/perfil', { nome: form.nome, email: form.email, avatar: form.avatar || null, dataNascimento: form.dataNascimento || null });
      showToast('sucesso', 'Perfil atualizado com sucesso!');
      carregarPerfil();
    } catch (e: any) {
      showToast('erro', e.response?.data?.mensagem || 'Erro ao salvar perfil');
    } finally { setSalvando(false); }
  };

  const alterarSenha = async () => {
    if (!senha.senhaAtual) { showToast('aviso', 'Informe a senha atual'); return; }
    if (senha.novaSenha.length < 6) { showToast('aviso', 'Nova senha deve ter pelo menos 6 caracteres'); return; }
    if (senha.novaSenha !== senha.confirmarSenha) { showToast('aviso', 'As senhas nao conferem'); return; }
    setAlterandoSenha(true);
    try {
      await api.put('/perfil/senha', senha);
      showToast('sucesso', 'Senha alterada com sucesso!');
      setSenha({ senhaAtual: '', novaSenha: '', confirmarSenha: '' });
    } catch (e: any) {
      showToast('erro', e.response?.data?.mensagem || 'Erro ao alterar senha');
    } finally { setAlterandoSenha(false); }
  };

  if (carregando) return <div className="carregando">Carregando...</div>;

  return (
    <div className="configuracoes">
      <h1>Meu Perfil</h1>
      <div className="config-content">
        <div className="config-section">
          <h3>Dados Pessoais</h3>
          <div className="perfil-header">
            {perfil?.avatar ? (
              <img src={perfil.avatar} alt={perfil.nome} className="perfil-avatar" />
            ) : (
              <div className="perfil-avatar-placeholder">{perfil?.nome?.charAt(0).toUpperCase()}</div>
            )}
            <div>
              <h2 style={{ margin: 0 }}>{perfil?.nome}</h2>
              <p style={{ margin: 0, color: 'var(--text-muted)' }}>{perfil?.perfil} - {perfil?.empresaNome}</p>
            </div>
          </div>

          <div className="form-grid">
            <div className="form-group required">
              <label>Nome</label>
              <input type="text" value={form.nome} onChange={e => setForm({ ...form, nome: e.target.value })} />
            </div>
            <div className="form-group required">
              <label>Email</label>
              <input type="email" value={form.email} onChange={e => setForm({ ...form, email: e.target.value })} />
            </div>
            <div className="form-group">
              <label>Avatar (URL)</label>
              <input type="text" value={form.avatar} onChange={e => setForm({ ...form, avatar: e.target.value })} placeholder="https://..." />
            </div>
            <div className="form-group">
              <label>Data de Nascimento</label>
              <input type="date" value={form.dataNascimento} onChange={e => setForm({ ...form, dataNascimento: e.target.value })} />
            </div>
          </div>
          <LoadingButton carregando={salvando} texto="Salvar Perfil" className="btn-primary" onClick={salvarPerfil} />
        </div>

        <div className="config-section">
          <h3>Alterar Senha</h3>
          <div className="form-grid">
            <div className="form-group required">
              <label>Senha Atual</label>
              <input type="password" value={senha.senhaAtual} onChange={e => setSenha({ ...senha, senhaAtual: e.target.value })} />
            </div>
            <div className="form-group required">
              <label>Nova Senha</label>
              <input type="password" value={senha.novaSenha} onChange={e => setSenha({ ...senha, novaSenha: e.target.value })} placeholder="Minimo 6 caracteres" />
            </div>
            <div className="form-group required">
              <label>Confirmar Senha</label>
              <input type="password" value={senha.confirmarSenha} onChange={e => setSenha({ ...senha, confirmarSenha: e.target.value })} />
            </div>
          </div>
          <LoadingButton carregando={alterandoSenha} texto="Alterar Senha" className="btn-primary" onClick={alterarSenha} />
        </div>
      </div>
    </div>
  );
}
