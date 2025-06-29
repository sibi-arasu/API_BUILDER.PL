using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace DATA_UTILITY.IL
{
    public class CmdExecuter
    {
        private readonly string _connectionString;
        public CmdExecuter(string connectionsting)
        {
            _connectionString = connectionsting;
        }
        public DataSet ExecuteQuery(string query)
        {
            DataSet dsQuery = new DataSet();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dsQuery);
                    }
                    conn.Close();
                }
            }

            return dsQuery;

        }
        public DataSet ExecuteQueryWithReader(string query)
        {
            DataSet dsQuery = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            do
                            {
                                // Create a new DataTable for each result set
                                DataTable table = new DataTable();
                                table.Load(reader); // Load the reader data into the table
                                dsQuery.Tables.Add(table); // Add the table to the DataSet
                            }
                            while (!reader.IsClosed && reader.NextResult()); // Ensure the reader is open
                        }
                        conn.Close();
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return dsQuery;
        }
        public DataSet ExecuteQueryWithReader(ArrayList queries)
        {
            DataSet dsQuery = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    foreach (string query in queries)
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                do
                                {
                                    // Create a new DataTable for each result set
                                    DataTable table = new DataTable();
                                    table.Load(reader); // Load the reader data into the table
                                    dsQuery.Tables.Add(table); // Add the table to the DataSet
                                }
                                while (!reader.IsClosed && reader.NextResult()); // Ensure the reader is open
                            }
                        }
                    }

                    conn.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
            }

            return dsQuery;
        }


    }
}
