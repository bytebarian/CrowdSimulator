using UnityEngine;

namespace Assets.Scripts.Loader
{
    public class FileLoader : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            string path = "";
            if (Application.platform == RuntimePlatform.OSXEditor)
            {
                path = "Data/";
            }
            else if (Application.platform == RuntimePlatform.OSXPlayer)
            {
                path = "../../";
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                path = "Data/";
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = "../";
            }
        }
    }
}
