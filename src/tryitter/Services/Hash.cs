using System.Security.Cryptography;
using System.Text;
namespace tryitter.Services
{

    public class Hash
    {
        public HashAlgorithm _hashAlgorithm;

        public Hash(HashAlgorithm hashAlgorithm)
        {
            _hashAlgorithm = hashAlgorithm;
        }

        public string CriptografarSenha(string senha)
        {
            var encryptedPassword = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(senha));

            var stringBuilder = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                stringBuilder.Append(caracter.ToString("X2"));
            }

            return stringBuilder.ToString();
        }

        public bool VerificarSenha(string digitada, string cadastrada)
        {
            if (string.IsNullOrEmpty(cadastrada))
                throw new NullReferenceException("Cadastre uma senha.");

            var encryptedPassword = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(digitada));

            var stringBuilder = new StringBuilder();
            foreach (var caractere in encryptedPassword)
            {
                stringBuilder.Append(caractere.ToString("X2"));
            }

            return stringBuilder.ToString() == cadastrada;
        }
    }
}
