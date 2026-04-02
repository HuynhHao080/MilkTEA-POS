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
├── frmDashboard.cs        # Màn hình chính
├── frmLogin.cs            # Đăng nhập
├── frmCategories.cs       # Quản lý danh mục ✅
├── frmMain.cs             # Form chính
├── database.sql           # Database schema
├── db.config.txt          # Connection string
└── README.md              # File này
```

---

## ✅ DANH SÁCH FORM ĐÃ HOÀN THÀNH

### 1. frmCategories - Quản lý danh mục sản phẩm ✅

**Trạng thái:** `HOÀN THÀNH 100%` ✅

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

**File liên quan:**
- `frmCategories.cs` (720 dòng)
- `frmCategories.Designer.cs` (494 dòng)
- `Models/Category.cs`

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

## 🔜 FORM ĐANG PHÁT TRIỂN

*(Cập nhật khi có form mới)*

---

## 📊 Database Schema

### Các bảng chính:
- `roles` - Vai trò người dùng
- `users` - Người dùng (nhân viên)
- `categories` - Danh mục sản phẩm ✅
- `products` - Sản phẩm
- `product_sizes` - Kích cỡ & giá
- `toppings` - Topping
- `customers` - Khách hàng
- `memberships` - Hội viên
- `vouchers` - Mã giảm giá
- `orders` - Đơn hàng
- `order_details` - Chi tiết đơn
- `payments` - Thanh toán
- `tables` - Bàn

Xem chi tiết trong `database.sql`

---

## 🛠️ Công nghệ sử dụng

| Component | Technology |
|-----------|-----------|
| **Framework** | .NET 10.0 |
| **Language** | C# |
| **UI** | Windows Forms |
| **Database** | PostgreSQL |
| **ORM** | Entity Framework Core |
| **Auth** | Supabase (planned) |

---

## 📝 Hướng dẫn phát triển

### Thêm form mới:
1. Tạo form: `Add → Windows Form`
2. Đặt tên: `frm<TênForm>.cs`
3. Implement logic
4. Update file này

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
