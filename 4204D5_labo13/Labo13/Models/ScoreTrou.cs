using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Labo13.Models
{
    [Table("ScoreTrou", Schema = "Golf")]
    public partial class ScoreTrou
    {
        [Key]
        [Column("ScoreTrouID")]
        public int ScoreTrouId { get; set; }
        public int Score { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Terme { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime DateTrou { get; set; }
        [Column("GolfeurID")]
        public int GolfeurId { get; set; }

        [ForeignKey("GolfeurId")]
        [InverseProperty("ScoreTrous")]
        public virtual Golfeur Golfeur { get; set; } = null!;
    }
}
