namespace _10_03;

public partial class Form1 : Form
{
    private Sentence _sentence;
    
    public Form1()
    {
        InitializeComponent();

        _sentence = new Sentence();
        this.ClientSize = new Size(800, 280);

        InitializeUI();
    }
    
    public void InitializeUI()
    {
        Label viewTextLabel = new Label();
        viewTextLabel.Size = new Size(700, 100);
        viewTextLabel.Location = new Point(50, 50);
        viewTextLabel.Text = "";
        viewTextLabel.Tag = 1;
        viewTextLabel.Font = new Font("맑은 고딕", 14f, FontStyle.Bold);
        viewTextLabel.TextAlign = ContentAlignment.MiddleCenter;
        viewTextLabel.BackColor = Color.White;
        this.Controls.Add(viewTextLabel);

        Button createButton = new Button();
        createButton.Size = new Size(100, 50);
        createButton.Location = new Point(350, 200);
        createButton.Text = "보기";
        createButton.Tag = 2;
        createButton.Font = new Font("맑은 고딕", 12f);
        createButton.BackColor = Color.White;
        createButton.Click += CreateButton_Click!;
        this.Controls.Add(createButton);
    }

    // Control의 Tag 속성으로 찾기
    // Control.Tag를 ToString()해서 문자열로 바꿔주고 비교
    // this.Control[]에서 탐색 후 반환 받는것보다 foreach문으로 순회하면서 찾는게 정확
    // 대신 순회하는 배열 또는 컬렉션의 크기가 커질 수록 자원을 많이 잡아먹음
    // 배열 또는 컬렉션의 크기가 큰 경우 Dictionary 사용하는게 효율적일 수 있음
    public void CreateButton_Click(object sender, EventArgs e)
    {
        foreach (Control control in this.Controls)
        {
            if (control.Tag != null && control.Tag.ToString() == "1")
            {
                control.Text = "";
                (string sentence, string person) = _sentence.GetRandomSentence();
                control.Text = $"'{sentence}'\n- {person} -";
            }
            else
            {
                return;
            }
        }
    }
}

// 간단한 튜플 사용, (string, string)
public class Sentence
{
    private Random _random;
    private (string sentence, string person)[] _famousSaying;

    public Sentence()
    {
        _random = new Random();
        
        _famousSaying =
        [
            ("오늘 지긴 했지만.. 저희끼리만 안 무너지면 충분히 이길 수 있을 것 같아요.", "김혁규(Deft), 2022 리그오브레전드 월드 챔피언십 그룹 스테이지 1라운드 로그전에서 패배 후 인터뷰에서."),
            ("4 Chinese Can't Win.", "김동하(Khan)"),
            ("챌린저를 해보니까 원딜은 쉬운 라인인게 맞고, 정글이 솔직히 제일 편하다.", "강승록(TheShy)"),
            ("우리 나이 대에 사랑이나 돈은 조금 늦게 올 수 있지만, 정글은 늦게 오면 안 된다.","김한색(GimGoon)"),
            ("그거 먹고 탑으로 와.", "장경환(Marin)"),
            ("마스터나 브론즈나 똑같다.", "배준식(Bang)"),
            ("앞 비전 뒷 점멸은 잘하는 이즈리얼의 어쩔 수 없는 부분.", "이민형(Gumayusi)"),
            ("사고 값은 아호이로 받을게.", "정지훈(Chovy), 여캠을 보며 한 말."),
            ("끝까지 가면 내가 다 이겨.", "황성훈(Kingen)"),
            ("니네가 라인을 못 서니까 서포터를 선택한 거 아냐? 그렇게 잘 했으면 탑, 미드로 프로했겠지.", "이서행(Kuro), 울프와 고릴라에게."),
            ("D 점멸을 쓰면 실버로 가는 거에요.", "이상혁(Faker)"),
            ("정글이 왜 이렇게 고집이 세냐. 시키는대로 좀 해라.", "최현준(Doran)"),
            ("솔킬 따서 뭐합니까? 제가 이겼어요 이 판.", "허수(Showmaker)"),
            ("모른다고 해서 계속 혼자 찾는 것보다 남의 것을 보고 얘네는 어떻게 했나를 보는게 더 나아요.", "조건희(BeryL), 롤드컵 기간 동안 솔랭을 한 판도 돌리지 않으며."),
            ("사람은 맞아야 정신을 차린다. 그리고 내가 팀원들을 정신 차리게 해줬다.", "강찬용(Ambition)"),
            ("정규에서 다 지고 상대가 우리보다 강해도, 결승에서 딱 하루만 쟤들보다 잘해서 이기면 된다.", "한왕호(Peanut)")
        ];
    }

    public (string, string) GetRandomSentence()
    {
        return _famousSaying[_random.Next(_famousSaying.Length)];
    }
}