using CommunityToolkit.Mvvm.ComponentModel;
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
