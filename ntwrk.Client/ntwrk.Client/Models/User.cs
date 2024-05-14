﻿namespace ntwrk.Client.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string LoginId { get; set; } = null!;

        public string AvatarSourceName { get; set; } = null!;
        public bool IsOnline { get; set; }
        public DateTime LastLogonTime { get; set; }
        public bool IsAway { get; set; }
        public string AwayDuration { get; set; } = null!;
    }
}
