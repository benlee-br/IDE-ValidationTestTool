using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTestApp.ViewModelBase
{
    public static class Id
    {
        public static Guid ToGuid(string id)
        {
            var base64 = FromFileSafeBase64(id);
            return new Guid(Convert.FromBase64String(base64 + "=="));
        }

        public static string FromGuid(Guid id)
        {
            var base64 = Convert.ToBase64String(id.ToByteArray());
            var fileNameSafeBase64 = ToFileSafeBase64(base64);
            return fileNameSafeBase64.Substring(0, fileNameSafeBase64.Length - 2);
        }

        public static string New()
        {
            var guid = Guid.NewGuid();
            var base64 = Convert.ToBase64String(guid.ToByteArray());
            var fileNameSafeBase64 = ToFileSafeBase64(base64);
            return fileNameSafeBase64.Substring(0, fileNameSafeBase64.Length - 2);
        }

        public static string ToFileSafeBase64(string base64)
        {
            var fileNameSafeBase64 = base64.Replace('+', '-').Replace('/', '_');
            return fileNameSafeBase64;
        }

        public static string FromFileSafeBase64(string fileNameSafeBase64)
        {
            var base64 = fileNameSafeBase64.Replace('-', '+').Replace('_', '/');
            return base64;
        }

        public static bool Parse(string input)
        {
            try
            {
                //22 is length of our ids
                if (input.Length != 22)
                {
                    return false;
                }

                var safeInput = FromFileSafeBase64(input);
                Convert.FromBase64String(safeInput + "==");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}