using CRUDPersonsAPI.models;
namespace CRUDPersonsAPI.Services
{
    public class UsuarioService
    {
        public async Task<TbUsuario> Register(TbUsuario user, string password) 
        {

            CreatePasswordHash(password, out byte[] outpasswordHash, out byte[] passwordSalt);

            user.Password = password;
            user.
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
