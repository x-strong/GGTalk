using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace ESFramework.Boost.Controls
{
    public partial class AutoDocker : Component
    {
        private const int DOCKING = 0;
        private const int PRE_DOCKING = 1;
        private const int OFF = 2;
        private Form dockedForm;
        private bool IsOrg;
        private Rectangle lastBoard;
        private int status = 2;
        internal AnchorStyles dockSide;       
        private Timer CheckPosTimer;

        public AutoDocker()
        {
            this.InitializeComponent();
        }
        public AutoDocker(IContainer container)
        {
            container.Add(this);
            this.InitializeComponent();
        }

        [Description("要自动靠边Dock的窗体")]
        public Form DockedForm
        {
            get
            {
                return this.dockedForm;
            }
            set
            {
                this.dockedForm = value;
                if (this.dockedForm != null)
                {
                    this.dockedForm.LocationChanged += new EventHandler(this._form_LocationChanged);
                    this.dockedForm.SizeChanged += new EventHandler(this._form_SizeChanged);
                    this.dockedForm.TopMost = true;
                }
            }
        }
        
        private void CheckPosTimer_Tick(object sender, EventArgs e)
        {
            if (base.DesignMode)
            {
                return;
            }
            if (this.dockedForm == null || !this.IsOrg)
            {
                return;
            }

            if (this.dockedForm.Bounds.Contains(Cursor.Position))
            {
                this.showOnce = false;
            }

            if (this.showOnce)
            {
                if (this.dockSide == AnchorStyles.Top)
                {
                    this.dockedForm.Location = new Point(this.dockedForm.Location.X, 0);
                }
                else if (this.dockSide == AnchorStyles.Right)
                {
                    this.dockedForm.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.dockedForm.Width, this.dockedForm.Location.Y);
                }
                else if (this.dockSide == AnchorStyles.Left)
                {
                    this.dockedForm.Location = new Point(0, this.dockedForm.Location.Y);
                }
                else if (this.dockSide == AnchorStyles.Bottom)
                {
                    this.dockedForm.Location = new Point(this.dockedForm.Location.X, Screen.PrimaryScreen.Bounds.Height - this.dockedForm.Height);
                }
                else
                {
                }
                
                this.dockedForm.Size = new Size(this.lastBoard.Width, this.lastBoard.Height);
                return;
            }

            if (this.dockedForm.Bounds.Contains(Cursor.Position))
            {              
                AnchorStyles anchorStyles = this.dockSide;
                if (anchorStyles != AnchorStyles.Top)
                {
                    if (anchorStyles != AnchorStyles.Left)
                    {
                        if (anchorStyles != AnchorStyles.Right)
                        {
                            return;
                        }
                        if (this.status == 0)
                        {
                            this.dockedForm.Location = new Point(Screen.PrimaryScreen.Bounds.Width - this.dockedForm.Width, this.dockedForm.Location.Y);
                            return;
                        }
                    }
                    else
                    {
                        if (this.status == 0)
                        {
                            this.dockedForm.Location = new Point(0, this.dockedForm.Location.Y);
                            return;
                        }
                    }
                }
                else
                {
                    if (this.status == 0)
                    {
                        this.dockedForm.Location = new Point(this.dockedForm.Location.X, 0);
                        return;
                    }
                }
            }
            else
            {
                AnchorStyles anchorStyles2 = this.dockSide;                
                switch (anchorStyles2)
                {
                    case AnchorStyles.None:
                        {
                            if (this.IsOrg && this.status == 2 && (this.dockedForm.Bounds.Width != this.lastBoard.Width || this.dockedForm.Bounds.Height != this.lastBoard.Height))
                            {
                                this.dockedForm.Size = new Size(this.lastBoard.Width, this.lastBoard.Height);
                            }
                            break;
                        }
                    case AnchorStyles.Top:
                        {
                            this.dockedForm.Location = new Point(this.dockedForm.Location.X, (this.dockedForm.Height - 4) * -1);
                            return;
                        }
                    case AnchorStyles.Bottom:
                    case AnchorStyles.Top | AnchorStyles.Bottom:
                        {
                            break;
                        }
                    case AnchorStyles.Left:
                        {
                            //this.dockedForm.Size = new Size(this.dockedForm.Width, Screen.PrimaryScreen.WorkingArea.Height);
                            this.dockedForm.Location = new Point(-1 * (this.dockedForm.Width - 4), this.dockedForm.Location.Y);
                            return;
                        }
                    default:
                        {
                            if (anchorStyles2 != AnchorStyles.Right)
                            {
                                return;
                            }
                            //this.dockedForm.Size = new Size(this.dockedForm.Width, Screen.PrimaryScreen.WorkingArea.Height);
                            this.dockedForm.Location = new Point(Screen.PrimaryScreen.Bounds.Width - 4, this.dockedForm.Location.Y);
                            return;
                        }
                }
            }
        }
        private void GetDockSide()
        {
            if (this.dockedForm.Top <= 0)
            {
                this.dockSide = AnchorStyles.Top;
                if (this.dockedForm.Bounds.Contains(Cursor.Position))
                {
                    this.status = 1;
                    return;
                }
                this.status = 0;
                return;
            }
            else
            {
                if (this.dockedForm.Left <= 0)
                {
                    this.dockSide = AnchorStyles.Left;
                    if (this.dockedForm.Bounds.Contains(Cursor.Position))
                    {
                        this.status = 1;
                        return;
                    }
                    this.status = 0;
                    return;
                }
                else
                {
                    if (this.dockedForm.Left < Screen.PrimaryScreen.Bounds.Width - this.dockedForm.Width)
                    {
                        this.dockSide = AnchorStyles.None;
                        this.status = 2;
                        return;
                    }
                    this.dockSide = AnchorStyles.Right;
                    if (this.dockedForm.Bounds.Contains(Cursor.Position))
                    {
                        this.status = 1;
                        return;
                    }
                    this.status = 0;
                    return;
                }
            }
        }
        private void _form_LocationChanged(object sender, EventArgs e)
        {
            this.GetDockSide();
            if (!this.IsOrg)
            {
                this.lastBoard = this.dockedForm.Bounds;
                this.IsOrg = true;
            }
        }
        private void _form_SizeChanged(object sender, EventArgs e)
        {
            if (this.IsOrg && this.status == 2)
            {
                this.lastBoard = this.dockedForm.Bounds;
            }
        }

        private bool showOnce = false;
        public void ShowOnce()
        {
            this.showOnce = true;
            this.status = 2;
        }
    }
}
