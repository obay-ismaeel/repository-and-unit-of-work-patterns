using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Interfaces;
using RepositoryPatternWithUOW.Core.Models;

namespace RepositoryPatternWithUOW.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthorsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = _unitOfWork.Authors.GetById(id);

        if (item is null) return NotFound();

        return Ok(item);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _unitOfWork.Authors.GetAll();
        return Ok(items);
    }

    [HttpPost]
    public IActionResult Create(Author authorDto)
    {
        var author = _unitOfWork.Authors.Add(authorDto);
        _unitOfWork.Complete();
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Author author)
    {

        if(author.Id != id)
        {
            return Conflict("IDs don't match");
        }

        var dbAuthor = _unitOfWork.Authors.GetById(id);

        if(dbAuthor is null) return NotFound();

        _unitOfWork.Authors.Update(author);
        _unitOfWork.Complete();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var author = _unitOfWork.Authors.GetById(id);

        if(author is null) return NotFound();

        _unitOfWork.Authors.Delete(author);
        _unitOfWork.Complete();
        return NoContent();
    }
}
