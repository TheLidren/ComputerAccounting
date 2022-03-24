using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComputerAccounting.Models
{
    public class CompAccounting
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите сотрудника")]
        public int? EmployeeId { get; set; }
        
        public Employee Employee { get; set; }

        [Required(ErrorMessage = "Выберите устройство")]
        public int? DeviceId { get; set; }

        public Device Device { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([а-яА-ЯёЁ\s\.\,\-\№0-9]{1,50})$", ErrorMessage = "Пример: 1 корпус кабинет № 10")]
        public string PlaceLocated { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateRecieve { get; set; }

        public DateTime DateDelete { get; set; }

        public bool Status { get; set; }

    }
}
