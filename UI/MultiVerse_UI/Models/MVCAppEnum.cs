namespace MultiVerse_UI.Models
{
    public class MVCAppEnum
    {
        public struct GetModelBodyType
        {
            public const string TRInput = "TRInput";
            public const string TRselect = "TRselect";
            public const string TRTextArea = "TRTextArea";
            public const string TRCheckBox = "TRCheckBox";
        }
        public struct GetInputStringType
        {
            public const string button = "button";
            public const string checkbox = "checkbox";
            public const string color = "color";
            public const string date = "date";
            public const string datetime = "datetime";
            public const string datetime_local = "datetime-local";
            public const string email = "email";
            public const string file = "file";
            public const string hidden = "hidden";
            public const string image = "image";
            public const string month = "month";
            public const string number = "number";
            public const string password = "password";
            public const string password1 = "password1";
            public const string radio = "radio";
            public const string range = "range";
            public const string reset = "reset";
            public const string search = "search";
            public const string submit = "submit";
            public const string tel = "tel";
            public const string text = "text";
            public const string text1 = "text1";
            public const string time = "time";
            public const string url = "url";
            public const string week = "week";
        }
        public struct GetModalSize
        {
            public const string modal_sm = "modal-sm";
            public const string modal_md = "modal-md";
            public const string modal_lg = "modal-lg";
            public const string modal_xl = "modal-xl";
        }

        public struct RightsList_ID
        {
            #region UserSetup;     
            public const int User_Setup_View = 202100;
            public const int User_Setup_Add = 202101;
            public const int User_Setup_Edit = 202102;
            public const int User_Setup_Delete = 202103;
            #endregion UserSetup;

            #region PageSetup;     
            public const int Page_Setup_View = 203100;
            public const int Page_Setup_Add = 203101;
            public const int Page_Setup_Edit = 203102;
            public const int Page_Setup_Delete = 203103;
            #endregion PageSetup;

            #region RoleSetup;     
            public const int Role_Setup_View = 204100;
            public const int Role_Setup_Add = 204101;
            public const int Role_Setup_Edit = 204102;
            public const int Role_Setup_Delete = 204103;
            #endregion RoleSetup;
            
            #region RightsSetup;     
            public const int Right_Setup_View = 205100;
            public const int Right_Setup_Add = 205101;
            public const int Right_Setup_Edit = 205102;
            public const int Right_Setup_Delete = 205103;
            #endregion RightsSetup;
            
            #region MasterSetup;
            public const int Master_Setup_View = 207100;
            public const int Master_Setup_Add = 207101;
            public const int Master_Setup_Edit = 207102;
            public const int Master_Setup_Delete = 207103;
            #endregion MasterSetup;
            
            #region AuditHistory;     
            public const int Audit_History_View = 206100;
            #endregion ;
        }
    }
}
