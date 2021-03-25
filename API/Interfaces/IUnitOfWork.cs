using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IArtWorkRepository ArtWorkRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}