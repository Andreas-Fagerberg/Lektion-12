public class UserEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
}

public class UserDto
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
