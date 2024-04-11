using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternWithUOW.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public BookController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var item = _unitOfWork.Books.GetById(id);

        if (item is null) return NotFound();

        return Ok(item);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var items = _unitOfWork.Books.GetAll();
        return Ok(items);
    }

    [HttpPost]
    public IActionResult Create(Book bookDto)
    {
        var author = _unitOfWork.Authors.GetById(bookDto.AuthorId);
        
        if(author is null) return NotFound();
        
        var book = _unitOfWork.Books.Add(bookDto);
        _unitOfWork.Complete();
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Book book)
    {

        if (book.Id != id)
        {
            return Conflict("IDs don't match");
        }

        var dbBook = _unitOfWork.Books.GetById(id);

        if (dbBook is null) return NotFound();

        var author = _unitOfWork.Authors.GetById(book.AuthorId);

        if (author is null) return NotFound();

        _unitOfWork.Books.Update(book);
        _unitOfWork.Complete();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var book = _unitOfWork.Books.GetById(id);

        if (book is null) return NotFound();

        _unitOfWork.Books.Delete(book);
        _unitOfWork.Complete();
        return NoContent();
    }
}
