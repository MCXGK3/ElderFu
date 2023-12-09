using Modding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace ElderFu
{
    public class ElderFu : Mod,ITogglableMod
    {
        internal static ElderFu Instance;

        //public override List<ValueTuple<string, string>> GetPreloadNames()
        //{
        //    return new List<ValueTuple<string, string>>
        //    {
        //        new ValueTuple<string, string>("White_Palace_18", "White Palace Fly")
        //    };
        //}

        public ElderFu() : base("laser hudiance")
        {
            Instance = this;
        }
        //public  new string Name = "laser hudiance";

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            Instance = this;
            On.PlayMakerFSM.OnEnable += ModRadiance;
            ModHooks.LanguageGetHook += ChangeLang;
            

            Log("Initialized");
        }

        private string ChangeLang(string key, string sheetTitle, string orig)
        {
            switch (key)
            {
                case "ABSOLUTE_RADIANCE_SUPER": return "LASER";
                case "ABSOLUTE_RADIANCE_MAIN": return "Hudiance";
                case "GG_S_RADIANCE": return"Laser Hudiance";

            }
            return orig;
        }

        private void ModRadiance(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            if(self.FsmName=="Control"&&self.gameObject.name=="Absolute Radiance")
            {
                self.gameObject.AddComponent<Radiance>();
            }
            orig(self);
        }

        public void Unload()
        {
            On.PlayMakerFSM.OnEnable -= ModRadiance;
            ModHooks.LanguageGetHook -= ChangeLang;
        }
    }
}