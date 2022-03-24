using System.ComponentModel.DataAnnotations;

namespace ComputerAccounting.ListViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[mail|gmail]*\.(ru|com)$", ErrorMessage = "Некорректный адрес. Только mail.ru или gmail.com")]
        [StringLength(100, ErrorMessage = "Длина строки должна быть до 100 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указана пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Длина пароль должна быть от 8 до 50 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
