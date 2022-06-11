using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class User
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("full_name")]
        public string FullName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("url_avatar")]
        public string UrlAvatar { get; set; }

        [Column("api_token")]
        public string ApiToken { get; set; }

        [Column("device_token")]
        public string DeviceToken { get; set; }

        [Column("trial_ends_at")]
        public DateTime? TrialEndsAt { get; set; }

        [Column("remember_token")]
        public string RememberToken { get; set; }

        [Column("is_active")]
        public bool IsActive { get; internal set; }

        [Column("created_at")]
        public DateTime CreatedDate { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedDate { get; set; }
    }
}
