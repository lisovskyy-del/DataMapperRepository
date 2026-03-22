using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternDemo.Persistence.Repositories.AdoImpl;

internal class PostAdoRepository :
    GenericAdoRepository<Post>,
    IPostRepository
{
    public PostAdoRepository(ConnectionManager connectionManager) :
        base(connectionManager, "Posts")
    { }

    public Post? GetBySlug(string slug)
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

    public IEnumerable<Post> GetByUserId(Guid userId)
    {
        string query = $"SELECT * FROM {TableName} WHERE UserId = @UserId;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var userIdParameter = command.CreateParameter();
        userIdParameter.ParameterName = "@UserId";
        userIdParameter.Value = userId;
        command.Parameters.Add(userIdParameter);

        var items = new List<Post>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            items.Add(Map(reader));

        return items;
    }

    public IEnumerable<Post> GetByTitle(string title)
    {
        string query = $"SELECT * FROM {TableName} WHERE Title LIKE @Title;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = query;
        var titleParameter = command.CreateParameter();
        titleParameter.ParameterName = "@Title";
        titleParameter.Value = $"%{title}%";
        command.Parameters.Add(titleParameter);

        var items = new List<Post>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            items.Add(Map(reader));

        return items;
    }
}
