using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    public class Events
    {
        [Key]
        public Guid EventID { get; set; }

        [Required]
        [StringLength(50)]
        public string EventNameEN { get; set; }

        [Required]
        [StringLength(50)]
        public string EventNameAR { get; set; }

        public bool IsActive { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        public string Description { get; set; }

    }
}