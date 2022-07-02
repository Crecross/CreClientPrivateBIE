using UnityEngine;

namespace CreClient.Modules
{
    [Module]
    public static class SpinBot
    {
        private static bool _enabled;
        [Configure<bool>("Movement.SpinBot", false, false)]
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
                        CameraMount.rotation = Quaternion.identity;
                }
            }
        }

        private static Transform CameraMount;

        private static void OnUpdate()
        {
            if (CameraMount == null)
            {
                Events.Update -= OnUpdate;
                return;
            }

            CameraMount.rotation = Quaternion.Euler(0, Time.time, 0);
        }
    }
}
