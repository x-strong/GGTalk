using ESFramework;
using Oracle.ManagedDataAccess.Client;
using OrayTalk;
using OrayTalk.Server.Oracle;
using System;
using System.Collections.Generic;
using System.Data;
using TalkBase;
using TalkBase.Server;
using TalkBase.Server.Application;

namespace OrayTalk.Server
{
    public class OraclePersister : IDBPersister<OrayUser, OrayGroup, OrayOrganization>
    {

        private OfflineMemoryCache offlineMemoryCache = new OfflineMemoryCache();


        public void ClearAllChatRecord()
        {
            OracleHelper.ExecuteNonQuery("delete from ChatMessageRecord t");
        }

        public void DeleteChatRecord(string myID, string friendID)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":myID", myID));
            parameters.Add(new OracleParameter(":friendID", friendID));
            parameters.Add(new OracleParameter(":friendID1", friendID));
            parameters.Add(new OracleParameter(":myID1", myID));
            OracleHelper.ExecuteNonQuery(" Delete from ChatMessageRecord where (speakerid=:myID and audienceid=:friendID) or (speakerid=:friendID1 and audienceid=:myID1) and isgroupchat =0 ", parameters.ToArray());
        }

        public void DeleteGroup(string groupID)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":groupID", groupID));
            OracleHelper.ExecuteNonQuery("Delete from ORAYGROUP where groupID=:groupID", parameters.ToArray());
        }

        public void DeleteGroupChatRecord(string groupID)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":audienceid", groupID));
            OracleHelper.ExecuteNonQuery("Delete from ChatMessageRecord where audienceid =:audienceid", parameters.ToArray());
        }

        public void DeleteOrgItem(string orgID, int version, bool itemDeletedInDB)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":orgID", orgID));
            if (!itemDeletedInDB)
            {
                OracleHelper.ExecuteNonQuery("Delete from OrayUser where orgID =:orgID ", parameters.ToArray());
            }

            UpdateOrgVersion(version);
        }

        private void UpdateOrgVersion(int version)
        {
            OrayConfiguration config = GetOrgConfiguration(GlobalConsts.OrgVersionFieldName);
            config.OrayValue = version.ToString();
            UpdateOrayConfiguration(config);
        }

        private void UpdateOrayConfiguration(OrayConfiguration config)
        {
            String sql = "update OrayConfiguration set OrayValue=:orayValue where  orayKey=:orayKey";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":orayKey", config.OrayKey));
            parameters.Add(new OracleParameter(":orayValue", config.OrayValue));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void DeleteUser(string userID)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":userID", userID));
            OracleHelper.ExecuteNonQuery("Delete from OrayUser where userID =:userID", parameters.ToArray());
        }

        public List<OrayGroup> GetAllGroup()
        {
            List<OrayGroup> result = new List<OrayGroup>();
            DataTable dt = OracleHelper.ExecuteDataTable("select * from OrayGroup");
            if (dt != null && dt.Rows.Count > 0)
            {
                result = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<OrayGroup>(dt) as List<OrayGroup>;
            }
            if (result != null)
            {
                foreach (OrayGroup item in result)
                {
                    OracleHelper.SetEmptyProperty4Null<OrayGroup>(item);
                }
            }
            return result;
        }

        public List<OrayGroup> SearchGroup(string idOrName)
        {
            List<OrayGroup> result = new List<OrayGroup>();
            string sql = string.Format("select * from OrayGroup where {0}=:idOrName or {1}=:idOrName",OrayGroup._GroupID,OrayGroup._Name);
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":idOrName", idOrName));
            DataTable dt = OracleHelper.ExecuteDataTable(sql, parameters.ToArray());
            if (dt != null && dt.Rows.Count > 0)
            {
                result = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<OrayGroup>(dt) as List<OrayGroup>;
            }
            if (result != null)
            {
                foreach (OrayGroup item in result)
                {
                    OracleHelper.SetEmptyProperty4Null<OrayGroup>(item);
                }
            }
            return result;
        }

        public List<OrayUser> GetAllUser()
        {
            List<OrayUser> result = new List<OrayUser>();
            DataTable dt = OracleHelper.ExecuteDataTable("select * from OrayUser");
            if (dt != null && dt.Rows.Count > 0)
            {
                result = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<OrayUser>(dt) as List<OrayUser>;
            }
            if (result != null)
            {
                foreach (OrayUser item in result)
                {
                    OracleHelper.SetEmptyProperty4Null<OrayUser>(item);
                }
            }
            return result;
        }


        public OrayGroup GetGroup(string groupID)
        {
            OrayGroup result = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":groupid", groupID));
            DataTable dt = OracleHelper.ExecuteDataTable("select * from OrayGroup where groupid =:groupid", parameters.ToArray());
            DataRow row = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
                result = DataRabbit.DBAccessing.DataHelper.ConvertRowToObject<OrayGroup>(row);
            }
            OracleHelper.SetEmptyProperty4Null<OrayGroup>(result);
            return result;
        }

        public OrganizationManager<OrayOrganization> GetOrganization()
        {
            List<OrayOrganization> list = new List<OrayOrganization>();
            DataTable dt = OracleHelper.ExecuteDataTable("select * from OrayOrganization t");               
            if (dt != null && dt.Rows.Count > 0)
            {
                list = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<OrayOrganization>(dt) as List<OrayOrganization>;
            }

            OrayConfiguration config = GetOrgConfiguration(GlobalConsts.OrgVersionFieldName);
            return new OrganizationManager<OrayOrganization>(list, int.Parse(config.OrayValue));
        }

        private OrayConfiguration GetOrgConfiguration(String orayKey)
        {
            OrayConfiguration config = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":orayKey", orayKey));
            DataTable dtConfig = OracleHelper.ExecuteDataTable(string.Format("select * from OrayConfiguration t where {0}=:orayKey",OrayConfiguration._OrayKey), parameters.ToArray());
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                config = DataRabbit.DBAccessing.DataHelper.ConvertRowToObject<OrayConfiguration>(dtConfig.Rows[0]);

            }
            OracleHelper.SetEmptyProperty4Null<OrayConfiguration>(config);
            return config;
        }

        public OrayUser GetUser(string userID)
        {
            OrayUser user = null;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":userID", userID));
            DataTable dt = OracleHelper.ExecuteDataTable("select * from OrayUser where userID =:userID ", parameters.ToArray());
                
            DataRow row = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
                user = DataRabbit.DBAccessing.DataHelper.ConvertRowToObject<OrayUser>(row);
            }
            OracleHelper.SetEmptyProperty4Null<OrayUser>(user);
            return user;
        }


        public string GetUserPassword(string userID)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":userID", userID));
            DataTable dt = OracleHelper.ExecuteDataTable("select * from OrayUser where userID =:userID ", parameters.ToArray());
            DataRow row = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                row = dt.Rows[0];
                OrayUser user = DataRabbit.DBAccessing.DataHelper.ConvertRowToObject<OrayUser>(row);
                return user.PasswordMD5;
            }
            return null;
        }

        public void InsertChatMessageRecord(ChatMessageRecord record)
        {
            String sql = "INSERT INTO  CHATMESSAGERECORD (SPEAKERID,AUDIENCEID,ISGROUPCHAT,CONTENT,OCCURETIME) VALUES(:SpeakerID,:AudienceID, :IsGroupChat, :Content, :OccureTime)";
            // byte[] bigTextByte = System.Text.Encoding.UTF8.GetBytes(bigText);
            OracleParameter SpeakerIDPara = new OracleParameter(":SpeakerID", record.SpeakerID);
            OracleParameter AudienceIDPara = new OracleParameter(":AudienceID", record.AudienceID);
            OracleParameter IsGroupChatPara = new OracleParameter(":IsGroupChat", record.IsGroupChat ? 1 : 0);
            OracleParameter ContentPara = new OracleParameter(":Content", OracleDbType.Blob, record.Content.Length);
            ContentPara.Value = record.Content;
            OracleParameter OccureTimePara = new OracleParameter(":OccureTime", record.OccureTime);
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(SpeakerIDPara);
            parameters.Add(AudienceIDPara);
            parameters.Add(IsGroupChatPara);
            parameters.Add(ContentPara);
            parameters.Add(OccureTimePara);
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void InsertGroup(OrayGroup bean)
        {
            String sql = "INSERT INTO ORAYGROUP VALUES(:groupid ,:name,:creatorid, :ANNOUNCE, :members, :createtime,:version)";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":groupid", bean.GroupID));
            parameters.Add(new OracleParameter(":name", OracleDbType.NVarchar2, bean.Name, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":creatorid", bean.CreatorID));
            parameters.Add(new OracleParameter(":ANNOUNCE", OracleDbType.NVarchar2, bean.Announce, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":members", bean.Members));
            parameters.Add(new OracleParameter(":createtime", bean.CreateTime));
            parameters.Add(new OracleParameter(":version", bean.Version));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void InsertUser(OrayUser bean)
        {
            String sql = "INSERT INTO ORAYUSER VALUES(:USERID ,:PASSWORDMD5,:NAME, :FRIENDS, :COMMENTNAMES, :ORGID,:SIGNATURE,:HEADIMAGEINDEX,:HEADIMAGEDATA,:GROUPS,:CREATETIME,:VERSION)";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":USERID", bean.UserID));
            parameters.Add(new OracleParameter(":PASSWORDMD5", bean.PasswordMD5));
            parameters.Add(new OracleParameter(":NAME", OracleDbType.NVarchar2, bean.Name, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":FRIENDS", OracleDbType.NVarchar2, bean.Friends, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":COMMENTNAMES", OracleDbType.NVarchar2, bean.CommentNames, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":ORGID", bean.OrgID));
            parameters.Add(new OracleParameter(":SIGNATURE", OracleDbType.NVarchar2, bean.Signature, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":HEADIMAGEINDEX", bean.HeadImageIndex));
            parameters.Add(new OracleParameter(":HEADIMAGEDATA", bean.HeadImageData));
            parameters.Add(new OracleParameter(":GROUPS", bean.Groups));
            parameters.Add(new OracleParameter(":CREATETIME", bean.CreateTime));
            parameters.Add(new OracleParameter(":VERSION", bean.Version));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public List<OfflineMessage> PickupOfflineMessage(string destUserID)
        {
            return this.offlineMemoryCache.PickupOfflineMessage(destUserID);
        }

        public void StoreOfflineMessage(OfflineMessage msg)
        {
            this.offlineMemoryCache.StoreOfflineMessage(msg);
        }

        public void StoreOfflineFileItem(OfflineFileItem item)
        {
            this.offlineMemoryCache.StoreOfflineFileItem(item);
        }

        public List<OfflineFileItem> PickupOfflineFileItem(string accepterID)
        {
            return this.offlineMemoryCache.PickupOfflineFileItem(accepterID);
        }

        public List<OfflineFileItem> PickupOfflineFileItem4Assistant(string accepterID, ClientType type)
        {
            return this.offlineMemoryCache.PickupOfflineFileItem4Assistant(accepterID, type);
        }

        public void UpdateGroupInfo(OrayGroup bean)
        {
            String sql = "update ORAYGROUP set name=:name,creatorid=:creatorid, ANNOUNCE=:annouce, members=:members, createtime=:createtime,version=:version where groupid =:groupid";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":name", OracleDbType.NVarchar2, bean.Name, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":creatorid", bean.CreatorID));
            parameters.Add(new OracleParameter(":ANNOUNCE", OracleDbType.NVarchar2, bean.Announce, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":members", bean.Members));
            parameters.Add(new OracleParameter(":createtime", bean.CreateTime));
            parameters.Add(new OracleParameter(":version", bean.Version));
            parameters.Add(new OracleParameter(":groupid", bean.GroupID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateGroupMembers(OrayGroup bean)
        {
            String sql = "update ORAYGROUP set members=:members where groupid =:groupid";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":members", bean.Members));
            parameters.Add(new OracleParameter(":groupid", bean.GroupID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void AddOrgItem(OrayOrganization bean, int version, bool itemExistInDB)
        {
            if (!itemExistInDB)
            {
                String sql = "INSERT INTO ORAYORGANIZATION values(:OrgID,:Name,:OrgFullPath,:ParentID,:OrgLevel) ";
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter(":OrgID", bean.OrgID));
                parameters.Add(new OracleParameter(":Name", OracleDbType.NVarchar2, bean.Name, ParameterDirection.InputOutput));
                parameters.Add(new OracleParameter(":OrgFullPath", OracleDbType.NVarchar2, bean.OrgFullPath, ParameterDirection.InputOutput));
                parameters.Add(new OracleParameter(":ParentID", bean.ParentID));
                parameters.Add(new OracleParameter(":OrgLevel", bean.OrgLevel));
                OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
            }

            UpdateOrgVersion(version);
        }

        public void UpdateOrgItem(OrayOrganization bean, int version, bool itemRevisedInDB)
        {
            if (!itemRevisedInDB)
            {
                String sql = "update OrayOrganization set Name=:Name,OrgFullPath=:OrgFullPath ,ParentID=:ParentID,OrgLevel =:OrgLevel where OrgID =:OrgID";
                List<OracleParameter> parameters = new List<OracleParameter>();
                parameters.Add(new OracleParameter(":Name", OracleDbType.NVarchar2, bean.Name, ParameterDirection.InputOutput));
                parameters.Add(new OracleParameter(":OrgFullPath", OracleDbType.NVarchar2, bean.OrgFullPath, ParameterDirection.InputOutput));
                parameters.Add(new OracleParameter(":ParentID", bean.ParentID));
                parameters.Add(new OracleParameter(":OrgLevel", bean.OrgLevel));
                parameters.Add(new OracleParameter(":OrgID", bean.OrgID));
                OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
            }

            UpdateOrgVersion(version);
        }

        public void UpdateUserBusinessInfo(OrayUser user, Dictionary<string, byte[]> businessInfo, int version)
        {
            
        }

        public void UpdateUserCommentNames(OrayUser bean)
        {
            String sql = "update ORAYUSER set COMMENTNAMES=:COMMENTNAMES where USERID=:USERID";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":COMMENTNAMES", bean.CommentNames));
            parameters.Add(new OracleParameter(":USERID", bean.UserID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateUserFriends(OrayUser bean)
        {
            String sql = "update ORAYUSER set FRIENDS=:FRIENDS where USERID=:USERID";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":FRIENDS", bean.Friends));
            parameters.Add(new OracleParameter(":USERID", bean.UserID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateUserGroups(OrayUser bean)
        {
            String sql = "update ORAYUSER set GROUPS=:GROUPS where USERID=:USERID";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":GROUPS", bean.Groups));
            parameters.Add(new OracleParameter(":USERID", bean.UserID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateUserHeadImage(string userID, int defaultHeadImageIndex, byte[] customizedHeadImage, int version)
        {
            String sql = "update ORAYUSER set HEADIMAGEINDEX=:HEADIMAGEINDEX,HEADIMAGEDATA=:HEADIMAGEDATA, VERSION=:VERSION where USERID=:USERID ";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":HEADIMAGEINDEX", defaultHeadImageIndex));
            parameters.Add(new OracleParameter(":HEADIMAGEDATA", customizedHeadImage));
            parameters.Add(new OracleParameter(":VERSION", version));
            parameters.Add(new OracleParameter(":USERID", userID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateUserInfo(string userID, string name, string signature, string orgID, int version)
        {
            String sql = "update ORAYUSER set NAME=:NAME,ORGID=:ORGID,SIGNATURE=:SIGNATURE, VERSION=:VERSION where USERID=:USERID ";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":NAME", name));
            parameters.Add(new OracleParameter(":ORGID", orgID));
            parameters.Add(new OracleParameter(":SIGNATURE", signature));
            parameters.Add(new OracleParameter(":VERSION", version));
            parameters.Add(new OracleParameter(":USERID", userID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateUserPassword(string userID, string newPasswordMD5)
        {
            String sql = "update ORAYUSER set PASSWORDMD5=:PASSWORDMD5 where USERID=:USERID";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":PASSWORDMD5", newPasswordMD5));
            parameters.Add(new OracleParameter(":USERID", userID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public ChatRecordPage GetChatRecordPage(ChatRecordTimeScope timeScope, string myID, string friendID, int pageSize, int pageIndex)
        {
            String sql = "select * from chatmessagerecord t where ((t.SPEAKERID=:myID and t.AUDIENCEID=:friendID) or( t.SPEAKERID=:myID1 and t.AUDIENCEID=:friendID1))   ";
            String timeSql = "";
            switch (timeScope)
            {
                case ChatRecordTimeScope.RecentWeek:
                    timeSql = "and to_char(occuretime, 'yyyy-mm-dd') in (";
                    timeSql += " SELECT to_char (SYSDATE- LEVEL + 1, 'yyyy-mm-dd') createTime    FROM    DUAL connect BY LEVEL <= 7 ";
                    timeSql += ") ";
                    break;
                case ChatRecordTimeScope.RecentMonth:
                    timeSql = "and to_char(occuretime, 'yyyy-mm-dd') in (";
                    timeSql += " SELECT to_char(SYSDATE-LEVEL + 1, 'yyyy-mm-dd') createTime FROM DUAL connect BY LEVEL <= sysdate - (add_months(sysdate, -1)) ";
                    timeSql += " ) ";
                    break;
                case ChatRecordTimeScope.Recent3Month:
                    timeSql = "and to_char(occuretime, 'yyyy-mm-dd') in (";
                    timeSql += " SELECT to_char(SYSDATE-LEVEL + 1, 'yyyy-mm-dd') createTime FROM DUAL connect BY LEVEL <= sysdate - (add_months(sysdate, -3)) ";
                    timeSql += ") ";
                    break;
                case ChatRecordTimeScope.All:
                    timeSql = "";
                    break;
                default:
                    break;
            }

            int startIndex = pageIndex * pageSize + 1;
            int endIndex = (pageIndex + 1) * pageSize;
            String pagingStr = "select  * from ( SELECT a.*, ROWNUM rn FROM( " + sql + timeSql + ")a ) WHERE rn BETWEEN " + startIndex + " AND " + endIndex;
            String pagingCountStr = "select count(*) from ( " + sql + timeSql + ")a  ";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":myID", myID));
            parameters.Add(new OracleParameter(":friendID", friendID));
            parameters.Add(new OracleParameter(":myID1", friendID));
            parameters.Add(new OracleParameter(":friendID1", myID));
            int totalCount = GetTotalCount4Page(pagingCountStr, parameters.ToArray());
            //最后一页
            if (pageIndex == int.MaxValue)
            {
                int pageCount = totalCount / pageSize;
                if (totalCount % pageSize > 0)
                {
                    pageCount += 1;
                }
                pageIndex = pageCount - 1;
            }

            DataTable dt = OracleHelper.ExecuteDataTable(pagingStr, parameters.ToArray());
            List<ChatMessageRecord> list = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<ChatMessageRecord>(dt) as List<ChatMessageRecord>;
            if (list != null && list.Count > 0)
            {
                foreach (ChatMessageRecord item in list)
                {
                    OracleHelper.SetEmptyProperty4Null<ChatMessageRecord>(item);
                }
            }
            return new ChatRecordPage(totalCount, pageIndex, list);
        }

        private int GetTotalCount4Page(String sql, params OracleParameter[] parameters)
        {
            DataTable dt = OracleHelper.ExecuteDataTable(sql, parameters);
            int totalCount = Int32.Parse(dt.Rows[0][0].ToString());
            return totalCount;
        }

        public ChatRecordPage GetGroupChatRecordPage(ChatRecordTimeScope timeScope, string groupID, int pageSize, int pageIndex)
        {
            String sql = "select * from chatmessagerecord t where t.speakerid=:groupID ";
            String timeSql = "";
            switch (timeScope)
            {
                case ChatRecordTimeScope.RecentWeek:
                    timeSql = "and to_char(occuretime, 'yyyy-mm-dd') in (";
                    timeSql += " SELECT to_char (SYSDATE- LEVEL + 1, 'yyyy-mm-dd') createTime FROM DUAL connect BY LEVEL <= 7 ";
                    timeSql += ") ";
                    break;
                case ChatRecordTimeScope.RecentMonth:
                    timeSql = "and to_char(occuretime, 'yyyy-mm-dd') in (";
                    timeSql += " SELECT to_char(SYSDATE-LEVEL + 1, 'yyyy-mm-dd') createTime FROM DUAL connect BY LEVEL <= sysdate - (add_months(sysdate, -1)) ";
                    timeSql += " ) ";
                    break;
                case ChatRecordTimeScope.Recent3Month:
                    timeSql = "and to_char(occuretime, 'yyyy-mm-dd') in (";
                    timeSql += " SELECT to_char(SYSDATE-LEVEL + 1, 'yyyy-mm-dd') createTime FROM DUAL connect BY LEVEL <= sysdate - (add_months(sysdate, -3)) ";
                    timeSql += ") ";
                    break;
                case ChatRecordTimeScope.All:
                    timeSql = "";
                    break;
                default:
                    break;
            }

            int startIndex = pageIndex * pageSize + 1;
            int endIndex = (pageIndex + 1) * pageSize;
            String pagingStr = "select  * from ( SELECT a.*, ROWNUM rn FROM( " + sql + timeSql + ")a ) WHERE rn BETWEEN " + startIndex + " AND " + endIndex;
            String pagingCountStr = "select count(*) from ( " + sql + timeSql + ")a  ";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":groupID", groupID));
            int totalCount = GetTotalCount4Page(pagingCountStr, parameters.ToArray());
            //最后一页
            if (pageIndex == int.MaxValue)
            {
                int pageCount = totalCount / pageSize;
                if (totalCount % pageSize > 0)
                {
                    pageCount += 1;
                }
                pageIndex = pageCount - 1;
            }

            DataTable dt = OracleHelper.ExecuteDataTable(pagingStr, parameters.ToArray());
            List<ChatMessageRecord> list = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<ChatMessageRecord>(dt) as List<ChatMessageRecord>;
            if (list != null && list.Count > 0)
            {
                foreach (ChatMessageRecord item in list)
                {
                    OracleHelper.SetEmptyProperty4Null<ChatMessageRecord>(item);
                }
            }
            return new ChatRecordPage(totalCount, pageIndex, new List<ChatMessageRecord>(list)); ;
        }

        public void InsertAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string comment, bool isNotified)
        {
            //删除正在申请中的记录
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":RequesterID", requesterID));
            parameters.Add(new OracleParameter(":AccepterID", accepterID));
            parameters.Add(new OracleParameter(":State", OracleDbType.Byte, (byte)RequsetType.Request, ParameterDirection.InputOutput));
            OracleHelper.ExecuteNonQuery("Delete from AddFriendRequest where RequesterID =:RequesterID and AccepterID=:AccepterID and State=:State", parameters.ToArray());

            parameters.Clear();
            String sql = string.Format("INSERT INTO ADDFRIENDREQUEST (REQUESTERID,ACCEPTERID, REQUESTERCATALOGNAME, ACCEPTERCATALOGNAME, \"COMMENT\", STATE, NOTIFIED , CREATETIME) VALUES('{0}' ,'{1}' , '{2}' , '{3}' , '{4}' ,{5} ,{6} ,:CreateTime)", requesterID, accepterID, requesterCatalogName, string.Empty, comment, (byte)RequsetType.Request, isNotified ? 1 : 0);
            parameters.Add(new OracleParameter(":CreateTime", DateTime.Now));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void SetAddFriendRequestNotified(string requesterID, string accepterID)
        {
            String sql = string.Format("update ADDFRIENDREQUEST set {0}=:Notified where {1}=:RequesterID and {2}=:AccepterID", AddFriendRequest._Notified,AddFriendRequest._RequesterID,AddFriendRequest._AccepterID) ;
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":Notified", OracleDbType.Byte, 1, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":RequesterID", requesterID));
            parameters.Add(new OracleParameter(":AccepterID", accepterID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateAddFriendRequest(string requesterID, string accepterID, string requesterCatalogName, string accepterCatalogName, bool isAgreed)
        {
            String sql = string.Format("update ADDFRIENDREQUEST set {0}=:AccepterCatalogName ,{1}=:State where {2}=:RequesterID and {3}=:AccepterID and {4}=:State2", AddFriendRequest._AccepterCatalogName, AddFriendRequest._State, AddFriendRequest._RequesterID,AddFriendRequest._AccepterID,AddFriendRequest._State);
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":AccepterCatalogName", accepterCatalogName));
            parameters.Add(new OracleParameter(":State",OracleDbType.Byte, isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":RequesterID", requesterID));
            parameters.Add(new OracleParameter(":AccepterID", accepterID));
            parameters.Add(new OracleParameter(":State2", OracleDbType.Byte, (byte)RequsetType.Request, ParameterDirection.InputOutput));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public List<AddFriendRequest> GetAddFriendRequest4NotNotified(string userID)
        {
            string sql = string.Format("select * from ADDFRIENDREQUEST where {0}=:Notified AND ({1}=:RequesterID OR {2}=:AccepterID )", AddFriendRequest._Notified, AddFriendRequest._RequesterID, AddFriendRequest._AccepterID);
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":Notified", OracleDbType.Byte, 0, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":RequesterID", userID));
            parameters.Add(new OracleParameter(":AccepterID", userID));
            DataTable dataTable= OracleHelper.ExecuteDataTable(sql, parameters.ToArray());
            List<AddFriendRequest> result = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<AddFriendRequest>(dataTable) as List<AddFriendRequest>;
            if (result != null && result.Count > 0)
            {
                foreach (AddFriendRequest item in result)
                {
                    OracleHelper.SetEmptyProperty4Null<AddFriendRequest>(item);
                }
            }
            return result;
        }

        public AddFriendRequestPage GetAddFriendRequestPage(string userID, int pageIndex, int pageSize)
        {
            string conditionStr = string.Format(" WHERE {0}={1} or {2}={3} ", AddFriendRequest._RequesterID, userID, AddFriendRequest._AccepterID, userID);
            string sql = @"SELECT *
  FROM(SELECT ROWNUM AS rowno, t.*
          FROM ADDFRIENDREQUEST t " + conditionStr +
        string.Format(" AND ROWNUM <= {0}) table_alias WHERE table_alias.rowno > {1} order by {2} desc", (pageIndex + 1) * pageSize, pageIndex * pageSize, AddFriendRequest._AutoID);

            AddFriendRequestPage page = new AddFriendRequestPage();
            string totalCountStr = "SELECT COUNT(*) FROM AddFriendRequest" + conditionStr;
            page.TotalEntityCount = this.GetTotalCount4Page(totalCountStr);
            DataTable dt = OracleHelper.ExecuteDataTable(sql);
            //去掉第一列 rowno
            if (dt != null && dt.Rows.Count > 0&&dt.Columns.Count>0)
            {
                foreach (DataColumn item in dt.Columns)
                {
                    if (item.ColumnName == "rowno".ToUpper())
                    {
                        dt.Columns.Remove(item);
                        item.Dispose();
                        break;
                    }
                }
            }            
            page.AddFriendRequestList = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<AddFriendRequest>(dt) as List<AddFriendRequest>;
            foreach (AddFriendRequest item in page.AddFriendRequestList)
            {
                OracleHelper.SetEmptyProperty4Null(item);
            }
            return page;
        }

        public string GetRequesterCatalogName(string requesterID, string accepterID)
        {
            string sql = string.Format("select {0} from ADDFRIENDREQUEST", AddFriendRequest._RequesterCatalogName);
            //string sql = string.Format("select * from AddFriendRequest", AddFriendRequest._RequesterCatalogName);
            string conditionStr = string.Format(" WHERE {0}=:RequesterID and {1}=:AccepterID and {2}=:State", AddFriendRequest._RequesterID,  AddFriendRequest._AccepterID,AddFriendRequest._State);
            List<OracleParameter> parameters = new List<OracleParameter>();            
            parameters.Add(new OracleParameter(":RequesterID", requesterID));
            parameters.Add(new OracleParameter(":AccepterID", accepterID));
            parameters.Add(new OracleParameter(":State", OracleDbType.Byte, (byte)RequsetType.Request,ParameterDirection.InputOutput));
            DataTable dt = OracleHelper.ExecuteDataTable(sql+conditionStr, parameters.ToArray());
            if (dt == null || dt.Rows.Count == 0)
            {
                return string.Empty;
            }
            //return DataRabbit.DBAccessing.DataHelper.ConvertRowToObject<AddFriendRequest>(dt.Rows[0]).RequesterCatalogName;
            return dt.Rows[0][0].ToString();
        }

        public void InsertAddGroupRequest(string requesterID, string groupID, string accepterID, string comment, bool isNotified)
        {
            //删除正在申请中的记录
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":RequesterID", requesterID));
            parameters.Add(new OracleParameter(":GroupID", groupID));
            parameters.Add(new OracleParameter(":State", OracleDbType.Byte, (byte)RequsetType.Request, ParameterDirection.InputOutput));
            OracleHelper.ExecuteNonQuery("Delete from AddGroupRequest where RequesterID =:RequesterID and GROUPID=:GroupID and State=:State", parameters.ToArray());

            parameters.Clear();
            String sql = string.Format("INSERT INTO ADDGROUPREQUEST (REQUESTERID,GROUPID, ACCEPTERID,  \"COMMENT\", STATE, NOTIFIED , CREATETIME) VALUES('{0}' ,'{1}' , '{2}' , '{3}' , '{4}' ,{5} ,:CreateTime)", requesterID, groupID, accepterID, comment, (byte)RequsetType.Request, isNotified ? 1 : 0);
            parameters.Add(new OracleParameter(":CreateTime", DateTime.Now));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void SetAddGroupRequestNotified(string requesterID, string groupID)
        {
            String sql = string.Format("update ADDGROUPREQUEST set {0}=:Notified where {1}=:RequesterID and {2}=:GROUPID", AddGroupRequest._Notified, AddGroupRequest._RequesterID, AddGroupRequest._GroupID);
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":Notified", OracleDbType.Byte, 1, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":RequesterID", requesterID));
            parameters.Add(new OracleParameter(":GROUPID", groupID));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public void UpdateAddGroupRequest(string requesterID, string groupID, bool isAgreed)
        {
            String sql = string.Format("update ADDGROUPREQUEST set {0}=:State where {1}=:RequesterID and {2}=:GroupID and {3}=:State2", AddGroupRequest._State, AddGroupRequest._RequesterID, AddGroupRequest._GroupID, AddGroupRequest._State);
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":State", OracleDbType.Byte, isAgreed ? (byte)RequsetType.Agree : (byte)RequsetType.Reject, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":RequesterID", requesterID));
            parameters.Add(new OracleParameter(":GroupID", groupID));
            parameters.Add(new OracleParameter(":State2", OracleDbType.Byte, (byte)RequsetType.Request, ParameterDirection.InputOutput));
            OracleHelper.ExecuteNonQuery(sql, parameters.ToArray());
        }

        public List<AddGroupRequest> GetAddGroupRequest4NotNotified(string userID)
        {
            string sql = string.Format("select * from ADDGROUPREQUEST where {0}=:Notified AND ({1}=:RequesterID OR {2}=:AccepterID )", AddGroupRequest._Notified, AddGroupRequest._RequesterID, AddGroupRequest._AccepterID);
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":Notified", OracleDbType.Byte, 0, ParameterDirection.InputOutput));
            parameters.Add(new OracleParameter(":RequesterID", userID));
            parameters.Add(new OracleParameter(":AccepterID", userID));
            DataTable dataTable = OracleHelper.ExecuteDataTable(sql, parameters.ToArray());
            List<AddGroupRequest> result = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<AddGroupRequest>(dataTable) as List<AddGroupRequest>;
            if (result != null && result.Count > 0)
            {
                foreach (AddGroupRequest item in result)
                {
                    OracleHelper.SetEmptyProperty4Null<AddGroupRequest>(item);
                }
            }
            return result;
        }

        public AddGroupRequestPage GetAddGroupRequestPage(string userID, int pageIndex, int pageSize)
        {
            string conditionStr = string.Format(" WHERE {0}={1} or {2}={3} ", AddGroupRequest._RequesterID, userID, AddGroupRequest._AccepterID, userID);
            string sql = @"SELECT *
  FROM(SELECT ROWNUM AS rowno, t.*
          FROM ADDGROUPREQUEST t " + conditionStr +
        string.Format(" AND ROWNUM <= {0}) table_alias WHERE table_alias.rowno > {1} order by {2} desc", (pageIndex + 1) * pageSize, pageIndex * pageSize, AddGroupRequest._AutoID);

            AddGroupRequestPage page = new AddGroupRequestPage();
            string totalCountStr = "SELECT COUNT(*) FROM ADDGROUPREQUEST" + conditionStr;
            page.TotalEntityCount = this.GetTotalCount4Page(totalCountStr);
            DataTable dt = OracleHelper.ExecuteDataTable(sql);
            //去掉第一列 rowno
            if (dt != null && dt.Rows.Count > 0 && dt.Columns.Count > 0)
            {
                foreach (DataColumn item in dt.Columns)
                {
                    if (item.ColumnName == "rowno".ToUpper())
                    {
                        dt.Columns.Remove(item);
                        item.Dispose();
                        break;
                    }
                }
            }
            page.AddGroupRequestList = DataRabbit.DBAccessing.DataHelper.ConvertDataTableToObjects<AddGroupRequest>(dt) as List<AddGroupRequest>;
            foreach (AddGroupRequest item in page.AddGroupRequestList)
            {
                OracleHelper.SetEmptyProperty4Null(item);
            }
            return page;
        }
    }
}
