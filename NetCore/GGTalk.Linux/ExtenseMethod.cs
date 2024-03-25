using CPF;
using CPF.Controls;
using CPF.Drawing;
using GGTalk.Linux.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GGTalk.Linux
{
    internal static class ExtenseMethod
    {
        #region Window
        /// <summary>
        /// 设置窗体在owner窗体的正中， 一般用于在owner窗体上 弹出新的窗体
        /// </summary>
        /// <param name="window"></param>
        /// <param name="owner"></param>
        public static void SetCenterInParent(this Window window, Window owner)
        {
            if (owner == null || window == null)
            {
                return;
            }
            PixelPoint newPoint = new PixelPoint(owner.Position.X + (int)(owner.Width.Value - window.Width.Value) / 2, owner.Position.Y + (int)(owner.Height.Value - window.Height.Value) / 2);
            window.Position = newPoint;
        }

        /// <summary>
        /// 显示在最前面
        /// </summary>
        /// <param name="window"></param>
        public static void Show_Topmost(this Window window)
        {
            window.TopMost = true;
            window.Show();
            window.TopMost = false;
        }

        public static Task<object> ShowDialog_Topmost(this Window window, Window owner)
        {
            window.TopMost = true;
            Task<object> task = window.ShowDialog(owner);
            window.SetCenterInParent(owner);
            window.TopMost = false;
            return task;
        }
        #endregion

        #region TreeView
        public static void ExpandFirstNode(this TreeView treeView)
        {
            IEnumerable<TreeViewItem> items = treeView.AllItems();
            List<TreeViewItem> children = new List<TreeViewItem>(items);
            if (children == null || children.Count == 0)
            {
                return;
            }
            TreeViewItem item = children[0];
            if (item != null)
            {
                item.IsExpanded = true;
            }
        }

        public static void ExpandAllNode(this TreeView treeView)
        {
            IEnumerable<TreeViewItem> items = treeView.AllItems();
            List<TreeViewItem> children = new List<TreeViewItem>(items);
            for (int i = 0; i < children.Count; i++)
            {
                TreeViewItem item = children[i];
                if (item != null)
                {
                    item.ExpandSubtree();
                }
            }
        }
        #endregion

        #region Image
        /// <summary>
        /// 保存为位图
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filePath"></param>
        public static void Save(this Image image, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                byte[] data = BitmapHelper.Bitmap2Byte(image);
                fileStream.Write(data, 0, data.Length);
                fileStream.Flush();
                fileStream.Close();
            }
        }
        #endregion

        #region TextBox
        /// <summary>
        /// 聚焦到最后位置
        /// </summary>
        /// <param name="textBox"></param>
        public static void FocusLastIndex(this TextBox textBox)
        {
            textBox.CaretIndex.Clear();
            textBox.CaretIndex.Add((uint)textBox.Document.Children.Count);

            Task.Factory.StartNew(() => { textBox.Focus(); });
        }

        #endregion


        /// <summary>
        /// 子控件集合排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controls"></param>
        /// <param name="comparison"></param>
        public static void Sort<T>(this ObservableObject<T> controls, Comparison<T> comparison) where T:Control
        {
            foreach (Control item in controls)
            {
                if (!(item is T))
                {
                    throw new Exception(string.Format("集合中存在不属于{0}控件类型",typeof(T)));
                }
            }
            QuickSort<T>(controls, 0, controls.Count - 1, comparison);
        }

        /// <summary>
        /// 快速排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_array"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="comparison"></param>
        public static void QuickSort<T>(IList<T> _array, int startIndex, int endIndex, Comparison<T> comparison)
        {
            if (startIndex < endIndex)
            {
                T tempT = _array[startIndex];//指定一位基准数
                int i = startIndex;
                int j = endIndex;
                while (i < j)
                {
                    while (i < j)
                    {
                        if (comparison(_array[j], tempT) <= 0)
                        {
                            _array[i] = _array[j];
                            i++;
                            break;
                        }
                        else
                        {
                            j--;
                        }
                    }
                    while (i < j)
                    {
                        if (comparison(_array[i], tempT) > 0)
                        {
                            _array[j] = _array[i];
                            j--;
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                }
                _array[i] = tempT;
                QuickSort(_array, startIndex, i - 1, comparison);
                QuickSort(_array, i + 1, endIndex, comparison);
            }
        }

        //public static bool Contains<(T1,T2)>(Collection<(T1, T2)> list, Predicate<(T1, T2)> match)
        //{
        //    T t =  list.Find(match);
        //    if (t == null)
        //    {
        //        return false;
        //    }
        //    return true;
        //}


    }
}
