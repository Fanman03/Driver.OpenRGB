using OpenRGB.NET.Enums;
using SimpleLed;

namespace Driver.OpenRGB
{
    class DeviceTypeConverter
    {
        public static string GetType(DeviceType type)
        {
            switch (type)
            {
                case DeviceType.Keyboard:
                    return DeviceTypes.Keyboard;
                case DeviceType.Mouse:
                    return DeviceTypes.Mouse;
                case DeviceType.Mousemat:
                    return DeviceTypes.MousePad;
                case DeviceType.Headset:
                    return DeviceTypes.Headset;
                case DeviceType.HeadsetStand:
                    return DeviceTypes.HeadsetStand;
                case DeviceType.Cooler:
                    return DeviceTypes.Cooler;
                case DeviceType.Gpu:
                    return DeviceTypes.GPU;
                case DeviceType.Motherboard:
                    return DeviceTypes.MotherBoard;
                case DeviceType.Dram:
                    return DeviceTypes.Memory;
                case DeviceType.Ledstrip:
                    return DeviceTypes.LedStrip;
                case DeviceType.Light:
                    return DeviceTypes.Bulb;
                default:
                    return DeviceTypes.Other;
            }
        }
    }
}
