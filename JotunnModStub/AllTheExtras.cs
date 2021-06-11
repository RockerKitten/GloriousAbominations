using BepInEx;
using UnityEngine;
using BepInEx.Configuration;
using Jotunn.Utils;
using System.Reflection;
using Jotunn.Entities;
using Jotunn.Configs;
using Jotunn.Managers;
using System;


namespace AllTheExtras
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid, "2.0.12")]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class AllTheExtras : BaseUnityPlugin
    {
        public const string PluginGUID = "com.RockerKitten.AllTheExtras";
        public const string PluginName = "AllTheExtras";
        public const string PluginVersion = "0.0.1";
        public AssetBundle assetBundle;
        public EffectList buildStone;
        public EffectList buildWood;
        public EffectList buildTorch;
        public AudioSource torchVol;


        private void Awake()
        {

            AssetLoad();
            
            ItemManager.OnVanillaItemsAvailable += LoadSounds;

        }
        private void AssetLoad()
        {
            assetBundle = AssetUtils.LoadAssetBundleFromResources("misc", Assembly.GetExecutingAssembly());
        }
        private void LoadSounds()
        {
            try
            {
                var sfxstone = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_stone");
                var vfxstone = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_stone_wall_2x1");
                var sfxwood = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_wood");
                var vfxwood = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_wood_wall");
                var sfxmetal = PrefabManager.Cache.GetPrefab<GameObject>("sfx_build_hammer_metal");
                var vfxmetal = PrefabManager.Cache.GetPrefab<GameObject>("vfx_Place_wood_pole");
                 


                
                buildStone = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxstone }, new EffectList.EffectData { m_prefab = vfxstone } } };
                buildWood = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxwood }, new EffectList.EffectData { m_prefab = vfxwood } } };
                buildTorch = new EffectList { m_effectPrefabs = new EffectList.EffectData[2] { new EffectList.EffectData { m_prefab = sfxmetal }, new EffectList.EffectData { m_prefab = vfxmetal } } };

                Jotunn.Logger.LogMessage("Loaded Game VFX and SFX");


                LoadTorch();
                LoadWindow();
                LoadWindows();
                LoadBonfire();
                torchVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;
               
                
            }
            catch (Exception ex)
            {
                Jotunn.Logger.LogError($"Error while running OnVanillaLoad: {ex.Message}");
            }
            finally
            {
                ItemManager.OnVanillaItemsAvailable -= LoadSounds;

            }

        }
        public void LoadTorch()

        {
            var torchFab = assetBundle.LoadAsset<GameObject>("rk_torchnew");

            //piece_grill

            var torch = new CustomPiece(torchFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Wood", Amount = 1, Recover = true }
                    }

                });

            torchVol = torchFab.AddComponent<AudioSource>();
            torchVol.clip = assetBundle.LoadAsset<AudioClip>("torch_clip");
            torchVol.playOnAwake = true;
            torchVol.loop = true;
            torchVol.rolloffMode = AudioRolloffMode.Linear;
            //if (AudioMan.instance == null) Jotunn.Logger.LogError("lolwut");
            //else torchVol.outputAudioMixerGroup = mixer;




            //torchVol.outputAudioMixerGroup


            var torchEffect = torchFab.GetComponent<Piece>();
            torchEffect.m_placeEffect = buildTorch;
            PieceManager.Instance.AddPiece(torch);
        }

        /*AudioSource torchVol;
        {

        torchVol = torchFab.AddComponent<AudioSource>();
        torchVol.clip = assetBundle.LoadAsset<AudioClip>("torch_clip");
            torchVol.playOnAwake = true;
            torchVol.loop = true;
            torchVol.outputAudioMixerGroup = AudioMan.instance.m_ambientMixer;

        }*/
        private void LoadWindow()
        {
            var windowFab = assetBundle.LoadAsset<GameObject>("rk_window");


            var window = new CustomPiece(windowFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Wood", Amount = 1, Recover = true }
                    }

                });
            var winEffect = windowFab.GetComponent<Piece>();
            winEffect.m_placeEffect = buildWood;
            PieceManager.Instance.AddPiece(window);
        }
        private void LoadWindows()
        {
            var windowsFab = assetBundle.LoadAsset<GameObject>("rk_windowshort");
            var windows = new CustomPiece(windowsFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "Wood", Amount = 1, Recover = true }
                    }

                });
            var winsEffect = windowsFab.GetComponent<Piece>();
            winsEffect.m_placeEffect = buildWood;
            PieceManager.Instance.AddPiece(windows);
        }
        private void LoadBonfire()
        {
            var bonfireFab = assetBundle.LoadAsset<GameObject>("opl_bonfire");
            var bonfire = new CustomPiece(bonfireFab,
                new PieceConfig
                {
                    CraftingStation = "",
                    AllowedInDungeons = false,
                    Enabled = true,
                    PieceTable = "_HammerPieceTable",
                    Requirements = new[]
                    {
                        new RequirementConfig { Item = "stone", Amount = 1, Recover = true }
                    }

                });
            var bonfireEffect = bonfireFab.GetComponent<Piece>();
            bonfireEffect.m_placeEffect = buildStone;
            PieceManager.Instance.AddPiece(bonfire);
        }
      
    }
}