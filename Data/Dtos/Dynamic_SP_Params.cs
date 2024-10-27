using Data.DataAccess;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Dtos
{
    public class Dynamic_SP_Params
    {
        public string ParameterName { get; set; } = "";
        public object? Val { get; set; } = null;
        public bool IsInputType { get; set; } = true;
        public bool IsCustomSetValueType { get; set; } = true;
        private Type _SetValueType = typeof(object);
        public Type SetValueType
        {
            get
            {
                return this._SetValueType;
            }
            set
            {
                this._SetValueType = value;
            }
        }
        public Type GetValueType
        {
            get
            {
                if (this.IsCustomSetValueType)
                    return this._SetValueType;
                else
                    return this.Val.GetType();
            }
        }
        public int Size { get; set; } = -1;
    }
    public class SelectDropDownList
    {
        public object code { get; set; }
        public string name { get; set; }
    }
    public class SelectDropDownListWithEncryption
    {
        public int ID { get; set; }
        public string code
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(this.ID);
            }
        }
        public string name { get; set; }
    }
    public class SelectDropDownListWithEncryptionString
    {
        private string _code = "";
        [SwaggerSchema(ReadOnly = true)]
        public string code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = Globals.ConvertDBNulltoNullIfExistsString(value) ?? "";
            }
        }
        public string encryptedcode
        {
            get
            {
                return Crypto.EncryptString(code);
            }
        }
        public string name { get; set; }
    }
}
