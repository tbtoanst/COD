using CertificatesOfDeposit.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Core.Entities
{
    public class AccountTranFcc
    {
		[Column("ma_cif")]
		public string MaCif { get; set; }
		[Column("ho_ten")]
		public string HoTen { get; set; }
		[Column("cmnd")]
		public string cmnd { get; set; }
		[Column("stk")]
		public string Stk { get; set; }
		[Column("loai_tien")]
		public string LoaiTien { get; set; }
		[Column("loai_hinh")]
		public string LoaiHinh { get; set; }
		[Column("lai_suat")]
		public decimal LaiSuat { get; set; }
		[Column("hinh_thuc_linh_lai")]
		public string HinhThucLinhLai { get; set; }
		[Column("ngay_mo")]
		public DateTime NgayMo { get; set; }
		[Column("ngay_tai_ky")]
		public DateTime NgayTaiKy { get; set; }
		[Column("ngay_cn_gan_nhat")]
		public DateTime NgayCnGanNhat { get; set; }
		[Column("so_ngay_nam_giu")]
		public decimal SoNgayNamGiu { get; set; }
		[Column("giatri_giao_dich")]
		public decimal GiaTriGiaoDich { get; set; }
		[Column("phi_chuyen_nhuong")]
		public decimal PhiChuyenNhuong { get; set; }
		[Column("so_tien_kh_nhan_cn")]
		public decimal SoTienKhNhanCn { get; set; }
		[Column("so_tien_kh_tt_nhan_cn")]
		public decimal SoTienKHTTNhanCN { get; set; }
		[Column("trang_thai")]
		public string TrangThai { get; set; }
		[Column("so_seri")]
		public string SoSeri { get; set; }
		[Column("so_du")]
		public decimal SoDu { get; set; }

		[Column("so_ngay_con_lai")]
		public int SoNgayConLai { get; set; }

		[Column("ngaytl_gannhat")]
		public DateTime NgayTLGanNhat { get; set; }

		[Column("lai_duoc_huong")]
		public decimal LaiDuocHuong { get; set; }

		[Column("don_vi_mo")]
		public string DonViMo { get; set; }

		[Column("so_tien_thu_phi")]
		public decimal SoTienThuPhi { get; set; }

		[Column("phan_tram_phi_chuyen_nhuong")]
		public decimal PhanTramPhiChuyenNhuong { get; set; }

		[Column("nguoi_dai_dien")]
		public string NguoiDaiDien { get; set; }
		[Column("loai_kh")]
		public string LoaiKh  { get; set; }

		[Column("hinhthuc_daohan")]
		public string HinhThucDaoHan { get; set; }
	}
}
