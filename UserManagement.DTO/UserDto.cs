public class UserDto
{
    public int UserID { get; set; }
    public string? Login { get; set; }        // Додай ?
    public string? FirstName { get; set; }    // Додай ?
    public string? LastName { get; set; }     // Додай ?
    public bool IsActive { get; set; }
    public int RoleID { get; set; }
    public string? RoleName { get; set; }     // Додай ?
}