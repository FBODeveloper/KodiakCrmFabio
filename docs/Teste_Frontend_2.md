#Contatos
 - na lista ainda tem uma coluna "parceiro" eliminar.
 - Ver ainda permite salvar, deveria só ver.
 - Criar um campo data do contato que deve ser salvo com a data que o contato é cadastrado no sistema, essa campo não pode ser alterado.

#Leads
 - Na lista tem uma coluna Status que exibe o Estágio e a coluna Estágio não exibe nada, gostaria que ficasse apenas a coluna EStágio.
 - Filtro também está como Status e não como Estágio.

#Clientes
 - Ver ainda permite salvar, deve permitir só ver.
 - Criar um campo data_cadastro que deve ser salvo a data que o client foi cadastrado e não pode ser alterado.
 - FIltro de clientes tem data pode retirar.

#Atividades
 - Lista tem a coluna parceiro, trocar para cliente.
 - Incluir cliente no cadastro da atividade (não obrigatório).
 - Incluir o responsável no cadastro da atividade (Admin e Gerente podem informar o responsável), usuário comum ele mesmo é o responsavel.
 - Incluir um campo de Status no cadastro da Atividade
   Vão Existir os status (Pendente, Concluído, Cancelado), ao cadastrar sem é pendente.
 - Criar um mecanismos para mudar o status da Atividade e marcar ela como COncluída ou Cancelada, esses status só podem ser mudado se o status atual for pendente.
 - A opção de marcar como concluída ou cancelada pode ser incluída junto com os botões (ver, editar, exluir).

#Proposta
 - Na lista tem o campo Status, manter.
 - Criar na lista mecanismos para setar o status da proposta para: Enviada, Aprovada, Rejeitada.
 - QUando for rejeitada criar uma função para o usuário informar o motivo da rejeição.
 - Ao alterar uma proposta que não tem número, gerar a numeração no padrão adotado.
 - Mesmo vinculando o cliente na proposta ele não é exibido na lista.
 - Na Lista trocar o nome da coluna cliente para Cliente/Contato e exibir nela o cliente caso exista ou o contato
   caso exista (Prioriadade ao cliente).
 - Incluir na lista a data da proposta.
 - Padronize os botões de editar, excluir para padrão (Ver, Editar, Excluir) apenas imagem igual ao lead.

#Oportunidades
 - Padronize os botões de editar, excluir para padrão (Ver, Editar, Excluir) apenas imagem igual ao lead.

#Histórico
 - Um ponto a ser implantado, toda alteração de status (de qualquer coisa que tenha status sendo proposta, atividade) deve ser registrado 
   no historico data e hora, status anterior e status atual e o responsável (usuário) pela alteração.

#Atenção geral
 - Todo botão (Ver) deve exibir o registro em questão mas não pode permitir altera-lo, ou seja, os campos devem estar bloqueados e o botão salvar enable.

#Usuários
 - Ao cadastro usuário dá erro mas não indica o erro, check por tudo, principalmente o campo data de nascimento que é onde sempre está com problemas.
 - Após o erro não exibe mais a lista e não permite mais nada sobre usuários.


#Erros na api durante o teste.
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      Npgsql.PostgresException (0x80004005): 42804: coluna "parametros" é do tipo jsonb mas expressão é do tipo text
      
      POSITION: 169
         at Npgsql.Internal.NpgsqlConnector.ReadMessageLong(Boolean async, DataRowLoadingMode dataRowLoadingMode, Boolean readingNotifications, Boolean isReadingPrependedMessage)
         at System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder`1.StateMachineBox`1.System.Threading.Tasks.Sources.IValueTaskSource<TResult>.GetResult(Int16 token)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
         at Dapper.SqlMapper.QueryRowAsync[T](IDbConnection cnn, Row row, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 489
         at KodiakCrm.Infrastructure.Repositories.RelatorioRepository.CriarAsync(RelatorioGerado relatorio) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\RelatorioRepository.cs:line 25
         at KodiakCrm.UseCases.Services.RelatorioService.SalvarRelatorioAsync(String idEmpresa, String cnpjEmpresa, String tipo, String titulo, RelatorioFiltroDTO filtro, Object resultado, Nullable`1 usuarioId, String usuarioNome) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\RelatorioService.cs:line 173
         at KodiakCrm.UseCases.Services.RelatorioService.GerarRelatorioVendasAsync(String idEmpresa, String cnpjEmpresa, RelatorioFiltroDTO filtro, Nullable`1 usuarioId, String usuarioNome) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\RelatorioService.cs:line 65
         at KodiakCrm.Api.Controllers.RelatorioController.RelatorioVendas(Nullable`1 dataInicio, Nullable`1 dataFim, String status, Nullable`1 responsavelId) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\RelatorioController.cs:line 35
         at lambda_method279(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
        Exception data:
          Severity: ERRO
          SqlState: 42804
          MessageText: coluna "parametros" é do tipo jsonb mas expressão é do tipo text
          Hint: Você precisará reescrever ou converter a expressão.
          Position: 169
          File: parse_target.c
          Line: 596
          Routine: transformAssignedExpr
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      Npgsql.PostgresException (0x80004005): 42804: coluna "parametros" é do tipo jsonb mas expressão é do tipo text
      
      POSITION: 169
         at Npgsql.Internal.NpgsqlConnector.ReadMessageLong(Boolean async, DataRowLoadingMode dataRowLoadingMode, Boolean readingNotifications, Boolean isReadingPrependedMessage)
         at System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder`1.StateMachineBox`1.System.Threading.Tasks.Sources.IValueTaskSource<TResult>.GetResult(Int16 token)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
         at Dapper.SqlMapper.QueryRowAsync[T](IDbConnection cnn, Row row, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 489
         at KodiakCrm.Infrastructure.Repositories.RelatorioRepository.CriarAsync(RelatorioGerado relatorio) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\RelatorioRepository.cs:line 25
         at KodiakCrm.UseCases.Services.RelatorioService.SalvarRelatorioAsync(String idEmpresa, String cnpjEmpresa, String tipo, String titulo, RelatorioFiltroDTO filtro, Object resultado, Nullable`1 usuarioId, String usuarioNome) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\RelatorioService.cs:line 173
         at KodiakCrm.UseCases.Services.RelatorioService.GerarRelatorioAtividadesAsync(String idEmpresa, String cnpjEmpresa, RelatorioFiltroDTO filtro, Nullable`1 usuarioId, String usuarioNome) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\RelatorioService.cs:line 105
         at KodiakCrm.Api.Controllers.RelatorioController.RelatorioAtividades(Nullable`1 dataInicio, Nullable`1 dataFim, String tipo, Nullable`1 responsavelId) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\RelatorioController.cs:line 47
         at lambda_method287(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
        Exception data:
          Severity: ERRO
          SqlState: 42804
          MessageText: coluna "parametros" é do tipo jsonb mas expressão é do tipo text
          Hint: Você precisará reescrever ou converter a expressão.
          Position: 169
          File: parse_target.c
          Line: 596
          Routine: transformAssignedExpr
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      Npgsql.PostgresException (0x80004005): 42804: coluna "parametros" é do tipo jsonb mas expressão é do tipo text
      
      POSITION: 169
         at Npgsql.Internal.NpgsqlConnector.ReadMessageLong(Boolean async, DataRowLoadingMode dataRowLoadingMode, Boolean readingNotifications, Boolean isReadingPrependedMessage)
         at System.Runtime.CompilerServices.PoolingAsyncValueTaskMethodBuilder`1.StateMachineBox`1.System.Threading.Tasks.Sources.IValueTaskSource<TResult>.GetResult(Int16 token)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlDataReader.NextResult(Boolean async, Boolean isConsuming, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteReader(Boolean async, CommandBehavior behavior, CancellationToken cancellationToken)
         at Npgsql.NpgsqlCommand.ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
         at Dapper.SqlMapper.QueryRowAsync[T](IDbConnection cnn, Row row, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 489
         at KodiakCrm.Infrastructure.Repositories.RelatorioRepository.CriarAsync(RelatorioGerado relatorio) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\RelatorioRepository.cs:line 25
         at KodiakCrm.UseCases.Services.RelatorioService.SalvarRelatorioAsync(String idEmpresa, String cnpjEmpresa, String tipo, String titulo, RelatorioFiltroDTO filtro, Object resultado, Nullable`1 usuarioId, String usuarioNome) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\RelatorioService.cs:line 173
         at KodiakCrm.UseCases.Services.RelatorioService.GerarRelatorioPerformanceAsync(String idEmpresa, String cnpjEmpresa, RelatorioFiltroDTO filtro, Nullable`1 usuarioId, String usuarioNome) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\RelatorioService.cs:line 136
         at KodiakCrm.Api.Controllers.RelatorioController.RelatorioPerformance(Nullable`1 dataInicio, Nullable`1 dataFim, Nullable`1 responsavelId) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\RelatorioController.cs:line 59
         at lambda_method295(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
        Exception data:
          Severity: ERRO
          SqlState: 42804
          MessageText: coluna "parametros" é do tipo jsonb mas expressão é do tipo text
          Hint: Você precisará reescrever ou converter a expressão.
          Position: 169
          File: parse_target.c
          Line: 596
          Routine: transformAssignedExpr
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Data.DataException: Error parsing column 11 (data_nascimento=16/07/1988 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         at Dapper.SqlMapper.QueryRowAsync[T](IDbConnection cnn, Row row, Type effectiveType, CommandDefinition command)
         at KodiakCrm.Infrastructure.Repositories.UsuarioRepository.ObterPorIdAsync(Int32 id, String idEmpresa) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\UsuarioRepository.cs:line 39
         at KodiakCrm.UseCases.Services.UsuarioGestaoService.CriarAsync(String idEmpresa, UsuarioCreateDTO dto) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\UsuarioGestaoService.cs:line 74
         at KodiakCrm.Api.Controllers.UsuarioGestaoController.Criar(UsuarioCreateDTO dto) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\UsuarioGestaoController.cs:line 86
         at lambda_method311(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.TaskOfActionResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Data.DataException: Error parsing column 11 (data_nascimento=16/07/1988 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.UsuarioRepository.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\UsuarioRepository.cs:line 91
         at KodiakCrm.UseCases.Services.UsuarioGestaoService.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\UsuarioGestaoService.cs:line 28
         at KodiakCrm.Api.Controllers.UsuarioGestaoController.ObterLista(String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\UsuarioGestaoController.cs:line 35
         at lambda_method106(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Data.DataException: Error parsing column 11 (data_nascimento=16/07/1988 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.UsuarioRepository.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\UsuarioRepository.cs:line 91
         at KodiakCrm.UseCases.Services.UsuarioGestaoService.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\UsuarioGestaoService.cs:line 28
         at KodiakCrm.Api.Controllers.UsuarioGestaoController.ObterLista(String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\UsuarioGestaoController.cs:line 35
         at lambda_method106(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Data.DataException: Error parsing column 11 (data_nascimento=16/07/1988 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.UsuarioRepository.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\UsuarioRepository.cs:line 91
         at KodiakCrm.UseCases.Services.UsuarioGestaoService.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\UsuarioGestaoService.cs:line 28
         at KodiakCrm.Api.Controllers.UsuarioGestaoController.ObterLista(String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\UsuarioGestaoController.cs:line 35
         at lambda_method106(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Data.DataException: Error parsing column 11 (data_nascimento=16/07/1988 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserializef0a69b07-c6da-4d8e-8d17-29979afa70e5(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.UsuarioRepository.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\UsuarioRepository.cs:line 91
         at KodiakCrm.UseCases.Services.UsuarioGestaoService.ObterListaAsync(String idEmpresa, String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\UsuarioGestaoService.cs:line 28
         at KodiakCrm.Api.Controllers.UsuarioGestaoController.ObterLista(String busca, String perfil, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\UsuarioGestaoController.cs:line 35
         at lambda_method106(Closure, Object)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
         at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
         at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
         at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
         at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
         at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)

