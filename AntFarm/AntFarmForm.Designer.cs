namespace AntFarm
{
    partial class AntFarmForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Node0");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Node5");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Node8");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Node9");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Node10");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Node6", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18,
            treeNode19});
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Node2", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode20,
            treeNode21});
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("test1");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("test");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("test");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("test");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiNewAntfarm = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cleanUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scenarioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseAntfarmToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManulaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpbxInspect = new System.Windows.Forms.GroupBox();
            this.cbxAnt = new System.Windows.Forms.ComboBox();
            this.lblAnt = new System.Windows.Forms.Label();
            this.lblAntInfo = new System.Windows.Forms.Label();
            this.lblAnthillInfo = new System.Windows.Forms.Label();
            this.cbxAntHill = new System.Windows.Forms.ComboBox();
            this.lblAnthill = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbxWithFood = new System.Windows.Forms.CheckBox();
            this.txtFoodSourceSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbxEnviroSize = new System.Windows.Forms.ComboBox();
            this.btnBrowseFoodSource = new System.Windows.Forms.Button();
            this.btnBrowseAnthill = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFoodSource = new System.Windows.Forms.Label();
            this.lblEnviroment = new System.Windows.Forms.Label();
            this.btnBrowseEviro = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.grpbxControls = new System.Windows.Forms.GroupBox();
            this.lblFoodGenLabel = new System.Windows.Forms.Label();
            this.lblFoodGenRoot = new System.Windows.Forms.Label();
            this.btnFoodGenPause = new System.Windows.Forms.Button();
            this.btnFoodGenStart = new System.Windows.Forms.Button();
            this.btnFoodGeneratorBrowse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.ckbxAutosave = new System.Windows.Forms.CheckBox();
            this.cbxSpeed = new System.Windows.Forms.ComboBox();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.runTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.timeElapsedLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.locationSelector = new System.Windows.Forms.TreeView();
            this.entitySelector = new System.Windows.Forms.ListView();
            this.gbDirectorySelector = new System.Windows.Forms.GroupBox();
            this.gbEntitySelector = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.gbSelectedEntities = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.grpbxInspect.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpbxControls.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.gbDirectorySelector.SuspendLayout();
            this.gbEntitySelector.SuspendLayout();
            this.gbSelectedEntities.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(998, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiNewAntfarm,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.cleanUpToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // tsiNewAntfarm
            // 
            this.tsiNewAntfarm.Name = "tsiNewAntfarm";
            this.tsiNewAntfarm.Size = new System.Drawing.Size(152, 22);
            this.tsiNewAntfarm.Text = "New AntFarm";
            this.tsiNewAntfarm.Click += new System.EventHandler(this.tsiNewAntfarm_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // cleanUpToolStripMenuItem
            // 
            this.cleanUpToolStripMenuItem.Name = "cleanUpToolStripMenuItem";
            this.cleanUpToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.cleanUpToolStripMenuItem.Text = "Clean Up";
            this.cleanUpToolStripMenuItem.Click += new System.EventHandler(this.cleanUpToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scenarioToolStripMenuItem,
            this.CloseAntfarmToolStripItem});
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            // 
            // scenarioToolStripMenuItem
            // 
            this.scenarioToolStripMenuItem.Name = "scenarioToolStripMenuItem";
            this.scenarioToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.scenarioToolStripMenuItem.Text = "Scenario";
            this.scenarioToolStripMenuItem.Click += new System.EventHandler(this.scenarioToolStripMenuItem_Click);
            // 
            // CloseAntfarmToolStripItem
            // 
            this.CloseAntfarmToolStripItem.Name = "CloseAntfarmToolStripItem";
            this.CloseAntfarmToolStripItem.Size = new System.Drawing.Size(152, 22);
            this.CloseAntfarmToolStripItem.Text = "AntFarm";
            this.CloseAntfarmToolStripItem.Click += new System.EventHandler(this.CloseAntfarmToolStripItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.userManulaToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // userManulaToolStripMenuItem
            // 
            this.userManulaToolStripMenuItem.Name = "userManulaToolStripMenuItem";
            this.userManulaToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.userManulaToolStripMenuItem.Text = "User Manual";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.aboutToolStripMenuItem.Text = "About...";
            // 
            // grpbxInspect
            // 
            this.grpbxInspect.Controls.Add(this.cbxAnt);
            this.grpbxInspect.Controls.Add(this.lblAnt);
            this.grpbxInspect.Controls.Add(this.lblAntInfo);
            this.grpbxInspect.Controls.Add(this.lblAnthillInfo);
            this.grpbxInspect.Controls.Add(this.cbxAntHill);
            this.grpbxInspect.Controls.Add(this.lblAnthill);
            this.grpbxInspect.Location = new System.Drawing.Point(424, 27);
            this.grpbxInspect.Name = "grpbxInspect";
            this.grpbxInspect.Size = new System.Drawing.Size(265, 350);
            this.grpbxInspect.TabIndex = 1;
            this.grpbxInspect.TabStop = false;
            this.grpbxInspect.Text = "Inspect";
            // 
            // cbxAnt
            // 
            this.cbxAnt.Enabled = false;
            this.cbxAnt.FormattingEnabled = true;
            this.cbxAnt.Location = new System.Drawing.Point(52, 131);
            this.cbxAnt.Name = "cbxAnt";
            this.cbxAnt.Size = new System.Drawing.Size(71, 21);
            this.cbxAnt.TabIndex = 5;
            this.cbxAnt.DropDown += new System.EventHandler(this.Ants_Update);
            this.cbxAnt.SelectedIndexChanged += new System.EventHandler(this.cbxAnt_SelectedIndexChanged);
            // 
            // lblAnt
            // 
            this.lblAnt.AutoSize = true;
            this.lblAnt.Location = new System.Drawing.Point(6, 134);
            this.lblAnt.Name = "lblAnt";
            this.lblAnt.Size = new System.Drawing.Size(26, 13);
            this.lblAnt.TabIndex = 4;
            this.lblAnt.Text = "Ant:";
            // 
            // lblAntInfo
            // 
            this.lblAntInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAntInfo.Enabled = false;
            this.lblAntInfo.Location = new System.Drawing.Point(9, 156);
            this.lblAntInfo.Name = "lblAntInfo";
            this.lblAntInfo.Size = new System.Drawing.Size(250, 191);
            this.lblAntInfo.TabIndex = 3;
            // 
            // lblAnthillInfo
            // 
            this.lblAnthillInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblAnthillInfo.Enabled = false;
            this.lblAnthillInfo.Location = new System.Drawing.Point(9, 52);
            this.lblAnthillInfo.Name = "lblAnthillInfo";
            this.lblAnthillInfo.Size = new System.Drawing.Size(250, 72);
            this.lblAnthillInfo.TabIndex = 2;
            // 
            // cbxAntHill
            // 
            this.cbxAntHill.FormattingEnabled = true;
            this.cbxAntHill.Location = new System.Drawing.Point(52, 22);
            this.cbxAntHill.Name = "cbxAntHill";
            this.cbxAntHill.Size = new System.Drawing.Size(44, 21);
            this.cbxAntHill.TabIndex = 1;
            this.cbxAntHill.DropDown += new System.EventHandler(this.Anthills_Update);
            this.cbxAntHill.SelectedIndexChanged += new System.EventHandler(this.cbxAnthill_SelectedIndexChanged);
            // 
            // lblAnthill
            // 
            this.lblAnthill.AutoSize = true;
            this.lblAnthill.Location = new System.Drawing.Point(6, 25);
            this.lblAnthill.Name = "lblAnthill";
            this.lblAnthill.Size = new System.Drawing.Size(40, 13);
            this.lblAnthill.TabIndex = 0;
            this.lblAnthill.Text = "AntHill:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbxWithFood);
            this.groupBox1.Controls.Add(this.txtFoodSourceSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbxEnviroSize);
            this.groupBox1.Controls.Add(this.btnBrowseFoodSource);
            this.groupBox1.Controls.Add(this.btnBrowseAnthill);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblFoodSource);
            this.groupBox1.Controls.Add(this.lblEnviroment);
            this.groupBox1.Controls.Add(this.btnBrowseEviro);
            this.groupBox1.Location = new System.Drawing.Point(218, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 350);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create";
            // 
            // ckbxWithFood
            // 
            this.ckbxWithFood.AutoSize = true;
            this.ckbxWithFood.Location = new System.Drawing.Point(9, 72);
            this.ckbxWithFood.Name = "ckbxWithFood";
            this.ckbxWithFood.Size = new System.Drawing.Size(75, 17);
            this.ckbxWithFood.TabIndex = 19;
            this.ckbxWithFood.Text = "With Food";
            this.ckbxWithFood.UseVisualStyleBackColor = true;
            // 
            // txtFoodSourceSize
            // 
            this.txtFoodSourceSize.Location = new System.Drawing.Point(56, 175);
            this.txtFoodSourceSize.Name = "txtFoodSourceSize";
            this.txtFoodSourceSize.Size = new System.Drawing.Size(130, 20);
            this.txtFoodSourceSize.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Size:";
            // 
            // cbxEnviroSize
            // 
            this.cbxEnviroSize.FormattingEnabled = true;
            this.cbxEnviroSize.Items.AddRange(new object[] {
            "Tiny",
            "Small",
            "Medium",
            "Large",
            "Huge",
            "Gargantuan"});
            this.cbxEnviroSize.Location = new System.Drawing.Point(48, 38);
            this.cbxEnviroSize.Name = "cbxEnviroSize";
            this.cbxEnviroSize.Size = new System.Drawing.Size(138, 21);
            this.cbxEnviroSize.TabIndex = 18;
            // 
            // btnBrowseFoodSource
            // 
            this.btnBrowseFoodSource.Location = new System.Drawing.Point(111, 201);
            this.btnBrowseFoodSource.Name = "btnBrowseFoodSource";
            this.btnBrowseFoodSource.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseFoodSource.TabIndex = 11;
            this.btnBrowseFoodSource.Text = "Browse...";
            this.btnBrowseFoodSource.UseVisualStyleBackColor = true;
            this.btnBrowseFoodSource.Click += new System.EventHandler(this.btnBrowseFoodSource_Click);
            // 
            // btnBrowseAnthill
            // 
            this.btnBrowseAnthill.Location = new System.Drawing.Point(111, 106);
            this.btnBrowseAnthill.Name = "btnBrowseAnthill";
            this.btnBrowseAnthill.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseAnthill.TabIndex = 9;
            this.btnBrowseAnthill.Text = "Browse...";
            this.btnBrowseAnthill.UseVisualStyleBackColor = true;
            this.btnBrowseAnthill.Click += new System.EventHandler(this.btnBrowseAnthill_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Size:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ant Hill:";
            // 
            // lblFoodSource
            // 
            this.lblFoodSource.AutoSize = true;
            this.lblFoodSource.Location = new System.Drawing.Point(6, 156);
            this.lblFoodSource.Name = "lblFoodSource";
            this.lblFoodSource.Size = new System.Drawing.Size(71, 13);
            this.lblFoodSource.TabIndex = 0;
            this.lblFoodSource.Text = "Food Source:";
            // 
            // lblEnviroment
            // 
            this.lblEnviroment.AutoSize = true;
            this.lblEnviroment.Location = new System.Drawing.Point(6, 22);
            this.lblEnviroment.Name = "lblEnviroment";
            this.lblEnviroment.Size = new System.Drawing.Size(63, 13);
            this.lblEnviroment.TabIndex = 16;
            this.lblEnviroment.Text = "Enviroment:";
            // 
            // btnBrowseEviro
            // 
            this.btnBrowseEviro.Location = new System.Drawing.Point(111, 68);
            this.btnBrowseEviro.Name = "btnBrowseEviro";
            this.btnBrowseEviro.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseEviro.TabIndex = 15;
            this.btnBrowseEviro.Text = "Browse...";
            this.btnBrowseEviro.UseVisualStyleBackColor = true;
            this.btnBrowseEviro.Click += new System.EventHandler(this.btnBrowseEviro_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.AcceptsReturn = true;
            this.txtOutput.AcceptsTab = true;
            this.txtOutput.BackColor = System.Drawing.Color.White;
            this.txtOutput.Location = new System.Drawing.Point(12, 383);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(677, 136);
            this.txtOutput.TabIndex = 3;
            this.txtOutput.TextChanged += new System.EventHandler(this.OutputChanged);
            // 
            // grpbxControls
            // 
            this.grpbxControls.Controls.Add(this.lblFoodGenLabel);
            this.grpbxControls.Controls.Add(this.lblFoodGenRoot);
            this.grpbxControls.Controls.Add(this.btnFoodGenPause);
            this.grpbxControls.Controls.Add(this.btnFoodGenStart);
            this.grpbxControls.Controls.Add(this.btnFoodGeneratorBrowse);
            this.grpbxControls.Controls.Add(this.label4);
            this.grpbxControls.Controls.Add(this.btnPrint);
            this.grpbxControls.Controls.Add(this.ckbxAutosave);
            this.grpbxControls.Controls.Add(this.cbxSpeed);
            this.grpbxControls.Controls.Add(this.lblSpeed);
            this.grpbxControls.Controls.Add(this.btnPause);
            this.grpbxControls.Controls.Add(this.btnStart);
            this.grpbxControls.Location = new System.Drawing.Point(12, 27);
            this.grpbxControls.Name = "grpbxControls";
            this.grpbxControls.Size = new System.Drawing.Size(200, 347);
            this.grpbxControls.TabIndex = 4;
            this.grpbxControls.TabStop = false;
            this.grpbxControls.Text = "Controls";
            // 
            // lblFoodGenLabel
            // 
            this.lblFoodGenLabel.AutoSize = true;
            this.lblFoodGenLabel.Location = new System.Drawing.Point(12, 141);
            this.lblFoodGenLabel.Name = "lblFoodGenLabel";
            this.lblFoodGenLabel.Size = new System.Drawing.Size(142, 13);
            this.lblFoodGenLabel.TabIndex = 24;
            this.lblFoodGenLabel.Text = "Food Generator Root Folder:";
            // 
            // lblFoodGenRoot
            // 
            this.lblFoodGenRoot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFoodGenRoot.Location = new System.Drawing.Point(12, 168);
            this.lblFoodGenRoot.Name = "lblFoodGenRoot";
            this.lblFoodGenRoot.Size = new System.Drawing.Size(176, 56);
            this.lblFoodGenRoot.TabIndex = 23;
            this.lblFoodGenRoot.Text = "Root Folder";
            // 
            // btnFoodGenPause
            // 
            this.btnFoodGenPause.Enabled = false;
            this.btnFoodGenPause.Location = new System.Drawing.Point(96, 111);
            this.btnFoodGenPause.Name = "btnFoodGenPause";
            this.btnFoodGenPause.Size = new System.Drawing.Size(92, 23);
            this.btnFoodGenPause.TabIndex = 22;
            this.btnFoodGenPause.Text = "Pause";
            this.btnFoodGenPause.UseVisualStyleBackColor = true;
            this.btnFoodGenPause.Click += new System.EventHandler(this.btnFoodGenPause_Click);
            // 
            // btnFoodGenStart
            // 
            this.btnFoodGenStart.Enabled = false;
            this.btnFoodGenStart.Location = new System.Drawing.Point(96, 85);
            this.btnFoodGenStart.Name = "btnFoodGenStart";
            this.btnFoodGenStart.Size = new System.Drawing.Size(92, 23);
            this.btnFoodGenStart.TabIndex = 21;
            this.btnFoodGenStart.Text = "Start";
            this.btnFoodGenStart.UseVisualStyleBackColor = true;
            this.btnFoodGenStart.Click += new System.EventHandler(this.btnFoodGenStart_Click);
            // 
            // btnFoodGeneratorBrowse
            // 
            this.btnFoodGeneratorBrowse.Location = new System.Drawing.Point(12, 111);
            this.btnFoodGeneratorBrowse.Name = "btnFoodGeneratorBrowse";
            this.btnFoodGeneratorBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnFoodGeneratorBrowse.TabIndex = 20;
            this.btnFoodGeneratorBrowse.Text = "Browse...";
            this.btnFoodGeneratorBrowse.UseVisualStyleBackColor = true;
            this.btnFoodGeneratorBrowse.Click += new System.EventHandler(this.btnFoodGeneratorBrowse_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Food Generator:";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(6, 318);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.Text = "Output Stats";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // ckbxAutosave
            // 
            this.ckbxAutosave.AutoSize = true;
            this.ckbxAutosave.Location = new System.Drawing.Point(123, 324);
            this.ckbxAutosave.Name = "ckbxAutosave";
            this.ckbxAutosave.Size = new System.Drawing.Size(71, 17);
            this.ckbxAutosave.TabIndex = 4;
            this.ckbxAutosave.Text = "Autosave";
            this.ckbxAutosave.UseVisualStyleBackColor = true;
            // 
            // cbxSpeed
            // 
            this.cbxSpeed.FormattingEnabled = true;
            this.cbxSpeed.Items.AddRange(new object[] {
            "200",
            "400",
            "600",
            "800",
            "1000",
            "1200",
            "1400",
            "1600",
            "1800",
            "2000"});
            this.cbxSpeed.Location = new System.Drawing.Point(53, 17);
            this.cbxSpeed.Name = "cbxSpeed";
            this.cbxSpeed.Size = new System.Drawing.Size(141, 21);
            this.cbxSpeed.TabIndex = 3;
            this.cbxSpeed.SelectedIndexChanged += new System.EventHandler(this.cbxSpeed_SelectedIndexChanged);
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(6, 20);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(41, 13);
            this.lblSpeed.TabIndex = 2;
            this.lblSpeed.Text = "Speed:";
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(105, 43);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(89, 23);
            this.btnPause.TabIndex = 1;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(9, 43);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(92, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // runTimer
            // 
            this.runTimer.Interval = 1000;
            this.runTimer.Tick += new System.EventHandler(this.runTimerTick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.timeElapsedLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 527);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(998, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip";
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Red;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(51, 17);
            this.statusLabel.Text = "Stopped";
            // 
            // timeElapsedLabel
            // 
            this.timeElapsedLabel.Name = "timeElapsedLabel";
            this.timeElapsedLabel.Size = new System.Drawing.Size(83, 17);
            this.timeElapsedLabel.Text = "Time Elapsed: ";
            // 
            // locationSelector
            // 
            this.locationSelector.Location = new System.Drawing.Point(6, 17);
            this.locationSelector.Name = "locationSelector";
            treeNode12.BackColor = System.Drawing.Color.Red;
            treeNode12.Name = "Node0";
            treeNode12.Text = "Node0";
            treeNode13.Name = "Node1";
            treeNode13.Text = "Node1";
            treeNode14.Name = "Node3";
            treeNode14.Text = "Node3";
            treeNode15.Name = "Node4";
            treeNode15.Text = "Node4";
            treeNode16.Name = "Node5";
            treeNode16.Text = "Node5";
            treeNode17.Name = "Node8";
            treeNode17.Text = "Node8";
            treeNode18.Name = "Node9";
            treeNode18.Text = "Node9";
            treeNode19.Name = "Node10";
            treeNode19.Text = "Node10";
            treeNode20.Name = "Node6";
            treeNode20.Text = "Node6";
            treeNode21.Name = "Node7";
            treeNode21.Text = "Node7";
            treeNode22.Name = "Node2";
            treeNode22.Text = "Node2";
            this.locationSelector.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode12,
            treeNode13,
            treeNode22});
            this.locationSelector.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.locationSelector.Size = new System.Drawing.Size(278, 174);
            this.locationSelector.TabIndex = 6;
            // 
            // entitySelector
            // 
            listViewItem8.ToolTipText = "Test1";
            this.entitySelector.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8});
            this.entitySelector.Location = new System.Drawing.Point(6, 19);
            this.entitySelector.Name = "entitySelector";
            this.entitySelector.Size = new System.Drawing.Size(278, 124);
            this.entitySelector.TabIndex = 7;
            this.entitySelector.UseCompatibleStateImageBehavior = false;
            // 
            // gbDirectorySelector
            // 
            this.gbDirectorySelector.Controls.Add(this.locationSelector);
            this.gbDirectorySelector.Location = new System.Drawing.Point(695, 27);
            this.gbDirectorySelector.Name = "gbDirectorySelector";
            this.gbDirectorySelector.Size = new System.Drawing.Size(290, 195);
            this.gbDirectorySelector.TabIndex = 8;
            this.gbDirectorySelector.TabStop = false;
            this.gbDirectorySelector.Text = "Location Selector";
            // 
            // gbEntitySelector
            // 
            this.gbEntitySelector.Controls.Add(this.entitySelector);
            this.gbEntitySelector.Location = new System.Drawing.Point(695, 228);
            this.gbEntitySelector.Name = "gbEntitySelector";
            this.gbEntitySelector.Size = new System.Drawing.Size(290, 149);
            this.gbEntitySelector.TabIndex = 9;
            this.gbEntitySelector.TabStop = false;
            this.gbEntitySelector.Text = "Entity Selector";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(7, 19);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(277, 111);
            this.listView1.TabIndex = 8;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // gbSelectedEntities
            // 
            this.gbSelectedEntities.Controls.Add(this.listView1);
            this.gbSelectedEntities.Location = new System.Drawing.Point(695, 383);
            this.gbSelectedEntities.Name = "gbSelectedEntities";
            this.gbSelectedEntities.Size = new System.Drawing.Size(290, 136);
            this.gbSelectedEntities.TabIndex = 9;
            this.gbSelectedEntities.TabStop = false;
            this.gbSelectedEntities.Text = "Selected Entities";
            // 
            // AntFarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 549);
            this.Controls.Add(this.gbSelectedEntities);
            this.Controls.Add(this.gbEntitySelector);
            this.Controls.Add(this.gbDirectorySelector);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.grpbxControls);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpbxInspect);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "AntFarmForm";
            this.Text = "AntFarmForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CleanUp);
            this.Load += new System.EventHandler(this.AntFarmForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpbxInspect.ResumeLayout(false);
            this.grpbxInspect.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpbxControls.ResumeLayout(false);
            this.grpbxControls.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.gbDirectorySelector.ResumeLayout(false);
            this.gbEntitySelector.ResumeLayout(false);
            this.gbSelectedEntities.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsiNewAntfarm;
        private System.Windows.Forms.GroupBox grpbxInspect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblFoodSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.GroupBox grpbxControls;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.ComboBox cbxSpeed;
        private System.Windows.Forms.CheckBox ckbxAutosave;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.TextBox txtFoodSourceSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowseFoodSource;
        private System.Windows.Forms.Button btnBrowseAnthill;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ComboBox cbxAntHill;
        private System.Windows.Forms.Label lblAnthill;
        private System.Windows.Forms.Label lblAnthillInfo;
        private System.Windows.Forms.ComboBox cbxAnt;
        private System.Windows.Forms.Label lblAnt;
        private System.Windows.Forms.Label lblAntInfo;
        private System.Windows.Forms.Timer runTimer;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManulaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cleanUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label lblEnviroment;
        private System.Windows.Forms.Button btnBrowseEviro;
        private System.Windows.Forms.ComboBox cbxEnviroSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ckbxWithFood;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel timeElapsedLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.Label lblFoodGenRoot;
        private System.Windows.Forms.Button btnFoodGenPause;
        private System.Windows.Forms.Button btnFoodGenStart;
        private System.Windows.Forms.Button btnFoodGeneratorBrowse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblFoodGenLabel;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scenarioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CloseAntfarmToolStripItem;
        private System.Windows.Forms.TreeView locationSelector;
        private System.Windows.Forms.ListView entitySelector;
        private System.Windows.Forms.GroupBox gbDirectorySelector;
        private System.Windows.Forms.GroupBox gbEntitySelector;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox gbSelectedEntities;
    }
}