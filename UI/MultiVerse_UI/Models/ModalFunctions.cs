using Data.DataAccess;
using Data.Dtos;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using static MultiVerse_UI.Models.ModalDtos;
using static MultiVerse_UI.Models.MVCAppEnum;
using static Data.Dtos.AppEnum;
using static Data.Dtos.CustomClasses;

namespace MultiVerse_UI.Models
{
    public class ModalFunctions
    {
        /// <summary>
        /// Get Modal Input Field For Table Row
        /// </summary>
        /// <param name="GetInputTypeString">Set from GetInputStringType Struct</param>
        public static string GetStandardTRInputString(string LabelName = "", bool IsRequired = false, bool IsHidden = false, string GetInputTypeString = GetInputStringType.text, string PlaceHolder = "", string id = "", object value = null, bool isdisabled = false, List<AttributeList> AttributeList = null)
        {
            return GetTRInputString(LabelColumnClass: "col-4", FieldColumnClass: "col-8", LabelName: LabelName, LabelClass: "fw-light mb-2", IsRequired: IsRequired, IsHidden: IsHidden, GetInputTypeString: GetInputTypeString, PlaceHolder: PlaceHolder, id: id, ClassName: "w-100 border-1", issmallformcontrol: true, value: value, isdisabled: isdisabled, datavalidationtypes: "", AttributeList: AttributeList);
        }
        /// <summary>
        /// Get Modal Input Field For Table Row
        /// </summary>
        /// <param name="GetInputTypeString">Set from GetInputStringType Struct</param>
        /// <param name="datavalidationtypes">Set from JavascriptValidationType Struct</param>
        public static string GetTRInputString(string LabelColumnClass = "col-4", string FieldColumnClass = "col-8", string LabelName = "", string LabelClass = "fw-light mb-2", bool IsRequired = false, bool IsHidden = false, string GetInputTypeString = GetInputStringType.text, string PlaceHolder = "", string id = "", string ClassName = "w-100 border-1", bool issmallformcontrol = true, object value = null, bool isdisabled = false, string datavalidationtypes = "", List<AttributeList> AttributeList = null, int noOfInput = 0, bool isMultiInput = false, int inputSize = int.MaxValue, int inputMaxlength = int.MaxValue, string inputTypeForNumber = "")
        {
            string InputTypeString = (GetInputTypeString == null ? GetInputStringType.text : GetInputTypeString);
            if (Information.IsDBNull(value))
                value = null;
            value = (value == null ? "" : value);
            string RequiredText = "";
            if (IsRequired == true)
                RequiredText = "<span class=\"text-red\"> *</span>";
            string FormControl = "form-control";
            if (issmallformcontrol)
                FormControl = "form-control-sm";

            string datavalidationtypesattribute = "";
            datavalidationtypesattribute = (datavalidationtypes == "" ? "" : "data-validation-types=" + "\"" + datavalidationtypes + "\"");

            string AttributeString = "";
            if (AttributeList != null)
            {
                if (AttributeList.Count > 0)
                {
                    for (int i = 0; i <= AttributeList.Count - 1; i++)
                    {
                        AttributeString += " " + AttributeList[i].Name + "=" + AttributeList[i].Value + " ";
                    }
                }
            }

            var IsHiddenText = "";
            if (IsHidden)
            {
                IsHiddenText = "d-none";
            }

            string HtmlString = "";
            HtmlString = string.Format("{0} <tr class=\"{1}\">", HtmlString, IsHiddenText);
            HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, LabelColumnClass);
            HtmlString = string.Format("{0} <h6 class=\"{1}\">{2}:{3}</h6>", HtmlString, LabelClass, LabelName, RequiredText);
            HtmlString = string.Format("{0} </td>", HtmlString);
            if (isMultiInput == true)
            {
                string display = "display:flex";

                HtmlString = string.Format("{0} <td  class=\"{1}\" style=\"{2}\">", HtmlString, FieldColumnClass, display);
            }
            else
            {
                HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, FieldColumnClass);
            }

            if (isMultiInput == true)
            {
                string? idforExt = "";
                string margin = "margin-right:20px;";
                int countListIndex = 0;
                string newValue = value.ToString().Trim();

                List<string> ListValues = new List<string>();

                if (newValue.Length == 10)
                {
                    ListValues.Add(newValue.Substring(0, 3));
                    ListValues.Add(newValue.Substring(3, 3));
                    ListValues.Add(newValue.Substring(6, 4));
                }
                else if (newValue.Length >= 14)
                {
                    ListValues.Add(newValue.Substring(0, 3));
                    ListValues.Add(newValue.Substring(3, 3));
                    ListValues.Add(newValue.Substring(6, 4));
                    ListValues.Add(newValue.Substring(10, 4));
                }


                if (inputTypeForNumber.ToLower().Trim() == "phone" && newValue.Length == 10)
                {
                    ListValues.Add("");
                }
                if (inputTypeForNumber.ToLower().Trim() == "phone")
                {
                    noOfInput += 4;
                }
                else
                {
                    noOfInput += 2;
                }

                for (int i = 0; i < noOfInput; i++)
                {
                    if (i == noOfInput - 1 && inputTypeForNumber.ToLower().Trim() != "phone")
                    {
                        inputSize += 1;
                        inputMaxlength += 1;
                        PlaceHolder = "XXXX";
                    }
                    else if (i == noOfInput - 3 && inputTypeForNumber.ToLower().Trim() == "phone")
                    {
                        inputSize += 1;
                        inputMaxlength += 1;
                        PlaceHolder = "XXXX";
                    }

                    if (i % 2 != 0)
                    {

                        string ClassNameforDisable = "form-control  widthfordisabple";


                        if (i == noOfInput - 2 && inputTypeForNumber.ToLower().Trim() == "phone")
                        {
                            newValue = "EXT";
                            PlaceHolder = "Ext";
                            idforExt = "idExt";
                            isdisabled = true;

                        }
                        else
                        {
                            newValue = "-";
                            isdisabled = true;
                        }

                        HtmlString = string.Format("{0} <input   type=\"{1}\" placeholder=\"{2}\" class=\"{3} {4} {10} \" id=\"{5}\" value=\"{6}\" {7} {8} {9} size=\"{11}\" style=\"{12}\" maxlength=\"{13}\"  />",
                       HtmlString, InputTypeString, PlaceHolder, FormControl, ClassNameforDisable, "_", newValue, (isdisabled == true ? "disabled" : ""), AttributeString
                    , datavalidationtypesattribute, (datavalidationtypesattribute == "" ? "custom-validation" : ""), inputSize, margin, inputMaxlength);
                    }
                    else
                    {

                        isdisabled = false;
                        string? newid = string.IsNullOrEmpty(id) ? id : id + "_" + i.ToString();
                        HtmlString = string.Format("{0} <input   type=\"{1}\" placeholder=\"{2}\" class=\"{3} {4} {10} \" id=\"{5}\" value=\"{6}\" {7} {8} {9} size=\"{11}\" style=\"{12}\" maxlength=\"{13}\"  />",
                    HtmlString, InputTypeString, PlaceHolder, FormControl, ClassName, (idforExt != "" ? idforExt : newid), (ListValues.Count > 0 ? ListValues[countListIndex] : ""), (isdisabled == true ? "disabled" : ""), AttributeString
                    , datavalidationtypesattribute, (datavalidationtypesattribute == "" ? "custom-validation" : ""), inputSize, margin, inputMaxlength);
                        countListIndex++;
                    }

                }

            }
            else
            {
                HtmlString = string.Format("{0} <input type=\"{1}\" placeholder=\"{2}\" class=\"{3} {4} {10} \" id=\"{5}\" value=\"{6}\" {7} {8} {9}/>",
                HtmlString, InputTypeString, PlaceHolder, FormControl, ClassName, id, value, (isdisabled == true ? "disabled" : ""), AttributeString
                , datavalidationtypesattribute, (datavalidationtypesattribute == "" ? "custom-validation" : ""));
            }

            if (datavalidationtypesattribute != "")
                HtmlString = string.Format("{0} <span class=\"validationError error\"></span>", HtmlString);
            HtmlString = string.Format("{0} </td>", HtmlString);
            HtmlString = string.Format("{0} </tr>", HtmlString);

            return HtmlString;
        }
        public static string GetStandardTRselectString(string LabelName = "", bool IsRequired = false, bool IsHidden = false, string PlaceHolder = "", string id = "", List<SelectDropDownList> selectLists = null, object value = null, bool isdisabled = false, List<AttributeList> AttributeList = null)
        {
            return GetTRselectString(LabelColumnClass: "col-4", FieldColumnClass: "col-8", LabelName: LabelName, LabelClass: "fw-light mb-2", IsRequired: IsRequired, IsHidden: IsHidden, PlaceHolder: PlaceHolder, id: id, ClassName: "w-100 border-1 custom-select", issmallformselect: true, isselect: true, selectLists: selectLists, IsSelectOption: true, value: value, isdisabled: isdisabled, datavalidationtypes: "", AttributeList: AttributeList);
        }
        /// <summary>
        /// Get Modal Select/DropDown Field For Table Row
        /// </summary>
        /// <param name="datavalidationtypes">Set from JavascriptValidationType Struct</param>
        public static string GetTRselectString(string LabelColumnClass = "col-4", string FieldColumnClass = "col-8", string LabelName = "", string LabelClass = "fw-light mb-2", bool IsRequired = false, bool IsHidden = false, string PlaceHolder = "", string id = "", string ClassName = "w-100 border-1 custom-select", bool issmallformselect = true, bool isselect = true, List<SelectDropDownList> selectLists = null, bool IsSelectOption = true, object value = null, bool isdisabled = false, string datavalidationtypes = "", List<AttributeList> AttributeList = null, bool ismultiselect = false, List<object>? listofobj = null, bool isCustomSelect = false)
        {
            if (listofobj == null)
                listofobj = new List<object>();

            if (Information.IsDBNull(value))
                value = null;
            value = (value == null ? "" : value);
            string RequiredText = "";
            if (IsRequired == true)
                RequiredText = "<span class=\"text-red\"> *</span>";
            string FormSelect = "form-select";
            if (issmallformselect)
                FormSelect = "form-select-sm";

            string datavalidationtypesattribute = "";
            datavalidationtypesattribute = (datavalidationtypes == "" ? "" : "data-validation-types=" + "\"" + datavalidationtypes + "\"");

            string AttributeString = "";
            if (AttributeList != null)
            {
                if (AttributeList.Count > 0)
                {
                    for (int i = 0; i <= AttributeList.Count - 1; i++)
                    {
                        AttributeString += " " + AttributeList[i].Name + "=" + AttributeList[i].Value + " ";
                    }
                }
            }

            var IsHiddenText = "";
            if (IsHidden)
            {
                IsHiddenText = "d-none";
            }


            string HtmlString = "";
            HtmlString = string.Format("{0} <tr class=\"{1}\">", HtmlString, IsHiddenText);
            HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, LabelColumnClass);
            HtmlString = string.Format("{0} <h6 class=\"{1}\">{2}:{3}</h6>", HtmlString, LabelClass, LabelName, RequiredText);
            HtmlString = string.Format("{0} </td>", HtmlString);
            HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, FieldColumnClass);
            if (isCustomSelect == true)
            {
                string selectoptions = "";
                string selectOptionValue = "";

                if (selectLists != null)
                {
                    if (selectLists.Any(a => a.code != value) && string.IsNullOrEmpty(value?.ToString()) == false)
                    {
                        selectLists.Add(new SelectDropDownList { name = value.ToString() + " - (Dropdown Value Deleted)", code = value });
                    }
                    for (var i = 0; i <= selectLists.Count - 1; i++)
                    {
                        selectoptions += string.Format("<div class=\"value\"  data-value=\"{0}\"  onclick=\"selectOption(this)\" >{1}</div>", selectLists[i].code, selectLists[i].name);
                    }
                }
                string? selectedValueText = "";
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    selectedValueText = selectLists.Where(x => x.code.ToString().ToLower() == value.ToString().ToLower()).Select(x => x.name).FirstOrDefault().ToString();
                }

                if (!string.IsNullOrEmpty(selectedValueText))
                {
                    selectOptionValue = string.Format("<div class=\"select-selected\"  id=\"{2}\" data-value=\"{0}\" onclick=\"toggleDropdown(this)\">{1}</div>", value, selectedValueText, id);
                }
                else
                {
                    selectOptionValue = string.Format("<div class=\"select-selected\" id=\"{0}\" data-value  onclick=\"toggleDropdown(this)\">[Select option]</div>", id);
                }
                string selectOptionHtml = "<div class=\"value\" data-value='' onclick=\"selectOption(this)\">[Select Option]</div>";

                HtmlString += string.Format("<div class=\"custom-select\" style=\"width:100%;\">" +
                             selectOptionValue +
                            "<div class=\"select-items\">{0}{1}</div>" +
                            "</div>", selectOptionHtml, selectoptions);

            }
            else
            {
                HtmlString = string.Format("{0} <select placeholder=\"{1}\" class=\"{2} {3} {4} {9}\" id=\"{5}\" {6} {7} {8}>",
               HtmlString, PlaceHolder, (isselect == true ? "select2" : ""), FormSelect, ClassName, id, (isdisabled == true ? "disabled" : ""), AttributeString
               , datavalidationtypesattribute, (datavalidationtypesattribute == "" ? "custom-validation" : ""));

                string selectoptions = "";
                bool isvalueselected = false;
                if (selectLists != null)
                {
                    //if (selectLists.Any(a => a.code != value) && !string.IsNullOrEmpty(value?.ToString()))
                    //{
                    //    selectLists.Add(new SelectDropDownList { name = value.ToString() + " - (Dropdown Value Deleted)", code = value });
                    //}
                    for (var i = 0; i <= selectLists.Count - 1; i++)
                    {
                        if ((selectLists[i].code.ToString() == value.ToString() && ismultiselect == false) || (listofobj.Any(a => a.ToString().ToLower().Trim() == selectLists[i].code.ToString().ToLower().Trim()) && ismultiselect))
                            selectoptions = string.Format("{0} <option selected value=\"{1}\">{2}</option>", selectoptions, selectLists[i].code, selectLists[i].name);
                        else
                            selectoptions = string.Format("{0} <option value=\"{1}\">{2}</option>", selectoptions, selectLists[i].code, selectLists[i].name);
                    }
                }
                if (IsSelectOption == true)
                    HtmlString = string.Format("{0} <option {3} value=\"{1}\">{2}</option> {4}", HtmlString, "", "[Select Option]", (isvalueselected == false ? "selected" : ""), selectoptions);

                HtmlString = string.Format("{0} </select>", HtmlString);

                if (datavalidationtypesattribute != "")
                    HtmlString = string.Format("{0} <span class=\"validationError error\"></span>", HtmlString);
            }


            HtmlString = string.Format("{0} </td>", HtmlString);
            HtmlString = string.Format("{0} </tr>", HtmlString);

            return HtmlString;
        }
        public static string GetStandardTRTextAreaString(string LabelName = "", bool IsRequired = false, string PlaceHolder = "", string id = "", object value = null, bool isdisabled = false, List<AttributeList> AttributeList = null)
        {
            return GetTRTextAreaString(LabelColumnClass: "col-4", FieldColumnClass: "col-8", LabelName: LabelName, LabelClass: "fw-light mb-2", IsRequired: IsRequired, rows: 5, PlaceHolder: PlaceHolder, id: id, ClassName: "w-100 border-1", value: value, isdisabled: isdisabled, datavalidationtypes: "", AttributeList: AttributeList);
        }
        /// <summary>
        /// Get Modal TextArea Field For Table Row
        /// </summary>
        /// <param name="datavalidationtypes">Set from JavascriptValidationType Struct</param>
        public static string GetTRTextAreaString(string LabelColumnClass = "col-4", string FieldColumnClass = "col-8", string LabelName = "", string LabelClass = "fw-light mb-2", bool IsRequired = false, int rows = 5, string PlaceHolder = "", string id = "", string ClassName = "w-100 border-1", object value = null, bool isdisabled = false, string datavalidationtypes = "", List<AttributeList> AttributeList = null)
        {
            if (Information.IsDBNull(value))
                value = null;
            value = (value == null ? "" : value);
            string RequiredText = "";
            if (IsRequired == true)
                RequiredText = "<span class=\"text-red\"> *</span>";
            string FormControl = ""; // "form-control";
            //if (issmallformcontrol)
            //    FormControl = "form-control-sm";

            string datavalidationtypesattribute = "";
            datavalidationtypesattribute = (datavalidationtypes == "" ? "" : "data-validation-types=" + "\"" + datavalidationtypes + "\"");

            string AttributeString = "";
            if (AttributeList != null)
            {
                if (AttributeList.Count > 0)
                {
                    for (int i = 0; i <= AttributeList.Count - 1; i++)
                    {
                        AttributeString += " " + AttributeList[i].Name + "=" + AttributeList[i].Value + " ";
                    }
                }
            }

            string HtmlString = "";
            HtmlString = string.Format("{0} <tr>", HtmlString);
            HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, LabelColumnClass);
            HtmlString = string.Format("{0} <h6 class=\"{1}\">{2}:{3}</h6>", HtmlString, LabelClass, LabelName, RequiredText);
            HtmlString = string.Format("{0} </td>", HtmlString);
            HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, FieldColumnClass);
            HtmlString = string.Format("{0} <textarea rows=\"{1}\" placeholder=\"{2}\" class=\"{3} {4} {10}\" id=\"{5}\" {7} {8} {9}>{6}</textarea>",
                HtmlString, rows, PlaceHolder, FormControl, ClassName, id, value, (isdisabled == true ? "disabled" : ""), AttributeString
                , datavalidationtypesattribute, (datavalidationtypesattribute == "" ? "custom-validation" : ""));
            if (datavalidationtypesattribute != "")
                HtmlString = string.Format("{0} <span class=\"validationError error\"></span>", HtmlString);
            HtmlString = string.Format("{0} </td>", HtmlString);
            HtmlString = string.Format("{0} </tr>", HtmlString);

            return HtmlString;
        }
        public static string GetStandardTRCheckBoxString(string LabelName = "", bool IsRequired = false, bool isratio = false, bool ischecked = false, string id = "", bool isdisabled = false, List<AttributeList> AttributeList = null)
        {
            return GetTRCheckBoxString(LabelColumnClass: "col-4", FieldColumnClass: "col-8", LabelName: LabelName, LabelClass: "fw-light mb-2", IsRequired: IsRequired, isratio: isratio, ischecked: ischecked, id: id, ClassName: "fw-light mb-2 me-2", isdisabled: isdisabled, datavalidationtypes: "", AttributeList: AttributeList);
        }
        /// <summary>
        /// Get Modal CheckBox/ Ratio Button Field For Table Row
        /// </summary>
        /// <param name="datavalidationtypes">Set from JavascriptValidationType Struct</param>
        public static string GetTRCheckBoxString(string LabelColumnClass = "col-4", string FieldColumnClass = "col-8", string LabelName = "", string LabelClass = "fw-light mb-2", bool IsRequired = false, bool isratio = false, bool ischecked = false, string id = "", string ClassName = "fw-light mb-2 me-2", bool isdisabled = false, string datavalidationtypes = "", List<AttributeList> AttributeList = null)
        {
            string InputTypeString = (isratio == true ? GetInputStringType.radio : GetInputStringType.checkbox);
            string RequiredText = "";
            if (IsRequired == true)
                RequiredText = "<span class=\"text-red\"> *</span>";

            string datavalidationtypesattribute = "";
            datavalidationtypesattribute = (datavalidationtypes == "" ? "" : "data-validation-types=" + "\"" + datavalidationtypes + "\"");

            string AttributeString = "";
            if (AttributeList != null)
            {
                if (AttributeList.Count > 0)
                {
                    for (int i = 0; i <= AttributeList.Count - 1; i++)
                    {
                        AttributeString += " " + AttributeList[i].Name + "=" + AttributeList[i].Value + " ";
                    }
                }
            }

            string HtmlString = "";
            HtmlString = string.Format("{0} <tr>", HtmlString);
            HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, LabelColumnClass);
            HtmlString = string.Format("{0} <h6 class=\"{1}\">{2}:{3}</h6>", HtmlString, LabelClass, LabelName, RequiredText);
            HtmlString = string.Format("{0} </td>", HtmlString);
            HtmlString = string.Format("{0} <td class=\"{1}\">", HtmlString, FieldColumnClass);
            HtmlString = string.Format("{0} <input type=\"{1}\" class=\"{2} {8}\" id=\"{3}\" {4} {5} {6} {7}/>",
                HtmlString, InputTypeString, ClassName, id, (isdisabled == true ? "disabled" : ""), AttributeString, (ischecked == true ? "checked" : "")
                , datavalidationtypesattribute, (datavalidationtypesattribute == "" ? "custom-validation" : "")); ;
            if (datavalidationtypesattribute != "")
                HtmlString = string.Format("{0} <span class=\"validationError error\"></span>", HtmlString);
            HtmlString = string.Format("{0} </td>", HtmlString);
            HtmlString = string.Format("{0} </tr>", HtmlString);

            return HtmlString;
        }
        /// <summary>
        /// Get Modal
        /// </summary>
        /// <param name="getmodelsize">Set from GetModalSize Struct</param>
        public static string GetModal(string getmodelsize = null, string modalheaderclass = "Theme-Header", string modaltitle = "", string onclickmodalclose = "resetmodal('dynamic-modal1')", string modalbodytabletbody = "", string modalfooterclass = "", string modalfootercancelbuttonname = "Cancel", string modalfootersuccessbuttonname = "Change", string modalfootersuccessbuttonclass = "Theme-button", string onclickmodalsuccess = "", string modaltableid = "")
        {
            string modelsize = (getmodelsize == null ? GetModalSize.modal_md : getmodelsize);
            string HtmlString = "";
            HtmlString = string.Format("{0} <div class=\"modal-dialog {1} modal-dialog-scrollable\">", HtmlString, modelsize);
            HtmlString = string.Format("{0} <div class=\"modal-content\">", HtmlString);
            HtmlString = string.Format("{0} <div class=\"modal-header modal-colored-header {1}\">", HtmlString, modalheaderclass);
            HtmlString = string.Format("{0} <h4 class=\"modal-title\" id=\"title_modal\">{1}</h4>", HtmlString, modaltitle);
            HtmlString = string.Format("{0} <button type=\"button\" class=\"btn-close\" data-bs-dismiss=\"modal\" onclick=\"{1};\" aria-label=\"Close\"></button>", HtmlString, onclickmodalclose);
            HtmlString = string.Format("{0} </div>", HtmlString);
            HtmlString = string.Format("{0} <div class=\"modal-body\">", HtmlString);
            HtmlString = string.Format("{0} <table id=\"{1}\" class=\"table-sm table-borderless mb-0 v-middle w-100\">", HtmlString, modaltableid);
            HtmlString = string.Format("{0} <tbody>", HtmlString);

            HtmlString = string.Format("{0} {1}", HtmlString, modalbodytabletbody);

            HtmlString = string.Format("{0} </tbody>", HtmlString);
            HtmlString = string.Format("{0} </table>", HtmlString);
            HtmlString = string.Format("{0} ", HtmlString);
            HtmlString = string.Format("{0} </div>", HtmlString);
            HtmlString = string.Format("{0} <div class=\"modal-footer {1}\">", HtmlString, modalfooterclass);
            HtmlString = string.Format("{0} <button type=\"button\" class=\"btn btn-light-danger text-danger font-weight-medium\" data-bs-dismiss=\"modal\" onclick=\"{1};\">{2}</button>", HtmlString, onclickmodalclose, modalfootercancelbuttonname);
            HtmlString = string.Format("{0} <button type=\"button\" class=\"btn {3} font-weight-medium\" onclick=\"{2};\">{1}</button>", HtmlString, modalfootersuccessbuttonname, onclickmodalsuccess, modalfootersuccessbuttonclass);
            HtmlString = string.Format("{0} </div>", HtmlString);
            HtmlString = string.Format("{0} </div>", HtmlString);
            HtmlString = string.Format("{0} </div>", HtmlString);
            return HtmlString;
        }

        /// <summary>
        /// used for the cp Mtv Modal
        /// </summary>
        public static string GetCPMasterTypeValueTableForModal(List<MasterType_value> MasterTypeValueList)
        {
            //string HtmlString = "";
            bool isModalDynamicRow = false;

            //string masterTypeTr = "";
            string parentTableTr = "";


            parentTableTr += "<tr id=\"dynamicMTVTableList\" class=\"\"><td colspan=\"4\"><table class=\" table table-hover\"><thead><tr class=\"table-light\"><th class=\"col-2\">MTV Code</th><th class=\"col-3\">MtV Name</th><th class=\"col-3\">Sub Master Type Name</th> <th class=\"col-4\">Sub MTV Name</th> <th class=\"d-none\">Sub MT ID</th>  <th class=\"d-none\">Sub MTV Value ID</th>  <th class=\"\">IsActive</th><th class=\"\">Action</th></tr></thead><tbody>";
            int countid = 1;
            foreach (var masterTypeValue in MasterTypeValueList)
            {

                P_GetCPMasterTypeForGenericModalTable_Response MasterTypeEdit = new P_GetCPMasterTypeForGenericModalTable_Response();
                List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "Sub_MTV_ID";
                Dynamic_SP_Params.Val = masterTypeValue.Sub_MTV_ID;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
                MasterTypeEdit = StaticPublicObjects.ado.ExecuteSelectSQLMap<P_GetCPMasterTypeForGenericModalTable_Response>("P_GetCPMasterTypeForGenericModalTable", true, 0, ref List_Dynamic_SP_Params);


                string masterTypeTr = string.Format("<tr id=\"RowNo_{0}\">", countid);
                masterTypeTr += string.Format("<td class=\"col-2\"><input type = \"text\" placeholder=\"MTV Code\" class=\"form-control-sm form-control custom-validation\" id=\"idMTV_Code_{1}\" value=\"{0}\" disabled></td>", masterTypeValue.MTV_CODE, countid);

                masterTypeTr += string.Format("<td class=\"col-3\"><input type = \"text\" placeholder=\"MtV Name\" class=\"form-control-sm form-control custom-validation\" id=\"idMTV_Name_{1}\" value=\"{0}\" disabled></td>", masterTypeValue.Name, countid);

                masterTypeTr += string.Format("<td class=\"col-3\"><input type = \"text\" placeholder=\"Sub Master Type Name\" class=\"form-control-sm form-control custom-validation\" id=\"idSubMT_Name_{1}\" value=\"{0}\" disabled></td>", MasterTypeEdit != null ? MasterTypeEdit.MasterTypeName : "", countid);

                masterTypeTr += string.Format("<td class=\"col-4\"><input type = \"text\" placeholder=\"Sub Master Type Value Name\" class=\"form-control-sm form-control custom-validation\" id=\"idSubMTV_Name_{1}\" value=\"{0}\" disabled></td>", MasterTypeEdit != null ? MasterTypeEdit.MasterTypeValueName : "", countid);
                masterTypeTr += string.Format("<td class=\"d-none\"><input type = \"text\" placeholder=\"Sub Master Type ID\" class=\"form-control-sm form-control custom-validation\" id=\"idSubMT_{1}\" value=\"{0}\" disabled></td>", MasterTypeEdit != null ? MasterTypeEdit.MT_ID : "", countid);
                masterTypeTr += string.Format("<td class=\"d-none\"><input type = \"text\" placeholder=\"Sub Master Type Value ID\" class=\"form-control-sm form-control custom-validation\" id=\"idSubMTV_{1}\" value=\"{0}\" disabled></td>", MasterTypeEdit != null ? MasterTypeEdit.MTV_ID : "", countid);


                masterTypeTr += "<td class=\"\">";
                if (masterTypeValue.IsActive == true)
                {
                    masterTypeTr += string.Format("<img  id=\"ImageRowNo_{2}\" src=\"../icon/tick.png\" alt=\"Active\" class=\"cursor-pointer\" onclick=\"RemoveMTV_CODE(this, '{0}', '{1}')\" style=\"margin-left: 8px\"/>", masterTypeValue.Encrypted_MTV_ID, countid, countid);
                }
                else
                {
                    masterTypeTr += string.Format("<img  id=\"ImageRowNo_{2}\" src=\"../icon/cross.png\" alt=\"InActive\" class=\"cursor-pointer\" onclick=\"RemoveMTV_CODE(this, '{0}', '{1}')\" style=\"margin-left: 8px\"/>", masterTypeValue.Encrypted_MTV_ID, countid, countid);
                }
                masterTypeTr += "</td>";

                masterTypeTr += "<td class=\"\">";
                masterTypeTr += string.Format("<a  class=\"\"><i class=\"fa fa-edit Theme-icon ms-1 cursor-pointer\" style=\"font-size: 20px;\"  onclick=EditMTV_CODE(\"this\",\"{0}\",\"{1}\")></i></a>", masterTypeValue.Encrypted_MTV_ID, countid);
                masterTypeTr += "</td>";



                masterTypeTr += "</tr>";

                parentTableTr += masterTypeTr;
                countid++;
            }

            parentTableTr += "</tbody></table></td></tr>";

            return parentTableTr;
        }
        public static string GetModalWithBody(GetModalDetail getModalDetail, string MTVTable = "")
        {
            string HtmlString = "";
            string BodyHtmlString = getModalDetail.modalbodytabletbody;
            List<ModalBodyTypeInfo> modalBodyTypeInfos = getModalDetail.modalBodyTypeInfos;

            if (modalBodyTypeInfos != null)
            {
                BodyHtmlString = "";
                for (int i = 0; i < modalBodyTypeInfos.Count; i++)
                {
                    ModalBodyTypeInfo obj = modalBodyTypeInfos[i];
                    if (obj.ModelBodyType == GetModelBodyType.TRInput)
                    {
                        BodyHtmlString += GetTRInputString(LabelColumnClass: obj.LabelColumnClass, FieldColumnClass: obj.FieldColumnClass, LabelName: obj.LabelName, LabelClass: obj.LabelClass
                            , IsRequired: obj.IsRequired, IsHidden: obj.IsHidden, GetInputTypeString: obj.GetInputTypeString, PlaceHolder: obj.PlaceHolder, id: obj.id, ClassName: obj.GetClassName
                            , issmallformcontrol: obj.issmallformcontrol, value: obj.value, isdisabled: obj.isdisabled, datavalidationtypes: obj.datavalidationtypes, AttributeList: obj.AttributeList
                            , obj.noOfInput, obj.isMultiInput, obj.inputSize, obj.inputMaxlength, obj.inputTypeForNumber);
                    }
                    else if (modalBodyTypeInfos[i].ModelBodyType == GetModelBodyType.TRselect)
                    {
                        BodyHtmlString += GetTRselectString(LabelColumnClass: obj.LabelColumnClass, FieldColumnClass: obj.FieldColumnClass, LabelName: obj.LabelName, LabelClass: obj.LabelClass
                            , IsRequired: obj.IsRequired, IsHidden: obj.IsHidden, PlaceHolder: obj.PlaceHolder, id: obj.id, ClassName: obj.GetClassName, selectLists: obj.selectLists
                            , issmallformselect: obj.issmallformselect, value: obj.value, isdisabled: obj.isdisabled, datavalidationtypes: obj.datavalidationtypes, AttributeList: obj.AttributeList, ismultiselect: obj.ismultiselect, listofobj: obj.listofobj, isselect: obj.isselect, isCustomSelect: obj.isCustomSelect);
                    }
                    else if (modalBodyTypeInfos[i].ModelBodyType == GetModelBodyType.TRTextArea)
                    {
                        BodyHtmlString += GetTRTextAreaString(LabelColumnClass: obj.LabelColumnClass, FieldColumnClass: obj.FieldColumnClass, LabelName: obj.LabelName, LabelClass: obj.LabelClass
                            , IsRequired: obj.IsRequired, rows: obj.rows, PlaceHolder: obj.PlaceHolder, id: obj.id, ClassName: obj.GetClassName
                            , value: obj.value, isdisabled: obj.isdisabled, datavalidationtypes: obj.datavalidationtypes, AttributeList: obj.AttributeList);
                    }
                    else if (modalBodyTypeInfos[i].ModelBodyType == GetModelBodyType.TRCheckBox)
                    {
                        BodyHtmlString += GetTRCheckBoxString(LabelColumnClass: obj.LabelColumnClass, FieldColumnClass: obj.FieldColumnClass, LabelName: obj.LabelName, LabelClass: obj.LabelClass
                            , IsRequired: obj.IsRequired, isratio: obj.isratio, ischecked: obj.ischecked, id: obj.id, ClassName: obj.GetClassName
                            , isdisabled: obj.isdisabled, datavalidationtypes: obj.datavalidationtypes, AttributeList: obj.AttributeList);
                    }

                }
                if (!string.IsNullOrEmpty(MTVTable))
                {
                    BodyHtmlString += MTVTable;
                }
            }

            HtmlString = GetModal(getmodelsize: getModalDetail.getmodelsize, modalheaderclass: getModalDetail.modalheaderclass, modaltitle: getModalDetail.modaltitle
                , onclickmodalclose: getModalDetail.onclickmodalclose, modalbodytabletbody: BodyHtmlString, modalfooterclass: getModalDetail.modalfooterclass
                , modalfootercancelbuttonname: getModalDetail.modalfootercancelbuttonname, modalfootersuccessbuttonname: getModalDetail.modalfootersuccessbuttonname
                , modalfootersuccessbuttonclass: getModalDetail.modalfootersuccessbuttonclass, onclickmodalsuccess: getModalDetail.onclickmodalsuccess, modaltableid: getModalDetail.modaltableid);

            return HtmlString;
        }
        public static string SetOrders(string StrOrder, bool Seperation_Logic_Is_Text)
        {
            StrOrder = StrOrder.Replace("'", "");
            StrOrder = StrOrder.Replace(',', Strings.Chr(13));
            StrOrder = StrOrder.Replace(' ', Strings.Chr(13));
            StrOrder = StrOrder.Replace(Strings.Chr(10), Strings.Chr(13));
            string StrRet = "";
            string[] A;
            A = StrOrder.Split(Strings.Chr(13));
            foreach (var OrderNo in A)
            {
                if (OrderNo.Trim() != "")
                {
                    if (StrRet == "")
                    {
                        if (Seperation_Logic_Is_Text)
                            StrRet = string.Format("'{0}'", OrderNo.Trim());
                        else
                            StrRet = string.Format("{0}", OrderNo.Trim());
                    }
                    else if (Seperation_Logic_Is_Text)
                        StrRet = string.Format("{0},'{1}'", StrRet, OrderNo.Trim());
                    else
                        StrRet = string.Format("{0},{1}", StrRet, OrderNo.Trim());
                }
            }
            if (!Seperation_Logic_Is_Text)
            {
            }
            return StrRet;
        }
        public static void SetReportFilterClause(ref ReportParams reportParams)
        {
            string FilterClause = " ";
            for (int i = 0; i <= reportParams.ReportFilterObjectList.Count - 1; i++)
            {
                List<SQLReportFilterObjectArry> ListSQLReportFilterObjectArry = new List<SQLReportFilterObjectArry>();
                string TempFilterClause = " ";
                for (int z = 0; z <= reportParams.ReportFilterObjectList[i].reportFilterObjectArry.Count - 1; z++)
                {
                    SQLReportFilterObjectArry sQLReportFilterObjectArry = new SQLReportFilterObjectArry();
                    GetSQLReportFilterOperator(ref sQLReportFilterObjectArry, reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z]);
                    ListSQLReportFilterObjectArry.Add(sQLReportFilterObjectArry);
                    string code = reportParams.ReportFilterObjectList[i].Code;
                    if (reportParams.ReportFilterObjectList[i].SRFieldType == KendoGridFilterSRFieldType.LowerString && reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Value.ToString() != "")
                    {
                        code = $"lower({reportParams.ReportFilterObjectList[i].Code})";
                    }
                    else if (reportParams.ReportFilterObjectList[i].SRFieldType == KendoGridFilterSRFieldType.UpperString && reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Value.ToString() != "")
                    {
                        code = $"upper({reportParams.ReportFilterObjectList[i].Code})";
                    }
                    else if (reportParams.ReportFilterObjectList[i].SRFieldType == KendoGridFilterSRFieldType.Date)
                    {
                        code = $"cast({reportParams.ReportFilterObjectList[i].Code} as date)";
                    }
                    if (reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Type == KendoGridFilterType.isnullorempty || reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Type == KendoGridFilterType.isnotnullorempty)
                        code = $"isnull({code},'')";

                    if (z == 0)
                    {
                        TempFilterClause = $" {code} {sQLReportFilterObjectArry.Type} {sQLReportFilterObjectArry.Value} ";
                    }
                    else
                    {
                        TempFilterClause += $" {reportParams.ReportFilterObjectList[i].reportFilterObjectArry[z].Logic} {code} {sQLReportFilterObjectArry.Type} {sQLReportFilterObjectArry.Value} ";
                    }
                }
                if (TempFilterClause.Trim() != "")
                    FilterClause += $" AND ({TempFilterClause}) ";

                reportParams.ReportFilterObjectList[i].sQLReportFilterObjectArry = ListSQLReportFilterObjectArry;
            }
            reportParams.FilterClause = FilterClause;

        }
        public static void GetSQLReportFilterOperator(ref SQLReportFilterObjectArry sQLReportFilterObjectArry, ReportFilterObjectArry reportFilterObjectArry)
        {
            sQLReportFilterObjectArry = new SQLReportFilterObjectArry();
            object val = reportFilterObjectArry.Value;
            if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Date)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "1900-01-01";
                else if (Information.IsDate(reportFilterObjectArry.Value))
                    val = Convert.ToDateTime(reportFilterObjectArry.Value.ToString()).ToString("yyyy-MM-dd");
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Datetime)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "1900-01-01";
                else if (Information.IsDate(reportFilterObjectArry.Value))
                    val = Convert.ToDateTime(reportFilterObjectArry.Value.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Float)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = 0.00;
                else if (Information.IsNumeric(reportFilterObjectArry.Value))
                    val = Convert.ToDouble(reportFilterObjectArry.Value.ToString());
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Int)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = 0;
                else if (Information.IsNumeric(reportFilterObjectArry.Value))
                    val = Convert.ToInt64(reportFilterObjectArry.Value.ToString());
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.LowerString)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "";
                else
                    val = reportFilterObjectArry.Value.ToString().ToLower();
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.UpperString)
            {
                if (reportFilterObjectArry.Value.ToString() == "")
                    val = "";
                else
                    val = reportFilterObjectArry.Value.ToString().ToUpper();
            }
            else if (reportFilterObjectArry.SRFieldType == KendoGridFilterSRFieldType.Boolean)
            {
                if (Convert.ToBoolean(reportFilterObjectArry.Value.ToString()))
                    val = 1;
                else
                    val = 0;
            }

            if (reportFilterObjectArry.Type == KendoGridFilterType.inlistfilter || reportFilterObjectArry.Type == KendoGridFilterType.notinlistfilter)
            {
                if (val.ToString() == "")
                    val = "''";
                else
                    val = SetOrders(val.ToString(), true);
            }

            if (reportFilterObjectArry.Type == KendoGridFilterType.contains)
            {
                sQLReportFilterObjectArry.Type = " like ";
                sQLReportFilterObjectArry.Value = $"'%{val}%'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.doesnotcontain)
            {
                sQLReportFilterObjectArry.Type = " not like ";
                sQLReportFilterObjectArry.Value = $"'%{val}%'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.notequal && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Date && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Boolean && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Number)
            {
                sQLReportFilterObjectArry.Type = " not in ";
                sQLReportFilterObjectArry.Value = $"('{val}')";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.notequal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number)
            {
                sQLReportFilterObjectArry.Type = " not in ";
                sQLReportFilterObjectArry.Value = $"({val})";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.notequal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Boolean)
            {
                sQLReportFilterObjectArry.Type = " <> ";
                sQLReportFilterObjectArry.Value = val;
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.notequal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Date)
            {
                sQLReportFilterObjectArry.Type = " <> ";
                sQLReportFilterObjectArry.Value = $"'{val}'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.equal && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Date && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Boolean && reportFilterObjectArry.FieldType != KendoGridFilterFieldType.Number)
            {
                sQLReportFilterObjectArry.Type = " in ";
                sQLReportFilterObjectArry.Value = $"('{val}')";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.equal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number)
            {
                sQLReportFilterObjectArry.Type = " in ";
                sQLReportFilterObjectArry.Value = $"({val})";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.equal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Boolean)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = val;
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.equal && reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Date)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = $"'{val}'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.startswith)
            {
                sQLReportFilterObjectArry.Type = " like ";
                sQLReportFilterObjectArry.Value = $"'{val}%'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.endswith)
            {
                sQLReportFilterObjectArry.Type = " like ";
                sQLReportFilterObjectArry.Value = $"'%{val}'";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnull)
            {
                sQLReportFilterObjectArry.Type = " is ";
                sQLReportFilterObjectArry.Value = $"null";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnotnull)
            {
                sQLReportFilterObjectArry.Type = " is not ";
                sQLReportFilterObjectArry.Value = $"null";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.orderno)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isempty)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnotempty)
            {
                sQLReportFilterObjectArry.Type = " <> ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnullorempty)
            {
                sQLReportFilterObjectArry.Type = " = ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isnotnullorempty)
            {
                sQLReportFilterObjectArry.Type = " <> ";
                sQLReportFilterObjectArry.Value = $"''";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isequalorgreather)
            {
                sQLReportFilterObjectArry.Type = " >= ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.greather)
            {
                sQLReportFilterObjectArry.Type = " > ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.isequalorless)
            {
                sQLReportFilterObjectArry.Type = " <= ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.less)
            {
                sQLReportFilterObjectArry.Type = " < ";
                sQLReportFilterObjectArry.Value = (reportFilterObjectArry.FieldType == KendoGridFilterFieldType.Number ? val : $"'{val}'");
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.notinlistfilter)
            {
                sQLReportFilterObjectArry.Type = " not in ";
                sQLReportFilterObjectArry.Value = $"({val})";
            }
            else if (reportFilterObjectArry.Type == KendoGridFilterType.inlistfilter)
            {
                sQLReportFilterObjectArry.Type = " in ";
                sQLReportFilterObjectArry.Value = $"({val})";
            }
        }
        public static void GetKendoFilter(ref ReportParams _ReportParams, ref List<Dynamic_SP_Params> List_Dynamic_SP_Params, PublicClaimObjects _PublicClaimObjects, bool IsUserNameSet)
        {
            ReportResponse reportResponse = new ReportResponse();
            SetReportFilterClause(ref _ReportParams);

            Int32 pagesize = Convert.ToInt32(_ReportParams.PageSize);
            Int32 pageindex = Convert.ToInt32(_ReportParams.PageIndex);
            string sortExpression = _ReportParams.SortExpression;

            List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageIndex";
            Dynamic_SP_Params.Val = pageindex;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "PageSize";
            Dynamic_SP_Params.Val = pagesize;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "SortExpression";
            Dynamic_SP_Params.Val = sortExpression;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "FilterClause";
            Dynamic_SP_Params.Val = _ReportParams.FilterClause;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "TotalRowCount";
            Dynamic_SP_Params.Val = 0;
            Dynamic_SP_Params.IsInputType = false;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            //Dynamic_SP_Params = new Dynamic_SP_Params();
            //Dynamic_SP_Params.ParameterName = "TimeZoneID";
            //Dynamic_SP_Params.Val = 13;
            //if (_PublicClaimObjects.P_Get_User_Info_Class != null)
            //    Dynamic_SP_Params.Val = _PublicClaimObjects.P_Get_User_Info_Class?.TimeZoneID;
            //List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "FilterObject";
            Dynamic_SP_Params.Val = JsonConvert.SerializeObject(_ReportParams.ReportFilterObjectList);
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "ColumnObject";
            Dynamic_SP_Params.Val = JsonConvert.SerializeObject(_ReportParams.ReportColumnObjectList);
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            if (IsUserNameSet)
            {
                Dynamic_SP_Params = new Dynamic_SP_Params();
                Dynamic_SP_Params.ParameterName = "Username";
                Dynamic_SP_Params.Val = _PublicClaimObjects.username;
                List_Dynamic_SP_Params.Add(Dynamic_SP_Params);
            }

        }
        public static ShowHideReportColumnObject GetGridReportTemplateColumns(PublicClaimObjects _PublicClaimObjects, int GRL_ID, int UGRTL_ID, string GRGUID, string UGRCGUID, string kendoid, string kendofunctionname, bool iscollapse = true)
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = _PublicClaimObjects.username;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            //Dynamic_SP_Params = new Dynamic_SP_Params();
            //Dynamic_SP_Params.ParameterName = "UserType_MTV_CODE";
            //Dynamic_SP_Params.Val = _PublicClaimObjects.P_Get_User_Info_Class.UserTypeMTVCode;
            //List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "GRL_ID";
            Dynamic_SP_Params.Val = GRL_ID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UGRTL_ID";
            Dynamic_SP_Params.Val = UGRTL_ID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "GRGUID";
            Dynamic_SP_Params.Val = GRGUID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "UGRCGUID";
            Dynamic_SP_Params.Val = UGRCGUID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            ShowHideReportColumnObject result = new ShowHideReportColumnObject();
            List<ReportColumnObject> reportColumnObjects = new List<ReportColumnObject>();
            reportColumnObjects = StaticPublicObjects.ado.ExecuteSelectSQLMapList<ReportColumnObject>("P_Get_User_Grid_Report_Columns_List", true, 1000, ref List_Dynamic_SP_Params, true);

            result.kendoid = kendoid;
            result.kendofunctionname = kendofunctionname;
            result.iscollapse = iscollapse;
            result.reportColumnObjectList = reportColumnObjects;

            return result;

        }
        public static List<ReportFilterDropDownList> GetGridUserReportTemplateList(PublicClaimObjects _PublicClaimObjects, int GRL_ID, string GRGUID)
        {
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = new List<Dynamic_SP_Params>();
            Dynamic_SP_Params Dynamic_SP_Params = new Dynamic_SP_Params();

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "Username";
            Dynamic_SP_Params.Val = _PublicClaimObjects.username;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            //Dynamic_SP_Params = new Dynamic_SP_Params();
            //Dynamic_SP_Params.ParameterName = "UserType_MTV_CODE";
            //Dynamic_SP_Params.Val = _PublicClaimObjects.P_Get_User_Info_Class.UserTypeMTVCode;
            //List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "GRL_ID";
            Dynamic_SP_Params.Val = GRL_ID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            Dynamic_SP_Params = new Dynamic_SP_Params();
            Dynamic_SP_Params.ParameterName = "GRGUID";
            Dynamic_SP_Params.Val = GRGUID;
            List_Dynamic_SP_Params.Add(Dynamic_SP_Params);

            List<ReportFilterDropDownList> ReportTemplateList = StaticPublicObjects.ado.ExecuteSelectSQLMapList<ReportFilterDropDownList>("P_Get_User_Grid_Report_Template_List", true, 1000, ref List_Dynamic_SP_Params, true);

            return ReportTemplateList;

        }
        public static List<SelectDropDownList> GetDropDownListByID(int ID, string code, string name, string tableName, string mapTableName, string filterByColumn, bool IsDistinct = false, string orderby = "")
        {
            var removeduplicate = "";
            List<Dynamic_SP_Params> paramsList = new List<Dynamic_SP_Params> { new Dynamic_SP_Params() { ParameterName = "ID", Val = ID } };
            if (IsDistinct == true)
            {
                removeduplicate = "DISTINCT";
            }
            string query = $"SELECT {removeduplicate} code = {code}, name = {name} FROM [POMS_DB].[dbo].[{tableName}] tn with (nolock)" +
                           $" WHERE {code} NOT IN (SELECT {code} FROM [POMS_DB].[dbo].[{mapTableName}] with (nolock) WHERE {filterByColumn} = @ID) AND IsActive = 1";
            if (orderby != "")
            {
                query = $"{query} {orderby}";
            }
            List<SelectDropDownList> result = StaticPublicObjects.ado.ExecuteSelectSQLMapList<SelectDropDownList>(query, false, 0, ref paramsList);
            return result;
        }
        public static List<SelectDropDownList> GetDropDownListByQuery(int ID, string query)
        {
            List<Dynamic_SP_Params> paramsList = new List<Dynamic_SP_Params> { new Dynamic_SP_Params() { ParameterName = "ID", Val = ID } };
            List<SelectDropDownList> result = StaticPublicObjects.ado.ExecuteSelectSQLMapList<SelectDropDownList>(query, false, 0, ref paramsList);
            return result;
        }
        public static List<SelectDropDownList> GetDropDownListCommon(string code, string name, string tableName, bool active = false, string filterBy = "", string orderByColumn = "")
        {
            string query = $"SELECT code = {code}, name = {name} FROM [POMS_DB].[dbo].[{tableName}] with (nolock)";
            if (active)
            {
                query += $" WHERE IsActive = 1";
                if (filterBy != "")
                {
                    query += " AND " + filterBy;
                }
            }
            else
            {
                if (filterBy != "")
                {
                    query += " WHERE" + filterBy;
                }
            }

            if (!string.IsNullOrWhiteSpace(orderByColumn))
            {
                query += $" ORDER BY {orderByColumn}";
            }
            List<SelectDropDownList> result = StaticPublicObjects.ado.Get_DropDownList_Result(query);
            return result;
        }
        public static bool IsPasswordValid(string password, int passwordlength)
        {
            // Define regular expressions for each criterion
            var hasCapitalLetter = new Regex(@"[A-Z]").IsMatch(password);
            var hasSmallLetter = new Regex(@"[a-z]").IsMatch(password);
            var hasSpecialCharacter = new Regex(@"[!@#$%^&*()_+{}\[\]:;<>,.?~\\/\-=]").IsMatch(password);
            var hasNumber = new Regex(@"\d").IsMatch(password);
            var haslength = password.Length < passwordlength ? false : true;

            // Check if all criteria are met
            return hasCapitalLetter && hasSmallLetter && hasSpecialCharacter && hasNumber && haslength;
        }
        public static bool IsValidEmail(string originalemail)
        {
            bool isvalidemail = false;
            if (string.IsNullOrWhiteSpace(originalemail))
                return false;

            try
            {
                string[] emailobj;
                emailobj = originalemail.Split(';');
                // Normalize the domain
                foreach (string s in emailobj)
                {
                    string email = s;
                    email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                    // Examines the domain part of the email and normalizes it.
                    string DomainMapper(Match match)
                    {
                        // Use IdnMapping class to convert Unicode domain names.
                        var idn = new IdnMapping();

                        // Pull out and process domain name (throws ArgumentException on invalid)
                        string domainName = idn.GetAscii(match.Groups[2].Value);

                        return match.Groups[1].Value + domainName;
                    }
                    isvalidemail = Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
                    if (isvalidemail == false)
                        break;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsValidEmail", SmallMessage: e.Message, Message: e.ToString());
                return false;
            }
            catch (ArgumentException e)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsValidEmail 1", SmallMessage: e.Message, Message: e.ToString());
                return false;
            }

            try
            {
                return isvalidemail;
            }
            catch (RegexMatchTimeoutException e)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "IsValidEmail 2", SmallMessage: e.Message, Message: e.ToString());
                return false;
            }
        }
        public static string GetSortingModelWithData(string query)
        {
            string HtmlString = "";
            List<Dynamic_SP_Params> List_Dynamic_SP_Params = null;
            List<Sorting_Result> SortList = StaticPublicObjects.ado.ExecuteSelectSQLMapList<Sorting_Result>(query, false, 0, ref List_Dynamic_SP_Params);
            foreach (var item in SortList)
            {
                HtmlString += "<tr>";
                HtmlString += "<td>";
                HtmlString += "<img src = '../icon/up.png' class='cursor-pointer up mr-1' /><br />";
                HtmlString += "<img src = '../icon/down.png' class='cursor-pointer down mr-1' /><br />";
                HtmlString += "</td>";
                HtmlString += "<td id='New_Sort_Value' class='text-center'>" + item.New_Sort_Value + "</td>";
                HtmlString += "<td id='Sort_ID' class='text-center d-none'>" + item.Sort_ID + "</td>";
                HtmlString += "<td id='Sort_Text' class='text-center'>" + item.Sort_Text + "</td>";
                HtmlString += "<td id='Old_Sort_Value' class='text-center'>" + item.Old_Sort_Value + "</td>";
                HtmlString += "<td id='Sort_Input' class='text-center'><input type='number' class='form-control Sort_Input_Value' placeholder='Sort' onkeyup='moveToPosition(this)'></td>";
                HtmlString += "</tr>";
            }
            return HtmlString;
        }
        public static T? GetValueFromReturnParameter<T>(List<Dynamic_SP_Params> List_Dynamic_SP_Params, string ParameterName, Type type_)
        {
            object? returnvalue = null;
            for (int i = 0; i <= List_Dynamic_SP_Params.Count - 1; i++)
            {
                if (type_ == typeof(int))
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName && Information.IsNumeric(List_Dynamic_SP_Params[i].Val))
                        returnvalue = Convert.ToInt32(List_Dynamic_SP_Params[i].Val);
                }
                else if (type_ == typeof(long))
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName && Information.IsNumeric(List_Dynamic_SP_Params[i].Val))
                        returnvalue = Convert.ToInt64(List_Dynamic_SP_Params[i].Val);
                }
                else if (type_ == typeof(string))
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName && List_Dynamic_SP_Params[i].Val != null)
                        returnvalue = List_Dynamic_SP_Params[i].Val.ToString();
                }
                else
                {
                    if (List_Dynamic_SP_Params[i].ParameterName == ParameterName)
                        returnvalue = List_Dynamic_SP_Params[i].Val;
                }
            }
            return (T?)returnvalue;
        }

        public static string GenerateOtp()
        {
            var random = new Random();
            return random.Next(101010, 909090).ToString();
        }


        public static string GetEmailBodyHtml(string OTP)
        {
            string HtmlString = "";
            string brand = "Career portal";
            string otp = OTP;

            HtmlString = string.Format("{0}<div style=\"font-family: Helvetica, Arial, sans-serif; min-width: 1000px; overflow: auto; line-height: 2;\">", HtmlString);
            HtmlString = string.Format("{0}<div style=\"margin: 50px auto; width: 70%; padding: 20px 0;\">", HtmlString);
            HtmlString = string.Format("{0}<div style=\"border-bottom: 1px solid #eee;\">", HtmlString);
            HtmlString = string.Format("{0}<a href=\"\" style=\"font-size: 1.4em; color: #00466a; text-decoration: none; font-weight: 600;\">{1}</a>", HtmlString, brand);
            HtmlString = string.Format("{0}</div>", HtmlString);
            HtmlString = string.Format("{0}<p style=\"font-size: 1.1em;\">Hi,</p>", HtmlString);
            HtmlString = string.Format("{0}<p>Thank you for choosing {1}. Use the following OTP to complete your Sign-Up procedures. OTP is valid for 1 minute.</p>", HtmlString, brand);
            HtmlString = string.Format("{0}<h2 style=\"background: #00466a; margin: 0 auto; width: 90px; padding: 0 10px; color: #fff; border-radius: 4px; text-align: center;\">{1}</h2>", HtmlString, otp);
            HtmlString = string.Format("{0}<p style=\"color: red; text-align: center;\">Do not share this OTP with others.</p>", HtmlString);
            HtmlString = string.Format("{0}<p style=\"font-size: 0.9em;\">Regards,<br />{1}</p>", HtmlString, brand);
            HtmlString = string.Format("{0}<hr style=\"border: none; border-top: 1px solid #eee;\" />", HtmlString);
            HtmlString = string.Format("{0}<div style=\"float: right; padding: 8px 0; color: #aaa; font-size: 0.8em; line-height: 1; font-weight: 300;\">", HtmlString);
            HtmlString = string.Format("{0}<p>{1} Inc</p>", HtmlString, brand);
            HtmlString = string.Format("{0}</div>", HtmlString);
            HtmlString = string.Format("{0}</div>", HtmlString);
            HtmlString = string.Format("{0}</div>", HtmlString);


            return HtmlString;
        }

        public static string MaskEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return email;

            int atIndex = email.IndexOf('@');
            if (atIndex < 3)
                return email;
            string firstThree = "";
            string lastThreeBeforeAt = "";

            if (atIndex < 4)
            {
                firstThree = email.Substring(0, 1);
                lastThreeBeforeAt = email.Substring(atIndex - 1, 1);

            }
            else if (atIndex < 6)
            {
                firstThree = email.Substring(0, 2);
                lastThreeBeforeAt = email.Substring(atIndex - 2, 2);
            }
            else
            {
                firstThree = email.Substring(0, 3);
                lastThreeBeforeAt = email.Substring(atIndex - 3, 3);
            }

            string maskedPart = "******";
            string domain = email.Substring(atIndex);

            return firstThree + maskedPart + lastThreeBeforeAt + domain;
        }
    }
}
