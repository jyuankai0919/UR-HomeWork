namespace UR_HomeWork.Models.DB_Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [StringLength(50)]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string PassWord { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(40)]
        public string Address { get; set; }
    }
}
