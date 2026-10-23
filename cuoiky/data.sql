-- KHỞI TẠO DATABASE
CREATE DATABASE QuanLyNhanSu;
GO

USE QuanLyNhanSu;
GO

-- 1. Vai trò
CREATE TABLE VaiTro (
    MaVaiTro INT PRIMARY KEY IDENTITY(1,1),
    TenVaiTro NVARCHAR(50) NOT NULL
);
GO

-- 2. Phòng ban
CREATE TABLE PhongBan (
    MaPhongBan INT PRIMARY KEY IDENTITY(1,1),
    TenPhongBan NVARCHAR(100) NOT NULL,
    TruongPhong INT NULL
);
GO

-- 3. Nhân viên
CREATE TABLE NhanVien (
    MaNV INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100),
    Phai NVARCHAR(10),
    NgaySinh DATE,
    SoDienThoai NVARCHAR(15),
    LuongCoBan DECIMAL(18,2),
    PhuCap DECIMAL(18,2),
    MaSoThue NVARCHAR(20),
    MaPhongBan INT,
    FOREIGN KEY (MaPhongBan) REFERENCES PhongBan(MaPhongBan)
);
GO

ALTER TABLE PhongBan
ADD CONSTRAINT FK_TruongPhong FOREIGN KEY (TruongPhong)
REFERENCES NhanVien(MaNV);
GO

-- 4. Dự án
CREATE TABLE DuAn (
    MaDuAn INT PRIMARY KEY IDENTITY(1,1),
    TenDuAn NVARCHAR(100),
    NgayBatDau DATE,
    NgayKetThuc DATE
);
GO

-- 5. Liên kết dự án - nhân viên
CREATE TABLE NhanVien_DuAn (
    MaNV INT,
    MaDuAn INT,
    NgayThamGia DATE,
    PRIMARY KEY (MaNV, MaDuAn),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaDuAn) REFERENCES DuAn(MaDuAn)
);
GO

-- 6. Tài khoản đăng nhập
CREATE TABLE NguoiDungDangNhap (
    TenDangNhap NVARCHAR(50) PRIMARY KEY,
    MatKhau NVARCHAR(255),
    MaNV INT,
    MaVaiTro INT,
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV),
    FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro)
);
GO

-- 7. Bảng Lương (Lương hàng tháng)
CREATE TABLE Luong (
    MaLuong INT PRIMARY KEY IDENTITY(1,1),
    MaNV INT,
    Thang INT,
    Nam INT,
    LuongCoBan DECIMAL(18,2),
    PhuCap DECIMAL(18,2),
    Thuong DECIMAL(18,2),
    KhauTru DECIMAL(18,2),
    LuongThucLinh AS (LuongCoBan + PhuCap + Thuong - KhauTru),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- 8. Chi tiết dự án
CREATE TABLE ChiTietDuAn (
    MaChiTiet INT PRIMARY KEY IDENTITY(1,1),
    MaDuAn INT,
    NoiDung NVARCHAR(255),
    TrangThai NVARCHAR(50),
    NgayCapNhat DATE,
    FOREIGN KEY (MaDuAn) REFERENCES DuAn(MaDuAn)
);
GO

-- 9. Chấm công
CREATE TABLE ChamCong (
    MaChamCong INT PRIMARY KEY IDENTITY(1,1),
    MaNV INT,
    NgayLam DATE,
    GioVao TIME,
    GioRa TIME,
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- 10. Hồ sơ nhân viên
CREATE TABLE HoSoNhanVien (
    MaHoSo INT PRIMARY KEY IDENTITY(1,1),
    MaNV INT,
    NgayTaoHoSo DATE,
    BangCap NVARCHAR(100),
    KinhNghiem NVARCHAR(255),
    GhiChu NVARCHAR(255),
    FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
);
GO

-- 11.lịch sử hoạt động nhân viên
CREATE TABLE LichSuHoatDongNhanSu (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ThoiGian DATETIME DEFAULT GETDATE(),
    MaNVThucHien INT,
    TenNVThucHien NVARCHAR(100),
    ThaoTac NVARCHAR(50), -- 'THEM', 'SUA', 'XOA'
    TacDongLenNV INT, -- MaNV bị tác động
    ChiTiet NVARCHAR(MAX)
);

-- Vai trò
INSERT INTO VaiTro (TenVaiTro) VALUES
(N'Nhân viên'),
(N'Trưởng phòng'),
(N'Giám đốc');

-- Phòng ban
INSERT INTO PhongBan (TenPhongBan) VALUES
(N'Phòng Nhân Sự'),
(N'Phòng Tài Vụ'),
(N'Phòng Kinh Doanh');

-- Nhân viên
INSERT INTO NhanVien (HoTen, Phai, NgaySinh, SoDienThoai, LuongCoBan, PhuCap, MaSoThue, MaPhongBan)
VALUES
(N'Nguyễn Văn A', N'Nam', '1985-03-12', '0912345678', 20000000, 2000000, N'MST001', 1),
(N'Lê Thị B', N'Nữ', '1990-05-01', '0987654321', 15000000, 1500000, N'MST002', 1),
(N'Phạm Văn C', N'Nam', '1988-07-09', '0909876543', 18000000, 1000000, N'MST003', 2),
(N'Trần Thị D', N'Nữ', '1995-09-21', '0966666666', 12000000, 800000, N'MST004', 2),
(N'Hoàng Văn E', N'Nam', '1992-02-11', '0911111111', 17000000, 1200000, N'MST005', 3),
(N'Phan Thị F', N'Nữ', '1998-10-10', '0977777777', 10000000, 500000, N'MST006', 3),
(N'Lưu Quang G', N'Nam', '1975-01-20', '0900000000', 50000000, 5000000, N'MST007', 3);

-- Cập nhật trưởng phòng
UPDATE PhongBan SET TruongPhong = 1 WHERE MaPhongBan = 1;
UPDATE PhongBan SET TruongPhong = 3 WHERE MaPhongBan = 2;
UPDATE PhongBan SET TruongPhong = 5 WHERE MaPhongBan = 3;

-- Dự án
INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc)
VALUES
(N'Hệ thống ERP nội bộ', '2025-01-01', '2025-12-31'),
(N'Phát triển website công ty', '2025-05-01', '2025-10-01'),
(N'Mở rộng chi nhánh miền Trung', '2025-02-15', '2026-02-15');
-- Liên kết nhân viên - dự án
INSERT INTO NhanVien_DuAn VALUES 
(1,1,'2025-01-05'), (2,1,'2025-02-01'), 
(3,2,'2025-05-05'), (5,2,'2025-06-01'), 
(6,3,'2025-03-01'), (7,3,'2025-03-10');

-- Tài khoản đăng nhập
INSERT INTO NguoiDungDangNhap (TenDangNhap, MatKhau, MaNV, MaVaiTro) VALUES
('nhansu_truong','123456',1,2),
('nhansu_nvdotnet add package Microsoft.EntityFrameworkCore
','123456',2,1),
('taivu_truong','123456',3,2),
('taivu_nv','123456',4,1),
('kinhdoanh_tp','123456',5,2),
('kinhdoanh_nv','123456',6,1),
('giamdoc','123456',7,3);

-- Lương
INSERT INTO Luong (MaNV, Thang, Nam, LuongCoBan, PhuCap, Thuong, KhauTru)
VALUES
(1,4,2026,20000000,2000000,1000000,500000),
(2,4,2026,15000000,1500000,500000,300000),
(3,4,2026,18000000,1000000,200000,400000),
(4,4,2026,12000000,800000,0,200000),
(5,4,2026,17000000,1200000,700000,400000),
(6,4,2026,10000000,500000,0,100000),
(7,4,2026,50000000,5000000,5000000,0);

-- Chi tiết dự án
INSERT INTO ChiTietDuAn (MaDuAn, NoiDung, TrangThai, NgayCapNhat)
VALUES
(1,N'Xây dựng module nhân sự',N'Hoàn thành', '2025-08-01'),
(1,N'Phân hệ tài vụ',N'Đang thực hiện', '2026-03-20'),
(2,N'Thiết kế giao diện website',N'Hoàn thành','2025-08-10'),
(3,N'Trưng cầu ý kiến khách hàng',N'Chuẩn bị','2026-02-01');

-- Chấm công
INSERT INTO ChamCong (MaNV, NgayLam, GioVao, GioRa, GhiChu)
VALUES
(1,'2026-04-29','08:00','17:00',N'Làm bình thường'),
(2,'2026-04-29','08:10','17:05',N'Đến muộn'),
(3,'2026-04-29','08:00','16:50',N'Làm cả ngày'),
(6,'2026-04-29','09:00','18:00',N'Đi gặp khách hàng');

-- Hồ sơ nhân viên
INSERT INTO HoSoNhanVien (MaNV, NgayTaoHoSo, BangCap, KinhNghiem, GhiChu)
VALUES
(1,'2020-01-10',N'Cử nhân Quản trị nhân lực',N'10 năm kinh nghiệm HR',N'Trưởng phòng NS'),
(2,'2021-03-12',N'Cử nhân Kinh tế',N'4 năm kinh nghiệm nhân sự',NULL),
(5,'2019-09-09',N'Cử nhân Marketing',N'6 năm kinh nghiệm KD',NULL),
(7,'2010-01-01',N'Thạc sĩ Quản trị',N'Giám đốc điều hành công ty',NULL);
GO


-------Bổ sung dự án
INSERT INTO DuAn (TenDuAn, NgayBatDau, NgayKetThuc)
VALUES 
(N'Nâng cấp hạ tầng mạng', '2026-01-01', '2026-06-01'),
(N'Đào tạo kỹ năng mềm nội bộ', '2026-02-10', '2026-03-10'),
(N'Số hóa hồ sơ nhân sự', '2026-01-15', '2026-12-31'),
(N'Phát triển App mobile', '2026-03-01', '2026-09-01'),
(N'Kiểm toán hệ thống bảo mật', '2026-04-01', '2026-05-01'),
(N'Khảo sát sự hài lòng nhân viên', '2026-05-15', '2026-06-15'),
(N'Xây dựng quy trình tuyển dụng mới', '2026-02-20', '2026-04-20'),
(N'Nghiên cứu thị trường miền Tây', '2026-06-01', '2026-11-01'),
(N'Tổ chức Team Building 2026', '2026-07-01', '2026-07-05'),
(N'Thiết kế bộ nhận diện thương hiệu', '2026-03-15', '2026-08-15');
-----
INSERT INTO NhanVien_DuAn (MaNV, MaDuAn, NgayThamGia)
SELECT 6, MaDuAn, GETDATE()
FROM (
    SELECT TOP 10 MaDuAn 
    FROM DuAn 
    ORDER BY MaDuAn DESC
) AS Projects;
-----------Bổ sung
-- 1. Thêm cột MaVaiTro vào bảng NhanVien
ALTER TABLE NhanVien ADD MaVaiTro INT;

-- 2. Tạo liên kết khóa ngoại
ALTER TABLE NhanVien ADD CONSTRAINT FK_NhanVien_VaiTro 
FOREIGN KEY (MaVaiTro) REFERENCES VaiTro(MaVaiTro);
-------------Bổ sung cột ảnh
-- Thêm cột để lưu tên file ảnh (ví dụ: nam.jpg)
ALTER TABLE NhanVien ADD HinhAnh NVARCHAR(255);
-- Gán tên file ảnh cho chính bạn
UPDATE NhanVien SET HinhAnh = 'nam_the.png' WHERE MaNV = 6;
UPDATE NhanVien SET HinhAnh = 'truongphong.png' WHERE MaNV = 1;
UPDATE NhanVien SET HinhAnh = 'Du.png' WHERE MaNV = 7;
-------- Bổ sung
-------- Bổ sung
CREATE TABLE LichSuHoatDong (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    MaNV INT,
    TenDangNhap NVARCHAR(50),
    HanhDong NVARCHAR(50),
    ChiTiet NVARCHAR(MAX),
    ThoiGian DATETIME DEFAULT GETDATE()
);

-- update vai trò cho bảng nhân viên
USE QuanLyNhanSu;
GO
UPDATE NhanVien SET MaVaiTro = 2 WHERE MaNV = 1; -- Trưởng phòng nhân sự
UPDATE NhanVien SET MaVaiTro = 1 WHERE MaNV = 2; -- NV phòng nhân sự
UPDATE NhanVien SET MaVaiTro = 2 WHERE MaNV = 3; -- Trưởng phòng (Tài vụ)
UPDATE NhanVien SET MaVaiTro = 1 WHERE MaNV = 4; -- NV phòng tài vụ
UPDATE NhanVien SET MaVaiTro = 2 WHERE MaNV = 5; -- Trưởng phòng (Kinh doanh)
UPDATE NhanVien SET MaVaiTro = 1 WHERE MaNV = 6; -- NV (Kinh doanh)
UPDATE NhanVien SET MaVaiTro = 3 WHERE MaNV = 7; -- Giám đốc


USE QuanLyNhanSu;
GO

-- Xóa các cột mã hóa (nếu có)
IF COL_LENGTH('NhanVien', 'LuongCoBan_Encrypted') IS NOT NULL
    ALTER TABLE NhanVien DROP COLUMN LuongCoBan_Encrypted;
GO

IF COL_LENGTH('NhanVien', 'PhuCap_Encrypted') IS NOT NULL
    ALTER TABLE NhanVien DROP COLUMN PhuCap_Encrypted;
GO

-- Xóa các cột lương gốc trong bảng NhanVien
ALTER TABLE NhanVien DROP COLUMN LuongCoBan;
ALTER TABLE NhanVien DROP COLUMN PhuCap;
GO



USE QuanLyNhanSu;
GO


--------------------BẢO MẬT 1 BĂM MẬT KHẨU (THUẬT TOÁN SHA2_256)------------------------

--Thêm một cột mới có kiểu VARBINARY để chứa mật khẩu đã băm
ALTER TABLE NguoiDungDangNhap ADD MatKhauHash VARBINARY(256);
GO

-- Cập nhật dữ liệu: Dùng thuật toán SHA2_256 băm mật khẩu cũ ('123456') và lưu vào cột mới
UPDATE NguoiDungDangNhap 
SET MatKhauHash = HASHBYTES('SHA2_256', MatKhau);
GO

-- Xóa bỏ cột mật khẩu chữ thường (trần) không bảo mật
ALTER TABLE NguoiDungDangNhap DROP COLUMN MatKhau;
GO

-- Tạo Stored Procedure (Thủ tục) để dùng cho Form Đăng nhập
CREATE OR ALTER PROCEDURE SP_KiemTraDangNhap
    @TenDangNhap NVARCHAR(50),
    @MatKhau NVARCHAR(255)
AS
BEGIN
    SELECT 
        nv.MaNV, 
        vt.TenVaiTro,
        ISNULL(pb.TenPhongBan, '') AS TenPhongBan
    FROM NguoiDungDangNhap nd
    JOIN NhanVien nv ON nd.MaNV = nv.MaNV
    JOIN VaiTro vt ON nd.MaVaiTro = vt.MaVaiTro
    LEFT JOIN PhongBan pb ON nv.MaPhongBan = pb.MaPhongBan 
    WHERE nd.TenDangNhap = @TenDangNhap 
      AND nd.MatKhauHash = HASHBYTES('SHA2_256', @MatKhau);
END;
GO


--------------------BẢO MẬT 2 MÃ HÓA LƯƠNG (MÃ HÓA ĐỐI XỨNG AES-256)------------------------
USE QuanLyNhanSu;
GO

IF NOT EXISTS (SELECT * FROM sys.symmetric_keys WHERE name = '##MS_DatabaseMasterKey##')
BEGIN
    CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'Ma_hoa_luong_@123456';
END
GO

IF NOT EXISTS (SELECT * FROM sys.certificates WHERE name = 'Cert_QuanLyNhanSu')
BEGIN
    CREATE CERTIFICATE Cert_QuanLyNhanSu 
    WITH SUBJECT = 'Chứng chỉ bảo mật thông tin nhân sự';
END
GO

IF NOT EXISTS (SELECT * FROM sys.symmetric_keys WHERE name = 'SymKey_QuanLyNhanSu')
BEGIN
    CREATE SYMMETRIC KEY SymKey_QuanLyNhanSu
    WITH ALGORITHM = AES_256
    ENCRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;
END
GO

--  Thêm các cột chứa dữ liệu MÃ HÓA vào bảng Luong
ALTER TABLE Luong ADD LuongCoBan_Encrypted VARBINARY(MAX);
ALTER TABLE Luong ADD PhuCap_Encrypted VARBINARY(MAX);
ALTER TABLE Luong ADD Thuong_Encrypted VARBINARY(MAX);
ALTER TABLE Luong ADD KhauTru_Encrypted VARBINARY(MAX);
ALTER TABLE Luong ADD LuongThucLinh_Encrypted VARBINARY(MAX);
GO

--  Mở khóa Symmetric Key để tiến hành mã hóa
OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu
DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;
GO

--  Mã hóa dữ liệu bảng Luong
UPDATE Luong
SET 
    LuongCoBan_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), LuongCoBan)),
    PhuCap_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), PhuCap)),
    Thuong_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), Thuong)),
    KhauTru_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), KhauTru)),
    -- Lương thực lĩnh là cột tự động tính (Computed Column) và cũng mã hóa luôn giá trị tổng của nó
    LuongThucLinh_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), (LuongCoBan + PhuCap + Thuong - KhauTru)));
GO

-- Đóng khóa lại an toàn
CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
GO

----------xóa các cột lương chưa mã hóa-------------------
USE QuanLyNhanSu;
GO

-- xóa cột LuongThucLinh trước vì nó phụ thuộc vào các cột khác
ALTER TABLE Luong DROP COLUMN LuongThucLinh;
GO

-- xóa các cột lương, phụ cấp, thưởng và khấu trừ gốc
ALTER TABLE Luong DROP COLUMN LuongCoBan;
ALTER TABLE Luong DROP COLUMN PhuCap;
ALTER TABLE Luong DROP COLUMN Thuong;
ALTER TABLE Luong DROP COLUMN KhauTru;
GO


--------------------BẢO MẬT 3 Procudure để giam đốc xem và cập nhật lương------------------------

USE QuanLyNhanSu;
GO

-- PROCEDURE DÀNH CHO GIÁM ĐỐC: XEM DANH SÁCH VÀ GIẢI MÃ LƯƠNG

USE QuanLyNhanSu;
GO

CREATE OR ALTER PROCEDURE SP_GiamDoc_XemDanhSachNhanVien
AS
BEGIN
    
    OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu
    DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;

    SELECT 
        nv.MaNV, 
        nv.HoTen, 
        nv.Phai, 
        nv.NgaySinh, 
        nv.SoDienThoai, 
        nv.MaSoThue, 
        nv.MaPhongBan,               
        pb.TenPhongBan,             
        nv2.HoTen AS TenTruongPhong,
        vt.TenVaiTro, 
        CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongCoBan_Encrypted)) AS LuongCoBan,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.PhuCap_Encrypted)) AS PhuCap,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.Thuong_Encrypted)) AS Thuong,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.KhauTru_Encrypted)) AS KhauTru,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongThucLinh_Encrypted)) AS LuongThucLinh
    FROM NhanVien nv
    LEFT JOIN Luong l ON nv.MaNV = l.MaNV
    LEFT JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
    LEFT JOIN PhongBan pb ON nv.MaPhongBan = pb.MaPhongBan
    LEFT JOIN NhanVien nv2 ON pb.TruongPhong = nv2.MaNV;

    CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
END;
GO

-- PROCEDURE DÀNH CHO GIÁM ĐỐC: CẬP NHẬT (MÃ HÓA) LƯƠNG ĐẦY ĐỦ

CREATE OR ALTER PROCEDURE SP_GiamDoc_CapNhatLuong
    @MaNV INT,
    @LuongCoBan DECIMAL(18,2),
    @PhuCap DECIMAL(18,2),
    @Thuong DECIMAL(18,2),  
    @KhauTru DECIMAL(18,2)  
AS
BEGIN
    
    OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu
    DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;

    DECLARE @LuongThucLinh DECIMAL(18,2) = @LuongCoBan + @PhuCap + @Thuong - @KhauTru;

    IF EXISTS (SELECT 1 FROM Luong WHERE MaNV = @MaNV)
    BEGIN
        UPDATE Luong
        SET 
            LuongCoBan_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @LuongCoBan)),
            PhuCap_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @PhuCap)),
            Thuong_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @Thuong)),
            KhauTru_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @KhauTru)),
            LuongThucLinh_Encrypted = EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @LuongThucLinh))
        WHERE MaNV = @MaNV;
    END
    ELSE
    BEGIN
        INSERT INTO Luong (MaNV, Thang, Nam, LuongCoBan_Encrypted, PhuCap_Encrypted, Thuong_Encrypted, KhauTru_Encrypted, LuongThucLinh_Encrypted)
        VALUES (
            @MaNV,
            MONTH(GETDATE()),
            YEAR(GETDATE()),
            EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @LuongCoBan)),
            EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @PhuCap)),
            EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @Thuong)),
            EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @KhauTru)),
            EncryptByKey(Key_GUID('SymKey_QuanLyNhanSu'), CONVERT(VARBINARY(100), @LuongThucLinh))
        );
    END

    CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
END;
GO

--------------------BẢO MẬT  Procudure khác dùng thêm cho giám đốc------------------------
USE QuanLyNhanSu;
GO

--  SP Lấy danh sách Vai Trò
CREATE OR ALTER PROCEDURE SP_LayDanhSachVaiTro AS
BEGIN SELECT MaVaiTro, TenVaiTro FROM VaiTro END;
GO

--  SP Lấy danh sách Phòng Ban
CREATE OR ALTER PROCEDURE SP_LayDanhSachPhongBan AS
BEGIN SELECT MaPhongBan, TenPhongBan FROM PhongBan END;
GO

--  SP Lấy hình ảnh nhân viên
CREATE OR ALTER PROCEDURE SP_LayHinhAnhNhanVien @MaNV INT AS
BEGIN SELECT HinhAnh FROM NhanVien WHERE MaNV = @MaNV END;
GO

--  SP Thêm lịch sử hoạt động
CREATE OR ALTER PROCEDURE SP_ThemLichSuHoatDong 
    @MaNV INT, @TenDangNhap NVARCHAR(50), @HanhDong NVARCHAR(50), @ChiTiet NVARCHAR(MAX) 
AS
BEGIN
    INSERT INTO LichSuHoatDong (MaNV, TenDangNhap, HanhDong, ChiTiet) 
    VALUES (@MaNV, @TenDangNhap, @HanhDong, @ChiTiet)
END;
GO

--  SP Xem lịch sử (có tìm kiếm)
CREATE OR ALTER PROCEDURE SP_LayLichSuHoatDong @Keyword NVARCHAR(255) AS
BEGIN
    SELECT L.ID, L.MaNV, N.HoTen AS NhanVienLienQuan, L.TenDangNhap, L.HanhDong, L.ChiTiet, L.ThoiGian
    FROM LichSuHoatDong L LEFT JOIN NhanVien N ON L.MaNV = N.MaNV
    WHERE L.TenDangNhap LIKE '%' + @Keyword + '%' 
       OR L.HanhDong LIKE '%' + @Keyword + '%' 
       OR L.ChiTiet LIKE '%' + @Keyword + '%'
    ORDER BY L.ThoiGian DESC
END;
GO

--------------------BẢO MẬT 4 Procudure cho nhân viên không thuộc các phòng ban Tài chính, nhân sự------------------------
USE QuanLyNhanSu;
GO

CREATE OR ALTER PROCEDURE SP_NhanVien_XemDanhSachCungPhong
    @MaNVDangNhap INT
AS
BEGIN
    
    EXEC sp_set_session_context @key = N'CurrentUserID', @value = @MaNVDangNhap;

    DECLARE @MaPhongBan INT;
    SELECT @MaPhongBan = MaPhongBan FROM NhanVien WHERE MaNV = @MaNVDangNhap;

    OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu
    DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;

    SELECT 
        nv.MaNV, 
        nv.HoTen, 
        nv.Phai, 
        nv.NgaySinh, 
        nv.SoDienThoai, 
        nv.MaSoThue, 
        pb.TenPhongBan,             
        vt.TenVaiTro,
        CASE 
            WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) 
            THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongCoBan_Encrypted))
            ELSE NULL 
        END AS LuongCoBan,
        CASE 
            WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) 
            THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.PhuCap_Encrypted))
            ELSE NULL 
        END AS PhuCap,
        CASE 
            WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) 
            THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.Thuong_Encrypted))
            ELSE NULL 
        END AS Thuong,
        CASE 
            WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) 
            THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.KhauTru_Encrypted))
            ELSE NULL 
        END AS KhauTru,
        CASE 
            WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) 
            THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongThucLinh_Encrypted))
            ELSE NULL 
        END AS LuongThucLinh
    FROM NhanVien nv
    LEFT JOIN Luong l ON nv.MaNV = l.MaNV
    LEFT JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
    LEFT JOIN PhongBan pb ON nv.MaPhongBan = pb.MaPhongBan
    WHERE nv.MaPhongBan = @MaPhongBan; 
    
    CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
END;
GO

--------------------BẢO MẬT 5 Procudure cho trưởng phòng không thuộc các phòng ban Tài chính, nhân sự------------------------

USE QuanLyNhanSu;
GO

CREATE OR ALTER PROCEDURE SP_TruongPhong_XemDanhSachNhanVien
    @MaNVDangNhap INT
AS
BEGIN
    -- Gắn định danh người dùng vào SESSION_CONTEXT để kiểm soát phiên làm việc
    EXEC sp_set_session_context @key = N'CurrentUserID', @value = @MaNVDangNhap;

    
    DECLARE @MaPhongBan INT;
    SELECT @MaPhongBan = MaPhongBan FROM NhanVien WHERE MaNV = @MaNVDangNhap;

    OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu
    DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;

    --  Lọc dòng (Row-level) kết hợp giải mã toàn bộ thông tin lương của phòng ban đó
    SELECT 
        nv.MaNV, 
        nv.HoTen, 
        nv.Phai, 
        nv.NgaySinh, 
        nv.SoDienThoai, 
        nv.MaSoThue, 
        pb.TenPhongBan,             
        vt.TenVaiTro,
        
        CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongCoBan_Encrypted)) AS LuongCoBan,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.PhuCap_Encrypted)) AS PhuCap,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.Thuong_Encrypted)) AS Thuong,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.KhauTru_Encrypted)) AS KhauTru,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongThucLinh_Encrypted)) AS LuongThucLinh
    FROM NhanVien nv
    LEFT JOIN Luong l ON nv.MaNV = l.MaNV
    LEFT JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
    LEFT JOIN PhongBan pb ON nv.MaPhongBan = pb.MaPhongBan
    WHERE nv.MaPhongBan = @MaPhongBan; -- Đảm bảo Trưởng phòng chỉ xem được người thuộc phòng mình

    CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
END;
GO


--------------------BẢO MẬT 6 Procudure cho nhân viên nhân sự (thêm, cập nhật dùng chung cho nhân viên, trưởng phòng nhân sự)------------------------

-- sp xem danh sách nhân viên của nhân viên nhân sự
CREATE OR ALTER PROCEDURE SP_NhanSu_LayDanhSachNV
    @MaNVDangNhap INT
AS
BEGIN
    EXEC sp_set_session_context @key = N'CurrentUserID', @value = @MaNVDangNhap;
    OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;

    SELECT 
        nv.MaNV, nv.HoTen, nv.Phai, nv.NgaySinh, nv.SoDienThoai, 
        nv.MaSoThue, nv.HinhAnh, pb.TenPhongBan, vt.TenVaiTro,
        nd.TenDangNhap, nv.MaPhongBan, nv.MaVaiTro,
        -- LOGIC BẢO MẬT: Chỉ hiển thị lương nếu đúng là nhân viên đang đăng nhập
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongCoBan_Encrypted)) ELSE NULL END AS LuongCoBan,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.PhuCap_Encrypted)) ELSE NULL END AS PhuCap,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.Thuong_Encrypted)) ELSE NULL END AS Thuong,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.KhauTru_Encrypted)) ELSE NULL END AS KhauTru,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongThucLinh_Encrypted)) ELSE NULL END AS LuongThucLinh
    FROM NhanVien nv
    JOIN PhongBan pb ON nv.MaPhongBan = pb.MaPhongBan
    JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
    JOIN NguoiDungDangNhap nd ON nv.MaNV = nd.MaNV
    LEFT JOIN Luong l ON nv.MaNV = l.MaNV
    WHERE pb.TenPhongBan <> N'Phòng Nhân Sự'; 

    CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
END;
GO

-- SP thêm nhân viên mới
CREATE OR ALTER PROCEDURE SP_NhanSu_ThemNV
    @HoTen NVARCHAR(100), @Phai NVARCHAR(10), @NgaySinh DATE, @SDT NVARCHAR(15),
    @MST NVARCHAR(20), @MaPB INT, @MaVT INT, @HinhAnh NVARCHAR(255),
    @TenDN NVARCHAR(50), @MatKhau NVARCHAR(255), @MaNVThucHien INT
AS
BEGIN
    -- Check tên đăng nhập tồn tại
    IF EXISTS (SELECT 1 FROM NguoiDungDangNhap WHERE TenDangNhap = @TenDN)
    BEGIN
        RAISERROR(N'Tên đăng nhập đã tồn tại!', 16, 1);
        RETURN;
    END

    -- Check Trưởng phòng (Nếu vai trò là Trưởng phòng - MaVT = 2 hoặc 4)
    IF EXISTS (SELECT 1 FROM VaiTro WHERE MaVaiTro = @MaVT AND TenVaiTro LIKE N'%Trưởng phòng%')
    BEGIN
        IF EXISTS (SELECT 1 FROM NhanVien WHERE MaPhongBan = @MaPB AND MaVaiTro = @MaVT)
        BEGIN
            RAISERROR(N'Phòng ban này đã có Trưởng phòng!', 16, 1);
            RETURN;
        END
    END

    -- Thực hiện thêm vào bảng NhanVien
    INSERT INTO NhanVien (HoTen, Phai, NgaySinh, SoDienThoai, MaSoThue, MaPhongBan, MaVaiTro, HinhAnh)
    VALUES (@HoTen, @Phai, @NgaySinh, @SDT, @MST, @MaPB, @MaVT, @HinhAnh);

    DECLARE @NewMaNV INT = SCOPE_IDENTITY();

    -- Thực hiện thêm vào bảng NguoiDungDangNhap (Băm mật khẩu)
    INSERT INTO NguoiDungDangNhap (TenDangNhap, MatKhauHash, MaNV, MaVaiTro)
    VALUES (@TenDN, HASHBYTES('SHA2_256', @MatKhau), @NewMaNV, @MaVT);

    -- Lưu lịch sử
    DECLARE @TenNV NVARCHAR(100) = (SELECT HoTen FROM NhanVien WHERE MaNV = @MaNVThucHien);
    INSERT INTO LichSuHoatDongNhanSu (MaNVThucHien, TenNVThucHien, ThaoTac, TacDongLenNV, ChiTiet)
    VALUES (@MaNVThucHien, @TenNV, N'THEM', @NewMaNV, N'Thêm nhân viên mới: ' + @HoTen);
END;
GO

-- SP cập nhật nhân viên của nhân viên nhân sự.
USE QuanLyNhanSu;
GO

CREATE OR ALTER PROCEDURE SP_NhanSu_CapNhatNV
    @MaNV INT,
    @HoTen NVARCHAR(100), @Phai NVARCHAR(10), @NgaySinh DATE, @SDT NVARCHAR(15),
    @MST NVARCHAR(20), @MaPB INT, @MaVT INT, @HinhAnh NVARCHAR(255),
    @TenDN NVARCHAR(50), @MatKhau NVARCHAR(255), @MaNVThucHien INT
AS
BEGIN
    --  Kiểm tra logic Trưởng phòng khi thay đổi Vai trò/Phòng ban
    IF EXISTS (SELECT 1 FROM VaiTro WHERE MaVaiTro = @MaVT AND TenVaiTro LIKE N'%Trưởng phòng%')
    BEGIN
        IF EXISTS (SELECT 1 FROM NhanVien WHERE MaPhongBan = @MaPB AND MaVaiTro = @MaVT AND MaNV <> @MaNV)
        BEGIN
            RAISERROR(N'Phòng ban này đã có Trưởng phòng!', 16, 1);
            RETURN;
        END
    END

    -- Cập nhật bảng NhanVien
    UPDATE NhanVien SET 
        HoTen = @HoTen, Phai = @Phai, NgaySinh = @NgaySinh, SoDienThoai = @SDT,
        MaSoThue = @MST, MaPhongBan = @MaPB, MaVaiTro = @MaVT, HinhAnh = @HinhAnh
    WHERE MaNV = @MaNV;

    -- Cập nhật bảng NguoiDungDangNhap (Nếu có đổi mật khẩu thì băm lại)
    UPDATE NguoiDungDangNhap SET 
        MaVaiTro = @MaVT,
        MatKhauHash = CASE WHEN @MatKhau <> '' THEN HASHBYTES('SHA2_256', @MatKhau) ELSE MatKhauHash END
    WHERE MaNV = @MaNV;

    -- Ghi lịch sử hoạt động
    DECLARE @TenNVThucHien NVARCHAR(100) = (SELECT HoTen FROM NhanVien WHERE MaNV = @MaNVThucHien);
    INSERT INTO LichSuHoatDongNhanSu (MaNVThucHien, TenNVThucHien, ThaoTac, TacDongLenNV, ChiTiet)
    VALUES (@MaNVThucHien, @TenNVThucHien, N'SUA', @MaNV, N'Cập nhật thông tin nhân viên: ' + @HoTen);
END;
GO

--------------------BẢO MẬT 7 Procudure cho trưởng phòng nhân sự------------------------

USE QuanLyNhanSu;
GO

-- lấy danh sách NV
CREATE OR ALTER PROCEDURE SP_TruongPhongNhanSu_LayDanhSachNV
    @MaNVDangNhap INT
AS
BEGIN
    EXEC sp_set_session_context @key = N'CurrentUserID', @value = @MaNVDangNhap;
    OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;

    SELECT
        nv.MaNV, nv.HoTen, nv.Phai, nv.NgaySinh, nv.SoDienThoai, 
        nv.MaSoThue, nv.HinhAnh, pb.TenPhongBan, vt.TenVaiTro,
        nd.TenDangNhap, nv.MaPhongBan, nv.MaVaiTro,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongCoBan_Encrypted)) ELSE NULL END AS LuongCoBan,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.PhuCap_Encrypted)) ELSE NULL END AS PhuCap,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.Thuong_Encrypted)) ELSE NULL END AS Thuong,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.KhauTru_Encrypted)) ELSE NULL END AS KhauTru,
        CASE WHEN nv.MaNV = CAST(SESSION_CONTEXT(N'CurrentUserID') AS INT) THEN CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongThucLinh_Encrypted)) ELSE NULL END AS LuongThucLinh
    FROM NhanVien nv
    JOIN PhongBan pb ON nv.MaPhongBan = pb.MaPhongBan
    JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
    JOIN NguoiDungDangNhap nd ON nv.MaNV = nd.MaNV
    LEFT JOIN Luong l ON nv.MaNV = l.MaNV;

    CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
END;
GO

-- xem lịch sử
CREATE OR ALTER PROCEDURE SP_TruongPhongNhanSu_XemLichSu
    @Keyword NVARCHAR(255)
AS
BEGIN
    SELECT ID, ThoiGian, MaNVThucHien, TenNVThucHien, ThaoTac, TacDongLenNV, ChiTiet
    FROM LichSuHoatDongNhanSu
    WHERE CAST(MaNVThucHien AS NVARCHAR) LIKE '%' + @Keyword + '%'
       OR TenNVThucHien LIKE N'%' + @Keyword + '%'
       OR ThaoTac LIKE N'%' + @Keyword + '%'
       OR ChiTiet LIKE N'%' + @Keyword + '%'
    ORDER BY ThoiGian DESC
END;
GO

--------------------BẢO MẬT 8 Procudure cho nhân viên tài vụ------------------------
USE QuanLyNhanSu;
GO

CREATE OR ALTER PROCEDURE SP_TaiVu_XemDanhSachNhanVien
    @MaNVDangNhap INT
AS
BEGIN
    -- Gắn SESSION_CONTEXT để lưu vết phiên làm việc
    EXEC sp_set_session_context @key = N'CurrentUserID', @value = @MaNVDangNhap;

    -- Lấy mã phòng ban của nhân viên tài vụ đang đăng nhập
    DECLARE @MaPhongBan INT;
    SELECT @MaPhongBan = MaPhongBan FROM NhanVien WHERE MaNV = @MaNVDangNhap;

    --  Mở khóa Symmetric Key để giải mã lương
    OPEN SYMMETRIC KEY SymKey_QuanLyNhanSu
    DECRYPTION BY CERTIFICATE Cert_QuanLyNhanSu;

    --  Truy vấn với Data Masking (Che giấu dữ liệu)
    SELECT 
        nv.MaNV, 
        nv.MaSoThue, 

        
        CASE WHEN nv.MaPhongBan = @MaPhongBan THEN nv.HoTen ELSE NULL END AS HoTen,
        CASE WHEN nv.MaPhongBan = @MaPhongBan THEN nv.Phai ELSE NULL END AS Phai,
        CASE WHEN nv.MaPhongBan = @MaPhongBan THEN CONVERT(VARCHAR(10), nv.NgaySinh, 103) ELSE NULL END AS NgaySinh,
        CASE WHEN nv.MaPhongBan = @MaPhongBan THEN nv.SoDienThoai ELSE NULL END AS SoDienThoai,
        CASE WHEN nv.MaPhongBan = @MaPhongBan THEN pb.TenPhongBan ELSE NULL END AS TenPhongBan,
        CASE WHEN nv.MaPhongBan = @MaPhongBan THEN vt.TenVaiTro ELSE NULL END AS TenVaiTro,
        CASE WHEN nv.MaPhongBan = @MaPhongBan THEN nv.HinhAnh ELSE NULL END AS HinhAnh,

       
        CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongCoBan_Encrypted)) AS LuongCoBan,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.PhuCap_Encrypted)) AS PhuCap,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.Thuong_Encrypted)) AS Thuong,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.KhauTru_Encrypted)) AS KhauTru,
        CONVERT(DECIMAL(18,2), DecryptByKey(l.LuongThucLinh_Encrypted)) AS LuongThucLinh
    FROM NhanVien nv
    LEFT JOIN Luong l ON nv.MaNV = l.MaNV
    LEFT JOIN VaiTro vt ON nv.MaVaiTro = vt.MaVaiTro
    LEFT JOIN PhongBan pb ON nv.MaPhongBan = pb.MaPhongBan;

    CLOSE SYMMETRIC KEY SymKey_QuanLyNhanSu;
END;
GO