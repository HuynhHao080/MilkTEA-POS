# 🧋 MilkTeaPOS - Hệ thống quản lý quán Trà sữa

## 📋 Mô tả dự án
Hệ thống POS (Point of Sale) quản lý quán trà sữa với các chức năng: quản lý danh mục, sản phẩm, order, thanh toán, membership, và voucher.

---

## 🚀 Cài đặt nhanh

### Yêu cầu hệ thống
- **.NET 10.0** trở lên
- **PostgreSQL** database
- **Visual Studio 2022** hoặc **VS Code**

### Các bước cài đặt

```bash
# 1. Clone repository
git clone <repository-url>
cd MilkTEA-POS

# 2. Restore NuGet packages
dotnet restore

# 3. Cấu hình database
# Edit file db.config.txt với connection string của bạn

# 4. Tạo database
# Chạy file database.sql trong PostgreSQL

# 5. Build project
dotnet build

# 6. Chạy ứng dụng
dotnet run
```

---

## 📊 DANH SÁCH 18 FORMS

### Tổng quan

| Nhóm | Số Forms | Forms |
|------|----------|-------|
| Authentication | 2 | Login, ChangePassword |
| Main | 1 | MDI Container |
| Dashboard | 1 | Charts, KPIs |
| Products | 3 | Categories, Products, Toppings |
| Tables | 1 | Visual layout |
| POS & Orders | 3 | Core business |
| Customers | 2 | CRM + Membership |
| Vouchers | 1 | Promotion |
| Order History | 1 | Search, Filter |
| Reports | 2 | Sales + Audit |
| Users | 1 | Staff management |
| **TỔNG** | **18** | |

**Tiến độ:** 4/18 forms (22%) ✅

---

## ✅ FORMS ĐÃ HOÀN THÀNH (4/18)

### 1. frmCategories - Quản lý danh mục sản phẩm ⭐

**Trạng thái:** `HOÀN THÀNH 100%` ✅

**File:**
- `frmCategories.cs` (720 dòng)
- `frmCategories.Designer.cs` (494 dòng)
- `Models/Category.cs`

**Chức năng:**
- ✅ Load danh sách danh mục
- ✅ Tìm kiếm theo tên, mô tả
- ✅ Thêm mới danh mục
- ✅ Sửa thông tin danh mục
- ✅ Xóa danh mục (có xác nhận)
- ✅ Chọn ảnh đại diện
- ✅ Auto-save ảnh khi duyệt
- ✅ Preview ảnh
- ✅ Xóa ảnh cũ tự động
- ✅ Validation trùng tên
- ✅ Loading indicator

**Điểm chất lượng:** 100/100 🏆

**Bảo mật:**
- ✅ Path traversal protection
- ✅ File size limit (10MB)
- ✅ Extension whitelist
- ✅ SQL injection safe (EF Core)

**Lưu ý:**
- Ảnh lưu trong: `Images/Categories/`
- Relative path lưu vào DB: `Images/Categories/filename.png`
- Form fullscreen, responsive

---

### 2. frmLogin - Đăng nhập

**File:** `frmLogin.cs`

**Chức năng:**
- ✅ Đăng nhập với username/password
- ✅ Phân quyền theo role

---

### 3. frmMain - Form chính (MDI Container)

**File:** `frmMain.cs`

**Chức năng:**
- ✅ MDI Container cho các form con
- ✅ Menu chính
- ✅ Toolbar

---

### 4. frmDashboard - Màn hình tổng quan

**File:** `frmDashboard.cs`

**Chức năng:**
- ✅ Charts, KPIs
- ✅ Thống kê nhanh
- ✅ Đọc dữ liệu từ orders, payments

---

## 🔜 FORMS SẮP LÀM (14/18)

### Ưu tiên cao (Core Business):

1. **frmProducts** - Quản lý sản phẩm
   - CRUD sản phẩm
   - Size matrix (S, M, L)
   - Upload nhiều ảnh

2. **frmToppings** - Quản lý topping
   - CRUD topping
   - Set giá

3. **frmTables** - Quản lý bàn
   - CRUD bàn
   - Visual layout
   - Status tracking

4. **frmPOS** ⭐ - Bán hàng (QUAN TRỌNG NHẤT)
   - Tạo đơn hàng
   - Custom: size, đường, đá, toppings
   - Tính tiền real-time

5. **frmPayment** ⭐ - Thanh toán (QUAN TRỌNG NHẤT)
   - Xử lý thanh toán
   - Multiple payment methods
   - In hóa đơn

### Ưu tiên thấp hơn:

6. **frmCustomers** - Quản lý khách hàng
7. **frmMemberships** - Quản lý hội viên
8. **frmVouchers** - Quản lý voucher
9. **frmOrderHistory** - Lịch sử đơn hàng
10. **frmSalesReport** - Báo cáo doanh thu
11. **frmAuditLog** - Audit log
12. **frmUsers** - Quản lý nhân viên
13. **frmChangePassword** - Đổi mật khẩu
14. **frmOrderDetail** - Chi tiết đơn hàng

---

## 📁 Cấu trúc dự án

```
MilkTEA-POS/
├── Models/                 # Entity models
│   ├── Category.cs
│   ├── Product.cs
│   ├── Order.cs
│   ├── Customer.cs
│   └── ...
├── Images/
│   └── Categories/        # Ảnh danh mục sản phẩm
├── frmDashboard.cs        # Màn hình tổng quan
├── frmLogin.cs            # Đăng nhập
├── frmCategories.cs       # Quản lý danh mục ✅
├── frmMain.cs             # Form chính
├── database.sql           # Database schema
├── db.config.txt          # Connection string
├── README.md              # File này
└── các form cần thiết kế.txt  # Danh sách chi tiết
```

---

## 🛠️ Công nghệ sử dụng

| Component | Technology |
|-----------|-----------|
| **Framework** | .NET 10.0 |
| **Language** | C# |
| **UI** | Windows Forms |
| **Database** | PostgreSQL |
| **ORM** | Entity Framework Core |

---

## 📝 Hướng dẫn phát triển

### Thêm form mới:
1. Tạo form: `Add → Windows Form`
2. Đặt tên: `frm<TênForm>.cs`
3. Implement logic
4. Update README.md và `các form cần thiết kế.txt`

### Quy ước đặt tên:
- Forms: `frm<Tên>.cs` (ví dụ: `frmCategories.cs`)
- Models: `<Tên>.cs` (ví dụ: `Category.cs`)
- Controls: `<Type><Tên>` (ví dụ: `btnAdd`, `txtName`)

### Code standards:
- ✅ Sử dụng `#region` để organize code
- ✅ Async/await cho DB operations
- ✅ Error handling với try-catch
- ✅ Friendly error messages (Vietnamese)
- ✅ Validation đầy đủ

---

## 🔧 Troubleshooting

### Lỗi thường gặp:

**1. Cannot access file (file being used)**
```
→ Đóng app đang chạy trước khi build
```

**2. Connection string error**
```
→ Kiểm tra db.config.txt
→ Đảm bảo PostgreSQL đang chạy
```

**3. Migration errors**
```
→ Chạy lại database.sql
→ Xóa bin/obj và build lại
```

---

## 👥 Đóng góp

Khi commit code:
```bash
git add .
git commit -m "[Form] Description"
# Ví dụ: "[Categories] Add image upload feature"
git push
```

---

## 📄 License

MIT License - Xem file LICENSE

---

## 📞 Liên hệ

Nếu có vấn đề, tạo issue hoặc liên hệ team lead.

---

**Cập nhật cuối:** April 3, 2026  
**Version:** 1.0.0  
**Status:** In Development 🚧  
**Progress:** 4/18 forms (22%)
