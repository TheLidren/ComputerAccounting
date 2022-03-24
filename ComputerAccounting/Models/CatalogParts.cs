using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComputerAccounting.Models
{
    public class CatalogParts
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([а-яА-ЯёЁa-zA-Z0-9\s\.\,\-]{1,50})$", ErrorMessage = "Пример: ОЗУ 8gb ddr4 Samsung")]
        public string Tittle { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [RegularExpression(@"(\d{1,10})", ErrorMessage = "Текст должен содержать только латинские буквы и нижний регистр")]
        public int Count { get; set; }

        [Required(ErrorMessage = "Данное поле не должно быть пустым")]
        [RegularExpression(@"([0-9]{1,6}.[0-9]{1,2})", ErrorMessage = "Текст должен содержать только 2 знака после запятой")]
        public float Price { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<RepairDevices> RepairDevices { get; set; }

    }
}
