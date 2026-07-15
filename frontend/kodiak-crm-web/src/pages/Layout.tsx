import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext';

export default function Layout() {
  const { usuario, logout, isAdmin } = useAuth();
  const navigate = useNavigate();
  const [darkMode, setDarkMode] = useState(() => {
    return localStorage.getItem('theme') === 'dark';
  });

  useEffect(() => {
    document.documentElement.setAttribute('data-theme', darkMode ? 'dark' : 'light');
    localStorage.setItem('theme', darkMode ? 'dark' : 'light');
  }, [darkMode]);

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="layout">
      <aside className="sidebar">
        <div className="sidebar-header">
          <h2>Kodiak CRM</h2>
        </div>
        
        <nav className="sidebar-nav">
          <NavLink to="/" end>Dashboard</NavLink>
          <NavLink to="/parceiros">Parceiros</NavLink>
          <NavLink to="/leads">Leads</NavLink>
          <NavLink to="/oportunidades">Oportunidades</NavLink>
          <NavLink to="/atividades">Atividades</NavLink>
          <NavLink to="/propostas">Propostas</NavLink>
          
          {isAdmin && (
            <>
              <div className="sidebar-divider"></div>
              <NavLink to="/empresas">Empresas</NavLink>
              <NavLink to="/usuarios">Usuários</NavLink>
            </>
          )}
        </nav>
        
        <div className="sidebar-footer">
          <div className="user-info">
            {usuario?.avatar ? (
              <img src={usuario.avatar} alt={usuario.nome} className="user-avatar" />
            ) : (
              <div className="user-avatar-placeholder">
                {usuario?.nome?.charAt(0).toUpperCase()}
              </div>
            )}
            <div className="user-details">
              <span className="user-name">{usuario?.nome}</span>
              <span className="user-role">{usuario?.perfil}</span>
            </div>
          </div>
          <div className="sidebar-actions">
            <button
              onClick={() => setDarkMode(!darkMode)}
              className="btn-theme-toggle"
              title={darkMode ? 'Modo claro' : 'Modo escuro'}
            >
              {darkMode ? '☀️' : '🌙'}
            </button>
            <button onClick={handleLogout}>Sair</button>
          </div>
        </div>
      </aside>
      
      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
}
