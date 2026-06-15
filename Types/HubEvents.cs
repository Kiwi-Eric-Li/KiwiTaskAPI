namespace KiwiTaskAPI.Types
{
    public static class HubEvents
    {

        public const string TaskOfferNew = "task.offer.new";  // new a task
        public const string TaskOfferAccepted = "task.offer.accepted";  // an offer is chosen as a preferred offer
        public const string TaskOfferCancelled = "task.offer.canceled";  // this preferred offer is cancelled
        public const string JoinedTask = "joined.task";
        public const string LeftTask = "left.task";
        public const string JoinedFeed = "joined.feed";
        public const string LeftFeed = "left.feed";
        
    }
}
