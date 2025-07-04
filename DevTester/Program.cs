using CoreApp;
using DataAccess.CRUD;
using DTOs;
using Newtonsoft.Json;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== BILLETICO - GESTIÓN DE ENTIDADES FINANCIERAS ===");
            Console.WriteLine();
            Console.WriteLine("REGISTRO (HU 2.1):");
            Console.WriteLine("1. Crear entidad financiera");
            Console.WriteLine();
            Console.WriteLine("ADMINISTRACIÓN (HU 2.2):");
            Console.WriteLine("2. Ver entidades pendientes");
            Console.WriteLine("3. Ver detalles de entidad");
            Console.WriteLine("4. Aprobar entidad");
            Console.WriteLine("5. Rechazar entidad");
            Console.WriteLine("6. Ver entidades activas");
            Console.WriteLine("7. Ver entidades rechazadas");
            Console.WriteLine("8. Verificar si entidad puede operar");
            Console.WriteLine();
            Console.WriteLine("0. Salir");
            Console.WriteLine();

            Console.Write("Seleccione una opción: ");
            var input = Console.ReadLine();

            if (!int.TryParse(input, out int option))
            {
                Console.WriteLine("❌ Opción inválida");
                Console.ReadKey();
                continue;
            }

            try
            {
                switch (option)
                {
                    case 1:
                        CreateFinancialEntity();
                        break;
                    case 2:
                        ListPendingEntities();
                        break;
                    case 3:
                        ViewEntityDetails();
                        break;
                    case 4:
                        ApproveEntity();
                        break;
                    case 5:
                        RejectEntity();
                        break;
                    case 6:
                        ListActiveEntities();
                        break;
                    case 7:
                        ListRejectedEntities();
                        break;
                    case 8:
                        CheckEntityCanOperate();
                        break;
                    case 0:
                        Console.WriteLine("👋 ¡Hasta luego!");
                        return;
                    default:
                        Console.WriteLine("❌ Opción no válida");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }

            Console.WriteLine("\n📌 Presione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }

    // =============================================
    // HU 2.1 - REGISTRO
    // =============================================
    private static void CreateFinancialEntity()
    {
        Console.WriteLine("\n=== CREAR ENTIDAD FINANCIERA (HU 2.1) ===");

        Console.Write("Cédula jurídica (12 dígitos): ");
        var legalId = Console.ReadLine();

        Console.Write("Código bancario: ");
        var bankCode = Console.ReadLine();

        Console.Write("Nombre de la entidad: ");
        var name = Console.ReadLine();

        Console.Write("Teléfono (8 dígitos): ");
        var phone = Console.ReadLine();

        Console.Write("Email: ");
        var email = Console.ReadLine();

        Console.Write("Latitud: ");
        var latitude = decimal.Parse(Console.ReadLine());

        Console.Write("Longitud: ");
        var longitude = decimal.Parse(Console.ReadLine());

        var financialEntity = new FinancialEntity()
        {
            LegalId = legalId,
            BankCode = bankCode,
            Name = name,
            Phone = phone,
            Email = email,
            Latitude = latitude,
            Longitude = longitude
        };

        var fem = new FinancialEntityManager();
        fem.Create(financialEntity);
        Console.WriteLine("✅ Su solicitud ha sido recibida correctamente y está pendiente de aprobación.");
    }

    // =============================================
    // HU 2.2 - ADMINISTRACIÓN
    // =============================================

    // T15: Lista de entidades pendientes
    private static void ListPendingEntities()
    {
        Console.WriteLine("\n=== ENTIDADES PENDIENTES (T15) ===");

        var fem = new FinancialEntityManager();
        var pendingEntities = fem.GetPendingEntities();

        if (pendingEntities.Count == 0)
        {
            Console.WriteLine("📭 No hay entidades pendientes");
            return;
        }

        Console.WriteLine($"📋 Se encontraron {pendingEntities.Count} entidades pendientes:\n");

        foreach (var entity in pendingEntities)
        {
            Console.WriteLine($"ID: {entity.Id} | Nombre: {entity.Name} | Código: {entity.BankCode} | Estado: {entity.Status}");
            Console.WriteLine($"   Cédula: {entity.LegalId} | Email: {entity.Email}");
            Console.WriteLine($"   Creado: {entity.Created:dd/MM/yyyy HH:mm}");
            Console.WriteLine();
        }
    }

    // T16: Ver detalles de solicitud
    private static void ViewEntityDetails()
    {
        Console.WriteLine("\n=== DETALLES DE ENTIDAD (T16) ===");

        Console.Write("Digite el ID de la entidad: ");
        var entityId = int.Parse(Console.ReadLine());

        var fem = new FinancialEntityManager();
        var entity = fem.GetEntityDetails(entityId);

        if (entity == null)
        {
            Console.WriteLine("❌ Entidad no encontrada");
            return;
        }

        Console.WriteLine("\n📄 DETALLES COMPLETOS:");
        Console.WriteLine($"ID: {entity.Id}");
        Console.WriteLine($"Cédula Jurídica: {entity.LegalId}");
        Console.WriteLine($"Código Bancario: {entity.BankCode}");
        Console.WriteLine($"Nombre: {entity.Name}");
        Console.WriteLine($"Teléfono: {entity.Phone}");
        Console.WriteLine($"Email: {entity.Email}");
        Console.WriteLine($"Coordenadas: {entity.Latitude}, {entity.Longitude}");
        Console.WriteLine($"Estado: {entity.Status}");
        Console.WriteLine($"Comisión: {entity.CommissionRate:F2}%");
        Console.WriteLine($"Creado: {entity.Created:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Actualizado: {(entity.Updated == DateTime.MinValue ? "N/A" : entity.Updated.ToString("dd/MM/yyyy HH:mm"))}");
    }

    // T17: Aprobar entidad
    private static void ApproveEntity()
    {
        Console.WriteLine("\n=== APROBAR ENTIDAD (T17) ===");

        Console.Write("Digite el ID de la entidad a aprobar: ");
        var entityId = int.Parse(Console.ReadLine());

        Console.Write("Digite la comisión a asignar (ej: 2.50 para 2.50%): ");
        var commissionRate = decimal.Parse(Console.ReadLine());

        var fem = new FinancialEntityManager();
        fem.ApproveEntity(entityId, commissionRate);

        Console.WriteLine("✅ Entidad aprobada exitosamente");
        Console.WriteLine($"✅ Estado actualizado a 'Activa' con comisión del {commissionRate:F2}%");
        Console.WriteLine("✅ La entidad ya puede operar en la plataforma");
    }

    // T17: Rechazar entidad
    private static void RejectEntity()
    {
        Console.WriteLine("\n=== RECHAZAR ENTIDAD (T17) ===");

        Console.Write("Digite el ID de la entidad a rechazar: ");
        var entityId = int.Parse(Console.ReadLine());

        var fem = new FinancialEntityManager();
        fem.RejectEntity(entityId);

        Console.WriteLine("❌ Entidad rechazada exitosamente");
        Console.WriteLine("❌ Estado actualizado a 'Rechazada'");
        Console.WriteLine("📧 Se debería enviar notificación a la entidad (T18)");
    }

    // Ver entidades activas
    private static void ListActiveEntities()
    {
        Console.WriteLine("\n=== ENTIDADES ACTIVAS ===");

        var fem = new FinancialEntityManager();
        var activeEntities = fem.GetActiveEntities();

        if (activeEntities.Count == 0)
        {
            Console.WriteLine("📭 No hay entidades activas");
            return;
        }

        Console.WriteLine($"✅ Se encontraron {activeEntities.Count} entidades activas:\n");

        foreach (var entity in activeEntities)
        {
            Console.WriteLine($"ID: {entity.Id} | Nombre: {entity.Name} | Código: {entity.BankCode}");
            Console.WriteLine($"   Comisión: {entity.CommissionRate:F2}% | Estado: {entity.Status}");
            Console.WriteLine($"   Aprobado: {entity.Updated:dd/MM/yyyy HH:mm}");
            Console.WriteLine();
        }
    }

    // Ver entidades rechazadas
    private static void ListRejectedEntities()
    {
        Console.WriteLine("\n=== ENTIDADES RECHAZADAS ===");

        var fem = new FinancialEntityManager();
        var rejectedEntities = fem.GetRejectedEntities();

        if (rejectedEntities.Count == 0)
        {
            Console.WriteLine("📭 No hay entidades rechazadas");
            return;
        }

        Console.WriteLine($"❌ Se encontraron {rejectedEntities.Count} entidades rechazadas:\n");

        foreach (var entity in rejectedEntities)
        {
            Console.WriteLine($"ID: {entity.Id} | Nombre: {entity.Name} | Código: {entity.BankCode}");
            Console.WriteLine($"   Estado: {entity.Status} | Rechazado: {entity.Updated:dd/MM/yyyy HH:mm}");
            Console.WriteLine();
        }
    }

    // T19: Verificar si entidad puede operar
    private static void CheckEntityCanOperate()
    {
        Console.WriteLine("\n=== VERIFICAR OPERATIVIDAD (T19) ===");

        Console.Write("Digite el ID de la entidad: ");
        var entityId = int.Parse(Console.ReadLine());

        var fem = new FinancialEntityManager();
        var canOperate = fem.CanEntityOperate(entityId);

        if (canOperate)
        {
            Console.WriteLine("✅ La entidad PUEDE operar (estado: Activa)");
            Console.WriteLine("✅ Puede acceder a promociones, cobros y otras funciones");
        }
        else
        {
            Console.WriteLine("❌ La entidad NO PUEDE operar");
            Console.WriteLine("❌ Solo entidades con estado 'Activa' pueden operar");
        }
    }
}