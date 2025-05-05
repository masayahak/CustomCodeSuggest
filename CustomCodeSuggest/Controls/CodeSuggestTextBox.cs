using System.ComponentModel;
using System.Data;

namespace CustomCodeSuggest.Controls
{
    public class コードサジェストアイテム
    {
        public string コード { get; set; } = string.Empty;
        public string 名称 { get; set; } = string.Empty;

        public override string ToString() => $"{コード} {名称}";
    }

    public class CodeSuggestTextBox : UserControl
    {
        private readonly NumericTextBox _textCode;
        private readonly TextBox _textName;

        // 選択する一覧（リストボックス）は親フォームに追加する
        private readonly ListBox _listBox = new ListBox();
        private Form? _parentForm;

        private List<コードサジェストアイテム> _候補リスト = [];
        private int _最大表示件数 = 20;
        private int _codeTextWidth = 100;

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

        [Category("外観"), Description("コードの幅")]
        [DefaultValue(100)]
        public int CodeTextWidth
        {
            get => _codeTextWidth;
            set { if (value > 0) _codeTextWidth = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get => _textCode.Text ?? string.Empty;
#pragma warning disable CS8765
            set => _textCode.Text = value ?? string.Empty;
#pragma warning restore CS8765
        }

        [Category("サジェスト候補リスト"), Description("サジェスト候補リスト")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<コードサジェストアイテム> 候補リスト
        {
            get => _候補リスト;
            set => _候補リスト = value ?? new List<コードサジェストアイテム>();
        }

        [Category("結果"), Description("選択された名称")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextName { get; private set; } = string.Empty;

        // -------------------------------------------------------
        // コンストラクタ
        // -------------------------------------------------------
        public CodeSuggestTextBox()
        {
            _textCode = new NumericTextBox
            {
                Location = new Point(0, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
            };

            _textName = new TextBox
            {
                Location = new Point(_textCode.Right + 5, 0),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                ReadOnly = true,
            };

            Controls.Add(_textCode);
            Controls.Add(_textName);

            // リストボックス初期化時
            _listBox.Visible = false;
            _listBox.TabStop = false;
            _listBox.Click += ListBox_Click;

            _textCode.KeyDown += TextBox_KeyDown;
            _textCode.TextChanged += TextBox_TextChanged;
            _textCode.LostFocus += TextBox_LostFocus;
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
        }

        // -------------------------------------------------------
        // イベントメソッド
        // -------------------------------------------------------
        private void UpdateLayout()
        {
            _textCode.Width = _codeTextWidth;
            _textName.Width = this.Width - _textCode.Width - 5;
            _textCode.Location = new Point(0, 0);
            _textName.Location = new Point(_textCode.Right + 5, 0);
        }

        private void TextBox_TextChanged(object? sender, EventArgs e)
        {
            TextName = string.Empty;
            UpdateIDLabel();
        }

        private void TextBox_Ime変換確定後(object? sender, EventArgs e)
        {
            TextName = string.Empty;
            UpdateIDLabel();
        }

        private void ShowPopupサジェストSafely()
        {
            TextName = string.Empty;
            UpdateIDLabel();
            ShowPopupサジェスト();
        }

        private void ShowPopupサジェスト()
        {
            HidePopup();

            string 入力値 = _textCode.Text;
            if (string.IsNullOrWhiteSpace(入力値)) return;

            var マッチ = _候補リスト
                .Where(x => x.コード.Contains(入力値))
                .Take(最大表示件数)
                .ToList();

            if (マッチ.Count == 0) return;

            _listBox.DataSource = null;
            _listBox.Items.Clear();
            _listBox.DataSource = マッチ;
            _listBox.DisplayMember = nameof(コードサジェストアイテム.ToString);

            var screenPos = _textCode.PointToScreen(new Point(0, _textCode.Height));
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
            if (_listBox.SelectedItem is コードサジェストアイテム item)
            {
                _textCode.Text = item.コード;
                TextName = item.名称;
                UpdateIDLabel();
                HidePopup();
                _textCode.Focus();
                _textCode.SelectionStart = _textCode.Text.Length;
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
            _textName.Text = TextName ?? string.Empty;
        }
    }
}
