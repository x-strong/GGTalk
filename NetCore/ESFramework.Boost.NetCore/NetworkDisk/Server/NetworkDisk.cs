using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESPlus.Application.FileTransfering.Server;
using ESBasic.Helpers;
using ESPlus.FileTransceiver;
using ESPlus.Application.FileTransfering;

namespace ESFramework.Boost.NetworkDisk.Server
{
    /// <summary>
    /// 网络硬盘，为用户提供在线的网络硬盘服务。可通过INetworkDiskPathManager来将不同用户的目录分散到不同的文件服务器上。
    /// </summary>
    public class NetworkDisk 
    {  
        private INDiskPathManager networkDiskPathManager ;
        private IFileController fileController;
        public NetworkDisk(INDiskPathManager mgr , IFileController controller)
        {
            this.networkDiskPathManager = mgr;
            this.fileController = controller;
            this.fileController.FileRequestReceived += new CbFileRequestReceived(fileController_FileRequestReceived);
        }

        void fileController_FileRequestReceived(string fileID, string senderID, ClientType senderType, string fileName, ulong fileLength, ResumedProjectItem resumedFileItem, string comment)
        {
            Console.WriteLine("触发了文件请求,comment:"+ comment);
            NDiskParameters paras = Comment4NDisk.Parse(comment);

            if (paras == null)
            {
                return;
            }

            if (resumedFileItem != null)
            {
                Console.WriteLine("resumedFileItem.LocalSavePath:" + resumedFileItem.LocalSavePath);
                this.fileController.BeginReceiveFile(fileID, resumedFileItem.LocalSavePath); //续传
                return;
            }

            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(senderID, paras.NetDiskID);
            Console.WriteLine("rootPath + paras.DirectoryPath:" + Comment4NDisk.GetRealPath( rootPath + paras.DirectoryPath));
            this.fileController.BeginReceiveFile(fileID, Comment4NDisk.GetRealPath(rootPath + paras.DirectoryPath));
        }           

        #region Methods
        #region GetNetworkDiskRootPath
        public string GetNetworkDiskRootPath(string clientUserID, string netDiskID)
        {
            return this.networkDiskPathManager.GetNetworkDiskRootPath(clientUserID, netDiskID);
        }
        #endregion

        #region GetNetworkDisk
        public SharedDirectory GetNetworkDisk(string clientUserID, string netDiskID, string dirPath)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(clientUserID, netDiskID);
            if (rootPath == null)
            {
                return null;
            }
            string iniDirName = this.networkDiskPathManager.GetNetworkDiskIniDirName(clientUserID, netDiskID);
            string diskRpotDir = Comment4NDisk.GetRealPath(rootPath + "/" + iniDirName + "/");
            if (!Directory.Exists(diskRpotDir))
            {
                Directory.CreateDirectory(diskRpotDir);
            }

            if (dirPath == null)
            {
                SharedDirectory dir = new SharedDirectory();
                DiskDrive disk = new DiskDrive();
                disk.Name = iniDirName;
                disk.TotalSize = this.networkDiskPathManager.GetNetworkDiskTotalSize(clientUserID, netDiskID);
                disk.AvailableFreeSpace = disk.TotalSize - this.networkDiskPathManager.GetNetworkDiskSizeUsed(clientUserID, netDiskID);

                dir.DriveList.Add(disk);
                return dir;
            }
            return SharedDirectory.GetSharedDirectory(Comment4NDisk.GetRealPath(rootPath + dirPath));
        }
        #endregion

        #region GetNetworkDiskState
        public NetworkDiskState GetNetworkDiskState(string clientUserID, string netDiskID)
        {
            ulong total = this.networkDiskPathManager.GetNetworkDiskTotalSize(clientUserID, netDiskID);
            ulong used = this.networkDiskPathManager.GetNetworkDiskSizeUsed(clientUserID, netDiskID);

            return new NetworkDiskState(total, used);
        }
        #endregion

        #region CreateDirectory
        public void CreateDirectory(string clientUserID, string netDiskID, string parentDirectoryPath, string newDirName)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(clientUserID, netDiskID); 
            Directory.CreateDirectory(Comment4NDisk.GetRealPath( rootPath + parentDirectoryPath + newDirName));
            Console.WriteLine("创建文件夹，路径" + Comment4NDisk.GetRealPath(rootPath + parentDirectoryPath + newDirName));
        }
        #endregion

        #region DeleteFileOrDirectory
        public void DeleteFileOrDirectory(string clientUserID, string netDiskID, string sourceParentDirectoryPath, IList<string> filesBeDeleted, IList<string> directoriesBeDeleted)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(clientUserID, netDiskID);

            if (filesBeDeleted != null)
            {
                foreach (string fileName in filesBeDeleted)
                {
                    string filePath = Comment4NDisk.GetRealPath(rootPath + sourceParentDirectoryPath + fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if (directoriesBeDeleted != null)
            {
                foreach (string dirName in directoriesBeDeleted)
                {
                    string dirPath = Comment4NDisk.GetRealPath(rootPath + sourceParentDirectoryPath + dirName + "/");
                    if (Directory.Exists(dirPath))
                    {
                        FileHelper.DeleteDirectory(dirPath);
                    }
                }
            }
        }
        #endregion

        #region Rename
        public void Rename(string clientUserID, string netDiskID, string parentDirectoryPath, bool isFile, string oldName, string newName)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(clientUserID, netDiskID);
            string sourceFileName = Comment4NDisk.GetRealPath(rootPath + parentDirectoryPath + oldName);
            string destFileName = Comment4NDisk.GetRealPath(rootPath + parentDirectoryPath + newName);
            if (isFile)
            {
                File.Move(sourceFileName, destFileName);
            }
            else
            {
                Directory.Move(sourceFileName, destFileName);
            }
        }
        #endregion

        #region Move
        public void Move(string clientUserID, string netDiskID, string oldParentDirectoryPath, IEnumerable<string> filesBeMoved, IEnumerable<string> directoriesBeMoved, string newParentDirectoryPath)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(clientUserID, netDiskID);
            FileHelper.Move(Comment4NDisk.GetRealPath(rootPath + oldParentDirectoryPath), filesBeMoved, directoriesBeMoved, Comment4NDisk.GetRealPath(rootPath + newParentDirectoryPath));
        }
        #endregion

        #region Copy
        public void Copy(string clientUserID, string netDiskID, string sourceParentDirectoryPath, IEnumerable<string> filesBeCopyed, IEnumerable<string> directoriesCopyed, string destParentDirectoryPath)
        {
            string rootPath = this.networkDiskPathManager.GetNetworkDiskRootPath(clientUserID, netDiskID);
            FileHelper.Copy(Comment4NDisk.GetRealPath(rootPath + sourceParentDirectoryPath), filesBeCopyed, directoriesCopyed, Comment4NDisk.GetRealPath(rootPath + destParentDirectoryPath));
        }
        #endregion
        #endregion



    }
}
