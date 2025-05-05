using System.ComponentModel;

namespace CustomCodeSuggest.Controls
{
    public class HorizontalRadioButtons : UserControl
    {
        private readonly FlowLayoutPanel flowPanel = new FlowLayoutPanel();
        private readonly List<(string Key, RadioButton Radio)> radioButtons = new List<(string, RadioButton)>();

        // -------------------------------------------------------
        // プロパティ
        // -------------------------------------------------------
        // 選択肢
        [Category("選択肢"), Description("Key:名称 をカンマ区切りで指定します。例 'E:東,W:西,S:南,N:北'")]
        [DefaultValue("")]
        public string 選択肢文字列
        {
            get => _選択肢文字列;
            set
            {
                _選択肢文字列 = value;
                Parse選択肢文字列(value);
                CreateRadioButtons();
            }
        }
        private string _選択肢文字列 = "";

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, string> 選択肢辞書 { get; private set; } = new();

        private void Parse選択肢文字列(string text)
        {
            var dict = new Dictionary<string, string>();

            foreach (var pair in text.Split(','))
            {
                var kv = pair.Split(':');
                if (kv.Length == 2)
                    dict[kv[0].Trim()] = kv[1].Trim();
            }

            選択肢辞書 = dict;
        }

        // 選択された結果（ID）
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? SelectedID
        {
            get
            {
                var selected = radioButtons.FirstOrDefault(r => r.Radio.Checked);
                return selected.Key;
            }

            // 外部から選択を可能
            set
            {
                var target = radioButtons.FirstOrDefault(r => r.Key == value);
                if (target.Radio != null)
                {
                    target.Radio.Checked = true;
                }
            }
        }

        // 選択された結果（名称）
        public string? SelectedText
        {
            get
            {
                var selected = radioButtons.FirstOrDefault(r => r.Radio.Checked);
                return selected.Radio.Text;
            }
        }

        // 選択変更イベント
        [Category("動作"), Description("選択されたIDまたは名称が変更されたときに発生します。")]
        public event EventHandler? SelectedChanged;

        private void RaiseSelectedChanged()
        {
            SelectedChanged?.Invoke(this, EventArgs.Empty);
        }


        // -------------------------------------------------------
        // コンストラクタ
        // -------------------------------------------------------
        public HorizontalRadioButtons()
        {
            flowPanel.Dock = DockStyle.Fill;
            flowPanel.FlowDirection = FlowDirection.LeftToRight;
            flowPanel.WrapContents = false;
            this.Controls.Add(flowPanel);

            this.KeyDown += HorizontalRadioButtons_KeyDown;
            this.SetStyle(ControlStyles.Selectable, true);
            this.TabStop = true;
        }

        private void CreateRadioButtons()
        {
            flowPanel.Controls.Clear();
            radioButtons.Clear();

            foreach (var pair in 選択肢辞書)
            {
                var radio = new RadioButton
                {
                    Text = pair.Value,
                    AutoSize = true
                };
                radio.CheckedChanged += Radio_CheckedChanged; // ★ここ追加
                flowPanel.Controls.Add(radio);
                radioButtons.Add((pair.Key, radio));
            }

            if (radioButtons.Count > 0)
                radioButtons[0].Radio.Checked = true;
        }

        private void Radio_CheckedChanged(object? sender, EventArgs e)
        {
            if (((RadioButton)sender!).Checked)
            {
                RaiseSelectedChanged();
            }
        }


        // -------------------------------------------------------
        // イベント
        // -------------------------------------------------------
        // 矢印キーで選択を移動イベント
        private void HorizontalRadioButtons_KeyDown(object? sender, KeyEventArgs e)
        {
            int currentIndex = radioButtons.FindIndex(r => r.Radio.Checked);
            if (currentIndex == -1) return;

            if (e.KeyCode == Keys.Right)
            {
                int next = (currentIndex + 1) % radioButtons.Count;
                radioButtons[next].Radio.Checked = true;
            }
            else if (e.KeyCode == Keys.Left)
            {
                int prev = (currentIndex - 1 + radioButtons.Count) % radioButtons.Count;
                radioButtons[prev].Radio.Checked = true;
            }
        }
    }
}
