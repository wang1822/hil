using System.Windows.Input;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HardwareSimulator.Services;

namespace HardwareSimulator.ViewModels
{
    /// <summary>
    /// 主窗口视图模型
    /// </summary>
    public partial class MainViewModel : ObservableObject
    {
        private readonly DataSimulationService _simulationService;
        private readonly DispatcherTimer _timer;

        [ObservableProperty]
        private PcsViewModel _pcsViewModel = new();

        [ObservableProperty]
        private BmsViewModel _bmsViewModel = new();

        [ObservableProperty]
        private AirConditionerViewModel _acViewModel = new();

        [ObservableProperty]
        private int _selectedModuleIndex = 0;

        [ObservableProperty]
        private bool _isSimulating;

        [ObservableProperty]
        private int _updateInterval = 1000;

        [ObservableProperty]
        private string _simulationButtonText = "开始模拟";

        [ObservableProperty]
        private DateTime _lastUpdateTime;

        [ObservableProperty]
        private int _updateCount;

        public MainViewModel()
        {
            _simulationService = new DataSimulationService();
            
            // 初始化数据
            _simulationService.InitializePcsData(PcsViewModel.Data);
            _simulationService.InitializeBmsData(BmsViewModel.Data);
            _simulationService.InitializeAcData(AcViewModel.Data);

            // 初始化定时器
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(UpdateInterval)
            };
            _timer.Tick += Timer_Tick;

            LastUpdateTime = DateTime.Now;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // 模拟数据更新
            _simulationService.SimulatePcsData(PcsViewModel.Data);
            _simulationService.SimulateBmsData(BmsViewModel.Data);
            _simulationService.SimulateAcData(AcViewModel.Data);

            // 通知属性更新
            OnPropertyChanged(nameof(PcsViewModel));
            OnPropertyChanged(nameof(BmsViewModel));
            OnPropertyChanged(nameof(AcViewModel));

            LastUpdateTime = DateTime.Now;
            UpdateCount++;
        }

        [RelayCommand]
        private void ToggleSimulation()
        {
            if (IsSimulating)
            {
                StopSimulation();
            }
            else
            {
                StartSimulation();
            }
        }

        [RelayCommand]
        private void StartSimulation()
        {
            if (!IsSimulating)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(UpdateInterval);
                _timer.Start();
                IsSimulating = true;
                SimulationButtonText = "停止模拟";
                PcsViewModel.IsSimulating = true;
                BmsViewModel.IsSimulating = true;
                AcViewModel.IsSimulating = true;
            }
        }

        [RelayCommand]
        private void StopSimulation()
        {
            if (IsSimulating)
            {
                _timer.Stop();
                IsSimulating = false;
                SimulationButtonText = "开始模拟";
                PcsViewModel.IsSimulating = false;
                BmsViewModel.IsSimulating = false;
                AcViewModel.IsSimulating = false;
            }
        }

        [RelayCommand]
        private void ResetData()
        {
            _simulationService.InitializePcsData(PcsViewModel.Data);
            _simulationService.InitializeBmsData(BmsViewModel.Data);
            _simulationService.InitializeAcData(AcViewModel.Data);
            UpdateCount = 0;
            LastUpdateTime = DateTime.Now;

            OnPropertyChanged(nameof(PcsViewModel));
            OnPropertyChanged(nameof(BmsViewModel));
            OnPropertyChanged(nameof(AcViewModel));
        }

        [RelayCommand]
        private void SetUpdateInterval(string intervalStr)
        {
            if (int.TryParse(intervalStr, out int interval) && interval >= 100)
            {
                UpdateInterval = interval;
                _timer.Interval = TimeSpan.FromMilliseconds(UpdateInterval);
            }
        }

        partial void OnUpdateIntervalChanged(int value)
        {
            if (_timer != null && value >= 100)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(value);
            }
        }

        [RelayCommand]
        private void SelectModule(string moduleIndexStr)
        {
            if (int.TryParse(moduleIndexStr, out int moduleIndex))
            {
                SelectedModuleIndex = moduleIndex;
            }
        }
    }
}
