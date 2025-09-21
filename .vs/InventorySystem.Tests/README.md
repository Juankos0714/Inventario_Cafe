# InventorySystem Tests

Esta suite de tests unitarios e integración proporciona cobertura completa para el Sistema de Inventario y Ventas.

## Estructura de Tests

### 📁 Services/
Tests para la capa de lógica de negocio (BLL):
- **AuthServiceTests**: Autenticación, login, logout, cambio de contraseñas
- **ProductServiceTests**: Gestión de productos, stock, validaciones
- **SalesServiceTests**: Procesamiento de ventas, validaciones de stock
- **ShiftServiceTests**: Manejo de turnos, validaciones de estado
- **ExcelExportServiceTests**: Exportación de reportes a Excel

### 📁 Models/
Tests para entidades y modelos:
- **ProductTests**: Validación de propiedades y comportamiento
- **UserTests**: Validación de usuarios, roles, encriptación

### 📁 Integration/
Tests de integración end-to-end:
- **SalesFlowTests**: Flujos completos de venta desde login hasta cierre

### 📁 TestHelpers/
Utilidades para testing:
- **DatabaseTestHelper**: Base de datos en memoria para tests
- **TestDataBuilder**: Generación de datos de prueba con AutoFixture
- **MockFactory**: Creación de mocks para dependencias

## Tecnologías Utilizadas

- **xUnit**: Framework de testing principal
- **FluentAssertions**: Assertions más legibles y descriptivas
- **Moq**: Mocking de dependencias
- **AutoFixture**: Generación automática de datos de prueba
- **SQLite In-Memory**: Base de datos temporal para tests

## Ejecutar Tests

### Todos los tests:
```bash
dotnet test
```

### Tests por categoría:
```bash
# Tests de autenticación
dotnet test --filter "Category=Authentication"

# Tests de gestión de productos
dotnet test --filter "Category=ProductManagement"

# Tests de ventas
dotnet test --filter "Category=Sales"

# Tests de turnos
dotnet test --filter "Category=ShiftManagement"

# Tests de integración
dotnet test --filter "Category=Integration"

# Tests de exportación Excel
dotnet test --filter "Category=ExcelExport"

# Tests de modelos
dotnet test --filter "Category=Models"
```

### Tests específicos:
```bash
# Un test específico
dotnet test --filter "FullyQualifiedName~AuthServiceTests.Login_WithValidCredentials_ShouldReturnTrue"

# Una clase específica
dotnet test --filter "ClassName~AuthServiceTests"
```

## Cobertura de Tests

### ✅ Escenarios Cubiertos

**Autenticación y Autorización:**
- ✅ Login exitoso con credenciales válidas
- ✅ Login fallido con credenciales inválidas
- ✅ Validación de roles y permisos
- ✅ Acceso denegado para operaciones sin permisos
- ✅ Encriptación correcta de contraseñas
- ✅ Manejo de sesiones activas

**Gestión de Productos:**
- ✅ Crear producto con datos válidos
- ✅ Validar datos inválidos (precio negativo, etc.)
- ✅ Actualizar stock correctamente
- ✅ Validar alertas de stock bajo
- ✅ Desactivar/activar productos
- ✅ Intentar vender productos sin stock

**Procesamiento de Ventas:**
- ✅ Procesar venta simple y múltiple
- ✅ Intentar venta con stock insuficiente
- ✅ Cálculos correctos de totales
- ✅ Actualización automática de stock
- ✅ Registro correcto de movimientos
- ✅ Venta sin turno activo

**Manejo de Turnos:**
- ✅ Iniciar/cerrar turno correctamente
- ✅ Intentar iniciar turno con uno activo
- ✅ Validar un turno activo por usuario
- ✅ Cálculo automático de totales

**Validaciones de Negocio:**
- ✅ Cantidades y precios negativos
- ✅ Validación de formato de datos
- ✅ Integridad referencial
- ✅ Operaciones concurrentes

**Exportación y Reportes:**
- ✅ Generación correcta de Excel
- ✅ Estructura correcta de datos
- ✅ Manejo de errores en exportación
- ✅ Validación de archivos generados

## Configuración de CI/CD

### GitHub Actions (ejemplo):
```yaml
name: Tests
on: [push, pull_request]
jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --no-restore
    - name: Test
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
    - name: Upload coverage
      uses: codecov/codecov-action@v3
```

## Mejores Prácticas Implementadas

### 🏗️ Patrón AAA (Arrange-Act-Assert)
Todos los tests siguen la estructura:
```csharp
[Fact]
public void Method_Scenario_ExpectedResult()
{
    // Arrange - Configurar datos y dependencias
    var service = new Service();
    var input = "test data";
    
    // Act - Ejecutar la acción a probar
    var result = service.Method(input);
    
    // Assert - Verificar el resultado
    result.Should().Be(expectedValue);
}
```

### 🏷️ Categorización y Traits
```csharp
[Trait("Category", "Authentication")]
[Trait("Category", "Integration")]
```

### 🧪 Datos de Prueba
- **TestDataBuilder** para generar datos consistentes
- **AutoFixture** para datos aleatorios válidos
- **Builders específicos** para escenarios complejos

### 🗄️ Base de Datos de Test
- **SQLite en memoria** para aislamiento
- **Setup/Teardown** automático
- **Datos semilla** consistentes

### 📊 Assertions Descriptivas
```csharp
result.Should().BeTrue("valid credentials should allow login");
user.Role.Should().Be(UserRole.Admin, "admin user should have Admin role");
```

## Mantenimiento

### Agregar Nuevos Tests:
1. Crear archivo en la carpeta apropiada
2. Seguir convenciones de nomenclatura
3. Usar TestDataBuilder para datos
4. Agregar traits apropiados
5. Documentar escenarios complejos

### Actualizar Tests Existentes:
1. Mantener compatibilidad con tests existentes
2. Actualizar TestDataBuilder si es necesario
3. Verificar que todos los tests pasen
4. Actualizar documentación si es necesario

## Troubleshooting

### Tests Fallan Localmente:
1. Verificar que todas las dependencias estén instaladas
2. Limpiar y reconstruir solución
3. Verificar permisos de archivos temporales
4. Revisar configuración de base de datos

### Tests Lentos:
1. Verificar uso de base de datos en memoria
2. Optimizar generación de datos de prueba
3. Paralelizar tests independientes
4. Revisar operaciones de I/O innecesarias

### Cobertura Baja:
1. Identificar código no cubierto
2. Agregar tests para edge cases
3. Verificar mocks y stubs
4. Revisar tests de integración

---

**Cobertura Actual**: ~85% en servicios críticos
**Total de Tests**: 50+ tests unitarios e integración
**Tiempo de Ejecución**: < 30 segundos