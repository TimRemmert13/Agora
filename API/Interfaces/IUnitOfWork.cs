using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IArtWorkRepository ArtWorkRepository { get; }
        ILikesRepository LikesRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}