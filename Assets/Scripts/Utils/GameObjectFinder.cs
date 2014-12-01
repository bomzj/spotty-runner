using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Classes.Utils
{
    // TODO: do not use this class as finder for inactive objects  
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
                var objs = Resources.FindObjectsOfTypeAll<GameObject>()
                    .Where(item => item.name == name);

                var objs1 = objs.ToArray();

                // Exclude prefabs
                foreach (var obj in objs)
                {
                    if (obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave)
                        continue;

                    // TODO: AssetDatabase is accessible only in Editor mode 
                    //var assetPath = AssetDatabase.GetAssetPath(obj.transform.root.gameObject);
                    //if (!String.IsNullOrEmpty(assetPath))
                    //    continue;

                    return obj;
                }
            }
            else
            {
                return GameObject.Find(name);
            }

            return null;
        }

        
        
    }
}
