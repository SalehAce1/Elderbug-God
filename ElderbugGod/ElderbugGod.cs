using System;
using System.Diagnostics;
using System.Reflection;
using Modding;
using JetBrains.Annotations;
using ModCommon;
using MonoMod.RuntimeDetour;
using UnityEngine.SceneManagement;
using UnityEngine;
using USceneManager = UnityEngine.SceneManagement.SceneManager;
using UObject = UnityEngine.Object;
using System.Collections.Generic;
using System.IO;

namespace ElderbugGod
{
    [UsedImplicitly]
    public class ElderbugGod : Mod, ITogglableMod
    {
        public static Dictionary<string, GameObject> preloadedGO = new Dictionary<string, GameObject>();

        public static ElderbugGod Instance;

        public static readonly List<Sprite> SPRITES = new List<Sprite>();

        public override string GetVersion()
        {
            return "0.0.0.0";
        }

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Mines_18_boss","Beam Ball"),
                ("Mines_18_boss","Beam"),
            };
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Storing GOs");
            preloadedGO.Add("ball", preloadedObjects["Mines_18_boss"]["Beam Ball"]);
            preloadedGO.Add("beam", preloadedObjects["Mines_18_boss"]["Beam"]);
            Instance = this;
            Log("Initalizing.");

            Unload();
            ModHooks.Instance.AfterSavegameLoadHook += AfterSaveGameLoad;
            ModHooks.Instance.NewGameHook += AddComponent;
            ModHooks.Instance.LanguageGetHook += LangGet;
            int ind = 0;
            Assembly asm = Assembly.GetExecutingAssembly();
            //MusicLoad.LoadAssets.LoadWavFile();
            foreach (string res in asm.GetManifestResourceNames())
            {
                if (!res.EndsWith(".png"))
                {
                    continue;
                }

                using (Stream s = asm.GetManifestResourceStream(res))
                {
                    if (s == null) continue;

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    s.Dispose();
                    var tex = new Texture2D(1, 1);
                    tex.LoadImage(buffer, true);
                    SPRITES.Add(Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
                    Log("Created sprite from embedded image: " + res + " at ind " + ++ind);
                }
            }
        }

        private string LangGet(string key, string sheettitle)
        {
            if (ElderFinder.died)
            {
                switch (key)
                {
                    case "ELDERBUG_INTRO_WALKEDBY": return "Young buggo, that was an incredibly rude thing you attempted to do, passing an old bug like me, you would have killed me too if I wasn't an NPC.<page>Kids these days have no manners.<page>";
                    case "ELDERBUG_INTRO_MAIN": return "What I don't understand is why you would come here, to this dead kingdom. I suggest you turn back while you can and head to Pharloom.<page>Now if you will excuse me, I have a tea party with Mr. Mushroom to attend to.";
                    case "ELDERBUG_MAIN": return "ElderGod";
                    case "ELDERBUG_SUB": return "Cutest Buggo";
                    case "LISTEN": return "Hear God's Words";
                    default: return Language.Language.GetInternal(key, sheettitle);
                }
            }
            else return Language.Language.GetInternal(key, sheettitle);
        }

        private void AfterSaveGameLoad(SaveGameData data) => AddComponent();

        private void AddComponent()
        {
            GameManager.instance.gameObject.AddComponent<ElderFinder>();
        }

        public void Unload()
        {
            AudioListener.volume = 1f;
            AudioListener.pause = false;
            ModHooks.Instance.AfterSavegameLoadHook -= AfterSaveGameLoad;
            ModHooks.Instance.NewGameHook -= AddComponent;
            ModHooks.Instance.LanguageGetHook -= LangGet;

            // ReSharper disable once Unity.NoNullPropogation
            //var x = GameManager.instance?.gameObject.GetComponent<ArenaFinder>();
            //if (x == null) return;
            //UObject.Destroy(x);
        }
    }
}