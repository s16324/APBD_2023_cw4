using cwiczenia_4_s16324.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace cwiczenia_4_s16324.Services
{

    public interface IDatabaseService
    {
        IEnumerable<Animal> GetAnimals();
        int UpdateAnimals(Animal updatedAnimal, int id);
        int AddAnimal(Animal newAnimal);
        IEnumerable<Animal> GetAnimals(string sortBy);
        int DeleteAnimal(int id);
    }
    public class MockDatabaseService : IDatabaseService
    {
        public static List<Animal> Animals { get; set; } = new List<Animal>();
        public int AddAnimal(Animal newAnimal)
        {
            Animals.Add(newAnimal);
            return 1;
        }

        public IEnumerable<Animal> GetAnimals()
        {
            return Animals;
        }

        public int UpdateAnimals(Animal updatedAnimal, int id)
        {
            return 1;

        }
        public IEnumerable<Animal> GetAnimals(string sortBy)
        {
            return null;
        }
        public int DeleteAnimal(int id)
        {
            return 1;
        }
    }

    public class SqlServerDatabaseService : IDatabaseService {

        private IConfiguration _configuration;

        public SqlServerDatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int AddAnimal(Animal newAnimal)
        {
            var res = new List<Animal>();
            int RowsAffected = 0;
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "insert into Animal(Name, Description, Category, Area) " +
                    "values(@Name, @Description, @Category, @Area)";
                com.Parameters.AddWithValue("@Name", newAnimal.Name);
                com.Parameters.AddWithValue("@Description", newAnimal.Description);
                com.Parameters.AddWithValue("@Category", newAnimal.Category);
                com.Parameters.AddWithValue("@Area", newAnimal.Area);
                con.Open();
                RowsAffected = com.ExecuteNonQuery();
                
            }
            return RowsAffected;
        }

        public IEnumerable<Animal> GetAnimals(string orderBy)
        {
            var res = new List<Animal>();
            using(SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;

                if (orderBy == "IdAnimal")
                {
                    com.CommandText = "SELECT * FROM Animal ORDER BY IdAnimal";
                }
                else
                {
                    com.CommandText = "SELECT * FROM Animal " +
                    "ORDER BY CASE @orderBy " +
                    "WHEN 'Name' THEN Name " +
                    "WHEN 'Description' THEN Description " +
                    "WHEN 'Category' THEN Category " +
                    "WHEN 'Area' THEN Area " +
                    "ELSE 'Name' " +
                    "END";
                }
                
                com.Parameters.AddWithValue("@orderBy", orderBy);
                
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    res.Add(new Animal
                    {
                        Id=Int32.Parse(dr["IdAnimal"].ToString()),
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        Category = dr["Category"].ToString(),
                        Area = dr["Area"].ToString()
                    }) ;
                }
            }


            return res;
        }

        public int UpdateAnimals(Animal updatedAnimal, int id)
        {
            var res = new List<Animal>();
            int RowsAffected = 0;
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "UPDATE Animal SET Name=@Name, Description=@Description, Category=@Category, Area=@Area WHERE IdAnimal=@Id";
                com.Parameters.AddWithValue("@Name", updatedAnimal.Name);
                com.Parameters.AddWithValue("@Description", updatedAnimal.Description);
                com.Parameters.AddWithValue("@Category", updatedAnimal.Category);
                com.Parameters.AddWithValue("@Area", updatedAnimal.Area);
                com.Parameters.AddWithValue("@Id", id);
                con.Open();
                RowsAffected = com.ExecuteNonQuery();

            }
            return RowsAffected;
        }

        public IEnumerable<Animal> GetAnimals()
        {
            var res = new List<Animal>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "SELECT * FROM Animal ORDER BY Name";
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    res.Add(new Animal
                    {
                        Id = Int32.Parse(dr["IdAnimal"].ToString()),
                        Name = dr["Name"].ToString(),
                        Description = dr["Description"].ToString(),
                        Category = dr["Category"].ToString(),
                        Area = dr["Area"].ToString()
                    });
                }
            }

            return res;
        }

        public int DeleteAnimal(int id)
        {
            var res = new List<Animal>();
            int RowsAffected = 0;
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
            {
                SqlCommand com = new SqlCommand();
                com.Connection = con;
                com.CommandText = "DELETE FROM Animal WHERE IdAnimal=@Id";
                com.Parameters.AddWithValue("@Id", id);
                con.Open();
                RowsAffected = com.ExecuteNonQuery();

            }
            return RowsAffected;
        }

    }

}
