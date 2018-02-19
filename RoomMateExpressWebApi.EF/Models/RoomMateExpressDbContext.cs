using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class RoomMateExpressDbContext : DbContext
    {
        public IDbSet<Admin> Admins { get; set; }
        public IDbSet<Chat> Chats { get; set; }
        public IDbSet<City> Cities { get; set; }
        public IDbSet<PostComment> PostComments { get; set; }
        public IDbSet<ProfileComment> ProfileComments { get; set; }
        public IDbSet<Message> Messages { get; set; }
        public IDbSet<Neighborhood> Neighborhoods { get; set; }
        public IDbSet<Post> Posts { get; set; }
        public IDbSet<PostPicture> PostPictures { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<UserReport> UserReports { get; set; }

        public RoomMateExpressDbContext(string cnnstr) : base(cnnstr)
        {
           
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Admin>().HasKey(a => a.Id);
            modelBuilder.Entity<Admin>().HasMany(a => a.UserReports).WithRequired(ur => ur.Admin);

            modelBuilder.Entity<Chat>().HasKey(c => c.Id);
            modelBuilder.Entity<Chat>().HasMany(c => c.Messages).WithRequired(m => m.Chat);
            modelBuilder.Entity<Chat>().HasMany(c => c.Users).WithMany(u => u.Chats);

            modelBuilder.Entity<City>().HasKey(c => c.Id);
            modelBuilder.Entity<City>().HasMany(c => c.Neighborhoods).WithRequired(n => n.City);

            modelBuilder.Entity<PostComment>().HasKey(cfp => cfp.Id);
            modelBuilder.Entity<PostComment>().HasOptional(cfp => cfp.User).WithMany(u => u.PostComments);
            modelBuilder.Entity<PostComment>().HasRequired(cfp => cfp.Post).WithMany(p => p.Comments);

            modelBuilder.Entity<ProfileComment>().HasKey(cfp => cfp.Id);
            modelBuilder.Entity<ProfileComment>().HasOptional(cfp => cfp.UserCommentator)
                .WithMany(u => u.ProfileComments);
            modelBuilder.Entity<ProfileComment>().HasOptional(cfp => cfp.UserProfile)
                .WithMany(u => u.CommentsOnProfile);

            modelBuilder.Entity<Message>().HasKey(m => m.Id);
            modelBuilder.Entity<Message>().HasOptional(m => m.UserSender).WithMany(u => u.SentMessages);

            modelBuilder.Entity<Neighborhood>().HasKey(n => n.Id);
            modelBuilder.Entity<Neighborhood>().HasMany(n => n.Posts).WithMany(p => p.Neighborhoods);

            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Post>().HasMany(p => p.PostPictures).WithRequired(pp => pp.Post);
            modelBuilder.Entity<Post>().HasOptional(p => p.User).WithMany(u => u.Posts);
            modelBuilder.Entity<Post>().Property(p => p.Price).HasColumnType("smallmoney");
            modelBuilder.Entity<Post>().HasMany(p => p.Likes).WithMany(u => u.Likes);

            modelBuilder.Entity<PostPicture>().HasKey(pp => pp.Id);

            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasMany(u => u.UserReports).WithOptional(ur => ur.UserReported);
            modelBuilder.Entity<User>().HasMany(u => u.UsersReported).WithOptional(ur => ur.UserReporting);
            modelBuilder.Entity<User>().HasMany(u => u.MyRoommates).WithMany(u => u.RoommatesWithMe);
            modelBuilder.Entity<User>().HasMany(u => u.SentRoommateRequests).WithMany(u => u.RecievedRoommateRequests);

            modelBuilder.Entity<UserReport>().HasKey(ur => ur.Id);
        }
    }
}
