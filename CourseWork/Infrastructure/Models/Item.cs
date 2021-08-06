using System;
using System.Collections.Generic;

namespace CourseWork.Infrastructure.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public int NumericField1 { get; set; }
        public int NumericField2 { get; set; }
        public int NumericField3 { get; set; }
        public string OneLineField1 { get; set; }
        public string OneLineField2 { get; set; }
        public string OneLineField3 { get; set; }
        public string TextField1 { get; set; }
        public string TextField2 { get; set; }
        public string TextField3 { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public DateTime Date3 { get; set; }
        public bool CheckBox1 { get; set; }
        public bool CheckBox2 { get; set; }
        public bool CheckBox3 { get; set; }
        public int CustomCollectionId { get; set; }
        public CustomCollection CustomCollection { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<LikeOnItem> Likes { get; set; }
    }
}
