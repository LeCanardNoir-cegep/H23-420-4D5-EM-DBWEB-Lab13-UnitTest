using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sem13.Models
{
    [Table("Produit", Schema = "Produits")]
    public partial class Produit
    {
        [Key]
        [Column("ProduitID")]
        public int ProduitId { get; set; }
        [StringLength(50)]
        public string Categorie { get; set; } = null!;
        [StringLength(100)]
        public string Nom { get; set; } = null!;
        [Column(TypeName = "money")]
        public decimal Prix { get; set; }
        public int QteStock { get; set; }
        public bool EstDiscontinue { get; set; }
    }
}
