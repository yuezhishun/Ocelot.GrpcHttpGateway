using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ocelot.GrpcHttpGateway
{
    public class GrpcHttpContent: HttpContent
    {
        private string result;

        public GrpcHttpContent(string result)
        {
            this.result = result;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var writer = new StreamWriter(stream);
            await writer.WriteAsync(result);
            await writer.FlushAsync();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = result.Length;
            return true;
        }
    }
}
