namespace Shared.DTOs;

public class TypeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class CreateTypeDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class UpdateTypeDto
{
    public string Name { get; set; }
    public string Description { get; set; }
}
