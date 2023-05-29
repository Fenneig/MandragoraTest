using System;
using System.Collections.Generic;

namespace Mandragora.Services
{
    public class ServiceLocator<T> : IServiceLocator<T>
    {
        protected Dictionary<Type, T> ItemsMap { get; }

        public ServiceLocator()
        {
            ItemsMap = new Dictionary<Type, T>();
        }
        
        public TP Register<TP>(TP newService) where TP : T
        {
            var type = newService.GetType();
            if (ItemsMap.ContainsKey(type))
            {
                throw new Exception($"Cannot add item of type {type}, this type already exists in the service locator!");
            }

            ItemsMap[type] = newService;
            return newService;
        }

        public void UnRegister<TP>(TP service) where TP : T
        {
            var type = service.GetType();
            if (ItemsMap.ContainsKey(type))
            {
                ItemsMap.Remove(type);
            }
        }

        public TP Get<TP>() where TP : T
        {
            var type = typeof(TP);
            if (!ItemsMap.ContainsKey(type))
            {
                throw new Exception($"There is no object of type {type} in the service locator!");
            }

            return (TP) ItemsMap[type];
        }
    }
}
