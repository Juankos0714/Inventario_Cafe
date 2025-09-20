# Sistema de Inventario y Ventas - C# Windows Forms

## Descripción
Sistema completo de punto de venta e inventario desarrollado en C# con Windows Forms, usando arquitectura multicapa y base de datos SQLite local.

## Características Principales

### Arquitectura
- **Presentación (UI)**: Windows Forms con interfaz adaptada por roles
- **Lógica de Negocio (BLL)**: Validaciones y reglas de negocio
- **Acceso a Datos (DAL)**: Repositorios para SQLite
- **Servicios**: Exportación a Excel

### Funcionalidades por Rol

#### Administrador
- Gestión completa de usuarios
- Gestión de productos e inventario
- Registro de ventas
- Manejo de turnos
- Generación de reportes
- Cierres diarios
- Exportación a Excel

#### Vendedor
- Registro de ventas
- Consulta de productos
- Manejo de turnos propios
- Cierre de turno

#### Contador
- Generación de reportes
- Exportación a Excel
- Consulta de información (sin modificar)

### Base de Datos
- **SQLite** local con las siguientes tablas:
  - Users (usuarios y roles)
  - Products (productos e inventario)
  - Movements (movimientos de stock)
  - Shifts (turnos de trabajo)
  - DailyCloses (cierres diarios)

## Requisitos del Sistema

### Software Necesario
- Visual Studio 2022 o superior
- .NET 8.0
- SQL Server (opcional, usa SQLite por defecto)

### Paquetes NuGet Incluidos
- `System.Data.SQLite` - Base de datos SQLite
- `ClosedXML` - Exportación a Excel
- `BCrypt.Net-Next` - Encriptación de contraseñas

## Instalación

1. **Clonar/Descargar el proyecto**
2. **Abrir en Visual Studio**
   - Abrir `InventorySystem.sln`
3. **Restaurar paquetes NuGet**
   - Visual Studio lo hará automáticamente
4. **Compilar y ejecutar**
   - Presionar F5 o Ctrl+F5

## Uso del Sistema

### Primer Inicio
- **Usuario por defecto**: `admin`
- **Contraseña por defecto**: `admin123`

### Flujo de Trabajo Típico

1. **Administrador configura el sistema**:
   - Crear usuarios (vendedores, contadores)
   - Cargar productos iniciales
   - Configurar stock mínimo

2. **Vendedor inicia turno**:
   - Login con sus credenciales
   - Iniciar turno desde el menú
   - Registrar ventas
   - Cerrar turno al finalizar

3. **Generación de reportes**:
   - Resúmenes diarios
   - Reportes de inventario
   - Exportación a Excel

### Funciones Principales

#### Gestión de Productos
- Crear/editar productos
- Control de stock mínimo
- Alertas de stock bajo
- Categorización

#### Sistema de Ventas
- Selección de productos
- Control automático de stock
- Validación de disponibilidad
- Cálculo automático de totales

#### Control de Turnos
- Inicio/cierre de turnos
- Cálculo automático de ventas por turno
- Restricción: un turno activo por usuario

#### Reportes y Análisis
- Resumen diario de ventas
- Reporte de movimientos de inventario
- Estado actual del inventario
- Productos más vendidos
- Exportación a Excel

## Estructura del Proyecto

```
InventorySystem/
├── Models/
│   ├── Entities/          # Entidades del dominio
│   ├── Enums/             # Enumeraciones
│   └── DTOs/              # Objetos de transferencia
├── DAL/                   # Capa de acceso a datos
├── BLL/                   # Lógica de negocio
├── Services/              # Servicios (Excel, etc.)
└── UI/Forms/              # Formularios Windows Forms
```

## Seguridad
- Contraseñas encriptadas con BCrypt
- Control de acceso por roles
- Validación de permisos en cada operación
- Base de datos local (sin exposición de red)

## Características Técnicas

### Base de Datos
- SQLite embebida
- Inicialización automática
- Creación de usuario admin por defecto
- Integridad referencial

### Interfaz de Usuario
- Formularios adaptativos por rol
- Validaciones en tiempo real
- Mensajes informativos
- Diseño profesional

### Exportación
- Reportes en Excel (.xlsx)
- Múltiples hojas de cálculo
- Formato profesional con colores
- Datos estructurados

## Mantenimiento

### Respaldos
La base de datos se encuentra en:
`%AppData%\InventorySystem\database.db`

### Logs
El sistema maneja errores internamente y muestra mensajes al usuario.

## Extensibilidad

El sistema está diseñado para ser extensible:
- Fácil adición de nuevos roles
- Nuevos tipos de reportes
- Integración con otros sistemas
- Personalización de la interfaz

## Solución de Problemas

### Problemas Comunes
1. **Error de base de datos**: Verificar permisos de escritura en %AppData%
2. **No se puede exportar**: Verificar que Excel esté cerrado
3. **Usuario no puede iniciar turno**: Verificar que no tenga un turno activo

### Contacto
Para soporte técnico o personalizaciones, contactar al desarrollador.

## Licencia
Este proyecto es de uso libre para propósitos educativos y comerciales.