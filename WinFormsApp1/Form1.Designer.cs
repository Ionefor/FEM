namespace WinFormsApp1
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
            XOY = new Panel();
            Parametrs = new Panel();
            sizeText = new Label();
            mediumSizeGrid = new RadioButton();
            minSizeGrid = new RadioButton();
            maxSizeGrid = new RadioButton();
            Kyy = new TextBox();
            KyyText = new Label();
            Kxx = new TextBox();
            KxxText = new Label();
            TempNodeText = new Label();
            TempNodeBut = new Button();
            TempNode = new TextBox();
            NumNodesText = new Label();
            Save = new Button();
            NumNodes = new TextBox();
            EnableNodes = new Button();
            GetSolution = new Button();
            Change = new Button();
            Show = new Button();
            qtxt = new Label();
            qval = new TextBox();
            Tval = new TextBox();
            Ttxt = new Label();
            htxt = new Label();
            hval = new TextBox();
            Numtext = new Label();
            NumG = new TextBox();
            Grantext = new Label();
            Qv = new TextBox();
            Qt = new Label();
            DataText = new Label();
            Parametrs.SuspendLayout();
            SuspendLayout();
            // 
            // XOY
            // 
            XOY.BackColor = Color.Linen;
            XOY.Dock = DockStyle.Fill;
            XOY.Location = new Point(0, 0);
            XOY.Name = "XOY";
            XOY.Size = new Size(1442, 666);
            XOY.TabIndex = 0;
            XOY.MouseClick += XOY_MouseClick;
            // 
            // Parametrs
            // 
            Parametrs.BackColor = Color.FromArgb(204, 204, 255);
            Parametrs.Controls.Add(sizeText);
            Parametrs.Controls.Add(mediumSizeGrid);
            Parametrs.Controls.Add(minSizeGrid);
            Parametrs.Controls.Add(maxSizeGrid);
            Parametrs.Controls.Add(Kyy);
            Parametrs.Controls.Add(KyyText);
            Parametrs.Controls.Add(Kxx);
            Parametrs.Controls.Add(KxxText);
            Parametrs.Controls.Add(TempNodeText);
            Parametrs.Controls.Add(TempNodeBut);
            Parametrs.Controls.Add(TempNode);
            Parametrs.Controls.Add(NumNodesText);
            Parametrs.Controls.Add(Save);
            Parametrs.Controls.Add(NumNodes);
            Parametrs.Controls.Add(EnableNodes);
            Parametrs.Controls.Add(GetSolution);
            Parametrs.Controls.Add(Change);
            Parametrs.Controls.Add(Show);
            Parametrs.Controls.Add(qtxt);
            Parametrs.Controls.Add(qval);
            Parametrs.Controls.Add(Tval);
            Parametrs.Controls.Add(Ttxt);
            Parametrs.Controls.Add(htxt);
            Parametrs.Controls.Add(hval);
            Parametrs.Controls.Add(Numtext);
            Parametrs.Controls.Add(NumG);
            Parametrs.Controls.Add(Grantext);
            Parametrs.Controls.Add(Qv);
            Parametrs.Controls.Add(Qt);
            Parametrs.Controls.Add(DataText);
            Parametrs.Dock = DockStyle.Right;
            Parametrs.Location = new Point(1032, 0);
            Parametrs.Name = "Parametrs";
            Parametrs.Size = new Size(410, 666);
            Parametrs.TabIndex = 1;
            // 
            // sizeText
            // 
            sizeText.AutoSize = true;
            sizeText.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            sizeText.Location = new Point(38, 297);
            sizeText.Name = "sizeText";
            sizeText.Size = new Size(51, 28);
            sizeText.TabIndex = 29;
            sizeText.Text = "Size:";
            // 
            // mediumSizeGrid
            // 
            mediumSizeGrid.AutoSize = true;
            mediumSizeGrid.BackColor = Color.Transparent;
            mediumSizeGrid.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            mediumSizeGrid.Location = new Point(20, 360);
            mediumSizeGrid.Name = "mediumSizeGrid";
            mediumSizeGrid.Size = new Size(102, 32);
            mediumSizeGrid.TabIndex = 28;
            mediumSizeGrid.Text = "medium";
            mediumSizeGrid.UseVisualStyleBackColor = false;
            // 
            // minSizeGrid
            // 
            minSizeGrid.AutoSize = true;
            minSizeGrid.BackColor = Color.Transparent;
            minSizeGrid.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            minSizeGrid.Location = new Point(20, 398);
            minSizeGrid.Name = "minSizeGrid";
            minSizeGrid.Size = new Size(63, 32);
            minSizeGrid.TabIndex = 27;
            minSizeGrid.Text = "min";
            minSizeGrid.UseVisualStyleBackColor = false;
            // 
            // maxSizeGrid
            // 
            maxSizeGrid.AutoSize = true;
            maxSizeGrid.BackColor = Color.Transparent;
            maxSizeGrid.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            maxSizeGrid.Location = new Point(20, 322);
            maxSizeGrid.Name = "maxSizeGrid";
            maxSizeGrid.Size = new Size(66, 32);
            maxSizeGrid.TabIndex = 26;
            maxSizeGrid.Text = "max";
            maxSizeGrid.UseVisualStyleBackColor = false;
            // 
            // Kyy
            // 
            Kyy.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Kyy.Location = new Point(244, 246);
            Kyy.Name = "Kyy";
            Kyy.Size = new Size(70, 34);
            Kyy.TabIndex = 25;
            Kyy.TextAlign = HorizontalAlignment.Center;
            // 
            // KyyText
            // 
            KyyText.AutoSize = true;
            KyyText.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            KyyText.Location = new Point(191, 246);
            KyyText.Name = "KyyText";
            KyyText.Size = new Size(47, 28);
            KyyText.TabIndex = 24;
            KyyText.Text = "Kyy:";
            // 
            // Kxx
            // 
            Kxx.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Kxx.Location = new Point(90, 246);
            Kxx.Name = "Kxx";
            Kxx.Size = new Size(70, 34);
            Kxx.TabIndex = 23;
            Kxx.TextAlign = HorizontalAlignment.Center;
            // 
            // KxxText
            // 
            KxxText.AutoSize = true;
            KxxText.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            KxxText.Location = new Point(38, 246);
            KxxText.Name = "KxxText";
            KxxText.Size = new Size(46, 28);
            KxxText.TabIndex = 22;
            KxxText.Text = "Kxx:";
            // 
            // TempNodeText
            // 
            TempNodeText.AutoSize = true;
            TempNodeText.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            TempNodeText.Location = new Point(218, 520);
            TempNodeText.Name = "TempNodeText";
            TempNodeText.Size = new Size(26, 28);
            TempNodeText.TabIndex = 21;
            TempNodeText.Text = "T:";
            // 
            // TempNodeBut
            // 
            TempNodeBut.BackColor = Color.Gainsboro;
            TempNodeBut.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            TempNodeBut.Location = new Point(218, 560);
            TempNodeBut.Name = "TempNodeBut";
            TempNodeBut.Size = new Size(180, 80);
            TempNodeBut.TabIndex = 20;
            TempNodeBut.Text = "Показать температуру в узле\r\n";
            TempNodeBut.UseVisualStyleBackColor = false;
            TempNodeBut.Click += TempNodeBut_Click;
            // 
            // TempNode
            // 
            TempNode.Enabled = false;
            TempNode.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            TempNode.Location = new Point(253, 517);
            TempNode.Name = "TempNode";
            TempNode.Size = new Size(100, 34);
            TempNode.TabIndex = 19;
            // 
            // NumNodesText
            // 
            NumNodesText.AutoSize = true;
            NumNodesText.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            NumNodesText.Location = new Point(20, 517);
            NumNodesText.Name = "NumNodesText";
            NumNodesText.Size = new Size(34, 28);
            NumNodesText.TabIndex = 18;
            NumNodesText.Text = "№";
            // 
            // Save
            // 
            Save.BackColor = Color.Gainsboro;
            Save.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Save.Location = new Point(218, 430);
            Save.Name = "Save";
            Save.Size = new Size(170, 68);
            Save.TabIndex = 15;
            Save.Text = "Сохранить в файл";
            Save.UseVisualStyleBackColor = false;
            Save.Click += Save_Click;
            // 
            // NumNodes
            // 
            NumNodes.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            NumNodes.Location = new Point(60, 517);
            NumNodes.Name = "NumNodes";
            NumNodes.Size = new Size(100, 34);
            NumNodes.TabIndex = 17;
            // 
            // EnableNodes
            // 
            EnableNodes.BackColor = Color.Gainsboro;
            EnableNodes.Font = new Font("Segoe UI", 13F, FontStyle.Regular, GraphicsUnit.Point);
            EnableNodes.Location = new Point(18, 560);
            EnableNodes.Name = "EnableNodes";
            EnableNodes.Size = new Size(170, 80);
            EnableNodes.TabIndex = 16;
            EnableNodes.Text = "Показать выбранные узлы";
            EnableNodes.UseVisualStyleBackColor = false;
            EnableNodes.Click += EnableNodes_Click;
            // 
            // GetSolution
            // 
            GetSolution.BackColor = Color.Gainsboro;
            GetSolution.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            GetSolution.Location = new Point(27, 430);
            GetSolution.Name = "GetSolution";
            GetSolution.Size = new Size(170, 68);
            GetSolution.TabIndex = 14;
            GetSolution.Text = "Получить решение";
            GetSolution.UseVisualStyleBackColor = false;
            GetSolution.Click += GetSolution_Click;
            // 
            // Change
            // 
            Change.BackColor = Color.Gainsboro;
            Change.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Change.Location = new Point(129, 371);
            Change.Name = "Change";
            Change.Size = new Size(170, 43);
            Change.TabIndex = 13;
            Change.Text = "Изменить";
            Change.UseVisualStyleBackColor = false;
            Change.Click += Change_Click;
            // 
            // Show
            // 
            Show.BackColor = Color.Gainsboro;
            Show.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Show.Location = new Point(129, 297);
            Show.Name = "Show";
            Show.Size = new Size(170, 57);
            Show.TabIndex = 12;
            Show.Text = "Показать";
            Show.UseVisualStyleBackColor = false;
            Show.Click += Show_Click;
            // 
            // qtxt
            // 
            qtxt.AutoSize = true;
            qtxt.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            qtxt.Location = new Point(267, 192);
            qtxt.Name = "qtxt";
            qtxt.Size = new Size(28, 28);
            qtxt.TabIndex = 11;
            qtxt.Text = "q:";
            // 
            // qval
            // 
            qval.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            qval.Location = new Point(301, 192);
            qval.Name = "qval";
            qval.Size = new Size(70, 34);
            qval.TabIndex = 10;
            qval.TextAlign = HorizontalAlignment.Center;
            // 
            // Tval
            // 
            Tval.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Tval.Location = new Point(191, 192);
            Tval.Name = "Tval";
            Tval.Size = new Size(70, 34);
            Tval.TabIndex = 9;
            Tval.TextAlign = HorizontalAlignment.Center;
            // 
            // Ttxt
            // 
            Ttxt.AutoSize = true;
            Ttxt.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Ttxt.Location = new Point(159, 192);
            Ttxt.Name = "Ttxt";
            Ttxt.Size = new Size(26, 28);
            Ttxt.TabIndex = 8;
            Ttxt.Text = "T:";
            // 
            // htxt
            // 
            htxt.AutoSize = true;
            htxt.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            htxt.Location = new Point(38, 198);
            htxt.Name = "htxt";
            htxt.Size = new Size(27, 28);
            htxt.TabIndex = 7;
            htxt.Text = "h:";
            // 
            // hval
            // 
            hval.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            hval.Location = new Point(71, 192);
            hval.Name = "hval";
            hval.Size = new Size(70, 34);
            hval.TabIndex = 6;
            hval.TextAlign = HorizontalAlignment.Center;
            // 
            // Numtext
            // 
            Numtext.AutoSize = true;
            Numtext.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Numtext.Location = new Point(126, 144);
            Numtext.Name = "Numtext";
            Numtext.Size = new Size(34, 28);
            Numtext.TabIndex = 5;
            Numtext.Text = "№";
            // 
            // NumG
            // 
            NumG.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            NumG.Location = new Point(166, 141);
            NumG.Name = "NumG";
            NumG.Size = new Size(100, 34);
            NumG.TabIndex = 4;
            NumG.TextAlign = HorizontalAlignment.Center;
            // 
            // Grantext
            // 
            Grantext.AutoSize = true;
            Grantext.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Grantext.Location = new Point(129, 101);
            Grantext.Name = "Grantext";
            Grantext.Size = new Size(168, 28);
            Grantext.TabIndex = 3;
            Grantext.Text = "Граница области";
            // 
            // Qv
            // 
            Qv.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Qv.Location = new Point(166, 55);
            Qv.Name = "Qv";
            Qv.Size = new Size(100, 34);
            Qv.TabIndex = 2;
            Qv.TextAlign = HorizontalAlignment.Center;
            // 
            // Qt
            // 
            Qt.AutoSize = true;
            Qt.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Qt.Location = new Point(129, 55);
            Qt.Name = "Qt";
            Qt.Size = new Size(31, 28);
            Qt.TabIndex = 1;
            Qt.Text = "Q:";
            // 
            // DataText
            // 
            DataText.AutoSize = true;
            DataText.Font = new Font("Segoe UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            DataText.Location = new Point(93, 9);
            DataText.Name = "DataText";
            DataText.Size = new Size(224, 28);
            DataText.TabIndex = 0;
            DataText.Text = "Введенные параметры:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(1442, 666);
            Controls.Add(Parametrs);
            Controls.Add(XOY);
            Name = "Form1";
            Text = "MKE";
            Load += Form1_Load;
            Parametrs.ResumeLayout(false);
            Parametrs.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel XOY;
        private Panel Parametrs;
        private Label Qt;
        private Label DataText;
        private Button Save;
        private Button GetSolution;
        private Button Change;
        private new Button Show;
        private Label qtxt;
        private TextBox qval;
        private TextBox Tval;
        private Label Ttxt;
        private Label htxt;
        private TextBox hval;
        private Label Numtext;
        private TextBox NumG;
        private Label Grantext;
        private TextBox Qv;
        private Button EnableNodes;
        private TextBox NumNodes;
        private Label NumNodesText;
        private Label TempNodeText;
        private Button TempNodeBut;
        private TextBox TempNode;
        private TextBox Kyy;
        private Label KyyText;
        private TextBox Kxx;
        private Label KxxText;
        private RadioButton mediumSizeGrid;
        private RadioButton minSizeGrid;
        private RadioButton maxSizeGrid;
        private Label sizeText;
    }
}