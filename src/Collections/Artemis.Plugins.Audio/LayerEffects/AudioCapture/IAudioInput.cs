﻿using System;

namespace Artemis.Plugins.LayerEffects.AudioVisualization.AudioCapture
{
    public delegate void AudioData(float left, float right);

    public interface IAudioInput : IDisposable
    {
        int SampleRate { get; }
        float MasterVolume { get; }

        event AudioData DataAvailable;

        void Initialize();
    }
}
