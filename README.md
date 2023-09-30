> ---------------------------------------------------------------------------

# PIM4.4 API
Uma API RestFull para conectar todos os sistemas relacionados ao PIM4.4. Feito usando ASP.NET Core.
> versão: .NET Core 6


## Clonando a solução
### Instale os seguintes pacotes NuGet:
* Microsoft.EntityFrameworkCore
* Microsoft.EntityFrameworkCore.Design
* Microsoft.EntityFrameworkCore.Tools
* Pomelo.EntityFrameworkCore.MySql
* Newtonsoft.Json
* Microsoft.AspNetCore.Authentication.JwtBearer (versão 6.0.*)
* iTextSharp.LGPLv2.Core

> Mude o JWT secretKey, issuer e audience em appsettings.json!

## Integrando a API
> A API usa o sistema de autorização e autenticação com tokens JWT (Json Web Token), e também faz uso de um sistema de refresh token.
1. Caso o banco de dados esteja limpo, é necessário acessar a rota "api/login/startup" para criar a primeira conta (do tipo AnalistaRH).
2. Caso contrário, utilize uma conta do tipo AnalistaRH ou Secretario para criar outra conta de nível infeior.
3. Toda conta é criada com uma senha automática (CPF), por isso é necessário alterar a senha inicial antes de fazer o primeiro login. Utilize a rota "api/login/mudarsenha".
4. Após alterar a senha, faça o login pela rota "api/login" para obter o seu token e o refresh_token e salve-os em cookies ou semelhantes.
5. Atente para o tempo de expiração do token de acesso, que é de 1 hora.
6. Quando seu token expirar, para manter-se logado sem a necessidade de  enviar suas credenciais novamente, utilize a rota "api/login/refresh" para obter um novo token.
