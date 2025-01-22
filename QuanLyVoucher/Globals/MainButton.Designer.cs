namespace QuanLyVoucher.Globals
{
    partial class MainButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainButton));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.btnBrowse = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnAdd = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnEdit = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnSave = new DevExpress.XtraBars.BarLargeButtonItem();
            this.btnDelete = new DevExpress.XtraBars.BarLargeButtonItem();
            this.cboSearch = new DevExpress.XtraBars.BarEditItem();
            this.cboSearchContainer = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
            this.cboSearchContainerControl = new DevExpress.XtraEditors.PopupContainerControl();
            this.barAndDockingController = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchContainer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchContainerControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar2});
            this.barManager.Controller = this.barAndDockingController;
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnBrowse,
            this.btnAdd,
            this.btnEdit,
            this.btnSave,
            this.btnDelete,
            this.cboSearch});
            this.barManager.MainMenu = this.bar2;
            this.barManager.MaxItemId = 11;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboSearchContainer});
            // 
            // bar2
            // 
            this.bar2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bar2.Appearance.Options.UseBackColor = true;
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnBrowse),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAdd, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnEdit, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnDelete, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.cboSearch)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Caption = "Làm mới (F1)";
            this.btnBrowse.Id = 0;
            this.btnBrowse.LargeGlyph = global::QuanLyVoucher.Properties.Resources.Browse;
            this.btnBrowse.Name = "btnBrowse";
            // 
            // btnAdd
            // 
            this.btnAdd.Caption = "Thêm (F2)";
            this.btnAdd.Id = 1;
            this.btnAdd.LargeGlyph = global::QuanLyVoucher.Properties.Resources.Add_button;
            this.btnAdd.Name = "btnAdd";
            // 
            // btnEdit
            // 
            this.btnEdit.Caption = "Sửa (F3)";
            this.btnEdit.Id = 2;
            this.btnEdit.LargeGlyph = global::QuanLyVoucher.Properties.Resources.Edit;
            this.btnEdit.Name = "btnEdit";
            // 
            // btnSave
            // 
            this.btnSave.Caption = "Lưu (F4)";
            this.btnSave.Id = 3;
            this.btnSave.LargeGlyph = global::QuanLyVoucher.Properties.Resources.stock_save;
            this.btnSave.Name = "btnSave";
            // 
            // btnDelete
            // 
            this.btnDelete.Caption = "Xóa (F5)";
            this.btnDelete.Id = 4;
            this.btnDelete.LargeGlyph = global::QuanLyVoucher.Properties.Resources.Stop;
            this.btnDelete.Name = "btnDelete";
            // 
            // cboSearch
            // 
            this.cboSearch.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.cboSearch.Caption = "Tìm kiếm";
            this.cboSearch.Edit = this.cboSearchContainer;
            this.cboSearch.Id = 10;
            this.cboSearch.Name = "cboSearch";
            this.cboSearch.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.cboSearch.Width = 250;
            // 
            // cboSearchContainer
            // 
            this.cboSearchContainer.AutoHeight = false;
            this.cboSearchContainer.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("cboSearchContainer.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.cboSearchContainer.Name = "cboSearchContainer";
            this.cboSearchContainer.PopupControl = this.cboSearchContainerControl;
            this.cboSearchContainer.PopupSizeable = false;
            this.cboSearchContainer.ShowPopupCloseButton = false;
            // 
            // cboSearchContainerControl
            // 
            this.cboSearchContainerControl.Location = new System.Drawing.Point(315, 51);
            this.cboSearchContainerControl.Name = "cboSearchContainerControl";
            this.cboSearchContainerControl.Size = new System.Drawing.Size(406, 100);
            this.cboSearchContainerControl.TabIndex = 4;
            // 
            // barAndDockingController
            // 
            this.barAndDockingController.LookAndFeel.UseDefaultLookAndFeel = false;
            this.barAndDockingController.LookAndFeel.UseWindowsXPTheme = true;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(754, 50);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 154);
            this.barDockControlBottom.Size = new System.Drawing.Size(754, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 50);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 104);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(754, 50);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 104);
            // 
            // MainButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboSearchContainerControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "MainButton";
            this.Size = new System.Drawing.Size(754, 154);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchContainer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboSearchContainerControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        public DevExpress.XtraBars.BarLargeButtonItem btnBrowse;
        public DevExpress.XtraBars.BarLargeButtonItem btnAdd;
        public DevExpress.XtraBars.BarLargeButtonItem btnEdit;
        public DevExpress.XtraBars.BarLargeButtonItem btnSave;
        public DevExpress.XtraBars.BarLargeButtonItem btnDelete;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController;
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit cboSearchContainer;
        private DevExpress.XtraEditors.PopupContainerControl cboSearchContainerControl;
        private DevExpress.XtraBars.BarEditItem cboSearch;
    }
}
