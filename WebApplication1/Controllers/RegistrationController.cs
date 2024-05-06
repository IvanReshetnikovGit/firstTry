using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Text;
namespace WebApplication1.Controllers;

public class RegistrationController : Controller
{
    private readonly IConfiguration _configuration;

    public RegistrationController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index(string password,int Id, bool submit)
    {
        List<User> users= new();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        string connectionString = _configuration.GetConnectionString("DatabaseConnection");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        using (MySqlConnection connection = new(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM Users";
            using (MySqlCommand command = new(query, connection))
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User user= new()
                        {
                            Id = reader.GetInt32("Id"),
                            Username = reader.GetString("Username"),
                            Password = reader.GetString("Password")
                        };
                        users.Add(user);
                    }
                }
            }
        }
        if(submit==false)
            return View("Index");
            
        else if(HashPassword(password) == users[Id].Password && submit == true)
            return View("~/Views/Lab1/Gather.cshtml");
        
        else if(submit == true)
        {
            ViewBag.Error = "Пароль неправильний";
            return View("Index");
        }
        else
            return View("Index");
    }
    
    public static string HashPassword(string password)
    {
        byte[] data = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            stringBuilder.Append(data[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }
}