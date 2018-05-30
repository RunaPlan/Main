using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// путевые точки = точки на маршруте(мероприятии)
namespace RAPSimple.Models
{
    public class WayPoint
    {
        public int ID { get; set; }
        // родительское мероприятие, к которому относится точка
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
        [Display(Name = "Название")]
        [StringLength(50, ErrorMessage = "name cannot be longer than 50 characters.")]
        public string Name { get; set; }

        [Display(Name = "Тип: 1 - на маршруте 2 - промежуточная 3 - дополнительная")]
        public int WPType { get; set; }

        [Display(Name = "Описание")]
        [Column("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Порядковый номер на маршруте")]
        public int Order { get; set; }

        [Display(Name = "Широта")]
        [StringLength(50, ErrorMessage = "name cannot be longer than 50 characters.")]
        public string Latitude { get; set; }

        [Display(Name = "Долгота")]
        [StringLength(50, ErrorMessage = "name cannot be longer than 50 characters.")]
        public string Longitude { get; set; }
    }
}