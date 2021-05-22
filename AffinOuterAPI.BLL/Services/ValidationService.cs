using AffinOuterAPI.Client.Requests;
using System;

namespace AffinOuterAPI.BLL.Services
{
    public static class ValidationService
    {
        public static void ValidateRequest(ref BaseRequest req)
        {
            if (req == null || string.IsNullOrEmpty(req.searchQuery))
                throw new ArgumentException("The search request is empty!");
        }

        public static void ValidateCoreRequest(ref BaseRequest req)
        {
            ValidateRequest(ref req);

            if (req.limit < 10 || req.limit > 100)
            {
                if (req.limit < 10)
                {
                    req.dbLimit = 10;
                }
                else req.dbLimit = 100;
            }
            else req.dbLimit = req.limit;

            if (req.offset < 1 || req.offset > 100)
            {
                if (req.offset < 1)
                {
                    req.offset = 1;
                }
                else req.offset = 100;
            }
        }

        public static void ValidateScopusRequest(ref BaseRequest req)
        {
            ValidateRequest(ref req);

            if (req.limit < 1 || req.limit > 25)
            {
                if (req.limit < 1)
                {
                    req.dbLimit = 1;
                }
                else req.dbLimit = 25;
            }
            else req.dbLimit = req.limit;

            if (req.offset < 1 || req.offset > 241)
            {
                if (req.offset < 1)
                {
                    req.offset = 1;
                }
                else req.offset = 241;
            }
        }
    }
}
