using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternWithUOW.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IBaseRepository<Author> _authorRepository;

    public AuthorsController(IBaseRepository<Author> authorRepository)
    {
        _authorRepository = authorRepository;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = _authorRepository.GetById(id);

        if (item is null) return NotFound();

        return Ok(item);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _authorRepository.GetAll();
        return Ok(items);
    }

    [HttpGet("NameFind/{name}")]
    public IActionResult GetByNameWithBooks(string name)
    {
        var item = _authorRepository.Find(x => x.Name == name);

        if (item is null) return NotFound();

        return Ok(item);
    }

    [HttpGet("ByName/{string name}")]
    public IActionResult GetAllByName(string name)
    {
        var items = _authorRepository.FindAll(x => x.Name.Contains(name));

        if (items is null) return NotFound();

        return Ok(items);
    }
}
