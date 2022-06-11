using CertificatesOfDeposit.Models.Sell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CertificatesOfDeposit.Models.Buy
{
    public class AccountTranFccModel
	{
		[JsonProperty("ma_cif")]
		public string MaCif { get; set; }
		[JsonProperty("ho_ten")]
		public string HoTen { get; set; }
		[JsonProperty("cmnd")]
		public string cmnd { get; set; }
		[JsonProperty("stk")]
		public string Stk { get; set; }
		[JsonProperty("loai_tien")]
		public string LoaiTien { get; set; }
		[JsonProperty("loai_hinh")]
		public string LoaiHinh { get; set; }
		[JsonProperty("lai_suat")]
		public decimal LaiSuat { get; set; }
		[JsonProperty("hinh_thuc_linh_lai")]
		public string HinhThucLinhLai { get; set; }
		[JsonProperty("ngay_mo")]
		public DateTime NgayMo { get; set; }
		[JsonProperty("ngay_tai_ky")]
		public DateTime NgayTaiKy { get; set; }
		[JsonProperty("ngay_cn_gan_nhat")]
		public DateTime NgayCnGanNhat { get; set; }
		[JsonProperty("so_ngay_nam_giu")]
		public decimal SoNgayNamGiu { get; set; }
		[JsonProperty("giatri_giao_dich")]
		public decimal GiaTriGiaoDich { get; set; }
		[JsonProperty("phi_chuyen_nhuong")]
		public decimal PhiChuyenNhuong { get; set; }
		[JsonProperty("so_tien_kh_nhan_cn")]
		public decimal SoTienKhNhanCn { get; set; }
		[JsonProperty("so_tien_kh_tt_nhan_cn")]
		public decimal SoTienKHTTNhanCN { get; set; }
		[JsonProperty("trang_thai")]
		public string TrangThai { get; set; }
		[JsonProperty("so_seri")]
		public string SoSeri { get; set; }
		[JsonProperty("so_du")]
		public decimal SoDu { get; set; }

		[JsonProperty("so_ngay_con_lai")]
		public int SoNgayConLai { get; set; }

		[JsonProperty("ngaytl_gannhat")]
		public DateTime NgayTLGanNhat { get; set; }

		[JsonProperty("lai_duoc_huong")]
		public decimal LaiDuocHuong { get; set; }

		[JsonProperty("don_vi_mo")]
		public string DonViMo { get; set; }

		[JsonProperty("so_tien_thu_phi")]
		public decimal SoTienThuPhi { get; set; }

		[JsonProperty("phan_tram_phi_chuyen_nhuong")]
		public decimal PhanTramPhiChuyenNhuong { get; set; }

		[JsonProperty("ds_dong_chu_so_huu")]
		public List<JointHolderModel>  DSDongChuSoHuu { get; set; }
		[JsonProperty("nguoi_dai_dien")]
		public string NguoiDaiDien { get; set; }
		[JsonProperty("loai_kh")]
		public string LoaiKh { get; set; }

		[JsonProperty("hinhthuc_daohan")]
		public string HinhThucDaoHan { get; set; }
	}
}
