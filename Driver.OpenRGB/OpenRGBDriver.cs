using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using SimpleLed;
using Color = OpenRGB.NET.Models.Color;
using Image = System.Drawing.Image;
using Timer = System.Timers.Timer;

namespace Driver.OpenRGB
{
    public class OpenRGBDriver : ISimpleLed
    {
        public event Events.DeviceChangeEventHandler DeviceAdded;
        public event Events.DeviceChangeEventHandler DeviceRemoved;
        public OpenRGBClient client;

        public static Assembly myAssembly = Assembly.GetExecutingAssembly();
        public static Stream orgbImage = myAssembly.GetManifestResourceStream("Driver.OpenRGB.ORGB.png");


        public void Configure(DriverDetails driverDetails)
        {
            client = new OpenRGBClient(name: "RGB Sync Studio", autoconnect: true, timeout: 1000);

            var deviceCount = client.GetControllerCount();
            var devices = client.GetAllControllerData();

            for (int devId = 0; devId < devices.Length; devId++)
            {
                ORGBControlDevice slsDevice = new ORGBControlDevice();
                slsDevice.id = devId;
                slsDevice.Driver = this;
                slsDevice.Name = devices[devId].Name;
                slsDevice.DeviceType = DeviceTypeConverter.GetType(devices[devId].Type);
                slsDevice.Has2DSupport = false;
                slsDevice.ProductImage = (Bitmap)Image.FromStream(orgbImage);

                List<ControlDevice.LedUnit> deviceLeds = new List<ControlDevice.LedUnit>();

                int i = 0;
                foreach (Led orgbLed in devices[devId].Leds)
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
            ORGBControlDevice orgbControlDevice = controlDevice as ORGBControlDevice;
            Device orgbDevice = client.GetAllControllerData().First(dev => dev.Name == controlDevice.Name);
            var leds = Enumerable.Range(0, orgbDevice.Colors.Length)
                .Select(_ => new Color((byte) controlDevice.LEDs[0].Color.Red, (byte)controlDevice.LEDs[0].Color.Green, (byte)controlDevice.LEDs[0].Color.Blue))
                .ToArray();
            client.UpdateLeds(orgbControlDevice.id, leds);
        }

        public void PutConfig<T>(T config) where T : SLSConfigData
        {
            throw new NotImplementedException();
        }

        public class ORGBControlDevice : ControlDevice
        {
            public int id { get; set; }
        }
    }
}
