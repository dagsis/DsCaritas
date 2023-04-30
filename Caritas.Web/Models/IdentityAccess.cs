﻿namespace Caritas.Web.Models
{
    public class IdentityAccess
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Nombre { get; set; }
        public string ImageUrl { get; set; }
        public string Token { get; set; }
        public int CompaniaId { get; set; }

        public ICollection<string> Roles { get; set; }
    }
}
