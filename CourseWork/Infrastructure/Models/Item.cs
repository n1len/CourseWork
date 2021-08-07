using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.Infrastructure.Models
{
    public class Item
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название айтема не может быть пустым")]
        [Display(Name = "Название айтема")]
        public string Title { get; set; }

        [Display(Name = "Тэги")]
        public string Tags { get; set; }

        [RegularExpression(@"^[0-9\s,.]{0,300}")]
        public string NumericField1 { get; set; }

        [RegularExpression(@"^[0-9\s,.]{0,300}")]
        public string NumericField2 { get; set; }

        [RegularExpression(@"^[0-9\s,.]{0,300}")]
        public string NumericField3 { get; set; }

        public string OneLineField1 { get; set; }
        public string OneLineField2 { get; set; }
        public string OneLineField3 { get; set; }
        public string TextField1 { get; set; }
        public string TextField2 { get; set; }
        public string TextField3 { get; set; }

        [DataType(DataType.Date)]
        public string Date1 { get; set; }

        [DataType(DataType.Date)]
        public string Date2 { get; set; }

        [DataType(DataType.Date)]
        public string Date3 { get; set; }

        public bool CheckBox1 { get; set; }
        public bool CheckBox2 { get; set; }
        public bool CheckBox3 { get; set; }
        public int CustomCollectionId { get; set; }
        public CustomCollection CustomCollection { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<LikeOnItem> Likes { get; set; }
    }
}
