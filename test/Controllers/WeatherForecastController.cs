using Microsoft.AspNetCore.Mvc;

namespace test.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{


    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IArticleRepository _repo;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IArticleRepository articleRepository)
    {
        _logger = logger;
        _repo = articleRepository;
    }

    [HttpPost("articles")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Article))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateArticle([FromBody] Article article)
    {
        if (string.IsNullOrEmpty(article.Title))
        {
            return BadRequest();
        }

        var newArticle = _repo.Add(article);

        var url = $"/api/articles/{newArticle.Id}";

        return new CreatedAtRouteResult(nameof(GetArticle), new { id = newArticle.Id }, newArticle);
    }

    [HttpGet("articles/{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Article))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetArticle(Guid id)
    {
        var article = _repo.Get(id);

        if(article == null)
        {
            return NotFound();
        }

        return Ok(article);
    }

    [HttpPut("articles/{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateArticle(Guid id, Article article)
    {
        var oldArticle = _repo.Get(id);

        if (string.IsNullOrEmpty(article.Title))
        {
            return BadRequest();
        }

        if (oldArticle == null)
        {
            return NotFound();
        }

        _repo.Update(oldArticle);

        return Ok();
    }

    [HttpDelete("articles/{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteArticle(Guid id)
    {
        var oldArticle = _repo.Get(id);

        if (oldArticle == null)
        {
            return NotFound();
        }

        _repo.Delete(id);

        return Ok();
    }
}

public class Article
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public object Descripton { get; set; }
}

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

