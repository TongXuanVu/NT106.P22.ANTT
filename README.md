# LAN SPY - Ứng dụng Giám sát & Quản lý thiết bị trong mạng LAN

**LAN SPY** là một ứng dụng máy tính (Desktop Application) chạy trên hệ điều hành Windows, được xây dựng trên nền tảng **C#** và giao diện **WPF (Windows Presentation Foundation)**. Dự án được thiết kế nhằm mục đích quét, giám sát, ghi nhận nhật ký và cảnh báo các hoạt động của các thiết bị kết nối trong mạng nội bộ (LAN). 

Ứng dụng này cực kỳ hữu ích cho việc quản trị mạng gia đình hoặc văn phòng nhỏ, giúp phát hiện sớm các thiết bị lạ xâm nhập trái phép, theo dõi tình trạng trực tuyến (Online/Offline) của các máy móc trong mạng, đo đạc lưu lượng băng thông thời gian thực và quản lý lịch sử quét một cách trực quan thông qua cơ sở dữ liệu MySQL tập trung.

---

## 🌟 Các tính năng nổi bật

### 1. 🧭 Dashboard - Tổng quan mạng
* **Thông tin mạng chi tiết**: Hiển thị tên Wifi hiện tại (SSID), địa chỉ MAC bộ phát (BSSID), tốc độ liên kết mạng (Wifi Speed), độ trễ (Latency) tới DNS.
* **Thống kê trạng thái**: Hiển thị tổng số thiết bị trong mạng, số thiết bị đang Online, và số lượng cảnh báo thiết bị lạ phát hiện được.
* **Biểu đồ băng thông real-time**: Sử dụng thư viện `LiveCharts` vẽ biểu đồ trực quan, cập nhật liên tục tốc độ Upload và Download của card mạng theo thời gian thực (đơn vị KB/s).
* **Đồng bộ thời gian**: Hiển thị chính xác mốc thời gian của lượt quét mạng LAN gần nhất.

### 2. 🔍 Scanner - Quét mạng LAN chuyên sâu
* **Quét mạng đa luồng tự động**: Tự động xác định địa chỉ IP và Subnet Mask của card mạng (Wifi hoặc Ethernet) đang hoạt động, sau đó gửi các gói tin ICMP Ping song song đến toàn bộ các IP khả dụng trong dải mạng (ví dụ: `/24`).
* **Kiểm soát hiệu năng**: Sử dụng `SemaphoreSlim` để giới hạn số luồng quét đồng thời dựa trên thông số `DeviceThreshold` được thiết lập trong Cài đặt, tránh làm nghẽn mạng hoặc quá tải CPU.
* **Phân giải chi tiết thiết bị**:
  * **Địa chỉ MAC**: Lấy trực tiếp từ bảng ARP hệ thống bằng cách thực thi lệnh hệ thống `arp -a`.
  * **Nhà sản xuất (Vendor/Manufacturer)**: Tra cứu tiền tố OUI (6 ký tự đầu của MAC) thông qua tệp cơ sở dữ liệu cục bộ `oui.csv` (ví dụ: Apple, Intel, TP-Link, Samsung,... ).
  * **Tên máy (Hostname)**: Giải quyết bằng hai bước dự phòng: truy vấn DNS (`Dns.GetHostEntryAsync`) và truy vấn NetBIOS qua lệnh `nbtstat -A` của Windows.
  * **Phân loại thiết bị thông minh**: Dựa trên tên nhà sản xuất để phán đoán loại thiết bị (Laptop/Desktop, Smartphone/Tablet, Home Router, IP Camera, Smart TV, Thiết bị IoT, Máy in, Máy ảo...).

### 3. ⚠️ Alerts - Hệ thống Cảnh báo thời gian thực
* **Cảnh báo tức thời (Popup Window)**: Hiển thị hộp thoại popup thông minh ở góc dưới bên phải màn hình khi phát hiện:
  * Một thiết bị mới kết nối vào mạng LAN.
  * Một thiết bị đã đăng ký bị mất kết nối đột ngột (Offline).
* **Quản lý danh sách cảnh báo**: Lưu lại lịch sử các cảnh báo trong phiên làm việc hiện tại, phân loại mức độ (Cảnh báo thường, Thiết bị lạ...).
* **Dọn dẹp & Xuất dữ liệu**: Hỗ trợ xóa cảnh báo cũ (theo thời gian tùy chỉnh) và xuất danh sách cảnh báo ra file báo cáo Excel (`.xlsx`) chuyên nghiệp bằng thư viện `ClosedXML`.

### 4. 📊 Logs - Nhật ký và Truy vấn lịch sử
* **Lưu trữ dữ liệu tập trung**: Mọi thiết bị được quét thấy sẽ tự động được lưu trữ/cập nhật vào bảng `scanner_devices` trên MySQL Server.
* **Tìm kiếm & Bộ lọc nâng cao**: Tìm kiếm thiết bị theo địa chỉ IP, địa chỉ MAC, tên thiết bị, hoặc lọc theo khoảng thời gian quét (Từ ngày - Đến ngày).
* **Xuất báo cáo Excel**: Hỗ trợ xuất toàn bộ hoặc danh sách thiết bị đã được lọc ra file Excel báo cáo (`LogsReport.xlsx`).
* **Quản lý dung lượng**: Cho phép quản trị viên xóa toàn bộ lịch sử log để làm nhẹ cơ sở dữ liệu khi cần thiết.

### 5. ⚙️ Setting - Cấu hình linh hoạt
* **Khởi động cùng Windows**: Tích hợp ghi/xóa Registry Run (`SOFTWARE\Microsoft\Windows\CurrentVersion\Run`) để ứng dụng tự chạy ngầm khi mở máy.
* **Bật/Tắt các loại thông báo**: Tùy chỉnh nhận cảnh báo khi có thiết bị mới, thiết bị ngắt kết nối, hoặc cảnh báo khi gặp địa chỉ MAC lạ chưa xác định.
* **Điều chỉnh thông số quét**:
  * Ngưỡng thiết bị (Device Threshold - tối đa 1000): Giới hạn số lượng thiết bị/luồng quét đồng thời.
  * Chu kỳ tự động quét (Scan Interval - từ 30s đến 3600s): Thiết lập khoảng thời gian tự động quét lại mạng LAN.
* **Lưu trữ cục bộ**: Mọi cấu hình cài đặt được lưu trữ dưới dạng JSON trong file `settings.json` tại thư mục chạy chương trình.

---

## 🛠️ Công nghệ & Thư viện sử dụng

Dự án được xây dựng dựa trên các công nghệ và thư viện phổ biến của hệ sinh thái .NET:
* **Framework**: .NET Framework (WPF)
* **LiveCharts / LiveCharts.Wpf** (v0.9.7): Thư viện vẽ biểu đồ thời gian thực trực quan và mượt mà cho WPF.
* **ClosedXML** (v0.102.1): Thư viện thao tác với tệp Microsoft Excel OpenXML (.xlsx) nhanh chóng mà không cần cài đặt phần mềm Microsoft Excel trên máy tính.
* **MySql.Data** (v8.0.33 hoặc tương thích): Thư viện kết nối và làm việc với cơ sở dữ liệu MySQL.
* **System.Text.Json**: Thư viện xử lý tuần tự hóa (serialization) cài đặt ứng dụng thành định dạng JSON.

---

## 📂 Cấu trúc dự án

Dưới đây là sơ đồ tổ chức mã nguồn của dự án trong thư mục `LANSPYproject`:

```text
LANSPYproject/
├── App.xaml / App.xaml.cs               # Điểm khởi đầu ứng dụng, thiết lập cửa sổ chạy chính (Lanspy.xaml)
├── AssemblyInfo.cs                      # Thông tin định danh và mô tả assembly của dự án
├── Lanspy.xaml / Lanspy.xaml.cs          # Giao diện chính (MainWindow), quản lý Sidebar điều hướng và trao đổi dữ liệu giữa các trang
│
├── Dashboard.xaml / Dashboard.xaml.cs   # Giao diện Trang chủ, biểu đồ băng thông LiveCharts và thông số Wifi cơ bản
├── Scanner.xaml / Scanner.xaml.cs       # Giao diện và Logic quét mạng LAN (Ping, ARP nbtstat, tra cứu OUI và ghi MySQL DB)
├── Logs.xaml / Logs.xaml.cs             # Giao diện Nhật ký lịch sử quét, chức năng tìm kiếm lọc dữ liệu và xuất Excel
├── Alerts.xaml / Alerts.xaml.cs         # Giao diện quản lý danh sách cảnh báo, xóa cảnh báo và xuất Excel cảnh báo
├── Setting.xaml / Setting.xaml.cs       # Giao diện Cài đặt hệ thống, lưu JSON và quản lý Windows Registry Startup
│
├── NotificationPopup.xaml / .cs         # Cửa sổ popup nhỏ hiển thị thông báo thiết bị Online/Offline ở góc màn hình
│
├── NetworkConnectionDevice.cs           # Lớp model đại diện cho thiết bị kết nối mạng (dùng cho Dashboard)
├── SettingsData.cs                      # Lớp model chứa cấu hình cài đặt hệ thống và logic Validate/Reset/Clone
│
├── LANSPYproject.csproj                 # Tệp quản lý dự án C# (MSBuild)
└── oui.csv                              # Cơ sở dữ liệu MAC OUI ánh xạ tiền tố MAC sang tên nhà sản xuất (cần đặt cùng thư mục build)
```

---

## ⚙️ Hướng dẫn cài đặt & Cấu hình hệ thống

### 1. Yêu cầu môi trường
* **Hệ điều hành**: Windows 10 / Windows 11.
* **IDE**: Microsoft Visual Studio 2022 (hoặc phiên bản mới hơn) hỗ trợ phát triển ứng dụng .NET Desktop Development.
* **Cơ sở dữ liệu**: MySQL Server (phiên bản 5.7 hoặc 8.0 trở lên).

### 2. Cài đặt Cơ sở dữ liệu (MySQL)
Chương trình được thiết kế tự động khởi tạo database và bảng dữ liệu khi mở nếu kết nối thành công. Tuy nhiên, bạn nên cấu hình kết nối của riêng mình.

Mặc định, ứng dụng đang kết nối tới một database MySQL chạy thử nghiệm trên dịch vụ đám mây Railway:
* **Host**: `yamabiko.proxy.rlwy.net`
* **Port**: `17335`
* **User**: `root`
* **Password**: `KRIDiTbBoaPMfoCjFVhzgVcliVcApIbP`
* **Database**: `lan_spy_db`

**Cấu trúc SQL khởi tạo bảng dữ liệu:**
```sql
CREATE DATABASE IF NOT EXISTS lan_spy_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE lan_spy_db;

CREATE TABLE IF NOT EXISTS scanner_devices (
    id INT AUTO_INCREMENT PRIMARY KEY,
    ip VARCHAR(64),
    mac VARCHAR(64),
    name VARCHAR(255),
    scan_time DATETIME,
    status VARCHAR(32),
    wifi_name VARCHAR(255)
);
```

> [!WARNING]
> **Khuyến nghị bảo mật**: Thông tin tài khoản quản trị MySQL (`root`) hiện tại đang được ghi cứng trong mã nguồn (`Scanner.xaml.cs` và `Logs.xaml.cs`). Để đảm bảo an toàn thông tin, hãy cài đặt MySQL cục bộ hoặc trên máy chủ riêng của bạn, sau đó thay đổi lại biến `connectionString` trong mã nguồn trước khi biên dịch và triển khai ứng dụng.

### 3. File cơ sở dữ liệu OUI (`oui.csv`)
Tệp `oui.csv` là tệp dữ liệu chứa danh sách ánh xạ từ mã nhà sản xuất MAC (OUI - Organizationally Unique Identifier) sang tên các tập đoàn/công ty sản xuất phần cứng mạng (như Intel, Apple, Cisco, Samsung, v.v.).

* **Vị trí**: Đặt tệp `oui.csv` cùng thư mục chứa tệp thực thi `.exe` của chương trình (Ví dụ: `LANSPYproject/bin/Debug/` hoặc `LANSPYproject/bin/Release/`).
* **Định dạng dữ liệu tệp CSV**: Tệp phải có tiêu đề ở dòng đầu tiên, bắt đầu từ dòng thứ 2 phân tách bởi dấu phẩy, trong đó cột 2 (chỉ số 1) chứa tiền tố OUI (6 ký tự Hex viết hoa, ví dụ `E00630`) và cột 3 (chỉ số 2) chứa tên tổ chức sản xuất.
  * *Ví dụ cấu trúc*:
    ```csv
    Registry,Assignment,Organization Name,Address
    MA-L,E00630,TP-LINK TECHNOLOGIES CO.,LTD.,Shenzhen
    MA-L,F8FFC2,Apple Inc.,Cupertino CA
    ```

### 4. Build và Chạy ứng dụng
1. Mở tệp `LANSPYproject.sln` bằng Visual Studio.
2. Visual Studio sẽ tự động khôi phục các NuGet Packages thiếu hụt (`LiveCharts`, `ClosedXML`, `MySql.Data`). Nếu không, click chuột phải vào **Solution** chọn **Restore NuGet Packages**.
3. Đảm bảo bạn đã đặt file `oui.csv` vào thư mục đầu ra `bin/Debug` hoặc `bin/Release`.
4. Nhấn **F5** hoặc nút **Start** để biên dịch và khởi chạy chương trình.

---

## 🔍 Nguyên lý hoạt động chi tiết

### 1. Thu thập địa chỉ IP của mạng LAN
Ứng dụng duyệt qua tất cả các Network Interface trong máy tính. Nó ưu tiên lấy IP Address và Subnet Mask của mạng không dây (Wireless80211 - Wifi) trước, nếu không có sẽ lấy của mạng có dây (Ethernet).
Từ IP và Subnet Mask, chương trình tính ra địa chỉ mạng (Network Address) và quét tất cả các máy host từ `.1` đến `.254` (với Subnet `/24`).

### 2. Gửi Ping đa luồng (ICMP Echo Request)
Sử dụng lớp `System.Net.NetworkInformation.Ping` để thực hiện ping bất đồng bộ (`SendPingAsync`).
* Nếu nhận được phản hồi thành công (`IPStatus.Success`), máy host được xác định là đang **Online (Trực tuyến)**.
* Nếu ping thất bại hoặc timeout (500ms), thiết bị sẽ được đánh dấu là **Offline (Ngoại tuyến)**.

### 3. Lấy địa chỉ MAC qua ARP
Vì C# thuần không hỗ trợ trực tiếp lấy MAC của thiết bị mạng khác ngoài local subnet một cách dễ dàng mà không qua thư viện ngoài, LAN SPY sử dụng lệnh `arp -a [IP]` thông qua Process CommandLine để lấy thông tin trực tiếp từ bảng ARP cache của hệ điều hành Windows, giúp tối ưu tốc độ và không cần cài thêm thư viện WinPcap/Npcap.

### 4. Giải quyết Hostname dự phòng
Khi phát hiện máy online, chương trình gửi yêu cầu phân giải tên máy:
* **DNS Resolution**: Gọi `Dns.GetHostEntryAsync(ip)`.
* **NetBIOS Resolution**: Nếu DNS thất bại (thường xảy ra với các thiết bị gia đình không đăng ký DNS), chương trình chạy lệnh `nbtstat -A [IP]` và bóc tách kết quả để tìm tên máy trạm Windows hoặc thiết bị samba tương thích.

---

## 🔒 Cấp phép và Đóng góp
Dự án được xây dựng và phát triển phục vụ mục đích học tập và nghiên cứu môn học Truyền thông và mạng máy tính (NT106) tại trường Đại học Công nghệ Thông tin - ĐHQG TP.HCM (UIT).

Mọi ý kiến đóng góp hoặc báo lỗi xin vui lòng mở Issue hoặc gửi Pull Request qua kho lưu trữ Git này.
