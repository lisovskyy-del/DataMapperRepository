using RepositoryPatternDemo.Persistence.Entities;

namespace RepositoryPatternDemo.Persistence.Repositories.Contracts;

internal interface IPostRepository : IRepository<Post>
{
    Post? GetBySlug(string slug);

    IEnumerable<Post> GetByUserId(Guid userId);

    IEnumerable<Post> GetByTitle(string title);
}
