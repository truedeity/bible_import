using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;

namespace BibleImport
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";

            WebClient client = new WebClient();
            Stream stream = client.OpenRead("https://www.o-bible.com/download/kjv.txt");
            StreamReader reader = new StreamReader(stream);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("INSERT INTO KJV_BibleRaw (Text) VALUES (@text)", connection))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@text", line);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
