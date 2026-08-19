using api.Dtos;

namespace api.Interfaces;

public interface IOwnerInterface
{
    Task<IEnumerable<Owner>> ListAllOwners();
    Task<Owner?> GetOwnerById(int id);
    Task<Owner?> CreateOwner(Owner owner);
    Task<Owner?> UpdateOwner(int id, Owner owner);
    Task<bool?> DeleteOwner(int id);
}
