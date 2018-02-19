using System;

namespace RoomMateExpress.Core.Models
{
    public abstract class Entity
    {
        public virtual Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj?.GetType() == GetType() && ((Entity)obj).Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
