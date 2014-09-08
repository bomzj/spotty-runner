using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes.Utils
{
    public static class GameObjectFinder
    {
        public static T[] FindObjectsOfType<T>(bool alsoSearchDisabled) where T : MonoBehaviour
        {
            if (alsoSearchDisabled)
            {
                return Resources.FindObjectsOfTypeAll<T>();
            }
            else
            {
                return GameObject.FindObjectsOfType<T>();
            }
        }

        public static T FindObjectOfType<T>(bool alsoSearchDisabled) where T : MonoBehaviour
        {
            return FindObjectsOfType<T>(alsoSearchDisabled).FirstOrDefault();
        }

        public static GameObject Find<T>(string name, bool alsoSearchDisabled) where T : MonoBehaviour
        {
            GameObject gameObject = null;
            
            var script = FindObjectsOfType<T>(alsoSearchDisabled)
                .Where(item => item.gameObject.name == name)
                .FirstOrDefault();
            
            if (script != null)
            {
                gameObject = script.gameObject;
            }
            
            return gameObject;
        }
        
        public static GameObject Find(string name, bool alsoSearchDisabled)
        {
            if (alsoSearchDisabled)
            {
                return Resources.FindObjectsOfTypeAll<GameObject>()
                    .Where(item => item.name == name)
                    .FirstOrDefault();
            }
            else
            {
                return GameObject.Find(name);
            }
        }

        
    }
}
