using Data.DataAccess;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Data.Models
{
    public class AccountSignInDTO
    {
        [SwaggerSchema(Description = "UserName", Nullable = false)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*UserID* is required")]
        public string UserID { get; set; } = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Password* is required")]
        [SwaggerSchema(Description = "Password", Nullable = false)]
        public string Password { get; set; } = "";
        [IgnoreDataMemberAttribute]
        [HideProperty(IsHideSerialize = true, IsCheckHideSerializeFromPublicObject = true)]
        public bool RememberMe { get; set; } = false;
    }
    public class LoginRequestDto
    {
        public string UserID { get; set; } = "";
        public string Password { get; set; } = "";
    }
    public class AccountSignInResDTO
    {
        [SwaggerSchema(Description = "true = Successful, false = Failed/Error", Nullable = false)]
        public bool ResponseCode { get; set; } = false;
        [SwaggerSchema(Nullable = false)]
        public string UserName { get; set; } = "";
        [SwaggerSchema(Nullable = false)]
        public string JWToken { get; set; } = "";
        [SwaggerSchema(Description = "null in case of *responsecode* is false. Datetime will be UTC TimeZone", Format = "yyyy-MM-dd HH:mm:ss.fff", Nullable = true)]
        public string? JWTokenExpiry { get; set; } = null;

        [SwaggerSchema(Nullable = false)]
        public string RefreshToken { get; set; } = "";
        
        [SwaggerSchema(Nullable = false, Description = "empty in case of ResponseCode is true")]
        public string ErrorMsg { get; set; } = "";
        
        [SwaggerSchema(Nullable = false, Description = "empty in case of ResponseCode is true")]
        public string ErrorCode { get; set; } = "";
        [SwaggerSchema(Nullable = false)]
        [IgnoreDataMemberAttribute]
        [HideProperty(IsHideSerialize = true, IsCheckHideSerializeFromPublicObject = true, IsCheckHideSerializeFromPublicObjectList = true)]
        public bool RememberMe { get; set; } = false;
    }
    public class RefreshTokenRequestDTO
    {
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "*token* is Required")]
        public string token { get; set; } = "";
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "*refreshtoken* is Required")]
        public string refreshToken { get; set; } = "";
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "*userid* is Required")]
        public string userID { get; set; } = "";
    }
    public class RefreshTokenResDTO
    {
        [SwaggerSchema(Description = "true = Successful, false = Failed/Error", Nullable = false)]
        public bool ResponseCode { get; set; } = false;
        [SwaggerSchema(Nullable = false)]
        public string JWToken { get; set; } = "";
        [SwaggerSchema(Description = "null in case of *responsecode* is false. Datetime will be UTC TimeZone", Format = "yyyy-MM-dd HH:mm:ss.fff", Nullable = true)]
        public string? JWTokenExpiry { get; set; } = null;
        [SwaggerSchema(Nullable = false)]
        public string RefreshToken { get; set; } = "";
        
        [SwaggerSchema(Nullable = false, Description = "empty in case of ResponseCode is true")]
        public string ErrorMsg { get; set; } = "";
        
        [SwaggerSchema(Nullable = false, Description = "empty in case of ResponseCode is true")]
        public string ErrorCode { get; set; } = "";
    }
    public class CacheRequestJson
    {
        
        [SwaggerSchema(Required = new[] {
            "TConfig",
        "TAPIUserMapRequestLimit",
        "MetropolitanUserAPIMap",
        "TAPIRemoteDomainIPRequestLimit",
        "TApiRemoteDomainIPWhiteListing"
        })]
        public List<string>? type_ { get; set; } = null;
        
        [SwaggerSchema(Required = new[] {
        "P_Get_API_User_Map_Request_Limit",
        "P_Get_API_User_Map",
        "P_Get_API_RemoteDomain_IP_Request_Limit",
        "P_Get_User_Info",
        "P_Get_API_RemoteDomain_IP_WhiteListing"
        })]
        public List<string>? subtype_ { get; set; } = null;
        public List<string>? key { get; set; } = null;
    }
    public class CacheResponseJson
    {
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_msg { get; set; } = "";
        public int entriesdeleted { get; set; } = 0;
        
        [SwaggerSchema(Description = "null in case of *response_code* is false or no warnings", Nullable = true)]
        public List<string>? warningtext { get; set; } = null;
        [SwaggerSchema(Description = "null in case of *response_code* is false or no key found", Nullable = true)]
        
        public List<string>? keylist { get; set; } = null;
        [SwaggerSchema(Description = "null in case of *response_code* is false or no subtype found", Nullable = true)]
        
        public List<string>? subtypelist { get; set; } = null;
    }
}
