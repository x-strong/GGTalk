﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCWin;
using CCWin.Win32;
using CCWin.Win32.Const;
using System.Diagnostics;
using System.Configuration;
using ESPlus.Rapid;
using Microsoft.Win32;
using GGTalk.Forms;

namespace GGTalk
{
    /// <summary>
    /// 支持换肤的基础窗体。
    /// </summary>
    public partial class BaseForm : CCSkinMain
    {
        public BaseForm()
        {
            //Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            InitializeComponent();            
        }

        #region UseCustomIcon
        private bool useCustomIcon = false;
        /// <summary>
        /// 是否使用自己的图标。
        /// </summary>
        public bool UseCustomIcon
        {
            get { return useCustomIcon; }
            set { useCustomIcon = value; }
        } 
        #endregion

        #region UseCustomBackImage
        private bool useCustomBackImage = true;
        /// <summary>
        /// 是否使用自己的背景图片
        /// </summary>
        public bool UseCustomBackImage
        {
            get { return useCustomBackImage; }
            set { useCustomBackImage = value; }
        } 
        #endregion

        private void Form_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (!this.useCustomIcon)
                {
                    this.Icon = GlobalResourceManager.Icon64;
                }

                if (!this.useCustomBackImage)
                {
                    //this.Back = GlobalResourceManager.MainBackImage;
                }                
            }
        }

       

        
         
    }
}
