using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComputerAccounting.Models
{
    public class Employee
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

        [Required(ErrorMessage = "Укажите дату")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateWork { get; set; }
        
        [Required(ErrorMessage = "Выберите должность")]
        public int? PositionId { get; set; }

        public Position Position { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<CompAccounting> CompAccs { get; set; }

    }
}
