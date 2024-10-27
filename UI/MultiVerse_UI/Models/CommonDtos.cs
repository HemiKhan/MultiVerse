using Data.DataAccess;
using static Data.Dtos.CustomClasses;

namespace MultiVerse_UI.Models
{
    #region Mtv Modal

    public class P_GetCPMasterTypeForModal_Lists_Response
    {
        public int MT_ID { get; set; } = 0;
        public string Encrypted_MT_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(MT_ID);
            }
        }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }

    public class P_GetCPMasterTypeValueForModal_Lists_Response
    {

        public int MT_ID { get; set; } = 0;
        public string? MasterTypeName { get; set; } = string.Empty;
        public int? Sub_MTV_ID { get; set; } = 0;
        public List<MasterType_value> masterType_Value { get; set; } = new List<MasterType_value>();


    }

    public class MasterType_value
    {
        public int MTV_ID { get; set; } = 0;
        public string? Name { get; set; } = string.Empty;
        public string? MTV_CODE { get; set; } = string.Empty;
        public int? Sub_MTV_ID { get; set; } = 0;
        public string Encrypted_MTV_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(MTV_ID);
            }
        }
        public bool IsActive { get; set; }
    }

    public class AddOrEditCPMasterTypeValue_request
    {

        public int MT_ID { get; set; }
        public string? MasterTypeValueName { get; set; }
        public string? MTV_CODE { get; set; }
        [ExcludeFromDynamicSPParamsAttribute]
        public string? Encrypted_MTV_ID { get; set; }
        public int? MTV_ID
        {
            get
            {
                return Crypto.DecryptNumericToStringWithOutNull(Encrypted_MTV_ID);
            }

        }
        public int Sub_MTV_ID { get; set; }
        public bool Active { get; set; }
    }

    public class P_CPReturnMessage_Result
    {
        public bool ReturnCode { get; set; } = false;
        public bool IsActiveChange { get; set; } = false;
        public string? ReturnText { get; set; } = "";
    }

    public class AddOrUpdateCPMTVModalTable
    {
        public bool ReturnCode { get; set; } = false;
        public string? ReturnText { get; set; } = "";
        public string? MTVListTableHtml { get; set; } = "";
    }

    public class P_GetCPMasterTypeForGenericModalTable_Response
    {

        public int? MT_ID { get; set; } = 0;
        public int? MTV_ID { get; set; } = 0;
        public string? MasterTypeValueName { get; set; } = string.Empty;
        public string? MasterTypeName { get; set; } = string.Empty;

    }


    #endregion Mtv Modal 
}
