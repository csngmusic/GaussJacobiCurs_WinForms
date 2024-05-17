namespace GaussJacobiCurs_WinForms
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
            ChooseBtn = new Button();
            openfileTxtBox = new TextBox();
            equationsTxtBox = new TextBox();
            equationslbl = new Label();
            calcBtn = new Button();
            jacobiLstBox = new ListBox();
            gaussSeidelLstBox = new ListBox();
            solutionsLbl = new Label();
            OpenFileBtn = new Button();
            JacobiNormLbl = new Label();
            GaussNormLbl = new Label();
            jacobiIterLbl = new Label();
            gaussIterLbl = new Label();
            SuspendLayout();
            // 
            // ChooseBtn
            // 
            ChooseBtn.Location = new Point(448, 12);
            ChooseBtn.Name = "ChooseBtn";
            ChooseBtn.Size = new Size(65, 29);
            ChooseBtn.TabIndex = 0;
            ChooseBtn.Text = "Выбор";
            ChooseBtn.UseVisualStyleBackColor = true;
            ChooseBtn.Click += ChooseBtn_Click;
            // 
            // openfileTxtBox
            // 
            openfileTxtBox.Location = new Point(12, 12);
            openfileTxtBox.Name = "openfileTxtBox";
            openfileTxtBox.Size = new Size(430, 27);
            openfileTxtBox.TabIndex = 1;
            openfileTxtBox.Text = "Путь к файлу";
            // 
            // equationsTxtBox
            // 
            equationsTxtBox.AcceptsReturn = true;
            equationsTxtBox.AcceptsTab = true;
            equationsTxtBox.Location = new Point(12, 101);
            equationsTxtBox.Multiline = true;
            equationsTxtBox.Name = "equationsTxtBox";
            equationsTxtBox.ScrollBars = ScrollBars.Vertical;
            equationsTxtBox.Size = new Size(210, 104);
            equationsTxtBox.TabIndex = 2;
            // 
            // equationslbl
            // 
            equationslbl.AutoSize = true;
            equationslbl.Location = new Point(12, 58);
            equationslbl.Name = "equationslbl";
            equationslbl.Size = new Size(210, 40);
            equationslbl.TabIndex = 3;
            equationslbl.Text = "Система уравнений,\r\nзаписанная в виде матрицы:";
            // 
            // calcBtn
            // 
            calcBtn.Location = new Point(12, 340);
            calcBtn.Name = "calcBtn";
            calcBtn.Size = new Size(582, 29);
            calcBtn.TabIndex = 4;
            calcBtn.Text = "Вычислить";
            calcBtn.UseVisualStyleBackColor = true;
            calcBtn.Click += calcBtn_Click;
            // 
            // jacobiLstBox
            // 
            jacobiLstBox.FormattingEnabled = true;
            jacobiLstBox.Location = new Point(247, 101);
            jacobiLstBox.Name = "jacobiLstBox";
            jacobiLstBox.Size = new Size(171, 104);
            jacobiLstBox.TabIndex = 5;
            // 
            // gaussSeidelLstBox
            // 
            gaussSeidelLstBox.FormattingEnabled = true;
            gaussSeidelLstBox.Location = new Point(424, 101);
            gaussSeidelLstBox.Name = "gaussSeidelLstBox";
            gaussSeidelLstBox.Size = new Size(171, 104);
            gaussSeidelLstBox.TabIndex = 8;
            // 
            // solutionsLbl
            // 
            solutionsLbl.AutoSize = true;
            solutionsLbl.Location = new Point(247, 58);
            solutionsLbl.Name = "solutionsLbl";
            solutionsLbl.Size = new Size(187, 40);
            solutionsLbl.TabIndex = 9;
            solutionsLbl.Text = "Решение СЛАУ методами\r\nЯкоби и Гаусса-Зейделя";
            // 
            // OpenFileBtn
            // 
            OpenFileBtn.Location = new Point(519, 11);
            OpenFileBtn.Name = "OpenFileBtn";
            OpenFileBtn.Size = new Size(76, 29);
            OpenFileBtn.TabIndex = 10;
            OpenFileBtn.Text = "Открыть";
            OpenFileBtn.UseVisualStyleBackColor = true;
            OpenFileBtn.Click += OpenFileBtn_Click;
            // 
            // JacobiNormLbl
            // 
            JacobiNormLbl.AutoSize = true;
            JacobiNormLbl.Location = new Point(247, 208);
            JacobiNormLbl.Name = "JacobiNormLbl";
            JacobiNormLbl.Size = new Size(141, 40);
            JacobiNormLbl.TabIndex = 11;
            JacobiNormLbl.Text = "Норма матрицы B \r\nравна:";
            // 
            // GaussNormLbl
            // 
            GaussNormLbl.AutoSize = true;
            GaussNormLbl.Location = new Point(424, 208);
            GaussNormLbl.Name = "GaussNormLbl";
            GaussNormLbl.Size = new Size(141, 40);
            GaussNormLbl.TabIndex = 12;
            GaussNormLbl.Text = "Норма матрицы B \r\nравна:";
            // 
            // jacobiIterLbl
            // 
            jacobiIterLbl.AutoSize = true;
            jacobiIterLbl.Location = new Point(247, 262);
            jacobiIterLbl.Name = "jacobiIterLbl";
            jacobiIterLbl.Size = new Size(81, 20);
            jacobiIterLbl.TabIndex = 13;
            jacobiIterLbl.Text = "Итераций:\r\n";
            // 
            // gaussIterLbl
            // 
            gaussIterLbl.AutoSize = true;
            gaussIterLbl.Location = new Point(424, 262);
            gaussIterLbl.Name = "gaussIterLbl";
            gaussIterLbl.Size = new Size(81, 20);
            gaussIterLbl.TabIndex = 14;
            gaussIterLbl.Text = "Итераций:\r\n";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(606, 381);
            Controls.Add(gaussIterLbl);
            Controls.Add(jacobiIterLbl);
            Controls.Add(GaussNormLbl);
            Controls.Add(JacobiNormLbl);
            Controls.Add(OpenFileBtn);
            Controls.Add(solutionsLbl);
            Controls.Add(gaussSeidelLstBox);
            Controls.Add(jacobiLstBox);
            Controls.Add(calcBtn);
            Controls.Add(equationslbl);
            Controls.Add(equationsTxtBox);
            Controls.Add(openfileTxtBox);
            Controls.Add(ChooseBtn);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ChooseBtn;
        private TextBox openfileTxtBox;
        private TextBox equationsTxtBox;
        private Label equationslbl;
        private Button calcBtn;
        private ListBox jacobiLstBox;
        private ListBox gaussSeidelLstBox;
        private Label solutionsLbl;
        private Button OpenFileBtn;
        private Label JacobiNormLbl;
        private Label GaussNormLbl;
        private Label jacobiIterLbl;
        private Label gaussIterLbl;
    }
}
