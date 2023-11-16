using MyApp.ServiceModel.Types;

namespace MyApp.Data;

/// <summary>
/// Example of using separate DataModel and DTO
/// </summary>
public class Contact
{
    public int Id { get; set; }
    public string UserAuthId { get; set; } = default!;
    public Title Title { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
    public FilmGenre? FavoriteGenre { get; set; }
    public int Age { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
