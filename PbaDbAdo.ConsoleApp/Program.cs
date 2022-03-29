using System;
using System.Data;
using System.Data.SqlClient;

namespace PbaDbAdo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {   
            var connectionString = "Server=PIXELFACTORY\\SQLEXPRESS;Database=eksempeldb;User Id=martin;Password=Password1;";

            Opgave831(connectionString);

            Console.ReadLine();
        }

        private static void Opgave831(string connectionString)
        {
            Console.Write("Indtast CPR nummer: ");
            var cpr = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(
                    connectionString))
            {
                var command = new SqlCommand("SELECT navn, stilling, loen FROM person WHERE cpr = @cpr", connection);
                command.Parameters.Add(new SqlParameter("cpr", cpr));

                command.Connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("Navn: {0}, stilling: {1}, løn: {2}", reader[0], reader[1], reader[2]));
                    }
                }

                Console.Write("Indtast ny løn: ");
                var newSalary = Console.ReadLine();

                command = new SqlCommand("UPDATE person set loen = @newSalary WHERE cpr = @cpr", connection);
                command.Parameters.Add(new SqlParameter("cpr", cpr));
                command.Parameters.Add(new SqlParameter("newSalary", newSalary));

                command.ExecuteNonQuery();

                command = new SqlCommand("SELECT navn, stilling, loen FROM person WHERE cpr = @cpr", connection);
                command.Parameters.Add(new SqlParameter("cpr", cpr));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(String.Format("Navn: {0}, stilling: {1}, løn: {2}", reader[0], reader[1], reader[2]));
                    }
                }
            }
        }
    }
}
