using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HardwareSimulator.Models;

namespace HardwareSimulator.ViewModels
{
    /// <summary>
    /// BMS数据视图模型
    /// </summary>
    public partial class BmsViewModel : ObservableObject
    {
        [ObservableProperty]
        private BmsData _data = new();

        [ObservableProperty]
        private bool _isSimulating;

        /// <summary>
        /// 设定的目标SOC (%)
        /// </summary>
        [ObservableProperty]
        private double _setTargetSoc = 80;

        /// <summary>
        /// 设定的充电电流限制 (A)
        /// </summary>
        [ObservableProperty]
        private double _setChargingCurrentLimit = 100;

        /// <summary>
        /// 设定的放电电流限制 (A)
        /// </summary>
        [ObservableProperty]
        private double _setDischargingCurrentLimit = 100;

        /// <summary>
        /// 应用参数设置到设备
        /// </summary>
        [RelayCommand]
        private void ApplySettings()
        {
            // 模拟将设置参数下发到BMS设备
            // 在实际应用中，这里会通过通信协议发送到真实设备
            // 这里我们设置目标SOC影响可用能量计算
            Data.AvailableEnergy = Data.TotalEnergy * SetTargetSoc / 100;
        }

        /// <summary>
        /// 从设备读取当前参数
        /// </summary>
        [RelayCommand]
        private void ReadFromDevice()
        {
            SetTargetSoc = Data.Soc;
            SetChargingCurrentLimit = Math.Abs(Data.TotalCurrent);
            SetDischargingCurrentLimit = Math.Abs(Data.TotalCurrent);
        }

        public string StatusText => Data.Status switch
        {
            BmsStatus.Idle => "空闲",
            BmsStatus.Charging => "充电中",
            BmsStatus.Discharging => "放电中",
            BmsStatus.Balancing => "均衡中",
            BmsStatus.Fault => "故障",
            BmsStatus.Protection => "保护",
            _ => "未知"
        };

        public string AlarmLevelText => Data.AlarmLevel switch
        {
            AlarmLevel.Normal => "正常",
            AlarmLevel.Warning => "警告",
            AlarmLevel.Alarm => "告警",
            AlarmLevel.Critical => "严重",
            _ => "未知"
        };

        public string StatusColor => Data.Status switch
        {
            BmsStatus.Idle => "#9E9E9E",
            BmsStatus.Charging => "#2196F3",
            BmsStatus.Discharging => "#4CAF50",
            BmsStatus.Balancing => "#FF9800",
            BmsStatus.Fault => "#F44336",
            BmsStatus.Protection => "#E91E63",
            _ => "#9E9E9E"
        };

        public string AlarmColor => Data.AlarmLevel switch
        {
            AlarmLevel.Normal => "#4CAF50",
            AlarmLevel.Warning => "#FF9800",
            AlarmLevel.Alarm => "#F44336",
            AlarmLevel.Critical => "#B71C1C",
            _ => "#9E9E9E"
        };
    }
}
