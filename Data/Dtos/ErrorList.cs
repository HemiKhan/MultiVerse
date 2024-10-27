namespace Data.Dtos
{
    public class ErrorList
    {
        public struct ErrorListInvalidCustomer
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "INCUS";
            public const string ErrorMsg = "Invalid Customer";
            public const string DetailErrorMsg = "Invalid Customer";
        }
        public struct ErrorListCustomerBlocked
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CUSBLK";
            public const string ErrorMsg = "Customer is Blocked";
            public const string DetailErrorMsg = "Customer is Blocked";
        }
        public struct ErrorListInvalidBillingType
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "INBT";
            public const string ErrorMsg = "Invalid Billing Type";
            public const string DetailErrorMsg = "Invalid Billing Type";
        }
        public struct ErrorListInvalidSellerKey
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "INCUSKEY";
            public const string ErrorMsg = "Invalid Customer Key";
            public const string DetailErrorMsg = "Invalid Customer Key";
        }
        public struct ErrorListInternalServerError
        {
            public const int StatusCode = 500;
            public const string ErrorCode = "INTSRVERR";
            public const string ErrorMsg = "Internal Server Error";
            public const string DetailErrorMsg = "Internal Server Error";
        }
        public struct ErrorListSystemDown
        {
            public const int StatusCode = 503;
            public const string ErrorCode = "SYTDOWN";
            public const string ErrorMsg = "System is Down for Maintenance";
            public const string DetailErrorMsg = "System is Down for Maintenance";
        }
        public struct ErrorListUnableToGenerateQuoteID
        {
            public const int StatusCode = 500;
            public const string ErrorCode = "UNBGENQTID";
            public const string ErrorMsg = "Unable to Generate QuoteID";
            public const string DetailErrorMsg = "Unable to Generate QuoteID";
        }
        public struct ErrorListInvalidJson
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "INJSON";
            public const string ErrorMsg = "Invalid JSON";
            public const string DetailErrorMsg = "Invalid JSON";
        }
        public struct ErrorListArryLimit
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "ARRYLMT";
            public const string ErrorMsg = "Json Array is Reached";
            public const string DetailErrorMsg = "Json Array is Reached";
        }
        public struct ErrorListLimitReached
        {
            public const int StatusCode = 429;
            public const string ErrorCode = "LMTRH";
            public const string ErrorMsg = "Limit Reached";
            public const string DetailErrorMsg = "Limit Reached";
        }
        public struct ErrorListLimitNotSetup
        {
            public const int StatusCode = 429;
            public const string ErrorCode = "LMTNST";
            public const string ErrorMsg = "Limit is Not Setup";
            public const string DetailErrorMsg = "Limit is Not Setup, Please Contact Metro to Setup Throttling";
        }
        public struct ErrorListInvalidAuth
        {
            public const int StatusCode = 401;
            public const string ErrorCode = "INAUTH";
            public const string ErrorMsg = "Invalid Authentication";
            public const string DetailErrorMsg = "Invalid Authentication";
        }
        public struct ErrorListNoRight
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "NORIGHT";
            public const string ErrorMsg = "No Right";
            public const string DetailErrorMsg = "No Right";
        }
        public struct ErrorListBlock
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "BLOCK";
            public const string ErrorMsg = "Blocked";
            public const string DetailErrorMsg = "Blocked";
        }
        public struct ErrorListNotWhiteList
        {
            public const int StatusCode = 429;
            public const string ErrorCode = "NWHTLT";
            public const string ErrorMsg = "Not White Listed";
            public const string DetailErrorMsg = "Not White Listed";
        }
        public struct ErrorListInvalidToken
        {
            public const int StatusCode = 401;
            public const string ErrorCode = "INTOKN";
            public const string ErrorMsg = "Invalid Token";
            public const string DetailErrorMsg = "Invalid Token";
        }
        public struct ErrorListInvalidRefreshToken
        {
            public const int StatusCode = 401;
            public const string ErrorCode = "INRFTOKN";
            public const string ErrorMsg = "Invalid Refresh Token";
            public const string DetailErrorMsg = "Invalid Refresh Token";
        }
        public struct ErrorListTokenRequired
        {
            public const int StatusCode = 401;
            public const string ErrorCode = "TOKNREQD";
            public const string ErrorMsg = "Token Required";
            public const string DetailErrorMsg = "Token Required";
        }
        public struct ErrorListTooManyRequest
        {
            public const int StatusCode = 429;
            public const string ErrorCode = "TOMNYREQ";
            public const string ErrorMsg = "Too Many Requests";
            public const string DetailErrorMsg = "Too Many Requests";
        }
        public struct ErrorListInvalidAPIKey
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "INKEY";
            public const string ErrorMsg = "Invalid API Key";
            public const string DetailErrorMsg = "Invalid API Key";
        }
        public struct ErrorListInvalidUser
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "INUSER";
            public const string ErrorMsg = "Invalid User";
            public const string DetailErrorMsg = "Invalid User";
        }
        public struct ErrorListInvalidReq
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "INREQ";
            public const string ErrorMsg = "Invalid Request";
            public const string DetailErrorMsg = "Invalid Request";
        }
        public struct ErrorListDuringExecution
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "EXECERR";
            public const string ErrorMsg = "Error During Execution";
            public const string DetailErrorMsg = "Error During Execution";
        }
        public struct ErrorListCHR50
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL50";
            public const string ErrorMsg = "Exceed 50 Characters";
            public const string DetailErrorMsg = "Exceed 50 Characters";
        }
        public struct ErrorListCHR75
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL75";
            public const string ErrorMsg = "Exceed 75 Characters";
            public const string DetailErrorMsg = "Exceed 75 Characters";
        }
        public struct ErrorListCHR150
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL150";
            public const string ErrorMsg = "Exceed 150 Characters";
            public const string DetailErrorMsg = "Exceed 150 Characters";
        }
        public struct ErrorListCHR250
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL250";
            public const string ErrorMsg = "Exceed 250 Characters";
            public const string DetailErrorMsg = "Exceed 250 Characters";
        }
        public struct ErrorListCHR500
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL500";
            public const string ErrorMsg = "Exceed 500 Characters";
            public const string DetailErrorMsg = "Exceed 500 Characters";
        }
        public struct ErrorListCHR1000
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL1000";
            public const string ErrorMsg = "Exceed 1000 Characters";
            public const string DetailErrorMsg = "Exceed 1000 Characters";
        }
        public struct ErrorListDuplicateRequest
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "DUPREQ";
            public const string ErrorMsg = "Duplicate Request";
            public const string DetailErrorMsg = "Duplicate Request";
        }
        public struct ErrorListAPINotActive
        {
            public const int StatusCode = 500;
            public const string ErrorCode = "INAPI";
            public const string ErrorMsg = "API is Not Active";
            public const string DetailErrorMsg = "API is Not Active";
        }
        public struct ErrorListCHR5
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL5";
            public const string ErrorMsg = "Exceed 5 Characters";
            public const string DetailErrorMsg = "Exceed 5 Characters";
        }
        public struct ErrorListCHR10
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL10";
            public const string ErrorMsg = "Exceed 10 Characters";
            public const string DetailErrorMsg = "Exceed 10 Characters";
        }
        public struct ErrorListCHR30
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL30";
            public const string ErrorMsg = "Exceed 30 Characters";
            public const string DetailErrorMsg = "Exceed 30 Characters";
        }
        public struct ErrorListCHR20
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "CHRL20";
            public const string ErrorMsg = "Exceed 20 Characters";
            public const string DetailErrorMsg = "Exceed 20 Characters";
        }
        public struct ErrorListNoRecord
        {
            public const int StatusCode = 400;
            public const string ErrorCode = "NOREC";
            public const string ErrorMsg = "No Record Found";
            public const string DetailErrorMsg = "No Record Found";
        }
    }
}
