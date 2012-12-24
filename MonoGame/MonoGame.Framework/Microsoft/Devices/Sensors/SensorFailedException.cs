// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;

namespace Microsoft.Devices.Sensors
{
    public class SensorFailedException : Exception
    {
        internal SensorFailedException(string message)
            : base(message)
        {
        }

        public int ErrorId { get; protected set; }
    }
}