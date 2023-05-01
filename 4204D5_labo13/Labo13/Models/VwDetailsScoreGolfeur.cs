using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Labo13.Models
{
    [Keyless]
    public partial class VwDetailsScoreGolfeur
    {
        [Column("GolfeurID")]
        public int GolfeurId { get; set; }
        [StringLength(100)]
        public string Nom { get; set; } = null!;
        public int ScoreTotal { get; set; }
        public int NbTrous { get; set; }
        public int? ScoreMoyen { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DatePremierTrou { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateDernierTrou { get; set; }
    }
}
