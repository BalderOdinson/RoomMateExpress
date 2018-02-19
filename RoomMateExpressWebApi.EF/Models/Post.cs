using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoomMateExpressWebApi.EF.Models
{
    public class Post : Entity
    {
        public User User { get; set; }

        [Required]
        [StringLength(maximumLength: 200, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public decimal? Price { get; set; }

        public AccomodationOptions AccomodationOption { get; set; }

        public AccomodationType AccomodationType { get; set; }

        public PetOptions PetOptions { get; set; }

        public bool ArePetsAllowed { get; set; }

        public bool IsSmokerAllowed { get; set; }

        public byte NumberOfRoommates { get; set; }

        public Gender WantedGender { get; set; }

        public DateTimeOffset PostDate { get; set; }

        public List<User> Likes { get; set; }

        public List<PostPicture> PostPictures { get; set; }

        public List<Neighborhood> Neighborhoods { get; set; }

        public List<PostComment> Comments { get; set; }

        public Post()
        {
            Likes = new List<User>();
            PostPictures = new List<PostPicture>();
            Neighborhoods = new List<Neighborhood>();
            Comments = new List<PostComment>();
        }
    }
}
