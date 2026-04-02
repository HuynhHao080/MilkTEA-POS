# 🧋 MilkTea POS - Hệ thống quản lý trà sữa

## 📋 Giới thiệu

MilkTea POS là hệ thống quản lý quán trà sữa hiện đại với UI/UX 2026, được phát triển trên nền tảng **WinForms (.NET 10)** và **PostgreSQL (Supabase)**.

## ✨ Tính năng chính

### 🎯 Đã hoàn thành (3 forms đầu tiên)
- ✅ **frmLogin** - Đăng nhập với UI hiện đại
- ✅ **frmMain** - MDI Container với menu sidebar
- ✅ **frmCategories** - Quản lý danh mục sản phẩm (CRUD)
- ✅ **frmDashboard** - Dashboard với thống kê nhanh

### 📅 Sắp phát triển
- 📦 Quản lý sản phẩm (Products)
- 🍮 Quản lý Toppings
- 🪑 Quản lý Bàn
- 🛒 Order (POS)
- 💳 Thanh toán
- 👥 Quản lý khách hàng
- 🎫 Vouchers
- 📈 Báo cáo

## 🎨 Chủ đề UI 2026

### Màu sắc chính
```
Primary:    #FF6B6B (Coral Red - trà sữa trân châu)
Secondary:  #4ECDC4 (Turquoise - tươi mát)
Background: #F7F9FC (Light gray-blue)
Surface:    #FFFFFF (White)
Text:       #2D3748 (Dark gray)
Success:    #48BB78
Warning:    #ED8936
Danger:     #DC3545
Info:       #17A2B8
```

### Font chữ
- **Chính**: Segoe UI (Microsoft)
- **Cỡ chữ**: 11-20pt tùy mục đích
- **Style**: Modern, Flat, Material Design

## 🚀 Cài đặt

### Yêu cầu hệ thống
- .NET 10 SDK
- PostgreSQL 18+
- Supabase account (để dùng cloud database)

### Cấu hình Database
File cấu hình: `db.config.txt`
```
Host=aws-1-ap-southeast-1.pooler.supabase.com
Port=5432
Database=postgres
Username=postgres.tisoidtsgtqwifjfunrs
Password=nliwmgmwbwAnhZk7
SSL Mode=Require
```

### Chạy ứng dụng
```bash
# Build project
dotnet build

# Run application
dotnet run
```

Hoặc mở file `.slnx` trong Visual Studio 2022 và nhấn F5.

## 👤 Tài khoản mặc định

| Username | Password | Role |
|----------|----------|------|
| admin | admin123 | Admin |
| staff01 | staff123 | Staff |
| cashier01 | cashier123 | Cashier |

## 📁 Cấu trúc project

```
MilkTeaPOS/
├── Models/                 # Entity Framework models
│   ├── Category.cs
│   ├── Product.cs
│   ├── User.cs
│   └── ...
├── frmLogin.cs            # Form đăng nhập
├── frmMain.cs             # Form chính (MDI)
├── frmCategories.cs       # Quản lý danh mục
├── frmDashboard.cs        # Dashboard
├── database.sql           # Database schema
└── Program.cs             # Entry point
```

## 🎯 Forms đã hoàn thành

### 1. frmLogin (Đăng nhập)
- UI 2 cột: Branding + Login form
- Validation đầy đủ
- Phân quyền theo role
- Emoji và modern styling

### 2. frmMain (Form chính)
- MDI Container
- Sidebar menu với 14 chức năng
- Header với user info và đồng hồ
- Welcome panel với stats cards
- Support mở forms con

### 3. frmCategories (Quản lý danh mục)
- Full CRUD operations
- Search/filter
- Image upload preview
- DataGridView styling
- Form validation

### 4. frmDashboard (Tổng quan)
- Stats cards với màu sắc
- Emoji icons
- Responsive layout

## 📊 Database Schema

Xem chi tiết trong `database.sql`:
- 16 tables
- 18 foreign keys
- 79 indexes
- 24 triggers
- 14 functions
- 11 custom types

## 🔧 Technical Stack

| Component | Technology |
|-----------|------------|
| Framework | .NET 10 / WinForms |
| Database | PostgreSQL 18 |
| ORM | Entity Framework Core |
| Hosting | Supabase Cloud |
| UI Style | Modern Material 2026 |

## 📝 Checklist phát triển

### Phase 1: Nền tảng ✅
- [x] Setup database
- [x] Tạo Models từ EF Core
- [x] frmLogin
- [x] frmMain
- [x] frmDashboard
- [x] frmCategories

### Phase 2: Core Business (Tiếp theo)
- [ ] frmToppings
- [ ] frmTables
- [ ] frmProducts
- [ ] frmPOS (Quan trọng!)
- [ ] frmPayment

### Phase 3: CRM & Reports
- [ ] frmCustomers
- [ ] frmMemberships
- [ ] frmVouchers
- [ ] frmOrderHistory
- [ ] frmSalesReport
- [ ] frmAuditLog
- [ ] frmUsers

## 🎓 Hướng dẫn phát triển

### Thêm form mới
1. Tạo form kế thừa từ Form
2. Setup UI trong `SetupModernUI()` method
3. Dùng màu sắc theme đã định nghĩa
4. Thêm vào menu trong frmMain

### Best practices
- ✅ Dùng async/await cho database operations
- ✅ Validation đầy đủ trước khi lưu
- ✅ MessageBox xác nhận khi xóa
- ✅ Handle exceptions properly
- ✅ Comment code rõ ràng

## 🐛 Known Issues

- Password lưu plaintext (cần hash trong production)
- Một số forms chưa implement (đang phát triển)

## 📄 License

Sinh viên sử dụng cho mục đích học tập.

## 👥 Nhóm phát triển

Developed with ❤️ by MilkTea POS Team - 2026

---

**Version**: 2026.1.0  
**Last Updated**: March 2026
