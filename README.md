# Laboratorio 3: Seguridad y Autenticación en ASP.NET Core MVC

Este repositorio contiene la implementación técnica del **Laboratorio 3**, enfocado en la aplicación de Arquitectura RBAC (Control de Acceso Basado en Roles) utilizando **ASP.NET Core Identity**, **Entity Framework Core** y **PostgreSQL**.

## 🛠 Tecnologías Utilizadas
* **Backend:** C# / .NET 8 (ASP.NET Core MVC)
* **ORM:** Entity Framework Core
* **Base de Datos:** PostgreSQL (con Npgsql)
* **Seguridad:** ASP.NET Core Identity
* **Autenticación Externa:** OAuth 2.0 / OpenID Connect (Google Login)

## 🔐 Características Principales
1. **Autenticación Local y Federada:** Los usuarios pueden iniciar sesión con credenciales almacenadas localmente (hash PBKDF2) o de forma tercerizada mediante su cuenta de Google.
2. **Autorización y RBAC:** Implementación del Principio de Mínimo Privilegio (PoLP). Rutas y acciones restringidas según los Claims y perfiles gerenciales:
   * Administrador
   * Supervisor
   * Operador
   * Consulta
3. **Renderizado Condicional:** Interfaz gráfica que muta en base a la autorización. Los elementos restringidos se ocultan para perfiles de baja jerarquía para mitigar ataques de enumeración y descubrimiento.
4. **Matriz de Accesos:** Panel auditor interno que documenta explícitamente los alcances transaccionales (Listar, Crear, Editar, Eliminar) de cada perfil.
5. **Aislamiento Criptográfico:** `ApplicationDbContext` para la familia de tablas `AspNet` segregado estructuralmente de la lógica de negocio comercial (`SakilaContext`).

## 🚀 Despliegue
La aplicación está completamente habilitada y configurada para su empaquetamiento optimizado para entornos como **IIS (Internet Information Services)** a través del comando nativo:

```bash
dotnet publish -c Release -o ./publish
```

---
*Desarrollado como evidencia de aprendizaje y rigor académico.*
