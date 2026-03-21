Câu 1:
- Đã cải tiến method SetView() trong file Item.cs 
- Đã cải tiến file BoardController.cs dòng 177
- Trong file GameManager.cs tôi đã bỏ m_boardController.Update() vì trong BoardController.cs đã có hàm Update();

Câu 5:
## Overview
Nhìn chung, project có thể chạy được và luồng gameplay tương đối rõ ràng. Tuy nhiên, về mặt tổ chức code và thiết kế kiến trúc thì vẫn còn một số điểm có thể cải thiện để dễ bảo trì, dễ mở rộng và phù hợp hơn với các nguyên tắc thiết kế phần mềm.
## 2 Ưu điểm
-Luồng gameplay chính tương đối dễ hiểu: "GameManager" quản lý trạng thái game, "BoardController" xử lý gameplay, còn "Board" quản lý dữ liệu bàn cờ
- Cách tổ chức bàn cờ bằng mảng 2 chiều "Cell[,]" giúp việc xử lý match, swap, fill và kiểm tra hàng xóm khá trực quan
- Phần điều kiện level được tách thành "LevelCondition", "LevelMoves", "LevelTime", đây là hướng tổ chức tốt để có thể mở rộng thêm các loại level khác
- Các panel UI được tách thành nhiều class riêng như "UIPanelMain", "UIPanelGame", "UIPanelPause", "UIPanelGameOver", giúp phần giao diện dễ đọc hơn
- Việc sử dụng "ScriptableObject" cho dữ liệu skin item là hướng tốt để tách dữ liệu cấu hình khỏi code
- Sử dụng tốt observer pattern 
 ## 3 Nhược điểm
- "BoardController" cũng đang ôm quá nhiều logic input, xử lý drag, swap, match, collapse, refill, shuffle và hint. Điều này làm class trở nên dài, khó bảo trì và khó mở rộng
- "Board" đang vừa giữ dữ liệu bàn cờ, vừa tạo object hiển thị trong scene. Điều này làm logic dữ liệu và logic hiển thị bị trộn với nhau
- Một số chỗ trong gameplay loop vẫn có thể tối ưu thêm để giảm cấp phát bộ nhớ và giảm công việc lặp lại trong Update
## 4 Đánh giá theo SOLID
### 4.1 Single Responsibility Principle (SRP)
- "IMenu" và các panel UI cũng tương đối rõ trách nhiệm
- "BoardController" vi phạm vì xử lý quá nhiều phần khác nhau của gameplay
- "Board" chưa tách rõ giữa phần dữ liệu logic và phần tạo object hiển thị
### 4.2 Open/Closed Principle (OCP)
- Khi thêm loại level mới, vẫn cần sửa trực tiếp trong "GameManager"
- Khi thêm state hoặc panel mới, "UIMainManager" vẫn phải sửa "switch/case"
- Một số logic trong "Board" đang hard-code theo pattern hiện tại nên chưa thật sự mở rộng tốt
## 5 Đề xuất cải
- Nên tách "BoardController" thành các phần nhỏ hơn như:
  - xử lý input
  - xử lý hint
  - xử lý resolve board sau khi swap  
  Việc này sẽ giúp class gọn hơn, rõ trách nhiệm hơn và dễ mở rộng hơn
- Nên giảm các runtime lookup như "FindObjectOfType", "Resources.Load", "GetComponent" ở những đoạn chạy thường xuyên
  Những chỗ này có thể được thay bằng cache reference hoặc khởi tạo sẵn từ đầu để tối ưu hơn.
- Nên tiếp tục sử dụng `ScriptableObject` cho các dữ liệu cấu hình như:
  - skin item
  - level data
  - gameplay config  
  Cách này giúp tách dữ liệu khỏi code và dễ thay đổi nội dung hơn

- Nên áp dụng Observer Pattern nhất quán hơn ở các luồng UI và gameplay phụ trợ  
  Hiện tại project đã dùng event ở một số phần như state game, điều kiện level và số lượt đi, đây là hướng tốt để giảm coupling giữa các hệ thống

- Nên unsubscribe đầy đủ các event trong "OnDestroy()" hoặc khi object không còn sử dụng nữa để tránh giữ reference ngoài ý muốn

- Project hiện chưa sử dụng Singleton Pattern đúng nghĩa
  ## 6 Một số vấn đề khác
 - Trong GameManager.LoadLevel, mode TIMER đang gọi "m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), this);" , đáng ra phải là m_gameSettings.LevelTime, không phải LevelMoves ,hiện tại timer mode sẽ dùng nhầm số moves làm thời gian
 - eSwipeDirection hiện chưa tham gia luồng nào
