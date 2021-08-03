using System.Collections.Generic;

namespace CourseWork.Infrastructure.Models
{
    public class CustomCollection
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Topic { get; set; }
        public string Img { get; set; }
        public bool IsNumericField1Visible { get; set; }
        public bool IsNumericField2Visible { get; set; }
        public bool IsNumericField3Visible { get; set; }
        public bool IsOneLineField1Visible { get; set; }
        public bool IsOneLineField2Visible { get; set; }
        public bool IsOneLineField3Visible { get; set; }
        public bool IsTextField1Visible { get; set; }
        public bool IsTextField2Visible { get; set; }
        public bool IsTextField3Visible { get; set; }
        public bool IsDate1Visible { get; set; }
        public bool IsDate2Visible { get; set; }
        public bool IsDate3Visible { get; set; }
        public bool IsCheckBox1Visible { get; set; }
        public bool IsCheckBox2Visible { get; set; }
        public bool IsCheckBox3Visible { get; set; }
        public ICollection<Item> Items { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
