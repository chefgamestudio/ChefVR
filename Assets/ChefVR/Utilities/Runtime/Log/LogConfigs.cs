using System.Linq;
using UnityEngine;

namespace gs.chef.utilities.log
{
    public class LogConfigs : ScriptableObject
    {
        public static LogConfigs Instance { get; private set; }
        
        [field: SerializeField] public LogState LogState { get; private set; }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/CHEF Game/LogConfigs")]
        public static void CreateSettingsAsset()
        {
            var title = "Create ChefSettings";
            var assetName = "LogConfigs";
            var assetExtension = "asset";
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject(title, assetName, assetExtension, string.Empty);

            if (string.IsNullOrEmpty(path))
                return;

            var settings = ScriptableObject.CreateInstance<LogConfigs>();
            UnityEditor.AssetDatabase.CreateAsset(settings, path);

            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.RemoveAll(x => x is LogConfigs);
            preloadedAssets.Add(settings);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
        
        public static void LoadInstanceFromPreloadAssets()
        {
            var preloadAsset = UnityEditor.PlayerSettings.GetPreloadedAssets().FirstOrDefault(x => x is LogConfigs);
            if (preloadAsset is LogConfigs instance)
            {
                instance.OnEnable();
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadInstanceFromPreloadAssetsOnLoad()
        {
            LoadInstanceFromPreloadAssets();
        }
#endif
        
        private void OnEnable()
        {
            if (Application.isPlaying)
                Instance = this;
        }
    }
}