using Data.DataAccess;
using Data.Dtos;
using Newtonsoft.Json;

namespace Data.Models
{

    public class P_Get_PatientReport_List
    {
        public int RowNo { get; set; }
        public int PR_ID { get; set; }
        public string Ery_PR_ID
        {
            get
            {
                return Crypto.EncryptNumericToStringWithOutNull(PR_ID);
            }
        }
        public int RT_ID { get; set; }
        public int InvoiceNo { get; set; }
        public string? Report_Title { get; set; }
        public string? Indication { get; set; }
        public string? ViralStatus { get; set; }
        public string? Template_Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsTemplate { get; set; }
    }

    public class P_AddOrEdit_PatientReport_Response
    {
        public int PR_ID { get; set; }
        public int RT_ID { get; set; }
        public int Invoice_ID { get; set; }
        public string? Report_Title { get; set; }
        public string? Indication { get; set; }
        public string? ViralStatus { get; set; }
        public bool IsActive { get; set; }
    }

    public class P_Get_Patient_Report_Detail
    {
        [JsonProperty("PR_ID")]
        public int PR_ID { get; set; }

        [JsonProperty("InvoiceNo")]
        public int InvoiceNo { get; set; }

        [JsonProperty("RT_ID")]
        public int RT_ID { get; set; }

        [JsonProperty("Report_Title")]
        public string? Report_Title { get; set; }

        [JsonProperty("Doctor_Name")]
        public string? Doctor_Name { get; set; }

        [JsonProperty("Patient_Name")]
        public string? Patient_Name { get; set; }

        [JsonProperty("Report_Template_Html")]
        public string Report_Template_Html { get; set; } = default!;

        [JsonProperty("MR")]
        public string? MR { get; set; }

        [JsonProperty("Date")]
        public string? Date { get; set; }

        [JsonProperty("AgeGender")]
        public string? AgeGender { get; set; }

        [JsonProperty("ViralStatus")]
        public string? ViralStatus { get; set; }

        [JsonProperty("Indication")]
        public string? Indication { get; set; }

        [JsonProperty("Report_Headers")]
        public List<Patient_Report_Section_Header>? Patient_Report_Section_Header { get; set; }

        [JsonProperty("Report_Body")]
        public List<Patient_Report_Section_Body>? Patient_Report_Section_Body { get; set; }
    }

    public class Patient_Report_Section_Header
    {
        [JsonProperty("PRD_ID")]
        public int PRD_ID { get; set; }
        [JsonProperty("Report_Header_Text")]
        public string? Report_Header_Text { get; set; }

        [JsonProperty("Report_Header_Value")]
        public bool Report_Header_Value { get; set; }
    }

    public class Patient_Report_Section_Body
    {
        [JsonProperty("PRD_ID")]
        public int PRD_ID { get; set; }
        [JsonProperty("Report_Body_Text")]
        public string? Report_Body_Text { get; set; }

        [JsonProperty("Report_Body_Value")]
        public string? Report_Body_Value { get; set; }
    }

    public class S_Dropdown_List_Str
    {
        public List<SelectDropDownList>? Drop_List { get; set; }
        public string? Drop_Options { get; set; }
    }
}
