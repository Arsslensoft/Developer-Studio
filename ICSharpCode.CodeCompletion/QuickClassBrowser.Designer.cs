namespace ICSharpCode.CodeCompletion
{
    partial class QuickClassBrowser
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickClassBrowser));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.membersbox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this._AutoListIcons = new System.Windows.Forms.ImageList(this.components);
            this.mempic = new System.Windows.Forms.PictureBox();
            this.classessbox = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.classpic = new System.Windows.Forms.PictureBox();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mempic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.classpic)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.panelEx1.Controls.Add(this.membersbox);
            this.panelEx1.Controls.Add(this.mempic);
            this.panelEx1.Controls.Add(this.classessbox);
            this.panelEx1.Controls.Add(this.classpic);
            this.panelEx1.DisabledBackColor = System.Drawing.Color.Empty;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(600, 20);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // membersbox
            // 
            this.membersbox.DisplayMember = "Text";
            this.membersbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.membersbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.membersbox.FormattingEnabled = true;
            this.membersbox.Images = this._AutoListIcons;
            this.membersbox.ItemHeight = 14;
            this.membersbox.Location = new System.Drawing.Point(321, 0);
            this.membersbox.Name = "membersbox";
            this.membersbox.Size = new System.Drawing.Size(279, 20);
            this.membersbox.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.membersbox.TabIndex = 1;
            this.membersbox.WatermarkText = "Members";
            this.membersbox.SelectedIndexChanged += new System.EventHandler(this.membersbox_SelectedIndexChanged);
            // 
            // _AutoListIcons
            // 
            this._AutoListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_AutoListIcons.ImageStream")));
            this._AutoListIcons.TransparentColor = System.Drawing.Color.Magenta;
            this._AutoListIcons.Images.SetKeyName(0, "VSObject_Class.bmp");
            this._AutoListIcons.Images.SetKeyName(1, "VSObject_Class_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(2, "VSObject_Class_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(3, "VSObject_Class_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(4, "VSObject_Class_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(5, "VSObject_Class_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(6, "VSObject_Constant.bmp");
            this._AutoListIcons.Images.SetKeyName(7, "VSObject_Constant_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(8, "VSObject_Constant_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(9, "VSObject_Constant_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(10, "VSObject_Constant_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(11, "VSObject_Constant_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(12, "VSObject_Delegate.bmp");
            this._AutoListIcons.Images.SetKeyName(13, "VSObject_Delegate_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(14, "VSObject_Delegate_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(15, "VSObject_Delegate_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(16, "VSObject_Delegate_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(17, "VSObject_Delegate_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(18, "VSObject_Enum.bmp");
            this._AutoListIcons.Images.SetKeyName(19, "VSObject_Enum_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(20, "VSObject_Enum_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(21, "VSObject_Enum_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(22, "VSObject_Enum_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(23, "VSObject_Enum_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(24, "VSObject_EnumItem.bmp");
            this._AutoListIcons.Images.SetKeyName(25, "VSObject_EnumItem_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(26, "VSObject_EnumItem_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(27, "VSObject_EnumItem_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(28, "VSObject_EnumItem_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(29, "VSObject_EnumItem_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(30, "VSObject_Event.bmp");
            this._AutoListIcons.Images.SetKeyName(31, "VSObject_Event_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(32, "VSObject_Event_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(33, "VSObject_Event_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(34, "VSObject_Event_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(35, "VSObject_Event_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(36, "VSObject_Exception.bmp");
            this._AutoListIcons.Images.SetKeyName(37, "VSObject_Exception_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(38, "VSObject_Exception_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(39, "VSObject_Exception_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(40, "VSObject_Exception_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(41, "VSObject_Exception_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(42, "VSObject_Field.bmp");
            this._AutoListIcons.Images.SetKeyName(43, "VSObject_Field_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(44, "VSObject_Field_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(45, "VSObject_Field_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(46, "VSObject_Field_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(47, "VSObject_Field_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(48, "VSObject_Interface.bmp");
            this._AutoListIcons.Images.SetKeyName(49, "VSObject_Interface_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(50, "VSObject_Interface_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(51, "VSObject_Interface_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(52, "VSObject_Interface_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(53, "VSObject_Interface_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(54, "VSObject_Macro.bmp");
            this._AutoListIcons.Images.SetKeyName(55, "VSObject_Macro_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(56, "VSObject_Macro_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(57, "VSObject_Macro_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(58, "VSObject_Macro_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(59, "VSObject_Macro_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(60, "VSObject_Map.bmp");
            this._AutoListIcons.Images.SetKeyName(61, "VSObject_Map_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(62, "VSObject_Map_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(63, "VSObject_Map_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(64, "VSObject_Map_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(65, "VSObject_Map_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(66, "VSObject_MapItem.bmp");
            this._AutoListIcons.Images.SetKeyName(67, "VSObject_MapItem_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(68, "VSObject_MapItem_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(69, "VSObject_MapItem_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(70, "VSObject_MapItem_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(71, "VSObject_MapItem_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(72, "VSObject_Method.bmp");
            this._AutoListIcons.Images.SetKeyName(73, "VSObject_Method_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(74, "VSObject_Method_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(75, "VSObject_Method_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(76, "VSObject_Method_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(77, "VSObject_Method_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(78, "VSObject_MethodOverload.bmp");
            this._AutoListIcons.Images.SetKeyName(79, "VSObject_MethodOverload_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(80, "VSObject_MethodOverload_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(81, "VSObject_MethodOverload_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(82, "VSObject_MethodOverload_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(83, "VSObject_MethodOverload_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(84, "VSObject_Module.bmp");
            this._AutoListIcons.Images.SetKeyName(85, "VSObject_Module_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(86, "VSObject_Module_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(87, "VSObject_Module_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(88, "VSObject_Module_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(89, "VSObject_Module_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(90, "VSObject_Namespace.bmp");
            this._AutoListIcons.Images.SetKeyName(91, "VSObject_Namespace_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(92, "VSObject_Namespace_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(93, "VSObject_Namespace_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(94, "VSObject_Namespace_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(95, "VSObject_Namespace_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(96, "VSObject_Object.bmp");
            this._AutoListIcons.Images.SetKeyName(97, "VSObject_Object_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(98, "VSObject_Object_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(99, "VSObject_Object_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(100, "VSObject_Object_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(101, "VSObject_Object_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(102, "VSObject_Operator.bmp");
            this._AutoListIcons.Images.SetKeyName(103, "VSObject_Operator_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(104, "VSObject_Operator_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(105, "VSObject_Operator_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(106, "VSObject_Operator_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(107, "VSObject_Operator_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(108, "VSObject_Properties.bmp");
            this._AutoListIcons.Images.SetKeyName(109, "VSObject_Properties_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(110, "VSObject_Properties_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(111, "VSObject_Properties_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(112, "VSObject_Properties_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(113, "VSObject_Properties_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(114, "VSObject_Structure.bmp");
            this._AutoListIcons.Images.SetKeyName(115, "VSObject_Structure_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(116, "VSObject_Structure_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(117, "VSObject_Structure_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(118, "VSObject_Structure_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(119, "VSObject_Structure_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(120, "VSObject_Template.bmp");
            this._AutoListIcons.Images.SetKeyName(121, "VSObject_Template_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(122, "VSObject_Template_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(123, "VSObject_Template_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(124, "VSObject_Template_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(125, "VSObject_Template_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(126, "VSObject_Type.bmp");
            this._AutoListIcons.Images.SetKeyName(127, "VSObject_Type_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(128, "VSObject_Type_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(129, "VSObject_Type_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(130, "VSObject_Type_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(131, "VSObject_Type_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(132, "VSObject_TypeDef.bmp");
            this._AutoListIcons.Images.SetKeyName(133, "VSObject_TypeDef_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(134, "VSObject_TypeDef_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(135, "VSObject_TypeDef_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(136, "VSObject_TypeDef_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(137, "VSObject_TypeDef_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(138, "VSObject_Union.bmp");
            this._AutoListIcons.Images.SetKeyName(139, "VSObject_Union_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(140, "VSObject_Union_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(141, "VSObject_Union_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(142, "VSObject_Union_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(143, "VSObject_Union_Shortcut.bmp");
            this._AutoListIcons.Images.SetKeyName(144, "VSObject_ValueType.bmp");
            this._AutoListIcons.Images.SetKeyName(145, "VSObject_ValueType_Friend.bmp");
            this._AutoListIcons.Images.SetKeyName(146, "VSObject_ValueType_Private.bmp");
            this._AutoListIcons.Images.SetKeyName(147, "VSObject_ValueType_Protected.bmp");
            this._AutoListIcons.Images.SetKeyName(148, "VSObject_ValueType_Sealed.bmp");
            this._AutoListIcons.Images.SetKeyName(149, "VSObject_ValueType_Shortcut.bmp");
            // 
            // mempic
            // 
            this.mempic.Dock = System.Windows.Forms.DockStyle.Left;
            this.mempic.Location = new System.Drawing.Point(301, 0);
            this.mempic.Name = "mempic";
            this.mempic.Size = new System.Drawing.Size(20, 20);
            this.mempic.TabIndex = 3;
            this.mempic.TabStop = false;
            // 
            // classessbox
            // 
            this.classessbox.DisplayMember = "Text";
            this.classessbox.Dock = System.Windows.Forms.DockStyle.Left;
            this.classessbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.classessbox.ForeColor = System.Drawing.SystemColors.WindowText;
            this.classessbox.FormattingEnabled = true;
            this.classessbox.Images = this._AutoListIcons;
            this.classessbox.ItemHeight = 14;
            this.classessbox.Location = new System.Drawing.Point(20, 0);
            this.classessbox.Name = "classessbox";
            this.classessbox.Size = new System.Drawing.Size(281, 20);
            this.classessbox.Style = DevComponents.DotNetBar.eDotNetBarStyle.OfficeMobile2014;
            this.classessbox.TabIndex = 0;
            this.classessbox.WatermarkText = "Classes";
            this.classessbox.SelectedIndexChanged += new System.EventHandler(this.classessbox_SelectedIndexChanged);
            // 
            // classpic
            // 
            this.classpic.Dock = System.Windows.Forms.DockStyle.Left;
            this.classpic.Location = new System.Drawing.Point(0, 0);
            this.classpic.Name = "classpic";
            this.classpic.Size = new System.Drawing.Size(20, 20);
            this.classpic.TabIndex = 2;
            this.classpic.TabStop = false;
            // 
            // QuickClassBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEx1);
            this.Name = "QuickClassBrowser";
            this.Size = new System.Drawing.Size(600, 20);
            this.Resize += new System.EventHandler(this.QuickClassBrowser_Resize);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mempic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.classpic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.ComboBoxEx classessbox;
        private DevComponents.DotNetBar.Controls.ComboBoxEx membersbox;
        private System.Windows.Forms.ImageList _AutoListIcons;
        private System.Windows.Forms.PictureBox mempic;
        private System.Windows.Forms.PictureBox classpic;
    }
}
