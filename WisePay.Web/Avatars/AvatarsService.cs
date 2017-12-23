using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jdenticon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WisePay.Web.Avatars
{
    public class AvatarsService
    {
        private IConfiguration _config;
        private readonly IHostingEnvironment _env;

        public AvatarsService(
            IConfiguration config,
            IHostingEnvironment hostingEnvironment)
        {
            _config = config;
            _env = hostingEnvironment;
        }

        public string GenerateAndSaveAvatar(string username)
        {
            var icon = Identicon.FromValue(username, size: 512);

            var filename = GenerateFilename() + ".png";
            var pathToSave = GetFullAvatarPath(filename);

            icon.SaveAsPng(pathToSave);
            return filename;
        }

        public bool DeleteAvatar(string filename)
        {
            var avatarPath = GetFullAvatarPath(filename);

            try
            {
                File.Delete(avatarPath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> UpdateAvatar(byte[] avatarBytes)
        {
            var filename = GenerateFilename();
            var pathToSave = GetFullAvatarPath(filename);

            await File.WriteAllBytesAsync(pathToSave, avatarBytes);
            return filename;
        }

        public string GetFullAvatarPath(string filename)
        {
            return Path.Combine(_env.WebRootPath, "avatars", filename);
        }

        public string GetFullAvatarUrl(string filename)
        {
            if (filename == null) return null;

            return Path.Combine(_config["StaticFilesUrl"], "avatars", filename);
        }

        private string GenerateFilename()
        {
            return Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        }
    }
}
