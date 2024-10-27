using Data.DataAccess;
using Data.Dtos;
using Microsoft.VisualBasic;
using System.Data;

namespace MultiVerse_UI.Models
{
    public class Side_Menu
    {
        public static string GetSideBar()
        {
            IHttpContextAccessor _HttpContextAccessor = StaticPublicObjects.ado.GetIHttpContextAccessor();
            DataTable DT = new DataTable();
            DataRow DR = null;
            DataSet DS = new DataSet();
            DataTable DTSUb;
            string Ret = "";
            string LitText = "";
            string CurrentURL = "";
            CurrentURL = StaticPublicObjects.ado.GetRequestPath();
            int Current_PG_ID = 0;
            int Current_P_ID = 0;

            string? Last_Page_Login = _HttpContextAccessor.HttpContext.Session.GetString("Last_Page_Login");
            if (Last_Page_Login != null)
            {
                if (CurrentURL.ToLower() == Last_Page_Login.ToLower())
                {
                    return Ret = LitText;
                }
                else
                {
                    (string Name, object Value)[] ParamsNameValues = { ("PageURL", (Strings.Left(CurrentURL, 1) == "/" ? "" : "/") + CurrentURL) };
                    DR = StaticPublicObjects.ado.ExecuteSelectDR("select CurrentPG_ID=p.PG_ID ,CurrentP_ID=p.P_ID from [dbo].[T_Page] p with (nolock) where p.PageURL= @PageURL", ParamsNameValues);
                }
            }

            if (DR == null)
            {
                List<Dynamic_SP_Params> List_dynamic_SP_Params = new List<Dynamic_SP_Params>();
                Dynamic_SP_Params dynamic_SP_Params = new Dynamic_SP_Params();


                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "Application_MTV_ID";
                dynamic_SP_Params.Val = AppEnum.ApplicationId.CareerPortalAppID;
                List_dynamic_SP_Params.Add(dynamic_SP_Params);

                dynamic_SP_Params = new Dynamic_SP_Params();
                dynamic_SP_Params.ParameterName = "CurrentURL";
                dynamic_SP_Params.Val = CurrentURL;
                List_dynamic_SP_Params.Add(dynamic_SP_Params);

                bool IsApplicantLogin = _HttpContextAccessor.HttpContext.Session.GetBool("IsApplicantLogin") ?? false;

                if (IsApplicantLogin == false)
                {
                    dynamic_SP_Params = new Dynamic_SP_Params();
                    dynamic_SP_Params.ParameterName = "USERNAME";
                    dynamic_SP_Params.Val = UserLogic_Security.CurrentUserStatic.UserName;
                    List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("P_Get_Pages_Info_By_User", ref List_dynamic_SP_Params);
                }
                else
                {
                    dynamic_SP_Params = new Dynamic_SP_Params();
                    dynamic_SP_Params.ParameterName = "Applicant_ID";
                    dynamic_SP_Params.Val = UserLogic_Security.CurrentUserStatic.UserName;
                    List_dynamic_SP_Params.Add(dynamic_SP_Params);
                    DS = StaticPublicObjects.ado.ExecuteStoreProcedureDS("P_Get_Pages_Info_By_Applicant_User", ref List_dynamic_SP_Params);
                }

                DT = DS.Tables[0];
                DTSUb = DS.Tables[1];

                if (DT.Rows.Count > 0 && Current_PG_ID == 0)
                {
                    Current_PG_ID = Convert.ToInt32(DT.Rows[0]["CurrentPG_ID"]);
                }

                if (DTSUb.Rows.Count > 0 && Current_P_ID == 0)
                {
                    Current_P_ID = Convert.ToInt32(DTSUb.Rows[0]["CurrentP_ID"]);
                }
            }
            else
            {
                DT = _HttpContextAccessor.HttpContext.Session.GetObject<DataTable>("PageGroupDT");
                DTSUb = _HttpContextAccessor.HttpContext.Session.GetObject<DataTable>("PageDT");
                Current_PG_ID = _HttpContextAccessor.HttpContext.Session.GetIntNotNull("CurrentPG_ID");
                Current_P_ID = _HttpContextAccessor.HttpContext.Session.GetIntNotNull("CurrentP_ID");

                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT.Rows[i]["CurrentPG_ID"] = Current_PG_ID;
                    DT.Rows[i]["PageGroupSelected"] = "";
                    DT.Rows[i]["PageGroupActive"] = "";
                    DT.Rows[i]["PageGroupActiveIn"] = "";
                    if (Convert.ToInt32(DT.Rows[i]["PG_ID"]) == Current_PG_ID)
                    {
                        DT.Rows[i]["PageGroupSelected"] = "selected";
                        DT.Rows[i]["PageGroupActive"] = "active";
                        DT.Rows[i]["PageGroupActiveIn"] = "in";
                    }
                }
                for (int i = 0; i < DTSUb.Rows.Count; i++)
                {
                    DTSUb.Rows[i]["CurrentP_ID"] = Current_P_ID;
                    DTSUb.Rows[i]["PageActive"] = "";
                    if (Convert.ToInt32(DTSUb.Rows[i]["P_ID"]) == Current_P_ID)
                    {
                        DTSUb.Rows[i]["PageActive"] = "active";
                    }
                }
            }

            _HttpContextAccessor.HttpContext.Session.SetString("Last_Page_Login", CurrentURL.ToLower());
            _HttpContextAccessor.HttpContext.Session.SetObject<DataTable>("PageGroupDT", DT);
            _HttpContextAccessor.HttpContext.Session.SetObject<DataTable>("PageDT", DTSUb);
            _HttpContextAccessor.HttpContext.Session.SetInt("CurrentPG", Current_PG_ID);

            return Ret = LitText;
        }
    }
}
