using System;
using AffinOuterAPI.Client.Requests;

namespace AffinOuterAPI.BLL.Services
{
    public static class ValidationService
    {
        public static void ValidateRequest<T>(ref T req) where T: BaseRequest
        {
            if (req.searchQuery == null || req.searchQuery == string.Empty)
                throw new ArgumentException("The search request is empty!");

            if(req.limit < 10 || req.limit > 100)
            {
                if (req.limit < 10)
                {
                    req.limit = 10;
                }
                else req.limit = 100;
            }

            if(req.offset < 1 || req.offset > 100)
            {
                if (req.offset < 1)
                {
                    req.offset = 1;
                }
                else req.offset = 100;
            }
        }
    }
}
