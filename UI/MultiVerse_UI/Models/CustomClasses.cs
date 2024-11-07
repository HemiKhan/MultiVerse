using Data.Dtos;
using static MultiVerse_UI.Models.MVCAppEnum;

namespace MultiVerse_UI.Models
{
    public class ModalDtos
    {
        public class LoginSaveCookie
        {
            public string UserName { get; set; } = "";
            public string Password { get; set; } = "";
            public bool RememberMe { get; set; } = false;
            public int LocalTimeZone { get; set; } = 0;
            public string LocalTimeZoneName { get; set; } = "";
        }
        public class GetModalDetail
        {
            /// <summary>
            /// Set Value from GetModalSize Struct
            /// </summary>
            public string getmodelsize { get; set; } = GetModalSize.modal_md;
            public string modalheaderclass { get; set; } = "Theme-Header";
            public string modaltitle { get; set; } = "";
            public string onclickmodalclose { get; set; } = "resetmodal('dynamic-modal1')";
            public string modalbodytabletbody { get; set; } = "";
            public string modalfooterclass { get; set; } = "";
            public string modalfootercancelbuttonname { get; set; } = "Cancel";
            public string modalfootersuccessbuttonname { get; set; } = "Change";
            public string modalfootersuccessbuttonclass { get; set; } = "Theme-button";
            public string onclickmodalsuccess { get; set; } = "";
            public string modaltableid { get; set; } = "";
            public List<ModalBodyTypeInfo> modalBodyTypeInfos { get; set; } = null;
        }
        public class ModalBodyTypeInfo
        {
            /// <summary>
            /// Set Value from GetModelBodyType Struct
            /// </summary>
            public string ModelBodyType { get; set; } = null;
            public string LabelColumnClass { get; set; } = "col-4";
            public string FieldColumnClass { get; set; } = "col-8";
            public string LabelName { get; set; } = "";
            public string LabelClass { get; set; } = "fw-light mb-2";
            public bool IsRequired { get; set; } = false;
            public bool IsHidden { get; set; } = false;
            /// <summary>
            /// Set Value from GetInputStringType Struct. Required if ModelBodyType is TRInput
            /// </summary>
            public string GetInputTypeString { get; set; } = null;
            public string PlaceHolder { get; set; } = "";
            public string id { get; set; } = "";
            public string ClassName { get; set; } = "";
            /// <summary>
            /// Required if ModelBodyType is TRInput
            /// </summary>
            public bool issmallformcontrol { get; set; } = true;
            public object value { get; set; } = null;
            public bool isdisabled { get; set; } = false;
            /// <summary>
            /// Set Value from JavascriptValidationType Struct
            /// </summary>
            public string datavalidationtypes { get; set; } = "";
            public List<AttributeList> AttributeList = new List<AttributeList>();
            /// <summary>
            /// Required if ModelBodyType is TRSelect
            /// </summary>
            public bool issmallformselect { get; set; } = true;
            /// <summary>
            /// Required if ModelBodyType is TRSelect
            /// </summary>
            public bool isselect { get; set; } = true;
            /// <summary>
            /// Required if ModelBodyType is TRSelect
            /// </summary>
            public bool ismultiselect { get; set; } = false;
            /// <summary>
            /// Optional when ismultiselect is true
            /// </summary>
            public List<object> listofobj { get; set; } = new List<object>();
            /// <summary>
            /// Required if ModelBodyType is TRSelect
            /// </summary>
            public List<SelectDropDownList> selectLists { get; set; } = null;
            /// <summary>
            /// Required if ModelBodyType is TRSelect
            /// </summary>
            public bool IsSelectOption { get; set; } = true;
            /// <summary>
            /// Required if ModelBodyType is TRTextArea
            /// </summary>
            public int rows { get; set; } = 5;
            /// <summary>
            /// Required if ModelBodyType is TRCheckBox
            /// </summary>
            public bool isratio { get; set; } = false;
            /// <summary>
            /// Required if ModelBodyType is TRCheckBox
            /// </summary>
            public bool ischecked { get; set; } = false;
            /// <summary>
            /// required for mobile and phone and phoneExt
            /// </summary>
            public int noOfInput { get; set; } = 0;
            public bool isMultiInput { get; set; } = false;
            public int inputSize { get; set; } = int.MaxValue;
            public int inputMaxlength { get; set; } = int.MaxValue;
            public string inputTypeForNumber { get; set; } = "";

            /// <summary>
            /// required for the custom select 
            public bool isCustomSelect { get; set; } = false;
            /// </summary>
            public string GetClassName
            {
                get
                {
                    string Ret = "";
                    if (this.ClassName != "")
                        Ret = this.ClassName;
                    else if (this.ModelBodyType == GetModelBodyType.TRInput)
                        Ret = "w-100 border-1";
                    else if (this.ModelBodyType == GetModelBodyType.TRselect)
                        Ret = "w-100 border-1 custom-select";
                    else if (this.ModelBodyType == GetModelBodyType.TRTextArea)
                        Ret = "w-100 border-1";
                    else if (this.ModelBodyType == GetModelBodyType.TRCheckBox)
                        Ret = "fw-light mb-2 me-2";
                    return Ret;
                }
            }

        }
        public class AttributeList
        {
            public string Name { get; set; } = "";
            public object Value { get; set; } = null;
        }
    }
}
