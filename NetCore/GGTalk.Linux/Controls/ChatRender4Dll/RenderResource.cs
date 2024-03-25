using System.Collections.Generic;

namespace GGTalk.Controls.ChatRender4Dll
{
    internal class RenderResource
    {
        internal Dictionary<int, object> Emotions;

        private RenderResource() {
            Emotions = new Dictionary<int, object>();
            foreach(var key in GlobalResourceManager.EmotionDictionary.Keys)
            {
                Emotions.Add((int)key, GlobalResourceManager.EmotionDictionary[key]);
            }
        }

        private static RenderResource singleton;

        internal static RenderResource GetSingleton
        {
            get
            {
                if(singleton == null)
                {
                    singleton = new RenderResource();
                }
                return singleton;
            }
        }

         

    }
}
