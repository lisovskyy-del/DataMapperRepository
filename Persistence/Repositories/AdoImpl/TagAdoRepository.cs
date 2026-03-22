using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.FileImpl;
using RepositoryPatternDemo.Persistence.Repositories.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternDemo.Persistence.Repositories.AdoImpl;

internal class TagAdoRepository :
    GenericAdoRepository<Tag>,
    ITagRepository
{
    private readonly PostAdoRepository _postRepository; // в інакшому випадку GetPosts не запрацює

    public TagAdoRepository(ConnectionManager connectionManager, PostAdoRepository postRepository) :
        base(connectionManager, "Tags")
    {
        _postRepository = postRepository;
    }

    public Tag? GetBySlug(string slug)
    {
        string query = $"SELECT * FROM {TableName} WHERE Slug = @Slug;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var slugParameter = command.CreateParameter();
        slugParameter.ParameterName = "@Slug";
        slugParameter.Value = slug;
        command.Parameters.Add(slugParameter);

        using var reader = command.ExecuteReader();

        return reader.Read() ? Map(reader) : default;
    }

    public IEnumerable<Post> GetPosts(Tag tag)
    {
        if (tag?.Id == null) return Enumerable.Empty<Post>();
        return _postRepository.GetPostsByTagId(tag.Id.Value);
    }
}
