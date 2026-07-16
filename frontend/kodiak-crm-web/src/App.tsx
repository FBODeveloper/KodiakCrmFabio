import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useEffect } from 'react';
import { AuthProvider, useAuth } from './contexts/AuthContext';
import Layout from './pages/Layout';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Parceiros from './pages/Parceiros';
import ParceiroDetalhes from './pages/ParceiroDetalhes';
import ParceiroForm from './pages/ParceiroForm';
import Leads from './pages/Leads';
import LeadForm from './pages/LeadForm';
import LeadKanban from './pages/LeadKanban';
import Empresas from './pages/Empresas';
import EmpresaForm from './pages/EmpresaForm';
import Usuarios from './pages/Usuarios';
import UsuarioForm from './pages/UsuarioForm';
import Oportunidades from './pages/Oportunidades';
import OportunidadeForm from './pages/OportunidadeForm';
import Atividades from './pages/Atividades';
import AtividadeForm from './pages/AtividadeForm';
import Propostas from './pages/Propostas';
import PropostaForm from './pages/PropostaForm';
import HistoricoPage from './pages/HistoricoPage';
import Clientes from './pages/Clientes';
import ClienteForm from './pages/ClienteForm';
import Contatos from './pages/Contatos';
import ContatoForm from './pages/ContatoForm';

function PrivateRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" />;
}

function AdminRoute({ children }: { children: React.ReactNode }) {
  const { isAuthenticated, isAdmin } = useAuth();
  if (!isAuthenticated) return <Navigate to="/login" />;
  if (!isAdmin) return <Navigate to="/" />;
  return <>{children}</>;
}

function AppRoutes() {
  const { isAuthenticated } = useAuth();

  return (
    <Routes>
      <Route path="/login" element={isAuthenticated ? <Navigate to="/" /> : <Login />} />
      
      <Route
        path="/"
        element={
          <PrivateRoute>
            <Layout />
          </PrivateRoute>
        }
      >
        <Route index element={<Dashboard />} />
        <Route path="parceiros" element={<Parceiros />} />
        <Route path="parceiros/novo" element={<ParceiroForm />} />
        <Route path="parceiros/:id" element={<ParceiroDetalhes />} />
        <Route path="leads" element={<Leads />} />
        <Route path="leads/novo" element={<LeadForm />} />
        <Route path="leads/kanban" element={<LeadKanban />} />
        <Route path="leads/:id" element={<LeadForm />} />
        <Route path="leads/:id/editar" element={<LeadForm />} />
        
        <Route path="empresas" element={<AdminRoute><Empresas /></AdminRoute>} />
        <Route path="empresas/novo" element={<AdminRoute><EmpresaForm /></AdminRoute>} />
        <Route path="empresas/:cnpj" element={<AdminRoute><EmpresaForm /></AdminRoute>} />
        
        <Route path="usuarios" element={<AdminRoute><Usuarios /></AdminRoute>} />
        <Route path="usuarios/novo" element={<AdminRoute><UsuarioForm /></AdminRoute>} />
        <Route path="usuarios/:id" element={<AdminRoute><UsuarioForm /></AdminRoute>} />

        <Route path="oportunidades" element={<Oportunidades />} />
        <Route path="oportunidades/novo" element={<OportunidadeForm />} />
        <Route path="oportunidades/:id" element={<OportunidadeForm />} />

        <Route path="atividades" element={<Atividades />} />
        <Route path="atividades/novo" element={<AtividadeForm />} />
        <Route path="atividades/:id" element={<AtividadeForm />} />

        <Route path="propostas" element={<Propostas />} />
        <Route path="propostas/novo" element={<PropostaForm />} />
        <Route path="propostas/:id" element={<PropostaForm />} />

        <Route path="historico" element={<HistoricoPage />} />

        <Route path="clientes" element={<Clientes />} />
        <Route path="clientes/novo" element={<ClienteForm />} />
        <Route path="clientes/:id" element={<ClienteForm />} />

        <Route path="contatos" element={<Contatos />} />
        <Route path="contatos/novo" element={<ContatoForm />} />
        <Route path="contatos/:id" element={<ContatoForm />} />
      </Route>
      
      <Route path="*" element={<Navigate to="/" />} />
    </Routes>
  );
}

export default function App() {
  useEffect(() => {
    const theme = localStorage.getItem('theme') || 'light';
    document.documentElement.setAttribute('data-theme', theme);
  }, []);

  return (
    <BrowserRouter>
      <AuthProvider>
        <AppRoutes />
      </AuthProvider>
    </BrowserRouter>
  );
}
