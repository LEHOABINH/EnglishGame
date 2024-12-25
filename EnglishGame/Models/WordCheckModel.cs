using System.Collections.Generic;

namespace EnglishGame.Models
{
    public class WordCheckModel
    {
        public string InputWord { get; set; }
        public bool IsEnglish { get; set; }
        public List<string> EnglishWords { get; set; } = new List<string>(); // Khởi tạo danh sách rỗng
        public int CorrectWordCount { get; set; } // Đếm số từ đã điền đúng
        public List<string> CorrectWords { get; set; } = new List<string>(); // Danh sách từ đã điền đúng
        public string UsedWordMessage { get; set; } = ""; // Thông điệp về từ đã sử dụng
    }
}
