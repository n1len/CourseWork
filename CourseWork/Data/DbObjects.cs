using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseWork.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Data
{
    public class DbObjects
    {
        public static async void CreateItem(ApplicationContext context, Item item,int id)
        {
            await context.AddAsync(new Item
            {
                Title = item.Title,
                Tags = item.Tags,
                NumericField1 = item.NumericField1,
                NumericField2 = item.NumericField2,
                NumericField3 = item.NumericField3,
                OneLineField1 = item.OneLineField1,
                OneLineField2 = item.OneLineField2,
                OneLineField3 = item.OneLineField3,
                TextField1 = item.TextField1,
                TextField2 = item.TextField2,
                TextField3 = item.TextField3,
                Date1 = item.Date1,
                Date2 = item.Date2,
                Date3 = item.Date3,
                CheckBox1 = item.CheckBox1,
                CheckBox2 = item.CheckBox2,
                CheckBox3 = item.CheckBox3,
                CustomCollectionId = id
            });

            context.SaveChanges();
        }

        public static async void CreateComment(ApplicationContext context, string description, int id,string userId)
        {
            await context.AddAsync(new Comment
            {
                Description = description,
                ItemId = id,
                UserId = userId
            });

            context.SaveChanges();
        }

        public static async void LikeComment(ApplicationContext context,int id, string userId,string userName)
        {
            await context.AddAsync(new LikeOnComment
            {
                IsLiked = true,
                CommentId = id,
                UserName = userName,
                UserId = userId
            });

            context.SaveChanges();
        }

        public static async void LikeItem(ApplicationContext context, int id, string userId,string userName)
        {
            await context.AddAsync(new LikeOnItem()
            {
                IsLiked = true,
                ItemId = id,
                UserName = userName,
                UserId = userId
            });

            context.SaveChanges();
        }
    }
}
