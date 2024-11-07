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
            #region JobSetup;
            public const int Job_Setup_View = 207100;
            public const int Job_Setup_Add = 207101;
            public const int Job_Setup_Edit = 207102;
            public const int Job_Setup_Delete = 207103;
            #endregion JobSetup;
        }
    }
}
