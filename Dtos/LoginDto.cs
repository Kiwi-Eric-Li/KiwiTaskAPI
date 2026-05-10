namespace KiwiTaskAPI.Dtos
{
    public class LoginDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool remember_me { get; set; }
    }
}
