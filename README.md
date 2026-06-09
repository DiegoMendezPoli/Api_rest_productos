# 📘 Guía Completa: Crear Contenedores Docker para API REST

## 📋 Índice
1. [Prerequisitos y Herramientas](#prerequisitos)
2. [Instalación de Herramientas](#instalación)
3. [Estructura del Proyecto](#estructura)
4. [Proceso Paso a Paso](#proceso)
5. [Validación y Testing](#validación)
6. [Troubleshooting](#troubleshooting)

---

## 🛠️ Prerequisitos

El proyecto requiere estas herramientas instaladas:

| Herramienta | Versión Mínima | Propósito |
|-------------|-----------------|----------|
| **Docker Desktop** | 4.0+ | Motor de contenedores |
| **.NET SDK** | 10.0+ | Compilar código C# |
| **Docker Compose** | 2.0+ (incluido en Docker Desktop) | Orquestar múltiples contenedores |
| **PowerShell** | 7.0+ o Windows PowerShell 5.1 | Terminal para ejecutar comandos |
| **Git** | 2.30+ (opcional) | Control de versiones |

---

## 🔧 Instalación de Herramientas

### **1. Docker Desktop (Windows)**

#### Descarga e Instalación:
1. Descarga desde: https://www.docker.com/products/docker-desktop
2. Ejecuta el instalador `.exe`
3. Acepta los términos y elige configuración por defecto
4. **Reinicia tu PC** cuando termina la instalación

#### Verificar Instalación:
```powershell
docker --version
docker run hello-world
```

**Resultado esperado:**
```
Docker version 27.x.x, build xxxxxxx
Hello from Docker!
```

#### Habilitar WSL 2 (Windows Subsystem for Linux):
Docker Desktop necesita **WSL 2** para mejor rendimiento:

```powershell
# Ejecutar como Administrador
wsl --install
# Reinicia PC
wsl --set-default-version 2
```

---

### **2. .NET SDK 10.0 (o versión adecuada)**

#### Verificar versión instalada:
```powershell
dotnet --version
```

#### Descargar e Instalar:
1. Ve a: https://dotnet.microsoft.com/download
2. Descarga **.NET SDK 10.0** (o la versión de tu proyecto)
3. Ejecuta el instalador
4. **Reinicia PowerShell**

#### Verificar instalación:
```powershell
dotnet --list-sdks
dotnet new console -n TestApp
```

---

### **3. Git (Opcional pero recomendado)**

```powershell
# Descarga desde: https://git-scm.com/download/win
# Usa configuración por defecto

# Verificar:
git --version
```

---

### **4. PowerShell 7 (Windows PowerShell 5.1 también funciona)**

Windows PowerShell 5.1 viene incluido. Si quieres PowerShell 7:

```powershell
# Instalarlo desde Microsoft Store o
winget install Microsoft.PowerShell
```

---

## 📁 Estructura del Proyecto

Tu proyecto debe tener esta estructura:

```
Api_rest_productos/
├── docker-compose.yml          ← Orquestación de contenedores
├── Dockerfile                  ← Instrucciones para construir imagen
├── TaskManagerApi/
│   ├── TaskManagerApi.csproj   ← Configuración .NET
│   ├── Program.cs              ← Entrada de la aplicación
│   ├── appsettings.json        ← Config producción
│   ├── appsettings.Development.json
│   ├── Controllers/
│   ├── Models/
│   ├── DTOs/
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── Migrations/
│   └── Services/
├── scripts/
│   ├── init.sql
│   └── seed_products.sql
└── Api_rest_productos.slnx     ← Solución Visual Studio
```

---

## 🚀 Proceso Paso a Paso

### **Paso 1: Verificar Configuraciones Críticas**

#### A. Verificar `docker-compose.yml`:
```yaml
# ✅ Debe tener:
services:
  db:
    image: postgres:16          # ← BD correcta
    environment:
      POSTGRES_PASSWORD: postgres
    ports:
      - "5432:5432"             # ← Puerto expuesto
      
  api:
    build:
      dockerfile: Dockerfile     # ← Ruta correcta
    ports:
      - "8080:8080"             # ← Puerto API
```

#### B. Verificar `Dockerfile`:
```dockerfile
# ✅ Versión SDK debe coincidir con proyecto
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

# ✅ Port correcto
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
```

#### C. Verificar `TaskManagerApi.csproj`:
```xml
<!-- ✅ Target framework debe ser >= 9.0 -->
<TargetFramework>net10.0</TargetFramework>
```

---

### **Paso 2: Preparar el Entorno**

Abre **PowerShell como Administrador**:

```powershell
# Navega a la carpeta del proyecto
cd C:\desarollo\Api_rest_productos

# Verifica que estás en la carpeta correcta
ls

# Resultado esperado:
# docker-compose.yml, Dockerfile, TaskManagerApi/, etc.
```

---

### **Paso 3: Limpiar Contenedores Anteriores (si aplica)**

```powershell
# Detener todos los contenedores relacionados
docker-compose down -v

# Limpiar imágenes antiguas (opcional)
docker system prune -a
```

---

### **Paso 4: Construir y Levantar Contenedores**

```powershell
# Opción A: Construir y levantar en background
docker-compose up -d

# Opción B: Construir sin caché (si tienen problemas)
docker-compose up -d --build --no-cache

# Opción C: Levantar con logs en vivo (para debugging)
docker-compose up
```

#### ⏱️ **Tiempo estimado:** 3-5 minutos (primera vez)

---

### **Paso 5: Verificar que todo está corriendo**

```powershell
# Ver estado de contenedores
docker-compose ps

# Resultado esperado:
# NAME                 STATUS
# taskmanager_postgres Up (healthy)
# taskmanager_api      Up

# Ver logs de la API
docker-compose logs api

# Ver logs de BD
docker-compose logs db
```

---

## ✅ Validación y Testing

### **Test 1: Verificar conectividad a BD**

```powershell
# Conectar a PostgreSQL dentro del contenedor
docker exec -it taskmanager_postgres psql -U postgres -d taskmanager_db -c "SELECT 1;"

# Resultado esperado:
# ?column?
# --------
# 1
```

### **Test 2: Verificar que la API está lista**

```powershell
# Esperar 30 segundos y luego probar
Start-Sleep -Seconds 30

# Test 1: Documentación OpenAPI
Invoke-WebRequest -Uri "http://localhost:8080/swagger" -ErrorAction SilentlyContinue

# Test 2: Scalar UI
Invoke-WebRequest -Uri "http://localhost:8080/scalar/v1"
```

### **Test 3: Verificar endpoint health (si existe)**

```powershell
Invoke-WebRequest -Uri "http://localhost:8080/api/health" -Method Get
```

### **Test 4: Listar productos (GET)**

```powershell
Invoke-WebRequest -Uri "http://localhost:8080/api/products" -Method Get | Select-Object -ExpandProperty Content
```

### **Test 5: Crear producto (POST)**

```powershell
$headers = @{"Content-Type" = "application/json"}
$body = @{
    nombre = "Laptop"
    precio = 1299.99
    stock = 5
} | ConvertTo-Json

$response = Invoke-WebRequest -Uri "http://localhost:8080/api/products" `
  -Method Post `
  -Headers $headers `
  -Body $body

$response.Content | ConvertFrom-Json
```

---

## 📊 Ver Datos desde Base de Datos

```powershell
# Conectar a PostgreSQL
docker exec -it taskmanager_postgres psql -U postgres -d taskmanager_db

# Dentro de psql:
SELECT * FROM "Products";
\q  # Para salir
```

---

## 🛑 Comandos Útiles

```powershell
# Detener los contenedores sin remover
docker-compose stop

# Reiniciar los contenedores
docker-compose restart

# Detener y remover todo (incluyendo volúmenes de datos)
docker-compose down -v

# Ver logs en tiempo real
docker-compose logs -f

# Ver fuerte consola de la API
docker-compose logs -f api

# Ejecutar comando dentro del contenedor API
docker exec -it taskmanager_api dotnet --version

# Remover solo imágenes
docker rmi taskmanager_api
```

---

## 🐛 Troubleshooting

### **Error 1: "Docker daemon is not running"**

```powershell
# Solución: Abre Docker Desktop
# O desde PowerShell:
wsl --install
```

### **Error 2: "failed to solve with frontend dockerfile.v0"**

```powershell
# Problema: .NET SDK version mismatch
# Solución: Verifica tu .csproj y actualiza Dockerfile

dotnet --version  # Ver tu versión actual

# Luego actualiza Dockerfile:
# FROM mcr.microsoft.com/dotnet/sdk:12.0  # Cambiar versión
```

### **Error 3: "Connection refused" a la BD**

```powershell
# Problema: PostgreSQL aún no está listo

# Solución: Esperar más tiempo
Start-Sleep -Seconds 60

# O ver logs de la BD
docker-compose logs db

# Reiniciar BD
docker-compose restart db
```

### **Error 4: "Port 5432 already in use"**

```powershell
# Problema: Otro PostgreSQL está en el puerto

# Solución A: Liberar el puerto
netstat -ano | findstr :5432
taskkill /PID <PID> /F

# Solución B: Cambiar puerto en docker-compose.yml
# ports:
#   - "5433:5432"  # Cambiar a 5433
```

### **Error 5: "No migrations exist"**

```powershell
# Problema: Las migraciones no se ejecutaron

# Solución: Ver logs
docker-compose logs api | Select-String -Pattern "Migration|Error"

# O recrear BD
docker-compose down -v
docker-compose up -d
```

### **Error 6: No puedes conectarte desde navegador**

```powershell
# Verificar que la API está respondiendo
curl http://localhost:8080/health

# O desde PowerShell
Invoke-WebRequest -Uri "http://localhost:8080/health" -Verbose
```

---

## 📝 Checklist Final

Antes de desplegrar en producción:

- [ ] Todos los contenedores están corriendo (`docker-compose ps`)
- [ ] La API responde en `http://localhost:8080`
- [ ] Se puede crear un producto con POST
- [ ] Se puede listar productos con GET
- [ ] Los datos se persisten en PostgreSQL
- [ ] Los logs no muestran errores críticos
- [ ] Variables de entorno están correctas en `docker-compose.yml`
- [ ] La contraseña de PostgreSQL es fuerte (NO usar "postgres" en producción)

---

## 🎯 Resumen de Comandos Esenciales

```powershell
# 1. Navegar al proyecto
cd C:\desarollo\Api_rest_productos

# 2. Levantar todo
docker-compose up -d

# 3. Verificar estado
docker-compose ps

# 4. Ver logs
docker-compose logs -f

# 5. Probar API
Invoke-WebRequest http://localhost:8080/api/products

# 6. Detener
docker-compose down
```

---

## 📚 Recursos Adicionales

- [Docker Docs](https://docs.docker.com/)
- [Docker Compose Docs](https://docs.docker.com/compose/)
- [Microsoft .NET Docs](https://learn.microsoft.com/dotnet/)
- [PostgreSQL Docs](https://www.postgresql.org/docs/)

---

**Última actualización:** 9 de junio de 2026
**Versión:** 1.0
