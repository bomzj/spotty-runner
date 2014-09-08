using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Classes.Models
{
    public class ObjectPool
    {
        private List<object> objectsInPool = new List<object>();

        public T GetObjectFromPool<T>() where T: class
        {
            foreach (var item in objectsInPool)
            {
                if (item is T) return item as T;
            }

            return null;
        }

        public void AddObjectToPool<T>(T obj)
        {
            objectsInPool.Add(obj);
        }

        public int GetObjectsCountByType<T>()
        {
            int count = 0;

            foreach (var item in objectsInPool)
            {
                if (item is T) count++;
            }

            return count;
        }
               
    }
}
