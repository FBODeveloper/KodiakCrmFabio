export interface Empresa {
  cnpj: string;
  razaoSocial: string;
  nomeFantasia?: string;
  telefone?: string;
  email?: string;
  endereco?: string;
  quantidadeUsuariosContratados: number;
  ativo: boolean;
  dataCadastro: string;
  totalUsuarios: number;
}

export interface EmpresaCreate {
  cnpj: string;
  razaoSocial: string;
  nomeFantasia?: string;
  telefone?: string;
  email?: string;
  endereco?: string;
  quantidadeUsuariosContratados: number;
}

export interface EmpresaUpdate {
  razaoSocial: string;
  nomeFantasia?: string;
  telefone?: string;
  email?: string;
  endereco?: string;
  quantidadeUsuariosContratados: number;
}

export interface EmpresaConfig {
  id: number;
  cnpjEmpresa: string;
  tema: string;
  fusoHorario: string;
  moeda: string;
  idioma: string;
  notificacoesEmail: boolean;
  notificacoesSistema: boolean;
}

export interface EmpresaConfigUpdate {
  tema?: string;
  fusoHorario?: string;
  moeda?: string;
  idioma?: string;
  notificacoesEmail?: boolean;
  notificacoesSistema?: boolean;
}

export interface Usuario {
  id: number;
  nome: string;
  email: string;
  idEmpresa: string;
  perfil: string;
  avatar?: string;
  dataNascimento?: string;
  empresa?: Empresa;
}

export interface LoginResponse {
  sucesso: boolean;
  token: string;
  mensagem: string;
  usuario?: Usuario;
}

export interface UsuarioGestao {
  id: number;
  nome: string;
  email: string;
  perfil: string;
  avatar?: string;
  dataNascimento?: string;
  ativo: boolean;
  dataCadastro: string;
}

export interface UsuarioCreate {
  nome: string;
  email: string;
  senha: string;
  perfil: string;
  avatar?: string;
  dataNascimento?: string;
  idEmpresa?: string;
}

export interface UsuarioUpdate {
  nome: string;
  email?: string;
  senha?: string;
  perfil: string;
  avatar?: string;
  dataNascimento?: string;
  ativo?: boolean;
}

export interface Parceiro {
  id: number;
  razaoSocial: string;
  nomeFantasia?: string;
  cpfCnpj?: string;
  tipoPessoa?: string;
  email?: string;
  telefone?: string;
  celular?: string;
  idParceiroKodiakErp?: number;
  observacao?: string;
  ativo: boolean;
  dataCadastro: string;
  idEmpresa: string;
}

export interface ParceiroCreate {
  razaoSocial: string;
  nomeFantasia?: string;
  cpfCnpj?: string;
  tipoPessoa?: string;
  email?: string;
  telefone?: string;
  celular?: string;
  observacao?: string;
}

export interface Lead {
  id: number;
  nome: string;
  empresa?: string;
  email?: string;
  telefone?: string;
  source?: string;
  status: string;
  temperatura: string;
  idEstagio?: number;
  estagioNome?: string;
  idParceiro?: number;
  observacao?: string;
  responsavelId?: number;
  responsavelNome?: string;
  responsavelAvatar?: string;
  ativo: boolean;
  dataCadastro: string;
  idEmpresa: string;
}

export interface LeadCreate {
  nome: string;
  empresa?: string;
  email?: string;
  telefone?: string;
  source?: string;
  temperatura?: string;
  idEstagio?: number;
  responsavelId?: number;
  observacao?: string;
}

export interface LeadEstagio {
  id: number;
  nome: string;
  ordem: number;
  cor: string;
}

export interface LeadKanban {
  estagios: LeadEstagio[];
  colunas: LeadKanbanColuna[];
}

export interface LeadKanbanColuna {
  estagioId: number;
  estagioNome: string;
  ordem: number;
  cor: string;
  leads: Lead[];
}

export interface Funil {
  id: number;
  nome: string;
  ativo: boolean;
  estagios: FunilEstagio[];
}

export interface FunilEstagio {
  id: number;
  nome: string;
  ordem: number;
  probabilidade: number;
}

export interface Oportunidade {
  id: number;
  titulo: string;
  idParceiro?: number;
  parceiroNome?: string;
  idEstagio?: number;
  estagioNome?: string;
  funilId?: number;
  funilNome?: string;
  valor?: number;
  dataPrevisao?: string;
  responsavelId?: number;
  responsavelNome?: string;
  observacao?: string;
  motivoPerda?: string;
  ativo: boolean;
  dataCadastro: string;
  idEmpresa: string;
}

export interface Kanban {
  funilId: number;
  funilNome: string;
  colunas: KanbanColuna[];
}

export interface KanbanColuna {
  estagioId: number;
  estagioNome: string;
  ordem: number;
  oportunidades: Oportunidade[];
}

export interface Atividade {
  id: number;
  tipo: string;
  titulo: string;
  descricao?: string;
  idParceiro?: number;
  parceiroNome?: string;
  idOportunidade?: number;
  oportunidadeTitulo?: string;
  responsavelId?: number;
  responsavelNome?: string;
  clienteId?: number;
  clienteNome?: string;
  status: string;
  dataInicio?: string;
  dataFim?: string;
  concluida: boolean;
  ativo: boolean;
  dataCadastro: string;
  idEmpresa: string;
}

export interface Proposta {
  id: number;
  titulo: string;
  numero?: string;
  dataProposta?: string;
  formaPagamento?: string;
  prazoEntrega?: string;
  idParceiro?: number;
  parceiroNome?: string;
  idOportunidade?: number;
  oportunidadeTitulo?: string;
  clienteId?: number;
  clienteNome?: string;
  contatoId?: number;
  contatoNome?: string;
  valorTotal?: number;
  dataValidade?: string;
  status: string;
  observacao?: string;
  itens: PropostaItem[];
  ativo: boolean;
  dataCadastro: string;
  idEmpresa: string;
}

export interface PropostaItem {
  id: number;
  descricao: string;
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
}

export interface DashboardResumo {
  totalParceiros: number;
  totalLeads: number;
  leadsNovos: number;
  totalOportunidades: number;
  valorFunil: number;
  atividadesPendentes: number;
  propostasEnviadas: number;
}

export interface LeadStats {
  total: number;
  novos: number;
  taxaConversao: number;
  followupPendente: number;
}

export interface DashboardLeadRecente {
  id: number;
  nome: string;
  empresa?: string;
  telefone?: string;
  status: string;
  dataCadastro: string;
}

export interface DashboardLeadsPorEstagio {
  estagioNome: string;
  quantidade: number;
  cor: string;
}

export interface DashboardTicketMedio {
  ticketMedio: number;
  totalComValor: number;
}

export interface DashboardConversao {
  totalLeads: number;
  leadsConvertidos: number;
  taxaConversao: number;
  oportunidadesGanhas: number;
  oportunidadesPerdidas: number;
  taxaSucesso: number;
}

export interface DashboardProdutividade {
  usuarioId: number;
  usuarioNome: string;
  totalOportunidades: number;
  oportunidadesGanhas: number;
  valorTotal: number;
}

export interface Historico {
  id: number;
  entidade: string;
  entidadeId: number;
  acao: string;
  descricao: string;
  dadosAntes?: string;
  dadosDepois?: string;
  usuarioId?: number;
  usuarioNome?: string;
  dataAcao: string;
}

export interface LeadConverterResponse {
  leadId: number;
  leadNome: string;
  oportunidadeId: number;
  oportunidadeTitulo: string;
}

export interface ClienteDTO {
  id: number;
  razaoSocial: string;
  nomeFantasia?: string;
  cnpjCpf?: string;
  email?: string;
  telefone?: string;
  celular?: string;
  endereco?: string;
  observacao?: string;
  origem?: string;
  dataConversao?: string;
  idOportunidade?: number;
  responsavelId?: number;
  responsavelNome?: string;
  ativo: boolean;
  dataCadastro: string;
  idEmpresa: string;
}

export interface ContatoDTO {
  id: number;
  nome: string;
  cargo?: string;
  email?: string;
  telefone?: string;
  celular?: string;
  idCliente?: number;
  clienteNome?: string;
  idParceiro?: number;
  parceiroNome?: string;
  observacao?: string;
  responsavelId?: number;
  responsavelNome?: string;
  ativo: boolean;
  dataCadastro: string;
  idEmpresa: string;
}

export interface PaginatedResponse<T> {
  itens: T[];
  total: number;
  pagina: number;
  itensPorPagina: number;
}

export interface RelatorioFiltro {
  dataInicio?: string;
  dataFim?: string;
  status?: string;
  responsavelId?: number;
  tipoAtividade?: string;
}

export interface RelatorioVendas {
  totalOportunidades: number;
  oportunidadesGanhas: number;
  oportunidadesPerdidas: number;
  oportunidadesAbertas: number;
  valorTotal: number;
  valorGanho: number;
  valorPerdido: number;
  ticketMedio: number;
  taxaConversao: number;
  porPeriodo: { periodo: string; quantidade: number; valor: number }[];
  porResponsavel: { responsavelNome: string; total: number; ganhas: number; valorTotal: number }[];
}

export interface RelatorioAtividades {
  totalAtividades: number;
  concluidas: number;
  pendentes: number;
  taxaConclusao: number;
  porTipo: { tipo: string; quantidade: number; concluidas: number }[];
  porResponsavel: { responsavelNome: string; total: number; concluidas: number }[];
}

export interface RelatorioPerformance {
  vendedores: {
    vendedorNome: string;
    totalLeads: number;
    totalOportunidades: number;
    oportunidadesGanhas: number;
    valorTotal: number;
    taxaConversao: number;
    totalAtividades: number;
  }[];
}

export interface Notificacao {
  id: number;
  titulo: string;
  mensagem: string;
  tipo: string;
  entidade?: string;
  entidadeId?: number;
  lida: boolean;
  dataCriacao: string;
}

export interface NotificacaoResumo {
  totalNaoLidas: number;
  recentes: Notificacao[];
}
