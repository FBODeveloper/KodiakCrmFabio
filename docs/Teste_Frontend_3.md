#Atividades
 - Após concluída ou cancelada uma atividade está podendo ser alterada e isso não pode acontecer.
 - Ao cadastrar/alterar uma atividade está sendo possível alterar o status e não pode. O Status é sempre Pendente ao cadastrar,
   os status Concluído e Cancelado só podem ser alterados pela lista pelos botões especificos.
 - AO cadastrar/alterar uma atividade os campos de Cliente e Responsável não estão carregando os clientes e responsáveis já cadastrados,
   impossibilitando o vinculo deles na atividade.

#Proposta
 - Gostaria que o botões de Alteração de status ficassem na coluna de acões e não junto a coluna Status. 
 - PRecisamos de mais um botão de alteração de status que é o Pendente para voltarmos pra esse status em alguma situação.
 - APenas proposta com status pendente podem ser alteradas.
 - Coluna cliente/contato na lista não exibindo nada.

#Contato
 - Campo data de cadastro não foi adicionado, vamos precisar dele, inclusive na lista.
 


#Geral
 - nas labels denominada "Razão Social" gostaria que fosse alterado para "Nome/Razão Social" em todo projeto, apenas label não precisa mudar em banco de dados.
 - nas labels denominada "Nome Fantasia" gostaria que fosse alterado para "Apelido/Nome Fantasia" em todo projeto, apenas label não precisa mudar em banco de dados.

#Oportunidade
 - cadastro de oportunidade o campo Funil fica vazio e não tem um local para cadastro de Funil, será um valor fixo ou vamos cadastrar?
 - Campo estágio não está carregando as informações.
 erro ao cadastrar oportunidade informando uma data de PRevisão:
 fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Data.DataException: Error parsing column 8 (data_previsao=28/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialize081fa629-a59a-470c-a502-e58d0f7b76cf(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialize081fa629-a59a-470c-a502-e58d0f7b76cf(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.OportunidadeRepository.ObterListaAsync(String idEmpresa, String busca, Nullable`1 idEstagio, Nullable`1 responsavelId, String status, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\FunilRepository.cs:line 202
         at KodiakCrm.UseCases.Services.OportunidadeService.ObterListaAsync(String idEmpresa, String busca, Nullable`1 idEstagio, Nullable`1 responsavelId, String status, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\FunilService.cs:line 130
         at KodiakCrm.Api.Controllers.OportunidadeController.ObterLista(String busca, Nullable`1 idEstagio, Nullable`1 responsavelId, String status, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\FunilController.cs:line 106
         at lambda_method231(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_previsao=28/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialize081fa629-a59a-470c-a502-e58d0f7b76cf(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialize081fa629-a59a-470c-a502-e58d0f7b76cf(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.OportunidadeRepository.ObterListaAsync(String idEmpresa, String busca, Nullable`1 idEstagio, Nullable`1 responsavelId, String status, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\FunilRepository.cs:line 202
         at KodiakCrm.UseCases.Services.OportunidadeService.ObterListaAsync(String idEmpresa, String busca, Nullable`1 idEstagio, Nullable`1 responsavelId, String status, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\FunilService.cs:line 130
         at KodiakCrm.Api.Controllers.OportunidadeController.ObterLista(String busca, Nullable`1 idEstagio, Nullable`1 responsavelId, String status, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\FunilController.cs:line 106
         at lambda_method231(Closure, Object)
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


