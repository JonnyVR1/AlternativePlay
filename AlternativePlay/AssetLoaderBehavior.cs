using System.Reflection;
using UnityEngine;

namespace AlternativePlay
{
    public class AssetLoaderBehavior : MonoBehaviour
    {
        public GameObject TrackerPrefab { get; private set; }
        public GameObject SaberPrefab { get; private set; }
        public GameObject FlailHandlePrefab { get; private set; }
        public GameObject LinkPrefab { get; private set; }

        private void Awake()
        {
            AssetBundle assetBundle = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("AlternativePlay.Resources.alternativeplaymodels"));
            TrackerPrefab = assetBundle.LoadAsset<GameObject>("APTracker");
            SaberPrefab = assetBundle.LoadAsset<GameObject>("APSaber");
            FlailHandlePrefab = assetBundle.LoadAsset<GameObject>("APFlailHandle");
            LinkPrefab = assetBundle.LoadAsset<GameObject>("APLink");
            assetBundle.Unload(false);
        }
    }
}
