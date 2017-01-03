﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;

namespace HapticGlove
{
    public class Glove
    {
        private const string BLE_DEVICE_FILTER = "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"";
        private const string DEVICE_NAME = "NotionTheory Haptic Glove";

        public static Glove DEFAULT;

        private static DeviceWatcher watcher;
        private static Dictionary<string, DeviceInformation> devices;

        static Glove()
        {
            DEFAULT = new Glove();

            devices = new Dictionary<string, DeviceInformation>();

            // This isn't strictly necessary, but it's helpful when trying to figure out what the Adafruit Feather M0 is doing.
            new GATTDefaultService("Nordic UART", new Guid("{6e400001-b5a3-f393-e0a9-e50e24dcca9e}"));
            new GATTDefaultCharacteristic("RX Buffer", new Guid("{6e400003-b5a3-f393-e0a9-e50e24dcca9e}"));
            new GATTDefaultCharacteristic("TX Buffer", new Guid("{6e400002-b5a3-f393-e0a9-e50e24dcca9e}"));
            new GATTDefaultService("Nordic Device Firmware Update Service", new Guid("{00001530-1212-efde-1523-785feabcd123}"));
            new GATTDefaultCharacteristic("DFU Packet", new Guid("{00001532-1212-efde-1523-785feabcd123}"));
            new GATTDefaultCharacteristic("DFU Control Point", new Guid("{00001531-1212-efde-1523-785feabcd123}"));
            new GATTDefaultCharacteristic("DFU Version", new Guid("{00001534-1212-efde-1523-785feabcd123}"));
        }

        public static void Search()
        {
            devices.Clear();

            watcher = DeviceInformation.CreateWatcher(BLE_DEVICE_FILTER, null, DeviceInformationKind.AssociationEndpoint);
            watcher.Added += Watcher_Added;
            watcher.Updated += Watcher_Updated;
            watcher.Removed += Watcher_Removed;
            watcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            watcher.Stopped += Watcher_Stopped;
            watcher.Start();
        }

        private static void Watcher_Added(DeviceWatcher sender, DeviceInformation device)
        {
            if(!devices.ContainsKey(device.Id))
            {
                devices.Add(device.Id, device);
                Connect(device);
            }
        }

        private static async void Connect(DeviceInformation device)
        {
            if(device.Name == DEVICE_NAME)
            {
                if(!device.Pairing.IsPaired && device.Pairing.CanPair)
                {
                    await device.Pairing.PairAsync();
                }
                else if(device.Pairing.IsPaired && DEFAULT.State == GloveState.NotReady)
                {
                    await DEFAULT.Connect();
                }
            }
        }

        private static void Watcher_Updated(DeviceWatcher sender, DeviceInformationUpdate deviceUpdate)
        {
            if(devices.ContainsKey(deviceUpdate.Id))
            {
                var device = devices[deviceUpdate.Id];
                device.Update(deviceUpdate);
                Connect(device);
            }
        }

        private static void Watcher_Removed(DeviceWatcher sender, DeviceInformationUpdate deviceUpdate)
        {
            if(devices.ContainsKey(deviceUpdate.Id))
            {
                devices[deviceUpdate.Id].Update(deviceUpdate);
                devices.Remove(deviceUpdate.Id);
            }
        }

        private static void Watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            watcher.Added -= Watcher_Added;
            watcher.Updated -= Watcher_Updated;
            watcher.Removed -= Watcher_Removed;
            watcher.EnumerationCompleted -= Watcher_EnumerationCompleted;
            if(watcher.Status == DeviceWatcherStatus.Started || watcher.Status == DeviceWatcherStatus.EnumerationCompleted)
            {
                watcher.Stop();
            }
        }

        private static async void Watcher_Stopped(DeviceWatcher sender, object args)
        {
            watcher.Stopped -= Watcher_Stopped;
            watcher = null;
            foreach(var device in devices.Values)
            {
                if(device.Name == Glove.DEVICE_NAME)
                {
                    if(!device.Pairing.IsPaired)
                    {
                        await device.Pairing.PairAsync(DevicePairingProtectionLevel.None);
                    }
                    await DEFAULT.Connect();
                    var props = device.Properties.ToArray();
                }
            }
        }

        public static byte GetByte(IBuffer stream)
        {
            var buffer = new byte[stream.Length];
            DataReader.FromBuffer(stream).ReadBytes(buffer);
            var b = buffer[0];
            return b;
        }

        public static string GetString(IBuffer stream)
        {
            var buffer = new byte[stream.Length];
            DataReader.FromBuffer(stream).ReadBytes(buffer);
            return Encoding.ASCII.GetString(buffer, 0, buffer.Length);
        }

        public static async Task<string> GetDescription(GattCharacteristic c)
        {
            var descriptor = c.GetDescriptors(GATTDefaultCharacteristic.CharacteristicUserDescription.UUID).FirstOrDefault();
            if(descriptor == null)
            {
                return null;
            }
            else
            {
                var stream = await descriptor.ReadValueAsync();
                return GetString(stream.Value);
            }
        }

        public static async Task<byte> GetValue(GattCharacteristic c)
        {
            var result = await c.ReadValueAsync();
            return GetByte(result.Value);
        }

        public GloveState State
        {
            get;
            private set;
        }

        public float Battery
        {
            get;
            private set;
        }

        public Exception Error
        {
            get;
            private set;
        }

        public FingerState Fingers
        {
            get;
            private set;
        }

        public MotorState Motors
        {
            get;
            private set;
        }

        public string Manufacturer
        {
            get
            {
                string value = null;
                this.properties.TryGetValue("Manufacturer Name String", out value);
                return value;
            }
        }

        public string ModelNumber
        {
            get
            {
                string value = null;
                this.properties.TryGetValue("Model Number String", out value);
                return value;
            }
        }

        public string SoftwareRevision
        {
            get
            {
                string value = null;
                this.properties.TryGetValue("Software Revision String", out value);
                return value;
            }
        }

        public string FirmwareRevision
        {
            get
            {
                string value = null;
                this.properties.TryGetValue("Firmware Revision String", out value);
                return value;
            }
        }

        public string HardwareRevision
        {
            get
            {
                string value = null;
                this.properties.TryGetValue("Hardware Revision String", out value);
                return value;
            }
        }

        private GattCharacteristic batteryCharacteristic;
        private Dictionary<string, string> properties;

        public Glove()
        {
            this.Fingers = new FingerState();
            this.Motors = new MotorState();
            this.State = GloveState.NotReady;
            this.properties = new Dictionary<string, string>();
        }

        private static GATTDefaultService[] SERVICES = new GATTDefaultService[]
        {
            GATTDefaultService.BatteryService
        };

        private async Task<DeviceInformation> GetService(GATTDefaultService service)
        {
            return (from dev in await DeviceInformation.FindAllAsync(service.Filter)
                    where dev.Name == DEVICE_NAME
                    select dev).FirstOrDefault();
        }

        private async Task Connect()
        {
            this.Error = null;
            this.State = GloveState.Searching;
            try
            {
                var deviceInformationService = await GetService(GATTDefaultService.DeviceInformation);
                if(deviceInformationService != null)
                {
                    this.State |= GloveState.DeviceInformationFound;
                    ReadDeviceInformation(await GattDeviceService.FromIdAsync(deviceInformationService.Id));
                }

                var batteryService = await GetService(GATTDefaultService.BatteryService);
                if(batteryService != null)
                {
                    this.State |= GloveState.ServiceFound;
                    ReadBatteryService(await GattDeviceService.FromIdAsync(batteryService.Id));
                }
            }
            catch(Exception exp)
            {
                this.Error = exp;
            }
            finally
            {
                this.State ^= GloveState.Searching;
            }
        }

        private async void ReadDeviceInformation(GattDeviceService genericAccess)
        {
            var characteristics = genericAccess.GetAllCharacteristics();
            foreach(var characteristic in characteristics)
            {
                if(characteristic.CharacteristicProperties.HasFlag(GattCharacteristicProperties.Read))
                {
                    var description = await GetDescription(characteristic) ?? GATTDefaultCharacteristic.Find(characteristic.Uuid)?.Description ?? characteristic.Uuid.ToString();
                    if(!this.properties.ContainsKey(description))
                    {
                        var result = await characteristic.ReadValueAsync();
                        var value = GetString(result.Value);
                        this.properties.Add(description, value);
                    }
                }
            }
        }

        private async void ReadBatteryService(GattDeviceService deviceService)
        {
            var characteristics = deviceService.GetAllCharacteristics();
            if(characteristics.Count > 0)
            {
                this.State |= GloveState.CharacteristicsFound;
            }
            foreach(var characteristic in characteristics)
            {
                if(characteristic.Uuid == GATTDefaultCharacteristic.BatteryLevel.UUID)
                {
                    this.batteryCharacteristic = characteristic;
                    await this.batteryCharacteristic.WriteClientCharacteristicConfigurationDescriptorAsync(GattClientCharacteristicConfigurationDescriptorValue.Notify);
                    this.batteryCharacteristic.ValueChanged += BatteryCharacteristic_ValueChanged;
                    this.State |= GloveState.BatteryFound;
                }
                else
                {
                    var description = await GetDescription(characteristic);
                    if(description != null)
                    {
                        if(description.StartsWith("Motor "))
                        {
                            await this.Motors.Connect(description, characteristic);
                            if(this.Motors.Ready)
                            {
                                this.State |= GloveState.MotorsFound;
                            }
                        }
                        else if(description.StartsWith("Sensor "))
                        {
                            await this.Fingers.Connect(description, characteristic);
                            if(this.Fingers.Ready)
                            {
                                this.State |= GloveState.FingersFound;
                            }
                        }
                    }
                }
            }
        }

        private void BatteryCharacteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            this.Battery = GetByte(args.CharacteristicValue);
        }
    }
}