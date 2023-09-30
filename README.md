> ---------------------------------------------------------------------------

# PIM4.4 API
A RestFull API to connect all PIM4.4 related systems. Made using ASP.NET Core.
> version: .NET 6


## Cloning the solution
### Install NuGet packages:
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.Tools
* Pomelo.EntityFrameworkCore.MySql
* Newtonsoft.Json
* Microsoft.AspNetCore.Authentication.JwtBearer (versão 6.0.*)
* iTextSharp.LGPLv2.Core

> Change Jwt secretKey, issuer and audience on appsettings.json!

## Routes
* GET api/login - Importante salvar o "token", o "refreshToken" e o "id"
* POST api/login/refresh
* POST api/login/startup
* PUT api/login/mudarsenha - NOT IMPLEMENTED YET
\
&nbsp;
* GET api/file/conteudo
\
&nbsp;
* GET api/analistarh
* GET api/analistarh/{id}
* POST api/analistarh
* PUT api/analistarh
* DELETE api/analistarh/{id}
\
&nbsp;
* GET api/secretario
* GET api/secretario/{id}
* POST api/secretario
* PUT api/secretario
* DELETE api/secretario/{id}
\
&nbsp;
* GET api/professor
* GET api/professor/{id}
* POST api/professor
* PUT api/professor
* DELETE api/professor/{id}
\
&nbsp;
* GET api/aluno
* GET api/aluno/{id}
* POST api/aluno
* PUT api/aluno
* DELETE api/aluno/{id}
* GET api/aluno/boletim/{id}
* GET api/aluno/declaracao/{id}
* GET api/aluno/historico/{id}
* GET api/aluno/relatorio/{id}
\
&nbsp;
* GET api/disciplina
* GET api/disciplina/{id}
* POST api/disciplina
* PUT api/disciplina
* DELETE api/disciplina/{id}
\
&nbsp;
* GET api/curso
* GET api/curso/{id}
* POST api/curso
* PUT api/curso
* DELETE api/curso/{id}
* POST api/curso/disciplina
* DELETE api/curso/disciplina
\
&nbsp;
* GET api/turma
* GET api/turma/{id}
* POST api/turma
* PUT api/turma
* DELETE api/turma/{id}
\
&nbsp;
* GET api/disciplinaministrada
* GET api/disciplinaministrada/{id}
* POST api/disciplinaministrada
* PUT api/disciplinaministrada
* DELETE api/disciplinaministrada/{id}
\
&nbsp;
* GET api/cursomatriculado
* GET api/cursomatriculado/{id}
* POST api/cursomatriculado
* PUT api/cursomatriculado
* DELETE api/cursomatriculado/{id}
\
&nbsp;
* GET api/disciplinacursada
* GET api/disciplinacursada/{id}
* POST api/disciplinacursada
* PUT api/disciplinacursada
* DELETE api/disciplinacursada/{id}
* PUT api/disciplinacursada/media/{id}
* PUT api/disciplinacursada/frequencia/{id}
* PUT api/disciplinacursada/situacao/{id}
