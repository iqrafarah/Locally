using Microsoft.Data.SqlClient;
using Locally.Models;

namespace Locally.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly string _connectionString = string.Empty;

    public LocationRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
    }

    public List<Location> GetByUser(string userId)
    {
        List<Location> locations = new();

        using SqlConnection conn = new(_connectionString);
        conn.Open();

        string query = "SELECT * FROM Locations WHERE UserId = @UserId";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@UserId", userId);

        using SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            locations.Add(new Location
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString()!,
                Website = reader["Website"]?.ToString(),
                City = reader["City"].ToString()!,
                Category = reader["Category"].ToString()!,
                Description = reader["Description"]?.ToString(),
                Status = Enum.Parse<LocationStatus>(reader["Status"].ToString()!)
            });
        }

        return locations;
    }

    public void Add(Location location)
    {
        using SqlConnection conn = new(_connectionString);
        conn.Open();

        string query = @"
            INSERT INTO Locations 
            (Name, Website, City, Category, PriceFrom, PriceTo, Description, Status, UserId, CreatedAt)
            VALUES 
            (@Name, @Website, @City, @Category, @PriceFrom, @PriceTo, @Description, @Status, @UserId, @CreatedAt)";

        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@Name", location.Name);
        cmd.Parameters.AddWithValue("@Website", (object?)location.Website ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@City", location.City);
        cmd.Parameters.AddWithValue("@Category", location.Category);
        cmd.Parameters.AddWithValue("@PriceFrom", (object?)location.PriceFrom ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@PriceTo", (object?)location.PriceTo ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Description", (object?)location.Description ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", location.Status.ToString());
        cmd.Parameters.AddWithValue("@UserId", location.UserId);
        cmd.Parameters.AddWithValue("@CreatedAt", location.CreatedAt);

        cmd.ExecuteNonQuery();
    }

    public Location? GetById(int id)
    {
        using SqlConnection conn = new(_connectionString);
        conn.Open();

        string query = "SELECT * FROM Locations WHERE Id = @Id";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            return new Location
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString()!,
                City = reader["City"].ToString()!,
                Category = reader["Category"].ToString()!
            };
        }

        return null;
    }

    public void Update(Location location)
    {
        using SqlConnection conn = new(_connectionString);
        conn.Open();

        string query = @"
            UPDATE Locations 
            SET Name=@Name, Website=@Website, City=@City, Category=@Category, Description=@Description
            WHERE Id=@Id";

        using SqlCommand cmd = new(query, conn);

        cmd.Parameters.AddWithValue("@Id", location.Id);
        cmd.Parameters.AddWithValue("@Name", location.Name);
        cmd.Parameters.AddWithValue("@Website", (object?)location.Website ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@City", location.City);
        cmd.Parameters.AddWithValue("@Category", location.Category);
        cmd.Parameters.AddWithValue("@Description", (object?)location.Description ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using SqlConnection conn = new(_connectionString);
        conn.Open();

        string query = "DELETE FROM Locations WHERE Id = @Id";

        using SqlCommand cmd = new(query, conn);
        cmd.Parameters.AddWithValue("@Id", id);

        cmd.ExecuteNonQuery();
    }
}