
namespace DispelTools.Components.CustomPropertyGridControl
{
    partial class CustomPropertyGrid
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
            ClearFields();
            base.Dispose(disposing);
        }

        /// <summary>
        /// An application sends the WM_SETREDRAW message to a window to allow changes in that 
        /// window to be redrawn or to prevent changes in that window from being redrawn.
        /// </summary>
        private const int WM_SETREDRAW = 11;

        /// <summary>
        /// Suspends painting for the target control. Do NOT forget to call EndControlUpdate!!!
        /// </summary>
        /// <param name="control">visual control</param>
        public void BeginControlUpdate()
        {
            System.Windows.Forms.Message msgSuspendUpdate = System.Windows.Forms.Message.Create(this.Handle, WM_SETREDRAW, System.IntPtr.Zero,
                  System.IntPtr.Zero);

            System.Windows.Forms.NativeWindow window = System.Windows.Forms.NativeWindow.FromHandle(this.Handle);
            window.DefWndProc(ref msgSuspendUpdate);
        }

        /// <summary>
        /// Resumes painting for the target control. Intended to be called following a call to BeginControlUpdate()
        /// </summary>
        /// <param name="control">visual control</param>
        public void EndControlUpdate()
        {
            // Create a C "true" boolean as an IntPtr
            System.IntPtr wparam = new System.IntPtr(1);
            System.Windows.Forms.Message msgResumeUpdate = System.Windows.Forms.Message.Create(this.Handle, WM_SETREDRAW, wparam,
                  System.IntPtr.Zero);

            System.Windows.Forms.NativeWindow window = System.Windows.Forms.NativeWindow.FromHandle(this.Handle);
            window.DefWndProc(ref msgResumeUpdate);
            this.Invalidate();
            this.Refresh();
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // CustomPropertyGrid
            // 
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(20, 20);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Size = new System.Drawing.Size(217, 257);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ToolTip toolTip;
    }
}
