using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternDemo.Persistence.Repositories.AdoImpl;

internal class CommentAdoRepository :
    GenericAdoRepository<Comment>,
    ICommentRepository
{
    public CommentAdoRepository(ConnectionManager connectionManager) :
        base(connectionManager, "Comments")
    { }

    public IEnumerable<Comment> GetByUserId(Guid userId)
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

        var items = new List<Comment>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            items.Add(Map(reader));

        return items;
    }

    public IEnumerable<Comment> GetByPostId(Guid postId)
    {
        string query = $"SELECT * FROM {TableName} WHERE PostId = @PostId;";

        using var connection = ConnectionManager.GetConnection();
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = query;
        var postIdParameter = command.CreateParameter();
        postIdParameter.ParameterName = "@PostId";
        postIdParameter.Value = postId;
        command.Parameters.Add(postIdParameter);

        var items = new List<Comment>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
            items.Add(Map(reader));

        return items;
    }
}
