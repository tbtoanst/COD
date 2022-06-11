export const PAYMENT_METHOD_LIST: any[] = [
    { code: 'CK', name: 'Tài khoản thanh toán' },
    { code: 'TM', name: 'Tiền mặt' }
]

export const ADVANCE_STATUS_LIST: any[] = [
    { code: '', name: '--- Tất cả ---' },
    { code: 'tam_ung_qua_ngay', name: 'Tạm ứng qua ngày' },
    { code: 'tam_ung_trong_ngay', name: 'Tạm ứng trong ngày' }
]

export const TKTK_CCTG_STATUS_LIST: any[] = [
    { code: 'HOAT_DONG', name: 'Hoạt động', color: "badge badge-success" },
    { code: 'DANG_CAM_CO', name: 'Tạm khóa', color: "badge badge-warning" },
    { code: 'DANG_PHONG TOA', name: 'Phong tỏa', color: "badge badge-danger" }
]

export const TRANSFER_SELL_STATUS_LIST: any[] = [
    { code: '', name: '--- Tất cả ---' },
    { code: 'CHO_GHI_NHAN_BAN', name: 'Chờ ghi nhận bán', color: "badge badge-primary" },
    { code: 'CHO_MUA_BAN', name: 'Chờ mua', color: "badge badge-info" },
    { code: 'CHO_DUYET_TT_BAN', name: 'Chờ duyệt thông tin bán', color: "badge badge-success" },
    { code: 'XOA_LENH_BAN', name: 'Đã xóa lệnh bán', color: "badge badge-secondary" },
    { code: 'CHO_CQSH_BAN', name: 'Chờ chuyển quyền sở hữu', color: "badge badge-pending" },
    { code: 'CHO_KHOP_LENH_BAN', name: 'Chờ khớp lệnh bán', color: "badge badge-pending" },
    { code: 'CHO_DUYET_CHI_TIEN_BAN', name: 'Chờ duyệt chi tiền bán', color: "badge badge-pending" },
    { code: 'TAM_GIU_BAN', name: 'Tạm giữ bán', color: "badge badge-pending" },
    { code: 'KHOP_LENH_BAN', name: 'Khớp lệnh bán', color: "badge badge-warning" },
]

export const TRANSFER_SELL_STATUS_CODE: any = {
    // CHO_GHI_NHAN_BAN: "A",
    // CHO_DUYET_THONG_TIN: "I",
    // CHO_MUA: "C",
    // TU_CHOI: "R",
    // DA_XOA: "D",

    CHO_GHI_NHAN_BAN: "CHO_GHI_NHAN_BAN",
    CHO_MUA_BAN: "CHO_MUA_BAN",
    CHO_DUYET_TT_BAN: "CHO_DUYET_TT_BAN",
    XOA_LENH_BAN: "XOA_LENH_BAN",
    CHO_CQSH_BAN: "CHO_CQSH_BAN",
    CHO_KHOP_LENH_BAN: "CHO_KHOP_LENH_BAN",
    CHO_DUYET_CHI_TIEN_BAN: "CHO_DUYET_CHI_TIEN_BAN",
    TAM_GIU_BAN: "TAM_GIU_BAN",
    KHOP_LENH_BAN: "KHOP_LENH_BAN",
}

export const TRANSFER_BUY_STATUS_LIST: any[] = [
    { code: '', name: '--- Tất cả ---' },
    { code: 'CHO_CQSH_MUA', name: 'Chờ chuyển quyền sở hữu', color: "badge badge-primary" },
    { code: 'XOA_LENH_MUA', name: 'Đã xóa lệnh mua', color: "badge badge-info" },
    { code: 'CHO_KHOP_LENH_MUA', name: 'Chờ khớp lệnh mua', color: "badge badge-pending" },
    { code: 'TAM_GIU_MUA', name: 'Tạm giữ', color: "badge badge-warning" },
    { code: 'CHO_DUYET_LENH_MUA', name: 'Chờ duyệt lệnh mua', color: "badge badge-secondary" },
    { code: 'CHO_DUYET_CQSH_MUA', name: 'Chờ duyệt CQSH mua', color: "badge badge-secondary" },
    { code: 'KHOP_LENH_MUA', name: 'Khớp lệnh mua', color: "badge badge-warning" },
]

export const TRANSFER_BUY_STATUS_CODE: any = {
    // TAM_GIU: "A",
    // CHO_DUYET_LENH_MUA: "S",
    // CHO_KHOP_LENH: "C",
    // TU_CHOI: "R",
    // DA_XOA: "D"

    CHO_CQSH_MUA: "CHO_CQSH_MUA",
    XOA_LENH_MUA: "XOA_LENH_MUA",
    CHO_KHOP_LENH_MUA: "CHO_KHOP_LENH_MUA",
    TAM_GIU_MUA: "TAM_GIU_MUA",
    CHO_DUYET_LENH_MUA: "CHO_DUYET_LENH_MUA",
    CHO_DUYET_CQSH_MUA: "CHO_DUYET_CQSH_MUA",
    KHOP_LENH_MUA: "KHOP_LENH_MUA",
}

export const TRANSFER_TYPE_LIST: any[] = [
    { code: '', name: '--- Tất cả ---' },
    { code: 'sell', name: 'Chuyển nhượng' },
    { code: 'buy', name: 'Nhận chuyển nhượng' }
]

export const CONTRACT_STATUS_LIST: any[] = [
    { code: '', name: '--- Tất cả ---' },
    { code: 'CHUYEN_NHUONG_CUNG_DON_VI', name: 'Chuyển nhượng cùng đơn vị' },
    { code: 'DON_VI_CHUYEN_NHUONG_CHUA_UPLOAD', name: 'Đơn vị chuyển nhượng chưa upload' },
    { code: 'DON_VI_NHAN_CHUYEN_NHUONG_CHUA_UPLOAD', name: 'Đơn vị nhận chuyển nhượng chưa upload' },
    { code: 'DON_VI_NHAN_CHUYEN_NHUONG_CHUA_DUYET', name: 'Đơn vị nhận chuyển nhượng chưa duyệt' },
    { code: 'DON_VI_NHAN_CHUYEN_NHUONG_CHO_DUYET', name: 'Đơn vị nhận chuyển nhượng chờ duyệt' },
    { code: 'DA_DUYET', name: 'Đã duyệt' },
    { code: 'HOAN_DUYET', name: 'Hoàn duyệt' },
    { code: 'LIEN_HE_HO', name: 'Liên hệ HO' }
]

export const PAYOUT_EXPIRE_TYPE_CODE: any[] = [
    { code: 'TAI KY GOC', name: 'Tái ký gốc' },
    { code: 'TAI KY LAI', name: 'Tái ký lãi' },
    { code: 'TAI KY GOC LAI', name: 'Tái ký toàn bộ gốc và lãi' },
    { code: 'KHONG TAI KY', name: 'Không tái ký' }
]

export const UPLOAD_CONTRACT_STATUS_CODE: any = {
    CHO_DUYET: 'CHO_DUYET',
    DA_DUYET: 'DA_DUYET'
}

export const TYPE_PAYMENT_STATUS_LIST: any = [
    { name: '', key: 'C', code: 'CUOI KY' },
    { name: '', key: 'T', code: 'TRA TRUOC' },
    { name: '', key: 'D', code: 'DINH KY' }
]

export const TYPE_PAYMENT_STATUS_CODE: any = {
    CUOI_KY: 'C',
    TRA_TRUOC: 'T',
    DINH_KY: 'D'
}




