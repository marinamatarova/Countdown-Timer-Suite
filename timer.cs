using System;
using System.Windows.Forms;
using System.Media;

namespace CountdownTimer
{
    public class MainForm : Form
    {
        private Label timeLabel;
        private ProgressBar progressBar;
        private TextBox hourBox, minBox, secBox;
        private System.Windows.Forms.Timer timer;
        private int remainingSeconds, totalSeconds;
        private bool isRunning;

        public MainForm()
        {
            Text = "Таймер обратного отсчёта";
            Size = new System.Drawing.Size(500, 400);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = System.Drawing.Color.FromArgb(44, 62, 80);

            // Панель ввода
            FlowLayoutPanel inputPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Padding = new Padding(10) };
            inputPanel.Controls.Add(new Label { Text = "ЧЧ:", ForeColor = System.Drawing.Color.White });
            hourBox = new TextBox { Text = "00", Width = 40 };
            inputPanel.Controls.Add(hourBox);
            inputPanel.Controls.Add(new Label { Text = "ММ:", ForeColor = System.Drawing.Color.White });
            minBox = new TextBox { Text = "01", Width = 40 };
            inputPanel.Controls.Add(minBox);
            inputPanel.Controls.Add(new Label { Text = "СС:", ForeColor = System.Drawing.Color.White });
            secBox = new TextBox { Text = "00", Width = 40 };
            inputPanel.Controls.Add(secBox);
            Button setBtn = new Button { Text = "Установить" };
            setBtn.Click += (s, e) => SetTime();
            inputPanel.Controls.Add(setBtn);
            Controls.Add(inputPanel);

            // Пресеты
            FlowLayoutPanel presetPanel = new FlowLayoutPanel { Dock = DockStyle.Top, Padding = new Padding(10) };
            foreach (int mins in new int[] { 1, 5, 10, 15 })
            {
                Button btn = new Button { Text = $"{mins} мин", Width = 70 };
                int seconds = mins * 60;
                btn.Click += (s, e) => SetPreset(seconds);
                presetPanel.Controls.Add(btn);
            }
            Controls.Add(presetPanel);

            // Отображение времени
            timeLabel = new Label
            {
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Courier New", 48, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.Gold,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                BackColor = System.Drawing.Color.Black
            };
            Controls.Add(timeLabel);

            // Прогресс-бар
            progressBar = new ProgressBar { Dock = DockStyle.Bottom, Height = 30, Style = ProgressBarStyle.Continuous };
            Controls.Add(progressBar);

            // Кнопки управления
            FlowLayoutPanel controlPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Padding = new Padding(10) };
            Button startBtn = new Button { Text = "▶ Старт", Width = 100 };
            Button pauseBtn = new Button { Text = "⏸ Пауза", Width = 100 };
            Button resetBtn = new Button { Text = "🔄 Сброс", Width = 100 };
            startBtn.Click += (s, e) => StartTimer();
            pauseBtn.Click += (s, e) => PauseTimer();
            resetBtn.Click += (s, e) => ResetTimer();
            controlPanel.Controls.Add(startBtn);
            controlPanel.Controls.Add(pauseBtn);
            controlPanel.Controls.Add(resetBtn);
            Controls.Add(controlPanel);

            SetTime();
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
        }

        private void SetTime()
        {
            int h = int.Parse(hourBox.Text);
            int m = int.Parse(minBox.Text);
            int s = int.Parse(secBox.Text);
            totalSeconds = h * 3600 + m * 60 + s;
            if (totalSeconds <= 0) totalSeconds = 1;
            remainingSeconds = totalSeconds;
            UpdateDisplay();
            progressBar.Value = 0;
            isRunning = false;
            timer.Stop();
        }

        private void SetPreset(int seconds)
        {
            totalSeconds = seconds;
            remainingSeconds = seconds;
            hourBox.Text = (seconds / 3600).ToString("00");
            minBox.Text = ((seconds % 3600) / 60).ToString("00");
            secBox.Text = (seconds % 60).ToString("00");
            UpdateDisplay();
            progressBar.Value = 0;
            timer.Stop();
            isRunning = false;
        }

        private void UpdateDisplay()
        {
            int h = remainingSeconds / 3600;
            int m = (remainingSeconds % 3600) / 60;
            int s = remainingSeconds % 60;
            timeLabel.Text = $"{h:00}:{m:00}:{s:00}";
            if (totalSeconds > 0)
                progressBar.Value = (totalSeconds - remainingSeconds) * 100 / totalSeconds;
        }

        private void StartTimer()
        {
            if (remainingSeconds <= 0) { MessageBox.Show("Установите время > 0"); return; }
            if (isRunning) return;
            isRunning = true;
            timer.Start();
        }

        private void PauseTimer()
        {
            if (isRunning)
            {
                timer.Stop();
                isRunning = false;
            }
        }

        private void ResetTimer()
        {
            timer.Stop();
            remainingSeconds = totalSeconds;
            UpdateDisplay();
            isRunning = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                UpdateDisplay();
                if (remainingSeconds == 0)
                {
                    timer.Stop();
                    isRunning = false;
                    System.Console.Beep(1000, 1000);
                    MessageBox.Show("Время вышло!", "Таймер", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                timer.Stop();
                isRunning = false;
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new MainForm());
        }
    }
}
