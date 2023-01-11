using UnityEditor;

namespace KidsLearning.Assets.Editor
{
    public class WebGLEditorScript
    {
        [MenuItem("WebGL/Enable Embedded Resources")]
        public static void EnableEmbeddedResources()
        {
            PlayerSettings.WebGL.useEmbeddedResources = true;
        }
    }
}
