using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CertificatesOfDeposit.Models.Globally
{
    public class PagedResultModel<T>
    {
        [JsonProperty("result")]
        public IEnumerable<T> Items { get; set; }
        [JsonProperty("count")]
        public int TotalCount { get; set; }

        [JsonProperty("total_status_buy_cho_hach_toan")]
        public int TotalStatusBuyChoHanhToan { get; set; }

        [JsonProperty("total_status_buy_cho_duyet_lenh_cn")]
        public int TotalStatusBuyhoDuyenLenhCN { get; set; }

        [JsonProperty("total_status_buy_chuyen_nhuong_thanh_cong")]
        public int TotalStatusBuyChuyenNhuongThanhCong { get; set; }

        [JsonProperty("total_status_buy_cho_duyet_lenh_ncn")]
        public int TotalStatusBuyChoDuyetLenhNCN { get; set; }

        [JsonProperty("total_status_buy_tu_choi")]
        public int TotalStatusBuyTuChoi { get; set; }

        [JsonProperty("total_status_buy_xoa")]
        public int TotalStatusBuyXoa { get; set; }

        [JsonProperty("total_status_sell_cho_hach_toan")]
        public int TotalStatusSellChoHanhToan { get; set; }

        [JsonProperty("total_status_sell_cho_duyet_lenh_cn")]
        public int TotalStatusSellChoDuyetLenhCN { get; set; }

        [JsonProperty("total_status_sell_cho_nhan_chuyen_nhuong")]
        public int TotalStatusSellChoNhanChuyenNhuong { get; set; }

        [JsonProperty("total_status_sell_cho_duyet_lenh_ncn")]
        public int TotalStatusSellChoDuyetLenhNCN { get; set; }

        [JsonProperty("total_status_sell_tu_choi")]
        public int TotalStatusSellTuChoi { get; set; }

        [JsonProperty("total_status_sell_xoa")]
        public int TotalStatusSellXoa { get; set; }
    }
}
