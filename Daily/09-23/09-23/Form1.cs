namespace _09_23;

public partial class Form1 : Form
{
    private readonly ListView _listView;
    private readonly TextBox _userInputTextBox;
    private readonly TextBox _searchBox;

    public Form1()
    {
        InitializeComponent();

        _userInputTextBox = new TextBox();
        _listView = new ListView();
        _searchBox = new TextBox();

        InitializeUI();
    }

    public void InitializeUI()
    {
        this.ClientSize = new Size(300, 430);
        this.BackColor = Color.Gray;

        _userInputTextBox.Name = "textBox";
        _userInputTextBox.Tag = 1;
        _userInputTextBox.Size = new Size(250, 50);
        _userInputTextBox.Location = new Point(25, 360);
        _userInputTextBox.TextChanged += TextBoxChanged!;
        this.Controls.Add(_userInputTextBox);

        Button addButton = new Button();
        addButton.Name = "addButton";
        addButton.Tag = 2;
        addButton.Size = new Size(50, 25);
        addButton.Location = new Point(175, 385);
        addButton.Text = "추가";
        addButton.BackColor = Color.White;
        addButton.Visible = false;
        addButton.Click += AddButton_Click!;
        this.Controls.Add(addButton);

        Button removeButton = new Button();
        removeButton.Name = "removeButton";
        removeButton.Tag = 3;
        removeButton.Size = new Size(50, 25);
        removeButton.Location = new Point(225, 385);
        removeButton.Text = "삭제";
        removeButton.BackColor = Color.White;
        removeButton.Visible = false;
        removeButton.Click += RemoveButton_Click!;
        this.Controls.Add(removeButton);

        _listView.Name = "listView";
        _listView.Tag = 4;
        _listView.Size = new Size(250, 300);
        _listView.Location = new Point(25, 55);
        _listView.View = View.List;
        _listView.SelectedIndexChanged += ListView_SelectedIndexChanged!;
        this.Controls.Add(_listView);
        
        Button saveButton = new Button();
        saveButton.Name = "saveButton";
        saveButton.Tag = 5;
        saveButton.Size = new Size(50, 25);
        saveButton.Location = new Point(25, 385);
        saveButton.Text = "저장";
        saveButton.BackColor = Color.White;
        saveButton.Click += SaveButton_Click!;
        this.Controls.Add(saveButton);

        Button openButton = new Button();
        openButton.Name = "openButton";
        openButton.Tag = 6;
        openButton.Size = new Size(50, 25);
        openButton.Location = new Point(75, 385);
        openButton.Text = "열기";
        openButton.BackColor = Color.White;
        openButton.Click += OpenButton_Click!;
        this.Controls.Add(openButton);

        _searchBox.Name = "searchBox";
        _searchBox.Tag = 7;
        _searchBox.Size = new Size(250, 50);
        _searchBox.Location = new Point(25, 25);
        _searchBox.TextChanged += SearchBoxChanged!;
        this.Controls.Add(_searchBox);
    }

    public void SearchBoxChanged(object sender, EventArgs e)
    {
        string searchString = _searchBox.Text;
        
        foreach (ListViewItem item in _listView.Items)
        {
            if (searchString == "")
            {
                item.BackColor = Color.White;
                continue;
            }
            
            if (item.Text.ToLower().Contains(searchString))
            {
                item.BackColor = Color.Yellow;
            }
            else
            {
                item.BackColor = Color.White;
            }
        }
    }

    public void TextBoxChanged(object sender, EventArgs e)
    {
        this.Controls["addButton"]!.Visible = _userInputTextBox.Text != "";
    }

    public void ListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.Controls["removeButton"]!.Visible = _listView.SelectedItems.Count > 0;
    }

    public void AddButton_Click(object sender, EventArgs e)
    {
        if (this.Controls["textBox"] == null)
            return;

        string addText = this.Controls["textBox"]!.Text;

        if (addText == "")
        {
            MessageBox.Show("추가할 내용을 입력해주세요.");
            return;
        }

        _listView.Items.Add(addText);
        this.Controls["textBox"]!.Text = "";
        _listView.SelectedItems.Clear();
    }

    public void RemoveButton_Click(object sender, EventArgs e)
    {
        if (_listView.SelectedIndices.Count < 0)
        {
            MessageBox.Show("삭제할 항목을 선택해주세요.");
            return;
        }

        foreach (int selectedIndex in _listView.SelectedIndices)
        {
            _listView.Items.RemoveAt(selectedIndex);
        }
        
        _listView.SelectedItems.Clear();
    }

    public void SaveButton_Click(object sender, EventArgs e)
    {
        using SaveFileDialog sfd = new SaveFileDialog();
        sfd.Filter = "텍스트 파일|*.txt";

        if (sfd.ShowDialog() != DialogResult.OK)
            return;
        
        using StreamWriter sw = new StreamWriter(sfd.FileName);
            
        foreach (ListViewItem item in _listView.Items)
        {
            sw.WriteLine(item.Text);
        }
    }

    public void OpenButton_Click(object sender, EventArgs e)
    {
        using OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "텍스트파일|*.txt";

        if (ofd.ShowDialog() != DialogResult.OK)
            return;

        _listView.Items.Clear();
        using StreamReader sr = new StreamReader(ofd.FileName);
        string line;
            
        while ((line = sr.ReadLine()) != null)
        {
            _listView.Items.Add(line);
        }
    }
}