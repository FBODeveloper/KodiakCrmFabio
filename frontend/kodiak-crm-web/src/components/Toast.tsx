import { useState, useEffect, useCallback, createContext, useContext } from 'react';

interface Toast {
  id: number;
  tipo: 'sucesso' | 'erro' | 'aviso' | 'info';
  mensagem: string;
}

interface ToastContextType {
  showToast: (tipo: Toast['tipo'], mensagem: string) => void;
}

const ToastContext = createContext<ToastContextType>({ showToast: () => {} });

export function useToast() {
  return useContext(ToastContext);
}

export function ToastProvider({ children }: { children: React.ReactNode }) {
  const [toasts, setToasts] = useState<Toast[]>([]);

  const showToast = useCallback((tipo: Toast['tipo'], mensagem: string) => {
    const id = Date.now();
    setToasts(prev => [...prev, { id, tipo, mensagem }]);
  }, []);

  useEffect(() => {
    if (toasts.length === 0) return;
    const timer = setTimeout(() => {
      setToasts(prev => prev.slice(1));
    }, 4000);
    return () => clearTimeout(timer);
  }, [toasts]);

  const remover = (id: number) => {
    setToasts(prev => prev.filter(t => t.id !== id));
  };

  return (
    <ToastContext.Provider value={{ showToast }}>
      {children}
      <div className="toast-container">
        {toasts.map(toast => (
          <div key={toast.id} className={`toast toast-${toast.tipo}`} onClick={() => remover(toast.id)}>
            <span className="toast-icon">
              {toast.tipo === 'sucesso' && '✓'}
              {toast.tipo === 'erro' && '✕'}
              {toast.tipo === 'aviso' && '⚠'}
              {toast.tipo === 'info' && 'ℹ'}
            </span>
            <span className="toast-mensagem">{toast.mensagem}</span>
          </div>
        ))}
      </div>
    </ToastContext.Provider>
  );
}
