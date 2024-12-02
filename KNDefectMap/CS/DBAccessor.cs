using KNDefectMap.Properties;
using Microsoft.VisualBasic.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Security.Policy;

namespace KNDefectMap
{
    public class DBAccessor
    {
        static string ConnStringSFC = "server=twprew3;uid=sfc3;pwd=sfc3;database=sfc3;CharSet = utf8;Connection Reset=True";
        static string ConnStringOra = "Provider=OraOLEDB.Oracle;Data Source=TWOR1.WUS.COM.TW;User Id=jmpsys;Password=jmpsys;OLEDB.NET=True;";
        #region 新版
        static MySqlConnection Conn=new MySqlConnection(Settings.Default.mysqlConnString );
       
        /// <summary>
        /// 連線
        /// </summary>
        /// <param name="conn">連線狀態</param>
        private static  void ConnOpen(MySqlConnection conn)
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();
        }

        /// <summary>
        /// 取得 ACM_Main 主鍵
        /// </summary>
        /// <param name="runCard">流程卡號</param>
        /// <returns></returns>
        public static  object GetACM_Main_RKey(string runCard)
        {
            ConnOpen(Conn);
            object result;
            MySqlCommand cmd = new MySqlCommand("", Conn);
            try
            {
                cmd.CommandText = string.Format("select max(rkey) from acm_Main " +
                                               " where runcard='{0}' "
                                                , runCard                                               
                                                );
                result = cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 由 spnlID 判斷acm_detial是否存在
        /// </summary>
        /// <param name="spnlId">SpnlID</param>
        /// <returns></returns>
        public static  bool ExistsACM_Detial(string spnlId)
        {
            ConnOpen(Conn);
            bool result;
            MySqlCommand cmd = new MySqlCommand("", Conn);
            try
            {
                cmd.CommandText = string.Format("select spnlID from acm_detial " +
                                               " where spnlID='{0}' "
                                                , spnlId
                                                );
                object dbReturn = cmd.ExecuteScalar();
                if (dbReturn == DBNull.Value)
                    result = false;
                else
                    result = true;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 取得 OK 的 spnlID
        /// </summary>
        /// <param name="spnlId">SpnlID</param>
        /// <returns></returns>
        public static bool PassedACM(string spnlId)
        {
            ConnOpen(Conn);
            bool result;
            MySqlCommand cmd = new MySqlCommand("", Conn);
            try
            {
                cmd.CommandText = string.Format("select spnlID from acm_detial " +
                                               " where spnlID='{0}' and flag=1 "
                                                , spnlId
                                                );
                object dbReturn = cmd.ExecuteScalar();
                if (dbReturn == DBNull.Value)
                    result = false;
                else
                    result = true;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
            return result;
        }


        /// <summary>
        /// 依SPNL ID取得GVR檢測記錄。
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
        public static Dictionary<string, string> GVRData(string spnlID)
        {
            ConnOpen(Conn);
            DataTable dt = new DataTable();
            Dictionary<string, string> result = new Dictionary<string, string>();
            string sql =string.Format( "SELECT machine,partNo,wo_num,operator " +
                                     "  FROM gmsys.gm_vi_xout_main " +
                                     "  where pnl_id = '{0}' and VRS like 'GVR%'"
                                     ,spnlID);

            MySqlDataAdapter da = new MySqlDataAdapter(sql,Conn);
            try
            {              
                da.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    result.Add("machine", dt.Rows[0]["machine"].ToString());
                    result.Add("engNo", dt.Rows[0]["partNo"].ToString());
                   
                    result.Add("operator", dt.Rows[0]["operator"].ToString());
                    if(dt.Rows[0]["wo_num"].ToString().Length>6)
                        result.Add("wo_num", dt.Rows[0]["wo_num"].ToString().Substring(0,6));
                    else
                        result.Add("wo_num", dt.Rows[0]["wo_num"].ToString());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                da.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 取得 image_path
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <param name="x">PCS行座標</param>
        /// <param name="y">PCS列座標</param>
        /// <param name="side">A = 非xout side、B = xout side</param>
        /// <returns></returns>
        public static string GetXOutImage(string spnlID,string x,string y,string side)
        {
            ConnOpen(Conn);
            object result;
            MySqlCommand cmd = new MySqlCommand("", Conn);
            try
            {
                cmd.CommandText = string.Format("select   p.image_path " +
                                                 //"  pnl_id spnl_id "+
                                                 //"  , d.x_id xoutSideX, d.y_id xoutSideY "+                                               
                                                 "from gm_vi_xout_main m, gm_vi_opt_defect_pcs p, gm_vi_xout_data d "+
                                                 "where  m.rkey = p.main_table_rkey "+
                                                 "  and p.main_table_rkey = d.gm_vi_xout_main_ptr and p.xout_side_pcs_index = d.pcs_index and p.side = d.side "+
                                                 "  and pnl_id = '{0}' "+
                                                 "  and x_id = {1} "+
                                                 "  and y_id = {2} "+
                                                 "  and p.side = '{3}'"
                                                , spnlID,x,y,side
                                                );
                result = cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
            if (result != null)
                return result.ToString();
            else
                return "";
        }
        #endregion

        /// <summary>
        /// 取得流程卡號
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
        public static object GetRunCard(string spnlID)
        {
            string sql = string.Format("select m.base_no from gmsys.gm_os2_main m " +
                             " where rkey=(select source_ptr from gm_os2_value v where finish_id='{0}')", spnlID);
            object result = ConnMySqlReturnObj(sql);
            if (result != null)
                if (result.ToString().Trim().Length == 0)
                    result = null;
            return result;
        }

        /// <summary>
        /// 找出報廢板的報廢代碼
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
        public static object CheckAllWasteByDoubleCheck(string spnlID)
        {
            string sql = string.Format("select defectCode from acm_appendxout " +
                             " where spnlID='{0}' And pcs_index = -1", spnlID);
            object result = ConnMySqlReturnObj(sql);
            if (result != null)
                if (result.ToString().Trim().Length == 0)
                    result = null;
            return result;
        }

        /// <summary>
        /// 依工號取得X最大值、Y最大值
        /// </summary>
        /// <param name="engNum">工號</param>
        /// <param name="pcsX">X最大值</param>
        /// <param name="pcsY">Y最大值</param>
        public static void GetSpnlBaseData(string engNum,out int pcsX,out int pcsY)
        {
            string sql = string.Format("Select  PCSX,PCSY,CP_REV From gmsys.gm_pcscoding m" +
                    " Where  cp_rev='{0}'", engNum);
          
            MySqlConnection conn = new MySqlConnection(Settings.Default.mysqlConnString);
            MySqlCommand cmd = new MySqlCommand("", conn);

            try
            {
                conn.Open();
                cmd.CommandText = sql;
                MySqlDataReader reader = cmd.ExecuteReader();
                engNum = "";
                pcsX = 0;
                pcsY = 0;
                if (reader.Read())
                {
                    engNum = reader["CP_REV"].ToString();
                    pcsX = int.Parse(reader["PCSX"].ToString());
                    pcsY = int.Parse(reader["PCSY"].ToString());
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }

        }

        /// <summary>
        /// 取得Wpnl各項data值 lot(批號), cp_rev(工號), wpnl_id, spnl_seq(在Wpnl上的順序), finish_id(SpnlID), flag(驗證OK=1，NG=0)
        /// </summary>
        /// <param name="input">輸入6個字元為批號; 輸入W開頭的字串為SPNL ID; 其它字串為WPNL ID</param>
        /// <returns></returns>
        public static DataTable GetWpnlData(string input)
        {
            string condition = "";
            if (input.Length == 6)
            {//lot
                condition = string.Format(" and m.base_no ='{0}000001'"
                                        , input);
            }
            else if (input.StartsWith("W"))
            {//spnl             
                condition = string.Format("   and  v.source_ptr = (select source_ptr from gm_mk4_value  "+
                                          "                        where  finish_id= '{0}' limit 1) " 
                                        , input);

            }
            else
            { //wpnl               
                condition = string.Format("   and  v.source_ptr = (select source_ptr from gm_mk4_value  " +
                                          "                        where  wpnl_id= '{0}' limit 1) "
                                        , input);
            }
            string sql = string.Format(" select  m.base_no lot,m.cp_rev,v.wpnl_id,v.spnl_seq,v.finish_id,acm.flag "+
                                       " from gm_mk4_main m inner join  gm_mk4_value v  on m.rkey = v.source_ptr " +
                                       " left join acm_detial acm on v.finish_id=acm.spnlid " +
                                       " where  wpnl_id is not null " +
                                       condition
                                        );

            DataTable resultDT = ConnMySqlReturnDataTable(sql);
            return resultDT;
        }

        /// <summary>
        /// 依Input取得X-Out Data.(同一PCS同一面有多種Defect全部撈出)
        /// </summary>
        /// <param name="input">輸入6個字元為批號; 輸入W開頭的字串為SPNL ID; 其它字串為WPNL ID.</param>
        /// <returns></returns>
        public static DataTable  GetXOut_AVI(string input)
        {
            string condition = "";
            if (input.Length == 6)
            {//lot
                condition =string.Format( "  and pnl_id in (select finish_id from gm_mk4_main m, gm_mk4_value v where m.rkey = v.source_ptr and m.base_no = '{0}000001') "
                                        ,input);
            }
            else if (input.StartsWith("W"))
            {//spnl             
                                
                condition = string.Format("  where pnl_id = '{0}' "
                                        , input);
            }
            
            else
            { //wpnl               
                condition = string.Format(" where pnl_id in (select finish_id from gm_mk4_value where wpnl_id = '{0}') "
                                        , input);
            }
            string sql = string.Format("select distinct 'AVI' station, aa.*,upper( bb.defectcode ) defectCode " +
                                       "from(                               " +
                                       "         SELECT  vm.partno cp_rev, vm.wo_num lot, vm.pnl_id finish_id,  vm.machine detector" +
                                       "                         , vd.pcs_index, vd.x_id x, vd.y_id y, vd.side " +
                                       "                         , concat_ws('-', l.name_segment, l.name_region, l.name_method) item " +
                                       "                         , vd.defect_code defectCode " +
                                       "         FROM gmsys.gm_vi_xout_main vm " +
                                       "                     left join gm_vi_xout_data vd on vm.rkey = vd.gm_vi_xout_main_ptr" +
                                       "                     left join gm_vi_opt_defect_pcs p on vd.gm_vi_xout_main_ptr = p.main_table_rkey and vd.pcs_index = p.xout_side_pcs_index and vd.side=p.side" +
                                       "                     left join gm_vi_opt_defect_location l on p.rkey = l.pcs_index_rkey" +                                       
                                         condition +
                                       "     ) aa " +
                                       //"     --查檢測項目defect code " +
                                       "      left join (  SELECT distinct pm.cp_rev,pm.machine detector,pr.side,pri.item,pri.DefectCode FROM gmsys.gvi_profile_main pm, gvi_recipe pr,gvi_recipe_items pri " +
                                       "         where pm.rkey = pr.profileptr and pr.rkey = pri.gvirecipeptr " +
                                       "      ) bb " +
                                       " on   aa.cp_rev = bb.cp_rev and aa.detector = bb.detector and aa.item = bb.item       and aa.side +'%' = bb.side +'%' "
                                                        );

            DataTable resultDT = ConnMySqlReturnDataTable(sql);
            return resultDT;
        }

        /// <summary>
        /// 取得 defect code
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <param name="x">PCS行座標</param>
        /// <param name="y">PCS列座標</param>
        /// <param name="side">A = 非xout side、B = xout side</param>
        /// <returns></returns>
        public static DataTable GetDefectLocation(string spnlID,string x,string y,string side)
        {
            string sql= string.Format("select  aa.*,upper( bb.defectcode ) defectCode " +
                                       "from(                               " +
                                       "         SELECT  vm.partno cp_rev, vm.wo_num lot, vm.pnl_id finish_id,  vm.machine " +
                                       "                         , vd.pcs_index, vd.x_id x, vd.y_id y, vd.side " +
                                       "                         ,l.orgx,l.orgy,l.width,l.height, concat_ws('-', l.name_segment, l.name_region, l.name_method) item " +
                                       "         FROM gmsys.gm_vi_xout_main vm " +
                                       "                     , gm_vi_xout_data vd " +
                                       "                     , gm_vi_opt_defect_pcs p " +
                                       "                     , gm_vi_opt_defect_location l " +
                                       "         where  vm.rkey = vd.gm_vi_xout_main_ptr " +
                                       "             and vd.gm_vi_xout_main_ptr = p.main_table_rkey  and vd.pcs_index = p.xout_side_pcs_index and vd.side=p.side " + //join後會濾掉Camtek
                                       "             and p.rkey = l.pcs_index_rkey " +
                                       "         and pnl_id = '{0}' and vd.side='{1}' and vd.x_id={2} and vd.y_id={3} "+
                                       "     ) aa " +
                                       //"     --查檢測項目defect code " +
                                       "      ,(  SELECT distinct pm.cp_rev,pm.machine,pr.side,pri.item,pri.DefectCode FROM gmsys.gvi_profile_main pm, gvi_recipe pr,gvi_recipe_items pri " +
                                       "         where pm.rkey = pr.profileptr and pr.rkey = pri.gvirecipeptr " +
                                       "      ) bb " +
                                       " where      aa.cp_rev = bb.cp_rev and aa.machine = bb.machine and aa.item = bb.item       and aa.side = bb.side "
                                         ,spnlID,side,x,y );
            DataTable resultDT = ConnMySqlReturnDataTable(sql);
            return resultDT;
        }

        /// <summary>
        /// 取得 XOut OS cp_rev(工號), lot(批號), finish_id(Spnl_ID), detector(電測機台名稱), 
        /// pcs_index(二維轉成一維的索引), x(PCS在SPNL長邊座標), y(PCS在SPNL短邊座標), 'B' side(Xout面), item(電測報廢碼), defectCode(電測報廢碼)
        /// </summary>
        /// <param name="input">輸入6個字元為批號; 輸入W開頭的字串為SPNL ID; 其它字串為WPNL ID</param>
        /// <returns></returns>
        public static DataTable GetXOut_OS(string input)
        {
            string condition = "";
            if (input.Length == 6)
            {//lot
                condition = string.Format("  and m.base_no= '{0}000001' "
                                        , input);
            }
            else if (input.StartsWith("W"))
            {//spnl             
                condition = string.Format("   and v.finish_id= '{0}' "
                                        , input);

            }
            else
            { //wpnl               
                condition = string.Format(" and v.wpnl_id = '{0}'  "
                                        , input);
            }
            string sql = string.Format("SELECT 'OS2' station, m.cp_rev,m.base_no lot " +
                                       "     , v.finish_id " +
                                       "     ,ov.machine_name detector " +
                                       "     ,(ov.ng_addry-1)*om.pcsx+ov.ng_addrx-1  pcs_index " +
                                       "     ,ov.ng_addrx x,ov.ng_addry y,'B' side,ov.ng_type item,ov.ng_type defectCode  " +
                                       "FROM gm_mk4_main m, gm_mk4_value v ,gm_os2_value ov,gm_os2_main om " +
                                     " where m.rkey=v.source_ptr 	and v.finish_id=ov.finish_id and ov.source_ptr=om.rkey " +
                                    // "      and ng_addrx<>0   " +
                                    "        and ov.ng_type is not null " +
                                     condition
                                     );
            DataTable resultDT = ConnMySqlReturnDataTable(sql);
            return resultDT;
        }
        /// <summary>
        /// 取得目檢X-Out
        /// </summary>
        /// <param name="input">6碼批號或W開頭的SPNL ID或WPNL ID</param>
        /// <returns></returns>
        public static DataTable GetXOut_human(string input)
        {
            string condition = "";
            if (input.Length == 6)
            {//lot
                condition = string.Format("  and m.base_no= '{0}000001' "
                                        , input);
            }
            else if (input.StartsWith("W"))
            {//spnl             
                condition = string.Format("   and v.finish_id= '{0}' "
                                        , input);

            }
            else
            { //wpnl               
                condition = string.Format(" and v.wpnl_id = '{0}'  "
                                        , input);
            }
            string sql = string.Format(@"SELECT '目檢' station,m.cp_rev,m.base_no lot , v.finish_id  
                                            ,vdc.editor detector  
                                            ,pcs_index  ,x_id x,y_id y,'B' side,'' item, apd.defectCode  
                                         FROM gm_mk4_main m inner join  gm_mk4_value v on  m.rkey = v.source_ptr                                              
                                                left join acm_appendxout apd on v.finish_id = apd.spnlid
                                                left join gm_vi_double_check vdc on v.finish_id  = vdc.spnl_id
                                                left join gm_vi_double_check_detail vdcd on v.finish_id = vdcd.spnl_id and  v.finish_id = vdcd.defect_Code
                                         where   1=1 " +                                                           
                                     condition
                                     );
            DataTable resultDT = ConnMySqlReturnDataTable(sql);
            return resultDT;
        }
        /// <summary>
        /// 取得報廢座標(OS+AVI+目檢)
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
        public static DataTable GetNGPcsListMySql(string spnlID)
        {
            string sql = string.Format("select  a.* from (   " +
                                      " select vid.x_id x,vid.y_id y,vid.defect_code defectCode     " +
                                      " from    (select * from gm_vi_xout_main where pnl_id='{0}')   vim , gm_vi_xout_data vid      " +
                                      " where  vim.rkey=vid.gm_vi_xout_main_ptr   " +
                                      " union   " +
                                      " SELECT ng_addrx x,ng_addry y,ng_type defectCode FROM gmsys.gm_os2_value   " +
                                      " where ng_addrx<>0   " +
                                      "     and finish_id='{0}'  " +
                                      " union     " +
                                      " select x_id x,y_id y,defectCode FROM gmsys.acm_appendxout  " +
                                      " where x_id<>0 and spnlid='{0}'   " +
                                      " ) a     " 
                                      //" group by defectCode;"
                                      , spnlID);
            DataTable resultDT = ConnMySqlReturnDataTable(sql);
            return resultDT;
        }

        /// <summary>
        /// 取得NG類型
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
        public static object CheckAllWaste(string spnlID)
        {
            object result = null;
            string sql = string.Format("SELECT NG_TYPE From gm_os2_value where finish_ID = '{0}' and NG_ADDRX = 0 and NG_ADDRY = 0", spnlID);
            result = ConnMySqlReturnObj(sql);
            if (result != null)
                if (result.ToString().Trim().Length == 0)
                    result = null;
            return result;
        }

        /// <summary>
        /// 取得報廢PCS數(OS + VI)不含OS手動標記。
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
        public static int GetNumOfNGPcs(string spnlID)
        {
            int result = 0;
            string sql = string.Format("select count(distinct X,Y)  from (" +
                                      "     SELECT NG_ADDRX X,NG_ADDRY Y" +
                                      "     FROM gmsys.gm_os2_value " +
                                      "     where finish_ID = '{0}' " +
                                      "         and NG_ADDRX > 0 and machine_name<>'1.手動標記' " +
                                      "    Union  " +
                                      "     select d.x_id X,d.y_id Y " +
                                      "     from (select * from gm_vi_xout_main where pnl_id='{0}' )  m,gm_vi_xout_data d" +
                                      "     where m.rkey=d.gm_vi_xout_main_ptr " +
                                      "   )a   "
                                      , spnlID);

            result =int.Parse( ConnMySqlReturnObj(sql).ToString());

            return result;
        }
        
        /// <summary>
        /// 取得報廢清單(OS)
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
        public static List<Point> GetOSNGPcsListMySql(string spnlID)
        {
            List<Point> ngList = new List<Point>();
            string sql = string.Format("SELECT distinct NG_ADDRX,NG_ADDRY FROM gmsys.gm_os2_value where finish_ID = '{0}'" +
                                       " and NG_ADDRX > 0 and NG_ADDRY > 0" , spnlID);

            DataTable resultDT = ConnMySqlReturnDataTable(sql);
            for (int i = 0; i < resultDT.Rows.Count; i++)
            {
                ngList.Add(new Point(int.Parse(resultDT.Rows[i]["NG_ADDRX"].ToString()), int.Parse(resultDT.Rows[i]["NG_ADDRY"].ToString())));

            }
            return ngList;
        }

        /// <summary>
        ///將query結果存成 DataTable
        /// </summary>
        /// <param name="sql">Sql陳述式</param>
        /// <returns></returns>
        private static DataTable ConnMySqlReturnDataTable(string sql)
        {           
            ConnOpen(Conn);

            MySqlDataAdapter da= new MySqlDataAdapter(sql, Conn);
            DataTable dt = new DataTable();
            try
            {
                //sql = "Select * from acm_main ";
                //da = new MySqlDataAdapter(sql, Settings.Default.mysqlConnString);
                
                da.Fill(dt);
            }
            catch
            {
                throw;
            }
            finally
            {
                da.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// Sql返回受影響列數值
        /// </summary>
        /// <param name="sql">Sql陳述式</param>
        /// <returns></returns>
        // ExecuteNonQuery
        private static int ConnMySqlReturnInt(string sql)
        {
            int result;
            MySqlConnection conn = new MySqlConnection(Settings.Default.mysqlConnString);
            MySqlCommand cmd = new MySqlCommand("", conn);
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                result = cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Sql指令僅取單一值
        /// </summary>
        /// <param name="sql">Sql陳述式</param>
        /// <returns></returns>
        private static object ConnMySqlReturnObj(string sql)
        {
            object result;
            MySqlConnection conn = new MySqlConnection(Settings.Default.mysqlConnString);
            MySqlCommand cmd = new MySqlCommand("", conn);
            try
            {
                conn.Open();
                cmd.CommandText = sql;
                result = cmd.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
                conn.Dispose();
            }
            return result;
        }

        /// <summary>
        /// 取得廢板簡碼轉為全碼
        /// </summary>
        /// <param name="simpleCode">簡碼</param>
        /// <returns></returns>
        public static object SearchWasteCode(string simpleCode)
        {
            object dataBaseSimpleCode;
            try
            {
                dataBaseSimpleCode = ConnMySqlReturnObj(string.Format("select wasteCode from gm_vi_waste_code where simpleCode='{0}'", simpleCode));
            }
            catch
            {
                throw;
            }

            return dataBaseSimpleCode;
        }

        /// <summary>
        /// 取得SFC流程卡號
        /// </summary>
        /// <param name="spnlID">SpnlID</param>
        /// <returns></returns>
 #region SFC
        public static object GetSFCRuncard(string spnlID)
        {
           
            object result;
            MySqlConnection conn = new MySqlConnection(ConnStringSFC);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("", conn);
            try
            {
                cmd.CommandText = string.Format("select run_card from sfc3.sfc_trace_wpnl_map " +
                                                "where org_id = 1 AND trace_id = '{0}' " +
                                                "order by create_time desc "
                                                , spnlID
                                                );
                result = cmd.ExecuteScalar();

                if (result != null)
                    if (result.ToString().Trim().Length == 0)
                        result = null;
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return result;
        }

        /// <summary>
        /// 檢查spnl在sfc中是否已報廢。
        /// </summary>
        /// <param name="spnlID">Barcode</param>
        /// <returns></returns>
        public static bool isSpnlWaste_SFC(string spnlID)
        {

            bool result=true;
            MySqlConnection conn = new MySqlConnection(ConnStringSFC);
            conn.Open();
            MySqlCommand cmd = new MySqlCommand("", conn);
            try
            {
                cmd.CommandText = string.Format("select * " +
                                                "FROM sfc_wpnlid_def_reason r " +
                                                "    , (select org_id, run_card, spnl_id " +
                                                "      from sfc_trace_wpnl_map " +
                                                "      where org_id = 1  and trace_id = '{0}') m " +
                                                "WHERE r.org_id = m.org_id and r.run_card = m.run_card and r.spnl_id = m.spnl_id " +
                                                "    and r.pcs_id is null "
                                                , spnlID
                                                );
                result = cmd.ExecuteScalar()==null?false:true;
              
            }
            catch
            {
                throw;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
            return result;
        }
        #endregion

        /// <summary>
        /// Sql指令僅取單一值
        /// </summary>
        /// <param name="sql">Sql陳述式</param>
        /// <returns></returns>
#region oracle
        public static object ConnOracleReturnObj(string sql)
        {
            object result;
            using (OleDbConnection connection = new OleDbConnection(ConnStringOra))
            {
                
                OleDbCommand command = new OleDbCommand(sql, connection);

                connection.Open();
                result = command.ExecuteScalar();

            }

            return result;
        }

        /// <summary>
        /// 取得該工號的可報廢數
        /// </summary>
        /// <param name="engNum">工號</param>
        /// <returns></returns>
        public static int GetTotalXoutNum(string engNum)
        {
            int result = 0;

            if (engNum != null)
            {
                //取得該工號可報廢數
                string sql = " SELECT b.value FROM FlwSubPnProc A,FlwSubPnProcFvarValue B  " +
                      " WHERE A.PNIDX = B.PNIDX AND A.SUBPN = B.SUBPN AND A.SEQ = B.SEQ  " +
                      " And a.pnidx in(Select bb.pnidx From esm_ac aa,masterpn bb Where aa.PN=bb.pn And aa.rev=bb.rev And ( enum = '" + engNum + "')) " +
                      " And b.varname='WP_ScrapQuant' And a.pricode = '1VI1-D' ";
                object numObj = ConnOracleReturnObj(sql);
                if (numObj != null)
                    result = Convert.ToInt32(numObj.ToString().Trim().Replace("PCS", ""));
                if (result == 0)
                {
                    //查詢舊系統
                    sql = " Select PARAMETER from ( with A as (select a.*,b.cp_rev from data0038 a,data0050 b  " +
                          " Where   a.source_ptr=b.rkey AND  a.ttype=4 AND b.CP_REV ='" + engNum + "' )   " +
                          " Select distinct cp_rev,CASE WHEN A.DEF_ROUT_PARA_1_PTR=76 THEN A.PARAMETER_1    " +
                          " WHEN A.DEF_ROUT_PARA_2_PTR=76 THEN A.PARAMETER_2    " +
                          " WHEN A.DEF_ROUT_PARA_3_PTR=76 THEN A.PARAMETER_3    " +
                          " WHEN A.DEF_ROUT_PARA_4_PTR=76 THEN A.PARAMETER_4    " +
                          " WHEN A.DEF_ROUT_PARA_5_PTR=76 THEN A.PARAMETER_5    " +
                          " WHEN A.DEF_ROUT_PARA_6_PTR=76 THEN A.PARAMETER_6    " +
                          " WHEN A.DEF_ROUT_PARA_7_PTR=76 THEN A.PARAMETER_7    " +
                          " WHEN A.DEF_ROUT_PARA_8_PTR=76 THEN A.PARAMETER_8    " +
                          " WHEN A.DEF_ROUT_PARA_9_PTR=76 THEN A.PARAMETER_9    " +
                          " WHEN A.DEF_ROUT_PARA_10_PTR=76 THEN A.PARAMETER_10  " +
                          " END AS PARAMETER From A  ) Where  TRIM(PARAMETER) is not null ";
                    numObj = ConnOracleReturnObj(sql);
                    if (numObj != null)
                        result = Convert.ToInt32(numObj.ToString().Trim().Replace("PCS", ""));
                }
            }

            return result;
        }

        /// <summary>
        /// 取得操作者名
        /// </summary>
        /// <param name="id">操作者ID</param>
        /// <returns></returns>
        public static string getUserName(string id)
        {
            object result;

            id = id.ToUpper();
            result = ConnOracleReturnObj(string.Format("select emp_name from wus9001 where emp_no='{0}'", id));

            if (result == null)
                return "";
            else
                return result.ToString();
        }

        /// <summary>
        /// 取得排板SPNL寬高。[0]: Width, [1]: Height
        /// </summary>
        /// <param name="cp_rev">工號</param>
        /// <returns></returns>
        public static float[] GetSPNLSize_mm(string cp_rev)
        {
            float[] result=new float[2];            
            
            using (OleDbConnection connection = new OleDbConnection(ConnStringOra))
            {
                string sql = "select cp_rev,sm_size1 h, sm_size2 w from spc9028 where cp_rev = :cp_rev";
                OleDbCommand command = new OleDbCommand(sql, connection);
                command.Parameters.AddWithValue(":cp_rev", cp_rev);//.Parameters.Add(":columnName", 2);

                connection.Open();
                OleDbDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    result[0] =float.Parse( dataReader["w"].ToString());
                    result[1] = float.Parse(dataReader["h"].ToString());
                }
                dataReader.Close();
            }

            return result;
        }

        /// <summary>
        /// 取得XoutSide為TOP或BOT
        /// </summary>
        /// <param name="cp_rev">工號</param>
        /// <returns></returns>
        public static string GetXoutSide(string cp_rev)
        {
            cp_rev = cp_rev.ToUpper();
            string xOutSide = "";
            //OracleConnection conn = new OracleConnection(OracleConnString);
            OleDbConnection conn = new OleDbConnection(ConnStringOra);

            try
            {
                conn.Open();
                //從設定表中擷取 XoutSide
                xOutSide = GetXOutSide_ezplan(cp_rev, conn).Trim();
                if (xOutSide == "")
                {
                    xOutSide = GetXOutSide_Paradigm(cp_rev, conn).Trim();
                    if (xOutSide == "")
                    {
                        xOutSide = "TOP";  //舊工號除K1777,J0787 , L0549,J2637已下DFM修改為BOT外，其餘預設TOP。

                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();

            }

            return xOutSide.ToUpper();
        }

        //擷取  Paradigm 工號的  'X-OUT SIDE'參數值
        private static string GetXOutSide_Paradigm(string cp_rev, OleDbConnection conn)
        {

            OleDbCommand cmd = new OleDbCommand("", conn);
            object result = "";

            //data0035.rkey=2610,參數名稱= 'X-OUT SIDE'    
            cmd.CommandText = string.Format(
                             "select PARAMETER from (     " +
                             "  with A as (select a.*,b.cp_rev from data0038 a,data0050 b  " +
                             "             where   a.source_ptr=b.rkey AND  a.ttype=4 AND b.CP_REV ='{0}' )   " +
                             "  select distinct cp_rev,CASE WHEN A.DEF_ROUT_PARA_1_PTR=2610 THEN A.PARAMETER_1    " +
                             "       WHEN A.DEF_ROUT_PARA_2_PTR=2610 THEN A.PARAMETER_2     " +
                             "       WHEN A.DEF_ROUT_PARA_3_PTR=2610 THEN A.PARAMETER_3      " +
                             "       WHEN A.DEF_ROUT_PARA_4_PTR=2610 THEN A.PARAMETER_4       " +
                             "       WHEN A.DEF_ROUT_PARA_5_PTR=2610 THEN A.PARAMETER_5        " +
                             "       WHEN A.DEF_ROUT_PARA_6_PTR=2610 THEN A.PARAMETER_6        " +
                             "       WHEN A.DEF_ROUT_PARA_7_PTR=2610 THEN A.PARAMETER_7        " +
                             "       WHEN A.DEF_ROUT_PARA_8_PTR=2610 THEN A.PARAMETER_8        " +
                             "       WHEN A.DEF_ROUT_PARA_9_PTR=2610 THEN A.PARAMETER_9        " +
                             "       WHEN A.DEF_ROUT_PARA_10_PTR=2610 THEN A.PARAMETER_10       " +
                             "        END AS PARAMETER                                        " +
                             "  from A  )                                                       " +
                             "where  PARAMETER is not null"
                             , cp_rev);
            result = cmd.ExecuteScalar();
            cmd.Dispose();

            if (result != null)
                return result.ToString().Trim().ToUpper();
            else
                return "";
        }

        //擷取  ezplan 工號的  'X-OUT SIDE'參數值
        private static string GetXOutSide_ezplan(string cp_rev, OleDbConnection conn)
        {
            object result = "";
            OleDbCommand cmd = new OleDbCommand("", conn);
            cmd.CommandText = string.Format(
                                            "SELECT B.VALUE    " +
                                            "FROM FlwSubPnProc A,FlwSubPnProcFvarValue B   " +
                                            "WHERE A.PNIDX = B.PNIDX AND A.SUBPN = B.SUBPN AND A.SEQ = B.SEQ  " +
                                            "       and a.pnidx in(select bb.pnidx from esm_ac aa,masterpn bb where aa.PN=bb.pn and aa.rev=bb.rev and ( enum = '{0}'))  " +
                                            "       and varname='WP_XoutSide' " +
                                            "       and a.pricode = '1VID-D'"
                                            , cp_rev);
            result = cmd.ExecuteScalar();
            cmd.Dispose();

            if (result != null)
                return result.ToString();
            else
                return "";
        }

        #endregion
    }
}
