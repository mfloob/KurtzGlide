using KurtzGlide.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace KurtzGlide
{
    public abstract class EngineTask
    {
        public abstract bool Validate();
        public abstract bool Execute();        
        public LocalPlayer LocalPlayer { get { return MainWindow.LocalPlayer; } set { MainWindow.LocalPlayer = value; } }
        public MemoryUtility Memory { get { return MainWindow.Memory; } set { MainWindow.Memory = value; } }
        public bool Attached { get { return MainWindow.Attached; } set { MainWindow.Attached = value; } }
        public Logger Log { get { return MainWindow.Log; } }
        public string Name { get; set; }

        public void SetLabelStatus(Label label, bool isFound)
        {
            label.Content = isFound ? "Found" : "Null";
            label.Foreground = isFound ? Brushes.LightGreen : Brushes.Red;
        }
    }

    public class AttachMemoryTask : EngineTask
    {
        private Label ProcessStatusLabel { get; }

        public AttachMemoryTask(Label processStatusLabel) => this.ProcessStatusLabel = processStatusLabel;

        public override bool Execute()
        {
            return true;
        }

        public override bool Validate()
        {
            SetLabelStatus(this.ProcessStatusLabel, this.Attached);            
            using (var process = Process.GetProcessesByName(MainWindow.PROCESS_NAME).FirstOrDefault())
            {
                if (process != null)
                {
                    if (this.Memory == null)
                        this.Memory = new MemoryUtility(MainWindow.PROCESS_NAME);
                    if (this.LocalPlayer == null)
                        this.LocalPlayer = new LocalPlayer(this.Memory);
                    if (!this.Attached)
                    {
                        this.Attached = true;
                        Log.Log("Process found. Searching for LocalPlayer...");
                    }
                }
                else
                {
                    this.Memory = null;
                    this.LocalPlayer = null;
                    if (this.Attached)
                    {
                        this.Attached = false;
                        Log.Log("Process not found. Searching...");
                    }
                }
                return false;
            }
        }
    }

    public class GetLocalPlayerTask : EngineTask
    {
        private Label LocalPlayerStatusLabel { get; }
        private Engine Engine { get; }
        private bool localFound;
        private int tickDelay;

        public GetLocalPlayerTask(Engine engine, Label localPlayerStatusLabel)
        {
            this.LocalPlayerStatusLabel = localPlayerStatusLabel;
            this.Engine = engine;
        }

        public override bool Execute()
        {
            if (this.tickDelay != 1000)
                this.tickDelay = this.Engine.TickDelay = 1000;
            this.LocalPlayer.FindBaseAddress();
            return true;
        }

        public override bool Validate()
        {
            if (!this.Attached)
                return false;

            SetLabelStatus(this.LocalPlayerStatusLabel, this.LocalPlayer.IsFound);

            if (this.LocalPlayer.IsFound && !localFound)
            {
                Log.Log("Found LocalPlayer! Ready to begin.");
                this.tickDelay = this.Engine.TickDelay = 75;
                this.localFound = true;
            }
            else if (!this.LocalPlayer.IsFound && localFound)
            {
                this.localFound = false;
                Log.Log("LocalPlayer not found. Searching...");
            }
            return this.Attached && !this.LocalPlayer.IsFound;
        }
    }

    public class InfiniteHealthTask : EngineTask
    {        
        public override bool Execute()
        {
            this.LocalPlayer.Health = 1;
            this.LocalPlayer.RegenHealth = 1;
            return true;
        }

        public override bool Validate() => this.LocalPlayer?.IsFound ?? false;        
    }

    public class InfiniteManaTask : EngineTask
    {
        public override bool Execute()
        {
            this.LocalPlayer.Mana = 1;
            return true;
        }

        public override bool Validate() => this.LocalPlayer?.IsFound ?? false;
    }

    public class InfiniteStaminaTask : EngineTask
    {
        public override bool Execute()
        {
            this.LocalPlayer.Stamina = 1;
            return true;
        }

        public override bool Validate() => this.LocalPlayer?.IsFound ?? false;
    }

    public class InfiniteKarmaTask : EngineTask
    {
        public override bool Execute()
        {
            this.LocalPlayer.Karma = 1;
            return true;
        }

        public override bool Validate() => this.LocalPlayer?.IsFound ?? false;
    }

    public class SetSlidersTask : EngineTask
    {
        private Slider attackSpeed;
        private Slider castSpeed;
        private Slider moveSpeed;
        private Slider damageMultiplier;

        public SetSlidersTask(Slider attackSpeed, Slider castSpeed, Slider moveSpeed, Slider damageMultiplier)
        {
            this.attackSpeed = attackSpeed;
            this.castSpeed = castSpeed;
            this.moveSpeed = moveSpeed;
            this.damageMultiplier = damageMultiplier;
        }

        public override bool Execute()
        {
            this.attackSpeed.Value = LocalPlayer.AttackSpeed;
            this.castSpeed.Value = LocalPlayer.CastSpeed;
            this.moveSpeed.Value = LocalPlayer.MoveSpeed;
            this.damageMultiplier.Value = LocalPlayer.DamageMultiplier;
            return true;
        }

        public override bool Validate() => this.LocalPlayer?.IsFound ?? false;
    }
}
