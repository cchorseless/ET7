using System;

namespace ET.Server
{
    public abstract partial class AMRpcHandler<Request, Response> : IMHandler where Request : class, IRequest where Response : class, IResponse {

        protected static void ReplyError(Response response, Exception e)
        {
            Log.Error(e);
            response.Error = ErrorCode.ERR_Error;
            response.Message = e.ToString();
        }
    }
}