using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace HelperTools
{
    public class ErrGobalLog
    {
        private readonly RequestDelegate next;

        public ErrGobalLog(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                var statusCode = context.Response.StatusCode;
                if (ex is ArgumentException)
                {
                    statusCode = 200;
                }
                await HandleExceptionAsync(context, statusCode, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {
                    msg = "未授权";
                }
                else if (statusCode == 404)
                {
                    msg = "未找到服务";
                }
                else if (statusCode == 502)
                {
                    msg = "请求错误";
                }
                else if (statusCode != 200)
                {
                    msg = "未知错误";
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, statusCode, msg);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, int statusCode, string msg)
        {            
            var Rstr = new {
                CurrentTime = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}==>" +
                    $"Code={statusCode.ToString()}",
                scuess = false,
                Msg = msg,
                IsResult = false,
                Result = new object(),
                IsToken = true
            };
            var result = JsonConvert.SerializeObject(Rstr);
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
    public static class ErrorHandlingExtensions
    {
        public static IApplicationBuilder ErrGobalLogHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrGobalLog>();
        }
    }
}
