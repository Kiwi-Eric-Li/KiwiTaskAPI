namespace KiwiTaskAPI.Dtos
{
    public class ModifyPasswordDto
    {
        public Guid id { get; set; }
        public string current_pwd { get; set; }
        public string new_pwd { get; set; }
    }
}
