using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComputerAccounting.Models
{
    public class Device
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([а-яА-ЯёЁa-zA-Z0-9\s\.\,\-]{1,50})$", ErrorMessage = "Пример: Lenovo Legion Y530")]
        public string Tittle { get; set; }

        [Required(ErrorMessage = "Выберите тип устройства")]
        public int? TypeDeviceId { get; set; }
        
        public TypeDevice TypeDevice { get; set; }

        [Required(ErrorMessage = "Выберите поставщика")]
        public int? ProviderId { get; set; }
        
        public Provider Provider { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^[а-яА-ЯёЁa-zA-Z0-9\s\-,.:]+$", ErrorMessage = "Пример: Lenovo Legion Y530")]
        public string Characteristics { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateBuy { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [RegularExpression(@"([0-9]{1,6}.[0-9]{1,2})", ErrorMessage = "Текст должен содержать только 2 знака после запятой")]
        public float Price { get; set; }

        public bool Status { get; set; }
        
        public virtual ICollection<CompAccounting> CompAccs { get; set; }
        public virtual ICollection<RepairDevices> RepairDevices { get; set; }

    }
}
