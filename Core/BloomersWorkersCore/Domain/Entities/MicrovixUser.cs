using System.Text;

namespace BloomersWorkersCore.Domain.Entities;

public class MicrovixUser
{
    public string usuario { get; set; }
    public string senha { get; set; }

    public static string GetNewRandomPassword()
    {
        const string CARACTERES = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@~!@#$%^&*()<>?";
        StringBuilder password = new StringBuilder();
        Random rnd = new Random();

        for (int i = 0; i < 20; i++)
        {
            int index = rnd.Next(CARACTERES.Length);
            password.Append(CARACTERES[index]);
        }

        return password.ToString();
    }
}
