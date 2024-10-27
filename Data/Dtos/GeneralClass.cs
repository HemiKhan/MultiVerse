using Data.DataAccess;
using Data.Swaggers;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Data.Dtos
{
    public class ListofErrors
    {
        [SwaggerSchema(Nullable = false)]
        public string errorcode { get; set; } = "";
        [SwaggerSchema(Nullable = false)]
        public string errormessage { get; set; } = "";
        [SwaggerSchema(Nullable = false)]
        public string detailmessage { get; set; } = "";
    }
    public class APILimitRequest
    {
        public int APIID { get; set; } = 0;
        public int MethodID { get; set; } = 0;
        public int ReqInSecond { get; set; } = 0;
        public int ReqInMinute { get; set; } = 0;
        public int ReqInHour { get; set; } = 0;
        public int ReqInDay { get; set; } = 0;
        public int ReqType { get; set; } = 0;
        public string ReqTypeName
        {
            get
            {
                string Ret = "";
                if (this.ReqType == 10000)
                    Ret = "Second";
                else if (this.ReqType == 20000)
                    Ret = "Minute";
                else if (this.ReqType == 30000)
                    Ret = "Hour";
                else if (this.ReqType == 40000)
                    Ret = "Day";
                return Ret;
            }
        }
        public int ReqLimits
        {
            get
            {
                int Ret = 0;
                if (this.ReqType == 10000)
                    Ret = this.ReqInSecond;
                else if (this.ReqType == 20000)
                    Ret = this.ReqInMinute;
                else if (this.ReqType == 30000)
                    Ret = this.ReqInHour;
                else if (this.ReqType == 40000)
                    Ret = this.ReqInDay;
                return Ret;
            }
        }
        public int ReqLimitsTime
        {
            get
            {
                int Ret = 0;
                if (this.ReqType == 10000)
                    Ret = 1;
                else if (this.ReqType == 20000)
                    Ret = 1 * 60;
                else if (this.ReqType == 30000)
                    Ret = 1 * 60 * 60;
                else if (this.ReqType == 40000)
                    Ret = 1 * 60 * 60 * 24;
                return Ret;
            }
        }
        public bool ReqOtherType_IsWarningEnabled { get; set; } = true;
        public bool ReqOtherType_IsErrorEnabled { get; set; } = true;
        public bool IsActive { get; set; } = true;
    }
    public class APIUserMapLimitRequest : APILimitRequest
    {
        public int UserID { get; set; } = 0;
    }
    public class APIRemoteDomainLimitRequest : APILimitRequest
    {
        public string RemoteDomain_Or_IP { get; set; } = "";
        public bool Is_RemoteDomain { get; set; } = true;
    }
    public class APIIPLimitRequest : APILimitRequest
    {
        public string RemoteDomain_Or_IP { get; set; } = "";
        public bool Is_RemoteDomain { get; set; } = false;
    }
    public class APILimitResponse
    {
        [SwaggerSchemaExample("true")]
        public bool response_code { get; set; } = false;
        [IgnoreDataMember]
        [JsonIgnore]
        [HideProperty(IsHideSerialize = true)]
        [SwaggerSchemaExample("0")]
        public int statuscode { get; set; } = 0;
        [IgnoreDataMember]
        [JsonIgnore]
        [HideProperty(IsHideSerialize = true)]
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string warningmsg { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string errorcode { get; set; } = "";
    }
    public class APIWhiteListingRequest
    {
        public int AHIW_ID { get; set; } = 0;
        public int APIID { get; set; } = 0;
        public bool IsWhiteList { get; set; } = true;
        public bool IsBlackList { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
    public class APIRemoteDomainWhiteListingRequest : APIWhiteListingRequest
    {
        public string RemoteDomain_Or_IP { get; set; } = "";
        public bool Is_RemoteDomain { get; set; } = true;
    }
    public class APIIPWhiteListingRequest : APIWhiteListingRequest
    {
        public string RemoteDomain_Or_IP { get; set; } = "";
        public bool Is_RemoteDomain { get; set; } = false;
    }
    public class APIWhiteListingResponse
    {
        [SwaggerSchemaExample("true")]
        public bool response_code { get; set; } = false;
        [IgnoreDataMember]
        [JsonIgnore]
        [HideProperty(IsHideSerialize = true)]
        [SwaggerSchemaExample("0")]
        public int statuscode { get; set; } = 0;
        [IgnoreDataMember]
        [JsonIgnore]
        [HideProperty(IsHideSerialize = true)]
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string warningmsg { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string errorcode { get; set; } = "";
    }
    public class DocumentTypeList
    {
        [SwaggerSchemaExample("true")]
        [SwaggerSchema(Description = "true = Sccuessful, false = failed/error")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string errorcode { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty in case of *response_code* is false or no warning", Nullable = false)]
        public string warningtext { get; set; } = "";
        [SwaggerSchema(Description = "null in case of *response_code* is false", Nullable = true)]
        public List<ListOfDocumnetType>? listofdocumenttype { get; set; } = null;
    }
    public class ListOfDocumnetType
    {
        [SwaggerSchemaExample("0")]
        public int MTVId { get; set; } = 0;
        [SwaggerSchemaExample("0")]
        public int MTVCode { get; set; } = 0;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string Name { get; set; } = "";
    }
}
