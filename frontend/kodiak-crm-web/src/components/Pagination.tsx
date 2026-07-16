interface PaginationProps {
  pagina: number;
  total: number;
  itensPorPagina: number;
  onMudarPagina: (pagina: number) => void;
}

export default function Pagination({ pagina, total, itensPorPagina, onMudarPagina }: PaginationProps) {
  const totalPaginas = Math.ceil(total / itensPorPagina);

  if (totalPaginas <= 1) return null;

  const paginas: (number | '...')[] = [];
  const maxVisiveis = 5;

  if (totalPaginas <= maxVisiveis + 2) {
    for (let i = 1; i <= totalPaginas; i++) paginas.push(i);
  } else {
    paginas.push(1);
    if (pagina > 3) paginas.push('...');
    const inicio = Math.max(2, pagina - 1);
    const fim = Math.min(totalPaginas - 1, pagina + 1);
    for (let i = inicio; i <= fim; i++) paginas.push(i);
    if (pagina < totalPaginas - 2) paginas.push('...');
    paginas.push(totalPaginas);
  }

  return (
    <div className="pagination">
      <button
        className="pagination-btn"
        disabled={pagina <= 1}
        onClick={() => onMudarPagina(pagina - 1)}
      >
        ‹ Anterior
      </button>

      <div className="pagination-numeros">
        {paginas.map((p, i) =>
          p === '...' ? (
            <span key={`dots-${i}`} className="pagination-dots">…</span>
          ) : (
            <button
              key={p}
              className={`pagination-numero ${p === pagina ? 'active' : ''}`}
              onClick={() => onMudarPagina(p)}
            >
              {p}
            </button>
          )
        )}
      </div>

      <button
        className="pagination-btn"
        disabled={pagina >= totalPaginas}
        onClick={() => onMudarPagina(pagina + 1)}
      >
        Próxima ›
      </button>
    </div>
  );
}
