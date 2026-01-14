using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HardwareSimulator.Models;

namespace HardwareSimulator.ViewModels
{
    /// <summary>
    /// 空调数据视图模型
    /// </summary>
    public partial class AirConditionerViewModel : ObservableObject
    {
        [ObservableProperty]
        private AirConditionerData _data = new();

        [ObservableProperty]
        private bool _isSimulating;

        /// <summary>
        /// 设定温度值 (°C)
        /// </summary>
        [ObservableProperty]
        private double _newSetTemperature = 25;

        /// <summary>
        /// 设定的运行模式
        /// </summary>
        [ObservableProperty]
        private AcMode _setMode = AcMode.Cooling;

        /// <summary>
        /// 设定运行模式的索引（用于ComboBox绑定）
        /// </summary>
        public int SetModeIndex
        {
            get => (int)SetMode;
            set
            {
                if (value >= 0 && value <= 4)
                {
                    SetMode = (AcMode)value;
                }
            }
        }

        /// <summary>
        /// 设定的风速档位
        /// </summary>
        [ObservableProperty]
        private FanSpeed _setFanSpeed = FanSpeed.Auto;

        /// <summary>
        /// 设定风速档位的索引（用于ComboBox绑定）
        /// </summary>
        public int SetFanSpeedIndex
        {
            get => (int)SetFanSpeed;
            set
            {
                if (value >= 0 && value <= 3)
                {
                    SetFanSpeed = (FanSpeed)value;
                }
            }
        }

        /// <summary>
        /// 设定的开关状态
        /// </summary>
        [ObservableProperty]
        private bool _setPowerOn = true;

        /// <summary>
        /// 应用参数设置到设备
        /// </summary>
        [RelayCommand]
        private void ApplySettings()
        {
            Data.SetTemperature = NewSetTemperature;
            Data.Mode = SetMode;
            Data.FanSpeedLevel = SetFanSpeed;
            Data.Status = SetPowerOn ? AcStatus.Running : AcStatus.Off;
        }

        /// <summary>
        /// 从设备读取当前参数
        /// </summary>
        [RelayCommand]
        private void ReadFromDevice()
        {
            NewSetTemperature = Data.SetTemperature;
            SetMode = Data.Mode;
            SetFanSpeed = Data.FanSpeedLevel;
            SetPowerOn = Data.Status == AcStatus.Running;
        }

        public string StatusText => Data.Status switch
        {
            AcStatus.Off => "关机",
            AcStatus.Running => "运行",
            AcStatus.Standby => "待机",
            AcStatus.Defrosting => "除霜",
            AcStatus.Fault => "故障",
            _ => "未知"
        };

        public string ModeText => Data.Mode switch
        {
            AcMode.Cooling => "制冷",
            AcMode.Heating => "制热",
            AcMode.Dehumidifying => "除湿",
            AcMode.Ventilation => "通风",
            AcMode.Auto => "自动",
            _ => "未知"
        };

        public string FanSpeedText => Data.FanSpeedLevel switch
        {
            FanSpeed.Low => "低速",
            FanSpeed.Medium => "中速",
            FanSpeed.High => "高速",
            FanSpeed.Auto => "自动",
            _ => "未知"
        };

        public string StatusColor => Data.Status switch
        {
            AcStatus.Running => "#4CAF50",
            AcStatus.Standby => "#FF9800",
            AcStatus.Fault => "#F44336",
            AcStatus.Off => "#9E9E9E",
            AcStatus.Defrosting => "#2196F3",
            _ => "#9E9E9E"
        };

        public string ModeColor => Data.Mode switch
        {
            AcMode.Cooling => "#2196F3",
            AcMode.Heating => "#F44336",
            AcMode.Dehumidifying => "#00BCD4",
            AcMode.Ventilation => "#8BC34A",
            AcMode.Auto => "#9C27B0",
            _ => "#9E9E9E"
        };
    }
}
