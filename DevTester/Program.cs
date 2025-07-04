using CoreApp;
using DataAccess.CRUD;
using DataAccess.DAO;
using DTOs;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

public class ProgramFZ
{
    public static void Main(string[] args)
    {
        CrearCommerce();

    }

    public static void CrearCommerce()
    {

        Console.WriteLine("=== Crear Nuevo Comercio ===");

        Console.Write("Cédula Jurídica del Comercio: ");
        string legalId = Console.ReadLine();

        Console.Write("Nombre: ");
        string name = Console.ReadLine();

        Console.Write("Télefono: ");
        string phone = Console.ReadLine();

        Console.Write("Correo Electrónico: ");
        string email = Console.ReadLine();

        Console.Write("Latitud: ");
        decimal latitude = decimal.Parse(Console.ReadLine());

        Console.Write("Longitud: ");
        decimal longitude = decimal.Parse(Console.ReadLine());

        Console.Write("IBAN: ");
        string iban = Console.ReadLine();

        //Creamos el objeto del usuario a partir de los datos ingresados

        var commerce = new Commerce()
        {
            LegalId = legalId,
            Name = name,
            Phone = phone,
            Email = email,
            Latitude = latitude,
            Longitude = longitude,
            IBAN = iban
        };

        var cManager = new CommerceManager();
        cManager.Create(commerce);

        Console.WriteLine("Comercio creado exitosamente.");

    }
}