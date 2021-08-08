using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseWork.Infrastructure.Models
{
    public class CustomCollection
    {
        public int Id { get; set; }
        [Display(Name = "Название коллекции:")]
        public string Title { get; set; }

        [Display(Name = "Описание:")]
        public string Description { get; set; }

        [Display(Name = "Тема:")]
        public string Topic { get; set; }

        [Display(Name = "Изображение:")]
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
        public string NumericField1 { get; set; }
        public string NumericField2 { get; set; }
        public string NumericField3 { get; set; }
        public string OneLineField1 { get; set; }
        public string OneLineField2 { get; set; }
        public string OneLineField3 { get; set; }
        public string TextField1 { get; set; }
        public string TextField2 { get; set; }
        public string TextField3 { get; set; }
        public string Date1 { get; set; }
        public string Date2 { get; set; }
        public string Date3 { get; set; }
        public string CheckBox1 { get; set; }
        public string CheckBox2 { get; set; }
        public string CheckBox3 { get; set; }
        public ICollection<Item> Items { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
