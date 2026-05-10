namespace KiwiTaskAPI.Dtos
{
    public class RegisterDto
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password_hash { get; set; }
    }
}
