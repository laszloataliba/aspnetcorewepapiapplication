using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.ViewModels
{
    public class AddressViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [DisplayName("Logradouro"),
         Required(ErrorMessage = "O campo {0} é obrigatório"),
         StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 10)]
        public string StreetAddress { get; set; }

        [DisplayName("Número"),
         Required(ErrorMessage = "O campo {0} é obrigatório"),
         StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string Number { get; set; }

        [DisplayName("Complemento"),
         StringLength(250, ErrorMessage = "O campo {0} pode ter no máximo {1} caracteres")]
        public string AddressAddOn { get; set; }

        [DisplayName("CEP"),
         Required(ErrorMessage = "O campo {0} é obrigatório"),
         StringLength(8, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 8)]
        public string ZipCode { get; set; }

        [DisplayName("Bairro"),
         Required(ErrorMessage = "O campo {0} é obrigatório"),
         StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 10)]
        public string Neighborhood { get; set; }

        [DisplayName("Cidade"),
         Required(ErrorMessage = "O campo {0} é obrigatório"),
         StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]
        public string City { get; set; }

        [DisplayName("Estado"),
         Required(ErrorMessage = "O campo {0} é obrigatório"),
         StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string State { get; set; }

        [HiddenInput]
        public Guid SupplierId { get; set; }
    }
}
