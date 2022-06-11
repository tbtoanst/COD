using CertificatesOfDeposit.Models.Buy;
using Newtonsoft.Json;
using System;

namespace CertificatesOfDeposit.Models.Sell
{
    public class ReportModel
    {
        [JsonProperty("sell_info")]
        public SellModel SellInfo { get; set; }
        [JsonProperty("buy_info")]
        public BuyModel BuyInfo { get; set; }
        [JsonProperty("cus_info")]
        public CustomerModel CustomerInfo { get; set; }
        [JsonProperty("data_info")]
        public AccountTranFccModel DataInfo { get; set; }

        [JsonProperty("other_info")]
        public OtherModel OtherInfo { get; set; }
        
    }

    public class CustomerModel
    {
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("id_no")]
        public string IdNo { get; set; }
        [JsonProperty("id_date")]
        public string IdDate { get; set; }
        [JsonProperty("id_location")]
        public string IdLocation { get; set; }
        [JsonProperty("address_thuong_tru")]
        public string AddressThuongTru { get; set; }
        [JsonProperty("address_tam_tru")]
        public string AddressTamTru { get; set; }
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("full_name_tc")]
        public string FullNameTC { get; set; }
        [JsonProperty("id_no_tc")]
        public string IdNoTC { get; set; }
        [JsonProperty("id_date_tc")]
        public string IdDateTC { get; set; }
        [JsonProperty("id_location_tc")]
        public string IdLocationTC { get; set; }
        [JsonProperty("address_thuong_tru_tc")]
        public string AddressThuongTruTC { get; set; }
        [JsonProperty("address_tam_tru_tc")]
        public string AddressTamTruTC { get; set ; }
        [JsonProperty("phone_number_tc")]
        public string PhoneNumberTC { get; set; }
        [JsonProperty("email_tc")]
        public string EmailTC { get; set; }
        [JsonProperty("fax_tc")]
        public string FaxTC { get; set; }
    }

    public class OtherModel
    {
        [JsonProperty("so_du_vn")]
        public string SoDuVn { get; set; }

        [JsonProperty("so_tien_cn")]
        public string SoTienCn { get; set; }

        [JsonProperty("so_tien_thue_phi")]
        public string SoTienThuePhi { get; set; }

        [JsonProperty("so_tien_cn_txt")]
        public string SoTienCnTxt { get; set; }

        [JsonProperty("hinh_thuc_tra_lai")]
        public string HinhThucTraLai { get; set; }

        [JsonProperty("gia_tri_giao_dich")]
        public string GiaTriGiaoDich { get; set; }

        [JsonProperty("ngay_mo")]
        public string NgayMo { get; set; }

        [JsonProperty("ngay_tai_ky")]
        public string NgayTaiKy { get; set; }

        [JsonProperty("lai_suat")]
        public string LaiSuat { get; set; }

        [JsonProperty("ngay_xuat_bao_cao")]
        public DateTime NgayXuatBc { get; set; } = DateTime.Now;
        [JsonProperty("branch_name")]
        public string BranchName { get; set; }
        [JsonProperty("nguoi_dai_dien")]
        public string NguoiDaiDien { get; set; }

        [JsonProperty("branch_name_acc")]
        public string BranchNameAcc { get; set; }

        [JsonProperty("branch_name_acc_out")]
        public string BranchNameAccOut { get; set; }

    }
}
