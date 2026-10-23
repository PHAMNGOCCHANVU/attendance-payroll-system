# 🛡️ Hệ Thống Quản Lý Nhân Sự, Chấm Công & Tính Lương (HRM - Payroll System)

> Dự án đồ án kết hợp **Phân Tích Thiết Kế Hướng Đối Tượng (OOAD)** & **Bảo Mật Cơ Sở Dữ Liệu (BMCSDL)**.  
> Ứng dụng Desktop Windows Forms (.NET Framework) tích hợp kiến trúc phân tầng (Layered/MVC - Repository & Service Pattern), mã hóa dữ liệu nhạy cảm AES-256, phân quyền mức dòng/cột (Row/Column Level Security), Data Masking, Audit Logging và quy trình CI/CD tự động bằng Jenkins.

---

## 📑 Mục lục
- [Giới thiệu tổng quan](#-giới-thiệu-tổng-quan)
- [Công nghệ & Nền tảng sử dụng](#-công-nghệ--nền-tảng-sử-dụng)
- [Kiến trúc hệ thống](#-kiến-trúc-hệ-thống)
- [Cơ chế bảo mật cơ sở dữ liệu (BMCSDL)](#-cơ-chế-bảo-mật-cơ-sở-dữ-liệu-bmcsdl)
- [Ma trận phân quyền (RBAC Matrix)](#-ma-trận-phân-quyền-rbac-matrix)
- [Tài khoản demo kiểm thử](#-tài-khoản-demo-kiểm-thử)
- [Cấu trúc thư mục dự án](#-cấu-trúc-thư-mục-dự-án)
- [Hướng dẫn cài đặt & Chạy ứng dụng](#-hướng-dẫn-cài-đặt--chạy-ứng-dụng)
- [Kiểm thử tự động (Unit Test) & CI/CD](#-kiểm-thử-tự-động-unit-test--cicd)

---

## 🌟 Giới thiệu tổng quan

Hệ thống được xây dựng nhằm giải quyết bài toán quản trị nhân sự toàn diện trong doanh nghiệp, bao gồm:
- **Quản lý cơ cấu tổ chức & hồ sơ nhân sự**: Quản lý phòng ban, thông tin cá nhân, hồ sơ bằng cấp, kinh nghiệm làm việc và ảnh thẻ.
- **Quản lý dự án & phân công**: Phân bổ nhân sự tham gia các dự án, theo dõi tiến độ chi tiết của dự án.
- **Chấm công & Thời gian làm việc**: Ghi nhận giờ vào, giờ ra và các trạng thái chuyên cần hàng ngày.
- **Tính lương & Quản lý thu nhập**: Tính toán lương cơ bản, phụ cấp, thưởng, khấu trừ và lương thực lĩnh.
- **Bảo mật chuyên sâu & Kiểm toán (Audit)**: Ngăn chặn truy cập trái phép, bảo vệ thông tin thu nhập nhạy cảm và ghi vết toàn bộ thao tác thêm/sửa nhân sự.

---

## 🛠️ Công nghệ & Nền tảng sử dụng

- **Ngôn ngữ**: C# (.NET Framework 4.7.2)
- **Giao diện (UI)**: Windows Forms (WinForms) với giao diện trực quan, chia theo từng phân hệ chuyên biệt.
- **Cơ sở dữ liệu**: Microsoft SQL Server 2019 / 2022
- **Mẫu kiến trúc**: MVC kết hợp **Repository & Service Pattern**, áp dụng nguyên lý Dependency Injection (IoC).
- **Kiểm thử tự động**: MSTest Framework (`cuoiky.Tests`).
- **CI/CD Pipeline**: Jenkins Pipeline (`Jenkinsfile` tự động restore, build, test và lưu trữ artifacts).

---

## 🏛️ Kiến trúc hệ thống

Dự án được phân chia theo mô hình phân tầng chặt chẽ giúp dễ mở rộng, bảo trì và viết Unit Test:

```text
[ Giao diện Windows Forms (Views) ]
          │
          ▼
   [ Controllers ]
          │
          ▼
 [ Services (Nghiệp vụ) ]
          │
          ▼
[ Repositories (Truy cập dữ liệu) ]
          │ (Gọi Stored Procedures & Bảo mật)
          ▼
[ Cơ sở dữ liệu SQL Server (QuanLyNhanSu) ]
   ├── Băm SHA2_256 (Mật khẩu)
   ├── Mã hóa AES_256 (Bảng lương)
   ├── Session Context & Data Masking
   └── Audit Logging (Lịch sử thao tác)
```

- **Views**: Chia theo từng vai trò (`FormLogin`, `FormGiamDoc`, `FormTruongPhong`, `FormNhanSu`, `FormTaiVu`, `FormNhanVien`).
- **Controllers**: Tiếp nhận sự kiện từ View, điều phối xử lý dữ liệu và phản hồi lại giao diện.
- **Services**: Chứa các quy tắc nghiệp vụ độc lập (Business Logic), dễ dàng mock khi kiểm thử.
- **Repositories**: Chịu trách nhiệm tương tác CSDL thông qua ADO.NET (`SqlCommand`, `SqlDataReader`, `StoredProcedure`), che giấu chi tiết truy vấn khỏi tầng trên.
- **Models**: Chứa các thực thể dữ liệu (`NhanVien`, `Luong`, `PhongBan`, `DuAn`, `ChamCong`, `HoSoNhanVien`, `LichSuHoatDong`...).

---

## 🔒 Cơ chế bảo mật cơ sở dữ liệu (BMCSDL)

Đây là điểm nổi bật cốt lõi của đồ án, áp dụng đầy đủ các kỹ thuật bảo mật CSDL hiện đại:

### 1. Băm mật khẩu (Password Hashing)
- Toàn bộ mật khẩu đăng nhập được băm bằng thuật toán **SHA2_256** thông qua hàm `HASHBYTES('SHA2_256', @MatKhau)`.
- Không lưu trữ mật khẩu ở dạng văn bản rõ (plain-text). Xác thực đăng nhập được thực hiện qua Stored Procedure `SP_KiemTraDangNhap`.

### 2. Mã hóa mức cột bằng Khóa đối xứng (Column-Level Encryption - AES-256)
- Tạo **Database Master Key**, **Certificate** (`Cert_QuanLyNhanSu`) và **Symmetric Key** (`SymKey_QuanLyNhanSu`) với thuật toán **AES_256**.
- Toàn bộ các trường dữ liệu thu nhập nhạy cảm trong bảng `Luong` (`LuongCoBan_Encrypted`, `PhuCap_Encrypted`, `Thuong_Encrypted`, `KhauTru_Encrypted`, `LuongThucLinh_Encrypted`) đều được mã hóa nhị phân `VARBINARY(MAX)`.
- Dữ liệu chỉ được mở khóa giải mã tạm thời trong ngữ cảnh phiên làm việc hợp lệ và được đóng lại ngay sau khi truy vấn.

### 3. Kiểm soát truy cập mức dòng & mức cột (Row & Column Level Security)
- Sử dụng `SESSION_CONTEXT(N'CurrentUserID')` để định danh nhân viên thực thi trong từng phiên gọi Stored Procedure.
- **Nhân viên thông thường**: Chỉ được phép xem bảng lương của **chính mình**. Cột lương của tất cả đồng nghiệp khác trong phòng sẽ tự động trả về `NULL`.
- **Trưởng phòng**: Chỉ được xem thông tin và lương của các nhân sự **thuộc phòng ban mình quản lý**.
- **Giám đốc**: Được quyền xem và quản lý bảng lương của toàn bộ nhân viên công ty.

### 4. Che giấu dữ liệu động (Dynamic Data Masking - DDM)
- Áp dụng đối với **Nhân viên Tài vụ (Finance)** qua `SP_TaiVu_XemDanhSachNhanVien`:
  - Nhân viên tài vụ cần thấy thông tin lương để xử lý kế toán và chi trả, nhưng để đảm bảo tính riêng tư, các thông tin định danh cá nhân (Họ tên, giới tính, ngày sinh, số điện thoại, hình ảnh) của nhân viên phòng ban khác sẽ bị che giấu (`NULL`), chỉ hiển thị `MaNV`, `MaSoThue` và các con số lương cần chi trả.

### 5. Ghi nhật ký & Kiểm toán bảo mật (Audit Trail Logging)
- Mọi thao tác thêm mới hoặc sửa đổi dữ liệu nhân sự đều được ghi nhận vào bảng `LichSuHoatDongNhanSu` và `LichSuHoatDong`.
- Lưu trữ rõ ràng: Thời gian thực hiện, Mã/Tên người thao tác, Hành động (`THEM`, `SUA`, `XOA`), Nhân viên bị tác động và chi tiết nội dung thay đổi.

---

## 📊 Ma trận phân quyền (RBAC Matrix)

| Vai trò | Phân hệ / Giao diện | Quyền hạn dữ liệu nhân sự | Quyền hạn dữ liệu lương | Ghi chú bảo mật |
| :--- | :--- | :--- | :--- | :--- |
| **Giám đốc** | `FormGiamDoc` | Toàn quyền xem/sửa toàn bộ công ty | Xem và cập nhật lương tất cả nhân viên (giải mã AES-256) | Quyền cao nhất trong hệ thống |
| **Trưởng phòng NS** | `FormTruongPhong` (NS) | Quản lý nhân viên toàn bộ các phòng ban, xem lịch sử audit | Chỉ xem được lương của chính mình | Ngăn chặn leo thang xem lương |
| **Nhân viên NS** | `FormNhanSu` | Thêm, sửa nhân viên (ngoại trừ phòng Nhân Sự) | Chỉ xem được lương của chính mình | Giới hạn can thiệp phòng ban nhạy cảm |
| **Trưởng phòng khác** | `FormTruongPhong` | Quản lý nhân viên trong phạm vi phòng ban mình | Xem được lương nhân viên trong phòng ban mình | Row-level theo mã phòng ban |
| **Nhân viên Tài vụ** | `FormTaiVu` | Xem thông tin nhân viên phòng Tài Vụ | Xem được lương tất cả nhân viên để chi trả | Masking thông tin cá nhân của phòng ban khác |
| **Nhân viên thường** | `FormNhanVien` | Xem danh sách đồng nghiệp cùng phòng | **Chỉ xem được lương của chính mình** | Cột lương người khác = `NULL` |

---

## 🔑 Tài khoản demo kiểm thử

Tất cả các tài khoản mặc định đều có mật khẩu: **`123456`**

| Tên đăng nhập | Mật khẩu | Vai trò | Phòng ban | Mục đích kiểm thử |
| :--- | :--- | :--- | :--- | :--- |
| `giamdoc` | `123456` | Giám đốc | Ban Giám Đốc | Kiểm tra toàn quyền xem và điều chỉnh lương AES-256 |
| `nhansu_truong` | `123456` | Trưởng phòng | Phòng Nhân Sự | Quản lý nhân sự toàn diện, xem lịch sử audit |
| `nhansu_nv` | `123456` | Nhân viên | Phòng Nhân Sự | Kiểm tra thêm/sửa nhân viên, kiểm tra lương chính mình |
| `taivu_truong` | `123456` | Trưởng phòng | Phòng Tài Vụ | Quản lý nghiệp vụ tài vụ |
| `taivu_nv` | `123456` | Nhân viên | Phòng Tài Vụ | Kiểm tra Data Masking (thấy lương, ẩn info cá nhân khác) |
| `kinhdoanh_tp` | `123456` | Trưởng phòng | Phòng Kinh Doanh | Kiểm tra xem danh sách & lương nội bộ phòng ban |
| `kinhdoanh_nv` | `123456` | Nhân viên | Phòng Kinh Doanh | Kiểm tra chỉ thấy lương cá nhân, ẩn lương đồng nghiệp |

---

## 📂 Cấu trúc thư mục dự án

```text
OOAD-PTKHDT/
├── Jenkinsfile                  # Kịch bản CI/CD Pipeline (Build, Test, Archive)
├── OOAD-PTKHDT.slnx             # Visual Studio Solution
├── cuoiky/                      # Project chính Windows Forms (.NET Framework 4.7.2)
│   ├── App.config               # Cấu hình Connection String SQL Server
│   ├── Program.cs               # Điểm khởi chạy ứng dụng (Khởi tạo DI & FormLogin)
│   ├── data.sql                 # Script khởi tạo Database, mã hóa & Stored Procedures
│   ├── cuoiky.csproj            # File cấu hình Project C#
│   ├── Controllers/             # Các lớp điều phối nghiệp vụ
│   │   ├── AuthController.cs
│   │   ├── GiamDocController.cs
│   │   ├── NhanSuController.cs
│   │   ├── NhanVienController.cs
│   │   ├── TaiVuController.cs
│   │   ├── TruongPhongController.cs
│   │   └── TruongPhongNhanSuController.cs
│   ├── Models/                  # Các lớp thực thể (DTO)
│   │   ├── ChamCong.cs
│   │   ├── ChiTietDuAn.cs
│   │   ├── DuAn.cs
│   │   ├── HoSoNhanVien.cs
│   │   ├── LichSuHoatDong.cs
│   │   ├── Luong.cs
│   │   ├── NhanVien.cs
│   │   ├── PhongBan.cs
│   │   └── VaiTro.cs
│   ├── Repositories/            # Các lớp thao tác dữ liệu (Data Access Layer)
│   │   ├── IAuthRepository.cs
│   │   ├── AuthRepository.cs
│   │   ├── GiamDocRepository.cs
│   │   ├── NhanSuRepository.cs
│   │   ├── NhanVienRepository.cs
│   │   ├── TaiVuRepository.cs
│   │   ├── TruongPhongRepository.cs
│   │   └── TruongPhongNhanSuRepository.cs
│   ├── Services/                # Lớp xử lý logic độc lập
│   │   └── AuthService.cs
│   ├── Views/                   # Giao diện người dùng theo phân hệ
│   │   ├── Login/               # Giao diện Đăng nhập
│   │   ├── GiamDoc/             # Giao diện Giám đốc
│   │   ├── TruongPhong/         # Giao diện Trưởng phòng
│   │   ├── NhanSu/              # Giao diện Nhân sự
│   │   ├── TaiVu/               # Giao diện Tài vụ
│   │   └── NhanVien/            # Giao diện Nhân viên
│   └── image/                   # Tài nguyên hình ảnh, avatar nhân viên
└── cuoiky.Tests/                # Project kiểm thử tự động (MSTest)
    ├── AuthServiceTests.cs      # Kiểm thử nghiệp vụ xác thực & xử lý đầu vào
    └── cuoiky.Tests.csproj
```

---

## 🚀 Hướng dẫn cài đặt & Chạy ứng dụng

### 1. Yêu cầu môi trường
- Hệ điều hành: Windows 10/11
- Visual Studio 2019 / 2022 (cài đặt workload **.NET desktop development**)
- .NET Framework 4.7.2 Developer Pack / Runtime
- Microsoft SQL Server 2019 trở lên (kèm SQL Server Management Studio - SSMS)

### 2. Thiết lập Cơ sở dữ liệu
1. Mở **SQL Server Management Studio (SSMS)** và kết nối tới SQL Server instance của bạn.
2. Mở file `cuoiky/data.sql`.
3. Nhấn **Execute** (`F5`) để thực thi toàn bộ script. Script sẽ tự động:
   - Tạo database `QuanLyNhanSu`.
   - Tạo các bảng, quan hệ khóa ngoại.
   - Thiết lập Master Key, Certificate và Symmetric Key (`SymKey_QuanLyNhanSu`).
   - Chèn dữ liệu mẫu, băm mật khẩu SHA-256 và mã hóa lương AES-256.
   - Tạo toàn bộ Stored Procedures phân quyền và bảo mật.

### 3. Cấu hình chuỗi kết nối (Connection String)
Mở file `cuoiky/App.config` và điều chỉnh chuỗi kết nối phù hợp với máy của bạn:

```xml
<connectionStrings>
    <add name="connectStr"
         connectionString="Data Source=.;Initial Catalog=QuanLyNhanSu;Integrated Security=True;TrustServerCertificate=True"
         providerName="System.Data.SqlClient"/>
</connectionStrings>
```
*(Nếu dùng SQL Express hoặc tài khoản `sa`, thay đổi `Data Source=.\SQLEXPRESS` hoặc bổ sung `User Id=...;Password=...`)*.

### 4. Biên dịch & Chạy dự án
- **Cách 1: Sử dụng Visual Studio**
  1. Mở solution `OOAD-PTKHDT.slnx` hoặc project `cuoiky/cuoiky.csproj` bằng Visual Studio.
  2. Chọn cấu hình `Debug` hoặc `Release` với nền tảng `AnyCPU` / `x86` / `x64`.
  3. Nhấn **F5** (hoặc `Ctrl + F5`) để biên dịch và khởi chạy ứng dụng.
- **Cách 2: Sử dụng dòng lệnh (.NET CLI / MSBuild)**
  ```powershell
  dotnet restore cuoiky/cuoiky.csproj
  dotnet build cuoiky/cuoiky.csproj --configuration Release
  .\cuoiky\bin\Release\cuoiky.exe
  ```

---

## 🧪 Kiểm thử tự động (Unit Test) & CI/CD

### 1. Chạy Unit Test
Dự án có sẵn bộ test nghiệp vụ đăng nhập và kiểm tra dữ liệu bằng MSTest:

```powershell
dotnet test cuoiky.Tests/cuoiky.Tests.csproj --configuration Release --logger "console;verbosity=detailed"
```

### 2. Tự động hóa CI/CD với Jenkins
Dự án tích hợp sẵn file cấu hình `Jenkinsfile` với các stage tự động:
1. **Checkout SCM**: Lấy mã nguồn mới nhất từ kho lưu trữ Git.
2. **Restore Dependencies**: Khôi phục các gói phụ thuộc NuGet.
3. **Build Project**: Biên dịch cả ứng dụng chính và project kiểm thử ở chế độ `Release`.
4. **Run Unit Tests**: Tự động chạy toàn bộ Unit Tests và xuất báo cáo kiểm thử định dạng `.trx` ra thư mục `TestResults/`.
5. **Archive Artifacts**: Tự động lưu trữ file thực thi `.exe` và file kết quả kiểm thử vào Jenkins Server.

---

## 👥 Tác giả & Giấy phép
- **Học phần**: Phân tích thiết kế hướng đối tượng (OOAD) & Bảo mật cơ sở dữ liệu (BMCSDL)
- Dự án phục vụ mục đích học tập, nghiên cứu và thực hành kiến trúc bảo mật trong phát triển phần mềm.
