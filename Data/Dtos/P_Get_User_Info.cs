using Data.DataAccess;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Data.Dtos
{
    public class P_Get_User_Info : P_Get_User_Info_SP
    {
        public string ip_address { get; set; } = "";
        public Int64 local_timezoneoffset { get; set; } = 0;
        public int local_timezone { get; set; } = 0;
        public string local_timezonename { get; set; } = "";
        public bool issecureconnection { get; set; } = false;
        public string browser { get; set; } = "";
        public bool ismobiledevice { get; set; } = false;
        public string userremotedomain { get; set; } = "";
        public int applicable_tz_id { get; set; } = 0;
        public Int64 applicable_offset { get; set; } = 0;
        public string applicable_timezonename { get; set; } = "";
    }

    public class P_Get_User_Info_SP
    {
        public int User_ID { get; set; }
        public int App_ID { get; set; }
        public string? App_Name { get; set; } = "";
        public string? UserName { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? FullName { get; set; } = "";
        public string? FirstName { get; set; } = "";
        public string? LastName { get; set; } = "";
        public string? UserType { get; set; } = "";
        public string? Department { get; set; } = "";
        public string? Designation { get; set; } = "";
        public string? PasswordExpiryDateTime { get; set; } = "";
        public bool IsBlocked { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public int RoleID { get; set; } = 0;
        public bool IsGroupRoleID { get; set; } = false;
        [JsonIgnore]
        [IgnoreDataMember]
        [NotMapped]
        public string encrypted_key { get; set; } = "";
    }
}
