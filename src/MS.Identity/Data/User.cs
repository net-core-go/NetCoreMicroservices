using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MS.Identity.Data
{
    public class User
    {
        [Key]
        [MaxLength(50)]
        public string UserId{get;set;}
        public string Name{get;set;}
        public string Password{get;set;}
        public bool IsActive{get;set;}
        public virtual ICollection<Claim> Claims{get;set;}

        [NotMapped]
        public IEnumerable<System.Security.Claims.Claim> IdentityClaims
        {
            get{
                return ConvertToIdentityClaim();
            }
        }

        private IEnumerable<System.Security.Claims.Claim> ConvertToIdentityClaim()
        {
            return Claims.Select(o=>new System.Security.Claims.Claim(o.Type,o.Value));
        }
    }
    public class Claim
    {
        [Key]
        public int ClaimId{get;set;}
        public string Type{get;set;}
        public string Value{get;set;}
        public string UserId{get;set;}
        public virtual User User{get;set;}
    }
}