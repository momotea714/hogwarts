using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Hogwarts.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Hogwarts", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<HogwartsSetting> HogwartsSettings { get; set; }

        public DbSet<Lecture> Lectures { get; set; }

        public DbSet<SubLecture> SubLectures { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<UserAnswerState> UserAnswerStates { get; set; }

        public DbSet<Information> Informations { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public DbSet<KnowledgeCategory> KnowledgeCategories { get; set; }

        public DbSet<Knowledge> Knowledges { get; set; }

        public DbSet<Record> Records { get; set; }

        public DbSet<Ask> Asks { get; set; }

        public DbSet<TargetTraineeOfLecture> TargetTraineeOfLectures { get; set; }
    }

}