using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Test
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {

            var connString = "Server=localhost;User ID=root;Password=root;Database=test";

            using (var conn = new MySqlConnection(connString))
            {
                await conn.OpenAsync();
                // Retrieve all rows
                using (var cmd = new MySqlCommand("SELECT * FROM hy_area", conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.WriteLine(reader.GetName(i) + ":" + reader.GetValue(i));
                            }

                        }
                    }

                }

            }
            Console.ReadLine();
        }
    }
}
