#PArceiros
 ainda aparece no dashboard... remover o card do dash e remodelar o dash pra preencher o espaço.

#Leads
- na lista, tem o botão de ... com as opções Ver, Editar e Excluir, Ver permite alterar e não deveria.
  Esse mesmo botão de ... quando o lead é o último do grid o menu aparece dentro do grid ficando com a visão oculta, gostaria que ao invés
  do botão ... fosse alterado para 3 botões, um sendo um olho (ver), um de edit e um de delete isso facilita a ação do usuário.
- No cadastro de um Lead o campo de responsável já pode vir com o usuário atual e se for Admin ou Gerente pode modificar.
- Tanto o Kankan com quando a lista parecem ter um lmitado na tela o ideal é o seguinte...
  Na lista exibir 50 registros e depois mostrar paginação para os próximos registros.....
  No Kanban não há limitação de tela... a coluna tem que crescer conforme a quantidade de cards dentro da coluna, sem limites, sem paginação.
  No Kanban também precisa ter o botão ver, editar e excluir (apenas imagem igual solicitado a alteração na lista).
  No Kankan mostra o avatar do responsável, seria possível exibir o nome quando passar o mouse por cima do avatar?

#Clientes
 - Seguir com a mesma lógica de botões (Ver, Editar, Excluir) solicitado no lead.

#Contato
 - Seguir com a mesma lógica de botões (Ver, Editar, Excluir) solicitado no lead.

#Atividades
 - Seguir com a mesma lógica de botões (Ver, Editar, Excluir) solicitado no lead.
 - Existe um tipo Followup que foi criado automaticamente por vocÊ em testes acredito, porém, não tem na tela. De onde vem esse tipo?

#Proposta
 - Não salva.
 - Incluir um campo de número da proposta que:
   Numero da proposta deve ser um número inteiro sequencial limitado ao ano da data da proposta, ou seja, todo ano zera esse número, para a 
   empresa, na impressão da proposta saírá assim:  Proposta N.º 1002/2026.
 - Incluir uma campo de data da proposta, além da data de validade que já existe.
 - Incluir um campo de texto, tipo observação, com nome Forma de Pagamento. Vamos usar esse campo para detalhar as formas de pagamentos
   possíveis e depois isso sairá no relatório de proposta.
 - Incluir um campo chamado prazo de entrega (texto também) não obrigatório.
 - O campo de observação que já existe deve ser movido para após os itens da proposta, fica um layout mais clean.
 - Na lista tem a coluna parceiro mas no cadastro não tem parceiro, porém, retiramos o parceiro e optamos por cliente, então acredito
  que uma proposta deve ser vinculado a um cliente e/ou contato, hoje ela está vaga sem um direcionamento. E porque um cliente ou contato, 
  podemos estar enviar uma proposta a um contato novo que está em negociação que aceitando vira um cliente e também podemos usar as proposta
  para um cliente já existe e enviando um proposta de um novo serviço ou produto.

#Histórico
 - não exibe nada, não entendi o que ele teria que exibir aqui.


#Filtros
 - Incluir CLiente/Parceiro e Responsável sempre que possível (se a função existir esses campos para filtro).




#Erros na api durante o teste do front
 -  abaixo os erros exibidos no console da api quando estavamos testando o front, se reparar parece que a maioria dos erros se trata de campos de data que devem estar sendo passado errado.


Usando as configurações de inicialização de KodiakCrm.Api\Properties\launchSettings.json...
Compilando...
D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Data\DapperConfig.cs(36,20): warning CS8603: Possível retorno de referência nula.
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5279
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api
fail: Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddleware[1]
      An unhandled exception has occurred while executing the request.
      System.Data.DataException: Error parsing column 8 (data_validade=21/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=21/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
      System.Data.DataException: Error parsing column 8 (data_validade=31/07/2026 - DateOnly)
       ---> System.InvalidCastException: Unable to cast object of type 'System.DateOnly' to type 'System.Nullable`1[System.DateTime]'.
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         --- End of inner exception stack trace ---
         at Dapper.SqlMapper.ThrowDataException(Exception ex, Int32 index, IDataReader reader, Object value) in /_/Dapper/SqlMapper.cs:line 4001
         at Deserialized8ff1eb3-effe-4397-bef4-17f7c57b0a7c(DbDataReader)
         at Dapper.SqlMapper.QueryAsync[T](IDbConnection cnn, Type effectiveType, CommandDefinition command) in /_/Dapper/SqlMapper.Async.cs:line 454
         at KodiakCrm.Infrastructure.Repositories.PropostaRepository.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Infrastructure\Repositories\PropostaRepository.cs:line 87
         at KodiakCrm.UseCases.Services.PropostaService.ObterListaAsync(String idEmpresa, String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.UseCases\Services\PropostaService.cs:line 27
         at KodiakCrm.Api.Controllers.PropostaController.ObterLista(String busca, String status, Nullable`1 idParceiro, Nullable`1 dataInicio, Nullable`1 dataFim, Int32 pagina, Int32 itensPorPagina) in D:\Dev\OpenCodeOllama\Teste1\kodiak-crm\backend\KodiakCrm.Api\Controllers\PropostaController.cs:line 36
         at lambda_method134(Closure, Object)
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
