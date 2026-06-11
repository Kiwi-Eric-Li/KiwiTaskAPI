namespace KiwiTaskAPI.Dtos
{
    public class OfferAcceptedEventDto
    {
        public Guid task_id { get; set; }
        public int offer_id { get; set; }
        public Guid tasker_id { get; set; }
        public DateTime matched_at { get; set; }
        public DateTime confirm_expires { get; set; }
    }
}
