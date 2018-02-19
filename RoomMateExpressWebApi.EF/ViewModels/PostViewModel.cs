using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.ViewModels
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        public UserViewModel User { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public AccomodationOptions AccomodationOption { get; set; }

        public AccomodationType AccomodationType { get; set; }

        public PetOptions PetOptions { get; set; }

        public bool ArePetsAllowed { get; set; }

        public bool IsSmokerAllowed { get; set; }

        public byte NumberOfRoommates { get; set; }

        public Gender WantedGender { get; set; }

        public int LikesCount { get; set; }

        public int CommentsCount { get; set; }

        public bool IsLiked { get; set; }

        public DateTimeOffset PostDate { get; set; }

        public List<PostPicture> PostPictures { get; set; }

        public List<Neighborhood> Neighborhoods { get; set; }

        public PostViewModel()
        {
            PostPictures = new List<PostPicture>();
            Neighborhoods = new List<Neighborhood>();
        }
    }
}
