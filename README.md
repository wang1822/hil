# 硬件模拟数据平台 (Hardware Simulator)

一个基于WPF和MVVM架构的硬件数据模拟平台，用于模拟PCS、BMS和空调系统的实时数据。

## 功能特点

- **PCS (功率转换系统)** 数据模拟
  - 直流侧参数：电压、电流、功率
  - 交流侧参数：电压、电流、功率、频率
  - 其他参数：功率因数、转换效率、设备温度、工作模式

- **BMS (电池管理系统)** 数据模拟
  - 电池状态：SOC、SOH
  - 电压信息：总电压、单体电压、压差
  - 温度信息：最高/最低/平均温度、温差
  - 其他信息：可用能量、绝缘电阻、告警等级

- **空调系统** 数据模拟
  - 温度信息：室内/室外温度、设定温度
  - 湿度信息：室内/室外湿度
  - 压缩机与风机：频率、转速
  - 制冷剂参数：蒸发器/冷凝器温度、压力
  - 能耗信息：实时功率、累计能耗、能效比

## 技术栈

- **框架**: .NET 8.0
- **UI**: WPF (Windows Presentation Foundation)
- **架构**: MVVM (Model-View-ViewModel)
- **依赖**: CommunityToolkit.Mvvm

## 项目结构

```
HardwareSimulator/
├── Models/                 # 数据模型
│   ├── PcsData.cs         # PCS数据模型
│   ├── BmsData.cs         # BMS数据模型
│   └── AirConditionerData.cs  # 空调数据模型
├── ViewModels/            # 视图模型
│   ├── MainViewModel.cs   # 主窗口视图模型
│   ├── PcsViewModel.cs    # PCS视图模型
│   ├── BmsViewModel.cs    # BMS视图模型
│   └── AirConditionerViewModel.cs  # 空调视图模型
├── Views/                 # 视图
│   └── MainWindow.xaml    # 主窗口
├── Services/              # 服务
│   └── DataSimulationService.cs  # 数据模拟服务
├── Commands/              # 命令
│   └── RelayCommand.cs    # 中继命令
├── Converters/            # 转换器
│   └── Converters.cs      # 值转换器
├── Resources/             # 资源
│   └── Styles.xaml        # 样式资源
├── App.xaml               # 应用程序入口
└── HardwareSimulator.csproj  # 项目文件
```

## 开发环境要求

- Windows 10/11
- Visual Studio 2022 或更高版本
- .NET 8.0 SDK

## 构建和运行

1. 克隆仓库
```bash
git clone https://github.com/wang1822/hil.git
cd hil
```

2. 还原依赖
```bash
cd HardwareSimulator
dotnet restore
```

3. 构建项目
```bash
dotnet build
```

4. 运行应用
```bash
dotnet run
```

或者使用Visual Studio打开 `HardwareSimulator/HardwareSimulator.csproj` 文件，按F5运行。

## 使用说明

1. 启动应用后，点击"开始模拟"按钮开始数据模拟
2. 使用下拉框调整数据更新间隔（500ms - 5000ms）
3. 点击"重置数据"按钮可以将数据恢复到初始状态
4. 底部状态栏显示模拟状态和更新次数

## 截图

应用程序界面包含三个主要面板：
- 左侧：PCS功率转换系统数据
- 中间：BMS电池管理系统数据
- 右侧：空调系统数据

每个面板都有实时状态指示器和详细的参数显示。

## 许可证

MIT License