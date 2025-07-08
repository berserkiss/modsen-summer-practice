using Microsoft.EntityFrameworkCore;
using EntityFramework.Models;

namespace EntityFramework.Data;

public class SchoolContext : DbContext
{
    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) {}
    public DbSet<Student> Students => Set<Student>();
}