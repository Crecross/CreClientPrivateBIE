using UnityEngine;

namespace CreClient.Modules
{
    [Module]
    public static class SpinBot
    {
        private static bool _enabled;
        [Configure<bool>("Movement.SpinBot.Enabled", false, false)]
        public static bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled == value) return;
                _enabled = value;

                if (value)
                {
                    CameraMount = VRC.SDKBase.Networking.LocalPlayer.gameObject.transform.Find("CameraMount");

                    Events.Update += OnUpdate;
                }
                else
                {
                    Events.Update -= OnUpdate;

                    if (CameraMount != null)
                        CameraMount.localRotation = Quaternion.Euler(0,0,0);
                }
            }
        }

        [Configure<float>("Movement.SpinBot.Speed", 1)]
        public static float Speed;

        private static Transform CameraMount;

        private static void OnUpdate()
        {
            if (CameraMount == null)
            {
                Events.Update -= OnUpdate;
                return;
            }

            CameraMount.localRotation = Quaternion.Euler(0, Time.time * Speed * 360, 0);
        }
    }
}
