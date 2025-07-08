using System.ComponentModel.DataAnnotations;
namespace ModelBinding.Models;

public class Book
{
    public int Id { get; set; }
        
    [Required(ErrorMessage = "Название книги обязательно")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    public string Title { get; set; }
        
    [Required(ErrorMessage = "Автор обязателен")]
    [StringLength(50, ErrorMessage = "Имя автора не должно превышать 50 символов")]
    public string Author { get; set; }
        
    [Range(1000, 2100, ErrorMessage = "Год должен быть между 1000 и 2100")]
    public int Year { get; set; }
        
    [Range(0.01, 10000, ErrorMessage = "Цена должна быть между 0.01 и 10000")]
    public decimal Price { get; set; }
}