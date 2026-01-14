using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HardwareSimulator.Models;

namespace HardwareSimulator.ViewModels
{
    /// <summary>
    /// PCS数据视图模型
    /// </summary>
    public partial class PcsViewModel : ObservableObject
    {
        [ObservableProperty]
        private PcsData _data = new();

        [ObservableProperty]
        private bool _isSimulating;

        /// <summary>
        /// 设定的工作模式（用于参数下发）
        /// </summary>
        [ObservableProperty]
        private PcsWorkMode _setWorkMode = PcsWorkMode.Discharging;

        /// <summary>
        /// 设定工作模式的索引（用于ComboBox绑定）
        /// </summary>
        public int SetWorkModeIndex
        {
            get => (int)SetWorkMode;
            set
            {
                if (value >= 0 && value <= 4)
                {
                    SetWorkMode = (PcsWorkMode)value;
                }
            }
        }

        /// <summary>
        /// 设定的直流电压目标值 (V)
        /// </summary>
        [ObservableProperty]
        private double _setDcVoltage = 750;

        /// <summary>
        /// 设定的功率目标值 (kW)
        /// </summary>
        [ObservableProperty]
        private double _setPower = 50;

        /// <summary>
        /// 应用参数设置到设备
        /// </summary>
        [RelayCommand]
        private void ApplySettings()
        {
            Data.WorkMode = SetWorkMode;
            Data.DcVoltage = SetDcVoltage;
            Data.DcPower = SetDcVoltage * Data.DcCurrent / 1000;
        }

        /// <summary>
        /// 从设备读取当前参数
        /// </summary>
        [RelayCommand]
        private void ReadFromDevice()
        {
            SetWorkMode = Data.WorkMode;
            SetDcVoltage = Data.DcVoltage;
            SetPower = Data.DcPower;
        }

        public string StatusText => Data.Status switch
        {
            PcsStatus.Stopped => "停机",
            PcsStatus.Running => "运行",
            PcsStatus.Standby => "待机",
            PcsStatus.Fault => "故障",
            PcsStatus.Maintenance => "维护",
            _ => "未知"
        };

        public string WorkModeText => Data.WorkMode switch
        {
            PcsWorkMode.Charging => "充电",
            PcsWorkMode.Discharging => "放电",
            PcsWorkMode.Idle => "空闲",
            PcsWorkMode.GridConnected => "并网",
            PcsWorkMode.OffGrid => "离网",
            _ => "未知"
        };

        public string StatusColor => Data.Status switch
        {
            PcsStatus.Running => "#4CAF50",
            PcsStatus.Standby => "#FF9800",
            PcsStatus.Fault => "#F44336",
            PcsStatus.Stopped => "#9E9E9E",
            PcsStatus.Maintenance => "#2196F3",
            _ => "#9E9E9E"
        };
    }
}
