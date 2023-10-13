using Auxiliar.Domain.Core.Events;
using Auxiliar.Domain.Core.Models;
using System.Reflection;

namespace Auxiliar.Domain.Core.Extensions
{
    public static class ExtensionMethods
    {
        #region Metodos Publicos

        public static T Cast<T>(this object model)
        {
            Type objectType = model.GetType();
            Type target = typeof(T);

            object objectInstance = Activator.CreateInstance(target, false);

            IEnumerable<MemberInfo> sourceMembers = from source in objectType.GetMembers()
                                                                             .ToList()
                                                    where source.MemberType == MemberTypes.Property
                                                    select source;

            IEnumerable<MemberInfo> targetMembers = from source in target.GetMembers()
                                                                         .ToList()
                                                    where source.MemberType == MemberTypes.Property
                                                    select source;

            List<MemberInfo> members = targetMembers.Where(memberInfo => targetMembers
                                                    .Select(c => c.Name)
                                                    .ToList()
                                                    .Contains(memberInfo.Name))
                                                    .ToList();
            PropertyInfo propertyInfo;
            object value;
            foreach (var memberInfo in members)
            {
                propertyInfo = typeof(T).GetProperty(memberInfo.Name);
                if (nameof(Message.MessageType) != propertyInfo.Name &&
                    nameof(Event.CreatedDate) != propertyInfo.Name &&
                    model.GetType().GetProperty(memberInfo.Name) != null)
                {
                    value = model.GetType()
                                 .GetProperty(memberInfo.Name)
                                 .GetValue(model, null);

                    propertyInfo.SetValue(objectInstance, value, null);
                }
            }

            if (target.IsSubclassOf(typeof(Event)))
            {
                PropertyInfo eventAggregateId = target.GetProperty(nameof(Event.AggregateId));
                PropertyInfo objectId = model.GetType().GetProperty(nameof(Entity.Id));
                eventAggregateId.SetValue(objectInstance, objectId.GetValue(model, null));
            }

            return (T)objectInstance;
        }

        #endregion Metodos Publicos
    }
}