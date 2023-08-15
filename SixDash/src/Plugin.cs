using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.IO;
using System;
using System.Linq;

#if BEPINEX
using BepInEx;
using BepInEx.Configuration;
#elif MELON
using MelonLoader;
#endif

using HarmonyLib;

using SixDash.API;

using UnityEngine;

namespace SixDash;

#if BEPINEX
[BepInPlugin("mod.cgytrus.plugins.sixdash", SixDashPluginInfo.PLUGIN_NAME, SixDashPluginInfo.PLUGIN_VERSION)]
#endif

internal class SixDash:

#if BEPINEX
    BaseUnityPlugin
#elif MELON
    MelonMod
#endif
{
    internal static Plugin? instance { get; private set; }

    #if BEPINEX
        private void Awake()
    #elif MELON
        public override void OnInitializeMelon()
    #endif
    {
        instance = this;

        Logger.LogInfo("Loading assets");
        AssetBundle bundle = Util.LoadPlatformAssetBundle("6dash");
        World.LoadAssets(bundle);

        Logger.LogInfo("Applying patches");
        World.Patch();
        Player.Patch();
        Music.Patch();
        Checkpoint.Patch();
        Online.Patch();
        Util.ApplyAllPatches();

        Logger.LogInfo("Initializing UI");
        UI.Setup();
        UI.AddVersionText($"6Dash v{SixDashPluginInfo.PLUGIN_VERSION}");
    }

    private void Start() {
        GameObject gizmosCamObj = new("Gizmos Camera");
        DontDestroyOnLoad(gizmosCamObj);
        gizmosCamObj.AddComponent<GizmosCamera>();
    }

    internal static void StartGlobalCoroutine(IEnumerator routine) => instance!.StartCoroutine(routine);
}
