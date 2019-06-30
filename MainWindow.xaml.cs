using KurtzGlide.Utilities;
using System.Windows;


namespace KurtzGlide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public const string PROCESS_NAME = "TheChase-Win64-Shipping";
        private Engine engine;
        public static MemoryUtility Memory { get; set; }
        public static LocalPlayer LocalPlayer { get; set; }
        public static bool Attached { get; set; }
        public static Logger Log { get; set; }

        private EngineTask infiniteHealthTask = new InfiniteHealthTask();
        private EngineTask infiniteManaTask = new InfiniteManaTask();
        private EngineTask infiniteStaminaTask = new InfiniteStaminaTask();
        private EngineTask infiniteKarmaTask = new InfiniteKarmaTask();

        public MainWindow()
        {
            InitializeComponent();
            Log = new Logger(this.LogBox);
            Log.Log("Initializing...");
            this.engine = new Engine(75);
            this.engine.Add( new AttachMemoryTask(this.ProcessStatusLabel), new GetLocalPlayerTask(this.engine, this.LocalPlayerStatusLabel), new SetSlidersTask(this.AttackSpeedSlider, this.CastSpeedSlider, this.MovementSpeedSlider, this.DamageMultiplierSlider));
        }

        private void HealthCheckBox_Checked(object sender, RoutedEventArgs e) => this.engine.Add(infiniteHealthTask);
        private void HealthCheckBox_Unchecked(object sender, RoutedEventArgs e) => this.engine.Remove(infiniteHealthTask);        
        private void ManaCheckBox_Checked(object sender, RoutedEventArgs e) => this.engine.Add(infiniteManaTask);
        private void ManaCheckBox_Unchecked(object sender, RoutedEventArgs e) => this.engine.Remove(infiniteManaTask);
        private void StaminaCheckBox_Checked(object sender, RoutedEventArgs e) => this.engine.Add(infiniteStaminaTask);
        private void StaminaCheckBox_Unchecked(object sender, RoutedEventArgs e) => this.engine.Remove(infiniteStaminaTask);
        private void KarmaCheckBox_Checked(object sender, RoutedEventArgs e) => this.engine.Add(infiniteKarmaTask);
        private void KarmaCheckBox_Unchecked(object sender, RoutedEventArgs e) => this.engine.Remove(infiniteKarmaTask);

        private void AttackSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!LocalPlayer.IsFound)
                return;
            LocalPlayer.AttackSpeed = (float) this.AttackSpeedSlider.Value;
            this.AttackSpeedLabel.Content = this.AttackSpeedSlider.Value.ToString("0.0");            
        }

        private void CastSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!LocalPlayer.IsFound)
                return;
            LocalPlayer.CastSpeed = (float) this.CastSpeedSlider.Value;
            this.CastSpeedLabel.Content = this.CastSpeedSlider.Value.ToString("0.0");
        }

        private void MovementSpeedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!LocalPlayer.IsFound)
                return;
            LocalPlayer.MoveSpeed = (float) MovementSpeedSlider.Value;
            this.MoveSpeedLabel.Content = this.MovementSpeedSlider.Value.ToString("0.0");
        }

        private void DamageMultiplierSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!LocalPlayer.IsFound)
                return;
            LocalPlayer.DamageMultiplier = (float) DamageMultiplierSlider.Value;
            this.DamageMultiplierLabel.Content = this.DamageMultiplierSlider.Value.ToString("0.0");
        }

        private void LogBox_SizeChanged(object sender, SizeChangedEventArgs e) => LogScroller.ScrollToBottom();
    }
}
