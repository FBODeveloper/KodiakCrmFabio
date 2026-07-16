import React from 'react';

interface LoadingButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  carregando?: boolean;
  texto?: string;
  textoCarregando?: string;
}

export default function LoadingButton({
  carregando = false,
  texto,
  textoCarregando = 'Salvando...',
  children,
  disabled,
  className = '',
  ...props
}: LoadingButtonProps) {
  return (
    <button
      className={`btn ${className} ${carregando ? 'btn-loading' : ''}`}
      disabled={disabled || carregando}
      {...props}
    >
      {carregando ? (
        <>
          <span className="btn-spinner"></span>
          {textoCarregando}
        </>
      ) : (
        texto || children
      )}
    </button>
  );
}
