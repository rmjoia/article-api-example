namespace test.Controllers;

public class ArticleRepository : IArticleRepository
{
    private readonly List<Article> _repo = new List<Article>();

    public Article Add(Article article)
    {
        var newArticle = new Article
        {
            Id = Guid.NewGuid(),
            Title = article.Title,
            Descripton = article.Descripton
        };

        _repo.Add(newArticle);
        return newArticle;
    }

    public Article? Get(Guid id) => _repo.FirstOrDefault(a => a.Id == id);

    public IEnumerable<Article> GetAll()
    {
        return _repo;
    }


    public bool Update(Article article)
    {
        var oldArticle = Get(article.Id);
        var oldArticleIndex = _repo.FindIndex(a => a.Id == article.Id);

        if (oldArticle == null)
        {
            return false;
        }
        else
        {
            oldArticle.Title = article.Title;
            oldArticle.Descripton = article.Descripton;

            _repo.RemoveAt(oldArticleIndex);
            _repo.Insert(oldArticleIndex, oldArticle);

            return true;
        }

    }

    public bool Delete(Guid id)
    {
        var article = Get(id);
        if (article == null)
        {
            return false;
        }
        _repo.Remove(article);
        return true;
    }
}

