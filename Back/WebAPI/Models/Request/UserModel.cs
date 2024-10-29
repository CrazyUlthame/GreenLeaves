using System;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Request
{
    public class UserModel
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Name { get; set; }        
        [Required(ErrorMessage = "El campo Email es obligatorio")]
        [EmailAddress(ErrorMessage = "El campo Email no tiene un formato válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo Telefono es obligatorio")]
        [Phone(ErrorMessage = "El campo Telefono no es válido")]
        [RegularExpression(@"^(\(?\d{3}\)?[- ]?)?\d{3}[- ]?\d{4}$", ErrorMessage = "El formato de Telefono debe ser (123) 456-7890, 123-456-7890, 123 456 7890, o 1234567890")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "El campo Fecha es obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "El campo Localidad es obligatorio")]
        public string City { get; set; }
        [Required(ErrorMessage = "El campo Localidad es obligatorio")]
        public string State { get; set; }
        [Required(ErrorMessage = "El campo Localidad es obligatorio")]
        public string Country { get; set; }
    }
}
