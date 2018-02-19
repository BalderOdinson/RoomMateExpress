using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdateManyToManyRelation<TEntity>(this TEntity entity, TEntity newValues, DbContext context,
            params string[] ignoreProperties) where TEntity : Entity
        {
            var properties = typeof(TEntity).GetProperties();
            foreach (var property in properties)
            {
                if(ignoreProperties.Contains(property.Name))
                    continue;
                if (property.PropertyType.IsArray)
                {
                    var deletedElements = (property.GetValue(entity) as ICollection<Entity>)
                        .Except(property.GetValue(newValues) as ICollection<Entity>).ToList();

                    var addedElements = (property.GetValue(newValues) as ICollection<Entity>)
                        .Except(property.GetValue(entity) as ICollection<Entity>);

                    deletedElements.ForEach(c => (property.GetValue(entity) as ICollection<Entity>).Remove(c));

                    foreach (var addedElement in addedElements)
                    {
                        context.Entry(addedElement).State = EntityState.Detached;
                        context.Set(addedElement.GetType()).Attach(addedElement);
                        (property.GetValue(entity) as ICollection<Entity>).Add(addedElement);
                    }
                }

                property.SetValue(entity, property.GetValue(newValues));
            }
        }
    }
}
