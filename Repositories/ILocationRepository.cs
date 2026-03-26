using Locally.Models;

namespace Locally.Repositories;

public interface ILocationRepository
{
    List<Location> GetByUser(string userId);
    Location? GetById(int id);
    void Add(Location location);
    void Update(Location location);
    void Delete(int id);
}