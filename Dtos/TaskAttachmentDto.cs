namespace KiwiTaskAPI.Dtos
{
    public class TaskAttachmentDto
    {
        public int id { get; set; }
        public Guid uploader_id { get; set; }
        public string url { get; set; }
        public Guid ctx_id { get; set; }    // 该附件从属于哪个任务或评论
    }
}
