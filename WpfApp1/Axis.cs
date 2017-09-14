namespace WpfApp1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Axis")]
    public partial class Axis
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int? Number { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public int? AdminMax { get; set; }

        public int? AdminMin { get; set; }

        public int? UserMax { get; set; }

        public int? UserMin { get; set; }

        public int? CurrentPosition { get; set; }

        public int? TargetPosition { get; set; }

        public int? Acceleration { get; set; }

        public int? Velocity { get; set; }

        public int? AxisGroup { get; set; }

        [StringLength(50)]
        public string AxisGroupName { get; set; }

        [StringLength(50)]
        public string IpAdress { get; set; }

        [StringLength(50)]
        public string NetID { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public bool? Faulted { get; set; }

        public int? FaultCode { get; set; }
    }
}
