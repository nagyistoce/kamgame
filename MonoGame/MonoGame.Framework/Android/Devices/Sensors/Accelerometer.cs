// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using Android.Content;
using Android.Hardware;
using Microsoft.Xna.Framework;
using Object = Java.Lang.Object;

namespace Microsoft.Devices.Sensors
{
    /// <summary>
    ///     Provides Android applications access to the device�s accelerometer sensor.
    /// </summary>
    public sealed class Accelerometer : SensorBase<AccelerometerReading>
    {
        private static readonly int MaxSensorCount = 10;
        private static SensorManager sensorManager;
        private static Sensor sensor;
        private static int instanceCount;
        private readonly SensorListener listener;
        private bool started;
        private SensorState state;

        /// <summary>
        ///     Creates a new instance of the Accelerometer object.
        /// </summary>
        public Accelerometer()
        {
            if (instanceCount >= MaxSensorCount)
                throw new SensorFailedException(
                    "The limit of 10 simultaneous instances of the Accelerometer class per application has been exceeded.");
            ++instanceCount;

            state = sensor != null ? SensorState.Initializing : SensorState.NotSupported;
            listener = new SensorListener();
        }

        /// <summary>
        ///     Gets or sets whether the device on which the application is running supports the accelerometer sensor.
        /// </summary>
        public static bool IsSupported
        {
            get
            {
                if (sensorManager == null)
                    Initialize();
                return sensor != null;
            }
        }

        /// <summary>
        ///     Gets the current state of the accelerometer. The value is a member of the SensorState enumeration.
        /// </summary>
        public SensorState State
        {
            get
            {
                if (IsDisposed)
                    throw new ObjectDisposedException(GetType().Name);
                if (sensorManager == null)
                    Initialize();
                return state;
            }
        }

        /// <summary>
        ///     Initializes the platform resources required for the accelerometer sensor.
        /// </summary>
        private static void Initialize()
        {
            sensorManager = (SensorManager) Game.Activity.GetSystemService(Context.SensorService);
            sensor = sensorManager.GetDefaultSensor(SensorType.Accelerometer);
        }

        private void ActivityPaused(object sender, EventArgs eventArgs)
        {
            sensorManager.UnregisterListener(listener, sensor);
        }

        private void ActivityResumed(object sender, EventArgs eventArgs)
        {
            sensorManager.RegisterListener(listener, sensor, SensorDelay.Game);
        }

        /// <summary>
        ///     Starts data acquisition from the accelerometer.
        /// </summary>
        public override void Start()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
            if (sensorManager == null)
                Initialize();
            if (started == false)
            {
                if (sensorManager != null && sensor != null)
                {
                    listener.accelerometer = this;
                    sensorManager.RegisterListener(listener, sensor, SensorDelay.Game);
                    // So the system can pause and resume the sensor when the activity is paused
                    AndroidGameActivity.Paused += ActivityPaused;
                    AndroidGameActivity.Resumed += ActivityResumed;
                }
                else
                {
                    throw new AccelerometerFailedException(
                        "Failed to start accelerometer data acquisition. No default sensor found.", -1);
                }
                started = true;
                state = SensorState.Ready;
                return;
            }
            else
            {
                throw new AccelerometerFailedException(
                    "Failed to start accelerometer data acquisition. Data acquisition already started.", -1);
            }
        }

        /// <summary>
        ///     Stops data acquisition from the accelerometer.
        /// </summary>
        public override void Stop()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
            if (started)
            {
                if (sensorManager != null && sensor != null)
                {
                    AndroidGameActivity.Paused -= ActivityPaused;
                    AndroidGameActivity.Resumed -= ActivityResumed;
                    sensorManager.UnregisterListener(listener, sensor);
                    listener.accelerometer = null;
                }
            }
            started = false;
            state = SensorState.Disabled;
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (started)
                        Stop();
                    --instanceCount;
                    if (instanceCount == 0)
                    {
                        sensor = null;
                        sensorManager = null;
                    }
                }
            }
            base.Dispose(disposing);
        }

        private class SensorListener : Object, ISensorEventListener
        {
            internal Accelerometer accelerometer;

            public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
            {
                //do nothing
            }

            public void OnSensorChanged(SensorEvent e)
            {
                try
                {
                    if (e != null && e.Sensor.Type == SensorType.Accelerometer && accelerometer != null)
                    {
                        IList<float> values = e.Values;
                        try
                        {
                            var reading = new AccelerometerReading();
                            accelerometer.IsDataValid = (values != null && values.Count == 3);
                            if (accelerometer.IsDataValid)
                            {
                                reading.Acceleration = new Vector3(values[0], values[1], values[2]);
                                reading.Timestamp = DateTime.Now;
                            }
                            accelerometer.FireOnCurrentValueChanged(this,
                                                                    new SensorReadingEventArgs<AccelerometerReading>(
                                                                        reading));
                        }
                        finally
                        {
                            var d = values as IDisposable;
                            if (d != null)
                                d.Dispose();
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    //Occassionally an NullReferenceException is thrown when accessing e.Values??
                    // mono    : Unhandled Exception: System.NullReferenceException: Object reference not set to an instance of an object
                    // mono    :   at Android.Runtime.JNIEnv.GetObjectField (IntPtr jobject, IntPtr jfieldID) [0x00000] in <filename unknown>:0 
                    // mono    :   at Android.Hardware.SensorEvent.get_Values () [0x00000] in <filename unknown>:0
                }
            }
        }
    }
}