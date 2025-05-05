using CustomCodeSuggest.Controls;

namespace CustomCodeSuggest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TextBox得意先 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            SuggestTextBox得意先 = new ImeSuggestTextBox();
            label3 = new Label();
            Cmb得意先 = new ComboBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            HorizontalRadioButtons = new HorizontalRadioButtons();
            ButtonTest = new Button();
            Button南 = new Button();
            label8 = new Label();
            label9 = new Label();
            CodeSuggestTextBox得意先 = new CodeSuggestTextBox();
            SuspendLayout();
            // 
            // TextBox得意先
            // 
            TextBox得意先.Location = new Point(146, 333);
            TextBox得意先.Name = "TextBox得意先";
            TextBox得意先.Size = new Size(147, 23);
            TextBox得意先.TabIndex = 2;
            TextBox得意先.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 336);
            label1.Name = "label1";
            label1.Size = new Size(85, 15);
            label1.TabIndex = 1;
            label1.Text = "通常のサジェスト";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 31);
            label2.Name = "label2";
            label2.Size = new Size(79, 15);
            label2.TabIndex = 3;
            label2.Text = "名称中間一致";
            // 
            // SuggestTextBox得意先
            // 
            SuggestTextBox得意先.Location = new Point(132, 10);
            SuggestTextBox得意先.Name = "SuggestTextBox得意先";
            SuggestTextBox得意先.Placeholder = "(名称などで中間一致検索)";
            SuggestTextBox得意先.Size = new Size(217, 43);
            SuggestTextBox得意先.TabIndex = 3;
            SuggestTextBox得意先.TextWidth = 147;
            SuggestTextBox得意先.最大表示件数 = 20;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(24, 280);
            label3.Name = "label3";
            label3.Size = new Size(94, 15);
            label3.TabIndex = 6;
            label3.Text = "通常コンボボックス";
            // 
            // Cmb得意先
            // 
            Cmb得意先.FormattingEnabled = true;
            Cmb得意先.Location = new Point(146, 277);
            Cmb得意先.Name = "Cmb得意先";
            Cmb得意先.Size = new Size(147, 23);
            Cmb得意先.TabIndex = 1;
            Cmb得意先.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Red;
            label4.Location = new Point(320, 280);
            label4.Name = "label4";
            label4.Size = new Size(85, 15);
            label4.TabIndex = 8;
            label4.Text = "(F4キーでOPEN)";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Red;
            label5.Location = new Point(320, 336);
            label5.Name = "label5";
            label5.Size = new Size(154, 15);
            label5.TabIndex = 9;
            label5.Text = "(前方一致でしか検索できない)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Red;
            label6.Location = new Point(306, 31);
            label6.Name = "label6";
            label6.Size = new Size(140, 15);
            label6.TabIndex = 10;
            label6.Text = "(中間一致で絞り込み可能)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(11, 144);
            label7.Name = "label7";
            label7.Size = new Size(67, 15);
            label7.TabIndex = 11;
            label7.Text = "少数選択肢";
            // 
            // HorizontalRadioButtons
            // 
            HorizontalRadioButtons.Location = new Point(133, 135);
            HorizontalRadioButtons.Name = "HorizontalRadioButtons";
            HorizontalRadioButtons.Size = new Size(177, 24);
            HorizontalRadioButtons.TabIndex = 12;
            HorizontalRadioButtons.選択肢文字列 = "E:東,W:西,S:南,N:北";
            // 
            // ButtonTest
            // 
            ButtonTest.Location = new Point(189, 180);
            ButtonTest.Name = "ButtonTest";
            ButtonTest.Size = new Size(120, 43);
            ButtonTest.TabIndex = 13;
            ButtonTest.Text = "選択結果を表示";
            ButtonTest.UseVisualStyleBackColor = true;
            ButtonTest.Click += ButtonTest_Click;
            // 
            // Button南
            // 
            Button南.Location = new Point(341, 130);
            Button南.Name = "Button南";
            Button南.Size = new Size(120, 29);
            Button南.TabIndex = 14;
            Button南.Text = "「南」を外部からセット";
            Button南.UseVisualStyleBackColor = true;
            Button南.Click += Button南_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(3, 242);
            label8.Name = "label8";
            label8.Size = new Size(581, 15);
            label8.TabIndex = 15;
            label8.Text = "以下、参考 -------------------------------------------------------------------------------------------------------";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(11, 80);
            label9.Name = "label9";
            label9.Size = new Size(79, 15);
            label9.TabIndex = 18;
            label9.Text = "コード中間一致";
            // 
            // CodeSuggestTextBox得意先
            // 
            CodeSuggestTextBox得意先.Location = new Point(132, 74);
            CodeSuggestTextBox得意先.Name = "CodeSuggestTextBox得意先";
            CodeSuggestTextBox得意先.Size = new Size(342, 21);
            CodeSuggestTextBox得意先.TabIndex = 4;
            CodeSuggestTextBox得意先.最大表示件数 = 20;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(507, 370);
            Controls.Add(CodeSuggestTextBox得意先);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(Button南);
            Controls.Add(ButtonTest);
            Controls.Add(HorizontalRadioButtons);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(Cmb得意先);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(TextBox得意先);
            Controls.Add(SuggestTextBox得意先);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox TextBox得意先;
        private Label label1;
        private Label label2;
        private ImeSuggestTextBox SuggestTextBox得意先;
        private Label label3;
        private ComboBox Cmb得意先;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private HorizontalRadioButtons HorizontalRadioButtons;
        private Button ButtonTest;
        private Button Button南;
        private Label label8;
        private Label label9;
        private CodeSuggestTextBox CodeSuggestTextBox得意先;
    }
}
