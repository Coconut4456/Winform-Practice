using Timer = System.Windows.Forms.Timer;

namespace _10_14;

public partial class Form1 : Form
{
    private int _hour;
    private int _minute;
    private int _second;
    private int _point;
    private Button _startStopButton;
    private Button _resetButton;
    private Label _timeViewLabel;
    private readonly Timer _stopWatchTimer;
    private readonly Timer _timer;
    private int _currentType;
    private bool _state;

    public Form1()
    {
        InitializeComponent();

        _stopWatchTimer = new Timer();
        _stopWatchTimer.Tick += StopWatchTimer_Tick!;
        _stopWatchTimer.Interval = 10;
        _timer = new Timer();
        _timer.Tick += Timer_Tick!;
        _timer.Interval = 10;
        _currentType = 1;
        _state = false;

        TimeReset();
        InitializeUI();
    }

    public void InitializeUI()
    {
        this.ClientSize = new Size(500, 300);
        this.BackColor = Color.Black;

        Label timeLabel = new Label();
        timeLabel.Size = new Size(200, 100);
        timeLabel.Location = new Point(190, 25);
        timeLabel.Name = "timeLabel";
        timeLabel.Text = "0:0:0.0";
        timeLabel.TextAlign = ContentAlignment.MiddleLeft;
        timeLabel.BackColor = Color.Black;
        timeLabel.ForeColor = Color.White;
        timeLabel.Font = new Font("맑은 고딕", 24f, FontStyle.Bold);
        _timeViewLabel = timeLabel;
        this.Controls.Add(timeLabel);

        Button startStopButton = new Button();
        startStopButton.Size = new Size(100, 50);
        startStopButton.Location = new Point(300, 150);
        startStopButton.Name = "startButton";
        startStopButton.Text = "시작";
        startStopButton.BackColor = Color.SkyBlue;
        startStopButton.ForeColor = Color.White;
        startStopButton.FlatStyle = FlatStyle.Flat;
        startStopButton.Font = new Font("맑은 고딕", 14f, FontStyle.Bold);
        startStopButton.Click += StartStopButton_Click!;
        _startStopButton = startStopButton;
        this.Controls.Add(startStopButton);

        Button resetButton = new Button();
        resetButton.Size = new Size(100, 50);
        resetButton.Location = new Point(100, 150);
        resetButton.Name = "stopButton";
        resetButton.Text = "초기화";
        resetButton.BackColor = Color.DarkGray;
        resetButton.ForeColor = Color.White;
        resetButton.FlatStyle = FlatStyle.Flat;
        resetButton.Font = new Font("맑은 고딕", 14f, FontStyle.Bold);
        resetButton.Click += ResetButton_Click!;
        resetButton.Visible = false;
        _resetButton = resetButton;
        this.Controls.Add(resetButton);

        Button stopWatchButton = new Button();
        stopWatchButton.Size = new Size(100, 50);
        stopWatchButton.Location = new Point(145, 230);
        stopWatchButton.Name = "stopWatchLabel";
        stopWatchButton.Text = "스톱워치";
        stopWatchButton.BackColor = Color.Black;
        stopWatchButton.ForeColor = Color.White;
        stopWatchButton.Font = new Font("맑은 고딕", 14f, FontStyle.Bold);
        stopWatchButton.Tag = 1;
        stopWatchButton.FlatStyle = FlatStyle.Flat;
        stopWatchButton.Click += TabButtons_Click!;
        this.Controls.Add(stopWatchButton);

        Button timerButton = new Button();
        timerButton.Size = new Size(100, 50);
        timerButton.Location = new Point(255, 230);
        timerButton.Name = "timerLabel";
        timerButton.Text = "타이머";
        timerButton.BackColor = Color.Black;
        timerButton.ForeColor = Color.White;
        timerButton.Font = new Font("맑은 고딕", 14f, FontStyle.Bold);
        timerButton.Tag = 2;
        timerButton.FlatStyle = FlatStyle.Flat;
        timerButton.Click += TabButtons_Click!;
        this.Controls.Add(timerButton);

        for (int i = 0; i < 3; i++)
        {
            TextBox textBox = new TextBox();
            textBox.Size = new Size(40, 50);
            textBox.Location = new Point((i * 65) + 170, 70);
            textBox.Name = $"timerTextBox{i + 1}";
            switch (i)
            {
                case 0:
                    textBox.Text = "시";
                    break;
                case 1:
                    textBox.Text = "분";
                    break;
                case 2:
                    textBox.Text = "초";
                    break;
            }

            textBox.BackColor = Color.Black;
            textBox.ForeColor = Color.White;
            textBox.Tag = 3 + i;
            textBox.Visible = false;
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.Enter += TextBox_FocusHandler!;
            textBox.Leave += TextBox_FocusHandler!;
            textBox.KeyPress += TextBox_KeyPress!;
            this.Controls.Add(textBox);
        }
    }

    public void TextBox_FocusHandler(object sender, EventArgs e)
    {
        if (sender is TextBox textBox)
        {
            int.TryParse(textBox.Text, out int num);

            if (num > 60)
            {
                textBox.Text = "60";
            }

            switch (textBox.Tag)
            {
                case 3:
                    if (ActiveControl == textBox)
                    {
                        if (textBox.Text == "시")
                            textBox.Clear();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                            textBox.Text = "시";
                    }

                    break;
                case 4:
                    if (ActiveControl == textBox)
                    {
                        if (textBox.Text == "분")
                            textBox.Clear();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                            textBox.Text = "분";
                    }

                    break;
                case 5:
                    if (ActiveControl == textBox)
                    {
                        if (textBox.Text == "초")
                            textBox.Clear();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                            textBox.Text = "초";
                    }

                    break;
            }
        }
    }

    public void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            e.Handled = true;
    }

    public void TimeReset()
    {
        _point = 0;
        _minute = 0;
        _second = 0;
        _hour = 0;
    }

    public void ViewStopWatchUI()
    {
        _timeViewLabel.Text = "0:0:0.0";
    }

    public void ViewTimerUI()
    {
        _timeViewLabel.Text = "  :     :";

        foreach (Control control in this.Controls)
        {
            switch (control.Tag)
            {
                case >= 3:
                    control.Visible = true;
                    control.BringToFront();
                    break;
            }
        }
    }

    public void StopWatchTimer_Tick(object sender, EventArgs e)
    {
        _point++;

        if (_point >= 60)
        {
            _point = 0;
            _second++;
        }

        if (_second >= 60)
        {
            _point = 0;
            _minute++;
        }

        if (_minute >= 60)
        {
            _point = 0;
            _hour++;
        }

        _timeViewLabel.Text = $"{_hour}:{_minute}:{_second}.{_point}";
    }

    public void Timer_Tick(object sender, EventArgs e)
    {
        _point--;

        if (_point <= 0)
        {
            _point = 59;
            _second--;
        }

        if (_second < 0)
        {
            _second = 59;
            _minute--;
        }

        if (_minute < 0)
        {
            _minute = 59;
            _hour--;
        }

        if (_hour == 0 && _minute == 0 && _second == 0 && _point == 0)
        {
            _timer.Stop();
        }

        _timeViewLabel.Text = $"{_hour}:{_minute}:{_second}:{_point}";
    }

    public void StartStopButton_Click(object sender, EventArgs e)
    {
        if (_stopWatchTimer.Enabled || _timer.Enabled)
        {
            Pause();
        }
        else
        {
            Start();
        }
    }

    public void Start()
    {
        switch (_currentType)
        {
            case 1:
                _point = 60;
                _stopWatchTimer.Start();
                break;
            case 2:
                _point = 60;
                _hour = int.Parse(this.Controls["timerTextBox1"]!.Text);
                _minute = int.Parse(this.Controls["timerTextBox2"]!.Text);
                _second = int.Parse(this.Controls["timerTextBox3"]!.Text);
                _timer.Start();
                break;
        }
        
        _startStopButton.BackColor = Color.Red;
        _startStopButton.Text = "중지";
        _resetButton.Visible = true;
    }

    public void Pause()
    {
        switch (_currentType)
        {
            case 1:
                _stopWatchTimer.Stop();
                break;
            case 2:
                _timer.Stop();
                break;
        }

        _startStopButton.BackColor = Color.SkyBlue;
        _startStopButton.Text = "계속";
    }

    public void ReSume()
    {
        switch (_currentType)
        {
            case 1:
                _stopWatchTimer.Start();
                break;
            case 2:
                _timer.Start();
                break;
        }
        
        _startStopButton.BackColor = Color.Red;
        _startStopButton.Text = "중지";
    }

    public void ResetButton_Click(object sender, EventArgs e)
    {
        switch (_currentType)
        {
            case 1:
                _stopWatchTimer.Stop();
                TimeReset();
                ViewStopWatchUI();
                break;
            case 2:
                _timer.Stop();
                TimeReset();
                ViewTimerUI();
                break;
        }
        
        _resetButton.Visible = false;
        _startStopButton.BackColor = Color.SkyBlue;
        _startStopButton.Text = "시작";
    }

    public void TabButtons_Click(object sender, EventArgs e)
    {
        _stopWatchTimer.Stop();
        _timer.Stop();
        _startStopButton.BackColor = Color.SkyBlue;
        _startStopButton.Text = "시작";
        _resetButton.Visible = false;

        if (sender is Control control)
        {
            switch (control.Tag)
            {
                default:
                    MessageBox.Show("'control' not found");
                    return;
                case 1:
                    _currentType = 1;
                    foreach (Control c in this.Controls)
                    {
                        if (c is TextBox)
                        {
                            c.Visible = false;
                        }
                    }

                    ViewStopWatchUI();
                    break;
                case 2:
                    _currentType = 2;
                    ViewTimerUI();
                    break;
            }
        }
    }
}