using System;
using FanFou.SDK.Objects;

namespace FanFou.SDK.API
{
    public class Photos : RequestBase
    {
        public Photos(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<Status> UpdateStatusCallBack { get; set; }

        public void Upload(Action<Status> callback, UploadFile image, string status, string source = null,
                           string location = null, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(status)) throw new ArgumentException("status不能为空");
            UpdateStatusCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("status", status);
            if (!string.IsNullOrEmpty(source)) parameters.Add("source", source);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            if (!string.IsNullOrEmpty(location)) parameters.Add("location", location);
            var file = new Files();
            file.Add("photo", image);
            PostData("http://api.fanfou.com/photos/upload.json", parameters, file, UpdateStatusEnd);
        }

        private void UpdateStatusEnd(string json)
        {
            if (UpdateStatusCallBack != null)
            {
                var result = Util.JsonToObject<Status>(json);
                UpdateStatusCallBack(result);
            }
        }
    }
}