
using CRUDPersonsAPI.BaseResponse;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace CRUDPersonsAPI.Features
{
    public static class ResponseApiService
    {
        public static BaseResponseModel Response(int StatusCode, object Data = null, string Message = null) {
            bool success = false;
            
            if (StatusCode >= 200 && StatusCode <300) 
            {
                success = true;
            }

            var result = new BaseResponseModel
            {

            Success = success, 
                Data = Data,
                Message = Message,
                StatusCode = StatusCode
            
            };


            return result;
        }
    }
}
