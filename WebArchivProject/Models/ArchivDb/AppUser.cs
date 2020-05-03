﻿namespace WebArchivProject.Models.ArchivDb
{
    public class AppUser
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
    }
}