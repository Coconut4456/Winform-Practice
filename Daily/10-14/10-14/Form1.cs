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
    private Type _currentType;
    private State _currentState;
    private State _pendingState;

    public Form1()
    {
        InitializeComponent();
        

        _stopWatchTimer = new Timer();
        _stopWatchTimer.Tick += StopWatchTimer_Tick!;
        _stopWatchTimer.Interval = 10;
        _timer = new Timer();
        _timer.Tick += Timer_Tick!;
        _timer.Interval = 10;
        _currentType = Type.StopWatch;
        _currentState = State.Stop;
        
        InitializeUI();
        UIHandler();
    }

    public enum Type
    {
        StopWatch,
        Timer
    }
    
    public enum State
    {
        Ready,
        Play,
        Pause,
        Stop
    }

    public void InitializeUI()
    {
        this.ClientSize = new Size(500, 300);
        this.BackColor = Color.Black;

        Label timeViewLabel = new Label();
        timeViewLabel.Size = new Size(250, 100);
        timeViewLabel.Location = new Point(160, 25);
        timeViewLabel.Name = "timeViewLabel";
        timeViewLabel.Text = "0 : 0 : 0 . 0";
        timeViewLabel.TextAlign = ContentAlignment.MiddleLeft;
        timeViewLabel.BackColor = Color.Black;
        timeViewLabel.ForeColor = Color.White;
        timeViewLabel.Font = new Font("맑은 고딕", 24f, FontStyle.Bold);
        _timeViewLabel = timeViewLabel;
        this.Controls.Add(timeViewLabel);

        Button startStopButton = new Button();
        startStopButton.Size = new Size(100, 50);
        startStopButton.Location = new Point(300, 150);
        startStopButton.Name = "startButton";
        startStopButton.Text = "시작";
        startStopButton.BackColor = Color.SkyBlue;
        startStopButton.ForeColor = Color.White;
        startStopButton.FlatStyle = FlatStyle.Flat;
        startStopButton.Font = new Font("맑은 고딕", 14f, FontStyle.Bold);
        startStopButton.Click += ActionButton_Click!;
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
            
            switch (i)
            {
                case 0:
                    textBox.Name = "hourTextBox";
                    textBox.Text = "시";
                    break;
                case 1:
                    textBox.Name = "minuteTextBox";
                    textBox.Text = "분";
                    break;
                case 2:
                    textBox.Name = "secondTextBox";
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

    /// <summary>
    /// 타이머 TextBox 가이드 표시
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void TextBox_FocusHandler(object sender, EventArgs e)
    {
        if (sender is not TextBox textBox)
            return;
        
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

    /// <summary>
    /// 타이머 TextBox 숫자만 입력
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void TextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            e.Handled = true;
    }

    /// <summary>
    /// 스톱워치 Timer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

        _timeViewLabel.Text = $"{_hour} : {_minute} : {_second} . {_point}";
    }

    /// <summary>
    /// 타이머 Timer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Timer_Tick(object sender, EventArgs e)
    {
        if (_hour <= 0 && _minute <= 0 && _second <= 0 && _point <= 0)
        {
            _currentState = State.Stop;
            UIHandler();
            ActionHandler();
            return;
        }

        _point--;
        
        if (_point < 0)
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
        
        _timeViewLabel.Text = $"{_hour} : {_minute} : {_second} . {_point}";
    }
    
    /// <summary>
    /// UI 업데이트 핸들러
    /// </summary>
    public void UIHandler()
    {
        switch(_currentType)
        {
            case Type.StopWatch:
                StopWatchUIHandler();
                break;
            case Type.Timer:
                TimerUIHandler();
                break;
        }
    }

    /// <summary>
    /// 스톱워치 UI 핸들러
    /// </summary>
    public void StopWatchUIHandler()
    {
        switch (_currentState)
        {
            case State.Ready: // play
                _startStopButton.BackColor = Color.Red;
                _startStopButton.Text = "중지";
                _resetButton.Visible = true;
                break;
            case State.Play: // pause
                _startStopButton.BackColor = Color.SkyBlue;
                _startStopButton.Text = "계속";
                break;
            case State.Pause: // resume
                _startStopButton.BackColor = Color.Red;
                _startStopButton.Text = "중지";
                break;
            case State.Stop: // reset
                foreach (Control control in this.Controls)
                {
                    if (control is TextBox textbox)
                        textbox.Visible = false;
                }
                _timeViewLabel.Text = $"{_hour} : {_minute} : {_second} . {_point}";
                _startStopButton.BackColor = Color.SkyBlue;
                _startStopButton.Text = "시작";
                _resetButton.Visible = false;
                break;
        }
    }

    /// <summary>
    /// 타이머 UI 핸들러
    /// </summary>
    public void TimerUIHandler()
    {
        switch (_currentState)
        {
            case State.Ready: // play
                foreach (Control control in this.Controls)
                {
                    if (control is TextBox textbox)
                        textbox.Visible = false;
                }
                _startStopButton.BackColor = Color.Red;
                _startStopButton.Text = "중지";
                _resetButton.Visible = true;
                break;
            case State.Play: // pause
                _startStopButton.BackColor = Color.SkyBlue;
                _startStopButton.Text = "계속";
                break;
            case State.Pause: // resume
                _startStopButton.BackColor = Color.Red;
                _startStopButton.Text = "중지";
                break;
            case State.Stop: // reset
                foreach (Control control in this.Controls)
                {
                    if (control is TextBox textbox)
                    { 
                        textbox.Visible = true;
                        textbox.BringToFront();
                    }
                }
                _startStopButton.BackColor = Color.SkyBlue;
                _startStopButton.Text = "시작";
                _resetButton.Visible = false; 
                _timeViewLabel.Text = "  :     :";
                break;
        }
    }

    /// <summary>
    /// 기능 동작 핸들러
    /// </summary>
    public void ActionHandler()
    {
        switch (_currentType)
        {
            case Type.StopWatch:
                StopWatchActionHandler();
                break;
            case Type.Timer:
                TimerActionHandler();
                break;
        }
    }
    
    /// <summary>
    /// 기능 핸들러
    /// </summary>
    /// <param name="state"></param>
    public void ActionHandler(State state)
    {
        _currentState = state;
        
        switch (_currentType)
        {
            case Type.StopWatch:
                StopWatchActionHandler();
                break;
            case Type.Timer:
                TimerActionHandler();
                break;
        }
    }

    /// <summary>
    /// 스톱워치 동작 핸들러
    /// </summary>
    public void StopWatchActionHandler()
    {
        switch (_currentState)
        {
            default:
                _pendingState = State.Ready;
                break;
            case State.Ready: // play
                _pendingState = State.Play;
                _point = 60;
                _stopWatchTimer.Start();
                break;
            case State.Play: // pause
                _pendingState = State.Pause;
                _stopWatchTimer.Stop();
                break;
            case State.Pause: // resume
                _pendingState = State.Play;
                _stopWatchTimer.Start();
                break;
            case State.Stop: // reset
                _pendingState = State.Ready;
                _stopWatchTimer.Stop();
                _point = 0;
                _second = 0;
                _minute = 0;
                _hour = 0;
                break;
        }
    }

    /// <summary>
    /// 타이머 동작 핸들러
    /// </summary>
    public void TimerActionHandler()
    {
        switch (_currentState)
        {
            default:
                _pendingState = State.Ready;
                break;
            case State.Ready: // play
                _point = 0;
                (_hour, _minute, _second) = GetTimeInput();
                if ((_hour, _minute, _second) == (0, 0, 0))
                {
                    _currentState = State.Stop;
                    MessageBox.Show("올바른 값을 입력해주세요.");
                }
                else
                { 
                    _pendingState = State.Play;
                    _timer.Start();
                }
                break;
            case State.Play: // pause
                _pendingState = State.Pause;
                _timer.Stop();
                break;
            case State.Pause: // resume
                _pendingState = State.Play;
                _timer.Start();
                break;
            case State.Stop: // reset
                _pendingState = State.Ready;
                _timer.Stop();
                break;
        }
    }

    /// <summary>
    /// 타이머 TextBox의 시, 분, 초 사용자 입력값 반환
    /// </summary>
    /// <returns></returns>
    public (int hour, int minute, int second) GetTimeInput()
    {
        return (int.Parse(this.Controls["hourTextBox"]!.Text),
            int.Parse(this.Controls["minuteTextBox"]!.Text),
            int.Parse(this.Controls["secondTextBox"]!.Text));
    }
    
    
    /// <summary>
    /// 시작/중지 버튼
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ActionButton_Click(object sender, EventArgs e)
    {
        ActionHandler();
        UIHandler();
        _currentState = _pendingState;
    }
    
    /// <summary>
    /// 초기화 버튼
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ResetButton_Click(object sender, EventArgs e)
    {
        ActionHandler(State.Stop);
        UIHandler();
        _currentState = _pendingState;
    }
    
    /// <summary>
    /// 탭 전환시
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void TabButtons_Click(object sender, EventArgs e)
    {
        _currentState = State.Stop;
        
        if (sender is Control control)
        {
            switch (control.Tag)
            {
                default:
                    MessageBox.Show("'control' not found");
                    return;
                case 1:
                    _currentType = Type.StopWatch;
                    break;
                case 2:
                    _currentType = Type.Timer;
                    break;
            }

            _timer.Stop();
            _stopWatchTimer.Stop();
            ActionHandler();
            UIHandler();
            _currentState = _pendingState;
        }
    }
}