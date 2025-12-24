using System;
using Microsoft.Data.SqlClient;

namespace SneakerStoreApp
{
    class Program
    {
    
        static string connectionString = @"Server=sql.bsite.net\MSSQL2016;Database=osipov_Zhukov;User Id=osipov_Zhukov;Password=12345;TrustServerCertificate=True;";

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n=== SneakerStore: Управление клиентами ===");
                Console.WriteLine("1. Просмотр всех");
                Console.WriteLine("2. Создать запись");
                Console.WriteLine("3. Обновить запись");
                Console.WriteLine("4. Удалить запись");
                Console.WriteLine("5. Найти по ID");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1": ReadAll(); break;
                        case "2": Create(); break;
                        case "3": Update(); break;
                        case "4": Delete(); break;
                        case "5": ReadById(); break;
                        case "0": return;
                    }
                }
                catch (SqlException ex)
                {

                    Console.WriteLine($"Ошибка базы данных: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }



        static void ReadAll()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT customer_id, first_name, last_name FROM Customer";
                SqlCommand cmd = new SqlCommand(sql, conn);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    Console.WriteLine("\nID | Имя | Фамилия");
                    while (rdr.Read()) Console.WriteLine($"{rdr[0]} | {rdr[1]} | {rdr[2]}");
                }
            }
        }

        static void Create()
        {
            Console.Write("Введите имя: "); string fn = Console.ReadLine();
            Console.Write("Введите фамилию: "); string ln = Console.ReadLine();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO Customer (first_name, last_name) VALUES (@fn, @ln)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@fn", fn);
                cmd.Parameters.AddWithValue("@ln", ln);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Клиент добавлен успешно!");
            }
        }

        static void Update()
        {
            Console.Write("Введите ID для изменения: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Новое имя: ");
            string fn = Console.ReadLine();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE Customer SET first_name=@fn WHERE customer_id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@fn", fn);
                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Данные обновлены." : "ID не найден.");
            }
        }

        static void Delete()
        {
            Console.Write("Введите ID для удаления: ");
            int id = int.Parse(Console.ReadLine());
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Customer WHERE customer_id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                int result = cmd.ExecuteNonQuery();
                Console.WriteLine(result > 0 ? "Запись удалена." : "ID не найден.");
            }
        }

        static void ReadById()
        {
            Console.Write("Введите ID поиска: ");
            int id = int.Parse(Console.ReadLine());
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "SELECT first_name, last_name FROM Customer WHERE customer_id=@id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read()) Console.WriteLine($"Результат: {rdr[0]} {rdr[1]}");
                    else Console.WriteLine("Клиент не найден.");
                }
            }
        }
    }
}   