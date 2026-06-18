namespace KiwiTaskAPI.Types
{
    public static class HubGroups
    {
        public const string FeedAll = "feed.all";
        public static string Task(Guid taskid)
        {
            return $"task:{taskid}";
        }

        public static string User(Guid userid)
        {
            return $"user:{userid}";
        }
    }
}
