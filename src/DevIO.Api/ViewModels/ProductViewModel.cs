//using DevIO.App.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevIO.Api.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório"), 
         DisplayName("Fornecedor")]
        public Guid SupplierId { get; set; }

        [DisplayName("Nome"),
         Required(ErrorMessage = "O campo {0} é obrigatório"), 
         StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 10)]
        public string Name { get; set; }

        [DisplayName("Descrição"), 
         Required(ErrorMessage = "O campo {0} é obrigatório"), 
         StringLength(400, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 20)]
        public string Description { get; set; }

        [DisplayName("Imagem do Produto")]
        public string ImageUpload { get; set; }

        public string Image { get; set; }

        [//Currency,
         DisplayName("Valor"), 
         Required(ErrorMessage = "O campo {0} é obrigatório")]
        public decimal Value { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }

        [DisplayName("Ativo?")]
        public bool Active { get; set; }

        public SupplierViewModel Supplier { get; set; }

        public IEnumerable<SupplierViewModel> Suppliers { get; set; }
    }
}
