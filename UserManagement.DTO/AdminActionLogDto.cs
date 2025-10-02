using System; 
public class AdminActionLogDto
{
    public int LogID { get; set; }
    public int AdminUserID { get; set; }
    public int TargetUserID { get; set; }
    public string? ActionDescription { get; set; }
    public DateTime? ActionDate { get; set; }

    // loging
    public string? AdminUserLogin { get; set; }
    public string? TargetUserLogin { get; set; }
}