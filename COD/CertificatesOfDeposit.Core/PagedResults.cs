using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CertificatesOfDeposit
{
    public class PagedResults<T>
    {
        public IReadOnlyList<T>? Items { get; set; }
        public int TotalCount { get; set; }

        public int TotalStatusSellChoHanhToan { get; set; }
        public int TotalStatusSellChoDuyetLenhCN { get; set; }
        public int TotalStatusSellChoNhanChuyenNhuong { get; set; }
        public int TotalStatusSellChoDuyenLenhNCN { get; set; }
        public int TotalStatusSellTuChoi { get; set; }
        public int TotalStatusSellXoa { get; set; }

        public int TotalStatusBuyChoHanhToan { get; set; }
        public int TotalStatusBuyChoDuyetLenhNCN { get; set; }
        public int TotalStatusBuyChuyenNhuongThanhCong { get; set; }
        public int TotalStatusBuyTuChoi { get; set; }
        public int TotalStatusBuyXoa { get; set; }
    }
}
