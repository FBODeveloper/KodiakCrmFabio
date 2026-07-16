export interface FiltroConfig {
  campo: string;
  label: string;
  tipo: 'texto' | 'select' | 'data';
  opcoes?: { valor: string; label: string }[];
  placeholder?: string;
}

interface FilterBarProps {
  filtros: FiltroConfig[];
  valores: Record<string, string>;
  onMudar: (campo: string, valor: string) => void;
  onLimpar: () => void;
}

export default function FilterBar({ filtros, valores, onMudar, onLimpar }: FilterBarProps) {
  const temFiltro = Object.values(valores).some(v => v !== '');

  return (
    <div className="filter-bar">
      {filtros.map(filtro => (
        <div key={filtro.campo} className="filter-item">
          <label className="filter-label">{filtro.label}</label>
          {filtro.tipo === 'texto' && (
            <input
              type="text"
              className="filter-input"
              placeholder={filtro.placeholder || filtro.label}
              value={valores[filtro.campo] || ''}
              onChange={e => onMudar(filtro.campo, e.target.value)}
            />
          )}
          {filtro.tipo === 'select' && (
            <select
              className="filter-select"
              value={valores[filtro.campo] || ''}
              onChange={e => onMudar(filtro.campo, e.target.value)}
            >
              <option value="">Todos</option>
              {filtro.opcoes?.map(op => (
                <option key={op.valor} value={op.valor}>{op.label}</option>
              ))}
            </select>
          )}
          {filtro.tipo === 'data' && (
            <input
              type="date"
              className="filter-input"
              value={valores[filtro.campo] || ''}
              onChange={e => onMudar(filtro.campo, e.target.value)}
            />
          )}
        </div>
      ))}
      {temFiltro && (
        <button className="filter-limpar" onClick={onLimpar}>Limpar filtros</button>
      )}
    </div>
  );
}
