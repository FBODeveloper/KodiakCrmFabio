import { useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axios';

interface SearchResult {
  tipo: string;
  id: number;
  titulo: string;
  subtitulo?: string;
  rota: string;
}

export default function SearchBar() {
  const [query, setQuery] = useState('');
  const [resultados, setResultados] = useState<SearchResult[]>([]);
  const [aberto, setAberto] = useState(false);
  const [carregando, setCarregando] = useState(false);
  const navigate = useNavigate();
  const wrapperRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const handleClickOutside = (e: MouseEvent) => {
      if (wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
        setAberto(false);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  useEffect(() => {
    if (query.length < 2) {
      setResultados([]);
      return;
    }
    const timer = setTimeout(buscar, 300);
    return () => clearTimeout(timer);
  }, [query]);

  const buscar = async () => {
    if (query.length < 2) return;
    setCarregando(true);
    try {
      const results: SearchResult[] = [];

      const [leadsRes, clientesRes, contatosRes] = await Promise.allSettled([
        api.get('/lead', { params: { busca: query, itensPorPagina: 3 } }),
        api.get('/cliente', { params: { busca: query, itensPorPagina: 3 } }),
        api.get('/contato', { params: { busca: query, itensPorPagina: 3 } })
      ]);

      if (leadsRes.status === 'fulfilled') {
        leadsRes.value.data.itens?.forEach((l: any) => {
          results.push({ tipo: 'Lead', id: l.id, titulo: l.nome, subtitulo: l.empresa, rota: `/leads/${l.id}` });
        });
      }
      if (clientesRes.status === 'fulfilled') {
        clientesRes.value.data.itens?.forEach((c: any) => {
          results.push({ tipo: 'Cliente', id: c.id, titulo: c.razaoSocial || c.nomeFantasia, subtitulo: c.cnpjCpf, rota: `/clientes/${c.id}` });
        });
      }
      if (contatosRes.status === 'fulfilled') {
        contatosRes.value.data.itens?.forEach((c: any) => {
          results.push({ tipo: 'Contato', id: c.id, titulo: c.nome, subtitulo: c.cargo, rota: `/contatos/${c.id}` });
        });
      }

      setResultados(results);
    } catch {
      // ignora
    } finally {
      setCarregando(false);
    }
  };

  const selecionar = (rota: string) => {
    navigate(rota);
    setQuery('');
    setAberto(false);
  };

  return (
    <div className="search-bar" ref={wrapperRef}>
      <div className="search-input-wrapper">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>
        <input
          type="text"
          className="search-input"
          placeholder="Buscar leads, clientes, contatos..."
          value={query}
          onChange={e => { setQuery(e.target.value); setAberto(true); }}
          onFocus={() => query.length >= 2 && setAberto(true)}
        />
        {carregando && <span className="search-spinner"></span>}
      </div>

      {aberto && resultados.length > 0 && (
        <div className="search-dropdown">
          {resultados.map((r, i) => (
            <div key={`${r.tipo}-${r.id}-${i}`} className="search-item" onClick={() => selecionar(r.rota)}>
              <span className="search-tipo">{r.tipo}</span>
              <span className="search-titulo">{r.titulo}</span>
              {r.subtitulo && <span className="search-sub">{r.subtitulo}</span>}
            </div>
          ))}
        </div>
      )}

      {aberto && query.length >= 2 && !carregando && resultados.length === 0 && (
        <div className="search-dropdown">
          <div className="search-vazio">Nenhum resultado para "{query}"</div>
        </div>
      )}
    </div>
  );
}
