using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Data.DataAccess;

namespace Data.Dtos
{
    public delegate string GenrateTokenDelgate(P_Get_User_Info oUser, string Encrypted_Key);

    #region Login
    public class SignIn_Req
    {
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public bool RememberMe { get; set; } = false;
    }
    public class SignIn_Res
    {
        public bool ResponseCode { get; set; } = false;
        public string UserName { get; set; } = "";
        public string JWToken { get; set; } = "";
        public string? JWTokenExpiry { get; set; } = null;
        public string RefreshToken { get; set; } = "";
        public string ErrorMsg { get; set; } = "";
        public string ErrorCode { get; set; } = "";
        [IgnoreDataMemberAttribute]
        public bool RememberMe { get; set; } = false;
        [IgnoreDataMemberAttribute]
        public virtual P_Get_User_Info? UserInfo { get; set; } = null;
    }

    public class RefreshTokenReq
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "*token* is Required")]
        public string token { get; set; } = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "*refreshtoken* is Required")]
        public string refreshToken { get; set; } = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "*userid* is Required")]
        public string UserName { get; set; } = "";
    }
    public class RefreshTokenRes
    {
        public bool ResponseCode { get; set; } = false;
        public string JWToken { get; set; } = "";
        public string? JWTokenExpiry { get; set; } = null;
        public string RefreshToken { get; set; } = "";
        public string ErrorMsg { get; set; } = "";
        public string ErrorCode { get; set; } = "";
        public P_Get_User_Info? UserInfo { get; set; } = null;
    }

    public class P_Get_User_Info
    {
        public int User_ID { get; set; }
        public string? UserName { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? FullName { get; set; } = "";
        public string? FirstName { get; set; } = "";
        public string? LastName { get; set; } = "";
        public string? UserType { get; set; } = "";
        public string? TelegramUserName { get; set; } = "";
        public string? TelegramID { get; set; } = "";
        public string? PasswordExpiryDateTime { get; set; } = "";
        public bool IsTempPassword { get; set; } = false;
        public bool IsBlocked { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public int RoleID { get; set; } = 0;
        public bool IsGroupRoleID { get; set; } = false;
        public bool IsApplicationAccessAllowed { get; set; } = false;
        [JsonIgnore]
        [IgnoreDataMember]
        [NotMapped]
        public string encrypted_key { get; set; } = "";
    }
    public class P_UserLoginPasswordModel
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
    public class RefreshToken
    {
        public string JWToken { get; set; } = "";
        public DateTime? Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Created { get; set; }
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
    public class User_Token_Expiry
    {
        public string? Username { get; set; } = null!;
        public string? Token { get; set; }
        public DateTime? TokenCreatedOn { get; set; }
        public DateTime? TokenExpiry { get; set; }
        public DateTime? TokenRevokedOn { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public int? OtpStatus { get; set; }
    }
    public partial class TUser
    {
        public string? Username { get; set; } = null!;
        public string? Token { get; set; }
        public DateTime? TokenCreatedOn { get; set; }
        public DateTime? TokenExpiry { get; set; }
        public DateTime? TokenRevokedOn { get; set; }
        public string? Otp { get; set; }
        public DateTime? OtpExpiry { get; set; }
        public int? OtpStatus { get; set; }
    }
    #endregion Login
}
