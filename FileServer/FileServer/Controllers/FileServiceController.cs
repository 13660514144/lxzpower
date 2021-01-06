using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HelperTools;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace FileServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileServiceController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        DataHandle Mo = new DataHandle();
        ParasGobalGet Paras = new ParasGobalGet();
        public RedisHelper Redis = new RedisHelper();
        public HostReqModel Host = new HostReqModel();
        public string DataConnType = "mssql";
        public string ThisConn;
        dynamic Obj = new DynamicObj();
        public string dir = AppDomain.CurrentDomain.BaseDirectory;
        public string UpFile;
        IDal dal;
        public FileServiceController(IConfiguration configuration)
        {
            _configuration = configuration;
            ThisConn = _configuration.GetValue<String>("ConnectionStr").ToString();
        }
        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("DelAttachFile")]
        public string DelAttachFile([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            Redis.DbType = "TbStruct";
            JObject Struct = JObject.Parse(Redis.Default.StringGet("AttachStore.Tbl_AttachList.Config.json"));

            MsDel Del = new MsDel();
            Del.ThisConn = ThisConn;
            Result = Del.DelAttachPara(Struct, ApiObj);

            return Result;
        }
        /// <summary>
        /// 附件列表请示
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        [HttpPost("GetAttachList")]
        public string GetAttachList([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            DataHandle.WorkResult WordArea = new DataHandle.WorkResult();
            WordArea.SqlCmd = $@"
                SELECT 
                      [ParentCode]
                      ,[SourceDb]
                      ,[SourceTb]
                      ,[WebFilePath]
                      ,[AgainName]
                      ,[SourceName]
                      ,[SysCode]
                      ,[FileType]
                      ,[UnitCode] 
                  FROM [AttachStore].[dbo].[Tbl_AttachList] WITH(NOLOCK)
                    where [UnitCode]=@UnitCode and ParentCode=@ParentCode
            ";
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@UnitCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = ApiObj.postdata.ParentData["UnitCode"].ToString()
                });
            WordArea.SqlParas.ValuePara.Add(
                new DataHandle.SQLtype
                {
                    ColName = $"@ParentCode",
                    ColType = Mo.ConvertSQLType("nvarchar"),
                    ColLeng = 50,
                    ColValue = ApiObj.postdata.ParentData["ParentCode"].ToString()
                });
            dal = DalFactory.CreateDal(DataConnType, ThisConn);
            DataTable Dt = Task.Run(() => dal.ListData(WordArea.SqlCmd, WordArea.SqlParas)).Result;
            dynamic DyObj = new DynamicObj();
            DyObj.Data = Dt;
            Result = Mo.GetResult(true, "执行成功", true, DyObj._values, true);
            return Result;
        }

        /// <summary>
        /// 文件上传 写附件列表
        /// </summary>
        [HttpPost("UpAttachFile")]
        public string UpAttachFile([FromBody] dynamic Param)
        {
            string Result = string.Empty;
            Paras.AipPara = JsonConvert.SerializeObject(Param);
            RequestPagePara.AllParaResult ApiObj = Paras.Default.GetParaEnity();
            Redis.DbType = "Tbstruct";
            string Attach = Redis.Default.StringGet("AttachStore.Tbl_AttachList.Config.json");
            MsIns Ins = new MsIns();
            Ins.ThisConn = ThisConn;
            Ins.MsInsSingleNoCode(JObject.Parse(Attach), ApiObj);   
            Result = Mo.GetResult(true, "上传成功", false, new object(), true);
            return Result;
        }
        /// <summary>
        /// Api 接收异步上传的文件及表单参数
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpLoadFileSave")]
        public async Task<IActionResult> UpLoadFileSave()
        {
            UpFile= System.Configuration.ConfigurationManager.AppSettings["UpFile"];
            var date = Request;
            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = string.Empty;
            string contentRootPath = string.Empty;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string[] F = formFile.FileName.Split('.');
                    string fileExt = F[F.Length-1];  //文件扩展名，不含“.”
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = System.Guid.NewGuid().ToString() + "." + fileExt; //随机生成新的文件名
                    var filePath =  $@"{UpFile}\upload\" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {

                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new { count = files.Count, size });
        }
    }
}
