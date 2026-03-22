using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Repositories.Contracts;
using RepositoryPatternDemo.Persistence.Repositories.Generics;
using RepositoryPatternDemo.Persistence.Repositories.Impl;

namespace RepositoryPatternDemo.Persistence.Repositories.FileImpl;

internal class PostFileRepository : GenericFileRepository<Post>, IPostRepository
{
    private readonly UserFileRepository _userFileRepository;
    private readonly TagFileRepository _tagFileRepository;

    public PostFileRepository(
        UserFileRepository userFileRepository,
        TagFileRepository tagFileRepository) : base(Path.Combine("Data", "posts.txt"))
    {
        this._userFileRepository = userFileRepository;
        this._tagFileRepository = tagFileRepository;
    }

    public Post? GetBySlug(string slug)
    {
        return Find(t => t.Slug == slug);
    }

    public IEnumerable<Post> GetByUserId(Guid userId)
    {
        return GetAll().Where(x => x.Id == userId).ToList();
    }

    public IEnumerable<Post> GetByTitle(string title)
    {
        return GetAll().Where(x => x.Title == title).ToList();
    }

    public IEnumerable<Post> GetPostsByTagId(Guid tagId)
    {
        return GetAll()
            .Where(p => p.Tags != null && p.Tags.Any(t => t.Id == tagId))
            .ToList();
    }

    protected override Post DeserializeEntity(string line)
    {
        string[] postFields = line.Split("|");
        Post newPost = new Post(
                id: Guid.Parse(postFields[0]),
                user: _userFileRepository.Get(Guid.Parse(postFields[1])),
                slug: postFields[3],
                title: postFields[4],
                body: postFields[5],
                image: !string.IsNullOrEmpty(postFields[6]) ? postFields[6] : null,
                publishedAt: DateTime.Parse(postFields[7]),
                createdAt: DateTime.Parse(postFields[8]),
                updatedAt: DateTime.Parse(postFields[9])
            );

        List<Tag?> tags = postFields[10].Split(",").Select(t => _tagFileRepository.Get(Guid.Parse(t))).ToList();

        newPost.Tags = tags;
        return newPost;
    }

    protected override string SerializeEntity(Post post)
    {
        string tagIds = string.Join(",", post.Tags.Select(tag => tag.Id));
        return $"{post.Id}|{post.User.Id}|{post.Slug}|{post.Title}|{post.Body}|{post.Image}|{post.PublishedAt}|{post.CreatedAt}|{post.UpdatedAt}|{tagIds}";
    }
}
