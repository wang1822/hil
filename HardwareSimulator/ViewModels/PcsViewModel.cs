using CommunityToolkit.Mvvm.ComponentModel;
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
