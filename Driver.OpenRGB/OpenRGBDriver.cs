using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using SimpleLed;

namespace Driver.OpenRGB
{
    public class OpenRGBDriver : ISimpleLed
    {
        public event Events.DeviceChangeEventHandler DeviceAdded;
        public event Events.DeviceChangeEventHandler DeviceRemoved;
        public OpenRGBClient client;

        public void Configure(DriverDetails driverDetails)
        {
            client = new OpenRGBClient(name: "RGB Sync Studio", autoconnect: true, timeout: 1000);

            var deviceCount = client.GetControllerCount();
            var devices = client.GetAllControllerData();

            foreach (Device orgbDevice in devices)
            {
                ControlDevice slsDevice = new ControlDevice();
                slsDevice.Driver = this;
                slsDevice.Name = orgbDevice.Name;
                slsDevice.DeviceType = DeviceTypeConverter.GetType(orgbDevice.Type);
                slsDevice.Has2DSupport = false;

                List<ControlDevice.LedUnit> deviceLeds = new List<ControlDevice.LedUnit>();

                int i = 0;
                foreach (Led orgbLed in orgbDevice.Leds)
                {
                    ControlDevice.LedUnit slsLed = new ControlDevice.LedUnit();
                    slsLed.LEDName = orgbLed.Name;
                    deviceLeds.Add(slsLed);
                }

                slsDevice.LEDs = deviceLeds.ToArray();

                DeviceAdded?.Invoke(slsDevice, new Events.DeviceChangeEventArgs(slsDevice));
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T GetConfig<T>() where T : SLSConfigData
        {
            throw new NotImplementedException();
        }

        public DriverProperties GetProperties()
        {
            return new DriverProperties
            {
                SupportsPull = false,
                SupportsPush = true,
                IsSource = false,
                SupportsCustomConfig = false,
                Id = Guid.Parse("2594d8f2-9db5-4efa-a631-7ca2a021fb50"),
                Author = "Fanman03",
                Blurb = "Driver that allows OpenRGB devices to be controlled.",
                CurrentVersion = new ReleaseNumber(1, 0, 0, 1),
                GitHubLink = "https://github.com/SimpleLed/Driver.OpenRGB",
                IsPublicRelease = true,
            };
        }

        public void InterestedUSBChange(int VID, int PID, bool connected)
        {
            throw new NotImplementedException();
        }

        public string Name()
        {
            return "OpenRGB";
        }

        public void Pull(ControlDevice controlDevice)
        {
            throw new NotImplementedException();
        }

        public void Push(ControlDevice controlDevice)
        {
            foreach (ControlDevice.LedUnit slsLED in controlDevice.LEDs)
            {
                //TODO figure out how to update LED color
            }
        }

        public void PutConfig<T>(T config) where T : SLSConfigData
        {
            throw new NotImplementedException();
        }
    }
}
