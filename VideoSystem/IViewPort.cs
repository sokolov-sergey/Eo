﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoSystem
{
    public interface IViewPort
    {
        void ProvideFrames(Action<Frame> action);
        int MaxFPS { get; }

        void ZoomIn(int x=0);
        void SetDeviceSize(int width, int height);
    }
}
