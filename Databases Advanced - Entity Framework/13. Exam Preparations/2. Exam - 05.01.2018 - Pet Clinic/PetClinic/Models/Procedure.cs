﻿namespace PetClinic.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class Procedure
    {
        public Procedure()
        {
            this.ProcedureAnimalAids = new HashSet<ProcedureAnimalAid>();
        }

        public int Id { get; set; }

        public int AnimalId { get; set; }

        [Required]
        public Animal Animal { get; set; }

        public int VetId { get; set; }

        [Required]
        public Vet Vet { get; set; }

        public ICollection<ProcedureAnimalAid> ProcedureAnimalAids { get; set; }

        public decimal Cost => this.ProcedureAnimalAids.Sum(pa => pa.AnimalAid.Price);

        [Required]
        public DateTime DateTime { get; set; }
    }
}
