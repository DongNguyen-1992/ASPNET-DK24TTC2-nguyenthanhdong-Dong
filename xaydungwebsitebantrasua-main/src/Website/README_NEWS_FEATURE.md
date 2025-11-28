# Hướng Dẫn Sử Dụng Tính Năng Quản Trị Tin Tức Mới

Hệ thống quản trị tin tức đã được nâng cấp toàn diện với giao diện hiện đại và nhiều tính năng mới. Dưới đây là hướng dẫn sử dụng:

## 1. Truy cập
- Đăng nhập vào trang quản trị Admin.
- Tại thanh menu bên trái, tìm mục **"Tin Tức"**.
- Menu này bao gồm 3 mục con:
  - **Danh sách tin tức**: Quản lý chung tất cả bài viết.
  - **Thêm tin tức mới**: Tạo bài viết mới.
  - **Thống kê tin tức**: Xem báo cáo và biểu đồ tăng trưởng.

## 2. Các Tính Năng Chính

### A. Danh Sách Tin Tức (Index)
- **Thống kê nhanh**: Xem ngay tổng số bài viết, số bài trong tháng và trong ngày ở đầu trang.
- **Tìm kiếm & Lọc**:
  - Tìm kiếm theo tiêu đề hoặc mô tả.
  - Lọc theo khoảng thời gian (Từ ngày - Đến ngày).
  - Sắp xếp theo tên hoặc ngày đăng.
- **Thao tác**: Xem chi tiết, Chỉnh sửa, Xóa nhanh chóng từ bảng danh sách.

### B. Thêm Mới & Chỉnh Sửa (Create/Edit)
- **Upload ảnh**:
  - Kéo thả hoặc click để chọn ảnh đại diện.
  - Xem trước ảnh (Preview) ngay lập tức trước khi lưu.
- **Soạn thảo nội dung**:
  - Nhập tiêu đề, mô tả ngắn và nội dung chi tiết.
  - Hệ thống tự động đếm ký tự giúp bạn tối ưu độ dài tiêu đề.
- **Validation**: Hệ thống sẽ nhắc nhở nếu bạn quên nhập các thông tin bắt buộc.

### C. Thống Kê (Statistics)
- Xem biểu đồ đường (Line Chart) thể hiện xu hướng đăng bài trong 12 tháng gần nhất.
- Các thẻ thống kê tổng quan giúp nắm bắt tình hình nội dung nhanh chóng.

### D. Xóa Tin Tức (Delete)
- Giao diện cảnh báo rõ ràng để tránh xóa nhầm.
- Hiển thị trước nội dung bài viết sắp xóa.
- **Lưu ý**: Khi xóa bài viết, hình ảnh đại diện đi kèm cũng sẽ được xóa khỏi server để tiết kiệm dung lượng.

## 3. Lưu Ý Kỹ Thuật
- Hình ảnh bài viết được lưu tại thư mục: `/img/Item/`.
- Tên file ảnh sẽ được tự động đổi tên theo thời gian upload để tránh trùng lặp (ví dụ: `blog_20231127153000.jpg`).
- Các thư viện sử dụng:
  - **Chart.js**: Vẽ biểu đồ thống kê.
  - **FontAwesome**: Icon giao diện.
  - **DataTables**: Bảng dữ liệu nâng cao.

Chúc bạn có trải nghiệm quản trị nội dung hiệu quả!
