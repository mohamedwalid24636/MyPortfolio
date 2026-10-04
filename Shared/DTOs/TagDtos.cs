namespace Shared.DTOs;

public class TagDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
}

public class CreateTagDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
}

public class UpdateTagDto
{
    public string Name { get; set; }
    public string Slug { get; set; }
}
