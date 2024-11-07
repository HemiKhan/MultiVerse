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
        [Required(AllowEmptyStrings = false, ErrorMessage = "*UserName* is required")]
        public string UserName { get; set; } = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Password* is required")]
        public string Password { get; set; } = "";
        [IgnoreDataMemberAttribute]
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
        public string? Department { get; set; } = "";
        public string? Designation { get; set; } = "";
        public string? PasswordExpiryDateTime { get; set; } = "";
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

    #region User Setup
    public class P_Users_Result
    {
        public int RowNo { get; set; } = 0;
        public int USER_ID { get; set; } = 0;
        public string Encrypted_USER_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(USER_ID);
            }
        }
        public string UserType_MTV_CODE { get; set; } = "";
        public int D_ID { get; set; } = 0;
        public int SecurityQuestion_MTV_ID { get; set; } = 0;
        public int BlockType_MTV_ID { get; set; } = 0;
        public string USERNAME { get; set; } = "";
        public string UserType { get; set; } = "";
        public string Department { get; set; } = "";
        public string Designation { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Company { get; set; } = "";
        public string Address { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Country { get; set; } = "";
        public string Email { get; set; } = "";
        public string Mobile { get; set; } = "";
        public string Phone { get; set; } = "";
        public string PhoneExt { get; set; } = "";
        public string SecurityQuestion { get; set; } = "";
        public string EncryptedAnswer { get; set; } = "";
        public int TIMEZONE_ID { get; set; } = 0;
        public string TIMEZONE_Name { get; set; } = "";
        public bool IsApproved { get; set; } = false;
        public string BlockType { get; set; } = "";
        public string PasswordExpiry { get; set; } = "";
        public bool IsAPIUser { get; set; } = false;
        public bool IsActive { get; set; } = false;
    }
    public class P_AddOrEdit_User_Response
    {
        public int USER_ID { get; set; } = 0;

        private string _USERNAME = "";
        public string USERNAME
        {
            get
            {
                return this._USERNAME;
            }
            set
            {
                this._USERNAME = (value == null ? "" : value.ToUpper());
            }
        }

        public string UserType_MTV_CODE { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public int D_ID { get; set; }
        public string Designation { get; set; } = default!;

        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                this._FirstName = (value == null ? "" : value);
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return this._MiddleName;
            }
            set
            {
                this._MiddleName = (value == null ? "" : value);
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                this._LastName = (value == null ? "" : value);
            }
        }

        public string Company { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Address2 { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public string Country { get; set; } = default!;

        private string _Email = "";
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = (value == null ? "" : value.ToLower());
            }
        }

        public string Mobile { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string PhoneExt { get; set; } = default!;
        public int SecurityQuestion_MTV_ID { get; set; }
        public string EncryptedAnswer { get; set; } = default!;
        public int TIMEZONE_ID { get; set; }
        public bool IsApproved { get; set; } = false;
        public int BlockType_MTV_ID { get; set; }
        public bool IsAPIUser { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
    public class P_Get_User_By_ID
    {
        public int USER_ID { get; set; } = 0;

        private string _USERNAME = "";
        public string USERNAME
        {
            get
            {
                return this._USERNAME;
            }
            set
            {
                this._USERNAME = (value == null ? "" : value.ToUpper());
            }
        }

        public string UserType_MTV_CODE { get; set; } = default!;
        public string UserType { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public int D_ID { get; set; }
        public string DepartmentName { get; set; }
        public string Designation { get; set; } = default!;

        private string _FirstName = "";
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                this._FirstName = (value == null ? "" : value);
            }
        }

        private string _MiddleName = "";
        public string MiddleName
        {
            get
            {
                return this._MiddleName;
            }
            set
            {
                this._MiddleName = (value == null ? "" : value);
            }
        }

        private string _LastName = "";
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                this._LastName = (value == null ? "" : value);
            }
        }

        public string Company { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Address2 { get; set; } = default!;
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;
        public string Country { get; set; } = default!;

        private string _Email = "";
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                this._Email = (value == null ? "" : value.ToLower());
            }
        }

        public string Mobile { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string PhoneExt { get; set; } = default!;
        public int SecurityQuestion_MTV_ID { get; set; }
        public string Question { get; set; } = default!;
        public string EncryptedAnswer { get; set; } = default!;
        public int TIMEZONE_ID { get; set; }
        public string TIMEZONE { get; set; } = default!;
        public bool IsApproved { get; set; } = false;
        public int BlockType_MTV_ID { get; set; }
        public string BlockType { get; set; } = default!;
        public bool IsAPIUser { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
    public class P_Get_SearchUsersName
    {
        public int USER_ID { get; set; }
        public string USERNAME { get; set; }
    }
    #endregion User Setup    
}
