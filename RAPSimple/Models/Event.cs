using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RAPSimple.Models
{
    public class Event
    {
        public int ID { get; set; }

        [Required]
        [StringLength(200)]
        [Column("Title")]
        [Display(Name = "Название мероприятия")]
        public string Title { get; set; }

        [Required]
        [DisplayFormat(DataFormatString="{0:dd-MM-yyyy}", ApplyFormatInEditMode=true)]
        [Column("StartDate")]
        [Display(Name = "Начало мероприятия")]
        public DateTime StartDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Column("EndDate")]
        [Display(Name = "Окончание мероприятия")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Описание")]
        [Column("Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Теги")]
        [StringLength(250, ErrorMessage = "Поле Tags не может превышать 250 символов.")]
        [Column("Tags")]
        [DataType(DataType.MultilineText)]
        public string Tags { get; set; }

        [Display(Name = "Средний бюджет (тыс. руб.)")]
        [Column("AvgBudget")]
        public int AvgBudget { get; set; }

        [Display(Name = "Локация")]
        [StringLength(50, ErrorMessage = "Поле Location не может превышать 50 символов.")]
        [Column("Location")]
        public string Location { get; set; }

        [Display(Name = "Уровень сложность")]
        [StringLength(50, ErrorMessage = "Поле Difficulties не может превышать 50 символов.")]
        [Column("Difficulties")]
        public string Difficulties { get; set; }
#if false
        [Display(Name = "Лого")]
        [Column("Description")]
        [DataType(DataType.MultilineText)]
        public string Details { get; set; }

        [Display(Name = "Фон")]
        [Column("BackgroundPicture")]
        [DataType(DataType.MultilineText)]
        public string BackgroundPicture { get; set; }
#endif
        [Display(Name = "Максимальное количество участников")]
 //       [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("MaxGroup")]
        public int MaxGroup { get; set; }

        // owner (who has created this item)
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
//        public virtual ICollection<File> Files { get; set; } перестроить схему общения с файлами

    }
}