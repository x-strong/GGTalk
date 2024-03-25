using System;
using ESPlus.Application.FileTransfering.Passive;

namespace ESFramework.Boost.NetworkDisk.Passive
{
    /// <summary>
    /// 用于访问网盘或远程磁盘的浏览器控件接口。
    /// </summary>
    public interface INDiskBrowser
    {
        /// <summary>
        /// 网盘的标志。（对于远程磁盘而言，即OwnerID为某个用户的ID时，该属性无效）。
        /// 如果是群组共享的文件夹，则可以将其设置为对应的群组的ID。
        /// </summary>
        string NetDiskID { get; set; }

        /// <summary>
        /// 是否允许上传文件夹
        /// </summary>
        bool AllowUploadFolder { get; set; }

        /// <summary>
        /// 锁定为只访问根目录（不允许操作子文件夹）。
        /// </summary>
        bool LockRootDirectory { get; set; }

        /// <summary>
        /// 当前所在目录的路径。
        /// </summary>
        string CurrentDirectoryPath { get; }

        /// <summary>
        /// OwnerIsOnline 远程机器是否在线。
        /// </summary>
        bool Connected { get; set; }

        /// <summary>
        /// 是否有文件正在传送中。
        /// </summary>
        bool IsFileTransfering { get; }

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="_ownerID">如果为null，表示访问服务器上的某个目录（网盘）。否则，表示访问在线的目标用户的硬盘。</param>       
        void Initialize(string _ownerID, IFileOutter _fileOutter, INDiskOutter _fileDirectoryOutter, string curUserID);

        /// <summary>
        /// 取消所有正在传送的项目。通常是在窗口被关闭时调用。
        /// </summary>
        void CancelAllTransfering();
        
    }
}
