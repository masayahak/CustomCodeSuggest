using System.ComponentModel;

namespace CustomCodeSuggest.Controls
{
    public class NumericTextBox : TextBox
    {
        [Browsable(true)]
        [Category("動作")]
        [Description("0 から 9 の数字のみ入力可能なテキストボックスです。")]
        public NumericTextBox()
        {
            ImeMode = ImeMode.Disable;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            // 数字と制御文字以外は無効
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            // 貼り付け時などに数字以外があれば除去
            string digitsOnly = new string(Text.Where(char.IsDigit).ToArray());
            if (Text != digitsOnly)
            {
                int selStart = SelectionStart;
                Text = digitsOnly;
                SelectionStart = Math.Min(selStart, Text.Length);
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_PASTE = 0x0302;
            if (m.Msg == WM_PASTE)
            {
                string clipboardText = Clipboard.GetText();
                if (!clipboardText.All(char.IsDigit))
                {
                    // 数字以外なら貼り付け無効
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
