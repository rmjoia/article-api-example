using Microsoft.AspNetCore.Mvc;

namespace test.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{


    private readonly ILogger<ArticlesController> _logger;
    private readonly IArticleRepository _repo;

    public ArticlesController(ILogger<ArticlesController> logger, IArticleRepository articleRepository)
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

