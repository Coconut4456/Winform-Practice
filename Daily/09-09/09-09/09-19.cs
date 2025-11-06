namespace _09_09;

public partial class Form1 : Form
{
    private readonly Random _random;
    private int _targetInt;
    private int _userChance;
    private bool _isActive;
    
    public Form1()
    {
        InitializeComponent();
        InitializeUI();
        
        _random = new Random();
        _targetInt = 0;
        _userChance = 0;
        _isActive = false;

        GameStart();
    }

    public void InitializeUI()
    {
        this.ClientSize = new Size(600, 400);
        Label targetViewLabel = new Label();
        targetViewLabel.Name = "targetViewLabel";
        targetViewLabel.Size = new Size(500, 100);
        targetViewLabel.Location = new Point(50, 100);
        targetViewLabel.ForeColor = Color.Black;
        targetViewLabel.BackColor = Color.Gray;
        targetViewLabel.Font = new Font(targetViewLabel.Font.FontFamily, 16f);
        targetViewLabel.TextAlign = ContentAlignment.MiddleCenter;
        targetViewLabel.Text = "";
        this.Controls.Add(targetViewLabel);
        
        TextBox userTextBox = new TextBox();
        userTextBox.Name = "userTextBox";
        userTextBox.Size = new Size(50, 20);
        userTextBox.Location = new Point(275, 250);
        userTextBox.KeyDown += userTextBox_KeyDown!;
        this.Controls.Add(userTextBox);
    }

    public void GameStart()
    {
        _targetInt = _random.Next(100);
        _userChance = 7;
        this.Controls["targetViewLabel"]!.Text = "1~100 사이 숫자를 입력해주세요.";
        this.Controls["userTextBox"]!.Text = "";
        _isActive = true;
    }

    public void userTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return; // Enter가 아닐 경우 중단
        
        Control viewLabel = this.Controls["targetViewLabel"]!;
        
        if (int.TryParse(this.Controls["userTextBox"]!.Text, out int number))
        {
            if (number == _targetInt)
            {
                viewLabel.Text = "정답입니다! \n다시 시작하시려면 0을 입력해주세요.";
                _isActive = false;
            }
            else if (!_isActive && number == 0)
            {
                GameStart();
            }
            else if (!_isActive && number != 0)
            {
                viewLabel.Text = "다시 시작하시려면 0을 입력해주세요.";
            }
            else if (_isActive && number == 0)
            {
                viewLabel.Text = "1~100 사이의 숫자만 입력해주세요.";
            }
            else
            {
                _userChance -= 1;

                if (_userChance <= 0)
                { 
                    this.Controls["targetViewLabel"]!.Text = "모든 기회가 소진되었습니다. \n다시 시작하시려면 0을 입력해주세요.";
                    _isActive = false;
                    return;
                }
                
                viewLabel.Text = number > _targetInt ? "더 작은 숫자입니다." + $"\n남은 기회:{_userChance}" : "더 큰 숫자입니다." + $"\n남은 기회:{_userChance}";
            }
        }
        else
        {
            viewLabel.Text = "숫자만 입력해주세요.";
        }
    }
}