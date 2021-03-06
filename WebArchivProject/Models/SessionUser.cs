﻿namespace WebArchivProject.Models
{
    public class SessionUser
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string SecondName { get; set; }
        public string SurName { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }
        public string AvatarUrl { get; set; }

        public long Expirate { get; set; }
    }
}
