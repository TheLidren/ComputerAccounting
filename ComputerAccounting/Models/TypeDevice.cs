﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComputerAccounting.Models
{
    public class TypeDevice
    {
        [HiddenInput(DisplayValue = false)] 
        public int Id { get; set; }

        [Required (ErrorMessage = "Данное поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Введите корректно размерность", MinimumLength = 2)]
        [RegularExpression(@"^([А-ЯЁ]{1}[а-яё\s]{1,50})$", ErrorMessage = "Текст должен содержать только латинские буквы и нижний регистр")]
        public string Tittle { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Device> Devices { get; set; }
    }
}
