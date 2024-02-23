﻿using Artemis.Core;
using Artemis.UI.Shared;
using Avalonia;
using Humanizer;

namespace Artemis.Plugins.Nodes.General.Nodes.Transition.Screens;

public class EasingFunctionViewModel : ViewModelBase
{
    public EasingFunctionViewModel(Easings.Functions easingFunction)
    {
        EasingFunction = easingFunction;
        Description = easingFunction.Humanize();

        EasingPoints = new List<Point>();
        for (int i = 1; i <= 10; i++)
        {
            double y = Easings.Interpolate(i / 10.0, EasingFunction) * 10;
            EasingPoints.Add(new Point(i, y));
        }
    }

    public Easings.Functions EasingFunction { get; }
    public List<Point> EasingPoints { get; }
    public string Description { get; }
}