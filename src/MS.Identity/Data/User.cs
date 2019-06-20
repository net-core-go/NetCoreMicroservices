using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    }
    public class Claim
    {
        [Key]
        public int ClaimId{get;set;}
        public string Type{get;set;}
        public string Value{get;set;}
        public virtual User User{get;set;}
    }
}