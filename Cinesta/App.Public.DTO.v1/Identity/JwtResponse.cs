namespace App.Public.DTO.v1.Identity;

public class JwtResponse
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public IList<string>  Roles = default!;
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}