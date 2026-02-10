namespace AlfaPeople.KingKhalidFoundation.Data.Model
{
    using System;
    using System.ComponentModel;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class IncubationWorkshopBLTransactionsType
    {
        public IncubationWorkshopBLTransactionsType()
        {
            this.IncubationWorkshopBLTrans = new List<IncubationWorkshopBLTransactions>();

        }

        [Key]
        public Guid TransTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string TransTypeName { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsMasterData { get; set; }

        public List<IncubationWorkshopBLTransactions> IncubationWorkshopBLTrans { get; set; }

    }
}