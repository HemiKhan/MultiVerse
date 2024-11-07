namespace Data.DataAccess
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HidePropertyAttribute : Attribute
    {
        /// <summary>
        /// if true than other parameters will be check and based on that field will be hide or show in serialization (object to string). if no other conditions were applied then hide.
        /// </summary>
        public bool IsHideSerialize { get; set; } = false;
        /// <summary>
        /// if true than other parameters will be check and based on that field will be hide or show in deserialization (string to object). if no other conditions were applied then hide.
        /// </summary>
        public bool IsHideDeSerialize { get; set; } = false;
        /// <summary>
        /// if true then field will be checked from public object isignorejsonserializeproperty and if true then will be *show* during serialization. priority for this field is 2.
        /// </summary>
        public bool IsCheckHideSerializeFromPublicObject { get; set; } = false;
        /// <summary>
        /// if true then field will be checked from public object isignorejsondeserializeproperty and if true then will be *show* during deserialization. priority for this field is 2.
        /// </summary>
        public bool IsCheckHideDeSerializeFromPublicObject { get; set; } = false;
        /// <summary>
        /// if true then it will be checked that public object ignorejsonserializepropertylist is null or not. if not null and exists then field will not be *show* during serialization. priority for this field is 3.
        /// </summary>
        public bool IsCheckHideSerializeFromPublicObjectList { get; set; } = false;
        /// <summary>
        /// if true then it will be checked that public object ignorejsondeserializepropertylist is null or not. if not null and exists then field will not be *show* during deserialization. priority for this field is 3.
        /// </summary>
        public bool IsCheckHideDeSerializeFromPublicObjectList { get; set; } = false;

        private string[]? _RemoteDomainIncludeHideSerialize = null;
        /// <summary>
        /// if not null then it will be checked that RemoteDomain exists or not. if exists then field will be *show* during serialization. priority for this field is 3.
        /// </summary>
        public string[]? RemoteDomainIncludeHideSerialize
        {
            get
            {
                return _RemoteDomainIncludeHideSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _RemoteDomainIncludeHideSerialize = Ret;
            }
        }
        private string[]? _RemoteDomainExcludeHideSerialize = null;
        /// <summary>
        /// if not null then it will be checked that RemoteDomain exists or not. if exists then field will be *hide* during serialization. priority for this field is 4.
        /// </summary>
        public string[]? RemoteDomainExcludeHideSerialize
        {
            get
            {
                return _RemoteDomainExcludeHideSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _RemoteDomainExcludeHideSerialize = Ret;
            }
        }
        private string[]? _RemoteDomainIncludeHideDeSerialize = null;
        /// <summary>
        /// if not null then it will be checked that RemoteDomain exists or not. if exists then field will be *show* during deserialization. priority for this field is 3.
        /// </summary>
        public string[]? RemoteDomainIncludeHideDeSerialize
        {
            get
            {
                return _RemoteDomainIncludeHideDeSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _RemoteDomainIncludeHideDeSerialize = Ret;
            }
        }
        private string[]? _RemoteDomainExcludeHideDeSerialize = null;
        /// <summary>
        /// if not null then it will be checked that RemoteDomain exists or not. if exists then field will be *hide* during deserialization. priority for this field is 4.
        /// </summary>
        public string[]? RemoteDomainExcludeHideDeSerialize
        {
            get
            {
                return _RemoteDomainExcludeHideDeSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _RemoteDomainExcludeHideDeSerialize = Ret;
            }
        }
        private string[]? _UserIncludeHideSerialize = null;
        /// <summary>
        /// if not null then it will be checked that claim username exists or not. if exists then field will be *show* during serialization. priority for this field is 5.
        /// </summary>
        public string[]? UserIncludeHideSerialize
        {
            get
            {
                return _UserIncludeHideSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _UserIncludeHideSerialize = Ret;
            }
        }
        private string[]? _UserExcludeHideSerialize = null;
        /// <summary>
        /// if not null then it will be checked that claim username exists or not. if exists then field will be *hide* during serialization. priority for this field is 6.
        /// </summary>
        public string[]? UserExcludeHideSerialize
        {
            get
            {
                return _UserExcludeHideSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _UserExcludeHideSerialize = Ret;
            }
        }
        private string[]? _UserIncludeHideDeSerialize = null;
        /// <summary>
        /// if not null then it will be checked that claim username exists or not. if exists then field will be *show* during deserialization. priority for this field is 5.
        /// </summary>
        public string[]? UserIncludeHideDeSerialize
        {
            get
            {
                return _UserIncludeHideDeSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _UserIncludeHideDeSerialize = Ret;
            }
        }
        private string[]? _UserExcludeHideDeSerialize = null;
        /// <summary>
        /// if not null then it will be checked that claim username exists or not. if exists then field will be *hide* during dedeserialization. priority for this field is 6.
        /// </summary>
        public string[]? UserExcludeHideDeSerialize
        {
            get
            {
                return _UserExcludeHideDeSerialize;
            }
            set
            {
                string[]? Ret = value;
                if (Ret != null)
                {
                    for (int i = 0; i <= Ret.Length - 1; i++)
                    {
                        Ret[i] = Ret[i].ToLower();
                    }
                }
                _UserExcludeHideDeSerialize = Ret;
            }
        }

    }
}
