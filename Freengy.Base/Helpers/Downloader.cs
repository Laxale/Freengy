// Created by Laxale 04.05.2018
//
//

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;
using Freengy.Common.ErrorReason;
using Freengy.Common.Helpers.Result;


namespace Freengy.Base.Helpers 
{
    /// <summary>
    /// Allows to download something from web.
    /// </summary>
    public class Downloader 
    {
        public Result<Stream> DownloadContent(string url) 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage resultMessage = client.GetAsync(url).Result;
                    if (resultMessage.StatusCode == HttpStatusCode.OK)
                    {
                        Stream resultStream = resultMessage.Content.ReadAsStreamAsync().Result;
                        return Result<Stream>.Ok(resultStream);
                    }

                    return Result<Stream>.Fail(new UnexpectedErrorReason(resultMessage.StatusCode.ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                return Result<Stream>.Fail(new UnexpectedErrorReason(ex.Message));
            }
        }
    }
}