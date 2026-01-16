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
        private const int MinIntervalMs = 100; // 最小更新间隔（毫秒）
        
        private readonly DataSimulationService _simulationService;
        private readonly ModbusTcpService _modbusTcpService;
        private readonly DispatcherTimer _timer;
        private readonly DispatcherTimer _sendTimer;

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

        // Modbus TCP 连接配置
        [ObservableProperty]
        private string _modbusIpAddress = "192.168.1.100";

        [ObservableProperty]
        private int _modbusPort = 502;

        [ObservableProperty]
        private byte _modbusSlaveId = 1;

        [ObservableProperty]
        private int _sendInterval = 1000;

        [ObservableProperty]
        private bool _isConnected;

        [ObservableProperty]
        private string _connectionStatusText = "未连接";

        [ObservableProperty]
        private string _connectionStatusColor = "#F44336";

        [ObservableProperty]
        private int _sendCount;

        [ObservableProperty]
        private int _sendSuccessCount;

        [ObservableProperty]
        private int _sendFailureCount;

        [ObservableProperty]
        private DateTime _lastSendTime;

        [ObservableProperty]
        private bool _autoSendEnabled;

        public MainViewModel()
        {
            _simulationService = new DataSimulationService();
            _modbusTcpService = new ModbusTcpService();
            
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

            // 初始化数据发送定时器
            _sendTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(SendInterval)
            };
            _sendTimer.Tick += SendTimer_Tick;

            LastUpdateTime = DateTime.Now;
            LastSendTime = DateTime.Now;
        }

        private async void SendTimer_Tick(object? sender, EventArgs e)
        {
            if (!IsConnected || !AutoSendEnabled)
                return;

            SendCount++;

            // 发送所有数据，失败时重试最多3次
            int retryCount = 0;
            bool success = false;

            while (retryCount < 3 && !success)
            {
                try
                {
                    var pcsTask = _modbusTcpService.SendPcsDataAsync(PcsViewModel.Data, ModbusSlaveId);
                    var bmsTask = _modbusTcpService.SendBmsDataAsync(BmsViewModel.Data, ModbusSlaveId);
                    var acTask = _modbusTcpService.SendAcDataAsync(AcViewModel.Data, ModbusSlaveId);

                    await Task.WhenAll(pcsTask, bmsTask, acTask);

                    if (pcsTask.Result && bmsTask.Result && acTask.Result)
                    {
                        success = true;
                        SendSuccessCount++;
                        LastSendTime = DateTime.Now;
                    }
                    else
                    {
                        retryCount++;
                    }
                }
                catch (Exception ex)
                {
                    retryCount++;
                    // 记录异常信息用于调试（在生产环境应使用日志系统）
                    System.Diagnostics.Debug.WriteLine($"数据发送失败: {ex.Message}");
                }
            }

            if (!success)
            {
                SendFailureCount++;
                // 连接失败，尝试重连
                _ = Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    await ConnectAsync();
                });
            }
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
            if (int.TryParse(intervalStr, out int interval) && interval >= MinIntervalMs)
            {
                UpdateInterval = interval;
                _timer.Interval = TimeSpan.FromMilliseconds(UpdateInterval);
            }
        }

        partial void OnUpdateIntervalChanged(int value)
        {
            if (_timer != null && value >= MinIntervalMs)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(value);
            }
        }

        partial void OnSendIntervalChanged(int value)
        {
            if (_sendTimer != null && value >= MinIntervalMs)
            {
                _sendTimer.Interval = TimeSpan.FromMilliseconds(value);
            }
        }

        [RelayCommand]
        private void SelectModule(string moduleIndexStr)
        {
            if (int.TryParse(moduleIndexStr, out int moduleIndex) && moduleIndex >= 0 && moduleIndex <= 3)
            {
                SelectedModuleIndex = moduleIndex;
            }
        }

        [RelayCommand]
        private async Task ConnectAsync()
        {
            if (IsConnected)
            {
                // 已连接，执行断开操作
                DisconnectFromModbus();
                return;
            }

            // 尝试连接
            bool success = await _modbusTcpService.ConnectAsync(ModbusIpAddress, ModbusPort);

            if (success)
            {
                IsConnected = true;
                ConnectionStatusText = "已连接";
                ConnectionStatusColor = "#4CAF50";

                // 启动自动发送
                if (!AutoSendEnabled)
                {
                    AutoSendEnabled = true;
                    _sendTimer.Start();
                }
            }
            else
            {
                IsConnected = false;
                ConnectionStatusText = "连接失败";
                ConnectionStatusColor = "#F44336";
            }
        }

        [RelayCommand]
        private void DisconnectFromModbus()
        {
            if (_sendTimer.IsEnabled)
            {
                _sendTimer.Stop();
            }

            AutoSendEnabled = false;
            _modbusTcpService.Disconnect();
            IsConnected = false;
            ConnectionStatusText = "未连接";
            ConnectionStatusColor = "#F44336";
        }

        [RelayCommand]
        private void ResetSendStatistics()
        {
            SendCount = 0;
            SendSuccessCount = 0;
            SendFailureCount = 0;
        }
    }
}
