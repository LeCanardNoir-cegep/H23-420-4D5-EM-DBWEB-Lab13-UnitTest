using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Labo13.Models
{
    [Table("Golfeur", Schema = "Golf")]
    public partial class Golfeur
    {
        public Golfeur()
        {
            ScoreTrous = new HashSet<ScoreTrou>();
        }

        [Key]
        [Column("GolfeurID")]
        public int GolfeurId { get; set; }
        [StringLength(100)]
        public string Nom { get; set; } = null!;
        public int ScoreTotal { get; set; }
        public int NbTrous { get; set; }

        [InverseProperty("Golfeur")]
        public virtual ICollection<ScoreTrou> ScoreTrous { get; set; }
    }
}
