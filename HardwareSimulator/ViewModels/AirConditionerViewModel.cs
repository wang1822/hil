using CommunityToolkit.Mvvm.ComponentModel;
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
