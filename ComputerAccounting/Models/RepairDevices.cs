using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComputerAccounting.Models
{
    public class RepairDevices
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Выберите устройство")]
        public int? DeviceId { get; set; }

        public Device Device { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([а-яА-ЯёЁa-zA-Z0-9\s\.\,\-]{1,50})$", ErrorMessage = "Пример: ОЗУ 8gb ddr4 Samsung")]
        public string BrokenParts { get; set; }

        [Required(ErrorMessage = "Выберите запчасть")]
        public int? CatalogPartsId { get; set; }

        public CatalogParts CatalogParts { get; set; }

        [Required(ErrorMessage = "Укажите дату")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DateRepair { get; set; }

        public bool Status { get; set; }
    }
}
