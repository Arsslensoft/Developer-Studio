namespace devstd.network
{
    partial class AddBCAST
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddBCAST));
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.nametext = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.desctext = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.superValidator1 = new DevComponents.DotNetBar.Validator.SuperValidator();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.highlighter1 = new DevComponents.DotNetBar.Validator.Highlighter();
            this.requiredFieldValidator1 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            this.requiredFieldValidator2 = new DevComponents.DotNetBar.Validator.RequiredFieldValidator("Your error message here.");
            this.regularExpressionValidator1 = new DevComponents.DotNetBar.Validator.RegularExpressionValidator();
            this.regularExpressionValidator2 = new DevComponents.DotNetBar.Validator.RegularExpressionValidator();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 0;
            this.labelX1.Text = "Name :";
            // 
            // nametext
            // 
            this.nametext.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.nametext.Border.Class = "TextBoxBorder";
            this.nametext.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.nametext.DisabledBackColor = System.Drawing.Color.White;
            this.nametext.ForeColor = System.Drawing.Color.Black;
            this.nametext.Location = new System.Drawing.Point(12, 41);
            this.nametext.MaxLength = 50;
            this.nametext.Name = "nametext";
            this.nametext.PreventEnterBeep = true;
            this.nametext.Size = new System.Drawing.Size(309, 22);
            this.nametext.TabIndex = 1;
            this.superValidator1.SetValidator1(this.nametext, this.requiredFieldValidator1);
            this.superValidator1.SetValidator2(this.nametext, this.regularExpressionValidator1);
            // 
            // desctext
            // 
            this.desctext.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.desctext.Border.Class = "TextBoxBorder";
            this.desctext.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.desctext.DisabledBackColor = System.Drawing.Color.White;
            this.desctext.ForeColor = System.Drawing.Color.Black;
            this.desctext.Location = new System.Drawing.Point(12, 109);
            this.desctext.MaxLength = 80;
            this.desctext.Multiline = true;
            this.desctext.Name = "desctext";
            this.desctext.PreventEnterBeep = true;
            this.desctext.Size = new System.Drawing.Size(309, 141);
            this.desctext.TabIndex = 3;
            this.superValidator1.SetValidator1(this.desctext, this.requiredFieldValidator2);
            this.superValidator1.SetValidator2(this.desctext, this.regularExpressionValidator2);
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.labelX2.Location = new System.Drawing.Point(12, 80);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "Description :";
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(246, 276);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(75, 23);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.buttonX1.TabIndex = 4;
            this.buttonX1.Text = "Create";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // superValidator1
            // 
            this.superValidator1.ContainerControl = this;
            this.superValidator1.ErrorProvider = this.errorProvider1;
            this.superValidator1.Highlighter = this.highlighter1;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            this.errorProvider1.Icon = ((System.Drawing.Icon)(resources.GetObject("errorProvider1.Icon")));
            // 
            // highlighter1
            // 
            this.highlighter1.ContainerControl = this;
            // 
            // requiredFieldValidator1
            // 
            this.requiredFieldValidator1.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator1.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // requiredFieldValidator2
            // 
            this.requiredFieldValidator2.ErrorMessage = "Your error message here.";
            this.requiredFieldValidator2.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            // 
            // regularExpressionValidator1
            // 
            this.regularExpressionValidator1.ErrorMessage = "Your error message here.";
            this.regularExpressionValidator1.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            this.regularExpressionValidator1.ValidationExpression = "^[a-zA-Z\'\'-\'\\s]{1,50}$";
            // 
            // regularExpressionValidator2
            // 
            this.regularExpressionValidator2.ErrorMessage = "Your error message here.";
            this.regularExpressionValidator2.HighlightColor = DevComponents.DotNetBar.Validator.eHighlightColor.Red;
            this.regularExpressionValidator2.ValidationExpression = "^[a-zA-Z0-9\'\'-\'\\s]{1,80}$";
            // 
            // AddBCAST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 311);
            this.Controls.Add(this.buttonX1);
            this.Controls.Add(this.desctext);
            this.Controls.Add(this.labelX2);
            this.Controls.Add(this.nametext);
            this.Controls.Add(this.labelX1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddBCAST";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Add New BroadCast";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DevComponents.DotNetBar.Validator.SuperValidator superValidator1;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator2;
        private DevComponents.DotNetBar.Validator.RegularExpressionValidator regularExpressionValidator2;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private DevComponents.DotNetBar.Validator.Highlighter highlighter1;
        private DevComponents.DotNetBar.Validator.RequiredFieldValidator requiredFieldValidator1;
        private DevComponents.DotNetBar.Validator.RegularExpressionValidator regularExpressionValidator1;
        public DevComponents.DotNetBar.Controls.TextBoxX nametext;
        public DevComponents.DotNetBar.Controls.TextBoxX desctext;
    }
}