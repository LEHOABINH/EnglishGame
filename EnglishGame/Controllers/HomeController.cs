using EnglishGame.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EnglishGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _filePath = @"C:\Users\Admin\Documents\Code C#\EnglishGame\words.txt"; // Đường dẫn đến tệp
        private static List<string> _usedWords = new List<string>(); // Danh sách tĩnh để lưu từ đã sử dụng
        private static List<string> _correctWords = new List<string>(); // Danh sách tĩnh để lưu từ đúng

        // Hiển thị trang chính
        public IActionResult Index()
        {
            var model = new WordCheckModel
            {
                EnglishWords = LoadEnglishWords(), // Đọc từ tiếng Anh từ tệp
                CorrectWordCount = _correctWords.Count, // Lấy số lượng từ đúng từ danh sách tĩnh
                CorrectWords = new List<string>(_correctWords), // Chuyển danh sách tĩnh sang danh sách mới
                UsedWordMessage = string.Empty // Đặt thông điệp rỗng ban đầu
            };
            return View(model);
        }

        // Xử lý yêu cầu kiểm tra từ
        [HttpPost]
        public IActionResult CheckWord(WordCheckModel model)
        {
            model.EnglishWords = LoadEnglishWords(); // Đọc từ tiếng Anh từ tệp
            model.IsEnglish = CheckIfEnglishWord(model.InputWord, model.EnglishWords); // Kiểm tra từ

            // Kiểm tra xem từ đã được sử dụng chưa
            var trimmedWord = model.InputWord.Trim().ToLower();

            // Cập nhật thông điệp đã sử dụng nếu từ đã được sử dụng
            if (_usedWords.Contains(trimmedWord))
            {
                model.UsedWordMessage = $"Từ \"{model.InputWord}\" đã được sử dụng."; // Thông báo từ đã sử dụng
            }
            else
            {
                // Thêm từ mới vào danh sách
                _usedWords.Add(trimmedWord);
                model.UsedWordMessage = string.Empty; // Đặt thông điệp về từ đã sử dụng là rỗng

                // Cập nhật danh sách các từ đã điền đúng chỉ khi từ chưa được sử dụng
                if (model.IsEnglish)
                {
                    if (!_correctWords.Contains(trimmedWord)) // Kiểm tra xem từ đã đúng chưa
                    {
                        _correctWords.Add(trimmedWord); // Thêm từ đã điền đúng vào danh sách tĩnh
                    }
                }
            }

            // Giữ lại danh sách từ đúng cho lần hiển thị tiếp theo
            model.CorrectWords = new List<string>(_correctWords); // Đảm bảo danh sách từ đúng được cập nhật
            model.CorrectWordCount = model.CorrectWords.Count; // Cập nhật số lượng từ đúng

            // Kiểm tra tính hợp lệ của mô hình
            if (!ModelState.IsValid)
            {
                // Không thay đổi model.CorrectWords và model.CorrectWordCount nếu có lỗi
                return View("Index", model); // Trả về view với mô hình có lỗi
            }

            return View("Index", model); // Trả về view với mô hình đã xử lý
        }


        // Phương thức đọc từ tiếng Anh từ tệp
        private List<string> LoadEnglishWords()
        {
            List<string> words = new List<string>();
            try
            {
                if (System.IO.File.Exists(_filePath)) // Đảm bảo dùng namespace System.IO
                {
                    words = System.IO.File.ReadAllLines(_filePath).ToList(); // Đọc tất cả các dòng từ tệp
                }
                else
                {
                    // Thông báo nếu không tìm thấy tệp
                    ModelState.AddModelError(string.Empty, "Tệp từ không tồn tại.");
                }
            }
            catch (IOException ex)
            {
                // Xử lý lỗi khi đọc tệp
                ModelState.AddModelError(string.Empty, "Có lỗi khi đọc tệp: " + ex.Message);
            }
            return words; // Trả về danh sách từ
        }

        // Phương thức kiểm tra từ
        private bool CheckIfEnglishWord(string word, List<string> englishWords)
        {
            // Chuyển từ nhập vào thành chữ thường và so sánh
            return !string.IsNullOrWhiteSpace(word) &&
                   englishWords.Select(w => w.ToLower()).Contains(word.Trim().ToLower());
        }
    }
}
