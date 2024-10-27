using Data.DataAccess;

namespace Data.Dtos
{
    public class MemoryCacheType
    {
        public string TypeString
        {
            get
            {
                List<string>? RetList = new List<string>();
                RetList = GlobalsFunctions.GetCacheType(this.SubType);
                string Ret = "";
                Ret = GlobalsFunctions.GetTypeString(RetList);
                return Ret;
            }
        }
        public string SubType { get; set; } = "";
        public int DefaultExpirySeconds
        {
            get
            {
                int Ret = 60;
                Ret = GlobalsFunctions.GetDefaultCacheExpirySeconds(this.SubType);
                return Ret;
            }
        }
        public int NewExpirySeconds { get; set; } = 0;
        public int GetExpirySeconds
        {
            get
            {
                int Ret = 0;
                Ret = Math.Max(this.DefaultExpirySeconds, this.NewExpirySeconds);
                return Ret;
            }
        }
        public bool IsInputType { get; set; } = true;
    }
    public class MemoryCacheValueType
    {
        public SetMemoryCacheValueType _SetMemoryCacheValueType { get; set; }
        public GetMemoryCacheValueType _GetMemoryCacheValueType { get; set; }
        public MemoryCacheValueType()
        {
            _SetMemoryCacheValueType = new SetMemoryCacheValueType();
            _GetMemoryCacheValueType = new GetMemoryCacheValueType();
        }
    }
    public class GetMemoryCacheValueType
    {
        public string typestring
        {
            get
            {
                List<string>? RetList = new List<string>();
                RetList = GlobalsFunctions.GetCacheType(this.subtype);
                string Ret = "";
                Ret = GlobalsFunctions.GetTypeString(RetList);
                return Ret;
            }
        }
        public string subtype { get; set; } = "";
        public string keypath
        {
            get
            {
                string Ret = this.defaultkeypath;
                if (setkeypath != null)
                    Ret = setkeypath;
                return Ret;
            }
        }
        public string keyrequest
        {
            get
            {
                string Ret = this.defaultkeyrequest;
                if (setkeyrequest != null)
                    Ret = setkeyrequest;
                return Ret;
            }
        }
        public string keyusername
        {
            get
            {
                string Ret = this.defaultkeyusername;
                if (setkeyusername != null)
                    Ret = setkeyusername;
                return Ret;
            }
        }
        public string keyother
        {
            get
            {
                string Ret = this.defaultkeyother;
                if (setkeyother != null)
                    Ret = setkeyother;
                return Ret;
            }
        }
        public string keyvalues
        {
            get
            {
                string Ret = this.defaultkeyvalues;
                if (setkeyvalues != null)
                    Ret = setkeyvalues;
                return Ret;
            }
        }
        public string keyparavalues
        {
            get
            {
                string Ret = "";
                if (setkeyparavalues != null)
                    Ret = setkeyparavalues;
                return Ret;
            }
        }
        public string? setkeypath { get; set; } = null;
        public string? setkeyrequest { get; set; } = null;
        public string? setkeyusername { get; set; } = null;
        public string? setkeyother { get; set; } = null;
        public string? setkeyvalues { get; set; } = null;
        public string setkeyparavalues { get; set; } = "";
        public string defaultkeypath
        {
            get
            {
                string Ret = StaticPublicObjects.ado.GetPublicClaimObjects().path;
                return Ret;
            }
        }
        public string defaultkeyrequest
        {
            get
            {
                string Ret = "";
                return Ret;
            }
        }
        public string defaultkeyusername
        {
            get
            {
                string Ret = StaticPublicObjects.ado.GetPublicClaimObjects().username;
                return Ret;
            }
        }
        public string defaultkeyother
        {
            get
            {
                string Ret = "";
                return Ret;
            }
        }
        public string defaultkeyvalues
        {
            get
            {
                string Ret = "";
                return Ret;
            }
        }
        public string cachekey
        {
            get
            {
                string Ret = "";
                if (this.setkeypath != null && this.setkeypath.ToString() != "")
                    Ret += "path:" + this.setkeypath + "|";
                else if (this.defaultkeypath != null && this.defaultkeypath.ToString() != "")
                    Ret += "path:" + this.defaultkeypath + "|";

                if (this.setkeyrequest != null && this.setkeyrequest.ToString() != "")
                    Ret += "request:" + this.setkeyrequest + "|";
                else if (this.defaultkeyrequest != null && this.defaultkeyrequest.ToString() != "")
                    Ret += "request:" + this.defaultkeyrequest + "|";

                if (this.setkeyusername != null && this.setkeyusername.ToString() != "")
                    Ret += "username:" + this.setkeyusername + "|";
                else if (this.defaultkeyusername != null && this.defaultkeyusername.ToString() != "")
                    Ret += "username:" + this.defaultkeyusername + "|";

                if (this.setkeyother != null && this.setkeyother.ToString() != "")
                    Ret += "other:" + this.setkeyother + "|";
                else if (this.defaultkeyother != null && this.defaultkeyother.ToString() != "")
                    Ret += "other:" + this.defaultkeyother + "|";

                if (this.setkeyvalues != null && this.setkeyvalues.ToString() != "")
                    Ret += "value:" + this.setkeyvalues + "|";
                else if (this.defaultkeyvalues != null && this.defaultkeyvalues.ToString() != "")
                    Ret += "value:" + this.defaultkeyvalues + "|";

                if (this.setkeyparavalues.ToString() != "")
                    Ret += "paravalues=" + this.setkeyparavalues + "|";

                if (this.typestring != "")
                    Ret += this.typestring;

                if (this.subtype != "")
                    Ret += "subtype=" + this.subtype + "|";

                return Ret.ToLower();
            }
        }
        public bool isgetfromcache
        {
            get
            {
                bool Ret = this.defaultisgetcache;
                if (this.setisgetcache != null)
                    Ret = (bool)this.setisgetcache;
                return Ret;
            }
        }
        public bool isgetfromsqlcache
        {
            get
            {
                bool Ret = this.defaultisgetsqlcache;
                if (this.setisgetsqlcache != null)
                    Ret = (bool)this.setisgetsqlcache;
                return Ret;
            }
        }
        public bool? setisgetcache { get; set; } = null;
        public bool? setisgetsqlcache { get; set; } = null;
        public bool defaultisgetcache
        {
            get
            {
                bool Ret = false;
                Ret = GlobalsFunctions.GetDefaultGetCache(this.subtype);
                return Ret;
            }
        }
        public bool defaultisgetsqlcache
        {
            get
            {
                bool Ret = false;
                Ret = GlobalsFunctions.GetDefaultGetSQLCache(this.subtype);
                return Ret;
            }
        }
    }
    public class SetMemoryCacheValueType
    {
        public string TypeString
        {
            get
            {
                List<string>? RetList = new List<string>();
                RetList = GlobalsFunctions.GetCacheType(this.subtype);
                string Ret = "";
                Ret = GlobalsFunctions.GetTypeString(RetList);
                return Ret;
            }
        }
        public string subtype { get; set; } = "";
        public bool issetcache
        {
            get
            {
                bool Ret = this.defaultissetcache;
                if (this.setissetcache != null)
                    Ret = (bool)this.setissetcache;
                return Ret;
            }
        }
        public bool issetsqlcache
        {
            get
            {
                bool Ret = this.defaultissetsqlcache;
                if (this.setsqlissetcache != null)
                    Ret = (bool)this.setsqlissetcache;
                return Ret;
            }
        }
        public bool isremovecache
        {
            get
            {
                bool Ret = this.defaultisremovecache;
                if (this.setisremovecache != null)
                    Ret = (bool)this.setisremovecache;
                return Ret;
            }
        }
        public bool isremovesqlcache
        {
            get
            {
                bool Ret = this.defaultisremovesqlcache;
                if (this.setisremovesqlcache != null)
                    Ret = (bool)this.setisremovesqlcache;
                return Ret;
            }
        }
        public bool? setissetcache { get; set; } = null;
        public bool? setsqlissetcache { get; set; } = null;
        public bool? setisremovecache { get; set; } = null;
        public bool? setisremovesqlcache { get; set; } = null;
        public bool defaultissetcache
        {
            get
            {
                bool Ret = false;
                Ret = GlobalsFunctions.GetDefaultSetCache(this.subtype);
                return Ret;
            }
        }
        public bool defaultissetsqlcache
        {
            get
            {
                bool Ret = false;
                Ret = GlobalsFunctions.GetDefaultSetSQLCache(this.subtype);
                return Ret;
            }
        }
        public bool defaultisremovecache
        {
            get
            {
                bool Ret = false;
                Ret = GlobalsFunctions.GetDefaultRemoveCache(this.subtype);
                return Ret;
            }
        }
        public bool defaultisremovesqlcache
        {
            get
            {
                bool Ret = false;
                Ret = GlobalsFunctions.GetDefaultRemoveSQLCache(this.subtype);
                return Ret;
            }
        }
        public int cacheexpiryseconds
        {
            get
            {
                int Ret = 0;
                if (this.setcacheexpiryseconds != null)
                    Ret = (int)this.setcacheexpiryseconds;
                else
                    Ret = this.cacheexpirydefaultseconds;
                return Ret;
            }
        }
        public int? setcacheexpiryseconds { get; set; } = null;
        public int cacheexpirydefaultseconds
        {
            get
            {
                int Ret = 0;
                Ret = GlobalsFunctions.GetDefaultCacheExpirySeconds(this.subtype);
                return Ret;
            }
        }
    }
}
