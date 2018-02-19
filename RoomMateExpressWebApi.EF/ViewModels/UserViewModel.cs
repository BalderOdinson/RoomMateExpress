using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal AverageGrade { get; set; }

        public string ProfilePictureUrl { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset BirthDate { get; set; }

        public byte Age { get; set; }

        public bool HasFaculty { get; set; }

        public string DescriptionOfStudyOrWork { get; set; }

        public bool IsSmoker { get; set; }

        public Gender Gender { get; set; }

        public int CommentsOnProfileCount { get; set; }

    }
}
