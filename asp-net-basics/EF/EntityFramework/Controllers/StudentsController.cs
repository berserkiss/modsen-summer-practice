using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntityFramework.Data;
using EntityFramework.Models;

namespace EntityFramework.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly SchoolContext _context;

    public StudentsController(SchoolContext context) => _context = context;

    [HttpGet]
    public async Task<List<Student>> Get() => await _context.Students.ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> Get(int id)
    {
        var student = await _context.Students.FindAsync(id);
        return student == null ? NotFound() : student;
    }

    [HttpPost]
    public async Task<Student> Post(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return student;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Student student)
    {
        if (id != student.Id) return BadRequest();
        _context.Entry(student).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _context.Students.Remove(new Student { Id = id });
        await _context.SaveChangesAsync();
        return NoContent();
    }
}