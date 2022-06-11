using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.SOA
{
    public class CifInfoModel
    {
        [JsonProperty("CIFNum")]
        public string CIFNum { get; set; }
        [JsonProperty("CIFIssuedDate")]
        public string CIFIssuedDate { get; set; }
        [JsonProperty("branchCode")]
        public string BranchCode { get; set; }
        [JsonProperty("customerType")]
        public string CustomerType { get; set; }
    }

    public class CustomerInfoModel
    {
        [JsonProperty("fullname")]
        public string Fullname { get; set; }
        [JsonProperty("fullname_vn")]
        public string Fullname_vn { get; set; }
        [JsonProperty("birthDay")]
        public string BirthDay { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("customerVIPType")]
        public string CustomerVIPType { get; set; }
        [JsonProperty("manageStaffID")]
        public string ManageStaffID { get; set; }
        [JsonProperty("recordStatus")]
        public string RecordStatus { get; set; }
        [JsonProperty("authStatus")]
        public string AuthStatus { get; set; }
        [JsonProperty("usIndicia")]
        public string UsIndicia { get; set; }
        [JsonProperty("directorName")]
        public string DirectorName { get; set; }
        [JsonProperty("nationlityCode")]
        public string NationlityCode { get; set; }
        [JsonProperty("nationlity")]
        public string Nationlity { get; set; }
        [JsonProperty("emailList")]
        public string EmailList { get; set; }
        [JsonProperty("feeDebt")]
        public string FeeDebt { get; set; }
        [JsonProperty("isStaff")]
        public string IsStaff { get; set; }
        [JsonProperty("segmentType")]
        public string SegmentType { get; set; }
        [JsonProperty("jobInfo")]
        public JobInfoModel jobInfo { get; set; }
        [JsonProperty("IDInfo")]
        public IdInfoModel IDInfo { get; set; }
        [JsonProperty("address")]
        public AddressModel Address { get; set; }
    }

    public class JobInfoModel
    {
        [JsonProperty("ProfessionalName")]
        public string ProfessionalName { get; set; }
        [JsonProperty("ProfessionalCode")]
        public string ProfessionalCode { get; set; }
    }

    public class IdInfoModel
    {
        [JsonProperty("IDNum")]
        public string IDNum { get; set; }
        [JsonProperty("IDIssuedDate")]
        public string IDIssuedDate { get; set; }
        [JsonProperty("IDIssuedLocation")]
        public string IDIssuedLocation { get; set; }
    }

    public class AddressModel
    {
        [JsonProperty("address1")]
        public string Address1 { get; set; }
        [JsonProperty("address_vn")]
        public string Address_vn { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("mobileNum")]
        public string MobileNum { get; set; }
        [JsonProperty("telephoneNum")]
        public string TelephoneNum { get; set; }
        [JsonProperty("phoneNum")]
        public string PhoneNum { get; set; }
        [JsonProperty("faxNum")]
        public string FaxNum { get; set; }
    }

    public class CustomerlistModel
    {
        [JsonProperty("CIFInfo")]
        public CifInfoModel CIFInfo { get; set; }
        [JsonProperty("customerInfo")]
        public CustomerInfoModel CustomerInfo { get; set; }
    }

    public class AccountInfoModel
    {
        [JsonProperty("accountNum")]
        public string AccountNum { get; set; }
        [JsonProperty("accountCurrency")]
        public string AccountCurrency { get; set; }
        [JsonProperty("accountNarrative")]
        public string AccountNarrative { get; set; }
        [JsonProperty("accountClassCode")]
        public string AccountClassCode { get; set; }
        [JsonProperty("accountInterestRate")]
        public string AccountInterestRate { get; set; }
        [JsonProperty("accountType")]
        public string AccountType { get; set; }
        [JsonProperty("accountBalance")]
        public decimal AccountBalance { get; set; }

        //
        public CifInfoModel CIFInfo { get; set; }
        public CustomerInfoModel customerInfo { get; set; }
        [JsonProperty("accountName")]
        public string AccountName { get; set; }
        [JsonProperty("accountTypeName")]
        public string AccountTypeName { get; set; }
        [JsonProperty("accountBalanceAvailable")]
        public string AccountBalanceAvailable { get; set; }
        [JsonProperty("accountExchangeBalanceEQV")]
        public string AccountExchangeBalanceEQV { get; set; }
        [JsonProperty("accountOpenDate")]
        public string AccountOpenDate { get; set; }
        [JsonProperty("accountStatus")]
        public string AccountStatus { get; set; }
        [JsonProperty("accountClassName")]
        public string AccountClassName { get; set; }
        [JsonProperty("accountOpenBrandCode")]
        public string AccountOpenBrandCode { get; set; }
        [JsonProperty("accountLatestTransDate")]
        public string AccountLatestTransDate { get; set; }
        [JsonProperty("accountOverdraftDate")]
        public string AccountOverdraftDate { get; set; }
        [JsonProperty("accountOverdraftExpiredDate")]
        public string AccountOverdraftExpiredDate { get; set; }
        [JsonProperty("accountOverdraftLimit")]
        public string AccountOverdraftLimit { get; set; }
        [JsonProperty("accountAuthorizedStatus")]
        public string AccountAuthorizedStatus { get; set; }
        [JsonProperty("accountDelegatedPerson")]
        public string AccountDelegatedPerson { get; set; }
        [JsonProperty("accountCoownerName")]
        public string AccountCoownerName { get; set; }
        [JsonProperty("accountLockStatus")]
        public string AccountLockStatus { get; set; }

        /// <summary>
        /// Trạng thái hoạt động của tài khoản trên core
        /// </summary>
        [JsonProperty("accountStatusActive")]
        public string AccountStatusActive { get; set; }
    }
}
