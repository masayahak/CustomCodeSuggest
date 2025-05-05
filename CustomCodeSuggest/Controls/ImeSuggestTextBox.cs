using System.ComponentModel;
using System.Runtime.InteropServices;

namespace CustomCodeSuggest.Controls
{
    public class 名称サジェストアイテム
    {
        public string ID { get; set; } = string.Empty;
        public string 名称 { get; set; } = string.Empty;
        public string 補助検索キー { get; set; } = string.Empty;

        public override string ToString() => $"{ID} {名称}";
    }

    public class ImeSuggestTextBox : UserControl
    {
        // プレースホルダー用 Win32 API
        private const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private readonly ImeTextBox _textBox;
        private readonly Label _labelID;

        // 選択する一覧（リストボックス）は親フォームに追加する
        private readonly ListBox _listBox = new ListBox();
        private Form? _parentForm;

        private List<名称サジェストアイテム> _候補リスト = [];
        private int _最大表示件数 = 20;
        private int _textWidth = 100;
        private string _placeholderText = string.Empty;

        // -------------------------------------------------------
        // プロパティ
        // -------------------------------------------------------
        [Category("外観"), Description("サジェスト最大表示件数")]
        [DefaultValue(10)]
        public int 最大表示件数
        {
            get => _最大表示件数;
            set => _最大表示件数 = value > 0 ? value : 10;
        }

        [Category("外観"), Description("テキストボックスの幅")]
        [DefaultValue(100)]
        public int TextWidth
        {
            get => _textWidth;
            set { if (value > 0) _textWidth = value; }
        }

        [Category("外観"), Description("プレースホルダーテキスト")]
        [DefaultValue("")]
        public string Placeholder
        {
            get => _placeholderText;
            set { _placeholderText = value ?? string.Empty; SetPlaceholder(); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get => _textBox.Text ?? string.Empty;
#pragma warning disable CS8765
            set => _textBox.Text = value ?? string.Empty;
#pragma warning restore CS8765
        }

        [Category("サジェスト候補リスト"), Description("サジェスト候補リスト")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<名称サジェストアイテム> 候補リスト
        {
            get => _候補リスト;
            set => _候補リスト = value ?? new List<名称サジェストアイテム>();
        }

        [Category("結果"), Description("選択されたID")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ID { get; private set; } = string.Empty;

        // -------------------------------------------------------
        // コンストラクタ
        // -------------------------------------------------------
        public ImeSuggestTextBox()
        {
            _textBox = new ImeTextBox
            {
                Location = new Point(0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
            };

            _labelID = new Label
            {
                AutoSize = true,
                BorderStyle = BorderStyle.None,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
            };

            Controls.Add(_labelID);
            Controls.Add(_textBox);

            // リストボックス初期化時
            _listBox.Visible = false;
            _listBox.TabStop = false;
            _listBox.Click += ListBox_Click;

            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.Ime変換確定後 += TextBox_Ime変換確定後;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.LostFocus += TextBox_LostFocus;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            _parentForm = FindForm();
            if (_parentForm != null)
            {
                _parentForm.Controls.Add(_listBox);
                _parentForm.Controls.SetChildIndex(_listBox, 0);
                _parentForm.FormClosed += (s, e) => _listBox.Dispose();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateLayout();
            SetPlaceholder();
        }

        // -------------------------------------------------------
        // イベントメソッド
        // -------------------------------------------------------
        private void UpdateLayout()
        {
            _textBox.Width = _textWidth;
            _labelID.Width = _textBox.Width;
            _labelID.Location = new Point(0, 0);
            _textBox.Location = new Point(0, _labelID.Bottom);
        }

        private void SetPlaceholder()
        {
            if (_textBox.IsHandleCreated)
                SendMessage(_textBox.Handle, EM_SETCUEBANNER, (IntPtr)1, _placeholderText);
        }

        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            if (InputLanguage.CurrentInputLanguage.LayoutName.Contains("Japanese")) return;
            ID = string.Empty;
            UpdateIDLabel();
        }

        private void TextBox_Ime変換確定後(object? sender, EventArgs e)
        {
            ID = string.Empty;
            UpdateIDLabel();
        }

        private void ShowPopupサジェストSafely()
        {
            ID = string.Empty;
            UpdateIDLabel();
            ShowPopupサジェスト();
        }

        private void ShowPopupサジェスト()
        {
            HidePopup();

            string 入力値 = _textBox.Text;
            if (string.IsNullOrWhiteSpace(入力値)) return;

            var マッチ = _候補リスト
                .Where(x => x.ID.Contains(入力値) || x.名称.Contains(入力値) || x.補助検索キー.Contains(入力値))
                .Take(最大表示件数)
                .ToList();

            if (マッチ.Count == 0) return;

            _listBox.DataSource = null;
            _listBox.Items.Clear();
            _listBox.DataSource = マッチ;
            _listBox.DisplayMember = nameof(名称サジェストアイテム.ToString);

            var screenPos = _textBox.PointToScreen(new Point(0, _textBox.Height));
            var clientPos = _parentForm!.PointToClient(screenPos);
            _listBox.Location = clientPos;

            // 表示位置（スクリーン座標）から高さの上限を計算
            int itemHeight = _listBox.ItemHeight > 0 ? _listBox.ItemHeight : 15;
            int maxHeight = _parentForm.Bottom - screenPos.Y - 10; // リストの表示開始位置から画面下端まで

            // 実際に必要な高さを計算して制限
            int desiredHeight = itemHeight * Math.Min(最大表示件数, マッチ.Count);
            int height = Math.Min(Math.Max(30, desiredHeight), maxHeight);

            _listBox.Size = new Size(this.Width, height);
            _listBox.Visible = true;
            _listBox.BringToFront();
        }

        private void HidePopup()
        {
            _listBox.Visible = false;
        }

        private void ListBox_Click(object? sender, EventArgs e)
        {
            if (_listBox.SelectedItem is 名称サジェストアイテム item)
            {
                _textBox.Text = item.名称;
                ID = item.ID;
                UpdateIDLabel();
                HidePopup();
                _textBox.Focus();
                _textBox.SelectionStart = _textBox.Text.Length;
            }
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (!_listBox.Visible)
            {
                if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
                {
                    ShowPopupサジェストSafely();
                    e.Handled = true;
                    return;
                }
                return;
            }

            if (e.KeyCode == Keys.Down)
            {
                if (_listBox.SelectedIndex < _listBox.Items.Count - 1)
                    _listBox.SelectedIndex++;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (_listBox.SelectedIndex > 0)
                    _listBox.SelectedIndex--;
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                ListBox_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HidePopup();
                e.Handled = true;
            }
        }

        private void TextBox_LostFocus(object? sender, EventArgs e)
        {
            if (!_listBox.Focused && !this.ContainsFocus)
                HidePopup();
        }

        private void UpdateIDLabel()
        {
            _labelID.Text = string.IsNullOrEmpty(ID) ? string.Empty : $"({ID})";
        }
    }
}
