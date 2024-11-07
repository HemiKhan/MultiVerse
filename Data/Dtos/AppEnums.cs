namespace Data.Dtos
{
    public class AppEnum
    {
        public AppEnum() { }
        public enum CustomValidationType
        {
            Custom = 0,
            Date = 1,
            DateIgnoreEmpty = 2,
            Email = 3,
            EmailIgnoreEmpty = 4,
            Numeric = 5,
            NumericIgnoreEmpty = 6,
        }
        public struct JsonIgnorePropertyType
        {
            public const int Standard = 0;
            public const int None = 1;
            public const int HideProperty = 2;
        }
        public struct CaptchaVersion
        {
            public const string Version3 = "Version3";
            public const string Version2Robot = "Version2Robot";
            public const string Version2Invisible = "Version2Invisible";
        }
        public struct KendoGridFilterType
        {
            public const string contains = "contains";
            public const string notequal = "neq";
            public const string equal = "eq";
            public const string doesnotcontain = "doesnotcontain";
            public const string startswith = "startswith";
            public const string endswith = "endswith";
            public const string isnull = "isnull";
            public const string isnotnull = "isnotnull";
            public const string orderno = "orderno";
            public const string isempty = "isempty";
            public const string isnotempty = "isnotempty";
            public const string isequalorgreather = "gte";
            public const string greather = "gt";
            public const string isequalorless = "lte";
            public const string less = "lt";
            public const string isnullorempty = "isnullorempty";
            public const string isnotnullorempty = "isnotnullorempty";
            public const string inlistfilter = "inlistfilter";
            public const string notinlistfilter = "notinlistfilter";
        }
        public struct KendoGridFilterSRFieldType
        {
            public const string String = "string";
            public const string UpperString = "upstring";
            public const string LowerString = "lwstring";
            public const string Int = "int";
            public const string Float = "float";
            public const string Date = "date";
            public const string Datetime = "datetime";
            public const string Boolean = "boolean";
        }
        public struct KendoGridFilterFieldType
        {
            public const string String = "string";
            public const string Number = "number";
            public const string Date = "date";
            public const string Boolean = "boolean";
        }
        public struct ExcelColumnType
        {
            public const string General = "General";
            public const string Date_mmddyyyy = "mm/dd/yyyy";
            public const string Time_hhmmss = "hh:mm:ss";
            public const string DateTime_mmddyyyyhhmmss = "mm/dd/yyyy hh:mm:ss";
            public const string Accounting = "$ #,##0.00;[Red]($ #,##0.00)";
            public const string Text = "@";
            public const string Number = "0.00";
            public const string Percentage = "0.00%";
            public const string Scientific = "0.00E+00";
            public const string Currency = "$ #,##0.00";
            public const string Fraction = "# ?/?";
            //public const string Custom = "0.0\" \"m/s";
        }
        public struct MasterTypeID
        {
            public const int Order_Status = 100;
            public const int Order_Source = 101;
            public const int Order_Sub_Source = 102;
            public const int Billing_Type = 103;
            public const int Area_Type = 104;
            public const int Schedule_Type = 105;
            public const int User_Type = 106;
            public const int View_Source = 107;
            public const int Order_Audit_Type = 108;
            public const int Order_Audit_Source = 109;
            public const int Order_Comment_Importance_Level = 110;
            public const int Order_Document_Type = 111;
            public const int Order_Event_Source = 112;
            public const int Order_Item_Scan_Type = 113;
            public const int Order_Item_Device_Code = 114;
            public const int Order_Manifest_Type = 115;
            public const int Item_To_Ship = 117;
            public const int Item_Code = 118;
            public const int Packing_Code = 119;
            public const int Weight_Unit = 120;
            public const int Dimension_Unit = 121;
            public const int Item_Delivery_Status = 122;
            public const int Item_Pickup_Status = 123;
            public const int Order_Special_Instruction_Type = 124;
            public const int Service_Type = 125;
            public const int Error_Type = 126;
            public const int Error_Sub_Type = 127;
            public const int Attachment_Type = 128;
            public const int Event_Activity = 129;
            public const int Address_Type = 130;
            public const int Address_Status = 131;
            public const int Application_Name = 132;
            public const int Page_Rights_Type = 133;
            public const int Region = 134;
            public const int Location_Type = 135;
            public const int Manager_Title = 136;
            public const int Data_Type = 137;
            public const int Order_Priority = 138;
            public const int Assign_To_Department = 139;
            public const int OED_Status = 140;
            public const int CSR_Status = 141;
            public const int Dispatch_Status = 142;
            public const int Account_Status = 143;
            public const int Payment_Status = 144;
            public const int Invoice_Type = 145;
            public const int Order_Type = 146;
            public const int Get_Record_Type = 147;
            public const int Applications_Access = 148;
            public const int Block_Type = 149;
            public const int Security_Question = 150;
            public const int Warehouse_Status = 151;
            public const int Document_Type = 152;
            public const int Invoice_Status = 153;
            public const int Approval_Value = 154;
            public const int Need_EDI = 155;
            public const int EDI_Status = 156;
            public const int Business_Line_Dimension = 157;
            public const int Update_Warehouse_Dimension_Type = 158;
            public const int Location_Section = 159;
            public const int Order_Import_Field_Set_Type = 160;
            public const int Carriers = 161;
            public const int Label_Size = 162;
            public const int Order_Process_Status = 163;
            public const int Where_Clause_Type = 164;
            public const int Item_Damage_Type = 165;
            public const int General_Audit_Type = 166;
            public const int General_Source_Type = 167;
            public const int Google_API_Type = 168;
            public const int Import_File_Fields_Type = 169;
            public const int Import_Order_Page_Source = 170;
            public const int Import_Order_File_Source = 171;
            public const int Import_Order_File_Source_Setup_Type = 172;
            public const int Mobile_Apps = 173;
            public const int Config_Type = 174;
            public const int TMS_Status = 175;
            public const int TMS_Priority = 176;
            public const int TMS_Category = 177;
            public const int TMS_Document_Type = 178;
            public const int TMS_Attachment_Type = 179;
            public const int TMS_Assign_To_Type = 180;
            public const int Chat_Room_Type = 181;
            public const int Address_Status_Sub_Type = 182;

        }
        public struct OrderStatus
        {
            public const int Active = 100100;
            public const int Completed = 100101;
            public const int Closed = 100102;
            public const int Cancelled = 100103;
            public const int Archived = 100104;
        }
        public struct ImportOrderPageSource
        {
            public const string UploadOrderNew = "UPLOAD-ORDER-NEW";
            public const string UploadOrderOld = "UPLOAD-ORDER-OLD";
            public const string ASN = "ASN";
        }
        public struct ImportOrderFileSource
        {
            public const string CommenerceHub = "COMMERENCE-HUB";
            public const string ASN_EXCEL = "ASN-EXCEL";
            public const string ASN_CSV = "ASN-CSV";
            public const string ASN_XML = "ASN-XML";
            public const string API = "API";
            public const string ASN_EDI_850 = "ASN-EDI-850";
        }
        public struct CommerenceHubSeller
        {
            public const string SellerKey = "AF4C1BC7-4903-4196-BE9B-D251C0A14024";
        }
        public struct AutoUser
        {
            public const string PPAutoUser = "PPLUS";
        }
        public struct ImportOrderFileSourceSetupType
        {
            public const string Seller_Code = "SELLER-CODE";
            public const string BillTo_Code = "BILLTO-CODE";
            public const string SellTo_Partner = "SELLTO-PARTNER";
            public const string ShipFrom_AddressCode = "SHIPFROM-ADDRESSCODE";
            public const string ShipFrom_Email = "SHIPFROM-EMAIL";
            public const string ShipTo_Email = "SHIPTO-EMAIL";
            public const string Pickup_ServiceCode = "PICKUP-SERVICECODE";
            public const string Delivery_ServiceCode = "DELIVERY-SERVICECODE";
            public const string Item_Code = "ITEM-CODE";
            public const string Item_Dimension = "ITEM-DIMENSION";
            public const string Item_Dimension_Unit = "ITEM-DIMENSION-UNIT";
            public const string Item_Weight = "ITEM-WEIGHT";
            public const string Item_Weight_Unit = "ITEM-WEIGHT-UNIT";
            public const string Item_Value = "ITEM-VALUE";
            public const string Item_PackingCode = "ITEM-PACKINGCODE";
            public const string ItemToShip_Code = "ITEMTOSHIP-CODE";
            public const string Pickup_Driver_Instruction = "PICKUP-DRIVER-INST";
            public const string Delivery_Driver_Instruction = "DELIVERY-DRIVER-INST";
            public const string Order_Initial_Comment_Is_Public = "INITIAL-COM-PUBLIC";
            public const string Order_Initial_Comment = "ORDER-INITIAL-COM";
            public const string Order_Initial_Comment_Is_Public_2 = "INITIAL-COM-PUBLIC2";
            public const string Order_Initial_Comment_2 = "ORDER-INITIAL-COM2";
        }
        public struct AddressType
        {
            public const int ShipFrom = 130100;
            public const int ShipTo = 130101;
            public const int BillTo = 130102;
        }
        public struct SeviceLevelTypeID
        {
            public const int Pickup = 125100;
            public const int Delivery_ForwardShipment = 125101;
            public const int Delivery_ReverseShipment = 125102;
        }
        public struct OrderSource
        {
            public const int Web = 101100;
            public const int API = 101101;
            public const int ASN = 101102;
            public const int Mobile = 101103;
            public const int Guest = 101104;
            public const int Import_Client_User = 101105;
            public const int Import_Metro_User = 101106;
            public const int PPlus = 101107;
        }
        public struct GeneralSourceType
        {
            public const int Web = 167100;
        }
        public struct OrderSubSource
        {
            public const string Excel = "EXCEL";
            public const string CSV = "CSV";
            public const string EDI = "EDI";
            public const string EDI_850 = "EDI-850";
            public const string EDI_204 = "EDI-204";
            public const string JSON = "JSON";
            public const string XML = "XML";
            public const string Custom_Excel = "CUSTOM-EXCEL";
            public const string Custom_CSV = "CUSTOM-CSV";
            public const string Custom_XML = "CUSTOM-XML";
        }
        public struct ZipCodeType
        {
            public const int BillTo = 1;
            public const int ShipFrom = 2;
            public const int ShipTo = 3;
        }
        public struct WebTokenExpiredTime
        {
            public const int Seconds = (60 * 60);
            public const int Minutes = 60;
            public const int Hours = 1;
            public const int Days = 0;
        }
        public struct WebRememberMeTokenExpiredTime
        {
            public const int Seconds = 30 * 24 * 60 * 60;
            public const int Minutes = 30 * 24 * 60;
            public const int Hours = 30 * 24;
            public const int Days = 30;
        }
        public struct NewTokenExpiry
        {
            public const int Seconds = (24 * 60 * 60);
            public const int Minites = (24 * 60);
            public const int Hours = 24;
            public const int Days = 1;
        }
        public struct BillingType
        {
            public const string Prepaid = "110";
            public const string PayBeforeDelivery = "120";
            public const string PayAtTimeOfOrderReg = "130";
        }
        public struct AttachmentType
        {
            public const int OrderLevel = 128100;
            public const int ItemLevel = 128101;
            public const int ItemScanLevel = 128102;
        }
        public struct Units
        {
            public const double Grams = 1;
            public const double KiloGrams = 0.001;
            public const double Milligram = 1000;
            public const double Pounds = 0.00220462;
            public const double KGtoPound = 2.20462;
            public const double Ounces = 0.035274;
            public const double Tonnes = 0.000001;
            public const double DimensionInches = 1.00 / 1728.00;
            public const double DimensionCentiMeters = 0.00003531466247;
            // Add Remaining units / values
        }
        public struct APIType
        {
            public const int MultiVerseAPI = 10120;
        }
        public struct APISourceName
        {
            public const string GetLogAllFailedRequest = "GLAFR";
            public const string GetOnChallenge = "GOC";
            public const string GetValidateUserToken = "GVUT";
            public const string SignInAsync = "SIA";
            public const string RefreshToken = "RT";
            public const string ResetMemoryCache = "RMC";
            public const string GetDocumentTypeList = "GDTL";
            public const string GetPackingDetaillList = "GPDL";
            public const string GetItemToShipList = "GITSL";
            public const string GetItemTypeList = "GITL";
            public const string GetServiceLevelList = "GSLL";
            public const string GetOrderIdentifierFieldsList = "GOIFL";
            public const string GetClientSpecialServicesList = "GCSSL";
            public const string GetSellerClientList = "GSCL";
            public const string CreateOrderAPI = "CO-API";
            public const string LogOut = "LO";
            public const string GetOrderRegisterSettingDetail = "GORSD";
            public const string ValidateCreateOrderAPIReq = "VCOAR";
            public const string GetSellerClientListFullDetail = "GSCLFD";
            public const string OrderDetailPage = "ODP";
            public const string ShareTokenRequestSet = "STRS";
            public const string ShareTokenRequestGet = "STRG";
            public const string GetPriceKey = "GPK";
            public const string GetSavedAddressList = "GSAL";
            public const string GetPriceWP = "GPWP";
            public const string GetOrderImportFieldsNameAndSetting = "GOIFNAS";
            public const string CreateOrderAPIList = "CO-API2";
            public const string GetCreateOrderRequestList = "GCORL";
            public const string SaveOrderProcessRequestJson = "SOPRJ";
            public const string GetOrderProcessRequestJson = "GOPRJ";
            public const string GetPriceWPFromQuoteID = "GPWFQ";
            public const string GetSearchSkuList = "GSSL";
            public const string GoogleAddressValidation = "GAV";
            public const string ProcessOrderImportFile = "POIF";
            public const string ProcessOrderFromRequestIDs = "ProcessOrderFromRequestIDs";
            public const string GoogleAddressValidationWrapper = "GAVW";
            public const string OrderDetailPageIU = "ODPIU";
            public const string GetOrderTypeList = "GOTL";
            public const string GetSubOrderTypeList = "GSOTL";
            public const string ReportsPage = "RP";
            public const string SSOSignInAsync = "SSOSIA";
            public const string SSORefreshToken = "SSORT";
            public const string SSOLogOut = "SSOLO";
            public const string SSOIsTokenValid = "SSOITV";
        }
        public struct APIMethods
        {
            public const int SignInAsync = 60010;
            public const int RefreshToken = 60020;
            public const int ResetMemoryCache = 60030;
            public const int GetDocumentTypeList = 60040;
            public const int GetPackingDetaillList = 60050;
            public const int GetItemToShipList = 60060;
            public const int GetItemTypeList = 60070;
            public const int GetServiceLevelList = 60080;
            public const int GetOrderIdentifierFieldsList = 60090;
            public const int GetClientSpecialServicesList = 60100;
            public const int GetSellerClientList = 60110;
            public const int CreateOrderAPI = 60120;
            public const int LogOut = 60130;
            public const int GetOrderRegisterSettingDetail = 60140;
            public const int ValidateCreateOrderAPIReq = 60150;
            public const int GetSellerClientListFullDetail = 60160;
            public const int OrderDetailPage = 60170;
            public const int ShareTokenRequestSet = 60180;
            public const int ShareTokenRequestGet = 60190;
            public const int GetPriceKey = 60200;
            public const int GetSavedAddressList = 60210;
            public const int GetPriceWP = 60220;
            public const int GetOrderImportFieldsNameAndSetting = 60230;
            public const int CreateOrderAPIList = 60240;
            public const int GetCreateOrderRequestList = 60250;
            public const int SaveOrderProcessRequestJson = 60260;
            public const int GetOrderProcessRequestJson = 60270;
            public const int GetPriceWPFromQuoteID = 60280;
            public const int GetSearchSkuList = 60290;
            public const int GoogleAddressValidation = 60300;
            public const int ProcessOrderImportFile = 60310;
            public const int ProcessOrderFromRequestIDs = 60340;
            public const int GoogleAddressValidationWrapper = 60350;
            public const int OrderDetailPageIU = 60360;
            public const int GetOrderTypeList = 60370;
            public const int GetSubOrderTypeList = 60380;
            public const int ReportsPage = 60390;
            public const int SSOSignInAsync = 60400;
            public const int SSORefreshToken = 60410;
            public const int SSOLogOut = 60420;
            public const int SSOIsTokenValid = 60430;
        }
        public struct CacheType
        {
            public const string TConfig = "TConfig";
            public const string TAPIUserMapRequestLimit = "TAPIUserMapRequestLimit";
            public const string TAPIRemoteDomainIPRequestLimit = "TAPIRemoteDomainIPRequestLimit";
            public const string MetropolitanAdditionalApprovers = "MetropolitanAdditionalApprovers";
            public const string MetropolitanUserAPIMap = "MetropolitanUserAPIMap";
            public const string TApiRemoteDomainIPWhiteListing = "TApiRemoteDomainIPWhiteListing";
            public const string TUsers = "TUsers";
            public const string MetropolitanWebUserLogin = "MetropolitanWebUserLogin";
            public const string MetropolitanTimeZone = "MetropolitanTimeZone";
            public const string MetropolitanCustomer = "MetropolitanCustomer";
            public const string TMasterTypeValue = "TMasterTypeValue";
            public const string TServiceType = "TServiceType";
            public const string TSubServiceType = "TSubServiceType";
            public const string TClientServiceType = "TClientServiceType";
            public const string TServiceTypeDetail = "TServiceTypeDetail";
            public const string TOrderIdentifierFields = "TOrderIdentifierFields";
            public const string TOrderClientIdentifierFields = "TOrderClientIdentifierFields";
            public const string TServiceLevelSpecialService = "TServiceLevelSpecialService";
            public const string TSpecialServicesList = "TSpecialServicesList";
            public const string TClientServiceLevelSpecialService = "TClientServiceLevelSpecialService";
            public const string TIgnoreAPIErrorsList = "TIgnoreAPIErrorsList";
            public const string TClientIgnoreAPIErrorsList = "TClientIgnoreAPIErrorsList";
            public const string TSellerList = "TSellerList";
            public const string TSellerBillToMapping = "TSellerBillToMapping";
            public const string TSellerPartnerMapping = "TSellerPartnerMapping";
            public const string TSellerPartnerList = "TSellerPartnerList";
            public const string TSellerTariffMapping = "TSellerTariffMapping";
            public const string TTariffList = "TTariffList";
            public const string TSellerAllMappingPriceKey = "TSellerAllMappingPriceKey";
            public const string QuotesTPriceKey = "QuotesTPriceKey";
            public const string TGetRoleRightsFromRoleID = "TGetRoleRightsFromRoleID";
            public const string TGetRoleRightsFromUsername = "TGetRoleRightsFromUsername";
            public const string TIsHasRightFromRoleID = "TIsHasRightFromRoleID";
            public const string TIsHasRightFromUsername = "TIsHasRightFromUsername";
            public const string TGetUserOrderDetailGridList = "TGetUserOrderDetailGridList";
            public const string TGetPagesInfoByUser = "TGetPagesInfoByUser";
            public const string ReadOnlyServer = "ReadOnlyServer";
            public const string TGetQuoteAPIToken = "TGetQuoteAPIToken";
            public const string TGetOrderGUIDByOrderID = "TGetOrderGUIDByOrderID";
            public const string TGetOrderAccessByGUID = "TGetOrderAccessByGUID";
            public const string TGetArchiveDetailByOrderID = "TGetArchiveDetailByOrderID";
            public const string TGetOrderDetailAssignmentByGUID = "TGetOrderDetailAssignmentByGUID";
            public const string TGetOrderDetailBasicSummaryByGUID = "TGetOrderDetailBasicSummaryByGUID";
            public const string TGetOrderDetailClientIdentifierByGUID = "TGetOrderDetailClientIdentifierByGUID";
            public const string TGetOrderDetailSpecialServicesByGUID = "TGetOrderDetailSpecialServicesByGUID";
            public const string TGetOrderDetailAccessLogByGUID = "TGetOrderDetailAccessLogByGUID";
            public const string TGetOrderDetailItemDetailByGUID = "TGetOrderDetailItemDetailByGUID";
            public const string TGetOrderDetailInvoiceHeaderByGUID = "TGetOrderDetailInvoiceHeaderByGUID";
            public const string TGetOrderDetailInvoiceLineByGUID = "TGetOrderDetailInvoiceLineByGUID";
            public const string TGetOrderDetailCommentsByGUID = "TGetOrderDetailCommentsByGUID";
            public const string TGetOrderDetailEventsByGUID = "TGetOrderDetailEventsByGUID";
            public const string TGetOrderDetailDocumentsByGUID = "TGetOrderDetailDocumentsByGUID";
            public const string TGetOrderDetailScanHistoryByGUID = "TGetOrderDetailScanHistoryByGUID";
            public const string TGetOrderDetailManifestByGUID = "TGetOrderDetailManifestByGUID";
            public const string TGetOrderDetailPOPPODByGUID = "TGetOrderDetailPOPPODByGUID";
            public const string TGetOrderDetailChangeLogByGUID = "TGetOrderDetailChangeLogByGUID";
            public const string TGetSellerImportFieldsNameSetting = "TGetSellerImportFieldsNameSetting";
            public const string TGoogleMapsAPICall = "TGoogleMapsAPICall";
            public const string TGetSellerBillToFromPinnacleUser = "TGetSellerBillToFromPinnacleUser";
            public const string TGetShipmentTypeFromServiceLevel = "TGetShipmentTypeFromServiceLevel";
            public const string TGetSearchSkuList = "TGetSearchSkuList";
            public const string TGoogleAddressValidationAPICall = "TGoogleAddressValidationAPICall";
            public const string TGetGoogleAddressValidationJson = "TGetGoogleAddressValidationJson";
            public const string TGetSellerAddressList = "TGetSellerAddressList";
            public const string TGetAddressList = "TGetAddressList";
            public const string TGetImportOrderFileSourceSetup = "TGetImportOrderFileSourceSetup";
            public const string TSubOrderTypeValue = "TSubOrderTypeValue";
            public const string TGetReportingAPIToken = "TGetReportingAPIToken";
            public const string TGetUserPOMSAPIToken = "TGetUserPOMSAPIToken";
            public const string TGetAdminPOMSAPIToken = "TGetAdminPOMSAPIToken";
        }
        public struct CacheSubType
        {
            public const string P_Get_API_User_Map_Request_Limit = "P_Get_API_User_Map_Request_Limit";
            public const string P_Get_API_User_Map = "P_Get_API_User_Map";
            public const string P_Get_API_RemoteDomain_IP_Request_Limit = "P_Get_API_RemoteDomain_IP_Request_Limit";
            public const string P_Get_User_Info = "P_Get_User_Info";
            public const string P_Get_API_RemoteDomain_IP_WhiteListing = "P_Get_API_RemoteDomain_IP_WhiteListing";
            public const string P_Get_T_Config_Detail = "P_Get_T_Config_Detail";
            public const string P_Get_List_By_ID = "P_Get_List_By_ID";
            public const string P_Get_Client_Service_Type_List = "P_Get_Client_Service_Type_List";
            public const string P_Get_Order_Client_Identifier_Fields_List = "P_Get_Order_Client_Identifier_Fields_List";
            public const string P_Get_Client_Special_Service_List = "P_Get_Client_Special_Service_List";
            public const string P_Get_Client_Ignore_API_Errors_List = "P_Get_Client_Ignore_API_Errors_List";
            public const string P_Get_Seller_All_MappingList_ReturnJson = "P_Get_Seller_All_MappingList_ReturnJson";
            public const string P_Get_SellToClientList = "P_Get_SellToClientList";
            public const string P_GetDBServerForDataRead_2 = "P_GetDBServerForDataRead_2";
            public const string P_Get_Price_Key = "P_Get_Price_Key";
            public const string P_Get_Role_Rights_From_RoleID = "P_Get_Role_Rights_From_RoleID";
            public const string P_Get_Role_Rights_From_Username = "P_Get_Role_Rights_From_Username";
            public const string P_Is_Has_Right_From_RoleID = "P_Is_Has_Right_From_RoleID";
            public const string P_Is_Has_Right_From_Username = "P_Is_Has_Right_From_Username";
            public const string P_Get_User_Order_Detail_Grid_List = "P_Get_User_Order_Detail_Grid_List";
            public const string P_Get_Pages_Info_By_User = "P_Get_Pages_Info_By_User";
            public const string GetQuoteAPIToken = "GetQuoteAPIToken";
            public const string P_Get_Order_GUID_By_OrderID = "P_Get_Order_GUID_By_OrderID";
            public const string P_Get_OrderAccess_By_GUID = "P_Get_OrderAccess_By_GUID";
            public const string P_Get_ArchiveDetail_By_OrderID = "P_Get_ArchiveDetail_By_OrderID";
            public const string P_Get_OrderDetail_Assignment_By_GUID = "P_Get_OrderDetail_Assignment_By_GUID";
            public const string P_Get_OrderDetail_BasicSummary_By_GUID = "P_Get_OrderDetail_BasicSummary_By_GUID";
            public const string P_Get_OrderDetail_ClientIdentifier_By_GUID = "P_Get_OrderDetail_ClientIdentifier_By_GUID";
            public const string P_Get_OrderDetail_SpecialServices_By_GUID = "P_Get_OrderDetail_SpecialServices_By_GUID";
            public const string P_Get_OrderDetail_AccessLog_By_GUID = "P_Get_OrderDetail_AccessLog_By_GUID";
            public const string P_Get_OrderDetail_ItemDetail_By_GUID = "P_Get_OrderDetail_ItemDetail_By_GUID";
            public const string P_Get_OrderDetail_InvoiceHeader_By_GUID = "P_Get_OrderDetail_InvoiceHeader_By_GUID";
            public const string P_Get_OrderDetail_InvoiceLine_By_GUID = "P_Get_OrderDetail_InvoiceLine_By_GUID";
            public const string P_Get_OrderDetail_Comments_By_GUID = "P_Get_OrderDetail_Comments_By_GUID";
            public const string P_Get_OrderDetail_Events_By_GUID = "P_Get_OrderDetail_Events_By_GUID";
            public const string P_Get_OrderDetail_Documents_By_GUID = "P_Get_OrderDetail_Documents_By_GUID";
            public const string P_Get_OrderDetail_ScanHistory_By_GUID = "P_Get_OrderDetail_ScanHistory_By_GUID";
            public const string P_Get_OrderDetail_Manifest_By_GUID = "P_Get_OrderDetail_Manifest_By_GUID";
            public const string P_Get_OrderDetail_POPPOD_By_GUID = "P_Get_OrderDetail_POPPOD_By_GUID";
            public const string P_Get_OrderDetail_ChangeLog_By_GUID = "P_Get_OrderDetail_ChangeLog_By_GUID";
            public const string P_Get_Seller_Import_Fields_Name_Setting = "P_Get_Seller_Import_Fields_Name_Setting";
            public const string GoogleMapsAPICall = "GoogleMapsAPICall";
            public const string P_Get_SellerBillTo_From_Pinnacle_User = "P_Get_SellerBillTo_From_Pinnacle_User";
            public const string P_Get_ShipmentType_From_ServiceLevel = "P_Get_ShipmentType_From_ServiceLevel";
            public const string P_Get_Search_Sku_List = "P_Get_Search_Sku_List";
            public const string GoogleAddressValidationAPICall = "GoogleAddressValidationAPICall";
            public const string P_Get_Google_Address_Validation_Json = "P_Get_Google_Address_Validation_Json";
            public const string P_Get_SellerAddressList = "P_Get_SellerAddressList";
            public const string P_Get_AddressList = "P_Get_AddressList";
            public const string P_Get_Import_Order_File_Source_Setup = "P_Get_Import_Order_File_Source_Setup";
            public const string P_Get_List_By_ID_2 = "P_Get_List_By_ID_2";
            public const string P_Get_Sub_OrderType_List = "P_Get_Sub_OrderType_List";
            public const string GetReportingAPIToken = "GetReportingAPIToken";
            public const string GetUserPOMSAPIToken = "GetUserPOMSAPIToken";
            public const string GetAdminPOMSAPIToken = "GetAdminPOMSAPIToken";
        }
        public struct Database_Name
        {
            public const string MultiVerseDB = "MultiVerseDB"; 
        }
        public struct ApplicationId
        {
            public const int CareerPortalAppID = 148116;
            public const int AppID = 148104;
        }
        public struct ApplicationName
        {
            public const string AppName = "CodeToCure";
        }
        public struct Granularity
        {
            public const string GRANULARITY_UNSPECIFIED = "GRANULARITY_UNSPECIFIED";
            public const string SUB_PREMISE = "SUB_PREMISE";
            public const string PREMISE = "PREMISE";
            public const string PREMISE_PROXIMITY = "PREMISE_PROXIMITY";
            public const string BLOCK = "BLOCK";
            public const string ROUTE = "ROUTE";
            public const string OTHER = "OTHER";
        }
        public struct ComponentType
        {
            public const string administrative_area_level_1 = "administrative_area_level_1";
            public const string administrative_area_level_2 = "administrative_area_level_2";
            public const string administrative_area_level_3 = "administrative_area_level_3";
            public const string administrative_area_level_4 = "administrative_area_level_4";
            public const string administrative_area_level_5 = "administrative_area_level_5";
            public const string administrative_area_level_6 = "administrative_area_level_6";
            public const string administrative_area_level_7 = "administrative_area_level_7";
            public const string archipelago = "archipelago";
            public const string colloquial_area = "colloquial_area";
            public const string continent = "continent";
            public const string country = "country";
            public const string establishment = "establishment";
            public const string finance = "finance";
            public const string floor = "floor";
            public const string food = "food";
            public const string general_contractor = "general_contractor";
            public const string geocode = "geocode";
            public const string health = "health";
            public const string intersection = "intersection";
            public const string landmark = "landmark";
            public const string locality = "locality";
            public const string natural_feature = "natural_feature";
            public const string neighborhood = "neighborhood";
            public const string place_of_worship = "place_of_worship";
            public const string plus_code = "plus_code";
            public const string point_of_interest = "point_of_interest";
            public const string political = "political";
            public const string post_box = "post_box";
            public const string postal_code = "postal_code";
            public const string postal_code_prefix = "postal_code_prefix";
            public const string postal_code_suffix = "postal_code_suffix";
            public const string postal_town = "postal_town";
            public const string premise = "premise";
            public const string room = "room";
            public const string route = "route";
            public const string street_address = "street_address";
            public const string street_number = "street_number";
            public const string sublocality = "sublocality";
            public const string sublocality_level_1 = "sublocality_level_1";
            public const string sublocality_level_2 = "sublocality_level_2";
            public const string sublocality_level_3 = "sublocality_level_3";
            public const string sublocality_level_4 = "sublocality_level_4";
            public const string sublocality_level_5 = "sublocality_level_5";
            public const string subpremise = "subpremise";
            public const string town_square = "town_square";
        }
        public struct ConfirmationLevel
        {
            public const string CONFIRMATION_LEVEL_UNSPECIFIED = "GRANULARITY_UNSPECIFIED";
            public const string CONFIRMED = "SUB_PREMISE";
            public const string UNCONFIRMED_BUT_PLAUSIBLE = "PREMISE";
            public const string UNCONFIRMED_AND_SUSPICIOUS = "PREMISE_PROXIMITY";
        }
    }
}
