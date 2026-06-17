# Laboratorio 3: Seguridad y Autenticacion en ASP.NET Core MVC

Este repositorio contiene la implementacion tecnica del Laboratorio 3, enfocado en la aplicacion de Arquitectura RBAC (Control de Acceso Basado en Roles) utilizando ASP.NET Core Identity, Entity Framework Core y PostgreSQL.

## Tecnologias Utilizadas
* Backend: C# / .NET 8 (ASP.NET Core MVC)
* ORM: Entity Framework Core
* Base de Datos: PostgreSQL (con Npgsql)
* Seguridad: ASP.NET Core Identity
* Autenticacion Externa: OAuth 2.0 / OpenID Connect (Google Login)

## Caracteristicas Principales
1. Autenticacion Local y Federada: Los usuarios pueden iniciar sesion con credenciales almacenadas localmente (hash PBKDF2) o de forma tercerizada mediante su cuenta de Google.
2. Autorizacion y RBAC: Implementacion del Principio de Minimo Privilegio (PoLP). Rutas y acciones restringidas segun los Claims y perfiles gerenciales:
   * Administrador
   * Supervisor
   * Operador
   * Consulta
3. Renderizado Condicional: Interfaz grafica que muta en base a la autorizacion. Los elementos restringidos se ocultan para perfiles de baja jerarquia para mitigar ataques de enumeracion y descubrimiento.
4. Matriz de Accesos: Panel auditor interno que documenta explicitamente los alcances transaccionales (Listar, Crear, Editar, Eliminar) de cada perfil.
5. Aislamiento Criptografico: ApplicationDbContext para la familia de tablas AspNet segregado estructuralmente de la logica de negocio comercial (SakilaContext).

## Despliegue
La aplicacion esta completamente habilitada y configurada para su empaquetamiento optimizado para entornos como IIS (Internet Information Services) a traves del comando nativo:

dotnet publish -c Release -o ./publish

---
Desarrollado como evidencia de aprendizaje y rigor academico.
