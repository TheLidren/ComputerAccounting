using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComputerAccounting.Models
{
    public class Provider
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([А-ЯЁ]{1}[а-яё]{1,50})$", ErrorMessage = "Текст должен содержать только латинские буквы и нижний регистр")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([А-ЯЁ]{1}[а-яё]{1,50})$", ErrorMessage = "Текст должен содержать только латинские буквы и нижний регистр")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([А-ЯЁ]{1}[а-яё]{1,50})$", ErrorMessage = "Текст должен содержать только латинские буквы и нижний регистр")]
        public string Patronymic { get; set; }
        
        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(15, ErrorMessage = "Введите корректно размерность", MinimumLength = 10)]
        [RegularExpression(@"^(\+375|80)(29|25|44|33)(\d{7})$", ErrorMessage = "Некорректно введён номер телефона")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([а-яА-ЯёЁ\s\""\-0-9]{1,50})$", ErrorMessage = "Текст должен содержать только латинские буквы и кавычки")]
        public string Organization { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([а-яА-ЯёЁ\s\.\,\-0-9]{1,50})$", ErrorMessage = "Пример: г. Гомель ул. Речицкая 4-99")]
        public string OrganizationAdress { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
