﻿using System.IO;
using Artemis.Core;
using Artemis.Core.DeviceProviders;
using Artemis.Core.Services;

namespace Artemis.Plugins.Devices.CoolerMaster
{
    // ReSharper disable once UnusedMember.Global
    [PluginFeature(Name = "CoolerMaster Device Provider")]
    public class CoolerMasterDeviceProvider : DeviceProvider
    {
        private readonly IRgbService _rgbService;

        public CoolerMasterDeviceProvider(IRgbService rgbService) : base(RGB.NET.Devices.CoolerMaster.CoolerMasterDeviceProvider.Instance)
        {
            _rgbService = rgbService;
        }

        public override void Enable()
        {
            RGB.NET.Devices.CoolerMaster.CoolerMasterDeviceProvider.PossibleX64NativePaths.Add(Path.Combine(Plugin.Directory.FullName, "x64", "CMSDK.dll"));
            RGB.NET.Devices.CoolerMaster.CoolerMasterDeviceProvider.PossibleX86NativePaths.Add(Path.Combine(Plugin.Directory.FullName, "x86", "CMSDK.dll"));
            _rgbService.AddDeviceProvider(RgbDeviceProvider);
        }

        public override void Disable()
        {
            _rgbService.RemoveDeviceProvider(RgbDeviceProvider);
            RgbDeviceProvider.Dispose();
        }
    }
}