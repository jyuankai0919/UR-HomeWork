namespace UR_HomeWork.Models.DB_Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.CompilerServices;

    [Table("Tokens")]
    public class Tokens
    {
        public int Id { get; set; }
        [Required]
        [StringLength(128)]
        public string Value { get; set; }
        [Required]
        [StringLength(50)]
        public string UserId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public virtual User User { get; set; }
    }
}
