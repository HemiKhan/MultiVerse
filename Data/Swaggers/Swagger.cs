using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.Serialization;
using Data.Dtos;
using Data.DataAccess;
using Data.Models;

namespace Data.Swaggers
{
    [SwaggerSchema(Description = "null in case of responsecode is false", Nullable = true)]
    public class UserInfoDTO2 : P_Get_User_Info_SP
    {
    }
    public class AccountSignInResDTO2 : AccountSignInResDTO
    {
        [JsonProperty(Order = 1)]
        [JsonIgnore]
        [IgnoreDataMember]
        [HideProperty(IsHideSerialize = true, IsCheckHideSerializeFromPublicObject = true)]
        public new UserInfoDTO2? UserInfo { get; set; } = null;
    }
}
