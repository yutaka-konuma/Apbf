// <copyright file="ChatSampleRepository.cs" company="Above Beyond Project Framework">
// Copyright (c) Above Beyond Project Framework. All rights reserved.
// </copyright>

namespace Sample05Chat.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AbpfBase.Repository;
    using AbpfUtil.Models;
    using AbpfUtil.Util;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Data.SqlClient;
    using Sample01Base.Repository;
    using Sample05Chat.Models;

    // データ型は呼び出し元で指定
    public class ChatSampleRepository : SampleRepositoryBase
    {
        private readonly SqlConnection sqlConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatSampleRepository"/> class.
        /// </summary>
        /// <param name="sqlConnection">sqlConnection.</param>
        public ChatSampleRepository(SqlConnection sqlConnection)
            : base(sqlConnection)
        {
            this.sqlConnection = sqlConnection;
        }

        public List<ChatSampleViewData> SearchAllChatGroup(UserInfo userInfo)
        {
            List<ChatSampleViewData> results = new List<ChatSampleViewData>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            StringBuilder sqlstr = new StringBuilder(@$"
SELECT CHAT_GROUP_ID  
     , CHAT_GROUP_NAME 
FROM C_CHAT_GROUP 
WHERE CHAT_GROUP_ID IN ('1', '2') -- とりあえず2件に限定
ORDER BY CHAT_GROUP_ID
            ");

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.sqlConnection))
            {
                var ret = this.ConvertList<ChatSampleViewData>(com, paramList, userInfo);
                return ret;
            }
        }

        public List<ChatSampleSendData> SearchChatLog(string chatGroupId, UserInfo userInfo)
        {
            List<ChatSampleSendData> results = new List<ChatSampleSendData>();
            List<SqlParameter> paramList = new List<SqlParameter>();

            StringBuilder sqlstr = new StringBuilder(@$"
SELECT L.CHAT_GROUP_ID  
     , L.CHAT_SEQ 
     , L.CHAT_USER_ID 
     , M.CHAT_USER_NAME
     , L.CHAT_CONNECTION_ID 
     , L.CHAT_TEXT 
     , '' AS CHAT_READED 
     , '' AS CHAT_NOREAD
     , (SUBSTRING(L.SYS_ENT_DATE,1,4) + '/' +
	    SUBSTRING(L.SYS_ENT_DATE,5,2) + '/' +
	    SUBSTRING(L.SYS_ENT_DATE,7,2) + ' ' +
	    SUBSTRING(L.SYS_ENT_TIME,1,2) + ':' +
	    SUBSTRING(L.SYS_ENT_TIME,3,2) + ':' +
	    SUBSTRING(L.SYS_ENT_TIME,5,2) ) AS CHAT_DATE
FROM C_CHAT_LOG AS L
LEFT JOIN C_CHAT_MEMBER AS M
 ON  M.CHAT_GROUP_ID  = L.CHAT_GROUP_ID
 AND M.CHAT_USER_ID   = L.CHAT_USER_ID
WHERE L.CHAT_GROUP_ID = @CHAT_GROUP_ID
ORDER BY L.CHAT_SEQ
            ");
            paramList.Add(AbpfDBUtil.GetSqlParameter("@CHAT_GROUP_ID", chatGroupId));

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.sqlConnection))
            {
                results = this.ConvertList<ChatSampleSendData>(com, paramList, userInfo);

                if (results != null && results.Count > 0)
                {
                    // 既読設定
                    decimal maxSeq = decimal.Parse(results.Max(a => a.ChatSeq).ToString());

                    sqlstr = new StringBuilder(@$"
UPDATE C_CHAT_MEMBER
SET CHAT_READED_SEQ = @CHAT_READED_SEQ 
   , SYS_UPD_DATE   = FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'yyyyMMdd')
   , SYS_UPD_TIME   = FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'HHmmss')
   , SYS_UPD_PGID   = 'Chat'
   , SYS_UPD_USER   = SUBSTRING(@SYS_USER, 1, 20)
WHERE CHAT_GROUP_ID = @CHAT_GROUP_ID
  AND CHAT_USER_ID  = @CHAT_USER_ID
                ");
                    using (SqlCommand com2 = new SqlCommand(sqlstr.ToString(), this.sqlConnection))
                    {
                        com2.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_READED_SEQ", maxSeq));
                        com2.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_GROUP_ID", chatGroupId));
                        com2.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_USER_ID", userInfo.UserIdentifier));
                        com2.Parameters.Add(AbpfDBUtil.GetSqlParameter("@SYS_USER", userInfo.UId));

                        // SQL実行
                        this.AbpfExecuteNonQuery(com2, userInfo);
                    }
                }
            }

            // 既読未読情報
            sqlstr = new StringBuilder(@$"
SELECT CHAT_USER_ID 
     , CHAT_USER_NAME
     , CHAT_READED_SEQ
FROM C_CHAT_MEMBER 
WHERE CHAT_GROUP_ID = @CHAT_GROUP_ID
            ");

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.sqlConnection))
            {
                com.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_GROUP_ID", chatGroupId));

                using (var ret = this.AbpfExecuteReader(com, userInfo))
                {
                    while (ret.Read())
                    {
                        decimal readedSeq = ret.GetDecimal(ret.GetOrdinal("CHAT_READED_SEQ"));
                        for (int i = 0; results.Count > i; i++)
                        {
                            if (results[i].ChatSeq > readedSeq)
                            {
                                // 未読
                                results[i].ChatNoread += ret.GetString(ret.GetOrdinal("CHAT_USER_NAME")) + " ";
                            }
                            else
                            {
                                // 既読
                                results[i].ChatReaded += ret.GetString(ret.GetOrdinal("CHAT_USER_NAME")) + " ";
                            }
                        }
                    }

                    ret.Close();
                }
            }

            return results;
        }

        public List<T> ConvertList<T>(SqlCommand com, List<SqlParameter> paramList, UserInfo userInfo)
            where T : class
        {
            List<T> results = new List<T>();
            if (paramList.Count > 0)
            {
                com.Parameters.AddRange(paramList.ToArray());
            }

            var dre = new AbpfDataReaderEnumerable<T>(com, userInfo);
            foreach (T row in dre.AsEnumerable())
            {
                results.Add(row);
            }

            return results;
        }

        public void InsertChatLog(string chatGroupId, string chatMessage, UserInfo userInfo)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();
            StringBuilder sqlstr = new StringBuilder(@$"
INSERT INTO C_CHAT_LOG(
     CHAT_GROUP_ID
   , CHAT_SEQ
   , CHAT_USER_ID
   , CHAT_CONNECTION_ID
   , CHAT_TEXT
   , SYS_ENT_DATE
   , SYS_ENT_TIME
   , SYS_ENT_PGID
   , SYS_ENT_USER
   , SYS_UPD_DATE
   , SYS_UPD_TIME
   , SYS_UPD_PGID
   , SYS_UPD_USER
   , SYS_DEL_FLG
) VALUES (
     @CHAT_GROUP_ID
   , (SELECT (ISNULL(MAX(CHAT_SEQ), 0) + 1) AS CHAT_SEQ
      FROM  C_CHAT_LOG
      WHERE CHAT_GROUP_ID = @CHAT_GROUP_ID
     )
   , @CHAT_USER_ID
   , @CHAT_CONNECTION_ID
   , @CHAT_TEXT
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'yyyyMMdd')
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'HHmmss')
   , 'Chat'
   , SUBSTRING(@SYS_USER, 1, 20)
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'yyyyMMdd')
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'HHmmss')
   , 'Chat'
   , SUBSTRING(@SYS_USER, 1, 20)
   , 0
)");
            paramList.Add(AbpfDBUtil.GetSqlParameter("@CHAT_GROUP_ID", chatGroupId));
            paramList.Add(AbpfDBUtil.GetSqlParameter("@CHAT_TEXT", chatMessage));
            paramList.Add(AbpfDBUtil.GetSqlParameter("@CHAT_USER_ID", userInfo.UserIdentifier));
            paramList.Add(AbpfDBUtil.GetSqlParameter("@CHAT_CONNECTION_ID", string.Empty));
            paramList.Add(AbpfDBUtil.GetSqlParameter("@SYS_USER", userInfo.UId));

            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.sqlConnection))
            {
                if (paramList.Count > 0)
                {
                    com.Parameters.AddRange(paramList.ToArray());
                }

                // SQL実行
                this.AbpfExecuteNonQuery(com, userInfo);
            }
        }

        public void AddToGroup(string chatGroupId, UserInfo userInfo)
        {
            StringBuilder sqlstr = new StringBuilder(@$"
SELECT COUNT(*) AS CNT
FROM C_CHAT_MEMBER 
WHERE CHAT_GROUP_ID = @CHAT_GROUP_ID
  AND CHAT_USER_ID  = @CHAT_USER_ID
            ");

            int cnt = 0;
            using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.sqlConnection))
            {
                com.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_GROUP_ID", chatGroupId));
                com.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_USER_ID", userInfo.UserIdentifier));

                using (var ret = this.AbpfExecuteReader(com, userInfo))
                {
                    if (ret.Read())
                    {
                        cnt = ret.GetInt32(0);
                    }

                    ret.Close();
                }
            }

            if (cnt == 0)
            {
                // グループ未登録者の場合
                sqlstr = new StringBuilder(@$"
INSERT INTO C_CHAT_MEMBER(
     CHAT_GROUP_ID
   , CHAT_USER_ID
   , CHAT_USER_NAME
   , CHAT_READED_SEQ
   , SYS_ENT_DATE
   , SYS_ENT_TIME
   , SYS_ENT_PGID
   , SYS_ENT_USER
   , SYS_UPD_DATE
   , SYS_UPD_TIME
   , SYS_UPD_PGID
   , SYS_UPD_USER
   , SYS_DEL_FLG
) VALUES (
     @CHAT_GROUP_ID
   , @CHAT_USER_ID
   , @CHAT_USER_NAME
   , 0
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'yyyyMMdd')
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'HHmmss')
   , 'Chat'
   , SUBSTRING(@SYS_USER, 1, 20)
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'yyyyMMdd')
   , FORMAT(DATEADD(hour, 9, SYSUTCDATETIME()),'HHmmss')
   , 'Chat'
   , SUBSTRING(@SYS_USER, 1, 20)
   , 0
)");
                using (SqlCommand com = new SqlCommand(sqlstr.ToString(), this.sqlConnection))
                {
                    com.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_GROUP_ID", chatGroupId));
                    com.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_USER_ID", userInfo.UserIdentifier));
                    com.Parameters.Add(AbpfDBUtil.GetSqlParameter("@CHAT_USER_NAME", userInfo.Name));
                    com.Parameters.Add(AbpfDBUtil.GetSqlParameter("@SYS_USER", userInfo.UId));

                    // SQL実行
                    this.AbpfExecuteNonQuery(com, userInfo);
                }
            }
        }
    }
}
