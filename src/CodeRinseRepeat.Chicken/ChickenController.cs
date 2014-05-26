using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace CodeRinseRepeat.Chicken
{
    [Stopwatch]
    public class ChickenController : ApiController
    {
        [Route ("api/chicken/puke")]
        [HttpGet]
        public object Puke ()
        {
            return new {
                LoadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(asm => asm.FullName),
                StackTrace = Environment.StackTrace.Split(new[] { "\r\n", "\n", }, StringSplitOptions.None),
            };
        }

        [Route ("api/chicken/encode")]
        [HttpPost]
        public async Task<string> Encode ()
        {
            var body = await Request.Content.ReadAsStringAsync();
            return ChickenCodec.Chicken(body);
        }

        [Route ("api/chicken/decode")]
        [HttpPost]
        public async Task<string> Decode ()
        {
            var body = await Request.Content.ReadAsStringAsync();
            return ChickenCodec.DeChicken(body);
        }
    }
}