using System;
using System.Collections.Generic;
using Artemis.Core;
using Artemis.Core.Modules;
using Artemis.Plugins.Audio.DataModelExpansion.DataModels;
using Artemis.Plugins.Audio.Services;
using NAudio.CoreAudioApi;
using Serilog;

namespace Artemis.Plugins.Audio.DataModelExpansion
{
    [PluginFeature]
    public class CaptureVolumeModule : AudioEndpointVolumeModule
    {
        #region Constructor

        public CaptureVolumeModule(ILogger logger, NAudioDeviceEnumerationService naudioDeviceEnumerationService, PluginSettings pluginSettings, NAudio.CoreAudioApi.Role role = Role.Communications) : base(logger, naudioDeviceEnumerationService, role, NAudio.CoreAudioApi.DataFlow.Capture)
        {
            _enableCaptureDeviceAccess  = pluginSettings.GetSetting("EnableCaptureDeviceAccess", true);
            _enableCaptureDeviceAccess.SettingChanged += _enableCaptureDeviceAccess_SettingChanged;
        }

        #endregion

        #region Properties and Fields

        internal readonly PluginSetting<bool> _enableCaptureDeviceAccess;
        internal NAudio.CoreAudioApi.WasapiCapture _wasapiCapture;

        #endregion

        #region Plugin Methods
        
        public override DataModelPropertyAttribute GetDataModelDescription()
        {
            return new DataModelPropertyAttribute
            {
                Name = "Capture Volume",
                Description = "Contains information about default communication capture device"
            };
        }

        private void _enableCaptureDeviceAccess_SettingChanged(object sender, EventArgs e)
        {
            _logger.Verbose($"EnableCaptureDeviceAccess setting change detected. Restarting data model connection.");
            _wasapiCapture?.StopRecording();
            _wasapiCapture?.Dispose();
            _wasapiCapture = null;
            _audioDeviceChanged = true;
            UpdateAudioEndpointDevice();
            if (_enableCaptureDeviceAccess.Value == true) {
                _wasapiCapture = new WasapiCapture(_audioDevice);
                //required to request access to the audio capture device
                _wasapiCapture.StartRecording();
            }
            _logger.Verbose($"EnableCaptureDeviceAccess setting change detected. Data model connection restarted.");
        }

        #endregion

        public override void Enable()
        {
            base.Enable();
            if (_enableCaptureDeviceAccess.Value == true) {
                _wasapiCapture = new WasapiCapture(_audioDevice);
                //required to request access to the audio capture device
                _wasapiCapture.StartRecording();
            }
        }

        public override void Disable()
        {
            _wasapiCapture?.StopRecording();
            _wasapiCapture?.Dispose();
            _wasapiCapture = null;
            base.Disable();
        }
    }
}