using System;
using System.IO;
using System.Collections.Generic;
using SqlDataTransfer.SqlServer;

namespace SqlDataTransfer.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test();

            try
            {
                Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Console.WriteLine("\nFinished");
            Console.ReadKey();
        }

        private static void Run()
        {
            Console.Write("DB Connection String: ");
            var connectionString = Console.ReadLine();
            var sqlJsonConverter = new SqlServerJsonConverter(connectionString);

            Console.WriteLine("Database Tables: ");
            var tableNames = sqlJsonConverter.GetAllTableNames();
            var tableNamesDictionary = new Dictionary<int, string>();

            int count = 0;
            foreach (var tableName in tableNames)
            {
                count++;
                Console.WriteLine($"{count}. {tableName}");
                tableNamesDictionary.Add(count, tableName);
            }

            Console.Write("Please choose table: ");
            int choosedTable = int.Parse(Console.ReadLine());
            Console.WriteLine($"Choosed table: {tableNamesDictionary[choosedTable]}");

            Console.WriteLine($"Please choose operation: ");
            Console.WriteLine("1. Get sql table scheme in json format");
            Console.WriteLine("2. Get sql table records in json format");
            Console.WriteLine("3. Get sql table scheme and records (combined) in json format");
            int choosedOperation = int.Parse(Console.ReadLine());

            Console.Write("Please enter output json file name: ");
            var outputFileName = Console.ReadLine();

            switch (choosedOperation)
            {
                case 1:
                    {
                        var tableSchemeJson = sqlJsonConverter.GetTableJsonScheme(tableNamesDictionary[choosedTable]);
                        File.WriteAllText(outputFileName, tableSchemeJson);
                        break;
                    }
                case 2:
                    {
                        var recordsJson = sqlJsonConverter.GetRecordsJson(tableNamesDictionary[choosedTable]);
                        File.WriteAllText(outputFileName, recordsJson);
                        break;
                    }
                case 3:
                    {
                        var tableRecordsJson = sqlJsonConverter.GetTableRecordsJson(tableNamesDictionary[choosedTable]);
                        File.WriteAllText(outputFileName, tableRecordsJson);
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid input");
                        break;
                    }
            }

            Console.WriteLine($"All operations finished and saved to file {outputFileName}");
        }

        private static void Test()
        {
            string connectionString = "Server=(localdb)\\ProjectsV14;Database=Sample;Trusted_Connection=True;MultipleActiveResultSets=true";
            var sqlJsonConverter = new SqlServerJsonConverter(connectionString);
            var tableNames = sqlJsonConverter.GetAllTableNames();

            foreach (var tableName in tableNames)
            {
                Console.WriteLine(tableName);
            }

            //Console.WriteLine(sqlJsonConverter.GetTableRecords(tableNames[0]).GetHashCode());
            File.WriteAllText("test_table_combined.json", sqlJsonConverter.GetTableRecordsJson(tableNames[0]));            
        }
    }
}
