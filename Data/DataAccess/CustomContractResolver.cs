using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Serialization;

namespace Data.DataAccess
{
    public class CustomContractResolverStandard : DefaultContractResolver
    {
        private readonly bool _useCustomContractResolver;
        private readonly bool _isoriginalmembername;
        private readonly bool _isexcludefilebase64field;
        public CustomContractResolverStandard(bool useCustomContractResolver, bool isoriginalmembername, bool isexcludefilebase64field) : base()
        {
            _useCustomContractResolver = useCustomContractResolver;
            _isoriginalmembername = isoriginalmembername;
            _isexcludefilebase64field = isexcludefilebase64field;
            NamingStrategy = new CamelCaseNamingStrategy();
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property != null && _useCustomContractResolver)
            {
                if (_isoriginalmembername && property.PropertyName != null)
                    property.PropertyName = member.Name;
                // Check if the IgnoreDataMemberAttribute is defined on the member
                if (member.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), true).Length > 0)
                {
                    // Only ignore the property during serialization
                    if (memberSerialization == MemberSerialization.Fields || memberSerialization == MemberSerialization.OptIn || memberSerialization == MemberSerialization.OptOut)
                        //if (memberSerialization != MemberSerialization.OptIn)
                        property.Ignored = false;
                }

                // Check if the JsonIgnoreAttribute is defined on the member
                if (member.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0)
                {
                    // Only ignore the property during serialization
                    if (memberSerialization == MemberSerialization.Fields || memberSerialization == MemberSerialization.OptIn || memberSerialization == MemberSerialization.OptOut)
                        //if (memberSerialization != MemberSerialization.OptIn)
                        property.Ignored = false;
                }
                if (_isexcludefilebase64field && (property.PropertyName?.ToLower() == "filebase64" || property.PropertyName?.ToLower() == "filedatabase64"))
                    property.Ignored = true;
            }

            return property;
        }
        //protected override string ResolvePropertyName(string propertyName)
        //{
        //    return propertyName.ToLower();
        //}
    }
    public class CustomContractResolverNone : DefaultContractResolver
    {
        private readonly bool _isoriginalmembername;
        private readonly bool _isexcludefilebase64field;
        public CustomContractResolverNone(bool isoriginalmembername, bool isexcludefilebase64field) : base()
        {
            _isoriginalmembername = isoriginalmembername;
            _isexcludefilebase64field = isexcludefilebase64field;
            NamingStrategy = new CamelCaseNamingStrategy();
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property != null)
            {
                if (_isoriginalmembername && property.PropertyName != null)
                    property.PropertyName = member.Name;

                property.Ignored = false;
                if (_isexcludefilebase64field && (property.PropertyName?.ToLower() == "filebase64" || property.PropertyName?.ToLower() == "filedatabase64"))
                    property.Ignored = true;
            }

            return property;
        }
        //protected override string ResolvePropertyName(string propertyName)
        //{
        //    return propertyName.ToLower();
        //}
    }
    public class CustomContractResolverHideProperty : DefaultContractResolver
    {
        private readonly bool _IsDeserialize;
        private readonly bool _isoriginalmembername;
        private readonly bool _isexcludefilebase64field;
        public CustomContractResolverHideProperty(bool IsDeserialize = false, bool isoriginalmembername = false, bool isexcludefilebase64field = false) : base()
        {
            _IsDeserialize = IsDeserialize;
            _isoriginalmembername = isoriginalmembername;
            _isexcludefilebase64field = isexcludefilebase64field;
            NamingStrategy = new CamelCaseNamingStrategy();
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);
            if (property != null)
            {
                if (_isoriginalmembername && property.PropertyName != null)
                    property.PropertyName = member.Name;
                if (_isexcludefilebase64field && (property.PropertyName?.ToLower() == "filebase64" || property.PropertyName?.ToLower() == "filedatabase64"))
                    property.Ignored = true;

                if (property.Ignored && !(_isexcludefilebase64field && (property.PropertyName?.ToLower() == "filebase64" || property.PropertyName?.ToLower() == "filedatabase64")))
                {
                    bool isDeserialize = _IsDeserialize;
                    HidePropertyAttribute hideAttribute = Attribute.GetCustomAttribute(member, typeof(HidePropertyAttribute)) as HidePropertyAttribute;
                    if (hideAttribute != null)
                    {
                        // Only ignore the property during serialization
                        if (((memberSerialization == MemberSerialization.OptIn || isDeserialize) && hideAttribute.IsHideDeSerialize) || ((memberSerialization == MemberSerialization.OptIn || isDeserialize == false) && hideAttribute.IsHideSerialize))
                        {
                            bool IsIgnored = property.Ignored;
                            if (isDeserialize == false && memberSerialization == MemberSerialization.OptIn)
                                isDeserialize = true;
                            string Propertyname = (property.PropertyName == null ? "" : property.PropertyName.ToLower());
                            VerifyHidePropertyAttributeValid(ref IsIgnored, hideAttribute, isDeserialize, Propertyname);
                            property.Ignored = IsIgnored;
                        }
                        else
                            property.Ignored = false;
                    }
                }
            }

            return property;
        }
        //protected override string ResolvePropertyName(string propertyName)
        //{
        //    return propertyName.ToLower();
        //}
        public void VerifyHidePropertyAttributeValid(ref bool IsIgnore, HidePropertyAttribute hidePropertyAttribute, bool isDeserialize, string Propertyname)
        {
            PublicClaimObjects _PublicClaimObjects = StaticPublicObjects.ado.GetPublicClaimObjects();
            string? remotedomain = "";
            try
            {
                remotedomain = StaticPublicObjects.ado.GetRemoteDomain();
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "VerifyHidePropertyValid", SmallMessage: ex.Message, Message: ex.ToString());
            }
            if (IsIgnore && hidePropertyAttribute.IsHideSerialize == false && isDeserialize == false)
            {
                IsIgnore = false;
                return;
            }
            else if (IsIgnore && hidePropertyAttribute.IsHideDeSerialize == false && isDeserialize)
            {
                IsIgnore = false;
                return;
            }

            if (hidePropertyAttribute.IsHideSerialize && isDeserialize == false)
            {
                if (hidePropertyAttribute.IsCheckHideSerializeFromPublicObject && _PublicClaimObjects.isignorejsonserializeproperty)
                {
                    IsIgnore = false;
                    return;
                }
                else if (hidePropertyAttribute.IsCheckHideSerializeFromPublicObjectList && _PublicClaimObjects.ignorejsonserializepropertylist != null)
                {
                    if (ValueExistsInArry(_PublicClaimObjects.ignorejsonserializepropertylist, Propertyname))
                    {
                        IsIgnore = false;
                        return;
                    }
                }
                else if (hidePropertyAttribute.RemoteDomainIncludeHideSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.RemoteDomainIncludeHideSerialize, remotedomain))
                    {
                        IsIgnore = false;
                        return;
                    }
                }
                else if (hidePropertyAttribute.RemoteDomainExcludeHideSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.RemoteDomainExcludeHideSerialize, remotedomain))
                    {
                        IsIgnore = true;
                        return;
                    }
                }
                else if (hidePropertyAttribute.RemoteDomainIncludeHideSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.RemoteDomainIncludeHideSerialize, _PublicClaimObjects.username))
                    {
                        IsIgnore = false;
                        return;
                    }
                }
                else if (hidePropertyAttribute.UserExcludeHideSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.UserExcludeHideSerialize, _PublicClaimObjects.username))
                    {
                        IsIgnore = true;
                        return;
                    }
                }
            }
            else if (hidePropertyAttribute.IsHideDeSerialize && isDeserialize)
            {
                if (hidePropertyAttribute.IsCheckHideDeSerializeFromPublicObject && _PublicClaimObjects.isignorejsondeserializeproperty)
                {
                    IsIgnore = false;
                    return;
                }
                else if (hidePropertyAttribute.IsCheckHideDeSerializeFromPublicObjectList && _PublicClaimObjects.ignorejsondeserializepropertylist != null)
                {
                    if (ValueExistsInArry(_PublicClaimObjects.ignorejsondeserializepropertylist, Propertyname))
                    {
                        IsIgnore = false;
                        return;
                    }
                }
                else if (hidePropertyAttribute.RemoteDomainIncludeHideDeSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.RemoteDomainIncludeHideDeSerialize, remotedomain))
                    {
                        IsIgnore = false;
                        return;
                    }
                }
                else if (hidePropertyAttribute.RemoteDomainExcludeHideDeSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.RemoteDomainExcludeHideDeSerialize, remotedomain))
                    {
                        IsIgnore = true;
                        return;
                    }
                }
                else if (hidePropertyAttribute.RemoteDomainIncludeHideDeSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.RemoteDomainIncludeHideDeSerialize, _PublicClaimObjects.username))
                    {
                        IsIgnore = false;
                        return;
                    }
                }
                else if (hidePropertyAttribute.UserExcludeHideDeSerialize != null)
                {
                    if (ValueExistsInArry(hidePropertyAttribute.UserExcludeHideDeSerialize, _PublicClaimObjects.username))
                    {
                        IsIgnore = true;
                        return;
                    }
                }
            }
        }
        public bool ValueExistsInArry(string[]? elements, string? comparevalue)
        {
            comparevalue = (comparevalue == null ? "" : comparevalue);
            if (elements != null)
            {
                foreach (string element in elements)
                {
                    if (element == comparevalue)
                        return true;
                }
            }
            return false;
        }
        public bool ValueExistsInArry(List<string>? elements, string? comparevalue)
        {
            comparevalue = (comparevalue == null ? "" : comparevalue);
            if (elements != null)
            {
                foreach (string element in elements)
                {
                    if (element == comparevalue)
                        return true;
                }
            }
            return false;
        }
    }
    public class IgnoreCasePropertyNamesContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
    public class CustomContractResolverMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor httpContextAccessor;
        public CustomContractResolverMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Response.HasStarted == false && StaticPublicObjects.ado.IsSwaggerCall())
            {
                // Get the serializer settings from the service provider
                var serializerSettings = (JsonSerializerSettings)context.RequestServices.GetService(typeof(JsonSerializerSettings))!;

                // Modify the serializer settings based on some logic
                if (StaticPublicObjects.ado.IsSwaggerCallAdmin())
                {
                    serializerSettings.ContractResolver = new CustomContractResolverNone(false, false);
                }
                else
                {
                    serializerSettings.ContractResolver = new CustomContractResolverHideProperty(true);
                }
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }
    }
}
