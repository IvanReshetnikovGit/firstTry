using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class Lab1Controller : Controller
    {
        private readonly IConfiguration _configuration;

        public Lab1Controller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Calculator()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

         public IActionResult Mushroom_picker(string sortOrder)
        {
            List<Mushroom_picker> mushroom_pickers = new List<Mushroom_picker>();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM mushroom_picker";

                // Добавляем сортировку в запрос SQL в зависимости от sortOrder
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "name_desc":
                            query += " ORDER BY name DESC";
                            break;
                        case "yearbirth":
                            query += " ORDER BY yearbirth";
                            break;
                        case "id":
                            query += " ORDER BY idmushroom_picker";
                            break;
                        default:
                            query += " ORDER BY idmushroom_picker"; // По умолчанию сортировка по ID
                            break;
                    }
                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mushroom_picker mushroom_picker = new Mushroom_picker
                            {
                                idmushroom_picker = reader.GetInt32("idmushroom_picker"),
                                name = reader.GetString("name"),
                                yearbirth = reader.GetInt32("yearbirth")
                            };
                            mushroom_pickers.Add(mushroom_picker);
                        }
                    }
                }
            }

            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.YearOfBirthSortParm = sortOrder == "yearbirth" ? "yearbirth_desc" : "yearbirth";

            ViewBag.Mushroom_Pickers = mushroom_pickers;
            return View();
        }

        public IActionResult SearchMushroomPickers(string searchString)
        {
            List<Mushroom_picker> mushroom_pickers = new List<Mushroom_picker>();

        #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string connectionString = _configuration.GetConnectionString("DatabaseConnection");
        #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Изменяем запрос SQL для поиска по имени
                string query = @"
            SELECT * 
            FROM mushroom_picker 
            WHERE 
                name LIKE @searchString OR 
                yearbirth LIKE @searchString OR
                idmushroom_picker LIKE @searchString";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchString", $"%{searchString}%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mushroom_picker mushroom_picker = new Mushroom_picker
                            {
                                idmushroom_picker = reader.GetInt32("idmushroom_picker"),
                                name = reader.GetString("name"),
                                yearbirth = reader.GetInt32("yearbirth")
                            };
                            mushroom_pickers.Add(mushroom_picker);
                        }
                    }
                }
            }

            ViewBag.Mushroom_Pickers = mushroom_pickers;
            return View("Mushroom_picker");
        }

       public IActionResult Gather(string sortOrder)
        {
            List<Gather> gathers = new List<Gather>();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT 
                        g.IdGather,
                        g.idmushroom_picker,
                        g.Idmushroom,
                        mp.name AS PickerName,
                        m.Name AS MushroomName
                    FROM 
                        Gather g
                    LEFT JOIN 
                        mushroom_picker mp ON g.idmushroom_picker = mp.idmushroom_picker
                    LEFT JOIN 
                        Mushroom m ON g.Idmushroom = m.Idmushroom";

                // Добавляем сортировку в запрос SQL в зависимости от sortOrder
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "id_desc":
                            query += " ORDER BY g.IdGather DESC";
                            break;
                        case "mushroom_picker":
                            query += " ORDER BY mp.name";
                            break;
                        case "mushroom":
                            query += " ORDER BY m.Name";
                            break;
                        default:
                            query += " ORDER BY g.IdGather"; // По умолчанию сортировка по ID
                            break;
                    }
                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gather gather = new Gather
                            {
                                idmushroom_picker = reader.GetInt32("idmushroom_picker"),
                                IdGather = reader.GetInt32("IdGather"),
                                Idmushroom = reader.GetInt32("Idmushroom"),

                            };
                            gathers.Add(gather);
                        }
                    }
                }
            }

            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.PickerIdSortParm = sortOrder == "mushroom_picker" ? "mushroom_picker_desc" : "mushroom_picker";
            ViewBag.MushroomIdSortParm = sortOrder == "mushroom" ? "mushroom_desc" : "mushroom";

            ViewBag.gathers = gathers;
            return View();
        }

        public IActionResult SearchGathers(string searchString)
        {
            List<Gather> gathers = new List<Gather>();

        #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                    string connectionString = _configuration.GetConnectionString("DatabaseConnection");
        #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Изменяем запрос SQL для поиска по разным колонкам
                string query = @"
                    SELECT * 
                    FROM Gather 
                    WHERE 
                        idmushroom_picker LIKE @searchString OR 
                        IdGather LIKE @searchString OR 
                        Idmushroom LIKE @searchString";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchString", $"%{searchString}%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Gather gather = new Gather
                            {
                                idmushroom_picker = reader.GetInt32("idmushroom_picker"),
                                IdGather = reader.GetInt32("IdGather"),
                                Idmushroom = reader.GetInt32("Idmushroom"),
                            };
                            gathers.Add(gather);
                        }
                    }
                }
            }

            ViewBag.gathers = gathers;
            return View("Gather");
        }

        public IActionResult Mushroom(string sortOrder)
        {
            List<Mushroom> mushrooms = new List<Mushroom>();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Mushroom";

                // Добавляем сортировку в запрос SQL в зависимости от sortOrder
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    switch (sortOrder)
                    {
                        case "Id_desc":
                            query += " ORDER BY Idmushroom DESC";
                            break;
                        case "Name":
                            query += " ORDER BY Name";
                            break;
                        case "Edibility":
                            query += " ORDER BY Edibility";
                            break;
                        case "Class":
                            query += " ORDER BY Class";
                            break;
                        case "Price":
                            query += " ORDER BY Market_price";
                            break;
                        case "Birthplace":
                            query += " ORDER BY Mushroom_birthplace";
                            break;
                        case "Rarity":
                            query += " ORDER BY Rarity";
                            break;
                        default:
                            query += " ORDER BY Idmushroom"; // По умолчанию сортировка по ID
                            break;
                    }
                }

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mushroom mushroom = new Mushroom
                            {
                                Idmushroom = reader.GetInt32("Idmushroom"),
                                Name = reader.GetString("Name"),
                                Edibility = reader.GetBoolean("Edibility"),
                                Class = reader.GetString("Class"),
                                Market_price = reader.GetInt32("Market_price"),
                                Mushroom_birthplace = reader.GetString("Mushroom_birthplace"),
                                Rarity = reader.GetString("Rarity"),
                            };
                            mushrooms.Add(mushroom);
                        }
                    }
                }
            }

            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "Id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewBag.EdibilitySortParm = sortOrder == "Edibility" ? "Edibility_desc" : "Edibility";
            ViewBag.ClassSortParm = sortOrder == "Class" ? "Class_desc" : "Class";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.BirthplaceSortParm = sortOrder == "Birthplace" ? "Birthplace_desc" : "Birthplace";
            ViewBag.RaritySortParm = sortOrder == "Rarity" ? "Rarity_desc" : "Rarity";

            ViewBag.mushrooms = mushrooms;
            return View();
        }

        public IActionResult SearchMushrooms(string searchString)
        {
            List<Mushroom> mushrooms = new List<Mushroom>();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Изменяем запрос SQL для поиска по разным колонкам
                string query = @"
                    SELECT * 
                    FROM Mushroom 
                    WHERE 
                        Name LIKE @searchString OR 
                        Edibility LIKE @searchString OR 
                        Class LIKE @searchString OR
                        Market_price LIKE @searchString OR
                        Mushroom_birthplace LIKE @searchString OR
                        Rarity LIKE @searchString";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchString", $"%{searchString}%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Mushroom mushroom = new Mushroom
                            {
                                Idmushroom = reader.GetInt32("Idmushroom"),
                                Name = reader.GetString("Name"),
                                Edibility = reader.GetBoolean("Edibility"),
                                Class = reader.GetString("Class"),
                                Market_price = reader.GetInt32("Market_price"),
                                Mushroom_birthplace = reader.GetString("Mushroom_birthplace"),
                                Rarity = reader.GetString("Rarity"),
                            };
                            mushrooms.Add(mushroom);
                        }
                    }
                }
            }

            ViewBag.mushrooms = mushrooms;
            return View("Mushroom");
        }

    public IActionResult AddGather()
    {
        return View();
    }


    public IActionResult AddGather1(Gather gather)
{
    try
    {
        string connectionString = _configuration.GetConnectionString("DatabaseConnection");
        
        // Проверяем, что данные gather корректны
        if (gather.idmushroom_picker <= 0 || gather.Idmushroom <= 0)
        {
            // Если данные неправильные, возвращаем пользователю сообщение об ошибке
            ViewBag.ErrorMessage = "Введены неправильные данные";
            return View("AddGather");
        }
        
        // Проверяем существование mushroom_picker с указанным idmushroom_picker
        string pickerCheckQuery = "SELECT COUNT(*) FROM mushroom_picker WHERE idmushroom_picker = @PickerId";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (MySqlCommand pickerCheckCommand = new MySqlCommand(pickerCheckQuery, connection))
            {
                pickerCheckCommand.Parameters.AddWithValue("@PickerId", gather.idmushroom_picker);
                int pickerCount = Convert.ToInt32(pickerCheckCommand.ExecuteScalar());

                if (pickerCount == 0)
                {
                    // Если mushroom_picker с указанным idmushroom_picker не существует, выводим сообщение об ошибке
                    ViewBag.ErrorMessage = "Mushroom picker с указанным idmushroom_picker не существует";
                    return View("AddGather");
                }
            }
        }

        // Проверяем существование mushroom с указанным Idmushroom
        string mushroomCheckQuery = "SELECT COUNT(*) FROM Mushroom WHERE Idmushroom = @MushroomId";
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (MySqlCommand mushroomCheckCommand = new MySqlCommand(mushroomCheckQuery, connection))
            {
                mushroomCheckCommand.Parameters.AddWithValue("@MushroomId", gather.Idmushroom);
                int mushroomCount = Convert.ToInt32(mushroomCheckCommand.ExecuteScalar());

                if (mushroomCount == 0)
                {
                    // Если mushroom с указанным Idmushroom не существует, выводим сообщение об ошибке
                    ViewBag.ErrorMessage = "Mushroom с указанным Idmushroom не существует";
                    return View("AddGather");
                }
            }
        }

        // Если все проверки пройдены успешно, выполняем операцию добавления gather в базу данных
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            
            string query = "INSERT INTO Gather (idmushroom_picker, Idmushroom) VALUES (@PickerId, @MushroomId)";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PickerId", gather.idmushroom_picker);
                command.Parameters.AddWithValue("@MushroomId", gather.Idmushroom);

                command.ExecuteNonQuery();
            }
        }

        // После успешного добавления перенаправляем на главную страницу
        return RedirectToAction("Gather", "Lab1");
    }
    catch (MySqlException ex)
    {
        // Обработка исключения при возникновении ошибки MySQL
        ViewBag.ErrorMessage = "Помилка додавання запису в таблицю Gather";
        return View("AddGather"); // Возвращаем ту же страницу с сообщением об ошибке
    }
}


    public IActionResult AddMushroomPicker()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddMushroomPicker1(Mushroom_picker picker)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Mushroom_picker (name, yearbirth) VALUES (@Name, @YearBirth)";
                
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", picker.name);
                    command.Parameters.AddWithValue("@YearBirth", picker.yearbirth);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Mushroom_picker", "Lab1"); // Перенаправляем на главную страницу после добавления записи
        }
        catch (MySqlException ex)
        {
            // Обработка исключения при возникновении ошибки MySQL
            ViewBag.ErrorMessage = "Помилка додавання запису в таблицю Mushroom_picker";
            return View("AddMushroomPicker"); // Возвращаем ту же страницу с сообщением об ошибке
        }
    }

    public IActionResult AddMushroom()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddMushroom1(Mushroom mushroom)
    {
        try
        {
            // Проверяем правильность переданных данных, в том числе значения Edibility
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DatabaseConnection");
                
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "INSERT INTO Mushroom (Name, Edibility, Class, Market_price, Mushroom_birthplace, Rarity) VALUES (@Name, @Edibility, @Class, @MarketPrice, @Birthplace, @Rarity)";
                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", mushroom.Name);
                        command.Parameters.AddWithValue("@Edibility", mushroom.Edibility ? 1 : 0); // Преобразовываем значение bool в tinyint
                        command.Parameters.AddWithValue("@Class", mushroom.Class);
                        command.Parameters.AddWithValue("@MarketPrice", mushroom.Market_price);
                        command.Parameters.AddWithValue("@Birthplace", mushroom.Mushroom_birthplace);
                        command.Parameters.AddWithValue("@Rarity", mushroom.Rarity);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Mushroom", "Lab1"); // Перенаправляем на главную страницу после добавления записи
            }
            else
            {
                // Если модель не прошла валидацию, возвращаем ту же страницу с ошибкой
                return View("AddMushroom", mushroom);
            }
        }
        catch (MySqlException ex)
        {
            // Обработка исключения при возникновении ошибки MySQL
            ViewBag.ErrorMessage = "Ошибка при добавлении записи в таблицу Mushroom";
            return View("AddMushroom", mushroom); // Возвращаем ту же страницу с сообщением об ошибке
        }
    }


    public IActionResult DeleteMushroom()
    {
        return View();
    }

    // POST метод для обработки запроса на удаление гриба
    [HttpPost]
    public IActionResult DeleteMushroom1(int id)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM Mushroom WHERE idmushroom = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Mushroom", "Lab1"); // Перенаправляем на главную страницу после удаления записи
        }
        catch (MySqlException ex)
        {
            // Обработка исключения при возникновении ошибки MySQL
            ViewBag.ErrorMessage = "Ошибка при удалении записи из таблицы Mushroom";
            return View("DeleteMushroom"); // Возвращаем ту же страницу с сообщением об ошибке
        }
    }



    public IActionResult DeleteGather()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DeleteGather1(int id)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM Gather WHERE idGather = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index", "Home"); // Перенаправляем на главную страницу после удаления записи
        }
        catch (MySqlException ex)
        {
            // Обработка исключения при возникновении ошибки MySQL
            ViewBag.ErrorMessage = "Ошибка при удалении записи из таблицы Gather";
            return View("DeleteGather"); // Возвращаем ту же страницу с сообщением об ошибке
        }
    }

    public IActionResult DeleteMushroomPicker()
    {
        return View();
    }

    [HttpPost]
    public IActionResult DeleteMushroomPicker1(int id)
    {
        try
        {
            string connectionString = _configuration.GetConnectionString("DatabaseConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "DELETE FROM mushroom_picker WHERE idmushroom_picker = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Index", "Home"); // Перенаправляем на главную страницу после удаления записи
        }
        catch (MySqlException ex)
        {
            // Обработка исключения при возникновении ошибки MySQL
            ViewBag.ErrorMessage = "Ошибка при удалении записи из таблицы MushroomPicker";
            return View("DeleteMushroomPicker"); // Возвращаем ту же страницу с сообщением об ошибке
        }
    }
















        public IActionResult Calculate(string num1, string num2, string operation)
        {
            double result = 0;

            double.TryParse(num1, out double number1);
            double.TryParse(num2, out double number2);

            switch (operation)
            {
                case "Add":
                    result = number1 + number2;
                    break;
                case "Subtract":
                    result = number1 - number2;
                    break;
                case "Multiply":
                    result = number1 * number2;
                    break;
                case "Divide":
                    if (number2 != 0)
                    {
                        result = number1 / number2;
                    }
                    else
                    {
                        ViewBag.Error = "Не можна ділити на 0";
                        return View("Calculator");
                    }
                    break;
                default:
                    ViewBag.Error = "Не має такої операції";
                    return View("Calculator");
            }

            ViewBag.Result = result;
            return View("Calculator");
        }
    }
}
