using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Threading;
using static System.Console;

namespace Product_Manager_v1._0
{
    class Program
    {
        static string connectionString = "Server=localhost;Database=TODO;Integrated Security=True";

        static void Main(string[] args)
        {
            bool mainMenu = true;

            do
            {
                CursorVisible = false;

                WriteLine("1. Add Article");

                WriteLine("2. Search Article");

                WriteLine("3. Exit");

                ConsoleKeyInfo mainMenuKeys = ReadKey();

                switch (mainMenuKeys.Key)
                {
                    case ConsoleKey.D1:
                        Clear();

                        bool invalidArticleCredentials = true;

                        AddProduct(invalidArticleCredentials);

                        Clear();

                        break;
                    case ConsoleKey.D2:
                        Clear();

                        Write("Search by article number: ");

                        ListTasks();

                        break;
                    case ConsoleKey.D3:
                        Clear();

                        Environment.Exit(0);

                        break;
                }
            } while (mainMenu);
        }

        private static void AddProduct(bool invalidArticleCredentials)
        {
            do
            {
                Write("Article Number: ");
                var articleNumber = ReadLine();

                Write("          Name: ");
                var name = ReadLine();

                Write("   Description: ");
                var description = ReadLine();

                Write("         Price: ");
                var price = ReadLine();

                Console.CursorVisible = false;

                WriteLine(" ");
                WriteLine("Is this correct? (Y)es (N)o");

                ConsoleKeyInfo yesNo = ReadKey(true);

                // I want to run a code here that checks if the registration number already exists
                if (yesNo.Key == ConsoleKey.Y)
                {
                    MyProduct myProduct;

                    myProduct = new MyProduct(articleNumber, name, description, price);

                    InsertMyTask(myProduct);

                    Clear();

                    WriteLine("Article saved.");

                    Thread.Sleep(2000);

                    Clear();

                    break;
                }
                else if (yesNo.Key == ConsoleKey.N)
                {
                    Clear();
                }
            } while (invalidArticleCredentials);
        }

        private static void InsertMyTask(MyProduct myProduct)
        {
            var sql = $@"INSERT INTO Products (ArticleNumber, Name, Description, Price) VALUES (@ArticleNumber, @Name, @Description, @Price)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ArticleNumber", myProduct.articleNumber);
                    command.Parameters.AddWithValue("@Name", myProduct.name);
                    command.Parameters.AddWithValue("@Description", myProduct.description);
                    command.Parameters.AddWithValue("@Price", myProduct.price);

                    connection.Open();

                    command.ExecuteNonQuery();

                    connection.Close();
                }
            }
        }
        private static List<MyProduct> FetchMyTasks()
        {
            string searchByArticleNumber = ReadLine();
            //string sql = "SELECT ArticleNumber, Name, Description, Price FROM Products";
            string sql = $@"SELECT * FROM Products WHERE ArticleNumber = {searchByArticleNumber}";

            List<MyProduct> myProductList = new List<MyProduct>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var articleNumber = (string)reader["ArticleNumber"];
                    var name = (string)reader["Name"];
                    var description = (string)reader["Description"];
                    var price = (int)reader["Price"];

                    myProductList.Add(new MyProduct(articleNumber, name, description, price));
                }

                connection.Close();
            }

            return myProductList;
        }

        private static void UpdateTask()
        {
            WriteLine(" ");

            Write("Enter article number of task to update: ");

            string searchByArticleNumber = ReadLine();

            Clear();

            WriteLine($"Editing Task with Article Number: {searchByArticleNumber}");

            WriteLine(" ");

            Write("Article Number: ");
            var articleNumber = ReadLine();

            Write("          Name: ");
            var name = ReadLine();

            Write("   Description: ");
            var description = ReadLine();

            Write("         Price: ");
            int price = Convert.ToInt32(ReadLine());

            //var sql = $@"UPDATE Products SET ArticleNumber = {articleNumber}, Name = {name}, Description = {description}, Price = {price} WHERE ArticleNumber={searchByArticleNumber}";

            var sql = $@"UPDATE Products SET ArticleNumber = @ArticleNumber, Name = @Name , Description = @Description, Price = @Price WHERE ArticleNumber = {searchByArticleNumber} ";

            Console.CursorVisible = false;

            WriteLine(" ");
            WriteLine("Is this correct? (Y)es (N)o");

            ConsoleKeyInfo yesNo = ReadKey(true);

            if (yesNo.Key == ConsoleKey.Y)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ArticleNumber", articleNumber);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);

                        connection.Open();

                        command.ExecuteNonQuery();

                        connection.Close();
                    }
                }

                Clear();

                WriteLine("Article updated.");

                Thread.Sleep(2000);

                Clear();
            }
            else if (yesNo.Key == ConsoleKey.N)
            {
                Clear();
            }

            Clear();

        }

        private static void DeleteTask()
        {
            WriteLine(" ");

            Write("Enter article number of task to delete: ");

            string searchByArticleNumber = ReadLine();

            //string sql = "SELECT ArticleNumber, Name, Description, Price FROM Products";

            string sql = $@"DELETE FROM Products WHERE ArticleNumber = {searchByArticleNumber}";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                connection.Close();
            }

            Clear();

            WriteLine("Article deleted.");

            Thread.Sleep(2000);

            Clear();
        }

        private static void ListTasks()
        {
            var myProductList = FetchMyTasks();

            WriteLine(" ");

            foreach (var myProduct in myProductList)
            {
                WriteLine($" Article Number: {myProduct.articleNumber}");

                WriteLine($"           Name: {myProduct.name}");

                WriteLine($"    Description: {myProduct.description}");

                WriteLine($"          Price: {myProduct.price}");

                WriteLine("___________________________________________________");

                WriteLine(" ");
            }

            WriteLine(" [E] Edit [D] Delete [Esc] Main Menu");

            ConsoleKeyInfo escDeleteEdit = ReadKey(true);

            if (escDeleteEdit.Key == ConsoleKey.Escape)
            {
                Clear();

            }
            else if (escDeleteEdit.Key == ConsoleKey.E)
            {
                UpdateTask();
            }
            else if (escDeleteEdit.Key == ConsoleKey.D)
            {
                DeleteTask();
            }

            Clear();
        }

    }
}
