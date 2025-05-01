namespace CustomCodeSuggest.Controls
{
    // -------------------------------------------------------
    // IMEで日本語入力すると、TextChangedイベントは入力された文字列の
    // 1文字目でまずは発火する。
    // 例）”あいうえお”と入力すると、TextChangedイベントでTextBox.Textは”あ”になる。
    // それは困るので"あいうえお"を取得できるようにする。
    // -------------------------------------------------------
    public partial class ImeTextBox : TextBox
    {
        public ImeTextBox()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private const int WM_IME_COMPOSITION = 0x10F;
        private const int GCS_RESULTSTR = 0x800;

        public event EventHandler? Ime変換確定後;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_IME_COMPOSITION)
            {
                if ((m.LParam.ToInt32() & GCS_RESULTSTR) != 0)
                {
                    // すぐだとText更新前なので、次のメッセージループで実行
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        Ime変換確定後?.Invoke(this, EventArgs.Empty);
                    }));
                }
            }
        }
    }
}
