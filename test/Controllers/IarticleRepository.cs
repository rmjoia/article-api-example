namespace test.Controllers
{
    public interface IArticleRepository
    {
        Article Add(Article article);
        bool Delete(Guid id);
        Article? Get(Guid id);
        IEnumerable<Article> GetAll();
        bool Update(Article article);
    }
}