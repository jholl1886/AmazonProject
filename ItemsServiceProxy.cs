using AWS.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS.library.Services
{
    public class ItemsServiceProxy
    {
        
        private ItemsServiceProxy()
        {
            items = new List<Items>();
        }

        private static ItemsServiceProxy? instance;
        private static object instanceLock = new object();

        public static ItemsServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ItemsServiceProxy();
                    }
                }
                return instance;
            }
        }

        private List<Items>? items;

        public IReadOnlyCollection<Items>? Items
        {
            get
            {
                return items?.AsReadOnly();
            }
        }

        //functionality 
        public int lastId
        {
            get
            {
                if (items?.Any() ?? false)
                {
                    return items?.Select(c => c.Id)?.Max() ?? 0;
                }
                return 0;
            }
        }

        public Items? AddOrUpdate(Items item)
        {
            if(items == null)
            {
                return null;
            }

            var IsAdd = false;
            if (item.Id == 0)
            {
                item.Id = lastId + 1;
                IsAdd = true;
            }
            if(IsAdd)
            {
                items.Add(item);
            }
            return item;
        }

        public void Delete(int id)
        {
            if (items == null)
            {
                return;
            }

            var itemToDelete = items.FirstOrDefault(c => c.Id == id);

            if(itemToDelete != null)
            {
                items.Remove(itemToDelete);
            }

    }
    }

   

}
