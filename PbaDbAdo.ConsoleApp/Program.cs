using System;
using System.Data;
using System.Data.SqlClient;

namespace PbaDbAdo.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {   
            var connectionString = "Server=PIXELFACTORYJR\\SQLEXPRESS;Database=eksempeldb;User Id=martin;Password=Password1;";
            
            //EnableAllowSnapshot(connectionString);

            Opgave832D(connectionString);

            Console.ReadLine();
        }

        private static void EnableAllowSnapshot(string connectionString)
        {

            using (SqlConnection connection = new SqlConnection(
                    connectionString))
            {
                connection.Open();
                var command = new SqlCommand("alter database eksempeldb set allow_snapshot_isolation on", connection);
                command.ExecuteNonQuery();
            }
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

        private static void Opgave832A(string connectionString)
        {
            Console.Write("Indtast CPR nummer: ");
            var cpr = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(
                    connectionString))
            {
                var command = new SqlCommand("SELECT navn, stilling, loen FROM person WHERE cpr = @cpr", connection);
                command.Parameters.Add(new SqlParameter("cpr", cpr));

                command.Connection.Open();

                var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);
                command.Transaction = transaction;

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
                
                command.Transaction = transaction;

                command.ExecuteNonQuery();

                transaction.Commit();

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

        private static void Opgave832B(string connectionString)
        {
            Console.Write("Indtast CPR nummer: ");
            var cpr = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(
                    connectionString))
            {
                var command = new SqlCommand("SELECT navn, stilling, loen FROM person WITH(UPDLOCK) WHERE cpr = @cpr", connection); // UPDLOCK
                command.Parameters.Add(new SqlParameter("cpr", cpr));

                command.Connection.Open();

                // var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted); // Ikke løsningen
                var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead); 
                command.Transaction = transaction;

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
                
                command.Transaction = transaction;

                command.ExecuteNonQuery();

                transaction.Commit();

                System.Console.WriteLine("asd");

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

        private static void Opgave832C(string connectionString)
        {
            Console.Write("Indtast CPR nummer: ");
            var cpr = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(
                    connectionString))
            {
                var command = new SqlCommand("SELECT navn, stilling, loen FROM person WHERE cpr = @cpr", connection);
                command.Parameters.Add(new SqlParameter("cpr", cpr));

                command.Connection.Open();

                int loen = 0;
                int nyLoen = 0;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        loen = (int)reader[2];
                        Console.WriteLine(String.Format("Navn: {0}, stilling: {1}, løn: {2}", reader[0], reader[1], reader[2]));
                    }
                }

                Console.Write("Indtast ny løn: ");
                var newSalary = Console.ReadLine();

                command = new SqlCommand("SELECT navn, stilling, loen FROM person WHERE cpr = @cpr", connection);
                command.Parameters.Add(new SqlParameter("cpr", cpr));
                
                var transaction = connection.BeginTransaction(IsolationLevel.RepeatableRead);
                command.Transaction = transaction;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        nyLoen = (int)reader[2];
                    }
                }

                if (loen == nyLoen)
                {
                    command = new SqlCommand("UPDATE person set loen = @newSalary WHERE cpr = @cpr", connection);
                    command.Parameters.Add(new SqlParameter("cpr", cpr));
                    command.Parameters.Add(new SqlParameter("newSalary", newSalary));
                    
                    command.Transaction = transaction;

                    command.ExecuteNonQuery();
                }
                else
                {
                    System.Console.WriteLine("Denied");
                }

                transaction.Commit();

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

        private static void Opgave832D(string connectionString)
        {
            Console.Write("Indtast CPR nummer: ");
            var cpr = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(
                    connectionString))
            {
                var command = new SqlCommand("SELECT navn, stilling, loen FROM person WHERE cpr = @cpr", connection);
                command.Parameters.Add(new SqlParameter("cpr", cpr));

                command.Connection.Open();

                var transaction = connection.BeginTransaction(IsolationLevel.Snapshot);
                command.Transaction = transaction;

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
                
                command.Transaction = transaction;

                command.ExecuteNonQuery();

                transaction.Commit();

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
