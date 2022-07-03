using System;
using UnityEngine;
using VRC.SDKBase;

namespace CreClient.Modules
{
    [Module]
    public static class AutoHead
    {
        private static bool _enabled;
        [Configure<bool>("Private.Auto Head.Enabled", false, false)]
        public static bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;

                if (value)
                {
                    TargetPlayer = VRCPlayerApi.AllPlayers.Find(
                        UnhollowerRuntimeLib.DelegateSupport.ConvertDelegate<Il2CppSystem.Predicate<VRCPlayerApi>>(
                            new Predicate<VRCPlayerApi>(player =>
#if MELONLOADER
                            player.displayName == (VRC.DataModel.UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_0
                                ?? VRC.DataModel.UserSelectionManager.field_Private_Static_UserSelectionManager_0.field_Private_APIUser_1).displayName
#elif BEPINEX
                                player.displayName == KiraiMod.Core.Types.UserSelectionManager.SelectedUser.displayName
#endif
                            )
                        )
                    );

                    

                    if (TargetPlayer is null)
                    {
                        Utils.SmartLogger.Message("No Target");
                        return;
                    }

                    HipOffset = Networking.LocalPlayer.gameObject.transform.position - Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips);

                    Events.Update += OnUpdate;
                }
                else Events.Update -= OnUpdate;

                _enabled = value;
            }
        }

        private static float _speed;
        [Configure<float>("Private.Auto Head.Speed", 8)]
        public static float Speed
        {
            get => _speed;
            set
            {
                if (_speed == value) return;
                _speed = value;

                TimeRebase = Time.time;
            }
        }

        [Configure<float>("Private.Auto Head.Offset", 0.25f)]
        public static float Offset;

        [Configure<float>("Private.Auto Head.Distance", 0.08f)]
        public static float Distance;

        private static Vector3 HipOffset;
        private static VRCPlayerApi TargetPlayer;
        private static float TimeRebase;

        private static void OnUpdate()
        {
            if (TargetPlayer == null) return;

            Networking.LocalPlayer.gameObject.transform.position =
                TargetPlayer.GetBonePosition(HumanBodyBones.Head)
                + HipOffset
                + TargetPlayer.gameObject.transform.forward
                * (float)(Math.Sin(TimeRebase + (Time.time - TimeRebase) * Speed) * Distance + Offset);
        }
    }
}
