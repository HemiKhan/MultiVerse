using Data.DataAccess;
using Data.Dtos;
using Data.Models;

namespace Services.AccountServices
{
    public class AccountService : IAccountService
    {
        public P_ReturnMessage_Result SignIn(LoginRequestDto req)
        {
            P_ReturnMessage_Result result = new P_ReturnMessage_Result();
            try
            {
                if (string.IsNullOrWhiteSpace(req.UserID))
                {
                    result.ReturnCode = false;
                    result.ReturnText = "user name is required!";
                    return result;
                }

                if (string.IsNullOrWhiteSpace(req.Password))
                {
                    result.ReturnCode = false;
                    result.ReturnText = "password is required!";
                    return result;
                }

                List<Dynamic_SP_Params> _Params = new List<Dynamic_SP_Params>();
                _Params.Add(new Dynamic_SP_Params { ParameterName = "UserName", Val = req.UserID });
                _Params.Add(new Dynamic_SP_Params { ParameterName = "Password", Val = req.Password });
                result = StaticPublicObjects.ado.P_ExecuteProc_Result("P_SignIn", _Params);
                if (!result.ReturnCode)
                {
                    StaticPublicObjects.logFile.ErrorLog(FunctionName: "SignIn", SmallMessage: result.ReturnText!, Message: result.ReturnText!);
                }
            }
            catch (Exception ex)
            {
                StaticPublicObjects.logFile.ErrorLog(FunctionName: "SignIn", SmallMessage: ex.Message, Message: ex.ToString());
            }
            return result;
        }
    }
}
