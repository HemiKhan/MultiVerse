using Data.DataAccess;
using Data.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System.Runtime.Serialization;

namespace Data.Swaggers
{
    public class AccountSignInResDTOBadRequest
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = Failed/Error", Nullable = false)]
        public bool responsecode { get; set; } = false;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string username { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string jwtoken { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string refreshtoken { get; set; } = "";
        [SwaggerSchemaExample("INREQ")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errorcode { get; set; } = "";
        [SwaggerSchemaExample("Invalid Username")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Nullable = false)]
        [IgnoreDataMemberAttribute]
        [HideProperty(IsHideSerialize = true, IsCheckHideSerializeFromPublicObject = true)]
        public bool rememberme { get; set; } = false;
    }
    public class AccountSignInResDTOTooManyRequest
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = Failed/Error", Nullable = false)]
        public bool responsecode { get; set; } = false;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string username { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string jwtoken { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string refreshtoken { get; set; } = "";
        [SwaggerSchemaExample("LMTRH")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errorcode { get; set; } = "";
        [SwaggerSchemaExample("Too Many Requests.")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Nullable = false)]
        [IgnoreDataMemberAttribute]
        [HideProperty(IsHideSerialize = true, IsCheckHideSerializeFromPublicObject = true)]
        public bool rememberme { get; set; } = false;
    }
    public class AccountSignInResDTOUnauthorized
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = Failed/Error", Nullable = false)]
        public bool responsecode { get; set; } = false;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string username { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string jwtoken { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string refreshtoken { get; set; } = "";
        [SwaggerSchemaExample("INUSER")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errorcode { get; set; } = "";
        [SwaggerSchemaExample("You Don't Have Access to This API")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Nullable = false)]
        [IgnoreDataMemberAttribute]
        [HideProperty(IsHideSerialize = true, IsCheckHideSerializeFromPublicObject = true)]
        public bool rememberme { get; set; } = false;
    }
    public class RefreshTokenResDTOTooManyRequests
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = Failed/Error", Nullable = false)]
        public bool responsecode { get; set; } = false;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string jwtoken { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string refreshtoken { get; set; } = "";
        [SwaggerSchemaExample("LMTRH")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errorcode { get; set; } = "";
        [SwaggerSchemaExample("Too Many Requests.")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errormsg { get; set; } = "";
    }
    public class RefreshTokenResDTOUnauthorized
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = Failed/Error", Nullable = false)]
        public bool responsecode { get; set; } = false;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string jwtoken { get; set; } = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Nullable = false)]
        public string refreshtoken { get; set; } = "";
        [SwaggerSchemaExample("INUSER")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errorcode { get; set; } = "";
        [SwaggerSchemaExample("You Don't Have Access to This API")]
        [SwaggerSchema(Nullable = false, Description = "empty in case of responsecode is true")]
        public string errormsg { get; set; } = "";
    }
    public class AspDotCoreWebApiReturnTooManyRequest
    {
        private string _request_id = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty string will be returned", Nullable = false)]
        public string request_id
        {
            get
            {
                return _request_id;
            }
            set
            {
                string Ret = "";
                if (value != null)
                {
                    Ret = value;
                }
                _request_id = Ret;
            }
        }

        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("LMTRH")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Too Many Requests.")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty string will be returned", Nullable = false)]
        public string AspDotCoreWebApi_id { get; set; } = "";
        [SwaggerSchemaExample("2022-12-31 14:35:25.145")]
        [SwaggerSchema(Description = "AspDotCoreWebApi Request DateTime in EST", ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd HH:mm:ss.fff")]
        public string AspDotCoreWebApi_request_date { get; set; } = Globals.GetCurrentESTDateTimeToString(true);
        [SwaggerSchemaExample("2023-01-01")]
        [SwaggerSchema(ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd")]
        public string AspDotCoreWebApi_expire_date { get; set; } = Globals.GetCurrentESTDateTimeToString(false);
    }
    public class AspDotCoreWebApiReturnUnauthorized
    {
        private string _request_id = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty string will be returned", Nullable = false)]
        public string request_id
        {
            get
            {
                return _request_id;
            }
            set
            {
                string Ret = "";
                if (value != null)
                {
                    Ret = value;
                }
                _request_id = Ret;
            }
        }

        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("INTOKN")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Invalid Token")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty string will be returned", Nullable = false)]
        public string AspDotCoreWebApi_id { get; set; } = "";
        [SwaggerSchemaExample("2022-12-31 14:35:25.145")]
        [SwaggerSchema(Description = "AspDotCoreWebApi Request DateTime in EST", ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd HH:mm:ss.fff")]
        public string AspDotCoreWebApi_request_date { get; set; } = Globals.GetCurrentESTDateTimeToString(true);
        [SwaggerSchemaExample("2023-01-01")]
        [SwaggerSchema(ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd")]
        public string AspDotCoreWebApi_expire_date { get; set; } = Globals.GetCurrentESTDateTimeToString(false);
    }
    public class AspDotCoreWebApiReturnBadRequest
    {
        private string _request_id = "";
        [SwaggerSchemaExample("b56efec3-d4d3-41c1-a1c8-5f2f32f80883")]
        [SwaggerSchema(Description = "whichever value required in request will be return. if it was not provide in request and empty string will be return", Nullable = false)]
        public string request_id
        {
            get
            {
                return _request_id;
            }
            set
            {
                string Ret = "";
                if (value != null)
                {
                    Ret = value;
                }
                _request_id = Ret;
            }
        }

        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("INREQ")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Invalid Service Level")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
        [SwaggerSchemaExample("F8G45-T54S45")]
        [SwaggerSchema(Description = "Unique Identifier Number", Nullable = false)]
        public string AspDotCoreWebApi_id { get; set; } = "";
        [SwaggerSchemaExample("2022-12-31 14:35:25.145")]
        [SwaggerSchema(Description = "AspDotCoreWebApi Request DateTime in EST", ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd HH:mm:ss.fff")]
        public string AspDotCoreWebApi_request_date { get; set; } = Globals.GetCurrentESTDateTimeToString(true);
        [SwaggerSchemaExample("2023-01-01")]
        [SwaggerSchema(ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd")]
        public string AspDotCoreWebApi_expire_date { get; set; } = Globals.GetCurrentESTDateTimeToString(false);
    }
    public class AspDotCoreWebApiReturnServerError
    {
        private string _request_id = "";
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty string will be returned", Nullable = false)]
        public string request_id
        {
            get
            {
                return _request_id;
            }
            set
            {
                string Ret = "";
                if (value != null)
                {
                    Ret = value;
                }
                _request_id = Ret;
            }
        }

        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("INTSRVERR")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Internal Server Error")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
        [SwaggerSchemaExample("")]
        [SwaggerSchema(Description = "empty string will be returned", Nullable = false)]
        public string AspDotCoreWebApi_id { get; set; } = "";
        [SwaggerSchemaExample("2022-12-31 14:35:25.145")]
        [SwaggerSchema(Description = "AspDotCoreWebApi Request DateTime in EST", ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd HH:mm:ss.fff")]
        public string AspDotCoreWebApi_request_date { get; set; } = Globals.GetCurrentESTDateTimeToString(true);
        [SwaggerSchemaExample("2023-01-01")]
        [SwaggerSchema(ReadOnly = true, Nullable = false, Format = "yyyy-MM-dd")]
        public string AspDotCoreWebApi_expire_date { get; set; } = Globals.GetCurrentESTDateTimeToString(false);
    }
    public class APILimitResponseBadRequest
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("InvalidKey")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("INREQ")]
        public string errorcode { get; set; } = "";
    }
    public class APILimitResponseTooManyRequest
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("Too Many Requests.")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("LMTRH")]
        public string errorcode { get; set; } = "";
    }
    public class APILimitResponseUnauthorized
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("Invalid Token")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("INTOKN")]
        public string errorcode { get; set; } = "";
    }
    public class APILimitResponseServerError
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("Internal Server Error")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("INTSRVERR")]
        public string errorcode { get; set; } = "";
    }
    public class OrderDetailResponseBadRequest
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("Invalid Order")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("INREQ")]
        public string errorcode { get; set; } = "";
    }
    public class OrderDetailResponseTooManyRequest
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("Too Many Requests.")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("LMTRH")]
        public string errorcode { get; set; } = "";
    }
    public class OrderDetailResponseUnauthorized
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("Invalid Token")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("INTOKN")]
        public string errorcode { get; set; } = "";
    }
    public class OrderDetailResponseServerError
    {
        [SwaggerSchemaExample("false")]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("Internal Server Error")]
        public string errormsg { get; set; } = "";
        [SwaggerSchemaExample("INTSRVERR")]
        public string errorcode { get; set; } = "";
    }
    public class ResponseOrderUnauthorized
    {
        [SwaggerSchema(Description = "true if order is successfully created else false.", Nullable = false)]
        [SwaggerSchemaExample("false")]
        public bool issuccess { get; set; } = false;
        [SwaggerSchema(Description = "null in case of issuccess is true", Nullable = true)]
        public List<ListofErrors>? listoferrors { get; set; } = null;

    }
    public class ResponseOrderBadRequest
    {
        [SwaggerSchema(Description = "true if order is successfully created else false.", Nullable = false)]
        [SwaggerSchemaExample("false")]
        public bool issuccess { get; set; } = false;
        [SwaggerSchema(Description = "null in case of issuccess is true", Nullable = true)]
        public List<ListofErrors>? listoferrors { get; set; } = null;

    }
    public class ResponseOrderTooManyRequest
    {
        [SwaggerSchema(Description = "true if order is successfully created else false.", Nullable = false)]
        [SwaggerSchemaExample("false")]
        public bool issuccess { get; set; } = false;
        [SwaggerSchema(Description = "null in case of issuccess is true", Nullable = true)]
        public List<ListofErrors>? listoferrors { get; set; } = null;

    }
    public class ResponseOrderServerError
    {
        [SwaggerSchema(Description = "true if order is successfully created else false.", Nullable = false)]
        [SwaggerSchemaExample("false")]
        public bool issuccess { get; set; } = false;
        [SwaggerSchema(Description = "null in case of issuccess is true", Nullable = true)]
        public List<ListofErrors>? listoferrors { get; set; } = null;

    }
    public class QuoteReturnWPBadRequest
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("INREQ")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Invalid ZipCode")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
    }
    public class QuoteReturnWPServerError
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("INTSRVERR")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Internal Server Error")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
    }
    public class QuoteReturnWPUnauthorized
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("INTOKN")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Invalid Token")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
    }
    public class QuoteReturnWPTooManyRequest
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool response_code { get; set; } = false;
        [SwaggerSchemaExample("LMTRH")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string error_code { get; set; } = "";
        [SwaggerSchemaExample("Too Many Requests.")]
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        public List<string>? error_text { get; set; } = null;
    }
    public class ImportFieldResponseTooManyRequest
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool ResponseCode { get; set; }
        [SwaggerSchemaExample("LMTRH")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string ErrorCode { get; set; }
        [SwaggerSchema(Description = "empty in case of *response_code* is true")]
        [SwaggerSchemaExample("Too Many Requests.")]
        public string ErrorMsg { get; set; }
    }
    public class ImportFieldResponseUnauthorized
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool ResponseCode { get; set; }
        [SwaggerSchemaExample("INTOKN")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string ErrorCode { get; set; }
        [SwaggerSchema(Description = "null in case of *response_code* is true")]
        [SwaggerSchemaExample("Invalid Token")]
        public string ErrorMsg { get; set; }
    }
    public class ImportFieldResponseBadRequest
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool ResponseCode { get; set; }
        [SwaggerSchemaExample("INREQ")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string ErrorCode { get; set; }
        [SwaggerSchema(Description = "empty in case of *response_code* is true")]
        [SwaggerSchemaExample("Invalid Request")]
        public string ErrorMsg { get; set; }
    }
    public class ImportFieldResponseServerError
    {
        [SwaggerSchemaExample("false")]
        [SwaggerSchema(Description = "true = Successful, false = failed/error", Nullable = false)]
        public bool ResponseCode { get; set; }
        [SwaggerSchemaExample("INTSRVERR")]
        [SwaggerSchema(Description = "empty in case of *response_code* is true", Nullable = false)]
        public string ErrorCode { get; set; }
        [SwaggerSchema(Description = "empty in case of *response_code* is true")]
        [SwaggerSchemaExample("Internal Server Error")]
        public string ErrorMsg { get; set; }
    }
}
